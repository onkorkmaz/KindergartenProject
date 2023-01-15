var isFirstLoad = true;

window.onload = function () {
    if (isFirstLoad) {
        onIncomeAndExpenseTypeChanged();
        onWorkerChanged();
        isFirstLoad = false;
    }
};

function onWorkerChanged() {

    var type = document.getElementById("drpIncomeAndExpenseType");
    var typeOfAmount = type.options[type.selectedIndex].getAttribute('typeOfAmount');
    if (typeOfAmount == TypeOfAmount.WorkerExpense) {
        var worker = document.getElementById("drpWorker");
        var calculatePrice = worker.options[worker.selectedIndex].getAttribute('calculatePrice');

        document.getElementById("txtAmount").value = calculatePrice.replace(".", ""); ;
    }

}

function onIncomeAndExpenseTypeChanged() {

    document.getElementById("txtAmount").value = "";

    var type = document.getElementById("drpIncomeAndExpenseType");
    var typeOfAmount = type.options[type.selectedIndex].getAttribute('typeOfAmount');

    var divWorker = document.getElementById("divWorker");
    divWorker.style.display = "none";


    var textbox = document.getElementById("txtIncomeAndExpenseTypeName");

    if (typeOfAmount == TypeOfAmount.Income) {
        textbox.style.backgroundColor = "green";
        textbox.value = "Gelir";
    }
    else if (typeOfAmount == TypeOfAmount.Expense) {
        textbox.style.backgroundColor = "red";
        textbox.value = "Gider";
    }
    else if (typeOfAmount == TypeOfAmount.WorkerExpense) {
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
    var typeOfAmount = drpIncomeAndExpenseType.options[drpIncomeAndExpenseType.selectedIndex].getAttribute('typeOfAmount');

    var worker = document.getElementById("drpWorker").value;

    if (worker == -1 || worker == -2) {
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
    }

    return false;

}

var hasError = false;
function addIncomeAndExpense(amount,workerId) {
    if (!hasError) {
        var id = document.getElementById("hdnId").value;
        var drpIncomeAndExpenseType = document.getElementById("drpIncomeAndExpenseType");
        incomeAndExpenseTypeId = drpIncomeAndExpenseType.value;
        var isActive = document.getElementById("chcIsActive").checked;
        var description = document.getElementById("txtDescription").value;
        var typeOfAmount = drpIncomeAndExpenseType.options[drpIncomeAndExpenseType.selectedIndex].getAttribute('typeOfAmount');

        var worker = "";
        if (typeOfAmount == TypeOfAmount.WorkerExpense) {
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


        var jsonData = "{ encryptId:" + JSON.stringify(id) + ", incomeAndExpenseEntity: " + JSON.stringify(incomeAndExpenseEntity) + " }";
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

const TypeOfAmount =
{
    "Income": 1,
    "Expense": 2,
    "WorkerExpense":3
}