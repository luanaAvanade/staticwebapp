using Gicaf.Application.Interface.Services;
using Gicaf.Application.Services;
using Gicaf.Domain.Entities;
using Gicaf.Domain.Entities.Base;
using Gicaf.Domain.Interfaces.Repository;
using Gicaf.Domain.Services;
using Gicaf.Domain.Validators;
using Gicaf.Infra.Data.Context;
//using Gicaf.Infra.Data.GraphQL;
//using Gicaf.Infra.Data.GraphQL.Models;
//using Gicaf.Infra.Data.GraphQL.Models;
using Gicaf.Infra.Data.Repositories;
using GraphQL.Conversion;
using GraphQL.Language.AST;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SisAgua.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using Gicaf.Application.GraphQL;
using Gicaf.Application.GraphQL.Models;
using Microsoft.EntityFrameworkCore.Metadata;
using FluentValidation;
using Gicaf.Domain.Interfaces.Services;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Gicaf.Infra.Data;
using VGPartner.Infra.CrossCutting.Utils.RazorMailer;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Web;
using Newtonsoft.Json.Linq;
using Gicaf.Infra.Data.Scripts;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System.IO;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using Gicaf.Domain.Entities.Fornecedores;
using System.ComponentModel.DataAnnotations;
using Gicaf.Domain.Interfaces;
using Gicaf.Infra.Data.PdfParser;

namespace Gicaf.Infra.CrossCutting.IoC
{
    public static class EntityMetaData
    {
        //public static DatabaseMetadata DatabaseMetadata { get; set; }
        public static DatabaseMetadata BuildDatabaseMetadata (DbContext dbcontext, ITableNameLookup tableNameLookup)
        {
            var tablesMetadata = new List<EntityMetadata>();

            foreach (var entityType in dbcontext.Model.GetEntityTypes())
            {
                var tableName = entityType.Relational().TableName;

                var tableMetadata = new EntityMetadata
                {
                    Name = tableName,
                    Type = entityType.ClrType,
                };
                tableMetadata.Columns = GetColumnsMetadata(entityType, tableMetadata);
                tableNameLookup.InsertKeyName(tableName);
                tablesMetadata.Add(tableMetadata);
                PersistedTypes.Add(entityType.ClrType);
            }
            return new DatabaseMetadata(tablesMetadata, null);
        }
        private static IReadOnlyList<ColumnMetadata> GetColumnsMetadata(IEntityType entityType, EntityMetadata tableMetadata)
        {
            var tableColumns = new List<ColumnMetadata>();
            var navigations = entityType.GetNavigations();
            var properties = entityType.ClrType.GetProperties().Where(x => !navigations.Any(y => y.Name == x.Name));
            foreach (var propertyType in properties)
            {
                //var relational = propertyType.Relational();
                tableColumns.Add(new ColumnMetadata
                {
                    //ColumnName = relational.ColumnName,
                    ColumnName = propertyType.Name,
                    //GetColumnType(propertyType, relational),
                    DataType = propertyType.PropertyType.Name,
                    IsScalar = true,
                    //Type = propertyType.ClrType,
                    Type = propertyType.PropertyType,
                    TableMetadata = tableMetadata
                });
            }

            
            foreach (var nav in navigations)
            {
                tableColumns.Add(new ColumnMetadata
                {
                    ColumnName = nav.Name,
                    DataType = nav.DeclaringEntityType.Name,
                    IsScalar = false,
                    Type = nav.ClrType,
                    TableMetadata = tableMetadata
                });
            }
            return tableColumns;
        }

        private static string GetColumnType(IProperty propertyType, IRelationalPropertyAnnotations relational)
        {
            if(relational.ColumnType != null)
            {
                return relational.ColumnType;
            }

            var type = Nullable.GetUnderlyingType(propertyType.ClrType) ?? propertyType.ClrType;

            if (type.IsEnum)
            {
                return "int";
            }

            switch (type.Name)
            {
                case nameof(DateTime): return "datetime2";
                case nameof(Int64): return "bigint";
                case nameof(Single): return "real";
                case nameof(Double): return "float";
                case nameof(Int32): return "int";
                case nameof(Boolean): return "bit";
                case nameof(Int16): return "smallint";
                default: return null;
            }
        }
    }

    public class InjectorBootStrapper
    {
        const string WEB_CONTEXT = "WEB";
        const string APP_CONTEXT = "APP";

        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            var container = new Container();

            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            services.AddSimpleInjector(container, options =>
            {
                options.AddAspNetCore().AddControllerActivation();
            });

            Config.LocalUser = configuration.GetSection("Database").GetValue<string>("WinAuthentication");
            Config.DirArquivoImportacao = configuration.GetSection("Upload").GetValue<string>("DirArquivoImportacao");

