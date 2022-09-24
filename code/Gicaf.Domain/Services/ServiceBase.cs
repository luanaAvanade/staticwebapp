using Gicaf.Domain.Entities;
using Gicaf.Domain.Entities.Base;
using Gicaf.Domain.Interfaces.Repository;
using Gicaf.Domain.Interfaces.Services;
using Gicaf.Domain.Validators;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Collections.ObjectModel;

namespace Gicaf.Domain.Services
{

    public class EntityValidationResult2<TEntity>: ValidationResult
    {
        public TEntity Entity { get; set; }
        public EntityValidationResult2()
        {
            
        }
    }

    public class EntityValidationResult<TEntity> where TEntity : BaseEntity
    {
        public EntityValidationResult(TEntity entity, IEnumerable<ValidationResult> result)
        {
            Entity = entity;
            Result = result;
        }

        public EntityValidationResult(TEntity entity)
        {
            Entity = entity;
        }

        public bool IsValid()
        {
            return Result.All(x => x.IsValid);
        }

        public TEntity Entity { get; set; }
        public IEnumerable<ValidationResult> Result { get; set; }
        

        //public EntityValidationResult(TEntity entity)
        //{
        //    Entity = entity;
        //    Result = entity.Validate();
        //}
    }

    public static class EntityValidation
    {
        //public static EntityValidationResult<TEntity> Validate<TEntity>(TEntity entity) where TEntity : BaseEntity
        //{
        //    return new EntityValidationResult<TEntity>(entity);
        //}

        public static EntityValidationResult<TEntity> Validate<TEntity>(TEntity entity, IEnumerable<ValidationResult> validationResult) where TEntity : BaseEntity
        {
            return new EntityValidationResult<TEntity>(entity, validationResult);
        }

