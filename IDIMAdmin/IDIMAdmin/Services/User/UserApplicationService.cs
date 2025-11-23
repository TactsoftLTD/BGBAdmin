using AutoMapper;

using IDIMAdmin.Entity;

namespace IDIMAdmin.Services.User
{
	public class UserApplicationService : IUserApplicationService
    {
        protected IMapper Mapper { get; set; }
        protected IDIMDBEntities Context { get; set; }

        public UserApplicationService(IMapper mapper)
        {
            Mapper = mapper;
            Context = new IDIMDBEntities();
        }

        //public async Task<IList<UserApplicationVm>> GetAllAsync()
        //{
        //    var userApplications = await Context.UserApplications.ToListAsync();
        //    return Mapper.Map<IList<UserApplicationVm>>(userApplications);
        //}

        //public async Task<UserApplicationVm> GetByIdAsync(int id)
        //{
        //    var userApplication = await Context.UserApplications.FindAsync(id);
        //    return Mapper.Map<UserApplicationVm>(userApplication);
        //}

        //public async Task<UserApplicationVm> InsertAsync(UserApplicationVm model)
        //{
        //    var existing = await Context.UserApplications
        //        .FirstOrDefaultAsync(e => e.UserId == model.UserId && e.ApplicationId == model.ApplicationId);
        //    if (existing != null)
        //        throw new ArgumentException($"User application already exists.");

        //    var entity = Mapper.Map<UserPriviledge>(model);
        //    entity.CreatedDateTime = DateTime.Now;
        //    entity.CreatedUser = 1;

        //    var added = Context.UserPriviledges.Add(entity);
        //    await Context.SaveChangesAsync();

        //    return Mapper.Map<UserApplicationVm>(added);
        //}

        //public async Task<UserApplicationVm> UpdateAsync(UserApplicationVm model)
        //{
        //    var existing = await Context.UserApplications
        //        .FirstOrDefaultAsync(e => e.UserId == model.UserId && e.ApplicationId == model.ApplicationId);
        //    if (existing == null)
        //        throw new ArgumentException($"User application not found for user.");


        //    existing.ApplicationId = model.ApplicationId;
        //    existing.UserId = model.UserId;
        //    existing.UpdatedDateTime = DateTime.Now;
        //    existing.UpdatedUser = UserExtention.GetUserId(); ;
        //    existing.UpdateNo += 1;

        //    await Context.SaveChangesAsync();

        //    return Mapper.Map<UserApplicationVm>(existing);
        //}

        //public async Task<UserApplicationVm> DeleteAsync(int id)
        //{
        //    var existing = await Context.UserApplications.FindAsync(id);
        //    if (existing == null)
        //        throw new ArgumentException("User application not exists.");

        //    Context.Entry(existing).State = EntityState.Deleted;
        //    await Context.SaveChangesAsync();

        //    return Mapper.Map<UserApplicationVm>(existing);
        //}

        /// <summary>
        /// add or delete menus from user
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <param name="applications">Applications</param>
        /// <returns></returns>
        //public async Task InsertDeleteAsync(int userId, IList<ApplicationAssignVm> applications)
        //{
        //    var existings = await Context.UserApplications
        //        .Where(e => e.UserId == userId)
        //        .ToListAsync();
        //    var existingIds = existings.Select(e => e.ApplicationId).ToList();

        //    var modelAssignedIds = applications.Where(x => x.IsAssigned == true)
        //        .Select(x => x.Application.ApplicationId)
        //        .ToList();

        //    var newAssignedIds = modelAssignedIds.Except(existingIds);

        //    foreach (var newAssignedId in newAssignedIds)
        //    {
        //        var entity = new UserApplication
        //        {
        //            UserId = userId,
        //            ApplicationId = newAssignedId,
        //            CreatedDateTime = DateTime.Now,
        //            CreatedUser = UserExtention.GetUserId()
        //        };
        //        Context.UserApplications.Add(entity);
        //    }

        //    foreach (var existing in existings)
        //    {
        //        if (!modelAssignedIds.Contains(existing.ApplicationId))
        //        {
        //            Context.UserApplications.Remove(existing);
        //        }
        //    }

        //    await Context.SaveChangesAsync();
        //}
    }
}