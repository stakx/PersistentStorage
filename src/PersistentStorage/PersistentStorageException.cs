// Copyright (c) 2017 stakx
// License available at https://github.com/stakx/PersistentStorage/blob/develop/LICENSE.md.

using System;
using System.Runtime.Serialization;

namespace PersistentStorage
{
    /// <summary>
    ///   The base type of <see cref="Exception"/> that an <see cref="IPersistentStorage"/> implementation may throw.
    /// </summary>
    [Serializable]
    public abstract class PersistentStorageException : Exception
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref="PersistentStorageException"/>-derived class with a specified error message.
        /// </summary>
        /// <param name="message">
        ///   The message that describes the error.
        /// </param>
        protected PersistentStorageException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="PersistentStorageException"/>-derived class with a specified error message
        ///   and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">
        ///   The message that describes the error.
        /// </param>
        /// <param name="innerException">
        ///   The exception that is the cause of the current exception, or <see langword="null"/> if no inner exception is specified.
        /// </param>
        protected PersistentStorageException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="PersistentStorageException"/> with serialized data.
        /// </summary>
        /// <param name="info">
        ///   The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        ///   The <see cref="StreamingContext"/> that contains contextual information about the source or destination.
        /// </param>
        protected PersistentStorageException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
