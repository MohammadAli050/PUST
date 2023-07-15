<%@ Page Title="Student Course Enrollment" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true" 
    CodeBehind="StudentCourseSectionAssign.aspx.cs" Inherits="EMS.Module.registration.StudentCourseSectionAssign" %>

<%@ Register Src="~/UserControls/SessionUserControl.ascx" TagPrefix="uc1" TagName="SessionUserControl" %>
<%@ Register Src="~/UserControls/ProgramUserControl.ascx" TagPrefix="uc1" TagName="ProgramUserControl" %>
<%@ Register Src="~/UserControls/DepartmentUserControl.ascx" TagPrefix="uc1" TagName="DepartmentUserControl" %>
<%@ Register Src="~/UserControls/AdmissionSessionUserControl.ascx" TagPrefix="uc1" TagName="AdmissionSessionUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Student Course Enrollment
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <style type="text/css">
        .auto-style2 {
            width: 170px;
        }

        .btnNew {
        border: none;
        color: white;
        font-weight: bold;
        cursor: pointer;
        }

        .success {
            background-color: #9acd32;
        }

        .success:hover {
            background-color: #8ab92d;
        }

        .info {
            background-color: #2196F3;
        }

        .info:hover {
            background: #0b7dda;
        }

        .auto-style3 {
            width: 273px;
        }

        .lblCss {
            width: 170px;
        }

        .auto-style4 {
            width: 668px;
        }

        .auto-style5 {
            width: 180px;
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
<asp:Content ID="Content3" ContentPlaceHolderID="MainContainer" runat="server">

    <div class="PageTitle">
        <label>Student Course Enrollment</label>
    </div>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlMessage" runat="server" Visible="true" CssClass="msgPanel">
                <div class="Message-Area">
                    <asp:Label ID="Label1" runat="server" Text="Message : " Font-Bold="true"></asp:Label>
                    <asp:Label ID="lblMsg" runat="server"></asp:Label>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div id="divProgress" style="display: none; z-index: 1000; position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%);">
        <asp:Image ID="LoadingImage" runat="server" ImageUrl="~/Images/Spinner.gif" Height="150px" Width="150px" />
    </div>

    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <div class="Message-Area">
                <div style="padding: 5px; margin: 5px; width: 75%;">
                    <asp:Panel runat="server" ID="pnlMStudent">
                        <table style="padding: 5px; width: 95%;">
                            <tr>
                                <td>
                                    <table>
                                        <tr>
                                            <td class="auto-style5">Department</td>
                                            <td class="auto-style5">
                                                 <uc1:DepartmentUserControl runat="server" ID="ucDepartment"  OnDepartmentSelectedIndexChanged="OnDepartmentSelectedIndexChanged" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style5">Program</td>
                                            <td class="auto-style5">
                                                <uc1:ProgramUserControl runat="server" ID="ucProgram" OnProgramSelectedIndexChanged="OnProgramSelectedIndexChanged" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style5"><b>Student Session </b></td>
                                            <td class="auto-style5">
                                                 <uc1:AdmissionSessionUserControl runat="server" ID="ucCurrentSession" OnSessionSelectedIndexChanged="ucSession_SessionSelectedIndexChanged"/>
                                            </td>
                                        </tr>
                                        <%--<tr>
                                            <td class="auto-style5">University Session</td>
                                            <td class="auto-style5">
                                                <uc1:AdmissionSessionUserControl runat="server" ID="ucVersitySession" />
                                            </td>
                                        </tr>--%>
                                        
                                        <%--<tr>
                                            <td class="auto-style5">Year</td>
                                            <td class="auto-style5">
                                                <asp:DropDownList ID="ddlYear" Width="180px" AutoPostBack="true" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged" runat="server"></asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style5">
                                                <asp:Label ID="lblTerm" runat="server">Semester</asp:Label>
                                            </td>
                                            <td class="auto-style5">
                                                <asp:DropDownList ID="ddlSemester" Width="180px" runat="server" AutoPostBack="true"  OnSelectedIndexChanged="ddlSemester_SelectedIndexChanged"></asp:DropDownList>
                                            </td>
                                        </tr>--%>
                                        <tr>
                                            <td class="auto-style5"><asp:Label ID="Label8" runat="server" Text="Year No "></asp:Label></td>
                                            <td class="auto-style5">
                                                <asp:DropDownList ID="ddlYearNo" Width="180" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlYearNo_SelectedIndexChanged"></asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style5"><asp:Label ID="Label9" runat="server" Text="Semester No "></asp:Label></td>
                                            <td class="auto-style5">
                                                <asp:DropDownList ID="ddlSemesterNo" Width="180"  AutoPostBack="true"  runat="server" OnSelectedIndexChanged="ddlSemesterNo_SelectedIndexChanged"></asp:DropDownList>
                                            </td>
                                        </tr>
                                       
                                        <tr>
                                           <td class="auto-style5">Student Id</td>
                                            <td class="auto-style5">
                                                <asp:TextBox ID="txtStudentId" AutoPostBack="true" OnTextChanged="txtStudentId_TextChanged" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                       
                                        <tr>
                                            <td class="auto-style5"></td>
                                            <td class="auto-style5">
                                                <asp:Button ID="btnLoadStudent" runat="server"  CssClass="btnNew info" Height="25px" Text="Load Student" OnClick="btnLoadStudent_Click" />
                                            </td>
                                        </tr>
                                       
                                    </table>
                                </td>
                                <td>
                                    <table>
                                        <tr>
                                            <td class="auto-style5"><b>Course Filter</b></td>
                                        </tr>
                                        <%--<tr>
                                            <td class="auto-style5">Tree</td>
                                            <td class="auto-style5">
                                                <asp:DropDownList ID="ddlTree" runat="server" AutoPostBack="true" Width="180px" OnSelectedIndexChanged="ddlTree_SelectedIndexChanged"></asp:DropDownList>
                                                <asp:Label ID="lblTreeId" runat="server" Visible="false"></asp:Label>
                                            </td>
                                        </tr>--%>
                                        <tr>
                                            <td class="auto-style5">Syllabus</td>
                                            <td class="auto-style5">
                                                <asp:DropDownList ID="ddlCalenderDistribution" runat="server" AutoPostBack="true" Width="180px" OnSelectedIndexChanged="ddlCalenderDistribution_SelectedIndexChanged"></asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="auto-style5">Semester Course In Syllabus</td>
                                            <td class="auto-style5">
                                                <asp:DropDownList ID="ddlAddCourseTrimester" runat="server" AutoPostBack="true" Width="180px" OnSelectedIndexChanged="ddlAddCourseTrimester_SelectedIndexChanged"></asp:DropDownList>
                                            </td>
                                        </tr>
                                         <tr>
                                            <td class="auto-style5">Course</td>
                                            <td class="auto-style5">
                                                <asp:DropDownList ID="ddlCourse" runat="server" Width="180px" AutoPostBack="true" OnSelectedIndexChanged="ddlCourse_SelectedIndexChanged"></asp:DropDownList>
                                            </td>
                                        </tr>
                                         <tr>
                                            <td class="auto-style5">
                                                
                                            </td>
                                            <td class="auto-style5">
                                                <asp:Button ID="btnAddCourseAll" runat="server" CssClass="btnNew info" Height="25px" Visible="true" Text="Load All Course" OnClick="btnAddCourseAll_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <td class="auto-style5">
                        <asp:Button ID="btnClear" runat="server" Visible="false" Text="Load Course" OnClick="btnClear_Click" />
                    </td>
                </div>
            </div>
            <div style="clear: both;"></div>
            <div class="Message-Area">
                <div>
                    <asp:GridView ID="gvStudentCourse" runat="server"  DataKeyNames="CourseID" AutoGenerateColumns="False" AllowPaging="false" CellPadding="4" Width="100%"
                        ShowHeader="true" ShowFooter="True" CssClass="table-bordered" ForeColor="#333333" GridLines="None">
                        <HeaderStyle BackColor="#1C5E55" ForeColor="White" Height="30" Font-Bold="True" />
                        <FooterStyle BackColor="#1C5E55" ForeColor="White" Height="30" Font-Bold="True" />
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:TemplateField HeaderText="Course Id" Visible="false">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblCourseId" Font-Bold="true" Text='<%#Eval("CourseID") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Width="100px" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Version Id" Visible="false">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblVersionId" Font-Bold="true" Text='<%#Eval("VersionID") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Width="100px" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Sl. No" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate><b><%# Container.DataItemIndex + 1 %></b></ItemTemplate>
                                <ItemStyle Width="50px" />
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkSelectAll" runat="server" Text="All" OnCheckedChanged="chkSelectAll_CheckedChanged"
                                        AutoPostBack="true" />
                                </HeaderTemplate>
                                <%--<ItemTemplate>
                                    <div style="text-align: center">
                                        <asp:HiddenField ID="hdnId" runat="server" Value='<%#Eval("CourseID") %>' />
                                        <asp:CheckBox runat="server" ID="ChkActive"></asp:CheckBox>
                                    </div>
                                </ItemTemplate>
                                <HeaderStyle Width="40px" />--%>

                                <ItemTemplate>
                                    <asp:CheckBox ID="CheckBox" runat="server" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                                <HeaderStyle Width="50px" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Course Code">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblCourseCode" Font-Bold="true" Text='<%#Eval("FormalCode") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Width="200px" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Course Name">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblCourseName" Font-Bold="true" Text='<%#Eval("Title") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Width="400px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Credits">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblCourseCredit" Font-Bold="true" Text='<%#Eval("Credits") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Width="150px" />
                            </asp:TemplateField>

                        </Columns>
                        <EmptyDataTemplate>
                            No data found!
                        </EmptyDataTemplate>
                        <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />

                        <RowStyle Height="25px" VerticalAlign="Middle" HorizontalAlign="Left" BackColor="#E3EAEB" />
                        <EditRowStyle BackColor="#7C6F57" />
                        <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#F8FAFA" />
                        <SortedAscendingHeaderStyle BackColor="#246B61" />
                        <SortedDescendingCellStyle BackColor="#D4DFE1" />
                        <SortedDescendingHeaderStyle BackColor="#15524A" />
                    </asp:GridView>
                </div>
                <div style="padding: 5px; margin: 5px; width: 900px;">
                    <asp:Panel ID="pnlEnrollment" runat="server">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="Label2" runat="server" Text="Enrollment Type "></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlCourseStatus" runat="server" Width="100px" AutoPostBack="true">
                                        <asp:ListItem Text="Enrollment Type" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Regular" Value="9"></asp:ListItem>
                                        <asp:ListItem Text="Improvemt" Value="12"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Label ID="Label10" runat="server" Text="Exam "></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlExam" Width="180" runat="server"></asp:DropDownList>
                                </td>
                                <td class="auto-style2">
                                    <asp:Button ID="btnWorkSheet" runat="server" CssClass="btnNew success" Height="30px" Width="200px" Text="Enrollment" OnClick="btnWorkSheet_Click" />
                                </td>
                            
                                <%--<td class="auto-style3">
                                    <asp:Button ID="btnRegistrationAndBilling" runat="server" Visible="false" Text="Registration + Bill Generate" OnClick="btnRegistrationAndBilling_Click" />
                                </td>--%>
                                <%--<td class="auto-style2">
                                    <asp:Button ID="btnRegistration" runat="server" Visible="false" Text="Registration" OnClick="btnRegistration_Click" />
                                </td>--%>
                            </tr>
                        </table>
                    </asp:Panel>
                    
                </div>

                <div>
                    <asp:GridView ID="gvStudentList" runat="server" DataKeyNames="StudentID" 
                       CellPadding="4" AutoGenerateColumns="False" AllowPaging="false" Width="100%"
                        ShowHeader="true" ShowFooter="True" CssClass="table-bordered" ForeColor="#333333" GridLines="None">
                        <HeaderStyle BackColor="#1C5E55" ForeColor="White" Height="30" Font-Bold="True" />
                        <FooterStyle BackColor="#1C5E55" ForeColor="White" Height="30" Font-Bold="True" />
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:TemplateField HeaderText="Student Id" Visible="false">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStudentId" Font-Bold="true" Text='<%#Eval("StudentID") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Width="100px" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Sl. No" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate><b><%# Container.DataItemIndex + 1 %></b></ItemTemplate>
                                <ItemStyle Width="50px" />
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkSelectAllStudent" runat="server" Text="All" OnCheckedChanged="chkSelectAllStudent_CheckedChanged"
                                        AutoPostBack="true" />
                                </HeaderTemplate>
                                <%--<ItemTemplate>
                                    <div style="text-align: center">
                                        <asp:HiddenField ID="hdnId" runat="server" Value='<%#Eval("CourseID") %>' />
                                        <asp:CheckBox runat="server" ID="ChkActive"></asp:CheckBox>
                                    </div>
                                </ItemTemplate>
                                <HeaderStyle Width="40px" />--%>

                                <ItemTemplate>
                                    <asp:CheckBox ID="CheckBox" runat="server" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                                <HeaderStyle Width="50px" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Student Id">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStudentRoll" Font-Bold="true" Text='<%#Eval("Roll") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Width="100px" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Student Name">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStudnetName" Font-Bold="true" Text='<%#Eval("Name") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Width="300px" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Registered Courses">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblRegisteredCourses" Font-Bold="true" Text='<%#Eval("Attribute2") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Width="300px" />
                            </asp:TemplateField>

                            <%--<asp:TemplateField HeaderText="Batch">
                                <HeaderTemplate>
                                    <asp:DropDownList ID="ddlBatch" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged"></asp:DropDownList>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblBatch" Font-Bold="true" Text='<%#Eval("Batch.BatchNo") %>'></asp:Label>
                                </ItemTemplate>

                                <HeaderStyle Width="150px" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>--%>

                            <%-- <asp:TemplateField HeaderText="Open Courses">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblOpenCourses" Font-Bold="true" Text='<%#Eval("Attribute1") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Width="300px" />
                            </asp:TemplateField>--%>
                        </Columns>
                        <EmptyDataTemplate>
                            No data found!
                        </EmptyDataTemplate>
                        <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />

                        <RowStyle Height="25px" VerticalAlign="Middle" HorizontalAlign="Left" BackColor="#E3EAEB" />
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
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="txtStudentId" EventName="TextChanged" />
            <%--<asp:AsyncPostBackTrigger ControlID="Button1" EventName="Click" />--%>
        </Triggers>
    </asp:UpdatePanel>

    <ajaxToolkit:UpdatePanelAnimationExtender
        ID="UpdatePanelAnimationExtender1"
        TargetControlID="UpdatePanel2"
        runat="server">
        <Animations>
            <OnUpdating>
               <Parallel duration="0">
                    <ScriptAction Script="InProgress();" />
                    <EnableAction AnimationTarget="btnStudentButton" 
                                  Enabled="false" />                   
                </Parallel>
            </OnUpdating>
            <OnUpdated>
                <Parallel duration="0">
                    <ScriptAction Script="onComplete();" />
                    <EnableAction   AnimationTarget="btnStudentButton" 
                                    Enabled="true" />
                </Parallel>
            </OnUpdated>
        </Animations>
    </ajaxToolkit:UpdatePanelAnimationExtender>
</asp:Content>
