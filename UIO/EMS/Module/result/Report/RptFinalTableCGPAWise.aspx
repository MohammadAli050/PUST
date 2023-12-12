<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true" CodeBehind="RptFinalTableCGPAWise.aspx.cs" Inherits="EMS.Module.result.Report.RptFinalTableCGPAWise" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<%@ Register Src="~/UserControls/ProgramUserControl.ascx" TagPrefix="uc1" TagName="ProgramUserControl" %>
<%@ Register Src="~/UserControls/DepartmentUserControl.ascx" TagPrefix="uc1" TagName="DepartmentUserControl" %>
<%@ Register Src="~/UserControls/AdmissionSessionUserControl.ascx" TagPrefix="uc1" TagName="AdmissionSessionUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Academic Grade Sheet
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">

    <script src="https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.3.min.js"></script>
    <link href="../../../CSS/select2.min.css" rel="stylesheet" />
    <script src="../../../JavaScript/select2.full.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script src="RptFinalTableCGPAWise.js"></script>
    <script type="text/javascript">

        function InProgress() {
            var panelProg = $get('divProgress');
            panelProg.style.display = '';
        }

        function onComplete() {
            var panelProg = $get('divProgress');
            panelProg.style.display = 'none';
        }

        function initdropdown() {
            $('#ctl00_MainContainer_ddlHeldIn').select2({
                allowClear: true
            });

            $('#ctl00_MainContainer_ddlCourse').select2({
                allowClear: true
            });

        }

    </script>
    <style>
        .academic {
            border-collapse: collapse;
        }

            .academic th, .academic td {
                border: 1px solid black;
                padding: 2px ;
            }

        .dotted-line {
            border-top: 1px dotted #000;
        }

        .mainTable {
            table-layout: fixed;
        }

            .mainTable td {
                margin-top: 50px;
            }

        table-container {
            display: inline-block;
            width: 50%; /* Adjust as needed */
            vertical-align: top; /* Align the containers at the top */
            box-sizing: border-box;
            padding: 0 10px; /* Add some spacing between tables */
        }

        .headerTable td {
            padding-right: 15px;
        }
        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=80);
            opacity: 0.8;
            z-index: 10000;
        }

        .dropdown {
            width: 266px;
        }

        .select2-results__option {
            line-height: 20px !important;
            height: 34px !important;
        }

        .select2-container {
            width: 306px !important;
        }

        .blink {
            animation: blinker 0.6s linear infinite;
            color: #1c87c9;
            font-size: 30px;
            font-weight: bold;
            font-family: sans-serif;
        }

        @keyframes blinker {
            50% {
                opacity: 0;
            }
        }

        #ctl00_MainContainer_ucDepartment_ddlDepartment, #ctl00_MainContainer_ucProgram_ddlProgram,
        #ctl00_MainContainer_ddlCourse, #ctl00_MainContainer_ddlContinousExam, #ctl00_MainContainer_btnLoad, #ctl00_MainContainer_btnLoadReport {
            height: 40px !important;
            font-size:20px;
        }

        span.select2-selection.select2-selection--single {
            height: 40px;
        }

        span.select2.select2-container.select2-container--default {
            width: 100% !important;
        }

        .sweet-alert {
            z-index: 10000000 !important;
        }
    </style>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContainer" runat="server">
    <div class="container-fluid">
        <div class="row">            
            <div class="col-sm-12" style="font-size: 12pt; margin-top: 10pt;">
                <label><b style="color: black; font-size: 26px">Academic Grade Sheet (For Single Student)</b></label>
            </div>
        </div>

        <div id="divProgress" style="display: none; z-index: 100000000; position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%);">
            <asp:Image ID="LoadingImage" runat="server" ImageUrl="~/Images/Img/Waiting.gif" Height="300px" Width="300px" />
            <div>
                <asp:Label ID="Label1" runat="server" Text="Processing your request.........." ForeColor="Red" Font-Bold="true" Font-Italic="true" Font-Size="30px"></asp:Label>
            </div>
        </div>

        <asp:UpdatePanel runat="server" ID="UpdatePanel02">
            <ContentTemplate>
                <div class="card">
                    <div class="card-body">
                        <div class="row">
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <b>Choose Department</b>
                                <br />
                                <uc1:DepartmentUserControl runat="server" ID="ucDepartment" OnDepartmentSelectedIndexChanged="ucDepartment_DepartmentSelectedIndexChanged" />
                            </div>
                            <div class="col-lg-3 col-md-3 col-sm-3">
                                <b>Choose Program <span style="color: red">*</span></b>
                                <br />
                                <uc1:ProgramUserControl runat="server" ID="ucProgram" OnProgramSelectedIndexChanged="ucProgram_ProgramSelectedIndexChanged" />
                            </div>
                            <div class="col-lg-5 col-md-5 col-sm-5">
                                <script type="text/javascript">
                                    Sys.Application.add_load(initdropdown);
                                </script>
                                <b>Choose Semester & Held In<span style="color: red;">*</span></b>
                                <br />
                                <asp:DropDownList ID="ddlHeldIn" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlHeldIn_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="card">
            <div class="card-body">
                <div class="row">
                    <div class="col-lg-4 col-md-5 col-sm-5">
                        <button class="btn btn-info w-100" onclick="loadFunction()">Load Report</button>
                    </div>
                    <div class="col-lg-4 col-md-5 col-sm-5">
                        <button class="btn btn-success w-50" id="print" onclick="printFunction()" style="display: none">Print</button>
                    </div>
                </div>
            </div>
        </div>
        <br />

        
        <br />

        <div id="table">
        </div>
    </div>
</asp:Content>
