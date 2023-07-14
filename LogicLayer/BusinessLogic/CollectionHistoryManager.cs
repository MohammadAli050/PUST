using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using LogicLayer.BusinessObjects;
using LogicLayer.BusinessObjects.DTO;
//using LogicLayer.BusinessObjects.RO;
using LogicLayer.DataLogic.DAFactory;

namespace LogicLayer.BusinessLogic
{
    public class CollectionHistoryManager
    {
        #region Cache

        public static readonly string[] MasterCacheKeyArray = { "CollectionHistoryCache" };
        const double CacheDuration = 5.0;

        public static string GetCacheKey(string cacheKey)
        {
            return string.Concat(MasterCacheKeyArray[0], "-", cacheKey);
        }

        public static List<CollectionHistory> GetCacheAsList(string rawKey)
        {
            List<CollectionHistory> list = (List<CollectionHistory>)HttpRuntime.Cache[GetCacheKey(rawKey)];
            return list;
        }

        public static CollectionHistory GetCacheItem(string rawKey)
        {
            CollectionHistory item = (CollectionHistory)HttpRuntime.Cache[GetCacheKey(rawKey)];
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


        public static int Insert(CollectionHistory collectionhistory)
        {
            int id = RepositoryManager.CollectionHistory_Repository.Insert(collectionhistory);
            InvalidateCache();
            return id;
        }

        public static bool Update(CollectionHistory collectionhistory)
        {
            bool isExecute = RepositoryManager.CollectionHistory_Repository.Update(collectionhistory);
            InvalidateCache();
            return isExecute;
        }

        public static bool Delete(int id)
        {
            bool isExecute = RepositoryManager.CollectionHistory_Repository.Delete(id);
            InvalidateCache();
            return isExecute;
        }

        public static CollectionHistory GetById(int id)
        {
            CollectionHistory collectionhistory = RepositoryManager.CollectionHistory_Repository.GetById(id);
            return collectionhistory;
        }

        public static List<CollectionHistory> GetAll()
        {
            // return RepositoryAdmission.Program_Repository.GetAll();

            const string rawKey = "CollectionHistoryGetAll";

            List<CollectionHistory> list = GetCacheAsList(rawKey);

            if (list == null)
            {
                // Item not found in cache - retrieve it and insert it into the cache
                list = RepositoryManager.CollectionHistory_Repository.GetAll();
                if (list != null)
                    AddCacheItem(rawKey, list);
            }

            return list;
        }

        public static bool IsDuplicateMoneyReceipt(string moneyReceiptNo, string paymentType)
        {
            CollectionHistory collectionHistoryObj = RepositoryManager.CollectionHistory_Repository.IsDuplicateMoneyReceipt(moneyReceiptNo, paymentType);
            if (collectionHistoryObj != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static List<CollectionHistory> GetByStudentIdFundIdBillHistoryMasterIdLastDate(int studentId, int fundId, int billHistoryMasterId, DateTime lastDate)
        {
            return RepositoryManager.CollectionHistory_Repository.GetByStudentIdFundIdBillHistoryMasterIdLastDate(studentId, fundId, billHistoryMasterId, lastDate);
        }

        public static List<CollectionHistory> GetByBillHistoryMasterId(int billHistoryMasterId)
        {
            return RepositoryManager.CollectionHistory_Repository.GetByBillHistoryMasterId(billHistoryMasterId);
        }

        public static List<BillDeleteDTO> GetBillPaidStudentsByProgramIdSessionIdAdmissionSessionId(int programId, int sessionId, int? admissionSessionId)
        {
            return RepositoryManager.CollectionHistory_Repository.GetBillPaidStudentsByProgramIdSessionIdAdmissionSessionId(programId, sessionId, admissionSessionId);
        }

        //public static List<rDepartmentWiseCollection> GetDepartmentWiseByDateRange(DateTime fromDate, DateTime toDate)
        //{
        //    List<rDepartmentWiseCollection> list = RepositoryManager.CollectionHistory_Repository.GetDepartmentWiseByDateRange(fromDate, toDate);
        //    return list;
        //}

        //public static List<rDepartmentWiseCollection> GetProgramFeeWiseByDateRange(DateTime fromDate, DateTime toDate)
        //{
        //    List<rDepartmentWiseCollection> list = RepositoryManager.CollectionHistory_Repository.GetProgramFeeWiseByDateRange(fromDate, toDate);
        //    return list;
        //}

        ///// <summary>
        ///// added for department wise collection new, 08-12-2018
        ///// </summary>
        ///// <param name="fundTypeId"></param>
        ///// <param name="billCounterTypeId"></param>
        ///// <param name="fromDate"></param>
        ///// <param name="toDate"></param>
        ///// <returns></returns>
        //public static List<rDepartmentWiseCollection> GetDepartmentWisePaymentCollectionByPaymentTypeIdFundIdAndDateRange(int? fundTypeId, int? billCounterTypeId, DateTime fromDate, DateTime toDate)
        //{
        //    List<rDepartmentWiseCollection> list = RepositoryManager.CollectionHistory_Repository.GetDepartmentWisePaymentCollectionByPaymentTypeIdFundIdAndDateRange(fundTypeId, billCounterTypeId, fromDate, toDate);
        //    return list;
        //}
        //public static List<rDepartmentWiseCollection> GetDepartmentWisePaymentCollectionDetailByPaymentTypeIdFundIdAndDateRange(int? fundTypeId, int? billCounterTypeId, DateTime fromDate, DateTime toDate)
        //{
        //    List<rDepartmentWiseCollection> list = RepositoryManager.CollectionHistory_Repository.GetDepartmentWisePaymentCollectionDetailByPaymentTypeIdFundIdAndDateRange(fundTypeId, billCounterTypeId, fromDate, toDate);
        //    return list;
        //}


        ////public static List<rStudentWiseCollection> GetStudentPaymentByDateRange(DateTime fromDate, DateTime toDate)
        ////{
        ////    List<rStudentWiseCollection> list = RepositoryManager.CollectionHistory_Repository.GetStudentPaymentByDateRange(fromDate, toDate);
        ////    return list;
        ////}

        //public static List<RStudentWiseCollection> GetStudentPaymentByPaymentTypeIdFundIdAndDateRange(int? fundTypeId, int? billCounterTypeId, DateTime fromDate, DateTime toDate)
        //{
        //    List<RStudentWiseCollection> list = RepositoryManager.CollectionHistory_Repository.GetStudentPaymentByPaymentTypeIdFundIdAndDateRange(fundTypeId, billCounterTypeId, fromDate, toDate);
        //    return list;
        //}

        //public static List<RStudentWiseCollection> GetStudentPaymentDetailByPaymentTypeIdFundIdAndDateRange(int? fundTypeId, int? billCounterTypeId, DateTime fromDate, DateTime toDate)
        //{
        //    List<RStudentWiseCollection> list = RepositoryManager.CollectionHistory_Repository.GetStudentPaymentDetailByPaymentTypeIdFundIdAndDateRange(fundTypeId, billCounterTypeId, fromDate, toDate);
        //    return list;
        //}

        //public static List<FundWiseCollectionViewObject> GetFundWiseTotalCollectionByDateRange(DateTime fromDate, DateTime toDate)
        //{
        //    return RepositoryManager.CollectionHistory_Repository.GetFundWiseTotalCollectionByDateRange(fromDate, toDate);
        //}

        //public static List<RStudentWiseCollection> GetStudentPaymentSummaryByPaymentTypeIdFundIdAndDateRange_TB(int? fundTypeId, int? billCounterTypeId, DateTime fromDate, DateTime toDate)
        //{
        //    List<RStudentWiseCollection> list = RepositoryManager.CollectionHistory_Repository.GetStudentPaymentSummaryByPaymentTypeIdFundIdAndDateRange_TB(fundTypeId, billCounterTypeId, fromDate, toDate);
        //    return list;
        //}

        //public static List<StudentPayments> GetAllCollectionByPaymentTypeIdFundIdAndDateRange_TB(int? fundTypeId, int? billCounterTypeId, DateTime fromDate, DateTime toDate)
        //{
        //    List<StudentPayments> list = RepositoryManager.CollectionHistory_Repository.GetAllCollectionByPaymentTypeIdFundIdAndDateRange_TB(fundTypeId, billCounterTypeId, fromDate, toDate);
        //    return list;
        //}

        
    }
}
