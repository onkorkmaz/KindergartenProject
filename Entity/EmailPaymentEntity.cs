using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    public class EmailPaymentEntity
    {
        public int Year { get; set; }

        public int Month { get; set; }

        public int PaymentTypeId { get; set; }

        public string AmountDescription { get; set; }

        public string AmountColor { get; set; }

        public decimal Amount { get; set; }

        public bool IsPayment { get; set; }

    }
}
