using System.Collections.Generic;

namespace IDIMAdmin.Extentions.Collections
{
    public interface IPagedList<T>
    {
        List<T> Items { get; set; }
        int Total { get; set; }
    }
}
