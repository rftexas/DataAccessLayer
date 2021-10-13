namespace DataAccessLayer.Tests
{
    public class TestQuery<T>: DataAccess.Query<T>.WithoutTransform where T: new()
    {
        public TestQuery(string query)
        {
            QueryText = query;
        }
    }
}
