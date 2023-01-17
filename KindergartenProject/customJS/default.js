window.onload = function () {
    loadIncomeAndExpenseSummaryForCurrentMonth();

};

function loadIncomeAndExpenseSummaryForCurrentMonth() {
    var jsonData = "{  }";
    CallServiceWithAjax('/KinderGartenWebService.asmx/Get_IncomeAndExpenseSummaryForCurrentMonth', jsonData, successFunctionGetIncomeAndExpenseSummaryForCurrentMonth, errorFunction);
}

function successFunctionGetIncomeAndExpenseSummaryForCurrentMonth(obje) {
    if (!obje.HasError && obje.Result) {
        var list = obje.Result;
        if (list.length > 0) {

            const d = new Date();
            let month = d.getMonth();
            document.getElementById("currentMonth").innerHTML = "<b>" + months[month][1] + "</b>";

            var firstSummary = list[0];
            document.getElementById("incomeForStudentPayment").innerHTML = firstSummary.IncomeForStudentPaymentStr;
            document.getElementById("waitingIncomeForStudentPayment").innerHTML = firstSummary.WaitingIncomeForStudentPaymentStr;
            document.getElementById("incomeWithoutStudentPayment").innerHTML = firstSummary.IncomeWithoutStudentPaymentStr;
            document.getElementById("workerExpenses").innerHTML = firstSummary.WorkerExpensesStr;

            document.getElementById("expenseWithoutWorker").innerHTML = firstSummary.ExpenseWithoutWorkerStr;

            var currentBalance = document.getElementById("currentBalance");
            currentBalance.innerHTML = firstSummary.CurrentBalanceStr;
            if (firstSummary.CurrentBalance < 0) {
                currentBalance.style.color = "red";
            }
            else {
                currentBalance.style.color = "green";
            }

            var totalBalance = document.getElementById("totalBalance");
            totalBalance.innerHTML = firstSummary.TotalBalanceStr;
            if (firstSummary.TotalBalance < 0) {
                totalBalance.style.color = "red";
            }
            else {
                totalBalance.style.color = "green";
            }
        }
    }
    else {
        alert("Hata var !!! Error : " + obje.ErrorDescription);
    }
}
