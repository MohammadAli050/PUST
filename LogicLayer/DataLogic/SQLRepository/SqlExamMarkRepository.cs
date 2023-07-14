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

namespace LogicLayer.DataLogic.SQLRepository
{
    public partial class SqlExamMarkRepository : IExamMarkRepository
    {

        Database db;

        private string sqlInsert = "ExamMarkInsert";
        private string sqlUpdate = "ExamMarkUpdate";
        private string sqlDelete = "ExamMarkDeleteById";
        private string sqlGetById = "ExamMarkGetById";
        private string sqlGetAll = "ExamMarkGetAll";

        private string sqlGetByExamMarkDtoByParameter = "ExamMarkDtoByParameter";

        private string sqlGetByStudentExamMarkColmneWiseProcess = "StudentExamMarkColmneWiseProcess";

        public int Insert(ExamMark exammark)
        {
            int id = 0;
            bool isInsert = true;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand(sqlInsert);

                db = addParam(db, cmd, exammark, isInsert);
                db.ExecuteNonQuery(cmd);

                object obj = db.GetParameterValue(cmd, "Id");

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

        public bool Update(ExamMark exammark)
        {
            bool result = false;
            bool isInsert = false;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand(sqlUpdate);

                db = addParam(db, cmd, exammark, isInsert);

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

                db.AddInParameter(cmd, "Id", DbType.Int32, id);
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

        public ExamMark GetById(int? id)
        {
            ExamMark _exammark = null;
            try
            {

                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExamMark> rowMapper = GetMaper();

                var accessor = db.CreateSprocAccessor<ExamMark>(sqlGetById, rowMapper);
                _exammark = accessor.Execute(id).SingleOrDefault();

            }
            catch (Exception ex)
            {
                return _exammark;
            }

            return _exammark;
        }

        public List<ExamMark> GetAll()
        {
            List<ExamMark> exammarkList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExamMark> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<ExamMark>(sqlGetAll, mapper);
                IEnumerable<ExamMark> collection = accessor.Execute();

                exammarkList = collection.ToList();
            }

            catch (Exception ex)
            {
                return exammarkList;
            }

            return exammarkList;
        }

        #region Mapper
        private Database addParam(Database db, DbCommand cmd, ExamMark exammark, bool isInsert)
        {
            if (isInsert)
            {
                db.AddOutParameter(cmd, "Id", DbType.Int32, Int32.MaxValue);
            }
            else
            {
                db.AddInParameter(cmd, "Id", DbType.Int32, exammark.Id);
            }

            db.AddInParameter(cmd, "CourseHistoryId", DbType.Int32, exammark.CourseHistoryId);
            db.AddInParameter(cmd, "ExamTemplateItemId", DbType.Int32, exammark.ExamTemplateItemId);
            db.AddInParameter(cmd, "Mark", DbType.Decimal, exammark.Mark);
            db.AddInParameter(cmd, "Attribute1", DbType.String, exammark.Attribute1);
            db.AddInParameter(cmd, "Attribute2", DbType.String, exammark.Attribute2);
            db.AddInParameter(cmd, "Attribute3", DbType.String, exammark.Attribute3);
            db.AddInParameter(cmd, "CreateBy", DbType.Int32, exammark.CreateBy);
            db.AddInParameter(cmd, "CreatedDate", DbType.DateTime, exammark.CreatedDate);
            db.AddInParameter(cmd, "ModifiedBy", DbType.Int32, exammark.ModifiedBy);
            db.AddInParameter(cmd, "ModifiedDate", DbType.DateTime, exammark.ModifiedDate);

            return db;
        }

        private IRowMapper<ExamMark> GetMaper()
        {
            IRowMapper<ExamMark> mapper = MapBuilder<ExamMark>.MapAllProperties()

            .Map(m => m.Id).ToColumn("Id")
            .Map(m => m.CourseHistoryId).ToColumn("CourseHistoryId")
            .Map(m => m.ExamTemplateItemId).ToColumn("ExamTemplateItemId")
            .Map(m => m.Mark).ToColumn("Mark")
            .Map(m => m.Attribute1).ToColumn("Attribute1")
            .Map(m => m.Attribute2).ToColumn("Attribute2")
            .Map(m => m.Attribute3).ToColumn("Attribute3")
            .Map(m => m.CreateBy).ToColumn("CreateBy")
            .Map(m => m.CreatedDate).ToColumn("CreatedDate")
            .Map(m => m.ModifiedBy).ToColumn("ModifiedBy")
            .Map(m => m.ModifiedDate).ToColumn("ModifiedDate")

            .Build();

            return mapper;
        }
        #endregion

        

        public ExamMark GetStudentMarkByExamId(int courseHistoryId, int examId)
        {
            ExamMark _studentresult = null;
            try
            {

                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExamMark> rowMapper = GetMaper();

                var accessor = db.CreateSprocAccessor<ExamMark>("ExamMarkByCourseHistoryIdExamId", rowMapper);
                _studentresult = accessor.Execute(courseHistoryId, examId).FirstOrDefault();

            }
            catch (Exception ex)
            {
                return _studentresult;
            }

            return _studentresult;
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
            .Map(m => m.AcaCalId).ToColumn("AcaCalId")
            .Map(m => m.SyllabusDetailId).ToColumn("AcaCalSectionID")
            .Map(m => m.Roll).ToColumn("Roll")
            .Map(m => m.ExamTemplateItemId).ToColumn("ExamTemplateItemId")
            .Map(m => m.ColumnSequence).ToColumn("ColumnSequence")
            .Map(m => m.ColumnName).ToColumn("ColumnName")
            .Map(m => m.GradePoint).ToColumn("GradePoint")
            .Map(m => m.Grade).ToColumn("Grade")
            .Map(m => m.Mark).ToColumn("Mark")
            .Map(m => m.Attribute1).ToColumn("Attribute1")

            .Build();

            return mapper;
        }

        public int InsertStudentCourseQuestionMarksMaster(StudentsCourseMarks aStudentCourseMarks)
        {
            int id = 0;
            string query = "InsertExamQuestionMarksMaster";
            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand(query);
                //db = addParam(db, cmd, aStudentCourseMarks, isInsert);
                db.AddOutParameter(cmd, "Id", DbType.Int32, Int32.MaxValue);
                db.AddInParameter(cmd, "CourseHistoryId", DbType.Int32, aStudentCourseMarks.StudentCourseHistoryId);
                db.AddInParameter(cmd, "ExamTemplateItemId", DbType.Int32, aStudentCourseMarks.ExamTemplateTypeId);
                db.AddInParameter(cmd, "CreatedBy", DbType.Int32, aStudentCourseMarks.CreateBy);
                db.AddInParameter(cmd, "CreatedDate", DbType.DateTime, aStudentCourseMarks.CreatedDate);
                db.AddInParameter(cmd, "ModifiedBy", DbType.Int32, aStudentCourseMarks.ModifiedBy);
                db.AddInParameter(cmd, "ModifiedDate", DbType.DateTime, aStudentCourseMarks.ModifiedDate);

                db.ExecuteNonQuery(cmd);

                object obj = db.GetParameterValue(cmd, "Id");

                if (obj != null)
                {
                    int.TryParse(obj.ToString(), out id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return id;
        }

        public bool InsertStudentCourseQuestionMarksDetail(Marks questionMarks)
        {
            int rowAffected;
            string query = "InsertExamQuestionMarksDetail";
            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand(query);
                db.AddInParameter(cmd, "CourseMasterId", DbType.Decimal, (int)questionMarks.Id);
                db.AddInParameter(cmd, "QuestionNumber", DbType.Int32, questionMarks.QuestionNumber);
                db.AddInParameter(cmd, "ObtainedMarks", DbType.Int32, questionMarks.ObtainedMarks);
                rowAffected = db.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                throw;
            }
            if (rowAffected > 0)
                return true;
            return false;
        }

        public bool DeleteStudentCourseQuestionMarksMasterDetail(StudentsCourseMarks aStudentCourseMarks)
        {
            bool result = false;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand commandDetail = db.GetStoredProcCommand("DeleteStudentQuestionMarksDetailByCourseHistoryIdAndExamTypeItemId");

                db.AddInParameter(commandDetail, "StudentCourseHistoryId", DbType.Int32, aStudentCourseMarks.StudentCourseHistoryId);
                db.AddInParameter(commandDetail, "ExamTypeItemId", DbType.Int32, aStudentCourseMarks.ExamTemplateTypeId);
                int rowsAffectedDetail = db.ExecuteNonQuery(commandDetail);

                DbCommand commandMaster = db.GetStoredProcCommand("DeleteStudentQuestionMarksMasterByCourseHistoryIdAndExamTypeItemId");
                db.AddInParameter(commandMaster, "StudentCourseHistoryId", DbType.Int32, aStudentCourseMarks.StudentCourseHistoryId);
                db.AddInParameter(commandMaster, "ExamTypeItemId", DbType.Int32, aStudentCourseMarks.ExamTemplateTypeId);
                int rowsAffectedMaster = db.ExecuteNonQuery(commandMaster);
                //if (rowsAffected > 0)
                //{
                //    result = true;
                //}
            }
            catch (Exception ex)
            {
                throw;
            }

            return result;
        }

        public List<Marks> GetAllQuestionMarksByCourseHistoryIdAndExamTypeItemId(int studentCourseHistoryId,
            int examTypeItemId)
        {
            StudentsCourseMarks studentCourseMarks = null;
            List<Marks> studentMarks = null;
            try
            {

                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                IRowMapper<Marks> studentMarksRowMapper = GetQuestionMarksMaper();
                var accessor = db.CreateSprocAccessor("GetStudentQuestionMarksByCourseHistoryIdAndExamTypeItemId", studentMarksRowMapper);
                IEnumerable<Marks> collectionOfMarks = accessor.Execute(studentCourseHistoryId, examTypeItemId);
                studentMarks = collectionOfMarks.ToList();
                //studentCourseMarks.StudentMarks = studentMarks;
            }
            catch (Exception ex)
            {
                return studentMarks;
            }

            return studentMarks;
        }

        private IRowMapper<Marks> GetQuestionMarksMaper()
        {
            IRowMapper<Marks> mapper = MapBuilder<Marks>.MapAllProperties()
            .Map(m => m.Id).ToColumn("MarksDetailID")
            .Map(m => m.QuestionNumber).ToColumn("QuestionNumber")
            .Map(m => m.ObtainedMarks).ToColumn("ObtainedMarks")
            .Build();

            return mapper;
        }


        public ExamMark GetByCourseHistoryId(int courseHistoryId)
        {
            ExamMark _exammark = null;
            try
            {

                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExamMark> rowMapper = GetMaper();

                var accessor = db.CreateSprocAccessor<ExamMark>("ExamMarkGetByCourseHistoryId", rowMapper);
                _exammark = accessor.Execute(courseHistoryId).SingleOrDefault();

            }
            catch (Exception ex)
            {
                return _exammark;
            }

            return _exammark;
        }

    }
}

