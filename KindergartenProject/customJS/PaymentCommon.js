var studentEntityWithId;

function findPaymentEntity(source, year, month, paymentTypeId) {
    for (var i = 0; i < source.length; i++) {
        if (source[i].Year === year && source[i].Month === month && source[i].PaymentType === paymentTypeId) {
            return source[i];
        }
    }
    return null;
}

function doPaymentOrUnPayment(id, studentId, year, month, txtAmountName, isPayment, paymentType) {

    if (isPayment == 0) {
        if (!confirm('Ödeme silme işlemine devam etmek istediğinize emin misiniz?')) {
            return;
        }
    }

    var amount = document.getElementById(txtAmountName).value;

    if (IsNullOrEmpty(amount) && amount <= 0) {
        alert("Tutar girmelisiniz");
        document.getElementById(txtAmountName).focus();
    }
    else {
        var jsonData = "{id: " + JSON.stringify(id) + ", studentId:" + JSON.stringify(studentId) + " , year:" + JSON.stringify(year) + ",month:" + JSON.stringify(month) + ", amount :" + JSON.stringify(amount) + " ,isPayment:" + JSON.stringify(isPayment) + ",paymentType:" + JSON.stringify(paymentType) + "}";
        CallServiceWithAjax('/KinderGartenWebService.asmx/DoPaymentOrUnPayment', jsonData, successFunctionDoPaymentOrUnPayment, errorFunction);
    }
}

function successFunctionDoPaymentOrUnPayment(result) {

    if (!result.HasError) {
        alert("İşlem başarılıdır.");

        var paymentEntity = result.Result;
        var uniqueName = "_" + paymentEntity.StudentId + "_" + paymentEntity.Year + "_" + paymentEntity.Month + "_" + paymentEntity.PaymentType;

        var tdPaymentName = "tdPaymentName" + uniqueName;
        var tdUnPaymentName = "tdUnPaymentName" + uniqueName;

        var hdnPaymentStatus = "hdnPaymentStatus" + uniqueName;

        var chcPayment = "chc" + uniqueName;
        var txtPayment = "txt" + uniqueName;


        if (paymentEntity.IsPayment) {
            document.getElementById(tdPaymentName).removeAttribute("style");
            document.getElementById(tdUnPaymentName).style.display = "none";
            document.getElementById(hdnPaymentStatus).value = true;
            //document.getElementById(chcPayment).setAttribute("disabled", "disabled");
            document.getElementById(txtPayment).setAttribute("disabled", "disabled");

        }
        else {
            document.getElementById(tdUnPaymentName).removeAttribute("style");
            document.getElementById(tdPaymentName).style.display = "none";
            document.getElementById(hdnPaymentStatus).value = false;
            //document.getElementById(chcPayment).removeAttribute("disabled");
            document.getElementById(txtPayment).removeAttribute("disabled");
        }
    }
    else {
        alert("Hata var !!!" + result.ErrorDescription);
    }
}

