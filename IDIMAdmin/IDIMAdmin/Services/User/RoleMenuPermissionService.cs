using AutoMapper;

using IDIMAdmin.Entity;
using IDIMAdmin.Models.User;

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace IDIMAdmin.Services.User
{
	public class RoleMenuPermissionService : IRoleMenuPermissionService
    {
        protected IMapper Mapper { get; set; }
        protected IDIMDBEntities Context { get; set; }
        public RoleMenuPermissionService(IMapper mapper)
        {
            Mapper = mapper;
            Context = new IDIMDBEntities();
        }

        public async Task InsertDeleteAsync(int roleId, IList<MenuGroupByVm> menuGroups)
        {
            var existings = await Context.RolePrivileges
                    .Where(e => e.RoleId == roleId)
                    .ToListAsync();

            var existingIds = existings.Select(e => e.MenuId).ToList();

            var modelAssignedIds = menuGroups.Select(mg => mg.Menus.Where(m => m.IsAssigned))
                .Select(e => e.Select(l => l.MenuId)).SelectMany(e=>e).ToList();

            var newAssignedIds = modelAssignedIds.Except(existingIds);

            foreach(var newAssignedId in newAssignedIds)
            {
                var entity = new RolePrivilege
                {
                    RoleId = roleId,
                    MenuId = newAssignedId,
                    CreatedDate = DateTime.Now,
                    //CreatedUser = UserExtention.GetUserId().ToString(),
                    CreatedUser = 1,
                };
                Context.RolePrivileges.Add(entity);
            }

            foreach(var existing in existings)
            {
                if (!modelAssignedIds.Contains(existing.MenuId))
                {
                    Context.RolePrivileges.Remove(existing);
                }
            }

            await Context.SaveChangesAsync();
        }
    }
}