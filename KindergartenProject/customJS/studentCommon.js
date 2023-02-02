function GetStudentList() {

    var studentList = window["studentList"];

    if (window["studentList"] == null) {

        var jsonData = "{}";
        CallServiceWithAjax('/KinderGartenWebService.asmx/GetAllStudent', jsonData, successFunctionCurrentPage, errorFunction);
        studentList = window["studentList"];
    }

    return studentList;

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