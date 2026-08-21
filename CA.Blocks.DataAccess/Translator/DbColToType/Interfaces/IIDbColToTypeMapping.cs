namespace CA.Blocks.DataAccess.Translator.DbColToType.Interfaces
{
    public interface IDbColToTypeMapping
    {
        string DestinationName { get;}
        string SourceNameName { get; }
        IDbColToTypeConverter Converter { get; }
        bool NormalizeSourceName { get; }
    }
}
