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
    public partial class SqlFeeSetupRepository : IFeeSetupRepository
    {

        Database db = null;

        private string sqlDelete = "FeeSetupDeleteById";
        private string sqlGetById = "FeeSetupGetById";
        private string sqlGetAll = "FeeSetupGetAll";

        public int Insert(FeeSetup feesetup)
        {
            int id = 0;
            bool isInsert = true;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand("InsertFeeSetup");

                db = addParam(db, cmd, feesetup, isInsert);
                db.ExecuteNonQuery(cmd);

                object obj = db.GetParameterValue(cmd, "FeeSetupId");

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

        public bool Update(FeeSetup feesetup)
        {
            bool result = false;
            bool isInsert = false;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand("UpdateFeeSetup");

                db = addParam(db, cmd, feesetup, isInsert);

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

                db.AddInParameter(cmd, "FeeSetupId", DbType.Int32, id);
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

        public FeeSetup GetById(int? id)
        {
            FeeSetup _feesetup = null;
            try
            {

                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<FeeSetup> rowMapper = GetMaper();

                var accessor = db.CreateSprocAccessor<FeeSetup>(sqlGetById, rowMapper);
                _feesetup = accessor.Execute(id).SingleOrDefault();

            }
            catch (Exception ex)
            {
                return _feesetup;
            }

            return _feesetup;
        }

        public List<FeeSetup> GetAll()
        {
            List<FeeSetup> feesetupList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<FeeSetup> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<FeeSetup>(sqlGetAll, mapper);
                IEnumerable<FeeSetup> collection = accessor.Execute();

                feesetupList = collection.ToList();
            }

            catch (Exception ex)
            {
                return feesetupList;
            }

            return feesetupList;
        }


        #region Mapper
        private Database addParam(Database db, DbCommand cmd, FeeSetup feesetup, bool isInsert)
        {
            if (isInsert)
            {
                db.AddOutParameter(cmd, "FeeSetupId", DbType.Int32, Int32.MaxValue);
                db.AddInParameter(cmd, "CreatedBy", DbType.Int32, feesetup.CreatedBy);
                db.AddInParameter(cmd, "CreatedDate", DbType.DateTime, feesetup.CreatedDate);
            }
            else
            {
                db.AddInParameter(cmd, "FeeSetupId", DbType.Int32, feesetup.FeeSetupId);
                db.AddInParameter(cmd, "ModifiedBy", DbType.Int32, feesetup.ModifiedBy);
                db.AddInParameter(cmd, "ModifiedDate", DbType.DateTime, feesetup.ModifiedDate);
            }


            db.AddInParameter(cmd, "AcademicCalendarId", DbType.Int32, feesetup.AcademicCalendarId);
            db.AddInParameter(cmd, "ProgramId", DbType.Int32, feesetup.ProgramId);
            db.AddInParameter(cmd, "BatchId", DbType.Int32, feesetup.BatchId);
            db.AddInParameter(cmd, "TypeDefinitionId", DbType.Int32, feesetup.TypeDefinitionId);
            db.AddInParameter(cmd, "FundTypeId", DbType.Int32, feesetup.FundTypeId);
            db.AddInParameter(cmd, "Amount", DbType.Decimal, feesetup.Amount);
            db.AddInParameter(cmd, "ScholarshipStatusType", DbType.Int32, feesetup.ScholarshipStatusType);
            db.AddInParameter(cmd, "GovNonGovType", DbType.Int32, feesetup.GovNonGovType);
            db.AddInParameter(cmd, "CalenderUnitTypeId", DbType.Int32, feesetup.CalenderUnitTypeId);
            return db;
        }

        private IRowMapper<FeeSetup> GetMaper()
        {
            IRowMapper<FeeSetup> mapper = MapBuilder<FeeSetup>.MapAllProperties()

           .Map(m => m.FeeSetupId).ToColumn("FeeSetupId")
        .Map(m => m.AcademicCalendarId).ToColumn("AcademicCalendarId")
        .Map(m => m.ProgramId).ToColumn("ProgramId")
        .Map(m => m.BatchId).ToColumn("BatchId")
        .Map(m => m.TypeDefinitionId).ToColumn("TypeDefinitionId")
        .Map(m => m.FundTypeId).ToColumn("FundTypeId")
        .Map(m => m.Amount).ToColumn("Amount")
        .Map(m => m.ScholarshipStatusType).ToColumn("ScholarshipStatusType")
        .Map(m => m.GovNonGovType).ToColumn("GovNonGovType")
        .Map(m => m.CalenderUnitTypeId).ToColumn("CalenderUnitTypeId")
        .Map(m => m.CreatedBy).ToColumn("CreatedBy")
        .Map(m => m.CreatedDate).ToColumn("CreatedDate")
        .Map(m => m.ModifiedBy).ToColumn("ModifiedBy")
        .Map(m => m.ModifiedDate).ToColumn("ModifiedDate")


        .DoNotMap(m => m.CalenderUnitTypeId)

            .DoNotMap(m => m.ScholarshipStatusType)

            .Build();

            return mapper;
        }

        private IRowMapper<FeeGroupMaster> GetAllFeeGroupByProgramIdAcaCalIdMapper()
        {
            IRowMapper<FeeGroupMaster> mapper = MapBuilder<FeeGroupMaster>.MapAllProperties()

                .Map(m => m.FeeGroupMasterId).ToColumn("FeeGroupMasterId")
                .Map(m => m.FeeGroupName).ToColumn("FeeGroupName")
                .Map(m => m.IsActive).ToColumn("IsActive")
                .Map(m => m.ProgramId).ToColumn("ProgramId")
                .Map(m => m.StudentAdmissionAcaCalId).ToColumn("StudentAdmissionAcaCalId")
                .Map(m => m.Remarks).ToColumn("Remarks")
                .Map(m => m.FundTypeId).ToColumn("FundTypeId")
                .Map(m => m.TypeDefinitionId).ToColumn("TypeDefinitionId")
                .Map(m => m.Amount).ToColumn("Amount")
                

                .DoNotMap(m => m.CreatedBy)
                .DoNotMap(m => m.CreatedDate)
                .DoNotMap(m => m.ModifiedBy)
                .DoNotMap(m => m.ModifiedDate)
                .DoNotMap(m => m.Attribute1)
                .DoNotMap(m => m.Attribute2)
                .DoNotMap(m => m.Attribute3)
                .DoNotMap(m => m.Attribute4)
                .DoNotMap(m => m.AccountNo)
                .DoNotMap(m => m.FundName)
                .DoNotMap(m => m.BatchNO)
                .DoNotMap(m => m.ProgramDetailName)
                .DoNotMap(m => m.ProgramName)
                .Build();

            return mapper;
        }

        private IRowMapper<FeeSetup> GetMaperForId()
        {
            IRowMapper<FeeSetup> mapper = MapBuilder<FeeSetup>.MapAllProperties()


                .Map(m => m.TypeDefinitionId).ToColumn("TypeDefinitionId")
                .Map(m => m.FeeSetupId).ToColumn("FeeSetupId")
                .Map(m => m.AcademicCalendarId).ToColumn("AcademicCalendarId")
                .Map(m => m.ProgramId).ToColumn("ProgramId")
                .Map(m => m.BatchId).ToColumn("BatchId")
                .Map(m => m.Amount).ToColumn("Amount")
                .Map(m => m.CreatedBy).ToColumn("CreatedBy")
                .Map(m => m.CreatedDate).ToColumn("CreatedDate")
                .Map(m => m.ModifiedBy).ToColumn("ModifiedBy")
                .Map(m => m.ModifiedDate).ToColumn("ModifiedDate")
                .Map(m => m.FundTypeId).ToColumn("FundTypeId")
                .Map(m => m.GovNonGovType).ToColumn("GovNonGovType")
                .Map(m => m.CalenderUnitTypeId).ToColumn("CalenderUnitTypeId")

                .DoNotMap(m => m.ScholarshipStatusType)

                .Build();

            return mapper;
        }


        #endregion

        public List<FeeSetup> GetAllByProgramIdAcaCalIdScholarshipStatusAndGovNonGov(int programId, int? acaCalId, int scholarshipStatusId, int govNonGovId)
        {
            List<FeeSetup> feeSetupList = null;
            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                IRowMapper<FeeSetup> mapper = GetMaper();
                var accessor = db.CreateSprocAccessor<FeeSetup>("FeeSetupGetByProgramIdAcaCalIdFeeType", mapper);
                IEnumerable<FeeSetup> collection = accessor.Execute(programId, acaCalId, scholarshipStatusId, govNonGovId);
                feeSetupList = collection.ToList();
            }

            catch (Exception ex)
            {
                return feeSetupList;
            }
            return feeSetupList;
        }

        public List<FeeGroupMaster> GetAllFeeGroupByProgramIdAcaCalId(int? programId, int? acaCalId)
        {
            List<FeeGroupMaster> feeSetupList = null;
            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                IRowMapper<FeeGroupMaster> mapper = GetAllFeeGroupByProgramIdAcaCalIdMapper();
                var accessor = db.CreateSprocAccessor<FeeGroupMaster>("GetAllFeeGroupByProgramIdAcaCalId", mapper);
                IEnumerable<FeeGroupMaster> collection = accessor.Execute(programId, acaCalId);
                feeSetupList = collection.ToList();
            }

            catch (Exception ex)
            {
                return feeSetupList;
            }
            return feeSetupList;
        }
    }
}

