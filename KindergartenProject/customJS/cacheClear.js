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
}
