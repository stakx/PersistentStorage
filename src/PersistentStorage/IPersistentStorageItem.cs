// Copyright (c) 2017 stakx
// License available at https://github.com/stakx/PersistentStorage/blob/develop/LICENSE.md.

using System.IO;
using System.Threading.Tasks;

namespace PersistentStorage
{
    public interface IPersistentStorageItem
    {
        PersistentStorageItemId Id { get; }
        Task<Stream> GetContentAsync();
    }
}
