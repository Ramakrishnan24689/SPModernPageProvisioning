using System;
using System.Net;
using System.Security;
using System.Threading;
using Microsoft.SharePoint.Client;
using PnP.Core.Model.SharePoint;
using PnP.Framework;
using PnP.Framework.Modernization;
using PnP.Framework.Provisioning.Model;

namespace SPModernPageProvisioning
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

            #region using PnPFramework starts
            var authManager = new PnP.Framework.AuthenticationManager("3a51724b-038d-4ae4-98e0-f317591af816", "admin@M365x297302.onmicrosoft.com", pwd);

            using (var context = authManager.GetContext(targetWebUrl))
            {
                var myPage = context.Web.AddClientSidePage("myPage.aspx", true);
                myPage.Save();
            }
            #endregion PnPFrameworkends


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
