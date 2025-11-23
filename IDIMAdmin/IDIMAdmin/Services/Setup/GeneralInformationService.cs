using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using IDIMAdmin.Entity;
using IDIMAdmin.Extentions;
using IDIMAdmin.Extentions.Collections.Select2;
using IDIMAdmin.Models.Setup;

namespace IDIMAdmin.Services.Setup
{
    public class GeneralInformationService : IGeneralInformationService
    {
        protected IDIMDBEntities Context { get; set; }
        protected IMapper Mapper { get; set; }

        public GeneralInformationService(IMapper mapper)
        {
            Context = new IDIMDBEntities();
            Mapper = mapper;
        }

        public IQueryable<GeneralInformation> GetAll()
        {
            return Context.GeneralInformations.OrderByDescending(e => e.RegimentNo).AsQueryable();
        }

        public async Task<List<GeneralInformationVm>> GetAllAsync(GeneralInformationSearchVm filter = null)
        {
            var query = GetAll();

            if (filter != null)
            {
                query = GetAll().Where(x => (string.IsNullOrEmpty(filter.RegimentNo) || x.Name.Contains(filter.RegimentNo)) &&
                                (string.IsNullOrEmpty(filter.Name) || x.Name.Contains(filter.Name)) &&
                                (string.IsNullOrEmpty(filter.BjoNo) || x.BjoNo.Contains(filter.BjoNo)) &&
                                (string.IsNullOrEmpty(filter.RdoNo) || x.RdoNo.Contains(filter.RdoNo)) &&
                                (string.IsNullOrEmpty(filter.Phone1) || x.Phone1.Contains(filter.Phone1)) &&
                                (string.IsNullOrEmpty(filter.Phone2) || x.Phone2.Contains(filter.Phone2)) &&
                                (string.IsNullOrEmpty(filter.NationalId) || x.NationalId.Contains(filter.NationalId)) &&
                                x.IsActiveStatus == filter.IsActiveStatus &&
                                (!filter.RankId.HasValue || x.RankId == filter.RankId.Value) &&
                                (!filter.UnitId.HasValue || x.UnitId == filter.UnitId));
            }

            query = query.Take(DefaultData.Take);

            var list = await query.ToListAsync();

            return Mapper.Map<List<GeneralInformationVm>>(list);
        }

        public async Task<GeneralInformationVm> GetByIdAsync(int? id)
        {
            var generalInformation = await Context.GeneralInformations.FirstOrDefaultAsync(e => e.ArmyId == id);

            return Mapper.Map<GeneralInformationVm>(generalInformation);
        }

        public async Task<IEnumerable<SelectListItem>> GetDropdownAsync(int? selected = 0)
        {
            var regiments = await Context.GeneralInformations.Where(e => e.ArmyId == selected).ToListAsync();

            return regiments.Select(e => new SelectListItem
            {
                Text = e.RegimentNo,
                Value = e.ArmyId.ToString(),
                Selected = e.ArmyId == selected
            });
        }

        public async Task<Select2PagedResult> GetBySelect2Async(Select2Param param)
        {
            var select2 = new Select2PagedResult();

            var query = Context.GeneralInformations.AsQueryable();

            if (!string.IsNullOrEmpty(param.Term))
                query = query.Where(e => e.RegimentNo.Contains(param.Term));

            var list = await query.OrderBy(e => e.RegimentNo).Take(20).ToListAsync();

            select2.Results = list.Select(e => new Select2Result
            {
                id = e.ArmyId.ToString(),
                text = e.RegimentNo
            }).ToList();

            return select2;
        }
    }
}