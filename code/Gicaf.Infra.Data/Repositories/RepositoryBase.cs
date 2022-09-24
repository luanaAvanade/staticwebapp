using Axxiom.Linq.Dynamic.Core;
using GDrive;
using Gicaf.Domain.Entities;
using Gicaf.Domain.Entities.Base;
using Gicaf.Domain.Entities.Fornecedores;
using Gicaf.Domain.Interfaces;
using Gicaf.Domain.Interfaces.Repository;
using Gicaf.Domain.Services;
using Gicaf.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using EFCore.BulkExtensions;
//using Gicaf.Infra.Data.Connected_Services;

namespace Gicaf.Infra.Data.Repositories
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : BaseTrailEntity
    {
        protected AppDbContext _db;
        protected DbSet<TEntity> _dbSet;
        protected GdriveRepository _gdriveRepository;
        //protected IQueryable<TEntity> _query; 
        //protected IQueryable _query;
        public RepositoryBase(AppDbContext db, GdriveRepository gdriveRepository)
        {
            _db = db;
            _dbSet = _db.Set<TEntity>();
            _gdriveRepository = gdriveRepository;
            //_query = _db.Query<TEntity>();
        }

        public virtual TEntity Add(TEntity obj, IDictionary<string, object> input)
        {
            if(input != null)
            {
                var entry = _db.Attach(obj);
                HandleEntityEntry(entry, input);
                return entry.Entity;
            }
            return _db.Update(obj).Entity;
        }

        private void HandleCollection(CollectionEntry collection, IEnumerable input)
        {

            RelatedEntities relatedEntities = null;
            var principalPk = collection.EntityEntry.Properties.FirstOrDefault(x => x.Metadata.IsPrimaryKey());
            if(principalPk != null)
            {
                relatedEntities = NewRelatedEntities(collection, principalPk);
            }
            
            if((input.ToCollection()?.Count ?? 0) == 0 ) return;
            
            var inputEnumerator = input.GetEnumerator();
            foreach (var item in collection.CurrentValue)
            {
                var itemEntry = collection.FindEntry(item);
                if (principalPk != null)
                {
                    var pk = itemEntry.Properties.FirstOrDefault(x => x.Metadata.IsPrimaryKey());
                    relatedEntities.AddPkValue(pk.CurrentValue);
                }

                inputEnumerator.MoveNext();
                var inputItem = inputEnumerator.Current;
                HandleEntityEntry(itemEntry, (IDictionary<string, object>)inputItem);
            }
        }

        private RelatedEntities NewRelatedEntities(NavigationEntry navigation, PropertyEntry principalPk)
        {
            var fk = navigation.Metadata.ForeignKey.Properties.FirstOrDefault();
            var fkType = fk.DeclaringEntityType.ClrType;
            var fkName = fk.Name;
            var fkValue = principalPk.CurrentValue;
            var isNullable = fk.IsNullable;
            return _db.AddRelatedEntity(fkType, fkName, fkValue, isNullable);
        }

        private void HandleEntityEntry(EntityEntry entityEntry, IDictionary<string, object> input)
        {
            foreach (var entry in input)
            {
                var member = entityEntry.Members.FirstOrDefault(x => x.Metadata.Name == entry.Key);
                if (member == null) continue;

                if (member is PropertyEntry property && !property.Metadata.IsPrimaryKey())
                {
                    property.IsModified = true;
                }
                else if (member is ReferenceEntry reference)
                {
                    reference.IsModified = true;
                    HandleEntityEntry(reference.TargetEntry, entry.Value as IDictionary<string, object>);
                }
                else if(member is CollectionEntry collection)
                {
                    collection.IsModified = true;
                    HandleCollection(collection, entry.Value as IEnumerable);
                }
            }
        }

        public virtual TEntity Get(long id, object queryDetails)
        {
            IQueryNode queryNode = queryDetails as IQueryNode;
            IQueryable<TEntity> query = _dbSet;

            if(queryNode != null)
            {
                return ConvertToEntity(query.Select(queryNode).FirstOrDefault("Id = @0", id));
            }
            
            return query.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<TEntity> GetAll(object queryDetails = null)
        {
            //var teste = new FornecedorSapRepository();
            var queryNode = queryDetails as QueryNode;
            var queryable = _dbSet.Select(queryNode);
            var result = ConvertToEntityList(queryable);
            GetFilesContent(result);
            return result;
        }

        public IEnumerable<TEntity> GetWhere(object selectDetails, Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            QueryNode selectNode = null;
            if (selectDetails != null)
            {
                selectNode = selectDetails as QueryNode;
            }
            if (includes != null && includes.Any())
            {
                foreach (var include in includes)
                {
                    _dbSet.Include(include);
                }
            }
            if (selectNode == null)
            {
                return _dbSet.Where(predicate);
            }
            return _dbSet.Where(predicate).Select2(selectNode);
        }

        public void Remove(long id)
        {
            var entity = _dbSet.Find(id);
            _dbSet.Remove(entity);
            //_db.SaveChanges();
        }

        public virtual TEntity Update(TEntity obj, IDictionary<string, object> inputs)
        {
            var includes = inputs.Where(x => (x.Value is IDictionary) || (typeof(IEnumerable).IsAssignableFrom(x.Value.GetType()) && !(x.Value is string))).Select(x => x.Key);
            var originalEntity = Get(obj.Id, null);

            foreach (var key in inputs.Select(x => x.Key))
            {
                var prop = typeof(TEntity).GetProperty(key);
                var value = prop.GetValue(obj);
                prop.SetValue(originalEntity, value);
            }
            return originalEntity;
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }

        public virtual IDictionary<string, int> CountBy(string where, string groupBy)
        {
            return _dbSet.CountBy(where, groupBy);
        }
        public object Aggregate(string where, string groupBy, params string[] functions)
        {
            return _dbSet.Aggregate(where, groupBy, functions);
        }
        public void SetFilesContent()
        {
            foreach (var entry in _db.ChangeTracker.Entries().Where(x => x.Entity.GetType() == typeof(Domain.Entities.Arquivo)))
            {
                //((Gicaf.Domain.Entities.Arquivo)entry.Entity).Conteudo = DateTime.Now;
            }
        }

        public IEnumerable<TEntity> ConvertToEntityList(IQueryable queryable)
        {
            return JsonConvert.DeserializeObject<IEnumerable<TEntity>>(JsonConvert.SerializeObject(queryable.ToDynamicList()));
        }

        public TEntity ConvertToEntity(object obj)
        {
            return JsonConvert.DeserializeObject<TEntity>(JsonConvert.SerializeObject(obj));
        }


        public void GetFilesContent(IEnumerable<TEntity> queryable)
        {
            var conteudoProp = typeof(TEntity).GetProperties()
                                    .FirstOrDefault(x => x.PropertyType == typeof(Domain.Entities.Arquivo));
            //.FirstOrDefault(x => x.DeclaringType == typeof(Domain.Entities.Arquivo) && x.Name == nameof(Domain.Entities.Arquivo.Conteudo));

            if (conteudoProp != null || typeof(TEntity) == typeof(Domain.Entities.Arquivo))
            {
                Domain.Entities.Arquivo arquivo = null;
                foreach (var item in queryable)
                {
                    if (conteudoProp != null)
                    {
                        arquivo = (Domain.Entities.Arquivo)conteudoProp.GetValue(item);
                        arquivo.Conteudo = null;
                    }
                    else
                    {
                        arquivo = item as Domain.Entities.Arquivo;
                    }
                }

                if (arquivo != null && arquivo.Origem == OrigemArquivo.Gdrive && arquivo.CodigoExterno != null)
                {
                    arquivo.Conteudo = arquivo.Conteudo ?? new List<byte[]>();
                    var conteudoArquivo = _gdriveRepository.BuscarConteudoArquivoAsync(arquivo.CodigoExterno).Result.ConteudoBase64Binary;
                    arquivo.Conteudo.ToList().Add(conteudoArquivo);
                }

                if (arquivo != null && arquivo.Origem == OrigemArquivo.SistemaDeArquivos)
                {
                    arquivo.Conteudo.ToArray()[0] = File.ReadAllBytes(arquivo.CaminhoCompleto.FirstOrDefault());
                }
            }
        }
    }
}

