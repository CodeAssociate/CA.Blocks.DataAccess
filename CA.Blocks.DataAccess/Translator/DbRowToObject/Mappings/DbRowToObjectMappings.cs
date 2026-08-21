using System;
using System.Collections.Generic;
using CA.Blocks.DataAccess.Translator.DbColToType.Interfaces;
using CA.Blocks.DataAccess.Translator.DbColToType.Mappings;

namespace CA.Blocks.DataAccess.Translator.DbRowToObject.Mappings
{
    public class DbRowToObjectMappings
    {

        public IList<IDbColToTypeMapping> MappingSet { get; set; } = new List<IDbColToTypeMapping>();

        public Func<string, string> NormalizeNameFunction { get; set; } = (name) => name;

        public void AddMapping(IDbColToTypeMapping mapping)
        {
            MappingSet.Add(mapping);
        }

        public void AddOneToOneMapping(string propertyName, IDbColToTypeConverter converter)
        {

            MappingSet.Add(new DbColToTypeMapping{DestinationName = propertyName, SourceNameName = propertyName, Converter = converter});
        }

    }
}
