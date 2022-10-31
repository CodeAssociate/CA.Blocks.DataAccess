using System.Data;
namespace CA.Blocks.DataAccess.Translator.DbColToType.Converters
{
    public class CharValueBoolDbColToTypeConverter : BaseDbColToTypeConverter<bool>
    {
        private readonly char _trueValue; 
        public CharValueBoolDbColToTypeConverter(char trueValue)
        {
            _trueValue = trueValue;
        }

        public override bool GetDataValue(DataRow dr, string columnName)
        {
            return dr.AsChar(columnName) == _trueValue;
        }

        public override bool GetDataValue(IDataReader dr, string columnName)
        {
            return dr.AsChar(columnName) == _trueValue;
        }

        public override bool GetDataValue(DataRow dr, int columnIndex)
        {
            return dr.AsChar(columnIndex) == _trueValue;
        }

        public override bool GetDataValue(IDataReader dr, int columnIndex)
        {
            return dr.AsChar(columnIndex) == _trueValue;
        }
    }

    public class NullCharValueDbColToTypeConverter : BaseDbColToTypeConverter<bool?>
    {
        private readonly char _trueValue;

        public NullCharValueDbColToTypeConverter(char trueValue)
        {
            _trueValue = trueValue;
        }

        private bool? ReturnValue(char? value)
        {
            if (value.HasValue)
                return value == _trueValue;
            else
                return null;
        }

        public override bool? GetDataValue(DataRow dr, string columnName)
        {
            return ReturnValue(dr.AsNullChar(columnName));
  
        }

        public override bool? GetDataValue(IDataReader dr, string columnName)
        {
            return ReturnValue(dr.AsNullChar(columnName));
        }

        public override bool? GetDataValue(DataRow dr, int columnIndex)
        {
            return ReturnValue(dr.AsNullChar(columnIndex));
        }

        public override bool? GetDataValue(IDataReader dr, int columnIndex)
        {
            return ReturnValue(dr.AsNullChar(columnIndex));
        }
    }
}
