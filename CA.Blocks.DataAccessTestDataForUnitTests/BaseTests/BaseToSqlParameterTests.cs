using NUnit.Framework;
using System;
using System.Data.Common;
using System.Data;
using System.Linq;
using CA.Blocks.DataAccessTestDataForUnitTests.AssertExtensions;
using System.Collections.Generic;
using CA.Blocks.DataAccessTestDataForUnitTests.TestTypes;

namespace CA.Blocks.DataAccessTestDataForUnitTests.BaseTests
{

    public class TypeToDbParameterResult
    {
        public Type SourceType { get; set; }
        public DbParameter DbParameter { get; set; }
    }

    public abstract class BaseToSqlParameterTests
    {
        // The inheriting class will provide this example for sql server
        //  return ToSqlParameterTypeTestMain<T, SqlParameter>(typeof(SqlServerParameterExtensions), test, expectedDbType);
        public abstract DbParameter ToSqlParameterTypeInstanceTestMain<T>(T test, DbType expectedDbType);


        protected bool IsNullable<T>(T value)
        {
            return Nullable.GetUnderlyingType(typeof(T)) != null;
        }

        // This tests that the ToSqlParameter exists for given type and is the expected underlying database type
        protected DbParameter ToSqlParameterTypeTestMain<T, TSqlp>(Type toSqlParameterExtensionClass, T test, DbType expectedDbType) where TSqlp : DbParameter, new()
        {
            // Act
            var methods = toSqlParameterExtensionClass.GetMethods().Where(x => x.Name == "ToSqlParameter");
            var methodForType = methods.Where(x => x.GetParameters()[0].ParameterType.FullName == typeof(T).FullName);

            var target = methodForType.FirstOrDefault();
            Assert.IsNotNull(target, $"There is no ToSqlParameter Method for Type - {typeof(T).FullName}");
            var targetParameters = target.GetParameters();
            var sqlParam = new TSqlp();
            // Invoke
            if (targetParameters.Length == 2)
            {
                sqlParam = (TSqlp)target.Invoke(null, new object[] { test, "@paramName" });
            }
            if (targetParameters.Length == 3)
            {
                sqlParam = (TSqlp)target.Invoke(null, new object[] { test, "@paramName",
                    targetParameters[2].DefaultValue });
            }
            if (targetParameters.Length == 4)
            {
                sqlParam = (TSqlp)target.Invoke(null, new object[] { test, "@paramName",
                    targetParameters[2].DefaultValue,
                    targetParameters[3].DefaultValue });
            }
            if (targetParameters.Length == 5)
            {
                sqlParam = (TSqlp)target.Invoke(null, new object[] { test, "@paramName",
                    targetParameters[2].DefaultValue,
                    targetParameters[3].DefaultValue,
                    targetParameters[4].DefaultValue
                });
            }

            // assert
            Assert.AreEqual(expectedDbType, sqlParam.DbType);
            Assert.AreEqual(ParameterDirection.Input, sqlParam.Direction);
            Assert.AreEqual(false, sqlParam.IsNullable);
            Assert.AreEqual("@paramName", sqlParam.ParameterName);
            return sqlParam;
        }

        public TypeToDbParameterResult ToSqlParameterTypeTest<T>(T test, DbType expectedDbType, Action<T, DbParameter> compareAction = null)
        {
            var result = new TypeToDbParameterResult{ SourceType = typeof(T)};
            result.DbParameter = ToSqlParameterTypeInstanceTestMain<T>(test, expectedDbType);
            if (compareAction != null)
            {
                compareAction(test, result.DbParameter);
            }
            else
            {
                Assert.AreEqual(test, result.DbParameter.Value, $"{test.GetType().FullName}");
            }

            if (IsNullable(test))
            {
                result.SourceType = typeof(T).GenericTypeArguments[0];
                // Test Null values
                T nullTest = default(T);
                var nullDbParameter = ToSqlParameterTypeInstanceTestMain<T>((T)nullTest, expectedDbType);
                Assert.AreEqual(DBNull.Value, nullDbParameter.Value, $"{test.GetType().FullName}");
            }
            return result;
        }

#if NET6_0_OR_GREATER

        public TypeToDbParameterResult ToSqlParameterTypeTestDateOnly(DateOnly test, DbType expectedDbType, Action<DateOnly?, DbParameter> compareAction = null)
        {
            var result = new TypeToDbParameterResult() { SourceType = typeof(DateOnly) };
            result.DbParameter = ToSqlParameterTypeInstanceTestMain<DateOnly>(test, expectedDbType);
            Assert.NotNull(result.DbParameter.Value);
            if (compareAction != null)
            {
                compareAction(test, result.DbParameter);
            }
            else
            {
                test.IsSameValueAs((DateTime)result.DbParameter.Value);
            }
            // Test Null values
            var nullDbParameter = ToSqlParameterTypeInstanceTestMain<DateOnly?>(null, expectedDbType);
            Assert.AreEqual(DBNull.Value, nullDbParameter.Value, $"{test.GetType().FullName}");
            return result;

        }

        public TypeToDbParameterResult ToSqlParameterTypeTestTimeOnly(TimeOnly test, DbType expectedDbType, Action<TimeOnly?, DbParameter> compareAction = null)
        {
            var result = new TypeToDbParameterResult() { SourceType = typeof(TimeOnly) };
            result.DbParameter = ToSqlParameterTypeInstanceTestMain<TimeOnly>(test, expectedDbType);
            Assert.NotNull(result.DbParameter.Value);
            if (compareAction != null)
            {
                compareAction(test, result.DbParameter);
            }
            else
            {
                test.IsSameValueAs((TimeSpan)result.DbParameter.Value);
            }

            // Test Null values
            var nullDbParameter = ToSqlParameterTypeInstanceTestMain<TimeOnly?>(null, expectedDbType);
            Assert.AreEqual(DBNull.Value, nullDbParameter.Value, $"{test.GetType().FullName}");
            return result;
        }
#endif


        protected IList<Type> GetUnTestedTypes(IList<TypeToDbParameterResult> results)
        {
            var untestedTypes = new List<Type>();
            var allExpectedTypes = TestDotNetTypesToSqlParameter.AllExpectedTypeValues();
            foreach (var expectedType in allExpectedTypes)
            {
                var found = results.FirstOrDefault(x => x.SourceType.FullName == expectedType.FullName);
                if (found == default)
                {
                    untestedTypes.Add(expectedType);
                }
            }
            return untestedTypes;
        }
    }
}
