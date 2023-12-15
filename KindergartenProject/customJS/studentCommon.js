var studentList = [];
function GetStudentList() {
    studentList = [];
    var jsonData = "{}";
    CallServiceWithAjax('/KinderGartenWebService.asmx/Get_StudentFromCache', jsonData, successFunctionCurrentPage, errorFunction);
    return studentList;

}

function GetActiveStudentList() {

    var studentList = GetStudentList();

    var newStudentList = []

    for (var i = 0; i < studentList.length; i++) {
        if (studentList[i].IsActive && studentList[i].IsStudent) {
            newStudentList.push(studentList[i]);
        }
    }
    return newStudentList;

}

function successFunctionCurrentPage(obje) {
    studentList = obje;
    return obje;
}

function txtSearchStudent_Change(searchValue) {

    loadData();
    SetCacheData("searchValue", searchValue);
}

