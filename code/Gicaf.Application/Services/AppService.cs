using Gicaf.Application.Interface.Services;
using Gicaf.Domain.Interfaces;
using Gicaf.Domain.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Application.Services
{
    public abstract class AppService<TDTO, TEntity> : IAppService<TDTO>
    {
        protected IRepositoryBase<TEntity> _repository;
        public AppService(IRepositoryBase<TEntity> repository)
        {
            _repository = repository;
        }

        public virtual void Add(TDTO obj)
        {
            //_repository.Add(obj);
        }

        public virtual TDTO Get(long id)
        {
            return Activator.CreateInstance<TDTO>();
        }

        public virtual IEnumerable<TDTO> GetAll()
        {
            var lista = Activator.CreateInstance<List<TDTO>>();
            lista.Add(Activator.CreateInstance<TDTO>());
            lista.Add(Activator.CreateInstance<TDTO>());
            lista.Add(Activator.CreateInstance<TDTO>());

            return lista;
        }

        public virtual void Remove(long id)
        {
            throw new NotImplementedException();
        }

        public virtual void Update(TDTO obj)
        {
            throw new NotImplementedException();
        }
    }
}
