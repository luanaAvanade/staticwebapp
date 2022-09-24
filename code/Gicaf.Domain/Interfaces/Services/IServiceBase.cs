using Gicaf.Domain.Entities.Base;
using Gicaf.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Gicaf.Domain.Interfaces.Services
{
    public interface IServiceBase
    {
        object AddGeneric(object entity, IDictionary<string, object> input);
        object AddRangeGeneric(object entities, IList<IDictionary<string, object>> inputs);
        object GetGeneric(long id, IQueryNode queryDetails = null);
        object GetAllGeneric(IQueryNode queryDetails = null);
        void RemoveGeneric(long id);
        object UpdateGeneric(object entity, IDictionary<string, object> inputs);
        object UpdateRangeGeneric(object entities, IList<IDictionary<string, object>> inputs);
        void SaveChanges();
        IDictionary<string, int> CountBy(string where, string groupBy);
        object Aggregate(string where, string groupBy, params string[] functions);
        void RemoveMany(long[] ids);
    }

    public interface IServiceBase<TEntity>: IServiceBase  where TEntity : BaseEntity
    {
        TEntity Add(TEntity entity, IDictionary<string, object> input);
        IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities, IList<IDictionary<string, object>> inputs);
        TEntity Get(long id, IQueryNode queryDetails = null);
        //IEnumerable<TEntity> GetAll(IQueryNode queryDetails = null);
        IEnumerable<TEntity> GetAll(IQueryNode queryDetails = null);
        void Remove(long id);
        TEntity Update(TEntity entity, IDictionary<string, object> inputs);
        IEnumerable<TEntity> UpdateRange(IEnumerable<TEntity> entities, IList<IDictionary<string, object>> inputs);
        IEnumerable<TEntity> GetWhere(IQueryNode queryDetails, Expression<Func<TEntity,bool>> predicate);
        //void SaveChanges();                
    }
}
