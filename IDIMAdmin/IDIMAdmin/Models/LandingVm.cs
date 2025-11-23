using IDIMAdmin.Models.User;

using System.Collections.Generic;

namespace IDIMAdmin.Models
{
	public class LandingVm
    {
        public IList<ApplicationVm> Applications { get; set; }
        public IList<ImageSlideVm> ImageSlides { get; set; }
    }
}