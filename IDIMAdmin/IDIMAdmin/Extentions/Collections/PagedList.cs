using System.Collections.Generic;

namespace IDIMAdmin.Extentions.Collections
{
    public class PagedList<T> : IPagedList<T> where T : class
    {
        public List<T> Items { get; set; }
        public int Total { get; set; }

        public PagedList()
        {

        }

        public PagedList(List<T> items, int total)
        {
            Items = items;
            Total = total;
        }

        public PagedList(IPagedList<T> list)
        {
            Items = list.Items;
            Total = list.Total;
        }
    }
}
