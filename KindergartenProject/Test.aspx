<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="KindergartenProject.Test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/2.1.4/jquery.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Verileri Normal Getir" />
            <br />
            <br />
            <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None">
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                <EditRowStyle BackColor="#999999" />
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
            </asp:GridView>
            <br />
            <br />

            <asp:TextBox ID="txtSearchStudent" runat="server"></asp:TextBox>

            <asp:Button ID="Button2" OnClientClick="loadData(); return false;" runat="server" Text="JavaScript ile verileri getir" Width="281px" />
            <br />
            <br />
            <br />
            <br />
            <br />

            <div class="row">

       

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
                            <asp:Label runat="server" ID="lblActiveStudent" Style="cursor: pointer;"></asp:Label>
                        </div>
                    </div>
                </div>
                <div class="col-sm-3">
                    <div class="alert alert-warning alert-dismissible" role="alert">
                        <div class="alert-message">
                            <asp:Label runat="server" ID="lblInterview" Style="cursor: pointer;"></asp:Label>
                        </div>
                    </div>
                </div>
                <div class="col-sm-3">
                    <div class="alert alert-secondary alert-dismissible" role="alert">
                        <div class="alert-message">
                            <asp:Label runat="server" ID="lblPassiveStudent" Style="cursor: pointer;"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <asp:HiddenField runat="server" ID="hdnId" />
        <div class="mb-3 row">
            <label class="col-form-label col-sm-2 text-sm-left" id ="lblClassName">Sınıf Adı</label>
            <div class="col-sm-2" id="divClassList">
                <asp:DropDownList runat="server" ID="drpClassList" onchange="onClassNameChanged();" CssClass="form-control"></asp:DropDownList>
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
                            <th scope="col">&nbsp;</th>
                            <th scope="col">İsim Soyisim</th>
                            <th scope="col">Veli Bilg.</th>
                            <th scope="col">Kayıt D.</th>
                            <th scope="col">O. Sınıfı</th>
                            <th scope="col">Deneme D</th>
                            <th scope="col">Deneme Ders T.</th>

                        </tr>
                    </thead>
                    <tbody runat="server" id="tBodyStudentList">
                    </tbody>
                </table>
            </div>
        </div>
    </div>

        </div>
    </form>
</body>
</html>
