using EFCore.Test;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Update.Internal;
using Microsoft.EntityFrameworkCore.Update;


// ReSharper disable once CheckNamespace
namespace Microsoft.EntityFrameworkCore
{
    public static class SqlServerDbContextOptionsExtensions
    {
        public static DbContextOptionsBuilder EnableWithChangeTracingContext(this DbContextOptionsBuilder optionsBuilder)
        {
            var sqlServerOptionsExtension = optionsBuilder.Options.FindExtension<SqlServerOptionsExtension>();
            if (sqlServerOptionsExtension == null)
                return optionsBuilder;
          
            optionsBuilder = optionsBuilder
                .ReplaceService<IModificationCommandBatchFactory, WithChangeTracingContextSqlServerModificationCommandBatchFactory>();
            return optionsBuilder.ReplaceService<ISqlServerUpdateSqlGenerator, WithChangeTracingContextSqlServerUpdateSqlGenerator>();
        }
    }
}