            var gdriveUrl = configuration.GetSection("GDrive").GetValue<string>("URL");
            var gdriveLogin = configuration.GetSection("GDrive").GetValue<string>("Login");
            var gdriveSenha = configuration.GetSection("GDrive").GetValue<string>("Senha");

            services.AddSingleton<IServiceProvider>(services.BuildServiceProvider());
            services.AddSingleton(resolver => new GdriveRepository(gdriveUrl, gdriveLogin, gdriveSenha));

            container.Register<IServiceProvider>(() => container, Lifestyle.Singleton);
            container.Register(() => new GdriveRepository(gdriveUrl, gdriveLogin, gdriveSenha), Lifestyle.Singleton);

            var ssisJobs = configuration.GetSection("SSIS").GetSection("Jobs");

            SSISConfig.VolumeCompras = ssisJobs.GetValue<string>("VolumeCompras");
            SSISConfig.PrazoMedioSubstituicaoFornecedor = ssisJobs.GetValue<string>("PrazoMedioSubstituicaoFornecedor");
            SSISConfig.NumeroFornecedores = ssisJobs.GetValue<string>("NumeroFornecedores");
            SSISConfig.GrauRegulamentacao = ssisJobs.GetValue<string>("GrauRegulamentacao");
            SSISConfig.PerguntaFormulario = ssisJobs.GetValue<string>("PerguntaFormulario");

            container.Register<IAppServiceResolver, GraphQLAppServiceResolver>(Lifestyle.Scoped);

            container.Register(typeof(IServiceBase<>), typeof(ServiceBase<>).Assembly, Lifestyle.Scoped);
            container.RegisterConditional(typeof(IServiceBase<>), typeof(ServiceBase<>), Lifestyle.Scoped, c => !c.Handled);

            container.Register(() =>
            {
                var optionsBuilder = new DbContextOptionsBuilder();
                var inMemoryDatabase = configuration.GetValue<bool>("InMemoryDatabase");

                if (inMemoryDatabase)
                {
                    optionsBuilder.UseInMemoryDatabase("Gicaf");
                }
                else
                {
                    optionsBuilder.UseSqlServer(configuration.GetConnectionString("MainDB"));
                }
                return new AppDbContext(optionsBuilder.Options);
            }, Lifestyle.Scoped);

            services.AddDbContext<AppDbContext>((options) =>
            {
                var optionsBuilder = new DbContextOptionsBuilder();
                var inMemoryDatabase = configuration.GetValue<bool>("InMemoryDatabase");

                if (inMemoryDatabase)
                {
                    options.UseInMemoryDatabase("Gicaf");
                }
                else
                {
                    options.UseSqlServer(configuration.GetConnectionString("MainDB"));
                }
            }, ServiceLifetime.Scoped, ServiceLifetime.Scoped);

            container.Register(typeof(IRepositoryBase<>), typeof(RepositoryBase<>).Assembly, Lifestyle.Scoped);
            container.RegisterConditional(typeof(IRepositoryBase<>), typeof(RepositoryBase<>), Lifestyle.Scoped, c => !c.Handled);
            container.Register<IAuthenticatorService>(() => new AuthenticatorClient(configuration.GetSection("Urls").GetValue<string>("Authenticator")), Lifestyle.Scoped);

