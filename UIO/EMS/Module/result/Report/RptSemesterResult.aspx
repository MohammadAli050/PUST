<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true" CodeBehind="RptSemesterResult.aspx.cs" Inherits="EMS.Module.result.Report.RptSemesterResult" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<%@ Register Src="~/UserControls/ProgramUserControl.ascx" TagPrefix="uc1" TagName="ProgramUserControl" %>
<%@ Register Src="~/UserControls/DepartmentUserControl.ascx" TagPrefix="uc1" TagName="DepartmentUserControl" %>
<%@ Register Src="~/UserControls/AdmissionSessionUserControl.ascx" TagPrefix="uc1" TagName="AdmissionSessionUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Semester Result
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <script src="https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.3.min.js"></script>
    <link href="../../../CSS/select2.min.css" rel="stylesheet" />
    <script src="../../../JavaScript/select2.full.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>

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
    <script src="RptSemesterResult.js?v.1.02"></script>
    <style>

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
        #tableInfo {
            border-collapse: collapse;
        }

            #tableInfo th, #tableInfo td {
                border: 1px solid black;
                height: 1.5rem;
                width: 1%;
                white-space: nowrap;
                padding: 20px;
            }

        footer span {
            font-family: serif;
            font-size: 20px;
        }

        footer label {
            font-family: serif;
            font-size: 20px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContainer" runat="server">

    <div class="row">
        <div class="col-sm-12" style="font-size: 12pt; margin-top: 10pt;">
            <label><b style="color: black; font-size: 26px">Semester Result Sheet</b></label>
        </div>
    </div>
    <div id="divProgress" style="display: none; z-index: 100000000; position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%);">
        <asp:Image ID="LoadingImage" runat="server" ImageUrl="~/Images/Img/Waiting.gif" Height="300px" Width="300px" />
        <div>
            <asp:Label ID="Label1" runat="server" Text="Processing your request.........." ForeColor="Red" Font-Bold="true" Font-Italic="true" Font-Size="30px"></asp:Label>
        </div>
    </div>

    <hr />

    <asp:UpdatePanel runat="server" ID="UpdatePanel02">
        <ContentTemplate>
            <div class="card" runat="server" id="divDD">
                <div class="card-body">
                    <div class="row">
                        <div class="col-lg-5 col-md-5 col-sm-5">
                            <b>Choose Department</b>
                            <br />
                            <uc1:DepartmentUserControl runat="server" ID="ucDepartment" OnDepartmentSelectedIndexChanged="ucDepartment_DepartmentSelectedIndexChanged" />
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <b>Choose Program</b>
                            <br />
                            <uc1:ProgramUserControl runat="server" ID="ucProgram" OnProgramSelectedIndexChanged="ucProgram_ProgramSelectedIndexChanged" />
                            <%--="ucProgram_ProgramSelectedIndexChanged"--%>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <script type="text/javascript">
                                Sys.Application.add_load(initdropdown);
                            </script>
                            <b>Choose Semester & Held in to Load Tabulation</b>
                            <br />
                            <asp:DropDownList ID="ddlHeldIn" runat="server" CssClass="form-control" AutoPostBack="true"></asp:DropDownList>
                            <%--OnSelectedIndexChanged="ddlHeldIn_SelectedIndexChanged"--%>
                        </div>
                    </div>

                    <div class="row" style="margin-top: 10px">

                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <br />
                            <%--<asp:Button ID="btnLoadReport" runat="server" Text="Click Here To View Report" CssClass="btn-info w-100" OnClick="btnLoadReport_Click" />--%>
                            <button class="btn btn-info w-100" id="btnLoad" onclick="loadFunction()">Click Here To View Report</button>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <br />
                            <%--<asp:Button ID="btnLoadReport" runat="server" Text="Click Here To View Report" CssClass="btn-info w-100" OnClick="btnLoadReport_Click" />--%>
                            <button class="btn btn-success w-50" id="btnPrint" onclick="printFunction()">Print</button>
                        </div>
                    </div>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

    <div id="tablediv"></div>    
</asp:Content>
