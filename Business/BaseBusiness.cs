using Common;
using Entity;
using System;
using System.Collections.Generic;
using System.Text;


namespace Business
{
    public abstract class BaseBusiness 
    {
        protected ProjectType ProjectType;
        public Int16 ProjectTypeInt16
        {
            get
            {
                return (Int16)ProjectType;
            }
        }
        public BaseBusiness(ProjectType projectType)
        {
            this.ProjectType = projectType;
            ValidateProcessProjectType();
        }

        private void ValidateProcessProjectType()
        {
            if(ProjectType == Common.ProjectType.None)
            {
                //throw new Exception("Database işlemlerinde projectType alanını doldurmak zorundasınız!!!");
            }
        }

    }
}
