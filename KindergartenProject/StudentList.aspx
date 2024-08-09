<%@ Page Title="Öğrenci Listesi" Language="C#" MasterPageFile="~/kindergarten.Master" AutoEventWireup="true" CodeBehind="StudentList.aspx.cs" Inherits="KindergartenProject.StudentList" %>

<%@ Register Src="~/userControl/divInformation.ascx" TagPrefix="uc1" TagName="divInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/customJS/studentList.js"></script>
    <script src="/customJS/studentCommon.js"></script>
    <script src="/customJS/StudentDetailCommon.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">

        <uc1:divInformation runat="server" ID="divInformation" InformationVisible="false" />
        <asp:Label runat="server" ID="lblTimer"></asp:Label>
        <div class="col-12 col-xl-12">
            <div class="mb-3 row">

                <%-- <div class="col-sm-3">
                    <div class="alert alert-primary alert-dismissible" role="alert">
                        <div class="alert-message">
                            <asp:Label runat="server" ID="lblAllStudent" Style="cursor: pointer;"></asp:Label>
                        </div>
                    </div>
                </div>--%>

                <div class="col-sm-3">  
                    <div class="alert alert-success alert-dismissible" role="alert">
                        <div class="alert-message">
                            <asp:Label runat="server" ID="lblActiveStudent" onclick="activeStudent();"></asp:Label>
                        </div>
                    </div>
                </div>
                <div class="col-sm-3">
                    <div class="alert alert-warning alert-dismissible" role="alert">
                        <div class="alert-message">
                            <asp:Label runat="server" ID="lblInterview"></asp:Label>
                        </div>
                    </div>
                </div>
                <div class="col-sm-3">
                    <div class="alert alert-secondary alert-dismissible" role="alert">
                        <div class="alert-message">
                            <asp:Label runat="server" ID="lblPassiveStudent"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <asp:HiddenField runat="server" ID="hdnId" />
        <div class="mb-3 row">
            <label class="col-form-label col-sm-2 text-sm-left" id ="lblClassName">Sınıf Adı</label>
            <div class="col-sm-5" id="divClassList">
                <asp:DropDownList runat="server" ID="drpClassList" onchange="onClassNameChanged();" CssClass="form-control"></asp:DropDownList>
            </div>
             <div class="col-sm-5" id="divCreatePdf">
                 <table>
                     <tr>
                         <td>Ücret Göster&nbsp;&nbsp;<asp:CheckBox runat="server" ID="chcShowPrice" CssClass="form-check-input" /></td>
                         <td>&nbsp;</td>
                         <td> <asp:Button runat="server" ID="btnCreatePdf" CssClass="btn btn-primary " Text="Döküman Oluştur" OnClientClick="return createPdf();" /></td>

                     </tr>
                 </table>
                
            </div>


            <div id="divOldInterview" style="display:none;" class="col-sm-4">
              
                     Eski Görüşmeler &nbsp;
                    <asp:CheckBox runat="server" onclick="onChangeChcOldInterview();" ID="chcInterview" CssClass="form-check-input" />
              
            </div>
        </div>
        <hr />



        <div class="col-12 col-xl-12">
            <div class="table-responsive">
                <table class="table mb-0">
                    <thead>
                        <tr>
                            <th scope="col">#####</th>
                             <th scope="col"></th>
                            <th scope="col">İsim Soyisim</th>
                            <th scope="col">Veli Bilg.</th>
                            <th scope="col">Kayıt D.</th>
                            <th scope="col">Deneme Ders T.</th>
                        </tr>
                    </thead>
                    <tbody runat="server" id="tBodyStudentList">
                    </tbody>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
