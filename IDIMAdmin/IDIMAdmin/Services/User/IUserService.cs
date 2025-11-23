using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using DataTables.Mvc;
using IDIMAdmin.Extentions.Collections.Select2;
using IDIMAdmin.Models.User;

namespace IDIMAdmin.Services.User
{
    public interface IUserService
    {
        Task<List<UserVm>> GetAllAsync(bool excludeNotActive = true);
        Task<UserVm> GetByIdAsync(int id, bool checkActive = false);
        Task<UserVm> InsertAsync(UserVm model);
        Task<UserVm> UpdateAsync(UserVm model);
        Task<UserVm> DeleteAsync(int id);

        //Task<IList<UserVm>> GetByDeviceIdAsync(int deviceId);
        Task<IList<UserVm>> GetByApplicationIdAsync(int applicationId);
        Task<List<UserVm>> GetUserByFilterAsync(UserSearchVm filter = null);
        Task<DataTablesResponse> GetByAsync(IDataTablesRequest requestModel, UserSearchVm filter);
        Task<Select2PagedResult> GetBySelect2Async(Select2Param param);
        Task<UserInformation> LoginAsync(string username, string password, bool? rememberMe = false);
        Task<UserInformation> AdLoginAsync(string username, string password, bool? rememberMe = false);
        Task<UserVm> ChangePasswordAsync(UserChangePasswordVm model);
        Task<bool> IsOtpValid(int userId, string otp);
        Task SaveOtpAndSendEmail(int userId);
        Task<IEnumerable<SelectListItem>> GetDropDownAsync(int? selected = 0);
        Task<IEnumerable<SelectListItem>> GetUnitDropDownAsync(int? selected = 0);
    }
}