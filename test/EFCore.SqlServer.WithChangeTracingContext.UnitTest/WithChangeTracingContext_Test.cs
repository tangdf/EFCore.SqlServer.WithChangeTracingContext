using System;
using System.Linq;
using System.Text;
using EFCore.Test;
using Xunit;

namespace EFCore.SqlServer.WithChangeTracingContext.UnitTest
{
    public class WithChangeTracingContext_Test
    {
 

 
 
        [Fact]
        public void WithChangeTracking_Test()
        {
            using (var dataContext = new SampleDbContext())
            {
                var id = 2;
                var entity = dataContext.Categories.Single(item => item.CategoryID == id);

                entity.CategoryName = Guid.NewGuid().ToString();
                dataContext.SaveChangesWithChangeTracingContext("testContext");
            }

            var sql = DumpSql();

            Assert.Contains("With Change_Tracking_Context(@Change_Tracking_Context)", sql);

            using (var dataContext = new SampleDbContext())
            {
                var id = 2;
                var entity = dataContext.Categories.Single(item => item.CategoryID == id);

                entity.CategoryName = Guid.NewGuid().ToString();
                dataContext.SaveChanges();
            }

              sql = DumpSql();

            Assert.DoesNotContain("With Change_Tracking_Context(@Change_Tracking_Context)", sql);

        }
 

        private static string DumpSql()
        {
            StringBuilder stringBuilder=new StringBuilder();
            foreach (var logMessage in SampleDbContext.LogMessages)
            {
                stringBuilder.AppendLine(logMessage);
            }

            return stringBuilder.ToString();
        }
    }
}