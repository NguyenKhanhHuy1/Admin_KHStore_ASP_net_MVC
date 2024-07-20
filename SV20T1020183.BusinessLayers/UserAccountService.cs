using SV20T1020183.DataLayers;
using SV20T1020183.DataLayers.SQLServer;
using SV20T1020183.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1020183.BusinessLayers
{
    public static class UserAccountService
    {
        private static readonly IUserAccountDAL userAccountDB;
        static UserAccountService()
        {
            userAccountDB = new EmployeeAccountDAL(Configuration.ConnectionString);
        }
        public static UserAccount? Authorize(string userName, string password)
        {

            return userAccountDB.Authorize(userName, password);
        }

        public static bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            return userAccountDB.ChangePassword(userName, oldPassword, newPassword);    
        }

    }
}
