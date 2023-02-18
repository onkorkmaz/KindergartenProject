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
        var jsonData = "{id:" + JSON.stringify(id) + ", year:" + year + " }";

        CallServiceWithAjax('/KinderGartenWebService.asmx/GetPaymentDetailSeason', jsonData, successFunctionCurrentPage, errorFunction);
    }
}

function drpYear_Changed() {
    loadData();
}

function successFunctionCurrentPage(obje) {

    if (obje != null) {

        var paymentTypeList = obje[0].PaymentTypeList;

        if (paymentTypeList != null) {

            var count = 0;
            var tbody = "<table class='table mb - 0'><thead><tr><th scope='col'>Ay</th>";
            for (var i in paymentTypeList) {

                tbody += "<th scope='col'>" + paymentTypeList[i].Name + "</th>";
                count = count + 1;
            }

            tbody += "</tr></thead>";

            for (var j in monthsSeasonFirst) {

                for (var k in obje[0].StudentList) {

                    tbody += "<tr>";
                    tbody += "<td>" + obje[0].Year + " - " + monthsSeasonFirst[j][1] + "</td>";
                    tbody += drawPaymentDetail(paymentTypeList, obje[0].Year, monthsSeasonFirst[j][0], obje[0].StudentList[k], 0);
                    tbody += "</tr>";
                }
            }

            for (var j in monthsSeasonSecond) {

                for (var k in obje[1].StudentList) {

                    tbody += "<tr>";
                    tbody += "<td>" + obje[1].Year + " - " +  monthsSeasonSecond[j][1] + "</td>";
                    tbody += drawPaymentDetail(paymentTypeList, obje[1].Year, monthsSeasonSecond[j][0], obje[1].StudentList[k], 0);
                    tbody += "</tr>";
                }
            }

            tbody += "</table>";

            document.getElementById("divMain").innerHTML = tbody;
        }
    }
    else {
        alert("Obje is null");
    }
}



