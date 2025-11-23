using System.Web.Mvc;

using IDIMAdmin.Services.User;

using Microsoft.Reporting.WebForms;

namespace IDIMAdmin.Controllers.Setup
{
	public class ReportController : BaseController
    {
        protected IActivityLogService ActivityLogService { get; set; }
        public ReportController(IActivityLogService activityLogService) : base(activityLogService)
        {
            ActivityLogService = activityLogService;
        }
        // GET: Report
        public ActionResult Index()
        {
            //byte[] data= {};
            //return new ReportResult(data,"", new ReportConfig());

            LocalReport report = new LocalReport();
            report.ReportPath = Server.MapPath("~/TestReport.rdlc");

            report.DataSources.Clear();
            report.DataSources.Add(new ReportDataSource());

            report.Refresh();

            return View();
        }
    }
}