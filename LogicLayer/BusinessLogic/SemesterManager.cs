using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using LogicLayer.BusinessObjects;
using LogicLayer.DataLogic.DAFactory;
using LogicLayer.BusinessObjects.DTO;

namespace LogicLayer.BusinessLogic
{
    public class SemesterManager
    {
        #region Cache

        public static readonly string[] MasterCacheKeyArray = { "SemesterCache" };
        const double CacheDuration = 5.0;

        public static string GetCacheKey(string cacheKey)
        {
            return string.Concat(MasterCacheKeyArray[0], "-", cacheKey);
        }

        public static List<Semester> GetCacheAsList(string rawKey)
        {
            List<Semester> list = (List<Semester>)HttpRuntime.Cache[GetCacheKey(rawKey)];
            return list;
        }

        public static Semester GetCacheItem(string rawKey)
        {
            Semester item = (Semester)HttpRuntime.Cache[GetCacheKey(rawKey)];
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


        public static int Insert(Semester semester)
        {
            int id = RepositoryManager.Semester_Repository.Insert(semester);
            InvalidateCache();
            return id;
        }

        public static bool Update(Semester semester)
        {
            bool isExecute = RepositoryManager.Semester_Repository.Update(semester);
            InvalidateCache();
            return isExecute;
        }

        public static bool Delete(int id)
        {
            bool isExecute = RepositoryManager.Semester_Repository.Delete(id);
            InvalidateCache();
            return isExecute;
        }

        public static Semester GetById(int? id)
        {
            string rawKey = "SemesterByID" + id;
            Semester semester = GetCacheItem(rawKey);

            if (semester == null)
            {
                semester = RepositoryManager.Semester_Repository.GetById(id);
                if (semester != null)
                    AddCacheItem(rawKey,semester);
            }

            return semester;
        }

        public static List<Semester> GetAll()
        {
            // return RepositoryAdmission.Program_Repository.GetAll();

            const string rawKey = "SemesterGetAll";

            List<Semester> list = GetCacheAsList(rawKey);

            if (list == null)
            {
                // Item not found in cache - retrieve it and insert it into the cache
                list = RepositoryManager.Semester_Repository.GetAll();
                if (list != null)
                    AddCacheItem(rawKey, list);
            }

            return list;
        }

        public static List<Semester> GetByYearId(int yearId)
        {
            List<Semester> list = RepositoryManager.Semester_Repository.GetByYearId(yearId);
            return list;
        }

        public static List<Semester> GetByProgramIdYearId(int programId, int yearId)
        {
            List<Semester> list = RepositoryManager.Semester_Repository.GetByProgramIdYearId(programId, yearId);
            return list;
        }


        public static List<SemesterDistinctDTO> GetAllDistinct()
        {
            List<SemesterDistinctDTO> list = RepositoryManager.Semester_Repository.GetAllDistinct();

            return list;
        }

    }
}

