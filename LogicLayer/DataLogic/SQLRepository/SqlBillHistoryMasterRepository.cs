﻿using System;
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
using System.Data.SqlClient;
using LogicLayer.BusinessObjects.DTO;

namespace LogicLayer.DataLogic.SQLRepository
{
    public partial class SqlBillHistoryMasterRepository : IBillHistoryMasterRepository
    {

        Database db = null;

        //private string sqlInsert = "BillHistoryMasterInsert";
        //private string sqlUpdate = "BillHistoryMasterUpdate";
        private string sqlDelete = "BillDeleteByMasterId";
        private string sqlGetById = "BillHistoryMasterGetById";
        private string sqlGetAll = "BillHistoryMasterGetAll";
        private string sqlGetByStudentIdAcaCalIdFees = "BillHistoryMasterGetByStudentIdAcaCalIdFees";
        private string sqlGetBillDueCountByStudentIdAcaCalId = "BillHistoryMasterGetDueCountByStudentIdAcaCalId";
        private string sqlGetByStudentIdReferenceNo = "BillHistoryMasterGetByStudentIdReferenceNo";

        public int Insert(BillHistoryMaster billhistorymaster)
        {
            int id = 0;
            bool isInsert = true;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand("BillHistoryMasterInsert");

                db = addParam(db, cmd, billhistorymaster, isInsert);
                db.ExecuteNonQuery(cmd);

                object obj = db.GetParameterValue(cmd, "BillHistoryMasterId");

                if (obj != null)
                {
                    int.TryParse(obj.ToString(), out id);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return id;
        }

        public bool Update(BillHistoryMaster billhistorymaster)
        {
            bool result = false;
            bool isInsert = false;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand("BillHistoryMasterUpdate");

                db = addParam(db, cmd, billhistorymaster, isInsert);

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

        public bool Delete(int id, int tag)
        {
            bool result = false;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand(sqlDelete);

                db.AddInParameter(cmd, "BillHistoryMasterId", DbType.Int32, id);
                db.AddInParameter(cmd, "Tag", DbType.Int32, tag);
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

        public BillHistoryMaster GetById(int? id)
        {
            BillHistoryMaster _billhistorymaster = null;
            try
            {

                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<BillHistoryMaster> rowMapper = GetMaper();

                var accessor = db.CreateSprocAccessor<BillHistoryMaster>(sqlGetById, rowMapper);
                _billhistorymaster = accessor.Execute(id).SingleOrDefault();

            }
            catch (Exception ex)
            {
                return _billhistorymaster;
            }

            return _billhistorymaster;
        }

        public List<BillHistoryMaster> GetAll()
        {
            List<BillHistoryMaster> billhistorymasterList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<BillHistoryMaster> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<BillHistoryMaster>(sqlGetAll, mapper);
                IEnumerable<BillHistoryMaster> collection = accessor.Execute();

                billhistorymasterList = collection.ToList();
            }

            catch (Exception ex)
            {
                return billhistorymasterList;
            }

            return billhistorymasterList;
        }

        public string GetBillMasterMaxReferenceNo(DateTime date)
        {
            string referenceNo;

            db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
            using (DbConnection dbConnection = db.CreateConnection())
            {
                dbConnection.Open();
                DbTransaction dbTransaction = dbConnection.BeginTransaction();

                DbCommand cmdTR = db.GetStoredProcCommand("BillHistoryMasterGetMaxReferenceNo");
                //SqlParameter outPutParameter = new SqlParameter();
                //outPutParameter.ParameterName = "@Id";
                //outPutParameter.SqlDbType = System.Data.SqlDbType.Int;
                //outPutParameter.Direction = System.Data.ParameterDirection.Output;
                db.AddOutParameter(cmdTR, "ReferenceNo", DbType.String, Int32.MaxValue);
                db.AddInParameter(cmdTR, "BillingDate", DbType.Date, date);

                db.ExecuteNonQuery(cmdTR, dbTransaction);

                object objSL = db.GetParameterValue(cmdTR, "ReferenceNo");

                if (objSL != null)
                {
                    referenceNo = Convert.ToString(objSL.ToString());
                }
                else
                {
                    referenceNo = null;
                }
            }

            return referenceNo;
        }

        public BillHistoryMaster GetByStudentIdAcaCalIdFees(int studentId, int acaCalId, decimal fees, bool isDue)
        {
            BillHistoryMaster _billhistorymaster = null;
            try
            {

                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<BillHistoryMaster> rowMapper = GetMaper();

                var accessor = db.CreateSprocAccessor<BillHistoryMaster>(sqlGetByStudentIdAcaCalIdFees, rowMapper);
                _billhistorymaster = accessor.Execute(studentId, acaCalId, fees, isDue).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return _billhistorymaster;
            }

            return _billhistorymaster;
        }

        public List<BillHistoryMaster> GetBillDueCountByStudentIdAcaCalId(int studentId, int acaCalId)
        {
            List<BillHistoryMaster> billhistorymasterList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<BillHistoryMaster> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<BillHistoryMaster>(sqlGetBillDueCountByStudentIdAcaCalId, mapper);
                IEnumerable<BillHistoryMaster> collection = accessor.Execute(studentId, acaCalId);

                billhistorymasterList = collection.ToList();
            }

            catch (Exception ex)
            {
                return billhistorymasterList;
            }

            return billhistorymasterList;
        }

        public BillHistoryMaster GetByReferenceId(int studentId, string referenceNo)
        {
            BillHistoryMaster _billhistorymaster = null;
            try
            {

                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<BillHistoryMaster> rowMapper = GetMaper();

                var accessor = db.CreateSprocAccessor<BillHistoryMaster>(sqlGetByStudentIdReferenceNo, rowMapper);
                _billhistorymaster = accessor.Execute(studentId, referenceNo).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return _billhistorymaster;
            }

            return _billhistorymaster;
        }

        public List<BillDeleteDTO> GetStudentsForBillPrintByProgramIdSessionIdStudentAdmissionSessionId(int programId, int sessionId, int? admissionSessionId)
        {
            List<BillDeleteDTO> billDeleteList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<BillDeleteDTO> mapper = GetBillForDeleteMaper();

                var accessor = db.CreateSprocAccessor<BillDeleteDTO>("GetStudentsForBillPrintByProgramIdSessionIdStudentAdmissionSessionId", mapper);
                IEnumerable<BillDeleteDTO> collection = accessor.Execute(programId, sessionId, admissionSessionId);

                billDeleteList = collection.ToList();
            }

            catch (Exception ex)
            {
                return billDeleteList;
            }

            return billDeleteList;
        }

