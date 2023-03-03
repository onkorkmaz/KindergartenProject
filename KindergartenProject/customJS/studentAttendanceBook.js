window.onload = function () {
    onChangeChcCurrentDay();
};

var packageList = [];

function loadData() {

    packageList = [];
    GetActiveAllStudentAndAttendanceList();

    var searchValue = document.getElementById("txtSearchStudent").value;
    
    if (!IsNullOrEmpty(searchValue)) {
        successFunctionSearchStudent(searchValue);
    }
    
}

function onChangeChcCurrentDay() {
    tbody = "";
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

    var toSearch = replaceTurkichChar(search.toLocaleLowerCase('tr-TR'));
    for (let i in packageList) {
        let studentEntity = packageList[i].StudentEntity;
        document.getElementById("tr_Student_" + studentEntity.Id).style.display = "";
        document.getElementById("tr_Student_Detail_" + studentEntity.Id).style.display = "none";
        if (studentEntity.SearchText.indexOf(toSearch) <= -1) {
            document.getElementById("tr_Student_" + studentEntity.Id).style.display = "none";
            document.getElementById("tr_Student_Detail_" + studentEntity.Id).style.display = "none";
        }
    }   
}


function GetActiveAllStudentAndAttendanceList() {
    var jsonData = "{}";
    CallServiceWithAjax('/KinderGartenWebService.asmx/GetAllStudentAndAttendanceList', jsonData, successFunctionForStudentAndAttendanceList, errorFunction);
}



function successFunctionForStudentAndAttendanceList(obje) {

    packageList = obje;
    if (packageList != null) {

        tbody = "";
        if (packageList == null || packageList.length == 0) {
            document.getElementById("studentAttendanceList").innerHTML = "";
        }
        else {

            for (let i in packageList) {
                drawStudentAttendanceBook(packageList[i]);
            }
        }
    }
}

function drawStudentAttendanceBook(package) {

    var header = "";
    let year = document.getElementById("drpYear").value;
    let month = document.getElementById("drpMonth").value;

    let currentDay = document.getElementById("chcCurrentDay");
    let today = new Date();
    let dd = String(today.getDate()).padStart(2, '0');


    if (month < monthsSeasonFirst[0][0]) {
        year++;
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

    header += "</tr >";

    document.getElementById("studentAttendanceHeader").innerHTML = header;

    var attendanceList = package.StudentAttendanceBookEntityList;
    var studentEntity = package.StudentEntity;
    tbody += "<tr id='tr_Student_" + studentEntity.Id + "' searchText = '" + studentEntity.SearchText+"'>";
    tbody += "<td style='cursor: pointer;' onclick =_onDetailRow(\"" + studentEntity.Id + "\") id='tdPlus' >+</td>";
    tbody += "<td>" + studentEntity.FullName + "</td>";

    for (let j = begin; j <= endDayValue; j++) {

        if (currentDay.checked && j != dd) {
            continue;
        }

        var uniqueName = "_" + year + "_" + month + "_" + j + "_" + studentEntity.Id;

        var attendance = attendanceList.find(o => o.Year == year && o.Month == month && o.Day == j);

        var isCheck = "";
        if (!IsNullOrEmpty(attendance) && attendance.IsArrival) {
            isCheck = "checked='checked'";
        }

        var chcAttandanceBookName = "chc" + j + "_" + uniqueName;
        tbody += "<td><input type='checkbox' year=" + year + " month=" + month + " day=" + j + " studentId=" + studentEntity.Id + " id='" + chcAttandanceBookName + "' name='" + chcAttandanceBookName + "' " + isCheck + " onchange='onChange(this)' ></td>";
    }

    tbody += "</tr> ";
    tbody += _getDetailRow(package);


    document.getElementById("studentAttendanceList").innerHTML = tbody;

    document.getElementById("lblInfo").innerHTML = year + " - " + months[month - 1][1];


}

function _getDetailRow(package) {
    let tbody = "";

    var studentEntity = package.StudentEntity;

    tbody += "<tr style='display: none;' id='tr_Student_Detail_" + studentEntity.Id + "' >";
    {
        tbody += "<td colspan='1'></td >";
        tbody += "<td colspan='4'>";
        {
            tbody += "<table border='1' width='100%' cellpadding='8'>";
            {
                tbody += "<tr><td width='150'><b>TCKN</b></td><td width='20'>:</td><td style='text-align: left'>" + studentEntity.CitizenshipNumberStr + "</td></tr>";

                tbody += "<tr><td width='150'><b>Anne Adı</b></td><td width='20'>:</td><td style='text-align: left'>" + studentEntity.MotherName + "</td></tr>";
                tbody += "<tr><td width='150'><b>Anne Tel</b></td><td width='20'>:</td><td style='text-align: left'><a href='tel:" + studentEntity.MotherPhoneNumber + "'>" + studentEntity.MotherPhoneNumber + "</a></td></tr>";

                tbody += "<tr><td width='150'><b>Baba Adı</b></td><td width='20'>:</td><td style='text-align: left'>" + studentEntity.FatherName + "</td></tr>";
                tbody += "<tr><td width='150'><b>Baba Tel</b></td><td width='20'>:</td><td style='text-align: left'><a href='tel:" + studentEntity.FatherPhoneNumber + "'>" + studentEntity.FatherPhoneNumber + "</a></td></tr>";

                tbody += "<tr><td width='150'><b>Konuşulan ücret</b></td><td width='20'>:</td><td style='text-align: left'>" + studentEntity.SpokenPriceStr + "</td></tr>";
                tbody += "<tr><td width='150'><b>Sınıf</b></td><td width='20'>:</td><td style='text-align: left'>" + studentEntity.ClassName + "</td></tr>";

                tbody += "<tr><td width='150'><b>Ana Öğretmen</b></td><td width='20'>:</td><td style='text-align: left'>" + studentEntity.MainTeacher + "</td></tr>";
                tbody += "<tr><td width='150'><b>Yardımcı Öğretmen</b></td><td width='20'>:</td><td style='text-align: left'>" + studentEntity.HelperTeacher + "</td></tr>";

                tbody += "<tr><td><b>Görüşülme tarihi</b></td><td>:</td><td style='text-align: left'>" + studentEntity.DateOfMeetingWithFormat + "</td></tr>";
                tbody += "<tr><td><b>Email</b></td><td>:</td><td style='text-align: left'>" + studentEntity.EmailStr + "</td></tr>";
                tbody += "<tr><td><b>Notlar</b></td><td>:</td><td style='text-align: left'>" + studentEntity.NotesStr + "</td></tr>";
                if (studentEntity.SchoolClassDesc != undefined && studentEntity.SchoolClassDesc != null && studentEntity.SchoolClassDesc != '') {
                    tbody += "<tr><td><b>O. Sınıfı</b></td><td>:</td><td style='text-align: left'>" + studentEntity.SchoolClassDesc + "</td></tr>";
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
    var row = document.getElementById("tr_Student_Detail_" + id);
    row.style.display = row.style.display === 'none' ? '' : 'none';

    if (row.style.display == '')
        document.getElementById("tdPlus").innerHTML = "-";
    else
        document.getElementById("tdPlus").innerHTML = "+";

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


