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

    if (window["StudentListAndPaymentTypeInfoForPaymentList"] != null)
        successFunctionCurrentPage(window["StudentListAndPaymentTypeInfoForPaymentList"]);
    else
        GetStudentListAndPaymentTypeInfoForPaymentList();

}

function GetStudentListAndPaymentTypeInfoForPaymentList() {

    var jsonData = "{}";
    CallServiceWithAjax('KinderGartenWebService.asmx/GetStudentListAndPaymentTypeInfoForPaymentList',
        jsonData,
        successFunctionCurrentPage,
        errorFunction);

}

function successFunctionCurrentPage(objects) {

    if (window["StudentListAndPaymentTypeInfoForPaymentList"] == null) {
        window["StudentListAndPaymentTypeInfoForPaymentList"] = objects;
    }

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


    var tbody = "<table class='table mb - 0'><thead><tr><th>##</th><th scope='col'>İsim</th>";
    for (var i in paymentTypeList) {

        tbody += "<th scope='col'>" + paymentTypeList[i].Name + "</th>";
    }

    tbody += "</tr></thead><tbody>";


    if (studentList != null) {

        var date = new Date();
        for (var i in studentList) {

            tbody += "<tr>";
            tbody += "<td>";
            tbody += "<a href = \"PaymentDetail.aspx?Id=" + studentList[i].EncryptId + "\" style='cursor: pointer;'><img src =\"img/icons/detail.png\" title='Ödeme detayı...'/></a>";
            tbody += " <a href = \"SendEmail.aspx?Id=" + studentList[i].EncryptId + "\" style='cursor: pointer;'><img src =\"img/icons/email.png\" title='Email Gönder'/></a>";
            tbody += "</td>";
            tbody += "<td>" + studentList[i].FullName + "</td>";
            tbody += drawPaymentDetail(paymentTypeList,year, month, studentList[i]);
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



