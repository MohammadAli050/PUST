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
    public partial class SqlSemesterRepository : ISemesterRepository
    {

        Database db = null;

        private string sqlInsert = "SemesterInsert";
        private string sqlUpdate = "SemesterUpdate";
        private string sqlDelete = "SemesterDelete";
        private string sqlGetById = "SemesterGetById";
        private string sqlGetAll = "SemesterGetAll";
        private string sqlGetByYearId = "SemesterGetByYearId";
        private string sqlGetByProgramIdYearId = "SemesterGetByProgramIdYearId";
        
               
        public int Insert(Semester semester)
        {
            int id = 0;
            bool isInsert = true;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand(sqlInsert);

                db = addParam(db, cmd, semester, isInsert);
                db.ExecuteNonQuery(cmd);

                object obj = db.GetParameterValue(cmd, "SemesterId");

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

        public bool Update(Semester semester)
        {
            bool result = false;
            bool isInsert = false;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand(sqlUpdate);

                db = addParam(db, cmd, semester, isInsert);

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

                db.AddInParameter(cmd, "SemesterId", DbType.Int32, id);
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

        public Semester GetById(int? id)
        {
            Semester _semester = null;
            try
            {

                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<Semester> rowMapper = GetMaper();

                var accessor = db.CreateSprocAccessor<Semester>(sqlGetById, rowMapper);
                _semester = accessor.Execute(id).SingleOrDefault();

            }
            catch (Exception ex)
            {
                return _semester;
            }

            return _semester;
        }

        public List<Semester> GetAll()
        {
            List<Semester> semesterList= null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<Semester> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<Semester>(sqlGetAll, mapper);
                IEnumerable<Semester> collection = accessor.Execute();

                semesterList = collection.ToList();
            }

            catch (Exception ex)
            {
                return semesterList;
            }

            return semesterList;
        }

       
        #region Mapper
        private Database addParam(Database db, DbCommand cmd, Semester semester, bool isInsert)
        {
            if (isInsert)
            {
                db.AddOutParameter(cmd, "SemesterId", DbType.Int32, Int32.MaxValue);
            }
            else
            {
                db.AddInParameter(cmd, "SemesterId", DbType.Int32, semester.SemesterId);
            }

            	
		    db.AddInParameter(cmd,"SemesterName",DbType.String,semester.SemesterName);
		    db.AddInParameter(cmd,"YearId",DbType.Int32,semester.YearId);
		    db.AddInParameter(cmd,"ProgramId",DbType.Int32,semester.ProgramId);
            db.AddInParameter(cmd,"SemesterNo", DbType.Int32, semester.SemesterNo);
		    db.AddInParameter(cmd,"Attribute1",DbType.String,semester.Attribute1);
		    db.AddInParameter(cmd,"Attribute2",DbType.String,semester.Attribute2);
		    db.AddInParameter(cmd,"Attribute3",DbType.String,semester.Attribute3);
		    db.AddInParameter(cmd,"CreatedBy",DbType.Int32,semester.CreatedBy);
		    db.AddInParameter(cmd,"CreatedDate",DbType.DateTime,semester.CreatedDate);
		    db.AddInParameter(cmd,"ModifiedBy",DbType.Int32,semester.ModifiedBy);
		    db.AddInParameter(cmd,"ModifiedDate",DbType.DateTime,semester.ModifiedDate);
            
            return db;
        }

        private IRowMapper<Semester> GetMaper()
        {
            IRowMapper<Semester> mapper = MapBuilder<Semester>.MapAllProperties()

       	   .Map(m => m.SemesterId).ToColumn("SemesterId")
		    .Map(m => m.SemesterName).ToColumn("SemesterName")
		    .Map(m => m.YearId).ToColumn("YearId")
		    .Map(m => m.ProgramId).ToColumn("ProgramId")
            .Map(m => m.SemesterNo).ToColumn("SemesterNo")
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

        private IRowMapper<SemesterDistinctDTO> GetMaperSemesterDistinct()
        {
            IRowMapper<SemesterDistinctDTO> mapper = MapBuilder<SemesterDistinctDTO>.MapAllProperties()

           .Map(m => m.SemesterNo).ToColumn("SemesterNo")
           .Map(m => m.SemesterNoName).ToColumn("SemesterNoName")

            .Build();

            return mapper;
        }
        #endregion


        public List<Semester> GetByYearId(int yearId)
        {
            List<Semester> semesterList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<Semester> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<Semester>(sqlGetByYearId, mapper);
                IEnumerable<Semester> collection = accessor.Execute(yearId);

                semesterList = collection.ToList();
            }

            catch (Exception ex)
            {
                return semesterList;
            }

            return semesterList;
        }



        public List<Semester> GetByProgramIdYearId(int programId, int yearId)
        {
            List<Semester> semesterList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<Semester> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<Semester>(sqlGetByProgramIdYearId, mapper);
                IEnumerable<Semester> collection = accessor.Execute(programId, yearId);

                semesterList = collection.ToList();
            }

            catch (Exception ex)
            {
                return semesterList;
            }

            return semesterList;
        }



        public List<SemesterDistinctDTO> GetAllDistinct()
        {
            List<SemesterDistinctDTO> semesterList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<SemesterDistinctDTO> mapper = GetMaperSemesterDistinct();

                var accessor = db.CreateSprocAccessor<SemesterDistinctDTO>("SemesterGetAllDistinct", mapper);
                IEnumerable<SemesterDistinctDTO> collection = accessor.Execute();

                semesterList = collection.ToList();
            }

            catch (Exception ex)
            {
                return semesterList;
            }

            return semesterList;
        }



    }
}

