using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using IDIMAdmin.Models.Validation;

namespace IDIMAdmin.Models.Setup
{
    [Validator(typeof(SetupUnitVmValidator))]
    public class UnitVm
    {
        [Display(Name = "Id")]
        public int UnitId { get; set; }

        [Display(Name = "Name")]
        public string UnitName { get; set; }

        [Display(Name = "Name (Bangla)")]
        public string UnitNameB { get; set; }

        [Display(Name = "Sub Organization")]
        public int? SubOrganizationId { get; set; }

        [Display(Name = "Code")]
        public string UnitCode { get; set; }

        [Display(Name = "Full Name")]
        public string UnitFullName { get; set; }

        [Display(Name = "Core")]
        public int? CoreId { get; set; }
        [Display(Name = "Core")]
        public string CoreName { get; set; }

        [Display(Name = "Place")]
        public int? LocationId { get; set; }
        [Display(Name = "Place")]
        public string PlaceName { get; set; }

        [Display(Name = "Remark")]
        public string Remark { get; set; }

        [Display(Name = "Israb")]
        public int? Israb { get; set; }

        [Display(Name = "Sector")]
        public int? SectorId { get; set; }
        [Display(Name = "Sector")]
        public string SectorName { get; set; }

        [Display(Name = "Region")]
        public int? RegionId { get; set; }
        [Display(Name = "Region")]
        public string RegionName { get; set; }


        public int CreatedUser { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int? UpdatedUser { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public int UpdateNo { get; set; }
    }
}