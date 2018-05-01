// Copyright (c) 2017 stakx
// License available at https://github.com/stakx/PersistentStorage/blob/develop/LICENSE.md.

using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace PersistentStorage
{
    public sealed partial class FileSystemBasedPersistentStorage : IPersistentStorage
    {
        private string path;

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
