using CA.Blocks.DataAccess.Translator.Basic;
using CA.Blocks.PostgreSQLDataAccessUnitTests.Base;
using CA.Blocks.SQLServerDataAccess;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace CA.Blocks.PostgreSQLDataAccessUnitTests.DbTypeTests;

[TestFixture]
public class DbTypeNCharTests : UnitTestDataAccess
{
    private void InsertTestDataSQL(char data)
    {
        ExecuteNonQuery(InsertTestDataSQL(string.Format("'{0}'", data)));
    }

    [SetUp]
    public void Setup()
    {
        ExecuteNonQuery(DropTestTableSQL());
        ExecuteNonQuery(CreateTestTable("Char(1) not null"));
        InsertTestDataSQL('ä');
        InsertTestDataSQL('B');
        InsertTestDataSQL('C');
        InsertTestDataSQL('D');
        InsertTestDataSQL('E');
    }

    [TearDown]
    public void TearDown()
    {
        ExecuteNonQuery(DropTestTableSQL());
    }

    [Test]
    public void SelectAllDataNChar()
    {
        //Setup 
        var cmd = CreateTextCommand(SelectTestDataSQL());
        var t = new CharTranslator(UNIT_TEST_COL_NAME);
        //Act
        var data = t.Translate(ExecuteDataTable(cmd));
        //Assert
        ClassicAssert.AreEqual(5, data.Count);
    }

    [Test]
    public void SelectAllDataNCharWithFilter ()
    {
        //setup
        char testvalue = 'ä';
        var t = new CharTranslator(UNIT_TEST_COL_NAME);
        var cmd = CreateTextCommand(SelectTestDataSQL("Where col = @testValue"));
        cmd.Parameters.Add(testvalue.ToPostgresParameter("@testValue"));

        //Act
        var data = t.Translate(ExecuteDataRow(cmd));

        //Asert
        ClassicAssert.AreEqual(testvalue, data);

 
    }


}

