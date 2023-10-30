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
    public partial class SqlStudentAttendancePercentageStatusRepository : IStudentAttendancePercentageStatusRepository
    {

        Database db = null;

        private string sqlInsert = "StudentAttendancePercentageStatusInsert";
        private string sqlUpdate = "StudentAttendancePercentageStatusUpdate";
        private string sqlDelete = "StudentAttendancePercentageStatusDelete";
        private string sqlGetById = "StudentAttendancePercentageStatusGetById";
        private string sqlGetAll = "StudentAttendancePercentageStatusGetAll";
               
        public int Insert(StudentAttendancePercentageStatus studentattendancepercentagestatus)
        {
            int id = 0;
            bool isInsert = true;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand(sqlInsert);

                db = addParam(db, cmd, studentattendancepercentagestatus, isInsert);
                db.ExecuteNonQuery(cmd);

                object obj = db.GetParameterValue(cmd, "StudentPercentageId");

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

        public bool Update(StudentAttendancePercentageStatus studentattendancepercentagestatus)
        {
            bool result = false;
            bool isInsert = false;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand(sqlUpdate);

                db = addParam(db, cmd, studentattendancepercentagestatus, isInsert);

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

                db.AddInParameter(cmd, "StudentPercentageId", DbType.Int32, id);
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

        public StudentAttendancePercentageStatus GetById(int? id)
        {
            StudentAttendancePercentageStatus _studentattendancepercentagestatus = null;
            try
            {

                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<StudentAttendancePercentageStatus> rowMapper = GetMaper();

                var accessor = db.CreateSprocAccessor<StudentAttendancePercentageStatus>(sqlGetById, rowMapper);
                _studentattendancepercentagestatus = accessor.Execute(id).SingleOrDefault();

            }
            catch (Exception ex)
            {
                return _studentattendancepercentagestatus;
            }

            return _studentattendancepercentagestatus;
        }

        public List<StudentAttendancePercentageStatus> GetAll()
        {
            List<StudentAttendancePercentageStatus> studentattendancepercentagestatusList= null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<StudentAttendancePercentageStatus> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<StudentAttendancePercentageStatus>(sqlGetAll, mapper);
                IEnumerable<StudentAttendancePercentageStatus> collection = accessor.Execute();

                studentattendancepercentagestatusList = collection.ToList();
            }

            catch (Exception ex)
            {
                return studentattendancepercentagestatusList;
            }

            return studentattendancepercentagestatusList;
        }

       
        #region Mapper
        private Database addParam(Database db, DbCommand cmd, StudentAttendancePercentageStatus studentattendancepercentagestatus, bool isInsert)
        {
            if (isInsert)
            {
                db.AddOutParameter(cmd, "StudentPercentageId", DbType.Int32, Int32.MaxValue);
            }
            else
            {
                db.AddInParameter(cmd, "StudentPercentageId", DbType.Int32, studentattendancepercentagestatus.StudentPercentageId);
            }

            	
		db.AddInParameter(cmd,"StudentCourseHistoryId",DbType.Int32,studentattendancepercentagestatus.StudentCourseHistoryId);
        db.AddInParameter(cmd, "Percentage", DbType.Decimal, studentattendancepercentagestatus.Percentage);
        db.AddInParameter(cmd,"Status",DbType.Int32,studentattendancepercentagestatus.Status);
		db.AddInParameter(cmd,"Attribute1",DbType.String,studentattendancepercentagestatus.Attribute1);
		db.AddInParameter(cmd,"Attribute2",DbType.String,studentattendancepercentagestatus.Attribute2);
		db.AddInParameter(cmd,"CreatedBy",DbType.Int32,studentattendancepercentagestatus.CreatedBy);
		db.AddInParameter(cmd,"CreatedDate",DbType.DateTime,studentattendancepercentagestatus.CreatedDate);
		db.AddInParameter(cmd,"ModifiedBy",DbType.Int32,studentattendancepercentagestatus.ModifiedBy);
		db.AddInParameter(cmd,"ModifiedDate",DbType.DateTime,studentattendancepercentagestatus.ModifiedDate);
            
            return db;
        }

        private IRowMapper<StudentAttendancePercentageStatus> GetMaper()
        {
            IRowMapper<StudentAttendancePercentageStatus> mapper = MapBuilder<StudentAttendancePercentageStatus>.MapAllProperties()

       	   .Map(m => m.StudentPercentageId).ToColumn("StudentPercentageId")
		.Map(m => m.StudentCourseHistoryId).ToColumn("StudentCourseHistoryId")
		.Map(m => m.Percentage).ToColumn("Percentage")
		.Map(m => m.Status).ToColumn("Status")
		.Map(m => m.Attribute1).ToColumn("Attribute1")
		.Map(m => m.Attribute2).ToColumn("Attribute2")
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

