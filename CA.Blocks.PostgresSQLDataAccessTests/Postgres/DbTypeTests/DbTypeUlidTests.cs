
using CA.Blocks.DataAccess.Extensions.Translators.NUlid;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Providers;
using CA.Blocks.DataAccess.Translator.Extensions;
using CA.Blocks.PostgreSQLDataAccessUnitTests.Base;
using CA.Blocks.SQLServerDataAccess;
using NUlid;

namespace CA.Blocks.PostgreSQLDataAccessUnitTests.DbTypeTests;

[Collection("DbTypeTests")]
public class DbTypeUlidAsStringTests : UnitTestDataAccess, IDisposable
{
    private IList<Ulid> _testData = new List<Ulid>();


    private class UlidDataType
    {
        public Ulid Col { get; set; }
    }

    private void InsertTestDataSQL(Ulid data)
    {
        ExecuteNonQuery(InsertTestDataSQL($"'{data.ToString()}'"));
    }


    private void LoadTestData()
    {
        _testData.Clear();
        _testData.Add(new Ulid("01H5HHY4ZG3CXE07C8TJKBCR0V")); // data for 17/07/2023 9:12:08 AM +00:00
        _testData.Add(new Ulid("01H5M4AVZGAM9FS8TAQEY6CH7R")); // data for 18/07/2023 9:12:08 AM +00:00
        _testData.Add(new Ulid("01H5S949ZG5HW7F1B0HTBWF3RR")); // data for 20/07/2023 9:12:08 AM +00:00
        _testData.Add(new Ulid("01H610AEZG2A3E3NRS3V5QH477")); // data for 23/07/2023 9:12:08 AM +00:00
        _testData.Add(new Ulid("01H6B9XAZGW2RDKHWHTB15JQ9W")); // data for 27/07/2023 9:12:08 AM +00:00

    }

    public DbTypeUlidAsStringTests()
    {
        LoadTestData();

        ExecuteNonQuery(DropTestTableSQL());
        ExecuteNonQuery(CreateTestTable("char(26) not null"));

        foreach (var item in _testData)
        {
            InsertTestDataSQL(item);
        }
    }

    public new void Dispose()
    {
        ExecuteNonQuery(DropTestTableSQL());
        base.Dispose();
    }

    [Fact]
    public void SelectAllData()
    {
        //Setup
        var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<UlidDataType>();
        var cmd = CreateTextCommand(SelectTestDataSQL());
        //Act
        var data = t.Translate(ExecuteDataTable(cmd));
        //Assert
        Assert.Equal(5, data.Count);
    }


    [Fact]
    public void SelectAllDataToListOf()
    {
        //Setup
        var cmd = CreateTextCommand(SelectTestDataSQL());
        //Act
        var data = Execute(cmd).ToListOf<UlidDataType>();
        //Assert
        Assert.Equal(5, data.Count);
        Assert.Equal(new Ulid("01H5HHY4ZG3CXE07C8TJKBCR0V"), data[0].Col);
    }




    [Fact]
    public void SelectAllDataWithFilter()
    {
        //setup
        var testvalue = new Ulid("01H5S949ZG5HW7F1B0HTBWF3RR");
        var cmd = CreateTextCommand(SelectTestDataSQL("Where col >= @testValue"));
        cmd.Parameters.Add(testvalue.AsString().ToPostgresParameter("@testValue"));

        //Act
        var data = Execute(cmd).ToListOf<UlidDataType>();

        //Asert
        Assert.Equal(3, data.Count);
    }

    [Fact]
    public void SelectSingleWithFilter()
    {
        var testvalue = new Ulid("01H5S949ZG5HW7F1B0HTBWF3RR");
        var cmd = CreateTextCommand(SelectTestDataSQL("Where col = @testValue"));
        cmd.Parameters.Add(testvalue.AsString().ToPostgresParameter("@testValue"));

        //Act
        var data = Execute(cmd).ToFirstOrDefault<UlidDataType>();

        //Asert
        Assert.Equal(testvalue, data.Col);
    }
}
////
///
///
///

