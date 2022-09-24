using Gicaf.Application.GraphQL.Models;
using Gicaf.Application.Services;
using Gicaf.Domain.Entities;
using Gicaf.Domain.Entities.Fornecedores;
using Gicaf.Domain.Entities.Base;
using Gicaf.Domain.Interfaces.Services;
using Gicaf.Domain.Services;
using GraphQL.Language.AST;
using GraphQL.Resolvers;
using GraphQL.Types;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gicaf.Application.GraphQL.Resolvers
{
    public enum ResolverType
    {
        FindById,
        CountBy,
        List,
        Insert,
        InsertMany,
        Update,
        UpdateMany,
        Remove,
        RemoveMany,
        ProcessarResultado,
        ProcessarMec,
        GerarRespostas,
        Aggregate,
    }

    public interface IFilePost
    {
        byte[] Content { get; }
        string ContentDisposition { get; }
        string ContentType { get; }
        string FileName { get; }
        long Length { get; }
        string Name { get; }
    }

    public class FieldResolver
    {
        const string PaginationKey = "Pages";
        private Type _entityType;
        private ResolverType _resolverType;
        ResultExtensions _resultExtensions;
        IServiceProvider _services;

        public IServiceBase GetService()
        {
            var serviceType = typeof(IServiceBase<>).MakeGenericType(_entityType);
            var service = (IServiceBase)_services.GetService(serviceType);
            return service;
        }

        public TServiceType GetService<TServiceType>()
        {
            var service = (TServiceType)_services.GetService(typeof(TServiceType));
            return service;
        }


        public void AddPaginationResult(Pagination pagination)
        {
            if (_resultExtensions.Extensions.TryGetValue(PaginationKey, out object result))
            {
                ((List<Pagination>)result).Add(pagination);
            }
            else
            {
                _resultExtensions.Extensions.Add(PaginationKey, new List<Pagination> { pagination });
            }
        }

        public FieldResolver(IServiceProvider services, ResultExtensions resultExtensions)
        {
            _services = services;
            _resultExtensions = resultExtensions;
        }

        public Task<object> Resolve(ResolveFieldContext context)
        {
            if(context.Source == null)
            {
                SetMetadataValues(context.FieldDefinition.Metadata);
            }
            else
            {
                return Task.FromResult(new NameFieldResolver().Resolve(context));
            }

            switch (_resolverType)
            {
                case ResolverType.FindById: return Task.FromResult(ResolveFindById(context));
                case ResolverType.List: return Task.FromResult(ResolveQuery(context));
                case ResolverType.Insert: return Task.FromResult(ResolveCreate(context));
                case ResolverType.InsertMany: return Task.FromResult(ResolveCreateRange(context));
                case ResolverType.Update: return Task.FromResult(ResolveUpdate(context));
                case ResolverType.UpdateMany: return Task.FromResult(ResolveUpdateRange(context));
                case ResolverType.Remove: ResolveRemove(context); return null;
                case ResolverType.RemoveMany: ResolveRemoveMany(context); return null;
                case ResolverType.CountBy: return Task.FromResult(ResolveCountBy(context));
                case ResolverType.ProcessarResultado: return Task.FromResult(ProcessarResultado(context));
                case ResolverType.ProcessarMec: return Task.FromResult(ProcessarMec(context));
                case ResolverType.GerarRespostas: return Task.FromResult(GerarRespostas(context));
                case ResolverType.Aggregate: return Task.FromResult(Aggregate(context));
                default: return null;
            }
        }

        private object Aggregate(ResolveFieldContext context)
        {
            var functions = context.GetArgument<string[]>(Constants.Arguments.functions);
            var where = context.GetArgument<string>(Constants.Arguments.where);
            var groupBy = context.GetArgument<string>(Constants.Arguments.groupBy);

            return GetService().Aggregate(where, groupBy, functions);
        }

        private object ResolveRemoveMany(ResolveFieldContext context)
        {
            var ids = context.GetArgument<long[]>(Constants.Arguments.ids);
            GetService().RemoveMany(ids);
            GetService().SaveChanges();
            return null;
        }

        private object GerarRespostas(ResolveFieldContext context)
        {
            var respostaInput = (Resposta)CreateInputFromContext(context, typeof(Resposta), out IDictionary<string, object> input);
            var selectDetails = CreateNodeFromContext(context, typeof(Resposta));
            return GetService<IRespostaService>().GerarRespostas(selectDetails, respostaInput.PerguntaId, respostaInput.UsuarioId);
        }

        private void SetMetadataValues(IDictionary<string, object> metadata)
        {
            _entityType = (Type)metadata[nameof(Type)];
            _resolverType = (ResolverType)metadata[nameof(ResolverType)];
        }

        private object ProcessarMec(ResolveFieldContext context)
        {
            GetService<IAvaliacaoCategoriaService>().ProcessarMec();
            return true;
        }

        private object ProcessarResultado(ResolveFieldContext context)
        {

            var perguntaId = context.GetArgument<long>(Constants.Arguments.codigoPergunta);
            GetService<IResultadoService>().Processar(perguntaId);
            return true;
            //throw new NotImplementedException();
        }

        private object ResolveCountBy(ResolveFieldContext context)
        {
            var where = context.GetArgument(Constants.Arguments.where, default(string));
            var groupBy = context.GetArgument(Constants.Arguments.groupBy, default(string));

            return GetService().CountBy(where, groupBy).Select(x => new { Group = x.Key, Count = x.Value });
        }

        private object ResolveFindById(ResolveFieldContext context)
        {
            var id = context.GetArgument<long>("id");
            return GetService().GetGeneric(id);
        }

        private object ResolveQuery(ResolveFieldContext context)
        {
            IQueryNode rootNode = CreateNodeFromContext(context, _entityType);
            var result = GetService().GetAllGeneric(rootNode);
            AddPaginationResult(rootNode.Pagination);
            return result;
        }

        private IQueryNode CreateNodeFromContext(ResolveFieldContext context, Type type)
        {
            var field = context.FieldAst;
            var node = new QueryNode(field.Name,
                                context.GetArgument(Constants.Arguments.where, default(string)),
                                context.GetArgument(Constants.Arguments.skip, default(int?)),
                                context.GetArgument(Constants.Arguments.take, default(int?)),
                                context.GetArgument(Constants.Arguments.orderBy, default(string)),
                                context.GetArgument(Constants.Arguments.paged, default(bool?)) ?? false,
                                new List<IQueryNode>(),
                                type);

            if (node.ReturnPagination)
            {
                node.Pagination = new Pagination(field.Alias ?? field.Name, node.Skip, node.Take);
            }

            foreach (var subSelection in field.SelectionSet.Selections)
            {
                FillNodeFromContext(context, node, subSelection, type);
            }
            return node;
        }

        private void FillNodeFromContext(ResolveFieldContext context, IQueryNode node, ISelection selection, Type baseType, string path = null)
        {
            Type type;
            if (path != null)
            {
                type = null;//type.PropertyFromPath(fieldPath).PropertyType;
            }
            else
            {
                type = baseType;
            }


            if (selection is Field)
            {
                var field = selection as Field;

                var nextNode = new QueryNode(field.Name,
                                        field.GetArgument<string>(Constants.Arguments.where),
                                        field.GetArgument<int?>(Constants.Arguments.skip),
                                        field.GetArgument<int?>(Constants.Arguments.take),
                                        field.GetArgument<string>(Constants.Arguments.orderBy),
                                        field.GetArgument<bool?>(Constants.Arguments.paged) ?? false,
                                        new List<IQueryNode>(),
                                        type);

                if (nextNode.ReturnPagination)
                {
                    nextNode.Pagination = new Pagination(field.Alias ?? field.Name, node.Skip, node.Take);
                }

                node.Selections.Add(nextNode);

                if (field.SelectionSet?.Selections?.Any() ?? false)
                {
                    foreach (var subSelection in field.SelectionSet.Selections)
                    {
                        FillNodeFromContext(context, nextNode, subSelection, typeof(object));
                    }
                }
            }
            else if (selection is FragmentSpread)
            {
                var fragmmentSpread = selection as FragmentSpread;
                var fragment = context.Fragments.FirstOrDefault(x => x.Name == fragmmentSpread.Name);

                foreach (var subSelection in fragment.SelectionSet.Selections)
                {
                    FillNodeFromContext(context, node, subSelection, typeof(object));
                }
            }
        }

        protected object ResolveUpdate(ResolveFieldContext context)
        {
            var id = context.GetArgument<long>(Constants.Arguments.id);
            var result = GetService().AddGeneric(CreateInputFromContext(context, _entityType, out IDictionary<string, object> input, id), input);
            GetService().SaveChanges();
            return result;
        }

        protected object ResolveUpdateRange(ResolveFieldContext context)
        {
            var ids = context.GetArgument<long[]>(Constants.Arguments.ids);
            //var service = GetService();
            var result = GetService().AddRangeGeneric(CreateInputListFromContext(context, _entityType, out IList<IDictionary<string, object>> input, ids), input);

            foreach (var item in (IEnumerable)result)
            {
            }

            GetService().SaveChanges();
            return result;
        }

        protected object ResolveCreate(ResolveFieldContext context)
        {
            var result = GetService().AddGeneric(CreateInputFromContext(context, _entityType, out IDictionary<string, object> input), input);

            //foreach (var item in (IEnumerable)result)
            //{

            //}

            GetService().SaveChanges();
            return result;
        }

        private object ResolveCreateRange(ResolveFieldContext context)
        {
            var result = GetService().AddRangeGeneric(CreateInputListFromContext(context, _entityType, out IList<IDictionary<string, object>> input), input);

            foreach (var item in (IEnumerable)result)
            {

            }


            GetService().SaveChanges();
            return result;
        }

        private object CreateInputFromContext(ResolveFieldContext context, Type type, out IDictionary<string, object> input, long? updateId = null)
        {
            //string inputName = char.ToLowerInvariant(type.Name[0]) + type.Name.Substring(1);
            input = context.GetArgument<IDictionary<string, object>>("input");
            object obj = JObject.FromObject(input).ToObject(type);

            if (context.UserContext != null && context.UserContext is Dictionary<string, object> userContext)
            {
                HandleFiles(obj, userContext);
            }

            if (updateId.HasValue && obj is BaseEntity)
            {
                ((BaseEntity)obj).Id = updateId.Value;
            }
            return obj;
        }

        private object CreateInputListFromContext(ResolveFieldContext context, Type type, out IList<IDictionary<string, object>> input, long[] updateIds = null)
        {
            var inputlist = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(type));
            input = context.GetArgument<IList<IDictionary<string, object>>>("input");

            for (int i = 0; i < input.Count; i++)
            {
                var entity = JObject.FromObject(input[i]).ToObject(type);

                if (updateIds != null)
                {
                    var id = updateIds[i];
                    ((BaseEntity)entity).Id = id;
                }

                if (context.UserContext != null && context.UserContext is Dictionary<string, object> userContext)
                {
                    HandleFiles(entity, userContext);
                }
                inputlist.Add(entity);
            }
            return inputlist;
        }

        protected void ResolveRemove(ResolveFieldContext context)
        {
            var id = context.GetArgument<long>("id");
            GetService().RemoveGeneric(id);
            GetService().SaveChanges();
        }

        protected object HandleFiles(object obj, Dictionary<string, object> userContext)
        {
            if (userContext.ContainsKey("files"))
            {
                var files = (List<IFilePost>)userContext["files"];
                var arquivoPropertyInfo = obj.GetType().GetProperties().FirstOrDefault(x => x.PropertyType == typeof(Arquivo));
                if (arquivoPropertyInfo == null)
                {
                    var documentoEmpresaPropertyInfo = obj.GetType().GetProperties().FirstOrDefault(x => x.PropertyType == typeof(ICollection<DocumentoEmpresa>));
                    if(documentoEmpresaPropertyInfo != null)
                    {
                        foreach(var doc in (ICollection<DocumentoEmpresa>)documentoEmpresaPropertyInfo.GetValue(obj))
                        {
                            var file = files.FirstOrDefault(x => x.Name == doc.Arquivo.Key);
                            if(file != null)
                            {
                                doc.Arquivo.Conteudo = new List<byte[]>() { file.Content }; 
                                doc.Arquivo.Extensao = Path.GetExtension(file.FileName);
                                doc.Arquivo.NomeArquivo = file.FileName;
                                doc.Arquivo.Origem = OrigemArquivo.Gdrive;
                            }
                        }
                    }
                    
                }    
                if (arquivoPropertyInfo != null)
                {
                    var arquivo = (Arquivo)arquivoPropertyInfo.GetValue(obj);
                    arquivo.NomeArquivo = string.Join(";", files.Select(x => Path.GetFileNameWithoutExtension(x.FileName)));
                    //arquivo.Caminho = string.Join(";", files.Select(x =>  Config.DirArquivoImportacao));
                    arquivo.Conteudo = files.Select(x => x.Content);
                    arquivo.Extensao = string.Join(";", files.Select(x => Path.GetExtension(x.FileName)));
                    arquivo.Origem = OrigemArquivo.SistemaDeArquivos;
                }
            }
            return null;
        }
    }
}

public static class etx
{
    public static T GetArgument<T>(this Field field, string name)
    {
        return (T)field.Arguments.FirstOrDefault(x => x.Name == name)?.Value?.Value;
    }

    public static object GetArgument(this Field field, string name)
    {
        return field.Arguments.FirstOrDefault(x => x.Name == name)?.Value?.Value;
    }
}