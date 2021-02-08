using Microsoft.SharePoint.Client;
using OfficeDevPnP.Core;
using OfficeDevPnP.Core.Framework.Provisioning.Model;
using OfficeDevPnP.Core.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace PnPSiteCore
{
    class Program
    {

        static void Main(string[] args)
        {
            //AppID 3a51724b-038d-4ae4-98e0-f317591af816
            //TenantID 894770f1-6b1b-43fc-aa6a-5d43106b3da9//
            //admin@M365x297302.onmicrosoft.com

            ConsoleColor defaultForeground = Console.ForegroundColor;

            // Collect information
            string targetWebUrl = GetInput("Enter the URL of the target site: ", false, defaultForeground);
            string userName = GetInput("Enter your user name:", false, defaultForeground);
            string pwdS = GetInput("Enter your password:", true, defaultForeground);
            SecureString pwd = new SecureString();
            foreach (char c in pwdS.ToCharArray()) pwd.AppendChar(c);
            AuthenticationManager am = new AuthenticationManager();
            using (var cc = am.GetSharePointOnlineAuthenticatedContextTenant(targetWebUrl, "admin@M365x297302.onmicrosoft.com", pwd))
            {
                #region using PnPsitecore starts
                var page = cc.Web.AddClientSidePage("newPage.aspx", true);
                ClientSideText txt1 = new ClientSideText() { Text = "Text Webpart" };
                page.AddControl(txt1, -1);
                page.Save();
                #endregion PnPsitecore
            }


            // Pause and modify the UI to indicate that the operation is complete
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("We're done. Press Enter to continue.");
            Console.ReadLine();
        }

        private static string GetInput(string label, bool isPassword, ConsoleColor defaultForeground)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("{0} : ", label);
            Console.ForegroundColor = defaultForeground;

            string value = "";

            for (ConsoleKeyInfo keyInfo = Console.ReadKey(true); keyInfo.Key != ConsoleKey.Enter; keyInfo = Console.ReadKey(true))
            {
                if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    if (value.Length > 0)
                    {
                        value = value.Remove(value.Length - 1);
                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                        Console.Write(" ");
                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                    }
                }
                else if (keyInfo.Key != ConsoleKey.Enter)
                {
                    if (isPassword)
                    {
                        Console.Write("*");
                    }
                    else
                    {
                        Console.Write(keyInfo.KeyChar);
                    }
                    value += keyInfo.KeyChar;
                }
            }
            Console.WriteLine("");

            return value;
        }
    }
}
