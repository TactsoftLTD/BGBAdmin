using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace IDIMAdmin.Models.Setup
{
	public class GeneralInformationSearchVm
    {
        public GeneralInformationSearchVm()
        {
            GeneralInformationDropdown = new List<SelectListItem>();
            RankDropdown = new List<SelectListItem>();
            UnitDropdown = new List<SelectListItem>();
            TradeDropdown = new List<SelectListItem>();
            GeneralInformations = new List<GeneralInformationVm>();
        }

        public string RegimentNo { get; set; }
        public string Name { get; set; }
        [DisplayName("BJO No.")]
        public string BjoNo { get; set; }
        [DisplayName("RDO No.")]
        public string RdoNo { get; set; }
        [DisplayName("Rank")]
        public int? RankId { get; set; }
        [DisplayName("Unit")]
        public int? UnitId { get; set; }
        public int? TradeId { get; set; }

        [DisplayName("Mobile No (Personal)")]
        public string Phone1 { get; set; }
        [DisplayName("Mobile No. (NOK)")]
        public string Phone2 { get; set; }
        [DisplayName("NID")]
        public string NationalId { get; set; }

        [DisplayName("Status")]
        public int? IsActiveStatus { get; set; }

        public IList<GeneralInformationVm> GeneralInformations { get; set; }
        public IEnumerable<SelectListItem> GeneralInformationDropdown { get; set; }
        public IEnumerable<SelectListItem> RankDropdown { get; set; }
        public IEnumerable<SelectListItem> UnitDropdown { get; set; }
        public IEnumerable<SelectListItem> TradeDropdown { get; set; }

    }
}