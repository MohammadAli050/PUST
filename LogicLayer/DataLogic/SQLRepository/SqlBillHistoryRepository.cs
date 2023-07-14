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
//using LogicLayer.BusinessObjects.RO;

namespace LogicLayer.DataLogic.SQLRepository
{
    public partial class SqlBillHistoryRepository : IBillHistoryRepository
    {

        Database db = null;
        private string sqlGetBillPaymentHistoryBillHistoryMasterId = "BillPaymentHistoryByStudentId";
        private string sqlGetStudentBillPaymentDueByProgramIdSessionId = "RptStudentBillPaymentDueByProgramIdSessionId";
        private string sqlGetBillForDelete = "BillHistoryMasterByProgramBatchSessionDateStudentId";
        private string sqlGetBillPaymentHistoryMasterByStudentId = "BillHistoryMasterForStudentBillHistory";
        private string sqlGetBillHistoryByStudentId = "RptBillHistoryByStudentId";
        private string sqlGetHeadWiseStudentDiscount = "GetHeadWiseStudentDiscountByProgramSessionId";
        private string sqlGetStudentDiscountEligibility = "RptStudentDiscountEligibilityCheckByProgramSession";



        public int Insert(BillHistory billHistory)
        {
            int id = 0;
            bool isInsert = true;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand("BillHistoryInsert");

                db = addParam(db, cmd, billHistory, isInsert);
                db.ExecuteNonQuery(cmd);

                object obj = db.GetParameterValue(cmd, "BillHistoryId");

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

        public bool Update(BillHistory billHistory)
        {
            bool result = false;
            bool isInsert = false;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand("BillHistoryUpdate");

                db = addParam(db, cmd, billHistory, isInsert);

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
                DbCommand cmd = db.GetStoredProcCommand("BillHistoryDeleteById");

                db.AddInParameter(cmd, "BillHistoryId", DbType.Int32, id);
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

        public BillHistory GetById(int? id)
        {
            BillHistory _billhistory = null;
            try
            {

                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<BillHistory> rowMapper = GetBillHistoryMaper();

                var accessor = db.CreateSprocAccessor<BillHistory>("BillHistoryGetById", rowMapper);
                _billhistory = accessor.Execute(id).SingleOrDefault();

            }
            catch (Exception ex)
            {
                return _billhistory;
            }

            return _billhistory;
        }

        public List<BillHistory> GetAll()
        {
            List<BillHistory> billhistoryList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<BillHistory> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<BillHistory>("BillHistoryGetAll", mapper);
                IEnumerable<BillHistory> collection = accessor.Execute();

                billhistoryList = collection.ToList();
            }

            catch (Exception ex)
            {
                return billhistoryList;
            }

            return billhistoryList;
        }

        public BillHistory GetByStudentCourseHistoryId(int courseHistoryId)
        {
            BillHistory _billhistory = null;
            try
            {

                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<BillHistory> rowMapper = GetMaperNewForCourse();

                var accessor = db.CreateSprocAccessor<BillHistory>("BillHistoryByCourseHistoryId", rowMapper);
                _billhistory = accessor.Execute(courseHistoryId).FirstOrDefault();

            }
            catch (Exception ex)
            {
                return _billhistory;
            }

            return _billhistory;
        }

        public List<StudentBillDetailsDTO> GetStudentBillingDetails(int studentId, int programId, int studentAdmissionAcaCalId, int acaCalId)
        {
            List<StudentBillDetailsDTO> studentCourseHistoryList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<StudentBillDetailsDTO> mapper = GetStudentBillMaper();

                var accessor = db.CreateSprocAccessor<StudentBillDetailsDTO>("GetStudentBillingDetails", mapper);
                IEnumerable<StudentBillDetailsDTO> collection = accessor.Execute(studentId, programId, studentAdmissionAcaCalId, acaCalId);

                studentCourseHistoryList = collection.ToList();
            }

            catch (Exception ex)
            {
                return studentCourseHistoryList;
            }

            return studentCourseHistoryList;
        }

        public List<StudentBillDetailsDTO> GetStudentBillingDetailsByGroup(int studentId, int programId, int studentAdmissionAcaCalId, int acaCalId, int feeGroupMasterId)
        {
            List<StudentBillDetailsDTO> studentCourseHistoryList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<StudentBillDetailsDTO> mapper = GetStudentBillByGroupMapperMapper();

                var accessor = db.CreateSprocAccessor<StudentBillDetailsDTO>("GetStudentBillingDetailsByGroup", mapper);
                IEnumerable<StudentBillDetailsDTO> collection = accessor.Execute(studentId, programId, studentAdmissionAcaCalId, acaCalId, feeGroupMasterId);

                studentCourseHistoryList = collection.ToList();
            }

            catch (Exception ex)
            {
                return studentCourseHistoryList;
            }

            return studentCourseHistoryList;
        }

        public List<StudentBillDetailsDTO> GetNcBillCalculationByCampusIdProgramIdAcaCalIdAcaCalSectionId(int? campusId, int? programId, int? acaCalId, int? acaCalSectionId)
        {
            List<StudentBillDetailsDTO> studentCourseHistoryList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<StudentBillDetailsDTO> mapper = GetStudentBillMaper();

                var accessor = db.CreateSprocAccessor<StudentBillDetailsDTO>("GetNCBillCalculationByCampusIdProgramIdAcaCalIdAcaCalSectionId", mapper);
                IEnumerable<StudentBillDetailsDTO> collection = accessor.Execute(campusId, programId, acaCalId, acaCalSectionId);

                studentCourseHistoryList = collection.ToList();
            }

            catch (Exception ex)
            {
                return studentCourseHistoryList;
            }

