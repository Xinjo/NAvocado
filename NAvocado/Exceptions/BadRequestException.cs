﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NAvocado.Exceptions
{
    class BadRequestException : NAvocadoException
    {
        public BadRequestException()
        {
        }

        public BadRequestException(string message) : base(message)
        {
        }

        public BadRequestException(string message, WebExceptionStatus status) : base(message, status)
        {
        }

        public BadRequestException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public BadRequestException(string message, Exception innerException, WebExceptionStatus status, WebResponse response) : base(message, innerException, status, response)
        {
        }

        protected BadRequestException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}