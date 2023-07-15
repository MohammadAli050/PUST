<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true" 
    CodeBehind="RptStudentList.aspx.cs" Inherits="EMS.Module.student.Report.RptStudentList" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" 
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Src="~/UserControls/DepartmentUserControl.ascx" TagPrefix="uc1" TagName="DepartmentUserControl" %>
<%@ Register Src="~/UserControls/ProgramUserControl.ascx" TagPrefix="uc1" TagName="ProgramUserControl" %>
<%@ Register Src="~/UserControls/AdmissionSessionUserControl.ascx" TagPrefix="uc1" TagName="AdmissionSessionUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
     Student List Report
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript">

        function InProgress() {
            var panelProg = $get('divProgress');
            panelProg.style.display = '';
        }

        function onComplete() {
            var panelProg = $get('divProgress');
            panelProg.style.display = 'none';
        }

    </script>
    <style type="text/css">
        .auto-style1 {
            width: 71px;
        }

        .auto-style2 {
            width: 150px;
        }

        .auto-style4 {
            width: 100px;
        }

        .auto-style5 {
            width: 300px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContainer" runat="server">
    <div class="PageTitle">
        <label>Program Wise Student List Report </label>
    </div>

    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlMessage" runat="server" Visible="true" CssClass="msgPanel">
                <div class="Message-Area">
                    <asp:Label ID="Label1" runat="server" Text="Message : " Font-Bold="true"></asp:Label>
                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
        <ContentTemplate>
            <div id="divProgress" style="display: none; z-index: 1000; position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%);">
                <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Spinner.gif" Height="250px" Width="250px" />

                <br />
                <b style="color: red">Processing your request ...</b>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="Message-Area" style="height: 80px;">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div>
                    <table id="Table1" style="padding: 5px; width: 100%; height: 70px;" border="0" runat="server">
                        <tr>
                            <td class="auto-style4"><b>Department : </b></td>
                            <td class="auto-style2">
                                <uc1:DepartmentUserControl runat="server" ID="ucDepartment"  OnDepartmentSelectedIndexChanged="OnDepartmentSelectedIndexChanged" />
                            </td>
                            <td class="auto-style4"><b>Program : </b></td>
                            <td class="auto-style2">
                                 <uc1:ProgramUserControl runat="server" ID="ucProgram" OnProgramSelectedIndexChanged="OnProgramSelectedIndexChanged" class="margin-zero dropDownList" />
                            </td>
                            
                        </tr>
                        <tr>
                            <td class="auto-style4"><b>Year : </b></td>
                            <td class="auto-style2">
                                <asp:DropDownList runat="server" ID="ddlYear" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged" AutoPostBack="true" />
                            </td>
                            <td class="auto-style4"><b>Semester : </b></td>
                            <td class="auto-style2">
                                <asp:DropDownList ID="ddlSemester" Width="150px" runat="server"></asp:DropDownList>
                            </td>
                            <td class="auto-style2"><b>Current Session : </b></td>
                            <td class="auto-style2">
                                <uc1:AdmissionSessionUserControl runat="server" ID="ucSession" class="margin-zero dropDownList"/>
                            </td>

                            <td>
                                <asp:Button ID="btnLoad" runat="server" CssClass="pointer" Width="100px" Height="29px" Text="LoadReport" OnClick="btnLoad_Click" />
                            </td>
                        </tr>
                    </table>
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <ajaxToolkit:UpdatePanelAnimationExtender ID="UpdatePanelAnimationExtender1" TargetControlID="UpdatePanel1" runat="server">
        <Animations>
        <OnUpdating>
            <Parallel duration="0">
                <ScriptAction Script="InProgress();" />
                <EnableAction AnimationTarget="btnLoad" Enabled="false" />                   
            </Parallel>
        </OnUpdating>
        <OnUpdated>
            <Parallel duration="0">
                <ScriptAction Script="onComplete();" />
                <EnableAction   AnimationTarget="btnLoad" Enabled="true" />
            </Parallel>
        </OnUpdated>
        </Animations>
    </ajaxToolkit:UpdatePanelAnimationExtender>
    <br />
    <br />

    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
        <ContentTemplate>
            <center>
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" asynrendering="true" Width="100%" Height="100%" SizeToReportContent="true">
            </rsweb:ReportViewer>
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
