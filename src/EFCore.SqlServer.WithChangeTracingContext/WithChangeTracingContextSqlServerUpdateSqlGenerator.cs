using System.Text;
using Microsoft.EntityFrameworkCore.SqlServer.Update.Internal;
using Microsoft.EntityFrameworkCore.Update;

// ReSharper disable once CheckNamespace
namespace Microsoft.EntityFrameworkCore
{
    public class WithChangeTracingContextSqlServerUpdateSqlGenerator : SqlServerUpdateSqlGenerator
    {

        public WithChangeTracingContextSqlServerUpdateSqlGenerator(UpdateSqlGeneratorDependencies dependencies)
            : base(dependencies)
        {
        }

        public override void AppendBatchHeader(StringBuilder commandStringBuilder)
        {
            base.AppendBatchHeader(commandStringBuilder);

            if (ChangeTracingContext.CurrentContextName != null)
            {
                commandStringBuilder.AppendLine("With Change_Tracking_Context(@Change_Tracking_Context)");
            }
        }


    }
}
