namespace IDIMAdmin.Models.User
{
	public class RoleAssignVm
    {
        public bool IsAssigned { get; set; }
        public int RoleId { get; set; }
        public string Name { get; set; }
        public int ApplicationId { get; set; }
    }
}