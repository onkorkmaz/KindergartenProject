<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="divInformation.ascx.cs" Inherits="KindergartenProject.userControl.divInformation" %>

<div class="col-12 col-xl-12" runat="server" id="Information" visible="false">
    <div class="alert alert-danger alert-dismissible" role="alert" runat="server" id="divError">
        <button type="button" class="close" data-dismiss="alert" aria-label="Close">
            <span aria-hidden="true">×</span>
        </button>
        <div class="alert-message">
            <asp:Label runat="server" ID="lblError"></asp:Label>
        </div>
    </div>

    <div class="alert alert-success alert-dismissible" role="alert" runat="server" id="divSuccuess">
        <button type="button" class="close" data-dismiss="alert" aria-label="Close">
            <span aria-hidden="true">×</span>
        </button>
        <div class="alert-message">
            <asp:Label runat="server" ID="lblSuccess"></asp:Label>
            <asp:LinkButton ID="lnkNewRecord" runat="server" OnClick="lnkNewRecord_Click">Yeni Kayıt için tıklayınız</asp:LinkButton> &nbsp;
            <asp:LinkButton ID="lnkRecordList" runat="server" OnClick="lnkRecordList_Click">Liste için tıklayınız</asp:LinkButton>
        </div>
    </div>

</div>
