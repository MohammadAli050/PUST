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
	public partial class SqlStudentAdditionalInfoRepository : IStudentAdditionalInfoRepository
	{

		Database db = null;

		private string sqlInsert = "StudentAdditionalInfoInsert";
		private string sqlUpdate = "StudentAdditionalInfoUpdate";
		private string sqlDelete = "StudentAdditionalInfoDelete";
		private string sqlGetById = "StudentAdditionalInfoGetById";
		private string sqlGetAll = "StudentAdditionalInfoGetAll";
		private string sqlGetByStudentId = "StudentAdditionalInfoGetByStudentId";
			   
		public int Insert(StudentAdditionalInfo studentadditionalinfo)
		{
			int id = 0;
			bool isInsert = true;

			try
			{
				db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
				DbCommand cmd = db.GetStoredProcCommand(sqlInsert);

				db = addParam(db, cmd, studentadditionalinfo, isInsert);
				db.ExecuteNonQuery(cmd);

				object obj = db.GetParameterValue(cmd, "InfoId");

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

		public bool Update(StudentAdditionalInfo studentadditionalinfo)
		{
			bool result = false;
			bool isInsert = false;

			try
			{
				db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
				DbCommand cmd = db.GetStoredProcCommand(sqlUpdate);

				db = addParam(db, cmd, studentadditionalinfo, isInsert);

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

				db.AddInParameter(cmd, "InfoId", DbType.Int32, id);
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

		public StudentAdditionalInfo GetById(int? id)
		{
			StudentAdditionalInfo _studentadditionalinfo = null;
			try
			{

				db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

				IRowMapper<StudentAdditionalInfo> rowMapper = GetMaper();

				var accessor = db.CreateSprocAccessor<StudentAdditionalInfo>(sqlGetById, rowMapper);
				_studentadditionalinfo = accessor.Execute(id).SingleOrDefault();

			}
			catch (Exception ex)
			{
				return _studentadditionalinfo;
			}

			return _studentadditionalinfo;
		}

		public List<StudentAdditionalInfo> GetAll()
		{
			List<StudentAdditionalInfo> studentadditionalinfoList= null;

			try
			{
				db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

				IRowMapper<StudentAdditionalInfo> mapper = GetMaper();

				var accessor = db.CreateSprocAccessor<StudentAdditionalInfo>(sqlGetAll, mapper);
				IEnumerable<StudentAdditionalInfo> collection = accessor.Execute();

				studentadditionalinfoList = collection.ToList();
			}

			catch (Exception ex)
			{
				return studentadditionalinfoList;
			}

			return studentadditionalinfoList;
		}

		#region Mapper
		private Database addParam(Database db, DbCommand cmd, StudentAdditionalInfo studentadditionalinfo, bool isInsert)
		{
			if (isInsert)
			{
				db.AddOutParameter(cmd, "InfoId", DbType.Int32, Int32.MaxValue);
			}
			else
			{
				db.AddInParameter(cmd, "InfoId", DbType.Int32, studentadditionalinfo.InfoId);
			}

			db.AddInParameter(cmd,"StudentId",DbType.Int32,studentadditionalinfo.StudentId);
			db.AddInParameter(cmd,"YearId",DbType.Int32,studentadditionalinfo.YearId);
			db.AddInParameter(cmd,"SemesterId",DbType.Int32,studentadditionalinfo.SemesterId);
            db.AddInParameter(cmd, "YearNo", DbType.Int32, studentadditionalinfo.YearNo);
            db.AddInParameter(cmd, "SemesterNo", DbType.Int32, studentadditionalinfo.SemesterNo);
            db.AddInParameter(cmd, "RunningSession", DbType.Int32, studentadditionalinfo.RunningSession);
			db.AddInParameter(cmd,"YearSectionId",DbType.Int32,studentadditionalinfo.YearSectionId);
			db.AddInParameter(cmd,"RegistrationNo",DbType.String,studentadditionalinfo.RegistrationNo);
            db.AddInParameter(cmd, "QuataId", DbType.Int32, studentadditionalinfo.QuataId);
            db.AddInParameter(cmd, "ParentsJobId", DbType.String, studentadditionalinfo.ParentsJobId);
            db.AddInParameter(cmd, "FatherPhoneNumber", DbType.String, studentadditionalinfo.FatherPhoneNumber);
            db.AddInParameter(cmd, "MotherPhoneNumber", DbType.String, studentadditionalinfo.MotherPhoneNumber);
            db.AddInParameter(cmd, "CampusId", DbType.String, studentadditionalinfo.CampusId);
			db.AddInParameter(cmd,"Attribute1",DbType.String,studentadditionalinfo.Attribute1);
			db.AddInParameter(cmd,"Attribute2",DbType.String,studentadditionalinfo.Attribute2);
			db.AddInParameter(cmd,"Attribute3",DbType.String,studentadditionalinfo.Attribute3);
			db.AddInParameter(cmd,"CreatedBy",DbType.Int32,studentadditionalinfo.CreatedBy);
			db.AddInParameter(cmd,"CreatedDate",DbType.DateTime,studentadditionalinfo.CreatedDate);
			db.AddInParameter(cmd,"ModifiedBy",DbType.Int32,studentadditionalinfo.ModifiedBy);
			db.AddInParameter(cmd,"ModifiedDate",DbType.DateTime,studentadditionalinfo.ModifiedDate);
			
			return db;
		}

		private IRowMapper<StudentAdditionalInfo> GetMaper()
		{
			IRowMapper<StudentAdditionalInfo> mapper = MapBuilder<StudentAdditionalInfo>.MapAllProperties()

			.Map(m => m.InfoId).ToColumn("InfoId")
			.Map(m => m.StudentId).ToColumn("StudentId")
			.Map(m => m.YearId).ToColumn("YearId")
			.Map(m => m.SemesterId).ToColumn("SemesterId")
            .Map(m => m.YearNo).ToColumn("YearNo")
            .Map(m => m.SemesterNo).ToColumn("SemesterNo")
            .Map(m => m.RunningSession).ToColumn("RunningSession")
			.Map(m => m.YearSectionId).ToColumn("YearSectionId")
			.Map(m => m.RegistrationNo).ToColumn("RegistrationNo")
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

		public StudentAdditionalInfo GetByStudentId(int studentId)
		{
			StudentAdditionalInfo _studentadditionalinfo = null;
			try
			{

				db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

				IRowMapper<StudentAdditionalInfo> rowMapper = GetMaper();

				var accessor = db.CreateSprocAccessor<StudentAdditionalInfo>(sqlGetByStudentId, rowMapper);
				_studentadditionalinfo = accessor.Execute(studentId).FirstOrDefault();

			}
			catch (Exception ex)
			{
				return _studentadditionalinfo;
			}

			return _studentadditionalinfo;
		}
	}
}

