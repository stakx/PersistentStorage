// Copyright (c) 2017 stakx
// License available at https://github.com/stakx/PersistentStorage/blob/develop/LICENSE.md.

using System;
using System.IO;
using System.Threading.Tasks;

namespace PersistentStorage
{
    /// <summary>
    ///   A persistent content-addressable storage (CAS).
    /// </summary>
    public interface IPersistentStorage
    {
        /// <summary>
        ///   Creates a new item with the specified content, or fetches the item if it already exists in this storage.
        /// </summary>
        /// <param name="writeContentAsync">
        ///   An asynchronous callback which is given a <see cref="Stream"/> to which it should write the desired item content.
        ///   That stream should be assumed to be write-only and non-seekable.
        /// </param>
        /// <returns>
        ///   The item with the specified content.
        /// </returns>
        /// <exception cref="PersistentStorageException">
        ///   The item could not be created, or the item's ID could not be computed from the specified content.
        /// </exception>
        Task<IPersistentStorageItem> CreateOrGetItemAsync(Func<Stream, Task> writeContentAsync);

        /// <summary>
        ///   Returns the item with the specified ID.
        /// </summary>
        /// <param name="id">
        ///   The ID of the item that should be returned.
        /// </param>
        /// <returns>
        ///   If the requested item exists, a <see cref="Task{TResult}"/> whose result is the requested item.
        ///   (If the requested item does not exist in this storage, a <see cref="PersistentStorageException"/> will be thrown.)
        /// </returns>
        /// <exception cref="PersistentStorageException">
        ///   The item with the specified ID does not exist in this storage.
        /// </exception>
        Task<IPersistentStorageItem> GetItemAsync(PersistentStorageItemId id);

        /// <summary>
        ///   Gets a value indicating whether the item with the specified ID exists in this storage.
        /// </summary>
        /// <param name="id">
        ///   The ID of the item whose existence should be checked.
        /// </param>
        /// <returns>
        ///   <see langword="true"/> if the item with the specified ID exists in this storage; otherwise, <see langword="false"/>.
        /// </returns>
        bool HasItem(PersistentStorageItemId id);
    }
}
