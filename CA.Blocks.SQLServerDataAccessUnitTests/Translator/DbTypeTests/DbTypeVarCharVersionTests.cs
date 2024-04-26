using System;
using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.SQLServerDataAccess;
using CA.Blocks.SQLServerDataAccessUnitTests.Base;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace CA.Blocks.SQLServerDataAccessUnitTests.Translator.DbTypeTests;

[TestFixture]
public class DbTypeVarCharVersionTests : UnitTestDataAccess
{

    private class VersionDataType
    {
        public Version Col { get; set; }
    }


    private void InsertTestData(string data)
    {
        ExecuteNonQuery(InsertTestDataSQL($"'{data}'"));
    }

    [SetUp]
    public void Setup()
    {
        ExecuteNonQuery(DropTestTableSQL());
        ExecuteNonQuery(CreateTestTable("varchar(20) null"));
        InsertTestData(new Version(1,2).ToString());
        InsertTestData(new Version(1, 2,3).ToString());
        InsertTestData(new Version(1, 2, 3, 4).ToString());
        ExecuteNonQuery(InsertTestDataSQL("null"));
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

        var data = Execute(cmd).ToListOf<VersionDataType>();
        //Assert
        ClassicAssert.AreEqual(4, data.Count);
        ClassicAssert.AreEqual(new Version("1.2"), data[0].Col);
        ClassicAssert.AreEqual(new Version("1.2.3"), data[1].Col);
        ClassicAssert.AreEqual(new Version("1.2.3.4"), data[2].Col);
        ClassicAssert.IsNull(data[3].Col);
    }



    [Test]
    public void SelectDataWithFilter()
    {
        //setup
        var testvalue = new Version(1,2,3);
        var cmd = CreateTextCommand(SelectTestDataSQL(), "Where col = @testValue");
        cmd.Parameters.Add(testvalue.ToSqlParameter("@testValue"));

        //Act
        var data = Execute(cmd).ToFirstOrDefault<VersionDataType>();
        //Asert
        ClassicAssert.AreEqual(data.Col, testvalue);
    }
}