using System.Data;
using CA.Blocks.DataAccess.Translator.DbColToType.Converters;
using NUlid;

namespace CA.Blocks.DataAccess.Extensions.Translators.NUlid.DbColToType.Converters
{
    public class UlidDbColToTypeConverter : BaseDbColToTypeConverter<Ulid>
    {
        public override Ulid GetDataValue(DataRow dr, string columnName)
        {
            return dr.AsUlid(columnName);
        }

        public override Ulid GetDataValue(IDataReader dr, string columnName)
        {
            return dr.AsUlid(columnName);
        }

        public override Ulid GetDataValue(DataRow dr, int columnIndex)
        {
            return dr.AsUlid(columnIndex);
        }

        public override Ulid GetDataValue(IDataReader dr, int columnIndex)
        {
            return dr.AsUlid(columnIndex);
        }
    }

    public class NullUlidDbColToTypeConverter : BaseDbColToTypeConverter<Ulid?>
    {
        public override Ulid? GetDataValue(DataRow dr, string columnName)
        {
            return dr.AsNullUlid(columnName);
        }

        public override Ulid? GetDataValue(IDataReader dr, string columnName)
        {
            return dr.AsNullUlid(columnName);
        }

        public override Ulid? GetDataValue(DataRow dr, int columnIndex)
        {
            return dr.AsNullUlid(columnIndex);
        }

        public override Ulid? GetDataValue(IDataReader dr, int columnIndex)
        {
            return dr.AsNullUlid(columnIndex);
        }
    }

}
