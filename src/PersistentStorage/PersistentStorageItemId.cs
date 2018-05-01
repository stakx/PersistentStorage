// Copyright (c) 2017 stakx
// License available at https://github.com/stakx/PersistentStorage/blob/develop/LICENSE.md.

using System;
using System.Globalization;
using System.Text;

namespace PersistentStorage
{
    public struct PersistentStorageItemId : IEquatable<PersistentStorageItemId>
    {
        private byte[] hash;

        public const int Length = 20;

        public PersistentStorageItemId(byte[] hash)
        {
            if (hash == null)
            {
                throw new ArgumentNullException(nameof(hash));
            }

            if (hash.Length != Length)
            {
                throw new ArgumentException($"The specified array does not have the required length of {Length} bytes.", nameof(hash));
            }

            this.hash = hash;
        }

        public bool Equals(PersistentStorageItemId other)
        {
            for (int i = 0; i < Length; ++i)
            {
                if (this.hash[i] != other.hash[i])
                {
                    return false;
                }
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            return obj is PersistentStorageItemId other && this.Equals(other);
        }

        public override int GetHashCode()
        {
            int hashCode = 0;
            for (int i = 0, n = Math.Min(7, Length); i < n; ++i)
            {
                hashCode = unchecked(17 * hashCode + this.hash[i]);
            }

            return hashCode;
        }

        public override string ToString()
        {
            var formattedHashBuilder = new StringBuilder();
            for (int i = 0; i < Length; ++i)
            {
                formattedHashBuilder.Append(this.hash[i].ToString("x2"));
            }

            return formattedHashBuilder.ToString();
        }

        public static bool operator ==(PersistentStorageItemId left, PersistentStorageItemId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PersistentStorageItemId left, PersistentStorageItemId right)
        {
            return !left.Equals(right);
        }

        public static PersistentStorageItemId Parse(string formattedId)
        {
            if (formattedId == null)
            {
                throw new ArgumentNullException(nameof(formattedId));
            }

            if (formattedId.Length != Length * 2)
            {
                throw new ArgumentException($"The string `{formattedId}` does not have the required length of {Length * 2} characters.");
            }

            var hash = new byte[Length];
            for (int i = 0; i < Length; ++i)
            {
                hash[i] = byte.Parse(formattedId.Substring(i * 2, 2), NumberStyles.AllowHexSpecifier);
            }

            return new PersistentStorageItemId(hash);
        }
    }
}
