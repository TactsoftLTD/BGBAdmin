namespace IDIMAdmin.Models.User
{
    public class LoginPinVm
    {
        public string[] PinCode { get; set; }
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }

    }
}