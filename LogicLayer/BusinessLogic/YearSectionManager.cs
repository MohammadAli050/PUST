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
    public class YearSectionManager
    {
        #region Cache

        public static readonly string[] MasterCacheKeyArray = { "YearSectionCache" };
        const double CacheDuration = 5.0;

        public static string GetCacheKey(string cacheKey)
        {
            return string.Concat(MasterCacheKeyArray[0], "-", cacheKey);
        }

        public static List<YearSection> GetCacheAsList(string rawKey)
        {
            List<YearSection> list = (List<YearSection>)HttpRuntime.Cache[GetCacheKey(rawKey)];
            return list;
        }

        public static YearSection GetCacheItem(string rawKey)
        {
            YearSection item = (YearSection)HttpRuntime.Cache[GetCacheKey(rawKey)];
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

        public static int Insert(YearSection yearsection)
        {
            int id = RepositoryManager.YearSection_Repository.Insert(yearsection);
            InvalidateCache();
            return id;
        }

        public static bool Update(YearSection yearsection)
        {
            bool isExecute = RepositoryManager.YearSection_Repository.Update(yearsection);
            InvalidateCache();
            return isExecute;
        }

        public static bool Delete(int id)
        {
            bool isExecute = RepositoryManager.YearSection_Repository.Delete(id);
            InvalidateCache();
            return isExecute;
        }

        public static YearSection GetById(int? id)
        {
            string rawKey = "YearSectionByID" + id;
            YearSection yearsection = GetCacheItem(rawKey);

            if (yearsection == null)
            {
                yearsection = RepositoryManager.YearSection_Repository.GetById(id);
                if (yearsection != null)
                    AddCacheItem(rawKey,yearsection);
            }

            return yearsection;
        }

        public static List<YearSection> GetAll()
        {
            // return RepositoryAdmission.Program_Repository.GetAll();

            const string rawKey = "YearSectionGetAll";

            List<YearSection> list = GetCacheAsList(rawKey);

            if (list == null)
            {
                // Item not found in cache - retrieve it and insert it into the cache
                list = RepositoryManager.YearSection_Repository.GetAll();
                if (list != null)
                    AddCacheItem(rawKey, list);
            }

            return list;
        }

    }
}

