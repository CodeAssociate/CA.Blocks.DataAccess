using CA.Blocks.DataAccess.Translator.Extensions;

namespace CA.Blocks.DataAccessUnitTests.Translator.Extensions;

[TestFixture]
public class DataReaderExtensionDataTableTests : DataReaderExtensionsBaseTests
{

	[Test]
	public async Task DataReaderExtensions_ToDataTable_ValidRow()
	{
		var numberOfRecords = 10;
		var dataReader = GenerateTestDataReaderAsync(numberOfRecords);

		var result = await dataReader.ToDataTable();
		Assert.That(result.Columns.Count, Is.EqualTo(4));
		Assert.That(result.Rows.Count, Is.EqualTo(numberOfRecords));
	}
}