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
    public partial class SqlExamSetupDetailRepository : IExamSetupDetailRepository
    {

        Database db = null;

        private string sqlInsert = "ExamSetupDetailInsert";
        private string sqlUpdate = "ExamSetupDetailUpdate";
        private string sqlDelete = "ExamSetupDetailDeleteById";
        private string sqlGetById = "ExamSetupDetailGetById";
        private string sqlGetAll = "ExamSetupDetailGetAll";

        public int Insert(ExamSetupDetail examsetupdetail)
        {
            int id = 0;
            bool isInsert = true;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand(sqlInsert);

                db = addParam(db, cmd, examsetupdetail, isInsert);
                db.ExecuteNonQuery(cmd);

                object obj = db.GetParameterValue(cmd, "ExamSetupDetailId");

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

        public bool Update(ExamSetupDetail examsetupdetail)
        {
            bool result = false;
            bool isInsert = false;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand(sqlUpdate);

                db = addParam(db, cmd, examsetupdetail, isInsert);

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

                db.AddInParameter(cmd, "ExamSetupDetailId", DbType.Int32, id);
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

        public ExamSetupDetail GetById(int? id)
        {
            ExamSetupDetail _examsetupdetail = null;
            try
            {

                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExamSetupDetail> rowMapper = GetMaper();

                var accessor = db.CreateSprocAccessor<ExamSetupDetail>(sqlGetById, rowMapper);
                _examsetupdetail = accessor.Execute(id).SingleOrDefault();

            }
            catch (Exception ex)
            {
                return _examsetupdetail;
            }

            return _examsetupdetail;
        }

        public List<ExamSetupDetail> GetAll()
        {
            List<ExamSetupDetail> examsetupdetailList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExamSetupDetail> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<ExamSetupDetail>(sqlGetAll, mapper);
                IEnumerable<ExamSetupDetail> collection = accessor.Execute();

                examsetupdetailList = collection.ToList();
            }

            catch (Exception ex)
            {
                return examsetupdetailList;
            }

            return examsetupdetailList;
        }




        public ExamSetupDetail GetByExamSetupIdSemesterNo(int examSetupId, int semesterNo)
        {
            ExamSetupDetail _examsetupdetail = null;
            try
            {

                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExamSetupDetail> rowMapper = GetMaper();

                var accessor = db.CreateSprocAccessor<ExamSetupDetail>("ExamSetupDetailGetByExamSetupIdSemesterNo", rowMapper);
                _examsetupdetail = accessor.Execute(examSetupId, semesterNo).SingleOrDefault();

            }
            catch (Exception ex)
            {
                return _examsetupdetail;
            }

            return _examsetupdetail;
        }

        public List<ExamSetupDetail> GetAllByExamSetupId(int examSetupId)
        {
            List<ExamSetupDetail> examsetupdetailList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExamSetupDetail> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<ExamSetupDetail>("ExamSetupDetailGetAllByExamSetupId", mapper);
                IEnumerable<ExamSetupDetail> collection = accessor.Execute(examSetupId);

                examsetupdetailList = collection.ToList();
            }

            catch (Exception ex)
            {
                return examsetupdetailList;
            }

            return examsetupdetailList;
        }



        #region Mapper
        private Database addParam(Database db, DbCommand cmd, ExamSetupDetail examsetupdetail, bool isInsert)
        {
            if (isInsert)
            {
                db.AddOutParameter(cmd, "ExamSetupDetailId", DbType.Int32, Int32.MaxValue);
            }
            else
            {
                db.AddInParameter(cmd, "ExamSetupDetailId", DbType.Int32, examsetupdetail.ExamSetupDetailId);
            }


            db.AddInParameter(cmd, "ExamSetupId", DbType.Int32, examsetupdetail.ExamSetupId);
            db.AddInParameter(cmd, "SemesterNo", DbType.Int32, examsetupdetail.SemesterNo);
            db.AddInParameter(cmd, "ExamName", DbType.String, examsetupdetail.ExamName);
            db.AddInParameter(cmd, "ExamShortName", DbType.String, examsetupdetail.ExamShortName);
            db.AddInParameter(cmd, "ExamStartDate", DbType.DateTime, examsetupdetail.ExamStartDate);
            db.AddInParameter(cmd, "ExamEndDate", DbType.DateTime, examsetupdetail.ExamEndDate);
            db.AddInParameter(cmd, "LastDateOfResultSubmission", DbType.DateTime, examsetupdetail.LastDateOfResultSubmission);
            db.AddInParameter(cmd, "ResultPublishDate", DbType.DateTime, examsetupdetail.ResultPublishDate);
            db.AddInParameter(cmd, "Remarks", DbType.String, examsetupdetail.Remarks);
            db.AddInParameter(cmd, "IsActive", DbType.Boolean, examsetupdetail.IsActive);
            db.AddInParameter(cmd, "Attribute1", DbType.String, examsetupdetail.Attribute1);
            db.AddInParameter(cmd, "Attribute2", DbType.String, examsetupdetail.Attribute2);
            db.AddInParameter(cmd, "Attribute3", DbType.String, examsetupdetail.Attribute3);
            db.AddInParameter(cmd, "CreatedBy", DbType.Int32, examsetupdetail.CreatedBy);
            db.AddInParameter(cmd, "CreatedDate", DbType.DateTime, examsetupdetail.CreatedDate);
            db.AddInParameter(cmd, "ModifiedBy", DbType.Int32, examsetupdetail.ModifiedBy);
            db.AddInParameter(cmd, "ModifiedDate", DbType.DateTime, examsetupdetail.ModifiedDate);

            return db;
        }

        private IRowMapper<ExamSetupDetail> GetMaper()
        {
            IRowMapper<ExamSetupDetail> mapper = MapBuilder<ExamSetupDetail>.MapAllProperties()

           .Map(m => m.ExamSetupDetailId).ToColumn("ExamSetupDetailId")
        .Map(m => m.ExamSetupId).ToColumn("ExamSetupId")
        .Map(m => m.SemesterNo).ToColumn("SemesterNo")
        .Map(m => m.ExamName).ToColumn("ExamName")
        .Map(m => m.ExamShortName).ToColumn("ExamShortName")
        .Map(m => m.ExamStartDate).ToColumn("ExamStartDate")
        .Map(m => m.ExamEndDate).ToColumn("ExamEndDate")
        .Map(m => m.LastDateOfResultSubmission).ToColumn("LastDateOfResultSubmission")
        .Map(m => m.ResultPublishDate).ToColumn("ResultPublishDate")
        .Map(m => m.Remarks).ToColumn("Remarks")
        .Map(m => m.IsActive).ToColumn("IsActive")
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

    }
}
