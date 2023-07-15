<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true" CodeBehind="StudentCourseSectionEnrollmentNewVersion.aspx.cs" Inherits="EMS.Module.registration.StudentCourseSectionEnrollmentNewVersion" %>





<%@ Register Src="~/UserControls/ProgramUserControl.ascx" TagPrefix="uc1" TagName="ProgramUserControl" %>
<%@ Register Src="~/UserControls/AdmissionSessionUserControl.ascx" TagPrefix="uc1" TagName="AdmissionSessionUserControl" %>
<%@ Register Src="~/UserControls/DepartmentUserControl.ascx" TagPrefix="uc1" TagName="DepartmentUserControl" %>



<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Course Registration
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
        #ctl00_MainContainer_ucAcademicSession_ddlSession, #ctl00_MainContainer_ddlHeldIn, #ctl00_MainContainer_ddlHeldIn, #ctl00_MainContainer_ddlCalenderDistribution, #ctl00_MainContainer_ddlAddCourseTrimester, #ctl00_MainContainer_ddlCourse {
            height: 40px !important;
            font-size: 20px;
        }

        span.select2-selection.select2-selection--single {
            height: 40px;
            font-size: 20px;
        }

        span.select2.select2-container.select2-container--default {
            width: 100% !important;
        }

        .sweet-alert {
            z-index: 10000000 !important;
        }

        }

        .header-center {
            text-align: center !important;
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
            <label><b style="color: black; font-size: 26px">Course Registration</b></label>
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

                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <b>Choose Program</b>
                            <br />
                            <uc1:ProgramUserControl runat="server" ID="ucProgram" OnProgramSelectedIndexChanged="ucProgram_ProgramSelectedIndexChanged" />
                        </div>
                        <div class="col-lg-5 col-md-5 col-sm-5">
                            <script type="text/javascript">
                                Sys.Application.add_load(initdropdown);
                            </script>
                            <b>Choose Semester & Held in to Load Student</b>
                            <br />
                            <asp:DropDownList ID="ddlHeldIn" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlHeldIn_SelectedIndexChanged"></asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>




            <%------------- Student Grid View --------------%>

            <div class="row" style="margin-top: 10px; font-size: 25px;">

                <div class="col-lg-3 col-md-3 col-sm-3">
                </div>
                <div class="col-lg-6 col-md-6 col-sm-6">
                    <asp:Button ID="btnCourseEnrollment" runat="server" CssClass="btn-danger w-100" Text="Click here to Register Regular courses for selected students" OnClick="btnCourseEnrollment_Click" OnClientClick="this.value = 'Updating Data....'; this.disabled = true;" UseSubmitBehavior="false" />
                </div>

                <div class="col-lg-3 col-md-3 col-sm-3">
                </div>

            </div>

            <div class="card" style="margin-top: 10px">
                <div class="card-body">
                    <asp:GridView runat="server" ID="gvStudentList" AllowSorting="True" CssClass="table-bordered"
                        AutoGenerateColumns="False" ShowFooter="True" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
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
                                        <asp:Label runat="server" ID="lblStudentRoll" Font-Bold="true" Text="Select All"></asp:Label>
                                    </div>
                                    <div style="text-align: center">

                                        <asp:CheckBox ID="chkSelectAll" runat="server" OnCheckedChanged="chkSelectAll_CheckedChanged"
                                            AutoPostBack="true" />
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div style="text-align: center">
                                        <asp:HiddenField ID="hdnStudentID" runat="server" Value='<%#Eval("StudentID") %>' />
                                        <asp:HiddenField ID="hdnYearId" runat="server" Value='<%#Eval("YearId") %>' />
                                        <asp:HiddenField ID="hdnSemesterId" runat="server" Value='<%#Eval("SemesterId") %>' />
                                        <asp:CheckBox runat="server" ID="ChkActive"></asp:CheckBox>
                                    </div>
                                </ItemTemplate>
                                <ItemStyle Width="5%" />
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <div style="text-align: center">
                                        Student Info
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div style="text-align: center">
                                        <asp:Label runat="server" ID="lblStudentRoll" Font-Bold="true" Text='<%# "Id : "+ Eval("Roll") %>'></asp:Label>
                                        <br />
                                        <asp:Label runat="server" ID="lblStudnetName" Font-Bold="true" Text='<%# "Name : "+Eval("FullName") %>'></asp:Label>
                                    </div>
                                </ItemTemplate>
                                <ItemStyle Width="10%" />
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <div style="text-align: center">
                                        Admission Session
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div style="text-align: center">
                                        <asp:Label runat="server" ID="lblAdmSession" Font-Bold="true" Text='<%#  Eval("RegistrationSession") %>'></asp:Label>
                                    </div>
                                    <%--<br />
                                    <asp:Label runat="server" ID="lblAcaSession" Font-Bold="true" Text='<%# "Aca. Session : "+ Eval("AcademicSession") %>'></asp:Label>
                                    <br />
                                    <asp:Label runat="server" ID="lblYear" Font-Bold="true" Text='<%# "Year : "+ Eval("YearName") %>'></asp:Label>
                                    <br />
                                    <asp:Label runat="server" ID="lblSemester" Font-Bold="true" Text='<%# "Semester : "+ Eval("SemesterName") %>'></asp:Label>--%>
                                </ItemTemplate>
                                <ItemStyle Width="10%" />
                            </asp:TemplateField>


                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <div style="text-align: center">
                                        Regular Course
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%--Text='<%#Eval("TotalRSSection") %>' OnClick="btnRSSection_Click" CommandArgument='<%#Eval("ProgramID")%>' Visible='<%#Eval("TotalRSSection").ToString()=="0" ? false : true %>'--%>
                                    <div style="text-align: center">
                                        <asp:LinkButton ID="lnkViewRegularCourse" runat="server" type="button" class=" btn-info btn-sm" Text="View" OnClick="lnkViewRegularCourse_Click"
                                            CommandArgument='<%#Eval("StudentID") +"_" + Eval("YearId")+"_"+Eval("SemesterId") %>'
                                            Style="text-align: center;" ForeColor="White" Font-Bold="true"> 
                                        </asp:LinkButton>
                                    </div>
                                    <div style="text-align: center">

                                        <asp:Label runat="server" ID="Label9" Font-Bold="true" Text="Regular Cr:"></asp:Label>
                                    </div>
                                    <div style="text-align: center">

                                        <asp:Label runat="server" ID="lblRegularCredit" Font-Bold="true" Text='<%#Eval("RegularCredit") %>'></asp:Label>
                                    </div>
                                </ItemTemplate>
                                <ItemStyle Width="10%" />
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <div style="text-align: center">
                                        Backlog Courses
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div style="text-align: center">
                                        <asp:LinkButton ID="lnkViewBacklogCourse" runat="server" type="button" class=" btn-info btn-sm" Text="View & Assign" OnClick="lnkViewBacklogCourse_Click"
                                            CommandArgument='<%#Eval("StudentID") +"_" + Eval("YearId")+"_"+Eval("SemesterId") %>'
                                            Style="text-align: center;" ForeColor="White" Font-Bold="true"> 
                                        </asp:LinkButton>
                                    </div>
                                    <div style="text-align: center">
                                        <asp:Label runat="server" ID="Label8" Font-Bold="true" Text="Backlog Cr:"></asp:Label>
                                    </div>
                                    <div style="text-align: center">
                                        <asp:Label runat="server" ID="lblBacklogCredit" Font-Bold="true" Text='<%#Eval("BacklogCredit") %>'></asp:Label>
                                    </div>
                                </ItemTemplate>

                                <ItemStyle Width="10%" />
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <div style="text-align: center">
                                        Special Courses
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div style="text-align: center">
                                        <asp:LinkButton ID="lnkViewSpecialCourse" runat="server" type="button" class=" btn-info btn-sm" Text="View & Assign" OnClick="lnkViewSpecialCourse_Click"
                                            CommandArgument='<%#Eval("StudentID") +"_" + Eval("YearId")+"_"+Eval("SemesterId") %>'
                                            Style="text-align: center;" ForeColor="White" Font-Bold="true"> 
                                        </asp:LinkButton>
                                    </div>
                                    <div style="text-align: center">

                                        <asp:Label runat="server" ID="lblSpecialCredit" Font-Bold="true" Text="Special Cr:"></asp:Label>
                                    </div>
                                    <div style="text-align: center">
                                        <asp:Label runat="server" ID="lblSpCredit" Font-Bold="true" Text="0.00"></asp:Label>
                                    </div>
                                </ItemTemplate>
                                <ItemStyle Width="10%" />
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <div style="text-align: center">
                                        <b>Registered Courses</b>
                                    </div>
                                </HeaderTemplate>

                                <ItemTemplate>
                                    <div style="text-align: center">
                                        <asp:LinkButton ID="lnkViewEnrolledCourse" runat="server" type="button" class=" btn-info btn-sm" Text="View & Remove" OnClick="lnkViewEnrolledCourse_Click"
                                            CommandArgument='<%#Eval("StudentID") %>'
                                            Style="text-align: center;" ForeColor="White" Font-Bold="true"> 
                                        </asp:LinkButton>
                                    </div>
                                    <div style="text-align: center">

                                        <asp:Label runat="server" ID="lblRegCredit" Font-Bold="true" Text="Registered Cr:"></asp:Label>
                                    </div>
                                    <div style="text-align: center">

                                        <asp:Label runat="server" ID="lblRegisteredCredit" Font-Bold="true" Text='<%#( Convert.ToDecimal(Eval("RegularCredit")) + Convert.ToDecimal(Eval("BacklogCredit"))) %>'></asp:Label>
                                    </div>
                                </ItemTemplate>
                                <ItemStyle Width="13%" />
                            </asp:TemplateField>

                            <%--<asp:TemplateField HeaderText="Regular Credit">
                                <ItemTemplate>
                                    <div style="text-align: center">
                                        <asp:Label runat="server" ID="lblRegularCredit" Font-Bold="true" Text='<%# Eval("RegularCredit") %>'></asp:Label>
                                    </div>
                                </ItemTemplate>
                                <ItemStyle Width="6%" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Backlog Credit">
                                <ItemTemplate>
                                    <div style="text-align: center">
                                        <asp:Label runat="server" ID="lblBacklogCredit" Font-Bold="true" Text='<%# Eval("BacklogCredit") %>'></asp:Label>
                                    </div>
                                </ItemTemplate>

                                <ItemStyle Width="6%" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Special Credit">
                                <ItemTemplate>
                                    <div style="text-align: center">
                                        <asp:Label runat="server" ID="lblSpecialCredit" Font-Bold="true" Text='<%# Eval("BacklogCredit") %>'></asp:Label>
                                    </div>
                                </ItemTemplate>

                                <ItemStyle Width="6%" />
                            </asp:TemplateField>--%>

                            <asp:TemplateField HeaderText="Registered Credit">
                                <ItemTemplate>
                                    <div style="text-align: center">
                                        <asp:Label runat="server" ID="lblTotalCredit" Font-Bold="true" Text='<%# Convert.ToDecimal(Eval("RegularCredit")) + Convert.ToDecimal(Eval("BacklogCredit")) %>'></asp:Label>
                                    </div>
                                </ItemTemplate>
                                <ItemStyle Width="4%" />
                            </asp:TemplateField>


                        </Columns>

                        <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
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



    <%-------------------------- Regular Course View Modal Popup ------------------------------%>


    <div class="col-md-15 col-lg-12">
        <asp:UpdatePanel ID="UpdatePanel9" runat="server">
            <ContentTemplate>

                <asp:Button ID="Button1" runat="server" Style="display: none" />
                <ajaxToolkit:ModalPopupExtender ID="ModalPopupCourse" runat="server" TargetControlID="Button1" PopupControlID="Panel2"
                    BackgroundCssClass="modalBackground" CancelControlID="Button2">
                </ajaxToolkit:ModalPopupExtender>

                <asp:Panel runat="server" ID="Panel2" Style="display: none; padding: 5px; overflow-y: scroll" BackColor="White"
                    Width="50%" Height="60%">

                    <div class="row">
                        <div class="col-lg-5 col-md-5 col-sm-5">
                            <asp:Label ID="lblId" runat="server" Text="" ForeColor="Blue" Font-Bold="true"></asp:Label>
                            <br />
                            <asp:Label ID="lblName" runat="server" Text="" ForeColor="Blue" Font-Bold="true"></asp:Label>
                        </div>
                        <div class="col-lg-5 col-md-5 col-sm-5" style="text-align: left">
                            <b style="font-weight: bold; color: Crimson">Regular Course List</b>
                        </div>
                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <asp:Button runat="server" ID="Button3" Text="Close" CssClass="btn-danger btn-sm" Style="display: inline-block; width: 100%; height: 38px; text-align: center; font-size: 17px;" />
                        </div>
                    </div>

                    <div class="card" style="margin-top: 5px">
                        <div class="card-body">

                            <asp:GridView runat="server" ID="gvModalCourseList" AllowSorting="True" CssClass="table-bordered" OnRowDataBound="gvModalCourseList_RowDataBound"
                                AutoGenerateColumns="False" ShowFooter="True" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                                <HeaderStyle BackColor="#4285f4" ForeColor="White" Height="10px" Font-Bold="True" />
                                <FooterStyle BackColor="#4285f4" ForeColor="White" Height="10px" Font-Bold="True" />
                                <AlternatingRowStyle BackColor="White" />
                                <RowStyle Height="10px" />

                                <Columns>

                                    <asp:TemplateField HeaderText="SL#">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblSL" Text='<%# Container.DataItemIndex + 1 %>' ForeColor="Black" Font-Bold="true"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Course Code">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblCourseCode" Font-Bold="true" Text='<%# Eval("FormalCode") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Course Title">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblCourseTitle" Font-Bold="true" Text='<%# Eval("Title") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Course Credit">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblCourseCredit" Font-Bold="true" Text='<%# Eval("Credits") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label runat="server" ID="lblCourseCredit" Font-Bold="true"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                </Columns>

                                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
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

                    <div class="row" style="margin-top: 5px">
                        <div class="col-lg-5 col-md-5 col-sm-5">
                        </div>
                        <div class="col-lg-5 col-md-5 col-sm-5">
                        </div>
                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <asp:Button runat="server" ID="Button2" Text="Close" CssClass="btn-danger btn-sm" Style="display: inline-block; width: 100%; height: 38px; text-align: center; font-size: 17px;" />
                        </div>
                    </div>


                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <%-------------------------- END Regular Course View Modal Popup ------------------------------%>

    <%-------------------------- Special Course View Modal Popup ------------------------------%>


    <div class="col-md-15 col-lg-12">
        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>

                <asp:Button ID="Button10" runat="server" Style="display: none" />
                <ajaxToolkit:ModalPopupExtender ID="ModalPopupSpecialCourse" runat="server" TargetControlID="Button10" PopupControlID="Panel4"
                    BackgroundCssClass="modalBackground" CancelControlID="Button13">
                </ajaxToolkit:ModalPopupExtender>

                <asp:Panel runat="server" ID="Panel4" Style="display: none; padding: 5px; overflow-y: scroll" BackColor="White"
                    Width="50%" Height="60%">

                    <div class="row">
                        <div class="col-lg-5 col-md-5 col-sm-5">
                            <asp:Label ID="Label6" runat="server" Text="" ForeColor="Blue" Font-Bold="true"></asp:Label>
                            <br />
                            <asp:Label ID="Label7" runat="server" Text="" ForeColor="Blue" Font-Bold="true"></asp:Label>
                        </div>
                        <div class="col-lg-5 col-md-5 col-sm-5" style="text-align: left">
                            <b style="font-weight: bold; color: Crimson">Special Course List</b>
                        </div>
                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <asp:Button runat="server" ID="Button12" Text="Close" CssClass="btn-danger btn-sm" Style="display: inline-block; width: 100%; height: 38px; text-align: center; font-size: 17px;" />
                        </div>
                    </div>

                    <div class="card" style="margin-top: 5px">
                        <div class="card-body">

                            <asp:GridView runat="server" ID="gvModalSpecialCourseList" AllowSorting="True" CssClass="table-bordered" OnRowDataBound="gvModalSpecialCourseList_RowDataBound"
                                AutoGenerateColumns="False" ShowFooter="True" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                                <HeaderStyle BackColor="#4285f4" ForeColor="White" Height="10px" Font-Bold="True" />
                                <FooterStyle BackColor="#4285f4" ForeColor="White" Height="10px" Font-Bold="True" />
                                <AlternatingRowStyle BackColor="White" />
                                <RowStyle Height="10px" />

                                <Columns>

                                    <asp:TemplateField HeaderText="SL#">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblSL" Text='<%# Container.DataItemIndex + 1 %>' ForeColor="Black" Font-Bold="true"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Course Code">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblCourseCode" Font-Bold="true" Text='<%# Eval("FormalCode") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Course Title">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblCourseTitle" Font-Bold="true" Text='<%# Eval("Title") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Course Credit">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblCourseCredit" Font-Bold="true" Text='<%# Eval("Credits") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label runat="server" ID="lblCourseCredit" Font-Bold="true"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                </Columns>

                                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
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

                    <div class="row" style="margin-top: 5px">
                        <div class="col-lg-5 col-md-5 col-sm-5">
                        </div>
                        <div class="col-lg-5 col-md-5 col-sm-5">
                        </div>
                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <asp:Button runat="server" ID="Button13" Text="Close" CssClass="btn-danger btn-sm" Style="display: inline-block; width: 100%; height: 38px; text-align: center; font-size: 17px;" />
                        </div>
                    </div>


                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <%-------------------------- END Special Course View Modal Popup ------------------------------%>



    <%-------------------------- Enrolled Course View Modal Popup ------------------------------%>

    <div class="col-md-15 col-lg-12">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>

                <asp:Button ID="Button4" runat="server" Style="display: none" />
                <ajaxToolkit:ModalPopupExtender ID="ModalPopupEnrolledCourse" runat="server" TargetControlID="Button4" PopupControlID="Panel1"
                    BackgroundCssClass="modalBackground" CancelControlID="Button6">
                </ajaxToolkit:ModalPopupExtender>

                <asp:Panel runat="server" ID="Panel1" Style="display: none; padding: 5px; overflow-y: scroll" BackColor="White"
                    Width="50%" Height="60%">

                    <div class="row">
                        <div class="col-lg-5 col-md-5 col-sm-5">
                            <asp:Label ID="Label2" runat="server" Text="" ForeColor="Crimson" Font-Bold="true"></asp:Label>
                            <br />
                            <asp:Label ID="Label3" runat="server" Text="" ForeColor="Crimson" Font-Bold="true"></asp:Label>
                        </div>
                        <div class="col-lg-5 col-md-5 col-sm-5" style="text-align: left">
                            <b style="font-weight: bold; color: blue">Registered Course List</b>
                        </div>
                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <asp:Button runat="server" ID="Button5" Text="Close" CssClass="btn-danger btn-sm" Style="display: inline-block; width: 100%; height: 38px; text-align: center; font-size: 17px;" />
                        </div>
                    </div>

                    <div class="card" style="margin-top: 5px">
                        <div class="card-body">

                            <asp:GridView runat="server" ID="gvModalEnrolledCourseList" AllowSorting="True" CssClass="table-bordered"
                                AutoGenerateColumns="False" ShowFooter="True" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                                <HeaderStyle BackColor="#4285f4" ForeColor="White" Height="10px" Font-Bold="True" />
                                <FooterStyle BackColor="#4285f4" ForeColor="White" Height="10px" Font-Bold="True" />
                                <AlternatingRowStyle BackColor="White" />
                                <RowStyle Height="10px" />

                                <Columns>

                                    <asp:TemplateField HeaderText="SL#">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblSL" Text='<%# Container.DataItemIndex + 1 %>' ForeColor="Black" Font-Bold="true"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Course Code">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblCourseCode" Font-Bold="true" Text='<%# Eval("FormalCode") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Course Title">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblCourseTitle" Font-Bold="true" Text='<%# Eval("Title") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Course Credit">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblCourseCredit" Font-Bold="true" Text='<%# Eval("CourseCredit") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label runat="server" ID="lblCourseCredit" Font-Bold="true"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Type">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblEnrolledType" Font-Bold="true" Text='<%# Eval("RegistrationType") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button ID="btnRemove" runat="server" CssClass="btn-danger w-100" Visible='<%# Eval("RegistrationType").ToString() =="Regular" ? false : true %>' Style="border-radius: 8px" Text="Remove" OnClick="btnRemove_Click" CommandArgument='<%#Eval("CourseHistoryId") %>' OnClientClick="return confirm('Are you sure you want to Remove this Entry ?');" />
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                </Columns>

                                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
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
                    <div class="row" style="margin-top: 5px">
                        <div class="col-lg-5 col-md-5 col-sm-5">
                        </div>
                        <div class="col-lg-5 col-md-5 col-sm-5">
                        </div>
                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <asp:Button runat="server" ID="Button6" Text="Close" CssClass="btn-danger btn-sm" Style="display: inline-block; width: 100%; height: 38px; text-align: center; font-size: 17px;" />
                        </div>
                    </div>


                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <%-------------------------- END Enrolled Course View Modal Popup ------------------------------%>



    <%-------------------------- Backlog Course View & Assign Modal Popup ------------------------------%>

    <div class="col-md-15 col-lg-12">
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>

                <asp:Button ID="Button7" runat="server" Style="display: none" />
                <ajaxToolkit:ModalPopupExtender ID="ModalPopupBacklogCourse" runat="server" TargetControlID="Button7" PopupControlID="Panel3"
                    BackgroundCssClass="modalBackground" CancelControlID="Button9">
                </ajaxToolkit:ModalPopupExtender>

                <asp:Panel runat="server" ID="Panel3" Style="display: none; padding: 5px; overflow-y: scroll" BackColor="White"
                    Width="70%" Height="60%">

                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:Label ID="Label4" runat="server" Text="" ForeColor="Blue" Font-Bold="true"></asp:Label>
                            <br />
                            <asp:Label ID="Label5" runat="server" Text="" ForeColor="Blue" Font-Bold="true"></asp:Label>
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4" style="text-align: left">
                            <b style="font-weight: bold; color: Crimson">Backlog Course List</b>
                        </div>
                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <asp:Button runat="server" ID="btnAssignBacklog" Text="Assign Backlog" OnClick="btnAssignBacklog_Click" OnClientClick="this.value = 'Updating Data....'; this.disabled = true;" UseSubmitBehavior="false" CssClass="btn-info btn-sm" Style="display: inline-block; width: 100%; height: 38px; text-align: center; font-size: 17px;" />
                        </div>
                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <asp:Button runat="server" ID="Button8" Text="Close" CssClass="btn-danger btn-sm" Style="display: inline-block; width: 100%; height: 38px; text-align: center; font-size: 17px;" />
                        </div>
                    </div>

                    <div class="card" style="margin-top: 5px">
                        <div class="card-body">
                            <asp:HiddenField ID="hdnbacklogStudentID" runat="server" Value='<%#Eval("StudentID") %>' />

                            <asp:GridView runat="server" ID="gvBacklogCourseList" AllowSorting="True" CssClass="table-bordered"
                                AutoGenerateColumns="False" ShowFooter="True" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                                <HeaderStyle BackColor="#4285f4" ForeColor="White" Height="10px" Font-Bold="True" />
                                <FooterStyle BackColor="#4285f4" ForeColor="White" Height="10px" Font-Bold="True" />
                                <AlternatingRowStyle BackColor="White" />
                                <RowStyle Height="10px" />

                                <Columns>

                                    <asp:TemplateField HeaderText="SL#">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblSL" Text='<%# Container.DataItemIndex + 1 %>' ForeColor="Black" Font-Bold="true"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderStyle-CssClass="header-center">
                                        <HeaderTemplate>
                                            <div style="text-align: center">

                                                <asp:CheckBox ID="chkSelectAllbacklog" runat="server" Text="Select All" OnCheckedChanged="chkSelectAllbacklog_CheckedChanged"
                                                    AutoPostBack="true" />
                                            </div>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="text-align: center">
                                                <asp:HiddenField ID="hdnbacklogCourseId" runat="server" Value='<%#Eval("CourseID") %>' />
                                                <asp:HiddenField ID="hdnbacklogYearId" runat="server" Value='<%#Eval("YearId") %>' />
                                                <asp:HiddenField ID="hdnbacklogSemesterId" runat="server" Value='<%#Eval("SemesterId") %>' />
                                                <asp:CheckBox runat="server" ID="ChkActivebacklog" AutoPostBack="true" OnCheckedChanged="ChkActivebacklog_CheckedChanged"
                                                    Checked='<%# Convert.ToInt32(Eval("Enrolled"))==1 ? true : false  %>' Enabled='<%# Convert.ToInt32(Eval("Enrolled"))==1 ? false : true  %>'></asp:CheckBox>
                                            </div>
                                        </ItemTemplate>
                                        <ItemStyle Width="5%" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Course Code">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblCourseCode" Font-Bold="true" Text='<%# Eval("FormalCode") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Course Title">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblCourseTitle" Font-Bold="true" Text='<%# Eval("Title") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Course Credit">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblCourseCredit" Font-Bold="true" Text='<%# Eval("Credits") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Registered">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblEnroll" Font-Bold="true" Text='<%# Convert.ToInt32(Eval("Enrolled"))==1 ? "Yes" : "" %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>

                                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
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

                    <div class="row" style="margin-top: 5px">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                        </div>
                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <asp:Button runat="server" ID="Button11" Text="Assign Backlog" OnClick="btnAssignBacklog_Click" OnClientClick="this.value = 'Updating Data....'; this.disabled = true;" UseSubmitBehavior="false" CssClass="btn-info btn-sm" Style="display: inline-block; width: 100%; height: 38px; text-align: center; font-size: 17px;" />
                        </div>
                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <asp:Button runat="server" ID="Button9" Text="Close" CssClass="btn-danger btn-sm" Style="display: inline-block; width: 100%; height: 38px; text-align: center; font-size: 17px;" />
                        </div>
                    </div>

                </asp:Panel>

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <%-------------------------- END Backlog Course View & Assign Modal Popup ------------------------------%>


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



