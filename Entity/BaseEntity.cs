using Common;
using System;

namespace Entity
{
    public class BaseEntity
    {
        public DatabaseProcess DatabaseProcess { get; set; }

        public int Id { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted{ get; set; }
        public DateTime AddedOn { get; set; }

        public string AddedOnStr
        {
            get
            {
                return AddedOn.ToString("yyyy-MM-ddTHH:mm:ss");
            }
        }
        public DateTime DeletedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public ProjectType ProjectType { get; set; }

        public Int16 ProjectTypeInt
        { 
            get
            {
                return (Int16)ProjectType;
            }
        }

        public string ProjectTypeDescription 
        { 
            get
            {
                return CommonFunctions.GetProjectType(ProjectType);
            }
        }


    }
}
