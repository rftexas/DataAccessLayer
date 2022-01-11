namespace DataAccessLayer
{
    public abstract partial class DataAccess
    {
        public interface IQuery<T>
        {
            /// <summary>
            /// Use this subclass to create a query that will transform from <see cref="T"/> to <see cref="TOut"/>
            /// </summary>
            /// <typeparam name="TOut"></typeparam>
            public interface IWithTransform<TOut>
            {
                /// <summary>
                /// The text of the query that will be executed against the database.
                /// </summary>
                string QueryText { get; }
                /// <summary>
                /// Transforms from queried type to desired type
                /// </summary>
                /// <param name="original"></param>
                /// <returns></returns>
                TOut Transform(T original);
                /// <summary>
                /// Optional parameters for the query.
                /// </summary>
                object Parameters { get; }
                /// <summary>
                /// The command type of the database interaction
                /// Defaults to <see cref="System.Data.CommandType.Text"/>
                /// </summary>
                System.Data.CommandType CommandType { get; }

                internal bool Validate()
                {
                    return !string.IsNullOrEmpty(QueryText);
                }
            }

            /// <summary>
            /// Use this subclass to create a query that has no transform.
            /// </summary>
            public interface IWithoutTransform
            {
                /// <summary>
                /// Optional parameters for the query.
                /// </summary>
                string QueryText { get; }

                /// <summary>
                /// The text of the query that will be executed against the database.
                /// </summary>
                object Parameters { get; }

                /// <summary>
                /// The command type of the database interaction
                /// Defaults to <see cref="System.Data.CommandType.Text"/>
                /// </summary>
                System.Data.CommandType CommandType { get; }


                internal bool Validate()
                {
                    return !string.IsNullOrEmpty(QueryText);
                }
            }
        }
    }
}
