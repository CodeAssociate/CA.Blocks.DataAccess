using System;
using System.Data;
using CA.Blocks.SQLServerDataAccess.Builder;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;
using NUnit.Framework;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Builder.Types
{
    [TestFixture]
    public class BuilderTypeTests : UnitTestDataAccess
    {
        protected void TestRoundTripType<T>(T value)
        {
            var sqlBuilder = new SafeSqlBuilder();
            sqlBuilder.AddSql($"Select {value:@testValue} as Test");
            var sqlcmd = sqlBuilder.BuildSqlCommand();

            var result = this.ExecuteScalarAs<T>(sqlcmd);
            Assert.That(sqlcmd.CommandText, Is.EqualTo("Select @testValue as Test"));
            Assert.That(sqlcmd.Parameters.Count, Is.EqualTo(1));
            Assert.That(sqlcmd.Parameters[0].Value, value == null ? Is.EqualTo(DBNull.Value) : Is.EqualTo(value));
            if (typeof(T) == typeof(TimeOnly))
            {
                Assert.That(result.ToString(), Is.EqualTo(value.ToString()));
            }
            else
            {
                Assert.That(result, Is.EqualTo(value));
            }
        }

        protected void TestRoundTripTypeWithConvert<T>(T value)
        {
            var sqlBuilder = new SafeSqlBuilder();
            sqlBuilder.AddSql($"Select {value:@testValue} as Test");
            var sqlcmd = sqlBuilder.BuildSqlCommand();


            var result = this.ExecuteScalarWithConvertAs<T>(sqlcmd);
            Assert.That(sqlcmd.CommandText, Is.EqualTo("Select @testValue as Test"));
            Assert.That(sqlcmd.Parameters.Count, Is.EqualTo(1));
            Assert.That(sqlcmd.Parameters[0].Value, value == null ? Is.EqualTo(DBNull.Value) : Is.EqualTo(value));
            Assert.That(result, Is.EqualTo(value));
        }


        protected void TestRoundTripTypeSpecificSqlType<T>(T value, SqlDbType expectedType)
        {
            var sqlBuilder = new SafeSqlBuilder();
            // you need to add a target for each as this is not run time 
            if (expectedType == SqlDbType.VarChar)
            {
                sqlBuilder.AddSql($"Select {value:@testValue|varchar} as Test");
            }
            var sqlcmd = sqlBuilder.BuildSqlCommand();

            var result = this.ExecuteScalarAs<T>(sqlcmd);
            Assert.That(sqlcmd.CommandText, Is.EqualTo("Select @testValue as Test"));
            Assert.That(sqlcmd.Parameters.Count, Is.EqualTo(1));
            Assert.That(sqlcmd.Parameters[0].SqlDbType, Is.EqualTo(expectedType));
            Assert.That(sqlcmd.Parameters[0].Value, value == null ? Is.EqualTo(DBNull.Value) : Is.EqualTo(value));

            Assert.That(result, Is.EqualTo(value));
        }


        [Test]
        public void BigIntTest()
        {
            TestRoundTripType<long>(123);
        }

        [Test]
        public void BigIntNullTest()
        {
            TestRoundTripType<long?>(null);
        }

        [Test]
        public void ByteTest()
        {
            TestRoundTripType<byte>(123);
        }

        [Test]
        public void ByteNullTest()
        {
            TestRoundTripType<byte?>(null);
        }



        [Test]
        public void BinaryTest()
        {

            TestRoundTripType<byte[]>(new byte[] {123, 124, 99, 98});
        }

        [Test]
        public void BinaryTestNull()
        {
            TestRoundTripType<byte[]?>(null);
        }

        [Test]
        public void BoolTest()
        {

            TestRoundTripType<bool>(true);
        }

        [Test]
        public void BoolTestNull()
        {
            TestRoundTripType<bool?>(null);
        }


        [Test]
        public void CharTest()
        {
            TestRoundTripTypeWithConvert<Char>('A');
        }

        [Test]
        public void CharNullTest()
        {
            TestRoundTripTypeWithConvert<Char?>(null);
        }

        [Test]
        public void DateOnlyTest()
        {
            TestRoundTripType<DateOnly>(new DateOnly(2025,2,28));
        }

        [Test]
        public void DateTimeTest()
        {
            TestRoundTripType<DateTime>(DateTime.Now);
        }

        [Test]
        public void DateTimeNullTest()
        {
            TestRoundTripType<DateTime?>(null);
        }

        [Test]
        public void DecimalTest()
        {
            TestRoundTripType<Decimal>(123.456m);
        }

        [Test]
        public void DecimalNullTest()
        {
            TestRoundTripType<Decimal?>(null);
            TestRoundTripType<Decimal?>(123.456m);
        }

        [Test]
        public void DoubleTest()
        {
            TestRoundTripType<Double>(123.456);
        }

        [Test]
        public void DoubleNullTest()
        {
            TestRoundTripType<Double?>(null);
            TestRoundTripType<Double?>(123.456);
        }

        [Test]
        public void GuidTest()
        {
            TestRoundTripType<Guid>(Guid.NewGuid());
        }

        [Test]
        public void GuidNullTest()
        {
            TestRoundTripType<Guid?>(null);
            TestRoundTripType<Guid?>(Guid.NewGuid());
        }

        [Test]
        public void TimeOnlyNullTest()
        {
            TestRoundTripType<DateOnly?>(null);
        }

        [Test]
        public void TimeOnlyTest()
        {
            TestRoundTripType<TimeOnly>(new TimeOnly(21, 22, 23));
        }

        [Test]
        public void DateOnlyNullTest()
        {
            TestRoundTripType<DateOnly?>(null);
        }


        [Test]
        public void StringTest()
        {
            TestRoundTripType<string>("Hello");
        }
        [Test]
        public void StringTest_AsVarchar()
        {
            TestRoundTripTypeSpecificSqlType<string>("Hello", SqlDbType.VarChar);
        }


        [Test]
        public void StringNullTest()
        {
            TestRoundTripType<string>(null);
        }


        [Test]
        public void IntTest()
        {
            TestRoundTripType<int>(123);
        }

        [Test]
        public void IntNullTest()
        {
            TestRoundTripType<int?>(null);
        }

    }

}
