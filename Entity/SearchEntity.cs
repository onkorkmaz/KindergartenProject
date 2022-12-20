using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Entity
{
    public class SearchEntity
    {
        public SearchEntity()
        {
            IsActive = null;
            IsDeleted = null;
        }

        public int? Id { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }

        public string Username;
        public string Tckn;
    }
}