using System.Text;
using CA.Blocks.DataAccess.Extensions;

namespace CA.Blocks.DataAccessUnitTests.Extensions
{
        public  class CompressionExtensionsUnitTests
    {

        [Fact]
        public void CompressDecompressAsUnicodeString()
        {
            // Compression will only work on 200+ strings anything smaller is viable
            string testData = $"{Guid.NewGuid()}-{Guid.NewGuid()}-{Guid.NewGuid()}";
            testData += testData;

            var compressedData = testData.CompressString(Encoding.Unicode);
            Assert.NotNull(compressedData);

            var decompressResult = compressedData.DecompressToString(Encoding.Unicode);
            Assert.True(compressedData.Length < testData.Length);
            Assert.Equal(testData, decompressResult);
        }

        [Fact]
        public void CompressDecompressAsNVarchar() // this is an alias for Encoding.Unicode)
        {
            string testData = $"{Guid.NewGuid()}-{Guid.NewGuid()}-{Guid.NewGuid()}";
            testData += testData;

            var compressedData = testData.CompressToSqlNVarcharString();

            Assert.NotNull(compressedData);
            
            var decompressResult = compressedData.DecompressToSqlNVarcharString();
            Assert.True(compressedData.Length < testData.Length);
            Assert.Equal(testData, decompressResult);
        }


        [Fact]
        public void CompresslDecompressAsASASCIIString()
        {
            // Compression will only work on 200+ strings anything smaller is viable
            string testData = $"{Guid.NewGuid()}-{Guid.NewGuid()}-{Guid.NewGuid()}";
            testData += testData;

            var compressedData = testData.CompressString(Encoding.ASCII);
            Assert.NotNull(compressedData);
            var decompressResult = compressedData.DecompressToString(Encoding.ASCII);
            Assert.True(compressedData.Length < testData.Length);
            Assert.Equal(testData, decompressResult);
        }

        [Fact]
        public void CompressDecompressAsVarchar() // this is an alias for Encoding.ASCII it is the safe option as we do not know the server ASCII to be safe as we don't know the server encoding
        {
            string testData = $"{Guid.NewGuid()}-{Guid.NewGuid()}-{Guid.NewGuid()}";
            testData += testData;

            var compressedData = testData.CompressToSQLVarcharString();
            Assert.NotNull(compressedData);
            var decompressResult = compressedData.DecompressToSQLVarcharString();
            Assert.True(compressedData.Length < testData.Length);
            Assert.Equal(testData, decompressResult);
        }
    }
}
