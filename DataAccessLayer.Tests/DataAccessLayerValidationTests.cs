using DataAccessLayer.Exceptions;
using System.Threading.Tasks;
using Xunit;

namespace DataAccessLayer.Tests
{
    public class DataAccessLayerValidationTests
    {
        private readonly AdoDataAccessLayer<DataAccessLayerValidationTests> _dal = new(new DataConnectionFactory<DataAccessLayerValidationTests>("", s => new FakeDbConnection(s)));

        [Fact]
        public async Task Validates_queries()
        {
            await Assert.ThrowsAsync<InvalidQueryException>(() => _dal.Query(new TestQuery<int>("")));
        }

        [Fact]
        public async Task Validates_queries_with_transforms()
        {
            await Assert.ThrowsAsync<InvalidQueryException>(() => _dal.Query(new TestQueryTransform<int>("")));
        }

        [Fact]
        public async Task Validates_commands()
        {
            await Assert.ThrowsAsync<InvalidCommandException>(() => _dal.Execute(new TestCommand("")));
        }
    }
}
