using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicLayer.BusinessObjects;

namespace LogicLayer.DataLogic.IRepository
{
    public interface IPersonAdditionalInfoRepository
    {
        int Insert(PersonAdditionalInfo personadditionalinfo);
        bool Update(PersonAdditionalInfo personadditionalinfo);
        bool Delete(int PersonAdditionalInfoId);
        PersonAdditionalInfo GetByPersonId(int? PersonId);
        List<PersonAdditionalInfo> GetAll();
    }
}
