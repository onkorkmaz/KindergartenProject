<%@ Page Title="" Language="C#" MasterPageFile="~/kindergarten.Master" AutoEventWireup="true" CodeBehind="StudentAttendanceBookList.aspx.cs" Inherits="KindergartenProject.StudentAttendanceBookList" %>

<%@ Register Src="~/userControl/divInformation.ascx" TagPrefix="uc1" TagName="divInformation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/customJS/studentAttendanceBook.js"></script>
    <script src="/customJS/studentCommon.js"></script>
    <script src="/customJS/StudentDetailCommon.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">

        <uc1:divInformation runat="server" ID="divInformation" InformationVisible="false" />

        <div class="col-12 col-xl-12">
            <div class="table-responsive">
                <div class="card">
                    <div class="card-body">
                        <div class="mb-2 row">
                            <label class="col-form-label col-sm-2 text-sm-left">Yıl :</label>
                            <div class="col-sm-2">
                                <asp:DropDownList runat="server" ID="drpYear" CssClass="form-control" onchange="drpYearMonthDayChanged('year');">
                                </asp:DropDownList>
                            </div>
                            <label class="col-form-label col-sm-2 text-sm-left">Ay :</label>
                            <div class="col-sm-2">
                                <asp:DropDownList runat="server" ID="drpMonth" CssClass="form-control" onchange="drpYearMonthDayChanged('month');">
                                </asp:DropDownList>
                            </div>
                            <label class="col-form-label col-sm-2 text-sm-left">Dönem</label>
                            <div class="col-sm-2">
                                <asp:DropDownList runat="server" ID="drpDays" CssClass="form-control" onchange="drpYearMonthDayChanged('day');">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="table-responsive">
                
                   <table>
                       <tr>
                           <td width="100"><b><label class="col-form-label text-sm-left" id="lblInfo"></label></b></td>
                           <td>
                               Yoklama da bugünü göster &nbsp;
                               <asp:CheckBox runat="server" Checked="true" onclick="onChangeChcCurrentDay();" ID="chcCurrentDay" CssClass="form-check-input" />
                           </td>
                           <td width="150"> </td>
                           <td width="100">Sınıf Adı : </td>
                           <td width="200"> <asp:DropDownList runat="server" ID="drpClassList" onchange="onClassNameChanged();" CssClass="form-control"></asp:DropDownList></td>
                       </tr>
                    
                       </table>
                <hr />
                <div class="table-responsive">
                    <table class="table mb-0">
                        <thead runat="server" id="studentAttendanceHeader">
                        </thead>
                        <tbody runat="server" id="studentAttendanceList">
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
