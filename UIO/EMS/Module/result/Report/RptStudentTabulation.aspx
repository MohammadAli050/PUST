<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true" CodeBehind="RptStudentTabulation.aspx.cs" Inherits="EMS.Module.result.Report.RptStudentTabulation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Tabulation Sheet For Single Student
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

     <div class="row">
        <div class="col-sm-12" style="font-size: 12pt; margin-top: 10pt;">
            <label><b style="color: black; font-size: 26px">Tabulation Sheet (For Single Student)</b></label>
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
            <div class="col-4">
                <lebel><b>Student Roll:</b></lebel>
                <input id="Text1" type="text" style="width:70%" />
            </div>
            <div class="col-4">
                <asp:Button ID="Button1" runat="server" Width="50%" Text="Load" CssClass="btn btn-info" />
            </div>
        </div>
                    </div>
                    </div>
                    </ContentTemplate>
        </asp:UpdatePanel>
    

    <div id="reportTable">
    </div>

    <br />

    <div class="d-flex flex-row-reverse">
        <button class="btn btn-success" id="btnPrint" onclick="printFunct()" style="display: none">Print</button>
    </div>
</asp:Content>
