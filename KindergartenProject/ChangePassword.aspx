<%@ Page Title="Benim Dünyam - Şifre Değiştir" Language="C#" MasterPageFile="~/kindergarten.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="KindergartenProject.ChangePassword" %>

<%@ Register Src="~/userControl/divInformation.ascx" TagPrefix="uc1" TagName="divInformation" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/customJS/changePassword.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<uc1:divInformation runat="server" ID="divInformation" InformationVisible="false" />

<div class="card">
        <div class="card-body">
            <asp:Panel runat="server" ID="pnlBody">
                <asp:HiddenField runat="server" ID="hdnId" />

                <div class="mb-3 row">
                    <label class="col-form-label col-sm-2 text-sm-left">Kullanıcı Adı</label>
                    <div class="col-sm-6">
                        <asp:TextBox runat="server" ID="txtUserName" CssClass="form-control" onchange="txtUserName_Change(this.value);" placeholder="Kullanıcı Adı"></asp:TextBox>
                    </div>
                </div>

                 <div class="mb-3 row">
                    <label class="col-form-label col-sm-2 text-sm-left">Şifre</label>
                    <div class="col-sm-6">
                        <asp:TextBox runat="server" TextMode="Password" ID="txtPassword" CssClass="form-control"  placeholder="Şifre"></asp:TextBox>
                    </div>
                </div>

                <div class="mb-3 row">
                    <label class="col-form-label col-sm-2 text-sm-left">Şifre Tekrar</label>
                    <div class="col-sm-6">
                        <asp:TextBox runat="server" TextMode="Password" ID="txtPasswordRepeat" CssClass="form-control"  placeholder="Şifre Tekrarı"></asp:TextBox>
                    </div>
                </div>
                <div class="col-12 col-xl-12">
                    <asp:Button runat="server" ID="btnSubmit" CssClass="btn btn-primary " Text="Güncelle" OnClientClick="javascript: return validateAndSave()"/>
                    <asp:Button runat="server" ID="btnCancel" CssClass="btn btn-secondary" Text="İptal" OnClientClick="javascript: return cancel()"/>
                </div>
            </asp:Panel>
        </div>
    </div>
</asp:Content>
