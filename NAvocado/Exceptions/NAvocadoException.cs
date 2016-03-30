using System;
using System.Net;
using System.Runtime.Serialization;

namespace NAvocado.Exceptions
{
    public class NAvocadoException : WebException
    {
        public NAvocadoException()
        {
        }

        public NAvocadoException(string message) : base(message)
        {
        }

        public NAvocadoException(string message, WebExceptionStatus status) : base(message, status)
        {
        }

        public NAvocadoException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public NAvocadoException(string message, Exception innerException, WebExceptionStatus status,
            WebResponse response) : base(message, innerException, status, response)
        {
        }

        protected NAvocadoException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}