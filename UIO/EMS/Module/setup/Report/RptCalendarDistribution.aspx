<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true"
    Inherits="RptCalendarDistribution" CodeBehind="RptCalendarDistribution.aspx.cs" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Src="~/UserControls/DepartmentUserControl.ascx" TagPrefix="uc1" TagName="DepartmentUserControl" %>
<%@ Register Src="~/UserControls/ProgramUserControl.ascx" TagPrefix="uc1" TagName="ProgramUserControl" %>

<asp:Content ID="Content3" ContentPlaceHolderID="Title" runat="Server">
    Course List
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
    <style type="text/css">
        .msgPanel {
            margin-top: 20px;
            margin-bottom: 25px;
            border: 1px solid #aaa;
            background-color: #f9f9f9;
            padding: 5px;
        }

        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=80);
            opacity: 0.8;
            z-index: 10000;
        }

        .auto-style1 {
            width: 278px;
        }

        .auto-style2 {
            width: 409px;
        }

        .center {
            margin: 0 auto;
            margin-left: 350px;
        }

        #ctl00_MainContainer_ucDepartment_ddlDepartment, #ctl00_MainContainer_ucProgram_ddlProgram, #ctl00_MainContainer_ddlYearNo, #ctl00_MainContainer_ddlSemesterNo, #ctl00_MainContainer_ucSession_ddlSession,
        #ctl00_MainContainer_ucAcademicSession_ddlSession, #ctl00_MainContainer_ddlHeldIn, #ctl00_MainContainer_ddlHeldIn, #ctl00_MainContainer_ddlCalenderDistribution, #ctl00_MainContainer_ddlAddCourseTrimester, #ctl00_MainContainer_ddlCourse,
        #ctl00_MainContainer_ddlTreeMasters, #ctl00_MainContainer_ddlLinkedCalendars, #ctl00_MainContainer_btnLoad {
            height: 40px !important;
            font-size: 20px;
        }
    </style>
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContainer" runat="Server">
    <div style="padding: 5px; width: 100%;">
        <div class="PageTitle">
            <label>Course List</label>
        </div>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <div class="Message-Area" style="height: auto;">
                    <asp:Label ID="Label1" runat="server" Text="Message : " Font-Bold="true"></asp:Label>
                    <asp:Label ID="lblMessage" ForeColor="Red" runat="server"></asp:Label>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div style="clear: both;"></div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>

                <div class="card">
                    <div class="card-body">

                        <div class="row">
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <b>Choose Department
                                </b>
                                <br />
                                <uc1:DepartmentUserControl runat="server" ID="ucDepartment" OnDepartmentSelectedIndexChanged="OnDepartmentSelectedIndexChanged" />

                            </div>
                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <b>Choose Program</b>
                                <br />
                                <uc1:ProgramUserControl runat="server" ID="ucProgram" OnProgramSelectedIndexChanged="ddlPrograms_SelectedIndexChanged" />

                            </div>
                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <b>Choose Course List</b>
                                <asp:DropDownList ID="ddlTreeMasters" runat="server" AutoPostBack="True" CssClass="form-control"
                                    Enabled="False" OnSelectedIndexChanged="ddlTreeMasters_SelectedIndexChanged"
                                    Width="100%">
                                </asp:DropDownList>
                            </div>
                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <b>Choose Syllabus</b>
                                <asp:DropDownList ID="ddlLinkedCalendars" runat="server" AutoPostBack="true" Width="100%" CssClass="form-control" OnSelectedIndexChanged="ddlLinkedCalendars_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <br />
                                <asp:Button ID="btnLoad" runat="server" Text="Load" OnClick="btnLoad_Click" CssClass="btn-info w-100" />
                            </div>
                        </div>

                    </div>
                </div>



            </ContentTemplate>
        </asp:UpdatePanel>

        <div id="divProgress" style="display: none; z-index: 100000000; position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%);">
            <asp:Image ID="LoadingImage" runat="server" ImageUrl="~/Images/Img/Waiting.gif" Height="300px" Width="300px" />
            <div>
                <asp:Label ID="Label2" runat="server" Text="Processing your request.........." ForeColor="Red" Font-Bold="true" Font-Italic="true" Font-Size="30px"></asp:Label>
            </div>
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
        <asp:UpdatePanel runat="server" ID="UpdatePanel04">
            <ContentTemplate>
                <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" asynrendering="true" Width="100%" SizeToReportContent="true" CssClass="center">
                </rsweb:ReportViewer>
            </ContentTemplate>
        </asp:UpdatePanel>

    </div>
</asp:Content>

