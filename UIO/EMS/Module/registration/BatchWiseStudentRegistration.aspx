<%@ Page Title="Student Course Enrollment" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true" 
    CodeBehind="BatchWiseStudentRegistration.aspx.cs" Inherits="EMS.Module.registration.BatchWiseStudentRegistration" %>

<%@ Register Src="~/UserControls/SessionUserControl.ascx" TagPrefix="uc1" TagName="SessionUserControl" %>
<%@ Register Src="~/UserControls/ProgramUserControl.ascx" TagPrefix="uc1" TagName="ProgramUserControl" %>
<%@ Register Src="~/UserControls/DepartmentUserControl.ascx" TagPrefix="uc1" TagName="DepartmentUserControl" %>
<%@ Register Src="~/UserControls/AdmissionSessionUserControl.ascx" TagPrefix="uc1" TagName="AdmissionSessionUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="Server">
    Student Course Enrollment
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="Server">

    <style>
        .msgPanel {
            margin-top: 20px;
            margin-bottom: 25px;
            background-color: #f9f9f9;
            padding: 5px;
        }
        .button-margin {
            margin: 0px 0px 8px 0px;
        }

        .center-text {
            text-align: center;
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

        .auto-style4 {
            width: 233px;
        }

        .auto-style5 {
            width: 100px;
        }

        .auto-style6 {
            width: 26px;
        }

        .auto-style7 {
            width: 20px;
        }

        .auto-style8 {
            width: 61px;
        }

        .auto-style9 {
            width: 282px;
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
<asp:Content ID="Content3" ContentPlaceHolderID="MainContainer" runat="Server">
    <div>
        <div>
            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                <ContentTemplate>
                    <div class="PageTitle">
                        <label>Student Course Enrollment</label>
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

                    <div class="Message-Area" style="height: 80px;">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <div class="div-margin">
                                    <div class="loadArea">
                                        <table>
                                            <tr>
                                                <td class="auto-style5">Department</td>
                                                <td class="auto-style5">
                                                    <uc1:DepartmentUserControl runat="server" ID="ucDepartment"  OnDepartmentSelectedIndexChanged="OnDepartmentSelectedIndexChanged" />
                                                </td>

                                                <td class="auto-style5">Program</td>
                                                <td class="auto-style5">
                                                    <uc1:ProgramUserControl runat="server" ID="ucProgram" OnProgramSelectedIndexChanged="OnProgramSelectedIndexChanged" />
                                                </td>
                                                <td class="auto-style5"><asp:Label ID="Label3" Width="120px" runat="server" Text="Current Session"></asp:Label></td>
                                                <td class="auto-style5">
                                                     <uc1:AdmissionSessionUserControl runat="server" ID="ucCurrentSession" class="margin-zero dropDownList"/>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="auto-style5"><asp:Label ID="Label8" runat="server" Text="Year No"></asp:Label></td>
                                                <td class="auto-style5">
                                                    <asp:DropDownList ID="ddlYearNo" Width="150px" AutoPostBack="true" runat="server"></asp:DropDownList>
                                                </td>
                                                <td class="auto-style5"><asp:Label ID="Label9" runat="server" Text="Semester No"></asp:Label></td>
                                                <td class="auto-style5">
                                                    <asp:DropDownList ID="ddlSemesterNo" Width="150px"  AutoPostBack="true"  runat="server"></asp:DropDownList>
                                                </td>

                                                <td class="auto-style5">Student Id</td>
                                                <td class="auto-style5">
                                                    <asp:TextBox ID="txtStudentId" AutoPostBack="true" OnTextChanged="txtStudentId_TextChanged" runat="server"></asp:TextBox>
                                                </td>


                                                <td style="width: 100px">
                                                    <asp:Button ID="btnLoad" runat="server" Text="Load" OnClick="btnLoad_Click" Height="30px" Width="70px" BackColor="#edd366" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <%--<td class="auto-style5">Year</td>
                                                <td class="auto-style5">
                                                    <asp:DropDownList ID="ddlYear" Width="180px" AutoPostBack="true" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged" runat="server"></asp:DropDownList>
                                                </td>

                                                <td class="auto-style5">
                                                    <asp:Label ID="lblTerm" runat="server">Semester</asp:Label>
                                                </td>
                                                <td class="auto-style5">
                                                    <asp:DropDownList ID="ddlSemester" Width="180px" runat="server" AutoPostBack="true"  OnSelectedIndexChanged="ddlSemester_SelectedIndexChanged"></asp:DropDownList>
                                                </td>--%>

                                               
                                            </tr>
                                        </table>

                                        <div id="divProgress" style="display: none; float: right; z-index: 1000; margin-top: -15px">
                                            <div style="float: left">
                                                <asp:Image ID="LoadingImage" runat="server" ImageUrl="~/Images/Img/Waiting.gif" Height="35px" Width="35px" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                    <ajaxToolkit:UpdatePanelAnimationExtender
                        ID="UpdatePanelAnimationExtender1"
                        TargetControlID="UpdatePanel2"
                        runat="server">
                        <Animations>
                        <OnUpdating>
                           <Parallel duration="0">
                                <ScriptAction Script="InProgress();" />
                                <EnableAction AnimationTarget="btnLoad" 
                                              Enabled="false" />                   
                            </Parallel>
                        </OnUpdating>
                        <OnUpdated>
                            <Parallel duration="0">
                                <ScriptAction Script="onComplete();" />
                                <EnableAction   AnimationTarget="btnLoad" 
                                                Enabled="true" />
                            </Parallel>
                        </OnUpdated>
                        </Animations>
                    </ajaxToolkit:UpdatePanelAnimationExtender>


                    <asp:Label ID="Label2" runat="server" Font-Bold="true" Text="Count : "></asp:Label>
                    <asp:Label ID="lblCount" runat="server" Font-Bold="true"></asp:Label>

                    <asp:Panel ID="pnlAdvisor" runat="server">
                        <div style="float: right;">
                            <%--<asp:Button ID="btnSubmitToHOD" runat="server" Visible="false" CssClass="btnNew success" Text="Submit To HOD" Height="30" Enabled="true" OnClick="btnSubmitToHOD_Click" OnClientClick=" return confirm('Are you sure you want to Submitted to Head of Department?');" />--%>
                        </div>
                    </asp:Panel>

                    <asp:Panel runat="server" ID="pnlHOD">
                        <div style="float: Right; margin-right: 25px;">
                            <%--<asp:Button ID="btnRejectHOD" runat="server" Visible="false" Enabled="true" CssClass="btnNew success" Height="30px" Width="200px" Text="Reject" OnClick="btnRejectHOD_Click" OnClientClick=" return confirm('Are you sure you want to Reject Student Registration?');" />--%>
                        </div>
                        <div style="float: Right; margin-right: 15px;">
                            <%--<asp:Button ID="btnApproveHOD" runat="server" Visible="false" Enabled="true" CssClass="btnNew success" Height="30px" Width="200px" Text="Approve (HOD)" OnClick="btnApproveHOD_Click" OnClientClick=" return confirm('Are you sure you want to Forward Student Registration to Admission Office?');" />--%>
                        </div>
                    </asp:Panel>

                    <asp:Panel runat="server" ID="pnlAdmissionOffice">
                        <div style="float: Right; margin-right: 15px;">
                            <%--<asp:Button ID="btnAproveAdmissionOffice" runat="server" Visible="false" Enabled="true" CssClass="btnNew success" Height="30px" Width="200px" Text="Froward To Reg. Office" OnClick="btnAproveAdmissionOffice_Click" OnClientClick=" return confirm('Are you sure you want to Forward Student Registration to Register Office?');" />--%>
                        </div>
                    </asp:Panel>

                    <asp:Panel ID="pnlRegisterOffice" runat="server">
                        <div style="float: Right; margin-right: 15px;">
                            <asp:Button ID="btnApproveRegisterOffice" runat="server" Visible="false" Enabled="true" CssClass="btnNew success" Height="30px" Width="200px" Text="Confirm Enrollment" OnClick="btnApproveRegisterOffice_Click" OnClientClick=" return confirm('Are you sure you want to Confirm Student Registration?');" />
                        </div>
                    </asp:Panel>
                    <%--<div style="float: Right; margin-right: 25px;">
                        <asp:Button ID="ButtonForward" runat="server" Visible="false" Enabled="true" CssClass="btn-success" Height="30px" Width="150px" Text="Submit" OnClick="ButtonForward_Click" OnClientClick=" return confirm('Are you sure you want to Submit to Head of Department?');" />
                    </div>
                    <div style="float: Right; margin-right: 15px;">
                        <asp:Button ID="ButtonApprove" runat="server" Visible="false" Enabled="true" CssClass="btn-success" Height="30px" Width="150px" Text="Approve" OnClick="btnApprove_Click" OnClientClick=" return confirm('Are you sure you want to Complete Student Registration?');" />
                    </div>--%>
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
                                    <asp:CheckBox ID="chkSelectAllStudent" runat="server" Text="All" OnCheckedChanged="chkSelectAll_CheckedChanged"
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

                            <%--<asp:TemplateField HeaderText="Syllabus">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblSyllabus" Font-Bold="true" Text='<%#Eval("Attribute2") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Width="300px" />
                            </asp:TemplateField>--%>

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

                             <asp:TemplateField HeaderText="Open Courses">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblOpenCourses" Font-Bold="true" Text='<%#Eval("Attribute1") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Width="300px" />
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
                    <%--<asp:GridView runat="server" ID="gvStudentList" AutoGenerateColumns="False"
                        AllowPaging="false" PageSize="20" OnRowCommand="gvStudentList_RowCommand" EmptyDataText="No data found."
                        PagerSettings-Mode="NumericFirstLast" Width="100%"
                        PagerStyle-Font-Bold="true" PagerStyle-Font-Size="Larger"
                        ShowHeader="true" CssClass="gridCss" DataKeyNames="StudentId" OnRowCreated="gvStudentList_OnRowCreated">

                        <HeaderStyle BackColor="SeaGreen" ForeColor="White" />
                        <AlternatingRowStyle BackColor="#FFFFCC" />
                        <Columns>
                            <asp:TemplateField HeaderText="SL" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate><b><%# Container.DataItemIndex + 1 %></b></ItemTemplate>
                                <HeaderStyle Width="5%" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Student Roll">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdnId" runat="server" Value='<%#Eval("StudentId") %>' />
                                    <asp:Label runat="server" ID="lblRoll" Font-Bold="true" Text='<%#Eval("Roll") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="FullName">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblName" Font-Bold="true" Text='<%#Eval("Name") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Course And Section">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblCourseSection" Font-Bold="true" Text='<%#Eval("Course") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Width="40%" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkAll" runat="server" Text="Select All" Font-Bold="true" AutoPostBack="true" OnCheckedChanged="chkAll_CheckedChanged"></asp:CheckBox>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div style="text-align: center">
                                        <asp:HiddenField ID="hiddenStudentId" runat="server" Value='<%#Eval("StudentID") %>' />
                                        <asp:CheckBox runat="server" ID="ChkActive"></asp:CheckBox>
                                    </div>
                                </ItemTemplate>
                                <HeaderStyle Width="5%" HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Assigned Credit">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblAssign" Font-Bold="true" Text='<%#Eval("AssignedCredit") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Requested Credit">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblOccupied" Font-Bold="true" Text='<%#Eval("OccupiedCredit") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status" HeaderStyle-Width="20%">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStatus" Text='<%#Eval("CourseStatus").ToString()=="1" ? "Submitted to Advisor" : Eval("CourseStatus").ToString()=="2" ? "Submitted to HOD" :Eval("CourseStatus").ToString()=="3" ?"HOD Approved": Eval("CourseStatus").ToString()=="4" ?"Forwarded to Reg. Office" : Eval("CourseStatus").ToString()=="5" ?"Registration Done" : "" %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                                <HeaderStyle Width="20%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <asp:Button ID="btnSelect" runat="server" Text="View" CommandArgument='<%#Eval("StudentID") %>'
                                        CommandName="View" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                                <HeaderStyle Width="450px" />
                            </asp:TemplateField>

                        </Columns>
                        <RowStyle Height="25px" VerticalAlign="Middle" HorizontalAlign="Left" />
                        <EmptyDataTemplate>
                            No data found!
                        </EmptyDataTemplate>
                    </asp:GridView>--%>

                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>

