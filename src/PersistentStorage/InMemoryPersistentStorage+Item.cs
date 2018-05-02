// Copyright (c) 2017 stakx
// License available at https://github.com/stakx/PersistentStorage/blob/develop/LICENSE.md.

using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace PersistentStorage
{
    partial class InMemoryPersistentStorage
    {
        private sealed class Item : IPersistentStorageItem
        {
            private byte[] content;
            private PersistentStorageItemId id;

            public Item(PersistentStorageItemId id, byte[] content)
            {
                Debug.Assert(content != null);

                this.content = content;
                this.id = id;
            }

            public PersistentStorageItemId Id => this.id;

            public Task<Stream> GetContentAsync()
            {
                Stream contentStream = new MemoryStream(this.content, index: 0, count: this.content.Length, writable: false, publiclyVisible: false);
                return Task.FromResult(contentStream);
            }
        }
    }
}
