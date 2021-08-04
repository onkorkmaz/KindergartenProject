using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Entity
{
    public class ClassEntity : BaseEntity
    {
        private String name;
        public String Name
        {
            get { return name; }
            set
            {
                name = value;
            }
        }
        private String description;
        public String Description
        {
            get { return description; }
            set
            {
                description = value;
            }
        }
        private Int32? warningofstudentcount;
        public Int32? WarningOfStudentCount
        {
            get { return warningofstudentcount; }
            set
            {
                warningofstudentcount = value;
            }
        }
    }
}