public static class DbSetExtension
{

    public static T AddWithNested<T>(this DbSet<T> dbSet, T data) where T : BaseEntity
    {
        var entry = dbSet.Add(data);

        foreach (var item in entry.Navigations)
        {
            var entity = (T)item.EntityEntry.Entity;
            if (entity.Id == 0)
            {
                item.EntityEntry.State = EntityState.Added;
            }
            else
            {
                item.EntityEntry.State = EntityState.Modified;
            }
        }

        return data;
    }

    public static void AddOrUpdate<T>(this DbSet<T> dbSet, T data) where T : class
    {
        var context = dbSet.GetContext();
        //context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties[0].

        var ids = context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties.Select(x => x.Name);

        var t = typeof(T);
        List<PropertyInfo> keyFields = new List<PropertyInfo>();

        foreach (var propt in t.GetProperties())
        {
            var keyAttr = ids.Contains(propt.Name);
            if (keyAttr)
            {
                keyFields.Add(propt);
            }
        }
        if (keyFields.Count <= 0)
        {
            throw new Exception(../../"{t.FullName} does not have a KeyAttribute field. Unable to exec AddOrUpdate call.");
        }
        var entities = dbSet.AsNoTracking().ToList();
        foreach (var keyField in keyFields)
        {
            var keyVal = keyField.GetValue(data);
            entities = entities.Where(p => p.GetType().GetProperty(keyField.Name).GetValue(p).Equals(keyVal)).ToList();
        }
        var dbVal = entities.FirstOrDefault();
        if (dbVal != null)
        {
            context.Entry(dbVal).CurrentValues.SetValues(data);
            context.Entry(dbVal).State = EntityState.Modified;
            return;
        }
        dbSet.Add(data);
    }

