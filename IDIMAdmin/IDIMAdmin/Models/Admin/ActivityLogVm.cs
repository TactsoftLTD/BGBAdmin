using System;
using System.ComponentModel;

namespace IDIMAdmin.Models.Admin
{
	public class ActivityLogVm
    {
        public int Id { get; set; }
        [DisplayName("User Id")]
        public int? UserId { get; set; }

        [DisplayName("Personnal Code")]
        public string PersonnelCode { get; set; }

        [DisplayName("Application Id")]
        public int? ApplicationId { get; set; }
        [DisplayName("Application Name")]
        public string ApplicationName { get; set; }
        [DisplayName("User Name")]
        public string UserName { get; set; }
        public int? UserRoleId { get; set; }
        [DisplayName("Session Id")]
        public string SessionId { get; set; }
        [DisplayName("Menu Name")]
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Url { get; set; }
        [DisplayName("Request Type")]
        public string RequestType { get; set; }
        [DisplayName("Activity Data")]
        public string ActivityData { get; set; }
        public string Agent { get; set; }
        public string Browser { get; set; }
        [DisplayName("Activity Time")]
        public DateTime? ActivityTime { get; set; }
        public string DeviceName { get; set; }
        public string DeviceMac { get; set; }
        [DisplayName("Activity Detail")]
        public string ActivityDescription { get; set; }
    }
}