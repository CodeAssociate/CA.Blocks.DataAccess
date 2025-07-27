namespace CA.Blocks.DataAccess.Translator.DbRowToObject.Interfaces
{
    public interface IDbRowTranslatorProvider
    {
        IDbRowTranslator<T> Resolve<T>(string byName = "");

        bool HasTranslatorFor<T>(string byName = "");
    }
}
