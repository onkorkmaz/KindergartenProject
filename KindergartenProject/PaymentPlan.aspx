<%@ Page Title="Benim Dünyam - Ödeme Planları" Language="C#" MasterPageFile="~/kindergarten.Master" AutoEventWireup="true" CodeBehind="PaymentPlan.aspx.cs" Inherits="KindergartenProject.PaymentPlan" %>

<%@ Register Src="~/userControl/divInformation.ascx" TagPrefix="uc1" TagName="divInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/customJS/paymentList.js"></script>
    <script src="/customJS/paymentCommon.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">

        <uc1:divInformation runat="server" ID="divInformation" InformationVisible="false" />
        <div class="col-12 col-xl-12">

            <table>
                <tr>
                    <td>Aidat Ödemesi Yapmayanlar &nbsp;
                               <asp:CheckBox runat="server" onclick="onIsPaymentDetailChange();" ID="chcIsPaymentDetail" CssClass="form-check-input" />
                    </td>
                </tr>

            </table>
            <hr />
            <div class="table-responsive" runat="server" id="divMain">
            </div>
        </div>
    </div>
</asp:Content>
