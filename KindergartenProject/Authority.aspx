<%@ Page Title="Benim Dünyam - Ekran - Yetki" Language="C#" MasterPageFile="~/kindergarten.Master" AutoEventWireup="true" CodeBehind="Authority.aspx.cs" Inherits="KindergartenProject.Authority" %>

<%@ Register Src="~/userControl/divInformation.ascx" TagPrefix="uc1" TagName="divInformation" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="/customJS/authority.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <uc1:divInformation runat="server" ID="divInformation" InformationVisible="false" />


    <div class="card">
        <div class="card-body">
            <asp:Panel runat="server" ID="pnlBody">
                <asp:HiddenField runat="server" ID="hdnId" />
                <div class="mb-3 row">
                    <label class="col-form-label col-sm-2 text-sm-left">Yetki Türü</label>
                    <div class="col-sm-6">
                         <asp:DropDownList runat="server" ID="drpAuthorityType" onchange="OnAuthorityType(this.value);" CssClass="form-control">
                                </asp:DropDownList>
                    </div>
                </div>
                <div class="table-responsive">
                    <table class="table mb-0">
                        <tbody runat="server" id="tblAuthorityScreen">
                        </tbody>
                    </table>
                </div>
            </asp:Panel>
        </div>
    </div>

    <div class="col-12 col-xl-12">
        <div class="table-responsive">
            <table class="table mb-0">
                <thead>
                    <tr>
                        <th scope="col">Yetki Adı</th>
                        <th scope="col">Yetki Var mı?</th>
                    </tr>
                </thead>
                <tbody runat="server" id="tbAuthorityList">
                </tbody>
            </table>
        </div>
    </div>

</asp:Content>
