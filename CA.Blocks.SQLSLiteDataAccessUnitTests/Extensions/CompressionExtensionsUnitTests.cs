using System;
using System.Collections.Generic;
using System.Text;
using CA.Blocks.DataAccess.Extensions;
using NUnit.Framework;

namespace CA.Blocks.SQLLiteDataAccessUnitTests.Extensions
{
    [TestFixture]
    public  class CompressionExtensionsUnitTests
    {

        [Test]
        public void CompresslDecompressAsUnicodeString()
        {
            // Compression will only work on 200+ strings anything smaller is viable
            string testData = $"{Guid.NewGuid()}-{Guid.NewGuid()}-{Guid.NewGuid()}";
            testData += testData;

            var compressedData = testData.CompressString(Encoding.Unicode);

            var decompressResult = compressedData.DecompressToString(Encoding.Unicode);
            Assert.True(compressedData.Length < testData.Length);
            Assert.AreEqual(testData, decompressResult);
        }

        [Test]
        public void CompresslDecompressAsASASCIIString()
        {
            // Compression will only work on 200+ strings anything smaller is viable
            string testData = $"{Guid.NewGuid()}-{Guid.NewGuid()}-{Guid.NewGuid()}";
            testData += testData;

            var compressedData = testData.CompressString(Encoding.ASCII);

            var decompressResult = compressedData.DecompressToString(Encoding.ASCII);
            Assert.True(compressedData.Length < testData.Length);
            Assert.AreEqual(testData, decompressResult);
        }

    }
}
