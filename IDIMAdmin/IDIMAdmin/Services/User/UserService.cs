using AutoMapper;

using DataTables.Mvc;

using IDIMAdmin.Entity;
using IDIMAdmin.Extentions;
using IDIMAdmin.Extentions.Ad;
using IDIMAdmin.Extentions.Collections.Select2;
using IDIMAdmin.Extentions.File;
using IDIMAdmin.Extentions.Session;
using IDIMAdmin.Models.User;

using MailKit.Net.Smtp;

using MimeKit;
using MimeKit.Text;

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IDIMAdmin.Services.User
{
	public class UserService : IUserService
    {
        protected IMapper Mapper { get; set; }
        protected IDIMDBEntities Context { get; set; }

        public UserService(IMapper mapper)
        {
            Context = new IDIMDBEntities();
            Mapper = mapper;
        }

        private async Task<Entity.User> Duplicate(UserVm model)
        {
            // check user exist
            var entity = await Context.Users.FindAsync(model.UserId);
            if (entity == null)
                throw new ArgumentException($"User '{model.Username}' not found.");

            // check duplicate user
            var duplicate = await Context.Users.Where(e => e.UserId != model.UserId)
                .FirstOrDefaultAsync(d => d.Username == model.Username);
            if (duplicate != null)
                throw new ArgumentException($"Username '{model.Username}' alreay exists.");

            // check regiment already assigned
            var regiment = await Context.Users.Where(e => e.UserId != model.UserId)
                .FirstOrDefaultAsync(d => d.ArmyId == model.ArmyId);
            if (regiment != null)
                throw new ArgumentException($"Regiment already assigned.");

            return entity;
        }



        public async Task<List<UserVm>> GetAllAsync(bool excludeNotActive = true)
        {
            var query = Context.Users.AsQueryable();

            if (excludeNotActive)
            {
                query = query.Where(e => e.IsActive);
            }

            var users = await query.ToListAsync();

            return Mapper.Map<List<UserVm>>(users);
        }

        public async Task<UserVm> GetByIdAsync(int id, bool checkActive = false)
        {
            var user = await Context.Users.FindAsync(id);

            if (checkActive)
            {
                user = user?.IsActive == true ? user : null;
            }

            return Mapper.Map<UserVm>(user);
        }

        public async Task<UserVm> InsertAsync(UserVm model)
        {
            var duplicate = await Context.Users.Where(e => e.UserId != model.UserId)
                .FirstOrDefaultAsync(d => d.Username == model.Username);
            if (duplicate != null)
                throw new ArgumentException($"Username '{model.Username}' alreay exists.");

            var entity = Mapper.Map<Entity.User>(model);
            entity.CreatedDateTime = DateTime.Now;
            entity.CreatedUser = UserExtention.GetUserId();

            var added = Context.Users.Add(entity);
            await Context.SaveChangesAsync();

            return Mapper.Map<UserVm>(added);
        }

        public async Task<UserVm> UpdateAsync(UserVm model)
        {
            var entity = await Context.Users.FindAsync(model.UserId);
            if (entity == null)
                throw new ArgumentException($"User '{model.Username}' not found.");

            // check duplicate user
            var duplicate = await Context.Users.Where(e => e.UserId != model.UserId)
                .FirstOrDefaultAsync(d => d.Username == model.Username);
            if (duplicate != null)
                throw new ArgumentException($"Username '{model.Username}' alreay exists.");

            entity.ArmyId = model.ArmyId;
            entity.Username = model.Username;
            entity.PersonnelCode = model.PersonnelCode;
            entity.Email = model.Email;
            entity.Phone = model.Phone;
            entity.IsActive = model.IsActive;
            entity.IsAll = model.IsAll;
            entity.UniteList = model.UniteList;
            entity.UserType = model.UserType;
            entity.UpdatedDateTime = DateTime.Now;
            entity.UpdatedUser = UserExtention.GetUserId();
            entity.UpdateNo += 1;
            await Context.SaveChangesAsync();

            return Mapper.Map<UserVm>(entity);
        }

        public async Task<UserVm> UpdatePasswordAsync(UserVm model)
        {
            var user = await Context.Users.FirstOrDefaultAsync(e => e.Username == model.Username);

            if (user == null)
                return null;

            user.Password = model.Password;
            await Context.SaveChangesAsync();

            return Mapper.Map<UserVm>(user);
        }

        public async Task<UserVm> DeleteAsync(int id)
        {
            var existing = await Context.Users.FindAsync(id);

            if (existing == null)
                throw new ArgumentException($"{nameof(existing.UserId)} not found.");

            Context.Entry(existing).State = EntityState.Deleted;
            await Context.SaveChangesAsync();

            return Mapper.Map<UserVm>(existing);
        }

        public async Task<Select2PagedResult> GetBySelect2Async(Select2Param param)
        {
            var select2 = new Select2PagedResult();

            var query = Context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(param.Term))
                query = query.Where(e => e.Username.Contains(param.Term));

            var list = await query.OrderBy(e => e.Username).Take(20).ToListAsync();

            select2.Results = list.Select(e => new Select2Result
            {
                id = e.UserId.ToString(),
                text = e.Username
            }).ToList();

            return select2;
        }
        public async Task<DataTablesResponse> GetByAsync(IDataTablesRequest requestModel, UserSearchVm filter)
        {
            try
            {
                if (filter == null)
                    filter = new UserSearchVm();

                var query = Context.Users
                            .Include(u => u.GeneralInformation);
                var totalCount = query.Count();

                var res = (from q in query
                           from up in Context.UserPrivileges.Where(x => x.UserId == q.UserId).DefaultIfEmpty()
                           from r in Context.Roles.Where(x => x.RoleId == up.RoleId).DefaultIfEmpty()
                           where ((!filter.ApplicationId.HasValue || r.ApplicationId == filter.ApplicationId.Value)
                                   && string.IsNullOrEmpty(filter.Username) || q.Username.Contains(filter.Username)
                                   && (!filter.ArmyId.HasValue || q.GeneralInformation.ArmyId == filter.ArmyId))
                           select new
                           {
                               q.UserId,
                               q.Username,
                               q.Email,
                               q.PersonnelCode,
                               q.UserType,
                               q.GeneralInformation.RegimentNo,
                               q.IsActive,
                               q.IsAll,
                               q.ArmyId,
                               q.UniteList,
                               q.Password,
                               q.Phone,                               
                               q.GeneralInformation.Picture,
                               q.CreatedUser,
                               q.CreatedDateTime,
                               q.UpdatedUser,
                               q.UpdatedDateTime,
                               q.UpdateNo
                           }).Distinct();

                if (filter.Active != Active.All)
                {
                    var active = filter.Active == Active.Active;
                    res = res.Where(e => e.IsActive == active);
                }

                var filteredCount = res.Count();

                var result = await res.OrderBy(x => x.UserId)
                               .Skip(requestModel.Start)
                              .Take(requestModel.Length).ToListAsync();

                var data = Mapper.Map<List<UserVm>>(result);
                

                return new DataTablesResponse(requestModel.Draw, data, filteredCount, totalCount);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }

        public async Task<List<UserVm>> GetUserByFilterAsync(UserSearchVm filter)
        {
            if (filter == null)
                filter = new UserSearchVm();

            var query = Context.Users
                .Include(u => u.GeneralInformation);

            var res = (from q in query
                      join up in Context.UserPrivileges on q.UserId equals up.UserId
                      join r in Context.Roles on up.RoleId equals r.RoleId
                      where ((!filter.ApplicationId.HasValue || r.ApplicationId == filter.ApplicationId.Value)
                              && string.IsNullOrEmpty(filter.Username) || q.Username.Contains(filter.Username)
                              && (!filter.ArmyId.HasValue || q.GeneralInformation.ArmyId == filter.ArmyId))
                      select new
                      {
                          q.UserId,
                          q.Username,
                          q.Email,
                          q.PersonnelCode,
                          q.GeneralInformation.RegimentNo,
                          q.IsActive,
                          q.ArmyId,
                          q.UserType,
                          q.IsAll,
                          q.UniteList,
                          q.Password,
                          q.Phone,
                          q.GeneralInformation.Picture,
                          q.CreatedUser,
                          q.CreatedDateTime,
                          q.UpdatedUser,
                          q.UpdatedDateTime,
                          q.UpdateNo
                      }).Distinct();

            if (filter.Active != Active.All)
            {
                var active = filter.Active == Active.Active;
                res = res.Where(e => e.IsActive == active);
            }

            var users = await res.OrderBy(e => e.Username).ToListAsync();

            return Mapper.Map<List<UserVm>>(users);
        }

        public async Task<UserInformation> LoginAsync(string username, string password, bool? rememberMe = false)
        {
            var user = await Context.Users
                .Include(e => e.GeneralInformation)
                .FirstOrDefaultAsync(e => e.Username == username && e.IsActive);

            if (user == null)
                return null;

            if (!string.Equals(user.Password, password))
                return null;

            var roleList = await (from r in Context.Roles
                                  join up in Context.UserPrivileges on r.RoleId equals up.RoleId
                                  where r.ApplicationId == DefaultData.ApplicationId && up.UserId == user.UserId
                                  select r.RoleId).Distinct().ToListAsync();

            var userInformation = new UserInformation
            {
                UserId = user.UserId,
                ArmyId = user.ArmyId,
                Name = user.GeneralInformation?.Name,
                Username = user.Username,
                UnitId = user.GeneralInformation?.UnitId,
                DeviceId = 1,
                ApplicationId = DefaultData.ApplicationId,
                Avatar = user.GeneralInformation?.Picture.ToThumbnail(),
                Menus = await GetRoleMenuAsync(roleList, DefaultData.ApplicationId),
                Applications = await GetUserApplicationAsync(roleList),
            };

            UserExtention.Save(nameof(UserInformation), userInformation);

            if (DefaultData.OtpEnable)
            {
                await SaveOtpAndSendEmail(user.UserId);
            }

            return userInformation;
        }

        public async Task SaveOtpAndSendEmail(int userId)
        {
            string otp = GenerateOTP();
            var user = await Context.Users.FirstOrDefaultAsync(x => x.UserId == userId);
            if (user == null)
                throw new ArgumentException("User can not be found!");

            string emailBody = string.Format(DefaultData.EmailBody, otp, DefaultData.OTPExpiredTime);

            user.OtpCode = otp;
            user.OtpGeneratedDate = DateTime.Now;
            await Context.SaveChangesAsync();

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(DefaultData.EmailForm));
            email.To.Add(MailboxAddress.Parse(user.Email));
            email.Subject = DefaultData.EmailSubject;
            email.Body = new TextPart(TextFormat.Html) { Text = emailBody };

            using (var smtp = new SmtpClient())
            {
                //smtp.Connect(DefaultData.EmailServer, DefaultData.EmailPort, SecureSocketOptions.StartTls);
                smtp.Connect(DefaultData.EmailServer, DefaultData.EmailPort, false);
                smtp.Authenticate(DefaultData.EmailUserName, DefaultData.EmailPassword);
                smtp.Send(email);
                smtp.Disconnect(true);
            }
        }

        public async Task<bool> IsOtpValid(int userId, string otp)
        {
            var user = await Context.Users.FirstOrDefaultAsync(x => x.UserId == userId);
            if (user == null)
                throw new ArgumentException("User can not be found!");

            bool isWithinTimePeriod = false;
            if (user.OtpGeneratedDate.HasValue)
            {
                DateTime currentTime = DateTime.Now;
                TimeSpan difference = currentTime - user.OtpGeneratedDate.Value;

                isWithinTimePeriod = difference.TotalMinutes <= DefaultData.OTPExpiredTime;
            }

            if (user.OtpCode == otp && isWithinTimePeriod)
                return true;
            else
                return false;
        }

        public async Task<UserInformation> AdLoginAsync(string username, string password, bool? rememberMe = false)
        {
            username = username.ToUsername();

            var message = "Username or password not correct";

            var adConnection = new LdapExtention(DefaultData.AdServer, username, password);

            if (!adConnection.IsAuthenticated())
                throw new ArgumentException(message);

            var user = await Context.Users
                .Include(e => e.GeneralInformation)
                .FirstOrDefaultAsync(e => e.Username == username);

            var newUser = new UserVm
            {
                Username = username,
                Password = password,
                IsActive = true
            };

            if (user == null)
            {
                await InsertAsync(newUser);
                user = await Context.Users
                    .Include(e => e.GeneralInformation)
                    .FirstOrDefaultAsync(e => e.Username == username);
            }
            else
            {
                if (!string.Equals(user.Password, password))
                {
                    var updatedUser = await UpdatePasswordAsync(newUser);
                    user = Mapper.Map<Entity.User>(updatedUser);
                }

                if (user.IsActive == false)
                    throw new ArgumentException("User is not active, contact with system administrator.");
            }

            if (user == null)
                throw new ArgumentException(message);

            var roleList = await (from r in Context.Roles
                                  join up in Context.UserPrivileges on r.RoleId equals up.RoleId
                                  where r.ApplicationId == DefaultData.ApplicationId && up.UserId == user.UserId
                                  select r.RoleId).Distinct().ToListAsync();

            var userInformation = new UserInformation
            {
                UserId = user.UserId,
                ArmyId = user.ArmyId,
                Name = user.GeneralInformation?.Name,
                Username = user.Username,
                UnitId = user.GeneralInformation?.UnitId,
                DeviceId = 1,
                ApplicationId = DefaultData.ApplicationId,
                Avatar = user.GeneralInformation?.Picture.ToThumbnail(),
                Menus = await GetRoleMenuAsync(roleList, DefaultData.ApplicationId),
                Applications = await GetUserApplicationAsync(roleList)
            };

            UserExtention.Save(nameof(UserInformation), userInformation);

            return userInformation;
        }

        public async Task<IList<UserVm>> GetByApplicationIdAsync(int applicationId)
        {
            var user = await (from u in Context.Users
                              join up in Context.UserPrivileges on u.UserId equals up.UserId
                              join r in Context.Roles on up.RoleId equals r.RoleId
                              where r.ApplicationId == applicationId
                              select u).ToListAsync();

            return Mapper.Map<List<UserVm>>(user);
        }

        private async Task<IList<ApplicationVm>> GetUserApplicationAsync(List<int> roleList)
        {
            if (roleList.Count != 0)
            {
                var applications = (from r in Context.Roles
                                    join a in Context.Applications on r.ApplicationId equals a.ApplicationId
                                    where roleList.Contains(r.RoleId)
                                    select a).Distinct();

                return await applications.Select(e => new ApplicationVm
                {
                    ApplicationName = e.ApplicationName,
                    ApplicationCode = e.ApplicationCode,
                    ApplicationShortName = e.ApplicationShortName,
                    ApplicationId = e.ApplicationId,
                    Color = e.Color,
                    Icon = e.Icon,
                    Url = e.Url
                }).Distinct().ToListAsync();
            }
            else
            {
                return null;
            }
        }

        private async Task<IList<MenuInformation>> GetRoleMenuAsync(List<int> roleList, int applicationId)
        {
            var menus = (from rp in Context.RolePrivileges
                         join m in Context.Menus on rp.MenuId equals m.MenuId
                         join r in Context.Roles on rp.RoleId equals r.RoleId
                         where (r.ApplicationId == applicationId && roleList.Contains(rp.RoleId) && m.IsPublished == true)
                         select m)
                         .GroupBy(x => x.MenuType)
                         .OrderBy(x => x.Key)
                         .Select(g => g);

            var menuList = await menus.Select(e => new MenuInformation
            {
                MenuType = (MenuType)e.Key,
                Menus = e.Select(m => new MenuVm
                {
                    Title = m.Title,
                    ControllerName = m.ControllerName,
                    MenuId = m.MenuId,
                    Icon = m.Icon,
                    MenuType = (MenuType)m.MenuType,
                    Priority = m.Priority
                }).OrderBy(m => m.Priority).Distinct().ToList()
            }).ToListAsync();

            return menuList;
        }

        public async Task<UserVm> ChangePasswordAsync(UserChangePasswordVm model)
        {
            var user = await Context.Users.FirstOrDefaultAsync(e => e.Username == model.Username);

            if (user == null)
                throw new ArgumentException($"User or password not correct, try again.");

            if (!string.Equals(model.Password, user.Password) && model.Password != null)
                throw new ArgumentException($"User or password not correct, try again.");

            user.Password = model.NewPassword;

            await Context.SaveChangesAsync();

            return Mapper.Map<UserVm>(user);
        }

        private static string GenerateOTP()
        {
            Random random = new Random();
            int otp = random.Next(1000, 9999);

            return otp.ToString();
        }


        public async Task<IEnumerable<SelectListItem>> GetDropDownAsync(int? selected = 0)
        {
            var userTypes = await Context.UserTypes.ToListAsync();            

            return userTypes.Select(e => new SelectListItem
            {
                Text = e.UserTypeName,
                Value = e.UserTypeId.ToString(),
                Selected = e.UserTypeId == selected
            });
        }
        public async Task<IEnumerable<SelectListItem>> GetUnitDropDownAsync(int? selected = 0)
        {
            var setupUnits = await Context.SetupUnits.ToListAsync();

            return setupUnits.Select(e => new SelectListItem
            {
                Text = e.UnitName,
                Value = e.UnitId.ToString(),
                Selected = e.UnitId == selected
            });
        }
    }
}