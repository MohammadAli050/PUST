<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true" CodeBehind="RptGradeSheetProvisional.aspx.cs" Inherits="EMS.Module.result.Report.RptGradeSheetProvisional" %>

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
    <script src="RptGradeSheetProvisional.js"></script>
    <style>
        #tableInfo {
            border-collapse: collapse;
        }

            #tableInfo th, #tableInfo td {
                border: 1px solid black;
                height: 1.5rem;
                width: 1%;
                /*white-space: nowrap;*/
                padding: 10px;
                font-size: 25px;
            }

        .gradeTable {
            border-collapse: collapse;
        }

            .gradeTable th, .gradeTable td {
                border: 1px solid black;
                font-size: 12px;
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
            <label><b style="color: black; font-size: 26px">Provisional GradeSheet</b></label>
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
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <b>Choose Department</b>
                            <br />
                            <uc1:DepartmentUserControl runat="server" ID="ucDepartment" OnDepartmentSelectedIndexChanged="ucDepartment_DepartmentSelectedIndexChanged" />
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
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
                    <br />
                    <div class="row">
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <b>Input Roll</b>
                            <br />
                            <input type="text" class="form-control" id="rollInput" placeholder="Input Roll" />
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
                            <button class="btn btn-secondary w-100" id="btnPrint" onclick="printFunction()">Print</button>
                        </div>
                    </div>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <div id="tablediv"></div>
</asp:Content>
