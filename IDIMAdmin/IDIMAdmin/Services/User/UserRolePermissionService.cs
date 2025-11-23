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
	public class UserRolePermissionService : IUserRolePermissionService
    {
        protected IDIMDBEntities Context { get; set; }
        protected IMapper Mapper { get; set; }
        public UserRolePermissionService(IMapper mapper)
        {
            Context = new IDIMDBEntities();
            Mapper = mapper;
        }

        public async Task InsertDeleteAsync(int userId, IList<RoleGroupByVm> roleGroups)
        {
            var existings = await Context.UserPrivileges
                    .Where(e => e.UserId == userId)
                    .ToListAsync();

            var existingIds = existings.Select(e => e.RoleId).ToList();

            var modelAssignedIds = roleGroups.Select(mg => mg.Roles.Where(r => r.IsAssigned))
                .Select(e => e.Select(l => l.RoleId)).SelectMany(e => e).ToList();

            var newAssignedIds = modelAssignedIds.Except(existingIds);

            foreach(var newAssignedId in newAssignedIds)
            {
                var entity = new UserPrivilege
                {
                    UserId = userId,
                    RoleId = newAssignedId,
                    CreatedDate = DateTime.Now,
                    //CreatedUser = UserExtention.GetUserId().ToString()
                    CreatedUser = 1
                };
                Context.UserPrivileges.Add(entity);
            }

            foreach (var existing in existings)
            {
                if (!modelAssignedIds.Contains(existing.RoleId))
                {
                    Context.UserPrivileges.Remove(existing);
                }
            }

            await Context.SaveChangesAsync();
        }
    }
}