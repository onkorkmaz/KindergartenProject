﻿<%@ Page Title="Benim dünyam - Analiz Paneli" Language="C#" MasterPageFile="~/kindergarten.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="KindergartenProject.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/customJS/default.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="container-fluid p-0">

        <div class="row mb-2 mb-xl-3">
            <div class="col-auto d-none d-sm-block">
                <h3><strong>Analiz</strong> Paneli</h3>
            </div>

            <div class="col-auto ml-auto text-right mt-n1">
                <nav aria-label="breadcrumb">
                    <ol class="breadcrumb bg-transparent p-0 mt-1 mb-0">
                        <li class="breadcrumb-item"><a href="#">Benim Dünyam</a></li>
                        <li class="breadcrumb-item"><a href="#">Panel</a></li>
                        <li class="breadcrumb-item active" aria-current="page">Analiz</li>
                    </ol>
                </nav>
            </div>
        </div>
        <div class="row">
            <div class="col-xl-12 d-flex">
                <div class="w-100">

                    <div class="row">
                        <div class="col-sm-12">
                            <div class="card">
                                <div class="card-body">
                                    <table class="table mb-0">
                                        <thead>
                                            <tr>
                                                <th scope="col">Ay</th>
                                                <th scope="col">Ödenen Aidatlar</th>
                                                <th scope="col">Beklenen Aidatlar</th>
                                                <th scope="col">Diğer Gelirler</th>
                                                <th scope="col">Öğr. Maaşları</th>
                                                <th scope="col">Diğer Giderler</th>
                                                <th scope="col">Anlık Toplam</th>
                                                <th scope="col">Beklenen Toplam</th>
                                            </tr>
                                            <tr>
                                                <td><span style="color: darkred;" id="currentMonth"></span></td>
                                                <td>&nbsp;&nbsp;<b><span style="color: green;" id="incomeForStudentPayment"></span></b></td>
                                                <td>&nbsp;&nbsp;<b><span id="waitingIncomeForStudentPayment"></span></b></td>
                                                <td>&nbsp;&nbsp;<b><span style="color: green;" id="incomeWithoutStudentPayment"></span></b></td>
                                                <td>&nbsp;&nbsp;<b><span style="color: #d5265b;" id="workerExpenses"></span></b></td>
                                                <td>&nbsp;&nbsp;<b><span style="color: red;" id="expenseWithoutWorker"></span></b></td>
                                                <td>&nbsp;&nbsp;<b><span style="font-size: 16px;" id="currentBalance"></span></b></td>
                                                <td>&nbsp;&nbsp;<b><span style="font-size: 16px;" id="totalBalance"></span></b></td>
                                            </tr>
                                        </thead>
                                    </table>
                                </div>
                            </div>
                        </div>

                        <div class="col-sm-6">
                            <div class="card">
                                <div class="card-body">
                                    <h5 class="card-title mb-4"><b>Öğrenci Sayısı</b> :
                                        <asp:Label runat="server" ID="lblStudent"></asp:Label></h5>

                                    <hr />
                                    <div class="mb-1">
                                        <span class="text-muted"><b>Bu ay yapılan kayıt sayısı</b> : </span>
                                        <span class="text-danger"><i class="mdi mdi-arrow-bottom-right"></i>
                                            <asp:Label runat="server" ID="lblMonthStudent"></asp:Label></span>
                                    </div>
                                </div>
                            </div>

                            <div class="card">
                                <div class="card-body">
                                    <h3 class="card-title mb-4"><b>Bu gün doğum günü olanlar</b></h3>
                                    <span class="text-success"><i class="mdi mdi-arrow-bottom-right"></i>
                                        <asp:Label runat="server" ID="lblBirthdayToday"></asp:Label></span>
                                    <hr />
                                    <div class="mb-1">

                                        <span class="text-muted"><b>Bu ay doğum günü olanlar</b></span><br />
                                        <br />
                                        <span class="text-success"><i class="mdi mdi-arrow-bottom-right"></i>
                                            <asp:Label runat="server" ID="lblBirthdayThisMonth"></asp:Label></span>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-sm-6">
                            <div class="card">
                                <div class="card-body">
                                    <h5 class="card-title mb-4"><b>Öğrenci Görüşme</b> :
                                        <asp:Label runat="server" ID="lblInterview"></asp:Label></h5>
                                    <h1 class="mt-1 mb-3"></h1>
                                    <hr />
                                    <div class="mb-1">
                                        <span class="text-muted"><b>Bu ay yapılan görüşme sayısı</b> : </span>
                                        <span class="text-danger"><i class="mdi mdi-arrow-bottom-right"></i>
                                            <asp:Label runat="server" ID="lblMonthInterview"></asp:Label>
                                        </span>

                                    </div>
                                </div>
                            </div>
                            <div class="card">
                                <div class="card-body">
                                    <h5 class="card-title mb-4"><b>Sınıf Dağılımları</b></h5>
                                    <hr />
                                    <div class="table-responsive">
                                        <div runat="server" id="divTblClass"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
