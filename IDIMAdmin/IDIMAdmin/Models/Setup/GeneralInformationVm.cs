using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace IDIMAdmin.Models.Setup
{
    public class GeneralInformationVm
    {
        public int ArmyId { get; set; }
        [DisplayName("Regiment No.")]
        public string RegimentNo { get; set; }
        [DisplayName("Regiment No. (Bengali)")]
        public string RegimentNoB { get; set; }
        [UIHint("Picture")]
        public string Picture { get; set; }
        // [DisplayName("Name")]
        public string Name { get; set; }
        [DisplayName("Name (Bengali)")]
        public string NameB { get; set; }
        [DisplayName("JCO No.")]
        public string BjoNo { get; set; }
        [DisplayName("Current Unit")]
        public int? UnitId { get; set; }
        [DisplayName("Birth Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? BirthDate { get; set; }
        public string Age { get; set; }

        [DisplayName("Enrollment Date")]
        [DataType(DataType.Date)]
        public DateTime? EnrollmentDate { get; set; }
        [DisplayName("Service Date")]
        public string ServiceDate { get; set; }




        [DisplayName("Father's Name")]
        public string FatherName { get; set; }
        [DisplayName("Father's Name (Bengali)")]
        public string FatherNameB { get; set; }
        [DisplayName("Is Father BGB Member")]
        public bool IsFatherBgb { get; set; }
        [DisplayName("If Father BGB")]
        public string FatherRegimentNo { get; set; }
        [DisplayName("Fathers Unit")]
        public int? ParentUnitId { get; set; }
        [DisplayName("Mother Name")]
        public string MotherName { get; set; }
        [DisplayName("Mother Name (Bengali)")]
        public string MotherNameB { get; set; }

        [DisplayName("Mobile No (Personal)")]
        public string Phone1 { get; set; }
        [DisplayName("Mobile No. (NOK)")]
        public string Phone2 { get; set; }
        public string Phone3 { get; set; }
        [DisplayName("NID / Voter Id")]
        public string NationalId { get; set; }
        [DisplayName("Freedom Fighter")]
        public bool IsFreedomFighter { get; set; }
        [DisplayName("Rank")]
        public int? RankId { get; set; }
        [DisplayName("Trade")]
        public int? TradeId { get; set; }
        [DisplayName("Blood Group")]
        public int? BloodGroupId { get; set; }
        [DisplayName("Medical Category")]
        public int? MedicalCategoryId { get; set; }
        [DisplayName("Batch")]
        public int? BatchId { get; set; }
        [DisplayName("Education")]
        public int? EducationId { get; set; }

        [DisplayName("Present Rank")]
        [DataType(DataType.Date)]
        public DateTime? PresentRankDate { get; set; }
        public int? EmploymentTypeIdR { get; set; }
        public int? ServiceTermsIdR { get; set; }
        [DisplayName("Core")]
        public int? CoreId { get; set; }
        public int? PoliceVarificationStatusR { get; set; }

        [DisplayName("Current District")]
        public int? PresentDistrictId { get; set; }



        [DisplayName("Location")]
        public int? LocationId { get; set; }




        [DisplayName("Marital Status")]
        public int? MaritalStatusId { get; set; }

        [DisplayName("Marriage Date")]
        [DataType(DataType.Date)]
        public DateTime? MarriageDate { get; set; }
        public int? PreviousBasicR { get; set; }
        [DisplayName("Current Basic")]
        public int? CurrentBasic { get; set; }

        [DisplayName("Latest SalaryFixation Date")]
        [DataType(DataType.Date)]

        public DateTime? NextIncrementDate { get; set; }

        [DisplayName("Expected Date Retirement")]
        [DataType(DataType.Date)]
        public DateTime? ExpectedDateRetirement { get; set; }
        [DisplayName("Pay Grade date")]
        [DataType(DataType.Date)]
        public DateTime? PayGradeDate { get; set; }
        [DisplayName("Religion")]
        public int? ReligionId { get; set; }
        [DisplayName("JCO No(Bengali)")]
        public string BjoNoB { get; set; }
        [DisplayName("BGDO No.")]
        public string RdoNo { get; set; }
        [DisplayName("BGDO No. (Bengali)")]
        public string RdoNoB { get; set; }
        [DisplayName("Status")]
        public int IsActiveStatus { get; set; }

        [DisplayName("Mission Status")]
        public string MissionStatus { get; set; }
        public string ClassR { get; set; }
        public string CastR { get; set; }
        [DisplayName("GPF No.")]
        public string GpfNo { get; set; }
        public DateTime? PostingDate { get; set; }

        [DisplayName("Injured in Liberation war")]
        public bool IsInjuredLiberation { get; set; }
        [DisplayName("Civil")]
        public bool IsCivil { get; set; }
        [DisplayName("CS")]
        public bool IsCs { get; set; }
        [DisplayName("IsTribal")]
        public bool IsTribal { get; set; }


        [DisplayName("IsCSTTribal")]
        public bool IsCSTTribal { get; set; }
        public int Gender { get; set; }
        public int CreatedUser { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int? UpdatedUser { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public int UpdateNo { get; set; }

        [DisplayName("Rank")]
        public string RankName { get; set; }
        [DisplayName("Trade")]
        public string TradeName { get; set; }
        [DisplayName("Batch")]
        public string BatchName { get; set; }
        [DisplayName("Fathers Unit Name")]
        public string ParentUnitName { get; set; }
        [DisplayName("Unit Name")]
        public string UnitName { get; set; }
        [DisplayName("Medical Category")]
        public string MedicalCategoryName { get; set; }
        [DisplayName("Blood Group Category")]
        public string BloodGroupName { get; set; }
        [DisplayName("Education")]
        public string EducationName { get; set; }
        [DisplayName("Core")]
        public string CoreName { get; set; }
        [DisplayName("Present District")]
        public string PresentDistrictName { get; set; }
        [DisplayName("Marital Status")]
        public string MaritalStatusName { get; set; }
        [DisplayName("Religion")]
        public string ReligionName { get; set; }
        [DisplayName("Status")]
        public string JobStatus { get; set; }

        public IEnumerable<SelectListItem> BatchDropdown { get; set; }
        public IEnumerable<SelectListItem> BloodDropdown { get; set; }
        public IEnumerable<SelectListItem> CoreDropdown { get; set; }
        public IEnumerable<SelectListItem> DistrictDropdown { get; set; }
        public IEnumerable<SelectListItem> LocationDropdown { get; set; }
        public IEnumerable<SelectListItem> EducationDropdown { get; set; }
        public IEnumerable<SelectListItem> MaritalStatusDropdown { get; set; }
        public IEnumerable<SelectListItem> MedicalCategoryDropdown { get; set; }
        public IEnumerable<SelectListItem> RankDropdown { get; set; }
        public IEnumerable<SelectListItem> TradeDropdown { get; set; }
        public IEnumerable<SelectListItem> UnitDropdown { get; set; }
        public IEnumerable<SelectListItem> ReligionDropdown { get; set; }
        public IEnumerable<SelectListItem> StatusDropdown { get; set; }
        public IEnumerable<SelectListItem> GenderDropdown { get; set; }
    }
}