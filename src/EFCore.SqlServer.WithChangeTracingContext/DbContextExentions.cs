using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Test
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

