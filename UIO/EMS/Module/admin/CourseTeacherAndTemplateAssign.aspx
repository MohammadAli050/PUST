<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true" CodeBehind="CourseTeacherAndTemplateAssign.aspx.cs" Inherits="EMS.Module.admin.CourseTeacherAndTemplateAssign" %>



<%@ Register Src="~/UserControls/ProgramUserControl.ascx" TagPrefix="uc1" TagName="ProgramUserControl" %>
<%@ Register Src="~/UserControls/DepartmentUserControl.ascx" TagPrefix="uc1" TagName="DepartmentUserControl" %>




<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
  Assign Course Teacher and Mark Distribution View
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">

    <script src="https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.3.min.js"></script>
    <link href="../../CSS/select2.min.css" rel="stylesheet" />
    <script src="../../JavaScript/select2.full.min.js"></script>

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

       #ctl00_MainContainer_ucDepartment_ddlDepartment, #ctl00_MainContainer_ucProgram_ddlProgram,
        #ctl00_MainContainer_ddlDesignation, #ctl00_MainContainer_DepartmentUserControl1_ddlDepartment, #ctl00_MainContainer_btnUpdateTemplate, #ctl00_MainContainer_btnUpdateTeacher, #ctl00_MainContainer_btnLoad, #ctl00_MainContainer_ddlCourseType, #ctl00_MainContainer_ddlHeldIn, #ctl00_MainContainer_txtCourse, #ctl00_MainContainer_ddlTeacher, #ctl00_MainContainer_ddlTemplate {
            height: 40px !important;
            font-size: 20px;
        }

        span.select2-selection.select2-selection--single {
            height: 35px;
        }

        span.select2.select2-container.select2-container--default {
            width: 100% !important;
        }

        .sweet-alert {
            z-index: 10000000 !important;
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
                allowClear: true
                //,
                //placeholder: { id: '0', text: 'Select' }
            });

            $('#ctl00_MainContainer_ddlTeacher').select2({
                allowClear: true
            });

            $('#ctl00_MainContainer_ddlCourseTeacher').select2({
                allowClear: true
            });

            $('#ctl00_MainContainer_ddlCourseTemplate').select2({
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

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContainer" runat="server">

    <div class="row">
        <div class="col-sm-12" style="font-size: 12pt; margin-top: 10pt;">
            <label><b style="color: black; font-size: 26px">Assign Course Teacher and Mark Distribution View</b></label>
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
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <b>Choose Department</b>
                            <br />
                            <uc1:DepartmentUserControl runat="server" ID="ucDepartment" OnDepartmentSelectedIndexChanged="ucDepartment_DepartmentSelectedIndexChanged" />
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <b>Choose Program <span style="color:red">*</span></b>
                            <br />
                            <uc1:ProgramUserControl runat="server" ID="ucProgram" OnProgramSelectedIndexChanged="ucProgram_ProgramSelectedIndexChanged" />
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <script type="text/javascript">
                                Sys.Application.add_load(initdropdown);
                            </script>
                            <b>Choose Semester & Held in to Load Courses<span style="color:red;">*</span></b>
                            <br />
                            <asp:DropDownList ID="ddlHeldIn" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlHeldIn_SelectedIndexChanged"></asp:DropDownList>
                        </div>
                    </div>

                    <div class="row" style="margin-top: 10px">
                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <b>Choose Course Type</b>
                            <asp:DropDownList ID="ddlCourseType" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlCourseType_SelectedIndexChanged"></asp:DropDownList>
                        </div>

                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <b>Choose Course</b>
                            <asp:TextBox ID="txtCourse" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>

                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <b>Choose Course Teacher</b>
                            <asp:DropDownList ID="ddlTeacher" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlTeacher_SelectedIndexChanged"></asp:DropDownList>

                        </div>

                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <b>Choose Mark Distribution</b>
                            <asp:DropDownList ID="ddlTemplate" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlTemplate_SelectedIndexChanged"></asp:DropDownList>

                        </div>
                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <br />
                            <asp:Button ID="btnLoad" runat="server" CssClass="btn-info w-100" Text="Load" OnClick="btnLoad_Click" OnClientClick="this.value = 'Loading Data....'; this.disabled = true;" UseSubmitBehavior="false" />
                        </div>
                    </div>

                </div>
            </div>

            <div class="card" style="margin-top: 10px" id="DivUpdate" runat="server">
                <div class="card-body">

                    <div class="row">
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <b>Choose Department</b>
                            <br />
                            <uc1:DepartmentUserControl runat="server" ID="DepartmentUserControl1" OnDepartmentSelectedIndexChanged="DepartmentUserControl1_DepartmentSelectedIndexChanged" />
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <b>Choose Designation</b>
                            <asp:DropDownList ID="ddlDesignation" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlDesignation_SelectedIndexChanged">
                                <asp:ListItem Text="All" Value="All"></asp:ListItem>
                                <asp:ListItem Text="Vice-Chancellor" Value="Vice-Chancellor"></asp:ListItem>
                                <asp:ListItem Text="Chairman" Value="Chairman"></asp:ListItem>
                                <asp:ListItem Text="Professor" Value="Professor"></asp:ListItem>
                                <asp:ListItem Text="Associate Professor" Value="Associate Professor"></asp:ListItem>
                                <asp:ListItem Text="Assistant Professor" Value="Assistant Professor"></asp:ListItem>
                                <asp:ListItem Text="Lecturer" Value="Lecturer"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <b>Choose Course Teacher</b>
                            <asp:DropDownList ID="ddlCourseTeacher" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>

                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <br />
                            <asp:Button ID="btnUpdateTeacher" runat="server" CssClass="btn-info w-100" Text="Update Teacher" OnClick="btnUpdateTeacher_Click" OnClientClick="this.value = 'Updating Data....'; this.disabled = true;" UseSubmitBehavior="false" />
                        </div>
                    </div>
                    <div class="row" style="margin-top: 10px">
                        <div class="col-lg-6 col-md-6 col-sm-6">
                            <b>Choose Mark Distribution</b>
                            <asp:DropDownList ID="ddlCourseTemplate" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>

                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <br />
                            <asp:Button ID="btnUpdateTemplate" runat="server" CssClass="btn-info w-100" Text="Update Mark Distribution" OnClick="btnUpdateTemplate_Click" OnClientClick="this.value = 'Updating Data....'; this.disabled = true;" UseSubmitBehavior="false" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="card" style="margin-top: 10px">
                <div class="card-body">
                    <asp:GridView runat="server" ID="gvScheduleList" AllowSorting="True" CssClass="table-bordered" OnSorting="gvScheduleList_Sorting"
                        AutoGenerateColumns="False" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None"
                        AllowPaging="true" PageSize="15" OnPageIndexChanging="gvScheduleList_PageIndexChanging" PagerStyle-HorizontalAlign="Right">
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

                            <asp:TemplateField HeaderStyle-CssClass="header-center">
                                <HeaderTemplate>
                                    <div style="text-align: center">
                                        <asp:Label runat="server" ID="ckhSelect" Text="Select All" Font-Bold="true"></asp:Label>
                                    </div>
                                    <div style="text-align: center">
                                        <asp:CheckBox ID="chkSelectAll" runat="server" OnCheckedChanged="chkSelectAll_CheckedChanged"
                                            AutoPostBack="true" />
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div style="text-align: center">
                                        <asp:HiddenField ID="hdnAcaCalSectionID" runat="server" Value='<%#Eval("AcaCal_SectionID") %>' />
                                        <asp:CheckBox runat="server" ID="ChkActive"></asp:CheckBox>
                                    </div>
                                </ItemTemplate>
                                <ItemStyle Width="5%" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Course Info">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblFormalCode" Font-Bold="true" Text='<%# "Course Code : "+ Eval("FormalCode") +"    , Credit : "+Eval("Credits") %>'></asp:Label>
                                    <br />
                                    <asp:Label runat="server" ID="lblTitle" Font-Bold="true" Text='<%# "Title : "+Eval("Title") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="20%" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Section Info">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblSection" Font-Bold="true" Text='<%# "Section : "+ Eval("SectionName") %>'></asp:Label>
                                    <br />
                                    <asp:Label runat="server" ID="lblCredit" Font-Bold="true" Text='<%# "Registered Student : "+Eval("TotalStudent") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="7%" />
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkInfo" runat="server" ForeColor="White" CommandName="Sort" CommandArgument="EmployeeCode">
                                                    <strong>Teacher Info&nbsp;<i class="fas fa-sort"></i></strong>
                                    </asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <p>
                                        <asp:HiddenField ID="hdnTeacherId" runat="server" Value='<%# Eval("EmployeeId") %>' />
                                        <asp:Label runat="server" ID="lblCode" Font-Bold="true" Text='<%# "Code : "+ Eval("EmployeeCode") %>'></asp:Label>
                                        <br />
                                        <asp:Label runat="server" ID="lblTeacheName" Font-Bold="true" Text='<%# "Name : "+Eval("EmployeeName") %>'></asp:Label>
                                    </p>
                                </ItemTemplate>
                                <HeaderStyle />
                                <ItemStyle HorizontalAlign="Left" Width="15%" />
                            </asp:TemplateField>


                            <asp:TemplateField>

                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkTemplate" runat="server" ForeColor="White" CommandName="Sort" CommandArgument="TemplateName">
                                                    <strong>Mark Distribution&nbsp;<i class="fas fa-sort"></i></strong>
                                    </asp:LinkButton>
                                </HeaderTemplate>

                                <ItemTemplate>
                                    <asp:HiddenField ID="hdnTemplateId" runat="server" Value='<%# Eval("TemplateId") %>' />
                                    <asp:Label runat="server" ID="lblTemplate" Font-Bold="true" Text='<%# "Mark Distribution : "+ Eval("TemplateName") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="15%" />
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
