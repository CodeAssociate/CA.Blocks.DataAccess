using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CA.Blocks.DataAccess.Generators
{
    /// <summary>
    /// An immutable, equatable array wrapper used to support Roslyn incremental generator caching.
    /// </summary>
    public readonly struct EquatableArray<T> : IEquatable<EquatableArray<T>>, IEnumerable<T>
        where T : IEquatable<T>
    {
        public static readonly EquatableArray<T> Empty = new(Array.Empty<T>());

        private readonly T[]? _array;

        public EquatableArray(T[]? array)
        {
            _array = array;
        }

        public EquatableArray(IEnumerable<T> items)
        {
            _array = items?.ToArray() ?? Array.Empty<T>();
        }

        public int Length => _array?.Length ?? 0;

        public T this[int index] => (_array ?? Array.Empty<T>())[index];

        public bool Equals(EquatableArray<T> other)
        {
            if (ReferenceEquals(_array, other._array)) return true;
            if (_array == null || other._array == null) return (_array?.Length ?? 0) == (other._array?.Length ?? 0);
            if (_array.Length != other._array.Length) return false;

            for (int i = 0; i < _array.Length; i++)
            {
                if (!_array[i].Equals(other._array[i])) return false;
            }

            return true;
        }

        public override bool Equals(object? obj) => obj is EquatableArray<T> other && Equals(other);

        public override int GetHashCode()
        {
            if (_array == null || _array.Length == 0) return 0;
            int hash = 17;
            foreach (var item in _array)
            {
                hash = hash * 31 + (item?.GetHashCode() ?? 0);
            }
            return hash;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)(_array ?? Array.Empty<T>())).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public static bool operator ==(EquatableArray<T> left, EquatableArray<T> right) => left.Equals(right);
        public static bool operator !=(EquatableArray<T> left, EquatableArray<T> right) => !left.Equals(right);
    }
}
