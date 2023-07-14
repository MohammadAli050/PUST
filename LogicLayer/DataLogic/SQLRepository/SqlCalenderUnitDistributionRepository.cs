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
	public partial class SqlCalenderUnitDistributionRepository : ICalenderUnitDistributionRepository
	{

		Database db = null;

		private string sqlInsert = "CalenderUnitDistributionInsert";
		private string sqlUpdate = "CalenderUnitDistributionUpdate";
		private string sqlDelete = "CalenderUnitDistributionDelete";
		private string sqlGetById = "CalenderUnitDistributionGetById";
		private string sqlGetAll = "CalenderUnitDistributionGetAll";
		private string sqlGetCourseDistributionReport = "RptCourseDistributionReport";

			   
		public int Insert(CalenderUnitDistribution calenderunitdistribution)
		{
			int id = 0;
			bool isInsert = true;

			try
			{
				db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
				DbCommand cmd = db.GetStoredProcCommand(sqlInsert);

				db = addParam(db, cmd, calenderunitdistribution, isInsert);
				db.ExecuteNonQuery(cmd);

				object obj = db.GetParameterValue(cmd, "CalenderUnitDistributionID");

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

		public bool Update(CalenderUnitDistribution calenderunitdistribution)
		{
			bool result = false;
			bool isInsert = false;

			try
			{
				db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
				DbCommand cmd = db.GetStoredProcCommand(sqlUpdate);

				db = addParam(db, cmd, calenderunitdistribution, isInsert);

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

				db.AddInParameter(cmd, "CalenderUnitDistributionID", DbType.Int32, id);
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

		public CalenderUnitDistribution GetById(int? id)
		{
			CalenderUnitDistribution _calenderunitdistribution = null;
			try
			{

				db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

				IRowMapper<CalenderUnitDistribution> rowMapper = GetMaper();

				var accessor = db.CreateSprocAccessor<CalenderUnitDistribution>(sqlGetById, rowMapper);
				_calenderunitdistribution = accessor.Execute(id).SingleOrDefault();

			}
			catch (Exception ex)
			{
				return _calenderunitdistribution;
			}

			return _calenderunitdistribution;
		}

		public List<CalenderUnitDistribution> GetAll()
		{
			List<CalenderUnitDistribution> calenderunitdistributionList= null;

			try
			{
				db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

				IRowMapper<CalenderUnitDistribution> mapper = GetMaper();

				var accessor = db.CreateSprocAccessor<CalenderUnitDistribution>(sqlGetAll, mapper);
				IEnumerable<CalenderUnitDistribution> collection = accessor.Execute();

				calenderunitdistributionList = collection.ToList();
			}

			catch (Exception ex)
			{
				return calenderunitdistributionList;
			}

			return calenderunitdistributionList;
		}

		public CalenderUnitDistribution GetByCourseId(int? id)
		{
			CalenderUnitDistribution _calenderUnitDistribution = null;
			try
			{

				db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

				IRowMapper<CalenderUnitDistribution> rowMapper = GetMaper();

				var accessor = db.CreateSprocAccessor<CalenderUnitDistribution>("CalenderUnitDistributionGetByCourseId", rowMapper);
				_calenderUnitDistribution = accessor.Execute(id).SingleOrDefault();

			}
			catch (Exception ex)
			{
				return _calenderUnitDistribution;
			}

			return _calenderUnitDistribution;
		}

		public List<rCourseDistribution> GetCourseDistributionByProgramIdAndTreeCalMasId(int ProgramId, int TreeCalMasId)
		{
			List<rCourseDistribution> _CourseDistributionList = null;

			try
			{
				db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

				IRowMapper<rCourseDistribution> mapper = GetCourseDistributionMaper();

				var accessor = db.CreateSprocAccessor<rCourseDistribution>(sqlGetCourseDistributionReport, mapper);
				IEnumerable<rCourseDistribution> collection = accessor.Execute(ProgramId, TreeCalMasId);

				_CourseDistributionList = collection.ToList();
			}

			catch (Exception ex)
			{
				return _CourseDistributionList;
			}

			return _CourseDistributionList;
		}


		#region Mapper
		private Database addParam(Database db, DbCommand cmd, CalenderUnitDistribution calenderunitdistribution, bool isInsert)
		{
			if (isInsert)
			{
				db.AddOutParameter(cmd, "CalenderUnitDistributionID", DbType.Int32, Int32.MaxValue);
			}
			else
			{
				db.AddInParameter(cmd, "CalenderUnitDistributionID", DbType.Int32, calenderunitdistribution.CalenderUnitDistributionID);
			}

				db.AddInParameter(cmd,"CalenderUnitMasterID",DbType.Int32,calenderunitdistribution.CalenderUnitMasterID);
				
				db.AddInParameter(cmd,"Name",DbType.String,calenderunitdistribution.Name);
				db.AddInParameter(cmd, "ProgramId", DbType.Int32, calenderunitdistribution.ProgramId);
				db.AddInParameter(cmd, "YearId", DbType.Int32, calenderunitdistribution.YearId);
                db.AddInParameter(cmd, "SemesterId", DbType.Int32, calenderunitdistribution.SemesterId);
				db.AddInParameter(cmd,"Sequence",DbType.Int32,calenderunitdistribution.Sequence);
				db.AddInParameter(cmd,"CreatedBy",DbType.Int32,calenderunitdistribution.CreatedBy);
				db.AddInParameter(cmd,"CreatedDate",DbType.DateTime,calenderunitdistribution.CreatedDate);
				db.AddInParameter(cmd,"ModifiedBy",DbType.Int32,calenderunitdistribution.ModifiedBy);
				db.AddInParameter(cmd,"ModifiedDate",DbType.DateTime,calenderunitdistribution.ModifiedDate);
			
			return db;
		}

		private IRowMapper<CalenderUnitDistribution> GetMaper()
		{
			IRowMapper<CalenderUnitDistribution> mapper = MapBuilder<CalenderUnitDistribution>.MapAllProperties()

			.Map(m => m.CalenderUnitDistributionID).ToColumn("CalenderUnitDistributionID")
			.Map(m => m.CalenderUnitMasterID).ToColumn("CalenderUnitMasterID")
			.Map(m => m.Name).ToColumn("Name")
			.Map(m => m.ProgramId).ToColumn("ProgramId")
			.Map(m => m.YearId).ToColumn("YearId")
            .Map(m => m.SemesterId).ToColumn("SemesterId")
			.Map(m => m.Sequence).ToColumn("Sequence")
			.Map(m => m.CreatedBy).ToColumn("CreatedBy")
			.Map(m => m.CreatedDate).ToColumn("CreatedDate")
			.Map(m => m.ModifiedBy).ToColumn("ModifiedBy")
			.Map(m => m.ModifiedDate).ToColumn("ModifiedDate")
			
			.Build();

			return mapper;
		}

		private IRowMapper<rCourseDistribution> GetCourseDistributionMaper()
		{
			IRowMapper<rCourseDistribution> mapper = MapBuilder<rCourseDistribution>.MapAllProperties()
			.Map(m => m.Sequence).ToColumn("Sequence")
			.Map(m => m.FormalCode).ToColumn("FormalCode")
			.Map(m => m.Title).ToColumn("Title")
			.Map(m => m.Credits).ToColumn("Credits")
			.Map(m => m.SemesterName).ToColumn("SemesterName")
			.Map(m => m.Priority).ToColumn("Priority")
			.Map(m => m.Marks).ToColumn("Marks")
			.Map(m => m.NodeId).ToColumn("NodeID")
			.Map(m => m.NodeName).ToColumn("NodeName")

			.Build();

			return mapper;
		}
		#endregion

	}
}

