using System.Data;
using CA.Blocks.DataAccess.Translator.DbColToType.Interfaces;

namespace CA.Blocks.DataAccess.Translator.DbColToType.Converters
{
    public abstract class BaseDbColToTypeConverter<T> : IDbColToTypeConverter<T>, IDbColToTypeConverter
    {
        public abstract T GetDataValue(DataRow dr, string columnName);
        public abstract T GetDataValue(IDataReader dr, string columnName);

        public object GetData(DataRow dr, string columnName)
        {
            return GetDataValue(dr, columnName);
        }
        public object GetData(IDataReader dr, string columnName)
        {
            return GetDataValue(dr, columnName);
        }
    }
}
