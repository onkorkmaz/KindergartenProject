﻿<%@ Page Title="Gelir - Gider Ekleme" Language="C#" MasterPageFile="~/kindergarten.Master" AutoEventWireup="true" CodeBehind="IncomeAndExpenseAdd.aspx.cs" Inherits="KindergartenProject.IncomeAndExpenseAdd" %>


<%@ Register Src="~/userControl/divInformation.ascx" TagPrefix="uc1" TagName="divInformation" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/customJS/incomeAndExpense.js"></script>
    <script src="/customJS/paymentSummaryCommon.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:divInformation runat="server" ID="divInformation" InformationVisible="false" />

    <div class="card">
        <div class="card-body">
            <asp:Panel runat="server" ID="pnlBody">
                <asp:HiddenField runat="server" ID="hdnId" />

                <div class="mb-3 row">
                    <label class="col-form-label col-sm-2 text-sm-left">Gelir / Gider Tipi</label>
                    <div class="col-sm-6">
                        <asp:DropDownList runat="server" ID="drpIncomeAndExpenseType" onchange="onIncomeAndExpenseTypeChanged()" CssClass="form-control"></asp:DropDownList>
                    </div>
                </div>

                <div class="mb-3 row" runat="server" id="divWorker">
                    <label class="col-form-label col-sm-2 text-sm-left">Çalışanlar</label>
                    <div class="col-sm-6">
                        <asp:DropDownList runat="server" ID="drpWorker" onchange="onWorkerChanged()" CssClass="form-control"></asp:DropDownList>
                    </div>
                </div>


                <div class="mb-3 row">
                    <label class="col-form-label col-sm-2 text-sm-left">Tutar</label>
                    <div class="col-sm-6">
                        <asp:TextBox runat="server" ID="txtAmount" CssClass="form-control" placeholder="Tutar" onkeypress="return isNumber(event)"></asp:TextBox>
                    </div>
                </div>

                <div class="mb-3 row">
                    <label class="col-form-label col-sm-2 text-sm-left">Açıklama</label>
                    <div class="col-sm-6">
                        <asp:TextBox runat="server" ID="txtDescription" CssClass="form-control" placeholder="Açıklama"></asp:TextBox>
                    </div>
                </div>


                <div class="mb-3 row">
                    <label class="col-form-label col-sm-2 text-sm-left">İşlem Tarihi</label>
                    <div class="col-sm-6">
                        <asp:TextBox ID="txtProcessDate" runat="server" TextMode="Date" CssClass="form-control" />
                    </div>
                </div>


                <div class="mb-3 row">
                    <label class="col-form-label col-sm-2 text-sm-left">Aktif</label>
                    <div class="col-sm-10">
                        <asp:CheckBox runat="server" ID="chcIsActive" CssClass="form-check-input" Checked="true" />
                    </div>
                </div>

                <div class="mb-3 row">
                    <div class="col-sm-5">
                        <table cellpadding="4">
                            <tr>
                                <td>
                                    <asp:Button runat="server" ID="btnSubmit" CssClass="btn btn-primary " Text="Kaydet" OnClientClick="javascript: return validateAndSave()" />
                                </td>
                                <td>
                                    <asp:Button runat="server" ID="btnCancel" CssClass="btn btn-secondary " Text="İptal" OnClick="btnCancel_Click" />
                                </td>
                                <td>
                                    <asp:Button runat="server" ID="btnDelete" CssClass="btn btn-danger" OnClientClick="javascript: return validateDelete()" Text="Sil" OnClick="btnDelete_Click" />
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ReadOnly="true" ID="txtIncomeAndExpenseTypeName" CssClass="form-control" placeholder="Tutar" Font-Bold="True" ForeColor="White"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>

            </asp:Panel>
        </div>
    </div>

        <div class="table-responsive">
            <div class="card">
                <div class="card-body">
                     <div class="mb-2 row">
                         Gelir Gider Düzenleme Listesi
                         <hr />
                         </div>
                    <div class="mb-2 row">
                        <label class="col-form-label col-sm-1 text-sm-left">Yıl :</label>
                        <div class="col-sm-2">
                            <asp:DropDownList runat="server" ID="drpYearList" CssClass="form-control" onchange="drpYearMonthChanged('year');">
                            </asp:DropDownList>
                        </div>
                        <label class="col-form-label col-sm-1 text-sm-left">Ay :</label>
                        <div class="col-sm-2">
                            <asp:DropDownList runat="server" ID="drpMonthList" CssClass="form-control" onchange="drpYearMonthChanged('month');">
                            </asp:DropDownList>
                        </div>
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
             </div>
         </div>


</asp:Content>
