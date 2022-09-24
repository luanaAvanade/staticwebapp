using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace EFCore.BulkExtensions
{
    public static class IQueryableBatchExtensions
    {
        public static int BatchDelete<T>(this IQueryable<T> query) where T : class
        {
            DbContext context = BatchUtil.GetDbContext(query);
            (string sql, var sqlParameters) = BatchUtil.GetSqlDelete(query, context);
            return context.Database.ExecuteSqlCommand(sql, sqlParameters);
        }

        public static int BatchUpdate<T>(this IQueryable<T> query, T updateValues, List<string> updateColumns = null) where T : class, new()
        {
            DbContext context = BatchUtil.GetDbContext(query);
            var (sql, sqlParameters) = BatchUtil.GetSqlUpdate(query, context, updateValues, updateColumns);
            return context.Database.ExecuteSqlCommand(sql, sqlParameters);
        }


        public static int BatchUpdate<T>(this IQueryable<T> query, Expression<Func<T, T>> updateExpression) where T : class
        {
            var context = BatchUtil.GetDbContext(query);
            var (sql, sqlParameters) = BatchUtil.GetSqlUpdate(query, updateExpression);
            return context.Database.ExecuteSqlCommand(sql, sqlParameters);
        }

        // Async methods

        public static async Task<int> BatchDeleteAsync<T>(this IQueryable<T> query, CancellationToken cancellationToken = default(CancellationToken)) where T : class
        {
            DbContext context = BatchUtil.GetDbContext(query);
            DbServer databaseType = context.Database.ProviderName.EndsWith(DbServer.Sqlite.ToString()) ? DbServer.Sqlite : DbServer.SqlServer;
            (string sql, var sqlParameters) = BatchUtil.GetSqlDelete(query, context);
            return await context.Database.ExecuteSqlCommandAsync(sql, sqlParameters, cancellationToken).ConfigureAwait(false);
        }

        public static async Task<int> BatchUpdateAsync<T>(this IQueryable<T> query, T updateValues, List<string> updateColumns = null, CancellationToken cancellationToken = default(CancellationToken)) where T : class, new()
        {
            DbContext context = BatchUtil.GetDbContext(query);
            var (sql, sqlParameters) = BatchUtil.GetSqlUpdate(query, context, updateValues, updateColumns);
            return await context.Database.ExecuteSqlCommandAsync(sql, sqlParameters, cancellationToken).ConfigureAwait(false);
        }

        public static async Task<int> BatchUpdateAsync<T>(this IQueryable<T> query, Expression<Func<T, T>> updateExpression, CancellationToken cancellationToken = default(CancellationToken)) where T : class
        {
            var context = BatchUtil.GetDbContext(query);
            var (sql, sqlParameters) = BatchUtil.GetSqlUpdate(query, updateExpression);
            return await context.Database.ExecuteSqlCommandAsync(sql, sqlParameters, cancellationToken).ConfigureAwait(false);
        }
    }
}
