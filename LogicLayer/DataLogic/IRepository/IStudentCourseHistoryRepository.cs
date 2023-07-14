using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicLayer.BusinessObjects;
using LogicLayer.BusinessObjects.DTO;

namespace LogicLayer.DataLogic.IRepository
{
    public interface IStudentCourseHistoryRepository
    {
        int Insert(StudentCourseHistory studentCourseHistory);
        bool Update(StudentCourseHistory studentCourseHistory);
        bool UpdateLevelByStudentIdAcaCalId(int StudentId, int AcaCalId, int LevelId);
        bool Delete(int id);
        bool DeleteByExamIdCourseIdStudentId(int examId, int courseId, int studentId);
        decimal GetCompletedCredit(string Roll);
        decimal GetAttemptedCredit(string Roll);
        StudentCourseHistory GetById(int? id);
        List<StudentCourseHistory> GetAll();
        List<StudentCourseHistory> GetAllByAcaCalSectionId(int id);
        List<StudentCourseHistory> GetAllByStudentId(int StudentID);
        List<StudentCourseHistory> GetAllByStudentIdAcaCalId(int studentID, int acaCalId);
        List<StudentCourseHistory> GetByStudentIdAcaCalId(int AcaCalId, int StudentId);
        List<StudentCourseHistoryDTO> GetAllByAcaCalIdCourseId(int acaCalId, int CourseId);
        List<StudentCourseHistory> GetAllRegisteredStudentByProgramSessionBatchCourse(int programId, int sessionId, int batchId, int courseId, int versionId);
        //List<StudentAdmitCardInfo> GetAllStudentAdmitCardInfoByProgramBatchSessionRoll(int programId, int sessionId, int batchId, string Roll);
        //List<StudentAdmitCardInfo> GetAllStudentAdmitCardInfoByProgramBatchSessionRollV2(int programId, int sessionId, int batchId, string Roll);
        //List<rResultCheck> GetStudentResultHisroyForResultCheck(int programId, int sessionId, int batchId, int year);
        bool IsDiscollegiateByStudentCourseHistoryId(int CourseHistoryId);
        bool IsAbsentInExamByStudentCourseHistoryId(int CourseHistoryId);
        List<StudentCourseHistory> GetAllByAcaCalId(int acaCalId);
        List<StudentCourseHistory> GetAllRegisteredStudentByProgramSessionBatchId(int programId, int sessionId, int batchId);
        //List<rStudentCourse> GetAllCourseHistoryByCampusProgramSessionBatchCreditGroupDateRegStatus(int campusId, int programId, int sessionId, int batchId, int creditGroup, DateTime fromDate, DateTime toDate, int ResStatus, int dateFilter);
        List<StudentCourseHistoryNewDTO> GetAllCourseHistoryDetailByStudentId(int studentId);
        List<StudentCourseHistory> GetAllByStudentIdYearNoSemesterNoExamId(int studentID, int yearNo, int semesterNo, int examId);
        bool DeleteCourseByProgramYearSemesterExamId(int programId, int yearId, int semesterId, int yearNo, int semesterNo, int examId, int courseId, int versionId);

        List<StudentCourseHistory> GetAllByExamSetupDetailIdFirstSecondThirdExaminerId(int ExamSetupDetailId, int acaCalSectionId, int? FirstExaminerId, int? SecondExaminerId, int? ThirdExaminerId);

    }
}
