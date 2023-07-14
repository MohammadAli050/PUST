using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using LogicLayer.BusinessObjects;
using LogicLayer.DataLogic.DAFactory;

namespace LogicLayer.BusinessLogic
{
    public class FeeSetupManager
    {
        #region Cache

        public static readonly string[] MasterCacheKeyArray = { "FeeSetupCache" };
        const double CacheDuration = 5.0;

        public static string GetCacheKey(string cacheKey)
        {
            return string.Concat(MasterCacheKeyArray[0], "-", cacheKey);
        }

        public static List<FeeSetup> GetCacheAsList(string rawKey)
        {
            List<FeeSetup> list = (List<FeeSetup>)HttpRuntime.Cache[GetCacheKey(rawKey)];
            return list;
        }

        public static FeeSetup GetCacheItem(string rawKey)
        {
            FeeSetup item = (FeeSetup)HttpRuntime.Cache[GetCacheKey(rawKey)];
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

        public static int Insert(FeeSetup feesetup)
        {
            int id = RepositoryManager.FeeSetup_Repository.Insert(feesetup);
            InvalidateCache();
            return id;
        }

        public static bool Update(FeeSetup feesetup)
        {
            bool isExecute = RepositoryManager.FeeSetup_Repository.Update(feesetup);
            InvalidateCache();
            return isExecute;
        }

        public static bool Delete(int id)
        {
            bool isExecute = RepositoryManager.FeeSetup_Repository.Delete(id);
            InvalidateCache();
            return isExecute;
        }

        public static FeeSetup GetById(int? id)
        {
            string rawKey = "FeeSetupByID" + id;
            FeeSetup feesetup = GetCacheItem(rawKey);

            if (feesetup == null)
            {
                feesetup = RepositoryManager.FeeSetup_Repository.GetById(id);
                if (feesetup != null)
                    AddCacheItem(rawKey, feesetup);
            }

            return feesetup;
        }

        public static List<FeeSetup> GetAll()
        {
            // return RepositoryAdmission.Program_Repository.GetAll();

            const string rawKey = "FeeSetupGetAll";

            List<FeeSetup> list = GetCacheAsList(rawKey);

            if (list == null)
            {
                // Item not found in cache - retrieve it and insert it into the cache
                list = RepositoryManager.FeeSetup_Repository.GetAll();
                if (list != null)
                    AddCacheItem(rawKey, list);
            }

            return list;
        }

        public static List<FeeSetup> GetAllByProgramIdAcaCalIdScholarshipStatusAndGovNonGov(int programId, int? acaCalId, int scholarshipStatusId, int govNonGovId)
        {
            List<FeeSetup> feeSetup = RepositoryManager.FeeSetup_Repository.GetAllByProgramIdAcaCalIdScholarshipStatusAndGovNonGov(programId, acaCalId, scholarshipStatusId, govNonGovId);
            return feeSetup;
        }

        public static List<FeeGroupMaster> GetAllFeeGroupByProgramIdAcaCalId(int? programId, int? acaCalId)
        {
            List<FeeGroupMaster> feeSetup = RepositoryManager.FeeSetup_Repository.GetAllFeeGroupByProgramIdAcaCalId(programId, acaCalId);
            return feeSetup;
        }

    }
}

