using CA.Blocks.DataAccess.Translator.DbRowToObject.Interfaces;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Mappings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CA.Blocks.DataAccess.Translator.DbRowToObject
{
    public class Db2ObjectTranslator<T> : IDbRowTranslator<T>
    {
        private readonly DbRowToObjectMappings _mappings;
        private readonly Func<T>  _factory;


        public Db2ObjectTranslator(DbRowToObjectMappings mappings, Func<T> factory)
        {
            _mappings = mappings;
            _factory = factory;
        }

        internal DbRowToObjectMappings Mappings => _mappings;

        public IList<T> Translate(DataTable dt)
        {
            return (from DataRow dr in dt.Rows select Translate(dr)).ToList();
        }

        public T Translate(DataRow dr)
        {
            var item = _factory();
            if (dr != null)
            {
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
            var result = _factory(); 
            if (dr is { IsClosed: false })
            {
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
            if (item == null) return;

            foreach (var mapping in _mappings.MappingSet)
            {
                object? data = mapping.Converter.GetData(dr, mapping.SourceNameName);
                var pi = item.GetType().GetProperty(mapping.DestinationName);
                if (pi != null)
                {
                    pi.SetValue(item, data, null);
                }
            }
            CustomTranslate(dr, item);
        }

        private void Translate(IDataReader dr, T item)
        {
            if (item == null) return;

            foreach (var mapping in _mappings.MappingSet)
            {
                object? data = mapping.Converter.GetData(dr, mapping.SourceNameName);
                var pi = item.GetType().GetProperty(mapping.DestinationName);
                if (pi != null)
                {
                    pi.SetValue(item, data, null);
                }
            }
            CustomTranslate(dr, item);
        }
    }
}
