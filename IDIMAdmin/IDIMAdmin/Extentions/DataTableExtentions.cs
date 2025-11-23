using System.Linq;

using DataTables.Mvc;

namespace IDIMAdmin.Extentions
{
	public static class DataTableExtentions
    {
        public static IQueryable<T> Sorting<T>(this IQueryable<T> query, IDataTablesRequest request) where T : class
        {
            var sortedColumns = request.Columns.GetSortedColumns();

            foreach (var column in sortedColumns)
            {
                query = query.OrderBy(column.Data, column.SortDirection == Column.OrderDirection.Ascendant);
            }

            //query = query.OrderBy(orderByString == string.Empty ? "BattalionIssueId asc" : orderByString);

            return query;
        }

        public static IQueryable<T> Paging<T>(this IQueryable<T> query, IDataTablesRequest request) where T : class
        {
            return query.Skip(request.Start).Take(request.Length);
        }
    }
}