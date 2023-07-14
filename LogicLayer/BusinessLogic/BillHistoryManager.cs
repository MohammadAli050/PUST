using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using LogicLayer.BusinessObjects;
using LogicLayer.DataLogic.DAFactory;
using LogicLayer.BusinessObjects.DTO;
//using LogicLayer.BusinessObjects.RO;

namespace LogicLayer.BusinessLogic
{
    public class BillHistoryManager
    {
        #region Cache

        public static readonly string[] MasterCacheKeyArray = { "BillHistoryCache" };
        const double CacheDuration = 5.0;

        public static string GetCacheKey(string cacheKey)
        {
            return string.Concat(MasterCacheKeyArray[0], "-", cacheKey);
        }

        public static List<BillHistory> GetCacheAsList(string rawKey)
        {
            List<BillHistory> list = (List<BillHistory>)HttpRuntime.Cache[GetCacheKey(rawKey)];
            return list;
        }

        public static BillHistory GetCacheItem(string rawKey)
        {
            BillHistory item = (BillHistory)HttpRuntime.Cache[GetCacheKey(rawKey)];
            return item;
        }

        public static void AddCacheItem(string rawKey, object value)
        {
            System.Web.Caching.Cache DataCache = HttpRuntime.Cache;

            // Make sure MasterCacheKeyArray[0] is in the cache - if not, add it
            if (DataCache[MasterCacheKeyArray[0]] == null)
                DataCache[MasterCacheKeyArray[0]] = DateTime.Now;

            // Add a CacheDependency
            System.Web.Caching.CacheDependency dependency = new System.Web.Caching.CacheDependency(null, MasterCacheKeyArray);
            DataCache.Insert(GetCacheKey(rawKey), value, dependency, DateTime.Now.AddMinutes(CacheDuration), System.Web.Caching.Cache.NoSlidingExpiration);
        }



        public static void InvalidateCache()
        {
            // Remove the cache dependency
            HttpRuntime.Cache.Remove(MasterCacheKeyArray[0]);
        }

        #endregion

        public static int Insert(BillHistory billHistory)
        {
            int id = RepositoryManager.BillHistory_Repository.Insert(billHistory);
            InvalidateCache();
            return id;
        }

        public static bool Update(BillHistory billhistory)
        {
            bool isExecute = RepositoryManager.BillHistory_Repository.Update(billhistory);
            InvalidateCache();
            return isExecute;
        }

        public static bool Delete(int id)
        {
            bool isExecute = RepositoryManager.BillHistory_Repository.Delete(id);
            InvalidateCache();
            return isExecute;
        }

        public static BillHistory GetById(int? id)
        {
            string rawKey = "BillHistoryByID" + id;
            BillHistory billhistory = GetCacheItem(rawKey);

            if (billhistory == null)
            {
                billhistory = RepositoryManager.BillHistory_Repository.GetById(id);
                if (billhistory != null)
                    AddCacheItem(rawKey, billhistory);
            }

            return billhistory;
        }

        public static BillHistory GetByStudentCourseHistoryId(int courseHistoryId)
        {
            BillHistory billhistory = RepositoryManager.BillHistory_Repository.GetByStudentCourseHistoryId(courseHistoryId);

            return billhistory;
        }

        public static List<BillHistory> GetAll()
        {
            // return RepositoryAdmission.Program_Repository.GetAll();
            List<BillHistory> list = null;
            return list;
        }

        public static List<StudentBillDetailsDTO> GetStudentBillingDetails(int studentId, int programId, int studentAdmissionAcaCalId, int acaCalId)
        {
            List<StudentBillDetailsDTO> list = RepositoryManager.BillHistory_Repository.GetStudentBillingDetails(studentId, programId, studentAdmissionAcaCalId, acaCalId);
            return list;
        }

        public static List<StudentBillDetailsDTO> GetStudentBillingDetailsByGroup(int studentId, int programId, int studentAdmissionAcaCalId, int acaCalId, int feeGroupMasterId)
        {
            List<StudentBillDetailsDTO> list = RepositoryManager.BillHistory_Repository.GetStudentBillingDetailsByGroup(studentId, programId, studentAdmissionAcaCalId, acaCalId, feeGroupMasterId);
            return list;
        }

        public static List<StudentBillDetailsDTO> GetNcBillCalculationByCampusIdProgramIdAcaCalIdAcaCalSectionId(int? campusId, int? programId, int? acaCalId, int? acaCalSectionId)
        {
            List<StudentBillDetailsDTO> list = RepositoryManager.BillHistory_Repository.GetNcBillCalculationByCampusIdProgramIdAcaCalIdAcaCalSectionId(campusId, programId, acaCalId, acaCalSectionId);
            return list;
        }

