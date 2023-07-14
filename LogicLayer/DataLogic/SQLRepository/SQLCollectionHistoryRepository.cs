using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicLayer.BusinessObjects;
using LogicLayer.BusinessObjects.DTO;
//using LogicLayer.BusinessObjects.RO;
using LogicLayer.DataLogic.IRepository;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace LogicLayer.DataLogic.SQLRepository
{
    public partial class SQLCollectionHistoryRepository : ICollectionHistoryRepository
    {

        Database db = null;

        private string sqlUpdate = "CollectionHistoryUpdate";
        private string sqlDelete = "CollectionHistoryDeleteById";
        private string sqlGetById = "CollectionHistoryGetById";
        private string sqlGetAll = "CollectionHistoryGetAll";
        private string sqlGetGetByMRNoPaymentType = "CollectionHistoryGetByMRNoPaymentType";
        private string sqlGetByStudentIdFundIdBillHistoryMasterIdLastDate = "GetCollectionHistoryByStudentIdFundIdBillHistoryMasterIdLastDate";

        public int Insert(CollectionHistory collectionhistory)
        {
            int id = 0;
            bool isInsert = true;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand("CollectionHistoryInsert");

                db = addParam(db, cmd, collectionhistory, isInsert);
                db.ExecuteNonQuery(cmd);

                object obj = db.GetParameterValue(cmd, "CollectionHistoryId");

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

        public bool Update(CollectionHistory collectionhistory)
        {
            bool result = false;
            bool isInsert = false;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand(sqlUpdate);

                db = addParam(db, cmd, collectionhistory, isInsert);

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

                db.AddInParameter(cmd, "CollectionHistoryId", DbType.Int32, id);
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

        public CollectionHistory GetById(int id)
        {
            CollectionHistory _collectionhistory = null;
            try
            {

                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<CollectionHistory> rowMapper = GetMaper();

                var accessor = db.CreateSprocAccessor<CollectionHistory>(sqlGetById, rowMapper);
                _collectionhistory = accessor.Execute(id).SingleOrDefault();

            }
            catch (Exception ex)
            {
                return _collectionhistory;
            }

            return _collectionhistory;
        }

        public List<CollectionHistory> GetAll()
        {
            List<CollectionHistory> collectionhistoryList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<CollectionHistory> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<CollectionHistory>(sqlGetAll, mapper);
                IEnumerable<CollectionHistory> collection = accessor.Execute();

                collectionhistoryList = collection.ToList();
            }

            catch (Exception ex)
            {
                return collectionhistoryList;
            }

            return collectionhistoryList;
        }

        #region Mapper
        private Database addParam(Database db, DbCommand cmd, CollectionHistory collectionhistory, bool isInsert)
        {
            if (isInsert)
            {
                db.AddOutParameter(cmd, "CollectionHistoryId", DbType.Int32, Int32.MaxValue);
                db.AddInParameter(cmd, "CreatedBy", DbType.Int32, collectionhistory.CreatedBy);
                db.AddInParameter(cmd, "CreatedDate", DbType.DateTime, collectionhistory.CreatedDate);
            }
            else
            {
                db.AddInParameter(cmd, "CollectionHistoryId", DbType.Int32, collectionhistory.CollectionHistoryId);
                db.AddInParameter(cmd, "ModifiedBy", DbType.Int32, collectionhistory.ModifiedBy);
                db.AddInParameter(cmd, "ModifiedDate", DbType.DateTime, collectionhistory.ModifiedDate);
            }


            db.AddInParameter(cmd, "StudentId", DbType.Int32, collectionhistory.StudentId);
            db.AddInParameter(cmd, "BillHistoryMasterId", DbType.Int32, collectionhistory.BillHistoryMasterId);
            db.AddInParameter(cmd, "BillHistoryId", DbType.Int32, collectionhistory.BillHistoryId);
            db.AddInParameter(cmd, "MoneyReciptSerialNo", DbType.String, collectionhistory.MoneyReciptSerialNo);
            db.AddInParameter(cmd, "AcaCalId", DbType.Int32, collectionhistory.AcaCalId);
            db.AddInParameter(cmd, "FeeSetupId", DbType.Int32, collectionhistory.FeeSetupId);
            db.AddInParameter(cmd, "TypeDefinitionId", DbType.Int32, collectionhistory.TypeDefinitionId);
            db.AddInParameter(cmd, "Amount", DbType.Decimal, collectionhistory.Amount);
            db.AddInParameter(cmd, "CollectionDate", DbType.DateTime, collectionhistory.CollectionDate);
            db.AddInParameter(cmd, "PaymentType", DbType.String, collectionhistory.PaymentType);
            db.AddInParameter(cmd, "CounterId", DbType.Int32, collectionhistory.CounterId);
            db.AddInParameter(cmd, "BankName", DbType.String, collectionhistory.BankName);
            db.AddInParameter(cmd, "ChequeNo", DbType.String, collectionhistory.ChequeNo);
            db.AddInParameter(cmd, "ReferenceNo", DbType.String, collectionhistory.ReferenceNo);
            db.AddInParameter(cmd, "Comments", DbType.String, collectionhistory.Comments);
            db.AddInParameter(cmd, "IsDeleted", DbType.Boolean, collectionhistory.IsDeleted);
            db.AddInParameter(cmd, "Attribute1", DbType.String, collectionhistory.Attribute1);
            db.AddInParameter(cmd, "Attribute2", DbType.String, collectionhistory.Attribute2);
            db.AddInParameter(cmd, "Attribute3", DbType.String, collectionhistory.Attribute3);
            db.AddInParameter(cmd, "Attribute4", DbType.String, collectionhistory.Attribute4);
            
            
            return db;
        }

        private IRowMapper<CollectionHistory> GetMaper()
        {
            IRowMapper<CollectionHistory> mapper = MapBuilder<CollectionHistory>.MapAllProperties()

           .Map(m => m.CollectionHistoryId).ToColumn("CollectionHistoryId")
            .Map(m => m.StudentId).ToColumn("StudentId")
            .Map(m => m.BillHistoryMasterId).ToColumn("BillHistoryMasterId")
            .Map(m => m.BillHistoryId).ToColumn("BillHistoryId")
            .Map(m => m.MoneyReciptSerialNo).ToColumn("MoneyReciptSerialNo")
            .Map(m => m.AcaCalId).ToColumn("AcaCalId")
            .Map(m => m.TypeDefinitionId).ToColumn("TypeDefinitionId")
            .Map(m => m.Amount).ToColumn("Amount")
            .Map(m => m.CollectionDate).ToColumn("CollectionDate")
            .Map(m => m.PaymentType).ToColumn("PaymentType")
            .Map(m => m.CounterId).ToColumn("CounterId")
            .Map(m => m.BankName).ToColumn("BankName")
            .Map(m => m.ChequeNo).ToColumn("ChequeNo")
            .Map(m => m.ReferenceNo).ToColumn("ReferenceNo")
            .Map(m => m.Comments).ToColumn("Comments")
            .Map(m => m.IsDeleted).ToColumn("IsDeleted")
            .Map(m => m.Attribute1).ToColumn("Attribute1")
            .Map(m => m.Attribute2).ToColumn("Attribute2")
            .Map(m => m.Attribute3).ToColumn("Attribute3")
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
                .DoNotMap(m => m.CollectionHistoryId)
                .Build();

            return mapper;
        }

        private IRowMapper<CollectionHistory> CollectionHistoryGetByBillHistoryMasterIdMapper()
        {
            IRowMapper<CollectionHistory> mapper = MapBuilder<CollectionHistory>.MapAllProperties()

                .Map(m => m.CollectionHistoryId).ToColumn("CollectionHistoryId")
                .Map(m => m.StudentId).ToColumn("StudentId")
                .Map(m => m.BillHistoryMasterId).ToColumn("BillHistoryMasterId")
                .Map(m => m.BillHistoryId).ToColumn("BillHistoryId")
                .Map(m => m.MoneyReciptSerialNo).ToColumn("MoneyReciptSerialNo")
                .Map(m => m.AcaCalId).ToColumn("AcaCalId")
                .Map(m => m.FeeSetupId).ToColumn("FeeSetupId")
                .Map(m => m.TypeDefinitionId).ToColumn("TypeDefinitionId")
                .Map(m => m.Amount).ToColumn("Amount")
                .Map(m => m.CollectionDate).ToColumn("CollectionDate")
                .Map(m => m.PaymentType).ToColumn("PaymentType")
                .Map(m => m.CounterId).ToColumn("CounterId")
                .Map(m => m.BankName).ToColumn("BankName")
                .Map(m => m.ChequeNo).ToColumn("ChequeNo")
                .Map(m => m.ReferenceNo).ToColumn("ReferenceNo")
                .Map(m => m.Comments).ToColumn("Comments")
                .Map(m => m.IsDeleted).ToColumn("IsDeleted")
                .Map(m => m.Attribute1).ToColumn("Attribute1")
                .Map(m => m.Attribute2).ToColumn("Attribute2")
                .Map(m => m.Attribute3).ToColumn("Attribute3")
                .Map(m => m.Attribute4).ToColumn("Attribute4")
                .Map(m => m.CreatedBy).ToColumn("CreatedBy")
                .Map(m => m.CreatedDate).ToColumn("CreatedDate")
                .Map(m => m.ModifiedBy).ToColumn("ModifiedBy")
                .Map(m => m.ModifiedDate).ToColumn("ModifiedDate")

                .Build();

            return mapper;
        }
        #endregion

        public CollectionHistory IsDuplicateMoneyReceipt(string moneyReceiptNo, string paymentType)
        {
            CollectionHistory _collectionhistory = null;
            try
            {

                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<CollectionHistory> rowMapper = GetMaper();

                var accessor = db.CreateSprocAccessor<CollectionHistory>(sqlGetGetByMRNoPaymentType, rowMapper);
                _collectionhistory = accessor.Execute(moneyReceiptNo, paymentType).FirstOrDefault();

            }
            catch (Exception ex)
            {
                return _collectionhistory;
            }

            return _collectionhistory;
        }

        public List<CollectionHistory> GetByStudentIdFundIdBillHistoryMasterIdLastDate(int studentId, int fundId, int billHistoryMasterId, DateTime lastDate)
        {
            List<CollectionHistory> collectionhistoryList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<CollectionHistory> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<CollectionHistory>(sqlGetByStudentIdFundIdBillHistoryMasterIdLastDate, mapper);
                IEnumerable<CollectionHistory> collection = accessor.Execute(studentId, fundId, billHistoryMasterId, lastDate).ToList();

                collectionhistoryList = collection.ToList();
            }

            catch (Exception ex)
            {
                return collectionhistoryList;
            }

            return collectionhistoryList;
        }

        public List<CollectionHistory> GetByBillHistoryMasterId(int billHistoryMasterId)
        {
            List<CollectionHistory> _collectionhistory = null;
            try
            {

                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<CollectionHistory> rowMapper = CollectionHistoryGetByBillHistoryMasterIdMapper();

                var accessor = db.CreateSprocAccessor<CollectionHistory>("CollectionHistoryGetByBillHistoryMasterId", rowMapper);
                _collectionhistory = accessor.Execute(billHistoryMasterId).ToList();

            }
            catch (Exception ex)
            {
                return _collectionhistory;
            }

            return _collectionhistory;
        }

        public List<BillDeleteDTO> GetBillPaidStudentsByProgramIdSessionIdAdmissionSessionId(int programId, int sessionId, int? admissionSessionId)
        {
            List<BillDeleteDTO> billDeleteList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<BillDeleteDTO> mapper = GetBillForDeleteMaper();

                var accessor = db.CreateSprocAccessor<BillDeleteDTO>("GetBillPaidStudentsByProgramIdSessionIdAdmissionSessionId", mapper);
                IEnumerable<BillDeleteDTO> collection = accessor.Execute(programId, sessionId, admissionSessionId);

                billDeleteList = collection.ToList();
            }

            catch (Exception ex)
            {
                return billDeleteList;
            }

            return billDeleteList;
        }
        //public List<DepartmentWiseCollection> GetDepartmentWiseByDateRange(DateTime fromDate, DateTime toDate)
        //{
        //    List<DepartmentWiseCollection> collectionhistoryList = null;

        //    try
        //    {
        //        db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

        //        IRowMapper<DepartmentWiseCollection> mapper = MapBuilder<DepartmentWiseCollection>.MapAllProperties()

        //        .Map(m => m.DeptID).ToColumn("DeptID")
        //        .Map(m => m.DeptName).ToColumn("DeptName")
        //        .Map(m => m.ProgramID).ToColumn("ProgramID")
        //        .Map(m => m.ShortName).ToColumn("ShortName")
        //        .Map(m => m.Amount).ToColumn("Amount")
        //        .DoNotMap(m => m.FeeTypeId)
        //        .DoNotMap(m => m.FeeName)

        //        .Build();

        //        var accessor = db.CreateSprocAccessor<rDepartmentWiseCollection>("CollectionHistoryDepartmentWiseByDateRange", mapper);
        //        IEnumerable<rDepartmentWiseCollection> collection = accessor.Execute(fromDate, toDate);

        //        collectionhistoryList = collection.ToList();
        //    }

        //    catch (Exception ex)
        //    {
        //        return collectionhistoryList;
        //    }

        //    return collectionhistoryList;
        //}

        //public List<rDepartmentWiseCollection> GetProgramFeeWiseByDateRange(DateTime fromDate, DateTime toDate)
        //{
        //    List<rDepartmentWiseCollection> collectionhistoryList = null;

        //    try
        //    {
        //        db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

        //        IRowMapper<rDepartmentWiseCollection> mapper = MapBuilder<rDepartmentWiseCollection>.MapAllProperties()

        //        .Map(m => m.DeptID).ToColumn("DeptID")
        //        .Map(m => m.DeptName).ToColumn("DeptName")
        //        .Map(m => m.ProgramID).ToColumn("ProgramID")
        //        .Map(m => m.ShortName).ToColumn("ShortName")
        //        .Map(m => m.FeeTypeId).ToColumn("FeeTypeId")
        //        .Map(m => m.FeeName).ToColumn("FeeName")
        //        .Map(m => m.Amount).ToColumn("Amount")

        //        .Build();

        //        var accessor = db.CreateSprocAccessor<rDepartmentWiseCollection>("CollectionHistoryProgramFeeWiseByDateRange", mapper);
        //        IEnumerable<rDepartmentWiseCollection> collection = accessor.Execute(fromDate, toDate);

        //        collectionhistoryList = collection.ToList();
        //    }

        //    catch (Exception ex)
        //    {
        //        return collectionhistoryList;
        //    }

        //    return collectionhistoryList;
        //}

        //public List<RStudentWiseCollection> GetStudentPaymentByPaymentTypeIdFundIdAndDateRange(int? fundTypeId, int? billCounterTypeId, DateTime fromDate, DateTime toDate)
        //{
        //    List<RStudentWiseCollection> collectionhistoryList = null;

        //    try
        //    {
        //        db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

        //        IRowMapper<RStudentWiseCollection> mapper = GetStudentPaymentByDateRangeMaper();

        //        var accessor = db.CreateSprocAccessor<RStudentWiseCollection>("CollectionHistoryByDateRange_1", mapper);
        //        IEnumerable<RStudentWiseCollection> collection = accessor.Execute(fromDate, toDate, fundTypeId, billCounterTypeId);

        //        collectionhistoryList = collection.ToList();
        //    }

        //    catch (Exception ex)
        //    {
        //        return collectionhistoryList;
        //    }

        //    return collectionhistoryList;
        //}


        //public List<RStudentWiseCollection> GetStudentPaymentDetailByPaymentTypeIdFundIdAndDateRange(int? fundTypeId, int? billCounterTypeId, DateTime fromDate, DateTime toDate)
        //{
        //    List<RStudentWiseCollection> collectionhistoryList = null;

        //    try
        //    {
        //        db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

        //        IRowMapper<RStudentWiseCollection> mapper = GetStudentPaymentDetailMapper();

        //        var accessor = db.CreateSprocAccessor<RStudentWiseCollection>("CollectionHistoryDetailByFundTypeIdBillCounterTypeIdAndDateRange", mapper);
        //        IEnumerable<RStudentWiseCollection> collection = accessor.Execute(fromDate, toDate, fundTypeId, billCounterTypeId);

        //        collectionhistoryList = collection.ToList();
        //    }

        //    catch (Exception ex)
        //    {
        //        return collectionhistoryList;
        //    }

        //    return collectionhistoryList;
        //}

        //public List<RStudentWiseCollection> GetStudentPaymentSummaryByPaymentTypeIdFundIdAndDateRange_TB(int? fundTypeId, int? billCounterTypeId, DateTime fromDate, DateTime toDate)
        //{
        //    List<RStudentWiseCollection> collectionhistoryList = null;

        //    try
        //    {
        //        db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

        //        IRowMapper<RStudentWiseCollection> mapper = GetStudentPaymentSummaryTbMapper();

        //        var accessor = db.CreateSprocAccessor<RStudentWiseCollection>("GetStudentPaymentSummaryByPaymentTypeIdFundIdAndDateRange_TB", mapper);
        //        IEnumerable<RStudentWiseCollection> collection = accessor.Execute(fromDate, toDate, fundTypeId, billCounterTypeId);

        //        collectionhistoryList = collection.ToList();
        //    }

        //    catch (Exception ex)
        //    {
        //        return collectionhistoryList;
        //    }

        //    return collectionhistoryList;
        //}

        //public List<rDepartmentWiseCollection>
        //    GetDepartmentWisePaymentCollectionByPaymentTypeIdFundIdAndDateRange(int? fundTypeId, int? billCounterTypeId,
        //        DateTime fromDate, DateTime toDate)
        //{
        //    List<rDepartmentWiseCollection> collectionhistoryList = null;

        //    try
        //    {
        //        db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

        //        IRowMapper<rDepartmentWiseCollection> mapper = MapBuilder<rDepartmentWiseCollection>.MapAllProperties()

        //        .Map(m => m.ShortName).ToColumn("ProgramShortName")
        //        .Map(m => m.FundName).ToColumn("FundName")
        //        .Map(m => m.PaymentType).ToColumn("CounterShortName")
        //        .Map(m => m.Amount).ToColumn("Amount")
        //        .Map(m => m.StudentCount).ToColumn("StudentCount")

        //        .DoNotMap(m => m.FeeTypeId)
        //        .DoNotMap(m => m.FeeName)
        //        .DoNotMap(m => m.DeptName)
        //        .DoNotMap(m => m.DeptID)
        //        .DoNotMap(m => m.ProgramID)
        //        .Build();

        //        var accessor = db.CreateSprocAccessor<rDepartmentWiseCollection>("DepartmentWiseCollectionByDateRangeFundIdAndPaymentTypeId", mapper);
        //        IEnumerable<rDepartmentWiseCollection> collection = accessor.Execute(fromDate, toDate, fundTypeId, billCounterTypeId);
        //        collectionhistoryList = collection.ToList();
        //    }

        //    catch (Exception ex)
        //    {
        //        return collectionhistoryList;
        //    }

        //    return collectionhistoryList;
        //}

        //public List<rDepartmentWiseCollection> GetDepartmentWisePaymentCollectionDetailByPaymentTypeIdFundIdAndDateRange(
        //    int? fundTypeId, int? billCounterTypeId, DateTime fromDate, DateTime toDate)
        //{
        //    List<rDepartmentWiseCollection> collectionhistoryList = null;

        //    try
        //    {
        //        db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

        //        IRowMapper<rDepartmentWiseCollection> mapper = MapBuilder<rDepartmentWiseCollection>.MapAllProperties()

        //        .Map(m => m.ShortName).ToColumn("ProgramShortName")
        //        .Map(m => m.FundName).ToColumn("FundName")
        //        .Map(m => m.PaymentType).ToColumn("CounterShortName")
        //        .Map(m => m.Amount).ToColumn("Amount")
        //        .Map(m => m.StudentCount).ToColumn("StudentCount")
        //        .Map(m => m.FeeName).ToColumn("FeeName")

        //        .DoNotMap(m => m.FeeTypeId)
        //        .DoNotMap(m => m.DeptName)
        //        .DoNotMap(m => m.DeptID)
        //        .DoNotMap(m => m.ProgramID)
        //        .Build();

        //        var accessor = db.CreateSprocAccessor<rDepartmentWiseCollection>("DepartmentWiseCollectionDetailByDateRangeFundIdAndPaymentTypeId", mapper);
        //        IEnumerable<rDepartmentWiseCollection> collection = accessor.Execute(fromDate, toDate, fundTypeId, billCounterTypeId);
        //        collectionhistoryList = collection.ToList();
        //    }

        //    catch (Exception ex)
        //    {
        //        return collectionhistoryList;
        //    }

        //    return collectionhistoryList;
        //}

        //public List<FundWiseCollectionViewObject> GetFundWiseTotalCollectionByDateRange(DateTime fromDate,
        //    DateTime toDate)
        //{
        //    List<FundWiseCollectionViewObject> collectionList = null;

        //    try
        //    {
        //        db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

        //        IRowMapper<FundWiseCollectionViewObject> mapper = MapBuilder<FundWiseCollectionViewObject>.MapAllProperties()

        //        .Map(m => m.FundTypeId).ToColumn("FundTypeId")
        //        .Map(m => m.FundName).ToColumn("FundName")
        //        .Map(m => m.AccountNo).ToColumn("AccountNo")
        //        .Map(m => m.CounterName).ToColumn("CounterFullName")
        //        .Map(m => m.Amount).ToColumn("Amount")

        //        .Build();

        //        var accessor = db.CreateSprocAccessor<FundWiseCollectionViewObject>("Collection_FundWiseTotalBillCollectionByDateRange", mapper);
        //        IEnumerable<FundWiseCollectionViewObject> collection = accessor.Execute(fromDate, toDate);
        //        collectionList = collection.ToList();
        //    }

        //    catch (Exception ex)
        //    {
        //        return collectionList;
        //    }

        //    return collectionList;
        //}

        //public List<StudentPayments> GetAllCollectionByPaymentTypeIdFundIdAndDateRange_TB(int? fundTypeId, int? billCounterTypeId, DateTime fromDate, DateTime toDate)
        //{
        //    List<StudentPayments> collectionhistoryList = null;

        //    try
        //    {
        //        db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

        //        IRowMapper<StudentPayments> mapper = MapBuilder<StudentPayments>.MapAllProperties()

        //        .Map(m => m.Roll).ToColumn("Roll")
        //        .Map(m => m.FundName).ToColumn("FundName")
        //        .Map(m => m.PaymentType).ToColumn("PaymentType")
        //        .Map(m => m.Amount).ToColumn("Amount")
        //        .Map(m => m.InvoiceNo).ToColumn("InvoiceNo")
        //        .Map(m => m.AccountNo).ToColumn("AccountNo")
        //        .Map(m => m.CollectionDate).ToColumn("CollectionDate")
        //        .Map(m => m.FullName).ToColumn("FullName")
        //        .Map(m => m.OrderId).ToColumn("OrderId")
        //        .Map(m => m.ReferenceNo).ToColumn("ReferenceNo")

        //        .Build();

        //        var accessor = db.CreateSprocAccessor<StudentPayments>("GetAllCollectionByPaymentTypeIdFundIdAndDateRange_TB", mapper);
        //        IEnumerable<StudentPayments> collection = accessor.Execute(fromDate, toDate, fundTypeId, billCounterTypeId);
        //        collectionhistoryList = collection.ToList();
        //    }

        //    catch (Exception ex)
        //    {
        //        return collectionhistoryList;
        //    }

        //    return collectionhistoryList;
        //}       

        //private IRowMapper<RStudentWiseCollection> GetStudentPaymentByDateRangeMaper()
        //{
        //    IRowMapper<RStudentWiseCollection> mapper = MapBuilder<RStudentWiseCollection>.MapAllProperties()

        //    .Map(m => m.StudentId).ToColumn("StudentId")
        //    .Map(m => m.ProgramID).ToColumn("ProgramID")
        //    .Map(m => m.ProgramName).ToColumn("ProgramName")
        //    .Map(m => m.DepartmentID).ToColumn("DeptID")
        //    .Map(m => m.DepartmentName).ToColumn("DepartmentName")
        //    .Map(m => m.Roll).ToColumn("Roll")
        //    .Map(m => m.StudentName).ToColumn("StudentName")

        //    .Map(m => m.InvoiceNo).ToColumn("InvoiceNo")
        //    .Map(m => m.Amount).ToColumn("Amount")
        //    .Map(m => m.FundName).ToColumn("FundName")
        //    .Map(m => m.CounterShortName).ToColumn("CounterShortName")
        //    .Map(m => m.CollectionDate).ToColumn("CollectionDate")

        //    .DoNotMap(m => m.ReferenceNo)
        //    .DoNotMap(m => m.Fees)
        //    .DoNotMap(m => m.BankSl)
        //    .DoNotMap(m => m.YearName)
        //    .DoNotMap(m => m.YearId)
        //    .DoNotMap(m => m.FeeName)
        //    .Build();

        //    return mapper;
        //}

        //private IRowMapper<RStudentWiseCollection> GetStudentPaymentDetailMapper()
        //{
        //    IRowMapper<RStudentWiseCollection> mapper = MapBuilder<RStudentWiseCollection>.MapAllProperties()

        //    .Map(m => m.StudentId).ToColumn("StudentId")
        //    .Map(m => m.ProgramID).ToColumn("ProgramID")
        //    .Map(m => m.ProgramName).ToColumn("ProgramName")
        //    .Map(m => m.DepartmentID).ToColumn("DeptID")
        //    .Map(m => m.DepartmentName).ToColumn("DepartmentName")
        //    .Map(m => m.Roll).ToColumn("Roll")
        //    .Map(m => m.StudentName).ToColumn("StudentName")
        //    .Map(m => m.InvoiceNo).ToColumn("InvoiceNo")
        //    .Map(m => m.Amount).ToColumn("Amount")
        //    .Map(m => m.FundName).ToColumn("FundName")
        //    .Map(m => m.CounterShortName).ToColumn("CounterShortName")

        //    .Map(m => m.FeeName).ToColumn("FeeName")
        //    .DoNotMap(m => m.ReferenceNo)
        //    .DoNotMap(m => m.Fees)
        //    .DoNotMap(m => m.BankSl)
        //    .DoNotMap(m => m.YearName)
        //    .DoNotMap(m => m.YearId)
        //    .DoNotMap(m => m.CollectionDate)
        //    .Build();

        //    return mapper;
        //}
        //private IRowMapper<RStudentWiseCollection> GetStudentPaymentSummaryTbMapper()
        //{
        //    IRowMapper<RStudentWiseCollection> mapper = MapBuilder<RStudentWiseCollection>.MapAllProperties()

        //    .Map(m => m.StudentId).ToColumn("StudentId")
        //    .Map(m => m.Amount).ToColumn("Amount")
        //    .Map(m => m.CounterShortName).ToColumn("CounterShortName")

        //    .DoNotMap(m => m.FeeName)
        //    .DoNotMap(m => m.FundName)
        //    .DoNotMap(m => m.ProgramID)
        //    .DoNotMap(m => m.ProgramName)
        //    .DoNotMap(m => m.DepartmentID)
        //    .DoNotMap(m => m.DepartmentName)
        //    .DoNotMap(m => m.Roll)
        //    .DoNotMap(m => m.StudentName)
        //    .DoNotMap(m => m.InvoiceNo)
        //    .DoNotMap(m => m.ReferenceNo)
        //    .DoNotMap(m => m.Fees)
        //    .DoNotMap(m => m.BankSl)
        //    .DoNotMap(m => m.YearName)
        //    .DoNotMap(m => m.YearId)
        //    .DoNotMap(m => m.CollectionDate)
        //    .Build();

        //    return mapper;
        //}
    }
}