            return studentCourseHistoryList;
        }


        public List<StudentBillDetailsDTO> GetStudentShortSupplementaryBillingDetailsByStudentIdSessionIdCalenderUnitTypeId(int studentId, int acaCalId, int calenderUnitTypeId)
        {
            List<StudentBillDetailsDTO> studentBillingDetails = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<StudentBillDetailsDTO> mapper = GetStudentBillMaper();

                var accessor = db.CreateSprocAccessor<StudentBillDetailsDTO>("GetStudentShortSupplementaryBillingDetailsByStudentIdSessionIdCalenderUnitTypeId", mapper);
                IEnumerable<StudentBillDetailsDTO> collection = accessor.Execute(studentId, acaCalId, calenderUnitTypeId);

                studentBillingDetails = collection.ToList();
            }

            catch (Exception ex)
            {
                return studentBillingDetails;
            }

            return studentBillingDetails;
        }

        public List<StudentBillDetailsDTO> GenerateStudentAnnualBillingDetails(int studentId, int programId, int batchId, int acaCalId)
        {
            List<StudentBillDetailsDTO> studentCourseHistoryList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<StudentBillDetailsDTO> mapper = GetStudentBillMaper();

                var accessor = db.CreateSprocAccessor<StudentBillDetailsDTO>("GenerateStudentOthersBillingDetails", mapper);
                IEnumerable<StudentBillDetailsDTO> collection = accessor.Execute(studentId, programId, batchId, acaCalId);

                studentCourseHistoryList = collection.ToList();
            }

            catch (Exception ex)
            {
                return studentCourseHistoryList;
            }

            return studentCourseHistoryList;
        }

        private IRowMapper<StudentBillDetailsDTO> GetStudentBillMaper()
        {
            IRowMapper<StudentBillDetailsDTO> mapper = MapBuilder<StudentBillDetailsDTO>.MapAllProperties()
            .Map(m => m.Roll).ToColumn("Roll")
            .Map(m => m.StudentCourseHistoryId).ToColumn("StudentCourseHistoryId")
            .Map(m => m.StudentID).ToColumn("StudentID")
            .Map(m => m.TypeDefinitionID).ToColumn("TypeDefinitionID")
            .Map(m => m.AcaCalId).ToColumn("AcaCalId")
            .Map(m => m.Amount).ToColumn("Amount")
            .Map(m => m.Remark).ToColumn("Remark")
            .Map(m => m.BillingDate).ToColumn("BillingDate")
            .Map(m => m.FormalCode).ToColumn("FormalCode")
            .Map(m => m.Credits).ToColumn("Credits")
            .Map(m => m.FeeSetupId).ToColumn("FeeSetupId")
            .Build();

            return mapper;
        }

        private IRowMapper<StudentBillDetailsDTO> GetStudentBillByGroupMapperMapper()
        {
            IRowMapper<StudentBillDetailsDTO> mapper = MapBuilder<StudentBillDetailsDTO>.MapAllProperties()
                .Map(m => m.Roll).ToColumn("Roll")
                .Map(m => m.StudentCourseHistoryId).ToColumn("StudentCourseHistoryId")
                .Map(m => m.StudentID).ToColumn("StudentID")
                .Map(m => m.TypeDefinitionID).ToColumn("TypeDefinitionID")
                .Map(m => m.AcaCalId).ToColumn("AcaCalId")
                .Map(m => m.Amount).ToColumn("Amount")
                .Map(m => m.Remark).ToColumn("Remark")
                .Map(m => m.BillingDate).ToColumn("BillingDate")
                .DoNotMap(m => m.FormalCode)
                .DoNotMap(m => m.Credits)
                .Map(m => m.FeeSetupId).ToColumn("FeeSetupId")
                .Build();

            return mapper;
        }

        public BillHistory GetStudentBillHistory(int studentCourseHistoryId, int studentId, int acaCalId, int typeDefinationId, decimal fee)
        {
            BillHistory _billhistory = null;
            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                IRowMapper<BillHistory> rowMapper = GetMaper();
                var accessor = db.CreateSprocAccessor<BillHistory>("BillHistoryByStudentAcacalHistoryTypeId", rowMapper);
                _billhistory = accessor.Execute(studentCourseHistoryId, studentId, acaCalId, typeDefinationId, fee).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return _billhistory;
            }

            return _billhistory;
        }

        public List<BillHistory> GetBillForPrintByBillHistoryMasterId(int billHistoryMasterId)
        {
            List<BillHistory> billhistoryList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<BillHistory> mapper = GetMaper();

                var accessor = db.CreateSprocAccessor<BillHistory>("GetBillForPrintByBillHistoryMasterId", mapper);
                IEnumerable<BillHistory> collection = accessor.Execute(billHistoryMasterId);

                billhistoryList = collection.ToList();
            }

            catch (Exception ex)
            {
                return billhistoryList;
            }

            return billhistoryList;
        }

        public List<BillHistory> GetBillHistoryByBillHistoryMasterId(int billHistorymasterId)
        {
            List<BillHistory> billhistoryList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<BillHistory> mapper = GetBillHistoryMaper();

                var accessor = db.CreateSprocAccessor<BillHistory>("GetBillHistoryByBillHistoryMasterId", mapper);
                IEnumerable<BillHistory> collection = accessor.Execute(billHistorymasterId);

                billhistoryList = collection.ToList();
            }

            catch (Exception ex)
            {
                return billhistoryList;
            }

            return billhistoryList;
        }

        public bool DeleteByBillHistoryMasterId(int billHistoryMasterId)
        {
            bool result = false;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();
                DbCommand cmd = db.GetStoredProcCommand("BillHistoryDeleteByBillHistoryMasterId");

                db.AddInParameter(cmd, "BillHistoryMasterId", DbType.Int32, billHistoryMasterId);
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

        public BillHistory GetStudentDiscountHistoryByStudentIdAcacalId(int studentId, int acaCalId, int typeDefId)
        {
            BillHistory _billhistory = null;
            try
            {

                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<BillHistory> rowMapper = GetMaper();

                var accessor = db.CreateSprocAccessor<BillHistory>("GetDiscountHistoryByStudentIdAcacalId", rowMapper);
                _billhistory = accessor.Execute(studentId, acaCalId, typeDefId).FirstOrDefault();

            }
            catch (Exception ex)
            {
                return _billhistory;
            }

            return _billhistory;
        }

        //public List<rHeadWiseStudentDiscount> GetHeadWiseStudentDiscount(int programId, int sessionId, int discountTypeId)
        //{
        //    List<rHeadWiseStudentDiscount> headWiseStudentDiscountList = null;

        //    try
        //    {
        //        db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

        //        IRowMapper<rHeadWiseStudentDiscount> mapper = MapBuilder<rHeadWiseStudentDiscount>.MapAllProperties()
        //        .Map(m => m.StudentId).ToColumn("StudentId")
        //        .Map(m => m.Name).ToColumn("Name")
        //        .Map(m => m.Gender).ToColumn("Gender")
        //        .Map(m => m.ProgramName).ToColumn("ProgramName")
        //        .Map(m => m.DiscountType).ToColumn("DiscountType")
        //        .Map(m => m.DiscountAmount).ToColumn("DiscountAmount")
        //        .Map(m => m.Percentage).ToColumn("Percentage")
        //        .Map(m => m.EquivalentPercentage).ToColumn("EquivalentPercentage")
        //        .Map(m => m.Remarks).ToColumn("Remarks")

        //        .Build();

        //        var accessor = db.CreateSprocAccessor<rHeadWiseStudentDiscount>(sqlGetHeadWiseStudentDiscount, mapper);
        //        IEnumerable<rHeadWiseStudentDiscount> collection = accessor.Execute(programId, sessionId, discountTypeId);

        //        headWiseStudentDiscountList = collection.ToList();
        //    }

        //    catch (Exception ex)
        //    {
        //        return headWiseStudentDiscountList;
        //    }

        //    return headWiseStudentDiscountList;
        //}

        //public List<PaymentPostingDTO> GetBillForPaymentPosting(int programId, int sessionId, int batchId, int studentId)
        //{
        //    List<PaymentPostingDTO> paymentPostingList = null;

        //    try
        //    {
        //        db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

        //        IRowMapper<PaymentPostingDTO> mapper = GetPaymentPostingMaper();

        //        var accessor = db.CreateSprocAccessor<PaymentPostingDTO>("BillHistoryForPaymentPosting2", mapper);
        //        IEnumerable<PaymentPostingDTO> collection = accessor.Execute(programId, sessionId, batchId, studentId);

        //        paymentPostingList = collection.ToList();
        //    }

        //    catch (Exception ex)
        //    {
        //        return paymentPostingList;
        //    }

        //    return paymentPostingList;
        //}

        //private IRowMapper<PaymentPostingDTO> GetPaymentPostingMaper()
        //{
        //    IRowMapper<PaymentPostingDTO> mapper = MapBuilder<PaymentPostingDTO>.MapAllProperties()

        //    .Map(m => m.BillHistoryMasterId).ToColumn("BillHistoryMasterId")
        //    .Map(m => m.StudentId).ToColumn("StudentId")
        //    .Map(m => m.Roll).ToColumn("Roll")
        //    .Map(m => m.Name).ToColumn("Name")
        //    .Map(m => m.ReferenceNo).ToColumn("ReferenceNo")
        //    .Map(m => m.Amount).ToColumn("Amount")
        //    .Map(m => m.BillingDate).ToColumn("BillingDate")
        //    .Map(m => m.CollectionHistoryId).ToColumn("CollectionHistoryId")

        //    .Build();

        //    return mapper;
        //}

        public List<BillPaymentHistoryDTO> GetBillPaymentHistoryByBillHistoryMasterId(int billHistoryMasterId)
        {
            List<BillPaymentHistoryDTO> billPaymentHistoryList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<BillPaymentHistoryDTO> mapper = GetBillPaymentHistoryMaper();

                var accessor = db.CreateSprocAccessor<BillPaymentHistoryDTO>(sqlGetBillPaymentHistoryBillHistoryMasterId, mapper);
                IEnumerable<BillPaymentHistoryDTO> collection = accessor.Execute(billHistoryMasterId);

                billPaymentHistoryList = collection.ToList();
            }

            catch (Exception ex)
            {
                return billPaymentHistoryList;
            }

            return billPaymentHistoryList;
        }

        public List<BillPaymentHistoryMasterDTO> GetBillPaymentHistoryMasterByStudentId(int studentId)
        {
            List<BillPaymentHistoryMasterDTO> billPaymentHistoryMasterList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<BillPaymentHistoryMasterDTO> mapper = GetBillPaymentHistoryMasterMaper();

                var accessor = db.CreateSprocAccessor<BillPaymentHistoryMasterDTO>(sqlGetBillPaymentHistoryMasterByStudentId, mapper);
                IEnumerable<BillPaymentHistoryMasterDTO> collection = accessor.Execute(studentId);

                billPaymentHistoryMasterList = collection.ToList();
            }

            catch (Exception ex)
            {
                return billPaymentHistoryMasterList;
            }

            return billPaymentHistoryMasterList;
        }

        public List<BillHistory> GetBillHistoryByStudentIdReferenceNo(int studentId, string referenceNo)
        {
            List<BillHistory> billHistoryList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<BillHistory> mapper = GetBillPrintMaper();

                var accessor = db.CreateSprocAccessor<BillHistory>("GetBillHistoryByStudentIdReferenceNo", mapper);
                IEnumerable<BillHistory> collection = accessor.Execute(studentId, referenceNo);

                billHistoryList = collection.ToList();
            }

            catch (Exception ex)
            {
                return billHistoryList;
            }

            return billHistoryList;
        }

        private IRowMapper<BillPaymentHistoryDTO> GetBillPaymentHistoryMaper()
        {
            IRowMapper<BillPaymentHistoryDTO> mapper = MapBuilder<BillPaymentHistoryDTO>.MapAllProperties()
            .Map(m => m.StudentId).ToColumn("StudentId")
            .Map(m => m.BillHistoryMasterId).ToColumn("BillHistoryMasterId")
            .Map(m => m.ReferenceNo).ToColumn("ReferenceNo")
            .Map(m => m.BillHistoryId).ToColumn("BillHistoryId")
            .Map(m => m.Semester).ToColumn("Semester")
            .Map(m => m.FeeTypeId).ToColumn("FeeTypeId")
            .Map(m => m.FeesName).ToColumn("FeesName")
            .Map(m => m.BillAmount).ToColumn("BillAmount")
            .Map(m => m.BillingDate).ToColumn("BillingDate")
            .Map(m => m.PaymentAmount).ToColumn("PaymentAmount")
            .Map(m => m.PaymentDate).ToColumn("PaymentDate")


            .Build();

            return mapper;
        }

        #region Mapper
        private Database addParam(Database db, DbCommand cmd, BillHistory billhistory, bool isInsert)
        {
            if (isInsert)
            {
                db.AddOutParameter(cmd, "BillHistoryId", DbType.Int32, Int32.MaxValue);
                db.AddInParameter(cmd, "CreatedBy", DbType.Int32, billhistory.CreatedBy);
                db.AddInParameter(cmd, "CreatedDate", DbType.DateTime, billhistory.CreatedDate);
            }
            else
            {
                db.AddInParameter(cmd, "BillHistoryId", DbType.Int32, billhistory.BillHistoryId);
                db.AddInParameter(cmd, "ModifiedBy", DbType.Int32, billhistory.ModifiedBy);
                db.AddInParameter(cmd, "ModifiedDate", DbType.DateTime, billhistory.ModifiedDate);
            }
            db.AddInParameter(cmd, "BillHistoryMasterId", DbType.Int32, billhistory.BillHistoryMasterId);
            db.AddInParameter(cmd, "CourseHistoryId", DbType.Int32, billhistory.StudentCourseHistoryId);
            db.AddInParameter(cmd, "StudentId", DbType.Int32, billhistory.StudentId);
            db.AddInParameter(cmd, "TypeDefinitionId", DbType.Int32, billhistory.TypeDefinitionId);
            db.AddInParameter(cmd, "FeeSetupId", DbType.Int32, billhistory.FeeSetupId);
            db.AddInParameter(cmd, "AcaCalId", DbType.Int32, billhistory.AcaCalId);
            db.AddInParameter(cmd, "Remark", DbType.String, billhistory.Remark);
            db.AddInParameter(cmd, "Fees", DbType.Decimal, billhistory.Fees);
            db.AddInParameter(cmd, "BillingDate", DbType.DateTime, billhistory.BillingDate);

            db.AddInParameter(cmd, "IsDeleted", DbType.Boolean, billhistory.IsDeleted);

            db.AddInParameter(cmd, "Attribute1", DbType.Boolean, billhistory.Attribute1);
            db.AddInParameter(cmd, "Attribute2", DbType.DateTime, billhistory.Attribute2);
            db.AddInParameter(cmd, "Attribute3", DbType.Int32, billhistory.Attribute3);
            db.AddInParameter(cmd, "Attribute4", DbType.DateTime, billhistory.Attribute4);

            return db;
        }

        private IRowMapper<BillHistory> GetMaper()
        {
            IRowMapper<BillHistory> mapper = MapBuilder<BillHistory>.MapAllProperties()

            .Map(m => m.BillHistoryId).ToColumn("BillHistoryId")
            .Map(m => m.StudentId).ToColumn("StudentId")
            .Map(m => m.StudentCourseHistoryId).ToColumn("CourseHistoryId")
            .Map(m => m.TypeDefinitionId).ToColumn("TypeDefinitionId")
            .Map(m => m.FeeSetupId).ToColumn("FeeSetupId")
            .Map(m => m.AcaCalId).ToColumn("AcaCalId")
            .Map(m => m.Remark).ToColumn("Remark")
            .Map(m => m.Fees).ToColumn("Fees")
            .Map(m => m.BillingDate).ToColumn("BillingDate")
            .Map(m => m.IsDeleted).ToColumn("IsDeleted")
            .Map(m => m.CreatedBy).ToColumn("CreatedBy")
            .Map(m => m.CreatedDate).ToColumn("CreatedDate")
            .Map(m => m.ModifiedBy).ToColumn("ModifiedBy")
            .Map(m => m.ModifiedDate).ToColumn("ModifiedDate")

            .Map(m => m.FundTypeId).ToColumn("FundTypeId")
            .Map(m => m.FundName).ToColumn("FundName")
            .Map(m => m.AccountNo).ToColumn("AccountNo")

            .Map(m => m.BillHistoryMasterId).ToColumn("BillHistoryMasterId")
            .DoNotMap(m => m.Attribute1)
            .DoNotMap(m => m.Attribute2)
            .DoNotMap(m => m.Attribute3)
            .DoNotMap(m => m.Attribute4)
            .Build();

            return mapper;
        }

        private IRowMapper<BillHistory> GetMaperNewForCourse()
        {
            IRowMapper<BillHistory> mapper = MapBuilder<BillHistory>.MapAllProperties()

            .Map(m => m.BillHistoryId).ToColumn("BillHistoryId")
            .Map(m => m.StudentId).ToColumn("StudentId")
            .Map(m => m.StudentCourseHistoryId).ToColumn("CourseHistoryId")
            .Map(m => m.TypeDefinitionId).ToColumn("TypeDefinitionId")
            .Map(m => m.FeeSetupId).ToColumn("FeeSetupId")
            .Map(m => m.AcaCalId).ToColumn("AcaCalId")
            .Map(m => m.Remark).ToColumn("Remark")
            .Map(m => m.Fees).ToColumn("Fees")
            .Map(m => m.BillingDate).ToColumn("BillingDate")
            .Map(m => m.IsDeleted).ToColumn("IsDeleted")
            .Map(m => m.CreatedBy).ToColumn("CreatedBy")
            .Map(m => m.CreatedDate).ToColumn("CreatedDate")
            .Map(m => m.ModifiedBy).ToColumn("ModifiedBy")
            .Map(m => m.ModifiedDate).ToColumn("ModifiedDate")

            .DoNotMap(m => m.FundTypeId)
            .DoNotMap(m => m.FundName)
            .DoNotMap(m => m.AccountNo)

            .Map(m => m.BillHistoryMasterId).ToColumn("BillHistoryMasterId")
            .DoNotMap(m => m.Attribute1)
            .DoNotMap(m => m.Attribute2)
            .DoNotMap(m => m.Attribute3)
            .DoNotMap(m => m.Attribute4)
            .Build();

            return mapper;
        }

        private IRowMapper<BillHistory> GetBillHistoryMaper()
        {
            IRowMapper<BillHistory> mapper = MapBuilder<BillHistory>.MapAllProperties()

            .Map(m => m.BillHistoryId).ToColumn("BillHistoryId")
            .Map(m => m.StudentId).ToColumn("StudentId")
            .Map(m => m.StudentCourseHistoryId).ToColumn("CourseHistoryId")
            .Map(m => m.TypeDefinitionId).ToColumn("TypeDefinitionId")
            .Map(m => m.FeeSetupId).ToColumn("FeeSetupId")
            .Map(m => m.AcaCalId).ToColumn("AcaCalId")
            .Map(m => m.Remark).ToColumn("Remark")
            .Map(m => m.Fees).ToColumn("Fees")
            .Map(m => m.BillingDate).ToColumn("BillingDate")
            .Map(m => m.IsDeleted).ToColumn("IsDeleted")
            .Map(m => m.BillHistoryMasterId).ToColumn("BillHistoryMasterId")
            .Map(m => m.Attribute1).ToColumn("Attribute1")
            .Map(m => m.Attribute2).ToColumn("Attribute2")
            .Map(m => m.Attribute3).ToColumn("Attribute3")
            .Map(m => m.Attribute4).ToColumn("Attribute4")
            .Map(m => m.CreatedBy).ToColumn("CreatedBy")
            .Map(m => m.CreatedDate).ToColumn("CreatedDate")
            .Map(m => m.ModifiedBy).ToColumn("ModifiedBy")
            .Map(m => m.ModifiedDate).ToColumn("ModifiedDate")

            .DoNotMap(m => m.FundTypeId)
            .DoNotMap(m => m.FundName)
            .DoNotMap(m => m.AccountNo)


            .Build();

            return mapper;
        }

        private IRowMapper<BillHistory> GetBillPrintMaper()
        {
            IRowMapper<BillHistory> mapper = MapBuilder<BillHistory>.MapAllProperties()

            .Map(m => m.BillHistoryId).ToColumn("BillHistoryId")
            .Map(m => m.BillHistoryMasterId).ToColumn("BillHistoryMasterId")
            .Map(m => m.StudentCourseHistoryId).ToColumn("CourseHistoryId")
            .Map(m => m.StudentId).ToColumn("StudentId")
            .Map(m => m.TypeDefinitionId).ToColumn("TypeDefinitionId")
            .Map(m => m.AcaCalId).ToColumn("AcaCalId")
                //.Map(m => m.FeeTypeId).ToColumn("FeeTypeId")
            .Map(m => m.Fees).ToColumn("Fees")
            .Map(m => m.Remark).ToColumn("Remark")
            .Map(m => m.BillingDate).ToColumn("BillingDate")
            .Map(m => m.IsDeleted).ToColumn("IsDeleted")
            .Map(m => m.Attribute1).ToColumn("Attribute1")
            .Map(m => m.Attribute2).ToColumn("Attribute2")
            .Map(m => m.Attribute3).ToColumn("Attribute3")
            .Map(m => m.Attribute4).ToColumn("Attribute4")
            .Map(m => m.CreatedBy).ToColumn("CreatedBy")
            .Map(m => m.CreatedDate).ToColumn("CreatedDate")
            .Map(m => m.ModifiedBy).ToColumn("ModifiedBy")
            .Map(m => m.ModifiedDate).ToColumn("ModifiedDate")
                //.Map(m => m.TypeName).ToColumn("Definition")

            .DoNotMap(m => m.FeeSetupId)
            .DoNotMap(m => m.FundTypeId)
            .DoNotMap(m => m.FundName)
            .DoNotMap(m => m.AccountNo)
                //.DoNotMap(m => m.TypeName)
                //.Map(m => m.FundAccountNo).ToColumn("FundAccountNo")
            .Build();

            return mapper;
        }

        //private IRowMapper<StudentBillPaymentDueViewObject> GetStudentBillPaymentDueMaper()
        //{
        //    IRowMapper<StudentBillPaymentDueViewObject> mapper = MapBuilder<StudentBillPaymentDueViewObject>.MapAllProperties()

        //        .Map(m => m.ProgramId).ToColumn("ProgramId")
        //        .Map(m => m.BatchId).ToColumn("BatchId")
        //        .Map(m => m.StudentId).ToColumn("StudentId")
        //        .Map(m => m.Roll).ToColumn("Roll")
        //        .Map(m => m.StudentName).ToColumn("StudentName")
        //        .Map(m => m.Bill).ToColumn("Bill")
        //        .Map(m => m.Payment).ToColumn("Payment")
        //        .Map(m => m.Due).ToColumn("Due")
        //        .Map(m => m.BillingDate).ToColumn("BillingDate")
        //        .Map(m => m.CollectionDate).ToColumn("CollectionDate")
        //        .Map(m => m.BillHistoryMasterId).ToColumn("BillHistoryMasterId")
        //        .Build();

        //    return mapper;
        //}

        //private IRowMapper<StudentBillPaymentDueViewObject> GetStudentBillPaymentDueSummeryMapper()
        //{
        //    IRowMapper<StudentBillPaymentDueViewObject> mapper = MapBuilder<StudentBillPaymentDueViewObject>.MapAllProperties()

        //        .Map(m => m.ProgramId).ToColumn("ProgramId")
        //        .Map(m => m.BatchId).ToColumn("BatchId")
        //        .Map(m => m.StudentId).ToColumn("StudentId")
        //        .Map(m => m.Roll).ToColumn("Roll")
        //        .Map(m => m.StudentName).ToColumn("StudentName")
        //        .Map(m => m.Bill).ToColumn("Bill")
        //        .Map(m => m.Payment).ToColumn("Payment")
        //        .Map(m => m.Due).ToColumn("Due")
        //        .DoNotMap(m => m.BillHistoryMasterId)
        //        .DoNotMap(m => m.BillingDate)
        //        .DoNotMap(m => m.CollectionDate)
        //        .Build();
        //    return mapper;
        //}


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

        private IRowMapper<BillPaymentHistoryMasterDTO> GetBillPaymentHistoryMasterMaper()
        {
            IRowMapper<BillPaymentHistoryMasterDTO> mapper = MapBuilder<BillPaymentHistoryMasterDTO>.MapAllProperties()
                .Map(m => m.BillHistoryMasterId).ToColumn("BillHistoryMasterId")
                .Map(m => m.Roll).ToColumn("Roll")
                .Map(m => m.StudentId).ToColumn("StudentId")
                .Map(m => m.Code).ToColumn("Code")
                .Map(m => m.ReferenceNo).ToColumn("ReferenceNo")
                .Map(m => m.BillingDate).ToColumn("BillingDate")
                .Map(m => m.CollectionDate).ToColumn("CollectionDate")
                .Map(m => m.BillAmount).ToColumn("BillAmount")
                .Map(m => m.PaymentAmount).ToColumn("PaymentAmount")
                .Map(m => m.AcaCalId).ToColumn("AcaCalId")

                .Build();

            return mapper;
        }

        #endregion

        //public List<StudentBillPaymentDueViewObject> GetBillPaymentDueByProgramIdBatchIdSessionId(int? programId, int? batchId, int? sessionId)
        //{
        //    List<StudentBillPaymentDueViewObject> billPaymentHistoryList = null;

        //    try
        //    {
        //        db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

        //        IRowMapper<StudentBillPaymentDueViewObject> mapper = GetStudentBillPaymentDueMaper();

        //        var accessor = db.CreateSprocAccessor<StudentBillPaymentDueViewObject>("GetBillPaymentDueByProgramIdBatchIdSessionId", mapper);
        //        IEnumerable<StudentBillPaymentDueViewObject> collection = accessor.Execute(programId, batchId, sessionId);

        //        billPaymentHistoryList = collection.ToList();
        //    }

        //    catch (Exception ex)
        //    {
        //        return billPaymentHistoryList;
        //    }

        //    return billPaymentHistoryList;
        //}

        //public List<StudentBillPaymentDueViewObject> GetBillPaymentDueSummeryBySessionId(int? sessionId)
        //{
        //    List<StudentBillPaymentDueViewObject> billPaymentHistoryList = null;

        //    try
        //    {
        //        db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

        //        IRowMapper<StudentBillPaymentDueViewObject> mapper = GetStudentBillPaymentDueSummeryMapper();

        //        var accessor = db.CreateSprocAccessor<StudentBillPaymentDueViewObject>("GetBillPaymentDueSummeryBySessionId", mapper);
        //        IEnumerable<StudentBillPaymentDueViewObject> collection = accessor.Execute( sessionId);

        //        billPaymentHistoryList = collection.ToList();
        //    }

        //    catch (Exception ex)
        //    {
        //        return billPaymentHistoryList;
        //    }

        //    return billPaymentHistoryList;
        //}



        public List<BillDeleteDTO> GetBillForDelete(int programId, int batchId, int sessionId, int studentId, DateTime? date)
        {
            List<BillDeleteDTO> billDeleteList = null;

            try
            {
                db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

                IRowMapper<BillDeleteDTO> mapper = GetBillForDeleteMaper();

                var accessor = db.CreateSprocAccessor<BillDeleteDTO>(sqlGetBillForDelete, mapper);
                IEnumerable<BillDeleteDTO> collection = accessor.Execute(programId, batchId, sessionId, studentId, date);

                billDeleteList = collection.ToList();
            }

            catch (Exception ex)
            {
                return billDeleteList;
            }

            return billDeleteList;
        }



        //public List<RegistrationBill> GetRegistrationBillHistoryByStudentIdAcaCalId(int studentId, int AcaCalId)
        //{
        //    List<RegistrationBill> billhistoryList = null;

        //    try
        //    {
        //        db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

        //        IRowMapper<RegistrationBill> mapper = MapBuilder<RegistrationBill>.MapAllProperties()
        //        .Map(m => m.StudentId).ToColumn("StudentId")
        //        .Map(m => m.TutionFee).ToColumn("TutionFee")
        //        .Map(m => m.Credits).ToColumn("Credits")
        //        .Map(m => m.LibraryDeposit).ToColumn("LibraryFee")
        //        .Map(m => m.OtherCharge).ToColumn("OthersCharges")
        //        .Map(m => m.Arrear).ToColumn("Arrear")
        //        .Map(m => m.Advance).ToColumn("Advance")
        //        .Map(m => m.LateFee).ToColumn("LateFee")
        //        .Map(m => m.GrandTotal).ToColumn("GrandTotal")
        //        .Map(m => m.FirstInstallment).ToColumn("FirstInstallment")

        //        .Build();

        //        var accessor = db.CreateSprocAccessor<RegistrationBill>("RegistrationBillHistoryForPrintOut", mapper);
        //        IEnumerable<RegistrationBill> collection = accessor.Execute(studentId, AcaCalId);

        //        billhistoryList = collection.ToList();
        //    }

        //    catch (Exception ex)
        //    {
        //        return billhistoryList;
        //    }

        //    return billhistoryList;
        //}

        //public List<StudentBillHistoryDTO> GetStudentBillHistoryByStudentId(int studentId)
        //{
        //    List<StudentBillHistoryDTO> studentBillHistoryList = null;

        //    try
        //    {
        //        db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

        //        IRowMapper<StudentBillHistoryDTO> mapper = GetStudentBillHistoryDTOMaper();

        //        var accessor = db.CreateSprocAccessor<StudentBillHistoryDTO>("BillHistoryByStudentId", mapper);
        //        IEnumerable<StudentBillHistoryDTO> collection = accessor.Execute(studentId);

        //        studentBillHistoryList = collection.ToList();
        //    }

        //    catch (Exception ex)
        //    {
        //        return studentBillHistoryList;
        //    }

        //    return studentBillHistoryList;
        //}

        //private IRowMapper<StudentBillHistoryDTO> GetStudentBillHistoryDTOMaper()
        //{
        //    IRowMapper<StudentBillHistoryDTO> mapper = MapBuilder<StudentBillHistoryDTO>.MapAllProperties()
        //    .Map(m => m.BillHistoryId).ToColumn("BillHistoryId")
        //    .Map(m => m.StudentCourseHistoryId).ToColumn("StudentCourseHistoryId")
        //    .Map(m => m.StudentId).ToColumn("StudentId")
        //    .Map(m => m.TypeDefinationId).ToColumn("TypeDefinationId")
        //    .Map(m => m.AcaCalId).ToColumn("AcaCalId")
        //    .Map(m => m.Fees).ToColumn("Fees")
        //    .Map(m => m.Remark).ToColumn("Remark")
        //    .Map(m => m.BillingDate).ToColumn("BillingDate")
        //    .Map(m => m.IsDeleted).ToColumn("IsDeleted")
        //    .Map(m => m.CreatedBy).ToColumn("CreatedBy")
        //    .Map(m => m.CreatedDate).ToColumn("CreatedDate")
        //    .Map(m => m.ModifiedBy).ToColumn("ModifiedBy")
        //    .Map(m => m.ModifiedDate).ToColumn("ModifiedDate")
        //    .Map(m => m.CollectionHistoryId).ToColumn("CollectionHistoryId")
        //    .Map(m => m.CourseTitle).ToColumn("Title")
        //    .Map(m => m.FormalCode).ToColumn("FormalCode")
        //    .Map(m => m.Credits).ToColumn("Credits")
        //    .Map(m => m.SemesterName).ToColumn("SemesterName")
        //    .DoNotMap(m => m.DiscountAmount)
        //    .DoNotMap(m => m.Payment)
        //    .Build();

        //    return mapper;
        //}

        //public List<rBillHistoryByStudentID> GetBillHistoryByStudentId(int studentId)
        //{
        //    List<rBillHistoryByStudentID> studentBillHistoryList = null;

        //    try
        //    {
        //        db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

        //        IRowMapper<rBillHistoryByStudentID> mapper = GetrBillHistoryByStudentIDMaper();

        //        var accessor = db.CreateSprocAccessor<rBillHistoryByStudentID>(sqlGetBillHistoryByStudentId, mapper);
        //        IEnumerable<rBillHistoryByStudentID> collection = accessor.Execute(studentId);

        //        studentBillHistoryList = collection.ToList();
        //    }

        //    catch (Exception ex)
        //    {
        //        return studentBillHistoryList;
        //    }

        //    return studentBillHistoryList;
        //}

        //private IRowMapper<rBillHistoryByStudentID> GetrBillHistoryByStudentIDMaper()
        //{
        //    IRowMapper<rBillHistoryByStudentID> mapper = MapBuilder<rBillHistoryByStudentID>.MapAllProperties()
        //    .Map(m => m.BatchCode).ToColumn("BatchCode")
        //    .Map(m => m.BillingDate).ToColumn("BillingDate")
        //    .Map(m => m.Type).ToColumn("Type")
        //    .Map(m => m.Definition).ToColumn("Definition")
        //    .Map(m => m.Fees).ToColumn("Fees")
        //    .Map(m => m.Remark).ToColumn("Remark")
        //    .Map(m => m.Credits).ToColumn("Credits")
        //    .Map(m => m.AcaCalCode).ToColumn("AcaCalCode")
        //    .Build();

        //    return mapper;
        //}

        //public List<rStudentReceivableReceivedDue> GetStudentReceivableReceivedDue(int programId, int semesterId, int studentTypeId)
        //{
        //    List<rStudentReceivableReceivedDue> rStudentReceivableReceivedDueList = null;

        //    try
        //    {
        //        db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

        //        IRowMapper<rStudentReceivableReceivedDue> mapper = GetStudentReceivableReceivedDueMaper();

        //        var accessor = db.CreateSprocAccessor<rStudentReceivableReceivedDue>("RptStudentReceivableReceivedDue", mapper);
        //        IEnumerable<rStudentReceivableReceivedDue> collection = accessor.Execute(programId, semesterId, studentTypeId);

        //        rStudentReceivableReceivedDueList = collection.ToList();
        //    }

        //    catch (Exception ex)
        //    {
        //        return rStudentReceivableReceivedDueList;
        //    }

        //    return rStudentReceivableReceivedDueList;
        //}

        //private IRowMapper<rStudentReceivableReceivedDue> GetStudentReceivableReceivedDueMaper()
        //{
        //    IRowMapper<rStudentReceivableReceivedDue> mapper = MapBuilder<rStudentReceivableReceivedDue>.MapAllProperties()
        //    .Map(m => m.StudentId).ToColumn("StudentId")
        //    .Map(m => m.Roll).ToColumn("Roll")
        //    .Map(m => m.StudentName).ToColumn("StudentName")
        //    .Map(m => m.ProgramName).ToColumn("ProgramName")
        //    .Map(m => m.Semester).ToColumn("Semester")
        //    .Map(m => m.Batch).ToColumn("Batch")
        //    .Map(m => m.ReceivableAmount).ToColumn("ReceivableAmount")
        //    .Map(m => m.RecivedAmount).ToColumn("RecivedAmount")
        //    .Map(m => m.Dues).ToColumn("Dues")
        //    .Build();

        //    return mapper;
        //}

        //public InstallmentBillDTO GetStudentInstallmentBill(int studentId, int acaCalId)
        //{
        //    InstallmentBillDTO studentBillHistory = null;

        //    try
        //    {
        //        db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

        //        IRowMapper<InstallmentBillDTO> mapper = GetInstallmentBillHistoryMaper();

        //        var accessor = db.CreateSprocAccessor<InstallmentBillDTO>("InstallmentBillByStudentIdAcaCalId", mapper);
        //        studentBillHistory = accessor.Execute(studentId, acaCalId).SingleOrDefault();
        //    }

        //    catch (Exception ex)
        //    {
        //        return studentBillHistory;
        //    }

        //    return studentBillHistory;
        //}

        //private IRowMapper<InstallmentBillDTO> GetInstallmentBillHistoryMaper()
        //{
        //    IRowMapper<InstallmentBillDTO> mapper = MapBuilder<InstallmentBillDTO>.MapAllProperties()
        //    .Map(m => m.StudentId).ToColumn("StudentId")
        //    .Map(m => m.CurrentFees).ToColumn("CurrentFees")
        //    .Map(m => m.TotalFees).ToColumn("TotalFees")
        //    .Map(m => m.TotalPaid).ToColumn("TotalPaid")
        //    .Map(m => m.CurrentPaid).ToColumn("CurrentPaid")
        //    .Map(m => m.FInstallment).ToColumn("FInstallment")
        //    .Map(m => m.SInstallment).ToColumn("SInstallment")
        //    .Map(m => m.TInstallment).ToColumn("TInstallment")
        //    .Map(m => m.Arear).ToColumn("Arear")
        //    .Build();

        //    return mapper;
        //}

        //public List<rStudentDiscountEligibility> GetStudentDiscountEligibility(int programId, int sessionId, int resultSessionId, decimal lessAmount)
        //{
        //    List<rStudentDiscountEligibility> studentDiscountEligibilityList = null;

        //    try
        //    {
        //        db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

        //        IRowMapper<rStudentDiscountEligibility> mapper = MapBuilder<rStudentDiscountEligibility>.MapAllProperties()
        //        .Map(m => m.StudentID).ToColumn("StudentID")
        //        .Map(m => m.Roll).ToColumn("Roll")
        //        .Map(m => m.Gender).ToColumn("Gender")
        //        .Map(m => m.StudentName).ToColumn("StudentName")
        //        .Map(m => m.ProgramName).ToColumn("ProgramName")
        //        .Map(m => m.SessionId).ToColumn("SessionId")
        //        .Map(m => m.AdmissionSession).ToColumn("AdmissionSession")
        //        .Map(m => m.RegisteredCredit).ToColumn("RegisteredCredit")
        //        .Map(m => m.TypeDefinitionID).ToColumn("TypeDefinitionID")
        //        .Map(m => m.WaiverType).ToColumn("WaiverType")
        //        .Map(m => m.WaiverPercentage).ToColumn("WaiverPercentage")
        //        .Map(m => m.SSC).ToColumn("SSC")
        //        .Map(m => m.HSC).ToColumn("HSC")
        //        .Map(m => m.OLevel).ToColumn("OLevel")
        //        .Map(m => m.ALevel).ToColumn("ALevel")
        //        .Map(m => m.Diploma).ToColumn("Diploma")
        //        .Map(m => m.Fail).ToColumn("Fail")
        //        .Map(m => m.FailCourseCount).ToColumn("FailCourseCount")
        //        .Map(m => m.Incomplete).ToColumn("Incomplete")
        //        .Map(m => m.IncompleteCourseCount).ToColumn("IncompleteCourseCount")
        //        .Map(m => m.Nil).ToColumn("Nil")
        //        .Map(m => m.NilCourseCount).ToColumn("NilCourseCount")
        //        .Map(m => m.SemesterBreak).ToColumn("SemesterBreak")
        //        .Map(m => m.PreviousTrimesterCourseCountWithOutDrop).ToColumn("PreviousTrimesterCourseCountWithOutDrop")
        //        .Map(m => m.SemesterGPA).ToColumn("SemesterGPA")
        //        .Map(m => m.PreviousTrimesterDueAmount).ToColumn("PreviousTrimesterDueAmount")
        //        .Map(m => m.Reported).ToColumn("Reported")
        //        .Map(m => m.ReportedCourseCount).ToColumn("ReportedCourseCount")
        //        .Map(m => m.Eligibility).ToColumn("Eligibility")
        //        .Map(m => m.ApprovedWaiverPercentage).ToColumn("ApprovedWaiverPercentage")



        //        .Build();

        //        var accessor = db.CreateSprocAccessor<rStudentDiscountEligibility>(sqlGetStudentDiscountEligibility, mapper);
        //        IEnumerable<rStudentDiscountEligibility> collection = accessor.Execute(programId, sessionId, resultSessionId, lessAmount);

        //        studentDiscountEligibilityList = collection.ToList();
        //    }

        //    catch (Exception ex)
        //    {
        //        return studentDiscountEligibilityList;
        //    }

        //    return studentDiscountEligibilityList;
        //}

        //public List<rCreditsAddAndDrop> GetCreditsAddAndDropReport(int acaCalID, DateTime fromDate, DateTime toDate, int creditType)
        //{
        //    List<rCreditsAddAndDrop> sreditsAddAndDropReportList = null;

        //    try
        //    {
        //        db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

        //        IRowMapper<rCreditsAddAndDrop> mapper = MapBuilder<rCreditsAddAndDrop>.MapAllProperties()

        //        .Map(m => m.StudentID).ToColumn("StudentID")
        //        .Map(m => m.Roll).ToColumn("Roll")
        //        .Map(m => m.FullName).ToColumn("FullName")
        //        .Map(m => m.Department).ToColumn("Department")
        //        .Map(m => m.BatchNO).ToColumn("BatchNO")
        //        .Map(m => m.FirstConfirmCredit).ToColumn("FirstConfirmCredit")
        //        .Map(m => m.BillFCC).ToColumn("BillFCC")
        //        .Map(m => m.LastUpdateCredit).ToColumn("LastUpdateCredit")
        //        .Map(m => m.BillLUC).ToColumn("BillLUC")
        //        .Map(m => m.DeffarentsCredit).ToColumn("DeffarentsCredit")
        //        .Map(m => m.DeffarentsBill).ToColumn("DeffarentsBill")
        //        .Map(m => m.DropCredit).ToColumn("DropCredit")
        //        .Map(m => m.CreditType).ToColumn("CreditType")


        //        .Build();

        //        var accessor = db.CreateSprocAccessor<rCreditsAddAndDrop>("RptCreditsAddAndDropReport", mapper);
        //        IEnumerable<rCreditsAddAndDrop> collection = accessor.Execute(acaCalID, fromDate, toDate, creditType);

        //        sreditsAddAndDropReportList = collection.ToList();
        //    }

        //    catch (Exception ex)
        //    {
        //        return sreditsAddAndDropReportList;
        //    }

        //    return sreditsAddAndDropReportList;
        //}

        //public List<rBillingSummery> GetBillingSummeryBySessionProgramId(int acaCalID, int programID, int billStudentTypeId, int calendarUnitmasterId)
        //{
        //    List<rBillingSummery> billingSummeryList = null;

        //    try
        //    {
        //        db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

        //        IRowMapper<rBillingSummery> mapper = MapBuilder<rBillingSummery>.MapAllProperties()

        //        .Map(m => m.BillingHeads).ToColumn("FeeType")
        //        .Map(m => m.ProgramName).ToColumn("ProgramName")
        //        .Map(m => m.Money).ToColumn("Fees")

        //        .Build();

        //        var accessor = db.CreateSprocAccessor<rBillingSummery>("RptBillingSummeryReportByProgramIdSessionId", mapper);
        //        IEnumerable<rBillingSummery> collection = accessor.Execute(programID, acaCalID, billStudentTypeId, calendarUnitmasterId);

        //        billingSummeryList = collection.ToList();
        //    }

        //    catch (Exception ex)
        //    {
        //        return billingSummeryList;
        //    }

        //    return billingSummeryList;
        //}

        //public List<rDuesOrAdvance> GetDuesOrAdvanceBySessionProgramId(int acaCalID, int programID, int billStudentTypeId, int billDuesTypeId, int calenderUnitMasterId)
        //{
        //    List<rDuesOrAdvance> duesOrAdvanceList = null;

        //    try
        //    {
        //        db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

        //        IRowMapper<rDuesOrAdvance> mapper = MapBuilder<rDuesOrAdvance>.MapAllProperties()

        //        .Map(m => m.StudentId).ToColumn("StudentID")
        //        .Map(m => m.StudentName).ToColumn("StudentName")
        //        .Map(m => m.Department).ToColumn("Department")
        //        .Map(m => m.TotalPayble).ToColumn("Payble")
        //        .Map(m => m.TotalPaid).ToColumn("Paid")
        //        .Map(m => m.NetBalance).ToColumn("NetBalance")

        //        .Build();

        //        var accessor = db.CreateSprocAccessor<rDuesOrAdvance>("RptStudentDuesAdvanceReportByProgramIdSessionId", mapper);
        //        IEnumerable<rDuesOrAdvance> collection = accessor.Execute(programID, acaCalID, billStudentTypeId, billDuesTypeId, calenderUnitMasterId);

        //        duesOrAdvanceList = collection.ToList();
        //    }

        //    catch (Exception ex)
        //    {
        //        return duesOrAdvanceList;
        //    }

        //    return duesOrAdvanceList;
        //}

        //public List<rDuesSummery> GetDuesSummeryByProgramIdSessionId(int programId, int acaCalID, int billStudentTypeId)
        //{
        //    List<rDuesSummery> duesOrAdvanceList = null;

        //    try
        //    {
        //        db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

        //        IRowMapper<rDuesSummery> mapper = MapBuilder<rDuesSummery>.MapAllProperties()

        //        .Map(m => m.DepartmentName).ToColumn("DepartmentName")
        //        .Map(m => m.TotalPayble).ToColumn("TotalPayble")
        //        .Map(m => m.TotalPaid).ToColumn("TotalPaid")
        //        .Map(m => m.Dues).ToColumn("TotalDues")

        //        .Build();

        //        var accessor = db.CreateSprocAccessor<rDuesSummery>("RptDepartmentDueSummeryReportByProgramIdSesionId", mapper);
        //        IEnumerable<rDuesSummery> collection = accessor.Execute(programId, acaCalID, billStudentTypeId);

        //        duesOrAdvanceList = collection.ToList();
        //    }

        //    catch (Exception ex)
        //    {
        //        return duesOrAdvanceList;
        //    }

        //    return duesOrAdvanceList;
        //}

        //public List<rBillingReport> GetBillingReportBySessionProgramIdId(int acaCalID, int programID, int billStudentTypeId, int calenderUnitMasterId)
        //{
        //    List<rBillingReport> billingReportList = null;

        //    try
        //    {
        //        db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

        //        IRowMapper<rBillingReport> mapper = MapBuilder<rBillingReport>.MapAllProperties()


        //        .Map(m => m.StudentId).ToColumn("Roll")
        //        .Map(m => m.StudentName).ToColumn("StudentName")
        //        .Map(m => m.Department).ToColumn("ProgramName")
        //        .Map(m => m.AdmissionFee).ToColumn("AdmissionFee")
        //        .Map(m => m.TutionFee).ToColumn("TutionFee")
        //        .Map(m => m.LabFee).ToColumn("LabFee")
        //        .Map(m => m.LibraryFee).ToColumn("LibraryFee")
        //        .Map(m => m.StudentActivity).ToColumn("StudentActivityFee")
        //        .Map(m => m.LateFee).ToColumn("LateFee")
        //        .Map(m => m.ProvssionalFee).ToColumn("Provisional")
        //        .Map(m => m.TranscriptFee).ToColumn("Transcript")
        //        .Map(m => m.OthersFee).ToColumn("OthersFee")
        //        .Map(m => m.TotalBill).ToColumn("Bill")
        //        .Map(m => m.BankPayment).ToColumn("Payment")
        //        .Map(m => m.WaiverDiscount).ToColumn("Discount")
        //        .Map(m => m.PreviousBill).ToColumn("PreviousBill")
        //        .Map(m => m.PreviousPayment).ToColumn("PreviousPayment")
        //        .Map(m => m.PreviousAdvance).ToColumn("PreviousAdvance")

        //        .Map(m => m.CurrentTrimesterBalance).ToColumn("CurrentTrimesterBalance")
        //        .Map(m => m.TotalCollectionAdjustment).ToColumn("CollectionOrAdjustment")
        //        .Map(m => m.NewBalance).ToColumn("NetBalance")

        //        .Build();

        //        var accessor = db.CreateSprocAccessor<rBillingReport>("RptBillCollectionReportByProgramIdSessionId", mapper);
        //        IEnumerable<rBillingReport> collection = accessor.Execute(programID, acaCalID, billStudentTypeId, calenderUnitMasterId);

        //        billingReportList = collection.ToList();
        //    }

        //    catch (Exception ex)
        //    {
        //        return billingReportList;
        //    }

        //    return billingReportList;
        //}

        //public List<rDiscountSummary> GetStudentDiscountSummaryBySessionId(int sessionId)
        //{
        //    List<rDiscountSummary> billingReportList = null;

        //    try
        //    {
        //        db = EnterpriseLibraryContainer.Current.GetInstance<Database>();

        //        IRowMapper<rDiscountSummary> mapper = MapBuilder<rDiscountSummary>.MapAllProperties()


        //        .Map(m => m.DiscountTypeId).ToColumn("DiscountTypeId")
        //        .Map(m => m.DiscountType).ToColumn("DiscountType")
        //        .Map(m => m.DiscountAmount).ToColumn("DiscountAmount")
        //        .Map(m => m.Percentage).ToColumn("Percentage")
        //        .Map(m => m.StudentNumber).ToColumn("StudentNumber")

        //        .Build();

        //        var accessor = db.CreateSprocAccessor<rDiscountSummary>("RptDiscountSummaryBySessionId", mapper);
        //        IEnumerable<rDiscountSummary> collection = accessor.Execute(sessionId);

        //        billingReportList = collection.ToList();
        //    }

        //    catch (Exception ex)
        //    {
        //        return billingReportList;
        //    }

        //    return billingReportList;
        //}


    }
}

