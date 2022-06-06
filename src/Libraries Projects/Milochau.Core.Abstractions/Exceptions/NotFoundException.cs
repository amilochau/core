using System;
using System.Runtime.Serialization;

namespace Milochau.Core.Abstractions.Exceptions
{
    /// <summary>Exception for a resource not found</summary>
    [Serializable]
    public class NotFoundException : SystemException
    {
        /// <summary>Constructor</summary>
        public NotFoundException() : base() { }

        /// <summary>Constructor</summary>
        public NotFoundException(string message) : base(message) { }

        /// <summary>Constructor</summary>
        public NotFoundException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>Constructor</summary>
        protected NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
