//===============================================================================
// Code Associate Data Access Block for .NET Core
//
//===============================================================================
// Copyright (C) 2002-2020 Ravin Enterprises Ltd. 
// All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================


using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;

namespace CA.Blocks.DataAccess.Translator
{
    /// <summary>
    /// This returns an ExpandoObject which makes for very fast prototyping, if you know the structure you should be the dataTable Extensions 
    /// </summary>
    public class DynamicDbRow2ObjectTranslator : SimpleDbRow2ObjectTranslator<dynamic>
    {
        public static DynamicDbRow2ObjectTranslator CurrentInstance = new DynamicDbRow2ObjectTranslator();

        protected override dynamic CustomTranslate(DataRow dr)
        {
            dynamic item = new ExpandoObject();
            var d = item as IDictionary<string, object?>;
            for (int i = 0; i < dr.Table.Columns.Count; i++)
                d!.Add(dr.Table.Columns[i].ColumnName,
                    DBNull.Value.Equals(dr[i]) ? null : dr[i]);
            return item;
        }
    }
}
