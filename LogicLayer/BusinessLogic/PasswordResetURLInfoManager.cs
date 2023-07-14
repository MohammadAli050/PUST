using LogicLayer.BusinessObjects;
using LogicLayer.DataLogic.DAFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.BusinessLogic
{
    public class PasswordResetURLInfoManager
    {
        public static int Insert(PasswordResetURLInfo passwordreseturlinfo)
        {
            int id = RepositoryManager.PasswordResetURLInfo_Repository.Insert(passwordreseturlinfo);
            return id;
        }

        public static bool Update(PasswordResetURLInfo passwordreseturlinfo)
        {
            bool isExecute = RepositoryManager.PasswordResetURLInfo_Repository.Update(passwordreseturlinfo);
            return isExecute;
        }

        public static bool Delete(int id)
        {
            bool isExecute = RepositoryManager.PasswordResetURLInfo_Repository.Delete(id);
            return isExecute;
        }

        public static PasswordResetURLInfo GetById(int? id)
        {
            PasswordResetURLInfo passwordreseturlinfo = RepositoryManager.PasswordResetURLInfo_Repository.GetById(id);

            return passwordreseturlinfo;
        }

        public static PasswordResetURLInfo GetByLoginId(string loginID)
        {
            PasswordResetURLInfo passwordreseturlinfo = RepositoryManager.PasswordResetURLInfo_Repository.GetByLoginId(loginID);

            return passwordreseturlinfo;
        }

        public static List<PasswordResetURLInfo> GetAll()
        {
            List<PasswordResetURLInfo> list = RepositoryManager.PasswordResetURLInfo_Repository.GetAll();

            return list;
        }


        public static int CheckLoginIdAndOfficialEmailAddressValidity(string loginID)
        {
            return RepositoryManager.PasswordResetURLInfo_Repository.CheckLoginIdAndOfficialEmailAddressValidity(loginID);
        }
    }
}
