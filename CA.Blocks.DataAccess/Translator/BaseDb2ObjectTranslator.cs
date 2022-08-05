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
using System.Linq;
using System.Reflection;

namespace CA.Blocks.DataAccess.Translator
{

    [System.Obsolete("This is in here for backwards compatibility only it and been replaced with more flexible provider this no longer supported.  in place of t = new Db2ObjectTranslator() use t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T>(); ")]
    public class DatabaseToObjectMappings : List<DatabaseToObjectMapping>
    {
        public void RemoveByName(string propertyName)
        {
            RemoveAll(m => m.DestinationName == propertyName);
        }
    }

    [System.Obsolete("This is in here for backwards compatibility only and has been replaced with more flexible provider.  in place of t = new BaseDb2ObjectTranslator<T>() use t = DefaultDbRowTranslatorProvider.DefaultInstance.Resolve<T>(); ")]

    public class BaseDb2ObjectTranslator<T> where T : new()
    {
        protected DatabaseToObjectMappings _mappings;
        

        #region Constructor
        public BaseDb2ObjectTranslator()
            : this(true)
        {

        }

        public BaseDb2ObjectTranslator(bool useDefault)
        {
            if (useDefault)
                GenerateDefaultMappings();
        }


        #endregion

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

        private void Translate(DataRow dr, T item)
        {
            foreach (var mapping in _mappings)
            {
                object data = mapping.GetData(dr);
                PropertyInfo pi = item.GetType().GetProperty(mapping.DestinationName);
                pi.SetValue(item, data, null);
            }
            CustomTranslate(dr, item);
        }

        protected DatabaseToObjectMapping CreateDatabaseToObjectMapping(string typeName, string destinationName, string sourceNameName, bool isNullable)
        {
            DatabaseToObjectMapping result = null;
            switch (typeName)
            {
                case "System.String":
                {
                    result = new DatabaseToObjectMappingString(destinationName, sourceNameName);
                    break;
                }
                case "System.DateTime":
                {
                    if (isNullable)
                        result = new DatabaseToObjectMappingNullDateTime(destinationName, sourceNameName);
                    else
                        result = new DatabaseToObjectMappingDateTime(destinationName, sourceNameName);
                    break;
                }
                case "System.Int64":
                {
                    if (isNullable)
                        result = new DatabaseToObjectMappingNullLong(destinationName, sourceNameName);
                    else
                        result = new DatabaseToObjectMappingLong(destinationName, sourceNameName);
                    break;
                }
                case "System.Int32":
                {
                    if (isNullable)
                        result = new DatabaseToObjectMappingNullInt(destinationName, sourceNameName);
                    else
                        result = new DatabaseToObjectMappingInt(destinationName, sourceNameName);
                    break;
                }
                case "System.Int16":
                {
                    if (isNullable)
                        result = new DatabaseToObjectMappingNullShort(destinationName, sourceNameName);
                    else
                        result = new DatabaseToObjectMappingShort(destinationName, sourceNameName);
                    break;
                }
                case "System.Byte":
                {
                    if (isNullable)
                        result = new DatabaseToObjectMappingNullByte(destinationName, sourceNameName);
                    else
                        result = new DatabaseToObjectMappingByte(destinationName, sourceNameName);
                    break;
                }
                case "System.Boolean":
                {
                    if (isNullable)
                        result = new DatabaseToObjectMappingNullBool(destinationName, sourceNameName);
                    else
                        result = new DatabaseToObjectMappingBool(destinationName, sourceNameName);
                    break;
                }
                case "System.Char":
                {
                    if (isNullable)
                        result = new DatabaseToObjectMappingNullChar(destinationName, sourceNameName);
                    else
                        result = new DatabaseToObjectMappingChar(destinationName, sourceNameName);
                    break;
                }

                case "System.TimeSpan":
                {
                    if (isNullable)
                        result = new DatabaseToObjectMappingNullTimeSpan(destinationName, sourceNameName);
                    else
                        result = new DatabaseToObjectMappingTimeSpan(destinationName, sourceNameName);
                    break;
                }
                case "System.Double":
                {

                    if (isNullable)
                        result = new DatabaseToObjectMappingNullDouble(destinationName, sourceNameName);
                    else
                        result = new DatabaseToObjectMappingDouble(destinationName, sourceNameName);
                    break;
                }
                case "System.Decimal":
                {
                    if (isNullable)
                        result = new DatabaseToObjectMappingNullDecimal(destinationName, sourceNameName);
                    else
                        result = new DatabaseToObjectMappingDecimal(destinationName, sourceNameName);
                    break;
                }
                default:
                {
                    throw new ArgumentException($"Unknown type '{typeName}' for DatabaseToObjectMapping");
                    // If you get here there is a missing  DatabaseToObjectMapping each to add in just follow the pattern above 
                }
            }
            return result;
        }

        #region  Default Mappings Domain is 1-1 with the query
        private DatabaseToObjectMapping GetDatabaseToObjectMapping(string typeName, string propertyName, bool isNullable)
        {
            return CreateDatabaseToObjectMapping(typeName, propertyName, propertyName, isNullable);
        }

        protected void GenerateDefaultMappings()
        {
            _mappings = new DatabaseToObjectMappings();
            var myObjectFields = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var pi in myObjectFields)
            {
                if (pi.CanWrite)
                {
                    if (pi.PropertyType.IsGenericType && pi.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        _mappings.Add(GetDatabaseToObjectMapping(pi.PropertyType.GetGenericArguments()[0].FullName, pi.Name, true));
                    }
                    else
                    {
                        _mappings.Add(GetDatabaseToObjectMapping(pi.PropertyType.FullName, pi.Name, false));
                    }
                }
            }
        }
        #endregion
    }
}