<%@ Page Title="Cache Temiizliği" Language="C#" MasterPageFile="~/kindergarten.Master" AutoEventWireup="true" CodeBehind="CacheClear.aspx.cs" Inherits="KindergartenProject.CacheClear" %>

<%@ Register Src="~/userControl/divInformation.ascx" TagPrefix="uc1" TagName="divInformation" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

     <script src="/customJS/cacheClear.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<uc1:divInformation runat="server" ID="divInformation" InformationVisible="false" />


<div class="card">
        <div class="card-body">
            <asp:Panel runat="server" ID="pnlBody">
                <div class="mb-3 row">
                    <label class="col-form-label col-sm-2 text-sm-left">Ödeme Cache Temizliği</label>
                    <div class="col-sm-6">
                       <asp:Button runat="server" ID="btnClearPaymentCache" CssClass="btn btn-primary " Text="Ödeme Cache" OnClientClick="javascript: return clearPaymentCache()" />
                    </div>
                </div>

                <div class="mb-3 row">
                    <label class="col-form-label col-sm-2 text-sm-left">Öğrenci Cache Temizliği</label>
                    <div class="col-sm-6">
                       <asp:Button runat="server" ID="Button1" CssClass="btn btn-primary " Text="Ödeme Cache" OnClientClick="javascript: return clearStudentCache()" />
                    </div>
                </div>


            </asp:Panel>
        </div>
    </div>

</asp:Content>
