window.onload = function () {

    var id = document.getElementById("hdnId").value;
    if (!IsNullOrEmpty(id)) {
        document.getElementById("hdnId").value = id;
        loadData();
    }
};

function loadData() {
    var id = document.getElementById("hdnId").value;
    if (!IsNullOrEmpty(id)) {
        var year = document.getElementById("drpYear").value;
        var jsonData = "{studentId:" + JSON.stringify(id) + ", year:" + year + " }";

        CallServiceWithAjax('/KinderGartenWebService.asmx/GetStudentAndListOfPaymentPackage', jsonData, successFunctionCurrentPage, errorFunction);
    }
}

function drpYear_Changed() {
    loadData();
}

function successFunctionCurrentPage(obje) {

    if (obje != null) {

        var paymentTypeList = obje.PaymentTypeEntityList;

        if (paymentTypeList != null) {

            var count = 0;
            var tbody = "<table class='table mb - 0'><thead><tr><th scope='col'>Ay</th>";
            for (var i in paymentTypeList) {

                tbody += "<th scope='col'>" + paymentTypeList[i].Name + "</th>";
                count = count + 1;
            }

            tbody += "</tr></thead>";

            var package = obje.StudentAndListOfPayment

            var yearInt = obje.Year;
            for (var j in monthsSeasonFirst) {
                tbody += "<tr>";
                tbody += "<td>" + yearInt + " - " + monthsSeasonFirst[j][1] + "</td>";
                tbody += drawPayment(paymentTypeList, yearInt, monthsSeasonFirst[j][0], 0, package);
                tbody += "</tr>";

            }
            yearInt = yearInt + 1;
            for (var j in monthsSeasonSecond) {
                tbody += "<tr>";
                tbody += "<td>" + yearInt + " - " + monthsSeasonSecond[j][1] + "</td>";
                tbody += drawPayment(paymentTypeList, yearInt, monthsSeasonSecond[j][0], 0, package);
                tbody += "</tr>";

            }

            tbody += "</table>";
            document.getElementById("divMain").innerHTML = tbody;
        }
    }
    else {
        alert("Obje is null");
    }
}



