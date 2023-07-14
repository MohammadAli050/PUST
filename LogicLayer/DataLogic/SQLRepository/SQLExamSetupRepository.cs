using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicLayer.BusinessObjects;
using LogicLayer.DataLogic.IRepository;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using LogicLayer.BusinessObjects.DTO;
using LogicLayer.BusinessObjects.RO;

namespace LogicLayer.DataLogic.SQLRepository
{
    public class SQLExamSetupRepository : IExamSetupRepository
    {
        Database db = null;

        private string sqlInsert = "ExamSetupInsert";
        private string sqlUpdate = "ExamSetupUpdate";
        private string sqlDelete = "ExamSetupDeleteById";
        private string sqlGetById = "ExamSetupGetById";
        private string sqlGetAll = "ExamSetupGetAll";
        private string sqlGetAllExamSetupDTO = "ExamSetupGetAllDTO";
        private string sqlGetYearSemesterByProgramIdYearNoSemesterNo = "ExamSetupYearSemesterByProgramIdYearNoSemesterNo";

        public int Insert(ExamSetup examsetup)
        {
            int id = 0;
            bool isInsert = true;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand(sqlInsert);

                db = addParam(db, cmd, examsetup, isInsert);
                db.ExecuteNonQuery(cmd);

                object obj = db.GetParameterValue(cmd, "ID");

                if (obj != null)
                {
                    int.TryParse(obj.ToString(), out id);
                }
            }
            catch (Exception ex)
            {
                id = 0;
            }

            return id;
        }

        public bool Update(ExamSetup examsetup)
        {
            bool result = false;
            bool isInsert = false;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand(sqlUpdate);

                db = addParam(db, cmd, examsetup, isInsert);

                int rowsAffected = db.ExecuteNonQuery(cmd);

                if (rowsAffected > 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }

        public bool Delete(int id)
        {
            bool result = false;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand(sqlDelete);

                db.AddInParameter(cmd, "ID", DbType.Int32, id);
                int rowsAffected = db.ExecuteNonQuery(cmd);

                if (rowsAffected > 0)
                {
                    result = true;
                }
            }
            catch
            {
                result = false;
            }

            return result;
        }

        public ExamSetup GetById(int? id)
        {
            ExamSetup _examsetup = null;
            try
            {

                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExamSetup> rowMapper = GetMaper();

                var accessor = db.CreateSprocAccessor<ExamSetup>(sqlGetById, rowMapper);
                _examsetup = accessor.Execute(id).SingleOrDefault();

            }
            catch (Exception ex)
            {
                return _examsetup;
            }

            return _examsetup;
        }

        public List<ExamSetup> GetAll()
        {
            List<ExamSetup> examsetupList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExamSetup> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<ExamSetup>(sqlGetAll, mapper);
                IEnumerable<ExamSetup> collection = accessor.Execute();

                examsetupList = collection.ToList();
            }

            catch (Exception ex)
            {
                return examsetupList;
            }

