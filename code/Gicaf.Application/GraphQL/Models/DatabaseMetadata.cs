using System.Collections.Generic;

namespace Gicaf.Application.GraphQL.Models
{
    public interface IDatabaseMetadata
    {
        //void ReloadMetadata();
        IEnumerable<EntityMetadata> GetTableMetadatas();
    }

    public sealed class DatabaseMetadata : IDatabaseMetadata
    {
        //private readonly DbContext _dbContext;
        private readonly ITableNameLookup _tableNameLookup;

        //private string _databaseName;
        private IEnumerable<EntityMetadata> _tables;

        public DatabaseMetadata(IEnumerable<EntityMetadata> tables, ITableNameLookup tableNameLookup)
        {
            _tables = tables;
            _tableNameLookup = tableNameLookup;

            //_databaseName = _dbContext.Database.GetDbConnection().Database;
        }

        public IEnumerable<EntityMetadata> GetTableMetadatas()
        {
            if (_tables == null)
                return new List<EntityMetadata>();

            return _tables;
        }
    }
}
