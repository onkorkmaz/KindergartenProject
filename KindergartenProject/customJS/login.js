function validate() {
    var errorMessage = "";

    var obje = document.getElementById("txtUserName").value;
    if (IsNullOrEmpty(obje))
        errorMessage += "Kullanıcı adı boş bırakılamaz\n";

    obje = document.getElementById("txtPassword").value;
    if (IsNullOrEmpty(obje))
        errorMessage += "Şifre boş bırakılamaz\n";

    if (!IsNullOrEmpty(errorMessage)) {
        alert(errorMessage);
        return false;
    }

    window["studentList"] = null;
    return true;
}

