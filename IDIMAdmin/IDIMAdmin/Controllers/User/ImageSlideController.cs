using IDIMAdmin.Extentions;
using IDIMAdmin.Models.User;
using IDIMAdmin.Services.User;

using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IDIMAdmin.Controllers.User
{
	public class ImageSlideController : BaseController
    {
        protected IImageSlideService ImageSlideService { get; set; }
        protected IActivityLogService ActivityLogService { get; set; }

        public ImageSlideController(IImageSlideService imageSlideService,
            IActivityLogService activityLogService) : base(activityLogService)
        {
            ImageSlideService = imageSlideService;
            ActivityLogService = activityLogService;
        }
        // GET: ImageSlide
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public async Task<ActionResult> List()
        {
            var model = await ImageSlideService.GetAllAsync();
            return View(model);
        }
        public async Task<ActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ImageSlideVm model)
        {
            Message message;
            if (ModelState.IsValid && model.ImageFile != null)
            {
                try
                {
                    await ImageSlideService.InsertAsync(model);
                    ModelState.Clear();
                    model = new ImageSlideVm();
                    message = Messages.Success(MessageType.Create.ToString());
                }
                catch (Exception exception)
                {
                    message = Messages.Failed(MessageType.Create.ToString(), exception.Message);
                }
            }
            else
            {
                message = Messages.InvalidInput(MessageType.Create.ToString());
            }

            ViewBag.Message = message;

            return View(model);
        }
        public async Task<ActionResult> Edit(int id)
        {
            var model = await ImageSlideService.GetByIdAsync(id);

            if (model == null)
                return HttpNotFound();

            Session["imgPath"] = model.ImagePath;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ImageSlideVm model)
        {
            Message message;
            if (ModelState.IsValid)
            {
                try
                {
                    string ImagePath = Request.MapPath(Session["imgPath"].ToString());
                    string OldImagePathDb = Session["imgPath"].ToString();
                    await ImageSlideService.UpdateAsync(model, ImagePath, OldImagePathDb);
                    message = Messages.Success(MessageType.Update.ToString());
                }
                catch (Exception exception)
                {
                    message = Messages.Failed(MessageType.Update.ToString(), exception.Message);
                }
            }
            else
            {
                message = Messages.InvalidInput(MessageType.Update.ToString());
            }

            ViewBag.Message = message;

            return View(model);
        }

        public async Task<ActionResult> Delete(int id)
        {
            var model = await ImageSlideService.GetByIdAsync(id);
            if (model == null)
                return HttpNotFound();
            Session["imgPath"] = model.ImagePath;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id)
        {
            Message message;
            int.TryParse(id.ToString(), out int imageId);
            var image = await ImageSlideService.GetByIdAsync(imageId);
            if (image == null)
                return HttpNotFound();

            try
            {
                string oldImagePath = Request.MapPath(Session["imgPath"].ToString());
                await ImageSlideService.DeleteAsync(image.ImageId, oldImagePath);
                message = Messages.Success(MessageType.Delete.ToString());
            }
            catch (Exception exception)
            {
                message = Messages.Failed(MessageType.Delete.ToString(), exception);
            }

            ViewBag.Message = message;

            return RedirectToAction("List");
        }

    }
}