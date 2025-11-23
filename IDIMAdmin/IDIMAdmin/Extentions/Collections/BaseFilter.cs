using System;
using System.ComponentModel.DataAnnotations;

namespace IDIMAdmin.Extentions.Collections
{
    public class BaseFilter
    {
        public int? UserId { get; set; }
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
        public string Status { get; set; }
        public string SearchTerm { get; set; }
        public string SortDir { get; set; }
        public string Sort { get; set; }
        public string SearchResultsMessage { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public bool ReturnAllRows { get; set; }
        public bool IsSearch { get; set; }
        public bool IsCalculateTotal { get; set; }

        public BaseFilter()
        {
            SearchTerm = string.Empty;
            Status = string.Empty;
            Sort = string.Empty;
            SortDir = string.Empty;
            Page = 1;
            PageSize = 10;
            ReturnAllRows = false;
        }
    }
}