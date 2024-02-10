<%@ Page Title="Gelir - Gider Ekleme" Language="C#" MasterPageFile="~/kindergarten.Master" AutoEventWireup="true" CodeBehind="IncomeAndExpenseList.aspx.cs" Inherits="KindergartenProject.IncomeAndExpenseList" %>


<%@ Register Src="~/userControl/divInformation.ascx" TagPrefix="uc1" TagName="divInformation" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/customJS/incomeAndExpenseList.js"></script>
    <script src="/customJS/paymentSummaryCommon.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:divInformation runat="server" ID="divInformation" InformationVisible="false" />

    <div class="col-12 col-xl-12">

        <div class="table-responsive">
            <div class="card">
                <div class="card-body">
                    <div class="mb-2 row">
                        <label class="col-form-label col-sm-1 text-sm-left">Yıl :</label>
                        <div class="col-sm-2">
                            <asp:DropDownList runat="server" ID="drpYear" CssClass="form-control" onchange="drpYearMonthChanged('year');">
                            </asp:DropDownList>
                        </div>
                        <label class="col-form-label col-sm-1 text-sm-left">Ay :</label>
                        <div class="col-sm-2">
                            <asp:DropDownList runat="server" ID="drpMonth" CssClass="form-control" onchange="drpYearMonthChanged('month');">
                            </asp:DropDownList>
                        </div>

                        <label class="col-form-label col-sm-2 text-sm-left">Gider Tipi:</label>
                        <div class="col-sm-2">
                            <asp:DropDownList runat="server" ID="drpIncomeAndExpenseType" CssClass="form-control" onchange="onIncomeAndExpenseTypeChanged();">
                            </asp:DropDownList>
                        </div>

                    </div>
                </div>
            </div>
        </div>


        <div class="table-responsive">
            <table class="table mb-0" border="1">
                <thead id="thBody">
                </thead>
            </table>
        </div>
    </div>
    <hr />
    <div class="col-12 col-xl-12">
        <div class="table-responsive">
            <table class="table mb-0">
                <thead>
                    <tr>
                        <th scope="col">Gider / Gelir</th>
                        <th scope="col">Gider / Gelir Adı</th>
                        <th scope="col">Tutar</th>
                        <th scope="col">Tarih</th>
                        <th scope="col">Açıklama</th>
                        <th scope="col">Aktif</th>
                    </tr>
                </thead>
                <tbody runat="server" id="tbIncomeAndExpenseList">
                </tbody>
            </table>
        </div>
    </div>


</asp:Content>
