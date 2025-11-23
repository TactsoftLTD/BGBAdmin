using IDIMAdmin.Models.User;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace IDIMAdmin.Services.User
{
	public interface IImageSlideService
    {
        Task<List<ImageSlideVm>> GetAllAsync();
        Task<ImageSlideVm> GetByIdAsync(int id);
        Task<ImageSlideVm> InsertAsync(ImageSlideVm model);
        Task<ImageSlideVm> UpdateAsync(ImageSlideVm model, string ImagePath, string OldImagePathDb);
        Task<ImageSlideVm> DeleteAsync(int id, string oldImagePath);
    }
}
