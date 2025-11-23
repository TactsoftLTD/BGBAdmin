using AutoMapper;
using IDIMAdmin.Entity;
using IDIMAdmin.Models.User;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using System.Web;
using System.Linq;

namespace IDIMAdmin.Services.User
{
    public class ImageSlideService : IImageSlideService
    {
        protected IDIMDBEntities Context { get; set; }
        protected IMapper Mapper { get; set; }

        public ImageSlideService(IMapper mapper)
        {
            Context = new IDIMDBEntities();
            Mapper = mapper;
        }

        public async Task<List<ImageSlideVm>> GetAllAsync()
        {
            var result = await Context.SlideImages.OrderBy(image => image.Priority)
                .ThenByDescending(image => image.ImageId)
                .ToListAsync();

            return Mapper.Map<List<ImageSlideVm>>(result);
        }
        public async Task<ImageSlideVm> GetByIdAsync(int id)
        {
            var entity = await Context.SlideImages.FindAsync(id);

            return Mapper.Map<ImageSlideVm>(entity);
        }

        public async Task<ImageSlideVm> InsertAsync(ImageSlideVm model)
        {
            string fileName = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);
            string extension = Path.GetExtension(model.ImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            model.ImagePath = "~/Images/" + fileName;
            string path = HttpContext.Current.Server.MapPath("~/Images/");
            fileName = Path.Combine(path, fileName);
            model.ImageFile.SaveAs(fileName);

            var entity = Mapper.Map<SlideImage>(model);
            var added = Context.SlideImages.Add(entity);
            await Context.SaveChangesAsync();

            return Mapper.Map<ImageSlideVm>(added);
        }
        public async Task<ImageSlideVm> UpdateAsync(ImageSlideVm model, string ImagePath, string OldImagePathDb)
        {
            var existing = await Context.SlideImages.FindAsync(model.ImageId);

            if (existing == null)
                throw new ArgumentException($"Image does not exists.");

            existing.Title = model.Title;
            if (model.ImageFile != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);
                string extension = Path.GetExtension(model.ImageFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                model.ImagePath = "~/Images/" + fileName;
                string path = HttpContext.Current.Server.MapPath("~/Images/");
                fileName = Path.Combine(path, fileName);
                model.ImageFile.SaveAs(fileName);
                if (File.Exists(ImagePath))
                {
                    File.Delete(ImagePath);
                }
                existing.ImagePath = model.ImagePath;
            }
            else
            {
                existing.ImagePath = OldImagePathDb;
            }

            existing.AlternateText = model.AlternateText;
            existing.Priority = model.Priority;
            existing.Description = model.Description;

            await Context.SaveChangesAsync();

            return Mapper.Map<ImageSlideVm>(existing);
        }
        public async Task<ImageSlideVm> DeleteAsync(int id, string ImagePath)
        {
            var existing = await Context.SlideImages.FindAsync(id);

            if (existing == null)
                throw new ArgumentException($"Image does not exists.");

            if (File.Exists(ImagePath))
            {
                File.Delete(ImagePath);
            }
            Context.Entry(existing).State = EntityState.Deleted;
            await Context.SaveChangesAsync();

            return Mapper.Map<ImageSlideVm>(existing);
        }
    }
}