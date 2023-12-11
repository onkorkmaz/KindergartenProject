using System;
using System.Collections.Generic;
using System.Text;
using Entity;
using Common;
using System.Linq;

namespace Business
{
    public class CacheBusines<T> where T : BaseEntity  
    {
        public ProjectType ProjectType;
        private CacheType _cacheType = CacheType.None;
        private static  Dictionary<string, CacheEntity<T>> dictCacheType = new Dictionary<string, CacheEntity<T>>();

        public CacheBusines(ProjectType projectType,CacheType cacheType) 
        {
            _cacheType = cacheType;
            ProjectType = projectType;
            string key = getKey();

            if (!dictCacheType.ContainsKey(key))
            {
                dictCacheType.Add(key, new CacheEntity<T>(cacheType));                
            }
        }

        public List<T> CacheList
        {
            get
            {
                string key = getKey();
                if (dictCacheType.ContainsKey(key))
                {
                    var result = dictCacheType[key];
                    List<T> cacheList = result.List;
                    if (cacheList != null && result.EndDate > DateTime.Now)
                    {
                        return cacheList;
                    }
                    else
                    {
                        return dictCacheType[key].List = new List<T>();
                    }
                }
                else
                {
                    dictCacheType.Add(key, new CacheEntity<T>(_cacheType));
                    dictCacheType[key].List = new List<T>();
                    return dictCacheType[key].List;
                }
            }
        }

        public bool IsCacheAvailable
        {
            get
            {
                string key = getKey();
                if (dictCacheType.ContainsKey(key))
                {
                    var result = dictCacheType[key];
                    List<T> cacheList = result.List;
                    return cacheList != null && cacheList.Count > 0 && result.EndDate > DateTime.Now;
                }
                return false;
            }
        }

        public void UpdateCache(T modifyRecord)
        {
            if (!IsCacheAvailable || modifyRecord == null)
            {
                return;
            }

            string key = getKey();
            CacheEntity<T> cache = dictCacheType[key];
            if (cache != null)
            {
                object list = cache.List;
                List<T> cacheList = (List<T>)list;
                if (IsCacheAvailable)
                {
                    int index = cacheList.FindIndex(o => o.Id == modifyRecord.Id);
                    if (modifyRecord.DatabaseProcess == DatabaseProcess.Deleted)
                    {
                        if (index > 0)
                        {
                            cacheList.RemoveAt(index);
                        }
                    }
                    else
                    {
                        if (index > 0)
                        {
                            cacheList.RemoveAt(index);
                            cacheList.Insert(index, modifyRecord);
                        }
                        else
                        {
                            cacheList.Add(modifyRecord);
                        }
                    }
                }
            }
        }

        private string getKey()
        {
            return CurrentContext.AdminEntity.Id.ToString() + "_" + (int)ProjectType + "_" + (int)_cacheType;
        }

        public void ClearCache(CacheType cacheType)
        {
            string key = getKey();
            if (dictCacheType.ContainsKey(key))
            {
                dictCacheType[key] = new CacheEntity<T>(cacheType);
            }
        }
        public void AddCacheListInDictionary(CacheType cacheType,List<T> list)
        {
            string key = getKey();
            dictCacheType[key] = new CacheEntity<T>(cacheType);
            dictCacheType[key].List = list;
        }
    }
}
