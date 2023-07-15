<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true" CodeBehind="StudentCourseSectionEnrollment.aspx.cs" Inherits="EMS.Module.registration.StudentCourseSectionEnrollment" %>


<%@ Register Src="~/UserControls/ProgramUserControl.ascx" TagPrefix="uc1" TagName="ProgramUserControl" %>
<%@ Register Src="~/UserControls/AdmissionSessionUserControl.ascx" TagPrefix="uc1" TagName="AdmissionSessionUserControl" %>
<%@ Register Src="~/UserControls/DepartmentUserControl.ascx" TagPrefix="uc1" TagName="DepartmentUserControl" %>



<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Student Course Section Enrollment
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

        #ctl00_MainContainer_ucDepartment_ddlDepartment, #ctl00_MainContainer_ucProgram_ddlProgram,
        #ctl00_MainContainer_ucAcademicSession_ddlSession, #ctl00_MainContainer_ddlHeldIn, #ctl00_MainContainer_ddlHeldIn, #ctl00_MainContainer_ddlCalenderDistribution, #ctl00_MainContainer_ddlAddCourseTrimester, #ctl00_MainContainer_ddlCourse {
            height: 35px !important;
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
            <label><b style="color: black; font-size: 26px">Student Course Section Enrollment</b></label>
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

                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <b>Department</b>
                            <br />
                            <uc1:DepartmentUserControl runat="server" ID="ucDepartment" OnDepartmentSelectedIndexChanged="ucDepartment_DepartmentSelectedIndexChanged" />
                        </div>

                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <b>Program</b>
                            <br />
                            <uc1:ProgramUserControl runat="server" ID="ucProgram" OnProgramSelectedIndexChanged="ucProgram_ProgramSelectedIndexChanged" />
                        </div>

                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <b>Academic Session</b>
                            <br />
                            <uc1:AdmissionSessionUserControl runat="server" ID="ucAcademicSession" OnSessionSelectedIndexChanged="ucAcademicSession_SessionSelectedIndexChanged" />
                        </div>

                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <b>Held In</b>
                            <br />
                            <asp:DropDownList ID="ddlHeldIn" runat="server" CssClass="form-control" AutoPostBack="true" Width="100%" OnSelectedIndexChanged="ddlHeldIn_SelectedIndexChanged"></asp:DropDownList>
                        </div>
                    </div>

                    <div class="row" style="margin-top: 10px">

                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <b>Syllabus</b>
                            <asp:DropDownList ID="ddlCalenderDistribution" runat="server" AutoPostBack="true" Width="100%" CssClass="form-control" OnSelectedIndexChanged="ddlCalenderDistribution_SelectedIndexChanged"></asp:DropDownList>

                        </div>

                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <b>Year Semester</b>
                            <asp:DropDownList ID="ddlAddCourseTrimester" runat="server" AutoPostBack="true" Width="100%" CssClass="form-control" OnSelectedIndexChanged="ddlAddCourseTrimester_SelectedIndexChanged"></asp:DropDownList>

                        </div>

                        <div class="col-lg-6 col-md-6 col-sm-6">
                            <b>Course List</b>
                            <asp:DropDownList ID="ddlCourse" runat="server" Width="100%" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlCourse_SelectedIndexChanged"></asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>


            <%------------- Course Grid View --------------%>
            <div class="card" style="margin-top: 10px">
                <div class="card-body">

                    <asp:GridView ID="gvStudentCourse" runat="server" AutoGenerateColumns="False" ShowFooter="True" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                        <HeaderStyle BackColor="#4285f4" ForeColor="White" Height="10px" Font-Bold="True" />
                        <FooterStyle BackColor="#4285f4" ForeColor="White" Height="10px" Font-Bold="True" />
                        <AlternatingRowStyle BackColor="White" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField HeaderText="Course Id" Visible="false">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblCourseId" Font-Bold="true" Text='<%#Eval("CourseID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Version Id" Visible="false">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblVersionId" Font-Bold="true" Text='<%#Eval("VersionID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Sl. No" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate><b><%# Container.DataItemIndex + 1 %></b></ItemTemplate>
                                <ItemStyle Width="5%" />
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <div style="text-align: center">
                                        <asp:CheckBox ID="chkSelectAllCourse" runat="server" Text="Select All" OnCheckedChanged="chkSelectAllCourse_CheckedChanged"
                                            AutoPostBack="true" />
                                    </div>
                                </HeaderTemplate>

                                <ItemTemplate>
                                    <asp:CheckBox ID="CheckBox" runat="server" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                                <ItemStyle Width="10%" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Course Code">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblCourseCode" Font-Bold="true" Text='<%#Eval("FormalCode") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="10%" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Course Name">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblCourseName" Font-Bold="true" Text='<%#Eval("Title") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="65%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Credits">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblCourseCredit" Font-Bold="true" Text='<%#Eval("Credits") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="10%" />
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

            <div class="row" id="divEnrollButtton" runat="server" style="margin-top: 10px">
                <div class="col-lg-2 col-md-2 col-sm-2">
                    <asp:Button ID="btnCourseEnrollment" runat="server" CssClass="btn-danger w-100" Text="Course Enrollment" OnClick="btnCourseEnrollment_Click" />
                </div>
            </div>

            <%------------- Student Grid View --------------%>
            <div class="card" style="margin-top: 10px">
                <div class="card-body">
                    <asp:GridView runat="server" ID="gvStudentList" AllowSorting="True"
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
                                <ItemStyle Width="5%" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderStyle-CssClass="header-center">
                                <HeaderTemplate>
                                    <div style="text-align: center">

                                        <asp:CheckBox ID="chkSelectAll" runat="server" Text="Select All" OnCheckedChanged="chkSelectAll_CheckedChanged"
                                            AutoPostBack="true" />
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div style="text-align: center">
                                         <asp:HiddenField ID="hdnStudentID" runat="server" Value='<%#Eval("StudentID") %>' />
                                        <%--<asp:HiddenField ID="hdnPromotionHistoryId" runat="server" Value='<%#Eval("PromotionHistoryId") %>' />--%>

                                        <asp:CheckBox runat="server" ID="ChkActive"></asp:CheckBox>
                                    </div>
                                </ItemTemplate>
                                <ItemStyle Width="10%" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Student Id">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStudentRoll" Font-Bold="true" Text='<%#Eval("Roll") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="10%" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Student Name">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStudnetName" Font-Bold="true" Text='<%#Eval("FullName") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="25%" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Registered Courses">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblRegisteredCourses" Font-Bold="true" Text='<%#Eval("EnrolledCourse") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="50%" />
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



    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
        <ContentTemplate>

            <asp:Button ID="Button1" runat="server" Style="display: none" />
            <ajaxToolkit:ModalPopupExtender ID="ModalPopupConfirm" runat="server" TargetControlID="Button1" PopupControlID="Panel2"
                BackgroundCssClass="modalBackground" CancelControlID="Button2">
            </ajaxToolkit:ModalPopupExtender>

            <asp:Panel runat="server" ID="Panel2" Style="display: none; padding: 5px;" BackColor="White" Width="40%">

                <div class="card" style="text-align:center">
                    <div class="card-body">
                        <div class="row col-lg-12 col-md-12 col-sm-12" style="color: crimson; font-weight: bold">
                            <b>Are you sure you want to enroll selected course ?</b>
                            <br />
                            <b>After confirmation you can not make any changes.</b>
                        </div>
                    </div>
                </div>
                <div class="row" style="margin-top:10px">
                    <div class="col-lg-6 col-md-6 col-sm-6">
                        <asp:Button runat="server" ID="btnConfirm" Text="Confirm" OnClick="btnConfirm_Click" CssClass="btn-info btn-sm"  />

                    </div>
                    <div class="col-lg-1 col-md-1 col-sm-1">
                    </div>
                    <div class="col-lg-5 col-md-5 col-sm-5" style="text-align:right">
                        <asp:Button runat="server" ID="Button2" Text="Close" CssClass="btn-danger btn-sm"  />
                    </div>
                </div>


            </asp:Panel>
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
