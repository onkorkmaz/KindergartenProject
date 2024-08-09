var studentListCache = [];

function GetStudentList() {

    if (studentListCache.length == 0) {
        GetStudentListInDB();
    }
    return studentListCache;
}

function GetStudentListInDB() {
    var jsonData = "{}";
    CallServiceWithAjax('/KinderGartenWebService.asmx/Get_StudentFromCache', jsonData, successFunctionCurrentPage, errorFunction);

}

function successFunctionCurrentPage(obje) {
    studentListCache = obje;
    return obje;
}

function GetActiveStudentList() {

    var newStudentList = []
    for (var i = 0; i < studentListCache.length; i++) {
        if (studentListCache[i].IsActive && studentListCache[i].IsStudent) {
            newStudentList.push(studentListCache[i]);
        }
    }
    return newStudentList;
}

function txtSearchStudent_Change(searchValue) {

    loadData();
}

