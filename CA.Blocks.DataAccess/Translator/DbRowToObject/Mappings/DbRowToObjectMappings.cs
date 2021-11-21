using System;
using System.Collections.Generic;
using CA.Blocks.DataAccess.Translator.DbColToType.Interfaces;
using CA.Blocks.DataAccess.Translator.DbColToType.Mappings;

namespace CA.Blocks.DataAccess.Translator.DbRowToObject.Mappings
{
    public class DbRowToObjectMappings
    {

        public IList<IDbColToTypeMapping> MappingSet { get; set; }

        public DbRowToObjectMappings()
        {
            MappingSet = new List<IDbColToTypeMapping>();
        }


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
