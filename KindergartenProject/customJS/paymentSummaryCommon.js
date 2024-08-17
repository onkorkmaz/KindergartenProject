
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

function loadIncomeExpensePackage(monthCount) {

    var jsonData = "{monthCount:" + JSON.stringify(monthCount) + "}";
    CallServiceWithAjax('/KinderGartenWebService.asmx/Get_IncomeExpensePackage', jsonData, successFunctionGetIncomeExpensePackage, errorFunction);

}

function loadExpenseSummaryDetail() {

    var jsonData = "{ }";
    CallServiceWithAjax('/KinderGartenWebService.asmx/Get_ExpenseSummaryDetail', jsonData, successFunctionGetExpenseSummaryDetailWithMonthAndYear, errorFunction);
}

function successFunctionGetIncomeExpensePackage(obje) {
    if (!obje.HasError && obje.Result) {

        loadExpenseSummary(obje.Result.ExpenseSummaryList);
        loadPaymentSummaryDetail(obje.Result.PaymentSummaryDetailList);
        loadPaymentSummary(obje.Result.PaymentSummaryList);
        
    }
    else {
        alert("Hata var !!! Error : " + obje.ErrorDescription);
    }
}

function loadPaymentSummary(list) {

    if (list.length > 0) {

        for (var i in list) {

            var summary = list[i];

            var index = summary.MonthIndex;

            document.getElementById("currentMonth" + index).innerHTML = "<b>" + months[summary.Month - 1][1] + "</b>";
            document.getElementById("incomeForStudentPayment" + index).innerHTML = summary.IncomeForStudentPaymentStr;
            document.getElementById("waitingIncomeForStudentPayment" + index).innerHTML = summary.WaitingIncomeForStudentPaymentStr;
            document.getElementById("incomeWithoutStudentPayment" + index).innerHTML = summary.IncomeWithoutStudentPaymentStr;
            document.getElementById("workerExpenses" + index).innerHTML = summary.WorkerExpensesStr;
            document.getElementById("expenseWithoutWorker" + index).innerHTML = summary.ExpenseWithoutWorkerStr;
            var currentBalance = document.getElementById("currentBalance" + index);
            currentBalance.innerHTML = summary.CurrentBalanceStr;
            if (summary.CurrentBalance < 0) {
                currentBalance.style.color = "red";
            }
            else {
                currentBalance.style.color = "green";
            }

            var totalBalance = document.getElementById("totalBalance" + index);
            totalBalance.innerHTML = summary.TotalBalanceStr;
            if (summary.TotalBalance < 0) {
                totalBalance.style.color = "red";
            }
            else {
                totalBalance.style.color = "green";
            }
                
            document.getElementById("totalEndorsement" + index).innerHTML = summary.EndorsmentForStudentPaymentStr;
            document.getElementById("totalExpense" + index).innerHTML = summary.TotalExpenseStr;
        }
   }
}

function loadPaymentSummaryDetail(list) {
    setIncomeAndWaiting(true, "tblIncomingPaymentDetail", list);
    setIncomeAndWaiting(false, "tblWaitingPaymentDetail", list);
}

function loadExpenseSummary(list) {

    if (list.length > 0) {

        var indexArray = [];

        for (let y in list) {
            var monthIndex = list[y].MonthIndex;
            if (indexArray.indexOf(monthIndex) < 0) {
                indexArray.push(monthIndex);
            }
        }

        for (let y in indexArray) {
            let tbody = "";
            var monthIndex = indexArray[y];

            tbody += "<table border='3' style='border-style:solid; border-color: #343a40;' class='table mb - 0'>";
            tbody += "<thead><tr><th scope='col'>Adı</th><th scope='col'>Durumu</th><th scope='col'>Tutar</th></thead>";

            for (var i in list) {

                var currentIndex = list[i].MonthIndex;;

                if (currentIndex == undefined || currentIndex != monthIndex) {
                    continue;
                }

                tbody += "<tr>";
                tbody += "<td>" + list[i].ExpenseTypeName + "</td>";
                var status = "<td style='color:red;'>Gider   </td>";
                tbody += status;
                tbody += "<td>" + list[i].ExpenseAmountStr + "</td>";
                tbody += "<tr>";
                monthIndex = currentIndex;
               

            }

            tbody += "</table>";

            if (monthIndex != 0 && IsNullOrEmpty(monthIndex)) {
                var tbl = document.getElementById("tblExpensePaymentDetail");
                if (tbl != null) {
                    tbl.innerHTML = tbody;
                }
            }

            else {
                var tbl = document.getElementById("tblExpensePaymentDetail" + monthIndex);
                if (tbl != null) {
                    tbl.innerHTML = tbody;
                }
            }
        }
    }
}


