using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Gicaf.Infra.Data.Context;
using LinqKit;

namespace Gicaf.Infra.Data.Repositories
{
    public class PaginationOption
    {
        public int PageSize { get; set; }
    }

    public static class HashCodeExtensions
    {
        public static int CombineHashCodes(this IEnumerable<object> objs)
        {
            unchecked
            {
                var hash = 17;
                foreach (var obj in objs)
                    hash = hash * 23 + (obj?.GetHashCode() ?? 0);
                return hash;
            }
        }
    }

    public abstract class ValueObjectBase
    {
        protected abstract IEnumerable<object> GetEqualityComponents();

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (ReferenceEquals(null, obj)) return false;
            if (GetType() != obj.GetType()) return false;
            var vo = obj as ValueObjectBase;
            return GetEqualityComponents().SequenceEqual(vo.GetEqualityComponents());
        }

        public override int GetHashCode()
        {
            return HashCodeExtensions.CombineHashCodes(GetEqualityComponents());
        }
    }

    public class Criterion : ValueObjectBase
    {
        public Criterion(int currentPage, int pageSize, PaginationOption defaultPagingOption, string sortBy = "", string sortOrder = "")
        {
            if (currentPage <= 0)
                throw new Exception("CurrentPage could not be less than zero.");

            if (pageSize <= 0)
                throw new Exception("PageSize could not be less than zero.");

            CurrentPage = currentPage - 1;
            PageSize = pageSize;
            DefaultPagingOption = defaultPagingOption;
            SortBy = sortBy;
            SortOrder = sortOrder;
        }

        public int CurrentPage { get; private set; }
        public int PageSize { get; private set; }
        public string SortBy { get; private set; }
        public string SortOrder { get; private set; }
        public PaginationOption DefaultPagingOption { get; private set; }

        public Criterion SetPageSize(int pageSize)
        {
            if (pageSize <= 0)
                throw new Exception("PageSize could not be less than zero.");

            PageSize = pageSize;
            return this;
        }

        public Criterion SetCurrentPage(int currentPage)
        {
            if (currentPage <= 0)
                throw new Exception("CurrentPage could not be less than zero.");

            CurrentPage = currentPage;
            return this;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return CurrentPage;
            yield return PageSize;
            yield return DefaultPagingOption;
            yield return SortBy;
            yield return SortOrder;
        }
    }

    public interface IIdentity
    {
        Guid Id { get; }
    }

    public interface IEntity : IIdentity
    { }

    public class PaginatedItem<TResponse> : ValueObjectBase
    {
        public PaginatedItem(int totalItems, int totalPages, IReadOnlyList<TResponse> items)
        {
            TotalItems = totalItems;
            TotalPages = totalPages;
            Items = items;
        }

        public int TotalItems { get; private set; }

        public long TotalPages { get; private set; }

        public IReadOnlyList<TResponse> Items { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return TotalItems;
            yield return TotalPages;
            yield return Items;
        }
    }

    public interface IQueryFluent<TEntity, TResponse> where TEntity : IEntity
    {
        IQueryFluent<TEntity, TResponse> OrderBy(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy);
        IQueryFluent<TEntity, TResponse> Include(Expression<Func<TEntity, object>> expression);
        IQueryFluent<TEntity, TResponse> Projection(Expression<Func<TEntity, TResponse>> selector);
        IQueryFluent<TEntity, TResponse> Criterion(Criterion criterion);
        Task<PaginatedItem<TResponse>> ComplexQueryAsync();
        Task<PaginatedItem<TResponse>> ComplexFindAllAsync();
        Task<TEntity> ComplexFindOneAsync();
    }

    public interface IQueryRepository<TEntity> where TEntity : IEntity
    {
        IQueryFluent<TEntity, TResponse> Return<TResponse>(IQueryObject<TEntity> queryObject);
        IQueryFluent<TEntity, TResponse> Return<TResponse>(Expression<Func<TEntity, bool>> query);
        IQueryFluent<TEntity, TResponse> Return<TResponse>();
        IQueryable<TEntity> Queryable();
    }

    public interface IEfQueryRepository<TDbContext, TEntity> : IQueryRepository<TEntity>
        where TDbContext : DbContext
        where TEntity : IEntity
    {
    }

    public interface IEfQueryRepository<TEntity> : IEfQueryRepository<AppDbContext, TEntity>
        where TEntity : IEntity
    {
    }

    public interface IQueryObject<TEntity> where TEntity : IEntity
    {
        Expression<Func<TEntity, bool>> Query();
        Expression<Func<TEntity, bool>> And(Expression<Func<TEntity, bool>> query);
        Expression<Func<TEntity, bool>> Or(Expression<Func<TEntity, bool>> query);
        Expression<Func<TEntity, bool>> And(IQueryObject<TEntity> queryObject);
        Expression<Func<TEntity, bool>> Or(IQueryObject<TEntity> queryObject);
    }

    public abstract class EntityBase : IEntity
    {
        //protected List<IDomainEvent> Events = new List<IDomainEvent>();

        protected EntityBase() : this(/*IdHelper.GenerateId()*/ Guid.NewGuid())
        {
        }

        protected EntityBase(Guid id)
        {
            Id = id;
            //Created = DateTimeHelper.GenerateDateTime();
        }

        public Guid Id { get; protected set; }

        public DateTime Created { get; protected set; }

        public DateTime Updated { get; protected set; }

        //public List<IDomainEvent> GetEvents()
        //{
        //    return Events;
        //}

        //public EntityBase RemoveEvent(IDomainEvent @event)
        //{
        //    if (Events.Find(e => e == @event) != null)
        //    {
        //        Events.Remove(@event);
        //    }
        //    return this;
        //}

        //public EntityBase RemoveAllEvents()
        //{
        //    Events = new List<IDomainEvent>();
        //    return this;
        //}
    }

    public abstract class QueryObject<TEntity> : IQueryObject<TEntity>
        where TEntity : EntityBase
    {
        private Expression<Func<TEntity, bool>> _query;

        public virtual Expression<Func<TEntity, bool>> Query()
        {
            return _query;
        }

        public Expression<Func<TEntity, bool>> And(Expression<Func<TEntity, bool>> query)
        {
            return _query = _query == null ? query : _query.And(query.Expand());
        }

        public Expression<Func<TEntity, bool>> Or(Expression<Func<TEntity, bool>> query)
        {
            return _query = _query == null ? query : _query.Or(query.Expand());
        }

        public Expression<Func<TEntity, bool>> And(IQueryObject<TEntity> queryObject)
        {
            return And(queryObject.Query());
        }

        public Expression<Func<TEntity, bool>> Or(IQueryObject<TEntity> queryObject)
        {
            return Or(queryObject.Query());
        }
    }


    public class EfQueryRepository<TDbContext, TEntity> : IEfQueryRepository<TDbContext, TEntity>
        where TDbContext : DbContext
        where TEntity : class, IEntity
    {
        private readonly TDbContext _dbContext;

        public EfQueryRepository(TDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbContext.ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public IQueryFluent<TEntity, TResponse> Return<TResponse>(IQueryObject<TEntity> queryObject)
            => new QueryFluent<TEntity, TResponse>(this as IEfQueryRepository<TEntity>, queryObject);

        public IQueryFluent<TEntity, TResponse> Return<TResponse>(Expression<Func<TEntity, bool>> query)
            => new QueryFluent<TEntity, TResponse>(this as IEfQueryRepository<TEntity>, query);

        public IQueryFluent<TEntity, TResponse> Return<TResponse>()
            => new QueryFluent<TEntity, TResponse>(this as IEfQueryRepository<TEntity>);

        public IQueryable<TEntity> Queryable() => _dbContext.Set<TEntity>();
    }

    public class Teste : EntityBase
    {
        
    }

}
