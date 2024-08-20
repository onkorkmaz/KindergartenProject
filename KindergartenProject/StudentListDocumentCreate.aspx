<%@ Page Title="Öğrenci Listesi Dökümanı Oluştur" Language="C#" MasterPageFile="~/kindergarten.Master" AutoEventWireup="true" CodeBehind="StudentListDocumentCreate.aspx.cs" Inherits="KindergartenProject.StudentListDocumentCreate" %>

<%@ Register Src="~/userControl/divInformation.ascx" TagPrefix="uc1" TagName="divInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/customJS/studenListDocumentCreate.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">

        <uc1:divInformation runat="server" ID="divInformation" InformationVisible="false" />
        <asp:Label runat="server" ID="lblTimer"></asp:Label>
        <div class="col-12 col-xl-12">
            <div class="mb-4 row">
                <label class="col-form-label col-sm-2 text-sm-left" id ="lblClassName">Sınıf Adı</label>
                <div class="col-sm-5" id="divClassList">
                    <asp:DropDownList runat="server" ID="drpClassList" onchange="onClassNameChanged();" CssClass="form-control"></asp:DropDownList>
                </div> 

             </div>

            <div class="mb-4 row">
                <label class="col-form-label col-sm-2 text-sm-left" >Öğrenci Ücretlerini Ekle</label>
              <asp:CheckBox runat="server" ID="chcShowPrice" CssClass="form-check-input" />

             </div>

              <div class="mb-4 row">
                <table width="100%">
                    <tr>
                        <td style="text-align:center;">  <asp:Button Width="200" runat="server" ID="btnCreatePdf" CssClass="btn btn-primary " Text="Döküman Oluştur" OnClientClick="return createPdf();" /> </td>
                    </tr>
                </table>
                 

             </div>
        </div>
    </div>
</asp:Content>
