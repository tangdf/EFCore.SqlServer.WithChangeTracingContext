using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Microsoft.EntityFrameworkCore.SqlServer.Update.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace EFCore.Test
{
    public class WithChangeTracingContextSqlServerModificationCommandBatch: SqlServerModificationCommandBatch
    {
        private readonly IRelationalCommandBuilderFactory _commandBuilderFactory;
        public WithChangeTracingContextSqlServerModificationCommandBatch(IRelationalCommandBuilderFactory commandBuilderFactory, ISqlGenerationHelper sqlGenerationHelper, ISqlServerUpdateSqlGenerator updateSqlGenerator, IRelationalValueBufferFactoryFactory valueBufferFactoryFactory, int? maxBatchSize)
            : base(commandBuilderFactory, sqlGenerationHelper, updateSqlGenerator, valueBufferFactoryFactory, maxBatchSize)
        {
            _commandBuilderFactory = commandBuilderFactory;
        }

        protected override RawSqlCommand CreateStoreCommand()
        {

            if (ChangeTracingContext.CurrentContextName == null)
            {
                return base.CreateStoreCommand();
            }

            var commandBuilder = _commandBuilderFactory
                .Create()
                .Append(GetCommandText());

            var parameterValues = new Dictionary<string, object>(GetParameterCount()+1);

            // ReSharper disable once ForCanBeConvertedToForeach
            for (var commandIndex = 0; commandIndex < ModificationCommands.Count; commandIndex++)
            {
                var command = ModificationCommands[commandIndex];
                // ReSharper disable once ForCanBeConvertedToForeach
                for (var columnIndex = 0; columnIndex < command.ColumnModifications.Count; columnIndex++)
                {
                    var columnModification = command.ColumnModifications[columnIndex];
                    if (columnModification.UseCurrentValueParameter)
                    {
                        commandBuilder.AddParameter(
                            columnModification.ParameterName,
                            SqlGenerationHelper.GenerateParameterName(columnModification.ParameterName),
                            columnModification.Property);

                        parameterValues.Add(columnModification.ParameterName, columnModification.Value);
                    }

                    if (columnModification.UseOriginalValueParameter)
                    {
                        commandBuilder.AddParameter(
                            columnModification.OriginalParameterName,
                            SqlGenerationHelper.GenerateParameterName(columnModification.OriginalParameterName),
                            columnModification.Property);

                        parameterValues.Add(columnModification.OriginalParameterName, columnModification.OriginalValue);
                    }
                }
            }

            var invariantName = "Change_Tracking_Context";
            var parameterValue = Encoding.UTF8.GetBytes(ChangeTracingContext.CurrentContextName);
            var parameter = new SqlParameter(SqlGenerationHelper.GenerateParameterName(invariantName), SqlDbType.VarBinary, 128)
            {
                Value = parameterValue
            };
            commandBuilder.AddRawParameter(invariantName,parameter);

            parameterValues.Add(invariantName, parameterValue);

            return new RawSqlCommand(commandBuilder.Build(), parameterValues);
        }
    }
}
