using System.Data;

namespace DataAccessLayer.Tests
{
    public class TestQuery<T>: DataAccess.IQuery<T>.IWithoutTransform where T: new()
    {
        public TestQuery(string query)
        {
            QueryText = query;
        }

        public string QueryText { get; }

        public CommandType CommandType{ get; } = CommandType.Text;

        public object Parameters { get; }
    }
}
