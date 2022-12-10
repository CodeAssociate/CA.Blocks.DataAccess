using System;
using System.Collections.Generic;
using System.Data;

namespace CA.Blocks.DataAccess.DataTableHelpers
{

    public static class DataTableHelpers
    {
        private static void PopulateValueRowFrom<T>(DataRow target, T source)
        {
            target[0] = source;
        }

        private static void SetupValueDataTableColumns(DataTable target, Type type)
        {
            target.Columns.Add("Value", type);
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



        /// <summary>
        /// When working with parameters, some providers they allow you to send in a datatable as a parameter,
        /// this is useful when working on bulk operations, or passing in sets of values as a parameter 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static DataTable ToValueDataTable<T>(this IEnumerable<T> input)
        {
            return ToObjectDataTable(input, SetupValueDataTableColumns, PopulateValueRowFrom);
        }

        // TODO Create the generic method for SetupValueDataTableColumns and PopulateValueRowFrom so that we can pass in all read parameters




    }
}
