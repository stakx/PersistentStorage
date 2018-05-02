// Copyright (c) 2017 stakx
// License available at https://github.com/stakx/PersistentStorage/blob/develop/LICENSE.md.

using System;
using System.Runtime.Serialization;

namespace PersistentStorage
{
    /// <summary>
    ///   The type of <see cref="PersistentStorageException"/> that a <see cref="InMemoryPersistentStorage"/> instance may throw.
    /// </summary>
    [Serializable]
    public sealed class InMemoryPersistentStorageException : PersistentStorageException
    {
        internal InMemoryPersistentStorageException(string message)
            : base(message)
        {
        }

        internal InMemoryPersistentStorageException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        private InMemoryPersistentStorageException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
