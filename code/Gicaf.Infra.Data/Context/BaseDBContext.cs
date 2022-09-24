using Gicaf.Domain.Entities;
using Gicaf.Domain.Entities.Base;
using Gicaf.Domain.Entities.Common;
using Gicaf.Infra.Data.Mappings;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Logging.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace SisAgua.Infra
{
    /*
    public class Teste<T> : DbSet<T> where T: class
    {
        public Teste(DbSet<T> dbSet)
        {
            this = dbSet;
        }
    }

    public class LoggingInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            if (Attribute.IsDefined(invocation.Method, typeof(LogAttribute))
            {
                Console.Writeline("Method called: " + invocation.Method.Name);
            }
            invocation.Proceed();
        }
    }
    */
    public abstract class BaseDBContext<TBaseEntity> : DbContext where TBaseEntity : class
    {
        public BaseDBContext(DbContextOptions options):base(options)
        {
        }
        //protected readonly IHostingEnvironment _environment;
        //protected bool _autoconfigFromBaseEntities;
        protected List<Assembly> _assembliesToApplyConfigurations;
        protected List<Func<Type, bool>> _assembliesToApplyConfigurationsPredicate;
        protected PropertyInfo _createdDate;
        protected PropertyInfo _modifiedDate;
        protected bool _inMemoryDatabase;

        public void ApplyCreatedDate(Expression<Func<TBaseEntity, DateTime>> expression)
        {
            _createdDate = GetProperty(expression);
        }

        public void ApplyModifiedDate(Expression<Func<TBaseEntity, DateTime>> expression)
        {
            _modifiedDate = GetProperty(expression);
        }

        private PropertyInfo GetProperty(Expression<Func<TBaseEntity, DateTime>> expression)
        {
            try
            {
                var propertyName = ((MemberExpression)expression.Body).Member.Name;
                return typeof(TBaseEntity).GetProperty(propertyName);
            }
            catch
            {
                throw new ArgumentException("Invalid parameter, must be a property expression");
            }
        }

        public virtual void ApplyConfigurationsFromAssembly(Assembly assembly, Func<Type, bool> predicate = null)
        {
            _assembliesToApplyConfigurations = _assembliesToApplyConfigurations ?? new List<Assembly>();
            _assembliesToApplyConfigurationsPredicate = _assembliesToApplyConfigurationsPredicate ?? new List<Func<Type, bool>>();

            _assembliesToApplyConfigurations.Add(assembly);
            _assembliesToApplyConfigurationsPredicate.Add(predicate);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (_assembliesToApplyConfigurations != null && _assembliesToApplyConfigurations.Any())
            {
                for (int i = 0; i < _assembliesToApplyConfigurations.Count; i++)
                {
                    var assembly = _assembliesToApplyConfigurations[i];
                    var predicate = _assembliesToApplyConfigurationsPredicate[i];
                    modelBuilder.ApplyConfigurationsFromAssembly(assembly, predicate);
                }
            }
            //modelBuilder.ApplyConfiguration(new UsuarioMap(new DefaultMapSettings()));
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected void FillCreatedEntityValues(object entity)
        {
            //if (_createdDate != null)
            //{
                try
                {
                    _createdDate.SetValue(entity, DateTime.Now);
                }
                catch { }
                
            //}

            //if (_modifiedDate != null)
            //{
                try
                {
                    _modifiedDate.SetValue(entity, DateTime.Now);
                }
                catch
                {

                }
            //}
        }

        protected void FillModifiedEntityValues(object entity)
        {
            if (_modifiedDate != null)
            {
                _modifiedDate.SetValue(entity, DateTime.Now);
            }
        }

        public override int SaveChanges()
        {
            if(ChangeTracker.HasChanges())
            {
                foreach (var entry in ChangeTracker.Entries())
                {
                    if(entry.State == EntityState.Added)
                    {
                        FillCreatedEntityValues(entry.Entity);
                    }

                    if(entry.State == EntityState.Modified)
                    {
                        entry.Property(nameof(BaseEntity.DataCriacao)).IsModified = false;
                        FillModifiedEntityValues(entry.Entity);   
                    }
                }
            }
            return base.SaveChanges();
        }
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

    public static bool IsNullableType(this Type type)
    {
        return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
    }

    public static bool IsConcreteClass(this Type type)
    {
        return type.IsClass && !type.IsAbstract;
    }
}