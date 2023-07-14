using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using LogicLayer.BusinessObjects;
using LogicLayer.DataLogic.DAFactory;
using LogicLayer.BusinessObjects.DTO;
using LogicLayer.BusinessObjects.RO;

namespace LogicLayer.BusinessLogic
{
    public class ExamSetupManager
    {
        public static int Insert(ExamSetup examSetup)
        {
            int id = RepositoryManager.ExamSetup_Repository.Insert(examSetup);
            return id;
        }

        public static bool Update(ExamSetup examSetup)
        {
            bool isExecute = RepositoryManager.ExamSetup_Repository.Update(examSetup);
            return isExecute;
        }

        public static bool Delete(int id)
        {
            bool isExecute = RepositoryManager.ExamSetup_Repository.Delete(id);
            return isExecute;
        }

        public static ExamSetup GetById(int id)
        {
            ExamSetup examSetup = RepositoryManager.ExamSetup_Repository.GetById(id);
            return examSetup;
        }

        public static List<ExamSetup> GetAll()
        {
            List<ExamSetup> list = RepositoryManager.ExamSetup_Repository.GetAll();
            return list;
        }
        public static List<ExamSetup> ExamSetupGetAllByAcaCalProgram(int? programId,  int? sessionId)
        {
            List<ExamSetup> list = RepositoryManager.ExamSetup_Repository.ExamSetupGetAllByAcaCalProgram(programId, sessionId);
            return list;
        }

        public static List<ExamSetup> ExamSetupGetProgramIdYearNoSemesterNo(int programId, int yearNo, int semesterNo)
        {
            List<ExamSetup> list = RepositoryManager.ExamSetup_Repository.ExamSetupGetProgramIdYearNoSemesterNo(programId, yearNo, semesterNo);
            return list;
        }

        public static List<ExamSetupDTO> GetAllExamSetupDTO(int? programId, int? yearNo, int? semesterNo, int? shal, int? sessionId)
        {
            List<ExamSetupDTO> list = RepositoryManager.ExamSetup_Repository.GetAllExamSetupDTO(programId, yearNo, semesterNo, shal, sessionId);
            return list;
        }


        public static YearSemesterDTO GetYearSemesterByProgramIdYearNoSemesterNo(int programId, int yearNo, int semesterNo)
        {
            YearSemesterDTO model = RepositoryManager.ExamSetup_Repository.GetYearSemesterByProgramIdYearNoSemesterNo(programId, yearNo, semesterNo);
            return model;
        }


        public static List<ExamSetupDetailDTO> ExamSetupDetailGetProgramIdYearNoSemesterNo(int programId, int yearNo, int semesterNo)
        {
            List<ExamSetupDetailDTO> list = RepositoryManager.ExamSetup_Repository.ExamSetupDetailGetProgramIdYearNoSemesterNo(programId, yearNo, semesterNo);
            return list;
        }


        public static ExamSetup GetByProgramIdYearNoShal(int programId, int yearNo, int shalId)
        {
            ExamSetup examSetup = RepositoryManager.ExamSetup_Repository.GetByProgramIdYearNoShal(programId, yearNo, shalId);
            return examSetup;
        }

        public static List<rExamCommitteePersonInfo> GetAllExamCommitteePersonInfoByParameter(int programId, int yearNo, int semesterNo, int shal, int sessionId)
        {
            List<rExamCommitteePersonInfo> list = RepositoryManager.ExamSetup_Repository.GetAllExamCommitteePersonInfoByParameter( programId,  yearNo,  semesterNo,  shal,  sessionId);
            return list;
        }
        
    }
}
