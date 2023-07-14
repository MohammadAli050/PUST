﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using LogicLayer.BusinessObjects;
using LogicLayer.DataLogic.DAFactory;

namespace LogicLayer.BusinessLogic
{
    public class FundTypeManager
    {
        #region Cache

        public static readonly string[] MasterCacheKeyArray = { "FundTypeCache" };
        const double CacheDuration = 5.0;

        public static string GetCacheKey(string cacheKey)
        {
            return string.Concat(MasterCacheKeyArray[0], "-", cacheKey);
        }

        public static List<FundType> GetCacheAsList(string rawKey)
        {
            List<FundType> list = (List<FundType>)HttpRuntime.Cache[GetCacheKey(rawKey)];
            return list;
        }

        public static FundType GetCacheItem(string rawKey)
        {
            FundType item = (FundType)HttpRuntime.Cache[GetCacheKey(rawKey)];
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


        public static int Insert(FundType fundtype)
        {
            int id = RepositoryManager.FundType_Repository.Insert(fundtype);
            InvalidateCache();
            return id;
        }

        public static bool Update(FundType fundtype)
        {
            bool isExecute = RepositoryManager.FundType_Repository.Update(fundtype);
            InvalidateCache();
            return isExecute;
        }

        public static bool Delete(int id)
        {
            bool isExecute = RepositoryManager.FundType_Repository.Delete(id);
            InvalidateCache();
            return isExecute;
        }

        public static FundType GetById(int? id)
        {
            string rawKey = "FundTypeByID" + id;
            FundType fundtype = GetCacheItem(rawKey);

            if (fundtype == null)
            {
                fundtype = RepositoryManager.FundType_Repository.GetById(id);
                if (fundtype != null)
                    AddCacheItem(rawKey, fundtype);
            }

            return fundtype;
        }

        public static List<FundType> GetAll()
        {
            return RepositoryManager.FundType_Repository.GetAll();
        }
    }
}

