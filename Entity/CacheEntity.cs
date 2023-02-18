using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    public class CacheEntity<T> : BaseEntity
    {
        public CacheEntity()
        {
            _createDate = DateTime.Now;
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
