using Axxiom.Linq.Dynamic.Core;
using Gicaf.Domain.Entities;
using Gicaf.Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SisAgua.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EFCore.BulkExtensions;
using Gicaf.Domain.Entities.Fornecedores;
using System.Transactions;
using Microsoft.Extensions.Configuration;

namespace Gicaf.Infra.Data.Context
{
    public class RelatedEntities
    {
        public static bool ClearOrphans { get; set; } = false;
        public Type FkType { get; set; }
        public string FkName { get; set; }
        public object FkValue { get; set; }
        public List<object> PkValues { get; set; } = new List<object>();
        public bool IsNullable { get; set; }

        public RelatedEntities(Type fkType, string fkName, object fkValue, List<object> pkValues, bool isNullable)
        {
            FkType = fkType;
            FkName = fkName;
            FkValue = fkValue;
            PkValues = pkValues ?? new List<object>();
            IsNullable = isNullable;
        }

        public void AddPkValue(object pkValie)
        {
            PkValues.Add(pkValie);
        }
    }
    public class AppDbContext : BaseDBContext<BaseEntity>
    {
        //private List<ChaveAssociada> _chaveAssociadas { get; set; }
        private Dictionary<string, RelatedEntities> _relatedEntities;

        public RelatedEntities AddRelatedEntity(Type type, string fkName, object fkValue, bool nullable)
        {
            _relatedEntities = _relatedEntities ?? new Dictionary<string, RelatedEntities>();

           var pkValues = new List<object>();
           var relatedEntities = new RelatedEntities(type, fkName, fkValue, pkValues, nullable);
           _relatedEntities.Add(../../"{type.Name}.{fkName}", relatedEntities);
            
            return relatedEntities;
        }

        /*
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder().Build();
            var inMemoryDatabase = configuration.GetValue<bool>("InMemoryDatabase");

            if (inMemoryDatabase)
            {
                optionsBuilder.UseInMemoryDatabase("Gicaf");
            }
            else
            {
                optionsBuilder.UseSqlServer("Server=localhost;Database=PED653;Integrated Security=true;MultipleActiveResultSets=true");
            }

            base.OnConfiguring(optionsBuilder);
        }
        */
        public AppDbContext(DbContextOptions options):base(options)
        {
            ApplyConfigurationsFromAssembly(this.GetType().Assembly);
            ApplyCreatedDate(x => x.DataCriacao);
            ApplyModifiedDate(x => x.DataModificacao);
            //ReadPropertyValue
            //this.GetService<ILocalViewListener>()?.RegisterView(OnStateManagerChanged);
            //this.GetService<ILocalViewListener>()?.RegisterView(OnReadPropertyValue);
        }


        private void OnReadPropertyValue(InternalEntityEntry arg1, EntityState arg2)
        {
            throw new NotImplementedException();
        }

        //public AppDbContext(): this(new HostingEnvironment())
        //{
        //    _environment.ApplicationName = "SisAgua";
        //    _environment.ContentRootPath = Environment.CurrentDirectory;
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        void OnStateManagerChanged(InternalEntityEntry entry, EntityState previousState)
        {
            foreach (var item in entry.ToEntityEntry().References)
            {
                if(item.EntityEntry.State == EntityState.Added)
                {

                }
            } 


            if (previousState == EntityState.Detached && entry.EntityState == EntityState.Unchanged)
            {
                // Process loaded entity
                var entity = entry.Entity;
            }
        }

        public override int SaveChanges()
        {
            if (_relatedEntities?.Any() ?? false)
            {
                return SaveChangesWithRelations<long>();
            }
            return base.SaveChanges();
        }

        private int SaveChangesWithRelations<TFkId>() where TFkId : struct
        {
            using (TransactionScope scope = new TransactionScope())
            {
                int result = 0;
                
                foreach (var key in _relatedEntities.Keys)
                {
                    var relatedEntity = _relatedEntities[key];
                    var obj = Activator.CreateInstance(relatedEntity.FkType);
                    relatedEntity.FkType.GetProperty(relatedEntity.FkName).SetValue(obj, null);

                    string queryPredicate = ../../"{relatedEntity.FkName} = {relatedEntity.FkValue}";

                    if (relatedEntity.PkValues?.Any() ?? false)
                    {
                        string containsPkValuesString = string.Format("!new {0}[]{{{1}}}.Contains(Id)", typeof(TFkId).Name, string.Join(',', relatedEntity.PkValues));
                        queryPredicate += ../../" and {containsPkValuesString}";
                    }
                    var query = this.Set(relatedEntity.FkType, null).Where(queryPredicate);

                    if (RelatedEntities.ClearOrphans || !relatedEntity.IsNullable)
                    {
                        result = query.Cast<object>().BatchDelete();
                    }
                    else
                    {
                        result = query.Cast<object>().BatchUpdate(obj, new List<string> { relatedEntity.FkName });
                    }
                }
                
                result += base.SaveChanges();
                scope.Complete();
                return result;
            }
        }
    }
}
