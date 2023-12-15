using Common;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Utility
{
    public class AdminContext : Page
    {
        public AdminEntity AdminEntity
        {
            get
            {
                if ((Session[CommonConst.Admin] == null || Session[CommonConst.ProjectType] == null))
                {
                    Response.Redirect("/uye-giris");
                }
            }
        }

    }
}
