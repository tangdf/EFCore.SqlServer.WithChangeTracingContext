using System.Threading;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace Microsoft.EntityFrameworkCore
{
    public static class DbContextExentions
    {
        public static int SaveChangesWithChangeTracingContext(this DbContext dbContext,string changeTracingContext)
        {
            using (new ChangeTracingContext(changeTracingContext))
            {
               return dbContext.SaveChanges();
            }
        }

        public static async Task<int> SaveChangesWithChangeTracingContextAsync(this DbContext dbContext, string changeTracingContext, CancellationToken cancellationToken = default)
        {
            using (new ChangeTracingContext(changeTracingContext))
            {
                return await dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}

