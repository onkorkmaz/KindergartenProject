using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Entity
{
    public class SearchEntity : BaseEntity
    {
        public SearchEntity()
        {
            IsActive = null;
            IsDeleted = null;
        }

        public string Username;
        public string Tckn;
    }
}