            return examsetupList;
        }


        public List<ExamSetupDTO> GetAllExamSetupDTO(int? programId, int? yearNo, int? semesterNo, int? shal, int? sessionId)
        {
            List<ExamSetupDTO> examsetupList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExamSetupDTO> mapper = GetMaperExamSetupDTO();

                var accessor = db.CreateSprocAccessor<ExamSetupDTO>(sqlGetAllExamSetupDTO, mapper);
                IEnumerable<ExamSetupDTO> collection = accessor.Execute(programId, yearNo, semesterNo, shal, sessionId);

                examsetupList = collection.ToList();
            }

            catch (Exception ex)
            {
                return examsetupList;
            }

            return examsetupList;
        }
        public List<ExamSetup> ExamSetupGetAllByAcaCalProgram(int? programId, int? sessionId)
        {
            List<ExamSetup> examsetupList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExamSetup> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<ExamSetup>("ExamSetupGetAllByAcaCalProgram", mapper);
                IEnumerable<ExamSetup> collection = accessor.Execute(programId, sessionId);

                examsetupList = collection.ToList();
            }

            catch (Exception ex)
            {
                return examsetupList;
            }

            return examsetupList;
        }

        public List<ExamSetup> ExamSetupGetProgramIdYearNoSemesterNo(int programId, int yearNo, int semesterNo)
        {
            List<ExamSetup> examsetupList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExamSetup> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<ExamSetup>("ExamSetupGetByProgramIdYearNoSemesterNo", mapper);
                IEnumerable<ExamSetup> collection = accessor.Execute(programId, yearNo, semesterNo);

                examsetupList = collection.ToList();
            }

            catch (Exception ex)
            {
                return examsetupList;
            }

            return examsetupList;
        }

        public YearSemesterDTO GetYearSemesterByProgramIdYearNoSemesterNo(int programId, int yearNo, int semesterNo)
        {
            YearSemesterDTO _examsetup = null;
            try
            {

                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<YearSemesterDTO> rowMapper = GetMaperYearSemester();

                var accessor = db.CreateSprocAccessor<YearSemesterDTO>(sqlGetYearSemesterByProgramIdYearNoSemesterNo, rowMapper);
                _examsetup = accessor.Execute(programId, yearNo, semesterNo).SingleOrDefault();

            }
            catch (Exception ex)
            {
                return _examsetup;
            }

            return _examsetup;
        }


        #region Mapper
        private Database addParam(Database db, DbCommand cmd, ExamSetup examsetup, bool isInsert)
        {
            if (isInsert)
            {
                db.AddOutParameter(cmd, "ID", DbType.Int32, Int32.MaxValue);
            }
            else
            {
                db.AddInParameter(cmd, "ID", DbType.Int32, examsetup.ID);
            }


            db.AddInParameter(cmd, "ProgramId", DbType.Int32, examsetup.ProgramId);
            db.AddInParameter(cmd, "YearNo", DbType.Int32, examsetup.YearNo);
            db.AddInParameter(cmd, "SemesterNo", DbType.Int32, examsetup.SemesterNo);
            db.AddInParameter(cmd, "AcaCalId", DbType.Int32, examsetup.AcaCalId);
            db.AddInParameter(cmd, "Year", DbType.Int32, examsetup.Year);
            db.AddInParameter(cmd, "Shal", DbType.Int32, examsetup.Shal);
            db.AddInParameter(cmd, "ExamName", DbType.String, examsetup.ExamName);
            db.AddInParameter(cmd, "ExamShortName", DbType.String, examsetup.ExamShortName);
            db.AddInParameter(cmd, "ExamStartDate", DbType.DateTime, examsetup.ExamStartDate);
            db.AddInParameter(cmd, "ExamEndDate", DbType.DateTime, examsetup.ExamEndDate);
            db.AddInParameter(cmd, "LastDateOfResultSubmission", DbType.DateTime, examsetup.LastDateOfResultSubmission);
            db.AddInParameter(cmd, "ResultPublishDate", DbType.DateTime, examsetup.ResultPublishDate);
            db.AddInParameter(cmd, "Remarks", DbType.String, examsetup.Remarks);
            db.AddInParameter(cmd, "Attribute1", DbType.String, examsetup.Attribute1);
            db.AddInParameter(cmd, "Attribute2", DbType.String, examsetup.Attribute2);
            db.AddInParameter(cmd, "Attribute3", DbType.String, examsetup.Attribute3);
            db.AddInParameter(cmd, "CreatedBy", DbType.Int32, examsetup.CreatedBy);
            db.AddInParameter(cmd, "CreatedDate", DbType.DateTime, examsetup.CreatedDate);
            db.AddInParameter(cmd, "ModifiedBy", DbType.Int32, examsetup.ModifiedBy);
            db.AddInParameter(cmd, "ModifiedDate", DbType.DateTime, examsetup.ModifiedDate);
            db.AddInParameter(cmd, "IsActive", DbType.Boolean, examsetup.IsActive);

            return db;
        }

        private IRowMapper<ExamSetup> GetMaper()
        {
            IRowMapper<ExamSetup> mapper = MapBuilder<ExamSetup>.MapAllProperties()

           .Map(m => m.ID).ToColumn("ID")
        .Map(m => m.ProgramId).ToColumn("ProgramId")
        .Map(m => m.YearNo).ToColumn("YearNo")
        .Map(m => m.SemesterNo).ToColumn("SemesterNo")
        .Map(m => m.AcaCalId).ToColumn("AcaCalId")
        .Map(m => m.Year).ToColumn("Year")
        .Map(m => m.Shal).ToColumn("Shal")
        .Map(m => m.ExamName).ToColumn("ExamName")
        .Map(m => m.ExamShortName).ToColumn("ExamShortName")
        .Map(m => m.ExamStartDate).ToColumn("ExamStartDate")
        .Map(m => m.ExamEndDate).ToColumn("ExamEndDate")
        .Map(m => m.LastDateOfResultSubmission).ToColumn("LastDateOfResultSubmission")
        .Map(m => m.ResultPublishDate).ToColumn("ResultPublishDate")
        .Map(m => m.Remarks).ToColumn("Remarks")
        .Map(m => m.Attribute1).ToColumn("Attribute1")
        .Map(m => m.Attribute2).ToColumn("Attribute2")
        .Map(m => m.Attribute3).ToColumn("Attribute3")
        .Map(m => m.CreatedBy).ToColumn("CreatedBy")
        .Map(m => m.CreatedDate).ToColumn("CreatedDate")
        .Map(m => m.ModifiedBy).ToColumn("ModifiedBy")
        .Map(m => m.ModifiedDate).ToColumn("ModifiedDate")
        .Map(m => m.IsActive).ToColumn("IsActive")

            .Build();

            return mapper;
        }


        private IRowMapper<ExamSetupDTO> GetMaperExamSetupDTO()
        {
            IRowMapper<ExamSetupDTO> mapper = MapBuilder<ExamSetupDTO>.MapAllProperties()

           .Map(m => m.ExamSetupID).ToColumn("ExamSetupID")
           .Map(m => m.ExamSetupDetailId).ToColumn("ExamSetupDetailId")
        .Map(m => m.ProgramName).ToColumn("ProgramName")
        .Map(m => m.YearNo).ToColumn("YearNo")
        .Map(m => m.YearNoName).ToColumn("YearNoName")
        .Map(m => m.SemesterNo).ToColumn("SemesterNo")
        .Map(m => m.SemesterNoName).ToColumn("SemesterNoName")
        .Map(m => m.ExamName).ToColumn("ExamName")
        .Map(m => m.FullCode).ToColumn("FullCode")
        .Map(m => m.Shal).ToColumn("Shal")

        .Map(m => m.ExamSetupWithExamCommitteesId).ToColumn("ExamSetupWithExamCommitteesId")
        .Map(m => m.ChairmanName).ToColumn("ChairmanName")
        .Map(m => m.MemberOneName).ToColumn("MemberOneName")
        .Map(m => m.MemberTwoName).ToColumn("MemberTwoName")
        .Map(m => m.ExternalMemberName).ToColumn("ExternalMemberName")

            .Build();

            return mapper;
        }


        private IRowMapper<YearSemesterDTO> GetMaperYearSemester()
        {
            IRowMapper<YearSemesterDTO> mapper = MapBuilder<YearSemesterDTO>.MapAllProperties()

           .Map(m => m.YearId).ToColumn("YearId")
        .Map(m => m.YearNo).ToColumn("YearNo")
        .Map(m => m.SemesterId).ToColumn("SemesterId")
        .Map(m => m.SemesterNo).ToColumn("SemesterNo")

            .Build();

            return mapper;
        }

        #endregion


        public List<ExamSetupDetailDTO> ExamSetupDetailGetProgramIdYearNoSemesterNo(int programId, int yearNo, int semesterNo)
        {
            List<ExamSetupDetailDTO> examsetupList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExamSetupDetailDTO> mapper = GetExamSetupDetailDTOMaper();

                var accessor = db.CreateSprocAccessor<ExamSetupDetailDTO>("ExamSetupDetailGetByProgramIdYearNoSemesterNo", mapper);
                IEnumerable<ExamSetupDetailDTO> collection = accessor.Execute(programId, yearNo, semesterNo);

                examsetupList = collection.ToList();
            }

            catch (Exception ex)
            {
                return examsetupList;
            }

            return examsetupList;
        }

        private IRowMapper<ExamSetupDetailDTO> GetExamSetupDetailDTOMaper()
        {
            IRowMapper<ExamSetupDetailDTO> mapper = MapBuilder<ExamSetupDetailDTO>.MapAllProperties()

            .Map(m => m.ExamSetupDetailId).ToColumn("ExamSetupDetailId")
            .Map(m => m.YearNo).ToColumn("YearNo")
            .Map(m => m.SemesterNo).ToColumn("SemesterNo")
            .Map(m => m.ExamYear).ToColumn("ExamYear")
            .Map(m => m.ExamName).ToColumn("ExamName")
            .Map(m => m.ExamShortName).ToColumn("ExamShortName")
            .Map(m => m.IsActive).ToColumn("IsActive")
        

            .Build();
            return mapper;
        }


        public ExamSetup GetByProgramIdYearNoShal(int programId, int yearNo, int shalId)
        {
            ExamSetup _examsetup = null;
            try
            {

                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExamSetup> rowMapper = GetMaper();

                var accessor = db.CreateSprocAccessor<ExamSetup>("ExamSetupGetByProgramIdYearNoShal", rowMapper);
                _examsetup = accessor.Execute(programId, yearNo, shalId).SingleOrDefault();

            }
            catch (Exception ex)
            {
                return _examsetup;
            }

            return _examsetup;
        }


        #region Exam Committee Person Information
       public List<rExamCommitteePersonInfo> GetAllExamCommitteePersonInfoByParameter(int programId, int yearNo, int semesterNo, int shal, int sessionId)
        {
            List<rExamCommitteePersonInfo> examsetupList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<rExamCommitteePersonInfo> mapper = GetExamCommitteePersonMaper();

                var accessor = db.CreateSprocAccessor<rExamCommitteePersonInfo>("GetAllExamCommitteePersonInfoByParameter", mapper);
                IEnumerable<rExamCommitteePersonInfo> collection = accessor.Execute(programId, yearNo, semesterNo,shal,sessionId);

                examsetupList = collection.ToList();
            }

            catch (Exception ex)
            {
                return examsetupList;
            }

            return examsetupList;
        }

        private IRowMapper<rExamCommitteePersonInfo> GetExamCommitteePersonMaper()
        {
            IRowMapper<rExamCommitteePersonInfo> mapper = MapBuilder<rExamCommitteePersonInfo>.MapAllProperties()

            .Map(m => m.ExamName).ToColumn("ExamName")
            .Map(m => m.FullCode).ToColumn("FullCode")
            .Map(m => m.Shal).ToColumn("Shal")
            .Map(m => m.ChairmanName).ToColumn("ChairmanName")
            .Map(m => m.ChairmanDept).ToColumn("ChairmanDept")
            .Map(m => m.MemberOneName).ToColumn("MemberOneName")
            .Map(m => m.MemberOneDept).ToColumn("MemberOneDept")
            .Map(m => m.MemberTwoName).ToColumn("MemberTwoName")
            .Map(m => m.MemberTwoDept).ToColumn("MemberTwoDept")
            .Map(m => m.ExternalMemberName).ToColumn("ExternalMemberName")
            .Map(m => m.ExternalMemberDept).ToColumn("ExternalMemberDept")


            .Build();
            return mapper;
        }
        #endregion

    }
}
