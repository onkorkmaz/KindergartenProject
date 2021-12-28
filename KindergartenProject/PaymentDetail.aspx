<%@ Page Title="Benim Dünyam - Ödeme Detayı" Language="C#" MasterPageFile="~/kindergarten.Master" AutoEventWireup="true" CodeBehind="PaymentDetail.aspx.cs" Inherits="KindergartenProject.PaymentDetail" %>

<%@ Register Src="~/userControl/divInformation.ascx" TagPrefix="uc1" TagName="divInformation" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/customJS/paymentDetail.js"></script>
    <script src="/customJS/paymentCommon.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <uc1:divInformation runat="server" ID="divInformation" InformationVisible="false" />

        <div class="col-12 col-xl-12">
            <div class="card">
                <div class="card-body">
                    <asp:HiddenField runat="server" ID="hdnId" />
                    <div class="mb-2 row">
                        <label class="col-form-label col-sm-2 text-sm-left">Öğrenci Bilgileri :</label>
                        <div class="col-sm-6">
                            <label class="col-form-label col-sm-12 text-sm-left"><asp:Label runat="server" ID="lblStudentInto"></asp:Label></label>
                            <hr />
                        </div>
                    </div>
                    <div class="mb-2 row">
                        <label class="col-form-label col-sm-2 text-sm-left">Ödeme Yılı :</label>
                        <div class="col-sm-2">
                            <asp:DropDownList runat="server" ID="drpYear" CssClass="form-control" onchange="drpYear_Changed();">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="card-body">
                    <asp:Panel runat="server" ID="pnlBody">
                        <div class="col-12 col-xl-12">
                            <div class="table-responsive" runat="server" id="divMain">
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
