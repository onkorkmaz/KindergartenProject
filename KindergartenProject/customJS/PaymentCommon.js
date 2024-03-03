var studentEntityWithId;

function findPaymentEntity(source, year, month, paymentTypeId) {
    for (var i = 0; i < source.length; i++) {
        if (source[i].Year === year && source[i].Month === month && source[i].PaymentType === paymentTypeId) {
            return source[i];
        }
    }
    return null;
}

function doPaymentOrUnPayment(id, studentId, year, month, txtAmountName, paymentType) {


    var amount = document.getElementById(txtAmountName).value;
    var paymentStatus = document.getElementById(txtAmountName).getAttribute("paymentStatus");

    if (IsNullOrEmpty(amount) && amount <= 0) {
        alert("Tutar girmelisiniz");
        document.getElementById(txtAmountName).focus();
    }
    else if (paymentStatus != "0" && paymentStatus != "1")
    {
        alert("Ödeme Durumu uygun değildir.");
    }
    else {
        var isPayment = false;
        if (paymentStatus == "0") {
            isPayment = true;
        }

        if (!isPayment) {
            if (!confirm('Ödeme silme işlemine devam etmek istediğinize emin misiniz?')) {
                return;
            }
        }

        var jsonData = "{id: " + JSON.stringify(id) + ", studentId:" + JSON.stringify(studentId) + " , year:" + JSON.stringify(year) + ",month:" + JSON.stringify(month) + ", amount :" + JSON.stringify(amount) + " ,isPayment:" + JSON.stringify(isPayment) + ",paymentType:" + JSON.stringify(paymentType) + "}";
        CallServiceWithAjax('/KinderGartenWebService.asmx/DoPaymentOrUnPayment', jsonData, successFunctionDoPaymentOrUnPayment, errorFunction);
    }
}

function successFunctionDoPaymentOrUnPayment(result) {

    if (!result.HasError) {
        alert("İşlem başarılıdır.");

        var paymentEntity = result.Result;
        var uniqueName = "_" + paymentEntity.StudentId + "_" + paymentEntity.Year + "_" + paymentEntity.Month + "_" + paymentEntity.PaymentType;
        var tdName = "td" + uniqueName;
        var txtPayment = "txt" + uniqueName;
        var paymentStatus = (paymentEntity.IsPayment) ? "1" : "0";
        var imgSource = getImageForPayment(paymentStatus, paymentEntity.Amount, uniqueName);
        var txtAmountName = "txt" + uniqueName;

        document.getElementById(tdName).innerHTML = imgSource;
        document.getElementById(txtAmountName).setAttribute("paymentStatus", paymentStatus);

        var onClickStr = "doPaymentOrUnPayment(" + paymentEntity.Id + ",'" + paymentEntity.StudentId + "'," + paymentEntity.Year + "," + paymentEntity.Month + ",'" + txtAmountName + "'," + paymentEntity.PaymentType + ")";

        document.getElementById(tdName).setAttribute('onclick', onClickStr);

        if (paymentEntity.IsPayment) {
            document.getElementById(txtPayment).setAttribute("disabled", "disabled");
        }
        else {
            document.getElementById(txtPayment).removeAttribute("disabled");
        }
    }
    else {
        alert("Hata var !!!" + result.ErrorDescription);
    }
}

function getImageForPayment(paymentStatus, amount, uniqueName, isStudentActive) {

    var pointer = "style='cursor: pointer;'";
    if (isStudentActive != undefined && !isStudentActive) {
        pointer = "";
    }
    var img = "";
    var imgUniqueName = "imgUniqueName" + uniqueName;
    if (amount == 0) {
        img = "";
    }
    else if (paymentStatus == "0") {
        img = "<img " + pointer + " id = '" + imgUniqueName + "' width='20' height='20' title='Ödeme Yapmak için tıklayınız' src=\"/img/icons/unPayment2.png\"/>";
    }
    else if (paymentStatus == "1") {
        img = "<img " + pointer + " id= '" + imgUniqueName + "' title = 'Ödemeyi Silmek için tıklayınız' src=\"/img/icons/greenSmile2.png\"/>";
    }

    return img;
}

