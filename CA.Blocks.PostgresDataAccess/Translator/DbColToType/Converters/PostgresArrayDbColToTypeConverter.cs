using System.Data;
using CA.Blocks.DataAccess;
using CA.Blocks.DataAccess.Translator.DbColToType.Converters;
using Npgsql;

namespace CA.Blocks.PostgresDataAccess.Translator.DbColToType.Converters
{
    public class PostgresArrayDbColToTypeConverter<T> : BaseDbColToTypeConverter<T>
    {
        public override T GetDataValue(DataRow dr, string columnName)
        {
            // old school just try cast. If it fails, it fails.
            var obj = DataHelper.GetValueFromRow(dr, columnName);
            return (T)obj;
        }

        public override T GetDataValue(IDataReader dr, string columnName)
        {
            var pgReader = dr as NpgsqlDataReader;
            return pgReader == null
                ? throw new InvalidCastException("IDataReader is not a NpgsqlDataReader")
                : pgReader.GetFieldValue<T>(columnName);
        }

        public override T GetDataValue(DataRow dr, int columnIndex)
        {
            // old school just try cast. If it fails, it fails.
            var obj = DataHelper.GetValueFromRow(dr, columnIndex);
            return (T)obj;
        }

        public override T GetDataValue(IDataReader dr, int columnIndex)
        {
            var pgReader = dr as NpgsqlDataReader;
            return pgReader == null
                ? throw new InvalidCastException("IDataReader is not a NpgsqlDataReader")
                : pgReader.GetFieldValue<T>(columnIndex);
        }
    }

    
    //public class NullPostgresArrayDbColToTypeConverter<T> : BaseDbColToTypeConverter<Nullable<T>> where T : struct
    //{
    //    public override Nullable<T> GetDataValue(DataRow dr, string columnName)
    //    {
    //        var obj = DataHelper.GetValueFromRow(dr, columnName);
    //        if (obj == DBNull.Value)
    //        {
    //            return null;
    //        };
    //        return (T?)obj;
    //    }

    //    public override Nullable<T> GetDataValue(IDataReader dr, string columnName)
    //    {
    //        var pgReader = dr as NpgsqlDataReader;
    //        return pgReader == null
    //            ? throw new InvalidCastException("IDataReader is not a NpgsqlDataReader")
    //            : pgReader.GetFieldValue<T>(columnName);
    //    }

    //    public override Nullable<T> GetDataValue(DataRow dr, int columnIndex)
    //    {
    //        var obj = DataHelper.GetValueFromRow(dr, columnIndex);
    //        if (obj == DBNull.Value)
    //        {
    //            return null;
    //        }
    //        ;
    //        return (T?)obj;
    //    }

    //    public override Nullable<T> GetDataValue(IDataReader dr, int columnIndex)
    //    {
    //        var pgReader = dr as NpgsqlDataReader;
    //        return pgReader == null
    //            ? throw new InvalidCastException("IDataReader is not a NpgsqlDataReader")
    //            : pgReader.GetFieldValue<T>(columnIndex);
    //    }
    //}

    /*

    public class PostgresBigIntArrayDbColToTypeConverter : PostgresArrayDbColToTypeConverter<long[]>
    {
        
    }

    public class PostgresBigIntListDbColToTypeConverter : PostgresArrayDbColToTypeConverter<List<long>>
    {

    }
    
    */


}
