using IDIMAdmin.Extentions;
using IDIMAdmin.Extentions.Session;
using IDIMAdmin.Models.Admin;
using IDIMAdmin.Models.User;
using IDIMAdmin.Services.User;

using Microsoft.Reporting.WebForms;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Routing;

namespace IDIMAdmin.Controllers
{
	public class BaseController : Controller
	{
		protected IActivityLogService ActivityLogService { get; set; }
		public BaseController(IActivityLogService activityLogService)
		{
			ActivityLogService = activityLogService;
		}
		public FileContentResult RenderReport(ReportConfig reportConfig)
		{
			var reportDataSource = new ReportDataSource(reportConfig.ReportSourceName, reportConfig.DataTable);
			return RenderReport(reportConfig, reportDataSource);
		}

		public FileContentResult RenderReport(ReportConfig reportConfig, ReportDataSource reportDataSource)
		{
			reportConfig.FileName += $"_{DateTime.UtcNow.AddHours(6):dMMMyyyy_hhmmtt}";

			LocalReport localReport = new LocalReport { ReportPath = reportConfig.ReportFilePath };
			localReport.DataSources.Add(reportDataSource);
			// string reportType = "PDF";
			string mimeType;
			string encoding;
			string fileNameExtension;
			string deviceInfo = GetDeviceInfo(reportConfig);
			Warning[] warnings;
			string[] streams;

			//Render the report
			var renderedBytes = localReport.Render(
				reportConfig.ReportType,
				deviceInfo,
				out mimeType,
				out encoding,
				out fileNameExtension,
				out streams,
				out warnings);

			Response.AddHeader("content-disposition", "attachment; filename=" + reportConfig.FileName + "." + fileNameExtension);
			return File(renderedBytes, mimeType);
		}

		private static string GetDeviceInfo(ReportConfig reportConfig)
		{
			//The DeviceInfo settings should be changed based on the reportType
			//http://msdn.microsoft.com/en-us/library/ms155397.aspx
			return $@"<DeviceInfo>
                             <OutputFormat>PDF</OutputFormat>
							 <PageWidth>{(reportConfig.IsPortrait ? "8.27in" : "11.69in")}</PageWidth>
							 <PageHeight>{(reportConfig.IsPortrait ? "11.69in" : "8.27in")}</PageHeight>
							 <MarginTop>0.5in</MarginTop>
							 <MarginLeft>.5in</MarginLeft>
							 <MarginRight>.27in</MarginRight>
							 <MarginBottom>0.2in</MarginBottom>
							 </DeviceInfo>";

		}

		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			var controller = Request.RequestContext.RouteData.Values["controller"].ToString().ToLower();
			var userInformation = UserExtention.Get<UserInformation>(nameof(UserInformation));

			if (userInformation == null)
			{
				filterContext.Result = RedirectToLogin(filterContext);
				return;
			}

			if (userInformation != null && DefaultData.OtpEnable && !userInformation.PinCodeValidate)
			{
				filterContext.Result = RedirectToPin(filterContext);
				return;
			}

			if (userInformation.Applications == null || userInformation.Applications.All(app => app.ApplicationId != userInformation.ApplicationId))
			{
				filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Forbidden, "no permission provide.");
				return;
			}

			var menus = userInformation?.Menus.SelectMany(e => e.Menus).ToList();

			if (menus.Any(e => e.ControllerName.ToLower() == controller) ||
				DefaultData.ExcludeMenu.Any(e => e.ToLower() == controller))
			{
				return;
			}
			filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Forbidden, "no permission provide.");

			filterContext.Result = RedirectToLogin(filterContext);
		}

		protected override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			var userInformation = UserExtention.Get<UserInformation>(nameof(UserInformation));

			var formData = new Dictionary<string, string>();
			foreach (string key in Request.Form.AllKeys)
			{
				formData[key] = Request.Form[key];
			}
			string jsonData = JsonConvert.SerializeObject(formData);

			var activityModel = new ActivityLogVm()
			{
				ApplicationId = DefaultData.ApplicationId,
				UserRoleId = null,
				SessionId = Request.RequestContext.HttpContext.Session.SessionID,
				Controller = Request.RequestContext.RouteData.Values["controller"].ToString(),
				Action = Request.RequestContext.RouteData.Values["action"].ToString(),
				Url = Request.Url.ToString(),
				RequestType = HttpContext.Request.RequestType,
				ActivityData = jsonData,
				Agent = Request.RequestContext.HttpContext.Request.UserHostAddress,
				Browser = Request.Browser.Browser,
				ActivityTime = DateTime.Now,
			};

			if (userInformation != null)
			{
				activityModel.UserId = userInformation.UserId;
			}

			ActivityLogService.InsertAsync(activityModel);
		}
		private ActionResult RedirectToLogin(ControllerContext context)
		{
			return new RedirectToRouteResult
			(
				new RouteValueDictionary
				{
					{ "controller", "Login" },
					{ "action", "Index" },
					{ "r", context.HttpContext.Request.Url?.GetComponents(UriComponents.PathAndQuery, UriFormat.SafeUnescaped) }
				}
			);
		}

		private ActionResult RedirectToPin(ControllerContext context)
		{
			return new RedirectToRouteResult
			(
				new RouteValueDictionary
				{
					{ "controller", "Login" },
					{ "action", "Pin" },
					{ "r", context.HttpContext.Request.Url?.GetComponents(UriComponents.PathAndQuery, UriFormat.SafeUnescaped) }
				}
			);
		}
	}
}