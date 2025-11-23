using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using IDIMAdmin.Entity;
using IDIMAdmin.Models.Dashboard;
using IDIMAdmin.Models.Setup;

namespace IDIMAdmin.Services.Admin
{
    public class DashboardService : IDashboardService
    {
        protected IMapper Mapper { get; set; }
        protected IDIMDBEntities Context { get; set; }

        public DashboardService(IMapper mapper)
        {
            Context = new IDIMDBEntities();
            Mapper = mapper;
        }

        public async Task<List<ApplicationDetailVm>> Application()
        {
            var model = await Context.Applications.Select(e => new ApplicationDetailVm
            {
                ApplicationId = e.ApplicationId,
                ApplicationCode = e.ApplicationCode,
                ApplicationName = e.ApplicationName,
                User = (from up in Context.UserPrivileges
                        join r in Context.Roles on up.RoleId equals r.RoleId
                        where r.ApplicationId==e.ApplicationId
                        select up.UserId).Count(),
                Menu = Context.Menus.Count(m => m.ApplicationId == e.ApplicationId),
            }).OrderBy(e=>e.ApplicationCode).ToListAsync();

            return model;
        }

        public async Task<List<RegionDetailVm>> Region()
        {
            var model = await Context.SetupUnits.Where(e => e.UnitName.ToLower().Contains(UnitType.Region.ToString().ToLower())).Select(e => new RegionDetailVm()
            {
                RegionName = e.UnitName,
                RegionCode = e.UnitCode,
                Battalion = Context.SetupUnits.Count(b => b.UnitName.ToLower().Contains(UnitType.Bgb.ToString().ToLower()) && b.RegionId == e.RegionId),
                Sector = Context.SetupUnits.Count(b => b.UnitName.ToLower().Contains(UnitType.Bgb.ToString().ToLower()) && b.SectorId == e.SectorId)
            }).ToListAsync();

            return model;
        }

        public async Task<DashboardVm> GetAll()
        {
            var model = new DashboardVm
            {
                Menu = await Context.Menus.CountAsync(),
                //Device = await Context.Devices.CountAsync(),
                User = await Context.Users.CountAsync(),
                Report = 100,
                Application = await Context.Applications.CountAsync(),
                Applications = await Application(),
                Regiment = await Context.GeneralInformations.CountAsync(),
                Region = await Context.SetupUnits.CountAsync(e => e.UnitName.ToLower().Contains(UnitType.Region.ToString().ToLower())),
                Regions = (await Region()).Take(6).ToList(),
                Sector = await Context.SetupUnits.CountAsync(e => e.UnitName.ToLower().Contains(UnitType.Shq.ToString().ToLower())),
                Battalion = await Context.SetupUnits.CountAsync(e => e.UnitName.ToLower().Contains(UnitType.Bgb.ToString().ToLower())),
            };

            return model;
        }
    }
}