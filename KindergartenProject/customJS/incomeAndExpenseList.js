
window.onload = function () {

    loadAllData();

};

function loadAllData() {
    loadData();
    drawSummaryWithIndex(1, "thBody");
    loadSummaryWithMonthAndYear(getMonth(), getYear());

}

function getYear() {
    let year = document.getElementById("drpYear").value;
    let month = getMonth();
    if (month < monthsSeasonFirst[0][0]) {
        year++;
    }

    return year;
}

function getMonth() {
    
    let month = document.getElementById("drpMonth").value;
    return month;
}

function loadData() {

    let year = getYear();
    let month = getMonth();


    if (month == "-1")
    {
        let incomeAndExpenseTypeId = document.getElementById("drpIncomeAndExpenseType").value;
        var jsonData = "{year:" + JSON.stringify(year) + ",incomeAndExpenseTypeId:" + JSON.stringify(incomeAndExpenseTypeId) + "}";
        CallServiceWithAjax('/KinderGartenWebService.asmx/GetIncomeAndExpenseWithIncomeAndExpenseTypeId', jsonData, successFunctionGetIncomeAndExpenseList, errorFunction);
    }
    else
    {
        var jsonData = "{month:" + JSON.stringify(month) + ",year:" + JSON.stringify(year) + "}";
        CallServiceWithAjax('/KinderGartenWebService.asmx/GetIncomeAndExpenseListWithMonthAndYear', jsonData, successFunctionGetIncomeAndExpenseList, errorFunction);
    }
}

function successFunctionGetIncomeAndExpenseList(obje) {
    if (!obje.HasError && obje.Result) {

        var entityList = obje.Result;
        if (entityList != null) {

            var tbody = "";
            for (var i in entityList) {

                tbody += "<tr>";

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

function drpYearMonthChanged(changeType) {

    const d = new Date();
    if (changeType == 'year') {
        document.getElementById("drpMonth").value = 1;
    }

    if (changeType == 'month') {
        let mnth = document.getElementById("drpMonth").value;
        document.getElementById("currentMonth0").innerHTML = "<b>" + months[mnth - 1][1] + "</b>";
    }


    document.getElementById("drpIncomeAndExpenseType").value = "-1";

    loadAllData();
}

function onIncomeAndExpenseTypeChanged() {

    document.getElementById("drpMonth").value = "-1";
    loadAllData();
}
