<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true" CodeBehind="RptAcademicGradeSheet.aspx.cs" Inherits="EMS.Module.result.Report.RptAcademicGradeSheet" %>

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
    <script src="RptAcademicGradeSheet.js"></script>
    <style>
        .academic {
            border-collapse: collapse;
        }

            .academic th, .academic td {
                border: 1px solid black;
                height: 1.5rem;
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
                <div class="card" runat="server" id="divDD">
                    <div class="card-body">
                        <div class="row">
                            <div class="col-4">
                                <lebel><b>Student Roll</b></lebel>
                                <asp:TextBox ID="Text1" runat="server" Cssclass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-4" style="margin-top:20px">
                                <asp:Button ID="btnLoad" runat="server" Width="50%" Text="Load" CssClass="btn btn-info" OnClientClick="loadFunction()" />
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <br />
        <div class="col-4">
            <button class="btn btn-success w-50" id="print" onclick="printFunction()" style="display: none">Print</button>
        </div>
        <br />

        <div id="header"></div>
        <br />
        <div id="first"></div>
        <br />
        <div id="second"></div>
        <br />
    </div>
</asp:Content>
