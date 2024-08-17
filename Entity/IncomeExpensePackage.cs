using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    public class IncomeExpensePackage : BaseEntity
    {
        public IncomeExpensePackage()
        {
            ExpenseSummaryList = new List<ExpenseSummary>();
            PaymentSummaryDetailList = new List<PaymentSummaryDetail>();
            PaymentSummaryList = new List<PaymentSummary>();

        }
        public List<ExpenseSummary> ExpenseSummaryList { get; set; }

        public List<PaymentSummaryDetail> PaymentSummaryDetailList { get; set; }

        public List<PaymentSummary> PaymentSummaryList { get; set; }
    }
}
