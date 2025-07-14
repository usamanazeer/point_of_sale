using System;
using System.Globalization;

namespace Models
{
    public static class ExtensionMethods
    {
        public static string ToString(this DateTime? date, string format) => date == null ? "" : Convert.ToDateTime(date).ToString(format);
        public static int ToInt(this Enum enumVal) => Convert.ToUInt16(enumVal);
        public static Exception ToException(this string message) => new Exception(message);

        public static float ToNDecimalPlaces(this float val, int n)
        {
            var stringVal = val.ToString(CultureInfo.InvariantCulture);
            var newLen = stringVal.IndexOf('.') + (n + 1);
            if (newLen > stringVal.Length || !stringVal.Contains('.')) return val;
            var newStringVal = stringVal.Substring(0, newLen);
            return Convert.ToSingle(newStringVal);
        }
        public static decimal ToNDecimalPlaces(this decimal val, int n)
        {
            var stringVal = val.ToString(CultureInfo.InvariantCulture);
            var newLen = stringVal.IndexOf('.') + (n + 1);
            if (newLen > stringVal.Length || !stringVal.Contains('.'))
                return val;
            var newStringVal = stringVal.Substring(0, newLen);
            return Convert.ToDecimal(newStringVal);
        }
        //public static decimal ToNDecimalPlaces(this decimal val, int n)
        //{
        //    var stringVal = val.ToString(CultureInfo.InvariantCulture);
        //    var newLen = stringVal.IndexOf('.') + (n + 1);
        //    if (newLen > stringVal.Length || !stringVal.Contains('.')) return val;
        //    var newStringVal = stringVal.Substring(0,newLen);
        //    return Convert.ToDecimal(newStringVal);
        //}
    }
}
