// Copyright (c) 2017 stakx
// License available at https://github.com/stakx/PersistentStorage/blob/develop/LICENSE.md.

using System;
using System.Globalization;
using System.Text;

namespace PersistentStorage
{
    /// <summary>
    ///   A <see cref="IPersistentStorage"/> item ID, which is equivalent to a SHA-1 hash.
    /// </summary>
    public struct PersistentStorageItemId : IEquatable<PersistentStorageItemId>
    {
        private byte[] hash;

        public const int Length = 20;

        /// <summary>
        ///   Initializes a new instance of the <see cref="PersistentStorageItemId"/> structure.
        /// </summary>
        /// <param name="hash">
        ///   A byte array containing a SHA-1 hash.
        ///   This array must contain exactly 20 bytes.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="hash"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///   <paramref name="hash"/> contains a number of bytes other than 20.
        /// </exception>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is PersistentStorageItemId other && this.Equals(other);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = 0;
            for (int i = 0, n = Math.Min(7, Length); i < n; ++i)
            {
                hashCode = unchecked(17 * hashCode + this.hash[i]);
            }

            return hashCode;
        }

        /// <summary>
        ///   Formats the item ID as a string of 40 hexadecimal digits in lower case.
        /// </summary>
        /// <returns>
        ///   A string representing the item ID.
        /// </returns>
        public override string ToString()
        {
            var formattedHashBuilder = new StringBuilder();
            for (int i = 0; i < Length; ++i)
            {
                formattedHashBuilder.Append(this.hash[i].ToString("x2"));
            }

            return formattedHashBuilder.ToString();
        }

        /// <inheritdoc/>
        public static bool operator ==(PersistentStorageItemId left, PersistentStorageItemId right)
        {
            return left.Equals(right);
        }

        /// <inheritdoc/>
        public static bool operator !=(PersistentStorageItemId left, PersistentStorageItemId right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        ///   Converts a formatted item ID to the corresponding <see cref="PersistentStorageItemId"/>.
        /// </summary>
        /// <param name="formattedId">
        ///   The formatted item ID that should be converted back to a <see cref="PersistentStorageItemId"/>.
        /// </param>
        /// <returns>
        ///   The <see cref="PersistentStorageItemId"/> that corresponds to the specified formatted item ID.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///   <paramref name="formattedId"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        ///   <paramref name="formattedId"/> contains a number of characters other than 40.
        /// </exception>
        /// <exception cref="FormatException">
        ///   <paramref name="formattedId"/> does not have the correct format.
        ///   It is required to contain exactly 40 hexadecimal digits (either in lower case or upper case) and nothing else.
        /// </exception>
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
