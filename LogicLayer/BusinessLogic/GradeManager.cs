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
    public class GradeManager
    {
        #region Cache

        public static readonly string[] MasterCacheKeyArray = { "GradeCache" };
        const double CacheDuration = 5.0;

        public static string GetCacheKey(string cacheKey)
        {
            return string.Concat(MasterCacheKeyArray[0], "-", cacheKey);
        }

        public static List<Grade> GetCacheAsList(string rawKey)
        {
            List<Grade> list = (List<Grade>)HttpRuntime.Cache[GetCacheKey(rawKey)];
            return list;
        }

        public static Grade GetCacheItem(string rawKey)
        {
            Grade item = (Grade)HttpRuntime.Cache[GetCacheKey(rawKey)];
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


        public static int Insert(Grade grade)
        {
            int id = RepositoryManager.Grade_Repository.Insert(grade);
            InvalidateCache();
            return id;
        }

        public static bool Update(Grade grade)
        {
            bool isExecute = RepositoryManager.Grade_Repository.Update(grade);
            InvalidateCache();
            return isExecute;
        }

        public static bool Delete(int id)
        {
            bool isExecute = RepositoryManager.Grade_Repository.Delete(id);
            InvalidateCache();
            return isExecute;
        }

        public static Grade GetById(int? id)
        {
            string rawKey = "GradeByID" + id;
            Grade grade = GetCacheItem(rawKey);

            if (grade == null)
            {
                grade = RepositoryManager.Grade_Repository.GetById(id);
                if (grade != null)
                    AddCacheItem(rawKey, grade);
            }

            return grade;
        }

        public static List<Grade> GetAll()
        {
            // return RepositoryAdmission.Program_Repository.GetAll();

            const string rawKey = "GradeGetAll";

            List<Grade> list = GetCacheAsList(rawKey);

            if (list == null)
            {
                // Item not found in cache - retrieve it and insert it into the cache
                list = RepositoryManager.Grade_Repository.GetAll();
                if (list != null)
                    AddCacheItem(rawKey, list);
            }

            return list;
        }

        public static List<Grade> GetByGradeMasterId(int gradeMasterId)
        {
            List<Grade> list = RepositoryManager.Grade_Repository.GetByGradeMasterId(gradeMasterId);
            return list;
        }
    }
}

