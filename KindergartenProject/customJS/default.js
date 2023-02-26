window.onload = function () {
    loadIncomeAndExpenseSummaryForCurrentMonth();
    loadIncomeAndExpenseSummaryForCurrentMonthDetail();
    loadExpenseSummaryForCurrentMonth();
};

function loadExpenseSummaryForCurrentMonth() {
    
    var jsonData = "{  }";
    CallServiceWithAjax('/KinderGartenWebService.asmx/Get_ExpenseSummaryWithMonthAndYear', jsonData, successFunctionGetExpenseSummaryForCurrentMonth, errorFunction);
}

function loadIncomeAndExpenseSummaryForCurrentMonth() {
    var jsonData = "{  }";
    CallServiceWithAjax('/KinderGartenWebService.asmx/Get_IncomeAndExpenseSummaryForCurrentMonth', jsonData, successFunctionGetIncomeAndExpenseSummaryForCurrentMonth, errorFunction);
}

function loadIncomeAndExpenseSummaryForCurrentMonthDetail() {
    var jsonData = "{  }";
    CallServiceWithAjax('/KinderGartenWebService.asmx/Get_IncomeAndExpenseSummaryForCurrentMonthDetail', jsonData, successFunctionGetIncomeAndExpenseSummaryForCurrentMonthDetail, errorFunction);
}

function resetDetail() {
    var rowIncoming = document.getElementById("trIncomingPaymentDetail");
    var rowWaiting = document.getElementById("trWaitingPaymentDetail");
    var rowExpense = document.getElementById("trExpensePaymentDetail");

    rowIncoming.style.display = 'none';
    rowWaiting.style.display = 'none';
    rowExpense.style.display = 'none';

    document.getElementById("tdIncomingPlus").innerHTML = "+";
    document.getElementById("tdWaitingPlus").innerHTML = "+";
    document.getElementById("tdExpensePlus").innerHTML = "+";

}

function onIncomingDetailRow() {
    resetDetail();
    var row = document.getElementById("trIncomingPaymentDetail");
    row.style.display = row.style.display === 'none' ? '' : 'none';
    if (row.style.display=='')
        document.getElementById("tdIncomingPlus").innerHTML = "-";
    else
        document.getElementById("tdIncomingPlus").innerHTML = "+";

}

function onWaitingDetailRow() {
    resetDetail();
    var row = document.getElementById("trWaitingPaymentDetail");
    row.style.display = row.style.display === 'none' ? '' : 'none';
    if (row.style.display == '')
        document.getElementById("tdWaitingPlus").innerHTML = "-";
    else
        document.getElementById("tdWaitingPlus").innerHTML = "+";

}

function onExpenseDetailRow() {
    resetDetail();
    var row = document.getElementById("trExpensePaymentDetail");
    row.style.display = row.style.display === 'none' ? '' : 'none';
    if (row.style.display == '')
        document.getElementById("tdExpensePlus").innerHTML = "-";
    else
        document.getElementById("tdExpensePlus").innerHTML = "+";

}

function setIncomeAndWaiting(isPayment, tblName,list) {
    if (list.length > 0) {
        let tbody = "";

        tbody += "<table class='table mb - 0'>";
        tbody += "<thead><tr><th scope='col'>Adı</th><th scope='col'>Ödeme Durumu</th><th scope='col'>Tutar</th></thead>";

        for (var i in list) {

            if (list[i].IsPayment == isPayment) {

                tbody += "<tr>";
                tbody += "<td>" + list[i].PaymentTypeName + "</td>";
                var status = "<td style='color:green;'>Ödendi</td>";
                if (!list[i].IsPayment)
                    status = "<td style='color:red;'>Ödenmedi   </td>";
                tbody += status;
                tbody += "<td>" + list[i].AmountStr + "</td>";
                tbody += "<tr>";
            }

        }

        tbody += "</table>";

        var tbl = document.getElementById(tblName);

        tbl.innerHTML = tbody;
    }
}

function successFunctionGetExpenseSummaryForCurrentMonth(obje) {
    if (!obje.HasError && obje.Result) {
        var list = obje.Result;

        if (list.length > 0) {
            let tbody = "";

            tbody += "<table class='table mb - 0'>";
            tbody += "<thead><tr><th scope='col'>Adı</th><th scope='col'>Durumu</th><th scope='col'>Tutar</th></thead>";

            for (var i in list) {


                tbody += "<tr>";
                tbody += "<td>" + list[i].ExpenseTypeName + "</td>";
                var status = "<td style='color:red;'>Gider   </td>";
                tbody += status;
                tbody += "<td>" + list[i].ExpenseAmountStr + "</td>";
                tbody += "<tr>";


            }

            tbody += "</table>";

            var tbl = document.getElementById("tblExpensePaymentDetail");

            tbl.innerHTML = tbody;
        }

       
    }
    else {
        alert("Hata var !!! Error : " + obje.ErrorDescription);
    }
}

function successFunctionGetIncomeAndExpenseSummaryForCurrentMonthDetail(obje) {
    if (!obje.HasError && obje.Result) {
        var list = obje.Result;

        setIncomeAndWaiting(true, "tblIncomingPaymentDetail", list);
        setIncomeAndWaiting(false, "tblWaitingPaymentDetail", list);
    }
    else {
        alert("Hata var !!! Error : " + obje.ErrorDescription);
    }
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
