<%@ Page Title="Benim Dünyam - Gelir - Gider Tipi" Language="C#" MasterPageFile="~/kindergarten.Master" AutoEventWireup="true" CodeBehind="IncomeAndExpenseType.aspx.cs" Inherits="KindergartenProject.IncomeAndExpenseType" %>


<%@ Register Src="~/userControl/divInformation.ascx" TagPrefix="uc1" TagName="divInformation" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/customJS/IncomeAndExpenseType.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:divInformation runat="server" ID="divInformation" InformationVisible="false" />

    <div class="card">
        <div class="card-body">
            <asp:Panel runat="server" ID="pnlBody">
                <asp:HiddenField runat="server" ID="hdnId" />

                <div class="mb-3 row">
                    <label class="col-form-label col-sm-2 text-sm-left">Tip</label>
                    <div class="col-sm-6">
                        <asp:DropDownList runat="server" ID="drpType" CssClass="form-control"></asp:DropDownList>
                    </div>
                </div>

                <div class="mb-3 row">
                    <label class="col-form-label col-sm-2 text-sm-left">Adı</label>
                    <div class="col-sm-6">
                        <asp:TextBox runat="server" ID="txtName" CssClass="form-control" placeholder="Adı"></asp:TextBox>
                    </div>
                </div>

                <div class="mb-3 row">
                    <label class="col-form-label col-sm-2 text-sm-left">Aktif</label>
                    <div class="col-sm-10">
                        <asp:CheckBox runat="server" ID="chcIsActive" CssClass="form-check-input" Checked="true" />
                    </div>
                </div>

                <div class="col-12 col-xl-12">
                    <asp:Button runat="server" ID="btnSubmit" CssClass="btn btn-primary " Text="Kaydet" OnClientClick="javascript: return validateAndSave()" />

                    <asp:Button runat="server" ID="btnCancel" CssClass="btn btn-secondary" Text="İptal" OnClientClick="javascript: return cancel()" />

                </div>

            </asp:Panel>
        </div>
    </div>

    <div class="col-12 col-xl-12">
        <div class="table-responsive">
            <table class="table mb-12" border="2" >
                <tr>
                    <td style="vertical-align:top">
                         <span style="text-align:center">Gelir Türleri</span><hr />
                        <table class="table mb-12" border="1">
                            <thead>
                                <tr>
                                    <th scope="col">#####</th>
                                    <th scope="col">İsim</th>
                                    <th scope="col">Aktif</th>
                                    <th scope="col">Tip</th>

                                </tr>
                                <tbody runat="server" id="tbIncome">
                                </tbody>
                            </thead>
                        </table>

                    </td>
                    <td>&nbsp;
                    </td>
                    <td style="vertical-align:top">
                        <span style="text-align:center">Gider Türleri</span><hr />
                        <table class="table mb-12" border="1">
                            <thead>
                                <tr>                                   
                                    <th scope="col">#####</th>
                                    <th scope="col">İsim</th>
                                    <th scope="col">Aktif</th>
                                    <th scope="col">Tip</th>

                                </tr>
                                <tbody runat="server" id="tbExpense">
                                </tbody>
                            </thead>
                        </table>
                    </td>
                </tr>

            </table>
        </div>
    </div>
</asp:Content>
