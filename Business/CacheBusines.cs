﻿using System;
using System.Collections.Generic;
using System.Text;
using Entity;
using Common;
using System.Linq;


namespace Business
{
    public class CacheBusines<T> where T : BaseEntity  
    {
        private ProjectType _projectType;
        private CacheType _cacheType = CacheType.None;
        private const int _cacheMinutes = 60;
        private static Dictionary<ProjectType, Dictionary<CacheType, CacheEntity<T>>> _dictProjectType = new Dictionary<ProjectType, Dictionary<CacheType, CacheEntity<T>>>();

        private Dictionary<CacheType, CacheEntity<T>> dictCacheType
        {
            get
            {
                return _dictProjectType[_projectType];
            }
        }


        public CacheBusines(ProjectType projectType,CacheType cacheType) 
        {
            _cacheType = cacheType;
            _projectType = projectType;
            if (!_dictProjectType.ContainsKey(_projectType))
            {
                _dictProjectType.Add(_projectType, new Dictionary<CacheType, CacheEntity<T>>());                
            }


            if (!_dictProjectType[_projectType].ContainsKey(cacheType))
            {
                _dictProjectType[_projectType].Add(cacheType, new CacheEntity<T>(cacheType));
            }

        }

        public List<T> CacheList
        {
            get
            {
                if (IsCacheAvailable)
                {
                    return _cacheList.List;
                }

                return new List<T>();
            }
        }

        private CacheEntity<T> _cacheList
        {
            get
            {
                if(IsCacheAvailable)
                {
                    return dictCacheType[_cacheType];
                }

                return null;
            }
        }

        public bool IsCacheAvailable
        {
            get
            {
                if (dictCacheType.ContainsKey(_cacheType))
                {
                    var result = dictCacheType[_cacheType];
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

            CacheEntity<T> cache = _cacheList;            

            if (cache != null)
            {
                object list = cache.List;
                List<T> cacheList = (List<T>)list;
                if (IsCacheAvailable && modifyRecord.DatabaseProcess == DatabaseProcess.Add)
                {
                    if (cacheList.Last().Id > modifyRecord.Id)
                    {
                        modifyRecord.DatabaseProcess = DatabaseProcess.Update;
                    }
                }

                if (modifyRecord.DatabaseProcess == DatabaseProcess.Add)
                {
                    cacheList.Add(modifyRecord);
                }
                else if (modifyRecord.DatabaseProcess == DatabaseProcess.Update)
                {
                    int index = cacheList.FindIndex(o => o.Id == modifyRecord.Id);
                    cacheList.RemoveAt(index);
                    cacheList.Insert(index, modifyRecord);
                }
                else if (modifyRecord.DatabaseProcess == DatabaseProcess.Deleted)
                {
                    int index = cacheList.FindIndex(o => o.Id == modifyRecord.Id);
                    cacheList.RemoveAt(index);
                }
            }
        }

        public void ClearCache(CacheType cacheType)
        {
            if (dictCacheType.ContainsKey(cacheType))
            {
                dictCacheType[cacheType] = new CacheEntity<T>(cacheType);
            }
        }

        public void AddCache(CacheType cacheType,List<T> list)
        {
            dictCacheType[cacheType] = new CacheEntity<T>(cacheType);
            dictCacheType[cacheType].List = list;
        }
    }
}