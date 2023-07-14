using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using LogicLayer.BusinessObjects;
using LogicLayer.BusinessObjects.DTO;
using LogicLayer.DataLogic.DAFactory;

namespace LogicLayer.BusinessLogic
{
    public class ExaminerSetupStudentWiseManager
    {
        public static int Insert(ExaminerSetupStudentWise examinersetupstudentwise)
        {
            int id = RepositoryManager.ExaminerSetupStudentWise_Repository.Insert(examinersetupstudentwise);
            return id;
        }

        public static bool Update(ExaminerSetupStudentWise examinersetupstudentwise)
        {
            bool isExecute = RepositoryManager.ExaminerSetupStudentWise_Repository.Update(examinersetupstudentwise);
            
            return isExecute;
        }

        public static bool Delete(int id)
        {
            bool isExecute = RepositoryManager.ExaminerSetupStudentWise_Repository.Delete(id);
            
            return isExecute;
        }

        public static ExaminerSetupStudentWise GetByCourseHistoryId(int courseHistoryId)
        {
            var examinersetupstudentwise = RepositoryManager.ExaminerSetupStudentWise_Repository.GetByCourseHistoryId(courseHistoryId);
            return examinersetupstudentwise;
        }

        public static List<ExaminerSetupStudentWise> GetAll()
        {
            var list = RepositoryManager.ExaminerSetupStudentWise_Repository.GetAll();          
            return list;
        }

        public static List<ExaminerSetupStudentWiseDTO> ExaminerSetupStudentWiseGetByProgramSessionYearNoSemesterNoExamAndCourse(int programId, int yearNo, int semesterNo, int courseId, int versionId, int examId)
        {
            List<ExaminerSetupStudentWiseDTO> list = RepositoryManager.ExaminerSetupStudentWise_Repository.ExaminerSetupStudentWiseGetByProgramSessionYearNoSemesterNoExamAndCourse(programId, yearNo, semesterNo, courseId, versionId, examId);
            return list;
        }

        public static List<ExaminerSetupStudentWise> GetExaminersByAcaCalSectionId(int acaCalSecId)
        {
            var list = RepositoryManager.ExaminerSetupStudentWise_Repository.GetExaminersByAcaCalSectionId(acaCalSecId);
            return list;
        }
        public static List<ExaminerSetupStudentWise> ExaminerSetupStudentWiseGetByAcaCalSectionIdExaminerIdExaminerNo(int acaCalSecId, int examinerId,int examinerNo)
        {
            var list = RepositoryManager.ExaminerSetupStudentWise_Repository.ExaminerSetupStudentWiseGetByAcaCalSectionIdExaminerIdExaminerNo(acaCalSecId, examinerId, examinerNo);
            return list;
        }
    }
}

