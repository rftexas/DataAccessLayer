using System.Net;
using System.Threading.Tasks;
using System;

namespace DataAccessLayer
{
    public class EndpointAccessLayer<TEndpoint> : IEndpointAccessLayer<TEndpoint>
    {
        private readonly EndpointConfiguration _configuration;
        public EndpointAccessLayer(EndpointConfiguration<TEndpoint> configuration) {

        }

        public async Task Execute<TRequest>(Endpoint.ICommand<TRequest> request) {
throw new NotImplementedException();
        }

        public async Task Execute(Endpoint.ICommand request) {
throw new NotImplementedException();
        }

        public async Task<TResult> Send<TRequest, TResult>(Endpoint.IRequest<TRequest>.IWithResponse<TResult>.IWithoutTransform request){
            throw new NotImplementedException();
        }

        public async Task<TTransformed> Send<TRequest, TResult, TTransformed>(Endpoint.IRequest<TRequest>.IWithResponse<TResult>.IWithTransform<TTransformed> request){
            throw new NotImplementedException();
        }
    }
}