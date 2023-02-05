window.onload = function () {
    loadIncomeAndExpenseSummaryForCurrentMonth();
    loadIncomeAndExpenseSummaryForCurrentMonthDetail();
};

function loadIncomeAndExpenseSummaryForCurrentMonth() {
    var jsonData = "{  }";
    CallServiceWithAjax('/KinderGartenWebService.asmx/Get_IncomeAndExpenseSummaryForCurrentMonth', jsonData, successFunctionGetIncomeAndExpenseSummaryForCurrentMonth, errorFunction);
}

function loadIncomeAndExpenseSummaryForCurrentMonthDetail() {
    var jsonData = "{  }";
    CallServiceWithAjax('/KinderGartenWebService.asmx/Get_IncomeAndExpenseSummaryForCurrentMonthDetail', jsonData, successFunctionGetIncomeAndExpenseSummaryForCurrentMonthDetail, errorFunction);
}

function onDetailRow() {
    var row = document.getElementById("trPaymentDetail");
    row.style.display = row.style.display === 'none' ? '' : 'none';

    if (row.style.display=='')
        document.getElementById("tdPlus").innerHTML = "-";
    else
        document.getElementById("tdPlus").innerHTML = "+";

}

function successFunctionGetIncomeAndExpenseSummaryForCurrentMonthDetail(obje) {
    if (!obje.HasError && obje.Result) {
        var list = obje.Result;
        if (list.length > 0) {
            let tbody = "";

            tbody += "<table class='table mb - 0'>";
            tbody += "<thead><tr><th scope='col'>Adı</th><th scope='col'>Ödeme Durumu</th><th scope='col'>Tutar</th></thead>";

            for (var i in list) {
                tbody += "<tr>";
                tbody += "<td>" + list[i].PaymentTypeName + "</td>";
                var status = "<td style='color:green;'>Ödendi</td>";
                if (!list[i].IsPayment)
                    status = "<td style='color:red;'>Ödenmedi   </td>";
                tbody +=  status ;
                tbody += "<td>" + list[i].AmountStr  + "</td>";
                tbody += "<tr>";

            }

            tbody += "</table>";

            tblPaymentDetail.innerHTML = tbody;
        }
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
