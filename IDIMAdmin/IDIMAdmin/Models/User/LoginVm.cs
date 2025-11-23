namespace IDIMAdmin.Models.User
{
	public class LoginVm
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public string Avatar { get; set; }
    }
}