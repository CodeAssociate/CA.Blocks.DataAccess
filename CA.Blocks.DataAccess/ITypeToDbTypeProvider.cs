using System;

namespace CA.Blocks.DataAccess
{
    public interface ITypeToDbTypeProvider<T>
    {
        void TryAdd(Type type, T sqlDbType, string specificType = "", bool errorOnExists = false);
        T Resolve(Type type, string? byName = "");
    }
}
