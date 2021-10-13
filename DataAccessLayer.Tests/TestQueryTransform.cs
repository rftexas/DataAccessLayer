namespace DataAccessLayer.Tests
{
    public class TestQueryTransform<T> : DataAccess.Query<T>.WithoutTransform where T : new()
    {
        public TestQueryTransform(string query)
        {
            QueryText = query;
        }
    }
}
