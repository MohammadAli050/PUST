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
	public partial class SqlCourseExtendOneRepository : ICourseExtendOneRepository
	{

		Database db = null;

		private string sqlInsert = "CourseExtendOneInsert";
		private string sqlUpdate = "CourseExtendOneUpdate";
		private string sqlDelete = "CourseExtendOneDelete";
		private string sqlGetById = "CourseExtendOneGetById";
		private string sqlGetAll = "CourseExtendOneGetAll";
			   
		public int Insert(CourseExtendOne courseextendone)
		{
			int id = 0;
			bool isInsert = true;

			try
			{
				db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
				DbCommand cmd = db.GetStoredProcCommand(sqlInsert);

				db = addParam(db, cmd, courseextendone, isInsert);
				db.ExecuteNonQuery(cmd);

				object obj = db.GetParameterValue(cmd, "CourseExtendId");

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

		public bool Update(CourseExtendOne courseextendone)
		{
			bool result = false;
			bool isInsert = false;

			try
			{
				db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
				DbCommand cmd = db.GetStoredProcCommand(sqlUpdate);

				db = addParam(db, cmd, courseextendone, isInsert);

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

				db.AddInParameter(cmd, "CourseExtendId", DbType.Int32, id);
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

		public CourseExtendOne GetById(int? id)
		{
			CourseExtendOne _courseextendone = null;
			try
			{

				db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

				IRowMapper<CourseExtendOne> rowMapper = GetMaper();

				var accessor = db.CreateSprocAccessor<CourseExtendOne>(sqlGetById, rowMapper);
				_courseextendone = accessor.Execute(id).SingleOrDefault();

			}
			catch (Exception ex)
			{
				return _courseextendone;
			}

			return _courseextendone;
		}

		public List<CourseExtendOne> GetAll()
		{
			List<CourseExtendOne> courseextendoneList= null;

			try
			{
				db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

				IRowMapper<CourseExtendOne> mapper = GetMaper();

				var accessor = db.CreateSprocAccessor<CourseExtendOne>(sqlGetAll, mapper);
				IEnumerable<CourseExtendOne> collection = accessor.Execute();

				courseextendoneList = collection.ToList();
			}

			catch (Exception ex)
			{
				return courseextendoneList;
			}

			return courseextendoneList;
		}

		public CourseExtendOne GetByCourseIdVersionId(int courseId, int versionId)
		{
			CourseExtendOne _courseextendone = null;
			try
			{

				db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

				IRowMapper<CourseExtendOne> rowMapper = GetMaper();

				var accessor = db.CreateSprocAccessor<CourseExtendOne>("CourseExtendOneGetByCourseIdVersionId", rowMapper);
				_courseextendone = accessor.Execute(courseId, versionId).SingleOrDefault();

			}
			catch (Exception ex)
			{
				return _courseextendone;
			}

			return _courseextendone;
		}
	   
		#region Mapper
		private Database addParam(Database db, DbCommand cmd, CourseExtendOne courseextendone, bool isInsert)
		{
			if (isInsert)
			{
				db.AddOutParameter(cmd, "CourseExtendId", DbType.Int32, Int32.MaxValue);
			}
			else
			{
				db.AddInParameter(cmd, "CourseExtendId", DbType.Int32, courseextendone.CourseExtendId);
			}

				
		db.AddInParameter(cmd,"CourseId",DbType.Int32,courseextendone.CourseId);
		db.AddInParameter(cmd, "VersionId", DbType.Int32, courseextendone.VersionId);
		db.AddInParameter(cmd,"Marks",DbType.Decimal,courseextendone.Marks);
		db.AddInParameter(cmd,"Attribute1",DbType.String,courseextendone.Attribute1);
		db.AddInParameter(cmd,"Attribute2",DbType.String,courseextendone.Attribute2);
		db.AddInParameter(cmd,"Attribute3",DbType.String,courseextendone.Attribute3);
		db.AddInParameter(cmd,"Attribute4",DbType.String,courseextendone.Attribute4);
		db.AddInParameter(cmd,"Attribute5",DbType.String,courseextendone.Attribute5);
		db.AddInParameter(cmd,"CreatedBy",DbType.Int32,courseextendone.CreatedBy);
		db.AddInParameter(cmd,"CreatedDate",DbType.DateTime,courseextendone.CreatedDate);
		db.AddInParameter(cmd,"ModifiedBy",DbType.Int32,courseextendone.ModifiedBy);
		db.AddInParameter(cmd,"ModifiedDate",DbType.DateTime,courseextendone.ModifiedDate);
			
			return db;
		}

		private IRowMapper<CourseExtendOne> GetMaper()
		{
			IRowMapper<CourseExtendOne> mapper = MapBuilder<CourseExtendOne>.MapAllProperties()

		   .Map(m => m.CourseExtendId).ToColumn("CourseExtendId")
		.Map(m => m.CourseId).ToColumn("CourseId")
		.Map(m => m.VersionId).ToColumn("VersionId")
		.Map(m => m.Marks).ToColumn("Marks")
		.Map(m => m.Attribute1).ToColumn("Attribute1")
		.Map(m => m.Attribute2).ToColumn("Attribute2")
		.Map(m => m.Attribute3).ToColumn("Attribute3")
		.Map(m => m.Attribute4).ToColumn("Attribute4")
		.Map(m => m.Attribute5).ToColumn("Attribute5")
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

