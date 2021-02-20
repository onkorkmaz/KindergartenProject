<%@ Page Title="Benim dünyam - Analiz Paneli" Language="C#" MasterPageFile="~/kindergarten.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="KindergartenProject.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
						<div class="col-xl-12 col-xxl-5 d-flex">
							<div class="w-100">
								<div class="row">
									<div class="col-sm-6">
										<div class="card">
											<div class="card-body">
												<h5 class="card-title mb-4">Öğrenci Sayısı</h5>
												<h1 class="mt-1 mb-3">
													<asp:Label runat="server" ID="lblStudent"></asp:Label>
												</h1>
												<div class="mb-1">
													<span class="text-muted">Bu ay yapılan kayıt sayısı</span><br />
													<span class="text-danger"> <i class="mdi mdi-arrow-bottom-right"></i><asp:Label runat="server" ID="lblMonthStudent"></asp:Label></span>
												</div>
											</div>
										</div>
										<div class="card">
											<div class="card-body">
												<h5 class="card-title mb-4">Öğrenci Görüşme</h5>
												<h1 class="mt-1 mb-3"><asp:Label runat="server" ID="lblInterview"></asp:Label></h1>
												<div class="mb-1">
													<span class="text-muted">Bu ay yapılan görüşme sayısı</span><br />
													<span class="text-danger"> <i class="mdi mdi-arrow-bottom-right"></i> <asp:Label runat="server" ID="lblMonthInterview"></asp:Label> </span>

												</div>
											</div>
										</div>
									</div>
									<div class="col-sm-6">
										<div class="card">
											<div class="card-body">
												<h5 class="card-title mb-4">Ödemeler</h5>
												<h1 class="mt-1 mb-3"><asp:Label runat="server" ID="lblPayment"></asp:Label></h1>
												<div class="mb-1">
													
													<span class="text-muted">Bu ay gelen ödeme</span><br />
													<span class="text-success"> <i class="mdi mdi-arrow-bottom-right"></i><asp:Label runat="server" ID="lblMonthPayment"></asp:Label></span>
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
