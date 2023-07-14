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
    public class CalenderUnitDistributionManager
    {
        #region Cache

        public static readonly string[] MasterCacheKeyArray = { "CalenderUnitDistributionCache" };
        const double CacheDuration = 5.0;

        public static string GetCacheKey(string cacheKey)
        {
            return string.Concat(MasterCacheKeyArray[0], "-", cacheKey);
        }

        public static List<CalenderUnitDistribution> GetCacheAsList(string rawKey)
        {
            List<CalenderUnitDistribution> list = (List<CalenderUnitDistribution>)HttpRuntime.Cache[GetCacheKey(rawKey)];
            return list;
        }

        public static CalenderUnitDistribution GetCacheItem(string rawKey)
        {
            CalenderUnitDistribution item = (CalenderUnitDistribution)HttpRuntime.Cache[GetCacheKey(rawKey)];
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


        public static int Insert(CalenderUnitDistribution calenderunitdistribution)
        {
            int id = RepositoryManager.CalenderUnitDistribution_Repository.Insert(calenderunitdistribution);
            InvalidateCache();
            return id;
        }

        public static bool Update(CalenderUnitDistribution calenderunitdistribution)
        {
            bool isExecute = RepositoryManager.CalenderUnitDistribution_Repository.Update(calenderunitdistribution);
            InvalidateCache();
            return isExecute;
        }

        public static bool Delete(int id)
        {
            bool isExecute = RepositoryManager.CalenderUnitDistribution_Repository.Delete(id);
            InvalidateCache();
            return isExecute;
        }

        public static CalenderUnitDistribution GetById(int? id)
        {
            string rawKey = "CalenderUnitDistributionByID" + id;
            CalenderUnitDistribution calenderunitdistribution = GetCacheItem(rawKey);

            if (calenderunitdistribution == null)
            {
                calenderunitdistribution = RepositoryManager.CalenderUnitDistribution_Repository.GetById(id);
                if (calenderunitdistribution != null)
                    AddCacheItem(rawKey,calenderunitdistribution);
            }

            return calenderunitdistribution;
        }

        public static List<CalenderUnitDistribution> GetAll()
        {
            // return RepositoryAdmission.Program_Repository.GetAll();

            const string rawKey = "CalenderUnitDistributionGetAll";

            List<CalenderUnitDistribution> list = GetCacheAsList(rawKey);

            if (list == null)
            {
                // Item not found in cache - retrieve it and insert it into the cache
                list = RepositoryManager.CalenderUnitDistribution_Repository.GetAll();
                if (list != null)
                    AddCacheItem(rawKey, list);
            }

            return list;
        }

        public static CalenderUnitDistribution GetByCourseId(int? id)
        {
            string rawKey = "CalenderUnitDistributionById" + id;
            CalenderUnitDistribution calenderUnitDistribution = GetCacheItem(rawKey);

            if (calenderUnitDistribution == null)
            {
                calenderUnitDistribution = RepositoryManager.CalenderUnitDistribution_Repository.GetByCourseId(id);
                if (calenderUnitDistribution != null)
                    AddCacheItem(rawKey, calenderUnitDistribution);
            }

            return calenderUnitDistribution;
        }

        //added by Mohammad Ali(21_4_2020)
        public static List<rCourseDistribution> GetCourseDistributionByProgramIdAndTreeCalMasId(int ProgramId, int TreeCalMasId)
        {
            List<rCourseDistribution> list = RepositoryManager.CalenderUnitDistribution_Repository.GetCourseDistributionByProgramIdAndTreeCalMasId(ProgramId, TreeCalMasId);
            return list;
        }

    }
}

