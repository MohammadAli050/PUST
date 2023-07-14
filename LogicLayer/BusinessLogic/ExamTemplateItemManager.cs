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
    public class ExamTemplateItemManager
    {
        #region Cache

        public static readonly string[] MasterCacheKeyArray = { "ExamTemplateItemCache" };
        const double CacheDuration = 5.0;

        public static string GetCacheKey(string cacheKey)
        {
            return string.Concat(MasterCacheKeyArray[0], "-", cacheKey);
        }

        public static List<ExamTemplateItem> GetCacheAsList(string rawKey)
        {
            List<ExamTemplateItem> list = (List<ExamTemplateItem>)HttpRuntime.Cache[GetCacheKey(rawKey)];
            return list;
        }

        public static ExamTemplateItem GetCacheItem(string rawKey)
        {
            ExamTemplateItem item = (ExamTemplateItem)HttpRuntime.Cache[GetCacheKey(rawKey)];
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


        public static int Insert(ExamTemplateItem examtemplateitem)
        {
            int id = RepositoryManager.ExamTemplateItem_Repository.Insert(examtemplateitem);
            InvalidateCache();
            return id;
        }

        public static bool Update(ExamTemplateItem examtemplateitem)
        {
            bool isExecute = RepositoryManager.ExamTemplateItem_Repository.Update(examtemplateitem);
            InvalidateCache();
            return isExecute;
        }

        public static bool Delete(int id)
        {
            bool isExecute = RepositoryManager.ExamTemplateItem_Repository.Delete(id);
            InvalidateCache();
            return isExecute;
        }

        public static ExamTemplateItem GetById(int? id)
        {
            ExamTemplateItem examtemplateitem =  RepositoryManager.ExamTemplateItem_Repository.GetById(id);
            return examtemplateitem;
        }

        public static List<ExamTemplateItem> GetAll()
        {
            List<ExamTemplateItem> list =  RepositoryManager.ExamTemplateItem_Repository.GetAll();
            return list;
        }

        public static bool GetByExamNameColumnSequence(int examTemplateId, decimal examMark, int columnSequence)
        {
            ExamTemplateItem examtemplateitem = RepositoryManager.ExamTemplateItem_Repository.GetByExamNameColumnSequence(examTemplateId, examMark, columnSequence);
            if (examtemplateitem != null)
            {
                return false;
            }
            else { return true; }
        }

        public static List<ExamTemplateItem> GetByExamTemplateId(int examTemplateId)
        {
            List<ExamTemplateItem> list = RepositoryManager.ExamTemplateItem_Repository.GetByExamTemplateId(examTemplateId);
            return list;
        }

        public static List<ExamTemplateItem> GetBasicWithOutFinalTemplateItemByExamTemplateId(int examTemplateId)
        {
            List<ExamTemplateItem> list = RepositoryManager.ExamTemplateItem_Repository.GetBasicWithOutFinalTemplateItemByExamTemplateId(examTemplateId);
            return list;
        }
    }
}

