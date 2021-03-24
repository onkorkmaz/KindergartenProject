
function validate() {
    var errorMessage = "";

    var obje = document.getElementById("txtEmail").value;
    if (IsNullOrEmpty(obje))
        errorMessage += "Email adresi boş bırakılamaz\n";


    obje = document.getElementById("hdnSelectedMonth").value;

    if (IsNullOrEmpty(obje))
        errorMessage += "Ay seçimi yapmalısınız\n";

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

    setSelectedMonthValue();
}

function setSelectedMonthValue() {

    var obje = document.getElementById("chc_All");
    var year = obje.getAttribute("year");

    var hdn = document.getElementById("hdnSelectedMonth");
    hdn.value = "";
    for (var j in months) {

        var uniqueName = "_" + year + "_" + months[j][0];
        var chcPaymentName = "chc" + uniqueName;

        var currentCheckbox = document.getElementById(chcPaymentName);
        if (currentCheckbox.checked) {

            if (IsNullOrEmpty(hdn.value)) {
                hdn.value = "" + months[j][0] + ",'" + months[j][1] + "'";
            }
            else {
                hdn.value += "_" + months[j][0] + ",'" + months[j][1] + "'";
            }
        }
    }
}

function chcChange() {
    setSelectedMonthValue();
}