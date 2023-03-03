<%@ Page Title="Benim Dünyam - Öğretmen Listesi" Language="C#" MasterPageFile="~/kindergarten.Master" AutoEventWireup="true" CodeBehind="WorkerList.aspx.cs" Inherits="KindergartenProject.WorkerList" %>


<%@ Register Src="~/userControl/divInformation.ascx" TagPrefix="uc1" TagName="divInformation" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script src="/customJS/worker.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <uc1:divInformation runat="server" ID="divInformation" InformationVisible="false" />

    <div class="card">
        <div class="card-body">
            <asp:Panel runat="server" ID="pnlBody">
                <asp:HiddenField runat="server" ID="hdnId" />

                <div class="mb-3 row">
                    <label class="col-form-label col-sm-2 text-sm-left">İsim</label>
                    <div class="col-sm-6">
                        <asp:TextBox runat="server" ID="txtName" CssClass="form-control" placeholder="İsim"></asp:TextBox>
                    </div>
                </div>

                <div class="mb-3 row">
                    <label class="col-form-label col-sm-2 text-sm-left">Soyisim</label>
                    <div class="col-sm-6">
                        <asp:TextBox runat="server" ID="txtSurname" CssClass="form-control" placeholder="Soyisim"></asp:TextBox>
                    </div>
                </div>

                <div class="mb-3 row">
                    <label class="col-form-label col-sm-2 text-sm-left">Yönetici mi ?</label>
                    <div class="col-sm-10">
                        <asp:CheckBox runat="server" ID="chcIsManager" CssClass="form-check-input" />
                    </div>
                </div>

                <div class="mb-3 row">
                    <label class="col-form-label col-sm-2 text-sm-left">Maaş</label>
                    <div class="col-sm-6">
                        <asp:TextBox runat="server" ID="txtPrice" CssClass="form-control" placeholder="Maaş Tutarı" onkeypress="return isNumber(event)"></asp:TextBox>
                    </div>
                </div>


                <div class="mb-3 row">
                    <label class="col-form-label col-sm-2 text-sm-left">Tel No</label>
                    <div class="col-sm-6">
                         <asp:TextBox runat="server" ID="txtPhoneNumber" CssClass="form-control" onkeypress="return isNumber(event)" MaxLength="11" placeholder="Tel No"></asp:TextBox>
                    </div>
                </div>
               
                <div class="mb-3 row">
                    <label class="col-form-label col-sm-2 text-sm-left">Öğretmen mi?</label>
                    <div class="col-sm-10">
                        <asp:CheckBox runat="server" Checked="true" ID="chcIsTeacher" CssClass="form-check-input" />
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
                            <th scope="col">İsim</th>
                            <th scope="col">Soyisim</th>
                            <th scope="col">Yönetici</th>
                            <th scope="col">Ücret</th>
                            <th scope="col">Tel</th>
                            <th scope="col">Öğretmen</th>
                            <th scope="col">Aktif</th>
                            <th scope="col">Güncellenme Tarihi</th>
                        </tr>
                    </thead>
                    <tbody runat="server" id="tbWorker">
                    </tbody>
                </table>
            </div>
        </div>

</asp:Content>
