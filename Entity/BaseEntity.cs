using Common;
using System;

namespace Entity
{
    public class BaseEntity
    {
        public DatabaseProcess DatabaseProcess { get; set; }

        public string EncryptId
        {
            get
            {
                if (Id.HasValue && Id > 0)
                {
                    return Cipher.Encrypt(Id.Value.ToString());
                }
                return "";
            }
        }

        public int? Id { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted{ get; set; }
        public DateTime AddedOn { get; set; }
        public DateTime DeletedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

    }
}
