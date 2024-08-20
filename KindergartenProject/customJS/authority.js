window.onload = function () {
};

function loadData() {

    document.getElementById("tbAuthorityList").innerHTML = "";
    var authorityTypeId = document.getElementById("drpAuthorityType").value;
    var jsonData = "{ authorityTypeId: " + JSON.stringify(authorityTypeId) + "  }";
    CallServiceWithAjax('/KinderGartenWebService.asmx/GetActiveAuthority', jsonData, successFunctionGetActiveAuthorityScreen, errorFunction);

}

function OnAuthorityType() {

    loadData();
}

function onAuthorityCheckBoxChange(element) {

    var checked = element.checked;
    var authorityScreenId = element.getAttribute("authorityScreenId");
    var authorityTypeId = document.getElementById("drpAuthorityType").value;
    var id = element.getAttribute("id");

    var jsonData = "{id: " + JSON.stringify(id) + ", authorityScreenId:" + JSON.stringify(authorityScreenId) + " , authorityTypeId:" + JSON.stringify(authorityTypeId) + ", hasAuthority :" + JSON.stringify(checked) + "}";
    CallServiceWithAjax('/KinderGartenWebService.asmx/AuthorityCheckBoxChange', jsonData, successFunctionAuthorityCheckBoxChange, errorFunction);


}

function successFunctionAuthorityCheckBoxChange(object) {
    if (object.HasError) {
        alert("Hata var !!!" + object.ErrorDescription);
    }
    else {
        alert("İşlem başarılıdır");
    }
}

function successFunctionGetActiveAuthorityScreen(obje) {
    var entityList = obje;
    var authorityTypeId = document.getElementById("drpAuthorityType").value;
    if (entityList != null) {

        var tbody = "";
        for (var i in entityList) {

            tbody += "<tr>";
            tbody += "<td>" + entityList[i].AuthorityScreenName + "</td>";

            var isCheck = "";
            if (entityList[i].HasAuthority) {
                isCheck = "checked='checked'";
            }

            tbody += "<td><input type='checkbox' authorityScreenId=" + entityList[i].AuthorityScreenId + "  id='" + entityList[i].Id + "' name='" + entityList[i].Id + "' " + isCheck + " onchange='onAuthorityCheckBoxChange(this)' ></td>";

            tbody += "</tr> ";
        }

        document.getElementById("tbAuthorityList").innerHTML = tbody;
    }
}