        #region Mapper
        private Database addParam(Database db, DbCommand cmd, BillHistoryMaster billhistorymaster, bool isInsert)
        {
            if (isInsert)
            {
                db.AddOutParameter(cmd, "BillHistoryMasterId", DbType.Int32, Int32.MaxValue);
                db.AddInParameter(cmd, "CreatedBy", DbType.Int32, billhistorymaster.CreatedBy);
                db.AddInParameter(cmd, "CreatedDate", DbType.DateTime, billhistorymaster.CreatedDate);
            }
            else
            {
                db.AddInParameter(cmd, "BillHistoryMasterId", DbType.Int32, billhistorymaster.BillHistoryMasterId);
                db.AddInParameter(cmd, "ModifiedBy", DbType.Int32, billhistorymaster.ModifiedBy);
                db.AddInParameter(cmd, "ModifiedDate", DbType.DateTime, billhistorymaster.ModifiedDate);
            }

            db.AddInParameter(cmd, "StudentId", DbType.Int32, billhistorymaster.StudentId);
            db.AddInParameter(cmd, "BillTypeId", DbType.Int32, billhistorymaster.BillTypeId);
            db.AddInParameter(cmd, "Amount", DbType.Decimal, billhistorymaster.Amount);
            db.AddInParameter(cmd, "ReferenceNo", DbType.String, billhistorymaster.ReferenceNo);
            db.AddInParameter(cmd, "BillingDate", DbType.DateTime, billhistorymaster.BillingDate);
            db.AddInParameter(cmd, "IsDeleted", DbType.Boolean, billhistorymaster.IsDeleted);
            db.AddInParameter(cmd, "IsDue", DbType.Boolean, billhistorymaster.IsDue);
            db.AddInParameter(cmd, "ParentBillHistroyMasterId", DbType.Int32, billhistorymaster.ParentBillHistroyMasterId);
            db.AddInParameter(cmd, "AcaCalId", DbType.Int32, billhistorymaster.AcaCalId);
            db.AddInParameter(cmd, "AcademicCalenadYear", DbType.String, billhistorymaster.AcademicCalenadYear);
            db.AddInParameter(cmd, "Serial", DbType.String, billhistorymaster.Serial);
            db.AddInParameter(cmd, "Invoice", DbType.String, billhistorymaster.Invoice);
            db.AddInParameter(cmd, "Attribute4", DbType.String, billhistorymaster.Attribute4);

            return db;
        }

