// Copyright (c) 2017 stakx
// License available at https://github.com/stakx/PersistentStorage/blob/develop/LICENSE.md.

using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace PersistentStorage
{
    /// <summary>
    ///   A type of <see cref="IPersistentStorage"/> that persists its items using the file system.
    ///   All items will be stored as files somewhere inside a specified root directory.
    /// </summary>
    public sealed partial class FileSystemBasedPersistentStorage : IPersistentStorage
    {
        private string path;

        /// <summary>
        ///   Initializes a new instance of the <see cref="FileSystemBasedPersistentStorage"/> class using the specified root directory.
        /// </summary>
        /// <param name="path">
        ///   Absolute or relative path of the root directory inside of which items will be stored.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="path"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">
        ///   The directory specified by <paramref name="path"/> does not exist.
        /// </exception>
        public FileSystemBasedPersistentStorage(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (!Path.IsPathRooted(path))
            {
                path = Path.GetPathRoot(path);
            }

            if (!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException($"The specified directory `{path}` does not exist.");
            }

            this.path = path;
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="writeContentAsync"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="FileSystemBasedPersistentStorageException">
        ///   The item could not be created, or the item's ID could not be computed from the specified content.
        /// </exception>
        public async Task<IPersistentStorageItem> CreateOrGetItemAsync(Func<Stream, Task> writeContentAsync)
        {
            if (writeContentAsync == null)
            {
                throw new ArgumentNullException(nameof(writeContentAsync));
            }

            var temporaryFilePath = Path.GetTempFileName();
            try
            {
                PersistentStorageItemId id;

                using (var sha = SHA1.Create())
                using (var temporaryFile = File.Open(temporaryFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                using (var cryptoStream = new CryptoStream(temporaryFile, sha, CryptoStreamMode.Write))
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
                        throw new FileSystemBasedPersistentStorageException("Could not write item content.", ex);
                    }

                    try
                    {
                        id = new PersistentStorageItemId(sha.Hash);
                    }
                    catch (Exception ex)
                    {
                        throw new FileSystemBasedPersistentStorageException("Could not compute identity from item content.", ex);
                    }
                }

                var contentFilePath = this.GetItemContentFilePath(id);
                if (!File.Exists(contentFilePath))
                {
                    var contentDirectoryPath = Path.GetDirectoryName(contentFilePath);
                    if (!Directory.Exists(contentDirectoryPath))
                    {
                        try
                        {
                            Directory.CreateDirectory(contentDirectoryPath);
                        }
                        catch (Exception ex)
                        {
                            throw new FileSystemBasedPersistentStorageException($"Could not create subdirectory for new item `{id}`.", ex);
                        }
                    }

                    try
                    {
                        File.Move(temporaryFilePath, contentFilePath);
                    }
                    catch (Exception ex)
                    {
                        throw new FileSystemBasedPersistentStorageException($"Could not create file for new item `{id}`.", ex);
                    }
                }

                return new Item(this, id);
            }
            finally
            {
                File.Delete(temporaryFilePath);
            }
        }

        /// <inheritdoc/>
        /// <exception cref="FileSystemBasedPersistentStorageException">
        ///   The item with the specified ID does not exist in this storage.
        /// </exception>
        public Task<IPersistentStorageItem> GetItemAsync(PersistentStorageItemId id)
        {
            if (this.HasItem(id))
            {
                return Task.FromResult<IPersistentStorageItem>(new Item(this, id));
            }
            else
            {
                throw new FileSystemBasedPersistentStorageException($"Item `{id}` does not exist.");
            }
        }

        /// <inheritdoc/>
        public bool HasItem(PersistentStorageItemId id)
        {
            return File.Exists(this.GetItemContentFilePath(id));
        }

        private string GetItemContentFilePath(PersistentStorageItemId id)
        {
            var fileName = id.ToString();
            var subDirectoryName = fileName.Substring(0, 2);
            return Path.Combine(this.path, subDirectoryName, fileName);
        }
    }
}
