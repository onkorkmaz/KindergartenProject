<%@ Page Title="" Language="C#" MasterPageFile="~/kindergarten.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="KindergartenProject.Login" %>

<%@ Register Src="~/userControl/divInformation.ascx" TagPrefix="uc1" TagName="divInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/customJS/login.js"></script>
    <link href="css/custom.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="col-12 col-xl-12">
            <div class="customCenter">
                <uc1:divInformation runat="server" ID="divInformation" InformationVisible="false" />
                <div class="card">
                    <div class="card-body">
                        <asp:Panel runat="server" ID="pnlBody">
                            <div class="col-sm-12">
                                <div class="mb-3 row">
                                    <div class="alert alert-primary alert-outline alert-dismissible" role="alert">
                                        <div class="alert-icon">
                                            <i class="far fa-fw fa-bell"></i>
                                        </div>
                                        <div class="alert-message">
                                            <strong>Benim Dünyam Montessori Okulları Kullanıcı Giriş</strong>
                                        </div>
                                    </div>
                                </div>
                                <div class="mb-3 row">
                                    <label class="col-form-label col-sm-4 text-sm-left">Kullanıcı Adı</label>
                                    <div class="col-sm-5">
                                        <asp:TextBox runat="server" ID="txtUserName" CssClass="form-control" placeholder="Kullanıcı adı"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="mb-3 row">
                                    <label class="col-form-label col-sm-4 text-sm-left">Şifre</label>
                                    <div class="col-sm-5">
                                        <asp:TextBox runat="server" TextMode="Password" ID="txtPassword" CssClass="form-control" placeholder="Şifre"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="mb-3 row">
                                    <div class="col-sm-12">
                                        <asp:Button runat="server" ID="btnLogin" CssClass="btn btn-primary " Text="Giriş" OnClientClick="javascript: return validate()" OnClick="btnLogin_Click" />

                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
