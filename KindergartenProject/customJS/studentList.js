window.onload = function () {

    var searchValue = document.getElementById("txtSearchStudent").value;

    if (!IsNullOrEmpty(searchValue)) {
        txtSearchStudent_Change(searchValue);
    }
    else {
        successFunctionSearchStudent(null);
    }
};

function GetStudentList() {

    var studentList = window["studentList"];
    if (window["studentList"] == null) {

        var jsonData = "{}";
        CallServiceWithAjax('KinderGartenWebService.asmx/GetAllStudent', jsonData, successFunctionCurrentPage, errorFunction);
        studentList = window["studentList"];
    }

    return studentList;

}

function successFunctionCurrentPage(obje) {

    var entityList = obje;
    if (entityList != null) {
        window["studentList"] = obje;
    }
}

function successFunctionSearchStudent(search) {

    var objects = GetStudentList();
    var entityList = [];

    if (!IsNullOrEmpty(search)) {
        entityList = GetFilterStudent(studentList, search);
    }
    else {
        entityList = objects;
    }
    drawList(entityList);

}

function drawList(entityList) {

    if (entityList != null) {

        var tbody = "";
        for (var i in entityList) {

            tbody += "<tr>";
            tbody += "<td style='cursor: pointer;' onclick =onDetailRow(\"" + entityList[i].EncryptId + "\") >+</td>";
            tbody += "<td>";
            tbody += "<a href = \"AddStudent.aspx?Id=" + entityList[i].EncryptId + "\"><img title='Güncelle' src =\"img/icons/update.png\"/></a>";
            tbody += "<a href = \"#\"><img src =\"img/icons/trush1.png\" title =''Sil onclick='deleteCurrentRecord(\"" + entityList[i].EncryptId + "\")' /></a>";
            tbody += "<a href = \"PaymentDetail.aspx?Id=" + entityList[i].EncryptId + "\"><img title = 'Ödeme Detayı' src =\"img/icons/paymentPlan2.png\"/></a>";
            tbody += "</td>";
            tbody += "<td>" + entityList[i].FullName + "</td>";
            tbody += "<td>" + entityList[i].BirthdayWithFormat + "</td>";
            tbody += "<td>" + entityList[i].FatherInfo + "</td>";
            tbody += "<td>" + entityList[i].MotherInfo + "</td>";

            if (entityList[i].IsStudent)
                tbody += "<td>&nbsp;<img src='img/icons/student.png' width='20' height ='20' /></td>";
            else
                tbody += "<td>&nbsp;<img src='img/icons/interview.png' width='23' height ='23' /></td>";


            if (entityList[i].IsActive)
                tbody += "<td><img src='img/icons/active.png' width='20' height ='20' /></td>";
            else
                tbody += "<td><img src='img/icons/passive.png' width='23' height ='23' /></td>";


            tbody += "</tr> ";

            tbody += "<tr style='display: none;' id='tr" + entityList[i].EncryptId + "' >";
            {
                tbody += "<td colspan=2></td >";
                tbody += "<td colspan=6>";
                {
                    tbody += "<table border='1' width='100%' cellpadding='8'>";
                    {
                        tbody += "<tr><td width='150'><b>TCKN</b></td><td width='20'>:</td><td style='text-align: left'>" + entityList[i].CitizenshipNumberStr + "</td></tr>";

                        tbody += "<tr><td width='150'><b>Konuşulan ücret</b></td><td width='20'>:</td><td style='text-align: left'>" + entityList[i].SpokenPriceStr + "</td></tr>";
                        tbody += "<tr><td><b>Görüşülme tarihi</b></td><td>:</td><td style='text-align: left'>" + entityList[i].DateOfMeetingWithFormat + "</td></tr>";
                        tbody += "<tr><td><b>Email</b></td><td>:</td><td style='text-align: left'>" + entityList[i].EmailStr + "</td></tr>";
                        tbody += "<tr><td><b>Notlar</b></td><td>:</td><td style='text-align: left'>" + entityList[i].NotesStr + "</td></tr>";
                    }
                    tbody += "</table>";
                }
                tbody += "</td > ";
            }
            tbody += "</tr>";

        }

        window["tbody"] = tbody;

        document.getElementById("tBodyStudentList").innerHTML = tbody;
    }
}


function onDetailRow(id) {
    var row = document.getElementById("tr" + id);
    row.style.display = row.style.display === 'none' ? '' : 'none';
}

function deleteCurrentRecord(id) {

    if (confirm('Silme işlemine devam etmek istediğinize emin misiniz?')) {

        var jsonData = "{ id: " + JSON.stringify(id) + " }";
        CallServiceWithAjax('KinderGartenWebService.asmx/DeleteStudent', jsonData, successFunctionDeleteStudent, errorFunction);
    }
}

function successFunctionDeleteStudent(obje) {
    if (!obje.HasError && obje.Result) {
        window["studentList"] = null;
        window["tbody"] = null;
        if (document.getElementById("txtSearchStudent") != null && !IsNullOrEmpty(document.getElementById("txtSearchStudent").value))
            successFunctionSearchStudent(document.getElementById("txtSearchStudent").value);
        else
            successFunctionSearchStudent(null);

        callDeleteInformationMessage();

    }
    else {
        alert("Hata var !!! Error : " + obje.ErrorDescription);
    }
}

function allStudent() {
    var objects = GetStudentList();

    drawList(objects, true);
}

function activeStudent() {
    var objects = GetStudentList();
    var entityList = [];

    for (var i = 0; i < objects.length; i++) {
        if (objects[i]["IsActive"] === true && objects[i]["IsStudent"] == true) {
            entityList.push(objects[i]);
        }
    }

    drawList(entityList);
}

function interviewStudent() {
    var objects = GetStudentList();
    var entityList = [];

    for (var i = 0; i < objects.length; i++) {
        if (objects[i]["IsStudent"] === false) {
            entityList.push(objects[i]);
        }
    }

    drawList(entityList);
}

function passiveStudent() {
    var objects = GetStudentList();
    var entityList = [];


    for (var i = 0; i < objects.length; i++) {
        if (objects[i]["IsActive"] === false && objects[i]["IsStudent"] === true) {
            entityList.push(objects[i]);
        }
    }
    drawList(entityList);
}