
function resetDetail() {

    for (var i = 0; i < 3; i++) {

        var rowIncoming = document.getElementById("trIncomingPaymentDetail" + i);
        if (rowIncoming != undefined) {
            rowIncoming.style.display = 'none';
            document.getElementById("tdIncomingPlus" + i).innerHTML = "<a href = \"#\">+</a>";
        }

        var rowWaiting = document.getElementById("trWaitingPaymentDetail" + i);
        if (rowWaiting != undefined) {
            rowWaiting.style.display = 'none';
            document.getElementById("tdWaitingPlus" + i).innerHTML = "<a href = \"#\">+</a>";
        }

        var rowExpense = document.getElementById("trExpensePaymentDetail" + i);
        if (rowExpense != undefined) {
            rowExpense.style.display = 'none';
            document.getElementById("tdExpensePlus" + i).innerHTML = "<a href = \"#\">+</a>";
        }
    }
}

function onIncomingDetailRow(index) {

    onCommonDetailRow(index, "trIncomingPaymentDetail", "tdIncomingPlus");

}

function onWaitingDetailRow(index) {

    onCommonDetailRow(index, "trWaitingPaymentDetail", "tdWaitingPlus");

}

function onCommonDetailRow(index, trName, tdName) {

    var row = document.getElementById(trName + index);
    var style = row.style.display;
    resetDetail();

    row.style.display = style === 'none' ? '' : 'none';
    if (row.style.display == '')
        document.getElementById(tdName + index).innerHTML = "<a href = \"#\">-</a>";
    else
        document.getElementById(tdName + index).innerHTML = "<a href = \"#\">+</a>";

}

function onExpenseDetailRow(index) {

    onCommonDetailRow(index, "trExpensePaymentDetail", "tdExpensePlus");
}

function loadExpenseSummaryDetailWithYearAndMonth(year, month, index) {

    var jsonData = "{year:" + JSON.stringify(year) + ",month:" + JSON.stringify(month) + ",index:" + JSON.stringify(index) + "}";
    CallServiceWithAjax('/KinderGartenWebService.asmx/Get_ExpenseSummaryDetailWithYearAndMonth', jsonData, successFunctionGetExpenseSummaryDetailWithYearAndMonth, errorFunction);
}

function successFunctionGetExpenseSummaryDetailWithYearAndMonth(obje) {
    if (!obje.HasError && obje.Result) {
        var list = obje.Result;

        if (list.length > 0) {
            let tbody = "";

            tbody += "<table border='3' style='border-style:solid; border-color: #343a40;' class='table mb - 0'>";
            tbody += "<thead><tr><th scope='col'>Adı</th><th scope='col'>Durumu</th><th scope='col'>Tutar</th></thead>";
            var index = "";
            for (var i in list) {
                tbody += "<tr>";
                tbody += "<td>" + list[i].ExpenseTypeName + "</td>";
                var status = "<td style='color:red;'>Gider   </td>";
                tbody += status;
                tbody += "<td>" + list[i].ExpenseAmountStr + "</td>";
                tbody += "<tr>";
                index = list[i].Index;
            }

            tbody += "</table>";
            if (IsNullOrEmpty(index)) {
                var tbl = document.getElementById("tblExpensePaymentDetail");
                if (tbl != null) {
                    tbl.innerHTML = tbody;
                }
            }
            else {
                var tbl = document.getElementById("tblExpensePaymentDetail" + index);
                if (tbl != null) {
                    tbl.innerHTML = tbody;
                }
            }
        }
    }
    else {
        alert("Hata var !!! Error : " + obje.ErrorDescription);
    }
}

function loadPaymentSummaryDetailWithYearAndMonth(year, month, index) {

    var jsonData = "{year:" + JSON.stringify(year) + ",month:" + JSON.stringify(month) + ",index:" + JSON.stringify(index) + "}";
    CallServiceWithAjax('/KinderGartenWebService.asmx/Get_PaymentSummaryDetailWithYearAndMonth', jsonData, successFunctionGetPaymentSummaryDetailWithYearAndMonth, errorFunction);

}

function successFunctionGetPaymentSummaryDetailWithYearAndMonth(obje) {
    if (!obje.HasError && obje.Result) {
        var list = obje.Result;

        setIncomeAndWaiting(true, "tblIncomingPaymentDetail", list);
        setIncomeAndWaiting(false, "tblWaitingPaymentDetail", list);
    }
    else {
        alert("Hata var !!! Error : " + obje.ErrorDescription);
    }
}

