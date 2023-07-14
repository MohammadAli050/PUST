using LogicLayer.BusinessObjects;
using LogicLayer.DataLogic.IRepository;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.DataLogic.SQLRepository
{
    public partial class SqlExamMarkQuestionRepository : IExamMarkQuestionRepository
    {

        Database db = null;

        private string sqlInsert = "ExamMarkQuestionInsert";
        private string sqlUpdate = "ExamMarkQuestionUpdate";
        private string sqlDelete = "ExamMarkQuestionDeleteById";
        private string sqlGetById = "ExamMarkQuestionGetById";
        private string sqlGetAll = "ExamMarkQuestionGetAll";

        public int Insert(ExamMarkQuestion exammarkquestion)
        {
            int id = 0;
            bool isInsert = true;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand(sqlInsert);

                db = addParam(db, cmd, exammarkquestion, isInsert);
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

        public bool Update(ExamMarkQuestion exammarkquestion)
        {
            bool result = false;
            bool isInsert = false;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand(sqlUpdate);

                db = addParam(db, cmd, exammarkquestion, isInsert);

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

        public ExamMarkQuestion GetById(int? id)
        {
            ExamMarkQuestion _exammarkquestion = null;
            try
            {

                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExamMarkQuestion> rowMapper = GetMaper();

                var accessor = db.CreateSprocAccessor<ExamMarkQuestion>(sqlGetById, rowMapper);
                _exammarkquestion = accessor.Execute(id).SingleOrDefault();

            }
            catch (Exception ex)
            {
                return _exammarkquestion;
            }

            return _exammarkquestion;
        }

        public List<ExamMarkQuestion> GetAll()
        {
            List<ExamMarkQuestion> exammarkquestionList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExamMarkQuestion> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<ExamMarkQuestion>(sqlGetAll, mapper);
                IEnumerable<ExamMarkQuestion> collection = accessor.Execute();

                exammarkquestionList = collection.ToList();
            }

            catch (Exception ex)
            {
                return exammarkquestionList;
            }

            return exammarkquestionList;
        }

        public ExamMarkQuestion GetByStudentIdCourseHistoryId(int studentId, int courseHistoryId, int questionNo)
        {
            ExamMarkQuestion _exammarkquestion = null;
            try
            {

                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExamMarkQuestion> rowMapper = GetMaper();

                var accessor = db.CreateSprocAccessor<ExamMarkQuestion>("ExamMarkQuestionGetByStudentIdCourseHistoryId", rowMapper);
                _exammarkquestion = accessor.Execute(studentId, courseHistoryId, questionNo).SingleOrDefault();

            }
            catch (Exception ex)
            {
                return _exammarkquestion;
            }

            return _exammarkquestion;
        }


        #region Mapper
        private Database addParam(Database db, DbCommand cmd, ExamMarkQuestion exammarkquestion, bool isInsert)
        {
            if (isInsert)
            {
                db.AddOutParameter(cmd, "ID", DbType.Int32, Int32.MaxValue);
            }
            else
            {
                db.AddInParameter(cmd, "ID", DbType.Int32, exammarkquestion.ID);
            }


            db.AddInParameter(cmd, "StudentId", DbType.Int32, exammarkquestion.StudentId);
            db.AddInParameter(cmd, "CourseHistoryId", DbType.Int32, exammarkquestion.CourseHistoryId);
            db.AddInParameter(cmd, "QuestionNo", DbType.Int32, exammarkquestion.QuestionNo);
            db.AddInParameter(cmd, "FirstExaminerMark", DbType.Decimal, exammarkquestion.FirstExaminerMark);
            db.AddInParameter(cmd, "SecondExaminerMark", DbType.Decimal, exammarkquestion.SecondExaminerMark);
            db.AddInParameter(cmd, "ThirdExaminerMark", DbType.Decimal, exammarkquestion.ThirdExaminerMark);
            db.AddInParameter(cmd, "Remarks", DbType.String, exammarkquestion.Remarks);
            db.AddInParameter(cmd, "Attribute1", DbType.String, exammarkquestion.Attribute1);
            db.AddInParameter(cmd, "Attribute2", DbType.String, exammarkquestion.Attribute2);
            db.AddInParameter(cmd, "Attribute3", DbType.String, exammarkquestion.Attribute3);
            db.AddInParameter(cmd, "CreateBy", DbType.Int32, exammarkquestion.CreateBy);
            db.AddInParameter(cmd, "CreatedDate", DbType.DateTime, exammarkquestion.CreatedDate);
            db.AddInParameter(cmd, "ModifiedBy", DbType.Int32, exammarkquestion.ModifiedBy);
            db.AddInParameter(cmd, "ModifiedDate", DbType.DateTime, exammarkquestion.ModifiedDate);
            db.AddInParameter(cmd, "ExamTemplateItemId", DbType.Int32, exammarkquestion.ExamTemplateItemId);

            return db;
        }

        private IRowMapper<ExamMarkQuestion> GetMaper()
        {
            IRowMapper<ExamMarkQuestion> mapper = MapBuilder<ExamMarkQuestion>.MapAllProperties()

           .Map(m => m.ID).ToColumn("ID")
        .Map(m => m.StudentId).ToColumn("StudentId")
        .Map(m => m.CourseHistoryId).ToColumn("CourseHistoryId")
        .Map(m => m.QuestionNo).ToColumn("QuestionNo")
        .Map(m => m.FirstExaminerMark).ToColumn("FirstExaminerMark")
        .Map(m => m.SecondExaminerMark).ToColumn("SecondExaminerMark")
        .Map(m => m.ThirdExaminerMark).ToColumn("ThirdExaminerMark")
        .Map(m => m.Remarks).ToColumn("Remarks")
        .Map(m => m.Attribute1).ToColumn("Attribute1")
        .Map(m => m.Attribute2).ToColumn("Attribute2")
        .Map(m => m.Attribute3).ToColumn("Attribute3")
        .Map(m => m.CreateBy).ToColumn("CreateBy")
        .Map(m => m.CreatedDate).ToColumn("CreatedDate")
        .Map(m => m.ModifiedBy).ToColumn("ModifiedBy")
        .Map(m => m.ModifiedDate).ToColumn("ModifiedDate")
        .Map(m => m.ExamTemplateItemId).ToColumn("ExamTemplateItemId")

            .Build();

            return mapper;
        }
        #endregion

    }
}
