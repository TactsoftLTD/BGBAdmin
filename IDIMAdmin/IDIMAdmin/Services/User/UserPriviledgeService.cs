using AutoMapper;

using IDIMAdmin.Entity;

namespace IDIMAdmin.Services.User
{
	public class UserPriviledgeService : IUserPriviledgeService
    {
        protected IMapper Mapper { get; set; }
        protected IDIMDBEntities Context { get; set; }

        public UserPriviledgeService(IMapper mapper)
        {
            Mapper = mapper;
            Context = new IDIMDBEntities();
        }

        //public async Task<IList<UserPriviledgeVm>> GetAllAsync()
        //{
        //    var userPriviledges = await Context.UserPriviledges.ToListAsync();
        //    return Mapper.Map<IList<UserPriviledgeVm>>(userPriviledges);
        //}

        //public async Task<UserPriviledgeVm> GetByIdAsync(int id)
        //{
        //    var userPriviledge = await Context.UserPriviledges.FindAsync(id);
        //    return Mapper.Map<UserPriviledgeVm>(userPriviledge);
        //}

        //public async Task<UserPriviledgeVm> InsertAsync(UserPriviledgeVm model)
        //{
        //    var existing = await Context.UserPriviledges.FirstOrDefaultAsync(m => m.UserId == model.UserId && m.MenuId == model.MenuId);
        //    if (existing != null)
        //        throw new ArgumentException($"User menu already exists ");

        //    var entity = Mapper.Map<UserPriviledge>(model);
        //    entity.CreatedDateTime = DateTime.Now;
        //    entity.CreatedUser = 1;

        //    var added = Context.UserPriviledges.Add(entity);
        //    await Context.SaveChangesAsync();

        //    return Mapper.Map<UserPriviledgeVm>(added);
        //}

        //public async Task<UserPriviledgeVm> UpdateAsync(UserPriviledgeVm model)
        //{
        //    var existing = await Context.UserPriviledges.FindAsync(model.MenuId);

        //    if (existing == null)
        //        throw new ArgumentException($"Menu not found for user");


        //    existing.MenuId = model.MenuId;
        //    existing.UserId = model.UserId;
        //    existing.UpdatedDateTime = DateTime.Now;
        //    existing.UpdatedUser = UserExtention.GetUserId(); ;
        //    existing.UpdateNo += 1;

        //    await Context.SaveChangesAsync();

        //    return Mapper.Map<UserPriviledgeVm>(existing);
        //}

        //public async Task<UserPriviledgeVm> DeleteAsync(int id)
        //{
        //    var existing = await Context.UserPriviledges.FindAsync(id);

        //    if (existing == null)
        //        throw new ArgumentException($"{nameof(Application)} not exists ");

        //    Context.Entry(existing).State = EntityState.Deleted;
        //    await Context.SaveChangesAsync();

        //    return Mapper.Map<UserPriviledgeVm>(existing);
        //}

        /// <summary>
        /// add or delete menus from user
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <param name="menuGroups">Menus group by application </param>
        /// <returns></returns>
        //public async Task InsertDeleteAsync(int userId, IList<MenuGroupByVm> menuGroups)
        //{
        //    var existings = await Context.UserPriviledges
        //        .Where(e => e.UserId == userId)
        //        .ToListAsync();

        //    var existingIds = existings.Select(e => e.MenuId).ToList();

        //    var modelAssignedIds = menuGroups.Select(mg => mg.Menus.Where(m => m.IsAssigned))
        //        .Select(e => e.Select(l=>l.MenuId)).SelectMany(e=>e).ToList();

        //    var newAssignedIds = modelAssignedIds.Except(existingIds);


        //    foreach (var newAssignedId in newAssignedIds)
        //    {
        //        var entity = new UserPriviledge
        //        {
        //            UserId = userId,
        //            MenuId = newAssignedId,
        //            CreatedDateTime = DateTime.Now,
        //            CreatedUser = UserExtention.GetUserId()
        //        };
        //        Context.UserPriviledges.Add(entity);
        //    }

        //    foreach (var existing in existings)
        //    {
        //        if (!modelAssignedIds.Contains(existing.MenuId))
        //        {
        //            Context.UserPriviledges.Remove(existing);
        //        }
        //    }

        //    await Context.SaveChangesAsync();
           
        //}
    }
}