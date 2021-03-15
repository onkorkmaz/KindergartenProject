function findPaymentEntity(source, month, paymentTypeId) {
    for (var i = 0; i < source.length; i++) {
        if (source[i].Month === month && source[i].PaymentType === paymentTypeId) {
            return source[i];
        }
    }
    return null;
}

function doPaymentOrUnPayment(id,encryptStudentId,year, month, txtAmountName, isPayment, paymentType) {

    if (isPayment == 0) {
        if (!confirm('Ödeme silme işlemine devam etmek istediğinize emin misiniz?')) {
            return;
        }
    }

    var amount = document.getElementById(txtAmountName).value;

    if (IsNullOrEmpty(amount)) {
        alert("Tutar girmelisiniz");
        document.getElementById(txtAmountName).focus();
    }
    else {

        var jsonData = "{id: " + JSON.stringify(id) + ", encryptStudentId:" + JSON.stringify(encryptStudentId) + " , year:" + JSON.stringify(year) + ",month:" + JSON.stringify(month) + ", amount :" + JSON.stringify(amount) + " ,isPayment:" + JSON.stringify(isPayment) + ",paymentType:" + JSON.stringify(paymentType) + "}";
        CallServiceWithAjax('KinderGartenWebService.asmx/DoPaymentOrUnPayment', jsonData, successFunctionDoPaymentOrUnPayment, errorFunction);
    }
}

function successFunctionDoPaymentOrUnPayment(result) {

    if (!result.HasError) {
        alert("İşlem başarılıdır.");
        loadData();
    }
}


function setPayableStatus(id, month, chcPaymentName, paymentType, txtAmountName, amount, tdImageName) {

    var isNotPayable = document.getElementById(chcPaymentName).checked;
    if (isNotPayable) {
        document.getElementById(txtAmountName).value = "";
        document.getElementById(txtAmountName).setAttribute("readonly", "readonly");
        document.getElementById(tdImageName).style.display = 'none';
    } else {
        document.getElementById(txtAmountName).value = amount;
        document.getElementById(txtAmountName).removeAttribute("readonly");
        document.getElementById(tdImageName).removeAttribute("style");
    }

}

function drawPaymentDetail(paymentTypeList, year, month, studentEntity) {

    var tbody = "";

    for (var i in paymentTypeList) {

        var id = 0;
        var isPayment = true;
        var isNotPayable = false;


        var paymentEntity = findPaymentEntity(studentEntity.PaymentList, month, paymentTypeList[i].Id);
        if (paymentEntity != null) {
            id = paymentEntity.Id;
            isNotPayable = paymentEntity.IsNotPayable;
        }


        var txtAmountName = "txt_" + month + "_" + paymentTypeList[i].Id;
        var chcPaymentName = "chc_" + month + "_" + paymentTypeList[i].Id;
        var tdImageName = "tdImage_" + month + "_" + paymentTypeList[i].Id;

        var amount = paymentTypeList[i].Amount;

        var img =
            "<img style='cursor: pointer;' title='Ödeme Yapmak için tıklayınız' src=\"img/icons/unPayment.png\"/>";


        if (paymentEntity != null && paymentEntity.IsPayment) {
            img =
                "<img style='cursor: pointer;' title = 'Ödemeyi Silmek için tıklayınız' src=\"img/icons/paymentOk.png\"/>";
            amount = paymentEntity.Amount;
            isPayment = false;
        }


        tbody += "<td><table cellpadding='4'><tr>";

        tbody += "<td><input type='checkbox' id='" +
            chcPaymentName +
            "' name='" +
            chcPaymentName +
            "' onclick =setPayableStatus(" + id + "," + month + ",'" + chcPaymentName + "'," + paymentTypeList[i].Id + ",'" + txtAmountName + "'," + amount + ",'" + tdImageName + "') /></td>";

        tbody += "<td><input size='3' CssClass='form - control' id='" +
            txtAmountName +
            "' name='" +
            txtAmountName +
            "' type='text' value='" +
            amount +
            "' onkeypress='return isNumber(event)' /></td>";

        tbody += "<td id='" +
            tdImageName +
            "' onclick =doPaymentOrUnPayment(" + id + ",'" + studentEntity.EncryptId + "'," + year +
            "," + month + ",'" + txtAmountName + "'," + isPayment + "," + paymentTypeList[i].Id + ")>" + img + "</td>";

        tbody += "</tr></table></td>";
    }
    return tbody;
}