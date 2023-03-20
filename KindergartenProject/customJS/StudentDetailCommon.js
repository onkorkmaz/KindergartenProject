function _getDetailRow(entity, colSpanFirst, colSpanSecond, uniquId) {
    let tbody = "";
    tbody += "<tr style='display: none;' id='tr_StudentDetail_" + uniquId + "' >";
    {
        tbody += "<td colspan='" + colSpanFirst + "'></td >";
        tbody += "<td colspan='" + colSpanSecond + "'>";
        {
            tbody += "<table border='1' width='100%' cellpadding='8'>";
            {
                tbody += "<tr><td width='150'><b>TCKN</b></td><td width='20'>:</td><td style='text-align: left'>" + entity.CitizenshipNumberStr + "</td></tr>";

                tbody += "<tr><td width='150'><b>Anne Adı</b></td><td width='20'>:</td><td style='text-align: left'>" + entity.MotherName + "</td></tr>";
                tbody += "<tr><td width='150'><b>Anne Tel</b></td><td width='20'>:</td><td style='text-align: left'><a href='tel:" + entity.MotherPhoneNumber + "'>" + entity.MotherPhoneNumber + "</a></td></tr>";

                tbody += "<tr><td width='150'><b>Baba Adı</b></td><td width='20'>:</td><td style='text-align: left'>" + entity.FatherName + "</td></tr>";
                tbody += "<tr><td width='150'><b>Baba Tel</b></td><td width='20'>:</td><td style='text-align: left'><a href='tel:" + entity.FatherPhoneNumber + "'>" + entity.FatherPhoneNumber + "</a></td></tr>";

                tbody += "<tr><td width='150'><b>Konuşulan ücret</b></td><td width='20'>:</td><td style='text-align: left'>" + entity.SpokenPriceStr + "</td></tr>";
                tbody += "<tr><td width='150'><b>Sınıf</b></td><td width='20'>:</td><td style='text-align: left'>" + entity.ClassName + "</td></tr>";

                tbody += "<tr><td width='150'><b>Ana Öğretmen</b></td><td width='20'>:</td><td style='text-align: left'>" + entity.MainTeacher + "</td></tr>";
                tbody += "<tr><td width='150'><b>Yardımcı Öğretmen</b></td><td width='20'>:</td><td style='text-align: left'>" + entity.HelperTeacher + "</td></tr>";

                tbody += "<tr><td><b>Görüşülme tarihi</b></td><td>:</td><td style='text-align: left'>" + entity.DateOfMeetingWithFormat + "</td></tr>";
                tbody += "<tr><td><b>Email</b></td><td>:</td><td style='text-align: left'>" + entity.EmailStr + "</td></tr>";
                tbody += "<tr><td><b>Notlar</b></td><td>:</td><td style='text-align: left'>" + entity.NotesStr + "</td></tr>";
                tbody += "<tr><td><b>Eklenme Tarihi</b></td><td>:</td><td style='text-align: left'>" + entity.AddedOnStr + "</td></tr>";

                if (entity.SchoolClassDesc != undefined && entity.SchoolClassDesc != null && entity.SchoolClassDesc != '') {
                    tbody += "<tr><td><b>O. Sınıfı</b></td><td>:</td><td style='text-align: left'>" + entity.SchoolClassDesc + "</td></tr>";
                }
                else {
                    tbody += "<tr><td><b>O. Sınıfı</b></td><td>:</td><td style='text-align: left'> - </td></tr>";

                }
                if (entity.IsActive)
                    tbody += "<tr><td><b>Aktif</b></td><td>:</td><td><img src='img/icons/active.png' width='20' height ='20' /></td></tr>";
                else
                    tbody += "<tr><td><b>Aktif</b></td><td>:</td><td><img src='img/icons/passive.png' width='20' height ='20' /></td></tr>";

                tbody += "<tr><td><input type='submit' value='Sil' id='btnDelete' class='btn btn-danger' onclick='return deleteCurrentRecord(\"" + entity.Id + "\")' /></td><td>&nbsp;</td><td>&nbsp;</td></tr>";

            }
            tbody += "</table>";
        }
        tbody += "</td > ";
    }
    tbody += "</tr>";

    return tbody;
}

function _onDetailRow(uniquId) {
    var row = document.getElementById("tr_StudentDetail_" + uniquId);
    row.style.display = row.style.display === 'none' ? '' : 'none';

    if (row.style.display == '')
        document.getElementById("tdPlus_" + uniquId).innerHTML = "<a href = \"#\"><img width='12' height='12' src =\"/img/icons/detailClose.png\"/></a>";
    else
        document.getElementById("tdPlus_" + uniquId).innerHTML = "<a href = \"#\"><img title='Detay için tıklayınız' width='12' height='12' src =\"/img/icons/detail.png\"/></a>";
}