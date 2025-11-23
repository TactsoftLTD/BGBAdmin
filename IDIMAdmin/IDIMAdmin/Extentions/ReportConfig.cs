using System.Data;

namespace IDIMAdmin.Extentions
{
	public class ReportConfig
    {
        public string FileName { get; set; }
        public DataTable DataTable { get; set; }
        public string ReportFilePath { get; set; }
        public string ReportSourceName { get; set; }
        public string ReportType { get; set; } // = "pdf",
        public string PageSize { get; set; } // = "a4",
        public bool IsPortrait { get; set; } // = true

        public ReportConfig()
        {
            ReportType = "pdf";
            PageSize = "a4";
            IsPortrait = true;
        }
    }
}