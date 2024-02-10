<%@ Page Title="Ödeme Planları" Language="C#" MasterPageFile="~/kindergarten.Master" AutoEventWireup="true" CodeBehind="PaymentPlan.aspx.cs" Inherits="KindergartenProject.PaymentPlan" %>

<%@ Register Src="~/userControl/divInformation.ascx" TagPrefix="uc1" TagName="divInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/customJS/paymentList.js"></script>
    <script src="/customJS/paymentCommon.js"></script>
    <script src="/customJS/StudentDetailCommon.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">

        <uc1:divInformation runat="server" ID="divInformation" InformationVisible="false" />
        <div class="col-12 col-xl-12">

            <div class="table-responsive">
                <div class="card">
                    <div class="card-body">
                        <div class="mb-2 row">
                            <label class="col-form-label col-sm-2 text-sm-left">Yıl :</label>
                            <div class="col-sm-2">
                                <asp:DropDownList runat="server" ID="drpYear" CssClass="form-control" onchange="drpYearMonthDayChanged('year');">
                                </asp:DropDownList>
                            </div>
                            <label class="col-form-label col-sm-2 text-sm-left">Ay :</label>
                            <div class="col-sm-2">
                                 <asp:DropDownList runat="server" ID="drpMonth" CssClass="form-control" onchange="drpYearMonthDayChanged('month');"></asp:DropDownList>
                            </div>
                            <label class="col-form-label col-sm-2 text-sm-left">Ödeme yapmayanlar :</label>
                            <div class="col-sm-2">
                                <asp:CheckBox runat="server" onclick="onIsPaymentDetailChange();" ID="chcIsPaymentDetail" CssClass="form-check-input" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <hr />
            <div class="table-responsive" runat="server" id="divMain">
            </div>
        </div>
    </div>
</asp:Content>
