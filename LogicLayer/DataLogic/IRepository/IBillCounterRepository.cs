using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicLayer.BusinessObjects;

namespace LogicLayer.DataLogic.IRepository
{
    public interface IBillCounterRepository
    {
        #region IMPLEMENT LATER IF NEEDED
        //int Insert(BillCounter fundtype);
        //bool Update(BillCounter fundtype);
        //bool Delete(int FundTypeId);
        //BillCounter GetById(int? FundTypeId);
        #endregion
        List<BillCounter> GetAll();
    }
}
