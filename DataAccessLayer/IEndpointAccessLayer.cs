using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface IEndpointAccessLayer<TEndpoint> {

        Task Execute(Endpoint.ICommand command);
        Task<TResult> Send<TRequest, TResult>(Endpoint.IRequest<TRequest>.IWithResponse<TResult>.IWithoutTransform request);
        Task<TTransformed> Send<TRequest,TResult,TTransformed>(Endpoint.IRequest<TRequest>.IWithResponse<TResult>.IWithTransform<TTransformed> request);
    }
}