        public static List<StudentBillDetailsDTO> GetStudentShortSupplementaryBillingDetailsByStudentIdSessionIdCalenderUnitTypeId(int studentId, int acaCalId, int calenderUnitTypeId)
        {
            List<StudentBillDetailsDTO> list = RepositoryManager.BillHistory_Repository.GetStudentShortSupplementaryBillingDetailsByStudentIdSessionIdCalenderUnitTypeId(studentId, acaCalId, calenderUnitTypeId);
            return list;
        }

        public static List<StudentBillDetailsDTO> GenerateStudentAnnualBillingDetails(int studentId, int programId, int batchId, int acaCalId)
        {
            List<StudentBillDetailsDTO> list = RepositoryManager.BillHistory_Repository.GenerateStudentAnnualBillingDetails(studentId, programId, batchId, acaCalId);
            return list;
        }

        public static bool GetStudentBillHistory(int studentCourseHistoryId, int studentId, int acaCalId, int typeDefinationId, decimal fee)
        {
            BillHistory list = RepositoryManager.BillHistory_Repository.GetStudentBillHistory(studentCourseHistoryId, studentId, acaCalId, typeDefinationId, fee);
            if (list == null)
            {
                return true;
            }
            return false;
        }

        public static List<BillHistory> GetBillForPrintByBillHistoryMasterId(int billHistoryMasterId)
        {
            List<BillHistory> list = RepositoryManager.BillHistory_Repository.GetBillForPrintByBillHistoryMasterId(billHistoryMasterId);
            return list;
        }

        public static List<BillHistory> GetBillHistoryByBillHistoryMasterId(int billHistoryMasterId)
        {
            List<BillHistory> list = RepositoryManager.BillHistory_Repository.GetBillHistoryByBillHistoryMasterId(billHistoryMasterId);
            return list;
        }

        public static bool DeleteByBillHistoryMasterId(int billHistoryMasterId)
        {
            bool isExecute = RepositoryManager.BillHistory_Repository.DeleteByBillHistoryMasterId(billHistoryMasterId);
            return isExecute;
        }

        public static BillHistory GetStudentDiscountHistoryByStudentIdAcacalId(int studentId, int acaCalId, int typeDefId)
        {
            BillHistory list = RepositoryManager.BillHistory_Repository.GetStudentDiscountHistoryByStudentIdAcacalId(studentId, acaCalId, typeDefId);
            return list;
        }

        //public static List<PaymentPostingDTO> GetBillForPaymentPosting(int programId, int sessionId, int batchId, int studentId)
        //{
        //    List<PaymentPostingDTO> list = RepositoryManager.BillHistory_Repository.GetBillForPaymentPosting(programId, sessionId, batchId, studentId);
        //    return list;
        //}

        public static List<BillPaymentHistoryDTO> GetBillPaymentHistoryByBillHistoryMasterId(int billHistoryMasterId)
        {
            List<BillPaymentHistoryDTO> list = RepositoryManager.BillHistory_Repository.GetBillPaymentHistoryByBillHistoryMasterId(billHistoryMasterId);
            return list;
        }
        public static List<BillHistory> GetBillHistoryByStudentIdReferenceNo(int studentId, string referenceNo)
        {
            List<BillHistory> list = RepositoryManager.BillHistory_Repository.GetBillHistoryByStudentIdReferenceNo(studentId, referenceNo);
            return list;
        }
        //public static List<StudentBillPaymentDueViewObject> GetBillPaymentDueByProgramIdBatchIdSessionId(int? programId, int? batchId, int? sessionId)
        //{
        //    return RepositoryManager.BillHistory_Repository.GetBillPaymentDueByProgramIdBatchIdSessionId(programId, batchId, sessionId);
        //}
        //public static List<StudentBillPaymentDueViewObject> GetBillPaymentDueSummeryBySessionId(int? sessionId)
        //{
        //    return RepositoryManager.BillHistory_Repository.GetBillPaymentDueSummeryBySessionId(sessionId);
        //}
        //public static List<BillDeleteDTO> GetBillForDelete(int programId, int batchId, int sessionId, int studentId, DateTime? date)
        //{
        //    List<BillDeleteDTO> billhistoryMaster = RepositoryManager.BillHistory_Repository.GetBillForDelete(programId, batchId, sessionId, studentId, date);
        //    return billhistoryMaster;
        //}    

        public static List<BillPaymentHistoryMasterDTO> GetBillPaymentHistoryMasterByStudentId(int studentId)
        {
            List<BillPaymentHistoryMasterDTO> list = RepositoryManager.BillHistory_Repository.GetBillPaymentHistoryMasterByStudentId(studentId);
            return list;
        }

