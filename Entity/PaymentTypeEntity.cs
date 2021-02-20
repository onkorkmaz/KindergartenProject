using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Entity
{
    public class PaymentTypeEntity : BaseEntity
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
        private Decimal? amount;
        public Decimal? Amount
        {
            get { return amount; }
            set
            {
                amount = value;
            }
        }
    }
}

