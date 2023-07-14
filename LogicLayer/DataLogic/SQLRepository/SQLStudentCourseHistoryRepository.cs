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
using System.Data.SqlClient;
using LogicLayer.BusinessObjects.DTO;

namespace LogicLayer.DataLogic.SQLRepository
{
    public class SQLStudentCourseHistoryRepository : IStudentCourseHistoryRepository
    {
        Database db = null;

        private string sqlInsert = "StudentCourseHistoryInsert";
        private string sqlUpdate = "StudentCourseHistoryUpdate";
        private string sqlDelete = "StudentCourseHistoryDeleteById";
        private string sqlGetById = "StudentCourseHistoryGetById";
        private string sqlGetAll = "StudentCourseHistoryGetAll";
        private string sqlGetCompletedCredit = "CompletedCreditByRoll";
        private string sqlGetAttemptedCredit = "AttemptedCreditByRoll";
        private string sqlGetAllByAcaCalId = "StudentCourseHistoryGetAllByAcaCalId";
        private string sqlGetAllByAcaCalSectionId = "StudentCourseHistoryGetAllByAcaCalSectionId";
        private string sqlGetAllByStudentId = "StudentCourseHistoryGetAllByStudentId";
        private string sqlGetAllByStudentIdAcaCalId = "StudentCourseHistoryGetAllByStudentIdAcaCalId";
        private string sqlGetByStudentIdAcaCalId = "StudentCourseHistoryGetByStudentIdAcaCalId";
        private string sqlDeleteCourseHistoryByExamIdCourseIdStudentId = "DeleteCourseHistoryByExamIdCourseIdStudentId";


        public int Insert(StudentCourseHistory studentCourseHistory)
        {
            int id = 0;
            bool isInsert = true;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand(sqlInsert);

                db = addParam(db, cmd, studentCourseHistory, isInsert);
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

        public bool Update(StudentCourseHistory studentCourseHistory)
        {
            bool result = false;
            bool isInsert = false;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand(sqlUpdate);

                db = addParam(db, cmd, studentCourseHistory, isInsert);

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

        public bool UpdateLevelByStudentIdAcaCalId(int StudentID, int AcaCalId, int LevelId)
        {
            bool result = false; 

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand("StudentCourseHistoryUpdateLevelIdByStudentIdAcaCalId");

                db.AddInParameter(cmd, "StudentID", DbType.Int32, StudentID);
                db.AddInParameter(cmd, "AcaCalID", DbType.Int32, AcaCalId);
                db.AddInParameter(cmd, "LevelId", DbType.Int32, LevelId);

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

        public bool DeleteByExamIdCourseIdStudentId(int examId, int courseId, int studentId)
        {
            bool result = false;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand(sqlDeleteCourseHistoryByExamIdCourseIdStudentId);

                db.AddInParameter(cmd, "ExamId", DbType.Int32, examId);
                db.AddInParameter(cmd, "CourseId", DbType.Int32, courseId);
                db.AddInParameter(cmd, "StudentId", DbType.Int32, studentId);


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

        public decimal GetCompletedCredit(string Roll)
        {
            decimal completedCredit = 0;
            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand(sqlGetCompletedCredit);

                SqlParameter completedCreditParam = new SqlParameter("@CompletedCredit", SqlDbType.Decimal);
                completedCreditParam.Direction = ParameterDirection.Output;
                completedCreditParam.Precision = 5;
                completedCreditParam.Scale = 2;
                cmd.Parameters.Add(completedCreditParam);
                db.AddInParameter(cmd, "Roll", DbType.String, Roll);

                db.ExecuteNonQuery(cmd);

                object obj = db.GetParameterValue(cmd, "CompletedCredit");

                if (obj != null)
                {
                    Decimal.TryParse(obj.ToString(), out completedCredit);
                }
            }
            catch (Exception ex)
            {
                completedCredit = 0;
            }
            return completedCredit;
        }

        public decimal GetAttemptedCredit(string Roll)
        {
            decimal attemptedCredit = 0;
            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand(sqlGetAttemptedCredit);

                SqlParameter attemptedCreditParam = new SqlParameter("@AttemptedCredit", SqlDbType.Decimal);
                attemptedCreditParam.Direction = ParameterDirection.Output;
                attemptedCreditParam.Precision = 5;
                attemptedCreditParam.Scale = 2;
                cmd.Parameters.Add(attemptedCreditParam);
                db.AddInParameter(cmd, "Roll", DbType.String, Roll);

                db.ExecuteNonQuery(cmd);

                object obj = db.GetParameterValue(cmd, "AttemptedCredit");

                if (obj != null)
                {
                    Decimal.TryParse(obj.ToString(), out attemptedCredit);
                }
            }
            catch (Exception ex)
            {
                attemptedCredit = 0;
            }
            return attemptedCredit;
        }

        public StudentCourseHistory GetById(int? id)
        {
            StudentCourseHistory _studentCourseHistory = null;
            try
            {

                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<StudentCourseHistory> rowMapper = GetMaper();

                var accessor = db.CreateSprocAccessor<StudentCourseHistory>(sqlGetById, rowMapper);
                _studentCourseHistory = accessor.Execute(id).SingleOrDefault();

            }
            catch (Exception ex)
            {
                return _studentCourseHistory;
            }

            return _studentCourseHistory;
        }

        public List<StudentCourseHistory> GetAll()
        {
            List<StudentCourseHistory> studentCourseHistoryList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<StudentCourseHistory> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<StudentCourseHistory>(sqlGetAll, mapper);
                IEnumerable<StudentCourseHistory> collection = accessor.Execute();

                studentCourseHistoryList = collection.ToList();
            }

            catch (Exception ex)
            {
                return studentCourseHistoryList;
            }

            return studentCourseHistoryList;
        }

        public List<StudentCourseHistory> GetAllByAcaCalSectionId(int id)
        {
            List<StudentCourseHistory> studentCourseHistoryList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<StudentCourseHistory> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<StudentCourseHistory>(sqlGetAllByAcaCalSectionId, mapper);
                IEnumerable<StudentCourseHistory> collection = accessor.Execute(id);

                studentCourseHistoryList = collection.ToList();
            }

            catch (Exception ex)
            {
                return studentCourseHistoryList;
            }

            return studentCourseHistoryList;
        }