[Collection("DbTypeTests")]
public class DbTypeUlidAsUuidTests : UnitTestDataAccess, IDisposable
{
    private IList<Ulid> _testData = new List<Ulid>();


    private class UlidDataType
    {
        public Ulid Col { get; set; }
    }

    private void InsertTestDataSQL(Ulid data)
    {
        ExecuteNonQuery(InsertTestDataSQL($"'{(data.ToGuid())}'"));
    }


    private void LoadTestData()
    {
        _testData.Clear();
        _testData.Add(new Ulid("01H5HHY4ZG3CXE07C8TJKBCR0V")); // data for 17/07/2023 9:12:08 AM +00:00
        _testData.Add(new Ulid("01H5M4AVZGAM9FS8TAQEY6CH7R")); // data for 18/07/2023 9:12:08 AM +00:00
        _testData.Add(new Ulid("01H5S949ZG5HW7F1B0HTBWF3RR")); // data for 20/07/2023 9:12:08 AM +00:00
        _testData.Add(new Ulid("01H610AEZG2A3E3NRS3V5QH477")); // data for 23/07/2023 9:12:08 AM +00:00
        _testData.Add(new Ulid("01H6B9XAZGW2RDKHWHTB15JQ9W")); // data for 27/07/2023 9:12:08 AM +00:00

    }

    public DbTypeUlidAsUuidTests()
    {
        LoadTestData();

        ExecuteNonQuery(DropTestTableSQL());
        ExecuteNonQuery(CreateTestTable("uuid not null"));

        foreach (var item in _testData)
        {
            InsertTestDataSQL(item);
        }
    }

    public new void Dispose()
    {
        ExecuteNonQuery(DropTestTableSQL());
        base.Dispose();
    }

    [Fact]
    public void SelectAllData()
    {
        //Setup
        var t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<UlidDataType>();
        var cmd = CreateTextCommand(SelectTestDataSQL());
        //Act
        var data = t.Translate(ExecuteDataTable(cmd));
        //Assert
        Assert.Equal(5, data.Count);
    }


    [Fact]
    public void SelectAllDataToListOf()
    {
        //Setup
        var cmd = CreateTextCommand(SelectTestDataSQL());
        //Act
        var data = Execute(cmd).ToListOf<UlidDataType>();
        //Assert
        Assert.Equal(5, data.Count);
        Assert.Equal(new Ulid("01H5HHY4ZG3CXE07C8TJKBCR0V"), data[0].Col);
    }



    // the uulid stored as a Guid in the postgress DB is not stored in ulid order
    //[Fact]
    //public void SelectAllDataWithFilter()
    //{
    //    //setup
    //    var testvalue = new Ulid("01H5S949ZG5HW7F1B0HTBWF3RR");
    //    var cmd = CreateTextCommand(SelectTestDataSQL("Where col >= @testValue"));
    //    cmd.Parameters.Add(testvalue.ToGuid().ToPostgresParameter("@testValue"));

    //    //Act
    //    var data = Execute(cmd).ToListOf<UlidDataType>();
    //    foreach (var item in data)
    //    {
    //        TestContext.WriteLine(item.Col);
    //    }

    //    //Asert
    //    Assert.Equal(3, data.Count);
    //}




    [Fact]
    public void SelectSingleWithFilter()
    {
        var testvalue = new Ulid("01H610AEZG2A3E3NRS3V5QH477");
        var cmd = CreateTextCommand(SelectTestDataSQL("Where col = @testValue"));
        cmd.Parameters.Add(testvalue.ToGuid().ToPostgresParameter("@testValue"));

        //Act
        var data = Execute(cmd).ToFirstOrDefault<UlidDataType>();

        //Asert
        Assert.Equal(testvalue, data.Col);
    }
}
