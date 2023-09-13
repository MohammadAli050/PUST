<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true" CodeBehind="RptStudentTabulation.aspx.cs" Inherits="EMS.Module.result.Report.RptStudentTabulation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <script src="RptStudentTabulation.js?v1.14"></script>
    <style>
        .academic {
            border-collapse: collapse;
        }

            .academic th, .academic td {
                border: 1px solid black;
                height: 1.5rem;
                width: 1%;
            }

        .footers td:empty {
            width: 50px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContainer" runat="server">
    <div class="container-fluid" style="margin-top: 10vh">
        <h2>Student Tabulation Sheet</h2><br />
        <div class="row">
            <div class="col-4">
                <span>Student Roll:</span>
                <input id="Text1" type="text" style="width: 70%" />
            </div>
            <div class="col-4">
                <asp:Button ID="Button1" runat="server" Text="Load" CssClass="btn btn-info" />
            </div>
        </div>
    </div>

    <div id="reportTable">
    </div>

    <br />

    <div class="d-flex flex-row-reverse">
        <button class="btn btn-success" id="btnPrint" onclick="printFunct()" style="display: none">Print</button>
    </div>
</asp:Content>
