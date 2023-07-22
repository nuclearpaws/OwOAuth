using System;

namespace ResponseWrapper
{
    public readonly struct ResponseWrapper<TResponse>
    {
        private readonly TResponse _response;
        private readonly AbstractResponseError _error;

        public bool IsError => !(_error is null);

        private ResponseWrapper(
            TResponse response,
            AbstractResponseError error)
        {
            _response = response;
            _error = error;
        }

        public TResult Unwrap<TResult>(
            Func<TResponse, TResult> responseHandler,
            Func<AbstractResponseError, TResult> errorHandler)
            => IsError
                ? errorHandler(_error)
                : responseHandler(_response);

        public static implicit operator ResponseWrapper<TResponse>(TResponse response)
            => new ResponseWrapper<TResponse>(response, null);

        public static implicit operator ResponseWrapper<TResponse>(AbstractResponseError error)
            => new ResponseWrapper<TResponse>(default, error);
    }
}
