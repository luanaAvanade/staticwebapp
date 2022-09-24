using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Domain.Interfaces.Repository
{
    public interface IRepositoryResolver
    {
        T Resolve<T>(object obj) where T : class; 
    }
}
