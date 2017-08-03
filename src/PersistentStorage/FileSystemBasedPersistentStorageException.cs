#region Copyright
// Copyright (c) 2017 stakx
// License available at https://github.com/stakx/PersistentStorage/blob/develop/LICENSE.md.
#endregion

using System;

namespace PersistentStorage
{
    public sealed class FileSystemBasedPersistentStorageException : Exception
    {
        internal FileSystemBasedPersistentStorageException(string message)
            : base(message)
        {
        }

        internal FileSystemBasedPersistentStorageException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
