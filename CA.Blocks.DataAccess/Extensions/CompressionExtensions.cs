using System.IO;
using System.IO.Compression;
using System.Text;

namespace CA.Blocks.DataAccess.Extensions
{
    /// <summary>
    /// A Extension class for to assist with compression of data for storage 
    /// </summary>
    public static class CompressionExtensions
    {

        /// <summary>
        /// Will compress a byte array using the GZip algorithm.
        /// The GZip algorithm is used as is is compatible with SQL server. THe opens up the possibility of Compressing and Decompressing at both
        /// SQL server as as at Application level
        /// </summary>
        /// <param name="input"> The byte array as the source data</param>
        /// <returns></returns>
        public static byte[] Compress(this byte[] input)
        {
            byte[] result;
            using (var sourceStream = new MemoryStream(input))
            {
                using (var targetStream = new MemoryStream())
                {
                    using (var zipStream = new GZipStream(targetStream, CompressionMode.Compress))
                    {
                        sourceStream.Position = 0;
                        sourceStream.CopyTo(zipStream);
                    }
                    result = targetStream.ToArray();
                    targetStream.Close();
                    sourceStream.Close();
                }
            }
            return result;
        }

        /// <summary>
        /// Will Decompress a byte array using the GZip algorithm.
        /// The GZip algorithm is used as is is compatible with SQL server. THe opens up the possibility of Compressing and Decompressing at both
        /// SQL server as as at Application level
        /// </summary>
        /// <param name="input">the decompressed Byte Array</param>
        /// <returns></returns>
        public static byte[] Decompress(this byte[] input)
        {
            byte[] result;
            using (var sourceStream = new MemoryStream(input))
            {
                using (var targetStream = new MemoryStream())
                {
                    using (var zipStream = new GZipStream(sourceStream, CompressionMode.Decompress))
                    {
                        zipStream.CopyTo(targetStream);
                        result = targetStream.ToArray();
                    }
                    targetStream.Close();
                    sourceStream.Close();
                }
            }
            return result;
        }

        /// <summary>
        /// Will encode and compress a source string.
        /// Note: Compression is intended to work on longer strings.
        /// when the source is less that 100 characters in length  chances are the compressed result will be bigger than the source
        /// </summary>
        /// <param name="input"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static byte[]? CompressString(this string input, Encoding encoding)
        {
            return string.IsNullOrWhiteSpace(input) ? null : Compress(encoding.GetBytes(input));
        }

        /// <summary>
        /// Will decompress and decode an byte array to a string. The input stream must be in a known format to start with
        /// </summary>
        /// <param name="input"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string? DecompressToString(this byte[] input, Encoding encoding)
        {
            return input.Length  == 0 ? null : encoding.GetString(Decompress(input));
        }

        /// <inheritdoc cref="CompressString(string, Encoding)"/>
        /// <remarks>The default string encoding is Encoding.ASCII, ASCII to be safe as we don't know the server encoding,
        /// if you need a different encoding you can specify the Encoding
        /// be aware you need to use the same collation on the server to read the data out  in a query on the server 
        /// </remarks>
        public static byte[]? CompressToSQLVarcharString(this string input)
        {
            return input.CompressString(Encoding.ASCII);
        }

        /// <inheritdoc cref="DecompressToString(byte[], Encoding)"/>
        /// <remarks>The default string encoding is Encoding.ASCII, ASCII to be safe as we don't know the server encoding,
        /// if you need a different encoding you can specify the Encoding
        /// be aware you need to use the same collation on the server to read the data out  in a query on the server 
        /// </remarks>
        public static string? DecompressToSQLVarcharString(this byte[] input)
        {
            return input.DecompressToString(Encoding.ASCII);
        }

        /// <inheritdoc cref="CompressString(string, Encoding)"/>
        /// <remarks>The default string encoding is Encoding.Unicode</remarks>
        public static byte[]? CompressToSqlNVarcharString(this string input)
        {
            return input.CompressString(Encoding.Unicode);
        }
        
        /// <inheritdoc cref="DecompressToString(byte[], Encoding)"/>
        /// <remarks>The default string encoding is Encoding.Unicode</remarks>
        public static string? DecompressToSqlNVarcharString(this byte[] input)
        {
            return input.DecompressToString(Encoding.Unicode);
        }
    }
}
