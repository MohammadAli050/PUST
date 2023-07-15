<%@ Page Title="Class Routine Manage" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true" Inherits="Admin_ClassRoutine" CodeBehind="ClassRoutine.aspx.cs" %>

<%@ Register Src="~/UserControls/ProgramUserControl.ascx" TagPrefix="uc1" TagName="ProgramUserControl" %>
<%@ Register Src="~/UserControls/SessionUserControl.ascx" TagPrefix="uc1" TagName="SessionUserControl" %>
<%@ Register Src="~/UserControls/BatchUserControlAll.ascx" TagPrefix="uc1" TagName="BatchUserControlAll" %>
<%@ Register Src="~/UserControls/BatchUserControl.ascx" TagPrefix="uc1" TagName="BatchUserControl" %>
<%@ Register Src="~/UserControls/DepartmentUserControl.ascx" TagPrefix="uc1" TagName="DepartmentUserControl" %>
<%@ Register Src="~/UserControls/AdmissionSessionUserControl.ascx" TagPrefix="uc1" TagName="AdmissionSessionUserControl" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="Server">Class Routine Manage</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="Server">

    <script src="../../JavaScript/jquery-3.0.0.js"></script>
    <%--<script src="../../JavaScript/script.js"></script>--%>
    <link href="../../CSS/select2.min.css" rel="stylesheet" />
    <script type="text/javascript" src="../../JavaScript/select2.full.min.js"></script>
    <script src="JS/ClassRoutine.js"></script>

    <style type="text/css">
        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=80);
            opacity: 0.8;
            z-index: -1;
        }

        .chkList-table {
            margin-left: 130px;
        }

        .btn-margin {
            margin-left: 10px;
        }

        .checkbox .btn,
        .checkbox-inline .btn {
            padding-left: 2em;
            min-width: 8em;
        }

        .checkbox label,
        .checkbox-inline label {
            text-align: left;
            padding-left: 0.5em;
        }

        .button_1 {
            height: 34px;
            width: 98px;
            border-radius: 5px;
            padding-left: 16px;
            background-color: #368445;
            color: white;
        }



        .button_2 {
            height: 34px;
            width: 107px;
            border-radius: 5px;
            padding-left: 20px;
            background-color: #39b54a;
            color: white;
            padding-left: 29px;
        }

        /*.dropdown {
            width: 203px;
        }*/

        .field {
            width: 189px;
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
    </style>
    <script type="text/javascript">
        $(function () {
            initdropdown();
        })

        function initdropdown() {
            $('#ctl00_MainContainer_ddlFaculty1').select2({ placeholder: { id: '0', text: '-Select-' }, allowClear: true });
        }

        $(document).ready(function () { $('#ctl00_MainContainer_ddlFaculty1').select2({ placeholder: { id: '0', text: '-Select-' }, allowClear: true }); });
    </script>

    <script type="text/javascript">
        $(function () {
            initdropdown2();
        })

        function initdropdown2() {
            $('#ctl00_MainContainer_ddlFaculty2').select2({ placeholder: { id: '0', text: '-Select-' }, allowClear: true });
        }

        $(document).ready(function () { $('#ctl00_MainContainer_ddlFaculty2').select2({ placeholder: { id: '0', text: '-Select-' }, allowClear: true }); });
    </script>

    <script type="text/javascript">
        $(function () {
            initdropdown3();
        })

        function initdropdown3() {
            $('#ctl00_MainContainer_ddlFaculty3').select2({ placeholder: { id: '0', text: '-Select-' }, allowClear: true });
        }

        $(document).ready(function () { $('#ctl00_MainContainer_ddlFaculty3').select2({ placeholder: { id: '0', text: '-Select-' }, allowClear: true }); });
    </script>

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

        function MandatoryFieldCheck() {
            if ($('#ctl00_MainContainer_ddlCourse option:selected').val() == "0" || $('#ctl00_MainContainer_txtSection').val() == "" || $('#ctl00_MainContainer_txtCapacity').val() == "") {
                $('#ctl00_MainContainer_lblPopUpMessage').text('Course, Sectoin and Capacity are mandatory');
                return false;
            }
            else {
                $('#ctl00_MainContainer_lblPopUpMessage').text('');
                return true;
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContainer" runat="Server">
    <div>
        <div class="PageTitle">
            <label>Class Routine Manage</label>
        </div>

        <div>
            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                <ContentTemplate>
                    <div class="Message-Area">
                        <label class="msgTitle">Message: </label>
                        <asp:Label runat="server" class="msgTitle" ID="lblMsg" ForeColor="Red" Text="" />
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="divProgress" style="display: none; z-index: 1000; position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%);">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Img/Waiting.gif" Height="150px" Width="150px" />
        </div>
        <div style="clear: both;"></div>
    </div>

    <asp:Panel runat="server" ID="panelContainer">
        <asp:UpdatePanel runat="server" ID="UpClassSchedule">
            <ContentTemplate>
                <div class="Message-Area">
                    <div class="div-margin">
                        <div class="loadArea">
                            <table>
                                <tr>
                                    <td>Department:</td>
                                    <td>
                                        <uc1:DepartmentUserControl runat="server" ID="ucDepartment" OnDepartmentSelectedIndexChanged="OnDepartmentSelectedIndexChanged" />
                                    </td>

                                    <td>Program:</td>
                                    <td>
                                        <uc1:ProgramUserControl runat="server" ID="ucProgram" OnProgramSelectedIndexChanged="OnProgramSelectedIndexChanged" />
                                    </td>
                                    <td><b>Session: </b></td>
                                    <td>
                                        <uc1:AdmissionSessionUserControl runat="server" ID="ucCurrentSession" />
                                    </td>
                                    <td style="visibility: hidden">Session:</td>
                                    <td style="visibility: hidden">
                                        <uc1:SessionUserControl runat="server" ID="ucSession" />
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label8" runat="server" Text="Year : "></asp:Label></td>
                                    <td>
                                        <asp:DropDownList ID="ddlYearNo" Width="150px" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlYearNo_SelectedIndexChanged"></asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label9" runat="server" Text="Semester : "></asp:Label></td>
                                    <td>
                                        <asp:DropDownList ID="ddlSemesterNo" Width="150px" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlSemesterNo_SelectedIndexChanged"></asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label10" runat="server" Text="Exam : "></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlExam" Width="250" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlExam_SelectedIndexChanged"></asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Button runat="server" ID="btnLoad" Text="Load" OnClick="btnLoad_Click" class="button_2" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <div class="Message-Area">
                    <div class="div-margin">
                        <div class="loadArea">
                            <table>
                                <tr>
                                    <td>Course:</td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtFilterCourse" />
                                    </td>
                                    <td>Room:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtFilterRoom" />
                                    </td>

                                    <td>Day:</label>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtFilterDay" />
                                    </td>

                                    <td>Time Slot:</label>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtFilteTimeSlotn" />
                                    </td>

                                    <td>Faculty</label>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtFilterFaculty" />
                                    </td>

                                    <td>
                                        <asp:Button runat="server" ID="btnSearch" Text="Filter" OnClick="btnSearch_Click" class="button_2" />
                                    </td>
                                </tr>
                            </table>
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

        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <div class="ClassRoutine-container">

                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <div>
                                <div style="margin: 5px; float: left;">
                                    <asp:Button ID="btnAddNew" runat="server" BackColor="LightSkyBlue" OnClick="btnAddNew_Click" Text="Add New" Visible="false"></asp:Button>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div style="clear: both;"></div>
                    <asp:GridView ID="gvClassSchedule" runat="server" AllowSorting="True" CssClass="table-bordered"
                        AutoGenerateColumns="False" ShowFooter="True" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                        <HeaderStyle BackColor="#1C5E55" ForeColor="White" Height="30" Font-Bold="True" />
                        <FooterStyle BackColor="#1C5E55" ForeColor="White" Height="30" Font-Bold="True" />
                        <AlternatingRowStyle BackColor="White" />
                        <RowStyle Height="25" />
                        <Columns>
                            <asp:TemplateField HeaderText="SL" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate><b><%# Container.DataItemIndex + 1 %></b></ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Course Code" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblCourseCode" Font-Bold="true" Text='<%# Eval("Course.FormalCode") %>' />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Section Information" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblCourseName" Text='<%# "<b>Course Name : </b>"+Eval("Course.Title") %>' />
                                    <br />
                                    <asp:Label runat="server" ID="lblCredits" Text='<%# "<b>Credit : </b>"+ Eval("Course.Credits").ToString() + ", <b>Section : </b>"+ Eval("SectionName") + ", <b>Size : </b>"+ Eval("Capacity") %>' />
                                    <br />
                                    <asp:Label runat="server" ID="lblFaculty1" Text='<%#"<b>Course Teacher One : </b>"+Eval("TeacherInfoOne") %>' />
                                    <br />
                                    <asp:Label runat="server" ID="lblFaculty2" Text='<%#"<b>Course Teacher Two: </b>"+Eval("TeacherInfoTwo") %>' />
                                    <br />
                                    <asp:Label runat="server" ID="lblFaculty3" Text='<%#"<b>Course Teacher Three: </b>"+Eval("TeacherInfoThree") %>' />
                                    <br />
                                    <asp:Label runat="server" ID="lblRoom" Text='<%#"<b>Room One : </b>"+Eval("RoomInfoOne") + ", <b>Room Two : </b>"+Eval("RoomInfoTwo")+ ", <b>Room Three : </b>"+Eval("RoomInfoThree") %>' />
                                    <br />
                                    <asp:Label runat="server" ID="lblDay" Text='<%#"<b>Day One : </b>"+ Eval("DayInfoOne") + ", <b>Day Two : </b>"+ Eval("DayInfoTwo") + ", <b>Day Three : </b>"+ Eval("DayInfoThree")%>' />
                                    <br />
                                    <asp:Label runat="server" ID="lblTimeSlot" Text='<%#"<b>Time One : </b>"+Eval("TimeSlotPlanInfoOne") + ", <b>Time Two : </b>"+Eval("TimeSlotPlanInfoTwo")+ ", <b>Time Three : </b>"+Eval("TimeSlotPlanInfoThree") %>' />
                                    <br />
                                    <asp:Label runat="server" ID="lblOnlineGradeSheetTemplateID" Text='<%#"<b>Mark Template : </b>"+Eval("GradeSheetTemplate") %>' />

                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Section Exam Information" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblYear" Text='<%# "<b>Year No : </b>"+Eval("YearNumber") %>' />
                                    <br />
                                    <asp:Label runat="server" ID="lblSemester" Text='<%# "<b>Semester No : </b>"+ Eval("SemesterNumber").ToString() %>' />
                                    <br />
                                    <asp:Label runat="server" ID="lblExam" Text='<%#"<b>Exam Name: </b>"+Eval("ExamName") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Program" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblProgram" Text='<%#Eval("ProgramName") %>' />
                                </ItemTemplate>
                                <ItemStyle Width="130" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ToolTip="Update" Text="Edit" ID="lbEdit" CommandArgument='<%#Eval("AcaCal_SectionID") %>' OnClick="lbEdit_Click"> 
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <HeaderStyle Width="100px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Delete" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ToolTip="Delete" Text="Delete" ID="lbDelete" CommandArgument='<%#Eval("AcaCal_SectionID") %>' OnClick="lbDelete_Click" OnClientClick="return confirm('Are you sure to Delete ?')"> 
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <HeaderStyle Width="100px" />
                            </asp:TemplateField>

                        </Columns>
                        <EmptyDataTemplate>
                            <label>Data Not Found</label>
                        </EmptyDataTemplate>
                        <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />

                        <RowStyle Height="25px" VerticalAlign="Middle" HorizontalAlign="Left" BackColor="#E3EAEB" />
                        <EditRowStyle BackColor="#7C6F57" />
                        <EmptyDataTemplate>
                            No data found!
                        </EmptyDataTemplate>
                        <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#F8FAFA" />
                        <SortedAscendingHeaderStyle BackColor="#246B61" />
                        <SortedDescendingCellStyle BackColor="#D4DFE1" />
                        <SortedDescendingHeaderStyle BackColor="#15524A" />
                    </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <%--</div>--%>
    <div class="ClassRoutine-container">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>

                <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnShowPopup" PopupControlID="pnPopUp"
                    CancelControlID="btnCancel" BackgroundCssClass="modalBackground">
                </ajaxToolkit:ModalPopupExtender>

                <asp:Panel runat="server" ID="pnPopUp">


                    <div class="updatePanel-Wrapper" style="width: 1200px; margin: 5px; padding-left: 355px; height: 536px; padding-right: 5px; padding-top: 5px; padding-bottom: 5px;">
                        <div class="col-sm-12" style="height: 530px; width: 784px; background-color: Window; overflow: scroll">
                            <div class="col-sm-12">
                                <fieldset style="padding: 5px; margin: 5px;">
                                    <legend style="font-weight: bold; font-size: large; text-align: center">Section</legend>
                                    <div>
                                        <div>
                                            <asp:Label runat="server" ID="lblPopUpMessageTitle" Text="Message" CssClass="label-width-popUp"></asp:Label>
                                            <asp:Label runat="server" ID="lblPopUpMessage" Style="color: red;"></asp:Label>
                                            <asp:Label runat="server" ID="lblAcaCalSectionId" Visible="false"></asp:Label>
                                        </div>

                                        <hr />

                                        <%--<div runat="server" id="divCourses" style="float: left; margin-right: 15px;">
                                        <label class="label-width-popUp">Course<sup style="color: red;">*</sup></label>
                                       
                                    </div>
                                    <div style="float: left; margin-right: 15px;">
                                        <label class="label-width">Section<sup style="color: red;">*</sup></label>
                                        
                                    </div>
                                    <div style="float: left; margin-right: 15px;">
                                        <label class="label-width">Capacity<sup style="color: red;">*</sup></label>
                                        
                                    </div>--%>
                                        <%-- (Visibility Off) Block By Sajib --%>
                                        <div style="float: left; margin-right: 15px; visibility: hidden; height: 0px;">
                                            <label class="label-width">Section Definition</label>
                                            <asp:TextBox CssClass="field-width" runat="server" ID="txtSectionDefination" />
                                        </div>
                                        <div style="clear: both; height: 1px;"></div>

                                        <%-- (Visibility Off) Block By Sajib --%>
                                        <div style="float: left; margin-right: 15px; visibility: hidden; height: 0px;">
                                            <label class="label-width">Section Gender</label>
                                            <asp:DropDownList CssClass="combo-width" runat="server" ID="ddlSectionGender" />
                                        </div>

                                        <table>
                                            <tr>
                                                <td>
                                                    <label class="label-width-popUp">Course : <sup style="color: red;">*</sup></label></td>
                                                <td colspan="2">
                                                    <asp:DropDownList CssClass="combo-width" AutoPostBack="false" EnableViewState="true" runat="server" ID="ddlCourse" /></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="label-width">Section : <sup style="color: red;">*</sup></label></td>
                                                <td colspan="2">
                                                    <asp:TextBox CssClass="field-width-popUp" runat="server" ID="txtSection" /></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="label-width">Capacity : <sup style="color: red;">*</sup></label></td>
                                                <td>
                                                    <asp:TextBox CssClass="field-width-popUp" runat="server" ID="txtCapacity" /></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="label-width">Year : <sup style="color: red;">*</sup></label></td>
                                                <td>
                                                    <asp:DropDownList ID="ddlModalYear" Width="150px" Enabled="false" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlModalYear_SelectedIndexChanged"></asp:DropDownList>

                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="label-width">Semester : <sup style="color: red;">*</sup></label></td>
                                                <td>
                                                    <asp:DropDownList ID="ddlModalSemester" Width="150px" Enabled="false" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlModalSemester_SelectedIndexChanged"></asp:DropDownList></td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <label class="label-width">ExamName : <sup style="color: red;">*</sup></label></td>
                                                <td>
                                                    <asp:DropDownList ID="ddlModalExam" Width="150px" Enabled="false" runat="server"></asp:DropDownList></td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div style="clear: both; margin: 2px"></div>

                                    <hr />

                                    <div>
                                        <table id="tbl">
                                            <tr style="background-color: #cecece;">
                                                <td>
                                                    <label style="width: 300px;color:blue">Department-1</label>
                                                    <uc1:DepartmentUserControl runat="server" ID="DepartmentUserControl1" OnDepartmentSelectedIndexChanged="DepartmentUserControl1_DepartmentSelectedIndexChanged" />
                                                </td>
                                                <td>
                                                    <label style="color:blue">Course Teacher-1</label>

                                                    <asp:DropDownList runat="server" ID="ddlFaculty1" class="dropdown" /></td>
                                                <td>
                                                    <label style="color:blue">Room-1 </label>

                                                    <asp:DropDownList runat="server" ID="ddlRoomInfo1" class="dropdown" /></td>
                                                <td>
                                                    <label style="color:blue">Day-1</label>

                                                    <asp:DropDownList runat="server" ID="ddlDay1" class="dropdown" /></td>
                                                <td>
                                                    <label style="color:blue">Time-1</label>
                                                    <asp:DropDownList runat="server" ID="ddlTimeSlot1" class="dropdown" /></td>


                                            </tr>
                                            <tr style="background-color: #bdc8ff;">
                                                <td>
                                                    <label style="width: 300px;color:blue">Department-2</label>
                                                    <uc1:DepartmentUserControl runat="server" ID="DepartmentUserControl2" OnDepartmentSelectedIndexChanged="DepartmentUserControl2_DepartmentSelectedIndexChanged" />
                                                </td>
                                                <td>
                                                    <label style="color:blue">Course Teacher-2</label>
                                                    <asp:DropDownList runat="server" ID="ddlFaculty2" class="dropdown" />
                                                </td>
                                                <td>
                                                    <label style="color:blue">Room-2 </label>
                                                    <asp:DropDownList runat="server" ID="ddlRoomInfo2" class="dropdown" />
                                                </td>
                                                <td>
                                                    <label style="color:blue">Day-2</label>
                                                    <asp:DropDownList runat="server" ID="ddlDay2" class="dropdown" />
                                                </td>
                                                <td>
                                                    <label style="color:blue">Time-2</label>
                                                    <asp:DropDownList runat="server" ID="ddlTimeSlot2" class="dropdown" />
                                                </td>
                                            </tr>

                                            <tr style="background-color: #cecece;">
                                                <td>
                                                    <label style="width: 300px;color:blue">Department-3</label>
                                                    <uc1:DepartmentUserControl runat="server" ID="DepartmentUserControl3" OnDepartmentSelectedIndexChanged="DepartmentUserControl3_DepartmentSelectedIndexChanged" />

                                                </td>
                                                <td>
                                                    <label style="color:blue"> Course Teacher-3</label>
                                                    <asp:DropDownList runat="server" ID="ddlFaculty3" class="dropdown" />
                                                </td>
                                                <td>
                                                    <label style="color:blue">Room-3 </label>
                                                    <asp:DropDownList runat="server" ID="ddlRoomInfo3" class="dropdown" />
                                                </td>
                                                <td>
                                                    <label style="color:blue">Day-3</label>
                                                    <asp:DropDownList runat="server" ID="ddlDay3" class="dropdown" />
                                                </td>
                                                <td>
                                                    <label style="color:blue">Time-3</label>
                                                    <asp:DropDownList runat="server" ID="ddlTimeSlot3" class="dropdown" />
                                                </td>
                                            </tr>

                                        </table>
                                        <%--<div style="float: left; margin-right: 15px;">
                                        <label class="label-width-popUp">Room-1 </label>
                                        <asp:DropDownList CssClass="combo-width" runat="server" ID="ddlRoomInfo1" />
                                    </div>
                                    <div style="float: left; margin-right: 15px;">
                                        <label class="label-width">Room-2 </label>
                                        <asp:DropDownList CssClass="combo-width" runat="server" ID="ddlRoomInfo2" />
                                    </div>
                                    <div style="float: left; margin-right: 15px;">
                                        <label class="label-width">Room-3 </label>
                                        <asp:DropDownList CssClass="combo-width" runat="server" ID="ddlRoomInfo3" />
                                    </div>
                                    <div style="clear: both;"></div>

                                    <div style="float: left; margin-right: 15px;">
                                        <label class="label-width-popUp">Day-1</label>
                                        <asp:DropDownList CssClass="combo-width" runat="server" ID="ddlDay1" />
                                    </div>
                                    <div style="float: left; margin-right: 15px;">
                                        <label class="label-width">Day-2</label>
                                        <asp:DropDownList CssClass="combo-width" runat="server" ID="ddlDay2" />
                                    </div>
                                    <div style="float: left; margin-right: 15px;">
                                        <label class="label-width">Day-3</label>
                                        <asp:DropDownList CssClass="combo-width" runat="server" ID="ddlDay3" />
                                    </div>
                                    <div style="clear: both;"></div>

                                    <div style="float: left; margin-right: 15px;">
                                        <label class="label-width-popUp">Time-1</sup></label>
                                        <asp:DropDownList CssClass="combo-width" runat="server" ID="ddlTimeSlot1" />
                                    </div>
                                    <div style="float: left; margin-right: 15px;">
                                        <label class="label-width">Time-2</label>
                                        <asp:DropDownList CssClass="combo-width" runat="server" ID="ddlTimeSlot2" />
                                    </div>
                                    <div style="float: left; margin-right: 15px;">
                                        <label class="label-width">Time-3</label>
                                        <asp:DropDownList CssClass="combo-width" runat="server" ID="ddlTimeSlot3" />
                                    </div>
                                    <div style="clear: both;"></div>

                                    <div style="float: left; margin-right: 15px;">
                                        <label class="label-width-popUp">Faculty-1</label>
                                        <asp:DropDownList CssClass="combo-width" runat="server" ID="ddlFaculty1" />
                                    </div>
                                    <div style="float: left; margin-right: 15px;">
                                        <label class="label-width">Faculty-2</label>
                                        <asp:DropDownList CssClass="combo-width" runat="server" ID="ddlFaculty2" />
                                    </div>
                                    <div style="float: left; margin-right: 15px;">
                                        <label class="label-width">Faculty-3</label>
                                        <asp:DropDownList CssClass="combo-width" runat="server" ID="ddlFaculty3" />
                                    </div>--%>

                                        <div class=" col-sm-12" style="padding-left: 40px; z-index: 60000">
                                            <script type="text/javascript">
                                                Sys.Application.add_load(initdropdown);
                                            </script>
                                            <script type="text/javascript">
                                                Sys.Application.add_load(initdropdown2);
                                            </script>
                                            <script type="text/javascript">
                                                Sys.Application.add_load(initdropdown3);
                                            </script>
                                            <%-- <script type="text/javascript">
                                            Sys.Application.add_load(loadFaculty);
                                            </script>--%>
                                        </div>
                                        <%-- (Visibility Off) Block By Sajib --%>
                                        <div style="float: left; margin-right: 15px; visibility: hidden; height: 0px;">
                                            <div style="float: left;">
                                                <label class="label-width">Add Share Batch</label>
                                            </div>
                                            <div style="float: left;" class="combo-width">
                                                <uc1:BatchUserControl runat="server" ID="ucBatch" OnBatchSelectedIndexChanged="OnBatchSelectedIndexChanged" />
                                            </div>
                                            <div style="float: left;">
                                                <asp:Button ID="btnBatchAdd" runat="server" Text="Add" CssClass="btn-margin" OnClick="btnBatchAdd_Click" />
                                                <asp:Button ID="btnBatchRemove" runat="server" Text="Remove" CssClass="btn-margin" OnClick="btnBatchRemove_Click" />
                                            </div>
                                        </div>
                                        <div style="clear: both;"></div>
                                        <%-- (Visibility Off) Block By Sajib --%>
                                        <div style="float: left; margin: 5px 15px 5px 0px; visibility: hidden; height: 0px;">
                                            <label class="label-width">Share Batch :</label>
                                            <asp:Label ID="lblShareBatch" runat="server"></asp:Label>
                                        </div>
                                        <%-- <div style="clear: both;"></div>--%>
                                        <%-- (Visibility Off) Block By Sajib --%>
                                        <%-- <div style="float: left; height: 100px;">
                                        <div style="float: left">
                                            <label class="label-width">Share Program </label>
                                        </div>
                                        <div style="float: left; margin-left: 5px; height: 80px; width: 617px; overflow-y: auto">
                                            <asp:CheckBoxList ID="chkListShareProgram" runat="server" RepeatColumns="4" Width="600px">
                                            </asp:CheckBoxList>
                                        </div>
                                    </div>--%>

                                        <div style="clear: both;"></div>
                                        <hr />
                                        <%-- <div style="float: left; margin-right: 15px;">
                                        <label class="label-width">Grade Tem.</label>
                                        <asp:DropDownList runat="server" CssClass="combo-width" ID="ddlGradeSheetTemplate" />
                                    </div>--%>
                                        <div style="float: left; margin-right: 15px;">
                                            <label class="label-width-popUp">Basic Grade Template</label><%-- Online Grade Tem. --%><asp:DropDownList runat="server" CssClass="combo-width-sp" ID="ddlExamTemplate"></asp:DropDownList>
                                            <div style="clear: both;"></div>

                                            <div style="float: left; margin-right: 15px;">
                                                <label class="label-width-popUp" style="vertical-align: top;">Remark</label>
                                                <asp:TextBox CssClass="field-width-popUp-textArea" runat="server" ID="txtRemark" TextMode="MultiLine" Height="50px" />
                                            </div>
                                            <div style="clear: both; margin-top: 15px;"></div>

                                            <div>
                                                <div style="padding: 15px;">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Button runat="server" ID="btnUpdateInsert" Text="" Style="width: 156px; height: 30px;" OnClick="btnUpdateInsert_Click" OnClientClick="return MandatoryFieldCheck();" /></td>
                                                            <td>
                                                                <asp:Button runat="server" ID="btnAddAndNext" Text="Insert & Next" Style="width: 156px; height: 30px;" OnClick="btnAddAndNext_Click" OnClientClick="return MandatoryFieldCheck();" /></td>
                                                            <td>
                                                                <asp:Button runat="server" ID="btnCancel" Text="Cancel" Style="width: 150px; height: 30px;" OnClick="btnCancel_Click" /></td>
                                                            <td>
                                                                <asp:Button runat="server" ID="btnCheckConflict" Text="Check" Style="width: 150px; height: 30px; margin-left: 20px" Visible="false" OnClick="btnCheckConflict_Click" /></td>
                                                            <td>
                                                                <asp:CheckBox ID="chkIsConflict" Font-Bold="true" ForeColor="Blue" Checked="false" Text=" Ignore Conflict" Visible="false" runat="server" /></td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                            <div style="height: 40px; margin-left: 120px">
                                                <asp:Label runat="server" ID="lblConflictMessage" Style="color: red;"></asp:Label>
                                            </div>
                                        </div>
                                </fieldset>
                            </div>
                        </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>

