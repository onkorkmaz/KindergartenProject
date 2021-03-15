<%@ Page Title="Benim dünyam - Ödeme Planları" Language="C#" MasterPageFile="~/kindergarten.Master" AutoEventWireup="true" CodeBehind="PaymentPlan.aspx.cs" Inherits="KindergartenProject.PaymentPlan" %>

<%@ Register Src="~/userControl/divInformation.ascx" TagPrefix="uc1" TagName="divInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script src="customJS/paymentList.js"></script>
     <script src="customJS/paymentCommon.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="row">

        <uc1:divInformation runat="server" ID="divInformation" InformationVisible="false" />

        <div class="col-12 col-xl-12">
            <div class="mb-3 row">

                <div class="col-sm-6">
                    <div class="alert alert-primary alert-dismissible" role="alert">
                        <div class="alert-message">
                            <asp:Label runat="server" ID="lblPaymentStudent" Style="cursor: pointer;"></asp:Label>
                        </div>
                    </div>
                </div>

                <div class="col-sm-6">
                    <div class="alert alert-success alert-dismissible" role="alert">
                        <div class="alert-message">
                            <asp:Label runat="server" ID="lblUnpaymentStudentList" style="cursor: pointer;"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="col-12 col-xl-12">
            <div class="table-responsive" runat="server" id="divMain">
            </div>
        </div>
    </div>
</asp:Content>
