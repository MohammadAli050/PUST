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
    public partial class SqlPreviousEducationRepository : IPreviousEducationRepository
    {

        Database db = null;

        private string sqlInsert = "PreviousEducationInsert";
        private string sqlUpdate = "PreviousEducationUpdate";
        private string sqlDelete = "PreviousEducationDeleteById";
        private string sqlGetById = "PreviousEducationGetById";
        private string sqlGetAll = "PreviousEducationGetAll";
               
        public int Insert(PreviousEducation previouseducation)
        {
            int id = 0;
            bool isInsert = true;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand(sqlInsert);

                db = addParam(db, cmd, previouseducation, isInsert);
                db.ExecuteNonQuery(cmd);

                object obj = db.GetParameterValue(cmd, "PreviousEducationId");

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

        public bool Update(PreviousEducation previouseducation)
        {
            bool result = false;
            bool isInsert = false;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand(sqlUpdate);

                db = addParam(db, cmd, previouseducation, isInsert);

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

                db.AddInParameter(cmd, "PreviousEducationId", DbType.Int32, id);
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

        public PreviousEducation GetById(int? id)
        {
            PreviousEducation _previouseducation = null;
            try
            {

                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<PreviousEducation> rowMapper = GetMaper();

                var accessor = db.CreateSprocAccessor<PreviousEducation>(sqlGetById, rowMapper);
                _previouseducation = accessor.Execute(id).FirstOrDefault();

            }
            catch (Exception ex)
            {
                return _previouseducation;
            }

            return _previouseducation;
        }

        public List<PreviousEducation> GetAll()
        {
            List<PreviousEducation> previouseducationList= null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<PreviousEducation> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<PreviousEducation>(sqlGetAll, mapper);
                IEnumerable<PreviousEducation> collection = accessor.Execute();

                previouseducationList = collection.ToList();
            }

            catch (Exception ex)
            {
                return previouseducationList;
            }

            return previouseducationList;
        }

        public List<PreviousEducation> GetAllByPersonId(int PersonId)
        {
            List<PreviousEducation> previouseducationList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<PreviousEducation> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<PreviousEducation>("PreviousEducationGetAllGetAllByPersonId", mapper);
                IEnumerable<PreviousEducation> collection = accessor.Execute(PersonId);

                previouseducationList = collection.ToList();
            }

            catch (Exception ex)
            {
                return previouseducationList;
            }

            return previouseducationList;
        }
       
        #region Mapper
        private Database addParam(Database db, DbCommand cmd, PreviousEducation previouseducation, bool isInsert)
        {
            if (isInsert)
            {
                db.AddOutParameter(cmd, "PreviousEducationId", DbType.Int32, Int32.MaxValue);
            }
            else
            {
                db.AddInParameter(cmd, "PreviousEducationId", DbType.Int32, previouseducation.PreviousEducationId);
            }

            	
		db.AddInParameter(cmd,"PersonId",DbType.Int32,previouseducation.PersonId);
		db.AddInParameter(cmd,"EducationTypeId",DbType.Int32,previouseducation.EducationTypeId);
		db.AddInParameter(cmd,"EducationCategoryId",DbType.Int32,previouseducation.EducationCategoryId);
		db.AddInParameter(cmd,"ExamRollNo",DbType.String,previouseducation.ExamRollNo);
		db.AddInParameter(cmd,"InstituteName",DbType.String,previouseducation.InstituteName);
		db.AddInParameter(cmd,"ConcentratedMajor",DbType.String,previouseducation.ConcentratedMajor);
		db.AddInParameter(cmd,"Board",DbType.String,previouseducation.Board);
		db.AddInParameter(cmd,"PassingYear",DbType.Int32,previouseducation.PassingYear);
		db.AddInParameter(cmd,"Session",DbType.String,previouseducation.Session);
		db.AddInParameter(cmd,"Duration",DbType.String,previouseducation.Duration);
		db.AddInParameter(cmd,"Result",DbType.String,previouseducation.Result);
		db.AddInParameter(cmd,"MarksOrGPA",DbType.String,previouseducation.MarksOrGPA);
		db.AddInParameter(cmd,"MarksOrGPAWithoutOptional",DbType.String,previouseducation.MarksOrGPAWithoutOptional);
		db.AddInParameter(cmd,"CreatedBy",DbType.Int32,previouseducation.CreatedBy);
		db.AddInParameter(cmd,"CreatedDate",DbType.DateTime,previouseducation.CreatedDate);
		db.AddInParameter(cmd,"ModifiedBy",DbType.Int32,previouseducation.ModifiedBy);
		db.AddInParameter(cmd,"ModifiedDate",DbType.DateTime,previouseducation.ModifiedDate);
            
            return db;
        }

        private IRowMapper<PreviousEducation> GetMaper()
        {
            IRowMapper<PreviousEducation> mapper = MapBuilder<PreviousEducation>.MapAllProperties()

       	   .Map(m => m.PreviousEducationId).ToColumn("PreviousEducationId")
		.Map(m => m.PersonId).ToColumn("PersonId")
		.Map(m => m.EducationTypeId).ToColumn("EducationTypeId")
		.Map(m => m.EducationCategoryId).ToColumn("EducationCategoryId")
		.Map(m => m.ExamRollNo).ToColumn("ExamRollNo")
		.Map(m => m.InstituteName).ToColumn("InstituteName")
		.Map(m => m.ConcentratedMajor).ToColumn("ConcentratedMajor")
		.Map(m => m.Board).ToColumn("Board")
		.Map(m => m.PassingYear).ToColumn("PassingYear")
		.Map(m => m.Session).ToColumn("Session")
		.Map(m => m.Duration).ToColumn("Duration")
		.Map(m => m.Result).ToColumn("Result")
		.Map(m => m.MarksOrGPA).ToColumn("MarksOrGPA")
		.Map(m => m.MarksOrGPAWithoutOptional).ToColumn("MarksOrGPAWithoutOptional")
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

