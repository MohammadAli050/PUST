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
    public partial class SqlYearSectionRepository : IYearSectionRepository
    {

        Database db = null;

        private string sqlInsert = "YearSectionInsert";
        private string sqlUpdate = "YearSectionUpdate";
        private string sqlDelete = "YearSectionDelete";
        private string sqlGetById = "YearSectionGetById";
        private string sqlGetAll = "YearSectionGetAll";
        private string sqlGetByYearSectionId = "YearSectionGetByYearSectionId";
               
        public int Insert(YearSection yearsection)
        {
            int id = 0;
            bool isInsert = true;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand(sqlInsert);

                db = addParam(db, cmd, yearsection, isInsert);
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

        public bool Update(YearSection yearsection)
        {
            bool result = false;
            bool isInsert = false;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand(sqlUpdate);

                db = addParam(db, cmd, yearsection, isInsert);

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

        public YearSection GetById(int? id)
        {
            YearSection _yearsection = null;
            try
            {

                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<YearSection> rowMapper = GetMaper();

                var accessor = db.CreateSprocAccessor<YearSection>(sqlGetById, rowMapper);
                _yearsection = accessor.Execute(id).SingleOrDefault();

            }
            catch (Exception ex)
            {
                return _yearsection;
            }

            return _yearsection;
        }

        public List<YearSection> GetAll()
        {
            List<YearSection> yearsectionList= null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<YearSection> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<YearSection>(sqlGetAll, mapper);
                IEnumerable<YearSection> collection = accessor.Execute();

                yearsectionList = collection.ToList();
            }

            catch (Exception ex)
            {
                return yearsectionList;
            }

            return yearsectionList;
        }
       
        #region Mapper
        private Database addParam(Database db, DbCommand cmd, YearSection yearsection, bool isInsert)
        {
            if (isInsert)
            {
                db.AddOutParameter(cmd, "Id", DbType.Int32, Int32.MaxValue);
            }
            else
            {
                db.AddInParameter(cmd, "Id", DbType.Int32, yearsection.Id);
            }

            	
		db.AddInParameter(cmd,"Name",DbType.String,yearsection.Name);
		db.AddInParameter(cmd,"Attribute1",DbType.String,yearsection.Attribute1);
		db.AddInParameter(cmd,"Attribute2",DbType.String,yearsection.Attribute2);
		db.AddInParameter(cmd,"Attribute3",DbType.String,yearsection.Attribute3);
		db.AddInParameter(cmd,"CreatedBy",DbType.Int32,yearsection.CreatedBy);
		db.AddInParameter(cmd,"CreatedDate",DbType.DateTime,yearsection.CreatedDate);
		db.AddInParameter(cmd,"ModifiedBy",DbType.Int32,yearsection.ModifiedBy);
		db.AddInParameter(cmd,"ModifiedDate",DbType.DateTime,yearsection.ModifiedDate);
            
            return db;
        }

        private IRowMapper<YearSection> GetMaper()
        {
            IRowMapper<YearSection> mapper = MapBuilder<YearSection>.MapAllProperties()

       	   .Map(m => m.Id).ToColumn("Id")
		.Map(m => m.Name).ToColumn("Name")
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