        //public static List<RegistrationBill> GetRegistrationBillHistoryByStudentIdAcaCalId(int studentId, int AcaCalId)
        //{
        //    List<RegistrationBill> list = RepositoryManager.BillHistory_Repository.GetRegistrationBillHistoryByStudentIdAcaCalId(studentId, AcaCalId);
        //    return list;
        //}

        //public static List<StudentBillHistoryDTO> GetStudentBillHistoryByStudentId(int studentId)
        //{
        //    List<StudentBillHistoryDTO> list = RepositoryManager.BillHistory_Repository.GetStudentBillHistoryByStudentId(studentId);
        //    return list;
        //}

        //public static List<rBillHistoryByStudentID> GetBillHistoryByStudentId(int studentId)
        //{
        //    List<rBillHistoryByStudentID> list = RepositoryManager.BillHistory_Repository.GetBillHistoryByStudentId(studentId);
        //    return list;
        //}

        //public static List<rStudentReceivableReceivedDue> GetStudentReceivableReceivedDue(int programId, int semesterId, int studentTypeId)
        //{
        //    List<rStudentReceivableReceivedDue> list = RepositoryManager.BillHistory_Repository.GetStudentReceivableReceivedDue(programId, semesterId, studentTypeId);
        //    return list;
        //}

        //public static InstallmentBillDTO GetStudentInstallmentBill(int studentId, int acaCalId)
        //{
        //    InstallmentBillDTO obj = RepositoryManager.BillHistory_Repository.GetStudentInstallmentBill(studentId, acaCalId);
        //    return obj;
        //}

        //public static List<rHeadWiseStudentDiscount> GetHeadWiseStudentDiscount(int programId, int sessionId, int discountTypeId)
        //{
        //    List<rHeadWiseStudentDiscount> list = RepositoryManager.BillHistory_Repository.GetHeadWiseStudentDiscount(programId, sessionId, discountTypeId);
        //    return list;
        //}

        //public static List<rStudentDiscountEligibility> GetStudentDiscountEligibility(int programId, int sessionId, int resultSessionId, decimal lessAmount)
        //{
        //    List<rStudentDiscountEligibility> list = RepositoryManager.BillHistory_Repository.GetStudentDiscountEligibility(programId, sessionId, resultSessionId, lessAmount);
        //    return list;
        //}

        //public static List<rCreditsAddAndDrop> GetCreditsAddAndDropReport(int acaCalID, DateTime fromDate, DateTime toDate, int creditType)
        //{
        //    List<rCreditsAddAndDrop> list = RepositoryManager.BillHistory_Repository.GetCreditsAddAndDropReport(acaCalID, fromDate, toDate, creditType);
        //    return list;
        //}

        //public static List<rBillingSummery> GetBillingSummeryBySessionProgramId(int acaCalID, int programID, int billStudentTypeId, int calenderUnitMasterId)
        //{
        //    List<rBillingSummery> list = RepositoryManager.BillHistory_Repository.GetBillingSummeryBySessionProgramId(acaCalID, programID, billStudentTypeId, calenderUnitMasterId);
        //    return list;
        //}

        //public static List<rDuesOrAdvance> GetDuesOrAdvanceBySessionProgramId(int acaCalID, int programID, int billStudentTypeId, int billDuesTypeId, int calenderUnitMasterId)
        //{
        //    List<rDuesOrAdvance> list = RepositoryManager.BillHistory_Repository.GetDuesOrAdvanceBySessionProgramId(acaCalID, programID, billStudentTypeId, billDuesTypeId, calenderUnitMasterId);
        //    return list;
        //}

        //public static List<rDuesSummery> GetDuesSummeryByProgramIdSessionId(int programId, int acaCalID, int billStudentTypeId)
        //{
        //    List<rDuesSummery> list = RepositoryManager.BillHistory_Repository.GetDuesSummeryByProgramIdSessionId(programId, acaCalID, billStudentTypeId);
        //    return list;
        //}

        //public static List<rBillingReport> GetBillingReportBySessionProgramIdId(int acaCalID, int programID, int billStudentTypeId, int calenderUnitMasterId)
        //{
        //    List<rBillingReport> list = RepositoryManager.BillHistory_Repository.GetBillingReportBySessionProgramIdId(acaCalID, programID, billStudentTypeId, calenderUnitMasterId);
        //    return list;
        //}

        //public static List<rDiscountSummary> GetStudentDiscountSummaryBySessionId(int sessionId)
        //{
        //    List<rDiscountSummary> list = RepositoryManager.BillHistory_Repository.GetStudentDiscountSummaryBySessionId(sessionId);
        //    return list;
        //}

        
    }
}

