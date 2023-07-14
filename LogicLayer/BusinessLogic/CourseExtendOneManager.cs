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
    public class CourseExtendOneManager
    {
        #region Cache

        public static readonly string[] MasterCacheKeyArray = { "CourseExtendOneCache" };
        const double CacheDuration = 5.0;

        public static string GetCacheKey(string cacheKey)
        {
            return string.Concat(MasterCacheKeyArray[0], "-", cacheKey);
        }

        public static List<CourseExtendOne> GetCacheAsList(string rawKey)
        {
            List<CourseExtendOne> list = (List<CourseExtendOne>)HttpRuntime.Cache[GetCacheKey(rawKey)];
            return list;
        }

        public static CourseExtendOne GetCacheItem(string rawKey)
        {
            CourseExtendOne item = (CourseExtendOne)HttpRuntime.Cache[GetCacheKey(rawKey)];
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


        public static int Insert(CourseExtendOne courseextendone)
        {
            int id = RepositoryManager.CourseExtendOne_Repository.Insert(courseextendone);
            InvalidateCache();
            return id;
        }

        public static bool Update(CourseExtendOne courseextendone)
        {
            bool isExecute = RepositoryManager.CourseExtendOne_Repository.Update(courseextendone);
            InvalidateCache();
            return isExecute;
        }

        public static bool Delete(int id)
        {
            bool isExecute = RepositoryManager.CourseExtendOne_Repository.Delete(id);
            InvalidateCache();
            return isExecute;
        }

        public static CourseExtendOne GetById(int? id)
        {
            string rawKey = "CourseExtendOneByID" + id;
            CourseExtendOne courseextendone = GetCacheItem(rawKey);

            if (courseextendone == null)
            {
                courseextendone = RepositoryManager.CourseExtendOne_Repository.GetById(id);
                if (courseextendone != null)
                    AddCacheItem(rawKey,courseextendone);
            }

            return courseextendone;
        }

        public static List<CourseExtendOne> GetAll()
        {
            // return RepositoryAdmission.Program_Repository.GetAll();

            const string rawKey = "CourseExtendOneGetAll";

            List<CourseExtendOne> list = GetCacheAsList(rawKey);

            if (list == null)
            {
                // Item not found in cache - retrieve it and insert it into the cache
                list = RepositoryManager.CourseExtendOne_Repository.GetAll();
                if (list != null)
                    AddCacheItem(rawKey, list);
            }

            return list;
        }

        public static CourseExtendOne GetByCourseIdVersionId(int courseId, int versionId)
        {
            return RepositoryManager.CourseExtendOne_Repository.GetByCourseIdVersionId(courseId, versionId);
        }
    }
}

