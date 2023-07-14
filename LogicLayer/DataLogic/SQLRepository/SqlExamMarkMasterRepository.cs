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
    public partial class SqlExamMarkMasterRepository : IExamMarkMasterRepository
    {

        Database db = null;

        private string sqlInsert = "ExamMarkMasterInsert";
        private string sqlUpdate = "ExamMarkMasterUpdate";
        private string sqlDelete = "ExamMarkMasterDelete";
        private string sqlGetById = "ExamMarkMasterGetById";
        private string sqlGetAll = "ExamMarkMasterGetAll";
        private string sqlGetByAcaCalSectionIdExamTemplateItemId = "ExamMarkMasterGetByAcaCalSectionIdExamTemplateItemId";
        private string sqlGetByStudentExamMarkColmneWiseProcess = "StudentExamMarkColmneWiseProcess";
        private string sqlGetByStudentContinuousMarkColmneWiseProcess = "StudentContinuousExamMarkColmneWiseProcess";
               

        public int Insert(ExamMarkMaster exammarkmaster)
        {
            int id = 0;
            bool isInsert = true;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand(sqlInsert);

                db = addParam(db, cmd, exammarkmaster, isInsert);
                db.ExecuteNonQuery(cmd);

                object obj = db.GetParameterValue(cmd, "ExamMarkMasterId");

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

        public bool Update(ExamMarkMaster exammarkmaster)
        {
            bool result = false;
            bool isInsert = false;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand(sqlUpdate);

                db = addParam(db, cmd, exammarkmaster, isInsert);

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

                db.AddInParameter(cmd, "ExamMarkMasterId", DbType.Int32, id);
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

        public ExamMarkMaster GetById(int? id)
        {
            ExamMarkMaster _exammarkmaster = null;
            try
            {

                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExamMarkMaster> rowMapper = GetMaper();

                var accessor = db.CreateSprocAccessor<ExamMarkMaster>(sqlGetById, rowMapper);
                _exammarkmaster = accessor.Execute(id).SingleOrDefault();

            }
            catch (Exception ex)
            {
                return _exammarkmaster;
            }

            return _exammarkmaster;
        }

        public List<ExamMarkMaster> GetAll()
        {
            List<ExamMarkMaster> exammarkmasterList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExamMarkMaster> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<ExamMarkMaster>(sqlGetAll, mapper);
                IEnumerable<ExamMarkMaster> collection = accessor.Execute();

                exammarkmasterList = collection.ToList();
            }

            catch (Exception ex)
            {
                return exammarkmasterList;
            }

            return exammarkmasterList;
        }


        #region Mapper
        private Database addParam(Database db, DbCommand cmd, ExamMarkMaster exammarkmaster, bool isInsert)
        {
            if (isInsert)
            {
                db.AddOutParameter(cmd, "ExamMarkMasterId", DbType.Int32, Int32.MaxValue);
            }
            else
            {
                db.AddInParameter(cmd, "ExamMarkMasterId", DbType.Int32, exammarkmaster.ExamMarkMasterId);
            }


            db.AddInParameter(cmd, "ExamMark", DbType.Decimal, exammarkmaster.ExamMark);
            db.AddInParameter(cmd, "ExamMarkEntryDate", DbType.DateTime, exammarkmaster.ExamMarkEntryDate);
            db.AddInParameter(cmd, "FinalSubmissionDate", DbType.DateTime, exammarkmaster.FinalSubmissionDate);
            db.AddInParameter(cmd, "ExamTemplateItemId", DbType.Int32, exammarkmaster.ExamTemplateItemId);
            db.AddInParameter(cmd, "AcaCalSectionId", DbType.Int32, exammarkmaster.AcaCalSectionId);
            db.AddInParameter(cmd, "IsFinalSubmit", DbType.Boolean, exammarkmaster.IsFinalSubmit);
            db.AddInParameter(cmd, "Attribute1", DbType.String, exammarkmaster.Attribute1);
            db.AddInParameter(cmd, "Attribute2", DbType.String, exammarkmaster.Attribute2);
            db.AddInParameter(cmd, "Attribute3", DbType.String, exammarkmaster.Attribute3);
            db.AddInParameter(cmd, "CreatedBy", DbType.Int32, exammarkmaster.CreatedBy);
            db.AddInParameter(cmd, "CreatedDate", DbType.DateTime, exammarkmaster.CreatedDate);
            db.AddInParameter(cmd, "ModifiedBy", DbType.Int32, exammarkmaster.ModifiedBy);
            db.AddInParameter(cmd, "ModifiedDate", DbType.DateTime, exammarkmaster.ModifiedDate);

            return db;
        }

        private IRowMapper<ExamMarkMaster> GetMaper()
        {
            IRowMapper<ExamMarkMaster> mapper = MapBuilder<ExamMarkMaster>.MapAllProperties()

            .Map(m => m.ExamMarkMasterId).ToColumn("ExamMarkMasterId")
            .Map(m => m.ExamMark).ToColumn("ExamMark")
            .Map(m => m.ExamMarkEntryDate).ToColumn("ExamMarkEntryDate")
            .Map(m => m.FinalSubmissionDate).ToColumn("FinalSubmissionDate")
            .Map(m => m.ExamTemplateItemId).ToColumn("ExamTemplateItemId")
            .Map(m => m.AcaCalSectionId).ToColumn("AcaCalSectionId")
            .Map(m => m.IsFinalSubmit).ToColumn("IsFinalSubmit")
            .Map(m => m.Attribute1).ToColumn("Attribute1")
            .Map(m => m.Attribute2).ToColumn("Attribute2")
            .Map(m => m.Attribute3).ToColumn("Attribute3")
            .Map(m => m.CreatedBy).ToColumn("CreatedBy")
            .Map(m => m.CreatedDate).ToColumn("CreatedDate")
            .Map(m => m.ModifiedBy).ToColumn("ModifiedBy")
            .Map(m => m.ModifiedDate).ToColumn("ModifiedDate")

            .Build();

            return mapper;
        }
        #endregion


        public ExamMarkMaster GetByAcaCalSectionIdExamTemplateItemId(int acaCalsectionId, int examTemplateBasicItemId)
        {
            ExamMarkMaster _exammarkmaster = null;
            try
            {

                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExamMarkMaster> rowMapper = GetMaper();

                var accessor = db.CreateSprocAccessor<ExamMarkMaster>(sqlGetByAcaCalSectionIdExamTemplateItemId, rowMapper);
                _exammarkmaster = accessor.Execute(acaCalsectionId, examTemplateBasicItemId).FirstOrDefault();

            }
            catch (Exception ex)
            {
                return _exammarkmaster;
            }

            return _exammarkmaster;
        }

        public bool FinalSubmitByAcacalSectionId(int acaCalSectionId, bool isFinalSubmit)
        {
            bool result = false;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand("ExamMarkFinalSubmitByAcacalSectionId");

                db.AddInParameter(cmd, "AcacalSectionId", DbType.Int32, acaCalSectionId);
                db.AddInParameter(cmd, "IsFinalSubmit", DbType.Int32, isFinalSubmit);
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

        public bool ExamMarkBackToTeacherByAcacalSectionId(int acaCalSectionId, bool isFinalSubmit)
        {
            bool result = false;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand("ExamMarkBackToTeacherByAcacalSectionId");

                db.AddInParameter(cmd, "AcacalSectionId", DbType.Int32, acaCalSectionId);
                db.AddInParameter(cmd, "IsFinalSubmit", DbType.Int32, isFinalSubmit);
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

        public List<TempStudentExamMarkColumnWise> GetContinuousMarkColumnWise(int acaCalSectionId)
        {
            List<TempStudentExamMarkColumnWise> examMarkColumnWiseList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<TempStudentExamMarkColumnWise> mapper = GetStudentExamMarkMaper();

                var accessor = db.CreateSprocAccessor<TempStudentExamMarkColumnWise>(sqlGetByStudentContinuousMarkColmneWiseProcess, mapper);
                IEnumerable<TempStudentExamMarkColumnWise> collection = accessor.Execute(acaCalSectionId);

                examMarkColumnWiseList = collection.ToList();
            }

            catch (Exception ex)
            {
                return examMarkColumnWiseList;
            }

            return examMarkColumnWiseList;
        }

        public List<TempStudentExamMarkColumnWise> GetAllMarkColumnWise(int acaCalSectionId)
        {
            List<TempStudentExamMarkColumnWise> examMarkColumnWiseList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<TempStudentExamMarkColumnWise> mapper = GetStudentExamMarkMaper();

                var accessor = db.CreateSprocAccessor<TempStudentExamMarkColumnWise>(sqlGetByStudentExamMarkColmneWiseProcess, mapper);
                IEnumerable<TempStudentExamMarkColumnWise> collection = accessor.Execute(acaCalSectionId);

                examMarkColumnWiseList = collection.ToList();
            }

            catch (Exception ex)
            {
                return examMarkColumnWiseList;
            }

            return examMarkColumnWiseList;
        }

        private IRowMapper<TempStudentExamMarkColumnWise> GetStudentExamMarkMaper()
        {
            IRowMapper<TempStudentExamMarkColumnWise> mapper = MapBuilder<TempStudentExamMarkColumnWise>.MapAllProperties()

            .Map(m => m.StudentCourseHistoryId).ToColumn("StudentCourseHistoryId")
            .Map(m => m.StudentId).ToColumn("StudentId")
            .Map(m => m.AcaCalSectionID).ToColumn("AcaCalSectionID")
            .Map(m => m.Roll).ToColumn("Roll")
            .Map(m => m.ExamTemplateItemId).ToColumn("ExamTemplateItemId")
            .Map(m => m.ColumnSequence).ToColumn("ColumnSequence")
            .Map(m => m.ColumnName).ToColumn("ColumnName")
            .Map(m => m.GradePoint).ToColumn("GradePoint")
            .Map(m => m.Grade).ToColumn("Grade")
            .Map(m => m.Marks).ToColumn("Marks")
            .Map(m => m.ExamStatus).ToColumn("ExamStatus")

            .Build();

            return mapper;
        }

        public List<ExamCommitteeDashboardDTO> GetExamCommitteeDashboard(int programId, int yearNo, int semesterNo, int examId)
        {
            List<ExamCommitteeDashboardDTO> examCommitteeDashboard = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExamCommitteeDashboardDTO> mapper = GetExamCommitteeDashboardMaper();

                var accessor = db.CreateSprocAccessor<ExamCommitteeDashboardDTO>("ExamCommitteDashboardByProgramYearSemesterExam", mapper);
                IEnumerable<ExamCommitteeDashboardDTO> collection = accessor.Execute(programId, yearNo, semesterNo, examId);

                examCommitteeDashboard = collection.ToList();
            }

            catch (Exception ex)
            {
                return examCommitteeDashboard;
            }

            return examCommitteeDashboard;
        }

        public CountinousMarkTeacherInfoAPI_DTO GetTeacherInfoAPIBySectionId(int sectionId)
        {
            CountinousMarkTeacherInfoAPI_DTO countinousMarkTeacherInfoAPIObj = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<CountinousMarkTeacherInfoAPI_DTO> rowMapper = GetTeacherInfoAPIMaper();

                var accessor = db.CreateSprocAccessor<CountinousMarkTeacherInfoAPI_DTO>("TeacherInfoForAPIBySectionId", rowMapper);
                countinousMarkTeacherInfoAPIObj = accessor.Execute(sectionId).SingleOrDefault();

            }

            catch (Exception ex)
            {
                return countinousMarkTeacherInfoAPIObj;
            }

            return countinousMarkTeacherInfoAPIObj;
        }

        public ExamCommitteeDashboardDTO GetExamCommitteeDashboardExtendByAcaCalSecId(int acaCalSecId)
        {
            ExamCommitteeDashboardDTO examCommitteeDashboard = null;
            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                IRowMapper<ExamCommitteeDashboardDTO> mapper = GetExamCommitteeDashboardExtendMapper();
                var accessor = db.CreateSprocAccessor<ExamCommitteeDashboardDTO>("GetExamCommitteeDashboardExtendByAcaCalSecId", mapper);
                examCommitteeDashboard = accessor.Execute(acaCalSecId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return examCommitteeDashboard;
            }
            return examCommitteeDashboard;
        }

        public List<ExamCommitteeDashboardDTO> GetExamCommitteeDashboardExtendForGroupByAcaCalSecId(int acaCalSecId)
        {
            List<ExamCommitteeDashboardDTO> examCommitteeDashboard = null;
            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                IRowMapper<ExamCommitteeDashboardDTO> mapper = GetExamCommitteeDashboardExtendForGroupMapper();
                var accessor = db.CreateSprocAccessor<ExamCommitteeDashboardDTO>("GetExamCommitteeDashboardExtendForGroupByAcaCalSecId", mapper);
                IEnumerable<ExamCommitteeDashboardDTO> collection = accessor.Execute(acaCalSecId);
                examCommitteeDashboard = collection.ToList();
            }
            catch (Exception ex)
            {
                return examCommitteeDashboard;
            }
            return examCommitteeDashboard;
        }

        public ExamCommitteeDashboardDTO GetExaminerByAcaCalSecId(int acaCalSecId)
        {
            ExamCommitteeDashboardDTO examCommitteeDashboard = null;
            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                IRowMapper<ExamCommitteeDashboardDTO> mapper = GetExaminerMapper();
                var accessor = db.CreateSprocAccessor<ExamCommitteeDashboardDTO>("GetExaminerByAcaCalSecId", mapper);
                examCommitteeDashboard = accessor.Execute(acaCalSecId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return examCommitteeDashboard;
            }
            return examCommitteeDashboard;
        }

        #region Mapper

        private IRowMapper<ExamCommitteeDashboardDTO> GetExamCommitteeDashboardMaper()
        {
            IRowMapper<ExamCommitteeDashboardDTO> mapper = MapBuilder<ExamCommitteeDashboardDTO>.MapAllProperties()

                .Map(m => m.AcaCal_SectionID).ToColumn("AcaCal_SectionID")
                .Map(m => m.CourseID).ToColumn("CourseID")
                .Map(m => m.VersionID).ToColumn("VersionID")
                .Map(m => m.YearNo).ToColumn("YearNo")
                .Map(m => m.SemesterNo).ToColumn("SemesterNo")
                .Map(m => m.ExamName).ToColumn("ExamName")
                .Map(m => m.Year).ToColumn("Year")
                .Map(m => m.CourseCode).ToColumn("CourseCode")
                .Map(m => m.CourseName).ToColumn("CourseName")
                .Map(m => m.TearcherOneName).ToColumn("TearcherOneName")
                .Map(m => m.IsContinousMarkSubmit).ToColumn("IsContinousMarkSubmit")
                .Map(m => m.FinalSubmissionDate).ToColumn("FinalSubmissionDate")
                .Map(m => m.ExamSetupId).ToColumn("ExamSetupId")


                .DoNotMap(m=>m.ThirdExaminationEligibleStudentCount)
                .DoNotMap(m=>m.TotalStudent)
                .DoNotMap(m=>m.FirstExaminerMarkSubmissionStatus)
                .DoNotMap(m=>m.SecondExaminerMarkSubmissionStatus)
                .DoNotMap(m=>m.ThirdExaminerMarkSubmissionStatus)
                .DoNotMap(m=>m.ExaminerMarkSubmissionStatus)
                .DoNotMap(m=>m.FirstExaminerName)
                .DoNotMap(m=>m.SecondExaminerName)
                .DoNotMap(m=>m.ThirdExaminerName)
                .DoNotMap(m=>m.FirstExaminerMarkSubmissionStatusDate)
                .DoNotMap(m=>m.SecondExaminerMarkSubmissionStatusDate)
                .DoNotMap(m=>m.ThirdExaminerMarkSubmissionStatusDate)
                .DoNotMap(m=>m.FirstExaminerMarkSubmissionDate)
                .DoNotMap(m=>m.SecondExaminerMarkSubmissionDate)
                .DoNotMap(m=>m.ThirdExaminerMarkSubmissionDate)

                .DoNotMap(m => m.FirstExaminerId)
                .DoNotMap(m => m.SecondExaminerId)
                .DoNotMap(m => m.ThirdExaminerId)

        
                .Build();

            return mapper;
        }

        private IRowMapper<ExamCommitteeDashboardDTO> GetExamCommitteeDashboardExtendMapper()
        {
            IRowMapper<ExamCommitteeDashboardDTO> mapper = MapBuilder<ExamCommitteeDashboardDTO>.MapAllProperties()

                .DoNotMap(m => m.AcaCal_SectionID)
                .DoNotMap(m => m.ExamSetupId)
                .DoNotMap(m => m.CourseID)
                .DoNotMap(m => m.VersionID)
                .DoNotMap(m => m.YearNo)
                .DoNotMap(m => m.SemesterNo)
                .DoNotMap(m => m.ExamName)
                .DoNotMap(m => m.Year)
                .DoNotMap(m => m.CourseCode)
                .DoNotMap(m => m.CourseName)
                .DoNotMap(m => m.TearcherOneName)
                .DoNotMap(m => m.IsContinousMarkSubmit)
                .DoNotMap(m => m.FinalSubmissionDate)

                .DoNotMap(m => m.FirstExaminerId)
                .DoNotMap(m => m.SecondExaminerId)
                .DoNotMap(m => m.ThirdExaminerId)
                .DoNotMap(m => m.FirstExaminerName)
                .DoNotMap(m => m.SecondExaminerName)
                .DoNotMap(m => m.ThirdExaminerName)
                .DoNotMap(m => m.ExaminerMarkSubmissionStatus)

                .DoNotMap(m=>m.FirstExaminerMarkSubmissionDate)
                .DoNotMap(m=>m.SecondExaminerMarkSubmissionDate)
                .DoNotMap(m=>m.ThirdExaminerMarkSubmissionDate)

                .Map(m => m.ThirdExaminationEligibleStudentCount).ToColumn("ThirdExaminationEligibleStudentCount")
                .Map(m => m.TotalStudent).ToColumn("TotalStudent")
                .Map(m => m.FirstExaminerMarkSubmissionStatus).ToColumn("FirstExaminerMarkSubmissionStatus")
                .Map(m => m.SecondExaminerMarkSubmissionStatus).ToColumn("SecondExaminerMarkSubmissionStatus")
                .Map(m => m.ThirdExaminerMarkSubmissionStatus).ToColumn("ThirdExaminerMarkSubmissionStatus")
               
                .Map(m => m.FirstExaminerMarkSubmissionStatusDate).ToColumn("FirstExaminerMarkSubmissionStatusDate")
                .Map(m => m.SecondExaminerMarkSubmissionStatusDate).ToColumn("SecondExaminerMarkSubmissionStatusDate")
                .Map(m => m.ThirdExaminerMarkSubmissionStatusDate).ToColumn("ThirdExaminerMarkSubmissionStatusDate")


                .Build();

            return mapper;
        }

        private IRowMapper<ExamCommitteeDashboardDTO> GetExamCommitteeDashboardExtendForGroupMapper()
        {
            IRowMapper<ExamCommitteeDashboardDTO> mapper = MapBuilder<ExamCommitteeDashboardDTO>.MapAllProperties()

                .DoNotMap(m => m.AcaCal_SectionID)
                .DoNotMap(m => m.ExamSetupId)
                .DoNotMap(m => m.CourseID)
                .DoNotMap(m => m.VersionID)
                .DoNotMap(m => m.YearNo)
                .DoNotMap(m => m.SemesterNo)
                .DoNotMap(m => m.ExamName)
                .DoNotMap(m => m.Year)
                .DoNotMap(m => m.CourseCode)
                .DoNotMap(m => m.CourseName)
                .DoNotMap(m => m.TearcherOneName)
                .DoNotMap(m => m.IsContinousMarkSubmit)
                .DoNotMap(m => m.FinalSubmissionDate)

                .DoNotMap(m => m.FirstExaminerName)
                .DoNotMap(m => m.SecondExaminerName)
                .DoNotMap(m => m.ThirdExaminerName)
                .DoNotMap(m => m.ExaminerMarkSubmissionStatus)

                .DoNotMap(m => m.FirstExaminerMarkSubmissionDate)
                .DoNotMap(m => m.SecondExaminerMarkSubmissionDate)
                .DoNotMap(m => m.ThirdExaminerMarkSubmissionDate)

                .Map(m => m.ThirdExaminationEligibleStudentCount).ToColumn("ThirdExaminationEligibleStudentCount")
                .Map(m => m.TotalStudent).ToColumn("TotalStudent")
                .Map(m => m.FirstExaminerMarkSubmissionStatus).ToColumn("FirstExaminerMarkSubmissionStatus")
                .Map(m => m.SecondExaminerMarkSubmissionStatus).ToColumn("SecondExaminerMarkSubmissionStatus")
                .Map(m => m.ThirdExaminerMarkSubmissionStatus).ToColumn("ThirdExaminerMarkSubmissionStatus")

                .Map(m => m.FirstExaminerMarkSubmissionStatusDate).ToColumn("FirstExaminerMarkSubmissionStatusDate")
                .Map(m => m.SecondExaminerMarkSubmissionStatusDate).ToColumn("SecondExaminerMarkSubmissionStatusDate")
                .Map(m => m.ThirdExaminerMarkSubmissionStatusDate).ToColumn("ThirdExaminerMarkSubmissionStatusDate")
                .Map(m => m.FirstExaminerId).ToColumn("FirstExaminer")
                .Map(m => m.SecondExaminerId).ToColumn("SecondExaminer")
                .Map(m => m.ThirdExaminerId).ToColumn("ThirdExaminer")

                .Build();

            return mapper;
        }
        private IRowMapper<ExamCommitteeDashboardDTO> GetExaminerMapper()
        {
            IRowMapper<ExamCommitteeDashboardDTO> mapper = MapBuilder<ExamCommitteeDashboardDTO>.MapAllProperties()

                .DoNotMap(m => m.AcaCal_SectionID)
                .DoNotMap(m => m.ExamSetupId)
                .DoNotMap(m => m.CourseID)
                .DoNotMap(m => m.VersionID)
                .DoNotMap(m => m.YearNo)
                .DoNotMap(m => m.SemesterNo)
                .DoNotMap(m => m.ExamName)
                .DoNotMap(m => m.Year)
                .DoNotMap(m => m.CourseCode)
                .DoNotMap(m => m.CourseName)
                .DoNotMap(m => m.TearcherOneName)
                .DoNotMap(m => m.IsContinousMarkSubmit)
                .DoNotMap(m => m.FinalSubmissionDate)
                .DoNotMap(m => m.ThirdExaminationEligibleStudentCount)
                .DoNotMap(m => m.TotalStudent)
                .DoNotMap(m => m.FirstExaminerMarkSubmissionStatus)
                .DoNotMap(m => m.SecondExaminerMarkSubmissionStatus)
                .DoNotMap(m => m.ThirdExaminerMarkSubmissionStatus)
                .DoNotMap(m => m.ExaminerMarkSubmissionStatus)

                .DoNotMap(m => m.FirstExaminerMarkSubmissionStatusDate)
                .DoNotMap(m => m.SecondExaminerMarkSubmissionStatusDate)
                .DoNotMap(m => m.ThirdExaminerMarkSubmissionStatusDate)

                .DoNotMap(m=>m.FirstExaminerMarkSubmissionDate)
                .DoNotMap(m=>m.SecondExaminerMarkSubmissionDate)
                .DoNotMap(m=>m.ThirdExaminerMarkSubmissionDate)

                .DoNotMap(m => m.FirstExaminerId)
                .DoNotMap(m => m.SecondExaminerId)
                .DoNotMap(m => m.ThirdExaminerId)

                .Map(m => m.FirstExaminerName).ToColumn("FirstExaminerName")
                .Map(m => m.SecondExaminerName).ToColumn("SecondExaminerName")
                .Map(m => m.ThirdExaminerName).ToColumn("ThirdExaminerName")

                .Build();

            return mapper;
        }

        private IRowMapper<CountinousMarkTeacherInfoAPI_DTO> GetTeacherInfoAPIMaper()
        {
            IRowMapper<CountinousMarkTeacherInfoAPI_DTO> mapper = MapBuilder<CountinousMarkTeacherInfoAPI_DTO>.MapAllProperties()

                .Map(m => m.TeacherName).ToColumn("TeacherName")
                .Map(m => m.DepartmentName).ToColumn("DepartmentName")
                .Map(m => m.SessionName).ToColumn("SessionName")
                .Map(m => m.ExamMark).ToColumn("ExamMark")
                
                .Build();

            return mapper;
        }

        #endregion

        #region Tabulation Data

        #region Methods
        public List<RTabulationData> GetTabulationDataByProgramYearSemesterExam(int programId, int yearNo, int semesterNo, int examId)
        {
            List<RTabulationData> TabulationData = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<RTabulationData> mapper = GetTabulationDataMaper();

                var accessor = db.CreateSprocAccessor<RTabulationData>("GetTabulationDataByProgramYearSemesterExam", mapper);
                IEnumerable<RTabulationData> collection = accessor.Execute(programId, yearNo, semesterNo, examId);

                TabulationData = collection.ToList();
            }

            catch (Exception ex)
            {
                return TabulationData;
            }

            return TabulationData;
        }
        #endregion


        #region Mapper
        private IRowMapper<RTabulationData> GetTabulationDataMaper()
        {
            IRowMapper<RTabulationData> mapper = MapBuilder<RTabulationData>.MapAllProperties()

            .Map(m => m.Roll).ToColumn("Roll")
            .Map(m => m.FullName).ToColumn("FullName")
            .Map(m => m.FormalCode).ToColumn("FormalCode")
            .Map(m => m.RegistrationNo).ToColumn("RegistrationNo")
            .Map(m => m.Code).ToColumn("Code")
            .Map(m => m.ExamMark).ToColumn("ExamMark")
            .Map(m => m.Credits).ToColumn("Credits")
            .Map(m => m.ID).ToColumn("ID")
            .Map(m => m.StudentID).ToColumn("StudentID")
            .Map(m => m.ExamGroupName).ToColumn("ExamGroupName")
            .Map(m => m.ExamName).ToColumn("ExamName")
            .Map(m => m.ColumnSequence).ToColumn("ColumnSequence")
            .Map(m => m.Marks).ToColumn("Marks")
            .Map(m => m.ObtainedGrade).ToColumn("ObtainedGrade")
            .Map(m => m.ObtainedGPA).ToColumn("ObtainedGPA")
            .Map(m => m.PointSecured).ToColumn("PointSecured")
            .Map(m => m.SemesterGPA).ToColumn("SemesterGPA")
            .Map(m => m.TotalCredit).ToColumn("TotalCredit")
            .Map(m => m.TotalPoint).ToColumn("TotalPoint")
            .Map(m => m.TotalMarks).ToColumn("TotalMarks")
            .Build();

            return mapper;
        }
        #endregion


        #endregion

        #region Continuous Assessment Auto Mark Entry

        public bool AutoMarkEntryForContinuousAssessmentTemplateIdBySectionId(int sectionId)
        {
            bool result = false;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                DbCommand cmd = db.GetStoredProcCommand("AutoMarkEntryForContinuousAssessmentTemplateIdBySectionId");
                db.AddInParameter(cmd, "sectionId", DbType.Int32, sectionId);
                db.AddOutParameter(cmd, "ReturnValue", DbType.Int32, 0);
                db.ExecuteNonQuery(cmd);

                object obj = db.GetParameterValue(cmd, "ReturnValue");

                int id = 0;
                if (obj != null)
                {
                    int.TryParse(obj.ToString(), out id);
                }

                if (id > 0)
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

        #endregion

    }
}

