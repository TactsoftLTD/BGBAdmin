using AutoMapper;

using IDIMAdmin.Entity;

namespace IDIMAdmin.Services.User
{
	public class DeviceService : IDeviceService
    {
        protected IMapper Mapper { get; set; }
        protected IDIMDBEntities Context { get; set; }

        public DeviceService(IMapper mapper)
        {
            Mapper = mapper;
            Context = new IDIMDBEntities();
        }

        //public async Task<IList<DeviceVm>> GetAllAsync()
        //{
        //    var list = await Context.Devices.OrderBy(e=>e.DeviceName).ToListAsync();

        //    return Mapper.Map<IList<DeviceVm>>(list);
        //}

        //public async Task<DeviceVm> GetByIdAsync(int id)
        //{
        //    var entity = await Context.Devices.FindAsync(id);

        //    return Mapper.Map<DeviceVm>(entity);
        //}

        //public async Task<IList<DeviceVm>> GetByUserIdAsync(int userId)
        //{
        //    var list = await Context.UserDevices.Where(e => e.UserId == userId)
        //        .Select(e => e.Device)
        //        .ToListAsync();

        //    return Mapper.Map<IList<DeviceVm>>(list);
        //}

        //public async Task<DeviceVm> InsertAsync(DeviceVm model)
        //{
        //    var existing = await Context.Devices.FirstOrDefaultAsync(m => m.DeviceName == model.DeviceName);
        //    if (existing != null)
        //        throw new ArgumentException($"{nameof(model.DeviceName)} already exists ");

        //    var entity = Mapper.Map<Device>(model);
        //    entity.CreatedDateTime = DateTime.Now;
        //    entity.CreatedUser = UserExtention.GetUserId();

        //    var added = Context.Devices.Add(entity);
        //    await Context.SaveChangesAsync();

        //    return Mapper.Map<DeviceVm>(added);
        //}

        //public async Task<DeviceVm> UpdateAsync(DeviceVm model)
        //{
        //    var existing = await Context.Devices.FindAsync(model.DeviceId);

        //    if (existing == null)
        //        throw new ArgumentException($"Device does not exists.");

        //    var duplicate = await Context.Devices.Where(e => e.DeviceId != model.DeviceId).FirstOrDefaultAsync(e => e.DeviceName == model.DeviceName);

        //    if (duplicate != null)
        //        throw new ArgumentException($"Name already exists.");

        //    existing.DeviceName = model.DeviceName;
        //    existing.UpdatedDateTime = DateTime.Now;
        //    existing.UpdatedUser = UserExtention.GetUserId();
        //    existing.UpdateNo += 1;

        //    await Context.SaveChangesAsync();

        //    return Mapper.Map<DeviceVm>(existing);
        //}

        //public async Task<DeviceVm> DeleteAsync(int id)
        //{
        //    var existing = await Context.Devices.FindAsync(id);

        //    if (existing == null)
        //        throw new ArgumentException($"Device does not exists.");

        //    Context.Entry(existing).State = EntityState.Deleted;
        //    await Context.SaveChangesAsync();

        //    return Mapper.Map<DeviceVm>(existing);
        //}

       //public async Task<IEnumerable<SelectListItem>> GetDropDownAsync(int? selected = 0)
       // {
       //     var devices = await Context.Devices.OrderBy(e => e.DeviceName).ToListAsync();

       //     return devices.Select(e => new SelectListItem
       //     {
       //         Text = e.DeviceName,
       //         Value = e.DeviceId.ToString(),
       //         Selected = e.DeviceId == selected
       //     });
       // }
    }
}