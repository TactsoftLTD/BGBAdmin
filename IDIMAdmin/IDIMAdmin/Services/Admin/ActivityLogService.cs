using AutoMapper;

using IDIMAdmin.Entity;
using IDIMAdmin.Models.Admin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace IDIMAdmin.Services.User
{
	public class ActivityLogService : IActivityLogService
    {
        protected IMapper Mapper { get; set; }
        protected IDIMDBEntities Context { get; set; }

        public ActivityLogService(IMapper mapper)
        {
            Mapper = mapper;
            Context = new IDIMDBEntities();
        }

        public void InsertAsync(ActivityLogVm model)
        {
            var entity = Mapper.Map<ActivityLog>(model);
            entity.Url = entity.Url.Length >= 100 ? entity.Url.Substring(0, 100) : entity.Url;

            Context.ActivityLogs.Add(entity);
            try
            {
                // Save changes to the database
                Context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Iterate through each entity validation error
                foreach (var entityValidationError in ex.EntityValidationErrors)
                {
                    // Get the entity name
                    string entityName = entityValidationError.Entry.Entity.GetType().Name;

                    // Iterate through each validation error for the entity
                    foreach (var validationError in entityValidationError.ValidationErrors)
                    {
                        // Get the error message and property name
                        string errorMessage = validationError.ErrorMessage;
                        string propertyName = validationError.PropertyName;
                    }
                }
            }
        }

        public async Task<object> GetAllAsync(ActivityLogPaginationSearchVm filter)
        {
            // Normalize search term for global search
            var searchTerm = string.IsNullOrWhiteSpace(filter.SearchValues)
                ? null
                : filter.SearchValues.Trim().ToLower();

            // Base query
            var query = from al in Context.ActivityLogs
                        join u in Context.Users on al.UserId equals u.UserId into joinedU
                        from resultU in joinedU.DefaultIfEmpty()
                        join a in Context.Applications on al.ApplicationId equals a.ApplicationId
                        select new
                        {
                            al,
                            UserName = resultU.Username,
                            ApplicationName = a.ApplicationName
                        };

            // ✅ Global search filter (UserId, UserName, Controller, Action, ApplicationName, RequestType)
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(x =>
                    (x.UserName != null && x.UserName.ToLower().Contains(searchTerm)) ||
                    x.al.UserId.ToString().Contains(searchTerm) ||
                    (x.al.Controller != null && x.al.Controller.ToLower().Contains(searchTerm)) ||
                    (x.al.Action != null && x.al.Action.ToLower().Contains(searchTerm)) ||
                    (x.ApplicationName != null && x.ApplicationName.ToLower().Contains(searchTerm)) ||
                    (x.al.RequestType != null && x.al.RequestType.ToLower().Contains(searchTerm))|| (x.al.PersonnelCode != null && x.al.PersonnelCode.ToLower().Contains(searchTerm))
                );
            }

            // Optional filters
            if (filter.UserName != null)
            {
                query = query.Where(x => x.al.User.Username == filter.UserName);
            }

            if (filter.ApplicationId != null)
            {
                query = query.Where(x => x.al.ApplicationId == filter.ApplicationId);
            }

            if (filter.StartDate.HasValue)
            {
                query = query.Where(x =>
                    DbFunctions.TruncateTime(x.al.ActivityTime) >= DbFunctions.TruncateTime(filter.StartDate));
            }

            if (filter.EndDate.HasValue)
            {
                query = query.Where(x =>
                    DbFunctions.TruncateTime(x.al.ActivityTime) <= DbFunctions.TruncateTime(filter.EndDate));
            }

            // Total count before pagination
            var total = await query.CountAsync();

            // Pagination (DataTables usually uses start/length)
            var pageIndex = Math.Max(0, filter.PageIndex);
            var pageSize = filter.PageSize > 0 ? filter.PageSize : 10;

            var pageData = await query
                .OrderByDescending(x => x.al.Id)
                .Skip(pageIndex)
                .Take(pageSize)
                .ToListAsync();

            // Projection
            var data = pageData.Select(x => new ActivityLogVm
            {
                Id = x.al.Id,
                ApplicationId = x.al.ApplicationId,
                ApplicationName = x.ApplicationName,
                UserId = x.al.UserId,
                UserName = x.UserName ?? "Not Found",
                UserRoleId = x.al.UserRoleId,
                SessionId = x.al.SessionId,
                PersonnelCode=x.al.PersonnelCode,
                Controller = x.al.Controller,
                Action = x.al.Action,
                Url = x.al.Url,
                RequestType = x.al.RequestType,
                ActivityData = x.al.ActivityData,
                Agent = x.al.Agent,
                Browser = x.al.Browser,
                ActivityTime = x.al.ActivityTime,
                DeviceMac = x.al.DeviceMac,
                DeviceName = x.al.DeviceName,
                ActivityDescription = (x.al.RequestType == "GET")
                    ? $"User accessed the {x.al.Action} page"
                    : $"User submitted the {x.al.Action} form"
            }).ToList();

            // Return to DataTables
            return new
            {
                recordsTotal = total,
                recordsFiltered = total,
                data = data.Select(record => new string[]
                {
            record.Id.ToString(),
            record.UserName,
            record.PersonnelCode,
            record.ApplicationName,
            record.Controller,
            record.RequestType,
            record.ActivityTime?.ToString("yyyy-MM-dd HH:mm:ss") ?? "",
            record.ActivityDescription,
            record.Id.ToString(),
            record.UserId.ToString(),
            record.ApplicationId.ToString(),
            record.UserRoleId.ToString(),
            record.SessionId,
            record.Action,
            record.Url,
            record.ActivityData,
            record.Agent,
            record.Browser,
            record.DeviceMac,
            record.DeviceName
                }).ToArray()
            };
        }




        //public async Task<object> GetAllAsync(ActivityLogPaginationSearchVm filter)
        //{
        //    // Normalize search term
        //    var userNameSearch = string.IsNullOrWhiteSpace(filter.UserName)
        //        ? null
        //        : filter.UserName.Trim().ToLower();

        //    // Base query
        //    var query = from al in Context.ActivityLogs
        //                join u in Context.Users on al.UserId equals u.UserId into joinedU
        //                from resultU in joinedU.DefaultIfEmpty()
        //                join a in Context.Applications on al.ApplicationId equals a.ApplicationId
        //                select new
        //                {
        //                    al,
        //                    UserName = resultU.Username,
        //                    ApplicationName = a.ApplicationName
        //                };

        //    // ✅ Case-insensitive username filter
        //    if (!string.IsNullOrEmpty(userNameSearch))
        //    {
        //        query = query.Where(x => x.UserName != null &&
        //                                 x.UserName.ToLower().Contains(userNameSearch));
        //    }

        //    // Optional filters
        //    if (filter.ApplicationId != null)
        //    {
        //        query = query.Where(x => x.al.ApplicationId == filter.ApplicationId);
        //    }

        //    if (filter.StartDate.HasValue)
        //    {
        //        query = query.Where(x =>
        //            DbFunctions.TruncateTime(x.al.ActivityTime) >= DbFunctions.TruncateTime(filter.StartDate));
        //    }

        //    if (filter.EndDate.HasValue)
        //    {
        //        query = query.Where(x =>
        //            DbFunctions.TruncateTime(x.al.ActivityTime) <= DbFunctions.TruncateTime(filter.EndDate));
        //    }

        //    // Total count before pagination
        //    var total = await query.CountAsync();

        //    // Pagination (DataTables usually uses start/length)
        //    var pageIndex = Math.Max(0, filter.PageIndex);
        //    var pageSize = filter.PageSize > 0 ? filter.PageSize : 10;

        //    // Page query
        //    var pageData = await query
        //        .OrderByDescending(x => x.al.Id)
        //        .Skip(pageIndex)
        //        .Take(pageSize)
        //        .ToListAsync();

        //    // Projection
        //    var data = pageData.Select(x => new ActivityLogVm
        //    {
        //        Id = x.al.Id,
        //        ApplicationId = x.al.ApplicationId,
        //        ApplicationName = x.ApplicationName,
        //        UserId = x.al.UserId,
        //        UserName = x.UserName ?? "Not Found",
        //        UserRoleId = x.al.UserRoleId,
        //        SessionId = x.al.SessionId,
        //        Controller = x.al.Controller,
        //        Action = x.al.Action,
        //        Url = x.al.Url,
        //        RequestType = x.al.RequestType,
        //        ActivityData = x.al.ActivityData,
        //        Agent = x.al.Agent,
        //        Browser = x.al.Browser,
        //        ActivityTime = x.al.ActivityTime,
        //        DeviceMac = x.al.DeviceMac,
        //        DeviceName = x.al.DeviceName,
        //        ActivityDescription = (x.al.RequestType == "GET")
        //            ? $"User accessed the {x.al.Action} page"
        //            : $"User submitted the {x.al.Action} form"
        //    }).ToList();

        //    // Return to DataTables
        //    return new
        //    {
        //        recordsTotal = total,
        //        recordsFiltered = total,
        //        data = data.Select(record => new string[]
        //        {
        //    record.Id.ToString(),
        //    record.UserName,
        //    record.ApplicationName,
        //    record.Controller,
        //    record.RequestType,
        //    record.ActivityTime?.ToString("yyyy-MM-dd HH:mm:ss") ?? "",
        //    record.ActivityDescription,
        //    record.Id.ToString(),
        //    record.UserId.ToString(),
        //    record.ApplicationId.ToString(),
        //    record.UserRoleId.ToString(),
        //    record.SessionId,
        //    record.Action,
        //    record.Url,
        //    record.ActivityData,
        //    record.Agent,
        //    record.Browser,
        //    record.DeviceMac,
        //    record.DeviceName
        //        }).ToArray()
        //    };
        //}        

        private string GenerateActivityMessage(string request, string action)
        {
            if (request == "get")
            {
                return "User accessed the " + action + " page";
            }
            else
            {
                return "User submitted the " + action + " form";
            }
        }

        public async Task<List<ActivityLogVm>> GetUserByFilterAsync(ActivityLogSearchVm filter)
        {
            if (filter == null)
                filter = new ActivityLogSearchVm();

            var query = await (from al in Context.ActivityLogs
                               join u in Context.Users on al.UserId equals u.UserId into joinedU
                               from resultU in joinedU.DefaultIfEmpty()
                               join a in Context.Applications on al.ApplicationId equals a.ApplicationId
                               where ((filter.UserId == null || filter.UserId == al.UserId) &&
                                      (filter.ApplicationId == null || filter.ApplicationId == al.ApplicationId) &&
                                      ((!filter.StartDate.HasValue && !filter.EndDate.HasValue) ||
                                       (filter.StartDate.HasValue && !filter.EndDate.HasValue &&
                                       DbFunctions.TruncateTime(al.ActivityTime) >= DbFunctions.TruncateTime(filter.StartDate)) ||
                                       (!filter.StartDate.HasValue && filter.EndDate.HasValue &&
                                       DbFunctions.TruncateTime(al.ActivityTime) <= DbFunctions.TruncateTime(filter.EndDate)) ||
                                       (filter.StartDate.HasValue && filter.EndDate.HasValue &&
                                       DbFunctions.TruncateTime(al.ActivityTime) >= DbFunctions.TruncateTime(filter.StartDate) &&
                                       DbFunctions.TruncateTime(al.ActivityTime) <= DbFunctions.TruncateTime(filter.EndDate))
                                       ))
                               select new ActivityLogVm
                               {
                                   Id = al.Id,
                                   ApplicationId = al.ApplicationId,
                                   ApplicationName = a.ApplicationName,
                                   UserId = al.UserId,
                                   UserName = resultU != null ? resultU.Username : "Not Found",
                                   UserRoleId = al.UserRoleId,
                                   SessionId = al.SessionId,
                                   Controller = al.Controller,
                                   Action = al.Action,
                                   Url = al.Url,
                                   RequestType = al.RequestType,
                                   ActivityData = al.ActivityData,
                                   Agent = al.Agent,
                                   Browser = al.Browser,
                                   ActivityTime = al.ActivityTime,
                                   DeviceMac = al.DeviceMac,
                                   DeviceName = al.DeviceName
                               }).ToListAsync();

            return Mapper.Map<List<ActivityLogVm>>(query);
        }

        public async Task<ActivityLogVm> GetByUserIdAsync(int Id)
        {
            var list = await (from al in Context.ActivityLogs
                              join u in Context.Users on al.UserId equals u.UserId into joinedU
                              from resultU in joinedU.DefaultIfEmpty()
                              join a in Context.Applications on al.ApplicationId equals a.ApplicationId
                              where al.Id == Id
                              select new ActivityLogVm
                              {
                                  Id = al.Id,
                                  ApplicationId = al.ApplicationId,
                                  ApplicationName = a.ApplicationName,
                                  UserId = al.UserId,
                                  UserName = resultU != null ? resultU.Username : "Not Found",
                                  UserRoleId = al.UserRoleId,
                                  SessionId = al.SessionId,
                                  Controller = al.Controller,
                                  Action = al.Action,
                                  Url = al.Url,
                                  RequestType = al.RequestType,
                                  ActivityData = al.ActivityData,
                                  Agent = al.Agent,
                                  Browser = al.Browser,
                                  ActivityTime = al.ActivityTime,
                                  DeviceMac = al.DeviceMac,
                                  DeviceName = al.DeviceName,
                                  ActivityDescription = (al.RequestType == "GET") ? "User accessed the " + al.Action + " page" : "User submitted the " + al.Action + " form"
                              }).FirstOrDefaultAsync();

            return Mapper.Map<ActivityLogVm>(list);
        }
    }
}