function txtAmount_Change(id, studentId, year, month, paymentType, amount) {

    var uniqueName = "_" + studentId + "_" + year + "_" + month + "_" + paymentType;
    var txtAmountName = "txt" + uniqueName;

    var currentAmount = document.getElementById(txtAmountName).value
    
    if (currentAmount != amount) {
        var jsonData = "{id: " + JSON.stringify(id) + ", studentId:" + JSON.stringify(studentId) + " , year:" + JSON.stringify(year) + ",month:" + JSON.stringify(month) + ", currentAmount :" + JSON.stringify(currentAmount) + " ,paymentType:" + JSON.stringify(paymentType) + "}";

        CallServiceWithAjax('/KinderGartenWebService.asmx/SetPaymentAmount', jsonData, successFunctionSetPaymentAmount, errorFunction);
    }
}

function successFunctionSetPaymentAmount(obje) {
    successFunctionPaymentAmountCommon(obje, 0);
}

function successFunctionSetAnotherPaymentAmount(obje) {

    successFunctionPaymentAmountCommon(obje, 1);

    if (!obje.HasError) {
        var result = obje.Result;
        var uniqueName = "_" + result.StudentId + "_" + result.Year + "_" + result.Month + "_" + result.PaymentType;
        var txtAmountName = "txt" + uniqueName;

        if (IsNullOrEmpty(result.IsPayment) || result.IsPayment == 0) {

            document.getElementById(txtAmountName).setAttribute("onchange", "txtAmount_Change(" + result.Id + ", " + result.StudentId + ", " + result.Year +
                "," + result.Month + "," + result.PaymentType + "," + result.Amount + ")");
            document.getElementById(txtAmountName).value = result.Amount;
        }
    }
}

function successFunctionPaymentAmountCommon(obje, isSetAnotherAmount) {
    if (obje.HasError) {
        alert("Hata var !!!" + obje.ErrorDescription);
    }
    else {
        var paymentEntity = obje.Result;
        var uniqueName = "_" + paymentEntity.StudentId + "_" + paymentEntity.Year + "_" + paymentEntity.Month + "_" + paymentEntity.PaymentType;
        var txtAmountName = "txt" + uniqueName;
        var isListScreen = document.getElementById(txtAmountName).getAttribute("islistscreen");
        var paymentStatus = (paymentEntity.IsPayment) ? "1" : "0";
        var imgSource = getImageForPayment(paymentStatus, paymentEntity.Amount, uniqueName);
        var tdName = "td" + uniqueName;
        var img = getImageForPayment(paymentStatus, paymentEntity.Amount, uniqueName);

        document.getElementById(txtAmountName).setAttribute("onchange", "txtAmount_Change(" + paymentEntity.Id + ", " + paymentEntity.StudentId + ", " + paymentEntity.Year +
            "," + paymentEntity.Month + "," + paymentEntity.PaymentType + "," + paymentEntity.Amount + ")");

        document.getElementById(tdName).innerHTML = imgSource;
        document.getElementById(txtAmountName).setAttribute("paymentStatus", paymentStatus);

        if (isListScreen == 0 && isSetAnotherAmount == 0) {
            studentEntityWithId = null;
            GetStudentEntityWithId(paymentEntity.StudentId);

            if (studentEntityWithId != null && (!studentEntityWithId.IsActive || studentEntityWithId.IsDeleted)) {
                return;
            }

            if (!confirm('Diğer aylara ait ödenmemiş kayıtları da ' + paymentEntity.Amount + ' TL olarak güncellemek ister misiniz?')) {
                return;
            }

            var year = 0;
            var nextYear = 0;
            if (paymentEntity.Month >= 9) {
                year = paymentEntity.Year;
                nextYear = paymentEntity.Year + 1;
            }
            else {
                year = paymentEntity.Year - 1;
                nextYear = paymentEntity.Year;
            }
            

            var paymentYear = [[0, 9, year], [1, 10, year], [2, 11, year], [3, 12, year], [4, 1, nextYear], [5, 2, nextYear], [6, 3, nextYear], [7, 4, nextYear], [8, 5, nextYear], [9, 6, nextYear], [10, 7, nextYear], [11, 8, nextYear]];

            var index = 0;

            for (var n = 0; n < 12; n++)
            {
                if (paymentYear[n][1] == paymentEntity.Month) {
                    index = paymentYear[n][0];
                    break;
                }
            }

            for (var i = index; i < 12; i++) {
                if (paymentYear[i][1] != paymentEntity.Month) {
                    
                    var nextUniqueName = "_" + paymentEntity.StudentId + "_" + paymentYear[i][2] + "_" + paymentYear[i][1] + "_" + paymentEntity.PaymentType;
                    var nextAmountName = "txt" + nextUniqueName;

                    var nextObje = document.getElementById(nextAmountName);
                    var studentId = nextObje.getAttribute('studentId');
                    var paymentStatus = nextObje.getAttribute('paymentStatus');

                    if (paymentStatus != "1") {
                        var jsonData = "{id: " + JSON.stringify(0) + ", studentId:" + JSON.stringify(studentId) + " , year:" + JSON.stringify(paymentYear[i][2]) + ",month:" + JSON.stringify(paymentYear[i][1]) + ", currentAmount :" + JSON.stringify(paymentEntity.Amount) + " ,paymentType:" + JSON.stringify(paymentEntity.PaymentType) + "}";

                        CallServiceWithAjax('/KinderGartenWebService.asmx/SetAnotherPaymentAmount', jsonData, successFunctionSetAnotherPaymentAmount, errorFunction);
                    }
                }
            }
        }
    }
}


