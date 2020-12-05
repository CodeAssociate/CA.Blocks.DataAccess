using System.Data;
using CA.Blocks.DataAccess.Translator.DbColToType.Interfaces;

namespace CA.Blocks.DataAccess.Translator.DbColToType.Mappings
{
    public class DbColToTypeMapping : IDbColToTypeMapping
    {
        public string DestinationName { get; set; }
        public string SourceNameName { get; set; }

        public IDbColToTypeConverter Converter { get; set; }
    }
}
