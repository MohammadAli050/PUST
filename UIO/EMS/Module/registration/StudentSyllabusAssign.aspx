<%@ Page Title="Student Syllabus Assign" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true"
    CodeBehind="StudentSyllabusAssign.aspx.cs" Inherits="EMS.Module.registration.StudentSyllabusAssign" %>


<%@ Register Src="~/UserControls/ProgramUserControl.ascx" TagPrefix="uc1" TagName="ProgramUserControl" %>
<%@ Register Src="~/UserControls/DepartmentUserControl.ascx" TagPrefix="uc1" TagName="DepartmentUserControl" %>
<%@ Register Src="~/UserControls/AdmissionSessionUserControl.ascx" TagPrefix="uc1" TagName="AdmissionSessionUserControl" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="Server">Course List Assignment</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="Server">
    <script type="text/javascript">
        $(document).ready(function () {
        });

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
        #ctl00_MainContainer_ucDepartment_ddlDepartment, #ctl00_MainContainer_ucProgram_ddlProgram,
        #ctl00_MainContainer_ucSession_ddlSession, #ctl00_MainContainer_ddlCourseTree, #ctl00_MainContainer_ddlLinkedCalendars {
            height: 38px !important;
        }
    </style>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContainer" runat="Server">


    <div class="row">
        <div class="col-sm-12" style="font-size: 12pt; margin-top: 10pt;">
            <label><b style="color: black; font-size: 26px">Course List Assignment</b></label>
        </div>
    </div>
    <div id="divProgress" style="display: none; z-index: 100000000; position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%);">
        <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Img/Waiting.gif" Height="300px" Width="300px" />
        <div>
            <asp:Label ID="Label1" runat="server" Text="Processing your request.........." ForeColor="Red" Font-Bold="true" Font-Italic="true" Font-Size="30px"></asp:Label>
        </div>
    </div>

    <hr />

    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
        <ContentTemplate>

            <div class="card">
                <div class="card-body">
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <b>Choose Department</b>
                            <br />
                            <uc1:DepartmentUserControl runat="server" ID="ucDepartment" OnDepartmentSelectedIndexChanged="ucDepartment_DepartmentSelectedIndexChanged" />
                        </div>
                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <b>Choose Program</b>
                            <br />
                            <uc1:ProgramUserControl runat="server" ID="ucProgram" OnProgramSelectedIndexChanged="OnProgramSelectedIndexChanged" />
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <b>Choose Session</b>
                            <br />
                            <uc1:AdmissionSessionUserControl runat="server" ID="ucSession" OnSessionSelectedIndexChanged="ucSession_SessionSelectedIndexChanged" />
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <br />
                            <asp:Button ID="btnLoad" runat="server" OnClick="btnLoad_Click" Text="Click Here To Load Student" CssClass="btn-info w-100" Width="100%" Height="38px" />
                        </div>
                    </div>

                    <div class="row" style="margin-top: 10px">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <b>Choose Course List</b>
                            <asp:DropDownList runat="server" ID="ddlCourseTree" DataValueField="TreeMasterID" Width="100%" CssClass="form-control"
                                DataTextField="Node_Name" AutoPostBack="true" OnSelectedIndexChanged="CourseTree_SelectedIndexChanged" />
                        </div>
                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <b>Choose Syllabus</b>
                            <asp:DropDownList runat="server" ID="ddlLinkedCalendars" Width="100%" CssClass="form-control" DataValueField="TreeCalendarMasterID"
                                DataTextField="Name" />
                        </div>
                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <br />
                            <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Click Here To Save" CssClass="btn-info w-100" Width="100%" Height="38px" />

                        </div>
                    </div>

                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>


    <ajaxToolkit:UpdatePanelAnimationExtender
        ID="UpdatePanelAnimationExtender1"
        TargetControlID="UpdatePanel3"
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

    <ajaxToolkit:UpdatePanelAnimationExtender
        ID="UpdatePanelAnimationExtender2"
        TargetControlID="UpdatePanel3"
        runat="server">
        <Animations>
                    <OnUpdating>
                    <Parallel duration="0">
                    <ScriptAction Script="InProgress();" />
                    <EnableAction AnimationTarget="btnSave" 
                    Enabled="false" />                   
                    </Parallel>
                    </OnUpdating>
                    <OnUpdated>
                    <Parallel duration="0">
                    <ScriptAction Script="onComplete();" />
                    <EnableAction   AnimationTarget="btnSave" 
                    Enabled="true" />
                    </Parallel>
                    </OnUpdated>
                    </Animations>
    </ajaxToolkit:UpdatePanelAnimationExtender>

    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
            <div class="card" style="margin-top: 10px">
                <div class="card-body">
                    <asp:Panel ID="PnlAssignCourseTree" runat="server" Wrap="False">
                        <asp:GridView ID="gvwCollection" runat="server" AutoGenerateColumns="False" TabIndex="6" ShowHeader="true"
                            HeaderStyle-BorderColor="Yellow" CssClass="table-bordered"
                            ShowFooter="True" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                            <HeaderStyle BackColor="#4285f4" ForeColor="White" Height="10px" Font-Bold="True" />
                            <FooterStyle BackColor="#4285f4" ForeColor="White" Height="10px" Font-Bold="True" />
                            <AlternatingRowStyle BackColor="White" />
                            <RowStyle Height="10px" />
                            <Columns>
                                <asp:TemplateField HeaderText="Sl. No">
                                    <ItemTemplate><b><%# Container.DataItemIndex + 1 %></b></ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkAll" Text="Select All" runat="server" Font-Bold="true"
                                            AutoPostBack="True" OnCheckedChanged="chkAll_CheckedChanged" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chk" runat="server" AutoPostBack="true" OnCheckedChanged="chk_CheckedChanged" />
                                        <asp:HiddenField runat="server" ID="hfStudentID" Value='<%#Eval("StudentID") %>' />
                                        <asp:HiddenField runat="server" ID="hfTreeMasterID" Value='<%#Eval("TreeMasterID") %>' />
                                        <asp:HiddenField runat="server" ID="hfTreeCalendarMasterID" Value='<%#Eval("TreeCalendarMasterID") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="StudentID" HeaderStyle-HorizontalAlign="Center" HeaderText="ID"></asp:BoundField>

                                <asp:BoundField DataField="Roll" HeaderText="Student ID" HeaderStyle-HorizontalAlign="Center"></asp:BoundField>

                                <asp:BoundField DataField="BasicInfo.FullName" HeaderText="Student name">
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>

                                <asp:BoundField DataField="TreeMasterID" HeaderText="tree id" Visible="false"></asp:BoundField>

                                <asp:BoundField DataField="TreeCalendarMasterID" HeaderText="link Canlanders" Visible="false"></asp:BoundField>

                                <asp:BoundField DataField="CourseTreeLinkCalendars" HeaderText="Assigned Course List"></asp:BoundField>

                                <asp:BoundField HeaderText="Course List to assign"></asp:BoundField>
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
                    </asp:Panel>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>


