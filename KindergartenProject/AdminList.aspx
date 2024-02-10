<%@ Page Title="Admin Listesi" Language="C#" MasterPageFile="~/kindergarten.Master" AutoEventWireup="true" CodeBehind="AdminList.aspx.cs" Inherits="KindergartenProject.AdminList" %>


<%@ Register Src="~/userControl/divInformation.ascx" TagPrefix="uc1" TagName="divInformation" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/customJS/admin.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:divInformation runat="server" ID="divInformation" InformationVisible="false" />

    <div class="card">
        <div class="card-body">
            <asp:Panel runat="server" ID="pnlBody">
                <asp:HiddenField runat="server" ID="hdnId" />

                 <div class="mb-3 row">
                    <label class="col-form-label col-sm-2 text-sm-left">İsim-Soyisim</label>
                    <div class="col-sm-6">
                        <asp:TextBox runat="server" ID="txtFullName" CssClass="form-control" placeholder="İsim"></asp:TextBox>
                    </div>
                </div>

                <div class="mb-3 row">
                    <label class="col-form-label col-sm-2 text-sm-left">Kullanıcı Adı</label>
                    <div class="col-sm-6">
                        <asp:TextBox runat="server" ID="txtUserName" CssClass="form-control" placeholder="İsim"></asp:TextBox>
                    </div>
                </div>

                <div class="mb-3 row">
                    <label class="col-form-label col-sm-2 text-sm-left">Şifre</label>
                    <div class="col-sm-6">
                        <asp:TextBox runat="server" TextMode="Password" ID="txtPassword" CssClass="form-control" placeholder="Şifre"></asp:TextBox>
                    </div>
                </div>

                <div class="mb-3 row">
                    <label class="col-form-label col-sm-2 text-sm-left">Şifre Tekrarı</label>
                    <div class="col-sm-6">
                        <asp:TextBox runat="server" TextMode="Password" ID="txtPasswordRepeat" CssClass="form-control" placeholder="Şifre Tekrarı"></asp:TextBox>
                    </div>
                </div>

                <div class="mb-3 row">
                    <label class="col-form-label col-sm-2 text-sm-left">Yetki Tipi</label>
                    <div class="col-sm-10">
                        <asp:DropDownList runat="server" ID="drpAuthorityType" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="mb-3 row">
                    <label class="col-form-label col-sm-2 text-sm-left">Yetki-Proje Türü</label>
                    <div class="col-sm-10">
                        <table cellpadding="4" border="1">
                            <tbody runat="server" id="adminBody">
                                <tr><td>BD Anaokulu</td><td>:</td><td><asp:CheckBox runat="server" ID="chcBenimDunyamAnaokulu" /></td></tr>
                                <tr><td>BD Eğitim Merkezi</td><td>:</td><td><asp:CheckBox runat="server" ID="chcBenimDunyamEgitimMerkezi" /></td></tr>
                                 <tr><td>Pembe Kule</td><td>:</td><td><asp:CheckBox runat="server" ID="chcPembeKule" /></td></tr>
                            </tbody>
                        </table>
                        <br />
                    <//div>
                </div>

                <div class="mb-3 row">
                    <label class="col-form-label col-sm-2 text-sm-left">Aktif</label>
                    <div class="col-sm-10">
                        <asp:CheckBox runat="server" ID="chcIsActive" CssClass="form-check-input" Checked="true" />
                    </div>
                </div>

                <div class="col-12 col-xl-12">
                    <asp:Button runat="server" ID="btnSubmit" CssClass="btn btn-primary " Text="Kaydet" OnClientClick="javascript: return validateAndSave()" />

                    <asp:Button runat="server" ID="btnCancel" CssClass="btn btn-secondary" Text="İptal" OnClientClick="javascript: return cancel()" />

                </div>
            </asp:Panel>
        </div>

    </div>


    <div class="col-12 col-xl-12">

        <div class="table-responsive">

            <table class="table mb-0">
                <tr>
                    <td>Sadece Aktif Adminler : &nbsp;
                        <asp:CheckBox runat="server" Checked="true" ID="chcOnlyActive" onclick="onChangeChcOnlyActiveStudent();" CssClass="form-check-input" /></td>
                </tr>
                <tr>
                    <td>
                        <table class="table mb-0">
                            <thead>
                                <tr>
                                    
                                    <th scope="col">#####</th>
                                    <th scope="col">İsim-Soyisim</th>
                                    <th scope="col">Kullanıcı Adı</th>
                                    <th scope="col">Şifre</th>
                                    <th scope="col">Yetki Türü</th>
                                    <th scope="col">Üst Yetki Türü</th>
                                    <th scope="col">Proje Yetki</th>
                                    <th scope="col">Aktif</th>
                                    <th scope="col">Güncellenme Tarihi</th>
                                </tr>
                            </thead>
                            <tbody runat="server" id="tbAdminList">
                            </tbody>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </div>

</asp:Content>
