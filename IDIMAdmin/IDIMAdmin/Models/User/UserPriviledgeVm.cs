using System;
using System.ComponentModel;

namespace IDIMAdmin.Models.User
{
    public class UserPriviledgeVm
    {
        [DisplayName("Id")]
        public int UserPriviledgeId { get; set; }

        [DisplayName("User")]
        public int UserId { get; set; }
        [DisplayName("User")]
        public string Username { get; set; }

        [DisplayName("Menu")]
        public int MenuId { get; set; }
        [DisplayName("Menu")]
        public string MenuName { get; set; }


        public int CreatedUser { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int? UpdatedUser { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public int UpdateNo { get; set; }

        public virtual MenuVm Menu { get; set; }
        public virtual UserVm User { get; set; }
    }
}