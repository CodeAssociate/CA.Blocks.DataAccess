using System.Collections.Generic;
using System.Data;
using System.Linq;
using CA.Blocks.DataAccess.Translator.DbColToType.Interfaces;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Interfaces;

namespace CA.Blocks.DataAccess.Translator.DbRowToObject
{
    public class Db2SingleColumnTranslator<T> : IDbRowTranslator<T>
    {
        private IDbColToTypeConverter _converter;

        public Db2SingleColumnTranslator(IDbColToTypeConverter converter)
        {
            _converter = converter;
        }

        public IList<T> Translate(DataTable dt)
        {
            return (from DataRow dr in dt.Rows select Translate(dr)).ToList();
        }

        public T? Translate(DataRow dr)
        {
            var item = default(T);
            if (dr != null)
            {
                item = (T)_converter.GetData(dr, 0);
            }
            return item;
        }

        public T? Translate(IDataReader dr)
        {
            T result = default(T);
            if (dr != null && !dr.IsClosed)
            {
                result = (T)_converter.GetData(dr, 0);
            }
            return result;
        }
    }
}