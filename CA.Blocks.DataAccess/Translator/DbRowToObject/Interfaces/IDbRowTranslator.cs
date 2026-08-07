using System.Collections.Generic;
using System.Data;

namespace CA.Blocks.DataAccess.Translator.DbRowToObject.Interfaces
{
    public interface IDbRowTranslator<T>
    {
        IList<T> Translate(DataTable dt);

        T? Translate(DataRow dr);

        T? Translate(IDataReader dr);
    }
}