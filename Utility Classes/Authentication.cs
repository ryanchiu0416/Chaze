using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Chaze.Utility_Classes
{
    static class Authentication
    {

        public static bool Authenticate(string username, string pass)
        {
            //process authentication info
            bool AuthenticateSuccess = ChecksAuthenticInfo(username, pass);

            return AuthenticateSuccess;

        }

        private static bool ChecksAuthenticInfo( string username, string pass)
        {
            //process user accounts and see if the user can be authenticated
            bool success = false;
       
            using (StreamReader fileReader = new StreamReader("..\\..\\accounts.txt"))
            {
                while (fileReader.EndOfStream == false && success == false)
                {
                    String[] UserInfo;
                    String line;
                    String usernameString;
                    String passwordString;
                    String isBanker;

                    line = fileReader.ReadLine();
                    UserInfo =line.Split(',');

                    usernameString = UserInfo[0];
                    passwordString = UserInfo[1];
                    isBanker = UserInfo[2];

                    
                    if (username == usernameString && pass == passwordString)
                    {
                        success = true;
                    }
                }
            }
            return success;
        }

        public static bool EscalateBanker ( string username, string pass )
        {
            //check if user is a banker or not
            bool bankerOrNot = false;
            bool success = false;

            using (StreamReader fileReader = new StreamReader("..\\..\\accounts.txt"))
            {
                while (fileReader.EndOfStream == false && success == false)
                {
                    String[] UserInfo;
                    String line;
                    String usernameString;
                    String passwordString;
                    String isBanker;

                    line = fileReader.ReadLine();
                    UserInfo = line.Split(',');

                    usernameString = UserInfo[0];
                    passwordString = UserInfo[1];
                    isBanker = UserInfo[2];


                    if (username == usernameString && pass == passwordString)
                    {
                        bankerOrNot = bool.Parse(isBanker);
                        success = true;
                    }
                }
            }


            return bankerOrNot;
        }
    }
}
