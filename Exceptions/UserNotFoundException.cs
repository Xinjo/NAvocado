using System;
using System.Net;
using System.Runtime.Serialization;

namespace NAvocado.Exceptions
{
    public class UserNotFoundException : NAvocadoException
    {
        public UserNotFoundException()
        {
        }

        public UserNotFoundException(string message) : base(message)
        {
        }

        public UserNotFoundException(string message, WebExceptionStatus status) : base(message, status)
        {
        }

        public UserNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public UserNotFoundException(string message, Exception innerException, WebExceptionStatus status,
            WebResponse response) : base(message, innerException, status, response)
        {
        }

        protected UserNotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}