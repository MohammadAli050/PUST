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
    public class YearManager
    {
        #region Cache

        public static readonly string[] MasterCacheKeyArray = { "YearCache" };
        const double CacheDuration = 5.0;

        public static string GetCacheKey(string cacheKey)
        {
            return string.Concat(MasterCacheKeyArray[0], "-", cacheKey);
        }

        public static List<Year> GetCacheAsList(string rawKey)
        {
            List<Year> list = (List<Year>)HttpRuntime.Cache[GetCacheKey(rawKey)];
            return list;
        }

        public static Year GetCacheItem(string rawKey)
        {
            Year item = (Year)HttpRuntime.Cache[GetCacheKey(rawKey)];
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


        public static int Insert(Year year)
        {
            int id = RepositoryManager.Year_Repository.Insert(year);
            InvalidateCache();
            return id;
        }

        public static bool Update(Year year)
        {
            bool isExecute = RepositoryManager.Year_Repository.Update(year);
            InvalidateCache();
            return isExecute;
        }

        public static bool Delete(int id)
        {
            bool isExecute = RepositoryManager.Year_Repository.Delete(id);
            InvalidateCache();
            return isExecute;
        }

        public static Year GetById(int? id)
        {
            string rawKey = "YearByID" + id;
            Year year = GetCacheItem(rawKey);

            if (year == null)
            {
                year = RepositoryManager.Year_Repository.GetById(id);
                if (year != null)
                    AddCacheItem(rawKey,year);
            }

            return year;
        }

        public static List<Year> GetAll()
        {
            // return RepositoryAdmission.Program_Repository.GetAll();

            const string rawKey = "YearGetAll";

            List<Year> list = GetCacheAsList(rawKey);

            if (list == null)
            {
                // Item not found in cache - retrieve it and insert it into the cache
                list = RepositoryManager.Year_Repository.GetAll();
                if (list != null)
                    AddCacheItem(rawKey, list);
            }

            return list;
        }

        public static List<Year> GetByProgramId(int programId)
        {
            List<Year> list = RepositoryManager.Year_Repository.GetByProgramId(programId);
            return list;
        }

        public static List<Year> GetByProgramIdYearName(int programId, string yearName)
        {
            List<Year> list = RepositoryManager.Year_Repository.GetByProgramIdYearName(programId, yearName);
            return list;
        }

        public static bool IsYearNameExist(int programId, string yearName)
        {
            List<Year> list = GetByProgramIdYearName(programId, yearName).ToList();
            if (list == null && list.Count == 0)
            {
                return true;
            }
            else { return false; }
        }




        public static List<YearDistinctDTO> GetAllDistinct()
        {

            List<YearDistinctDTO> list = RepositoryManager.Year_Repository.GetAllDistinct();
            return list;
        }






    }
}

