using LogicLayer.BusinessObjects;
using LogicLayer.BusinessObjects.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.DataLogic.IRepository
{
    public interface IExamMarkFirstSecondThirdExaminerRepository
    {
        int Insert(ExamMarkFirstSecondThirdExaminer exammarkfirstsecondthirdexaminer);
        bool Update(ExamMarkFirstSecondThirdExaminer exammarkfirstsecondthirdexaminer);
        bool Delete(int ID);
        ExamMarkFirstSecondThirdExaminer GetById(int? ID);
        List<ExamMarkFirstSecondThirdExaminer> GetAll();
        List<ExamMarkEntryDTO> GetExamTemplateItemCourseSectionExamByFirstSecondThirdExaminerId(int? firstExaminerId, int? secondExaminerId, int? thirdExaminerId, int? examSetupDetailId);

        ExamMarkFirstSecondThirdExaminer GetByCourseHistoryIdExamTemplateTypeId(int courseHistoryId, int examTemplateItemId);
        ExamMarkGridViewShoetInfoDTO GetExamMarkModalGridViewShoetInfoByCourseHistoryId(int acaCalSectionId, int examSetupId);
        ExamMarkFirstSecondThirdExaminer GetByCourseHistoryId(int courseHistoryId);
        List<ExamMarkFirstSecondThirdExaminer> GetAllByAcaCalSectionId(int acaCalSectionId);
        List<ExamMarkFirstSecondThirdExaminer> GetAllThirdExaminerStudentListByThirdExaminerStatus(int thirdExaminerStatus);

        List<ExamMarkFirstSecondThirdDTO> GetExamMarkFirstSecondThirdByAcaCalSectionIdFirstSecondThirdTypeId(int acaCalSectionId, int firstSecondThirdTypeId, int examId, int employeeId);
        List<Student> GetThirdExaminerEligibleStudentListByAcaCalSecId(int acaCalSectionId);

        List<ExaminerQuestionWiseMarkDTO> GetByFinalExamQuestionwiseMarkBySectionIdExaminerType(int acaCalSectionId, int firstSecondThirdTypeId, int examId, int employeeId);
        List<ExamMarkFirstSecondThirdExaminer> GetAllByAcaCalSectionIdExamIdExaminerId(int acaCalSectionId, int examId, int? FirstExaminerId, int? SecondExaminerId, int? ThirdExaminerId);
    }
}