function loadIncomeAndExpenseSummaryWithYearAndMonth(year, month, index) {

    var jsonData = "{year:" + JSON.stringify(year) + ",month:" + JSON.stringify(month) + ",index:" + JSON.stringify(index) + "}";
    CallServiceWithAjax('/KinderGartenWebService.asmx/Get_IncomeAndExpenseSummaryWithYearAndMonth', jsonData, successFunctionGetIncomeAndExpenseSummaryWithYearAndMonth, errorFunction);

}

function successFunctionGetIncomeAndExpenseSummaryWithYearAndMonth(obje) {
    if (!obje.HasError && obje.Result) {
        var list = obje.Result;
        if (list.length > 0) {

            var firstSummary = list[0];

            var index = firstSummary.Index;

            document.getElementById("currentMonth" + index).innerHTML = "<b>" + months[firstSummary.Month - 1][1] + "</b>";
            document.getElementById("incomeForStudentPayment" + index).innerHTML = firstSummary.IncomeForStudentPaymentStr;
            document.getElementById("waitingIncomeForStudentPayment" + index).innerHTML = firstSummary.WaitingIncomeForStudentPaymentStr;
            document.getElementById("incomeWithoutStudentPayment" + index).innerHTML = firstSummary.IncomeWithoutStudentPaymentStr;
            document.getElementById("workerExpenses" + index).innerHTML = firstSummary.WorkerExpensesStr;
            document.getElementById("expenseWithoutWorker" + index).innerHTML = firstSummary.ExpenseWithoutWorkerStr;
            var currentBalance = document.getElementById("currentBalance" + index);
            currentBalance.innerHTML = firstSummary.CurrentBalanceStr;
            if (firstSummary.CurrentBalance < 0) {
                currentBalance.style.color = "red";
            }
            else {
                currentBalance.style.color = "green";
            }

            var totalBalance = document.getElementById("totalBalance" + index);
            totalBalance.innerHTML = firstSummary.TotalBalanceStr;
            if (firstSummary.TotalBalance < 0) {
                totalBalance.style.color = "red";
            }
            else {
                totalBalance.style.color = "green";
            }

            var incomeForStudentPayment = firstSummary.IncomeForStudentPayment;
            var incomeWithoutStudentPayment = firstSummary.IncomeWithoutStudentPayment;
            var workerExpenses = firstSummary.WorkerExpenses;
            var expenseWithoutWorker = firstSummary.ExpenseWithoutWorker;
            //var waitingIncomeForStudentPayment = firstSummary.WaitingIncomeForStudentPaymentStr;


            document.getElementById("totalEndorsement" + index).innerHTML = firstSummary.EndorsmentForStudentPaymentStr;
            document.getElementById("totalExpense" + index).innerHTML = firstSummary.TotalExpenseStr;

        }
    }
    else {
        alert("Hata var !!! Error : " + obje.ErrorDescription);
    }
}

function setIncomeAndWaiting(isPayment, tblName, list) {
    if (list.length > 0) {

        let tbody = "";
        tbody += "<table border='3' style='border-style:solid; border-color: #343a40;' class='table mb - 0'>";
        tbody += "<thead><tr><th scope='col'>Adı</th><th scope='col'>Ödeme Durumu</th><th scope='col'>Tutar</th></thead>";

        var index = "";
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
            index = list[i].Index;
        }

        tbody += "</table>";
        if (IsNullOrEmpty(index)) {
            var tbl = document.getElementById(tblName);
            if (tbl != null) {
                tbl.innerHTML = tbody;
            }
        }
        else {
            var tbl = document.getElementById(tblName + index);
            if (tbl != null) {
                tbl.innerHTML = tbody;
            }
        }
    }
}

