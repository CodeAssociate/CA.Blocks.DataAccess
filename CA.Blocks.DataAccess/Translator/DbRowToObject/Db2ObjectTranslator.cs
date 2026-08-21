using CA.Blocks.DataAccess.Translator.DbRowToObject.Interfaces;
using CA.Blocks.DataAccess.Translator.DbRowToObject.Mappings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using CA.Blocks.DataAccess.Translator.DbColToType.Exceptions;
using CA.Blocks.DataAccess.Translator.DbColToType.Interfaces;

namespace CA.Blocks.DataAccess.Translator.DbRowToObject
{
    public class Db2ObjectTranslator<T>(DbRowToObjectMappings mappings, Func<T> factory) : IDbRowTranslator<T>
    {
        // Container holding the resolved column index and PropertyInfo per mapping for this query
        private struct ResolvedMapping
        {
            public int ColumnIndex;
            public System.Reflection.PropertyInfo Property;
            public IDbColToTypeConverter Converter;

        }
        // Weak reference table keyed by the IDataReader instance itself.
        // Automatically garbage-collected when the reader is closed/disposed!
        private static readonly ConditionalWeakTable<IDataReader, ResolvedMapping[]> ReaderCache = new();

        private static readonly ConditionalWeakTable<DataTable, ResolvedMapping[]> DateTableCache = new();
        
        
        private string NormalizeName(string name)
        {
            return mappings.NormalizeNameFunction(name);
        }

        private ResolvedMapping[] ResolveMappingsForReader(IDataReader dr)
        {
            var dbColumnMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < dr.FieldCount; i++)
            {
                string normalizedCol = NormalizeName(dr.GetName(i));
                dbColumnMap[normalizedCol] = i;
            }

            var list = new List<ResolvedMapping>();
            var targetType = typeof(T);

            foreach (var mapping in mappings.MappingSet)
            {
                int ordinal = -1;
                if (!mapping.NormalizeSourceName)
                {
                    ordinal = dr.GetOrdinal(mapping.SourceNameName);
                }
                else
                {
                    string normalizedName = NormalizeName(mapping.SourceNameName);
                    if (!(dbColumnMap.TryGetValue(normalizedName, out ordinal)))
                    {
                        ordinal = -1;
                    }
                }
                if (ordinal > -1)
                {
                    var pi = targetType.GetProperty(mapping.DestinationName);
                    if (pi != null && pi.CanWrite)
                    {
                        list.Add(new ResolvedMapping
                        {
                            ColumnIndex = ordinal,
                            Property = pi,
                            Converter = mapping.Converter
                        });
                    }
                }
                else
                {
                    throw new ConverterColumnNotFoundException(
                        $"The column '{mapping.SourceNameName}' was expected in the result set but not found");
                }
            }
            return list.ToArray();
        }
        
        private void TranslateRow(IDataReader dr, T item, ResolvedMapping[] resolvedMappings)
        {
            for (int i = 0; i < resolvedMappings.Length; i++)
            {
                ref readonly var mapping = ref resolvedMappings[i];
                object? val = mapping.Converter.GetData(dr, mapping.ColumnIndex);
                mapping.Property.SetValue(item, val, null);
            }
            CustomTranslate(dr, item);
        }
        
        private ResolvedMapping[] ResolveMappingsForDataTable(DataTable dt)
        {
            var dbColumnMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                string normalizedCol = NormalizeName(dt.Columns[i].ColumnName);
                dbColumnMap[normalizedCol] = i;
            }

            var list = new List<ResolvedMapping>();
            var targetType = typeof(T);

            foreach (var mapping in mappings.MappingSet)
            {
                int ordinal = -1;
                if (!mapping.NormalizeSourceName)
                {
                    ordinal = dt.Columns.IndexOf(mapping.SourceNameName);
                }
                else
                {
                    string normalizedName = NormalizeName(mapping.SourceNameName);
                    if (!(dbColumnMap.TryGetValue(normalizedName, out ordinal)))
                    {
                        ordinal = -1;
                    }
                }
                if (ordinal > -1)
                {
                    var pi = targetType.GetProperty(mapping.DestinationName);
                    if (pi != null && pi.CanWrite)
                    {
                        list.Add(new ResolvedMapping
                        {
                            ColumnIndex = ordinal,
                            Property = pi,
                            Converter = mapping.Converter
                        });
                    }
                }
                else
                {
                    throw new ConverterColumnNotFoundException(
                        $"The column '{mapping.SourceNameName}' was expected in the result set but not found");
                }
            }
            return list.ToArray();
        }
       
        private void TranslateRow(DataRow dr, T item, ResolvedMapping[] resolvedMappings)
        {
            for (int i = 0; i < resolvedMappings.Length; i++)
            {
                ref readonly var mapping = ref resolvedMappings[i];
                object? val = mapping.Converter.GetData(dr, mapping.ColumnIndex);
                mapping.Property.SetValue(item, val, null);
            }
            CustomTranslate(dr, item);
        }
        
        
        
        public IList<T> Translate(DataTable dt)
        {
            return (from DataRow dr in dt.Rows select Translate(dr)).ToList();
        }

        public T Translate(DataRow dr)
        {
            var item = factory();
            if (dr != null)
            {
                // Retrieve or create mapping for THIS specific DateTable instance
                var resolvedMappings = DateTableCache.GetValue(dr.Table, ResolveMappingsForDataTable);
                TranslateRow(dr, item, resolvedMappings);
            }
            return item;
        }

        protected virtual void CustomTranslate(DataRow dr, T item)
        {

        }
        #region DataReader


        public T Translate(IDataReader dr)
        {
            var result = factory(); 
            if (dr is { IsClosed: false })
            {
                // Retrieve or create mapping for THIS specific reader instance
                var resolvedMappings = ReaderCache.GetValue(dr, ResolveMappingsForReader);
                TranslateRow(dr, result, resolvedMappings);
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

            foreach (var mapping in mappings.MappingSet)
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
