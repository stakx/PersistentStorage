// Copyright (c) 2017 stakx
// License available at https://github.com/stakx/PersistentStorage/blob/develop/LICENSE.md.

using System;
using System.Runtime.Serialization;

namespace PersistentStorage
{
    /// <summary>
    ///   The type of <see cref="PersistentStorageException"/> that a <see cref="FileSystemBasedPersistentStorage"/> instance may throw.
    /// </summary>
    [Serializable]
    public sealed class FileSystemBasedPersistentStorageException : PersistentStorageException
    {
        internal FileSystemBasedPersistentStorageException(string message)
            : base(message)
        {
        }

        internal FileSystemBasedPersistentStorageException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        private FileSystemBasedPersistentStorageException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
