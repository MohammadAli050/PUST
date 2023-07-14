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
    public partial class SqlYearRepository : IYearRepository
    {

        Database db = null;

        private string sqlInsert = "YearInsert";
        private string sqlUpdate = "YearUpdate";
        private string sqlDelete = "YearDelete";
        private string sqlGetById = "YearGetById";
        private string sqlGetAll = "YearGetAll";
        private string sqlGetByprogramId = "YearGetByProgramId";
        private string sqlGetByProgramIdYearName = "YearGetByProgramIdYearName";
               
        public int Insert(Year year)
        {
            int id = 0;
            bool isInsert = true;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand(sqlInsert);

                db = addParam(db, cmd, year, isInsert);
                db.ExecuteNonQuery(cmd);

                object obj = db.GetParameterValue(cmd, "YearId");

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

        public bool Update(Year year)
        {
            bool result = false;
            bool isInsert = false;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand(sqlUpdate);

                db = addParam(db, cmd, year, isInsert);

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

                db.AddInParameter(cmd, "YearId", DbType.Int32, id);
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

        public Year GetById(int? id)
        {
            Year _year = null;
            try
            {

                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<Year> rowMapper = GetMaper();

                var accessor = db.CreateSprocAccessor<Year>(sqlGetById, rowMapper);
                _year = accessor.Execute(id).SingleOrDefault();

            }
            catch (Exception ex)
            {
                return _year;
            }

            return _year;
        }

        public List<Year> GetAll()
        {
            List<Year> yearList= null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<Year> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<Year>(sqlGetAll, mapper);
                IEnumerable<Year> collection = accessor.Execute();

                yearList = collection.ToList();
            }

            catch (Exception ex)
            {
                return yearList;
            }

            return yearList;
        }

       
        #region Mapper
        private Database addParam(Database db, DbCommand cmd, Year year, bool isInsert)
        {
            if (isInsert)
            {
                db.AddOutParameter(cmd, "YearId", DbType.Int32, Int32.MaxValue);
            }
            else
            {
                db.AddInParameter(cmd, "YearId", DbType.Int32, year.YearId);
            }

		    db.AddInParameter(cmd,"YearName",DbType.String,year.YearName);
		    db.AddInParameter(cmd,"Code",DbType.String,year.Code);
            db.AddInParameter(cmd, "YearNo", DbType.String, year.YearNo);
		    db.AddInParameter(cmd,"ProgramId",DbType.Int32,year.ProgramId);
		    db.AddInParameter(cmd,"DepartmentId",DbType.Int32,year.DepartmentId);
		    db.AddInParameter(cmd,"CreatedBy",DbType.Int32,year.CreatedBy);
		    db.AddInParameter(cmd,"CreatedDate",DbType.DateTime,year.CreatedDate);
		    db.AddInParameter(cmd,"ModifiedBy",DbType.Int32,year.ModifiedBy);
		    db.AddInParameter(cmd,"ModifiedDate",DbType.DateTime,year.ModifiedDate);
            
            return db;
        }

        private IRowMapper<Year> GetMaper()
        {
            IRowMapper<Year> mapper = MapBuilder<Year>.MapAllProperties()

       	   .Map(m => m.YearId).ToColumn("YearId")
		    .Map(m => m.YearName).ToColumn("YearName")
		    .Map(m => m.Code).ToColumn("Code")
            .Map(m => m.YearNo).ToColumn("YearNo")
		    .Map(m => m.ProgramId).ToColumn("ProgramId")
		    .Map(m => m.DepartmentId).ToColumn("DepartmentId")
		    .Map(m => m.CreatedBy).ToColumn("CreatedBy")
		    .Map(m => m.CreatedDate).ToColumn("CreatedDate")
		    .Map(m => m.ModifiedBy).ToColumn("ModifiedBy")
		    .Map(m => m.ModifiedDate).ToColumn("ModifiedDate")
            
            .Build();

            return mapper;
        }

        private IRowMapper<YearDistinctDTO> GetMaperYearDistinct()
        {
            IRowMapper<YearDistinctDTO> mapper = MapBuilder<YearDistinctDTO>.MapAllProperties()

           .Map(m => m.YearNo).ToColumn("YearNo")
           .Map(m => m.YearNoName).ToColumn("YearNoName")

            .Build();

            return mapper;
        }
        #endregion


        public List<Year> GetByProgramId(int programId)
        {
            List<Year> yearList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<Year> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<Year>(sqlGetByprogramId, mapper);
                IEnumerable<Year> collection = accessor.Execute(programId);

                yearList = collection.ToList();
            }

            catch (Exception ex)
            {
                return yearList;
            }

            return yearList;
        }

        public List<Year> GetByProgramIdYearName(int programId, string yearName) 
        {
            List<Year> yearList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<Year> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<Year>(sqlGetByProgramIdYearName, mapper);
                IEnumerable<Year> collection = accessor.Execute(programId, yearName);

                yearList = collection.ToList();
            }

            catch (Exception ex)
            {
                return yearList;
            }

            return yearList;
        }


        public List<YearDistinctDTO> GetAllDistinct()
        {
            List<YearDistinctDTO> yearList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<YearDistinctDTO> mapper = GetMaperYearDistinct();

                var accessor = db.CreateSprocAccessor<YearDistinctDTO>("YearGetAllDistinct", mapper);
                IEnumerable<YearDistinctDTO> collection = accessor.Execute();

                yearList = collection.ToList();
            }

            catch (Exception ex)
            {
                return yearList;
            }

            return yearList;
        }
    }
}

