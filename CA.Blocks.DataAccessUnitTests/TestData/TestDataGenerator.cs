using System.Data;
using System.Text.RegularExpressions;

namespace CA.Blocks.DataAccessUnitTests.TestData
{
    public static class TestDataGenerator
    {
        public enum SourceColNamingConvention
        {
            // PascalCase
            PascalCase,
            // camelCase
            CamelCase,
            // snake_case
            SnakeCase,
        }
        
        private static string SnakeToCamelCase(string input)
        {
            if (string.IsNullOrEmpty(input)) 
                return input;
            
            string pascalCase = SnakeToPascalCase(input);

            // Ensure the very first character is lowercase
            return char.ToLower(pascalCase[0]) + pascalCase.Substring(1);
        }
        private static string SnakeToPascalCase(string input)
        {
            if (string.IsNullOrEmpty(input)) 
                return input;

            // Capitalize letters following an underscore
            string pascalCase = Regex.Replace(input, @"_([a-z])", m => m.Groups[1].Value.ToUpper());

            // Ensure the very first character is lowercase
            return char.ToUpper(pascalCase[0]) + pascalCase.Substring(1);
        }

        private static string ConvertSnakeToCase(string input, SourceColNamingConvention namingConvention)
        {
            switch (namingConvention)
            {
                case SourceColNamingConvention.PascalCase:
                    return SnakeToPascalCase(input);
                case SourceColNamingConvention.CamelCase:
                    return SnakeToCamelCase(input);
                default:
                    return input;
            }
        }

        public static DataTable GenerateTestDataForTestDataClassAsDataTable(int start, int count, SourceColNamingConvention namingConvention = SourceColNamingConvention.PascalCase)
        {
            var testData = new DataTable();
            testData.Columns.Add(ConvertSnakeToCase("int_col", namingConvention), typeof(int));
            testData.Columns.Add(ConvertSnakeToCase("string_col", namingConvention), typeof(string));
            testData.Columns.Add(ConvertSnakeToCase("guid_col", namingConvention), typeof(Guid));
            testData.Columns.Add(ConvertSnakeToCase("date_col", namingConvention), typeof(DateTime));
            testData.AcceptChanges();
            for (var i = start; i <= (start + count - 1); i++)
            {
                testData.Rows.Add(i, $"row#{i}", Guid.NewGuid(), DateTime.Now.AddMinutes(i));
            }
            testData.AcceptChanges();
            return testData;
        }
        public static IDataReader GenerateTestDataForTestDataClassAsDataReader(int start, int count, SourceColNamingConvention namingConvention = SourceColNamingConvention.PascalCase)
        {
            return GenerateTestDataForTestDataClassAsDataTable(start, count, namingConvention).CreateDataReader();
        }

    }
}
