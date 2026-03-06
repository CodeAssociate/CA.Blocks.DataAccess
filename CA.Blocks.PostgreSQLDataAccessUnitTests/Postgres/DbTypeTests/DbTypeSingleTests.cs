using System;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.PostgreSQLDataAccessUnitTests.Base;
using CA.Blocks.SQLServerDataAccess;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace CA.Blocks.PostgreSQLDataAccessUnitTests.DbTypeTests;

[TestFixture]
public class DbTypeSingleTests : UnitTestDataAccess
{

    private class SingleDataType
    {
        public Single Col { get; set; }
    }


    private void InsertTestDataSQL(float data)
    {
        ExecuteNonQuery(InsertTestDataSQL($"{data}"));
    }

    [SetUp]
    public void Setup()
    {
        ExecuteNonQuery(DropTestTableSQL());
        ExecuteNonQuery(CreateTestTable("real not null"));
        InsertTestDataSQL((float)-1.2);
        InsertTestDataSQL(0);
        InsertTestDataSQL((float)123.456);
        InsertTestDataSQL(int.MaxValue);
        InsertTestDataSQL((float)123456789.987654321);
    }

    [TearDown]
    public void TearDown()
    {
        ExecuteNonQuery(DropTestTableSQL());
    }

    [Test]
    public void SelectAllData()
    {
        //Setup 
        var cmd = CreateTextCommand(SelectTestDataSQL());
        //Act
        var data = ExecuteDataTable(cmd);
        //Assert
        ClassicAssert.AreEqual(5, data.Rows.Count);
    }


    [Test]
    public void SelectAllDataToListOf()
    {
        //Setup 
        var cmd = CreateTextCommand(SelectTestDataSQL());
        //Act
        var data = ExecuteToListOf<SingleDataType>(cmd);
        //Assert
        ClassicAssert.AreEqual(5, data.Count);
        ClassicAssert.AreEqual(-(float)1.2, data[0].Col);
    }

    [Test]
    public void SelectAllDataFilter ()
    {
        //setup
        const Single testvalue = 123;
        var cmd = CreateTextCommand(SelectTestDataSQL("Where col > @testValue"));
        cmd.Parameters.Add(testvalue.ToPostgresParameter("@testValue"));

        //Act
        var data = ExecuteToListOf<SingleDataType>(cmd);

        //Asert
        ClassicAssert.AreEqual(3, data.Count);
    }
        
    [Test]
    public void SelectAllDataIntWithFilterWithTranslator ()
    {
        //setup
        const Single testvalue = 123; 
        var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<SingleDataType>();
        var cmd = CreateTextCommand(SelectTestDataSQL("Where col >= @testValue"));
        cmd.Parameters.Add(testvalue.ToPostgresParameter("@testValue"));

        //Act
        var data = t.Translate(ExecuteDataTable(cmd));

        //Asert
        ClassicAssert.AreEqual(3, data.Count);
    }
}

