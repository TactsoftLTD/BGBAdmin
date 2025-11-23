using System.Threading.Tasks;
using System.Web.Mvc;

namespace IDIMAdmin.Controllers
{
	public class ContactUsController : Controller
    {
        public async Task<ActionResult> Index()
        {
            return View();
        }
    }
}