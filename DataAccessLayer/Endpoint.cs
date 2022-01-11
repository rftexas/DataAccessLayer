using System.Net.Http;

namespace DataAccessLayer {
    public class Endpoint {
        public interface ICommand: IBaseRequest {
            
        }

        public interface ICommand<TRequest>: ICommand {
            TRequest Body { get; }
        }

        public interface IRequest<TRequest> {
            public interface IWithResponse<TResponse> {

                public interface IWithoutTransform : IBaseRequest {
                    TRequest Body { get; }
                }

                public interface IWithTransform<TTransform> : IBaseRequest {
                    TRequest Body { get; }
                    TTransform Transform(TResponse response);
                }
            }
        }
    }

    public interface IBaseRequest {
        /// <summary>The Method to use for the Http call</summary>
        HttpMethod Action { get; }

        string RelativeUrl { get; }

        object Params { get; }
    }
}