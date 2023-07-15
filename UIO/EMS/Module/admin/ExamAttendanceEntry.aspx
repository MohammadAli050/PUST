<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true" CodeBehind="ExamAttendanceEntry.aspx.cs" Inherits="EMS.Module.admin.ExamAttendanceEntry" %>



<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<%@ Register Src="~/UserControls/ProgramUserControl.ascx" TagPrefix="uc1" TagName="ProgramUserControl" %>
<%@ Register Src="~/UserControls/DepartmentUserControl.ascx" TagPrefix="uc1" TagName="DepartmentUserControl" %>
<%@ Register Src="~/UserControls/AdmissionSessionUserControl.ascx" TagPrefix="uc1" TagName="AdmissionSessionUserControl" %>




<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="Server">
Exam Attendance Entry & Top Sheet Print
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="Server">

    <script src="https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.3.min.js"></script>
    <link href="../../CSS/select2.min.css" rel="stylesheet" />
    <script src="../../JavaScript/select2.full.min.js"></script>

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

        function jsShowHideProgress() {
            setTimeout(function () {
                document.getElementById('divProgress').style.display = 'block';
            }, 200);
            deleteCookie();

            var timeInterval = 500; // milliseconds (checks the cookie for every half second )

            var loop = setInterval(function () {
                if (IsCookieValid()) {
                    document.getElementById('divProgress').style.display =
                    'none'; clearInterval(loop)
                }

            }, timeInterval);
        }
        // cookies
        function deleteCookie() {
            var cook = getCookie('ExcelDownloadFlag');
            if (cook != "") {
                document.cookie = "ExcelDownloadFlag=;Path=/; expires=Thu, 01 Jan 1970 00:00:00 UTC";
            }
        }

        function IsCookieValid() {
            var cook = getCookie('ExcelDownloadFlag');
            return cook != '';
        }

        function getCookie(cname) {
            var name = cname + "=";
            var ca = document.cookie.split(';');
            for (var i = 0; i < ca.length; i++) {
                var c = ca[i];
                while (c.charAt(0) == ' ') {
                    c = c.substring(1);
                }
                if (c.indexOf(name) == 0) {
                    return c.substring(name.length, c.length);
                }
            }
            return "";
        }

    </script>


    <style type="text/css">
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

        #ctl00_MainContainer_btnTopSheet,#ctl00_MainContainer_ucDepartment_ddlDepartment, #ctl00_MainContainer_ucProgram_ddlProgram,
        #ctl00_MainContainer_ddlCourse, #ctl00_MainContainer_ddlContinousExam, #ctl00_MainContainer_btnLoad, #ctl00_MainContainer_btnLoadReport {
            height: 40px !important;
            font-size : 20px;
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

    
    <style>
        th {
            text-align: center;
        }

        #ctl00_MainContainer_gvStudentlists {
            width: 81%;
            margin-left: 106px;
        }

        @media (max-width: 576px) {
            .search-btn {
                color: #e84118;
                float: right;
                width: 40px;
                height: 40px;
                border-radius: 50%;
                background: #2f3640;
                display: flex;
                justfy-content: center;
                align-item: center;
            }

            .search-txt {
                border: none;
                background: none;
                outline: none;
                float: left;
                padding: 0;
                color: white;
                font-size: 16px;
                transition: 0.4s;
                line-height: 40px;
                width: 0px;
            }

            .search-box:hover > .search-txt {
                width: 240px;
                padding: 0 6px;
            }

            #ctl00_MainContainer_gvStudentlists {
                width: 100%;
                margin-left: 0px;
            }

            select, textarea, input[type="text"], input[type="password"], input[type="datetime"], input[type="datetime-local"], input[type="date"], input[type="month"], input[type="time"], input[type="week"], input[type="number"], input[type="email"], input[type="url"], input[type="search"], input[type="tel"], input[type="color"], .uneditable-input {
                display: inline-block;
                padding: 4px 6px;
                margin-bottom: 9px;
                font-size: 14px;
                line-height: 20px;
                color: #555555;
                -webkit-border-radius: 3px;
                -moz-border-radius: 3px;
                border-radius: 6px;
                width: 63px;
            }

            label, input, button, select, textarea {
                font-size: 9px;
                font-weight: normal;
                line-height: 20px;
            }

            table {
                max-width: 100%;
                /* background-color: transparent; */
                border-collapse: collapse;
                border-spacing: 0;
                font-size: 9px;
            }

            #ctl00_MainContainer_gvStudentlists {
                /*width:50%;*/
            }

            .stgrid {
                margin-left: -7pt;
                overflow: scroll;
            }
        }

        .ClsAE {
            font-family: arial;
            font-size: 15pt;
            padding: 18pt;
        }

        .ClsAEr {
            margin-top: 46pt;
            background-color: #fff;
            border-radius: 5pt;
            margin-bottom: 10pt;
        }

        .statuswidth {
            width: auto;
        }

        @media (max-width: 576px) {
            .statuswidth {
                width: 100px;
            }

            .checkbox-btn .slide:after {
                content: 'A';
                position: absolute;
                top: 0;
                right: -30px !important;
                text-align: center;
                width: 60px;
                height: 100%;
                line-height: 25px;
                background: #ff002d;
                font-weight: bold;
                color: #fff;
            }

            .checkbox-btn {
                position: absolute !important;
                transform: translate(-50%, -50%) !important;
                width: 46px !important;
                height: 26px !important;
                margin-top: 10pt !important;
                margin-left: 9pt !important;
            }

            .ClsAE {
                font-family: arial;
                font-size: 15pt;
                padding: 18pt;
            }

            .ClsAEr {
                margin-top: 46pt;
                background-color: #fff;
                border-radius: 5pt;
                margin-bottom: 10pt;
                width: 482px;
            }
        }

        .checkbox-btn input {
            position: absolute;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            display: block;
            cursor: pointer;
            opacity: 0;
            z-index: 1;
        }

        .checkbox-btn div {
            position: absolute;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            /*border: 2px solid #000;*/
            border-radius: 4px;
            box-shadow: 0 10px 20px rgba(0,0,0,0.5);
            box-sizing: border-box;
            overflow: hidden;
        }

            .checkbox-btn div .slide {
                position: absolute;
                top: 0;
                left: 0;
                width: 30px;
                height: 40px;
                background: #000;
                transition: 0.5s;
            }

        .checkbox-btn input:checked + div .slide {
            transform: translateX(60px);
        }

        .checkbox-btn .slide:before {
            content: 'P';
            position: absolute;
            top: 0;
            left: -60px;
            text-align: center;
            width: 60px;
            height: 100%;
            line-height: 25px;
            background: #20c997;
            font-weight: bold;
            color: #fff;
        }

        .checkbox-btn .slide:after {
            content: 'A';
            position: absolute;
            top: 0;
            right: -40px;
            text-align: center;
            width: 60px;
            height: 100%;
            line-height: 25px;
            background: #dc3545;
            font-weight: bold;
            color: #fff;
        }

        .checkbox-btn {
            position: absolute;
            transform: translate(-50%, -50%);
            width: 59px;
            height: 26px;
            margin-top: 10pt;
            margin-left: 26pt;
        }

        .Comment_expand {
        }

            .Comment_expand::after {
            }

            .Comment_expand:before {
            }



        .switch {
            position: relative;
            display: inline-block;
            width: 60px;
            height: 25px;
            bottom: 10px;
        }

            /* Hide default HTML checkbox */
            .switch input {
                opacity: 0;
                width: 0;
                height: 0;
            }
        /* The slider */
        .slider {
            position: absolute;
            cursor: pointer;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background-color: #ccc;
            -webkit-transition: .4s;
            transition: .4s;
        }

            .slider:before {
                content: "Not";
                position: absolute;
                height: 20px;
                width: 30px;
                left: 1px;
                bottom: 3px;
                background-color: white;
                -webkit-transition: .4s;
                transition: .4s;
            }
            /* Rounded sliders */
            .slider.round {
                border-radius: 25px;
            }

                .slider.round:before {
                    border-radius: 100%;
                }

        input:checked + .slider {
            background-color: #2196F3;
        }

        input:focus + .slider {
            box-shadow: 0 0 1px #2196F3;
        }

        input:checked + .slider:before {
            -webkit-transform: translateX(26px);
            -ms-transform: translateX(26px);
            transform: translateX(26px);
        }

        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=80);
            opacity: 0.8;
            z-index: 10000;
        }

        .blink {
            animation: blinker 0.6s linear infinite;
            color: #1c87c9;
            font-size: 15px;
            font-weight: bold;
            font-family: sans-serif;
        }

        @keyframes blinker {
            50% {
                opacity: 0;
            }
        }

        .header-center {
            text-align: center;
        }
    </style>
