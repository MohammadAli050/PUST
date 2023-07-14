using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicLayer.BusinessObjects;
using LogicLayer.BusinessObjects.DTO;
using LogicLayer.BusinessObjects.RO;

namespace LogicLayer.DataLogic.IRepository
{
    public interface IExamMarkMasterRepository
    {
        int Insert(ExamMarkMaster exammarkmaster);
        bool Update(ExamMarkMaster exammarkmaster);
        bool Delete(int ExamMarkMasterId);
        ExamMarkMaster GetById(int? ExamMarkMasterId);
        List<ExamMarkMaster> GetAll();
        ExamMarkMaster GetByAcaCalSectionIdExamTemplateItemId(int acaCalsectionId, int examTemplateItemId);
        bool FinalSubmitByAcacalSectionId(int acaCalSectionId, bool isFinalSubmit);
        bool ExamMarkBackToTeacherByAcacalSectionId(int acaCalSectionId, bool isFinalSubmit);
        List<TempStudentExamMarkColumnWise> GetContinuousMarkColumnWise(int acaCalSectionId);
        List<TempStudentExamMarkColumnWise> GetAllMarkColumnWise(int acaCalSectionId);
        List<ExamCommitteeDashboardDTO> GetExamCommitteeDashboard(int programId, int yearNo, int semesterNo, int examId);
        CountinousMarkTeacherInfoAPI_DTO GetTeacherInfoAPIBySectionId(int sectionId);
        List<RTabulationData> GetTabulationDataByProgramYearSemesterExam(int programId, int yearNo, int semesterNo, int examId);
        ExamCommitteeDashboardDTO GetExamCommitteeDashboardExtendByAcaCalSecId(int acaCalSecId);
        List<ExamCommitteeDashboardDTO> GetExamCommitteeDashboardExtendForGroupByAcaCalSecId(int acaCalSecId);
        ExamCommitteeDashboardDTO GetExaminerByAcaCalSecId(int acaCalSecId);
        bool AutoMarkEntryForContinuousAssessmentTemplateIdBySectionId(int sectionId);
    }
}

