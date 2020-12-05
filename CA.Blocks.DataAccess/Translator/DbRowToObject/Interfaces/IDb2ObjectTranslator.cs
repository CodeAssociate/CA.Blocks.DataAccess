using System.Collections.Generic;
using System.Data;

namespace CA.Blocks.DataAccess.Translator.DbRowToObject.Interfaces
{
    public interface IDb2ObjectTranslator<T>
    {
        IList<T> Translate(DataTable dt);

        T Translate(DataRow dr);
    }
}