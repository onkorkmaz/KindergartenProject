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
        private Int32? mainteachercode;
        public Int32? MainTeacherCode
        {
            get { return mainteachercode; }
            set
            {
                mainteachercode = value;
            }
        }
        private Int32? helperteachercode;
        public Int32? HelperTeacherCode
        {
            get { return helperteachercode; }
            set
            {
                helperteachercode = value;
            }
        }
        private String mainteacher;
        public String MainTeacher
        {
            get { return mainteacher; }
            set
            {
                mainteacher = value;
            }
        }
        private String helperteacher;
        public String HelperTeacher
        {
            get { return helperteacher; }
            set
            {
                helperteacher = value;
            }
        }


        public bool IsActiveMainTeacher { get; set; }
        public bool IsActiveHelperTeacer { get; set; }

        public string ClassAndMainTeacherName
        {
            get
            {
                string result = "";
                if (Id > 0)
                {
                    result = name + " - " + mainteacher + " (" + StudentCount + ") ";
                }
                else
                {
                    return name;
                }

                return result;
            }
        }

        public string StudentCount { get; set; }

        public string TeacherOutGoing { get; set; }

        public string StudentIncome { get; set; }

        public string StudentCurrentIncome { get; set; }


    }
}

