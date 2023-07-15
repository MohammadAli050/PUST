<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true" CodeBehind="RptStudentApplicationForm.aspx.cs" Inherits="EMS.Module.student.RptStudentApplicationForm" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Student Application Form
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContainer" runat="server">
    <div class="container">

        <div class="page-header">
            <h4 class="page-title text-center">আবেদন পত্র</h4>
        </div>

        <div class="row">
            <div class="col-lg-12">
                <asp:Panel ID="FinalMsg" runat="server" CssClass="alert alert-success">
                    <asp:Label ID="lblFinalMsg" runat="server" Text="আপনার চূড়ান্ত ভাবে জমা সম্পন্ন হয়েছে।"></asp:Label>
                </asp:Panel>
            </div>
        </div>

        <div class="card">
            <div class="card-body">
                <div class="row">
                    <div class="col-lg-6 float-left">
                        <asp:Button ID="btnRetunPrevious" runat="server" Text="পূর্ববর্তী পৃষ্ঠা ফিরে যান" CssClass="btn btn-md btn-success" OnClick="btnRetunPrevious_Click" />
                    </div>                    
                    <div class="col-lg-6">
                        <asp:Button ID="btnDownloadAndFinalSubmit" runat="server" Text="চূড়ান্ত ভাবে জমা দিন" OnClick="btnFinalSubmit_Click" CssClass="btn btn-md btn-success float-right" OnClientClick="return confirm('Do you really want to final submit?');" />
                    </div>
                </div>
            </div>
        </div>

        <div class="card">
            <div class="card-header text-center">
                আবেদন পত্র
            </div>

            <div class="card-body">
                <div class="row">
                    <div class="col-lg-12 text-center">
                        <rsweb:ReportViewer ID="ReportViewer1" runat="server" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" asynrendering="true" Width="100%" Height="100%" SizeToReportContent="true">
                        </rsweb:ReportViewer>
                    </div>
                </div>
            </div>

        </div>
    </div>    
</asp:Content>
