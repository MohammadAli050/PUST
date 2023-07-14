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

namespace LogicLayer.DataLogic.SQLRepository
{
    public partial class SqlRegistrationWorksheetRepository : IRegistrationWorksheetRepository
    {

        Database db = null;

        private string sqlInsert = "RegistrationWorksheetInsert";
        private string sqlUpdate = "RegistrationWorksheetUpdate";
        private string sqlDelete = "RegistrationWorksheetDelete";
        private string sqlGetById = "RegistrationWorksheetGetById";
        private string sqlGetAll = "RegistrationWorksheetGetAll";
        private string sqlGetAllByStudentID = "RegistrationWorksheetGetAllByStudentID";
        private string sqlGetAllByStudentIdAcacalId = "RegistrationWorksheetGetAllByStudentIdAcacalId";
               
        public int Insert(RegistrationWorksheet registrationworksheet)
        {
            int id = 0;
            bool isInsert = true;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand(sqlInsert);

                db = addParam(db, cmd, registrationworksheet, isInsert);
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

        public bool Update(RegistrationWorksheet registrationworksheet)
        {
            bool result = false;
            bool isInsert = false;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand(sqlUpdate);

                db = addParam(db, cmd, registrationworksheet, isInsert);

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

        public RegistrationWorksheet GetById(int? id)
        {
            RegistrationWorksheet _registrationworksheet = null;
            try
            {

                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<RegistrationWorksheet> rowMapper = GetMaper();

                var accessor = db.CreateSprocAccessor<RegistrationWorksheet>(sqlGetById, rowMapper);
                _registrationworksheet = accessor.Execute(id).SingleOrDefault();

            }
            catch (Exception ex)
            {
                return _registrationworksheet;
            }

            return _registrationworksheet;
        }

        public List<RegistrationWorksheet> GetAll()
        {
            List<RegistrationWorksheet> registrationworksheetList= null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<RegistrationWorksheet> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<RegistrationWorksheet>(sqlGetAll, mapper);
                IEnumerable<RegistrationWorksheet> collection = accessor.Execute();

                registrationworksheetList = collection.ToList();
            }

            catch (Exception ex)
            {
                return registrationworksheetList;
            }

            return registrationworksheetList;
        }
       
        #region Mapper
        private Database addParam(Database db, DbCommand cmd, RegistrationWorksheet registrationworksheet, bool isInsert)
        {
            if (isInsert)
            {
                db.AddOutParameter(cmd, "ID", DbType.Int32, Int32.MaxValue);
            }
            else
            {
                db.AddInParameter(cmd, "ID", DbType.Int32, registrationworksheet.ID);
            }

            	
		db.AddInParameter(cmd,"StudentID",DbType.Int32,registrationworksheet.StudentID);
		db.AddInParameter(cmd,"CalCourseProgNodeID",DbType.Int32,registrationworksheet.CalCourseProgNodeID);
		db.AddInParameter(cmd,"IsCompleted",DbType.Boolean,registrationworksheet.IsCompleted);
		db.AddInParameter(cmd,"OriginalCalID",DbType.Int32,registrationworksheet.OriginalCalID);
        db.AddInParameter(cmd, "Session", DbType.String, registrationworksheet.Session);
        db.AddInParameter(cmd, "YearId", DbType.Int32, registrationworksheet.YearId);
        db.AddInParameter(cmd, "SemesterId", DbType.Int32, registrationworksheet.SemesterId);
        db.AddInParameter(cmd, "YearNo", DbType.Int32, registrationworksheet.YearNo);
        db.AddInParameter(cmd, "SemesterNo", DbType.Int32, registrationworksheet.SemesterNo);
        db.AddInParameter(cmd, "ExamId", DbType.Int32, registrationworksheet.ExamId);
		db.AddInParameter(cmd,"IsAutoAssign",DbType.Boolean,registrationworksheet.IsAutoAssign);
		db.AddInParameter(cmd,"IsAutoOpen",DbType.Boolean,registrationworksheet.IsAutoOpen);
		db.AddInParameter(cmd,"IsRequisitioned",DbType.Boolean,registrationworksheet.IsRequisitioned);
		db.AddInParameter(cmd,"IsMandatory",DbType.Boolean,registrationworksheet.IsMandatory);
		db.AddInParameter(cmd,"IsManualOpen",DbType.Boolean,registrationworksheet.IsManualOpen);
		db.AddInParameter(cmd,"TreeCalendarDetailID",DbType.Int32,registrationworksheet.TreeCalendarDetailID);
		db.AddInParameter(cmd,"TreeCalendarMasterID",DbType.Int32,registrationworksheet.TreeCalendarMasterID);
		db.AddInParameter(cmd,"TreeMasterID",DbType.Int32,registrationworksheet.TreeMasterID);
		db.AddInParameter(cmd,"CalendarMasterName",DbType.String,registrationworksheet.CalendarMasterName);
		db.AddInParameter(cmd,"CalendarDetailName",DbType.String,registrationworksheet.CalendarDetailName);
		db.AddInParameter(cmd,"CourseID",DbType.Int32,registrationworksheet.CourseID);
		db.AddInParameter(cmd,"VersionID",DbType.Int32,registrationworksheet.VersionID);
		db.AddInParameter(cmd,"Credits",DbType.Int32,registrationworksheet.Credits);
		db.AddInParameter(cmd,"Node_CourseID",DbType.Int32,registrationworksheet.Node_CourseID);
		db.AddInParameter(cmd,"NodeID",DbType.Int32,registrationworksheet.NodeID);
		db.AddInParameter(cmd,"IsMinorRelated",DbType.Boolean,registrationworksheet.IsMinorRelated);
		db.AddInParameter(cmd,"IsMajorRelated",DbType.Boolean,registrationworksheet.IsMajorRelated);
		db.AddInParameter(cmd,"NodeLinkName",DbType.String,registrationworksheet.NodeLinkName);
		db.AddInParameter(cmd,"FormalCode",DbType.String,registrationworksheet.FormalCode);
		db.AddInParameter(cmd,"VersionCode",DbType.String,registrationworksheet.VersionCode);
		db.AddInParameter(cmd,"CourseTitle",DbType.String,registrationworksheet.CourseTitle);
		db.AddInParameter(cmd,"AcaCal_SectionID",DbType.Int32,registrationworksheet.AcaCal_SectionID);
		db.AddInParameter(cmd,"SectionName",DbType.String,registrationworksheet.SectionName);
		db.AddInParameter(cmd,"ProgramID",DbType.Int32,registrationworksheet.ProgramID);
		db.AddInParameter(cmd,"DeptID",DbType.Int32,registrationworksheet.DeptID);
		db.AddInParameter(cmd,"Priority",DbType.Int32,registrationworksheet.Priority);
		db.AddInParameter(cmd,"RetakeNo",DbType.Int32,registrationworksheet.RetakeNo);
		db.AddInParameter(cmd,"ObtainedGPA",DbType.Decimal,registrationworksheet.ObtainedGPA);
		db.AddInParameter(cmd,"ObtainedGrade",DbType.String,registrationworksheet.ObtainedGrade);
		db.AddInParameter(cmd,"AcaCalYear",DbType.Int32,registrationworksheet.AcaCalYear);
		db.AddInParameter(cmd,"BatchCode",DbType.String,registrationworksheet.BatchCode);
		db.AddInParameter(cmd,"AcaCalTypeName",DbType.String,registrationworksheet.AcaCalTypeName);
        db.AddInParameter(cmd, "CalCrsProgNdCredits", DbType.Decimal, registrationworksheet.CalCrsProgNdCredits);
		db.AddInParameter(cmd,"CalCrsProgNdIsMajorRelated",DbType.Boolean,registrationworksheet.CalCrsProgNdIsMajorRelated);
		db.AddInParameter(cmd,"IsMultipleACUSpan",DbType.Boolean,registrationworksheet.IsMultipleACUSpan);
		db.AddInParameter(cmd,"CompletedCredit",DbType.Decimal,registrationworksheet.CompletedCredit);
		db.AddInParameter(cmd,"CourseCredit",DbType.Decimal,registrationworksheet.CourseCredit);
		db.AddInParameter(cmd,"CreatedBy",DbType.Int32,registrationworksheet.CreatedBy);
		db.AddInParameter(cmd,"CreatedDate",DbType.DateTime,registrationworksheet.CreatedDate);
		db.AddInParameter(cmd,"ModifiedBy",DbType.Int32,registrationworksheet.ModifiedBy);
		db.AddInParameter(cmd,"ModifiedDate",DbType.DateTime,registrationworksheet.ModifiedDate);
		db.AddInParameter(cmd,"CourseStatusId",DbType.Int32,registrationworksheet.CourseStatusId);
		db.AddInParameter(cmd,"IsRegistered",DbType.Boolean,registrationworksheet.IsRegistered);
		db.AddInParameter(cmd,"IsAdd",DbType.Boolean,registrationworksheet.IsAdd);
		db.AddInParameter(cmd,"ConflictedCourse",DbType.String,registrationworksheet.ConflictedCourse);
		db.AddInParameter(cmd,"SequenceNo",DbType.Int32,registrationworksheet.SequenceNo);
		db.AddInParameter(cmd,"IsOfferedCourse",DbType.Boolean,registrationworksheet.IsOfferedCourse);
		db.AddInParameter(cmd,"CourseResultAcaCalID",DbType.Int32,registrationworksheet.CourseResultAcaCalID);
		db.AddInParameter(cmd,"PostMajorNodeLevel",DbType.Int32,registrationworksheet.PostMajorNodeLevel);
		db.AddInParameter(cmd,"IsCreditCourse",DbType.Boolean,registrationworksheet.IsCreditCourse);
		db.AddInParameter(cmd,"BatchID",DbType.Int32,registrationworksheet.BatchID);
        db.AddInParameter(cmd, "HeldInRelationId", DbType.Int32, registrationworksheet.HeldInRelationId);

            
            return db;
        }

        private IRowMapper<RegistrationWorksheet> GetMaper()
        {
            IRowMapper<RegistrationWorksheet> mapper = MapBuilder<RegistrationWorksheet>.MapAllProperties()

       	   .Map(m => m.ID).ToColumn("ID")
		.Map(m => m.StudentID).ToColumn("StudentID")
		.Map(m => m.CalCourseProgNodeID).ToColumn("CalCourseProgNodeID")
		.Map(m => m.IsCompleted).ToColumn("IsCompleted")
		.Map(m => m.OriginalCalID).ToColumn("OriginalCalID")
        .Map(m => m.Session).ToColumn("Session")
        .Map(m => m.YearId).ToColumn("YearId")
        .Map(m => m.SemesterId).ToColumn("SemesterId")
        .Map(m => m.YearNo).ToColumn("YearNo")
        .Map(m => m.SemesterNo).ToColumn("SemesterNo")
        .Map(m => m.ExamId).ToColumn("ExamId")
		.Map(m => m.IsAutoAssign).ToColumn("IsAutoAssign")
		.Map(m => m.IsAutoOpen).ToColumn("IsAutoOpen")
		.Map(m => m.IsRequisitioned).ToColumn("IsRequisitioned")
		.Map(m => m.IsMandatory).ToColumn("IsMandatory")
		.Map(m => m.IsManualOpen).ToColumn("IsManualOpen")
		.Map(m => m.TreeCalendarDetailID).ToColumn("TreeCalendarDetailID")
		.Map(m => m.TreeCalendarMasterID).ToColumn("TreeCalendarMasterID")
		.Map(m => m.TreeMasterID).ToColumn("TreeMasterID")
		.Map(m => m.CalendarMasterName).ToColumn("CalendarMasterName")
		.Map(m => m.CalendarDetailName).ToColumn("CalendarDetailName")
		.Map(m => m.CourseID).ToColumn("CourseID")
		.Map(m => m.VersionID).ToColumn("VersionID")
		.Map(m => m.Credits).ToColumn("Credits")
		.Map(m => m.Node_CourseID).ToColumn("Node_CourseID")
		.Map(m => m.NodeID).ToColumn("NodeID")
		.Map(m => m.IsMinorRelated).ToColumn("IsMinorRelated")
		.Map(m => m.IsMajorRelated).ToColumn("IsMajorRelated")
		.Map(m => m.NodeLinkName).ToColumn("NodeLinkName")
		.Map(m => m.FormalCode).ToColumn("FormalCode")
		.Map(m => m.VersionCode).ToColumn("VersionCode")
		.Map(m => m.CourseTitle).ToColumn("CourseTitle")
		.Map(m => m.AcaCal_SectionID).ToColumn("AcaCal_SectionID")
		.Map(m => m.SectionName).ToColumn("SectionName")
		.Map(m => m.ProgramID).ToColumn("ProgramID")
		.Map(m => m.DeptID).ToColumn("DeptID")
		.Map(m => m.Priority).ToColumn("Priority")
		.Map(m => m.RetakeNo).ToColumn("RetakeNo")
		.Map(m => m.ObtainedGPA).ToColumn("ObtainedGPA")
		.Map(m => m.ObtainedGrade).ToColumn("ObtainedGrade")
		.Map(m => m.AcaCalYear).ToColumn("AcaCalYear")
		.Map(m => m.BatchCode).ToColumn("BatchCode")
		.Map(m => m.AcaCalTypeName).ToColumn("AcaCalTypeName")
		.Map(m => m.CalCrsProgNdCredits).ToColumn("CalCrsProgNdCredits")
		.Map(m => m.CalCrsProgNdIsMajorRelated).ToColumn("CalCrsProgNdIsMajorRelated")
		.Map(m => m.IsMultipleACUSpan).ToColumn("IsMultipleACUSpan")
		.Map(m => m.CompletedCredit).ToColumn("CompletedCredit")
		.Map(m => m.CourseCredit).ToColumn("CourseCredit")
		.Map(m => m.CreatedBy).ToColumn("CreatedBy")
		.Map(m => m.CreatedDate).ToColumn("CreatedDate")
		.Map(m => m.ModifiedBy).ToColumn("ModifiedBy")
		.Map(m => m.ModifiedDate).ToColumn("ModifiedDate")
		.Map(m => m.CourseStatusId).ToColumn("CourseStatusId")
		.Map(m => m.IsRegistered).ToColumn("IsRegistered")
		.Map(m => m.IsAdd).ToColumn("IsAdd")
		.Map(m => m.ConflictedCourse).ToColumn("ConflictedCourse")
		.Map(m => m.SequenceNo).ToColumn("SequenceNo")
		.Map(m => m.IsOfferedCourse).ToColumn("IsOfferedCourse")
		.Map(m => m.CourseResultAcaCalID).ToColumn("CourseResultAcaCalID")
		.Map(m => m.PostMajorNodeLevel).ToColumn("PostMajorNodeLevel")
		.Map(m => m.IsCreditCourse).ToColumn("IsCreditCourse")
		.Map(m => m.BatchID).ToColumn("BatchID")
        .Map(m => m.HeldInRelationId).ToColumn("HeldInRelationId")

            
            .Build();

            return mapper;
        }
        #endregion

        public List<RegistrationWorksheet> GetAllOpenCourseWhichSectionIsMatchInStudentBatchByStudentID(int studentId, int registrationSession)
        {
            List<RegistrationWorksheet> list = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                IRowMapper<RegistrationWorksheet> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<RegistrationWorksheet>("RegistrationWorksheetGetAllOpenCourseWhichSectionIsMatchInStudentBatchByStudentID", mapper);
                IEnumerable<RegistrationWorksheet> collection = accessor.Execute(studentId, registrationSession);

                list = collection.ToList();
            }

            catch (Exception ex)
            {
                return list;
            }

            return list;
        }

