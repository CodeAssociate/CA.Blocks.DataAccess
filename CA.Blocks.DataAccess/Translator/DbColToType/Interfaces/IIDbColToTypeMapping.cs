using System.Data;

namespace CA.Blocks.DataAccess.Translator.DbColToType.Interfaces
{
    public interface IDbColToTypeMapping
    {
        string DestinationName { get; set; }
        string SourceNameName { get; set; }
        IDbColToTypeConverter Converter { get; set; }
    }
}
