window.onload = function () {

    var searchValue = document.getElementById("txtSearchStudent").value;

    if (!IsNullOrEmpty(searchValue)) {
        txtSearchStudent_Change(searchValue);
    }
    else {
        loadData();
    }
};

function txtSearchStudent_Change(searchValue) {

    loadData();
    SetCacheData("searchValue", searchValue);
}

function loadData() {

        GetStudentListAndPaymentTypeInfoForCurrentMonth();

}

function GetStudentListAndPaymentTypeInfoForCurrentMonth() {

    var jsonData = "{}";
    CallServiceWithAjax('/KinderGartenWebService.asmx/GetStudentListAndPaymentTypeInfoForCurrentMonth',
        jsonData,
        successFunctionCurrentPage,
        errorFunction);

}

function successFunctionCurrentPage(objects) {

    var studentList = objects.StudentList;

    var currentStudentList = [];

    var searchValue = document.getElementById("txtSearchStudent").value;
    if (!IsNullOrEmpty(searchValue)) {

        currentStudentList = GetFilterStudent(studentList, searchValue);
    }
    else {
        currentStudentList = studentList;
    }

    drawList(currentStudentList, objects.PaymentTypeList, objects.Year, objects.Month);
}

function drawList(studentList, paymentTypeList, year,month) {


    var tbody = "<table class='table mb - 0'><thead><tr><th>("+months[month-1][1]+" Ayı)</th><th scope='col'>İsim</th>";
    var colspan = 2;
    for (var i in paymentTypeList) {

        tbody += "<th scope='col'>" + paymentTypeList[i].Name + "</th>";
        colspan++;
    }

    tbody += "</tr></thead><tbody>";

  


    if (studentList != null) {

        var date = new Date();
        for (var i in studentList) {

            tbody += "<tr>";
            tbody += "<td>";
            tbody += "<a href = \"/odeme-plani-detay/" + studentList[i].EncryptId + "\" style='cursor: pointer;'><img src =\"/img/icons/paymentPlan.png\" title='Ödeme detayı...'/></a>";
            tbody += " <a href = \"/email-gonder/" + studentList[i].EncryptId + "\" style='cursor: pointer;'><img src =\"/img/icons/email.png\" title='Email Gönder'/></a>";
            tbody += "</td>";
            tbody += "<td>" + studentList[i].FullName + "</td>";
            tbody += drawPaymentDetail(paymentTypeList, year, month, studentList[i], 1);
            tbody += "</tr>";
        }

        tbody += "</tbody></table>";
        window["tbody"] = tbody;

        document.getElementById("divMain").innerHTML = tbody;
    }
}

function successFunctionGetPaymentAllPaymentType(result) {

    return result;

}



