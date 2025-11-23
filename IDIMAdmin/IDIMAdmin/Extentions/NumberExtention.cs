using System;

namespace IDIMAdmin.Extentions
{
	public static class NumberExtention
    {
        public static bool IsNumber(this string value)
        {
            return int.TryParse(value, out int id);
        }

        public static bool IsNumeric(this object o)
        {
            switch (Type.GetTypeCode(o.GetType()))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }

        public static int ToInt(this int? value)
        {
            int id;
            int.TryParse(value.ToString(), out id);

            return id;
        }
    }
}