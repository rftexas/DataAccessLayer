using System.Data;
using System;

namespace DataAccessLayer.Tests
{
    public class TestQueryTransform<T, TOut> : DataAccess.IQuery<T>.IWithTransform<TOut> where T : new()
    {

        private readonly Func<T,TOut> _transform;
        public TestQueryTransform(string query, Func<T,TOut> transform)
        {
            QueryText = query;
            _transform = transform;
        }



        public string QueryText { get; }

        public CommandType CommandType{ get; } = CommandType.Text;

        public object Parameters { get; }

        public TOut Transform(T item) => _transform(item);
    }
}
