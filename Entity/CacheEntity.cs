using System;
using System.Collections.Generic;
using System.Text;
using Common;

namespace Entity
{
    public class CacheEntity<T> : BaseEntity
    {
        public CacheType _cacheType { get; set; }

        public CacheEntity(CacheType cacheType)
        {
            _createDate = DateTime.Now;
            _cacheType = cacheType;
        }
        public const int CacheMinutes = 60;

        public List<T> List { get; set; }

        private DateTime _createDate;
        public DateTime CreateDate 
        { 
            get
            {
                return _createDate;
            }
        }

        public DateTime EndDate
        {
            get
            {
                return _createDate.AddMinutes(CacheMinutes);
            }
        }
    }
}
