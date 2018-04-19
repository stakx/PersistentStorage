#region Copyright
// Copyright (c) 2017 stakx
// License available at https://github.com/stakx/PersistentStorage/blob/develop/LICENSE.md.
#endregion

using System;
using System.IO;

using Xunit;

namespace PersistentStorage.Tests
{
    public class FileSystemBasedPersistentStorageTests
    {
        [Fact]
        public void Ctor_given_null_reference_as_path_throws_ArgumentNullException()
        {
            Action ctor = () =>
            {
                var _ = new FileSystemBasedPersistentStorage(path: null);
            };

            Assert.Throws<ArgumentNullException>(ctor);
        }

        [Fact]
        public void Ctor_given_a_nonexisting_directory_as_path_throws_DirectoryNotFoundException()
        {
            var nonExistingDirectoryName = Guid.NewGuid().ToString("N");

            Action ctor = () =>
            {
                var _ = new FileSystemBasedPersistentStorage(path: nonExistingDirectoryName);
            };

            Assert.Throws<DirectoryNotFoundException>(ctor);
        }

        [Fact]
        public void Ctor_given_existing_directory_as_path_succeeds()
        {
            var existingDirectoryName = Path.GetTempPath();

            var _ = new FileSystemBasedPersistentStorage(path: existingDirectoryName);
        }
    }
}
