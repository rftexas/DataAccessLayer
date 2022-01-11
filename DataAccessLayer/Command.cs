namespace DataAccessLayer
{
    public abstract partial class DataAccess
    {
        /// <summary>
        /// Use this type if you do not want a return type.
        /// </summary>
        public interface ICommand
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
