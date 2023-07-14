using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicLayer.BusinessObjects;

namespace LogicLayer.DataLogic.IRepository
{
    public interface IRegistrationWorksheetRepository
    {
        int Insert(RegistrationWorksheet registrationworksheet);
        bool Update(RegistrationWorksheet registrationworksheet);
        bool Delete(int ID);
        RegistrationWorksheet GetById(int? ID);
        List<RegistrationWorksheet> GetAll();
        List<RegistrationWorksheet> GetAllOpenCourseWhichSectionIsMatchInStudentBatchByStudentID(int studentId, int registrationSession);
        List<RegistrationWorksheet> GetByStudentID(int studentID);
        List<RegistrationWorksheet> GetByStudentIDAcacalID(int studentId, int acacalId);
        List<RegistrationWorksheet> GetByStudentIdYearNoSemesterNoExamId(int studentId, int yearNo, int semesterNo, int examId);
    }
}

