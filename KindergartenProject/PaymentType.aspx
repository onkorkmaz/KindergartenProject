<%@ Page Title="Benim Dünyam - Ödeme Tipleri" Language="C#" MasterPageFile="~/kindergarten.Master" AutoEventWireup="true" CodeBehind="PaymentType.aspx.cs" Inherits="KindergartenProject.PaymentType" %>


<%@ Register Src="~/userControl/divInformation.ascx" TagPrefix="uc1" TagName="divInformation" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script src="/customJS/paymentType.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:divInformation runat="server" ID="divInformation" InformationVisible="false" />

    <div class="card">
        <div class="card-body">
            <asp:Panel runat="server" ID="pnlBody">
                <asp:HiddenField runat="server" ID="hdnId" />

                <div class="mb-3 row">
                    <label class="col-form-label col-sm-2 text-sm-left">Ödeme Tipi</label>
                    <div class="col-sm-6">
                        <asp:TextBox runat="server" ID="txtPaymentType" CssClass="form-control" onchange="txtPaymentType_Change(this.value);" placeholder="Ödeme Tipi"></asp:TextBox>
                    </div>
                </div>

                <div class="mb-3 row">
                    <label class="col-form-label col-sm-2 text-sm-left">Ödeme Tutarı</label>
                    <div class="col-sm-6">
                        <asp:TextBox runat="server" ID="txtAmount" CssClass="form-control" placeholder="Ödeme Tutarı"></asp:TextBox>
                    </div>
                </div>

                <div class="mb-3 row">
                    <label class="col-form-label col-sm-2 text-sm-left">Aktif</label>
                    <div class="col-sm-10">
                        <asp:CheckBox runat="server" ID="chcIsActive" CssClass="form-check-input" Checked="true" />
                    </div>
                </div>

                <div class="col-12 col-xl-12">
                    <asp:Button runat="server" ID="btnSubmit" CssClass="btn btn-primary " Text="Kaydet" OnClientClick="javascript: return validateAndSave()"/>

                     <asp:Button runat="server" ID="btnCancel" CssClass="btn btn-secondary" Text="İptal" OnClientClick="javascript: return cancel()"/>

                </div>

            </asp:Panel>
        </div>
    </div>

   <div class="col-12 col-xl-12">
            <div class="table-responsive">
                <table class="table mb-0">
                    <thead>
                        <tr>
                            <th scope="col">#####</th>
                            <th scope="col">Ödeme Tipi</th>
                            <th scope="col">Ödeme Tutarı</th>
                            <th scope="col">Aktif</th>
                            <th scope="col">Güncellenme Tarihi</th>

                        </tr>
                    </thead>
                    <tbody runat="server" id="tbPaymentTypeList">
                    </tbody>
                </table>
            </div>
        </div>

</asp:Content>
