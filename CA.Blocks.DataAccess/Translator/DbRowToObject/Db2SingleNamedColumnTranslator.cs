using System;
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
        private readonly Func<T> _defaultFactory;
        
        public Db2SingleNamedColumnTranslator(IDbColToTypeConverter converter, string columnName, Func<T> defaultFactory)
        {
            _converter = converter ?? throw new System.ArgumentNullException(nameof(converter));
            _columnName = columnName ?? throw new System.ArgumentNullException(nameof(columnName));
            _defaultFactory = defaultFactory ?? throw new System.ArgumentNullException(nameof(defaultFactory));
        }

        public IList<T> Translate(DataTable dt)
        {
            return (from DataRow dr in dt.Rows select Translate(dr)).ToList()!;
        }

        public T Translate(DataRow dr)
        {
            if (dr != null)
            {
                var item = _converter.GetData(dr, 0);
                if (item is T result)
                {
                    return (T)item;
                }
            }
            return _defaultFactory();;
        }

        public T Translate(IDataReader dr)
        {
            if (dr != null && !dr.IsClosed)
            {
                var item = _converter.GetData(dr, 0);
                if (item is T result)
                {
                    return (T)item;
                }
            }
            return _defaultFactory();
        }
    }
}