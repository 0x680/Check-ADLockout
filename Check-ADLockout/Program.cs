using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using NDesk.Options;

class Program
{

    public static void Main(string[] args)
    {
        string username = null, domain = null;
        bool showhelp = false;

        OptionSet argParser = new OptionSet()
            .Add("u|username=", "Username to check lockout status for.", delegate (string v) { username = v; })
            .Add("d|domain=", "Domain which the user belongs to.", delegate (string v) { domain = v; })
            .Add("h|?|help", delegate (string v) { showhelp = (v != null); });

        argParser.Parse(args);

        if (!showhelp && !String.IsNullOrEmpty(username) && !String.IsNullOrEmpty(domain))
        {
            try
            {
                Console.WriteLine("Checking whether " + domain + "\\" + username + " is locked out..." + "\n");
                using (PrincipalContext ctx = new PrincipalContext(ContextType.Domain, domain))
                {
                    using (UserPrincipal usr = UserPrincipal.FindByIdentity(ctx, IdentityType.SamAccountName, username))
                    {
                        Console.WriteLine("Account locked out = " + usr.IsAccountLockedOut() + "\n");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\n");
            }
        }

        else
        {
            Console.WriteLine("Usage: Check-ADLockout.exe [OPTION]...");
            Console.WriteLine("A tool to check whether an AD user is locked out.");
            Console.WriteLine("Options:");
            argParser.WriteOptionDescriptions(Console.Out);
        }
    }
}
