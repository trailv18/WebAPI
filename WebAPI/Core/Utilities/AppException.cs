using System;
using System.Runtime.Serialization;

namespace WebAPI.Core.Utilities
{
    [Serializable]
    public class AppException : Exception
    {
        public int Area { get; set; }

        public int StatusCode { get; set; }

        public AppException(string message, int statusCode)
             : base(message)
        {
            StatusCode = statusCode;
        }

        protected AppException(SerializationInfo info, StreamingContext context)
             : base(info, context)
        {
        }
    }
}