function GetStudentEntityWithId(studentId) {
    if (!IsNullOrEmpty(studentId)) {
        var jsonData = "{ id: " + JSON.stringify(studentId) + " }";
        CallServiceWithAjax('/KinderGartenWebService.asmx/GetStudentEntityWithId',
            jsonData,
            successFunctionGetStudentEntityWithId,
            errorFunction);
    }
}

function successFunctionGetStudentEntityWithId(resultSet) {
    if (!resultSet.HasError) {
        studentEntityWithId = resultSet.Result;
    }
    else {
        alert(resultSet.ErrorDescription);
        return null;
    }
}

function drawPayment(paymentTypeList, year, month, isListScreen, studentAndListOfPayment) {

    var tbody = "";
    var paymentList = studentAndListOfPayment.PaymentEntityList;
    var studentEntity = studentAndListOfPayment.StudentEntity;

    for (var i in paymentTypeList) {

        var displayAmount = 0;
        var amount = 0;
        if (!studentEntity.IsActive) {
            amount = 0;
            displayAmount = 0;
        }
        var id = 0;
        var paymentStatus = "-";
        var uniqueName = "_" + studentEntity.Id + "_" + year + "_" + month + "_" + paymentTypeList[i].Id;
        var inputDisabled = "";
        var passiveStudentDisabled = "";

        if (!studentEntity.IsActive) {
            passiveStudentDisabled = " disabled='disabled';";
        }

        var paymentEntity = findPaymentEntity(paymentList, year, month, paymentTypeList[i].Id);

        if (paymentEntity != null) {
            id = paymentEntity.Id;

            amount = paymentEntity.Amount;
            displayAmount = paymentEntity.Amount;

            if (paymentEntity.IsPayment) {
                inputDisabled = " disabled='disabled';";
                paymentStatus = "1";
            }
            else if (amount > 0) {
                paymentStatus = "0";
            }
        }
        else {
            var studentAddedDate = studentEntity.AddedOnStr;
            var lastDayOfMonth = new Date(year, (month - 1), 0).getDate();

            var currentLastDate = new Date(year, (month - 1), lastDayOfMonth);
            var studentAddedDateJs = new Date(studentAddedDate);

            if (studentAddedDateJs > currentLastDate) {
                displayAmount = 0;
                amount = 0;
            }
            else if (displayAmount > 0) {
                paymentStatus = "0";
            }
            else {

            }
        }
        var txtAmountName = "txt" + uniqueName;
        var tdName = "td" + uniqueName;

        var img = getImageForPayment(paymentStatus, amount, uniqueName, studentEntity.IsActive);
       
        tbody += "<td><table cellpadding='4'><tr>";

        tbody += "<td><input studentId='" + studentEntity.Id + "' paymentStatus='" + paymentStatus + "' islistscreen='" + isListScreen + "' " + passiveStudentDisabled + " " + inputDisabled+" size='3' CssClass='form - control' id='" +
            txtAmountName + "' name='" + txtAmountName + "' type='text' value='" + displayAmount + "' onkeypress='return isNumber(event)' onchange =txtAmount_Change(" + id + "," + studentEntity.Id + "," + year +
            "," + month + "," + paymentTypeList[i].Id + "," + amount + ") /></td>";


        var onClickStr = "doPaymentOrUnPayment(" + id + ",'" + studentEntity.Id + "'," + year + "," + month + ",'" + txtAmountName + "'," + paymentTypeList[i].Id + ")";

        if (!studentEntity.IsActive) {
            onClickStr = "";
        }

        tbody += "<td " + passiveStudentDisabled + " id='" + tdName + "' onclick = " + onClickStr + ">" + img + "</td>";

        tbody += "</tr></table></td>";
    }
    return tbody;
}