function textAmount_Change(id, studentId, year, month, paymentType, amount) {

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

            var isListScreen = document.getElementById(txtAmountName).getAttribute("isListScreen");
            document.getElementById(txtAmountName).setAttribute("onchange", "textAmount_Change(" + result.Id + ", " + result.StudentId + ", " + result.Year +
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
        var result = obje.Result;
        var uniqueName = "_" + result.StudentId + "_" + result.Year + "_" + result.Month + "_" + result.PaymentType;
        var txtAmountName = "txt" + uniqueName;
        var isListScreen = document.getElementById(txtAmountName).getAttribute("islistscreen");


        document.getElementById(txtAmountName).setAttribute("onchange", "textAmount_Change(" + result.Id + ", " + result.StudentId + ", " + result.Year +
            "," + result.Month + "," + result.PaymentType + "," + result.Amount + ")");


        if (result.Amount == 0) {
            var imgUnPaymentName = "imgUnPaymentName" + uniqueName;
            document.getElementById(imgUnPaymentName).style.display = 'none';

            var tdUnPaymentName = "tdUnPaymentName" + uniqueName;
            document.getElementById(tdUnPaymentName).style.pointerEvents = '';
            document.getElementById(tdUnPaymentName).style.display = 'none';

        }


        if (result.Amount > 0 && result.IsPayment != true) {

            var tdUnPaymentName = "tdUnPaymentName" + uniqueName;
            document.getElementById(tdUnPaymentName).style.pointerEvents = 'auto';
            var imgUnPaymentName = "imgUnPaymentName" + uniqueName;
            document.getElementById(imgUnPaymentName).style.display = '';
            document.getElementById(tdUnPaymentName).style.display = '';

        }

        if (isListScreen == 0 && isSetAnotherAmount == 0) {
            studentEntityWithId = null;
            GetStudentEntityWithId(result.StudentId);

            if (studentEntityWithId != null && (!studentEntityWithId.IsActive || studentEntityWithId.IsDeleted)) {
                return;
            }

            if (!confirm('Diğer aylara ait ödenmemiş kayıtları da ' + result.Amount + ' TL olarak güncellemek ister misiniz?')) {
                return;
            }

            var year = 0;
            var nextYear = 0;
            if (result.Month >= 9) {
                year = result.Year;
                nextYear = result.Year + 1;
            }
            else {
                year = result.Year - 1;
                nextYear = result.Year;
            }
            

            var paymentYear = [[0, 9, year], [1, 10, year], [2, 11, year], [3, 12, year], [4, 1, nextYear], [5, 2, nextYear], [6, 3, nextYear], [7, 4, nextYear], [8, 5, nextYear], [9, 6, nextYear], [10, 7, nextYear], [11, 8, nextYear]];

            var index = 0;

            for (var n = 0; n < 12; n++)
            {
                if (paymentYear[n][1] == result.Month) {
                    index = paymentYear[n][0];
                    break;
                }
            }


            for (var i = index; i < 12; i++) {
                if (paymentYear[i][1] != result.Month) {
                    
                    var nextUniqueName = "_" + result.StudentId + "_" + paymentYear[i][2] + "_" + paymentYear[i][1] + "_" + result.PaymentType;
                    var nextAmountName = "txt" + nextUniqueName;

                    var nextObje = document.getElementById(nextAmountName);
                    var studentId = nextObje.getAttribute('studentId');

                    var jsonData = "{id: " + JSON.stringify(0) + ", studentId:" + JSON.stringify(studentId) + " , year:" + JSON.stringify(paymentYear[i][2]) + ",month:" + JSON.stringify(paymentYear[i][1]) + ", currentAmount :" + JSON.stringify(result.Amount) + " ,paymentType:" + JSON.stringify(result.PaymentType) + "}";

                    CallServiceWithAjax('/KinderGartenWebService.asmx/SetAnotherPaymentAmount', jsonData, successFunctionSetAnotherPaymentAmount, errorFunction);
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

function drawPaymentDetail(paymentTypeList, year, month, studentEntity, isListScreen, paymentList) {

    var tbody = "";

    for (var i in paymentTypeList) {

        var displayAmount = paymentTypeList[i].Amount;
        var amount = paymentTypeList[i].Amount;
        if (!studentEntity.IsActive) {
            amount = 0;
            displayAmount = 0;
        }
        var id = 0;
        var isPayment = false;
        var uniqueName = "_" + studentEntity.Id + "_" + year + "_" + month + "_" + paymentTypeList[i].Id;

        var tdPaymentName = "tdPaymentName" + uniqueName;
        var tdUnPaymentName = "tdUnPaymentName" + uniqueName;

        var tdUnPaymentStyle = "";
        var tdPaymentStyle = "style ='display:none;'";

        var hdnPaymentStatus = "hdnPaymentStatus" + uniqueName;
        

        var paymentOkInputStyle = "";
        var passiveInputTextStyle = "";

        if (displayAmount == 0) {
            tdPaymentStyle = "style ='display:none;'";
            tdUnPaymentStyle = "style ='display:none;'";
        }

        if (!studentEntity.IsActive) {
            passiveInputTextStyle = " disabled='disabled';";
        }

        var imgDisplay = "";

        var paymentEntity =findPaymentEntity(paymentList, year, month, paymentTypeList[i].Id);
        
        if (paymentEntity != null) {
            id = paymentEntity.Id;

            if (paymentEntity.Amount > 0 && paymentEntity.IsActive) {
                passiveInputTextStyle = "";
            }

            amount = paymentEntity.Amount;
            displayAmount = paymentEntity.Amount;

            if (paymentEntity.IsPayment) {
                tdUnPaymentStyle = "style ='display:none;'";
                tdPaymentStyle = "";
                isPayment = true;
                paymentOkInputStyle = " disabled='disabled';";
            }
            else if (paymentEntity.Amount > 0) {
                tdPaymentStyle = "style ='display:none;'";
                tdUnPaymentStyle = "";
            }
            else if (paymentEntity.Amount == 0) {
                tdPaymentStyle = "style ='display:none;'";
                tdUnPaymentStyle = "style ='display:none;'";
            }
        }
        else if (!studentEntity.IsActive)
        {
            displayAmount = 0;
            amount = 0;
        }
        else {
            if (paymentTypeList[i].Id == PaymentType.Okul && studentEntity.SpokenPrice > 0) {
                displayAmount = studentEntity.SpokenPrice;
                amount = studentEntity.SpokenPrice;
            }
        }
        var txtAmountName = "txt" + uniqueName;
        var chcPaymentName = "chc" + uniqueName;

        var imgUnPaymentName = "imgUnPaymentName" + uniqueName;
        var imgPaymentName = "imgPaymentName" + uniqueName;

        /*
        document.getElementById(chcPayment).setAttribute("disabled", "disabled");
        document.getElementById(txtPayment).setAttribute("disabled", "disabled");
        */
        var imgUnPayment =
            "<img style='cursor: pointer; " + imgDisplay + "' id = '" + imgUnPaymentName + "' width='20' height='20' title='Ödeme Yapmak için tıklayınız' src=\"/img/icons/unPayment2.png\"/>";
        var imgPayment =
            "<img style='cursor: pointer; " + imgDisplay +"'  id= '" + imgPaymentName + "' title = 'Ödemeyi Silmek için tıklayınız' src=\"/img/icons/greenSmile2.png\"/>";

        tbody += "<td><table cellpadding='4'><tr>";

        tbody += "<td style='display:none;'><input type='hidden' value='" + isPayment + "' id = '" + hdnPaymentStatus + "'/></td>";

        tbody += "<td><input studentId='" + studentEntity.Id + "' islistscreen='" + isListScreen + "' " + paymentOkInputStyle + " " + passiveInputTextStyle + " size='3' CssClass='form - control' id='" +
            txtAmountName + "' name='" + txtAmountName + "' type='text' value='" + displayAmount + "' onkeypress='return isNumber(event)' onchange =textAmount_Change(" + id + "," + studentEntity.Id + "," + year +
            "," + month + "," + paymentTypeList[i].Id + "," + amount + ") /></td>";

        tbody += "<td " + tdPaymentStyle+" id='" + tdPaymentName +
            "' onclick =doPaymentOrUnPayment(" + id + ",'" + studentEntity.Id + "'," + year +
            "," + month + ",'" + txtAmountName + "'," + false + "," + paymentTypeList[i].Id + ")>" + imgPayment + "</td>";

        tbody += "<td " + tdUnPaymentStyle+" id='" + tdUnPaymentName +
            "' onclick =doPaymentOrUnPayment(" + id + ",'" + studentEntity.Id + "'," + year +
            "," + month + ",'" + txtAmountName + "'," + true + "," + paymentTypeList[i].Id + ")>" + imgUnPayment + "</td>";

        tbody += "</tr></table></td>";
    }
    return tbody;
}


function drawPayment(paymentTypeList, year, month, isListScreen, studentAndListOfPayment) {

    var tbody = "";
    var paymentList = studentAndListOfPayment.PaymentEntityList;
    var studentEntity = studentAndListOfPayment.StudentEntity;

    for (var i in paymentTypeList) {

        var displayAmount = paymentTypeList[i].Amount;
        var amount = paymentTypeList[i].Amount;
        if (!studentEntity.IsActive) {
            amount = 0;
            displayAmount = 0;
        }
        var id = 0;
        var isPayment = false;
        var uniqueName = "_" + studentEntity.Id + "_" + year + "_" + month + "_" + paymentTypeList[i].Id;

        var tdPaymentName = "tdPaymentName" + uniqueName;
        var tdUnPaymentName = "tdUnPaymentName" + uniqueName;

        var tdUnPaymentStyle = "";
        var tdPaymentStyle = "style ='display:none;'";

        var hdnPaymentStatus = "hdnPaymentStatus" + uniqueName;


        var paymentOkInputStyle = "";
        var passiveInputTextStyle = "";

        if (displayAmount == 0) {
            tdPaymentStyle = "style ='display:none;'";
            tdUnPaymentStyle = "style ='display:none;'";
        }

        if (!studentEntity.IsActive) {
            passiveInputTextStyle = " disabled='disabled';";
        }

        var imgDisplay = "";
        var paymentEntity = findPaymentEntity(paymentList, year, month, paymentTypeList[i].Id);

        if (paymentEntity != null) {
            id = paymentEntity.Id;

            if (paymentEntity.Amount > 0 && paymentEntity.IsActive) {
                passiveInputTextStyle = "";
            }

            amount = paymentEntity.Amount;
            displayAmount = paymentEntity.Amount;

            if (paymentEntity.IsPayment) {
                tdUnPaymentStyle = "style ='display:none;'";
                tdPaymentStyle = "";
                isPayment = true;
                paymentOkInputStyle = " disabled='disabled';";
            }
            else if (paymentEntity.Amount > 0) {
                tdPaymentStyle = "style ='display:none;'";
                tdUnPaymentStyle = "";
            }
            else if (paymentEntity.Amount == 0) {
                tdPaymentStyle = "style ='display:none;'";
                tdUnPaymentStyle = "style ='display:none;'";
            }
        }
        else if (!studentEntity.IsActive) {
            displayAmount = 0;
            amount = 0;
        }
        else {
            if (paymentTypeList[i].Id == PaymentType.Okul && studentEntity.SpokenPrice > 0) {
                displayAmount = studentEntity.SpokenPrice;
                amount = studentEntity.SpokenPrice;
            }
        }
        var txtAmountName = "txt" + uniqueName;
        var chcPaymentName = "chc" + uniqueName;

        var imgUnPaymentName = "imgUnPaymentName" + uniqueName;
        var imgPaymentName = "imgPaymentName" + uniqueName;

        /*
        document.getElementById(chcPayment).setAttribute("disabled", "disabled");
        document.getElementById(txtPayment).setAttribute("disabled", "disabled");
        */
        var imgUnPayment =
            "<img style='cursor: pointer; " + imgDisplay + "' id = '" + imgUnPaymentName + "' width='20' height='20' title='Ödeme Yapmak için tıklayınız' src=\"/img/icons/unPayment2.png\"/>";
        var imgPayment =
            "<img style='cursor: pointer; " + imgDisplay + "'  id= '" + imgPaymentName + "' title = 'Ödemeyi Silmek için tıklayınız' src=\"/img/icons/greenSmile2.png\"/>";

        tbody += "<td><table cellpadding='4'><tr>";

        tbody += "<td style='display:none;'><input type='hidden' value='" + isPayment + "' id = '" + hdnPaymentStatus + "'/></td>";

        tbody += "<td><input studentId='" + studentEntity.Id + "' islistscreen='" + isListScreen + "' " + paymentOkInputStyle + " " + passiveInputTextStyle + " size='3' CssClass='form - control' id='" +
            txtAmountName + "' name='" + txtAmountName + "' type='text' value='" + displayAmount + "' onkeypress='return isNumber(event)' onchange =textAmount_Change(" + id + "," + studentEntity.Id + "," + year +
            "," + month + "," + paymentTypeList[i].Id + "," + amount + ") /></td>";

        tbody += "<td " + tdPaymentStyle + " id='" + tdPaymentName +
            "' onclick =doPaymentOrUnPayment(" + id + ",'" + studentEntity.Id + "'," + year +
            "," + month + ",'" + txtAmountName + "'," + false + "," + paymentTypeList[i].Id + ")>" + imgPayment + "</td>";

        tbody += "<td " + tdUnPaymentStyle + " id='" + tdUnPaymentName +
            "' onclick =doPaymentOrUnPayment(" + id + ",'" + studentEntity.Id + "'," + year +
            "," + month + ",'" + txtAmountName + "'," + true + "," + paymentTypeList[i].Id + ")>" + imgUnPayment + "</td>";

        tbody += "</tr></table></td>";
    }
    return tbody;
}

