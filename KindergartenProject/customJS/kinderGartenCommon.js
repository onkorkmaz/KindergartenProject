
function CallServiceWithAjax(url, jsonData, successFunction, errorFunction) {
    $.ajax({
        url: url, //
        data: jsonData,
        contentType: "application/json; charset=utf-8",
        type: "POST",
        async: false,
        success: function (data) {
            successFunction(JSON.parse(JSON.stringify(data.d)));
            return data.d;
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

function validateDelete() {
    if (!confirm('Kayıt silinecektir, işleme devam etmek istediğinize emin misiniz?')) {
        return false;
    }
    return true;
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
    CallServiceWithAjax('/KinderGartenWebService.asmx/SetCacheData', jsonData, successSetCacheData, errorFunction);
}

function successSetCacheData() {
}

function GetCacheData(key, value) {
    var jsonData = "{ key: " + JSON.stringify(key) + " }";
    CallServiceWithAjax('/KinderGartenWebService.asmx/GetCacheData', jsonData, successGetCacheData, errorFunction);
}

function successGetCacheData(obje) {
    return obje;
}

function checkDec(el) {
    var ex = /^[0-9]+\.?[0-9]*$/;
    if (ex.test(el.value) == false) {
        el.value = el.value.substring(0, el.value.length - 1);
    }
}

function Encrypt(id) {
    var jsonData = "{ id: " + JSON.stringify(id) + "}";
     CallServiceWithAjax('/KinderGartenWebService.asmx/Encrypt', jsonData, successEncrypt, errorFunction);
}

function successEncrypt(obje) {
    return obje;
}

function Decrypt(id) {
    var jsonData = "{ id: " + JSON.stringify(id) + "}";
    CallServiceWithAjax('/KinderGartenWebService.asmx/Decrypt', jsonData, successDecrypt, errorFunction);
}

function successDecrypt(obje) {
    return obje;
}

function getParameterByName(name, url = window.location.href) {
    name = name.replace(/[\[\]]/g, '\\$&');
    var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return results[2];
}

var months = [[1, "Ocak"], [2, "Şubat"], [3, "Mart"], [4, "Nisan"], [5, "Mayıs"], [6, "Haziran"], [7, "Temmuz"], [8, "Ağustos"], [9, "Eylül"], [10, "Ekim"], [11, "Kasım"], [12, "Aralık"]];

var monthsSeasonFirst = [[9, "Eylül"], [10, "Ekim"], [11, "Kasım"], [12, "Aralık"]];
var monthsSeasonSecond = [[1, "Ocak"], [2, "Şubat"], [3, "Mart"], [4, "Nisan"], [5, "Mayıs"], [6, "Haziran"], [7, "Temmuz"], [8, "Ağustos"]];




function GetFilterStudent(studentList, search) {

    var entityList = [];
    var toSearch = replaceTurkichChar(search.toLocaleLowerCase('tr-TR'));

    for (var i = 0; i < studentList.length; i++) {

        if (studentList[i]["CitizenshipNumber"] != null && replaceTurkichChar(studentList[i]["CitizenshipNumber"].toString().toLocaleLowerCase('tr-TR')).indexOf(toSearch) != -1) {
            entityList.push(studentList[i]);
        }

        else if (studentList[i]["FullName"] != null && replaceTurkichChar(studentList[i]["FullName"].toString().toLocaleLowerCase('tr-TR')).indexOf(toSearch) != -1) {
            entityList.push(studentList[i]);
        }

        else if (studentList[i]["FatherName"] != null && replaceTurkichChar(studentList[i]["FatherName"].toString().toLocaleLowerCase('tr-TR')).indexOf(toSearch) != -1) {
            entityList.push(studentList[i]);
        }

        else if (studentList[i]["MotherName"] != null && replaceTurkichChar(studentList[i]["MotherName"].toString().toLocaleLowerCase('tr-TR')).indexOf(toSearch) != -1) {
            entityList.push(studentList[i]);
        }

        else if (studentList[i]["FatherPhoneNumber"] != null && replaceTurkichChar(studentList[i]["FatherPhoneNumber"].toString().toLocaleLowerCase('tr-TR')).indexOf(toSearch) != -1) {
            entityList.push(studentList[i]);
        }

        else if (studentList[i]["MotherPhoneNumber"] != null && replaceTurkichChar(studentList[i]["MotherPhoneNumber"].toString().toLocaleLowerCase('tr-TR')).indexOf(toSearch) != -1) {
            entityList.push(studentList[i]);
        }
    }

    return entityList;
}

const PaymentType = {

    "Okul": 1,
    "Servis": 2,
    "Kirtasiye": 3,
    "Mental": 4,
    "Diger": 5
}

