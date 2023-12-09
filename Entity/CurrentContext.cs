using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    public class CurrentContext
    {
        public CurrentContext()
        {
            AdminEntity = new AdminEntity();
        }
        public static AdminEntity AdminEntity { get; set; }

        public static ProjectType ProjectType { get; set; }

        public static AuthorityScreenEnum ScreenAuthorityEnum { get; set; }
    }
}
