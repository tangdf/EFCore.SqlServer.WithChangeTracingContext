using System.Linq;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Update.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;

// ReSharper disable once CheckNamespace
namespace Microsoft.EntityFrameworkCore
{
    public class WithChangeTracingContextSqlServerModificationCommandBatchFactory : SqlServerModificationCommandBatchFactory
    {
 
        private readonly IRelationalCommandBuilderFactory _commandBuilderFactory;
        private readonly ISqlGenerationHelper _sqlGenerationHelper;
        private readonly ISqlServerUpdateSqlGenerator _updateSqlGenerator;
        private readonly IRelationalValueBufferFactoryFactory _valueBufferFactoryFactory;
        private readonly IDbContextOptions _options;
        public WithChangeTracingContextSqlServerModificationCommandBatchFactory(IRelationalCommandBuilderFactory commandBuilderFactory, ISqlGenerationHelper sqlGenerationHelper, ISqlServerUpdateSqlGenerator updateSqlGenerator, IRelationalValueBufferFactoryFactory valueBufferFactoryFactory, IDbContextOptions options)
            : base(commandBuilderFactory, sqlGenerationHelper, updateSqlGenerator, valueBufferFactoryFactory, options)
        {

            _commandBuilderFactory = commandBuilderFactory;
            _sqlGenerationHelper = sqlGenerationHelper;
            _updateSqlGenerator = updateSqlGenerator;
            _valueBufferFactoryFactory = valueBufferFactoryFactory;
            _options = options;
        }

        public override ModificationCommandBatch Create()
        {
            var optionsExtension = _options.Extensions.OfType<SqlServerOptionsExtension>().FirstOrDefault();

            return new WithChangeTracingContextSqlServerModificationCommandBatch(
                _commandBuilderFactory,
                _sqlGenerationHelper,
                _updateSqlGenerator,
                _valueBufferFactoryFactory,
                optionsExtension?.MaxBatchSize);
        }
       
    }
}
