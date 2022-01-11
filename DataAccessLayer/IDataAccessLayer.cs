using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface IDataAccessLayer<TConnection>
    {
        /// <summary>
        /// Executes a query against the database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> Query<T>(DataAccess.IQuery<T>.IWithoutTransform query);

        /// <summary>
        /// Executes a query against the database.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IEnumerable<TOut>> Query<T, TOut>(DataAccess.IQuery<T>.IWithTransform<TOut> query);
        
        /// <summary>
        /// Execute a query against the database
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task Execute(DataAccess.ICommand command);
    }
}
