function validate() {
    var errorMessage = "";

    var obje = document.getElementById("txtUserName").value;
    if (IsNullOrEmpty(obje))
        errorMessage += "Kullanıcı adı boş bırakılamaz.\n";

    obje = document.getElementById("txtPassword").value;
    if (IsNullOrEmpty(obje))
        errorMessage += "Şifre boş bırakılamaz.\n";


    obje = document.getElementById("drpProjectType").value;
    if (IsNullOrEmpty(obje) || obje ==0)
        errorMessage += "Eğitim Kurumu Seçmek zorundasınız.\n";

    if (!IsNullOrEmpty(errorMessage)) {
        alert(errorMessage);
        return false;
    }
    return true;
}

