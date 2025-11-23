using System.Threading.Tasks;
using System.Web.Mvc;
using IDIMAdmin.Extentions.Collections.Select2;
using IDIMAdmin.Models.Setup;
using IDIMAdmin.Services.Setup;
using IDIMAdmin.Services.User;

namespace IDIMAdmin.Controllers.Setup
{
    public class GeneralInformationController : BaseController
    {
        protected IGeneralInformationService GeneralInformationService { get; set; }
        public GeneralInformationController(IGeneralInformationService generalInformationService,
            IActivityLogService activityLogService) : base(activityLogService)
        {
            GeneralInformationService = generalInformationService;
            ActivityLogService = activityLogService;
        }

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public async Task<ActionResult> List()
        {
            var model = new GeneralInformationSearchVm
            {
                GeneralInformations = await GeneralInformationService.GetAllAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> List(GeneralInformationSearchVm model)
        {
            model.GeneralInformations = await GeneralInformationService.GetAllAsync(model);

            return View(model);
        }

        // GET: GeneralInformation
        public async Task<ActionResult> Get(Select2Param id)
        {
            var generalInformation = await GeneralInformationService.GetBySelect2Async(id);

            return Json(generalInformation, JsonRequestBehavior.AllowGet);
        }
    }
}