

window.onload = function () {

    loadAllData();

};

function loadAllData() {
    loadData();
    loadIncomeAndExpenseSummaryWithYearAndMonth();
}

function loadIncomeAndExpenseSummaryWithYearAndMonth() {
    let year = GetYear();
    let month =GetMonth();

    var jsonData = "{year:" + JSON.stringify(year) + ",month:" + JSON.stringify(month) + "}";
    CallServiceWithAjax('/KinderGartenWebService.asmx/Get_IncomeAndExpenseSummaryWithYearAndMonth', jsonData, successFunctionGetIncomeAndExpenseSummaryForCurrentMonth, errorFunction);
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

function GetYear() {
    let year = document.getElementById("drpYear").value;
    let month = GetMonth();
    if (month < monthsSeasonFirst[0][0]) {
        year++;
    }

    return year;
}

function GetMonth() {
    
    let month = document.getElementById("drpMonth").value;
    return month;
}

function loadData() {

    let year = GetYear();
    let month = GetMonth();

    var jsonData = "{year:" + JSON.stringify(year) + ",month:" + JSON.stringify(month) + "}";
    CallServiceWithAjax('/KinderGartenWebService.asmx/GetIncomeAndExpenseListWithMonthAndYear', jsonData, successFunctionGetIncomeAndExpenseList, errorFunction);
}

function deleteCurrentRecord(id) {

    if (confirm('Silme işlemine devam etmek istediğinize emin misiniz?')) {

        var jsonData = "{ id: " + JSON.stringify(id) + " }";
        CallServiceWithAjax('/KinderGartenWebService.asmx/DeleteIncomeAndExpenseWithId', jsonData, successFunctionDeleteIncomeAndExpenseWithId, errorFunction);
    }

}

function successFunctionDeleteIncomeAndExpenseWithId(obje) {
    if (!obje.HasError && obje.Result) {
        loadData();
        loadIncomeAndExpenseSummaryForCurrentMonth();
        setDefaultValues();
        callDeleteInformationMessage();

    }
    else {
        alert("Hata var !!! Error : " + obje.ErrorDescription);
    }
}

function updateCurrentRecord(id) {

    document.getElementById("hdnId").value = id;
    var jsonData = "{ id: " + JSON.stringify(id) + " }";
    CallServiceWithAjax('/KinderGartenWebService.asmx/GetIncomeAndExpenseWithId', jsonData, successFunctionGetIncomeAndExpenseWithId, errorFunction);

}

function successFunctionGetIncomeAndExpenseWithId(obje) {
    if (!obje.HasError && obje.Result != null) {
        var entity = obje.Result;
        document.getElementById("hdnId").value = entity.Id;

        var type = document.getElementById("drpIncomeAndExpenseType");
        type.value = entity.IncomeAndExpenseTypeId;
        var incomeAndExpenseSubType = type.options[type.selectedIndex].getAttribute('incomeAndExpenseSubType');
        onIncomeAndExpenseTypeChanged();
        if (incomeAndExpenseSubType == IncomeAndExpenseSubType.WorkerExpense) {
            var worker = document.getElementById("drpWorker");
            worker.value = entity.WorkerId;
        }

        document.getElementById("txtAmount").value = entity.Amount;
        document.getElementById("txtDescription").value = entity.Description;
        document.getElementById("chcIsActive").checked = entity.IsActive;
        document.getElementById("txtProcessDate").value = entity.ProcessDateWithFormatyyyyMMdd;

        
        document.getElementById("btnSubmit").value = "Güncelle";
        document.getElementById("btnSubmit").disabled = "";
    }
    else {
        alert("Hata var !!! Error : " + obje.ErrorDescription);
    }
}

function successFunctionGetIncomeAndExpenseList(obje) {
    if (!obje.HasError && obje.Result) {

        var entityList = obje.Result;
        if (entityList != null) {

            var tbody = "";
            for (var i in entityList) {

                tbody += "<tr>";
                tbody += "<td>";
                tbody += "</td>";

                if (entityList[i].IncomeAndExpenseType == 1) {
                    tbody += "<td style='color:Green;'>Gelir</td>";
                }
                else if (entityList[i].IncomeAndExpenseType == 2) {
                    tbody += "<td style='color:red;'>Gider</td>";
                }
                else if (entityList[i].IncomeAndExpenseType == 3) {
                    tbody += "<td style='color:#d5265b;'>Çalışan Gideri</td>";
                }

                if (entityList[i].IncomeAndExpenseType == 3) {
                    tbody += "<td>" + entityList[i].Title + "</td>";
                }
                else {
                    tbody += "<td>" + entityList[i].IncomeAndExpenseTypeName + "</td>";
                }

                tbody += "<td>" + entityList[i].AmountStr + "</td>";
                tbody += "<td>" + entityList[i].ProcessDateWithFormat;

                tbody += "<td>" + entityList[i].Description;

                if (entityList[i].IsActive)
                    tbody += "<td><img src='/img/icons/active.png' width='25' height ='25' /></td>";
                else
                    tbody += "<td><img src='/img/icons/passive.png' width='20' height ='20' /></td>";

                tbody += "</tr> ";
            }

            document.getElementById("tbIncomeAndExpenseList").innerHTML = tbody;

        }
    }
    else {
        alert("Hata var !!! Error : " + obje.ErrorDescription);
    }
}

function onWorkerChanged() {

    var type = document.getElementById("drpIncomeAndExpenseType");
    var incomeAndExpenseSubType = type.options[type.selectedIndex].getAttribute('incomeAndExpenseSubType');
    if (incomeAndExpenseSubType == IncomeAndExpenseSubType.WorkerExpense) {
        var worker = document.getElementById("drpWorker");
        var calculatePrice = worker.options[worker.selectedIndex].getAttribute('calculatePrice');

        document.getElementById("txtAmount").value = calculatePrice.replace(".", ""); ;
    }
}

function onIncomeAndExpenseTypeChanged() {

    document.getElementById("txtAmount").value = "";

    var type = document.getElementById("drpIncomeAndExpenseType");
    var incomeAndExpenseSubType = type.options[type.selectedIndex].getAttribute('incomeAndExpenseSubType');

    var divWorker = document.getElementById("divWorker");
    divWorker.style.display = "none";


    var textbox = document.getElementById("txtIncomeAndExpenseTypeName");

    if (incomeAndExpenseSubType == IncomeAndExpenseSubType.Income) {
        textbox.style.backgroundColor = "green";
        textbox.value = "Gelir";
    }
    else if (incomeAndExpenseSubType == IncomeAndExpenseSubType.Expense) {
        textbox.style.backgroundColor = "red";
        textbox.value = "Gider";
    }
    else if (incomeAndExpenseSubType == IncomeAndExpenseSubType.WorkerExpense) {
        textbox.style.backgroundColor = "#d5265b";
        textbox.value = "Gider";
        divWorker.style.display = "";
        onWorkerChanged();
    }
}

var workerList = [];

function validateAndSave() {
    if (!validate())
        return false;

    workerList = [];
    var drpIncomeAndExpenseType = document.getElementById("drpIncomeAndExpenseType");
    incomeAndExpenseTypeId = drpIncomeAndExpenseType.value;
    var amount = document.getElementById("txtAmount").value;
    var incomeAndExpenseSubType = drpIncomeAndExpenseType.options[drpIncomeAndExpenseType.selectedIndex].getAttribute('incomeAndExpenseSubType');

    var worker = document.getElementById("drpWorker").value;

    if (incomeAndExpenseSubType == IncomeAndExpenseSubType.WorkerExpense &&(worker == -1 || worker == -2)) {
        var jsonData = "{  }";
        CallServiceWithAjax('/KinderGartenWebService.asmx/GetAllWorker', jsonData, successFunctionGetWorker, errorFunction);

        if (workerList != null) {

            for (var i in workerList) {

                if (worker == -2 && workerList[i].IsActive == false)
                    continue;

                amount = workerList[i].Price;
                addIncomeAndExpense(amount, workerList[i].Id);
            }
        }
    }
    else {

        addIncomeAndExpense(amount, 0);
    }

    if (!hasError) {
        callInsertOrUpdateInformationMessage("hdnId");
        loadIncomeAndExpenseSummaryForCurrentMonth();
        loadData();
        setDefaultValues();
    }
    return false;
}


function setDefaultValues() {
    document.getElementById("hdnId").value = "";
    document.getElementById("drpIncomeAndExpenseType").selectedIndex = 0;
    onIncomeAndExpenseTypeChanged();
    document.getElementById("txtAmount").value = "0";
    document.getElementById("txtDescription").value = "";

    var today = new Date();
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
    var yyyy = today.getFullYear();
    today = yyyy + '-' + mm + '-' + dd ;

    document.getElementById("txtProcessDate").value = today;
    document.getElementById("chcIsActive").checked = true;
    document.getElementById("btnSubmit").value = "Kaydet";

}

function cancel() {
    setDefaultValues();
    return false;
}


var hasError = false;
function addIncomeAndExpense(amount, workerId) {
    if (!hasError) {
        var id = document.getElementById("hdnId").value;
        var drpIncomeAndExpenseType = document.getElementById("drpIncomeAndExpenseType");
        incomeAndExpenseTypeId = drpIncomeAndExpenseType.value;
        var isActive = document.getElementById("chcIsActive").checked;
        var description = document.getElementById("txtDescription").value;
        var incomeAndExpenseSubType = drpIncomeAndExpenseType.options[drpIncomeAndExpenseType.selectedIndex].getAttribute('incomeAndExpenseSubType');
        var processDate = document.getElementById("txtProcessDate").value;

        var worker = "";
        if (incomeAndExpenseSubType == IncomeAndExpenseSubType.WorkerExpense) {
            if (workerId > 0) {
                worker = workerId;
            }
            else {
                worker = document.getElementById("drpWorker").value;
            }
        }

        var incomeAndExpenseEntity = {};
        incomeAndExpenseEntity["IncomeAndExpenseTypeId"] = incomeAndExpenseTypeId;
        incomeAndExpenseEntity["Amount"] = amount;
        incomeAndExpenseEntity["IsActive"] = isActive;
        incomeAndExpenseEntity["Description"] = description;

        incomeAndExpenseEntity["WorkerId"] = worker;
        incomeAndExpenseEntity["ProcessDate"] = processDate;

        var jsonData = "{ id:" + JSON.stringify(id) + ", incomeAndExpenseEntity: " + JSON.stringify(incomeAndExpenseEntity) + " }";
        CallServiceWithAjax('/KinderGartenWebService.asmx/AddIncomeAndExpense', jsonData, successFunctionAddIncomeAndExpense, errorFunction);
    }
}

function successFunctionAddIncomeAndExpense(obje) {
    if (obje.HasError) {
        alert(obje.ErrorDescription);
        hasError = true;
    }
}

function successFunctionGetWorker(obje) {

     workerList = obje;
}

function validate() {
    var errorMessage = "";

    var obje = document.getElementById("drpIncomeAndExpenseType").value;

    if (IsNullOrEmpty(obje))
        errorMessage += "Ödeme türü boş bırakılamaz\n";

    obje = document.getElementById("txtAmount").value;
    if (IsNullOrEmpty(obje))
        errorMessage += "Ödeme tutarı boş bırakılamaz\n";


    if (!IsNullOrEmpty(errorMessage)) {
        alert(errorMessage);
        return false;
    }

    return true;
}

const IncomeAndExpenseSubType =
{
    "Income": 1,
    "Expense": 2,
    "WorkerExpense":3
}


function drpYearMontChanged(changeType) {

    if (changeType == 'year') {
        document.getElementById("drpMonth").value = 1;
    }
    loadAllData();
}
