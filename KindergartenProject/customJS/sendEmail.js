window.onload = function () {
    toggleMenu();
};


function validate() {

    document.getElementById("btnSendEmail").onclick = function () {
        //disable
        this.disabled = true;

        //do some validation stuff
    }

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

    document.getElementById("btnSendEmail").style.display = "none";
    document.getElementById("btnSendEmailCopy").style.display = "";

    
    return true;
}

function chcAllChange() {

    var obje = document.getElementById("chc_All");
    var isCheckAll = obje.checked;
    var year = obje.getAttribute("year");

    for (var j in monthsSeasonFirst) {

        var uniqueName = "_" + year + "_" + monthsSeasonFirst[j][0];
        var chcPaymentName = "chc" + uniqueName;

        document.getElementById(chcPaymentName).checked = isCheckAll;
    }

    for (var k in monthsSeasonSecond) {

        var uniqueName = "_" + (year + 1) + "_" + monthsSeasonSecond[k][0];
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
    for (var j in monthsSeasonFirst) {

        var uniqueName = "_" + year + "_" + monthsSeasonFirst[j][0];
        var chcPaymentName = "chc" + uniqueName;

        var currentCheckbox = document.getElementById(chcPaymentName);
        if (currentCheckbox.checked) {

            if (IsNullOrEmpty(hdn.value)) {
                hdn.value = "" + monthsSeasonFirst[j][0] + ",'" + monthsSeasonFirst[j][1] + "'";
            }
            else {
                hdn.value += "_" + monthsSeasonFirst[j][0] + ",'" + monthsSeasonFirst[j][1] + "'";
            }
        }
    }

    for (var k in monthsSeasonSecond) {
        year = year + 1;
        var uniqueName = "_" + year + "_" + monthsSeasonSecond[k][0];
        var chcPaymentName = "chc" + uniqueName;

        var currentCheckbox = document.getElementById(chcPaymentName);
        if (currentCheckbox.checked) {

            if (IsNullOrEmpty(hdn.value)) {
                hdn.value = "" + monthsSeasonSecond[k][0] + ",'" + monthsSeasonSecond[k][1] + "'";
            }
            else {
                hdn.value += "_" + monthsSeasonSecond[k][0] + ",'" + monthsSeasonSecond[k][1] + "'";
            }
        }
    }
}

function chcChange() {
    setSelectedMonthValue();
}