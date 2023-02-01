function GetStudentList() {

    var studentList = window["studentList"];

    if (window["studentList"] == null) {

        var jsonData = "{}";
        CallServiceWithAjax('/KinderGartenWebService.asmx/GetAllStudent', jsonData, successFunctionCurrentPage, errorFunction);
        studentList = window["studentList"];
    }

    return studentList;

}

function GetDetailRow(entity, colspanFirst, colspanSecond, isAddDeleteRow) {
    let tbody = "";
    tbody += "<tr style='display: none;' id='tr" + entity.EncryptId + "' >";
    {
        tbody += "<td colspan='" + colspanFirst + "'></td >";
        tbody += "<td colspan='" + colspanSecond + "'>";
        {
            tbody += "<table border='1' width='100%' cellpadding='8'>";
            {
                tbody += "<tr><td width='150'><b>TCKN</b></td><td width='20'>:</td><td style='text-align: left'>" + entity.CitizenshipNumberStr + "</td></tr>";

                tbody += "<tr><td width='150'><b>Anne Adı</b></td><td width='20'>:</td><td style='text-align: left'>" + entity.MotherName + "</td></tr>";
                tbody += "<tr><td width='150'><b>Anne Tel</b></td><td width='20'>:</td><td style='text-align: left'><a href='tel:" + entity.MotherPhoneNumber + "'>" + entity.MotherPhoneNumber + "</a></td></tr>";

                tbody += "<tr><td width='150'><b>Baba Adı</b></td><td width='20'>:</td><td style='text-align: left'>" + entity.FatherName + "</td></tr>";
                tbody += "<tr><td width='150'><b>Baba Tel</b></td><td width='20'>:</td><td style='text-align: left'><a href='tel:" + entity.FatherPhoneNumber + "'>" + entity.FatherPhoneNumber + "</a></td></tr>";

                tbody += "<tr><td width='150'><b>Konuşulan ücret</b></td><td width='20'>:</td><td style='text-align: left'>" + entity.SpokenPriceStr + "</td></tr>";
                tbody += "<tr><td width='150'><b>Sınıf</b></td><td width='20'>:</td><td style='text-align: left'>" + entity.ClassName + "</td></tr>";

                tbody += "<tr><td width='150'><b>Ana Öğretmen</b></td><td width='20'>:</td><td style='text-align: left'>" + entity.MainTeacher + "</td></tr>";
                tbody += "<tr><td width='150'><b>Yardımcı Öğretmen</b></td><td width='20'>:</td><td style='text-align: left'>" + entity.HelperTeacher + "</td></tr>";

                tbody += "<tr><td><b>Görüşülme tarihi</b></td><td>:</td><td style='text-align: left'>" + entity.DateOfMeetingWithFormat + "</td></tr>";
                tbody += "<tr><td><b>Email</b></td><td>:</td><td style='text-align: left'>" + entity.EmailStr + "</td></tr>";
                tbody += "<tr><td><b>Notlar</b></td><td>:</td><td style='text-align: left'>" + entity.NotesStr + "</td></tr>";
                if (entity.SchoolClassDesc != undefined && entity.SchoolClassDesc != null && entity.SchoolClassDesc != '') {
                    tbody += "<tr><td><b>O. Sınıfı</b></td><td>:</td><td style='text-align: left'>" + entity.SchoolClassDesc + "</td></tr>";
                }
                else {
                    tbody += "<tr><td><b>O. Sınıfı</b></td><td>:</td><td style='text-align: left'> - </td></tr>";

                }

                if (isAddDeleteRow) {
                    if (entity.IsActive)
                        tbody += "<tr><td><b>Aktif</b></td><td>:</td><td><img src='img/icons/active.png' width='20' height ='20' /></td></tr>";
                    else
                        tbody += "<tr><td><b>Aktif</b></td><td>:</td><td><img src='img/icons/passive.png' width='20' height ='20' /></td></tr>";

                    tbody += "<tr><td><input type='submit' value='Sil' id='btnDelete' class='btn btn-danger' onclick='return deleteCurrentRecord(\"" + entity.EncryptId + "\")' /></td><td>&nbsp;</td><td>&nbsp;</td></tr>";
                }



            }
            tbody += "</table>";
        }
        tbody += "</td > ";
    }
    tbody += "</tr>";

    return tbody;
}


function onDetailRow(id) {
    var row = document.getElementById("tr" + id);
    row.style.display = row.style.display === 'none' ? '' : 'none';
}


function GetActiveStudentList() {

    var studentList = window["studentList"];

    if (window["studentList"] == null) {

        var jsonData = "{}";
        CallServiceWithAjax('/KinderGartenWebService.asmx/GetAllStudent', jsonData, successFunctionCurrentPage, errorFunction);
        studentList = window["studentList"];
    }

    var newStudentList = []

    for (var i = 0; i < studentList.length; i++) {
        if (studentList[i].IsActive && studentList[i].IsStudent) {
            newStudentList.push(studentList[i]);
        }
    }
    return newStudentList;

}

function GetActiveAllStudentAndAttendanceList() {

    var studentList = window["studentAndAttendanceList"];

    var jsonData = "{}";
    CallServiceWithAjax('/KinderGartenWebService.asmx/GetAllStudentAndAttendanceList', jsonData, successFunctionForStudentAndAttendanceList, errorFunction);
    studentList = window["studentAndAttendanceList"];

    var newStudentList = []

    for (var i = 0; i < studentList.length; i++) {
        if (studentList[i].IsActive && studentList[i].IsStudent) {
            newStudentList.push(studentList[i]);
        }
    }
    return newStudentList;
}


function successFunctionCurrentPage(obje) {

    var studentList = obje;
    if (studentList != null) {
        window["studentList"] = obje;
    }
    return obje;
}

function successFunctionForStudentAndAttendanceList(obje) {

    var studentList = obje;
    if (studentList != null) {
        window["studentAndAttendanceList"] = obje;
    }
}


function txtSearchStudent_Change(searchValue) {

    successFunctionSearchStudent(searchValue);
    SetCacheData("searchValue", searchValue);
}