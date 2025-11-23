using System;
using System.Linq;
using System.Linq.Expressions;

namespace IDIMAdmin.Extentions
{
	public static class DynamicSort
    {
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source
            , string columnName, bool isAscending = true)
        {
            if (String.IsNullOrEmpty(columnName))
            {
                return source;
            }

            var parameter = Expression.Parameter(source.ElementType, "");

            var property = Expression.Property(parameter, columnName);
            var lambda = Expression.Lambda(property, parameter);

            var methodName = isAscending ? "OrderBy" : "OrderByDescending";

            Expression methodCallExpression = Expression.Call(typeof(Queryable), methodName,
                new Type[] { source.ElementType, property.Type },
                source.Expression, Expression.Quote(lambda));

            return source.Provider.CreateQuery<T>(methodCallExpression);
        }
    }
}