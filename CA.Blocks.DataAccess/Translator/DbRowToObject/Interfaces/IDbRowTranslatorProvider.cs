namespace CA.Blocks.DataAccess.Translator.DbRowToObject.Interfaces
{
    public interface IDbRowTranslatorProvider
    {
        IDbRowTranslator<T> Resolve<T>(string byName = "") where T : new();
    }
}
