using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace CA.Blocks.DataAccess.DataTableHelpers
{

    public static class DataTableHelpers
    {
        private static void PopulateValueRowFrom<T>(DataRow target, T source) 
        {
            if (source == null)
            {
                target[0] = DBNull.Value;
            }
            else
            {
                target[0] = source;
            }
            
        }

        private static void SetupValueDataTableColumns(DataTable target, Type type)
        {
            var nullableType = Nullable.GetUnderlyingType(type);
            if (nullableType != null)
            {
                var dc = target.Columns.Add("Value", nullableType);
                dc.AllowDBNull = true;
            }
            else
            {
                target.Columns.Add("Value", type);
            }
        }

        private static void PopulateObjectRowFrom<T>(DataRow target, T source)
        {

            var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var pi in propertyInfos)
            {
                if (pi.CanRead)
                {
                    // // create test on allowed types we only deal with simple types // need to be more specific on this on we not deal
                    if (pi.PropertyType.IsGenericParameter || pi.PropertyType.IsArray)
                    {
                        continue;
                    }
                    var value = pi.GetValue(source);
                    target[pi.Name] = value ?? DBNull.Value;
                }
            }
        }

        private static void SetupObjectDataTableColumns(DataTable target, Type type)
        {
            var propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var pi in propertyInfos)
            {
                if (pi.CanRead)
                {
                    // // create test on allowed types we only deal with simple types 
                    if (pi.PropertyType.IsGenericParameter || pi.PropertyType.IsArray)
                    {
                        continue;
                    }
                    var nullableType = Nullable.GetUnderlyingType(pi.PropertyType);
                    if (nullableType != null)
                    {
                        var dc = target.Columns.Add(pi.Name, nullableType);
                        dc.AllowDBNull = true;
                    }
                    else
                    {
                        target.Columns.Add(pi.Name, pi.PropertyType);
                    }
                }

            }
        }

        public static DataTable ToObjectDataTable<T>(this IEnumerable<T> input,
            Action<DataTable, Type> setupDataTableColumns,
            Action<DataRow, T> populateRowFrom
        )
        {
            var result = new DataTable("data");
            setupDataTableColumns(result, typeof(T));
            result.AcceptChanges();
            foreach (var value in input)
            {
                var row = result.NewRow();
                populateRowFrom(row, value);
                result.Rows.Add(row);
            }
            result.AcceptChanges();
            return result;
        }

        // Overload to ToObjectDataTable when calling code does not need to know typeof(T) 
        public static DataTable ToObjectDataTable<T>(this IEnumerable<T> input,
            Action<DataTable> setupDataTableColumns,
            Action<DataRow, T> populateRowFrom
        )
        {
            var result = new DataTable("data");
            setupDataTableColumns(result);
            result.AcceptChanges();
            foreach (var value in input)
            {
                var row = result.NewRow();
                populateRowFrom(row, value);
                result.Rows.Add(row);
            }
            result.AcceptChanges();
            return result;
        }

        public static DataTable ToObjectDataTable<T>(this IEnumerable<T> input)
        {
            return ToObjectDataTable(input, SetupObjectDataTableColumns, PopulateObjectRowFrom);
        }

        /// <summary>
        /// When working with parameters, some providers they allow you to send in a dataTable as a parameter,
        /// this is useful when working on bulk operations, or passing in sets of values as a parameter 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static DataTable ToValueDataTable<T>(this IEnumerable<T> input)
        {
            return ToObjectDataTable(input, SetupValueDataTableColumns, PopulateValueRowFrom);
        }
    }
}
