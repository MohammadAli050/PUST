<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true" CodeBehind="ExamMarkDateRangeSetup.aspx.cs" Inherits="EMS.Module.admin.ExamMarkDateRangeSetup" %>



<%@ Register Src="~/UserControls/ProgramUserControl.ascx" TagPrefix="uc1" TagName="ProgramUserControl" %>
<%@ Register Src="~/UserControls/DepartmentUserControl.ascx" TagPrefix="uc1" TagName="DepartmentUserControl" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Mark Entry Date Range Setup
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">


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

        #ctl00_MainContainer_ddlHeldIn, #ctl00_MainContainer_ucDepartment_ddlDepartment, #ctl00_MainContainer_ucProgram_ddlProgram,
        #ctl00_MainContainer_btnUpdateInfo {
            height: 40px !important;
            font-size: 20px;
        }

        span.select2-selection.select2-selection--single, #ctl00_MainContainer_ddlInternalQSetterName, #ctl00_MainContainer_ddlThirdExaminerName, #ctl00_MainContainer_ddlExternalQSetterName, #ctl00_MainContainer_ddlInternalScriptExName, #ctl00_MainContainer_ddlExternalScriptExName {
            height: 40px;
        }

        span.select2.select2-container.select2-container--default {
            width: 100% !important;
        }

        .sweet-alert {
            z-index: 10000000 !important;
        }


        input {
            height: 40px !important;
        }

        #ctl00_MainContainer_chkActive {
            height: 35px !important;
            width: 35px;
        }

        .header-center {
            text-align: center;
        }

        .table > thead > tr > th, .table > tbody > tr > th, .table > tfoot > tr > th, .table > thead > tr > td, .table > tbody > tr > td, .table > tfoot > tr > td {
            vertical-align: middle !important;
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

        function initdropdown() {
            $('#ctl00_MainContainer_ddlHeldIn').select2({
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

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContainer" runat="server">



    <asp:UpdatePanel runat="server" ID="UpdatePanel1">
        <ContentTemplate>
            <div class="row">
                <div class="col-lg-8 col-md-8 col-sm-8" style="font-size: 12pt; margin-top: 10pt;">
                    <label><b style="color: black; font-size: 26px">Mark Entry Date Range Setup</b></label>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
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
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <b>Choose Department</b>
                            <br />
                            <uc1:DepartmentUserControl runat="server" ID="ucDepartment" OnDepartmentSelectedIndexChanged="ucDepartment_DepartmentSelectedIndexChanged" />
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <b>Choose Program <span style="color: red">*</span></b>
                            <br />
                            <uc1:ProgramUserControl runat="server" ID="ucProgram" OnProgramSelectedIndexChanged="ucProgram_ProgramSelectedIndexChanged" />
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <script type="text/javascript">
                                Sys.Application.add_load(initdropdown);
                            </script>
                            <b>Choose Semester & Held in to Load Courses<span style="color: red;">*</span></b>
                            <br />
                            <asp:DropDownList ID="ddlHeldIn" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlHeldIn_SelectedIndexChanged"></asp:DropDownList>
                        </div>
                    </div>

                </div>
            </div>




            <div class="card" style="margin-top: 10px">
                <div class="card-body">
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <%--<asp:Button ID="btnUpdateInfo" runat="server" CssClass="btn-info w-100" Text="Assign Question Setter And Script Examiner" OnClick="btnUpdateInfo_Click" OnClientClick="this.value = 'Please wait....'; this.disabled = true;" UseSubmitBehavior="false" />--%>
                        </div>
                    </div>
                    <div style="margin-top: 10px">
                        <asp:GridView runat="server" ID="gvScheduleList" AllowSorting="True" CssClass="table table-bordered table-responsive"
                            AutoGenerateColumns="False" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None" Font-Size="12px" ShowHeader="true" ShowFooter="true"
                            AllowPaging="false">
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

                                <asp:TemplateField HeaderText="Course Info">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblFormalCode" Font-Bold="true" Text='<%# "Course Code : "+ Eval("FormalCode") +"     , Credit : "+Eval("Credits") +"    , Section : "+Eval("SectionName")  %>'></asp:Label>
                                        <br />
                                        <asp:Label runat="server" ID="lblTitle" Font-Bold="true" Text='<%# "Title : "+Eval("Title") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="40%" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Exam Date" HeaderStyle-CssClass="header-center">
                                    <ItemTemplate>
                                        <div>
                                            <asp:TextBox ID="txtExamDate" runat="server" Text='<%# Eval("ExamDate") != null ? Eval("ExamDate", "{0: dd/MM/yyyy}") : "" %>'></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtExamDate" Format="dd/MM/yyyy" />
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="CA Start Date" HeaderStyle-CssClass="header-center">
                                    <ItemTemplate>
                                        <div>
                                            <asp:TextBox ID="txtCAStartDate" runat="server" Text='<%# Eval("CAStartDate") != null ? Eval("CAStartDate", "{0: dd/MM/yyyy}") : "" %>'></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtCAStartDate" Format="dd/MM/yyyy" />
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="CA End Date" HeaderStyle-CssClass="header-center">
                                    <ItemTemplate>
                                        <div>
                                            <asp:TextBox ID="txtCAEndDate" runat="server" Text='<%# Eval("CAEndDate") != null ? Eval("CAEndDate", "{0: dd/MM/yyyy}") : "" %>'></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtCAEndDate" Format="dd/MM/yyyy" />
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Final Start Date" HeaderStyle-CssClass="header-center">
                                    <ItemTemplate>
                                        <div>
                                            <asp:TextBox ID="txtFinalStartDate" runat="server" Text='<%# Eval("FinalStartDate") != null ? Eval("FinalStartDate", "{0: dd/MM/yyyy}") : "" %>'></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtFinalStartDate" Format="dd/MM/yyyy" />
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Final End Date" HeaderStyle-CssClass="header-center">
                                    <ItemTemplate>
                                        <div>
                                            <asp:TextBox ID="txtFinalEndDate" runat="server" Text='<%# Eval("FinalEndDate") != null ? Eval("FinalEndDate", "{0: dd/MM/yyyy}") : "" %>'></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender5" runat="server" TargetControlID="txtFinalEndDate" Format="dd/MM/yyyy" />
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                     <HeaderTemplate>
                                        <div style="text-align: center">
                                            <asp:Button ID="btnSaveAll" runat="server" Text="Update All" CssClass="btn-danger btn-sm w-100 form-control" OnClick="btnSaveAll_Click" OnClientClick="return confirm('Do you really want save all date ?');" />
                                        </div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div style="padding: 5px">
                                            <asp:LinkButton ID="Edit" ToolTip="Edit" CssClass="btn-primary btn-sm w-100" CommandArgument='<%#Eval("AcaCal_SectionID")%>' runat="server" OnClick="Edit_Click" OnClientClick="this.style.display = 'none'">
                                                                             <strong><i class="fas fa-pencil-alt"></i>&nbsp;Update</strong>
                                            </asp:LinkButton>
                                            <asp:Label runat="server" Visible="false" ID="lblSection" Font-Bold="true" Text='<%#Eval("AcaCal_SectionID") %>' />

                                        </div>
                                    </ItemTemplate>
                                     <FooterTemplate>
                                        <div style="text-align: center">
                                            <asp:Button ID="btnSaveAll2" runat="server" Text="Update All" CssClass="btn-danger btn-sm w-100 form-control" OnClick="btnSaveAll_Click" OnClientClick="return confirm('Do you really want save all date ?');" />
                                        </div>
                                    </FooterTemplate>
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
            </div>


        </ContentTemplate>
    </asp:UpdatePanel>




    <ajaxToolkit:UpdatePanelAnimationExtender
        ID="UpdatePanelAnimationExtender1" TargetControlID="UpdatePanel02" runat="server">
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


</asp:Content>
