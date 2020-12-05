namespace CA.Blocks.DataAccess.Translator.DbRowToObject.Interfaces
{
    public interface IDbRowToObjectProvider
    {
        IDb2ObjectTranslator<T> Resolve<T>(string byName = "") where T : new();
    }
}
