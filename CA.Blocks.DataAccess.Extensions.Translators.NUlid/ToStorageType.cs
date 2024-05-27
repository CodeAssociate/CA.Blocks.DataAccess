using NUlid;

namespace CA.Blocks.DataAccess.Extensions.Translators.NUlid
{

    // If you using this assembly the underlying storage is not likely going to have support for string the ulid
    // so we have to covert it, to keep the properties we need to store as string 26 or binary 16 
    public static class ToStorageType
    {
        public static byte[] AsByteArray(this Ulid target)
        {
            return target.ToByteArray();
        }

        public static byte[]? AsByteArray(this Ulid? target)
        {
            return target?.ToByteArray();
        } 

        public static string AsString(this Ulid target)
        {
            return target.ToString();
        }

        public static string? AsString(this Ulid? target)
        {
            return target?.ToString();
        }

        public static string AsBinaryString(this Ulid target)
        {
            return $"0x{string.Join("", target.ToByteArray().Select(b => b.ToString("x2")))}";
        }

        public static string? AsBinaryString(this Ulid? target)
        {
            return !target.HasValue ? null : $"0x{string.Join("", target.Value.ToByteArray().Select(b => b.ToString("x2")))}";
        }
    }
}