window.onload = function () {
    onChangeChcCurrentDay();
};

function loadData() {
    var searchValue = document.getElementById("txtSearchStudent").value;

    if (!IsNullOrEmpty(searchValue)) {
        successFunctionSearchStudent(searchValue);
    }
    else {
        successFunctionSearchStudent(null, true);
    }
}

function onChangeChcCurrentDay() {

    let currentDay = document.getElementById("chcCurrentDay");
    let year = document.getElementById("drpYear");
    let month = document.getElementById("drpMonth");
    let days = document.getElementById("drpDays");

    var today = new Date();
    var dd = String(today.getDate()).padStart(2, '0');
    if (dd > 15) {
        days.selectedIndex = "1";
    }

    if (currentDay.checked) {
        year.disabled = true;
        month.disabled = true;
        days.disabled = true;
    }
    else {
        year.disabled = false;
        month.disabled = false;
        days.disabled = false;
    }


    loadData();
}

function successFunctionSearchStudent(search) {
    var studentList = GetActiveAllStudentAndAttendanceList();
    var entityList = [];

    if (!IsNullOrEmpty(search)) {
        entityList = GetFilterStudent(studentList, search);
    }
    else {
        entityList = studentList;
    }

    drawStudentAttendanceBook(entityList);
}


function drawStudentAttendanceBook(entityList) {

    var tbody = "";
    var header = "";

    var year = document.getElementById("drpYear").value;

    let month = document.getElementById("drpMonth").value;

    let currentDay = document.getElementById("chcCurrentDay");
    var today = new Date();
    var dd = String(today.getDate()).padStart(2, '0');


    if (month < monthsSeasonFirst[0][0]) {
        year++;
    }

    if (entityList == null || entityList.length <= 0) {
        document.getElementById("tBodyStudentList").innerHTML = tbody;
        return;
    }


    header = "<tr><th scope='col' width='20'>#####</th><th scope='col'>İsim Soyisim</th>";

    var endDayValue = document.getElementById("drpDays").value;
    var begin = 1;
    if (endDayValue > 16)
        begin = 16;

    for (let i = begin; i <= endDayValue; i++) {

        if (currentDay.checked && i != dd) {
            continue;
        }

        var dayDesc = getDayName(year, (month - 1), i);
        if (dayDesc == "Crtsi" || dayDesc == "Pzr") {
            header += "<th scope='col'><span style='color:red;'>" + i + "(" + dayDesc + ")" + "</span></th >";
        }
        else {
            header += "<th scope='col'>" + i + "(" + dayDesc + ")" + "</th >";
        }

    }

    header +="</tr >";

    document.getElementById("studentAttendanceHeader").innerHTML = header;

    if (entityList != null) {
        for (var i in entityList) {

            var attendanceList = entityList[i].StudentDetail.StudentAttendanceBookList;

            tbody += "<tr>";
            //tbody += "<td>";
            //tbody += "<a href = \"/ogrenci-yoklama-detay/" + entityList[i].EncryptId + "\"><img title='Güncelle' src =\"/img/icons/detail.png\"/></a> &nbsp; ";
            //tbody += "</td>";

            tbody += "<td style='cursor: pointer;' onclick =_onDetailRow(\"" + entityList[i].EncryptId + "\") >+</td>";

            tbody += "<td>" + entityList[i].FullName + "</td>";

            for (let j = begin; j <= endDayValue; j++) {

                if (currentDay.checked && j != dd) {
                    continue;
                }

                var uniqueName = "_" + year + "_" + month + "_" + j + "_" + entityList[i]["Id"];

                var attendance = attendanceList.find(o => o.Year == year && o.Month == month && o.Day == j);

                var isCheck = "";
                if (!IsNullOrEmpty(attendance) && attendance.IsArrival) {
                    isCheck = "checked='checked'";
                }
                

                var chcAttandanceBookName = "chc" + j + "_" + uniqueName;
                tbody += "<td><input type='checkbox' year=" + year + " month=" + month + " day=" + j + " studentId=" + entityList[i]["Id"] + " id='" + chcAttandanceBookName + "' name='" + chcAttandanceBookName + "' " + isCheck + " onchange='onChange(this)' ></td>";
            }


            tbody += "</tr> ";
            tbody += _getDetailRow(entityList[i]);

        }
        document.getElementById("studentAttendanceList").innerHTML = tbody;

        document.getElementById("lblInfo").innerHTML = year + " - " + months[month-1][1];

    }
}

function _getDetailRow(entity) {
    let tbody = "";
    tbody += "<tr style='display: none;' id='tr" + entity.EncryptId + "' >";
    {
        tbody += "<td colspan='1'></td >";
        tbody += "<td colspan='4'>";
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
            }
            tbody += "</table>";
        }
        tbody += "</td > ";
    }
    tbody += "</tr>";

    return tbody;
}

function _onDetailRow(id) {
    var row = document.getElementById("tr" + id);
    row.style.display = row.style.display === 'none' ? '' : 'none';
}

function getDayName(year, month, day) {
    var a = new Date(year, month, day);
    var weekdays = new Array(7);
    weekdays[0] = "Pzr";
    weekdays[1] = "Prtsi";
    weekdays[2] = "Salı";
    weekdays[3] = "Çrş";
    weekdays[4] = "Prş";
    weekdays[5] = "Cma";
    weekdays[6] = "Crtsi";
    var r = weekdays[a.getDay()];
    return r;
}

function onChange(element) {
    var checked = element.checked;
    var checkBit = (checked) ? 1 : 0;
    var year = element.getAttribute("year");
    var month = element.getAttribute("month");
    var day = element.getAttribute("day");
    var studentId = element.getAttribute("studentId");

    var jsonData = "{studentId: " + JSON.stringify(studentId) + ", year:" + JSON.stringify(year) + " , month:" + JSON.stringify(month) + ",day:" + JSON.stringify(day) + ", isArrival :" + JSON.stringify(checked) + "}";
    CallServiceWithAjax('/KinderGartenWebService.asmx/ProcessAttendanceBook', jsonData, successFunctionProcessAttendanceBook, errorFunction);


}

function successFunctionProcessAttendanceBook(object) {
    if (object.HasError) {
        alert("Hata var !!!" + object.ErrorDescription);
    }
    else {
        var entity = object.Result;

        if (entity.IsArrival)
            alert("Yoklama kaydı eklenmiştir.");
        else
            alert("Yoklama kaydı silinmiştir.");

    }
}

function drpYearMonthDayChanged(changeType) {

    var year = document.getElementById("drpYear").value;

    const d = new Date();
    let month = document.getElementById("drpMonth").value;

    if (changeType == "year") {
        month = d.getMonth();
        document.getElementById("drpMonth").value = month + 1;

    }


    if (changeType == "year" || changeType == "month") {
        var select = document.getElementById("drpDays");
        var length = select.options.length;
        for (i = length - 1; i >= 0; i--) {
            select.options[i] = null;
        }

        var lastDayOfMonth = new Date(year, month, 0).getDate();

        var option = document.createElement('option');
        option.text = "1-15";
        option.value = "15";
        select.add(option, 15)

        option = document.createElement('option');
        option.text = "16-" + lastDayOfMonth;
        option.value = lastDayOfMonth;
        select.add(option, lastDayOfMonth)
    }


    loadData();

}


