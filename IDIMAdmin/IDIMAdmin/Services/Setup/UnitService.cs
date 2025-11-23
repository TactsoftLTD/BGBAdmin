using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using IDIMAdmin.Entity;
using IDIMAdmin.Extentions.Session;
using IDIMAdmin.Models.Setup;

namespace IDIMAdmin.Services.Setup
{
    public class UnitService : IUnitService
    {
        protected IDIMDBEntities Context { get; set; }
        protected IMapper Mapper { get; set; }

        public UnitService(IMapper mapper)
        {
            Context = new IDIMDBEntities();
            Mapper = mapper;
        }
        public async Task<List<UnitVm>> GetAllAsync()
        {
            var list = await Context.SetupUnits.ToListAsync();

            return Mapper.Map<List<UnitVm>>(list);
        }

        public async Task<UnitVm> GetByIdAsync(int id)
        {
            var entity = await Context.SetupUnits.FindAsync(id);

            return Mapper.Map<UnitVm>(entity);
        }

        public async Task<UnitVm> InsertAsync(UnitVm model)
        {
            var existing = await Context.SetupUnits.Where(e => e.UnitName == model.UnitName).FirstOrDefaultAsync();

            if (existing != null)
                throw new ArgumentException($"Name already exists.");

            var entity = Mapper.Map<SetupUnit>(model);
            entity.CreatedDateTime = DateTime.Now;
            entity.CreatedUser = UserExtention.GetUserId();

            var added = Context.SetupUnits.Add(entity);
            await Context.SaveChangesAsync();

            return Mapper.Map<UnitVm>(added);
        }

        public async Task<UnitVm> UpdateAsync(UnitVm model)
        {
            var existing = await Context.SetupUnits.Where(e => e.UnitId == model.UnitId).FirstOrDefaultAsync();

            if (existing == null)
                throw new ArgumentException("Unit does not exists.");

            var duplicate = await Context.SetupUnits
                .Where(e => e.UnitId != model.UnitId)
                .FirstOrDefaultAsync(e => e.UnitName == model.UnitName);

            if (duplicate != null)
                throw new ArgumentException("Name already exists.");

            existing.UnitName = model.UnitName;
            existing.UnitNameB = model.UnitNameB;
            existing.SubOrganizationId = model.SubOrganizationId;
            existing.UnitCode = model.UnitCode;
            existing.UnitFullName = model.UnitFullName;
            existing.CoreId = model.CoreId;
            //existing.loc = model.LocationId;
            existing.Remark = model.Remark;
            existing.Israb = model.Israb;
            existing.SectorId = model.SectorId;
            existing.RegionId = model.RegionId;
            existing.UpdatedDateTime = DateTime.Now;
            existing.UpdatedUser = UserExtention.GetUserId();
            existing.UpdateNo += 1;

            await Context.SaveChangesAsync();

            return Mapper.Map<UnitVm>(existing);
        }

        public async Task<UnitVm> DeleteAsync(int id)
        {
            var existing = await Context.SetupUnits.Where(e => e.UnitId == id).FirstOrDefaultAsync();

            if (existing == null)
                throw new ArgumentException($"Unit does not exists.");

            Context.SetupUnits.Remove(existing);
            await Context.SaveChangesAsync();

            return Mapper.Map<UnitVm>(existing);
        }

        public async Task<IEnumerable<SelectListItem>> GetDropDownAsync(int? selected = 0, UnitType? type = null, int? id = null)
        {
            var query = Context.SetupUnits.AsQueryable();

            switch (type)
            {
                case null:
                    break;
                case UnitType.Region:
                    query = query.Where(e => e.UnitName.Contains(UnitType.Region.ToString()));
                    break;
                case UnitType.Shq:
                    query = query.Where(e => e.UnitName.Contains(UnitType.Shq.ToString()) && (!id.HasValue || e.RegionId == id));
                    break;
                case UnitType.Bgb:
                    query = query.Where(e => e.UnitName.Contains(UnitType.Bgb.ToString()) && (!id.HasValue || e.SectorId == id));
                    break;
            }

            var units = await query.ToListAsync();

            return units.Select(e => new SelectListItem
            {
                Text = e.UnitName,
                Value = e.UnitId.ToString(),
                Selected = e.UnitId == selected
            });
        }

        public async Task<IEnumerable<SelectListItem>> GetRegionDropdownAsync(int? selected = 0)
        {
            return await GetDropDownAsync(selected, UnitType.Region);
        }

        public async Task<IEnumerable<SelectListItem>> GetSectorDropdownAsync(int? selected = 0, int? id = null)
        {
            return await GetDropDownAsync(selected, UnitType.Shq, id);
        }

        public async Task<IEnumerable<SelectListItem>> GetBattalionDropdownAsync(int? selected = 0, int? id = null)
        {
            return await GetDropDownAsync(selected, UnitType.Bgb, id);
        }

        //public async Task<SelectList> GetSelectAsync(int? selected = 0)
        //{
        //    var units = await Context.SetupUnits.Select(e=>new
        //    {
        //        e.UnitId,
        //        e.UnitName,
        //        e.SetupRegion.RegionName
        //    }).ToListAsync();
        //    selected = units.FirstOrDefault(e => e.UnitId == selected)?.UnitId;
        //    var data = new SelectList(units, "UnitId", "UnitName", "RegionName", selected);
        //    return new SelectList(units, "UnitId", "UnitName", "RegionName", selected);
        //}
    }
}