        private IRowMapper<BillHistoryMaster> GetMaper()
        {
            IRowMapper<BillHistoryMaster> mapper = MapBuilder<BillHistoryMaster>.MapAllProperties()

            .Map(m => m.BillHistoryMasterId).ToColumn("BillHistoryMasterId")
            .Map(m => m.StudentId).ToColumn("StudentId")
            .Map(m => m.BillTypeId).ToColumn("BillTypeId")
            .Map(m => m.Amount).ToColumn("Amount")
            .Map(m => m.ReferenceNo).ToColumn("ReferenceNo")
            .Map(m => m.BillingDate).ToColumn("BillingDate")
            .Map(m => m.IsDeleted).ToColumn("IsDeleted")
            .Map(m => m.IsDue).ToColumn("IsDue")
            .Map(m => m.ParentBillHistroyMasterId).ToColumn("ParentBillHistroyMasterId")
            .Map(m => m.AcaCalId).ToColumn("AcaCalId")
            .Map(m => m.AcademicCalenadYear).ToColumn("AcademicCalenadYear")
            .Map(m => m.Serial).ToColumn("Serial")
            .Map(m => m.Invoice).ToColumn("Invoice")
            .Map(m => m.Attribute4).ToColumn("Attribute4")
            .Map(m => m.CreatedBy).ToColumn("CreatedBy")
            .Map(m => m.CreatedDate).ToColumn("CreatedDate")
            .Map(m => m.ModifiedBy).ToColumn("ModifiedBy")
            .Map(m => m.ModifiedDate).ToColumn("ModifiedDate")

            .Build();

            return mapper;
        }

        private IRowMapper<BillDeleteDTO> GetBillForDeleteMaper()
        {
            IRowMapper<BillDeleteDTO> mapper = MapBuilder<BillDeleteDTO>.MapAllProperties()
            .Map(m => m.BillHistoryMasterId).ToColumn("BillHistoryMasterId")
            .Map(m => m.StudentId).ToColumn("StudentId")
            .Map(m => m.Roll).ToColumn("Roll")
            .Map(m => m.Name).ToColumn("Name")
            .Map(m => m.ReferenceNo).ToColumn("ReferenceNo")
            .Map(m => m.Amount).ToColumn("Amount")
            .Map(m => m.BillingDate).ToColumn("BillingDate")
            .Map(m => m.CollectionHistoryId).ToColumn("CollectionHistoryId")
            .Build();

            return mapper;
        }
        #endregion

        //public BillHistoryOrder GetOrderIdByBillHistoryIdList(string BillHistoryIdList, int UserId)
        //{
        //    BillHistoryOrder _billHistoryOrder = null;
        //    try
        //    {

        //        db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

        //        IRowMapper<BillHistoryOrder> rowMapper = MapBuilder<BillHistoryOrder>.MapAllProperties()

        //        .Map(m => m.OrderId).ToColumn("OrderId")
        //        .Map(m => m.StatusId).ToColumn("StatusId")

        //        .Build();

        //        var accessor = db.CreateSprocAccessor<BillHistoryOrder>("GenerateOrderIdAndInitializeIntoBillHistory", rowMapper);
        //        _billHistoryOrder = accessor.Execute(BillHistoryIdList, UserId).FirstOrDefault();
        //    }
        //    catch (Exception ex)
        //    {
        //        return _billHistoryOrder;
        //    }

        //    return _billHistoryOrder;
        //}

        //public BillHistoryOrder InsertCollectionHistoryFromOnlinePayment(string refId, int billHistoryMasterId, decimal amount, string paymentType)
        //{
        //    BillHistoryOrder _billHistoryOrder = null;
        //    try
        //    {

        //        db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

        //        IRowMapper<BillHistoryOrder> rowMapper = MapBuilder<BillHistoryOrder>.MapAllProperties()

        //        .Map(m => m.OrderId).ToColumn("OrderId")
        //        .Map(m => m.StatusId).ToColumn("StatusId")

        //        .Build();

        //        var accessor = db.CreateSprocAccessor<BillHistoryOrder>("InsertCollectionHistoryFromOnlinePayment", rowMapper);
        //        _billHistoryOrder = accessor.Execute(refId, billHistoryMasterId, amount, paymentType, DateTime.Now).FirstOrDefault();
        //    }
        //    catch (Exception ex)
        //    {
        //        return _billHistoryOrder;
        //    }

        //    return _billHistoryOrder;
        //}

    }
}

