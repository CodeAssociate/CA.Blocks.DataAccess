using System.Collections.Generic;
using System.Data;
using System.Linq;
using CA.Blocks.DataAccess.Translator.DbColToType.Interfaces;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Interfaces;

namespace CA.Blocks.DataAccess.Translator.DbRowToObject
{
    public class Db2SingleNamedColumnTranslator<T> : IDbRowTranslator<T> 
    {
        private readonly IDbColToTypeConverter _converter;
        private string _columnName;

        public Db2SingleNamedColumnTranslator(IDbColToTypeConverter converter, string columnName)
        {
            _converter = converter;
            _columnName = columnName;
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
                item = (T)_converter.GetData(dr, _columnName);
            }
            return item;
        }

        public T? Translate(IDataReader dr)
        {
            var result = default(T);
            if (dr != null && !dr.IsClosed)
            {
                result = (T)_converter.GetData(dr, _columnName);
            }
            return result;
        }
    }
}