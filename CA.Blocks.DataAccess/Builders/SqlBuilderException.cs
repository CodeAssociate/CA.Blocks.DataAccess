#if NET6_0_OR_GREATER

using System;

namespace CA.Blocks.DataAccess.Builders
{
    public class SqlBuilderException : ApplicationException
    {
        public SqlBuilderException(string message) : base(message)
        {
        }
    }
}

#endif