            container.GraphQLBoot();
            var synFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "PdfParser");
            container.Register<IDocumentInfoExtractor>(() => { return new PdfInfoExtractor(synFilePath); });

            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Database.startup");
            if (File.Exists(path))
            {
                var scripts = DataBaseStartup(services);
                File.WriteAllText(path,scripts);
                File.Move(path, path + ".log");                
            }
        }

        private static string DataBaseStartup(IServiceCollection services)
        {
            var dbcontext = services.BuildServiceProvider().GetRequiredService<AppDbContext>();
            
            using(var scripts = new StringWriter())
            {
                scripts.WriteLine(DatabaseStartup.DeleteAllTables);
                scripts.WriteLine(dbcontext.Database.GenerateCreateScript());
                scripts.WriteLine(DatabaseStartup.SP_ProcessaPergunta);
                scripts.WriteLine(DatabaseStartup.SP_CalcularMatriz);
                scripts.WriteLine(DatabaseStartup.SP_RetornaNumeroPergunta);
                scripts.WriteLine(DatabaseStartup.FN_CalcularMedia);
                scripts.WriteLine(DatabaseStartup.FN_CalcularMediana);
                scripts.WriteLine(DatabaseStartup.FN_CalcularModa);
                scripts.WriteLine(DatabaseStartup.FN_ConstruirFormula);
                scripts.WriteLine(DatabaseStartup.Pd_Dim_Categorias);
                scripts.WriteLine(DatabaseStartup.Pd_Stg1_P1_Peso_Valor_Compra);
                scripts.WriteLine(DatabaseStartup.Pd_Stg1_P4_Contratos_Pedidos);
                scripts.WriteLine(DatabaseStartup.Pd_Stg1_P4_Leadtime);
                scripts.WriteLine(DatabaseStartup.Pd_Stg1_P4_Peso_Fornecedor);
                scripts.WriteLine(DatabaseStartup.Pd_Stg1_P5_Num_Fornecedor);
                scripts.WriteLine(DatabaseStartup.Pd_Stg1_P6_Peso_Valor_Compra);
                scripts.WriteLine(DatabaseStartup.Pd_Stg2_P4_Leadtime);
                scripts.WriteLine(DatabaseStartup.Pd_Stg2_P4_Prazos);

                using (SqlConnection connection = (SqlConnection)dbcontext.Database.GetDbConnection())
                {
                    Server server = new Server(new ServerConnection(connection));
                    server.ConnectionContext.ExecuteNonQuery(scripts.ToString());
                }
                return scripts.ToString();
            }
        }

        private static string[] GetContextsFromValidationType(Type validationType)
        {
            switch (validationType.Name)
            {
                case (nameof(CoordenadaValidator)): return new string[] { WEB_CONTEXT };
                default: return null;
            }
        }
    }

    public enum RegisterType
    {
        Scoped,
        Singleton,
        Transient
    }

    public static class ServiceCollectionExtensions
    {
        public static void RegisterClosestTypesFromGenericInterface(this IServiceCollection services, Assembly assembly, Type interfaceGenericType, RegisterType registerType = RegisterType.Scoped)
        {
            foreach (var implementationType in assembly.GetTypes().Where(x => x.BaseType != null && x.BaseType.ImplementsGenericInterface(interfaceGenericType) && x.IsConcreteClass()))
            {
                //var genericType = implementationType.BaseType.GetGenericArguments()[0];
                //var interfaceType =  interfaceGenericType.MakeGenericType(genericType);

                //var interfaces = implementationType.GetInterfaces().Where(x => x.IsGenericType || (x.BaseType != null && x.BaseType.IsGenericType));
                var interfaces = implementationType.GetInterfaces();

                foreach (var interfaceType in interfaces)
                {
                    if (registerType == RegisterType.Scoped)
                    {
                        services.AddScoped(interfaceType, implementationType);
                    }
                    else if (registerType == RegisterType.Singleton)
                    {
                        services.AddSingleton(interfaceType, implementationType);
                    }
                    else if (registerType == RegisterType.Transient)
                    {
                        services.AddTransient(interfaceType, implementationType);
                    }
                }
                //var interfaceType = implementationType.GetInterfaces().LastOrDefault();
            }
        }

        public static Container GraphQLBoot(this Container container)
        {
            container.Register(() =>
            {
                var extensions = new ResultExtensions();
                return extensions;
            }, Lifestyle.Scoped);

            container.Register<ITableNameLookup, TableNameLookup>(Lifestyle.Singleton);

            container.Register<IDatabaseMetadata>(() =>
            {
                var dbContext = container.GetRequiredService<AppDbContext>();
                var tableNameLookup = container.GetRequiredService<ITableNameLookup>();
                return EntityMetaData.BuildDatabaseMetadata(dbContext, tableNameLookup);
            }, Lifestyle.Singleton);

            container.Register(() =>
            {
                var metaDatabase = container.GetRequiredService<IDatabaseMetadata>();
                var tableNameLookup = container.GetRequiredService<ITableNameLookup>();
                return GraphQLTypesBuilder.GetPersistedTypes(metaDatabase, tableNameLookup);
            }, Lifestyle.Singleton);

            container.Register(() =>
            {
                var metaDatabase = container.GetRequiredService<IDatabaseMetadata>();
                var tableNameLookup = container.GetRequiredService<ITableNameLookup>();
                var persistedTypes = GraphQLTypesBuilder.GetPersistedTypes(metaDatabase, tableNameLookup);

                var schema = new GraphQL.Types.Schema
                {
                    Query = new GraphQLQuery(persistedTypes),
                    Mutation = new GraphQLMutation(persistedTypes),
                    FieldNameConverter = new DefaultFieldNameConverter() 
                };
                schema.Initialize();
                return schema;
            }, Lifestyle.Singleton);

            return container;
        }
    }

    public static class TypeExtensions
    {
        public static bool ImplementsGenericInterface(this Type type, Type interfaceType)
        {
            if (interfaceType.IsGenericType)
            {
                return type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == interfaceType);
            }
            throw new Exception("the value from interfaceType parameter is not valid");
        }

        public static bool IsSubclassOfRawGeneric(this Type generic, Type toCheck)
        {
            while (generic != null)
            {
                if (generic.IsGenericType && generic.GetGenericTypeDefinition() == toCheck)
                {
                    return true;
                }
                generic = generic.BaseType;
            }
            return false;
        }

        public static bool IsConcreteClass(this Type type)
        {
            return type.IsClass && !type.IsAbstract;
        }
    }

    public class AuthenticatorClient: IAuthenticatorService
    {
        HttpClient _client = new HttpClient();

        public async Task<object> RegisterUserAsync(Usuario usuario)
        {
            HttpResponseMessage response = null;
            response = await _client.PostAsJsonAsync(
                "api/register", new { Name = usuario.Nome, UserName = usuario.UserName, Email = usuario.Email, PassWord = usuario.PassWord });
            //response.EnsureSuccessStatusCode();
 
             var result = await response.Content.ReadAsAsync<object>();
             var token = JObject.FromObject(result).SelectToken("result").ToObject<string>();
             SendConfirmationEmail(usuario, token);
             return result;
        }

        public AuthenticatorClient(string baseUrl)
        {
            _client.BaseAddress = new Uri(baseUrl);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private void SendConfirmationEmail(Usuario usuario, string token)
        {
            var linkConfirmacao = usuario?.LinkConfirmacao?.Replace("{userName}", usuario.UserName)?.Replace("{token}", HttpUtility.UrlEncode(token));
            var confirmacaoEmail = new ConfirmacaoEmail { Nome = usuario.Nome, LinkConfirmacao = linkConfirmacao , Email = usuario.Email };
            var message = EmailManager.MailerEngine.CreateMessage("BemVindo", confirmacaoEmail, usuario.Email, "Confirmação de Cadastro");
            var result = EmailManager.MailerEngine.SendAsync(message);
        }

        public void SendResultadoAnaliseEmail(Usuario usuario, string NomeEmpresa, List<ItemAnalise> itensAnalise, string linkCadastro)
        {            
            ResultadoAnaliseEmail resultadoEmail = new ResultadoAnaliseEmail { NomeEmpresa = NomeEmpresa, ItensAnalise = new Dictionary<string, List<ItemAnaliseDisplay>>(), LinkReajuste = linkCadastro };

            List<ItemAnalise> itensReprovados = itensAnalise.FindAll(x => x.Status == StatusAnalise.Reprovado);

            foreach(var item in itensReprovados)
            {
                string displayGroupItem = EnumHelper<EnumItemAnalise>.GetGroupValue(item.TipoItem);
                
                ItemAnaliseDisplay itemAnalise = new ItemAnaliseDisplay(){
                    DisplayItem = EnumHelper<EnumItemAnalise>.GetDisplayValue(item.TipoItem),
                    DisplayGroupItem = displayGroupItem,
                    DisplayStatus = EnumHelper<StatusAnalise>.GetDisplayValue(item.Status),
                    Justificativa = item.Justificativa 
                };

                if(resultadoEmail.ItensAnalise.ContainsKey(displayGroupItem)){
                    List<ItemAnaliseDisplay> itens = resultadoEmail.ItensAnalise.GetValueOrDefault(displayGroupItem);
                    itens.Add(itemAnalise);
                }else{
                    List<ItemAnaliseDisplay> itens = new List<ItemAnaliseDisplay>();
                    itens.Add(itemAnalise);
                    resultadoEmail.ItensAnalise.Add(displayGroupItem, itens);
                }
            }   

            System.Net.Mail.MailMessage message;
            if(itensReprovados.Count == 0){
                message = EmailManager.MailerEngine.CreateMessage("Aprovacao", resultadoEmail, usuario.Email, "Resultado Análise de Cadastro");
            }else{
                message = EmailManager.MailerEngine.CreateMessage("Reprovacao", resultadoEmail, usuario.Email, "Resultado Análise de Cadastro");
            }
            var result = EmailManager.MailerEngine.SendAsync(message);
           // var linkConfirmacao = usuario?.LinkConfirmacao?.Replace("{userName}", usuario.UserName)?.Replace("{token}", HttpUtility.UrlEncode(token));
        }
    }
    public class ConfirmacaoEmail
    {
        public string Nome { get; set; }
        public string LinkConfirmacao { get; set; }
        public string Email{ get; set; }
    }

    public class ResultadoAnaliseEmail
    {
        public string NomeEmpresa { get; set; }
        public Dictionary<string, List<ItemAnaliseDisplay>> ItensAnalise { get; set; }
        public string LinkReajuste { get; set; }
    }

    public class ItemAnaliseDisplay
    {
        public string DisplayItem { get; set; }
        public string DisplayGroupItem { get; set; }
        public string DisplayStatus { get; set; }
        public string Justificativa { get; set; }
    }
}