    public static void AddOrUpdate<T>(this DbSet<T> dbSet, Expression<Func<T, object>> key, T data) where T : class
    {
        var context = dbSet.GetContext();
        var ids = context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties.Select(x => x.Name);
        var t = typeof(T);
        var keyObject = key.Compile()(data);
        PropertyInfo[] keyFields = keyObject.GetType().GetProperties().Select(p => t.GetProperty(p.Name)).ToArray();
        if (keyFields == null)
        {
            throw new Exception(../../"{t.FullName} does not have a KeyAttribute field. Unable to exec AddOrUpdate call.");
        }
        var keyVals = keyFields.Select(p => p.GetValue(data));
        var entities = dbSet.AsNoTracking().ToList();
        int i = 0;
        foreach (var keyVal in keyVals)
        {
            entities = entities.Where(p => p.GetType().GetProperty(keyFields[i].Name).GetValue(p).Equals(keyVal)).ToList();
            i++;
        }
        if (entities.Any())
        {
            var dbVal = entities.FirstOrDefault();
            var keyAttrs =
                data.GetType().GetProperties().Where(p => ids.Contains(p.Name)).ToList();
            if (keyAttrs.Any())
            {
                foreach (var keyAttr in keyAttrs)
                {
                    keyAttr.SetValue(data,
                        dbVal.GetType()
                            .GetProperties()
                            .FirstOrDefault(p => p.Name == keyAttr.Name)
                            .GetValue(dbVal));
                }
                context.Entry(dbVal).CurrentValues.SetValues(data);
                context.Entry(dbVal).State = EntityState.Modified;
                return;
            }
        }
        dbSet.Add(data);
    }
}

public static class HackyDbSetGetContextTrick
{
    public static DbContext GetContext<TEntity>(this DbSet<TEntity> dbSet)
        where TEntity : class
    {
        return (DbContext)dbSet
            .GetType().GetTypeInfo()
            .GetField("_context", BindingFlags.NonPublic | BindingFlags.Instance)
            .GetValue(dbSet);
    }
}

public static class ObjectExtensions
{
    public static T ToObject<T>(this IDictionary<string, object> source)
        where T : class, new()
    {
        var someObject = new T();
        var someObjectType = someObject.GetType();

        foreach (var item in source)
        {
            someObjectType
                     .GetProperty(item.Key)
                     .SetValue(someObject, item.Value, null);
        }

        return someObject;
    }

    //public static IDictionary<string, object> AsDictionary(this object source, BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
    public static IDictionary<string, object> AsDictionary(this object source, string[] propertyNames = null, BindingFlags bindingAttr = BindingFlags.Public | BindingFlags.Instance)
    {
        IEnumerable<PropertyInfo> properties;
        if (propertyNames?.Any() ?? false)
        {
            properties = source.GetType().GetProperties(bindingAttr).Where(x => propertyNames.Contains(x.Name));
        }
        else
        {
            properties = source.GetType().GetProperties(bindingAttr);
        }

        return properties.ToDictionary
        (
            propInfo => propInfo.Name,
            propInfo => propInfo.GetValue(source, null)
        );
    }
}

public static class CollectionEntryExtensions
{
    public static EntityEntry FindEntry(this CollectionEntry collectionEntry, object obj)
    {
        return collectionEntry.EntityEntry.Context.ChangeTracker.Entries().FirstOrDefault(entry => entry.Entity == obj);
    }
}