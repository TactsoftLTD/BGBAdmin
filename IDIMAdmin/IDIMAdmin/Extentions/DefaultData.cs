using System;
using System.Collections.Generic;
using System.Configuration;

namespace IDIMAdmin.Extentions
{
    public static class DefaultData
    {
        public static string ApplicationName = "IDIMAdmin";
        public static int ApplicationId = 10;

        public static bool AdEnable = ConfigurationManager.AppSettings["ADEnable"].ToBool();
        public static bool SmsEnable = ConfigurationManager.AppSettings["SmsEnable"].ToBool();
        public static bool OtpEnable = ConfigurationManager.AppSettings["OtpEnable"].ToBool();
        public static string LockTime = ConfigurationManager.AppSettings["LockTime"];
        public static string AdServer = ConfigurationManager.AppSettings["ADServer"];
        public static string AdUsername = ConfigurationManager.AppSettings["ADUsername"];
        public static string AdPassword = ConfigurationManager.AppSettings["ADPassword"];
        public static string FtpAvater = ConfigurationManager.AppSettings["FtpAvater"];
        public static string HttpAvatar = ConfigurationManager.AppSettings["HttpAvatar"];

        //Email Configuration
        public static string EmailServer = ConfigurationManager.AppSettings["EmailServer"];
        public static int EmailPort = Convert.ToInt32(ConfigurationManager.AppSettings["EmailPort"]);
        public static string EmailUserName = ConfigurationManager.AppSettings["EmailUserName"];
        public static string EmailPassword = ConfigurationManager.AppSettings["EmailPassword"];
        public static string EmailForm = ConfigurationManager.AppSettings["EmailForm"];
        public static string EmailSubject = ConfigurationManager.AppSettings["EmailSubject"];
        public static string EmailBody = ConfigurationManager.AppSettings["EmailBody"];
        public static int OTPExpiredTime = Convert.ToInt32(ConfigurationManager.AppSettings["OTPExpiredTime"]);

        public static string DefaultAvatar = "~/Content/Images/avatar.png";
        public static string AvatarLocation = "~/Content/Attachment";
        public static string DefaultDateFormat = "dd-MMM-yyyy";

        // default pagination data
        public static int Take = 1000;
        // generate menu except
        public static List<string> ExcludeMenu = new List<string>
        {
            "Landing", "Base", "Login",  "Account", "Manage", "Error", "Profile", "Setting", "Help", "ContactUs"
        };
    }
}