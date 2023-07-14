using LogicLayer.BusinessObjects;
using LogicLayer.BusinessObjects.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.DataLogic.IRepository
{
    public interface IAcademicCalenderSectionRepository
    {
        int Insert(AcademicCalenderSection academicCalenderSection);
        bool Update(AcademicCalenderSection academicCalenderSection);
        bool Delete(int id);
        AcademicCalenderSection GetById(int? id);
        List<AcademicCalenderSection> GetAll();
        List<AcademicCalenderSection> GetAll(int studentId, int acaCalId);
        List<AcademicCalenderSection> GetAllByAcaCalId(int id);
        List<AcademicCalenderSection> GetAllByAcaCalIdStudentRoll(int id, string studentRoll);
        List<AcademicCalenderSection> GetAllByAcaCalIdState(int id, string state);
        List<AcademicCalenderSection> GetAllByRoomDayTime(int Room1, int Room2, int Day1, int Day2, int Time1, int Time2);
        AcademicCalenderSection GetByCourseVersionSecFac(int courseId, int versionId, string section, int facultyId);
        AcademicCalenderSection GetByAcaCalCourseVersionSection(int acaCalId, int courseId, int versionId, string sectionName);
        List<AcademicCalenderSection> GetByAcaCalCourseVersion(int acaCalId, int courseId, int versionId);
        List<AcademicCalenderSection> GetAllByRoomInRegSession(int roomId);
        List<AcademicCalenderSection> GetAllByTeacherInRegSession(int teacherId);
        List<AcademicCalenderSection> GetAllByAcaCalProgram(int acaCalId, int programId);
        //List<DDAcademicCalenderSection> GetAllSectionForUnlockBySessionProgram(int acaCalId, int programId, int employeeId, int AcaCalSectionFacultyType, int BasicExamTemplateItemId, int IsForMarkSubmit);
        List<AcademicCalenderSection> GetAllByAcaCalAndProgram(int programId, int sessionId);
        //added for course evaluation report
        //List<AcademicCalenderSectionWithCourse> GetAllCourseWithSectionByAcaCalAndProgramAndTeacher(int acaCalId, int programId, int teacherId);
        //List<rTeacherInfo> GetAllTeacherByAcaCalAndProgram(int acaCalId, int programId);
        //-----------------------------------//
        //string GetConflictResultBySessionDayRoomTimeTeacher(int acaSecId, int seq, int acaCalId, int dayId, int roomId, int timeId, int teacherId);
        List<TeacherInfo> GetAllTeacherInfoByAcaSecId(int AcaSecId);
        List<TeacherInfo> GetAllTeacherInfoForFinalExamReportByAcaSecId(int AcaSecId, int BasicExamTempItmId); 
        string GetLockedExamNameInfoByAcaSecId(int AcaSecId);
        string GetLockedTheoryAndSessionalExamNameInfoByAcaSecId(int AcaSecId);
        string GetLockedFinalExamNameInfoByAcaSecId(int AcaSecId);
        string GetLockedExamNameInfoByAcaSecIdExamId(int AcaSecId,int ExamId);
        string GetScrutinizerLockedExamNameInfoByAcaSecIdExamId(int AcaSecId, int ExamId);
        string GetScrutinizerLockedExamNameInfoByAcaSecId(int AcaSecId);

        List<AcademicCalenderSection> GetAllCoursesForFinalExamByProgramAcaCalEmployeeId(int programId, int acaCalId, int EmployeeId);
        List<AcademicCalenderSection> GetAllCoursesForFinalExamByProgramAcaCalEmployeeId(int programId, int acaCalId, int EmployeeId, int AcaCalSectionFacultyTypeId);
        
        //added for teacher course evaluation
        List<AcademicCalenderSectionWithTeacherId> GetRestCourseByStudentAcaCal(int studentId, int acaCalId);
        bool GenerateSection(int programId, int yearId, int semesterId, int yearNo, int semesterNo, int examId);
        List<AcademicCalenderSection> GetByProgramIdExamIdYearNoSemesterNoCourseIdVersionId(int programId, int examId, int yearNo, int semesterNo, int courseId, int versionId);
        List<AcademicCalenderSection> GetByProgramIdYearNoSemesterNoExamId(int programId, int yearNo, int semesterNo, int examId);
        List<AcademicCalenderSection> GetByProgramIdYearNoSemesterNoExamId_FinalExamMarksEntry(int programId, int yearNo, int semesterNo, int examId);
        //List<AcademicCalenderSection> GetByProgramIdYearNoSemesterNoVersitySession(int programId, int yearNo, int semesterNo, string currentVersitySession);
        //List<AcademicCalenderSection> GetAllByProgramyearnosemesternoexamid(int programId, int yearNo, int semesterNo, int examId);
        List<AcademicCalenderSection> GetByExaminerIdYearNoSemesterNoExamId(int examinerId, int yearNo, int semesterNo, int examId, int examinerTypeId);
        List<AcademicCalenderSection> GetAllByHeldInRelationId(int HeldInRelationId);
    
    }
}
