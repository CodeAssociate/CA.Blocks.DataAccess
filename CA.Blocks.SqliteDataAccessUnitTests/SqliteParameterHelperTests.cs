using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using CA.Blocks.SqliteDataAccess;
using Microsoft.Data.Sqlite;
using NUnit.Framework;

namespace CA.Blocks.SqliteDataAccessUnitTests;

[TestFixture]
public class SqliteParameterHelperTests
{

	/*
	/// https://learn.microsoft.com/en-gb/dotnet/standard/data/sqlite/types

Boolean			INTEGER
Byte			INTEGER
Byte[]			BLOB
Char			TEXT
DateOnly		TEXT	yyyy-MM-dd
DateTime		TEXT	yyyy-MM-dd HH:mm:ss.FFFFFFF
DateTimeOffset	TEXT	yyyy-MM-dd HH:mm:ss.FFFFFFFzzz
Decimal			TEXT	0.0########################### format. REAL would be lossy.
Double			REAL
Guid			TEXT	00000000-0000-0000-0000-000000000000
Int16			INTEGER
Int32			INTEGER
Int64			INTEGER
SByte			INTEGER
Single			REAL
String			TEXT	UTF-8
TimeOnly		TEXT	HH:mm:ss.fffffff
TimeSpan		TEXT	d.hh:mm:ss.fffffff
UInt16			INTEGER
UInt32			INTEGER
UInt64			INTEGER	Large values overflow
	*/


	private class TestCase
	{
		public Type TargetType { get; init; }
		public SqliteType ExpectedResult { get; init; }
	}


	private IList<TestCase> GetTestCases()
	{
		var result = new List<TestCase>();
		result.Add(new TestCase{ TargetType = typeof(bool), ExpectedResult = SqliteType.Integer});
		result.Add(new TestCase { TargetType = typeof(bool?), ExpectedResult = SqliteType.Integer });

		result.Add(new TestCase { TargetType = typeof(byte), ExpectedResult = SqliteType.Integer });
		result.Add(new TestCase { TargetType = typeof(byte?), ExpectedResult = SqliteType.Integer });

		result.Add(new TestCase { TargetType = typeof(char), ExpectedResult = SqliteType.Text });
		result.Add(new TestCase { TargetType = typeof(char?), ExpectedResult = SqliteType.Text });

		result.Add(new TestCase { TargetType = typeof(DateOnly), ExpectedResult = SqliteType.Text });
		result.Add(new TestCase { TargetType = typeof(DateOnly?), ExpectedResult = SqliteType.Text });


		result.Add(new TestCase { TargetType = typeof(DateTime), ExpectedResult = SqliteType.Text });
		result.Add(new TestCase { TargetType = typeof(DateTime?), ExpectedResult = SqliteType.Text });

		result.Add(new TestCase { TargetType = typeof(DateTimeOffset), ExpectedResult = SqliteType.Text });
		result.Add(new TestCase { TargetType = typeof(DateTimeOffset?), ExpectedResult = SqliteType.Text });

		result.Add(new TestCase { TargetType = typeof(TimeOnly), ExpectedResult = SqliteType.Text });
		result.Add(new TestCase { TargetType = typeof(TimeOnly?), ExpectedResult = SqliteType.Text });

		result.Add(new TestCase { TargetType = typeof(TimeSpan), ExpectedResult = SqliteType.Text });
		result.Add(new TestCase { TargetType = typeof(TimeSpan?), ExpectedResult = SqliteType.Text });

		// Note decimal is stored as text  REAL would be lossy.
		result.Add(new TestCase { TargetType = typeof(Decimal), ExpectedResult = SqliteType.Text });
		result.Add(new TestCase { TargetType = typeof(Decimal?), ExpectedResult = SqliteType.Text });

		result.Add(new TestCase { TargetType = typeof(Double), ExpectedResult = SqliteType.Real });
		result.Add(new TestCase { TargetType = typeof(Double?), ExpectedResult = SqliteType.Real });

		result.Add(new TestCase { TargetType = typeof(Single), ExpectedResult = SqliteType.Real });
		result.Add(new TestCase { TargetType = typeof(Single?), ExpectedResult = SqliteType.Real });


		result.Add(new TestCase { TargetType = typeof(sbyte), ExpectedResult = SqliteType.Integer });
		result.Add(new TestCase { TargetType = typeof(sbyte?), ExpectedResult = SqliteType.Integer });

		result.Add(new TestCase { TargetType = typeof(Int16), ExpectedResult = SqliteType.Integer });
		result.Add(new TestCase { TargetType = typeof(Int16?), ExpectedResult = SqliteType.Integer });

		result.Add(new TestCase { TargetType = typeof(Int32), ExpectedResult = SqliteType.Integer });
		result.Add(new TestCase { TargetType = typeof(Int32?), ExpectedResult = SqliteType.Integer });

		result.Add(new TestCase { TargetType = typeof(Int64), ExpectedResult = SqliteType.Integer });
		result.Add(new TestCase { TargetType = typeof(Int64?), ExpectedResult = SqliteType.Integer });


		result.Add(new TestCase { TargetType = typeof(UInt16), ExpectedResult = SqliteType.Integer });
		result.Add(new TestCase { TargetType = typeof(UInt16?), ExpectedResult = SqliteType.Integer });

		result.Add(new TestCase { TargetType = typeof(UInt32), ExpectedResult = SqliteType.Integer });
		result.Add(new TestCase { TargetType = typeof(UInt32?), ExpectedResult = SqliteType.Integer });

		result.Add(new TestCase { TargetType = typeof(UInt64), ExpectedResult = SqliteType.Integer });
		result.Add(new TestCase { TargetType = typeof(UInt64?), ExpectedResult = SqliteType.Integer });

		result.Add(new TestCase { TargetType = typeof(Guid), ExpectedResult = SqliteType.Text });
		result.Add(new TestCase { TargetType = typeof(Guid?), ExpectedResult = SqliteType.Text });

		result.Add(new TestCase { TargetType = typeof(byte[]), ExpectedResult = SqliteType.Blob });
		return result;
	}

	[Test]
	public void GetDefaultStorageTypeFor_Tests()
	{
		var testCases = GetTestCases();
		foreach (var testCase in testCases)
		{
			// SetUp  + Act 
			var result = SqliteParameterHelper.GetDefaultStorageTypeFor(testCase.TargetType);
			// Asset
			Assert.AreEqual(testCase.ExpectedResult, result);

			TestContext.WriteLine($"\t {testCase.TargetType.FullName} -> {testCase.ExpectedResult}.");
		}
	}


}