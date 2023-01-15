using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    public class PaymentSummary : BaseEntity    
    {
        public decimal Income { get; set; }
        public decimal WaitingInCome { get; set; }
        public decimal WorkerExpenses { get; set; }
    }
}