function drawSummaryWithIndex(index, thBody) {


    var tbody = '';
    tbody += '<tr>';
    tbody += '<th scope="col">Ay</th>';
    tbody += '<th scope="col">Ödenen Aidatlar</th>';
    tbody += '<th scope="col">Beklenen Aidatlar</th>';
    tbody += '<th scope="col">Diğer Gelirler</th>';
    tbody += '<th scope="col">Öğr. Maaşları</th>';
    tbody += '<th scope="col">Diğer Giderler</th>';
    tbody += '<th scope="col">Top. Ciro(Anlık)</th>';
    tbody += '<th scope="col">Top. Gider(Anlık)</th>';
    tbody += '<th scope="col">Top. Anlık</th>';
    tbody += '<th scope="col">Ay Sonu Beklenen</th>';
    tbody += '</tr>';

    for (var i = 0; i < index; i++) {
        tbody += '<tr>';
        tbody += '<td><span style="color: darkred;" id="currentMonth' + i + '"></span></td>';
        tbody += '<td>';
        tbody += '<table>';
        tbody += '<tr>';
        tbody += '<td style="cursor: pointer;" onclick="onIncomingDetailRow(' + i + ');" id="tdIncomingPlus' + i + '">+</a></td>';
        tbody += '<td>&nbsp;&nbsp;<b><span style="color: darkgreen;" id="incomeForStudentPayment' + i + '"></span></b></td>';
        tbody += '</tr>';
        tbody += '</table>';
        tbody += '</td>';
        tbody += '<td>';
        tbody += '<table>';
        tbody += '<tr>';
        tbody += '<td style="cursor: pointer;" onclick="onWaitingDetailRow(' + i + ');" id="tdWaitingPlus' + i + '">+</td>';
        tbody += '<td>&nbsp;&nbsp;<b><span style="color: lightseagreen;" id="waitingIncomeForStudentPayment' + i + '"></span></b></td>';
        tbody += '</tr>';
        tbody += '</table>';
        tbody += '</td>';
        tbody += '<td>&nbsp;&nbsp;<b><span style="color: darkgreen;" id="incomeWithoutStudentPayment' + i + '"></span></b></td>';
        tbody += '<td>&nbsp;&nbsp;<b><span style="color: red;" id="workerExpenses' + i + '"></span></b></td>';
        tbody += '<td>';
        tbody += '<table>';
        tbody += '<tr>';
        tbody += '<td style="cursor: pointer;" onclick="onExpenseDetailRow(' + i + ');" id="tdExpensePlus' + i + '">+</td>';
        tbody += '<td>&nbsp;&nbsp;<b><span style="color: red;" id="expenseWithoutWorker' + i + '"></span></b></td>';
        tbody += '</tr>';
        tbody += '</table>';
        tbody += '</td>';        
        tbody += '<td>&nbsp;&nbsp;<b><span style="font-size: 16px;" id="totalEndorsement' + i + '"></span></b></td>';
        tbody += '<td>&nbsp;&nbsp;<b><span style="font-size: 16px;" id="totalExpense' + i + '"></span></b></td>';
        tbody += '<td>&nbsp;&nbsp;<b><span style="font-size: 16px;" id="currentBalance' + i + '"></span></b></td>';
        tbody += '<td>&nbsp;&nbsp;<b><span style="font-size: 16px;" id="totalBalance' + i + '"></span></b></td>';
        tbody += '</tr>';
        tbody += '<tr id="trIncomingPaymentDetail' + i + '" style="display: none;">';
        tbody += '<td colspan="8">';
        tbody += '<hr/>';
        tbody += '<h4>Ödenen Aidatlar</h4>';
        tbody += '<div class="table-responsive" id="tblIncomingPaymentDetail' + i + '">';
        tbody += '</div>';
        tbody += '</td>';
        tbody += '</tr>';
        tbody += '<tr id="trWaitingPaymentDetail' + i + '" style="display: none;">';
        tbody += '<td colspan="8">';
        tbody += '<hr/>';
        tbody += '<h4>Beklenen Aidatlar</h4>';
        tbody += '<div class="table-responsive" id="tblWaitingPaymentDetail' + i + '">';
        tbody += '</div>';
        tbody += '</td>';
        tbody += '</tr>';
        tbody += '<tr id="trExpensePaymentDetail' + i + '" style="display: none;">';
        tbody += '<td colspan="8">';
        tbody += '<hr/>';
        tbody += '<h4>Giderler</h4>';
        tbody += '<div class="table-responsive" id="tblExpensePaymentDetail' + i + '">';
        tbody += '</div>';
        tbody += '</td>';
        tbody += '</tr>';
    }

    if (document.getElementById(thBody) != null) {
        document.getElementById(thBody).innerHTML = tbody;
    }

}

function loadSummaryWithIndex(index) {

    const d = new Date();
    let year = d.getFullYear();
    let month = d.getMonth() + 1;

    for (var i = (index-1); i >= 0; i--) {
        month = d.getMonth() + 1;
        if (month - i <= 0) {
            year = year - 1;
            month = month - i + 12;
        }
        else {
            month = month - i;
        }

        loadExpenseSummaryDetailWithYearAndMonth(year, month, i);
        loadPaymentSummaryDetailWithYearAndMonth(year, month, i);
        loadIncomeAndExpenseSummaryWithYearAndMonth(year, month, i);
        year = year + 1;
    }
}

function loadSummaryWithYearAndMonth(year, month) {

    loadExpenseSummaryDetailWithYearAndMonth(year, month, 0);
    loadPaymentSummaryDetailWithYearAndMonth(year, month, 0);
    loadIncomeAndExpenseSummaryWithYearAndMonth(year, month, 0);
}



