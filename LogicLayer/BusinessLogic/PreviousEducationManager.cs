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
    public class PreviousEducationManager
    {
        #region Cache

        public static readonly string[] MasterCacheKeyArray = { "PreviousEducationCache" };
        const double CacheDuration = 5.0;

        public static string GetCacheKey(string cacheKey)
        {
            return string.Concat(MasterCacheKeyArray[0], "-", cacheKey);
        }

        public static List<PreviousEducation> GetCacheAsList(string rawKey)
        {
            List<PreviousEducation> list = (List<PreviousEducation>)HttpRuntime.Cache[GetCacheKey(rawKey)];
            return list;
        }

        public static PreviousEducation GetCacheItem(string rawKey)
        {
            PreviousEducation item = (PreviousEducation)HttpRuntime.Cache[GetCacheKey(rawKey)];
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


        public static int Insert(PreviousEducation previouseducation)
        {
            int id = RepositoryManager.PreviousEducation_Repository.Insert(previouseducation);
            InvalidateCache();
            return id;
        }

        public static bool Update(PreviousEducation previouseducation)
        {
            bool isExecute = RepositoryManager.PreviousEducation_Repository.Update(previouseducation);
            InvalidateCache();
            return isExecute;
        }

        public static bool Delete(int id)
        {
            bool isExecute = RepositoryManager.PreviousEducation_Repository.Delete(id);
            InvalidateCache();
            return isExecute;
        }

        public static PreviousEducation GetById(int? id)
        {
            PreviousEducation previouseducation = new PreviousEducation();

            previouseducation = RepositoryManager.PreviousEducation_Repository.GetById(id);


            return previouseducation;
        }

        public static List<PreviousEducation> GetAll()
        {
            List<PreviousEducation> list = new List<PreviousEducation>();

            list = RepositoryManager.PreviousEducation_Repository.GetAll();


            return list;
        }

        public static List<PreviousEducation> GetAllByPersonId(int PersonId)
        {
            List<PreviousEducation> list = RepositoryManager.PreviousEducation_Repository.GetAllByPersonId(PersonId);
            return list;
        }
    }
}

