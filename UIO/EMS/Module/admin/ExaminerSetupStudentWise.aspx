<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true" CodeBehind="ExaminerSetupStudentWise.aspx.cs" Inherits="EMS.Module.admin.ExaminerSetupStudentWise" %>


<%@ Register Src="~/UserControls/AdmissionSessionUserControl.ascx" TagPrefix="uc1" TagName="AdmissionSessionUserControl" %>
<%@ Register Src="~/UserControls/ProgramUserControl.ascx" TagPrefix="uc1" TagName="ProgramUserControl" %>
<%@ Register Src="~/UserControls/DepartmentUserControl.ascx" TagPrefix="uc1" TagName="DepartmentUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="Server">
    Examiner Setup Student Wise
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="Server">
    <style type="text/css">
        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=80);
            opacity: 0.8;
            z-index: 10000;
        }

        .marginTop {
            margin-top: -5px;
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
    <link href="../../CSS/select2.min.css" rel="stylesheet" />
    <script src="../../JavaScript/select2.min.js"></script>
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

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContainer" runat="Server">

    <div class="container-fluid">
        <asp:UpdatePanel ID="UpdatePanel4" class="row" runat="server">
            <ContentTemplate>
                <div class="col-md-12">
                    <div class="h2 text-primary pt-2 border-bottom">Examiner Setup Student Wise</div>
                </div>
                <div class="col-md-12">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:Panel ID="pnlMessage" runat="server" Visible="true" CssClass="card">
                                <div class="card-body">
                                    <asp:Label ID="Label1" runat="server" Text="Message : " Font-Bold="true"></asp:Label>
                                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="col-md-12 mt-3">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="card" style="height: 187px;">
                                        <div class="card-body">
                                            <div class="form-row">
                                                <div class="col-md-12 row">
                                                    <div class="col-4 mt-2">Department : </div>
                                                    <div class="col-8 mb-2">
                                                        <uc1:DepartmentUserControl runat="server" ID="ucDepartment" OnDepartmentSelectedIndexChanged="OnDepartmentSelectedIndexChanged" />
                                                    </div>

                                                </div>
                                                <div class="col-md-12 row">
                                                    <div class="col-4 mt-2">Program : </div>
                                                    <div class="col-8 mb-2">
                                                        <uc1:ProgramUserControl runat="server" ID="ucProgram" OnProgramSelectedIndexChanged="OnProgramSelectedIndexChanged" />

                                                    </div>
                                                </div>
                                                <div class="col-md-12 row">
                                                    <div class="col-4 mt-2">Session : </div>
                                                    <div class="col-8 mb-2">
                                                        <uc1:AdmissionSessionUserControl runat="server" ID="ucCurrentSession" class="margin-zero dropDownList" />

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="card">
                                        <div class="card-body">
                                            <div class="form-row">
                                                <div class="col-md-12 row">
                                                    <div class="col-4 mt-2">
                                                        <asp:Label ID="Label8" runat="server" Text="Year No : "></asp:Label>
                                                    </div>
                                                    <div class="col-8">
                                                        <asp:DropDownList ID="ddlYearNo" Width="100%" CssClass="form-control" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlYearNo_SelectedIndexChanged"></asp:DropDownList>

                                                    </div>
                                                </div>


                                                <div class="col-md-12 row">
                                                    <div class="col-4 mt-2">
                                                        <asp:Label ID="Label9" runat="server" Text="Semester No : "></asp:Label>
                                                    </div>
                                                    <div class="col-8 mb-2">
                                                        <asp:DropDownList ID="ddlSemesterNo" Width="100%" CssClass="form-control" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlSemesterNo_SelectedIndexChanged"></asp:DropDownList>

                                                    </div>
                                                </div>
                                                <div class="col-md-12 row">
                                                    <div class="col-4 mt-2">
                                                        <asp:Label ID="Label10" runat="server" Text="Exam : "></asp:Label>
                                                    </div>
                                                    <div class="col-8 mb-2">
                                                        <asp:DropDownList ID="ddlExam" Width="100%" CssClass="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlExam_SelectedIndexChanged"></asp:DropDownList>

                                                    </div>
                                                </div>
                                                <div class="col-md-12 row">
                                                    <div class="col-4 mt-2">Course: </div>
                                                    <div class="col-8 mb-2">
                                                        <asp:DropDownList ID="ddlCourse" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCourse_SelectedIndexChanged" Width="100%" CssClass="form-control">
                                                        </asp:DropDownList>

                                                    </div>
                                                </div>
                                                <div class="col-md-12 row">
                                                    <div class="col-4 mt-2">
                                                        <asp:Button ID="btnLoad" runat="server" Visible="false" OnClick="btnLoad_Click" Text="Load" CssClass="btn btn-sm w-100 btn-primary" />
                                                         
                                                        <%--CommandArgument='<%#Eval("ExaminerSetupStudentWiseId")  %>'--%>

                                                    </div>
                                                    <div class="col-8 mt-2">
                                                        <div id="divProgress" style="display: none; float: right; z-index: 1000;">
                                                            <div style="float: left">
                                                                <asp:Image ID="LoadingImage" runat="server" ImageUrl="~/Images/Img/Waiting.gif" Height="35px" Width="35px" />
                                                            </div>
                                                        </div>
                                                    </div>

                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div>
                                <table>
                                    <tr>
                                        <td><asp:Label ID="Label2" runat="server" Text="Select student "></asp:Label></td>
                                        <td><asp:Label ID="Label22" runat="server" Text="From: "></asp:Label></td>
                                        <td><asp:DropDownList ID="ddlRangeFrom" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlRangeFrom_OnSelectedIndexChanged"></asp:DropDownList></td>
                                        <td><asp:Label ID="Label222" runat="server" Text="To: "></asp:Label></td>
                                        <td><asp:DropDownList ID="ddlRangeTo" runat="server"></asp:DropDownList></td>
                                        <td><asp:Button ID="btnCheckRange" runat="server" Text="check Range" OnClick="btnCheckRange_OnClick"/></td>
                                        
                                    </tr>
                                </table>
                                
                            </div>

                            <asp:Button ID="btnExaminer1" runat="server" Text="Assign Examiner"  OnClick="btnExaminer_Click" />


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



                <%--<div style="float: Right; margin-right: 25px;">
                        <asp:Button ID="ButtonForward" runat="server" Visible="false" Enabled="true" CssClass="btn-success" Height="30px" Width="150px" Text="Submit" OnClick="ButtonForward_Click" OnClientClick=" return confirm('Are you sure you want to Submit to Head of Department?');" />
                    </div>
                    <div style="float: Right; margin-right: 15px;">
                        <asp:Button ID="ButtonApprove" runat="server" Visible="false" Enabled="true" CssClass="btn-success" Height="30px" Width="150px" Text="Approve" OnClick="btnApprove_Click" OnClientClick=" return confirm('Are you sure you want to Complete Student Registration?');" />
                    </div>--%>

                <div class="col-md-12">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="card mt-2">
                                <div class="card-body">
                                    <asp:GridView ID="gvStudentList" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3" CssClass="table table-bordered" EmptyDataText="No data found." Width="100%">

                                        <HeaderStyle BackColor="#4285f4" ForeColor="White" Font-Bold="True" HorizontalAlign="Center" />
                                        <AlternatingRowStyle BackColor="#FFFFCC" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="SL" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate><b><%# Container.DataItemIndex + 1 %></b></ItemTemplate>
                                                <HeaderStyle Width="5%" HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ID" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblAcaCalSecID" Font-Bold="true" Text='<%#Eval("AcaCalSectionId") %>'></asp:Label>
                                                    <asp:Label runat="server" ID="lblCourseHistoryId" Font-Bold="true" Text='<%#Eval("StudentCourseHistoryId") %>'></asp:Label>
                                                    <asp:Label runat="server" ID="lblExamSetupDetailId" Font-Bold="true" Text='<%#Eval("ExamSetupDetailId") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle Width="100px" />
                                            </asp:TemplateField>
                                              <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <%--<asp:CheckBox ID="chkAll" runat="server" Text="Select All" Font-Bold="true" AutoPostBack="true" ></asp:CheckBox>--%>
                                                        <%--OnCheckedChanged="chkAll_CheckedChanged"--%>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <div style="text-align: center">
                                                            <%--<asp:HiddenField ID="hiddenStudentId" runat="server" Value='<%#Eval("ID") %>' />--%>
                                             <asp:CheckBox runat="server" ID="ChkActive"></asp:CheckBox>
                                                        </div>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="5%" HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            


                                            <asp:TemplateField HeaderText="Student Name">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblStudentName" Font-Bold="true" Text='<%#Eval("StudentName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle Width="30%" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField HeaderText="Roll">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblRoll" Font-Bold="true" Text='<%#Eval("Roll") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle Width="30px" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="First Examiner">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblFirstExaminer" Font-Bold="true" Text='<%#Eval("FirstExaminer") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle Width="15%" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Second Examiner">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblSecondExaminer" Font-Bold="true" Text='<%#Eval("SecondExaminer") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle Width="15%" />
                                                 <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Third Examiner">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblThirdExaminor" Font-Bold="true" Text='<%#Eval("ThirdExaminer") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle Width="15%" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Exam Name">
                                                  
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblExamName" Font-Bold="true" Text='<%#Eval("ExamName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle Width="15%" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <%--<asp:TemplateField HeaderText="Assign Examiner">

                                                <ItemTemplate>
                                                    <asp:Button ID="btnExaminer" runat="server" Text="Assign Examiner" CommandArgument='<%#Eval("ExaminerSetupStudentWiseId")  %>' OnClick="btnExaminer_Click" />
                                                </ItemTemplate>
                                                <HeaderStyle Width="15%" HorizontalAlign="Center" />
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
                                                </asp:TemplateField>--%>
                                        </Columns>
                                        <RowStyle Height="25px" VerticalAlign="Middle" HorizontalAlign="Left" />
                                        <EmptyDataTemplate>
                                            No data found!
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                    <div>
                                        <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
                                        <ajaxToolkit:ModalPopupExtender
                                            ID="ModalPopupExtender1"
                                            runat="server"
                                            TargetControlID="btnShowPopup"
                                            PopupControlID="pnPopUp"
                                            CancelControlID="btnCancel"
                                            BackgroundCssClass="modalBackground">
                                        </ajaxToolkit:ModalPopupExtender>
                                        

                                        <asp:Panel runat="server" ID="pnPopUp" Style="display: none; background-color: #CCCCCC;">
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                <ContentTemplate>
                                                    <div class="updatePanel-Wrapper" style="width: 487px; padding: 5px; margin: 5px;">
                                                        <fieldset style="padding: 10px; margin: 5px; border-color: lightgreen;">
                                                            <legend>Assign Examiner</legend>
                                                            <div class="StudentCourseHistory-container">
                                                                <div style="float: left; width: 100%;">
                                                                    <div style="float: left">
                                                                        <fieldset style="padding: 10px; margin: 5px; border-color: lightgreen; width: 100%;">
                                                                            
                                                                           <asp:Label ID="lblGroupRoll" runat="server" ></asp:Label>
                                                                               <br/>  
                                                                            <table>
                                                                                <asp:Label ID="lblID" runat="server"  class="margin-zero input-Size"></asp:Label>
                                                                                
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="lblCourseSection" runat="server" Text="Course Section"></asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Label ID="lblCourseSectionName" runat="server" ></asp:Label>
                                                                                        <%--<asp:TextBox ID="TxtSection" Enabled="False" Width="100%"  runat="server"></asp:TextBox>--%>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <label style="color: blue">Department-1</label>
                                                                                        <td>
                                                                                            <uc1:DepartmentUserControl runat="server"  ID="DepartmentUserControl1" OnDepartmentSelectedIndexChanged="DepartmentUserControl1_DepartmentSelectedIndexChanged" />
                                                                                        </td>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="lblEx1" runat="server" Text="First Examiner"></asp:Label>
                                                                                    </td>
                                                                                    <td class="auto-style9">
                                                                                        <script type="text/javascript">

                                                                                            $(document).ready(function () {
                                                                                                initdropdownExamCommitteeChairman();

                                                                                            });

                                                                                            function initdropdownExamCommitteeChairman() {
                                                                                                $("#ctl00_MainContainer_ddlEx1").select2({ placeholder: { id: '0', text: '-Select-' }, allowClear: true });
                                                                                            }
                                                                                        </script>
                                                                                        <script type="text/javascript">
                                                                                            Sys.Application.add_load(initdropdownExamCommitteeChairman);
                                                                                        </script>
                                                                                        <asp:DropDownList ID="ddlEx1" runat="server"  class="dropdown"></asp:DropDownList>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <label style="color: blue">Department-2</label>
                                                                                        <td>
                                                                                            <uc1:DepartmentUserControl runat="server" ID="DepartmentUserControl2" OnDepartmentSelectedIndexChanged="DepartmentUserControl2_DepartmentSelectedIndexChanged" />
                                                                                            <%--OnDepartmentSelectedIndexChanged="DepartmentUserControl2_DepartmentSelectedIndexChanged"--%>
                                                                                        </td>
                                                                                    </td>

                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="lblEx2" runat="server" Text="Second Examiner"></asp:Label>
                                                                                    </td>
                                                                                    <td class="auto-style9">
                                                                                        <script type="text/javascript">

                                                                                            $(document).ready(function () {
                                                                                                initdropdownExamCommitteeChairman();

                                                                                            });

                                                                                            function initdropdownExamCommitteeChairman() {
                                                                                                $("#ctl00_MainContainer_ddlEx2").select2({ placeholder: { id: '0', text: '-Select-' }, allowClear: true });
                                                                                            }
                                                                                        </script>
                                                                                        <script type="text/javascript">
                                                                                            Sys.Application.add_load(initdropdownExamCommitteeChairman);
                                                                                        </script>
                                                                                        <asp:DropDownList ID="ddlEx2" runat="server" class="dropdown"></asp:DropDownList>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <label style="color: blue">Department-3</label>
                                                                                        <td>
                                                                                            <uc1:DepartmentUserControl runat="server" ID="DepartmentUserControl3" OnDepartmentSelectedIndexChanged="DepartmentUserControl3_DepartmentSelectedIndexChanged" />
                                                                                            <%--OnDepartmentSelectedIndexChanged="DepartmentUserControl3_DepartmentSelectedIndexChanged" --%>
                                                                                        </td>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="lblEx3" runat="server" Text="Third Examiner"></asp:Label>
                                                                                    </td>
                                                                                    <td class="auto-style9">
                                                                                        <script type="text/javascript">

                                                                                            $(document).ready(function () {
                                                                                                initdropdownExamCommitteeChairman();

                                                                                            });

                                                                                            function initdropdownExamCommitteeChairman() {
                                                                                                $("#ctl00_MainContainer_ddlEx3").select2({ placeholder: { id: '0', text: '-Select-' }, allowClear: true });
                                                                                            }
                                                                                        </script>
                                                                                        <script type="text/javascript">
                                                                                            Sys.Application.add_load(initdropdownExamCommitteeChairman);
                                                                                        </script>
                                                                                        <asp:DropDownList ID="ddlEx3" runat="server" class="dropdown"></asp:DropDownList>
                                                                                    </td>
                                                                                </tr>



                                                                            </table>

                                                                        </fieldset>
                                                                        <div style="clear: both;"></div>
                                                                        <div style="margin-top: 10px; margin-bottom: 5px;">
                                                                            <asp:Button runat="server" ID="btnUpdte" Text="Insert/Update" Style="width: 150px; height: 30px;" OnClick="btnUpdte_Click" />
                                                                            <asp:Button runat="server" ID="btnCancel" Text="Cancel" Style="width: 150px; height: 30px;" OnClick="btnCancel_Click" />
                                                                        </div>
                                                                    </div>

                                                                </div>
                                                            </div>
                                                        </fieldset>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </asp:Panel>
                                    </div>

                                    <div>
                                        <asp:Button ID="btnShowPopup2" runat="server" Style="display: none" />
                                        <ajaxToolkit:ModalPopupExtender
                                            ID="ModalPopupExtender2"
                                            runat="server"
                                            TargetControlID="btnShowPopup2"
                                            PopupControlID="pnPopUp2"
                                            CancelControlID="btnCancel"
                                            BackgroundCssClass="modalBackground">
                                        </ajaxToolkit:ModalPopupExtender>
                                        <asp:Panel runat="server" ID="pnPopUp2" Style="display: none; background-color: #CCCCCC;">
                                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                <ContentTemplate>
                                                    <div class="updatePanel-Wrapper" style="width: 400px; padding: 5px; margin: 5px;">
                                                        <fieldset style="padding: 10px; margin: 5px; border-color: lightgreen;">
                                                            <legend>Assign Exam</legend>
                                                            <div class="StudentCourseHistory-container">
                                                                <div style="float: left; width: 100%;">
                                                                    <div style="float: left">
                                                                        <fieldset style="padding: 10px; margin: 5px; border-color: lightgreen; width: 323px;">
                                                                            <table>
                                                                                <asp:Label ID="lblID1" runat="server" Visible="false" class="margin-zero input-Size"></asp:Label>

                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="lblExam" runat="server" Text="Assign Exam"></asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:DropDownList ID="ddlExam1" runat="server"></asp:DropDownList>
                                                                                    </td>
                                                                                </tr>




                                                                            </table>

                                                                        </fieldset>
                                                                        <div style="clear: both;"></div>
                                                                        <div style="margin-top: 10px; margin-bottom: 5px;">
                                                                            <%--<asp:Button runat="server" ID="btnExamAssin" Text="Update" Style="width: 150px; height: 30px;" OnClick="btnExamAssin_Click" />
                                                                                    <asp:Button runat="server" ID="btnExamcancel" Text="Cancel" Style="width: 150px; height: 30px;" OnClick="btnExamcancel_Click" />--%>
                                                                        </div>
                                                                    </div>

                                                                </div>
                                                            </div>
                                                        </fieldset>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </asp:Panel>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

</asp:Content>


