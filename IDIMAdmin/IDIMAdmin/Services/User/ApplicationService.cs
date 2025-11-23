using AutoMapper;
using IDIMAdmin.Entity;
using IDIMAdmin.Extentions.Session;
using IDIMAdmin.Models;
using IDIMAdmin.Models.User;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IDIMAdmin.Services.User
{
    public class ApplicationService : IApplicationService
    {
        protected IMapper Mapper { get; set; }
        protected IDIMDBEntities Context { get; set; }

        public ApplicationService(IMapper mapper)
        {
            Mapper = mapper;
            Context = new IDIMDBEntities();
        }

        public async Task<IList<ApplicationVm>> GetAllAsync()
        {
            var list = await Context.Applications.ToListAsync();
            list = list.OrderBy(e => e.Priority).ThenBy(e => e.ApplicationShortName).ToList();

            return Mapper.Map<IList<ApplicationVm>>(list);
        }
        public async Task<IList<ApplicationVm>> GetAllPublishedAsync()
        {
            var list = await Context.Applications.ToListAsync();
            list = list.Where(e => e.IsPublished == true).OrderBy(e => e.Priority).ThenBy(e => e.ApplicationShortName).ToList();

            return Mapper.Map<IList<ApplicationVm>>(list);
        }
        public async Task<LandingVm> GetApplicationsAndSliderImages()
        {
            var model = new LandingVm();
            var apps = await Context.Applications.ToListAsync();
            apps = apps.Where(e => e.IsPublished == true).OrderBy(e => e.Priority).ThenBy(e => e.ApplicationShortName).ToList();

            model.Applications = Mapper.Map<IList<ApplicationVm>>(apps);

            var images = await Context.SlideImages.OrderBy(e => e.Priority).ToListAsync();
            model.ImageSlides = Mapper.Map<IList<ImageSlideVm>>(images);

            return model;
        }

        public async Task<ApplicationVm> GetByIdAsync(int id)
        {
            var entity = await Context.Applications.FindAsync(id);

            return Mapper.Map<ApplicationVm>(entity);
        }

        public async Task<IList<ApplicationVm>> GetByUserIdAsync(int userId)
        {
            var list = await (from up in Context.UserPrivileges
                              join r in Context.Roles on up.RoleId equals r.RoleId
                              where up.UserId == userId
                              select r.Application).ToListAsync();

            return Mapper.Map<IList<ApplicationVm>>(list);
        }

        public async Task<ApplicationVm> InsertAsync(ApplicationVm model)
        {
            var existing = await Context.Applications.FirstOrDefaultAsync(m => m.ApplicationName == model.ApplicationName);
            if (existing != null)
                throw new ArgumentException($"{nameof(model.ApplicationName)} already exists ");

            var entity = Mapper.Map<Application>(model);
            entity.CreatedDateTime = DateTime.Now;
            entity.CreatedUser = UserExtention.GetUserId();

            var added = Context.Applications.Add(entity);
            await Context.SaveChangesAsync();

            return Mapper.Map<ApplicationVm>(added);
        }

        public async Task<ApplicationVm> UpdateAsync(ApplicationVm model)
        {
            var existing = await Context.Applications.FindAsync(model.ApplicationId);

            if (existing == null)
                throw new ArgumentException($"Application does not exists.");

            var duplicate = await Context.Applications.Where(e => e.ApplicationId != model.ApplicationId).FirstOrDefaultAsync(e => e.ApplicationName == model.ApplicationName);

            if (duplicate != null)
                throw new ArgumentException($"Name already exists.");

            existing.ApplicationName = model.ApplicationName;
            existing.ApplicationShortName = model.ApplicationShortName;
            existing.Url = model.Url;
            existing.Priority = model.Priority;
            existing.Icon = model.Icon;
            existing.Color = model.Color;
            existing.IsPublished = model.IsPublished;
            existing.UpdatedDateTime = DateTime.Now;
            existing.UpdatedUser = UserExtention.GetUserId();
            existing.UpdateNo += 1;

            await Context.SaveChangesAsync();

            return Mapper.Map<ApplicationVm>(existing);
        }

        public async Task<ApplicationVm> DeleteAsync(int id)
        {
            var existing = await Context.Applications.FindAsync(id);

            if (existing == null)
                throw new ArgumentException($"Application does not exists.");

            Context.Entry(existing).State = EntityState.Deleted;
            await Context.SaveChangesAsync();

            return Mapper.Map<ApplicationVm>(existing);
        }

        public async Task<IEnumerable<SelectListItem>> GetDropDownAsync(int? selected = 0)
        {
            var applications = await Context.Applications.OrderBy(e => e.ApplicationCode).ToListAsync();

            return applications.Select(e => new SelectListItem
            {
                Text = e.ApplicationCode,
                Value = e.ApplicationId.ToString(),
                Selected = e.ApplicationId == selected
            });
        }

        #region user application
        /// <summary>
        /// Get all applications with assigned application list
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns></returns>
        //public async Task<IList<ApplicationAssignVm>> GetAssignByUserIdAsync(int userId)
        //{
        //    var applications = await Context.Applications.Select(e => new ApplicationAssignVm
        //    {
        //        Application = new ApplicationVm
        //        {
        //            ApplicationId = e.ApplicationId,
        //            ApplicationName = e.ApplicationName
        //        }
        //    }).OrderBy(e=>e.Application.ApplicationName).ToListAsync();

        //    var userApplications = await Context.UserApplications
        //        .Where(e => e.UserId == userId)
        //        .ToListAsync();

        //    if (!userApplications.Any())
        //        return applications;

        //    applications = applications.Select(e => new ApplicationAssignVm
        //    {
        //        Application = e.Application,
        //        IsAssigned = userApplications.Any(m => m.ApplicationId == e.Application.ApplicationId)
        //    }).ToList();

        //    return applications;
        //}
        #endregion
    }
}