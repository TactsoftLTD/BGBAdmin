using AutoMapper;
using DataTables.Mvc;
using IDIMAdmin.Entity;
using IDIMAdmin.Extentions;
using IDIMAdmin.Models.User;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace IDIMAdmin.Services.User
{
    public class RoleService : IRoleService
    {
        protected IDIMDBEntities Context { get; set; }
        protected IMapper Mapper { get; set; }

        public RoleService(IMapper mapper)
        {
            Context = new IDIMDBEntities();
            Mapper = mapper;
        }

        public async Task<RoleVm> InsertAsync(RoleVm model)
        {
            var existing = await Context.Roles
                .FirstOrDefaultAsync(m => m.Name.ToLower() == model.Name.ToLower() && m.ApplicationId == model.ApplicationId);
            if (existing != null)
                throw new ArgumentException($"Same role already exists in this application");

            var entity = Mapper.Map<Role>(model);
            entity.CreatedDateTime = DateTime.Now;
            //entity.CreatedUser = UserExtention.GetUserId();
            entity.CreatedUser = 1;

            var added = Context.Roles.Add(entity);
            await Context.SaveChangesAsync();

            return Mapper.Map<RoleVm>(added);
        }

        public async Task<RoleVm> GetByIdAsync(int id)
        {
            var entity = await Context.Roles.FindAsync(id);

            return Mapper.Map<RoleVm>(entity);
        }

        public async Task<DataTablesResponse> GetByAsync(IDataTablesRequest requestModel, RoleSearchVm filter)
        {
            if (filter == null)
                filter = new RoleSearchVm();

            var query = Context.Roles.AsQueryable();

            var totalCount = query.Count();

            query = query
                .Where(x => (string.IsNullOrEmpty(filter.Name) || x.Name.Contains(filter.Name)) &&
                            (!filter.ApplicationId.HasValue || x.ApplicationId == filter.ApplicationId.Value)).
                            OrderBy(e => e.ApplicationId).AsQueryable();

            var filteredCount = query.Count();

            query = query.Sorting(requestModel);
            query = query.Paging(requestModel);
            var list = await query.ToListAsync();
            var data = Mapper.Map<List<RoleVm>>(list);

            return new DataTablesResponse(requestModel.Draw, data, filteredCount, totalCount);
        }

        public async Task<RoleVm> UpdateAsync(RoleVm model)
        {
            var existing = await Context.Roles.FindAsync(model.RoleId);

            if (existing == null)
                throw new ArgumentException($"Role does not exists.");

            var duplicate = await Context.Roles.Where(x => x.Name == model.Name && x.ApplicationId == model.ApplicationId && x.IsActive == model.IsActive).FirstOrDefaultAsync();

            if (duplicate != null)
                throw new ArgumentException($"Same role already exists in this application.");

            existing.Name = model.Name;
            existing.ApplicationId = model.ApplicationId;
            existing.IsActive = model.IsActive;
            existing.UpdatedDateTime = DateTime.Now;
            //existing.UpdatedUser = UserExtention.GetUserId();
            existing.UpdatedUser = 1;
            existing.UpdateNo += 1;

            await Context.SaveChangesAsync();

            return Mapper.Map<RoleVm>(existing);
        }

        public async Task<RoleVm> DeleteAsync(int id)
        {
            var existing = await Context.Roles.FindAsync(id);

            if (existing == null)
                throw new ArgumentException($"Role does not exists.");

            Context.Entry(existing).State = EntityState.Deleted;
            await Context.SaveChangesAsync();

            return Mapper.Map<RoleVm>(existing);
        }

        public async Task<IList<RoleGroupByVm>> GetByUserIdAsync(int userId)
        {
            var roles = await RoleByApplication().ToListAsync();
            var userRoles = await UserRolesByUserId(userId).ToListAsync();

            if (userRoles.Any())
                roles = roles.Select(mg =>
                {
                    mg.Roles = mg.Roles.Select(l =>
                    {
                        l.IsAssigned = userRoles.Select(e => e.RoleId).Contains(l.RoleId);
                        return l;
                    }).ToList();

                    return mg;
                }).ToList();

            return roles;
        }

        public async Task<RoleVm> GetRoleInfo(int? id, bool checkActive = false)
        {
            var role = await Context.Roles.FindAsync(id);

            if (checkActive)
            {
                role = role?.IsActive == true ? role : null;
            }

            var application = await Context.Applications.Where(e => e.ApplicationId == role.ApplicationId).Select(e => e.ApplicationName).SingleOrDefaultAsync();

            return new RoleVm
            {
                RoleId = role.RoleId,
                Name = role.Name,
                ApplicationId = role.ApplicationId,
                CreatedUser = role.CreatedUser,
                IsActive = (bool)role.IsActive,
                ApplicationName = application
            };
        }

        private IQueryable<RoleGroupByVm> RoleByApplication()
        {
            var roleGroups = Context.Applications
                             .Include(e => e.Roles)
                             .GroupBy(e => new { e.ApplicationId, e.ApplicationName })
                             .Select(g => new RoleGroupByVm
                             {
                                 ApplicationId = g.Key.ApplicationId,
                                 ApplicationName = g.Key.ApplicationName,
                                 Roles = g.SelectMany(r => r.Roles.Select(l => new RoleAssignVm
                                 {
                                     RoleId = l.RoleId,
                                     Name = l.Name,
                                     ApplicationId = l.ApplicationId
                                 })).ToList()
                             }).OrderBy(e => e.ApplicationName);

            return roleGroups;
        }

        private IQueryable<UserPrivilege> UserRolesByUserId(int userId)
        {
            return Context.UserPrivileges.Where(e => e.UserId == userId);
        }
    }
}