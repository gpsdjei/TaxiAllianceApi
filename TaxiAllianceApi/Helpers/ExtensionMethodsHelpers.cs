using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace TaxiAllianceApi
{
    public static class ExtraExtensions
    {
        struct ResultItem
        {
            public string ColumnName;
            public string ColumnValue;
        }

        public static SqlParameter WithValue(this SqlParameter parameter, object value)
        {
            parameter.Value = value;
            return parameter;
        }

        public static T ParseResult<T>(this ResultModel result)
        {
            List<ResultItem> resultItems = new();

            string[] subresult = result.Result.Split('|');
            for (int i = 0; i < subresult.Length; i++)
                resultItems.Add(new ResultItem { ColumnName = subresult[i].Split('"')[0], ColumnValue = subresult[i].Split('"')[1] });

            string subtype = "";
            T parsedResult = (T)Activator.CreateInstance(typeof(T));
            for (int i = 0; i < resultItems.Count; i++)
            {
                var item = resultItems[i];
                PropertyInfo property;

                if (string.IsNullOrEmpty(subtype))
                    property = typeof(T).GetProperty(item.ColumnName);
                else
                {
                    if (!string.IsNullOrWhiteSpace(item.ColumnValue))
                        property = typeof(T).GetProperty(subtype).PropertyType.GetProperty(item.ColumnName);
                    else
                        continue;
                }
                if (property != null)
                {
                    if (property.PropertyType == typeof(bool))
                        item.ColumnValue = Convert.ToInt32(item.ColumnValue) == 1 ? "true" : "false";

                    if (string.IsNullOrEmpty(subtype))
                        property.SetValue(parsedResult, TypeDescriptor.GetConverter(property.PropertyType).ConvertFromInvariantString(item.ColumnValue));
                    else
                    {
                        var value = typeof(T).GetProperty(subtype).GetValue(parsedResult);
                        property.SetValue(value, TypeDescriptor.GetConverter(property.PropertyType).ConvertFromInvariantString(item.ColumnValue));
                    }
                }
                else
                {
                    if (item.ColumnName.StartsWith("ID"))
                    {
                        subtype = item.ColumnName.Replace("ID_", "");
                        i--;
                    }
                }
            }
            return parsedResult;
        }
    }
}

