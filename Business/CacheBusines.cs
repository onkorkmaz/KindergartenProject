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
        private bool _isUseIdKey;
        private int _id;
        public ProjectType ProjectType;
        private CacheType _cacheType = CacheType.None;
        private Dictionary<string, CacheEntity<T>> _dictCacheType = new Dictionary<string, CacheEntity<T>>();

        public CacheBusines(ProjectType projectType,CacheType cacheType,int id,bool isUseIdKey) 
        {
            _cacheType = cacheType;
            ProjectType = projectType;
            _id = id;
            _isUseIdKey = isUseIdKey;
            string key = getKey();

            if (!_dictCacheType.ContainsKey(key))
            {
                _dictCacheType.Add(key, new CacheEntity<T>(cacheType));                
            }
        }

        public List<T> CacheList
        {
            get
            {
                string key = getKey();
                if (_dictCacheType.ContainsKey(key))
                {
                    var result = _dictCacheType[key];
                    List<T> cacheList = result.List;
                    if (cacheList != null && result.EndDate > DateTime.Now)
                    {
                        return cacheList;
                    }
                    else
                    {
                        return _dictCacheType[key].List = new List<T>();
                    }
                }
                else
                {
                    _dictCacheType.Add(key, new CacheEntity<T>(_cacheType));
                    _dictCacheType[key].List = new List<T>();
                    return _dictCacheType[key].List;
                }
            }
        }

        public bool IsCacheAvailable
        {
            get
            {
                string key = getKey();
                if (_dictCacheType.ContainsKey(key))
                {
                    var result = _dictCacheType[key];
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
            CacheEntity<T> cache = _dictCacheType[key];
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
            if(_isUseIdKey)
                return _id.ToString() + "_" + (int)ProjectType + "_" + (int)_cacheType;
            else
                return (int)ProjectType + "_" + (int)_cacheType;

        }

        public void ClearCache(CacheType cacheType)
        {
            string key = getKey();
            if (_dictCacheType.ContainsKey(key))
            {
                _dictCacheType[key] = new CacheEntity<T>(cacheType);
            }
        }
        public void AddCacheListInDictionary(CacheType cacheType,List<T> list)
        {
            string key = getKey();
            _dictCacheType[key] = new CacheEntity<T>(cacheType);
            _dictCacheType[key].List = list;
        }
    }
}
