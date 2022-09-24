using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Gicaf.Infra.Data.Repositories
{
    public static class OrderByExtension
    {
        public static IQueryable<TEntity> OrderByPropertyName<TEntity>(this IQueryable<TEntity> source, string propertyName, bool isDescending)
            where TEntity : IEntity
        {
            if (source == null)
            {
                throw new ArgumentException("source");
            }

            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException("propertyName");
            }

            Type type = typeof(TEntity);
            ParameterExpression arg = Expression.Parameter(type, "x");
            PropertyInfo propertyInfo = type.GetProperty(propertyName);
            Expression expression = Expression.Property(arg, propertyInfo);
            type = propertyInfo.PropertyType;

            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(TEntity), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, expression, arg);

            var methodName = isDescending ? "OrderByDescending" : "OrderBy";
            object result = typeof(Queryable).GetMethods().Single(
                method => method.Name == methodName
                        && method.IsGenericMethodDefinition
                        && method.GetGenericArguments().Length == 2
                        && method.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(TEntity), type)
                .Invoke(null, new object[] { source, lambda });

            return (IQueryable<TEntity>)result;
        }
    }

    public static class EfRepositoryExtensions
    {
        public static async Task<TEntity> GetByIdAsync<TDbContext, TEntity>(
            this IEfQueryRepository<TDbContext, TEntity> repo,
            Guid id,
            params Expression<Func<TEntity, object>>[] includeProperties)
                where TDbContext : DbContext
                where TEntity : class, IEntity

        {
            var queryable = repo.Queryable().AsNoTracking() as IQueryable<TEntity>;

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    queryable = queryable.Include(includeProperty);
                }
            }

            return await queryable.SingleOrDefaultAsync(e => e.Id.Equals(id));
        }

        public static async Task<IReadOnlyList<TEntity>> ListAsync<TDbContext, TEntity>(
            this IEfQueryRepository<TDbContext, TEntity> repo,
            params Expression<Func<TEntity, object>>[] includeProperties)
                where TDbContext : DbContext
                where TEntity : class, IEntity
        {
            var queryable = repo.Queryable().AsNoTracking() as IQueryable<TEntity>;

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    queryable = queryable.Include(includeProperty);
                }
            }

            return await queryable.ToListAsync();
        }

        internal static async Task<PaginatedItem<TResponse>> QueryAsync<TDbContext, TEntity, TResponse>(
            this IEfQueryRepository<TDbContext, TEntity> repo,
            Criterion criterion,
            Expression<Func<TEntity, TResponse>> selector,
            params Expression<Func<TEntity, object>>[] includeProperties)
                where TDbContext : DbContext
                where TEntity : class, IEntity
        {
            return await GetDataAsync(repo, criterion, selector, null, includeProperties);
        }

        internal static async Task<PaginatedItem<TResponse>> FindAllAsync<TDbContext, TEntity, TResponse>(
            this IEfQueryRepository<TDbContext, TEntity> repo,
            Criterion criterion,
            Expression<Func<TEntity, TResponse>> selector,
            Expression<Func<TEntity, bool>> filter,
            params Expression<Func<TEntity, object>>[] includeProperties)
                where TDbContext : DbContext
                where TEntity : class, IEntity
        {
            return await GetDataAsync(repo, criterion, selector, filter, includeProperties);
        }

        internal static async Task<TEntity> FindOneAsync<TDbContext, TEntity>(
            this IEfQueryRepository<TDbContext, TEntity> repo,
            Expression<Func<TEntity, bool>> filter,
            params Expression<Func<TEntity, object>>[] includeProperties)
                where TDbContext : DbContext
                where TEntity : class, IEntity
        {
            var dbSet = repo.Queryable();
            foreach (var includeProperty in includeProperties)
            {
                dbSet = dbSet.Include(includeProperty);
            }

            return await dbSet.FirstOrDefaultAsync(filter);
        }

        private static async Task<PaginatedItem<TResponse>> GetDataAsync<TDbContext, TEntity, TResponse>(
            IEfQueryRepository<TDbContext, TEntity> repo,
            Criterion criterion,
            Expression<Func<TEntity, TResponse>> selector,
            Expression<Func<TEntity, bool>> filter = null,
            params Expression<Func<TEntity, object>>[] includeProperties)
                where TDbContext : DbContext
                where TEntity : class, IEntity
        {
            if (criterion.PageSize < 1 || criterion.PageSize > criterion.DefaultPagingOption.PageSize)
            {
                criterion.SetPageSize(criterion.DefaultPagingOption.PageSize);
            }

            var queryable = repo.Queryable();
            var totalRecord = await queryable.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalRecord / criterion.PageSize);

            if (includeProperties != null && includeProperties.Count() > 0)
            {
                queryable = includeProperties.Aggregate(
                    queryable,
                    (current, include) => current.Include(include));
            }

            if (filter != null)
                queryable = queryable.Where(filter);

            if (!string.IsNullOrWhiteSpace(criterion.SortBy))
            {
                var isDesc = string.Equals(criterion.SortOrder, "desc", StringComparison.OrdinalIgnoreCase) ? true : false;
                queryable = queryable.OrderByPropertyName(criterion.SortBy, isDesc);
            }

            if (criterion.CurrentPage > totalPages)
            {
                criterion.SetCurrentPage(totalPages);
            }

            var results = await queryable
                .Skip(criterion.CurrentPage * criterion.PageSize)
                .Take(criterion.PageSize)
                .AsNoTracking()
                .Select(selector)
                .ToListAsync();

            return new PaginatedItem<TResponse>(totalRecord, totalPages, results);
        }
    }


    public class QueryFluent<TEntity, TResponse> : IQueryFluent<TEntity, TResponse>
        where TEntity : class, IEntity
    {
        public readonly Expression<Func<TEntity, bool>> _filter;
        public readonly List<Expression<Func<TEntity, object>>> _includes;
        public Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> _orderBy;
        public Expression<Func<TEntity, TResponse>> _selector;
        public Criterion _criterion;
        private readonly IEfQueryRepository<TEntity> _repository;

        public QueryFluent(IEfQueryRepository<TEntity> repository)
        {
            _repository = repository;
            _includes = new List<Expression<Func<TEntity, object>>>();
        }

        public QueryFluent(IEfQueryRepository<TEntity> repository, IQueryObject<TEntity> queryObject)
            : this(repository) { _filter = queryObject.Query(); }

        public QueryFluent(IEfQueryRepository<TEntity> repository, Expression<Func<TEntity, bool>> filter)
            : this(repository) { _filter = filter; }

        public IQueryFluent<TEntity, TResponse> Include(Expression<Func<TEntity, object>> expression)
        {
            _includes.Add(expression);
            return this;
        }

        public IQueryFluent<TEntity, TResponse> Criterion(Criterion criterion)
        {
            _criterion = criterion;
            return this;
        }

        public IQueryFluent<TEntity, TResponse> OrderBy(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy)
        {
            _orderBy = orderBy;
            return this;
        }

        public IQueryFluent<TEntity, TResponse> Projection(Expression<Func<TEntity, TResponse>> selector)
        {
            _selector = selector;
            return this;
        }

        public async Task<PaginatedItem<TResponse>> ComplexQueryAsync()
        {
            return await _repository.QueryAsync(_criterion, _selector, _includes.ToArray());
        }

        public async Task<PaginatedItem<TResponse>> ComplexFindAllAsync()
        {
            if (_filter == null)
            {
                throw new InvalidOperationException("Need to set expression for the query.");
            }

            return await _repository.FindAllAsync(_criterion, _selector, _filter, _includes.ToArray());
        }

        public async Task<TEntity> ComplexFindOneAsync()
        {
            if (_filter == null)
            {
                throw new InvalidOperationException("Need to set expression for the query.");
            }

            return await _repository.FindOneAsync(_filter, _includes.ToArray());
        }
    }
}
