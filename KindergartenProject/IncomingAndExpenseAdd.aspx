<%@ Page Title="Benim Dünyam - Gelir - Gider Ekleme" Language="C#" MasterPageFile="~/kindergarten.Master" AutoEventWireup="true" CodeBehind="IncomingAndExpenseAdd.aspx.cs" Inherits="KindergartenProject.IncomingAndExpenseAdd" %>


<%@ Register Src="~/userControl/divInformation.ascx" TagPrefix="uc1" TagName="divInformation" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/customJS/incomingAndExpense.js"></script>
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
                         <asp:DropDownList runat="server" ID="drpIncomingAndExpenseType" onchange="onIncomingAndExpenseTypeChanged()" CssClass="form-control"></asp:DropDownList>
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
                                    <asp:Button runat="server" ID="btnSubmit" CssClass="btn btn-primary " Text="Kaydet" OnClientClick="javascript: return validate()" OnClick="btnSubmit_Click" /></td>
                                <td>
                                    <asp:Button runat="server" ID="btnCancel" CssClass="btn btn-secondary " Text="İptal" OnClick="btnCancel_Click" />
                                </td>
                                <td>
                                    <asp:Button runat="server" ID="btnDelete" CssClass="btn btn-danger" OnClientClick="javascript: return validateDelete()" Text="Sil" OnClick="btnDelete_Click" />
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ReadOnly="true" ID="txtIncomingAndExpenseTypeName" CssClass="form-control" placeholder="Tutar" Font-Bold="True"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>

            </asp:Panel>
        </div>
    </div>
</asp:Content>
