namespace IDIMAdmin.Extentions.Client
{
    public class SmsClientExtention
    {
        public string BaseUrl { get; set; }
        public string Username { get; set; }
        
        public SmsClientExtention(string baseUrl, string username, string password)
        {
            BaseUrl = baseUrl;
            Username = username;


        }
    }
}