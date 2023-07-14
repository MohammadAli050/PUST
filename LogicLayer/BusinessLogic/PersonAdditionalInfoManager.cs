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
    public class PersonAdditionalInfoManager
    {
        #region Cache

        public static readonly string[] MasterCacheKeyArray = { "PersonAdditionalInfoCache" };
        const double CacheDuration = 5.0;

        public static string GetCacheKey(string cacheKey)
        {
            return string.Concat(MasterCacheKeyArray[0], "-", cacheKey);
        }

        public static List<PersonAdditionalInfo> GetCacheAsList(string rawKey)
        {
            List<PersonAdditionalInfo> list = (List<PersonAdditionalInfo>)HttpRuntime.Cache[GetCacheKey(rawKey)];
            return list;
        }

        public static PersonAdditionalInfo GetCacheItem(string rawKey)
        {
            PersonAdditionalInfo item = (PersonAdditionalInfo)HttpRuntime.Cache[GetCacheKey(rawKey)];
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


        public static int Insert(PersonAdditionalInfo personadditionalinfo)
        {
            int id = RepositoryManager.PersonAdditionalInfo_Repository.Insert(personadditionalinfo);
            InvalidateCache();
            return id;
        }

        public static bool Update(PersonAdditionalInfo personadditionalinfo)
        {
            bool isExecute = RepositoryManager.PersonAdditionalInfo_Repository.Update(personadditionalinfo);
            InvalidateCache();
            return isExecute;
        }

        public static bool Delete(int id)
        {
            bool isExecute = RepositoryManager.PersonAdditionalInfo_Repository.Delete(id);
            InvalidateCache();
            return isExecute;
        }

        public static PersonAdditionalInfo GetByPersonId(int? id)
        {
            PersonAdditionalInfo personadditionalinfo = null;

            if (personadditionalinfo == null)
            {
                personadditionalinfo = RepositoryManager.PersonAdditionalInfo_Repository.GetByPersonId(id);
            }

            return personadditionalinfo;
        }

        public static List<PersonAdditionalInfo> GetAll()
        {
            // return RepositoryAdmission.Program_Repository.GetAll();


            // Item not found in cache - retrieve it and insert it into the cache
            List<PersonAdditionalInfo> list = RepositoryManager.PersonAdditionalInfo_Repository.GetAll();


            return list;
        }
    }
}
