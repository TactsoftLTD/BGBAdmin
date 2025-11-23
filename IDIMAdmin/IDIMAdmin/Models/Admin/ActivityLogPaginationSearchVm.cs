using System;

namespace IDIMAdmin.Models.Admin
{
	public class ActivityLogPaginationSearchVm
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public string SearchValues { get; set; }
        public int? ApplicationId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string SortColumn { get; set; }
        public string SortDirection { get; set; }
    }
}