        public static EntityValidationResult<TEntity> Validate<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            return new EntityValidationResult<TEntity>(entity);
        }
    }

    public class EntityValidator<TValidator, TEntity> where TValidator : BaseValidator<TEntity>
    {
        public EntityValidator(params Expression<Func<TValidator, object>> [] expressions)
        {

        }
    }

    public class QueryBuilder<TEntity>
    {
        public string[] Fields;

        public QueryBuilder<TEntity> Select<TProperty>(params Expression<Func<TEntity, TProperty>>[] fields)
        {

            //fields[0].GetPropertyName
            return this;
        }

        public QueryBuilder<TProperty> SelectFromList<TProperty>(Expression<Func<TEntity, IEnumerable<TProperty>>> field) //where TProperty: IEnumerable
        {
            //fields[0].GetPropertyName
            return new QueryBuilder<TProperty>();
        }

        public QueryBuilder<TProperty> SelectFrom<TProperty>(Expression<Func<TEntity, TProperty>> field) //where TProperty: IEnumerable
        {
            //fields[0].GetPropertyName
            return new QueryBuilder<TProperty>();
        }

        public QueryBuilder<TEntity> Where(string where)
        {
            return this;
        }

        public QueryBuilder<TEntity> OrderBy(string where)
        {
            return this;
        }

        public QueryBuilder<TEntity> Skip(int skip)
        {
            return this;
        }

        public QueryBuilder<TEntity> Take(int take)
        {
            return this;
        }

    }

    public class Pagination
    {
        public Pagination(string name, int? skip, int? take)
        {
            Name = name;
            PageSize = take.Value;
            Page = ((skip ?? 0) / PageSize) + 1;
        }

        public string Name { get; set; }
        public int Count { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
    }

    public interface IQuerySpecs: IQueryNode
    {
        Type Type { get; set; }
        //Pagination Pagination { get; set; }
    }

    //public class QuerySpecs : QueryNode, IQuerySpecs
    //{
    //    public QuerySpecs(string name, object where, int? skip, int? take, object orderBy, List<IQueryNode> selections, Type type)
    //        :base(name, where, skip, take, orderBy, selections, type)
    //    {
    //    }

    //    public QuerySpecs(string name, List<IQueryNode> selections, Type type)
    //        : base(name, selections, type)
    //    {
    //    }

    //    public Type Type { get; set; }

    //    public IQueryable ToQueryable()
    //    {
    //        return null;
    //    }
    //}
    public class Props
    {
        public string PropertyName { get; set; }
        public object Value { get; set; }

        public Collection<object> collectionIds { get; set; }
    }

    public interface IQueryNode
    {
        bool HasSelection { get; }
        bool IsCollection { get; set; }
        string Name { get; set; }
        object OrderBy { get; set; }
        List<IQueryNode> Selections { get; set; }
        int? Skip { get; set; }
        int? Take { get; set; }
        //Type Type { get; set; }
        object Where { get; set; }
        Pagination Pagination { get; set; }
        object Mutation { get; set; }
        ///List<Props> MutationFields { get; set; }
    }

    public class QueryNode : IQueryNode
    {
        public string Name { get; set; }
        public object Where { get; set; }
        public int? Skip { get; set; }
        public int? Take { get; set; }
        public object OrderBy { get; set; }
        public List<IQueryNode> Selections { get; set; }

        public bool IsCollection { get; set; }
        public bool HasSelection => Selections != null && Selections.Any();
        public Type BaseType { get; set; }
        public string Path { get; set; }
        public Pagination Pagination { get; set; }
        public bool ReturnPagination { get; set; }
        public object Mutation { get; set; }
        //public List<Props> MutationFields { get; set; }
        //
        public QueryNode()
        {

        }
        public QueryNode(string name, object where, int? skip, int? take, object orderBy, bool paging, List<IQueryNode> selections, Type type)
        {
            Name = name;
            Where = where;
            Skip = skip;
            Take = take;
            OrderBy = orderBy;
            Selections = selections;
            IsCollection = type is IEnumerable;
            ReturnPagination = paging;
            //Type = type;
        }

        public QueryNode(string name, List<IQueryNode> selections, Type type)
        {
            Name = name;
            Selections = selections;
            IsCollection = type is IEnumerable;
            //Type = type;
        }
    }

    public class QueryBuilder
    {
        public string[] Properties;
        public QueryBuilder Select(string where, params string [] properties)
        {
            return this;
        }

        public QueryBuilder SelectFrom(string navigationProperty, params string[] properties)
        {
            return this;
        }

        public QueryBuilder Where(string where)
        {
            return this;
        }

        public QueryBuilder OrderBy(string where)
        {
            return this;
        }

        public QueryBuilder Skip(int skip)
        {
            return this;
        }

        public QueryBuilder Take(int take)
        {
            return this;
        }
    }

    public class ServiceBase<TEntity> : IServiceBase, IServiceBase<TEntity> where TEntity : BaseEntity
    {
        IRepositoryBase<TEntity> _repository;
        IServiceProvider _serviceProvider;
        BaseValidator<TEntity> _validator;
        //public ServiceBase(IRepositoryBase<TEntity> repository)

        protected IRepositoryBase<TEntity> GetRepository()
        {
            //var query = new QueryBuilder<Coleta>();

            //query.Select(x => x.DataInicial, x => x.DataFinal);
            //    query.SelectFrom(x => x.AmostraDagua).Select(x => x.Id);
            //    query.SelectFrom(x => x.Ambiente).SelectFromList(x => x.Coletas);

            if (_repository == null)
            {
                _repository = (IRepositoryBase<TEntity>)_serviceProvider.GetService(typeof(IRepositoryBase<TEntity>));
            }
            return _repository;
        }

        protected TRepository GetRepository<TRepository>()
        {
            return (TRepository)_serviceProvider.GetService(typeof(TRepository));
        }

        protected TService GetService<TService>()
        {
            return (TService)_serviceProvider.GetService(typeof(TService));
        }


        public ServiceBase(IServiceProvider serviceProvider)
        {
            //_repository = repository;
            _serviceProvider = serviceProvider;
        }

        public virtual TEntity Get(long id, IQueryNode queryDetails = null)
        {
            //return _repository.Get(id, queryDetails);
            return GetRepository().Get(id, queryDetails);
        }

        public virtual IEnumerable<TEntity> GetAll(IQueryNode queryDetails = null)
        {
            //return _repository.GetAll(queryDetails);
            return GetRepository().GetAll(queryDetails);
        }

        public virtual TEntity Add(TEntity entity, IDictionary<string, object> input)
        {
            var validation = Validate(entity);
            // if (validation.IsValid())
            // {
                return GetRepository().Add(entity, input);
            // }
            // return validation.Entity;
        }
        public virtual TEntity Update(TEntity entity, IDictionary<string, object> inputs)
        {
            var validation = Validate(entity);
            // if (validation.IsValid())
            // {
                return GetRepository().Update(entity, inputs);
            // }
            // return validation.Entity;
        }

        protected virtual EntityValidationResult<TEntity> Validate(TEntity entity)
        {
            var validator = (IContextValidator<TEntity>)_serviceProvider.GetService(typeof(IContextValidator<TEntity>));

            if (validator != null)
            {
                return EntityValidation.Validate(entity, new List<ValidationResult> { validator.ValidateFromContext(null, entity) });
            }

            else
            {
                return EntityValidation.Validate(entity, new List<ValidationResult> { new ValidationResult() });
            }
        }
        public virtual void Remove(long id)
        {
            GetRepository().Remove(id);
        }

        public virtual void RemoveMany(long[] ids)
        {
            foreach (var id in ids)
            {
                Remove(id);
            }
        }

        public virtual object GetGeneric(long id, IQueryNode queryDetails = null) => Get(id, queryDetails);

        public virtual object AddGeneric(object entity, IDictionary<string, object> input) => Add((TEntity)entity, input);
        public object AddRangeGeneric(object entities, IList<IDictionary<string, object>> inputs) => AddRange((IEnumerable<TEntity>)entities, inputs);

        public virtual object GetAllGeneric(IQueryNode queryDetails = null) => GetAll(queryDetails);

        public virtual void RemoveGeneric(long id) => Remove(id);

        public object UpdateGeneric(object entity, IDictionary<string, object> inputs) => Update((TEntity)(entity), inputs);

        public object UpdateRangeGeneric(object entities, IList<IDictionary<string, object>> inputs) => UpdateRange((IEnumerable<TEntity>)entities, inputs);

        public IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities, IList<IDictionary<string, object>> inputs)
        {
            int index = 0;
            foreach (var entity in entities)
            {
                Add(entity, inputs[index]);
                index++;
            }
            return entities;
        }

        public IEnumerable<TEntity> UpdateRange(IEnumerable<TEntity> entities, IList<IDictionary<string, object>> input)
        {
            for (int i = 0; i < entities.Count(); i++)
            {
                yield return Update(entities.ElementAt(i), input.ElementAt(i));
            }
        }

        public void SaveChanges()
        {
            GetRepository().SaveChanges();
        }

        public IDictionary<string, int> CountBy(string where, string groupBy)
        {
            return GetRepository().CountBy(where, groupBy);
        }

        public IEnumerable<TEntity> GetWhere(IQueryNode queryDetails, Expression<Func<TEntity, bool>> predicate)
        {
            return GetRepository().GetWhere(null, predicate);
        }

        //IDictionary<string, int> IServiceBase.CountBy(string where, string groupBy)
        //{
        //    throw new NotImplementedException();
        //}

        public object Aggregate(string where, string groupBy, params string[] functions)
        {
            
            return GetRepository().Aggregate(where, groupBy, functions);
        }
    }
}

public static class ext
{
    public static ICollection ToCollection(this IEnumerable enumerable)
    {
        if(enumerable != null)
        {
            var collection = new Collection<object>();
            foreach (var item in enumerable)
            {
                collection.Add(item);
            }
            return collection;
        }
        return null;        
    }
}

