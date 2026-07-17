using System;
using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.SqliteDataAccess;
using CA.Blocks.SqliteDataAccessUnitTests.Base;

namespace CA.Blocks.SqliteDataAccessUnitTests.SQLLite.DbTypeTests
{
    public class DbTypeGuidTests : UnitTestDataAccess, IDisposable
    {
        private const string TestGuidValue = "CE69B300-F9EA-4F3B-BBA8-676D12737E3E";
        private class GuidDataType
        {
            public Guid Col { get; set; }
        }


        private void InsertTestDataSQL(Guid data)
        {
            var cmd = CreateTextCommand(InsertTestDataSQL("@data")).WithParameter(data.ToSqlParameter("@data"));
            ExecuteNonQuery(cmd);
        }
        public DbTypeGuidTests()
{
            ExecuteNonQuery(DropTestTableSQL());
            ExecuteNonQuery(CreateTestTable("VARCHAR(36) not null"));
            InsertTestDataSQL(Guid.Empty);
            InsertTestDataSQL(Guid.NewGuid());
            InsertTestDataSQL(Guid.NewGuid());
            InsertTestDataSQL(Guid.NewGuid());
            InsertTestDataSQL(Guid.Parse(TestGuidValue));
        }
        public new void Dispose()
{
            ExecuteNonQuery(DropTestTableSQL());
        }

        [Fact]
        public void SelectAllData()
        {
            //Setup 
            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = Execute(cmd).ToListOf<GuidDataType>();
            //Assert
            Assert.True(data.Count == 5);
        }



        [Fact]
        public void SelectAllDataToListOf()
        {
            //Setup 

            var cmd = CreateTextCommand(SelectTestDataSQL());
            //Act
            var data = ExecuteToListOf<GuidDataType>(cmd);
            //Assert
            Assert.True(data.Count == 5);
            Assert.Equal(Guid.Parse(TestGuidValue), data[4].Col);
        }

        [Fact]
        public void SelectAllDataTimeWithFilter()
        {
            //setup
            Guid testvalue = Guid.Parse(TestGuidValue);
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col = @testValue");
            cmd.Parameters.Add(testvalue.ToSqlParameter("@testValue"));

    
            //Act
            var data = ExecuteTo<GuidDataType>(cmd);

            //Asert
            Assert.NotNull(data);
            Assert.Equal(testvalue, data.Col);
        }

        [Fact]
        public void SelectAllDataWithWithTranslator()
        {
            //setup
            Guid testValue = Guid.Parse(TestGuidValue);
            var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col = @value").WithParameter(testValue.ToSqlParameter("@value"));
            var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<GuidDataType>();
            //Act
            var data = t.Translate(ExecuteDataRow(cmd));

            Assert.NotNull(data);
            Assert.Equal(testValue, data.Col);
        }
    }
}





