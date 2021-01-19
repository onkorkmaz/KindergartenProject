using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Entity
{
    public class ResultSet
    {
        public bool IsValid ;
        public string ExceptionMessage;
        public Exception Exception;

        public DataTable DT;
    }
}
