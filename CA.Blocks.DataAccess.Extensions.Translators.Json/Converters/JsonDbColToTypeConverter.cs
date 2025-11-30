using System.Text.Json;

namespace CA.Blocks.DataAccess.Extensions.Translators.Json.Converters
{


    /// <summary>
    /// This is a instance of the GeneralJsonDbColToTypeConverter with the factory set as new T()
    /// </summary>
    public class JsonDbColToTypeConverter<T> : GeneralJsonDbColToTypeConverter<T> where T : new()
    {
        public JsonDbColToTypeConverter(JsonSerializerOptions options) : base(options, () => new T())
        {
 
        }

    }

#if NET6_0_OR_GREATER
#nullable enable
        // We have to be using C# 7.3 + to use nullable reference types 

        /// <summary>
        /// This is a instance of the GeneralJsonDbColToTypeConverter with the factory set as default will support nullable types on C# 7.3 +
        /// </summary>   
        public class NullJsonDbColToTypeConverter<T> : GeneralJsonDbColToTypeConverter<T?>
        {
             public NullJsonDbColToTypeConverter(JsonSerializerOptions options) : base(options, () => default)
            {
 
            }
        }
#nullable restore
#endif
}