        public List<StudentCourseHistory> GetAllByAcaCalId(int acaCalId)
        {
            List<StudentCourseHistory> studentCourseHistoryList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<StudentCourseHistory> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<StudentCourseHistory>(sqlGetAllByAcaCalId, mapper);
                IEnumerable<StudentCourseHistory> collection = accessor.Execute(acaCalId);

                studentCourseHistoryList = collection.ToList();
            }

            catch (Exception ex)
            {
                return studentCourseHistoryList;
            }

            return studentCourseHistoryList;
        }

        public List<StudentCourseHistory> GetAllByStudentId(int studentID)
        {
            List<StudentCourseHistory> studentCourseHistoryList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<StudentCourseHistory> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<StudentCourseHistory>(sqlGetAllByStudentId, mapper);
                IEnumerable<StudentCourseHistory> collection = accessor.Execute(studentID);

                studentCourseHistoryList = collection.ToList();
            }

            catch (Exception ex)
            {
                return studentCourseHistoryList;
            }

            return studentCourseHistoryList;
        }

        public List<StudentCourseHistory> GetAllByStudentIdAcaCalId(int studentID, int acaCalId)
        {
            List<StudentCourseHistory> studentCourseHistoryList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<StudentCourseHistory> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<StudentCourseHistory>(sqlGetAllByStudentIdAcaCalId, mapper);
                IEnumerable<StudentCourseHistory> collection = accessor.Execute(studentID, acaCalId);

                studentCourseHistoryList = collection.ToList();
            }

            catch (Exception ex)
            {
                return studentCourseHistoryList;
            }

            return studentCourseHistoryList;
        }

        public List<StudentCourseHistory> GetByStudentIdAcaCalId(int AcaCalId, int StudentId)
        {
            List<StudentCourseHistory> studentCourseHistoryList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<StudentCourseHistory> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<StudentCourseHistory>(sqlGetByStudentIdAcaCalId, mapper);
                IEnumerable<StudentCourseHistory> collection = accessor.Execute(StudentId, AcaCalId);

                studentCourseHistoryList = collection.ToList();
            }

            catch (Exception ex)
            {
                return studentCourseHistoryList;
            }

            return studentCourseHistoryList;
        }

        private Database addParam(Database db, DbCommand cmd, StudentCourseHistory studentCourseHistory, bool isInsert)
        {
            if (isInsert)
            {
                db.AddOutParameter(cmd, "ID", DbType.Int32, Int32.MaxValue);
            }
            else
            {
                db.AddInParameter(cmd, "ID", DbType.Int32, studentCourseHistory.ID);
            }

            db.AddInParameter(cmd, "StudentID", DbType.Int32, studentCourseHistory.StudentID);
            db.AddInParameter(cmd, "CalCourseProgNodeID", DbType.Int32, studentCourseHistory.CalCourseProgNodeID);
            db.AddInParameter(cmd, "AcaCalSectionID", DbType.String, studentCourseHistory.AcaCalSectionID);
            db.AddInParameter(cmd, "RetakeNo", DbType.Int32, studentCourseHistory.RetakeNo);
            db.AddInParameter(cmd, "ObtainedTotalMarks", DbType.Decimal, studentCourseHistory.ObtainedTotalMarks);
            db.AddInParameter(cmd, "ObtainedGPA", DbType.Decimal, studentCourseHistory.ObtainedGPA);
            db.AddInParameter(cmd, "ObtainedGrade", DbType.String, studentCourseHistory.ObtainedGrade);
            db.AddInParameter(cmd, "GradeId", DbType.Int32, studentCourseHistory.GradeId);
            db.AddInParameter(cmd, "CourseStatusID", DbType.Int32, studentCourseHistory.CourseStatusID);
            db.AddInParameter(cmd, "CourseStatusDate", DbType.DateTime, studentCourseHistory.CourseStatusDate);
            db.AddInParameter(cmd, "AcaCalID", DbType.Int32, studentCourseHistory.AcaCalID);
            db.AddInParameter(cmd, "CourseID", DbType.Int32, studentCourseHistory.CourseID);
            db.AddInParameter(cmd, "VersionID", DbType.Int32, studentCourseHistory.VersionID);
            db.AddInParameter(cmd, "CourseCredit", DbType.Decimal, studentCourseHistory.CourseCredit);
            db.AddInParameter(cmd, "CompletedCredit", DbType.Decimal, studentCourseHistory.CompletedCredit);
            db.AddInParameter(cmd, "Node_CourseID", DbType.Int32, studentCourseHistory.Node_CourseID);
            db.AddInParameter(cmd, "NodeID", DbType.Int32, studentCourseHistory.NodeID);
            db.AddInParameter(cmd, "IsMultipleACUSpan", DbType.Boolean, studentCourseHistory.IsMultipleACUSpan);
            db.AddInParameter(cmd, "IsConsiderGPA", DbType.Boolean, studentCourseHistory.IsConsiderGPA);
            db.AddInParameter(cmd, "CourseWavTransfrID ", DbType.Int32, studentCourseHistory.CourseWavTransfrID);
            db.AddInParameter(cmd, "SemesterNo", DbType.Int32, studentCourseHistory.SemesterNo);
            db.AddInParameter(cmd, "YearNo", DbType.Int32, studentCourseHistory.YearNo);

            db.AddInParameter(cmd, "ExamId ", DbType.Int32, studentCourseHistory.ExamId);
            db.AddInParameter(cmd, "YearId", DbType.Int32, studentCourseHistory.YearId);
            db.AddInParameter(cmd, "SemesterId", DbType.Int32, studentCourseHistory.SemesterId);

            db.AddInParameter(cmd, "EqCourseHistoryId", DbType.Int32, studentCourseHistory.EqCourseHistoryId);
            db.AddInParameter(cmd, "Attribute1", DbType.String, studentCourseHistory.Attribute1);
            db.AddInParameter(cmd, "Attribute2", DbType.String, studentCourseHistory.Attribute2);
            db.AddInParameter(cmd, "Attribute3", DbType.String, studentCourseHistory.Attribute3);
            db.AddInParameter(cmd, "CreatedBy", DbType.Int32, studentCourseHistory.CreatedBy);
            db.AddInParameter(cmd, "CreatedDate", DbType.DateTime, studentCourseHistory.CreatedDate);
            db.AddInParameter(cmd, "ModifiedBy", DbType.Int32, studentCourseHistory.ModifiedBy);
            db.AddInParameter(cmd, "ModifiedDate", DbType.DateTime, studentCourseHistory.ModifiedDate);
            db.AddInParameter(cmd, "Remark", DbType.String, studentCourseHistory.Remark);
            db.AddInParameter(cmd, "HeldInRelationId", DbType.Int32, studentCourseHistory.HeldInRelationId);


            return db;
        }

        private IRowMapper<StudentCourseHistory> GetMaper()
        {
            IRowMapper<StudentCourseHistory> mapper = MapBuilder<StudentCourseHistory>.MapAllProperties()
            .Map(m => m.ID).ToColumn("ID")
            .Map(m => m.StudentID).ToColumn("StudentID")
            .Map(m => m.CalCourseProgNodeID).ToColumn("CalCourseProgNodeID")
            .Map(m => m.AcaCalSectionID).ToColumn("AcaCalSectionID")
            .Map(m => m.RetakeNo).ToColumn("RetakeNo")
            .Map(m => m.ObtainedTotalMarks).ToColumn("ObtainedTotalMarks")
            .Map(m => m.ObtainedGPA).ToColumn("ObtainedGPA")
            .Map(m => m.ObtainedGrade).ToColumn("ObtainedGrade")
            .Map(m => m.GradeId).ToColumn("GradeId")
            .Map(m => m.CourseStatusID).ToColumn("CourseStatusID")
            .Map(m => m.CourseStatusDate).ToColumn("CourseStatusDate")
            .Map(m => m.AcaCalID).ToColumn("AcaCalID")
            .Map(m => m.CourseID).ToColumn("CourseID")
            .Map(m => m.VersionID).ToColumn("VersionID")
            .Map(m => m.CourseCredit).ToColumn("CourseCredit")
            .Map(m => m.CompletedCredit).ToColumn("CompletedCredit")
            .Map(m => m.Node_CourseID).ToColumn("Node_CourseID")
            .Map(m => m.NodeID).ToColumn("NodeID")
            .Map(m => m.IsMultipleACUSpan).ToColumn("IsMultipleACUSpan")
            .Map(m => m.IsConsiderGPA).ToColumn("IsConsiderGPA")
            .Map(m => m.CourseWavTransfrID).ToColumn("CourseWavTransfrID")
            .Map(m => m.SemesterNo).ToColumn("SemesterNo")
            .Map(m => m.YearNo).ToColumn("YearNo")

            .Map(m => m.ExamId).ToColumn("ExamId")
            .Map(m => m.YearId).ToColumn("YearId")
            .Map(m => m.SemesterId).ToColumn("SemesterId")

            .Map(m => m.EqCourseHistoryId).ToColumn("EqCourseHistoryId")
            .Map(m => m.Attribute1).ToColumn("Attribute1")
            .Map(m => m.Attribute2).ToColumn("Attribute2")
            .Map(m => m.Attribute3).ToColumn("Attribute3")
            .Map(m => m.CreatedBy).ToColumn("CreatedBy")
            .Map(m => m.CreatedDate).ToColumn("CreatedDate")
            .Map(m => m.ModifiedBy).ToColumn("ModifiedBy")
            .Map(m => m.ModifiedDate).ToColumn("ModifiedDate")
            .Map(m => m.Remark).ToColumn("Remark")
            .Map(m => m.HeldInRelationId).ToColumn("HeldInRelationId")

            .DoNotMap(m => m.FormalCode)
            .DoNotMap(m => m.CourseTitle)
            .DoNotMap(m => m.SessionFullCode)
            .DoNotMap(m => m.StudentRoll)
            .DoNotMap(m => m.StudentName)

            .DoNotMap(m => m.Semester)
            .DoNotMap(m => m.CourseCode)
            .DoNotMap(m => m.CourseName)
                //.DoNotMap(m => m.CourseStatus)
            .DoNotMap(m => m.RegCredit)
            .DoNotMap(m => m.LastCGPA)
            .DoNotMap(m => m.RetakeStatus)

            .Build();

            return mapper;
        }

        public List<StudentCourseHistoryDTO> GetAllByAcaCalIdCourseId(int acaCalId, int CourseId)
        {
            List<StudentCourseHistoryDTO> studentCourseHistoryList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<StudentCourseHistoryDTO> mapper = GetStudentCourseHistoryMaper();

                var accessor = db.CreateSprocAccessor<StudentCourseHistoryDTO>("StudentCourseHistoryDTOGetAllByAcaCalIdCourseId", mapper);
                IEnumerable<StudentCourseHistoryDTO> collection = accessor.Execute(acaCalId, CourseId);

                studentCourseHistoryList = collection.ToList();
            }

            catch (Exception ex)
            {
                return studentCourseHistoryList;
            }

            return studentCourseHistoryList;
        }

        private IRowMapper<StudentCourseHistoryDTO> GetStudentCourseHistoryMaper()
        {
            IRowMapper<StudentCourseHistoryDTO> mapper = MapBuilder<StudentCourseHistoryDTO>.MapAllProperties()
            .Map(m => m.StudentCourseHistoryId).ToColumn("ID")
            .Map(m => m.StudentId).ToColumn("StudentID")
            .Map(m => m.StudentRoll).ToColumn("Roll")
            .Map(m => m.StudentName).ToColumn("FullName")
            .Build();

            return mapper;
        }

        public List<StudentCourseHistory> GetAllRegisteredStudentByProgramSessionBatchCourse(int programId, int sessionId, int batchId, int courseId, int versionId)
        {
            List<StudentCourseHistory> list = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<StudentCourseHistory> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<StudentCourseHistory>("StudentCourseHistoryGetAllRegisteredStudentByProgramSessionBatchCourse", mapper);
                IEnumerable<StudentCourseHistory> collection = accessor.Execute(programId, sessionId, batchId, courseId, versionId);

                list = collection.ToList();
            }

            catch (Exception ex)
            {
                return list;
            }

            return list;
        }

        //public List<StudentAdmitCardInfo> GetAllStudentAdmitCardInfoByProgramBatchSessionRoll(int programId, int sessionId, int batchId, string Roll)
        //{
        //    List<StudentAdmitCardInfo> list = null;

        //    try
        //    {
        //        db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

        //        IRowMapper<StudentAdmitCardInfo> mapper = GetStudentAdmitCardInfoMaper();


        //        var accessor = db.CreateSprocAccessor<StudentAdmitCardInfo>("RptStudentAdmitCardInfoByProgramSessionBatchRoll", mapper);
        //        IEnumerable<StudentAdmitCardInfo> collection = accessor.Execute(programId, sessionId, batchId, Roll);

        //        list = collection.ToList();
        //    }

        //    catch (Exception ex)
        //    {
        //        return list;
        //    }

        //    return list;
        //}

        //public List<StudentAdmitCardInfo> GetAllStudentAdmitCardInfoByProgramBatchSessionRollV2(int programId, int sessionId, int batchId, string Roll)
        //{
        //    List<StudentAdmitCardInfo> list = null;

        //    try
        //    {
        //        db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

        //        IRowMapper<StudentAdmitCardInfo> mapper = GetStudentAdmitCardInfoMaper();

        //        var accessor = db.CreateSprocAccessor<StudentAdmitCardInfo>("RptStudentAdmitCardInfoByProgramSessionBatchRollV2", mapper);
        //        IEnumerable<StudentAdmitCardInfo> collection = accessor.Execute(programId, sessionId, batchId, Roll);

        //        list = collection.ToList();
        //    }

        //    catch (Exception ex)
        //    {
        //        return list;
        //    }

        //    return list;
        //}

        //private IRowMapper<StudentAdmitCardInfo> GetStudentAdmitCardInfoMaper()
        //{
        //    IRowMapper<StudentAdmitCardInfo> mapper = MapBuilder<StudentAdmitCardInfo>.MapAllProperties()
        //        .Map(m => m.ProgramID).ToColumn("ProgramID")
        //        .Map(m => m.Code).ToColumn("Code")
        //        .Map(m => m.ShortName).ToColumn("ShortName")
        //        .Map(m => m.Roll).ToColumn("Roll")
        //        .Map(m => m.LevelTerm).ToColumn("LevelTerm")
        //        .Map(m => m.RegistrationNo).ToColumn("RegistrationNo")
        //        .Map(m => m.AcademicSession).ToColumn("AcademicSession")
        //        .Map(m => m.RegistrationSession).ToColumn("RegistrationSession")
        //        .Map(m => m.FullName).ToColumn("FullName")
        //        .Map(m => m.FatherName).ToColumn("FatherName")
        //        .Map(m => m.MotherName).ToColumn("MotherName")
        //        .Map(m => m.BatchNO).ToColumn("BatchNO")
        //        .Map(m => m.GuardianName).ToColumn("GuardianName")
        //        .Map(m => m.FormalCode).ToColumn("FormalCode")
        //        .Map(m => m.Title).ToColumn("Title")
        //        .Map(m => m.CourseCredit).ToColumn("CourseCredit")
        //        .Map(m => m.IsCollegiate).ToColumn("IsCollegiate")
        //        .Map(m => m.TypeId).ToColumn("TypeId")
        //        .Map(m => m.TypeName).ToColumn("TypeName")
        //        .Map(m => m.Month).ToColumn("Month")
        //        .Map(m => m.Year).ToColumn("Year")
        //        .Map(m => m.LevelCreditName).ToColumn("LevelCreditName")
        //        .Map(m => m.PhotoPath).ToColumn("PhotoPath")
        //        .Map(m => m.OverAllPercentage).ToColumn("OverAllPercentage")
        //        .Map(m => m.NCDCOverAllStatus).ToColumn("NCDCOverAllStatus")


        //    .Build();

        //    return mapper;
        //}

        //public List<rResultCheck> GetStudentResultHisroyForResultCheck(int programId, int sessionId, int batchId, int year)
        //{
        //    List<rResultCheck> list = null;

        //    try
        //    {
        //        db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

        //        IRowMapper<rResultCheck> mapper = MapBuilder<rResultCheck>.MapAllProperties()
        //        .Map(m => m.StudentId).ToColumn("StudentId")
        //        .Map(m => m.StudentName).ToColumn("StudentName")
        //        .Map(m => m.Course).ToColumn("Course")
        //        .Map(m => m.Grade).ToColumn("Grade")
        //        .Map(m => m.AcaCalID).ToColumn("AcaCalID")
        //        .Map(m => m.ExamType).ToColumn("ExamType")
        //        .Map(m => m.ExamName).ToColumn("ExamName")
        //        .Map(m => m.CalenderUnitTypeID).ToColumn("CalenderUnitTypeID")
        //        .Map(m => m.SemesterName).ToColumn("SemesterName")
        //        .Map(m => m.Year).ToColumn("Year")
        //        .Map(m => m.Sequence).ToColumn("Sequence")

        //        .Build();


        //        var accessor = db.CreateSprocAccessor<rResultCheck>("RptStudentResultMigrationCheck", mapper);
        //        IEnumerable<rResultCheck> collection = accessor.Execute(programId, sessionId, batchId, year);

        //        list = collection.ToList();
        //    }

        //    catch (Exception ex)
        //    {
        //        return list;
        //    }

        //    return list;
        //}

        public bool IsDiscollegiateByStudentCourseHistoryId(int CourseHistoryId)
        {
            bool IsDiscollegiate = false;
            int isDiscollegiate = 0;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand("ClassAttendanceStatusByCourseHistoryId");

                db.AddOutParameter(cmd, "IsDiscollegiate", DbType.Int32, Int32.MaxValue);
                db.AddInParameter(cmd, "CourseHistoryId", DbType.Int32, CourseHistoryId);
                db.ExecuteNonQuery(cmd);

                object obj = db.GetParameterValue(cmd, "IsDiscollegiate");

                if (obj != null)
                {
                    int.TryParse(obj.ToString(), out isDiscollegiate);
                }
            }
            catch (Exception ex)
            {
                isDiscollegiate = 0;
            }

            if (isDiscollegiate == 1)
            {
                IsDiscollegiate = true;
            }

            return IsDiscollegiate;
        }

        public bool IsAbsentInExamByStudentCourseHistoryId(int CourseHistoryId)
        {
            bool IsAbsent = false;
            int isAbsent = 0;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand("AbsentStatusInFinalExamsByCourseHistoryId");

                db.AddOutParameter(cmd, "IsAbsent", DbType.Int32, Int32.MaxValue);
                db.AddInParameter(cmd, "CourseHistoryId", DbType.Int32, CourseHistoryId);
                db.ExecuteNonQuery(cmd);

                object obj = db.GetParameterValue(cmd, "IsAbsent");

                if (obj != null)
                {
                    int.TryParse(obj.ToString(), out isAbsent);
                }
            }
            catch (Exception ex)
            {
                isAbsent = 0;
            }

            if (isAbsent == 1)
            {
                IsAbsent = true;
            }

            return IsAbsent;
        }

        public List<StudentCourseHistory> GetAllRegisteredStudentByProgramSessionBatchId(int programId, int sessionId, int batchId)
        {
            List<StudentCourseHistory> list = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<StudentCourseHistory> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<StudentCourseHistory>("StudentCourseHistoryGetAllRegisteredStudentByProgramSessionBatchId", mapper);
                IEnumerable<StudentCourseHistory> collection = accessor.Execute(programId, sessionId, batchId);

                list = collection.ToList();
            }

            catch (Exception ex)
            {
                return list;
            }

            return list;
        }

        //public List<rStudentCourse> GetAllCourseHistoryByCampusProgramSessionBatchCreditGroupDateRegStatus(int campusId, int programId, int sessionId, int batchId, int creditGroup, DateTime fromDate, DateTime toDate, int ResStatus, int dateFilter)
        //{
        //    List<rStudentCourse> list = null;

        //    try
        //    {
        //        db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

        //        IRowMapper<rStudentCourse> mapper = GetStudentCourseMaper();

        //        var accessor = db.CreateSprocAccessor<rStudentCourse>("GetAllCourseHistoryByCampusProgramSessionBatchCreditDateReg", mapper);
        //        IEnumerable<rStudentCourse> collection = accessor.Execute(campusId, programId, sessionId, batchId, creditGroup, fromDate, toDate, ResStatus, dateFilter);

        //        list = collection.ToList();
        //    }

        //    catch (Exception ex)
        //    {
        //        return list;
        //    }

        //    return list;
        //}

        //private IRowMapper<rStudentCourse> GetStudentCourseMaper()
        //{
        //    IRowMapper<rStudentCourse> mapper = MapBuilder<rStudentCourse>.MapAllProperties()
        //    .Map(m => m.Roll).ToColumn("Roll")
        //    .Map(m => m.FullName).ToColumn("FullName")
        //    .Map(m => m.ShortName).ToColumn("ShortName")
        //    .Map(m => m.FormalCode).ToColumn("FormalCode")
        //    .Map(m => m.Title).ToColumn("Title") 
        //    .Map(m => m.Credits).ToColumn("Credits")   
        //    .Map(m => m.BatchNO).ToColumn("BatchNO")
        //    .Map(m => m.Gender).ToColumn("Gender")
        //    .Map(m => m.RegistrationStatus).ToColumn("RegistrationStatus") 


        //    .Build();

        //    return mapper;
        //}

        //public 


        public List<StudentCourseHistoryNewDTO> GetAllCourseHistoryDetailByStudentId(int studentId)
        {
            List<StudentCourseHistoryNewDTO> list = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<StudentCourseHistoryNewDTO> mapper = MapBuilder<StudentCourseHistoryNewDTO>.MapAllProperties()
                    .Map(m => m.CourseHistoryId).ToColumn("CourseHistoryId")
                    .Map(m => m.YearNo).ToColumn("YearNo")
                    .Map(m => m.SemesterNo).ToColumn("SemesterNo")
                    .Map(m => m.ExamYear).ToColumn("ExamYear")
                    .Map(m => m.ExamName).ToColumn("ExamName")
                    .Map(m => m.Session).ToColumn("Session")
                    .Map(m => m.YearName).ToColumn("YearName")
                    .Map(m => m.Semester).ToColumn("Semester")
                    .Map(m => m.CourseCode).ToColumn("CourseCode")
                    .Map(m => m.CourseName).ToColumn("CourseName")
                    .Map(m => m.ObtainedGPA).ToColumn("ObtainedGPA")
                    .Map(m => m.ObtainedGrade).ToColumn("ObtainedGrade")
                    .Map(m => m.CourseCredit).ToColumn("CourseCredit")
                    .Map(m => m.CourseStatusCode).ToColumn("CourseStatusCode")
                    .Map(m => m.CourseStatus).ToColumn("CourseStatus")
                    .Build();

                var accessor = db.CreateSprocAccessor<StudentCourseHistoryNewDTO>("GetCourseHistoryByStudentId", mapper);
                IEnumerable<StudentCourseHistoryNewDTO> collection = accessor.Execute(studentId);

                list = collection.ToList();
            }

            catch (Exception ex)
            {
                return list;
            }

            return list;
        }

        public List<StudentCourseHistory> GetAllByStudentIdYearNoSemesterNoExamId(int studentID, int yearNo, int semesterNo, int examId)
        {
            List<StudentCourseHistory> list = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<StudentCourseHistory> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<StudentCourseHistory>("StudentCourseHistoryGetByStudentIdYearNoSemesterNoExamId", mapper);
                IEnumerable<StudentCourseHistory> collection = accessor.Execute(studentID, yearNo, semesterNo, examId);

                list = collection.ToList();
            }

            catch (Exception ex)
            {
                return list;
            }

            return list;
        }

        public bool DeleteCourseByProgramYearSemesterExamId(int programId, int yearId, int semesterId, int yearNo, int semesterNo, int examId, int courseId, int versionId) 
        {
            bool result = false;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand("StudentCourseHistoryDeleteProgramYearSemesterExamId");

                db.AddInParameter(cmd, "ProgramId", DbType.Int32, programId);
                db.AddInParameter(cmd, "YearId", DbType.Int32, yearId);
                db.AddInParameter(cmd, "SemesterId", DbType.Int32, semesterId);
                db.AddInParameter(cmd, "YearNo", DbType.Int32, yearNo);
                db.AddInParameter(cmd, "SemesterNo", DbType.Int32, semesterNo);
                db.AddInParameter(cmd, "ExamId", DbType.Int32, examId);
                db.AddInParameter(cmd, "CourseId", DbType.Int32, courseId);
                db.AddInParameter(cmd, "VersionId", DbType.Int32, versionId);


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



        public List<StudentCourseHistory> GetAllByExamSetupDetailIdFirstSecondThirdExaminerId(int ExamSetupDetailId, int acaCalSectionId, int? FirstExaminerId, int? SecondExaminerId, int? ThirdExaminerId)
        {
            List<StudentCourseHistory> studentCourseHistoryList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<StudentCourseHistory> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<StudentCourseHistory>("SPGetAllStudentCourseHistoryByExamSetupDetailIdFirstSecondThirdExaminerId", mapper);
                IEnumerable<StudentCourseHistory> collection = accessor.Execute(ExamSetupDetailId, acaCalSectionId, FirstExaminerId, SecondExaminerId, ThirdExaminerId);

                studentCourseHistoryList = collection.ToList();
            }

            catch (Exception ex)
            {
                return studentCourseHistoryList;
            }

            return studentCourseHistoryList;
        }



    }
}
