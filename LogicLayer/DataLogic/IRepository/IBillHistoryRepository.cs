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
    public interface IBillHistoryRepository
    {
        int Insert(BillHistory billHistory);
        
        bool Update(BillHistory billHistory);
        
        bool Delete(int billHistoryId);
        
        BillHistory GetById(int? billHistoryId);
       
        BillHistory GetByStudentCourseHistoryId(int courseHistoryId);
        
        List<BillHistory> GetAll();
        
        List<StudentBillDetailsDTO> GetStudentBillingDetails(int studentId, int programId, int studentAdmissionAcaCalId, int acaCalId);

        List<StudentBillDetailsDTO> GetStudentBillingDetailsByGroup(int studentId, int programId, int studentAdmissionAcaCalId, int acaCalId, int feeGroupMasterId);

        List<StudentBillDetailsDTO> GetNcBillCalculationByCampusIdProgramIdAcaCalIdAcaCalSectionId(int? campusId,
            int? programId, int? acaCalId, int? acaCalSectionId);

        List<StudentBillDetailsDTO> GetStudentShortSupplementaryBillingDetailsByStudentIdSessionIdCalenderUnitTypeId(int studentId, int acaCalId, int calenderUnitTypeId);

        BillHistory GetStudentBillHistory(int studentCourseHistoryId, int studentId, int acaCalId, int typeDefinationId, decimal fee);
        
        List<BillHistory> GetBillForPrintByBillHistoryMasterId(int billHistoryMasterId);
       
        bool DeleteByBillHistoryMasterId(int billHistoryMasterId);
        
        List<BillHistory> GetBillHistoryByBillHistoryMasterId(int billHistorymasterId);
       
        BillHistory GetStudentDiscountHistoryByStudentIdAcacalId(int studentId, int acaCalId, int typeDefId);
        
        List<BillPaymentHistoryMasterDTO> GetBillPaymentHistoryMasterByStudentId(int studentId);
        
        List<BillPaymentHistoryDTO> GetBillPaymentHistoryByBillHistoryMasterId(int billHistoryMasterId);
       
        List<StudentBillDetailsDTO> GenerateStudentAnnualBillingDetails(int studentId, int programId, int batchId,
            int acaCalId);

        List<BillHistory> GetBillHistoryByStudentIdReferenceNo(int studentId, string referenceNo);
        //List<StudentBillPaymentDueViewObject> GetBillPaymentDueByProgramIdBatchIdSessionId(int? programId, int? batchId, int? sessionId);
        //List<StudentBillPaymentDueViewObject> GetBillPaymentDueSummeryBySessionId(int? sessionId);
        //List<BillDeleteDTO> GetBillForDelete(int programId, int batchId, int sessionId, int studentId, DateTime? date);
        //List<PaymentPostingDTO> GetBillForPaymentPosting(int programId, int sessionId, int batchId, int studentId);

        //List<RegistrationBill> GetRegistrationBillHistoryByStudentIdAcaCalId(int studentId, int AcaCalId);
        //List<StudentBillHistoryDTO> GetStudentBillHistoryByStudentId(int studentId);
        //List<rBillHistoryByStudentID> GetBillHistoryByStudentId(int studentId);
        //List<rStudentReceivableReceivedDue> GetStudentReceivableReceivedDue(int programId, int semesterId, int studentTypeId);
        //InstallmentBillDTO GetStudentInstallmentBill(int studentId, int acaCalId);
        //List<rHeadWiseStudentDiscount> GetHeadWiseStudentDiscount(int programId, int sessionId, int discountTypeId);
        //List<rStudentDiscountEligibility> GetStudentDiscountEligibility(int programId, int sessionId, int resultSessionId, decimal lessAmount);
        //List<rCreditsAddAndDrop> GetCreditsAddAndDropReport(int acaCalID, DateTime fromDate, DateTime toDate, int creditType);
        //List<rBillingSummery> GetBillingSummeryBySessionProgramId(int acaCalID, int programID, int billStudentTypeId, int calenderUnnitMasterId);
        //List<rDuesOrAdvance> GetDuesOrAdvanceBySessionProgramId(int acaCalID, int programID, int billStudentTypeId, int billDuesTypeId, int calenderUnnitMasterId);
        //List<rDuesSummery> GetDuesSummeryByProgramIdSessionId(int programId, int acaCalID, int billStudentTypeId);
        //List<rBillingReport> GetBillingReportBySessionProgramIdId(int acaCalID, int programID, int billStudentTypeId, int calenderUnnitMasterId);
        //List<rDiscountSummary> GetStudentDiscountSummaryBySessionId(int sessionId);

    }
}

