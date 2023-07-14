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
    public partial class SqlExamMarkSubmissionDateRepository : IExamMarkSubmissionDateRepository
    {

        Database db = null;

        private string sqlInsert = "ExamMarkSubmissionDateInsert";
        private string sqlUpdate = "ExamMarkSubmissionDateUpdate";
        private string sqlDelete = "ExamMarkSubmissionDateDelete";
        private string sqlGetById = "ExamMarkSubmissionDateGetById";
        private string sqlGetAll = "ExamMarkSubmissionDateGetAll";
               
        public int Insert(ExamMarkSubmissionDate exammarksubmissiondate)
        {
            int id = 0;
            bool isInsert = true;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand(sqlInsert);

                db = addParam(db, cmd, exammarksubmissiondate, isInsert);
                db.ExecuteNonQuery(cmd);

                object obj = db.GetParameterValue(cmd, "ExamMarkSubmissionDateId");

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

        public bool Update(ExamMarkSubmissionDate exammarksubmissiondate)
        {
            bool result = false;
            bool isInsert = false;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand(sqlUpdate);

                db = addParam(db, cmd, exammarksubmissiondate, isInsert);

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

                db.AddInParameter(cmd, "ExamMarkSubmissionDateId", DbType.Int32, id);
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

        public ExamMarkSubmissionDate GetById(int? id)
        {
            ExamMarkSubmissionDate _exammarksubmissiondate = null;
            try
            {

                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExamMarkSubmissionDate> rowMapper = GetMaper();

                var accessor = db.CreateSprocAccessor<ExamMarkSubmissionDate>(sqlGetById, rowMapper);
                _exammarksubmissiondate = accessor.Execute(id).SingleOrDefault();

            }
            catch (Exception ex)
            {
                return _exammarksubmissiondate;
            }

            return _exammarksubmissiondate;
        }

        public ExamMarkSubmissionDate GetByAcaCalSecId(int acaCalSecId)
        {
            ExamMarkSubmissionDate _exammarksubmissiondate = null;
            try
            {

                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExamMarkSubmissionDate> rowMapper = GetMaper();

                var accessor = db.CreateSprocAccessor<ExamMarkSubmissionDate>("ExamMarkSubmissionDateGetByAcaCalSecId", rowMapper);
                _exammarksubmissiondate = accessor.Execute(acaCalSecId).SingleOrDefault();

            }
            catch (Exception ex)
            {
                return _exammarksubmissiondate;
            }

            return _exammarksubmissiondate;
        }

        public List<ExamMarkSubmissionDate> GetAll()
        {
            List<ExamMarkSubmissionDate> exammarksubmissiondateList= null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<ExamMarkSubmissionDate> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<ExamMarkSubmissionDate>(sqlGetAll, mapper);
                IEnumerable<ExamMarkSubmissionDate> collection = accessor.Execute();

                exammarksubmissiondateList = collection.ToList();
            }

            catch (Exception ex)
            {
                return exammarksubmissiondateList;
            }

            return exammarksubmissiondateList;
        }

       
        #region Mapper
        private Database addParam(Database db, DbCommand cmd, ExamMarkSubmissionDate exammarksubmissiondate, bool isInsert)
        {
            if (isInsert)
            {
                db.AddOutParameter(cmd, "ExamMarkSubmissionDateId", DbType.Int32, Int32.MaxValue);
            }
            else
            {
                db.AddInParameter(cmd, "ExamMarkSubmissionDateId", DbType.Int32, exammarksubmissiondate.ExamMarkSubmissionDateId); db.AddInParameter(cmd, "ModifiedBy", DbType.Int32, exammarksubmissiondate.ModifiedBy);
                db.AddInParameter(cmd, "ModifiedDate", DbType.DateTime, exammarksubmissiondate.ModifiedDate);
            }

            	
		db.AddInParameter(cmd,"AcaCalSectionId",DbType.Int32,exammarksubmissiondate.AcaCalSectionId);
		db.AddInParameter(cmd,"FirstExaminerSubmissionDate",DbType.DateTime,exammarksubmissiondate.FirstExaminerSubmissionDate);
		db.AddInParameter(cmd,"SecondExaminerSubmissionDate",DbType.DateTime,exammarksubmissiondate.SecondExaminerSubmissionDate);
		db.AddInParameter(cmd,"ThirdExaminerSubmissionDate",DbType.DateTime,exammarksubmissiondate.ThirdExaminerSubmissionDate);
		db.AddInParameter(cmd,"CreatedBy",DbType.Int32,exammarksubmissiondate.CreatedBy);
		db.AddInParameter(cmd,"CreatedDate",DbType.DateTime,exammarksubmissiondate.CreatedDate);
		
            
            return db;
        }

        private IRowMapper<ExamMarkSubmissionDate> GetMaper()
        {
            IRowMapper<ExamMarkSubmissionDate> mapper = MapBuilder<ExamMarkSubmissionDate>.MapAllProperties()

       	   .Map(m => m.ExamMarkSubmissionDateId).ToColumn("ExamMarkSubmissionDateId")
		.Map(m => m.AcaCalSectionId).ToColumn("AcaCalSectionId")
		.Map(m => m.FirstExaminerSubmissionDate).ToColumn("FirstExaminerSubmissionDate")
		.Map(m => m.SecondExaminerSubmissionDate).ToColumn("SecondExaminerSubmissionDate")
		.Map(m => m.ThirdExaminerSubmissionDate).ToColumn("ThirdExaminerSubmissionDate")
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

