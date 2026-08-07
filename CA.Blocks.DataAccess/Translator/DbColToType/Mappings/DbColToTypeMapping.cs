using System.Data;
using CA.Blocks.DataAccess.Translator.DbColToType.Interfaces;

namespace CA.Blocks.DataAccess.Translator.DbColToType.Mappings
{
    public class DbColToTypeMapping : IDbColToTypeMapping
    {
        public required string DestinationName { get; set; }
        public required string SourceNameName { get; set; }

        public required IDbColToTypeConverter Converter { get; set; }
    }
}
