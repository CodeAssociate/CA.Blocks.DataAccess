using System;

namespace CA.Blocks.DataAccess.Translator.DbColToType.Interfaces
{
    public interface IDbColToTypeProvider
    {
        IDbColToTypeConverter Resolve<T>(string byName = "");
        IDbColToTypeConverter Resolve(Type targetType, string byName = "");
    }
}
