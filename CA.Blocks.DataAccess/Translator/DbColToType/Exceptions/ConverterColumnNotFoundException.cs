using System;

namespace CA.Blocks.DataAccess.Translator.DbColToType.Exceptions
{

    public class ConverterColumnNotFoundException : Exception
    {

        public ConverterColumnNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
