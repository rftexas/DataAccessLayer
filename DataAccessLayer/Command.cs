namespace DataAccessLayer
{
    public abstract partial class DataAccess
    {
        /// <summary>
        /// Use this type if you do not want a return type.
        /// </summary>
        public abstract class Command
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

            /// <summary>
            /// Validate the Command.
            /// </summary>
            /// <returns></returns>
            internal bool Validate()
            {
                return !string.IsNullOrEmpty(QueryText);
            }
        }
    }
}
