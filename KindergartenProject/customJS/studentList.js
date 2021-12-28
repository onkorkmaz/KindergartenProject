window.onload = function () {

    var searchValue = document.getElementById("txtSearchStudent").value;

    if (!IsNullOrEmpty(searchValue)) {
        successFunctionSearchStudent(searchValue);
    }
    else {
        successFunctionSearchStudent(null);
    }
};

function txtSearchStudent_Change(searchValue) {

    successFunctionSearchStudent(searchValue);
    SetCacheData("searchValue", searchValue);
}

function GetStudentList() {

    var studentList = window["studentList"];

    if (window["studentList"] == null) {

        var jsonData = "{}";
        CallServiceWithAjax('/KinderGartenWebService.asmx/GetAllStudent', jsonData, successFunctionCurrentPage, errorFunction);
        studentList = window["studentList"];
    }

    return studentList;

}

function onClassNameChanged() {

    if (document.getElementById("txtSearchStudent") != null && !IsNullOrEmpty(document.getElementById("txtSearchStudent").value))
        successFunctionSearchStudent(document.getElementById("txtSearchStudent").value);
    else
        successFunctionSearchStudent(null);
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

    if (document.getElementById("drpClassList") != null && !IsNullOrEmpty(document.getElementById("drpClassList").value)) {
        if (document.getElementById("drpClassList").value > 0) {

            var newEntityList = [];

            for (var i = 0; i < entityList.length; i++) {

                if (entityList[i]["ClassId"] == document.getElementById("drpClassList").value) {
                    newEntityList.push(entityList[i]);
                }
            }
            drawList(newEntityList);
        }
        else {
            drawList(entityList);
        }
    }
    else {
        drawList(entityList);
    }
}

function drawList(entityList) {

    var tbody = "";

    if (entityList == null || entityList.length <= 0) {
        window["tbody"] = tbody;
        document.getElementById("tBodyStudentList").innerHTML = tbody;
        return;
    }

    if (entityList != null) {
        for (var i in entityList) {

            tbody += "<tr>";
            tbody += "<td style='cursor: pointer;' onclick =onDetailRow(\"" + entityList[i].EncryptId + "\") >+</td>";
            tbody += "<td>";
            tbody += "<a href = \"/ogrenci-guncelle/" + entityList[i].EncryptId + "\"><img title='Güncelle' src =\"/img/icons/update1.png\"/></a> &nbsp; ";       
            //tbody += "<a href = \"#\"><img src =\"/img/icons/trush1.png\" title ='Sil' onclick='deleteCurrentRecord(\"" + entityList[i].EncryptId + "\")' /></a>";

            if (entityList[i].IsStudent == true) {
                tbody += "<a href = \"/odeme-plani-detay/" + entityList[i].EncryptId + "\"><img title = 'Ödeme detayı' src =\"/img/icons/paymentPlan.png\"/></a> &nbsp; ";
                tbody += "<a href = \"/email-gonder/" + entityList[i].EncryptId + "\"><img title = 'Email gönder' src =\"/img/icons/email4.png\"/></a> &nbsp; ";
            }
            
            tbody += "</td>";
            tbody += "<td>" + entityList[i].FullName + "</td>";
            tbody += "<td>" + entityList[i].BirthdayWithFormat + "</td>";
            tbody += "<td>" + entityList[i].FatherInfo + "</td>";
            tbody += "<td>" + entityList[i].MotherInfo + "</td>";

            if (entityList[i].IsActive) {
                if (entityList[i].IsStudent)
                    tbody += "<td>&nbsp;<img src='img/icons/student3.png' width='20' height ='20' /></td>";
                else
                    tbody += "<td>&nbsp;<a href = \"#\"><img title='Öğrenciye Çevir' src='/img/icons/interview.png' width='23' height ='23' onclick='convertStudent(\"" + entityList[i].EncryptId + "\")' /></a></td>";
            }
            else {
                tbody += "<td>&nbsp;</td>";
            }
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
                        tbody += "<tr><td width='150'><b>Sınıf</b></td><td width='20'>:</td><td style='text-align: left'>" + entityList[i].ClassName + "</td></tr>";

                        tbody += "<tr><td width='150'><b>Ana Öğretmen</b></td><td width='20'>:</td><td style='text-align: left'>" + entityList[i].MainTeacher + "</td></tr>";
                        tbody += "<tr><td width='150'><b>Yardımcı Öğretmen</b></td><td width='20'>:</td><td style='text-align: left'>" + entityList[i].HelperTeacher + "</td></tr>";

                        tbody += "<tr><td><b>Görüşülme tarihi</b></td><td>:</td><td style='text-align: left'>" + entityList[i].DateOfMeetingWithFormat + "</td></tr>";
                        tbody += "<tr><td><b>Email</b></td><td>:</td><td style='text-align: left'>" + entityList[i].EmailStr + "</td></tr>";
                        tbody += "<tr><td><b>Notlar</b></td><td>:</td><td style='text-align: left'>" + entityList[i].NotesStr + "</td></tr>";

                        if (entityList[i].IsActive)
                            tbody += "<tr><td><b>Aktif</b></td><td>:</td><td><img src='img/icons/active.png' width='20' height ='20' /></td></tr>";
                        else
                            tbody += "<tr><td><b>Aktif</b></td><td>:</td><td><img src='img/icons/passive.png' width='20' height ='20' /></td></tr>";

                        tbody += "<tr><td><a href = \"#\"><img src =\"/img/icons/trush1.png\" title ='Sil' onclick='deleteCurrentRecord(\"" + entityList[i].EncryptId + "\")' /></a></td><td>&nbsp;</td><td>&nbsp;</td></tr>";

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

    if (confirm('Kayıt silinecektir, işleme devam etmek istediğinize emin misiniz?')) {
        var jsonData = "{ id: " + JSON.stringify(id) + " }";
        CallServiceWithAjax('/KinderGartenWebService.asmx/DeleteStudent', jsonData, successFunctionDeleteStudent, errorFunction);
    }
}

function convertStudent(id) {

    if (confirm('Görüşme olan kaydı öğrenciye çevirmek istediğinize emin misiniz?')) {

        var jsonData = "{ id: " + JSON.stringify(id) + " }";
        CallServiceWithAjax('/KinderGartenWebService.asmx/ConvertStudent', jsonData, successFunctionConvertStudent, errorFunction);
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

function successFunctionConvertStudent(obje) {
    if (!obje.HasError && obje.Result) {
        window["studentList"] = null;
        window["tbody"] = null;
        if (document.getElementById("txtSearchStudent") != null && !IsNullOrEmpty(document.getElementById("txtSearchStudent").value))
            successFunctionSearchStudent(document.getElementById("txtSearchStudent").value);
        else
            successFunctionSearchStudent(null);

        alert("Görüşme başarılı bir şekilde öğrenciye çevrilmiştir.");

    }
    else {
        alert("Hata var !!! Error : " + obje.ErrorDescription);
    }
}

function allStudent() {

    document.getElementById("drpClassList").value = "-1";
    var objects = GetStudentList();

    drawList(objects, true);
}

function activeStudent() {

    document.getElementById("drpClassList").value = "-1";

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

    document.getElementById("drpClassList").value = "-1";
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

    document.getElementById("drpClassList").value = "-1";
    var objects = GetStudentList();
    var entityList = [];


    for (var i = 0; i < objects.length; i++) {
        if (objects[i]["IsActive"] === false && objects[i]["IsStudent"] === true) {
            entityList.push(objects[i]);
        }
    }
    drawList(entityList);
}