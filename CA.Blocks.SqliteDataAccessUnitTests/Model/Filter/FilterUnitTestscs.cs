using System;
using CA.Blocks.DataAccess.Model.Filter;
using CA.Blocks.SqliteDataAccess;
using NUnit.Framework;

namespace CA.Blocks.SqliteDataAccessUnitTests.Model.Filter
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

        // This is an idea for now, to use FormattableString to set a filter
        // ie "Col1 = {param}"
        // Issues we need to solve:
        // 1) We need the user to be aware this is short hand syntax only it not a smart. ie if value is null and used in where clause id not valid ie "col1 = null" is not the same as "col1 is null" 
        // 2) The need the underlying  provider  to support the datatype. example SQLLite will not have native support for DateTime ie col1 = @Datetime is not valid, it should be col1 = Date(@DatetimeAsString)
        // 3) Does the filter belong in the model. we exposing internal data storage concerns.  we need a model to represent a filter but not be a filter. 
        // 4) Again With line above the filter an parsing are two separate concerns  
        // 5) If we need to do a model filter then the FormattableString can be avoided as we need to build the filter from the model this loop back to the simple TestFilter model. 
        /*

        private object[]  GetParametersForToSqlParameter(object target, string paramName, int numberParameters)
        {
            var result = new object[numberParameters];
            result[0] = target;
            result[1] = paramName;
            for (int i = 2; i < numberParameters; i++)
            {
                result[i] = Type.Missing;
            }

            return result;
        }


        public void Parse(FormattableString query)
        {
            var s = query.Format;
            var objects = query.GetArguments();
            var args = Enumerable.Range(0, query.ArgumentCount).Select(i => (object)("@p" + i)).ToArray();
            var newquery = string.Format(query.Format, args);
            TestContext.WriteLine(s);
            TestContext.WriteLine(newquery);
            int i = 0;
            foreach (var o in objects)
            {
                if (o != null)
                {
                    var type = o.GetType();

                    var method = ExtensionMethodsHelper.GetExtensionMethodOrNull(type, "ToSqlParameter");
                    if (method != null)
                    {
                        var sqlparam =  method.Invoke(null, GetParametersForToSqlParameter(o, $"@p{i}", method.GetParameters().Length));
                       var sqlparm1 = (SqliteParameter) sqlparam; 
                       TestContext.WriteLine($"{sqlparm1.ParameterName}-{sqlparm1.DbType}{sqlparm1.Value}");
                    }
                    // need to work out if o has ToSQL

                    TestContext.WriteLine(o.GetType().ToString());
                    TestContext.WriteLine(o);
                }
                else
                {
                    TestContext.WriteLine("null");
                }

                i++;
            }
        }

        [Test]
        public void Debug()
        {
            https://www.meziantou.net/interpolated-strings-advanced-usages.htm
            int param1 = 1;
            string param2 = "Hello";
            //DateTime param3 = DateTime.Now;
            int? param4 = null;
            Parse($" Col1 = {param1} and col2 = {param2} and col3 = ? and col4 = {param4}");
        }
    }



    internal static class ExtensionMethodsHelper
    {

        private static readonly ConcurrentDictionary<Type, IDictionary<string, MethodInfo>> methodsMap = new ConcurrentDictionary<Type, IDictionary<string, MethodInfo>>();

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static MethodInfo GetExtensionMethodOrNull(Type type, string methodName)
        {
            var methodsForType = methodsMap.GetOrAdd(type, GetExtensionMethodsForType);
            return methodsForType.ContainsKey(methodName)
                ? methodsForType[methodName]
                : null;
        }

        private static IDictionary<string, MethodInfo> GetExtensionMethodsForType(Type extendedType, Type returnType)
        {
            // WARNING! Two methods with the same name won't work here
            // for sake of example I ignore this fact
            // but you'll have to do something with that

            return AppDomain.CurrentDomain
                .GetAssemblies()
                .Select(asm => GetExtensionMethods(asm, extendedType, returnType))
                .Aggregate((a, b) => a.Union(b))
                .ToDictionary(mi => mi.Name, mi => mi);
        }

        private static IEnumerable<MethodInfo> GetExtensionMethods(Assembly assembly, Type extendedType, Type returnType)
        {
            var query = from type in assembly.GetTypes()
                where type.IsSealed && !type.IsGenericType && !type.IsNested
                from method in type.GetMethods(BindingFlags.Static | BindingFlags.Public)
                where method.IsDefined(typeof(ExtensionAttribute), false)
                where method.GetParameters()[0].ParameterType == extendedType
                where method.ReturnParameter?.ParameterType == returnType
                select method;
            return query;
        }
        */
    }

}
