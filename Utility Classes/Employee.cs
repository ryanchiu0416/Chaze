using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chaze.Utility_Classes
{
    static class Employee
    {
        private static bool isBanker = false;

        public static bool CheckIn(string usr, string pw)
        {
            //call CheckIfBanker()
            CheckIfBanker(usr, pw);

            return Authentication.Authenticate(usr, pw);
        }

        private static void CheckIfBanker(string usr, string pw)
        {
            //call authentication class and attempt to escalate
            if (Authentication.EscalateBanker(usr, pw))
            {
                isBanker = true;
            }
        }

        public static bool IsBanker()
        {
            //tells if one is a banker
            return isBanker; 
        }
    }
}