function loadPaymentSummaryDetailWithMonthAndYear(month,year, index) {

    var jsonData = "{month:" + JSON.stringify(month) + ",year:" + JSON.stringify(year) + ",index:" + JSON.stringify(index) + "}";
    CallServiceWithAjax('/KinderGartenWebService.asmx/Get_PaymentSummaryDetailWithMonthAndYear', jsonData, successFunctionGetPaymentSummaryDetailWithMonthAndYear, errorFunction);

}

function successFunctionGetPaymentSummaryDetailWithMonthAndYear(obje) {
    if (!obje.HasError && obje.Result) {
        var list = obje.Result;
        setIncomeAndWaiting(true, "tblIncomingPaymentDetail", list);
        setIncomeAndWaiting(false, "tblWaitingPaymentDetail", list);
       
    }
    else {
        alert("Hata var !!! Error : " + obje.ErrorDescription);
    }
}

function loadIncomeAndExpenseSummaryWithMonthAndYear(month,year, index) {

    var jsonData = "{month:" + JSON.stringify(month) + ",year:" + JSON.stringify(year) + ",index:" + JSON.stringify(index) + "}";
    CallServiceWithAjax('/KinderGartenWebService.asmx/Get_IncomeAndExpenseSummaryWithMonthAndYear', jsonData, successFunctionGetIncomeAndExpenseSummaryWithMonthAndYear, errorFunction);

}

