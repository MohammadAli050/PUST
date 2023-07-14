using LogicLayer.BusinessObjects;
using LogicLayer.BusinessObjects.DTO;
using LogicLayer.BusinessObjects.RO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.DataLogic.IRepository
{
    public interface IExamSetupRepository
    {
        int Insert(ExamSetup examsetup);
        bool Update(ExamSetup examsetup);
        bool Delete(int ID);
        ExamSetup GetById(int? ID);
        List<ExamSetup> GetAll();
        List<ExamSetupDTO> GetAllExamSetupDTO(int? programId, int? yearId, int? semesterNo, int? shal, int? sessionId);
        YearSemesterDTO GetYearSemesterByProgramIdYearNoSemesterNo(int programId, int yearNo, int semesterNo);
        List<ExamSetup> ExamSetupGetAllByAcaCalProgram(int? programId, int? sessionId);
        List<ExamSetup> ExamSetupGetProgramIdYearNoSemesterNo(int programId, int yearNo, int semesterNo);
        List<ExamSetupDetailDTO> ExamSetupDetailGetProgramIdYearNoSemesterNo(int programId, int yearNo, int semesterNo);

        ExamSetup GetByProgramIdYearNoShal(int programId, int yearNo, int shalId);
        List<rExamCommitteePersonInfo> GetAllExamCommitteePersonInfoByParameter(int programId, int yearNo, int semesterNo, int shal, int sessionId);
    }
}
