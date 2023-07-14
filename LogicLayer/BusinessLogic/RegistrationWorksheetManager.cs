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
    public class RegistrationWorksheetManager
    {
        #region Cache

        public static readonly string[] MasterCacheKeyArray = { "RegistrationWorksheetCache" };
        const double CacheDuration = 5.0;

        public static string GetCacheKey(string cacheKey)
        {
            return string.Concat(MasterCacheKeyArray[0], "-", cacheKey);
        }

        public static List<RegistrationWorksheet> GetCacheAsList(string rawKey)
        {
            List<RegistrationWorksheet> list = (List<RegistrationWorksheet>)HttpRuntime.Cache[GetCacheKey(rawKey)];
            return list;
        }

        public static RegistrationWorksheet GetCacheItem(string rawKey)
        {
            RegistrationWorksheet item = (RegistrationWorksheet)HttpRuntime.Cache[GetCacheKey(rawKey)];
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


        public static int Insert(RegistrationWorksheet registrationworksheet)
        {
            int id = RepositoryManager.RegistrationWorksheet_Repository.Insert(registrationworksheet);
            InvalidateCache();
            return id;
        }

        public static bool Update(RegistrationWorksheet registrationworksheet)
        {
            bool isExecute = RepositoryManager.RegistrationWorksheet_Repository.Update(registrationworksheet);
            InvalidateCache();
            return isExecute;
        }

        public static bool Delete(int id)
        {
            bool isExecute = RepositoryManager.RegistrationWorksheet_Repository.Delete(id);
            InvalidateCache();
            return isExecute;
        }

        public static RegistrationWorksheet GetById(int? id)
        {
            string rawKey = "RegistrationWorksheetByID" + id;
            RegistrationWorksheet registrationworksheet = GetCacheItem(rawKey);

            if (registrationworksheet == null)
            {
                registrationworksheet = RepositoryManager.RegistrationWorksheet_Repository.GetById(id);
                if (registrationworksheet != null)
                    AddCacheItem(rawKey,registrationworksheet);
            }

            return registrationworksheet;
        }

        public static List<RegistrationWorksheet> GetAll()
        {
            // return RepositoryAdmission.Program_Repository.GetAll();

            const string rawKey = "RegistrationWorksheetGetAll";

            List<RegistrationWorksheet> list = GetCacheAsList(rawKey);

            if (list == null)
            {
                // Item not found in cache - retrieve it and insert it into the cache
                list = RepositoryManager.RegistrationWorksheet_Repository.GetAll();
                if (list != null)
                    AddCacheItem(rawKey, list);
            }

            return list;
        }

        public static List<RegistrationWorksheet> GetAllOpenCourseWhichSectionIsMatchInStudentBatchByStudentID(int studentId, int regSession)
        {
            List<RegistrationWorksheet> list = RepositoryManager.RegistrationWorksheet_Repository.GetAllOpenCourseWhichSectionIsMatchInStudentBatchByStudentID(studentId, regSession);
            return list;
        }

        public static List<RegistrationWorksheet> GetByStudentID(int studentId)
        {
            string rawKey = "RegistrationWorksheetByStudentID" + studentId;

            List<RegistrationWorksheet> list = GetCacheAsList(rawKey);

            if (list == null)
            {
                list = RepositoryManager.RegistrationWorksheet_Repository.GetByStudentID(studentId);
                list = list.OrderBy(l => l.Priority).ToList();
                if (list != null && list.Count() != 0)
                    AddCacheItem(rawKey, list);
            }

            return list;
        }

        public static List<RegistrationWorksheet> GetByStudentIDAcacalID(int studentId, int acacalId)
        {
            List<RegistrationWorksheet> list = RepositoryManager.RegistrationWorksheet_Repository.GetByStudentIDAcacalID(studentId, acacalId);
            return list;
        }

        public static List<RegistrationWorksheet> GetByStudentIdYearNoSemesterNoExamId(int studentId, int yearNo, int semesterNo, int examId)
        {
            List<RegistrationWorksheet> list = RepositoryManager.RegistrationWorksheet_Repository.GetByStudentIdYearNoSemesterNoExamId(studentId, yearNo, semesterNo, examId);
            return list;
        }
    }
}

