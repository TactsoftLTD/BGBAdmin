using System;
using System.Threading.Tasks;
using System.Web.Mvc;

using IDIMAdmin.Services.Setup;
using IDIMAdmin.Services.User;

namespace IDIMAdmin.Controllers.Setup
{
	public class UnitController : BaseController
    {
        protected IUnitService UnitService { get; set; }
        protected IActivityLogService ActivityLogService { get; set; }

        public UnitController(IUnitService unitService,
            IActivityLogService activityLogService) : base(activityLogService)
        {
            UnitService = unitService;
            ActivityLogService = activityLogService;
        }

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public async Task<ActionResult> List()
        {
            var list = await UnitService.GetAllAsync();

            return View(list);
        }

        public ActionResult Get()
        {
            return View();
        }

        /// <summary>
        /// get battalion units for specific sector
        /// </summary>
        /// <param name="id">sector identifier</param>
        /// <returns></returns>
        public async Task<ActionResult> GetBattalionUnit(int? id)
        {
            int? sectorId = null;
            var sectorUnit = await UnitService.GetByIdAsync(Int32.Parse(id.ToString()));
            if (sectorUnit != null)
                sectorId = sectorUnit.SectorId;

            var units = await UnitService.GetBattalionDropdownAsync(id: sectorId);

            var data = new
            {
                data = units
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}