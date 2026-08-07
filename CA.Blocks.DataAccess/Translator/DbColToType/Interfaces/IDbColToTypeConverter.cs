using System.Data;

namespace CA.Blocks.DataAccess.Translator.DbColToType.Interfaces
{
    public interface IDbColToTypeConverter<T>
    {
        T? GetDataValue(DataRow dr, string columnName);

        T? GetDataValue(IDataReader dr, string columnName);

        T? GetDataValue(DataRow dr, int columnIndex);

        // we do both DataRow and IDataReader so that we can switch between IdataReader and DataTable 
        T? GetDataValue(IDataReader dr, int columnIndex);
    }

    public interface IDbColToTypeConverter
    {
        object? GetData(DataRow dr, string columnName);

        // we do both DataRow and IDataReader so that we can switch between IdataReader and DataTable 
        object? GetData(IDataReader dr, string columnName);

        object? GetData(DataRow dr, int columnIndex);

        // we do both DataRow and IDataReader so that we can switch between IdataReader and DataTable 
        object? GetData(IDataReader dr, int columnIndex);
    }
}
