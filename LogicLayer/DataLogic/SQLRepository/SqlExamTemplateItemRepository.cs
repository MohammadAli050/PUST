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
    public partial class SqlExamTemplateItemRepository : IExamTemplateItemRepository
    {

        Database db = null;

        private string sqlInsert = "ExamTemplateItemInsert";
        private string sqlUpdate = "ExamTemplateItemUpdate";
        private string sqlDelete = "ExamTemplateItemDeleteById";
        private string sqlGetById = "ExamTemplateItemGetById";
        private string sqlGetAll = "ExamTemplateItemGetAll";

        private string sqlGetByExamNameColumnSequence = "ExamTemplateItemGetByExamNameColumnSequence";
        private string sqlGetByExamTemplateId = "ExamTemplateItemByExamTemplateId";
        private string sqlBasicTemplateItemByExamTemplateId = "ExamTemplateBasicItemByExamTemplateId";

        public int Insert(ExamTemplateItem examtemplateitem)
        {
            int id = 0;
            bool isInsert = true;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand(sqlInsert);

                db = addParam(db, cmd, examtemplateitem, isInsert);
                db.ExecuteNonQuery(cmd);

                object obj = db.GetParameterValue(cmd, "ExamTemplateItemId");

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

        public bool Update(ExamTemplateItem examtemplateitem)
        {
            bool result = false;
            bool isInsert = false;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand(sqlUpdate);

                db = addParam(db, cmd, examtemplateitem, isInsert);

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

                db.AddInParameter(cmd, "ExamTemplateItemId", DbType.Int32, id);
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

        public ExamTemplateItem GetById(int? id)
        {
            ExamTemplateItem _examtemplateitem = null;
            try
            {

                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExamTemplateItem> rowMapper = GetMaper();

                var accessor = db.CreateSprocAccessor<ExamTemplateItem>(sqlGetById, rowMapper);
                _examtemplateitem = accessor.Execute(id).SingleOrDefault();

            }
            catch (Exception ex)
            {
                return _examtemplateitem;
            }

            return _examtemplateitem;
        }

        public List<ExamTemplateItem> GetAll()
        {
            List<ExamTemplateItem> examtemplateitemList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExamTemplateItem> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<ExamTemplateItem>(sqlGetAll, mapper);
                IEnumerable<ExamTemplateItem> collection = accessor.Execute();

                examtemplateitemList = collection.ToList();
            }

            catch (Exception ex)
            {
                return examtemplateitemList;
            }

            return examtemplateitemList;
        }

        #region Mapper
        private Database addParam(Database db, DbCommand cmd, ExamTemplateItem examtemplateitem, bool isInsert)
        {
            if (isInsert)
            {
                db.AddOutParameter(cmd, "ExamTemplateItemId", DbType.Int32, Int32.MaxValue);
            }
            else
            {
                db.AddInParameter(cmd, "ExamTemplateItemId", DbType.Int32, examtemplateitem.ExamTemplateItemId);
            }

            db.AddInParameter(cmd, "ExamTemplateId", DbType.Int32, examtemplateitem.ExamTemplateId);
            db.AddInParameter(cmd, "ExamName", DbType.String, examtemplateitem.ExamName);
            db.AddInParameter(cmd, "ExamMark", DbType.Decimal, examtemplateitem.ExamMark);
            db.AddInParameter(cmd, "PassMark", DbType.Decimal, examtemplateitem.PassMark);
            db.AddInParameter(cmd, "ColumnSequence", DbType.Int32, examtemplateitem.ColumnSequence);
            db.AddInParameter(cmd, "PrintColumnSequence", DbType.Int32, examtemplateitem.PrintColumnSequence);
            db.AddInParameter(cmd, "ColumnType", DbType.Int32, examtemplateitem.ColumnType);
            db.AddInParameter(cmd, "CalculationType", DbType.Int32, examtemplateitem.CalculationType);
            db.AddInParameter(cmd, "DivideBy", DbType.Decimal, examtemplateitem.DivideBy);
            db.AddInParameter(cmd, "MultiplyBy", DbType.Decimal, examtemplateitem.MultiplyBy);
            db.AddInParameter(cmd, "ShowInTabulation", DbType.Boolean, examtemplateitem.ShowInTabulation);
            db.AddInParameter(cmd, "TabulationTitle", DbType.String, examtemplateitem.TabulationTitle);
            db.AddInParameter(cmd, "Attribute1", DbType.String, examtemplateitem.Attribute1);
            db.AddInParameter(cmd, "Attribute2", DbType.String, examtemplateitem.Attribute2);
            db.AddInParameter(cmd, "Attribute3", DbType.String, examtemplateitem.Attribute3);
            db.AddInParameter(cmd, "CreatedBy", DbType.Int32, examtemplateitem.CreatedBy);
            db.AddInParameter(cmd, "CreatedDate", DbType.DateTime, examtemplateitem.CreatedDate);
            db.AddInParameter(cmd, "ModifiedBy", DbType.Int32, examtemplateitem.ModifiedBy);
            db.AddInParameter(cmd, "ModifiedDate", DbType.DateTime, examtemplateitem.ModifiedDate);
            db.AddInParameter(cmd, "IsFinalExam", DbType.Boolean, examtemplateitem.IsFinalExam);
            db.AddInParameter(cmd, "MultipleExaminer", DbType.Int32, examtemplateitem.MultipleExaminer);



            db.AddInParameter(cmd, "SingleQuestionAnswer", DbType.Boolean, examtemplateitem.SingleQuestionAnswer);
            db.AddInParameter(cmd, "ShowAllContinuousInSubTotal", DbType.Boolean, examtemplateitem.ShowAllContinuousInSubTotal);
            db.AddInParameter(cmd, "ShowAllInGrandTotal", DbType.Boolean, examtemplateitem.ShowAllInGrandTotal);
            db.AddInParameter(cmd, "NumberOfExaminer", DbType.Int32, examtemplateitem.NumberOfExaminer);

            return db;
        }

        private IRowMapper<ExamTemplateItem> GetMaper()
        {
            IRowMapper<ExamTemplateItem> mapper = MapBuilder<ExamTemplateItem>.MapAllProperties()

            .Map(m => m.ExamTemplateItemId).ToColumn("ExamTemplateItemId")
            .Map(m => m.ExamTemplateId).ToColumn("ExamTemplateId")
            .Map(m => m.ExamName).ToColumn("ExamName")
            .Map(m => m.ExamMark).ToColumn("ExamMark")
            .Map(m => m.PassMark).ToColumn("PassMark")
            .Map(m => m.ColumnSequence).ToColumn("ColumnSequence")
            .Map(m => m.PrintColumnSequence).ToColumn("PrintColumnSequence")
            .Map(m => m.ColumnType).ToColumn("ColumnType")
            .Map(m => m.CalculationType).ToColumn("CalculationType")
            .Map(m => m.DivideBy).ToColumn("DivideBy")
            .Map(m => m.MultiplyBy).ToColumn("MultiplyBy")
            .Map(m => m.ShowInTabulation).ToColumn("ShowInTabulation")
            .Map(m => m.TabulationTitle).ToColumn("TabulationTitle")
            .Map(m => m.Attribute1).ToColumn("Attribute1")
            .Map(m => m.Attribute2).ToColumn("Attribute2")
            .Map(m => m.Attribute3).ToColumn("Attribute3")
            .Map(m => m.CreatedBy).ToColumn("CreatedBy")
            .Map(m => m.CreatedDate).ToColumn("CreatedDate")
            .Map(m => m.ModifiedBy).ToColumn("ModifiedBy")
            .Map(m => m.ModifiedDate).ToColumn("ModifiedDate")
            .Map(m => m.IsFinalExam).ToColumn("IsFinalExam")
            .Map(m => m.MultipleExaminer).ToColumn("MultipleExaminer")

            .Map(m => m.SingleQuestionAnswer).ToColumn("SingleQuestionAnswer")
            .Map(m => m.ShowAllContinuousInSubTotal).ToColumn("ShowAllContinuousInSubTotal")
            .Map(m => m.ShowAllInGrandTotal).ToColumn("ShowAllInGrandTotal")
            .Map(m => m.NumberOfExaminer).ToColumn("NumberOfExaminer")
            .Build();

            return mapper;
        }
        #endregion

        public ExamTemplateItem GetByExamNameColumnSequence(int examTemplateId, decimal examMark, int columnSequence)
        {
            ExamTemplateItem _examtemplateitem = null;
            try
            {

                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExamTemplateItem> rowMapper = GetMaper();

                var accessor = db.CreateSprocAccessor<ExamTemplateItem>(sqlGetByExamNameColumnSequence, rowMapper);
                _examtemplateitem = accessor.Execute(examTemplateId, examMark, columnSequence).FirstOrDefault();

            }
            catch (Exception ex)
            {
                return _examtemplateitem;
            }

            return _examtemplateitem;
        }

        public List<ExamTemplateItem> GetByExamTemplateId(int examTemplateId)
        {
            List<ExamTemplateItem> examtemplateitemList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExamTemplateItem> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<ExamTemplateItem>(sqlGetByExamTemplateId, mapper);
                IEnumerable<ExamTemplateItem> collection = accessor.Execute(examTemplateId).ToList();

                examtemplateitemList = collection.ToList();
            }

            catch (Exception ex)
            {
                return examtemplateitemList;
            }

            return examtemplateitemList;
        }

        public List<ExamTemplateItem> GetBasicWithOutFinalTemplateItemByExamTemplateId(int examTemplateId)
        {
            List<ExamTemplateItem> examtemplateitemList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExamTemplateItem> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<ExamTemplateItem>(sqlBasicTemplateItemByExamTemplateId, mapper);
                IEnumerable<ExamTemplateItem> collection = accessor.Execute(examTemplateId).ToList();

                examtemplateitemList = collection.ToList();
            }

            catch (Exception ex)
            {
                return examtemplateitemList;
            }

            return examtemplateitemList;
        }
    }
}

