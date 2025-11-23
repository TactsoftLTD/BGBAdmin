using System;
using System.DirectoryServices;

namespace IDIMAdmin.Extentions.Ad
{
    public class LdapExtention
    {
        public string AdServer { get; set; }
        public string AdPort { get; set; }
        public string AdUsername { get; set; }
        public string LdapConnectionString { get; set; }
        public DirectoryEntry Entry { get; set; }
        
        public LdapExtention(string adServer, string adUsername, string adPassword)
        {
            AdServer = adServer;
            LdapConnectionString = $"LDAP://{adServer}";

            Entry = new DirectoryEntry(LdapConnectionString, adUsername, adPassword);
        }

        public bool IsAuthenticated()
        {
            var authenticated = false;

            try
            {
                var nativeObject = Entry.NativeObject;
                authenticated = true;
            }
            catch (Exception)
            {
                // ignored
            }

            return authenticated;
        }
    }
}

