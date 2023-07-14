using LogicLayer.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.DataLogic.IRepository
{
    public interface IPasswordResetURLInfoRepository
    {
        int Insert(PasswordResetURLInfo passwordreseturlinfo);
        bool Update(PasswordResetURLInfo passwordreseturlinfo);
        bool Delete(int Id);
        PasswordResetURLInfo GetById(int? Id);
        List<PasswordResetURLInfo> GetAll();
        int CheckLoginIdAndOfficialEmailAddressValidity(string loginID);
        PasswordResetURLInfo GetByLoginId(string loginID);
    }
}
