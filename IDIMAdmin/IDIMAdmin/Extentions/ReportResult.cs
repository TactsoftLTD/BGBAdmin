using System;
using System.Web.Mvc;

using Microsoft.Reporting.WebForms;

using Warning = Microsoft.Reporting.WebForms.Warning;

namespace IDIMAdmin.Extentions
{
	public class ReportResult : FileContentResult
    {
        private ReportConfig ReportConfig { get; }
       
        public ReportResult(byte[] fileContents, string contentType, ReportConfig reportConfig) : base(fileContents, contentType)
        {
            ReportConfig = reportConfig;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            ReportDataSource reportDataSource = new ReportDataSource();
            ReportConfig.FileName += "_" + $"{DateTime.UtcNow.AddHours(6):dMMMyyyy_hhmmtt}";

            LocalReport localReport = new LocalReport { ReportPath = ReportConfig.ReportFilePath };
            localReport.DataSources.Add(reportDataSource);
            // string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;

            //The DeviceInfo settings should be changed based on the reportType
            //http://msdn.microsoft.com/en-us/library/ms155397.aspx
            string deviceInfo;

            if (ReportConfig.IsPortrait)
            {
                deviceInfo = "<DeviceInfo>" +
                             "  <OutputFormat>PDF</OutputFormat>" +
                             "  <PageWidth>8.27in</PageWidth>" +
                             "  <PageHeight>11.69in</PageHeight>" +
                             "  <MarginTop>0.5in</MarginTop>" +
                             "  <MarginLeft>.5in</MarginLeft>" +
                             "  <MarginRight>.27in</MarginRight>" +
                             "  <MarginBottom>0.2in</MarginBottom>" +
                             "</DeviceInfo>";
            }
            else
            {
                deviceInfo = "<DeviceInfo>" +
                             "  <OutputFormat>PDF</OutputFormat>" +
                             "  <PageWidth>11.69in</PageWidth>" +
                             "  <PageHeight>8.27in</PageHeight>" +
                             "  <MarginTop>0.5in</MarginTop>" +
                             "  <MarginLeft>.5in</MarginLeft>" +
                             "  <MarginRight>.27in</MarginRight>" +
                             "  <MarginBottom>0.2in</MarginBottom>" +
                             "</DeviceInfo>";
            }

            Warning[] warnings;
            string[] streams;

            //Render the report
            var renderedBytes = localReport.Render(
                ReportConfig.ReportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            var response = context.HttpContext.Response;
            response.ContentType = ContentType;
            response.AddHeader("content-disposition", "attachment; filename=" + ReportConfig.FileName);
            WriteFile(response);
        }

       
    }
}