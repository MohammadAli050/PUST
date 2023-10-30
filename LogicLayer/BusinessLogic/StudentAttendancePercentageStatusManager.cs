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
    public class StudentAttendancePercentageStatusManager
    {
        #region Cache

        public static readonly string[] MasterCacheKeyArray = { "StudentAttendancePercentageStatusCache" };
        const double CacheDuration = 5.0;

        public static string GetCacheKey(string cacheKey)
        {
            return string.Concat(MasterCacheKeyArray[0], "-", cacheKey);
        }

        public static List<StudentAttendancePercentageStatus> GetCacheAsList(string rawKey)
        {
            List<StudentAttendancePercentageStatus> list = (List<StudentAttendancePercentageStatus>)HttpRuntime.Cache[GetCacheKey(rawKey)];
            return list;
        }

        public static StudentAttendancePercentageStatus GetCacheItem(string rawKey)
        {
            StudentAttendancePercentageStatus item = (StudentAttendancePercentageStatus)HttpRuntime.Cache[GetCacheKey(rawKey)];
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


        public static int Insert(StudentAttendancePercentageStatus studentattendancepercentagestatus)
        {
            int id = RepositoryManager.StudentAttendancePercentageStatus_Repository.Insert(studentattendancepercentagestatus);
            InvalidateCache();
            return id;
        }

        public static bool Update(StudentAttendancePercentageStatus studentattendancepercentagestatus)
        {
            bool isExecute = RepositoryManager.StudentAttendancePercentageStatus_Repository.Update(studentattendancepercentagestatus);
            InvalidateCache();
            return isExecute;
        }

        public static bool Delete(int id)
        {
            bool isExecute = RepositoryManager.StudentAttendancePercentageStatus_Repository.Delete(id);
            InvalidateCache();
            return isExecute;
        }

        public static StudentAttendancePercentageStatus GetById(int? id)
        {
            string rawKey = "StudentAttendancePercentageStatusByID" + id;
            StudentAttendancePercentageStatus studentattendancepercentagestatus = GetCacheItem(rawKey);

            if (studentattendancepercentagestatus == null)
            {
                studentattendancepercentagestatus = RepositoryManager.StudentAttendancePercentageStatus_Repository.GetById(id);
                if (studentattendancepercentagestatus != null)
                    AddCacheItem(rawKey,studentattendancepercentagestatus);
            }

            return studentattendancepercentagestatus;
        }

        public static List<StudentAttendancePercentageStatus> GetAll()
        {
            // return RepositoryAdmission.Program_Repository.GetAll();

            const string rawKey = "StudentAttendancePercentageStatusGetAll";

            List<StudentAttendancePercentageStatus> list = GetCacheAsList(rawKey);

            if (list == null)
            {
                // Item not found in cache - retrieve it and insert it into the cache
                list = RepositoryManager.StudentAttendancePercentageStatus_Repository.GetAll();
                if (list != null)
                    AddCacheItem(rawKey, list);
            }

            return list;
        }
    }
}

