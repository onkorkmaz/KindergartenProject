using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    public class PaymentSummary : BaseEntity    
    {
        public decimal Incoming { get; set; }
        public decimal WaitingInComing { get; set; }
        public decimal WorkersExpenses { get; set; }
    }
}
