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
        private readonly string _columnName;

        public Db2SingleNamedColumnTranslator(IDbColToTypeConverter converter, string columnName)
        {
            _converter = converter ?? throw new System.ArgumentNullException(nameof(converter));
            _columnName = columnName ?? throw new System.ArgumentNullException(nameof(columnName));
        }

        public IList<T> Translate(DataTable dt)
        {
            return (from DataRow dr in dt.Rows select Translate(dr)).ToList()!;
        }

        public T? Translate(DataRow dr)
        {
            if (dr != null)
            {
                return (T?)_converter.GetData(dr, _columnName);
            }
            return default;
        }

        public T? Translate(IDataReader dr)
        {
            if (dr != null && !dr.IsClosed)
            {
                return (T?)_converter.GetData(dr, _columnName);
            }
            return default;
        }
    }
}