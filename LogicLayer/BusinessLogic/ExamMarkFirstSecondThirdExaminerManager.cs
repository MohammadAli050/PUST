using LogicLayer.BusinessObjects;
using LogicLayer.BusinessObjects.DTO;
using LogicLayer.DataLogic.DAFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LogicLayer.BusinessLogic
{
    public class ExamMarkFirstSecondThirdExaminerManager
    {
        public static int Insert(ExamMarkFirstSecondThirdExaminer exammarkfirstsecondthirdexaminer)
        {
            int id = RepositoryManager.ExamMarkFirstSecondThirdExaminer_Repository.Insert(exammarkfirstsecondthirdexaminer);
            
            return id;
        }

        public static bool Update(ExamMarkFirstSecondThirdExaminer exammarkfirstsecondthirdexaminer)
        {
            bool isExecute = RepositoryManager.ExamMarkFirstSecondThirdExaminer_Repository.Update(exammarkfirstsecondthirdexaminer);
            
            return isExecute;
        }

        public static bool Delete(int id)
        {
            bool isExecute = RepositoryManager.ExamMarkFirstSecondThirdExaminer_Repository.Delete(id);
            
            return isExecute;
        }

        public static ExamMarkFirstSecondThirdExaminer GetById(int? id)
        {
            ExamMarkFirstSecondThirdExaminer exammarkfirstsecondthirdexaminer = RepositoryManager.ExamMarkFirstSecondThirdExaminer_Repository.GetById(id);
            
            return exammarkfirstsecondthirdexaminer;
        }

        public static ExamMarkFirstSecondThirdExaminer GetByCourseHistoryId(int courseHistoryId)
        {
            ExamMarkFirstSecondThirdExaminer examMarkFirstSecondThirdExaminer = RepositoryManager.ExamMarkFirstSecondThirdExaminer_Repository.GetByCourseHistoryId(courseHistoryId);

            return examMarkFirstSecondThirdExaminer;
        }

        public static List<ExamMarkFirstSecondThirdExaminer> GetAll()
        {
            List<ExamMarkFirstSecondThirdExaminer> list = RepositoryManager.ExamMarkFirstSecondThirdExaminer_Repository.GetAll();
            
            return list;
        }


        public static List<ExamMarkEntryDTO> GetExamTemplateItemCourseSectionExamByFirstSecondThirdExaminerId(int? firstExaminerId, int? secondExaminerId, int? thirdExaminerId, int? examSetupDetailId)
        {
            List<ExamMarkEntryDTO> list = RepositoryManager.ExamMarkFirstSecondThirdExaminer_Repository.GetExamTemplateItemCourseSectionExamByFirstSecondThirdExaminerId(firstExaminerId, secondExaminerId, thirdExaminerId, examSetupDetailId);

            return list;
        }

        public static ExamMarkFirstSecondThirdExaminer GetByCourseHistoryIdExamTemplateTypeId(int courseHistoryId, int examTemplateItemId)
        {
            ExamMarkFirstSecondThirdExaminer exammarkfirstsecondthirdexaminer = RepositoryManager.ExamMarkFirstSecondThirdExaminer_Repository.GetByCourseHistoryIdExamTemplateTypeId(courseHistoryId, examTemplateItemId);

            return exammarkfirstsecondthirdexaminer;
        }

        public static ExamMarkGridViewShoetInfoDTO GetExamMarkModalGridViewShoetInfoByCourseHistoryId(int acaCalSectionId, int examSetupId)
        {
            ExamMarkGridViewShoetInfoDTO exammarkfirstsecondthirdexaminer = RepositoryManager.ExamMarkFirstSecondThirdExaminer_Repository.GetExamMarkModalGridViewShoetInfoByCourseHistoryId(acaCalSectionId, examSetupId);

            return exammarkfirstsecondthirdexaminer;
        }

        public static List<ExamMarkFirstSecondThirdExaminer> GetAllByAcaCalSectionId(int acaCalSectionId)
        {
            List<ExamMarkFirstSecondThirdExaminer> list = RepositoryManager.ExamMarkFirstSecondThirdExaminer_Repository.GetAllByAcaCalSectionId(acaCalSectionId);

            return list;
        }

        public static List<ExamMarkFirstSecondThirdExaminer> GetAllThirdExaminerStudentListByThirdExaminerStatus(int thirdExaminerStatus)
        {
            List<ExamMarkFirstSecondThirdExaminer> list = RepositoryManager.ExamMarkFirstSecondThirdExaminer_Repository.GetAllThirdExaminerStudentListByThirdExaminerStatus(thirdExaminerStatus);

            return list;
        }


        public static List<ExamMarkFirstSecondThirdDTO> GetExamMarkFirstSecondThirdByAcaCalSectionIdFirstSecondThirdTypeId(int acaCalSectionId, int firstSecondThirdTypeId, int examId, int employeeId)
        {
            List<ExamMarkFirstSecondThirdDTO> list = RepositoryManager.ExamMarkFirstSecondThirdExaminer_Repository.GetExamMarkFirstSecondThirdByAcaCalSectionIdFirstSecondThirdTypeId(acaCalSectionId, firstSecondThirdTypeId, examId, employeeId);

            return list;
        }


        public static List<Student> GetThirdExaminerEligibleStudentListByAcaCalSecId(int acaCalSectionId)
        {
            List<Student> list = RepositoryManager.ExamMarkFirstSecondThirdExaminer_Repository.GetThirdExaminerEligibleStudentListByAcaCalSecId(acaCalSectionId);
            return list;
        }

        public static List<ExaminerQuestionWiseMarkDTO> GetByFinalExamQuestionwiseMarkBySectionIdExaminerType(int acacalSectionId, int examinerTypeId, int examId, int employeeId)
        {
            List<ExaminerQuestionWiseMarkDTO> list = RepositoryManager.ExamMarkFirstSecondThirdExaminer_Repository.GetByFinalExamQuestionwiseMarkBySectionIdExaminerType(acacalSectionId, examinerTypeId, examId, employeeId);
            return list;
        }

        public static List<ExamMarkFirstSecondThirdExaminer> GetAllByAcaCalSectionIdExamIdExaminerId(int acaCalSectionId, int examId, int? FirstExaminerId, int? SecondExaminerId, int? ThirdExaminerId)
        {
            List<ExamMarkFirstSecondThirdExaminer> list = RepositoryManager.ExamMarkFirstSecondThirdExaminer_Repository.GetAllByAcaCalSectionIdExamIdExaminerId(acaCalSectionId, examId, FirstExaminerId, SecondExaminerId, ThirdExaminerId);

            return list;
        }


    }
}
