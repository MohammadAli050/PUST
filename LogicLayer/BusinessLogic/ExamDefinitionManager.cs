using LogicLayer.BusinessObjects;
using LogicLayer.DataLogic.DAFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LogicLayer.BusinessLogic
{
    public class ExamDefinitionManager
    {
        #region Cache

        public static readonly string[] MasterCacheKeyArray = { "ExamDefinitionCache" };
        const double CacheDuration = 5.0;

        public static string GetCacheKey(string cacheKey)
        {
            return string.Concat(MasterCacheKeyArray[0], "-", cacheKey);
        }

        public static List<ExamDefinition> GetCacheAsList(string rawKey)
        {
            List<ExamDefinition> list = (List<ExamDefinition>)HttpRuntime.Cache[GetCacheKey(rawKey)];
            return list;
        }

        public static ExamDefinition GetCacheItem(string rawKey)
        {
            ExamDefinition item = (ExamDefinition)HttpRuntime.Cache[GetCacheKey(rawKey)];
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


        public static int Insert(ExamDefinition examdefinition)
        {
            int id = RepositoryManager.ExamDefinition_Repository.Insert(examdefinition);
            InvalidateCache();
            return id;
        }

        public static bool Update(ExamDefinition examdefinition)
        {
            bool isExecute = RepositoryManager.ExamDefinition_Repository.Update(examdefinition);
            InvalidateCache();
            return isExecute;
        }

        public static bool Delete(int id)
        {
            bool isExecute = RepositoryManager.ExamDefinition_Repository.Delete(id);
            InvalidateCache();
            return isExecute;
        }

        public static ExamDefinition GetById(int? id)
        {
            string rawKey = "ExamDefinitionByID" + id;
            ExamDefinition examdefinition   = RepositoryManager.ExamDefinition_Repository.GetById(id);
                

            return examdefinition;
        }

        public static List<ExamDefinition> GetAll()
        {
            // return RepositoryAdmission.Program_Repository.GetAll();

            const string rawKey = "ExamDefinitionGetAll";

            List<ExamDefinition> list = GetCacheAsList(rawKey);

            if (list == null)
            {
                // Item not found in cache - retrieve it and insert it into the cache
                list = RepositoryManager.ExamDefinition_Repository.GetAll();
                if (list != null)
                    AddCacheItem(rawKey, list);
            }

            return list;
        }

        public static ExamDefinition GetByAcaCalIdExamTypeId(int? acaCalId, int? ExamTypeId)
        {
            ExamDefinition ed = RepositoryManager.ExamDefinition_Repository.GetByAcaCalIdExamTypeId(acaCalId, ExamTypeId);
            return ed;
        }

        public static ExamDefinition GetByAcaCalId(int acaCalId)
        {
            ExamDefinition ed = RepositoryManager.ExamDefinition_Repository.GetByAcaCalId(acaCalId);
            return ed;
        }
        

    }
}
