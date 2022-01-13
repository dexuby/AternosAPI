using System;

namespace AternosAPI
{
    public class Response<T> : ValueHolder<T>
    {
        private readonly ResponseStatus _status;
        private readonly Exception _exception;

        public Response(ResponseStatus status, T value = default, Exception exception = null) : base(value)
        {
            _status = status;
            _exception = exception;
        }

        public ResponseStatus GetStatus() => _status;

        public bool Succeeded() => _status == ResponseStatus.Success;

        public bool Failed() => _status == ResponseStatus.Failed;

        public Exception GetException() => _exception;

        public bool HasException() => _exception != null;

        public static Response<T> Success(T value) => new(ResponseStatus.Success, value);

        public static Response<T> Failure(Exception ex) => new(ResponseStatus.Failed, exception: ex);
    }
}