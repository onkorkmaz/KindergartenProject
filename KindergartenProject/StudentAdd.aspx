<%@ Page Title="Benim dünyam - Öğrenci Kayıt" Language="C#" Debug="true" MasterPageFile="~/kindergarten.Master" AutoEventWireup="true" CodeBehind="StudentAdd.aspx.cs" Inherits="KindergartenProject.StudentAdd" %>

<%@ Register Src="~/userControl/divInformation.ascx" TagPrefix="uc1" TagName="divInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/customJS/student.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">

        <uc1:divInformation runat="server" ID="divInformation" InformationVisible="false" />

        <div class="col-12 col-xl-12">
            <div class="card">
                <div class="card-body">
                    <asp:Panel runat="server" ID="pnlBody">
                        <asp:HiddenField runat="server" ID="hdnId" />

                        <div class="mb-3 row">
                            <label class="col-form-label col-sm-2 text-sm-left">Kimlik Numarası</label>
                            <div class="col-sm-6">
                                <asp:TextBox runat="server" ID="txtTckn" CssClass="form-control" onchange="txtCitizenshipNumber_Change(this.value);" placeholder="TCKN" MaxLength="11" onkeypress="return isNumber(event)"></asp:TextBox>
                            </div>
                        </div>
                        <div class="mb-3 row">
                            <label class="col-form-label col-sm-2 text-sm-left">Adı</label>
                            <div class="col-sm-4">
                                <asp:TextBox runat="server" ID="txtName" onchange="fullName_Change();" CssClass="form-control" placeholder="Adı"></asp:TextBox>
                            </div>
                            <div class="col-sm-2">
                                <asp:TextBox runat="server" ID="txtMiddleName" onchange="fullName_Change();" CssClass="form-control" placeholder="İkinci Adı"></asp:TextBox>
                            </div>
                        </div>
                        <div class="mb-3 row">
                            <label class="col-form-label col-sm-2 text-sm-left">Soyadı</label>
                            <div class="col-sm-6">
                                <asp:TextBox runat="server" ID="txtSurname" onchange="fullName_Change();" CssClass="form-control" placeholder="Soyadı"></asp:TextBox>
                            </div>
                        </div>
                        <div class="mb-3 row">
                            <label class="col-form-label col-sm-2 text-sm-left">Doğum Tarihi</label>
                            <div class="col-sm-3">
                                <asp:TextBox ID="txtBirthday" runat="server" TextMode="Date" CssClass="form-control" />
                            </div>
                        </div>
                        <div class="mb-3 row">
                            <label class="col-form-label col-sm-2 text-sm-left">Baba Bilgileri</label>
                            <div class="col-sm-2">
                                <asp:TextBox runat="server" ID="txtFatherName" CssClass="form-control" placeholder="Baba Adı"></asp:TextBox>
                            </div>
                            <div class="col-sm-4">
                                <asp:TextBox runat="server" ID="txtFatherPhoneNumber" CssClass="form-control" onkeypress="return isNumber(event)" MaxLength="11" placeholder="Baba Tel"></asp:TextBox>

                            </div>
                        </div>
                        <div class="mb-3 row">
                            <label class="col-form-label col-sm-2 text-sm-left">Anne Bilgileri</label>
                            <div class="col-sm-2">
                                <asp:TextBox runat="server" ID="txtMotherName" CssClass="form-control" placeholder="Anne Adı"></asp:TextBox>
                            </div>

                            <div class="col-sm-4">
                                <asp:TextBox runat="server" ID="txtMotherPhoneNumber" CssClass="form-control" onkeypress="return isNumber(event)" MaxLength="11" placeholder="Anne Tel"></asp:TextBox>

                            </div>
                        </div>

                        <div class="mb-3 row">

                            <label class="col-form-label col-sm-2 text-sm-left">Email</label>
                            <div class="col-sm-3">
                                <asp:TextBox runat="server" ID="txtEmail" CssClass="form-control" MaxLength="50" placeholder="Email"></asp:TextBox>
                            </div>


                        </div>

                        <div class="mb-3 row">

                            <label class="col-form-label col-sm-2 text-sm-left">Öğrenci Durumu</label>
                            <div class="col-sm-2">
                                <asp:DropDownList runat="server" ID="drpStudentState" onchange="OnStudentStateChanged(this.value);" CssClass="form-control">
                                    <asp:ListItem Text="Kayıt" Selected="True" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Görüşme" Value="1"></asp:ListItem>
                                </asp:DropDownList>
                            </div>

                            <asp:HiddenField runat="server" ID="hdnStudentState" />
                        </div>

                        <div class="mb-3 row" id="divClassList">

                            <label class="col-form-label col-sm-2 text-sm-left">Benim Dünyam Sınıfı</label>
                            <div class="col-sm-2">
                                <asp:DropDownList runat="server" ID="drpClassList" onchange="OnClassListChanged(this.value);" CssClass="form-control">
                                </asp:DropDownList>

                            </div>
                            <div class="col-sm-2">
                                <asp:Label ID="lblMaxStudentCount" ForeColor="red" runat="server" Text=""></asp:Label>
                            </div>

                            <asp:HiddenField runat="server" ID="hdnCurrentClassId" />
                        </div>


                         <div class="mb-3 row">
                            <label class="col-form-label col-sm-2 text-sm-left">Mevcut Sınıfı</label>
                            <div class="col-sm-2">
                                <asp:DropDownList runat="server" ID="drpSchoolClass"  CssClass="form-control">
                                    <asp:ListItem Text="Seçininiz" Value ="-1" Selected="True"></asp:ListItem>
                                    
                                    <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                    <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                    <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                    <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                    <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
 
                        </div>

                        <div class="mb-3 row">
                            <label class="col-form-label col-sm-2 text-sm-left">Aktif</label>
                            <div class="col-sm-6">
                                <asp:CheckBox runat="server" ID="chcIsActive" CssClass="form-check-input" Checked="true" />
                            </div>
                        </div>

                        <div class="mb-3 row">
                            <label class="col-form-label col-sm-2 text-sm-left">Konuşulan Ücret</label>
                            <div class="col-sm-2">
                                <asp:TextBox runat="server" ID="txtSpokenPrice" onkeyup="checkDec(this);" CssClass="form-control" placeholder="Ücret"></asp:TextBox>
                            </div>

                        </div>
                        <div class="mb-3 row">
                            <label class="col-form-label col-sm-2 text-sm-left">Görüşme Tarihi</label>
                            <div class="col-sm-3">
                                <asp:TextBox ID="txtDateOfMeeting" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                            </div>
                        </div>

                        <div class="mb-3 row">
                            <label class="col-form-label col-sm-2 text-sm-left">Notlar</label>
                            <div class="col-sm-6">
                                <asp:TextBox runat="server" TextMode="MultiLine" ID="txtNotes" CssClass="form-control" placeholder="Notlar"></asp:TextBox>
                            </div>
                        </div>


                        <div class="mb-3 row">
                            <div class="col-sm-5">
                                <table cellpadding="4">
                                    <tr>
                                        <td>
                                            <asp:Button runat="server" ID="btnSubmit" CssClass="btn btn-primary " Text="Kaydet" OnClientClick="javascript: return validate()" OnClick="btnSubmit_Click" /></td>
                                        <td>
                                            <asp:Button runat="server" ID="btnCancel" CssClass="btn btn-secondary " Text="İptal" OnClick="btnCancel_Click" />
                                        </td>
                                        <td>
                                            <asp:Button runat="server" ID="btnDelete" CssClass="btn btn-danger" OnClientClick="javascript: return validateDelete()" Text="Sil" OnClick="btnDelete_Click" />
                                        </td>
                                        <td>
                                            <asp:Button runat="server" ID="btnPaymentDetail" CssClass="btn btn-warning " Text="Ödeme Detayı" OnClick="btnPayment_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
