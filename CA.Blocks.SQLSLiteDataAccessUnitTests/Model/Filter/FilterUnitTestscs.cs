using System;
using CA.Blocks.SQLLiteDataAccess;
using CA.CoreBlocks.DataAccess.Model.Filter;
using NUnit.Framework;

namespace CA.Blocks.SQLLiteDataAccessUnitTests.Model.Filter
{
    public class TestFilter : BaseFilterSegment
    {
        public void HasIntColFilter(int value)
        {
            AddFilter("intCol = @value", value.ToSqlParameter("@value"));
        }

        public void HasShortColFilterBadColName(short value)
        {
            AddFilter("ShortCol = @value", value.ToSqlParameter("@value"));
        }
    }


    [TestFixture]
    public class FilterUnitTestscs
    {
        [Test]
        public void BasicTest()
        {
            var target = new TestFilter();
            target.HasIntColFilter(123);
            Assert.IsTrue(target.Parameters.Count == 1);
            Assert.AreEqual("intCol = @value", target.ToSQLFilter());
        }

        [Test]
        public void BasicTestWithWhere()
        {
            var target = new TestFilter();
            target.HasIntColFilter(123);
            Assert.IsTrue(target.Parameters.Count == 1);
            Assert.AreEqual("WHERE intCol = @value", target.ToSQLFilter(true));
        }


        [Test]
        public void BasicBadFilterDiffrentTypes()
        {
            var target = new TestFilter();
            target.HasIntColFilter(123);
            Assert.Throws<ApplicationException>(() => target.HasShortColFilterBadColName(123));
        }

        [Test]
  
        public void HasIntColFilter()
        {
            var target = new TestFilter();
            target.HasIntColFilter(123);
            Assert.Throws<ApplicationException>(() => target.HasIntColFilter(456));
        }

        [Test]
        public void BasicTestSillyAnd()
        {
            var target = new TestFilter();
            target.HasIntColFilter(123);
            target.HasIntColFilter(123);
            Assert.IsTrue(target.Parameters.Count == 1);
            Assert.AreEqual("intCol = @value And intCol = @value", target.ToSQLFilter());
        }

    }
}
