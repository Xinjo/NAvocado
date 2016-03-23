using System;
using System.Net;
using System.Runtime.Serialization;

namespace NAvocado.Exceptions
{
    public class AuthenticationFailedException : NAvocadoException
    {
        public AuthenticationFailedException()
        {
        }

        public AuthenticationFailedException(string message) : base(message)
        {
        }

        public AuthenticationFailedException(string message, WebExceptionStatus status) : base(message, status)
        {
        }

        public AuthenticationFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public AuthenticationFailedException(string message, Exception innerException, WebExceptionStatus status,
            WebResponse response) : base(message, innerException, status, response)
        {
        }

        protected AuthenticationFailedException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}