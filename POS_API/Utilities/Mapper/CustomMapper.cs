using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace POS_API.Utilities.Mapper
{
    public static class CustomMapper
    {
        public static DataTable ToDataTable<T>(this IList<T> items)
        {
            var dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type ?? throw new InvalidOperationException());
            }
            foreach (T item in items)
            {
                var values = new object[props.Length];
                for (int i = 0; i < props.Length; i++)
                {
                    //inserting property values to data-table rows
                    values[i] = props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check data-table
            return dataTable;
        }
        public static List<T> DataTableToList<T>(this DataTable table) where T : class, new()
        {
            try
            {
                var list = new List<T>();
                foreach (var row in table.AsEnumerable())
                {
                    T obj = new T();
                    foreach (var prop in obj.GetType().GetProperties())
                        try
                        {
                            var propertyInfo = obj.GetType().GetProperty(prop.Name);
                            if (propertyInfo != null)
                                propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                        }
                        catch (Exception)
                        {
                            // ignored
                        }

                    list.Add(obj);
                }
                return list;
            }
            catch
            {
                return null;
            }
        }
    }
}
