using System;

namespace CA.Blocks.DataAccess.Translator.DbColToType.Exceptions
{
    public class ConverterColumnBadDataException : Exception
    {
        public ConverterColumnBadDataException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}