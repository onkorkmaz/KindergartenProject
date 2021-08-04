﻿<%@ Page Title="Benim Dünyam - Sınıflar" Language="C#" MasterPageFile="~/kindergarten.Master" AutoEventWireup="true" CodeBehind="ClassList.aspx.cs" Inherits="KindergartenProject.ClassList" %>



<%@ Register Src="~/userControl/divInformation.ascx" TagPrefix="uc1" TagName="divInformation" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script src="customJS/classList.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:divInformation runat="server" ID="divInformation" InformationVisible="false" />

    <div class="card">
        <div class="card-body">
            <asp:Panel runat="server" ID="pnlBody">
                <asp:HiddenField runat="server" ID="hdnId" />

                <div class="mb-3 row">
                    <label class="col-form-label col-sm-2 text-sm-left">Ödeme Tipi</label>
                    <div class="col-sm-6">
                        <asp:TextBox runat="server" ID="txtName" CssClass="form-control" onchange="txtClass_Change(this.value);" placeholder="Adı"></asp:TextBox>
                    </div>
                </div>

                <div class="mb-3 row">
                    <label class="col-form-label col-sm-2 text-sm-left">Ödeme Tipi</label>
                    <div class="col-sm-6">
                        <asp:TextBox runat="server" ID="txtDescription" CssClass="form-control" placeholder="Açıklama"></asp:TextBox>
                    </div>
                </div>

                <div class="mb-3 row">
                    <label class="col-form-label col-sm-2 text-sm-left">Ödeme Tutarı</label>
                    <div class="col-sm-6">
                        <asp:TextBox runat="server" ID="txtWarningOfStudentCount" onkeypress="return isNumber(event)" CssClass="form-control" placeholder="Max Öğrenci Sayısı"></asp:TextBox>
                    </div>
                </div>

                <div class="mb-3 row">
                    <label class="col-form-label col-sm-2 text-sm-left">Aktif</label>
                    <div class="col-sm-10">
                        <asp:CheckBox runat="server" ID="chcIsActive" CssClass="form-check-input" Checked="true" />
                    </div>
                </div>

                <div class="col-12 col-xl-12">
                    <asp:Button runat="server" ID="btnSubmit" CssClass="btn btn-primary " Text="Kaydet" OnClientClick="javascript: return validateAndSave()"/>

                     <asp:Button runat="server" ID="btnCancel" CssClass="btn btn-secondary" Text="İptal" OnClientClick="javascript: return cancel()"/>

                </div>

            </asp:Panel>
        </div>
    </div>

   <div class="col-12 col-xl-12">
            <div class="table-responsive">
                <table class="table mb-0">
                    <thead>
                        <tr>
                            <th scope="col">#####</th>
                            <th scope="col">Sınıf Adı</th>
                            <th scope="col">Açıklama</th>
                            <th scope="col">Max Öğrenci Sayısı</th>
                            <th scope="col">Aktif</th>
                            <th scope="col">Güncellenme Tarihi</th>

                        </tr>
                    </thead>
                    <tbody runat="server" id="tbClassList">
                    </tbody>
                </table>
            </div>
        </div>

</asp:Content>

