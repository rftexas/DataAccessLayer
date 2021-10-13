namespace DataAccessLayer
{
    public abstract partial class DataAccess
    {
        public abstract class Query<T> where T:new()
        {
            /// <summary>
            /// Use this subclass to create a query that will transform from <see cref="T"/> to <see cref="TOut"/>
            /// </summary>
            /// <typeparam name="TOut"></typeparam>
            public abstract class WithTransform<TOut> where TOut : new()
            {
                /// <summary>
                /// The text of the query that will be executed against the database.
                /// </summary>
                public string QueryText { get; protected set; }
                /// <summary>
                /// Transforms from queried type to desired type
                /// </summary>
                /// <param name="original"></param>
                /// <returns></returns>
                public abstract TOut Transform(T original);
                /// <summary>
                /// Optional parameters for the query.
                /// </summary>
                public object Parameters { get; protected set; }
                /// <summary>
                /// The command type of the database interaction
                /// Defaults to <see cref="System.Data.CommandType.Text"/>
                /// </summary>
                public System.Data.CommandType CommandType { get; protected set; } = System.Data.CommandType.Text;

                internal bool Validate()
                {
                    return !string.IsNullOrEmpty(QueryText);
                }
            }

            /// <summary>
            /// Use this subclass to create a query that has no transform.
            /// </summary>
            public abstract class WithoutTransform
            {
                /// <summary>
                /// Optional parameters for the query.
                /// </summary>
                public string QueryText { get; protected set; }

                /// <summary>
                /// The text of the query that will be executed against the database.
                /// </summary>
                public object Parameters { get; protected set; }

                /// <summary>
                /// The command type of the database interaction
                /// Defaults to <see cref="System.Data.CommandType.Text"/>
                /// </summary>
                public System.Data.CommandType CommandType { get; protected set; } = System.Data.CommandType.Text;


                internal bool Validate()
                {
                    return !string.IsNullOrEmpty(QueryText);
                }
            }
        }
    }
}
