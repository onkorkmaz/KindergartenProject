using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    public class PaymentSummary : BaseEntity
    {
        public decimal IncomeForStudentPayment { get; set; }

        public string IncomeForStudentPaymentStr
        {
            get
            {
                if (IncomeForStudentPayment != 0)
                    return IncomeForStudentPayment.ToString(CommonConst.TL);
                else
                    return "0";
            }

        }

        public string EndorsmentForStudentPaymentStr
        {
            get
            {
                return (IncomeForStudentPayment + IncomeWithoutStudentPayment).ToString(CommonConst.TL);
            }
        }

        public string TotalExpenseStr
        {
            get
            {
                return (WorkerExpenses + ExpenseWithoutWorker).ToString(CommonConst.TL);
            }
        }

    public decimal WaitingIncomeForStudentPayment { get; set; }

        public string WaitingIncomeForStudentPaymentStr
        {
            get
            {
                if (WaitingIncomeForStudentPayment != 0)
                    return WaitingIncomeForStudentPayment.ToString(CommonConst.TL);
                else
                    return "0";
            }

        }


        public decimal IncomeWithoutStudentPayment { get; set; }

        public string IncomeWithoutStudentPaymentStr
        {
            get
            {
                if (IncomeWithoutStudentPayment != 0)
                    return IncomeWithoutStudentPayment.ToString(CommonConst.TL);
                else
                    return "0";
            }
        }


        public decimal WorkerExpenses { get; set; }

        public string WorkerExpensesStr
        {
            get
            {
                if (WorkerExpenses != 0)
                    return WorkerExpenses.ToString(CommonConst.TL);
                else
                    return "0";
            }
        }

        public decimal ExpenseWithoutWorker{ get; set; }

        public string ExpenseWithoutWorkerStr
        {
            get
            {
                if (ExpenseWithoutWorker != 0)
                    return ExpenseWithoutWorker.ToString(CommonConst.TL);
                else
                    return "0";
            }
        }


        public decimal CurrentBalance { get; set; }

        public string CurrentBalanceStr
        {
            get
            {
                if (CurrentBalance != 0)
                    return CurrentBalance.ToString(CommonConst.TL);
                else
                    return "0";
            }
        }

        public decimal TotalBalance { get; set; }

        public string TotalBalanceStr
        {
            get
            {
                if (TotalBalance != 0)
                    return TotalBalance.ToString(CommonConst.TL);
                else
                    return "0";
            }
        }

        public int Year { get; set; }
        public int Month { get; set; }

        public int MonthIndex { get; set; }

    }
}
