using System.Collections.Generic;

namespace IDIMAdmin.Models.Dashboard
{
    public class DashboardVm
    {
        public DashboardVm()
        {
            Applications = new List<ApplicationDetailVm>();
            Regions= new List<RegionDetailVm>();
        }

        public int Application { get; set; }
        public int Device { get; set; }
        public int Menu { get; set; }
        public int User { get; set; }
        public int Report { get; set; }

        public int Regiment { get; set; }
        public int Region { get; set; }
        public int Battalion { get; set; }
        public int Sector { get; set; }

        public List<ApplicationDetailVm> Applications { get; set; }
        public List<RegionDetailVm> Regions { get; set; }
    }
}