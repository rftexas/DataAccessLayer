using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace DataAccessLayer.Tests
{
    public class DataAccessQueryTests
    {
        public class Result
        {
            public int Id { get; set; }
            public string Name { get; set; }

        }

        public struct ResultStruct
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        [Fact]
        public async Task Handles_queries()
        {
            var expectedResult = new Result { Id = 1, Name = "First" };
            var conn = new FakeDbConnection("");
            conn.AddResultSet(expectedResult);

            var dal = new DbDataAccessLayer<DataAccessQueryTests>(new DataConnectionFactory<DataAccessQueryTests>("", _ => conn));

            var result = await dal.Query(new TestQuery<Result>("i"));
            result.Should().NotBeEmpty();
            result.Should().Contain(r => r.Id == expectedResult.Id && r.Name == expectedResult.Name);
        }


        [Fact]
        public async Task Handles_queries_with_structs()
        {
            var expectedResult = new ResultStruct { Id = 1, Name = "First" };
            var conn = new FakeDbConnection("");
            conn.AddResultSet(expectedResult);

            var dal = new DbDataAccessLayer<DataAccessQueryTests>(new DataConnectionFactory<DataAccessQueryTests>("", _ => conn));

            var result = await dal.Query(new TestQuery<ResultStruct>("i"));
            result.Should().NotBeEmpty();
            result.Should().Contain(r => r.Id == expectedResult.Id && r.Name == expectedResult.Name);
        }
    }
}
