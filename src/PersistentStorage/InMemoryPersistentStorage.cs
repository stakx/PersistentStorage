// Copyright (c) 2017 stakx
// License available at https://github.com/stakx/PersistentStorage/blob/develop/LICENSE.md.

using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace PersistentStorage
{
    /// <summary>
    ///   A type of <see cref="IPersistentStorage"/> that stores its items in random-access memory (RAM).
    ///   It is intended to be used mainly in testing scenarios, not for actual data storage.
    /// </summary>
    public sealed partial class InMemoryPersistentStorage : IPersistentStorage
    {
        private Dictionary<PersistentStorageItemId, Item> items;

        /// <summary>
        ///   Initializes a new instance of the <see cref="InMemoryPersistentStorage"/> class.
        /// </summary>
        public InMemoryPersistentStorage()
        {
            this.items = new Dictionary<PersistentStorageItemId, Item>();
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="writeContentAsync"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InMemoryPersistentStorageException">
        ///   The item could not be created, or the item's ID could not be computed from the specified content.
        /// </exception>
        public async Task<IPersistentStorageItem> CreateOrGetItemAsync(Func<Stream, Task> writeContentAsync)
        {
            if (writeContentAsync == null)
            {
                throw new ArgumentNullException(nameof(writeContentAsync));
            }

            PersistentStorageItemId id;

            using (var sha = SHA1.Create())
            using (var buffer = new MemoryStream())
            using (var cryptoStream = new CryptoStream(buffer, sha, CryptoStreamMode.Write))
            {
                try
                {
                    await writeContentAsync(cryptoStream);

                    await cryptoStream.FlushAsync();
                    if (!cryptoStream.HasFlushedFinalBlock)
                    {
                        cryptoStream.FlushFinalBlock();
                    }
                }
                catch (Exception ex)
                {
                    throw new InMemoryPersistentStorageException("Could not write item content.", ex);
                }

                try
                {
                    id = new PersistentStorageItemId(sha.Hash);
                }
                catch (Exception ex)
                {
                    throw new InMemoryPersistentStorageException("Could not compute identity from item content.", ex);
                }

                if (this.items.TryGetValue(id, out var item) == false)
                {
                    if (buffer.TryGetBuffer(out var contentBytes) == false)
                    {
                        throw new InMemoryPersistentStorageException("Could not compute identity from item content.");
                    }

                    var content = new byte[contentBytes.Count];
                    Array.Copy(contentBytes.Array, contentBytes.Offset, content, 0, contentBytes.Count);
                    item = new Item(id, content);
                    this.items[id] = item;
                }

                return item;
            }
        }

        /// <inheritdoc/>
        /// <exception cref="InMemoryPersistentStorageException">
        ///   The item with the specified ID does not exist in this storage.
        /// </exception>
        public Task<IPersistentStorageItem> GetItemAsync(PersistentStorageItemId id)
        {
            if (this.items.TryGetValue(id, out var item))
            {
                return Task.FromResult<IPersistentStorageItem>(item);
            }
            else
            {
                throw new InMemoryPersistentStorageException($"Item `{id}` does not exist.");
            }
        }

        /// <inheritdoc/>
        public bool HasItem(PersistentStorageItemId id)
        {
            return this.items.ContainsKey(id);
        }
    }
}
