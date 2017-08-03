# PersistentStorage #

A simple, file system-based content addressable storage (CAS).

## Basic usage ##

```csharp
using PersistentStorage;

// Data will be read from and written to a directory structure contained in
// the directory passed to the constructor:
IPersistentStorage storage = new FileSystemBasedPersistentStorage(@"C:\path\to\some\directory");

// This will create a storage item with a single line of text as its content.
// If the storage already contains an item with the same content, the existing
// item is returned:
IPersistentStorageItem item = await storage.CreateOrGetItemAsync(async (Stream content) =>
{
    using (var contentWriter = new StreamWriter(content))
    {
        contentWriter.WriteLine("Hello world.");
    }
});

// Print the above item's ID (which is the SHA1 hash of its content, formatted
// as a 40-digit hexadecimal number):
Console.WriteLine(item.Id);

// Open some item by its ID, then read and print its content:
var anotherItemId = PersistentStorageItemId.Parse("f479d393efe0776a65b9523319422e0a3dfc9baf");
IPersistentStorageItem anotherItem = await storage.GetItemAsync(anotherItemId);
using (var content = await anotherItem.GetContentAsync())
using (var contentReader = new StreamReader(content))
{
    Console.WriteLine(await contentReader.ReadToEndAsync());
}
```

## Similar prior art ##

 * [Git's object data store](https://git-scm.com/book/en/v2/Git-Internals-Git-Objects)
 * [drewnoakes/cassette](https://github.com/drewnoakes/cassette)
