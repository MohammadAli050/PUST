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
using LogicLayer.BusinessObjects.RO;

namespace LogicLayer.DataLogic.SQLRepository
{
    public partial class SqlExamTemplateRepository : IExamTemplateRepository
    {

        Database db = null;

        private string sqlInsert = "ExamTemplateInsert";
        private string sqlUpdate = "ExamTemplateUpdate";
        private string sqlDelete = "ExamTemplateDeleteById";
        private string sqlGetById = "ExamTemplateGetById";
        private string sqlGetAll = "ExamTemplateGetAll";
        private string sqlGetByExamTemplateName = "ExamTemplateGetByName";
        private string sqlGetBySyllabusDetailIdAcaCalId = "ExamTemplateGetBySyllabusDetailIdAcaCalId";

        public int Insert(ExamTemplate examtemplate)
        {
            int id = 0;
            bool isInsert = true;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand(sqlInsert);

                db = addParam(db, cmd, examtemplate, isInsert);
                db.ExecuteNonQuery(cmd);

                object obj = db.GetParameterValue(cmd, "ExamTemplateId");

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

        public bool Update(ExamTemplate examtemplate)
        {
            bool result = false;
            bool isInsert = false;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand(sqlUpdate);

                db = addParam(db, cmd, examtemplate, isInsert);

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

                db.AddInParameter(cmd, "ExamTemplateId", DbType.Int32, id);
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

        public ExamTemplate GetById(int? id)
        {
            ExamTemplate _examtemplate = null;
            try
            {

                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExamTemplate> rowMapper = GetMaper();

                var accessor = db.CreateSprocAccessor<ExamTemplate>(sqlGetById, rowMapper);
                _examtemplate = accessor.Execute(id).SingleOrDefault();

            }
            catch (Exception ex)
            {
                return _examtemplate;
            }

            return _examtemplate;
        }

        public List<ExamTemplate> GetAll()
        {
            List<ExamTemplate> examtemplateList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExamTemplate> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<ExamTemplate>(sqlGetAll, mapper);
                IEnumerable<ExamTemplate> collection = accessor.Execute();

                examtemplateList = collection.ToList();
            }

            catch (Exception ex)
            {
                return examtemplateList;
            }

            return examtemplateList;
        }

        #region Mapper
        private Database addParam(Database db, DbCommand cmd, ExamTemplate examtemplate, bool isInsert)
        {
            if (isInsert)
            {
                db.AddOutParameter(cmd, "ExamTemplateId", DbType.Int32, Int32.MaxValue);
            }
            else
            {
                db.AddInParameter(cmd, "ExamTemplateId", DbType.Int32, examtemplate.ExamTemplateId);
            }


            db.AddInParameter(cmd, "ExamTemplateName", DbType.String, examtemplate.ExamTemplateName);
            db.AddInParameter(cmd, "ExamTemplateMarks", DbType.Int32, examtemplate.ExamTemplateMarks);
            db.AddInParameter(cmd, "CreatedBy", DbType.Int32, examtemplate.CreatedBy);
            db.AddInParameter(cmd, "CreatedDate", DbType.DateTime, examtemplate.CreatedDate);
            db.AddInParameter(cmd, "ModifiedBy", DbType.Int32, examtemplate.ModifiedBy);
            db.AddInParameter(cmd, "ModifiedDate", DbType.DateTime, examtemplate.ModifiedDate);

            return db;
        }

        private IRowMapper<ExamTemplate> GetMaper()
        {
            IRowMapper<ExamTemplate> mapper = MapBuilder<ExamTemplate>.MapAllProperties()

           .Map(m => m.ExamTemplateId).ToColumn("ExamTemplateId")
        .Map(m => m.ExamTemplateName).ToColumn("ExamTemplateName")
        .Map(m => m.ExamTemplateMarks).ToColumn("ExamTemplateMarks")
        .Map(m => m.CreatedBy).ToColumn("CreatedBy")
        .Map(m => m.CreatedDate).ToColumn("CreatedDate")
        .Map(m => m.ModifiedBy).ToColumn("ModifiedBy")
        .Map(m => m.ModifiedDate).ToColumn("ModifiedDate")

            .Build();

            return mapper;
        }
        #endregion

        public ExamTemplate GetExamTemplateByName(string examTemplateName)
        {
            ExamTemplate _examTemplate = null;
            try
            {

                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExamTemplate> rowMapper = GetMaper();

                var accessor = db.CreateSprocAccessor<ExamTemplate>(sqlGetByExamTemplateName, rowMapper);
                _examTemplate = accessor.Execute(examTemplateName).FirstOrDefault();

            }
            catch (Exception ex)
            {
                return _examTemplate;
            }

            return _examTemplate;
        }

        public ExamTemplate GetBySyllabusDetailIdAcaCalId(int syllabusDetailId, int acaCalId)
        {
            ExamTemplate _examtemplate = null;
            try
            {

                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExamTemplate> rowMapper = GetMaper();

                var accessor = db.CreateSprocAccessor<ExamTemplate>(sqlGetBySyllabusDetailIdAcaCalId, rowMapper);
                _examtemplate = accessor.Execute(syllabusDetailId, acaCalId).FirstOrDefault();

            }
            catch (Exception ex)
            {
                return _examtemplate;
            }

            return _examtemplate;
        }

        public void ProcessFirstSecondThirdExaminerMarkToExamMark(int acacalSectionId)
        {
            bool result = false;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand("ProcessFirstSecondThirdExaminerMarkToExamMark");

                db.AddInParameter(cmd, "AcacalSectionId", DbType.Int32, acacalSectionId);
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

            //return result;
        }



        #region Continuous Mark Report



        public List<rContinuousAssessmentMark> GetContinuousMarkBySectionID(int SectionId)
        {
            List<rContinuousAssessmentMark> list = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<rContinuousAssessmentMark> mapper = GetContinuousAssessmentMarkMapper();

                var accessor = db.CreateSprocAccessor<rContinuousAssessmentMark>("GetContinuousMarkBySectionID", mapper);
                IEnumerable<rContinuousAssessmentMark> collection = accessor.Execute(SectionId);

                list = collection.ToList();
            }

            catch (Exception ex)
            {
                return list;
            }

            return list;
        }

        #region Mapper
        private IRowMapper<rContinuousAssessmentMark> GetContinuousAssessmentMarkMapper()
        {
            IRowMapper<rContinuousAssessmentMark> mapper = MapBuilder<rContinuousAssessmentMark>.MapAllProperties()
           .Map(m => m.StudentCourseHistoryId).ToColumn("StudentCourseHistoryId")
           .Map(m => m.StudentId).ToColumn("StudentId")
           .Map(m => m.AcaCalSectionID).ToColumn("AcaCalSectionID")
           .Map(m => m.Roll).ToColumn("Roll")
           .Map(m => m.ExamTemplateItemId).ToColumn("ExamTemplateItemId")
           .Map(m => m.ColumnSequence).ToColumn("ColumnSequence")
           .Map(m => m.ColumnName).ToColumn("ColumnName")
           .Map(m => m.GradePoint).ToColumn("GradePoint")
           .Map(m => m.Grade).ToColumn("Grade")
           .Map(m => m.TemplateGroupName).ToColumn("TemplateGroupName")
           .Map(m => m.Marks).ToColumn("Marks")
           .Map(m => m.ExamStatus).ToColumn("ExamStatus")
           .Map(m => m.ColumnCount).ToColumn("ColumnCount")
           .Map(m => m.Serial).ToColumn("Serial")


           .Build();

            return mapper;
        }
        #endregion
        #endregion
    }
}

