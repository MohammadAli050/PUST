using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicLayer.BusinessObjects;
using LogicLayer.BusinessObjects.DTO;

namespace LogicLayer.DataLogic.IRepository
{
    public interface IExaminerSetupStudentWiseRepository
    {
        int Insert(ExaminerSetupStudentWise examinersetupstudentwise);
        bool Update(ExaminerSetupStudentWise examinersetupstudentwise);
        bool Delete(int ExaminerSetupStudentWiseId);
        ExaminerSetupStudentWise GetByCourseHistoryId(int ExaminerSetupStudentWiseId);
        List<ExaminerSetupStudentWise> GetAll();
        List<ExaminerSetupStudentWiseDTO> ExaminerSetupStudentWiseGetByProgramSessionYearNoSemesterNoExamAndCourse(int programId, int yearNo, int semesterNo, int courseId, int versionId, int examId);
        List<ExaminerSetupStudentWise> GetExaminersByAcaCalSectionId(int acaCalSecId);
        List<ExaminerSetupStudentWise> ExaminerSetupStudentWiseGetByAcaCalSectionIdExaminerIdExaminerNo(int acaCalSecId, int examinerId, int examinerNo);

    }
}

