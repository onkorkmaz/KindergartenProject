<%@ Page Title="Benim dünyama - Öğrenci Listesi" Language="C#" MasterPageFile="~/kindergarten.Master" AutoEventWireup="true" CodeBehind="StudentList.aspx.cs" Inherits="KindergartenProject.StudentList" %>

<%@ Register Src="~/userControl/divInformation.ascx" TagPrefix="uc1" TagName="divInformation" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="customJS/studentList.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">

        <uc1:divInformation runat="server" ID="divInformation" InformationVisible="false" />

        <div class="col-12 col-xl-12">
            <div class="mb-3 row">
                           
                            <div class="col-sm-2">
                                <asp:CheckBox runat="server" Text="Aktif Öğrenciler" ID="chcIsActive" CssClass="form-check-input" />
                            </div>
                            <div class="col-sm-2">
                                <asp:CheckBox runat="server" Text="Görüşmeler" ID="chcInterview" CssClass="form-check-input" />
                            </div>
                        </div>
        </div>

        <div class="col-12 col-xl-12">
            <div class="table-responsive">
                <table class="table mb-0">
                    <thead>
                        <tr>
                            <th scope="col">#####</th>
                            <th scope="col">TCKN</th>
                            <th scope="col">İsim Soyisim</th>
                            <th scope="col">Doğum Tarihi</th>
                            <th scope="col">Baba Bilg.</th>
                            <th scope="col">Anne Bilg.</th>
                            <th scope="col">Kayıt D.</th>
                            <th scope="col">Aktif</th>

                        </tr>
                    </thead>
                    <tbody runat="server" id="tBodyStudentList">
                    </tbody>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