function successFunctionGetIncomeAndExpenseSummaryWithMonthAndYear(obje) {
    if (!obje.HasError && obje.Result) {
        var list = obje.Result;
        if (list.length > 0) {

            var firstSummary = list[0];

            var index = firstSummary.MonthIndex;

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

        var indexArray = [];

        for (let y in list) {
            var monthIndex = list[y].MonthIndex;
            if (indexArray.indexOf(monthIndex) < 0) {
                indexArray.push(monthIndex);
            }
        }

        for (let y in indexArray) {

            var monthIndex = indexArray[y];

            let tbody = "";
            tbody += "<table border='3' style='border-style:solid; border-color: #343a40;' class='table mb - 0'>";
            tbody += "<thead><tr><th scope='col'>Adı</th><th scope='col'>Ödeme Durumu</th><th scope='col'>Tutar</th></thead>";

            for (var i in list) {

                var currentIndex = list[i].MonthIndex;;

                if (currentIndex == undefined || currentIndex != monthIndex) {
                    continue;
                }



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

            if (monthIndex !=0 && IsNullOrEmpty(monthIndex)) {
                var tbl = document.getElementById(tblName);
                if (tbl != null) {
                    tbl.innerHTML = tbody;
                }
            }
            else {
                var tbl = document.getElementById(tblName + monthIndex);
                if (tbl != null) {
                    tbl.innerHTML = tbody;
                }
            }
        }
    }
}

function drawSummaryWithMonthCount(monthCount, thBody) {


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

    for (var index = 0; index < monthCount; index++) {
        tbody += '<tr>';
        tbody += '<td><span style="color: darkred;" id="currentMonth' + index + '"></span></td>';
        tbody += '<td>';
        tbody += '<table>';
        tbody += '<tr>';
        tbody += '<td style="cursor: pointer;" onclick="onIncomingDetailRow(' + index + ');" id="tdIncomingPlus' + index + '">+</a></td>';
        tbody += '<td>&nbsp;&nbsp;<b><span style="color: darkgreen;" id="incomeForStudentPayment' + index + '"></span></b></td>';
        tbody += '</tr>';
        tbody += '</table>';
        tbody += '</td>';
        tbody += '<td>';
        tbody += '<table>';
        tbody += '<tr>';
        tbody += '<td style="cursor: pointer;" onclick="onWaitingDetailRow(' + index + ');" id="tdWaitingPlus' + index + '">+</td>';
        tbody += '<td>&nbsp;&nbsp;<b><span style="color: lightseagreen;" id="waitingIncomeForStudentPayment' + index + '"></span></b></td>';
        tbody += '</tr>';
        tbody += '</table>';
        tbody += '</td>';
        tbody += '<td>&nbsp;&nbsp;<b><span style="color: darkgreen;" id="incomeWithoutStudentPayment' + index + '"></span></b></td>';
        tbody += '<td>&nbsp;&nbsp;<b><span style="color: red;" id="workerExpenses' + index + '"></span></b></td>';
        tbody += '<td>';
        tbody += '<table>';
        tbody += '<tr>';
        tbody += '<td style="cursor: pointer;" onclick="onExpenseDetailRow(' + index + ');" id="tdExpensePlus' + index + '">+</td>';
        tbody += '<td>&nbsp;&nbsp;<b><span style="color: red;" id="expenseWithoutWorker' + index + '"></span></b></td>';
        tbody += '</tr>';
        tbody += '</table>';
        tbody += '</td>';        
        tbody += '<td>&nbsp;&nbsp;<b><span style="font-size: 16px;" id="totalEndorsement' + index + '"></span></b></td>';
        tbody += '<td>&nbsp;&nbsp;<b><span style="font-size: 16px;" id="totalExpense' + index + '"></span></b></td>';
        tbody += '<td>&nbsp;&nbsp;<b><span style="font-size: 16px;" id="currentBalance' + index + '"></span></b></td>';
        tbody += '<td>&nbsp;&nbsp;<b><span style="font-size: 16px;" id="totalBalance' + index + '"></span></b></td>';
        tbody += '</tr>';
        tbody += '<tr id="trIncomingPaymentDetail' + index + '" style="display: none;">';
        tbody += '<td colspan="8">';
        tbody += '<hr/>';
        tbody += '<h4>Ödenen Aidatlar</h4>';
        tbody += '<div class="table-responsive" id="tblIncomingPaymentDetail' + index + '">';
        tbody += '</div>';
        tbody += '</td>';
        tbody += '</tr>';
        tbody += '<tr id="trWaitingPaymentDetail' + index + '" style="display: none;">';
        tbody += '<td colspan="8">';
        tbody += '<hr/>';
        tbody += '<h4>Beklenen Aidatlar</h4>';
        tbody += '<div class="table-responsive" id="tblWaitingPaymentDetail' + index + '">';
        tbody += '</div>';
        tbody += '</td>';
        tbody += '</tr>';
        tbody += '<tr id="trExpensePaymentDetail' + index + '" style="display: none;">';
        tbody += '<td colspan="8">';
        tbody += '<hr/>';
        tbody += '<h4>Giderler</h4>';
        tbody += '<div class="table-responsive" id="tblExpensePaymentDetail' + index + '">';
        tbody += '</div>';
        tbody += '</td>';
        tbody += '</tr>';
    }

    if (document.getElementById(thBody) != null) {
        document.getElementById(thBody).innerHTML = tbody;
    }

}

const d = new Date();
let year = d.getFullYear();
let month = d.getMonth() + 1;

function loadSummary(monthCount) {

    loadIncomeExpensePackage(monthCount);

    //loadPaymentSummaryDetailWithMonthAndYear(month, year, i);
    //loadIncomeAndExpenseSummaryWithMonthAndYear(month, year, i);
}

function loadSummaryWithMonthAndYear(month,year) {

    loadExpenseSummaryDetailWithMonthAndYear(month,year, 0);
    loadPaymentSummaryDetailWithMonthAndYear(month, year, 0);
    loadIncomeAndExpenseSummaryWithMonthAndYear(month, year, 0);
}



