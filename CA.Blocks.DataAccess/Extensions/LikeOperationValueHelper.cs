namespace CA.Blocks.DataAccess.Extensions
{
    // This is standard function for standard sql 92 Like operator https://www.w3schools.com/sql/sql_like.asp
    // we deals with the adding Wildcard and escaping  Wildcard within content
    public static class LikeOperationValueHelper
    {
        public static string EscapeWildcards(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return input;
            }
            var result = input;

#pragma warning disable CA1847 
            //we still supporting .net standard
            if (result.Contains("["))
#pragma warning restore CA1847
            {
                result = result.Replace("[", "[[]");
            }
#pragma warning disable CA1847 
            //we still supporting .net standard
            if (result.Contains("_"))
#pragma warning restore CA1847
            {
                result = result.Replace("_", "[_]");
            }
#pragma warning disable CA1847 
            //we still supporting .net standard
            if (result.Contains("%"))
#pragma warning restore CA1847
            {
                result = result.Replace("%", "[%]");
            }
            return result;
        }

        // Looking for "Search Value" -> the result will be "%Search Value%" we deal with the other wild cards within the search
        public static string PrepContainsValue(string input)
        {
            var result = EscapeWildcards(input);
            if (string.IsNullOrWhiteSpace(input))
            {
                return input;
            }
#pragma warning disable CA1847 
            //we still supporting .net standard
            if (!result.StartsWith("%"))
#pragma warning restore CA1847
            {
                result = $"%{result}";
            }
#pragma warning disable CA1847 
            //we still supporting .net standard
            if (!result.EndsWith("%"))
                
#pragma warning restore CA1847
            {
                result = $"{result}%";
            }
            return result;
        }

        public static string PrepStartsWithValue(string input)
        {
            var result = EscapeWildcards(input);
            if (string.IsNullOrWhiteSpace(input))
            {
                return input;
            }
#pragma warning disable CA1847 
            //we still supporting .net standard
            if (!result.EndsWith("%"))
#pragma warning restore CA1847
            {
                result = $"{result}%";
            }
            return result;
        }
        
        public static string PrepEndsWithValue(string input)
        {
            var result = EscapeWildcards(input);
            if (string.IsNullOrWhiteSpace(input))
            {
                return input;
            }
#pragma warning disable CA1847 
            // we still supporting .net standard
            if (!result.StartsWith("%"))
#pragma warning restore CA1847
            {
                result = $"%{result}";
            }
            return result;
        }
    }
}