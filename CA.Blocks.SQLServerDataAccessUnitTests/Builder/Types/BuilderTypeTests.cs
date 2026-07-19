#nullable enable
using System;
using System.Data;
using CA.Blocks.SQLServerDataAccess.Builder;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Builder.Types
{
    [Collection("DbIntegrationTests")]
    public class BuilderTypeTests : UnitTestDataAccess
    {
        protected void TestRoundTripType<T>(T value)
        {
            var sqlBuilder = new SafeSqlBuilder($"Select {value:@testValue} as Test");
            var sqlcmd = sqlBuilder.BuildSqlCommand();

            var result = this.ExecuteScalarAs<T>(sqlcmd);
            Assert.Equal("Select @testValue as Test", sqlcmd.CommandText);
            Assert.Equal(1, sqlcmd.Parameters.Count);
            Assert.Equal(value is null ? DBNull.Value : value, sqlcmd.Parameters[0].Value);
            if (typeof(T) == typeof(TimeOnly))
            {
                Assert.Equal(value!.ToString(), result!.ToString());
            }
            else
            {
                Assert.Equal(value, result);
            }
        }

        protected void TestRoundTripTypeWithConvert<T>(T value)
        {
            var sqlBuilder = new SafeSqlBuilder();
            sqlBuilder.AddSql($"Select {value:@testValue} as Test");
            var sqlcmd = sqlBuilder.BuildSqlCommand();


            var result = this.ExecuteScalarWithConvertAs<T>(sqlcmd);
            Assert.Equal("Select @testValue as Test", sqlcmd.CommandText);
            Assert.Equal(1, sqlcmd.Parameters.Count);
            Assert.Equal(value is null ? DBNull.Value : value, sqlcmd.Parameters[0].Value);
            Assert.Equal(value, result);
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
            Assert.Equal("Select @testValue as Test", sqlcmd.CommandText);
            Assert.Equal(1, sqlcmd.Parameters.Count);
            Assert.Equal(expectedType, sqlcmd.Parameters[0].SqlDbType);
            Assert.Equal(value is null ? DBNull.Value : value, sqlcmd.Parameters[0].Value);

            Assert.Equal(value, result);
        }


        [Fact]
        public void BigIntTest()
        {
            TestRoundTripType<long>(123);
        }

        [Fact]
        public void BigIntNullTest()
        {
            TestRoundTripType<long?>(null);
        }

        [Fact]
        public void ByteTest()
        {
            TestRoundTripType<byte>(123);
        }

        [Fact]
        public void ByteNullTest()
        {
            TestRoundTripType<byte?>(null);
        }



        [Fact]
        public void BinaryTest()
        {

            TestRoundTripType<byte[]>(new byte[] {123, 124, 99, 98});
        }

        [Fact]
        public void BinaryTestNull()
        {
            TestRoundTripType<byte[]?>(null);
        }

        [Fact]
        public void BoolTest()
        {

            TestRoundTripType<bool>(true);
        }

        [Fact]
        public void BoolTestNull()
        {
            TestRoundTripType<bool?>(null);
        }


        [Fact]
        public void CharTest()
        {
            TestRoundTripTypeWithConvert<Char>('A');
        }

        [Fact]
        public void CharNullTest()
        {
            TestRoundTripTypeWithConvert<Char?>(null);
        }

        [Fact]
        public void DateOnlyTest()
        {
            TestRoundTripType<DateOnly>(new DateOnly(2025,2,28));
        }

        [Fact]
        public void DateTimeTest()
        {
            TestRoundTripType<DateTime>(DateTime.Now);
        }

        [Fact]
        public void DateTimeNullTest()
        {
            TestRoundTripType<DateTime?>(null);
        }

        [Fact]
        public void DecimalTest()
        {
            TestRoundTripType<Decimal>(123.456m);
        }

        [Fact]
        public void DecimalNullTest()
        {
            TestRoundTripType<Decimal?>(null);
            TestRoundTripType<Decimal?>(123.456m);
        }

        [Fact]
        public void DoubleTest()
        {
            TestRoundTripType<Double>(123.456);
        }

        [Fact]
        public void DoubleNullTest()
        {
            TestRoundTripType<Double?>(null);
            TestRoundTripType<Double?>(123.456);
        }

        [Fact]
        public void GuidTest()
        {
            TestRoundTripType<Guid>(Guid.NewGuid());
        }

        [Fact]
        public void GuidNullTest()
        {
            TestRoundTripType<Guid?>(null);
            TestRoundTripType<Guid?>(Guid.NewGuid());
        }

        [Fact]
        public void TimeOnlyNullTest()
        {
            TestRoundTripType<DateOnly?>(null);
        }

        [Fact]
        public void TimeOnlyTest()
        {
            TestRoundTripType<TimeOnly>(new TimeOnly(21, 22, 23));
        }

        [Fact]
        public void DateOnlyNullTest()
        {
            TestRoundTripType<DateOnly?>(null);
        }


        [Fact]
        public void StringTest()
        {
            TestRoundTripType<string>("Hello");
        }
        [Fact]
        public void StringTest_AsVarchar()
        {
            TestRoundTripTypeSpecificSqlType<string>("Hello", SqlDbType.VarChar);
        }


        [Fact]
        public void StringNullTest()
        {
            TestRoundTripType<string?>(null);
        }


        [Fact]
        public void IntTest()
        {
            TestRoundTripType<int>(123);
        }

        [Fact]
        public void IntNullTest()
        {
            TestRoundTripType<int?>(null);
        }

    }

}




