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
    public class StudentAdditionalInfoManager
    {
        #region Cache

        public static readonly string[] MasterCacheKeyArray = { "StudentAdditionalInfoCache" };
        const double CacheDuration = 5.0;

        public static string GetCacheKey(string cacheKey)
        {
            return string.Concat(MasterCacheKeyArray[0], "-", cacheKey);
        }

        public static List<StudentAdditionalInfo> GetCacheAsList(string rawKey)
        {
            List<StudentAdditionalInfo> list = (List<StudentAdditionalInfo>)HttpRuntime.Cache[GetCacheKey(rawKey)];
            return list;
        }

        public static StudentAdditionalInfo GetCacheItem(string rawKey)
        {
            StudentAdditionalInfo item = (StudentAdditionalInfo)HttpRuntime.Cache[GetCacheKey(rawKey)];
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


        public static int Insert(StudentAdditionalInfo studentadditionalinfo)
        {
            int id = RepositoryManager.StudentAdditionalInfo_Repository.Insert(studentadditionalinfo);
            InvalidateCache();
            return id;
        }

        public static bool Update(StudentAdditionalInfo studentadditionalinfo)
        {
            bool isExecute = RepositoryManager.StudentAdditionalInfo_Repository.Update(studentadditionalinfo);
            InvalidateCache();
            return isExecute;
        }

        public static bool Delete(int id)
        {
            bool isExecute = RepositoryManager.StudentAdditionalInfo_Repository.Delete(id);
            InvalidateCache();
            return isExecute;
        }

        public static StudentAdditionalInfo GetById(int? id)
        {
            string rawKey = "StudentAdditionalInfoByID" + id;
            StudentAdditionalInfo studentadditionalinfo = GetCacheItem(rawKey);

            if (studentadditionalinfo == null)
            {
                studentadditionalinfo = RepositoryManager.StudentAdditionalInfo_Repository.GetById(id);
                if (studentadditionalinfo != null)
                    AddCacheItem(rawKey,studentadditionalinfo);
            }

            return studentadditionalinfo;
        }

        public static List<StudentAdditionalInfo> GetAll()
        {
            // return RepositoryAdmission.Program_Repository.GetAll();

            const string rawKey = "StudentAdditionalInfoGetAll";

            List<StudentAdditionalInfo> list = GetCacheAsList(rawKey);

            if (list == null)
            {
                // Item not found in cache - retrieve it and insert it into the cache
                list = RepositoryManager.StudentAdditionalInfo_Repository.GetAll();
                if (list != null)
                    AddCacheItem(rawKey, list);
            }

            return list;
        }

        public static StudentAdditionalInfo GetByStudentId(int studentId)
        {
            StudentAdditionalInfo studentadditionalinfo = RepositoryManager.StudentAdditionalInfo_Repository.GetByStudentId(studentId);
            return studentadditionalinfo;
        }
    }
}

