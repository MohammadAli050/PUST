using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicLayer.BusinessObjects;
using LogicLayer.BusinessObjects.DTO;

namespace LogicLayer.DataLogic.IRepository
{
    public interface ISemesterRepository
    {
        int Insert(Semester semester);
        bool Update(Semester semester);
        bool Delete(int SemesterId);
        Semester GetById(int? SemesterId);
        List<Semester> GetAll();
        List<Semester> GetByYearId(int yearId);
        List<Semester> GetByProgramIdYearId(int programId, int yearId);
        List<SemesterDistinctDTO> GetAllDistinct();
    }
}