        public List<RegistrationWorksheet> GetByStudentID(int studentID)
        {
            List<RegistrationWorksheet> list = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                IRowMapper<RegistrationWorksheet> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<RegistrationWorksheet>(sqlGetAllByStudentID, mapper);
                IEnumerable<RegistrationWorksheet> collection = accessor.Execute(studentID);

                list = collection.ToList();
            }

            catch (Exception ex)
            {
                return list;
            }

            return list;
        }

        public List<RegistrationWorksheet> GetByStudentIDAcacalID(int studentId, int acacalId)
        {
            List<RegistrationWorksheet> list = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                IRowMapper<RegistrationWorksheet> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<RegistrationWorksheet>(sqlGetAllByStudentIdAcacalId, mapper);
                IEnumerable<RegistrationWorksheet> collection = accessor.Execute(studentId, acacalId);

                list = collection.ToList();
            }

            catch (Exception ex)
            {
                return list;
            }

            return list;
        }

        public List<RegistrationWorksheet> GetByStudentIdYearNoSemesterNoExamId(int studentId, int yearNo, int semesterNo, int examId)
        {
            List<RegistrationWorksheet> list = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                IRowMapper<RegistrationWorksheet> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<RegistrationWorksheet>("RegistrationWorksheetGetByStudentIdYearNoSemesterNoExamId", mapper);
                IEnumerable<RegistrationWorksheet> collection = accessor.Execute(studentId, yearNo, semesterNo, examId);

                list = collection.ToList();
            }

            catch (Exception ex)
            {
                return list;
            }

            return list;
        }
    }
}

