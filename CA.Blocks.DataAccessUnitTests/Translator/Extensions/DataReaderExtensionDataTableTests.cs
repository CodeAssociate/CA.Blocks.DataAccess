using CA.Blocks.DataAccess.Translator.Extensions;

namespace CA.Blocks.DataAccessUnitTests.Translator.Extensions;

public class DataReaderExtensionDataTableTests : DataReaderExtensionsBaseTests
{

	[Fact]
	public async Task DataReaderExtensions_ToDataTable_ValidRow()
	{
		var numberOfRecords = 10;
		var dataReader = GenerateTestDataReaderAsync(numberOfRecords);

		var result = await dataReader.ToDataTable();
		Assert.Equal(4, result.Columns.Count);
		Assert.Equal(numberOfRecords, result.Rows.Count);
	}
}
