using System.Web.Mvc;

using IDIMAdmin.Models.User;
using IDIMAdmin.Services.User;

namespace IDIMAdmin.Controllers.User
{
	public class DeviceController : BaseController
    {
        protected IDeviceService DeviceService { get; set; }
        protected IUserService UserService { get; set; }
        protected IActivityLogService ActivityLogService { get; set; }

        public DeviceController(IDeviceService deviceService,
            IUserService userService,
            IActivityLogService activityLogService) : base(activityLogService)
        {
            DeviceService = deviceService;
            UserService = userService;
            ActivityLogService = activityLogService;
        }

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        //public async Task<ActionResult> List()
        //{
        //    var devices = await DeviceService.GetAllAsync();

        //    return View(devices);
        //}

        //public async Task<ActionResult> Detail(int? id)
        //{
        //    int.TryParse(id.ToString(), out int deviceId);
        //    var device = await DeviceService.GetByIdAsync(deviceId);
        //    if (device == null)
        //        return HttpNotFound();

        //    var model = new DeviceDetailVm
        //    {
        //        Device = device,
        //        //Users = await UserService.GetByDeviceIdAsync(device.DeviceId)
        //    };

        //    return View(model);
        //}

        public ActionResult Create()
        {
            return View(new DeviceVm());
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create(DeviceVm model)
        //{
        //    Message message;
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            await DeviceService.InsertAsync(model);
        //            ModelState.Clear();
        //            model = new DeviceVm();
        //            message = Messages.Success(MessageType.Create.ToString());
        //        }
        //        catch (Exception exception)
        //        {
        //            message = Messages.Failed(MessageType.Create.ToString(), exception.Message);
        //        }
        //    }
        //    else
        //    {
        //        message = Messages.InvalidInput(MessageType.Create.ToString());
        //    }

        //    ViewBag.Message = message;

        //    return View(model);
        //}

        //public async Task<ActionResult> Edit(int id)
        //{
        //    var model = await DeviceService.GetByIdAsync(id);
        //    if (model == null)
        //        return HttpNotFound();

        //    return View(model);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit(DeviceVm model)
        //{
        //    Message message;
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            await DeviceService.UpdateAsync(model);
        //            message = Messages.Success(MessageType.Update.ToString());
        //        }
        //        catch (Exception exception)
        //        {
        //            message = Messages.Failed(MessageType.Update.ToString(), exception.Message);
        //        }
        //    }
        //    else
        //    {
        //        message = Messages.InvalidInput(MessageType.Update.ToString());
        //    }

        //    ViewBag.Message = message;

        //    return View(model);
        //}

        //public async Task<ActionResult> Delete(int? id)
        //{
        //    int.TryParse(id.ToString(), out int userId);
        //    var model = await DeviceService.GetByIdAsync(userId);
        //    if (model == null)
        //        return HttpNotFound();

        //    return View(model);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Delete(int id)
        //{
        //    Message message;
        //    var device = await DeviceService.GetByIdAsync(id);
        //    if (device == null)
        //        return HttpNotFound();

        //    try
        //    {
        //        await DeviceService.DeleteAsync(id);
        //        message = Messages.Success(MessageType.Delete.ToString());
        //    }
        //    catch (Exception exception)
        //    {
        //        message = Messages.Failed(MessageType.Delete.ToString(), exception.Message);
        //    }

        //    ViewBag.Message = message;

        //    return View(device);
        //}

        /// <summary>
        /// All device data 
        /// </summary>
        /// <returns>All device data in JSON format</returns>
        //public async Task<ActionResult> Get()
        //{
        //    var devices = await DeviceService.GetAllAsync();

        //    return Json(new { data = devices }, JsonRequestBehavior.AllowGet);
        //}
    }
}