</asp:Content>



<asp:Content ID="Content3" ContentPlaceHolderID="MainContainer" runat="server">

    
    <div class="row">
        <div class="col-sm-12" style="font-size: 12pt; margin-top: 10pt;">
            <label><b style="color: black; font-size: 26px">Exam Attendance Entry & Top Sheet Print</b></label>
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
            <div class="card">
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
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <script type="text/javascript">
                                Sys.Application.add_load(initdropdown);
                            </script>
                            <b>Choose Semester & Held in to Load Courses</b>
                            <br />
                            <asp:DropDownList ID="ddlHeldIn" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlHeldIn_SelectedIndexChanged"></asp:DropDownList>
                        </div>
                    </div>

                    <div class="row" style="margin-top: 10px">
                        <div class="col-lg-5 col-md-5 col-sm-5">
                            <b>Choose Course</b>
                            <asp:DropDownList ID="ddlCourse" AutoPostBack="true" CssClass="form-control" Width="100%" OnSelectedIndexChanged="ddlCourse_SelectedIndexChanged" runat="server"></asp:DropDownList>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <br />
                            <asp:Button ID="btnLoad" runat="server" Text="Click Here To Load Student" CssClass="btn-info w-100" OnClick="btnLoad_Click" />
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <br />
                            <asp:Button ID="btnTopSheet" runat="server" Text="Click Here To Print Top Sheet" CssClass="btn-info w-100" OnClick="btnTopSheet_Click" />
                        </div>
                    </div>
                </div>
            </div>


            <div class="card" style="margin-top:10px">
                <div class="card-body">
                    <asp:GridView runat="server" ID="gvStudentList" AllowSorting="True" CssClass="table table-bordered table-responsive"
                            AutoGenerateColumns="False" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None" Font-Size="12px"
                            AllowPaging="false" ShowFooter="true">
                            <HeaderStyle BackColor="#4285f4" ForeColor="White" Height="10px" Font-Bold="True" />
                            <FooterStyle BackColor="#4285f4" ForeColor="White" Height="10px" Font-Bold="True" />
                            <AlternatingRowStyle BackColor="White" />
                            <RowStyle Height="10px" />

                            <Columns>

                                <asp:TemplateField HeaderText="SL#">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblSL" Text='<%# Container.DataItemIndex + 1 %>' ForeColor="Black" Font-Bold="true"></asp:Label>

                                    </ItemTemplate>
                                    <ItemStyle Width="1%" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Student ID" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblRoll" Text='<%#Eval("Roll") %>' ForeColor="Black" Font-Bold="true"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle />
                                    </asp:TemplateField>

                                 <asp:TemplateField HeaderText="Student Name">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblName" Text='<%#Eval("FullName") %>' ForeColor="Black" Font-Bold="true"></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle />
                                    </asp:TemplateField>

                                <asp:TemplateField HeaderStyle-CssClass="header-center">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkAttendanceSave" runat="server" Width="100%" CssClass="btn-warning form-control" OnClick="lnkAttendanceSave_Click" OnClientClick="jsShowHideProgress();">
                                                                    <b>Save</b>
                                            </asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div class=" checkbox-btn" id="status" style="text-align: center; margin-top: 0px; margin-left: 60px">
                                                <asp:Label runat="server" ID="lblSetupId" Text='<%#Eval("SetupId") %>' Visible="false"></asp:Label>
                                                <asp:Label runat="server" ID="lblHistoryId" Text='<%#Eval("HistoryId") %>' Visible="false"></asp:Label>
                                                
                                                <asp:CheckBox ID="chkStatus" runat="server" Checked='<%# Convert.ToInt32(Eval("PresentAbsentStatus"))==1 ? true : false %>' />
                                                <div><span class="slide"></span></div>
                                            </div>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:LinkButton ID="lnkAttendanceSave2" runat="server" Width="100%" Style="text-align: center" CssClass="btn-warning form-control" OnClick="lnkAttendanceSave_Click" OnClientClick="jsShowHideProgress();">
                                                                    <b>Save</b>  </asp:LinkButton>
                                        </FooterTemplate>
                                        <HeaderStyle />
                                    <ItemStyle Width="10%" />
                                    </asp:TemplateField>

                            </Columns>

                            <PagerStyle BackColor="#4285f4" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle Height="10px" VerticalAlign="Middle" HorizontalAlign="Left" BackColor="#E3EAEB" />
                            <EditRowStyle BackColor="#7C6F57" />
                            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                            <SortedAscendingCellStyle BackColor="#F8FAFA" />
                            <SortedAscendingHeaderStyle BackColor="#246B61" />
                            <SortedDescendingCellStyle BackColor="#D4DFE1" />
                            <SortedDescendingHeaderStyle BackColor="#15524A" />
                        </asp:GridView>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
            <div>
                <rsweb:ReportViewer ID="ReportViewer1" Visible="True" runat="server" Font-Names="Verdana" Font-Size="8pt" Height="30%" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" asynrendering="false" SizeToReportContent="true" BackColor="Wheat" CssClass="center" BorderColor="WhiteSmoke" BorderStyle="Solid" BorderWidth="1">
                </rsweb:ReportViewer>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

      <ajaxToolkit:UpdatePanelAnimationExtender
        ID="UpdatePanelAnimationExtender1"
        TargetControlID="UpdatePanel02"
        runat="server">
        <Animations>
            <OnUpdating>
               <Parallel duration="0">
                    <ScriptAction Script="InProgress();" />
                    <EnableAction AnimationTarget="btnLoad" 
                                  Enabled="false" /> 
                   <EnableAction AnimationTarget="btnTopSheet" 
                                  Enabled="false" />                   
                </Parallel>
            </OnUpdating>
            <OnUpdated>
                <Parallel duration="0">
                    <ScriptAction Script="onComplete();" />
                    <EnableAction   AnimationTarget="btnLoad" 
                                    Enabled="true" />
                    <EnableAction   AnimationTarget="btnTopSheet" 
                                    Enabled="true" />
                </Parallel>
            </OnUpdated>
        </Animations>
    </ajaxToolkit:UpdatePanelAnimationExtender>

</asp:Content>
