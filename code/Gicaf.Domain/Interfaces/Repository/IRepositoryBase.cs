using Gicaf.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Gicaf.Domain.Interfaces.Repository
{
    public interface IRepositoryBase<TEntity>
    {
        //TEntity Get(long id, object queryDetails = null);
        //IEnumerable<TEntity> GetAll(object queryDetails = null);
        //TEntity Add(TEntity obj);
        //TEntity Update(TEntity obj);
        //void Remove(long id);

        //TEntity Get(long id, object queryDetails, string[] inputs = null);
        TEntity Get(long id, object queryDetails);
        IEnumerable<TEntity> GetAll(object queryDetails = null);
        TEntity Add(TEntity obj, IDictionary<string, object> input);
        TEntity Update(TEntity obj, IDictionary<string, object> inputs);
        void Remove(long id);
        IEnumerable<TEntity> GetWhere(object selectDetails, Expression<Func<TEntity,bool>> predicate, params Expression<Func<TEntity,object>>[] includes);
        IDictionary<string, int> CountBy(string where, string groupBy);
        object Aggregate(string where, string groupBy, params string[] functions);
        void SaveChanges();
    }
}
