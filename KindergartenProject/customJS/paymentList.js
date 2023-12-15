window.onload = function () {

    loadData();

    var searchValue = document.getElementById("txtSearchStudent").value;

    if (!IsNullOrEmpty(searchValue)) {
        txtSearchStudent_Change(searchValue);
    }

};

var studentAndListOfPaymentList = [];

function txtSearchStudent_Change(searchValue) {
    SetCacheData("searchValue", searchValue);

    var toSearch = replaceTurkichChar(searchValue.toLocaleLowerCase('tr-TR'));
    for (let i in studentAndListOfPaymentList) {
        let studentEntity = studentAndListOfPaymentList[i].StudentEntity;

        var tr = document.getElementById("tr_Student_" + studentEntity.Id);
        if (tr != null || tr != undefined) {
            tr.style.display = "";
            if (studentEntity.SearchText.indexOf(toSearch) <= -1) {
                tr.style.display = "none";
            }
        }

        var trDetail = document.getElementById("tr_StudentDetail_" + studentEntity.Id);
        if (trDetail != null || trDetail != undefined) {
            trDetail.style.display = "none";
        }
    }
}

function loadData() {
    packageList = [];
    GetStudentAndListOfPaymentListPackageWithMonth();
}

function GetStudentAndListOfPaymentListPackageWithMonth() {

    let year = document.getElementById("drpYear").value;
    let month = document.getElementById("drpMonth").value;
    if (month < monthsSeasonFirst[0][0]) {
        year++;
    }

    var jsonData = "{year:" + JSON.stringify(year) + ", month: " + JSON.stringify(month) + "  }";
    CallServiceWithAjax('/KinderGartenWebService.asmx/GetStudentAndListOfPaymentListPackageWithMonth',
        jsonData,
        successFunctionPaymentListPage,
        errorFunction);
}

function successFunctionPaymentListPage(objects) {

    studentAndListOfPaymentList = objects.StudentAndListOfPaymentList;
    drawList(studentAndListOfPaymentList, objects.PaymentTypeEntityList, objects.Year, objects.Month);
}

function drawList(package, paymentTypeList, year,month) {


    var tbody = "<table class='table mb - 0'><thead><tr><th>" + months[month - 1][1] +" Ayı</th><th>&nbsp;</th><th scope='col'>İsim</th>";
    var colspan = 2;
    for (var i in paymentTypeList) {

        tbody += "<th scope='col'>" + paymentTypeList[i].Name + "</th>";
        colspan++;
    }

    tbody += "</tr></thead><tbody>";

    if (package != null) {

        var date = new Date();
        for (var i in package) {

            var isContinue = true;
            var chc = document.getElementById("chcIsPaymentDetail");
            if (chc.checked) {
                isContinue = false;
                for (var k in paymentTypeList) {
                    var paymentList = package[i].PaymentEntityList;
                    var _paymentEntity = findPaymentEntity(paymentList, year, month, paymentTypeList[k].Id);
                    if (_paymentEntity != null && !_paymentEntity.IsPayment && _paymentEntity.Amount > 0) {
                        isContinue = true;
                        break;
                    }
                }
            }

            if (!isContinue)
                continue;

            var studentEntity = package[i].StudentEntity;
            tbody += "<tr id='tr_Student_" + studentEntity.Id + "' searchText = '" + studentEntity.SearchText + "'>";
            tbody += "<td>";
            tbody += "<a href = \"/odeme-plani-detay/" + studentEntity.Id + "\" style='cursor: pointer;'><img src =\"/img/icons/paymentPlan.png\" title='Ödeme detayı...'/></a>";
            tbody += " <a href = \"/email-gonder/" + studentEntity.Id + "\" style='cursor: pointer;'><img src =\"/img/icons/email.png\" title='Email Gönder'/></a>";
            tbody += "</td>";
            tbody += "<td style='cursor: pointer;' onclick =_onDetailRow(\"" + studentEntity.Id + "\") id='tdPlus_" + studentEntity.Id + "' >+</td>";

            tbody += "<td>" + studentEntity.FullName + "</td>";
            tbody += drawPayment(paymentTypeList, year, month, 1, package[i]);
            tbody += "</tr>";
            tbody += _getDetailRow(studentEntity, 2, 4, studentEntity.Id);
        }

        tbody += "</tbody></table>";
        window["tbody"] = tbody;

        document.getElementById("divMain").innerHTML = tbody;
    }
}

function onIsPaymentDetailChange() {
    loadData();

    var search = document.getElementById("txtSearchStudent");
    if (search != null && search != undefined && !IsNullOrEmpty(search)) {
        txtSearchStudent_Change(search.value);
    }
}

function drpYearMonthDayChanged(changeType) {

    const d = new Date();
    let month = document.getElementById("drpMonth").value;

    if (changeType == "year") {
        month = d.getMonth();
        document.getElementById("drpMonth").value = month + 1;
    }
    loadData();
}


