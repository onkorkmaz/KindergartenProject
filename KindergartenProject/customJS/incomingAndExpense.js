function onIncomingAndExpenseTypeChanged() {
    var type = document.getElementById("drpIncomingAndExpenseType");
    var typeOfAmount = type.options[type.selectedIndex].getAttribute('typeOfAmount');

    var textbox = document.getElementById("txtIncomingAndExpenseTypeName");

    if (typeOfAmount == TypeOfAmount.Incoming) {
        textbox.style.backgroundColor = "green";
        textbox.value = "Gelir";
    }
    else if (typeOfAmount == TypeOfAmount.Expense) {
        textbox.style.backgroundColor = "red";
        textbox.value = "Gider";
    }
}


function validate() {
    var errorMessage = "";

    var obje = document.getElementById("drpIncomingAndExpenseType").value;

    if (IsNullOrEmpty(obje))
        errorMessage += "Ödeme türü boş bırakılamaz\n";

    obje = document.getElementById("txtAmount").value;
    if (IsNullOrEmpty(obje))
        errorMessage += "Ödeme tutarı boş bırakılamaz\n";


    if (!IsNullOrEmpty(errorMessage)) {
        alert(errorMessage);
        return false;
    }

    return true;
}

const TypeOfAmount =
{
    "Incoming": 1,
    "Expense": 2,
}