function clearPaymentCache() {
    var jsonData = "{ }";
    CallServiceWithAjax('/KinderGartenWebService.asmx/ClearPaymentCache', jsonData, successFunctionClearPaymentCache, errorFunction);
}

function successFunctionClearPaymentCache(obje) {
    if (obje.HasError) {
        alert(obje.ErrorDescription);
    }
    else {
        alert("Ödeme cache temizlenmiştir");
    }
    return false;
}

function clearStudentCache() {
    var jsonData = "{ }";
    CallServiceWithAjax('/KinderGartenWebService.asmx/ClearStudentCache', jsonData, successFunctionClearStudentCache, errorFunction);
}

function successFunctionClearStudentCache(obje) {
    if (obje.HasError) {
        alert(obje.ErrorDescription);
    }
    else {
        alert("Ödeme cache temizlenmiştir");
    }
    return false;
}

window.onload = function () {

    toggleMenu();
};

