using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    public class ExpenseSummary : BaseEntity    
    {
        public int ExpenseTypeId { get; set; }
        public string ExpenseTypeName { get; set; }

        public Decimal ExpenseAmount { get; set; }

        public string ExpenseAmountStr
        {
            get
            {
                if (ExpenseAmount != 0)
                    return ExpenseAmount.ToString(CommonConst.TL);
                else
                    return "0";
            }
        }

        public int Year { get; set; }
        public int Month { get; set; }
        public string Index { get; set; }

    }
}
