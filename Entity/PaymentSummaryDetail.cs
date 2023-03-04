using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    public class PaymentSummaryDetail : BaseEntity    
    {
        public bool IsPayment { get; set; }
        public string PaymentTypeName { get; set; }
        public Decimal Amount { get; set; }

        public string AmountStr
        {
            get
            {
                if (Amount != 0)
                    return Amount.ToString(CommonConst.TL);
                else
                    return "-";
            }
        }

        public int Year { get; set; }
        public int Month { get; set; }
        public string Index { get; set; }


    }
}
