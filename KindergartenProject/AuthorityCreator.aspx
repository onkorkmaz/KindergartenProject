<%@ Page Title="Benim Dünyam Yetki Generator" Language="C#" MasterPageFile="~/kindergarten.Master" AutoEventWireup="true" CodeBehind="AuthorityCreator.aspx.cs" Inherits="KindergartenProject.AuthorityCreator" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="customJS/authorityCreator.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="col-12 col-xl-12">
        <div class="card">
            <div class="card-body">
                <div class="mb-3 row">
                    <div class="col-sm-5">
                        <asp:Button runat="server" ID="btnAuthorityCreator" CssClass="btn btn-primary " Text="Yetki Üret" OnClientClick="javascript: return btnAuthorityCreator_click()" />
                    </div>
                </div>
               <div class="mb-3 row">
                            <label class="col-form-label col-sm-2 text-sm-left">Notlar</label>
                            <div class="col-sm-6">
                                <asp:TextBox runat="server" TextMode="MultiLine" ID="txtNotes" CssClass="form-control" Height="400" placeholder="Code"></asp:TextBox>
                            </div>
                        </div>
            </div>
        </div>
    </div>
</asp:Content>
