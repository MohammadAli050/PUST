<%@ Page Title="Continuous Marks Submit" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true" 
    CodeBehind="ExamResultSubmit.aspx.cs" Inherits="EMS.Module.result.ExamResultSubmit" %>

<%@ Register Src="~/UserControls/ProgramUserControl.ascx" TagPrefix="uc1" TagName="ProgramUserControl" %>
<%@ Register Src="~/UserControls/SessionUserControl.ascx" TagPrefix="uc1" TagName="SessionUserControl" %>
<%@ Register Src="~/UserControls/DepartmentUserControl.ascx" TagPrefix="uc1" TagName="DepartmentUserControl" %>
<%@ Register Src="~/UserControls/AdmissionSessionUserControl.ascx" TagPrefix="uc1" TagName="AdmissionSessionUserControl" %>

<asp:Content ID="Content3" ContentPlaceHolderID="Title" runat="Server">
    Continuous Marks Submit
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
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
<div>
    <div class="PageTitle">
        <label>Continuous Marks Submit</label>
    </div>

    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
            <div class="Message-Area">
                <label class="msgTitle">Message: </label>
                <asp:Label runat="server" class="msgTitle" ID="lblMsg" ForeColor="Red" Text="" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div id="divProgress" style="display: none; z-index: 1000; position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%);">
        <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Img/Waiting.gif" Height="150px" Width="150px" />
    </div>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <div class="Message-Area">
                    <table>
                        <tr>
                            <td class="auto-style4">
                                <b>Department :</b>
                            </td>
                            <td>
                                <uc1:DepartmentUserControl runat="server" ID="ucDepartment"  OnDepartmentSelectedIndexChanged="OnDepartmentSelectedIndexChanged" />
                            </td>
                            <td class="auto-style4">
                                <b>Program :</b>
                            </td>
                            <td class="auto-style6">
                                <uc1:ProgramUserControl runat="server" ID="ucProgram" OnProgramSelectedIndexChanged="OnProgramSelectedIndexChanged" />
                            </td>
                        </tr>
                        <tr>
                            <%--<td class="auto-style4">
                                <asp:Label ID="Label3" Width="120px" runat="server" Text="Versity Session"></asp:Label>
                            </td>
                            <td class="auto-style5">
                                <uc1:AdmissionSessionUserControl Width="150px" runat="server" ID="ucVersitySession" class="margin-zero dropDownList"/>
                            </td>--%>                        
                            <td class="auto-style4"><asp:Label ID="Label8" runat="server" Text="Year : "></asp:Label></td>
                            <td class="auto-style6">
                                <asp:DropDownList ID="ddlYearNo" Width="150px" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlYearNo_SelectedIndexChanged"></asp:DropDownList>
                            </td>
                            <td class="auto-style4"><asp:Label ID="Label9" runat="server" Text="Semester : "></asp:Label></td>
                            <td class="auto-style6">
                                <asp:DropDownList ID="ddlSemesterNo" Width="150px"  AutoPostBack="true"  runat="server" OnSelectedIndexChanged="ddlSemesterNo_SelectedIndexChanged"></asp:DropDownList>
                            </td>
                            <td class="auto-style4">
                                <asp:Label ID="Label10" runat="server" Text="Exam : "></asp:Label>
                            </td>
                            <td class="auto-style6">
                                <asp:DropDownList ID="ddlExam" Width="180" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlExam_SelectedIndexChanged"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style4">
                                <b>Course :</b>
                            </td>
                            <td class="auto-style6">
                                <asp:DropDownList ID="ddlCourse" AutoPostBack="true" Width="350px" OnSelectedIndexChanged="ddlCourse_SelectedIndexChanged" runat="server"></asp:DropDownList>
                            </td>
                            <td class="auto-style4">
                                <b>Exam :</b>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlContinousExam" Width="150px" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlContinousExam_SelectedIndexChanged"></asp:DropDownList>
                                <asp:Label ID="lblExamTemplateBasicItemId" Visible="false" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:Button ID="btnLoad" runat="server" Text="Load" OnClick="btnLoad_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td class="auto-style4">
                                <asp:Label ID="Label1" runat="server" Text="Mark : " Font-Bold="true" ForeColor="Blue"></asp:Label>
                                <asp:Label ID="lblExamMark" runat="server" Text="" Font-Bold="true" ForeColor="Blue"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                    </table>
                </div>
                <br />
                <asp:GridView ID="ResultEntryGrid" runat="server" AutoGenerateColumns="False" CssClass="table-bordered" Width="60%" 
                    EmptyDataText="No data found." CellPadding="4" OnRowDataBound="ResultEntryGrid_OnRowDataBound" OnRowCommand="ResultEntryGrid_RowCommand">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:TemplateField Visible="false" HeaderText="Student Id">
                            <ItemTemplate>
                                <asp:Label ID="lblCourseHistoryId" runat="server" Text='<%# Bind("CourseHistoryId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <HeaderStyle Width="150px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="SI." ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate><b><%# Container.DataItemIndex + 1 %></b></ItemTemplate>
                            <HeaderStyle Width="30px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Student Id">
                            <ItemTemplate>
                                <asp:Label ID="lblStudentRoll" runat="server" Text='<%# Bind("StudentRoll") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Student Name">
                            <ItemTemplate>
                                <asp:Label ID="lblStudentName" runat="server" Text='<%# Bind("StudentName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Mark">
                            <ItemTemplate>
                                <asp:TextBox ID="txtMark" Width="70px" runat="server" Text='<%# Bind("Mark") %>' />
                            </ItemTemplate>
                            <ItemStyle CssClass="center" />
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Status">
                        <ItemTemplate >
                            <div class="controls radio">
                                <div class="checkbox inline">
                                    <asp:Label ID="statusLabel" Visible="False" runat="server" Text='<%# Bind("Attribute1") %>'></asp:Label>
                                    <asp:CheckBox ID="chkStatus" runat="server" CssClass="display-inline" Font-Bold="true" Text="Absent" AutoPostBack="true" OnCheckedChanged="chkStatus_CheckedChanged"/>
                                    <%--<asp:CheckBox ID="CheckBox1" runat="server" CssClass="display-inline" Font-Bold="true" Checked='<%#(Eval("Attribute1")).ToString() == "Absent" ? true : false %>' Text="Absent" AutoPostBack="true" OnCheckedChanged="chkStatus_CheckedChanged"/>--%>
                                </div>
                            </div>
                        </ItemTemplate>
                        <ItemStyle CssClass="center" />
                        <HeaderStyle Width="100px"/>
                    </asp:TemplateField>

                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Button ID="SubmitAllMarkButton" runat="server" Text="Submit All" OnClick="SubmitAllMarkButton_Click" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="SubmitButton" CommandName="ResultSubmit" Text="Submit" CommandArgument='<%# Bind("CourseHistoryId") %>' runat="server"></asp:LinkButton>
                                <%--<asp:LinkButton ID="DeleteButton" CommandName="TestSetDelete" Text="Delete" CommandArgument='<%# Bind("StudentId") %>' runat="server"></asp:LinkButton>  OnClick="SubmitButton_Click"--%>
                            </ItemTemplate>
                            <HeaderStyle Width="80px"></HeaderStyle>
                            <ItemStyle CssClass="center" />
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <label>Data Not Found</label>
                    </EmptyDataTemplate>
                    <EditRowStyle BackColor="#999999" />
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#F7F6F3" ForeColor="#5D7B9D" HorizontalAlign="left" />
                    <RowStyle VerticalAlign="Middle" HorizontalAlign="Left"  BackColor="#F7F6F3" ForeColor="#333333" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                </asp:GridView>
            </div>
            <asp:HiddenField ID="StudentIdHiddenField" runat="server" />
            <asp:HiddenField ID="ExamIdHiddenField" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <ajaxToolkit:UpdatePanelAnimationExtender
        ID="UpdatePanelAnimationExtender1"
        TargetControlID="UpdatePanel1"
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
</div>
</asp:Content>
