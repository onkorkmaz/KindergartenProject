
function validate() {
    var errorMessage = "";

    var obje = document.getElementById("txtEmail").value;
    if (IsNullOrEmpty(obje))
        errorMessage += "Email adresi boş bırakılamaz\n";

    if (!IsNullOrEmpty(errorMessage)) {
        alert(errorMessage);
        return false;
    }

    return true;
}

function chcAllChange() {

    var obje = document.getElementById("chc_All");
    var isCheckAll = obje.checked;
    var year = obje.getAttribute("year");

    for (var j in months) {

        var uniqueName = "_" + year + "_" + months[j][0];
        var chcPaymentName = "chc" + uniqueName;

        document.getElementById(chcPaymentName).checked = isCheckAll;
    }
}