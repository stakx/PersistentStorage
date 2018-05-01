// Copyright (c) 2017 stakx
// License available at https://github.com/stakx/PersistentStorage/blob/develop/LICENSE.md.

using System.IO;
using System.Threading.Tasks;

namespace PersistentStorage
{
    /// <summary>
    ///   An item in a <see cref="IPersistentStorage"/>.
    /// </summary>
    public interface IPersistentStorageItem
    {
        /// <summary>
        ///   Gets the item's ID.
        /// </summary>
        /// <value>
        ///   The item's ID.
        /// </value>
        PersistentStorageItemId Id { get; }

        /// <summary>
        ///   Returns a <see cref="Stream"/> from which the item's content can be read. 
        ///   That stream should be assumed to be read-only.
        /// </summary>
        /// <returns>
        ///   A <see cref="Task{TResult}"/> whose result is a <see cref="Stream"/> from which the item's content can be read.
        /// </returns>
        Task<Stream> GetContentAsync();
    }
}
