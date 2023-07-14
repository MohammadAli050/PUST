using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicLayer.BusinessObjects;
using LogicLayer.BusinessObjects.DTO;
//using LogicLayer.BusinessObjects.RO;

namespace LogicLayer.DataLogic.IRepository
{
    public interface ICollectionHistoryRepository
    {
        int Insert(CollectionHistory collectionhistory);

        bool Update(CollectionHistory collectionhistory);

        bool Delete(int collectionHistoryId);

        CollectionHistory GetById(int collectionHistoryId);

        List<CollectionHistory> GetAll();

        CollectionHistory IsDuplicateMoneyReceipt(string moneyReceiptNo, string paymentType);

        List<CollectionHistory> GetByStudentIdFundIdBillHistoryMasterIdLastDate(int studentId, int fundId, int billHistoryMasterId, DateTime lastDate);
        
        List<CollectionHistory> GetByBillHistoryMasterId(int billHistoryMasterId);

        List<BillDeleteDTO> GetBillPaidStudentsByProgramIdSessionIdAdmissionSessionId(int programId, int sessionId, int? admissionSessionId);

        //List<StudentPayments> GetAllCollectionByPaymentTypeIdFundIdAndDateRange_TB(int? fundTypeId, int? billCounterTypeId, DateTime fromDate, DateTime toDate);

        //List<rDepartmentWiseCollection> GetDepartmentWiseByDateRange(DateTime fromDate, DateTime toDate);
        //List<rDepartmentWiseCollection> GetProgramFeeWiseByDateRange(DateTime fromDate, DateTime toDate);
        ////List<rStudentWiseCollection> GetStudentPaymentByDateRange(DateTime fromDate, DateTime toDate);

        //List<RStudentWiseCollection> GetStudentPaymentByPaymentTypeIdFundIdAndDateRange(int? fundTypeId, int? billCounterTypeId, DateTime fromDate, DateTime toDate);
        //List<RStudentWiseCollection> GetStudentPaymentDetailByPaymentTypeIdFundIdAndDateRange(int? fundTypeId, int? billCounterTypeId, DateTime fromDate, DateTime toDate);

        //List<rDepartmentWiseCollection> GetDepartmentWisePaymentCollectionByPaymentTypeIdFundIdAndDateRange(
        //    int? fundTypeId, int? billCounterTypeId, DateTime fromDate, DateTime toDate);
        //List<rDepartmentWiseCollection> GetDepartmentWisePaymentCollectionDetailByPaymentTypeIdFundIdAndDateRange(
        //    int? fundTypeId, int? billCounterTypeId, DateTime fromDate, DateTime toDate);
        //List<FundWiseCollectionViewObject> GetFundWiseTotalCollectionByDateRange(DateTime fromDate, DateTime toDate);
        //List<RStudentWiseCollection> GetStudentPaymentSummaryByPaymentTypeIdFundIdAndDateRange_TB(int? fundTypeId,
        //    int? billCounterTypeId, DateTime fromDate, DateTime toDate);

    }
}
