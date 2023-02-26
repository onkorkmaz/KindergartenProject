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
        document.getElementById("tr_Student_" + studentEntity.Id).style.display = "";
        if (studentEntity.SearchText.indexOf(toSearch) <= -1) {
            document.getElementById("tr_Student_" + studentEntity.Id).style.display = "none";
        }
    }
}

function loadData() {
    packageList = [];
    GetStudentAndListOfPaymentListPackageForCurrentMonth();
}

function GetStudentAndListOfPaymentListPackageForCurrentMonth() {

    var jsonData = "{}";
    CallServiceWithAjax('/KinderGartenWebService.asmx/GetStudentAndListOfPaymentListPackageForCurrentMonth',
        jsonData,
        successFunctionCurrentPage,
        errorFunction);
}

function successFunctionCurrentPage(objects) {

    studentAndListOfPaymentList = objects.StudentAndListOfPaymentList;
    drawList(studentAndListOfPaymentList, objects.PaymentTypeEntityList, objects.Year, objects.Month);
}

function drawList(package, paymentTypeList, year,month) {


    var tbody = "<table class='table mb - 0'><thead><tr><th>("+months[month-1][1]+" Ayı)</th><th scope='col'>İsim</th>";
    var colspan = 2;
    for (var i in paymentTypeList) {

        tbody += "<th scope='col'>" + paymentTypeList[i].Name + "</th>";
        colspan++;
    }

    tbody += "</tr></thead><tbody>";

    if (package != null) {

        var date = new Date();
        for (var i in package) {
            var studentEntity = package[i].StudentEntity;
            tbody += "<tr id='tr_Student_" + studentEntity.Id + "' searchText = '" + studentEntity.SearchText + "'>";
            tbody += "<td>";
            tbody += "<a href = \"/odeme-plani-detay/" + studentEntity.Id + "\" style='cursor: pointer;'><img src =\"/img/icons/paymentPlan.png\" title='Ödeme detayı...'/></a>";
            tbody += " <a href = \"/email-gonder/" + studentEntity.Id + "\" style='cursor: pointer;'><img src =\"/img/icons/email.png\" title='Email Gönder'/></a>";
            tbody += "</td>";
            tbody += "<td>" + studentEntity.FullName + "</td>";
            tbody += drawPayment(paymentTypeList, year, month, 1, package[i]);
            tbody += "</tr>";
        }

        tbody += "</tbody></table>";
        window["tbody"] = tbody;

        document.getElementById("divMain").innerHTML = tbody;
    }
}


