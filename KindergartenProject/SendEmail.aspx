<%@ Page Title="Benim dünyam - Mail Gönder" Language="C#" MasterPageFile="~/kindergarten.Master" AutoEventWireup="true" CodeBehind="SendEmail.aspx.cs" Inherits="KindergartenProject.SendEmail" %>

<%@ Register Src="~/userControl/divInformation.ascx" TagPrefix="uc1" TagName="divInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="customJS/sendEmail.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <uc1:divInformation runat="server" ID="divInformation" InformationVisible="false" />
        
        <div class="col-12 col-xl-12">
            <div class="card">
                <div class="card-body">
                    <asp:HiddenField runat="server" ID="hdnId" />
                    <div class="mb-2 row">
                        <label class="col-form-label col-sm-2 text-sm-left">Öğrenci Bilgileri :</label>
                        <div class="col-sm-6">
                            <label class="col-form-label col-sm-12 text-sm-left"><asp:Label runat="server" ID="lblStudentInto"></asp:Label></label>
                            <hr />
                        </div>
                    </div>
                    <div class="mb-2 row">
                        <label class="col-form-label col-sm-2 text-sm-left">Email Adresi :</label>
                        <div class="col-sm-3">
                            <asp:TextBox runat="server" ID="txtEmail" CssClass="form-control"  MaxLength="50" placeholder="Email"></asp:TextBox>
                        </div>
                        <div class="col-sm-2">
                            <asp:Button runat="server" ID="btnSendEmail" CssClass="btn btn-primary " Text="Mail Gönder"  OnClientClick="javascript: return validate()" OnClick="btnSendEmail_Click" />
                        </div>
                    </div>
                </div>
                <div class="card-body">
                    <asp:Panel runat="server" ID="pnlBody">
                        <div class="col-12 col-xl-12">
                            <div class="table-responsive" runat="server" id="divMain">
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>

    </div>
</asp:Content>
