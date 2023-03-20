function GetStudentList() {

    var studentList = window["studentList"];

    if (window["studentList"] == null) {

        var jsonData = "{}";
        CallServiceWithAjax('/KinderGartenWebService.asmx/Get_StudentFromCache', jsonData, successFunctionCurrentPage, errorFunction);
        studentList = window["studentList"];
    }

    return studentList;

}

function GetActiveStudentList() {

    var studentList = window["studentList"];

    if (window["studentList"] == null) {

        var jsonData = "{}";
        CallServiceWithAjax('/KinderGartenWebService.asmx/Get_StudentFromCache', jsonData, successFunctionCurrentPage, errorFunction);
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

function successFunctionCurrentPage(obje) {

    var studentList = obje;
    if (studentList != null) {
        window["studentList"] = obje;
    }
    return obje;
}

function txtSearchStudent_Change(searchValue) {

    successFunctionSearchStudent(searchValue);
    SetCacheData("searchValue", searchValue);
}

