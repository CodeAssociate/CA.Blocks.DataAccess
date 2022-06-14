using System.IO;
using System.IO.Compression;
using System.Text;

namespace CA.Blocks.DataAccess.Extensions
{
    /// <summary>
    /// A Extension class for to assist with compression of data for storage 
    /// </summary>
    public static  class CompressionExtensions
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
        public static byte[] CompressString(this string input, Encoding encoding)
        {
            if (string.IsNullOrWhiteSpace(input))
                return null;
            return Compress(encoding.GetBytes(input));
        }

        /// <summary>
        /// Will decompress and decode an byte array to a string. The input stream must be in a known format to start with
        /// </summary>
        /// <param name="input"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string DecompressToString(this byte[] input, Encoding encoding)
        {
            if (input.Length  == 0)
                return null;
            return encoding.GetString(Decompress(input));
        }

        //
        public static byte[] CompressToSQLVarcharString(this string input)
        {
            return input.CompressString(Encoding.ASCII);
        }
        public static string DecompressToSQLVarcharString(this byte[] input)
        {
            return input.DecompressToString(Encoding.ASCII);
        }

        public static byte[] CompressToSQLNVarcharString(this string input)
        {
            return input.CompressString(Encoding.Unicode);
        }
        public static string DecompressToSQLNVarcharString(this byte[] input)
        {
            return input.DecompressToString(Encoding.Unicode);
        }
    }


}
