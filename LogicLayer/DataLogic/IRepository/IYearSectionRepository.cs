using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicLayer.BusinessObjects;

namespace LogicLayer.DataLogic.IRepository
{
    public interface IYearSectionRepository
    {
        int Insert(YearSection yearsection);
        bool Update(YearSection yearsection);
        bool Delete(int Id);
        YearSection GetById(int? Id);
        List<YearSection> GetAll();
    }
}

