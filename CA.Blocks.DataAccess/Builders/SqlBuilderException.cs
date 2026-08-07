using System;

namespace CA.Blocks.DataAccess.Builders
{
    public class SqlBuilderException(string message) : ApplicationException(message);
}