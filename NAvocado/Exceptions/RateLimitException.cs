using System;
using System.Net;
using System.Runtime.Serialization;

namespace NAvocado.Exceptions
{
    public class RateLimitException : NAvocadoException
    {
        public RateLimitException()
        {
        }

        public RateLimitException(string message) : base(message)
        {
        }

        public RateLimitException(string message, WebExceptionStatus status) : base(message, status)
        {
        }

        public RateLimitException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public RateLimitException(string message, Exception innerException, WebExceptionStatus status,
            WebResponse response) : base(message, innerException, status, response)
        {
        }

        protected RateLimitException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}