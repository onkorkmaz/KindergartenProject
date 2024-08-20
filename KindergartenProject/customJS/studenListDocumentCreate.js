window.onload = function () {

};


function createPdf() {

    if (confirm('Seçilen sınıf için Döküman oluşturmak istediğinze emin misiniz?')) {

        var classId = document.getElementById("drpClassList").value;
        var isShowPrice = document.getElementById("chcShowPrice").checked;

        var jsonData = "{ classId: " + JSON.stringify(classId) + ", isShowPrice: " + JSON.stringify(isShowPrice) + " }";
        CallServiceWithAjax('/KinderGartenWebService.asmx/CreatePDF', jsonData, successFunctionCreatePDF, errorFunction);
    }

    return false;
}


function successFunctionCreatePDF(obje) {
    if (!obje.HasError && obje.Result) {
        alert("Döküman Oluşturulmuştur.");

    }
    else {
        alert("Hata var !!! Error : " + obje.ErrorDescription);
    }
}