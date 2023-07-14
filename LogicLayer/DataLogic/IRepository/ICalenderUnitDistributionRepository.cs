using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicLayer.BusinessObjects;

namespace LogicLayer.DataLogic.IRepository
{
    public interface ICalenderUnitDistributionRepository
    {
        int Insert(CalenderUnitDistribution calenderunitdistribution);
        bool Update(CalenderUnitDistribution calenderunitdistribution);
        bool Delete(int CalenderUnitDistributionID);
        CalenderUnitDistribution GetById(int? CalenderUnitDistributionID);
        List<CalenderUnitDistribution> GetAll();
        CalenderUnitDistribution GetByCourseId(int? id);
        List<rCourseDistribution> GetCourseDistributionByProgramIdAndTreeCalMasId(int ProgramId, int TreeCalMasId);
    }
}

