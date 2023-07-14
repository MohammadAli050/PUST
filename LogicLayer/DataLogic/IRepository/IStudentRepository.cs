using LogicLayer.BusinessObjects;
using LogicLayer.BusinessObjects.RO;
//using LogicLayer.BusinessObjects.DTO;
//using LogicLayer.BusinessObjects.RO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.DataLogic.IRepository
{
    public interface IStudentRepository
    {
        int Insert(Student student);
        bool Update(Student student);
        bool Delete(int id);
        Student GetById(int? id);
        List<Student> GetAll();

        List<Student> GetAllByNameOrID(string searchKey);
        List<Student> GetAllByProgramIdBatchId(int programID, int academicCalenderID);
        List<Student> GetAllByProgramIdBatchIdGroupId(int programID, int batchID, int groupId);
        List<Student> GetAllByProgramIdSessionId(int programId, int sessionId);
        List<Student> GetStudentsByExamIdCourseId(int examId, int courseId);

        Student GetByRoll(string roll);
        Student GetByPersonID(int personID);
        
        List<Student> GetAllRegisteredStudentByProgramSessionBatchId(int programId, int sessionId, int? batchId);

        //Student GetByRollOrSerialNo(string roll,int serialNo);
        
        //LoadStudentByIdDTO GetByRollEdit(string roll);

        //Student GetByPersonID(int personID);

        //List<Student> GetAllFromRegWorksheetByProgramAndBatch(int programId, int acaCalId);

        //List<Student> GetFromRegWorksheetByStudentRoll(string roll);

        //List<Student> GetAllByProgramOrBatchOrRoll(int programId, int batchId, string roll);

        //List<Student> GetAllByBatchProgram(string batch, string program);

        //List<Student> GetAllByProgramOrBatchOrRoll(int programId, int acaCalId, string rollFrom, string rollTo);

        //List<Student> StudentGetAllActiveInactiveWithRegistrationStatus(int programId, int acaCalBatchId, int acaCalSessionId, string roll);

        //List<Student> GetAllRegisteredStudentBySession(int acaCalSessionId);

        //List<rStudentMajorDefine> GetAllStudentByMajorDefine(int programId, int batchId, string roll);

        //List<StudentBlockCountByProgramDTO> GetAllBlockStudentByProgram();

        //bool DeleteAllBlockStudentByProgram(int programId);

        //List<StudentBlockCountByProgramDTO> GetAllInActiveStudentByProgram();
        
        //bool UpdateInActiveToActiveByProgram(int programId);

        //List<StudentProbationDTO> GetAllByProbationStatus(int programId, int acaCalBatchId, int? minProb, int? maxProb);

        //List<rStudentBatchWise> GetStudentProgramOrBatch(int programId, int batchId);

        //List<rStudentTranscript> GetStudentTrancriptById(string studentId);

        //List<rStudentTranscriptNew> GetStudentTrancriptByIdNew(string studentId);

        //rStudentGeneralInfo GetStudentGeneralInfoById(int studentId);

        //List<Student> GetAllRegisteredStudentByProgramSessionCourse(int programId, int sessionId, int courseId, int versionId);

        //List<StudentDiscountInitialDTO> GetAllDiscountInitialByProgramBatchRoll(int programId, int batchId, string roll);

        //List<RunningStudent> GetStudentListByProgramAndBatch(int programId, int batchId);

        //List<StudentDTO> GetAllDTOByProgramOrBatchOrRoll(int programId, int acaCalId, string roll);

        //List<StudentDTO> GetAllDTOHasInitialDiscountGetByProgramOrBatchOrRoll(int programId, int batchId, string roll);

        //List<StudentDTO> GetAllDTOForSiblingByProgramOrBatchOrRoll(int programId, int acaCalId,int relationDiscountType, string roll);

        //List<StudentDTO> GetDTOAllBySiblingGroupId(int groupId);

        //List<rStudentTranscript> GetStudentTrancriptByIdRunning(string studentId);

        //List<rStudentTranscript> GetConsolidatedCrAssessment(string studentId);

        //List<rStudentGPACGPA> GetStudentGPACGPAByRoll(string studentId);

        //List<rStudentByProgramAndBatch> GetStudentByProgramIdBatchId(int programId, int batchId);

        //List<RunningStudent> GetRunningStudentByProgramIdAcaCalId(int programId, int acaCalId, int batchId);
        
        //List<RunningStudent> GetRunningStudentByProgramIdAcaCalIdNew(int programId, int acaCalId, int batchId);

        //List<rStudentCourse> GetRunningStudentCourseByProgramIdAcaCalId(int programId, int acaCalId, int batchId);
        
        //List<rStudentByProgramAndBatch> GetCompleteStudentByProgramIdBatchId(int programId, int batchId, int acaCalId);
        
        //List<StudentDTO> GetAllDTOByProgramBatchResultSessionRoll(int programId, int acaCalId, int sessionId, int resultSessionId, string roll);
        
        //List<StudentDTO> GetAllDTOHasInitialDiscountGetByProgramOrBatchOrRoll(int programId, int batchId, string roll, int sessionId, int resultSessionId);

        //List<rStudentBatchWise> GetStudentYearOrSemesterOrCalenderUnitOrProgramOrBatch(string year, int semesterId, int calenderUnitTypeId, int programId, int batchId);

        //List<rStudentResultProgramBatch> GetStudentGPACGPAByProgramBatchSession(int programId, int BatchId, int sessionId, int SemesterNo);

        //List<rStudentRollSheet> GetStudentRollSheetByProgramBatchSession(int programId, int BatchId, int SessoinId);

        //List<StudentCountProgramBatchWise> GetStudentCountProgramBatchWiseByAcaCalIdProgramId(int ProgramId,int AcaCalId);

        //List<rStudentTabulationSheet> GetStudentTabulationSheetByProgramBatchSession(int programId, int BatchId, int SessoinId);

        //List<StudentBillCourseCountDTO> GetStudentForBillPosting(int sessionId, int programId, int batchId);

        //List<rStudentGradeCertificateInfo> GetStudentGradeCertificateInfoByRoll(string Roll, int sessionId);

        //List<StudentRollOnly> GetStudentListRollByProgramBatchSession(int sessionId, int programId, int batchId);

        //List<StudentIdCardInfo> GetStudentIdCardInfoByRoll(string Roll);

        //List<Student> GetStudentByProgramIdAcaCalId(int programId, int acaCalId, int batchId);
        //List<Student> GetAllByProgramIdAcacalIdYearIdSemsterId(int programId, int yearId, int semesterId, int acacalId);
        //List<Student> GetAllByProgramIdAcacalIdYearIdSemsterIdForRegistration(int programId, int yearId, int semesterId, int acacalId);
        List<Student> GetAllByProgramIdYearNoSemsterNoCurrentSessionIdForRegistration(int programId, int yearNo, int semesterNo, int currentSessionId);
        List<rStudentList> GetStudentListByProgramAndYearOrSemester(int programId, int admSessionId, int yearId, int semesterId);
        //List<Student> GetAllByProgramIdYearIdSemsterIdCurrentSessionId(int programId, int yearId, int semesterId, int acacalId);
        List<Student> GetAllByProgramIdYearNoSemsterNoCurrentSessionId(int programId, int yearNo, int semesterNo, int currentSessionId);
        
    }
}
