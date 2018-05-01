// Copyright (c) 2017 stakx
// License available at https://github.com/stakx/PersistentStorage/blob/develop/LICENSE.md.

using System;
using System.Runtime.Serialization;

namespace PersistentStorage
{
    [Serializable]
    public abstract class PersistentStorageException : Exception
    {
        protected PersistentStorageException(string message)
            : base(message)
        {
        }

        protected PersistentStorageException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected PersistentStorageException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
