using System;
using System.Linq;

namespace IDIMAdmin.Extentions
{
	public static class DomainExtention
    {
        /// <summary>
        /// Get the username from a domain username
        /// </summary>
        /// <param name="value">generic string selector</param>
        /// <returns></returns>
        public static string ToUsername(this string value)
        {
            if (value.Contains('@'))
            {
                return value.Split('@')[0];
            }

            return value.Contains('\\') ? value.Split('\\')[1] : value;
        }
    }
}