#region Copyright
// Copyright (c) 2017 stakx
// License available at https://github.com/stakx/PersistentStorage/blob/develop/LICENSE.md.
#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace PersistentStorage
{
    partial class FileSystemBasedPersistentStorage
    {
        private sealed class Item : IPersistentStorageItem
        {
            private FileSystemBasedPersistentStorage storage;
            private PersistentStorageItemId id;

            public Item(FileSystemBasedPersistentStorage storage, PersistentStorageItemId id)
            {
                Debug.Assert(storage != null);

                this.storage = storage;
                this.id = id;
            }

            public PersistentStorageItemId Id => this.id;

            public Task<Stream> GetContentAsync()
            {
                var contentFilePath = this.storage.GetItemContentFilePath(this.id);
                var content = File.Open(contentFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                return Task.FromResult<Stream>(content);
            }
        }
    }
}
