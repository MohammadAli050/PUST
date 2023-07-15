<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true" 
    CodeBehind="SingleStudentRegistrationV2.aspx.cs" Inherits="EMS.Module.registration.SingleStudentRegistrationV2" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Registration
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <style>
        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=80);
            opacity: 0.8;
            z-index: 10000;
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
            width: 93px;
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

        //function openPopup(strOpen) {
        //    open(strOpen, "Info",
        //         "status=1, width=300, height=200, top=100, left=300");
        //}

    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContainer" runat="server">
    <div style="height: auto; width: 100%">
        <div class="PageTitle">
            <label>Registration</label>
        </div>

        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <div class="Message-Area">
                <div class="TeacherManagement-container">
                    <div class="div-margin">
                        <div class="loadArea">
                            <table>
                                <tr>
                                    <td>
                                        <label><b>Session :</b></label>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlSession" AutoPostBack="true" OnSelectedIndexChanged="ddlSession_SelectedIndexChanged"></asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblStudentId" runat="server" Font-Bold="true" Text="Student ID :"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtStudent" runat="server" Text=""></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnLoad" CssClass="btnNew info" runat="server" Text="Load" OnClick="btnLoad_Click" Height="25px" Width="65px" />
                                    </td>
                                    <td class="auto-style8"></td>
                                </tr>
                                <tr>
                                    <td colspan="6"></td>
                                </tr>
                                <%--<tr>
                                    <td>
                                        <asp:Label ID="Label5" runat="server" Font-Bold="true" Text="Group Name :"></asp:Label></td>
                                    <td class="auto-style9">
                                        <asp:Label ID="lblGroupName" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label4" runat="server" Font-Bold="true" Text="Level/Earned Credit Group :"></asp:Label></td>
                                    <td class="auto-style9">
                                        <asp:Label ID="lblLevel" runat="server" Width="300px" Text=""></asp:Label>
                                    </td>
                                    <td colspan="2"></td>
                                </tr>--%>
                            </table>
                        </div>
                        <div class="loadedArea">
                            <table>
                                <tr>
                                    <td><b>Name:</b></td>
                                    <td class="auto-style4">
                                        <asp:Label ID="lblName" runat="server" Text=""></asp:Label>
                                    </td>
                                    <%--<td><b>Batch:</b></td>
                                    <td class="auto-style5">
                                        <asp:Label ID="lblBatch" runat="server" Text=""></asp:Label>
                                    </td>--%>
                                    <td><b>Program:</b></td>
                                    <td>
                                        <asp:Label ID="lblProgram" runat="server" Text=""></asp:Label>
                                        &nbsp; ||&nbsp;
                                    </td>
                                    <td>
                                        <asp:Label ID="lblRegistrationOpenMsg" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td class="auto-style6"></td>
                                    <td>
                                        <asp:Label ID="Label1" Font-Bold="true" runat="server" Text="Registration Session:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblRegistrationSession" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td class="auto-style7"></td>
                                    <td>
                                        <asp:Label ID="lblSectionCount" runat="server" ForeColor="Blue" Font-Bold="true"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCreditCount" runat="server" ForeColor="Blue" Font-Bold="true"></asp:Label>
                                        <asp:Label ID="lblAssignCredit" runat="server" ForeColor="Blue" Font-Bold="true"></asp:Label>
                                    </td>
                                    <td></td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <div id="divProgress" style="display: none; z-index: 1000; position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%);">
            <asp:Image ID="LoadingImage" runat="server" ImageUrl="~/Images/Spinner.gif" Height="150px" Width="150px" />
        </div>

        <asp:UpdatePanel ID="UpdatePanelMessage" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pnlMessage" runat="server" Visible="false">
                    <div class="Message-Area">
                        <asp:Label ID="Label2" runat="server" Text="Message : " Font-Bold="false"></asp:Label>
                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                    </div>
                </asp:Panel>
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

        <ajaxToolkit:UpdatePanelAnimationExtender
            ID="UpdatePanelAnimationExtender2"
            TargetControlID="UpdatePanel1"
            runat="server">
            <Animations>
            <OnUpdating>
               <Parallel duration="0">
                    <ScriptAction Script="InProgress();" />
                    <EnableAction AnimationTarget="btnRegistration" 
                                  Enabled="false" />                   
                </Parallel>
            </OnUpdating>
            <OnUpdated>
                <Parallel duration="0">
                    <ScriptAction Script="onComplete();" />
                    <EnableAction   AnimationTarget="btnRegistration" 
                                    Enabled="true" />
                </Parallel>
            </OnUpdated>
            </Animations>
        </ajaxToolkit:UpdatePanelAnimationExtender>

        <div style="clear: both"></div>

        <div style="padding: 5px; margin: 5px;">
            <fieldset>
                <legend>Final Registration</legend>
              
                <%--<div style="float: left; margin-right: 15px;">
                    <asp:Button ID="btnDownload" runat="server" Visible="true"  Enabled="true" CssClass="btnNew success" Height="30px" Width="200px" Text="Download PDF" OnClick="btnDownload_Click" OnClientClick=" return confirm('Are you sure you want to Download Student Registration Information?');" />
                </div>--%>
                

                <div class="education-info-body">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                            <asp:Panel runat="server" ID ="pnlRefreshDownload">
                                <div style="float: right; padding: 5px; margin: 5px;">
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>
                                            <%--<asp:LinkButton ID="lBtnRefresh" runat="server" Visible="false" Text="Refresh" OnClick="lBtnRefresh_Click"></asp:LinkButton>--%>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                
                            </asp:Panel>

                            <asp:Panel runat="server" ID ="pnlHOD">
                                <div style="float: Right; margin-right: 25px;">
                                    <%--<asp:Button ID="btnRejectHOD" runat="server" Visible="false" Enabled="true" CssClass="btnNew success" Height="30px" Width="200px" Text="Reject" OnClick="btnRejectHOD_Click" OnClientClick=" return confirm('Are you sure you want to Reject Student Registration?');" />--%>
                                </div>
                                <div style="float: Right; margin-right: 15px;">
                                    <%--<asp:Button ID="btnApproveHOD" runat="server" Visible="false" Enabled="true" CssClass="btnNew success" Height="30px" Width="200px" Text="Approve" OnClick="btnApproveHOD_Click" OnClientClick=" return confirm('Are you sure you want to Forward Student Registration to Admission Office?');" />--%>
                                </div>
                            </asp:Panel>

                            <asp:Panel runat="server" ID ="pnlAdmissionOffice">
                                <div style="float: Right; margin-right: 15px;">
                                    <%--<asp:Button ID="btnAproveAdmissionOffice" runat="server" Visible="false" Enabled="true" CssClass="btnNew success" Height="30px" Width="200px" Text="Froward To Reg. Office" OnClick="btnAproveAdmissionOffice_Click" OnClientClick=" return confirm('Are you sure you want to Forward Student Registration to Register Office?');" />--%>
                                </div>
                            </asp:Panel>

                            <asp:Panel ID="pnlRegisterOffice" runat="server">
                                <div style="float: Right; margin-right: 15px;">
                                    <asp:Button ID="btnApproveRegisterOffice" runat="server" Visible="true" Enabled="true" CssClass="btnNew success" Height="30px" Width="200px" Text="Confirm Registration" OnClick="btnApproveRegisterOffice_Click" OnClientClick=" return confirm('Are you sure you want to Confirm Student Registration?');" />
                                </div>
                            </asp:Panel>

                           
                            <div style="clear: both"></div>
                        </ContentTemplate>
                        
                    </asp:UpdatePanel>
                </div>

                <div class="education-info-body">
                    <div>
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:Panel runat="server" Visible="true" ID="pnlStudentCourseGrid">
                                    <div id="GridViewTable">
                                        <asp:GridView runat="server" ID="gvCourseRegistration" AutoGenerateColumns="False"
                                            AllowPaging="false" PagerSettings-Mode="NumericFirstLast" DataKeyNames="ID"
                                            PageSize="20" CellPadding="4" Width="100%"
                                            ShowHeader="true" ShowFooter="True" CssClass="table-bordered" ForeColor="#333333" GridLines="None">
                                        
                                            <HeaderStyle BackColor="#1C5E55" ForeColor="White" Height="30" Font-Bold="True" />
                                            <FooterStyle BackColor="#1C5E55" ForeColor="White" Height="30" Font-Bold="True" />
                                            <AlternatingRowStyle BackColor="White" />

                                            <Columns>
                                                <asp:TemplateField HeaderText="ID" Visible="false" HeaderStyle-Width="1%">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Visible="false" ID="lblWorkSheetId" Text='<%#Eval("ID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="1%" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Code" HeaderStyle-Width="10%">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblFormalCode" Text='<%#Eval("FormalCode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="6%" />
                                                </asp:TemplateField>
                                            
                                                <asp:TemplateField HeaderText="Version Code" Visible="false" HeaderStyle-Width="10%">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblVersionCode" Text='<%#Eval("VersionCode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="6%" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Title" HeaderStyle-Width="20%">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblCourseTitle" Text='<%#Eval("CourseTitle") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="20%" />
                                                </asp:TemplateField>

                                               <%-- <asp:TemplateField HeaderText="Program" HeaderStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblProgramName" Text='<%#Eval("ProgramAttribute1") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="15%" />
                                                </asp:TemplateField>--%>

                                                <asp:TemplateField HeaderText="Credits" HeaderStyle-Width="3%">
                                                    <ItemTemplate>
                                                        <div style="text-align: center;">
                                                            <asp:Label runat="server" ID="lblCredits" Text='<%#Eval("Credits") %>'></asp:Label>
                                                        </div>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="3%" />
                                                </asp:TemplateField>

                                                <%--<asp:TemplateField HeaderText="Group" Visible="false" HeaderStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblGroup" Text='<%#Eval("NodeGroup") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="5%" />
                                                </asp:TemplateField>--%>

                                               <%-- <asp:TemplateField HeaderText="Grade" HeaderStyle-Width="3%">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblGrade" Text='<%#Eval("ObtainedGrade") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="3%" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>--%>

                                                <%--<asp:TemplateField HeaderText="Reg Type" HeaderStyle-Width="6%">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblRegType" Font-Size="Smaller" Text='<%#Eval("AcaCalTypeName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="6%" />
                                                </asp:TemplateField>--%>

                                                <asp:TemplateField Visible="false" HeaderText="Session" HeaderStyle-Width="12%">
                                                    <ItemTemplate>
                                                        <div style="text-align: center;">
                                                            <asp:Label runat="server" ID="lblSession" Text='<%#Eval("CalendarDetailName") %>'></asp:Label>
                                                        </div>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="4%" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Section" HeaderStyle-Width="20%">
                                                    <ItemTemplate>
                                                        <div style="text-align: center;">
                                                           <asp:Button ID="btnSectionAdd" runat="server"  Text="Add" Width="50" ForeColor="Blue"
                                                                ToolTip="Add section." CommandArgument='<%#Eval("ID") %>'></asp:Button>
                                                            <%--OnClick="btnSectionAdd_Click"--%>

                                                            <asp:Button ID="btnRemoveSection" runat="server" Visible="false"  Text="Remove" Width="65" ForeColor="Crimson"
                                                                ToolTip="Remove section from course." CommandArgument='<%#Eval("ID") %>'
                                                                OnClientClick="return confirm('Are you sure to remove section ?')"></asp:Button>
                                                            <%--OnClick="btnRemoveSection_Click"--%>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                               <%-- <asp:TemplateField HeaderText="Select">
                                                    <ItemTemplate>
                                                        <div style="text-align: center">
                                                            <asp:HiddenField ID="hdnId" runat="server" Value='<%#Eval("ID") %>' />
                                                            <asp:CheckBox runat="server" ID="ChkActive" OnCheckedChanged="btnAssignCourse_Click" AutoPostBack="true" Checked='<%# Convert.ToBoolean(Eval("IsAutoAssign")) %>'></asp:CheckBox>
                                                        </div>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="4%" HorizontalAlign="Center" />
                                                </asp:TemplateField>--%>

                                                <%--<asp:TemplateField Visible="false" HeaderText="Requisition?" HeaderStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnRequisition" runat="server" OnClick="btnRequisition_Click" Font-Bold="true"
                                                            ForeColor='<%# (Boolean.Parse(Eval("Isrequisitioned").ToString())) ? System.Drawing.Color.Red : System.Drawing.Color.Blue %>'
                                                            OnClientClick='<%# Eval("CourseTitle","return confirm(\"Are you sure to Take the action for course: {0}\")") %>'
                                                            ToolTip="If course section is not found then requisition this course." CommandArgument='<%#Eval("ID") %>'>
                                                    <div style="text-align: center; font-weight:100;" >                                                 
                                                       <%# (Boolean.Parse(Eval("Isrequisitioned").ToString())) ? "x" : "√" %>                                                     
                                                    </div>
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>

                                                <asp:TemplateField HeaderText="Class Routine Name and Time" HeaderStyle-Width="12%">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblSection" Font-Size="Smaller" Text='<%#Eval("SectionName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="12%" />
                                                </asp:TemplateField>

                                                <asp:TemplateField Visible="false" HeaderText="Conflict" HeaderStyle-Width="10%">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblFormSL" Text='<%#Eval("ConflictedCourse")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="12%" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Status" HeaderStyle-Width="10%">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblStatus" Text='<%#Eval("RetakeNo").ToString()=="1" ? "Request for Registration" :Eval("RetakeNo").ToString()=="2" ?"Registration Done": "Request for Registration" %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle Width="10%" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Registration" HeaderStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <div style="text-align: center;">
                                                            <asp:Label ID="lblReg" runat="server" Font-Bold="true"
                                                                Text=' <%# (Boolean.Parse(Eval("IsRegistered").ToString())) ? "Done" : "--" %>'
                                                                ForeColor='<%# (Boolean.Parse(Eval("IsRegistered").ToString())) ? System.Drawing.Color.Red : System.Drawing.Color.Blue %>'></asp:Label>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" HeaderStyle-Width="40%">
                                                    <ItemTemplate>
                                                        <div style="text-align: center;">
                                                            <asp:Label runat="server" ID="lblRegType" Text='<%#Eval("AcaCalTypeName")%>'></asp:Label>
                                                            <asp:DropDownList ID="ddlCourseStatus" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCourseStatus_SelectedIndexChanged">
                                                                <asp:ListItem Text="RegType" Value="0"></asp:ListItem>
                                                                <asp:ListItem Text="R" Value="9"></asp:ListItem>
                                                                <asp:ListItem Text="RT" Value="10"></asp:ListItem>
                                                                <asp:ListItem Text="IM" Value="12"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:Button ID="btnRemoveCourse" runat="server" OnClick="btnRemoveCourse_Click" Text="Remove Course" Width="125" ForeColor="Red"
                                                                ToolTip="Remove Course" CommandArgument='<%#Eval("ID") %>'></asp:Button>
                                                        </div>
                                                    </ItemTemplate>
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
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </fieldset>
            
            <asp:UpdatePanel ID="UpdatePanel7" runat="server">
            <ContentTemplate>
                <fieldset>
                <legend>Add Retake/Improvement Course</legend>
                <div class="education-info-body">
                    <asp:Panel ID="pnlStudentReg" runat="server">
                        <div style="float: left; margin-right: 15px;">
                            <%--<asp:Button ID="btnSubmitToAdvisor" runat="server" Visible="false" Enabled="true" CssClass="btnNew success" Height="30px" Width="200px" OnClick="btnSubmitToAdvisor_Click" Text="Submit to Advisor" OnClientClick=" return confirm('Are you sure you want to Request for Registration ?');" />--%>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlAdvisor" runat="server">
                        <div style="float: left;">
                            <%--<asp:Button ID="btnSubmitToHOD" runat="server" Visible="false" CssClass="btnNew success" Text="Submit To HOD" Height="30" Enabled="true" OnClick="btnSubmitToHOD_Click" OnClientClick=" return confirm('Are you sure you want to Submitted to Head of Department?');" />--%>
                        </div>

                        <div style="float: left;">
                            <%--<asp:Button ID="btnAddCourse" runat="server" Visible="false" CssClass="btnNew success" Text="Add Retake Course" Height="30" Enabled="true" OnClick="btnAddCourse_Click"/>--%>
                        </div>

                        <div style="float: left;">
                            <%--<asp:Button ID="btnAddAnyCourse" runat="server" Visible="false" CssClass="btnNew success" Text="Add Course" Height="30" Enabled="true" OnClick="btnAddAnyCourse_Click"/>--%>
                        </div>
                    </asp:Panel>
                </div>
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
        </div>

        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
            <ContentTemplate>
                <asp:Panel ID="Panel1" runat="server">
                    <div class="Message-Area">
                        <asp:Label ID="Label3" runat="server" Text="Forwarded Message : " Font-Bold="false" Visible="false"></asp:Label>
                        <asp:Label ID="lblForwardedCourse" runat="server" ForeColor="Red" Font-Bold="true" Visible="false"></asp:Label>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
            <ContentTemplate>

                <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnShowPopup" PopupControlID="pnPopUp"
                    CancelControlID="btnCancel" BackgroundCssClass="modalBackground">
                </ajaxToolkit:ModalPopupExtender>

                <asp:Panel runat="server" ID="pnPopUp" Style="display: none; padding: 5px;" BackColor="WhiteSmoke">
                    <fieldset style="padding: 5px; margin: 5px;">
                        <legend style="font-weight: bold; font-size: large; text-align: center">Section Selection</legend>
                        <div style="float: right; padding: 10px;">
                            <asp:Button runat="server" ID="btnCancel" Text="X" Font-Bold="true" Style="width: 30px; height: 30px;" BackColor="SkyBlue" OnClick="btnCancel_Click" />
                        </div>
                        <div style="clear: both;">
                            <div>
                                <asp:GridView ID="GridViewSection" runat="server" DataKeyNames="AcaCalSectionID"
                                    AutoGenerateColumns="False"
                                     Width="700">
                                    <%--OnSelectedIndexChanged="GridViewSection_SelectedIndexChanged"--%>
                                    <Columns>
                                        <asp:CommandField ButtonType="Link" ShowSelectButton="True" />
                                        <asp:TemplateField HeaderText="Id" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblId" runat="server" Text='<%# Bind("AcaCalSectionID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Section Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSectionName" runat="server" Text='<%# Bind("SectionName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="AcaCal_SectionID" HeaderText="AcaCal_SectionID"
                                            Visible="False" />
                                        <asp:TemplateField HeaderText="Time Slot1">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTimeSlot1" runat="server" Text='<%# Bind("TimeSlot1") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Day One">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDayOne" runat="server" Text='<%# Bind("DayOne") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Time Slot2">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTimeSlot2" runat="server" Text='<%# Bind("TimeSlot2") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Day Two">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDayTwo" runat="server" Text='<%# Bind("DayTwo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Faculty1" HeaderText="Faculty (1)" Visible="False" />
                                        <asp:BoundField DataField="Faculty2" HeaderText="Faculty (2)" Visible="False" />
                                        <asp:BoundField DataField="RoomNo1" HeaderText="RoomNo (1)" Visible="False" />
                                        <asp:BoundField DataField="RoomNo2" HeaderText="RoomNo (2)" Visible="False" />
                                        <asp:BoundField DataField="Capacity" HeaderText="Capacity" />
                                        <asp:BoundField DataField="Occupied" HeaderText="Occupied" />

                                    </Columns>
                                    <HeaderStyle HorizontalAlign="Center"
                                        VerticalAlign="Middle" />
                                </asp:GridView>
                            </div>
                    </fieldset>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
            <ContentTemplate>

                <asp:Button ID="btnShowCourseAddPopUp" runat="server" Style="display: none" />
                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="btnShowCourseAddPopUp" PopupControlID="pnlCourseAdd"
                    CancelControlID="btnAddCourseCancel" BackgroundCssClass="modalBackground">
                </ajaxToolkit:ModalPopupExtender>
                <asp:Panel runat="server" ID="pnlCourseAdd" Style="display: none; padding: 5px;" BackColor="WhiteSmoke">
                    <fieldset style="padding: 5px; margin: 5px;">
                        <legend style="font-weight: bold; font-size: large; text-align: center">Course Selection</legend>
                        <div style="float: right; padding: 10px;">
                            <asp:Button runat="server" ID="btnAddCourseCancel" Text="X" Font-Bold="true" Style="width: 30px; height: 30px;" BackColor="SkyBlue" OnClick="btnAddCourseCancel_Click" />
                        </div>
                        <div style="clear: both;">
                            <div>
                                <asp:GridView ID="gvAddCourse" runat="server" DataKeyNames="CourseID"
                                    AutoGenerateColumns="False" OnSelectedIndexChanged="gvAddCourse_SelectedIndexChanged" Width="700">
                                    <Columns>
                                        <asp:CommandField ButtonType="Link" ShowSelectButton="True" />
                                        
                                        <asp:TemplateField HeaderText="Id" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCourseID" runat="server" Text='<%# Bind("CourseID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Id" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVersionID" runat="server" Text='<%# Bind("VersionID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Course Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFormalCode" runat="server" Text='<%# Bind("FormalCode") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Course Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTitle" runat="server" Text='<%# Bind("Title") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Credits">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCredits" runat="server" Text='<%# Bind("Credits") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField Visible="false" HeaderText="CourseFailCounter">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCourseFailCounter" runat="server" Text='<%# Bind("CourseFailCounter") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>
                                    <HeaderStyle HorizontalAlign="Center"
                                        VerticalAlign="Middle" />
                                </asp:GridView>
                            </div>
                    </fieldset>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
     <div>
        <rsweb:ReportViewer Visible="false" ID="ReportViewer" runat="server" WaitMessageFont-Names="Verdana" 
            WaitMessageFont-Size="14pt" BackColor="Wheat" BorderColor="WhiteSmoke" 
            BorderStyle="Solid" BorderWidth="1" CssClass="center" 
            asynrendering="true" Width="57%" Height="100%" 
            SizeToReportContent="true">
        </rsweb:ReportViewer>
    </div>
</asp:Content>
