<%@ Page Title="Benim Dünyam - Gelir - Gider Ekleme" Language="C#" MasterPageFile="~/kindergarten.Master" AutoEventWireup="true" CodeBehind="IncomeAndExpenseList.aspx.cs" Inherits="KindergartenProject.IncomeAndExpenseList" %>


<%@ Register Src="~/userControl/divInformation.ascx" TagPrefix="uc1" TagName="divInformation" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/customJS/incomeAndExpenseList.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:divInformation runat="server" ID="divInformation" InformationVisible="false" />

    <div class="col-12 col-xl-12">

         <div class="table-responsive">
                <div class="card">
                    <div class="card-body">
                        <div class="mb-2 row">
                            <label class="col-form-label col-sm-2 text-sm-left">Yıl :</label>
                            <div class="col-sm-2">
                                <asp:DropDownList runat="server" ID="drpYear" CssClass="form-control" onchange="drpYearMontChanged('year');">
                                </asp:DropDownList>
                            </div>
                            <label class="col-form-label col-sm-2 text-sm-left">Ay :</label>
                            <div class="col-sm-2">
                                <asp:DropDownList runat="server" ID="drpMonth" CssClass="form-control" onchange="drpYearMontChanged('month');">
                                </asp:DropDownList>
                            </div>
                                                   
                        </div>
                    </div>
                </div>
            </div>


        <div class="table-responsive">
            <table class="table mb-0" border="1">
                <thead>
                    <tr>
                        <th scope="col">Ay</th>
                        <th scope="col">Ödenen Aidatlar</th>
                        <th scope="col">Beklenen Aidatlar</th>
                        <th scope="col">Diğer Gelirler</th>
                        <th scope="col">Öğr. Maaşları</th>
                        <th scope="col">Diğer Giderler</th>
                        <th scope="col">Anlık Toplam</th>
                        <th scope="col">Beklenen Toplam</th>
                    </tr>
                    <tr>
                        <td><span style="color:darkred;" id="currentMonth"></span></td>
                        <td>&nbsp;&nbsp;<b><span style="color:green;" id="incomeForStudentPayment"></span></b></td>
                        <td>&nbsp;&nbsp;<b><span id="waitingIncomeForStudentPayment"></span></b></td>
                        <td>&nbsp;&nbsp;<b><span style="color:green;" id="incomeWithoutStudentPayment"></span></b></td>
                        <td>&nbsp;&nbsp;<b><span style="color:#d5265b;" id="workerExpenses"></span></b></td>
                        <td>&nbsp;&nbsp;<b><span style="color:red;" id="expenseWithoutWorker"></span></b></td>
                        <td>&nbsp;&nbsp;<b><span style="font-size:16px;" id="currentBalance"></span></b></td>
                        <td>&nbsp;&nbsp;<b><span style="font-size:16px;" id="totalBalance"></span></b></td>
                    </tr>
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
                        <th scope="col">#####</th>
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
