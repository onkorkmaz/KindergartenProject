
function CallServiceWithAjax(url, jsonData, successFunction, errorFunction) {
    $.ajax({
        url: url, //
        data: jsonData,
        contentType: "application/json; charset=utf-8",
        type: "POST",
        async: false,
        success: function (data) {
            successFunction(JSON.parse(JSON.stringify(data.d)));

        },
        error: function (x, y, z) {
            alert(JSON.stringify(x));
            try {
                errorFunction();
            } catch (e) { }
        }
    });
}

function convertToJavaScriptDate(value) {
    var pattern = /Date\(([^)]+)\)/;
    var results = pattern.exec(value);
    var dt = new Date(parseFloat(results[1]));
    return pad(dt.getDate(), 2) + "-" + pad((dt.getMonth() + 1), 2) + "-" + dt.getFullYear();
}

function pad(str, max) {
    str = str.toString();
    return str.length < max ? pad("0" + str, max) : str;
}

function isNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}

function errorFunction() { }

function IsNullOrEmpty(value) {
    return (!value || value == undefined || value == "");
}

function callInsertOrUpdateInformationMessage(id) {
    var id = document.getElementById(id).value;

    if (IsNullOrEmpty(id)) {
        alert("Kayıt başarılı bir şekilde eklenmiştir.");
    }
    else {
        alert("Kayıt başarılı bir şekilde güncellenmiştir.");
    }
}

function callDeleteInformationMessage() {
    alert("Kayıt başarılı bir şekilde silinmiştir.");
}

function replaceTurkichChar(text) {

    text = text.replace("ı", "i");
    text = text.replace("ğ", "g");
    text = text.replace("ö", "o");
    text = text.replace("ü", "u");
    text = text.replace("ş", "s");
    text = text.replace("ç", "c");
    return text;

}

function SetCacheData(key, value) {
    var jsonData = "{ key: " + JSON.stringify(key) + " , value: " + JSON.stringify(value) + " }";
    CallServiceWithAjax('KinderGartenWebService.asmx/SetCacheData', jsonData, successSetCacheData, errorFunction);
}

function successSetCacheData() {
}

function GetCacheData(key, value) {
    var jsonData = "{ key: " + JSON.stringify(key) + " }";
    CallServiceWithAjax('KinderGartenWebService.asmx/GetCacheData', jsonData, successGetCacheData, errorFunction);
}

function successGetCacheData(obje) {
    return obje;
}