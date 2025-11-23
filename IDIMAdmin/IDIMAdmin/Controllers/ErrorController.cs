using System.Web.Mvc;

namespace IDIMAdmin.Controllers
{
	public class ErrorController : Controller
    {
        public ViewResult Index()
        {
            return View("Error");
        }

        public ViewResult NotFound()
        {
            Response.StatusCode = 404;
            return View();
        }

        public ViewResult Forbidden()
        {
            Response.StatusCode = 403;
            return View();
        }

        public ViewResult NotAllowed()
        {
            Response.StatusCode = 405;
            return View();
        }

        public ActionResult NonAuthoriative()
        {
            Response.StatusCode = 501;
            return View();
        }
    }
}