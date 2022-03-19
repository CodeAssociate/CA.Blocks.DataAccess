using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Interfaces;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Mappings;

namespace CA.Blocks.DataAccess.Translator.DbRowToObject
{
    public class Db2ObjectTranslator<T> : IDbRowTranslator<T> where T : new()
    {
        private DbRowToObjectMappings _mappings;

        public Db2ObjectTranslator(DbRowToObjectMappings mappings)
        {
            _mappings = mappings;
        }

        public IList<T> Translate(DataTable dt)
        {
            return (from DataRow dr in dt.Rows select Translate(dr)).ToList();
        }

        public T Translate(DataRow dr)
        {
            T item = default(T);
            if (dr != null)
            {
                item = new T();
                Translate(dr, item);
            }
            return item;
        }

        protected virtual void CustomTranslate(DataRow dr, T item)
        {

        }


        #region DataReader


        public T Translate(IDataReader dr)
        {
            T result = default(T);
            if (dr != null && !dr.IsClosed)
            {
                result = new T();
                Translate(dr, result);
            }
            return result;
        }


        protected virtual void CustomTranslate(IDataReader dr, T item)
        {
        }
        #endregion

        private void Translate(DataRow dr, T item)
        {
            foreach (var mapping in _mappings.MappingSet)
            {
                object data = mapping.Converter.GetData(dr, mapping.SourceNameName);
                PropertyInfo pi = item.GetType().GetProperty(mapping.DestinationName);
                pi.SetValue(item, data, null);
            }
            CustomTranslate(dr, item);
        }

        private void Translate(IDataReader dr, T item)
        {
            foreach (var mapping in _mappings.MappingSet)
            {
                object data = mapping.Converter.GetData(dr, mapping.SourceNameName);
                PropertyInfo pi = item.GetType().GetProperty(mapping.DestinationName);
                pi.SetValue(item, data, null);
            }
            CustomTranslate(dr, item);
        }
    }
}
