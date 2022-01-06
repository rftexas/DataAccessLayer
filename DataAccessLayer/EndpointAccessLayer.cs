namespace DataAccessLayer
{
    public interface IEndpointAccessLayer<TEndpoint> {

        Task Execute(Endpoint.Command command);
        Task<TResult> Send<TResult>(Endpoint.Request<TResult>.WithoutTransform request);
        Task<TTransformed> Send<TResult,TTransformed>(Endpoint.Request<TResult>.WithTransform<TTransformed> request);
    }
}