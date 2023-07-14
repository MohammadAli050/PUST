using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicLayer.BusinessObjects;
using LogicLayer.BusinessObjects.DTO;

namespace LogicLayer.DataLogic.IRepository
{
    public interface IYearRepository
    {
        int Insert(Year year);
        bool Update(Year year);
        bool Delete(int YearId);
        Year GetById(int? YearId);
        List<Year> GetAll();
        List<Year> GetByProgramId(int programId);
        List<Year> GetByProgramIdYearName(int programId, string yearName);
        List<YearDistinctDTO> GetAllDistinct();
    }
}

