<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true" UnobtrusiveValidationMode="none"
    CodeBehind="StudentManagementNewVersion.aspx.cs" Inherits="EMS.Module.student.StudentManagementNewVersion" %>

<%@ Register Src="~/UserControls/ProgramUserControl.ascx" TagPrefix="uc1" TagName="ProgramUserControl" %>
<%@ Register Src="~/UserControls/AdmissionSessionUserControl.ascx" TagPrefix="uc1" TagName="AdmissionSessionUserControl" %>
<%@ Register Src="~/UserControls/DepartmentUserControl.ascx" TagPrefix="uc1" TagName="DepartmentUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Student Profile
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">


    <link rel="stylesheet" href="https://www.w3schools.com/w3css/4/w3.css">

    <style type="text/css">
        .btn {
            background-image: none !important;
        }

        .oo {
            margin: 0 auto !important;
        }


        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=80);
            opacity: 0.8;
            z-index: 10000;
        }

        .marginTop {
            margin-top: -5px;
        }

        #ctl00_MainContainer_ucDepartment_ddlDepartment, #ctl00_MainContainer_ucProgram_ddlProgram, #ctl00_MainContainer_ddlYearNo, #ctl00_MainContainer_ddlSemesterNo, #ctl00_MainContainer_ucSession_ddlSession,
        #ctl00_MainContainer_ucAcademicSession_ddlSession, #ctl00_MainContainer_ddlHeldIn, #ctl00_MainContainer_ddlHeldIn, #ctl00_MainContainer_ddlCalenderDistribution, #ctl00_MainContainer_ddlAddCourseTrimester, #ctl00_MainContainer_ddlCourse {
            height: 40px !important;
            font-size: 20px;
        }
    </style>

    <style type="text/css">
        .auto-style1 {
            width: 71px;
        }

        .auto-style2 {
            width: 150px;
        }

        .auto-style4 {
            width: 100px;
        }

        .auto-style5 {
            width: 300px;
        }
    </style>

    <script type="text/javascript">

        //function togglediv(id) {
        //    var div = document.getElementById(id);

        //    if (div.style.display !== 'none') {
        //        div.style.display = 'none';
        //    }
        //    else {
        //        div.style.display = 'block';
        //    }

        //    var target = document.getElementById("pnPopUp");
        //    target.style.display = 'block';
        //}

        function InProgress() {
            var panelProg = $get('divProgress');
            panelProg.style.display = '';
        }

        function onComplete() {
            var panelProg = $get('divProgress');
            panelProg.style.display = 'none';
        }
        $(document).ready(function () {

            var index1 = $('#ContentPlaceHolder_ddlExamTypeSecondary option:selected').val();
            if (index1 == '0') {
                $('#ContentPlaceHolder_ddlResultTypeSecondary').attr('disabled', 'disabled');
                $('#ContentPlaceHolder_txtGW4SSecondary').attr('disabled', 'disabled');
                $('#ContentPlaceHolder_txtGPASecondary').attr('disabled', 'disabled');
                $('#ContentPlaceHolder_txtInstituteSecondary').attr('disabled', 'disabled');
                $('#ContentPlaceHolder_txtPassingYearSecondary').attr('disabled', 'disabled');
            }
            else {
                $('#ContentPlaceHolder_ddlResultTypeSecondary').removeAttr('disabled');
                $('#ContentPlaceHolder_txtGW4SSecondary').removeAttr('disabled');
                $('#ContentPlaceHolder_txtGPASecondary').removeAttr('disabled');
                $('#ContentPlaceHolder_txtInstituteSecondary').removeAttr('disabled');
                $('#ContentPlaceHolder_txtPassingYearSecondary').removeAttr('disabled');
            }

            var index2 = $('#ContentPlaceHolder_ddlExamTypeHigherSecondary option:selected').val();
            if (index2 == '0') {
                $('#ContentPlaceHolder_ddlResultTypeHigherSecondary').attr('disabled', 'disabled');
                $('#ContentPlaceHolder_txtGW4SHigherSecondary').attr('disabled', 'disabled');
                $('#ContentPlaceHolder_txtGPAHigherSecondary').attr('disabled', 'disabled');
                $('#ContentPlaceHolder_txtInstituteHigherSecondary').attr('disabled', 'disabled');
                $('#ContentPlaceHolder_txtPassingYearHigherSecondary').attr('disabled', 'disabled');

            }
            else {
                $('#ContentPlaceHolder_ddlResultTypeHigherSecondary').removeAttr('disabled');
                $('#ContentPlaceHolder_txtGW4SHigherSecondary').removeAttr('disabled');
                $('#ContentPlaceHolder_txtGPAHigherSecondary').removeAttr('disabled');
                $('#ContentPlaceHolder_txtInstituteHigherSecondary').removeAttr('disabled');
                $('#ContentPlaceHolder_txtPassingYearHigherSecondary').removeAttr('disabled');
            }

            var index3 = $('#ContentPlaceHolder_ddlExamTypeUndergraduate option:selected').val();
            if (index3 == '0') {
                $('#ContentPlaceHolder_ddlResultTypeUndergraduate').attr('disabled', 'disabled');
                $('#ContentPlaceHolder_txtGW4SUndergraduate').attr('disabled', 'disabled');
                $('#ContentPlaceHolder_txtGPAUndergraduate').attr('disabled', 'disabled');
                $('#ContentPlaceHolder_txtInstituteUndergraduate').attr('disabled', 'disabled');
                $('#ContentPlaceHolder_txtPassingYearUndergraduate').attr('disabled', 'disabled');

            }
            else {
                $('#ContentPlaceHolder_ddlResultTypeUndergraduate').removeAttr('disabled');
                $('#ContentPlaceHolder_txtGW4SUndergraduate').removeAttr('disabled');
                $('#ContentPlaceHolder_txtGPAUndergraduate').removeAttr('disabled');
                $('#ContentPlaceHolder_txtInstituteUndergraduate').removeAttr('disabled');
                $('#ContentPlaceHolder_txtPassingYearUndergraduate').removeAttr('disabled');
            }

            var index4 = $('#ContentPlaceHolder_ddlExamTypeGraduate option:selected').val();
            if (index4 == '0') {
                $('#ContentPlaceHolder_ddlResultTypeGraduate').attr('disabled', 'disabled');
                $('#ContentPlaceHolder_txtGW4SGraduate').attr('disabled', 'disabled');
                $('#ContentPlaceHolder_txtGPAGraduate').attr('disabled', 'disabled');
                $('#ContentPlaceHolder_txtInstituteGraduate').attr('disabled', 'disabled');
                $('#ContentPlaceHolder_txtPassingYearGraduate').attr('disabled', 'disabled');

            }
            else {
                $('#ContentPlaceHolder_ddlResultTypeGraduate').removeAttr('disabled');
                $('#ContentPlaceHolder_txtGW4SGraduate').removeAttr('disabled');
                $('#ContentPlaceHolder_txtGPAGraduate').removeAttr('disabled');
                $('#ContentPlaceHolder_txtInstituteGraduate').removeAttr('disabled');
                $('#ContentPlaceHolder_txtPassingYearGraduate').removeAttr('disabled');
            }
        });

        function ExamTypeSecondary() {
            var index1 = $('#ContentPlaceHolder_ddlExamTypeSecondary option:selected').val();
            if (index1 == '0') {
                $('#ContentPlaceHolder_ddlResultTypeSecondary').attr('disabled', 'disabled');
                $('#ContentPlaceHolder_txtGW4SSecondary').attr('disabled', 'disabled');
                $('#ContentPlaceHolder_txtGPASecondary').attr('disabled', 'disabled');
                $('#ContentPlaceHolder_txtInstituteSecondary').attr('disabled', 'disabled');
                $('#ContentPlaceHolder_txtPassingYearSecondary').attr('disabled', 'disabled');
            }
            else {
                $('#ContentPlaceHolder_ddlResultTypeSecondary').removeAttr('disabled');
                $('#ContentPlaceHolder_txtGW4SSecondary').removeAttr('disabled');
                $('#ContentPlaceHolder_txtGPASecondary').removeAttr('disabled');
                $('#ContentPlaceHolder_txtInstituteSecondary').removeAttr('disabled');
                $('#ContentPlaceHolder_txtPassingYearSecondary').removeAttr('disabled');
            }
        }

        function ExamTypeHigherSecondary() {
            var index2 = $('#ContentPlaceHolder_ddlExamTypeHigherSecondary option:selected').val();
            if (index2 == '0') {
                $('#ContentPlaceHolder_ddlResultTypeHigherSecondary').attr('disabled', 'disabled');
                $('#ContentPlaceHolder_txtGW4SHigherSecondary').attr('disabled', 'disabled');
                $('#ContentPlaceHolder_txtGPAHigherSecondary').attr('disabled', 'disabled');
                $('#ContentPlaceHolder_txtInstituteHigherSecondary').attr('disabled', 'disabled');
                $('#ContentPlaceHolder_txtPassingYearHigherSecondary').attr('disabled', 'disabled');

            }
            else {
                $('#ContentPlaceHolder_ddlResultTypeHigherSecondary').removeAttr('disabled');
                $('#ContentPlaceHolder_txtGW4SHigherSecondary').removeAttr('disabled');
                $('#ContentPlaceHolder_txtGPAHigherSecondary').removeAttr('disabled');
                $('#ContentPlaceHolder_txtInstituteHigherSecondary').removeAttr('disabled');
                $('#ContentPlaceHolder_txtPassingYearHigherSecondary').removeAttr('disabled');
            }
        }

        function ExamTypeUndergraduate() {
            var index3 = $('#ContentPlaceHolder_ddlExamTypeUndergraduate option:selected').val();
            if (index3 == '0') {
                $('#ContentPlaceHolder_ddlResultTypeUndergraduate').attr('disabled', 'disabled');
                $('#ContentPlaceHolder_txtGW4SUndergraduate').attr('disabled', 'disabled');
                $('#ContentPlaceHolder_txtGPAUndergraduate').attr('disabled', 'disabled');
                $('#ContentPlaceHolder_txtInstituteUndergraduate').attr('disabled', 'disabled');
                $('#ContentPlaceHolder_txtPassingYearUndergraduate').attr('disabled', 'disabled');

            }
            else {
                $('#ContentPlaceHolder_ddlResultTypeUndergraduate').removeAttr('disabled');
                $('#ContentPlaceHolder_txtGW4SUndergraduate').removeAttr('disabled');
                $('#ContentPlaceHolder_txtGPAUndergraduate').removeAttr('disabled');
                $('#ContentPlaceHolder_txtInstituteUndergraduate').removeAttr('disabled');
                $('#ContentPlaceHolder_txtPassingYearUndergraduate').removeAttr('disabled');
            }
        }

        function ExamTypeGraduate() {
            var index4 = $('#ContentPlaceHolder_ddlExamTypeGraduate option:selected').val();
            if (index4 == '0') {
                $('#ContentPlaceHolder_ddlResultTypeGraduate').attr('disabled', 'disabled');
                $('#ContentPlaceHolder_txtGW4SGraduate').attr('disabled', 'disabled');
                $('#ContentPlaceHolder_txtGPAGraduate').attr('disabled', 'disabled');
                $('#ContentPlaceHolder_txtInstituteGraduate').attr('disabled', 'disabled');
                $('#ContentPlaceHolder_txtPassingYearGraduate').attr('disabled', 'disabled');

            }
            else {
                $('#ContentPlaceHolder_ddlResultTypeGraduate').removeAttr('disabled');
                $('#ContentPlaceHolder_txtGW4SGraduate').removeAttr('disabled');
                $('#ContentPlaceHolder_txtGPAGraduate').removeAttr('disabled');
                $('#ContentPlaceHolder_txtInstituteGraduate').removeAttr('disabled');
                $('#ContentPlaceHolder_txtPassingYearGraduate').removeAttr('disabled');
            }
        }

        function ResultTypeSecondary() {
            var index = $('#ContentPlaceHolder_ddlResultTypeSecondary option:selected').val();
            if (index == '0') {
                $('#ContentPlaceHolder_txtGW4SSecondary').removeAttr('disabled');
                $('#ContentPlaceHolder_txtGPASecondary').removeAttr('disabled');
            }
            else {
                $('#ContentPlaceHolder_txtGW4SSecondary').attr('disabled', 'disabled');
                $('#ContentPlaceHolder_txtGPASecondary').attr('disabled', 'disabled');
            }
        }
        function ResultTypeHigherSecondary() {
            var index = $('#ContentPlaceHolder_ddlResultTypeHigherSecondary option:selected').val();
            if (index == '0') {
                $('#ContentPlaceHolder_txtGW4SHigherSecondary').removeAttr('disabled');
                $('#ContentPlaceHolder_txtGPAHigherSecondary').removeAttr('disabled');
            }
            else {
                $('#ContentPlaceHolder_txtGW4SHigherSecondary').attr('disabled', 'disabled');
                $('#ContentPlaceHolder_txtGPAHigherSecondary').attr('disabled', 'disabled');
            }
        }
        function ResultTypeUndergraduate() {
            var index = $('#ContentPlaceHolder_ddlResultTypeUndergraduate option:selected').val();
            if (index == '0') {
                $('#ContentPlaceHolder_txtGPAUndergraduate').removeAttr('disabled');
            }
            else {
                $('#ContentPlaceHolder_txtGPAUndergraduate').attr('disabled', 'disabled');
            }
        }
        function ResultTypeGraduate() {
            var index = $('#ContentPlaceHolder_ddlResultTypeGraduate option:selected').val();
            if (index == '0') {
                $('#ContentPlaceHolder_txtGPAGraduate').removeAttr('disabled');
            }
            else {
                $('#ContentPlaceHolder_txtGPAGraduate').attr('disabled', 'disabled');
            }
        }

    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContainer" runat="server">
    <div>
        <div class="PageTitle">
            <label>Student Profile</label>
        </div>

        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <div class="Message-Area">
                    <label class="msgTitle">Message: </label>
                    <asp:Label runat="server" ID="lblMsg" Text="" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>

                <div class="card" style="margin-top: 5px">
                    <div class="card-body">

                        <div class="row">
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <b>Choose Department</b>
                                <br />
                                <uc1:DepartmentUserControl runat="server" ID="ucDepartment" OnDepartmentSelectedIndexChanged="OnDepartmentSelectedIndexChanged" />
                            </div>
                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <b>Choose Program</b>
                                <br />
                                <uc1:ProgramUserControl runat="server" ID="ucProgram" OnProgramSelectedIndexChanged="OnProgramSelectedIndexChanged" class="margin-zero dropDownList" />
                            </div>
                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <b>Choose Year No</b>
                                <asp:DropDownList ID="ddlYearNo" AutoPostBack="true" runat="server" CssClass="form-control" Width="100%" OnSelectedIndexChanged="ddlYearNo_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <b>Choose Semester No</b>
                                <asp:DropDownList ID="ddlSemesterNo" AutoPostBack="true" runat="server" CssClass="form-control" Width="100%" OnSelectedIndexChanged="ddlSemesterNo_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <b>Choose Session</b>
                                <br />
                                <uc1:AdmissionSessionUserControl runat="server" ID="ucSession" OnSessionSelectedIndexChanged="ucSession_SessionSelectedIndexChanged" />
                            </div>

                        </div>

                        <div class="row" style="margin-top: 10px">

                            <div class="col-lg-2 col-md-2 col-sm-2">
                            <b>Student ID</b>
                                <asp:TextBox ID="txtStudentId" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                            </div>

                            <div class="col-lg-2 col-md-2 col-sm-2">
                               <br />
                                <asp:Button ID="btnLoad" runat="server" Text="Load" Width="100%" Height="35px" Style="text-align: center" CssClass="btn-info form-control" OnClick="btnLoad_Click" />
                            </div>
                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <asp:Button ID="btnAddNew" runat="server" Visible="false" CssClass="btn-primary" OnClick="btnAddNew_Click" Text="Add New"></asp:Button>
                            </div>
                        </div>

                    </div>
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>


        <div class="TeacherManagement-container">
            <div>
                <ajaxToolkit:UpdatePanelAnimationExtender
                    ID="UpdatePanelAnimationExtender1"
                    TargetControlID="UpdatePanel3"
                    runat="server">
                    <Animations>
            <OnUpdating>
               <Parallel duration="0">
                    <ScriptAction Script="InProgress();" />
                    <EnableAction AnimationTarget="btnLoad" Enabled="false" />  
                    <EnableAction AnimationTarget="btnSaveBasic" Enabled="false" />                   
                    <EnableAction AnimationTarget="btnParAdd" Enabled="false" />                   
                    <EnableAction AnimationTarget="btnPreAdd" Enabled="false" />                   
                    <EnableAction AnimationTarget="btnSSCAdd" Enabled="false" />                   
                    <EnableAction AnimationTarget="btnHSCAdd" Enabled="false" />                   
                    <EnableAction AnimationTarget="btnPopUpSave" Enabled="false" />                   
                                    
                </Parallel>
            </OnUpdating>
            <OnUpdated>
                <Parallel duration="0">
                    <ScriptAction Script="onComplete();" />
                    <EnableAction   AnimationTarget="btnLoad" Enabled="true" />
                    <EnableAction   AnimationTarget="btnSaveBasic" Enabled="true" />
                    <EnableAction   AnimationTarget="btnParAdd" Enabled="true" />
                    <EnableAction   AnimationTarget="btnPreAdd" Enabled="true" />
                    <EnableAction   AnimationTarget="btnSSCAdd" Enabled="true" />
                    <EnableAction   AnimationTarget="btnHSCAdd" Enabled="true" />
                    <EnableAction   AnimationTarget="btnPopUpSave" Enabled="true" />

                </Parallel>
            </OnUpdated>
                    </Animations>
                </ajaxToolkit:UpdatePanelAnimationExtender>
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <div>
                            <div style="margin-bottom: 5px; margin-top: 5px; float: left; width: 100%;">
                                <div style="float: left;">
                                </div>
                            </div>

                            <div class="Teacher-container">
                                <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
                                <ajaxToolkit:ModalPopupExtender
                                    ID="ModalPopupExtender1"
                                    runat="server"
                                    TargetControlID="btnShowPopup"
                                    PopupControlID="pnPopUp"
                                    CancelControlID="btnClose"
                                    BackgroundCssClass="modalBackground">
                                </ajaxToolkit:ModalPopupExtender>

                                <asp:Panel runat="server" ID="pnPopUp" Style="display: none;">
                                    <div style="height: 700px; width: 1100px; margin: 5px; background-color: Window; overflow: scroll">
                                        <fieldset style="padding: 0px 10px; margin: 5px; border-color: lightgreen;">
                                            <legend>Student Info</legend>
                                            <asp:Button runat="server" ID="btnClose" Text="Close" Style="width: 100px; height: 25pt; float: right" class="button-margin SearchKey w3-button w3-red w3-border w3-border-white w3-round-large" />
                                            <div class=" text-center">
                                                <div class="w-25" style="margin: 0 auto">
                                                    <label style="color: red;"><b>***Less than 200KB</b></label>
                                                    <br />
                                                    <asp:Image runat="server" CssClass="img-fluid" ID="imgPhoto" Height="150" Width="150" />
                                                    <asp:HiddenField runat="server" ID="hfPersonID" Value="" />
                                                    <asp:HiddenField runat="server" ID="hfPhotoPath" Value="" />
                                                    <asp:HiddenField runat="server" ID="hdnStudentId" Value="" />
                                                    <br />
                                                    <br />
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:FileUpload ID="FileUploadPhoto" runat="server" />
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnUpload" runat="server" CssClass="btn-primary" Text="Upload" OnClick="btnUpload_Click" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                            <br />
                                            <br />

                                            <div>
                                                <asp:Label ID="btnBasicInfo" runat="server" Text="Student Basic Information" Width="100%" Style="text-align: center" Height="30pt"
                                                    class="button-margin SearchKey w3-button w3-blue w3-border w3-border-white w3-round-large"> </asp:Label>
                                                <br />
                                                <br />
                                                <div id="item">

                                                    <div class="row">
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>Student ID</b>
                                                            <asp:TextBox ID="txtRoll" Enabled="false" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>Student Name</b>
                                                            <asp:TextBox ID="txtName" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>Registration No</b>
                                                            <asp:TextBox ID="txtreg" Enabled="false" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>Department</b>
                                                            <asp:TextBox ID="txtdept" Enabled="false" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="row" style="margin-top: 10px">
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>Program</b>
                                                            <asp:TextBox ID="txtprogram" Enabled="false" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>DOB</b>
                                                            <asp:TextBox ID="txtDOB" runat="server" placeholder="dd/MM/yyyy" Width="100%" CssClass="form-control"></asp:TextBox>
                                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDOB" Format="dd/MM/yyyy" />
                                                        </div>
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>Gender</b>
                                                            <asp:DropDownList ID="ddlGender" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                                                        </div>
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>Marital Status</b>
                                                            <asp:DropDownList ID="ddlmaritalStatus" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>
                                                        </div>
                                                    </div>

                                                    <div class="row" style="margin-top: 10px">
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>Blood Group</b>
                                                            <asp:DropDownList ID="ddlBloodGroup" runat="server" Width="100%" CssClass="form-control">
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>Religion</b>
                                                            <asp:DropDownList ID="ddlReligion" runat="server" Width="100%" CssClass="form-control">
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>Parents Job</b>
                                                            <asp:DropDownList ID="ddlParentJob" runat="server" Width="100%" CssClass="form-control">
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>Quota</b>
                                                            <asp:DropDownList ID="ddlQuata" runat="server" Width="100%" CssClass="form-control">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>

                                                    <div class="row" style="margin-top: 10px">
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>Phone</b>
                                                            <asp:TextBox ID="txtPhone" runat="server" Width="100%" CssClass="form-control" placeholder="+880XXXXXXXXXX"></asp:TextBox>
                                                        </div>
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>SMS Number</b>
                                                            <asp:TextBox ID="txtSMS" runat="server" Width="100%" CssClass="form-control" placeholder="+880XXXXXXXXXX"></asp:TextBox>
                                                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtSMS" FilterType="Custom" ValidChars="+881234567890" Enabled="true" />
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                                                ControlToValidate="txtSMS" ErrorMessage="Wrong Format"
                                                                Style="z-index: 101; left: 424px; position: absolute; top: 285px"
                                                                ValidationExpression="^(?:\+?88)?01[15-9]\d{8}$" ValidationGroup="check">
                                                            </asp:RegularExpressionValidator>
                                                        </div>
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>Email</b>
                                                            <asp:TextBox ID="txtEmail" runat="server" Width="100%" CssClass="form-control" placeholder="XXXXXX@gmail.com"></asp:TextBox>
                                                        </div>
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>Nationality</b>
                                                            <asp:TextBox ID="txtNationality" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="row" style="margin-top: 10px">
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>Father Name</b>
                                                            <asp:TextBox ID="txtFather" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>Father Phone</b>
                                                            <asp:TextBox ID="txtFatherphone" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>Mother Name</b>
                                                            <asp:TextBox ID="txtMother" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>Mother Phone</b>
                                                            <asp:TextBox ID="txtMotherphone" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="row" style="margin-top: 10px">
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>Guardian Name</b>
                                                            <asp:TextBox ID="txtGuardianName" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>Guardian Phone</b>
                                                            <asp:TextBox ID="txtGuardianPhone" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>Guardian Email</b>
                                                            <asp:TextBox ID="txtGuardianEmail" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>Hall Info</b>
                                                            <asp:DropDownList ID="ddlHallInfo" runat="server" Width="100%" CssClass="form-control">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>

                                                    <div class="row" style="margin-top: 10px">
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>Bank Account No</b>
                                                            <asp:TextBox ID="txtAcNo" runat="server" CssClass="form-control" />
                                                        </div>
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>Bank Name</b>
                                                            <asp:TextBox ID="txtBankName" runat="server" CssClass="form-control" />
                                                        </div>
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>Branch Name</b>
                                                            <asp:TextBox ID="txtBranch" runat="server" CssClass="form-control" />
                                                        </div>
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>Routing No</b>
                                                            <asp:TextBox ID="txtRoutingNo" runat="server" CssClass="form-control" />
                                                        </div>
                                                    </div>

                                                    <div class="row" style="margin-top: 10px">
                                                        <div class="col-lg-9 col-md-9 col-sm-9">
                                                        </div>
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <br />
                                                            <asp:Button runat="server" class="button-margin SearchKey w3-button w3-blue w3-border w3-border-white w3-round-large"
                                                                Style="margin-top: -5px" Width="100%" ID="btnSaveBasic" Text="Save Basic Info" OnClick="btnSaveBasic_Click" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <br />
                                                <asp:Label ID="btnParmanentAddress" runat="server" Text="Student Permanent Address" Width="100%" Style="text-align: center" Height="30pt"
                                                    class="button-margin SearchKey w3-button w3-purple w3-border w3-border-white w3-round-large"></asp:Label>
                                                <br />
                                                <br />
                                                <div id="item1">


                                                    <div class="row" style="margin-top: 10px">
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>Appartment No</b>
                                                            <asp:TextBox ID="txtParApp" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>House No</b>
                                                            <asp:TextBox ID="txtParHouse" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>RoadNo No</b>
                                                            <asp:TextBox ID="txtParRoad" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>Post Code</b>
                                                            <asp:TextBox ID="txtParPostcode" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="row" style="margin-top: 10px">
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>Post Office</b>
                                                            <asp:TextBox ID="txtParPost" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>Police Station</b>
                                                            <asp:TextBox ID="txtParPoliceStation" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>District</b>
                                                            <asp:TextBox ID="txtParDistrict" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>Country</b>
                                                            <asp:TextBox ID="txtParCountry" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="row" style="margin-top: 10px">
                                                        <div class="col-lg-9 col-md-9 col-sm-9">
                                                            <b>Area Info</b>
                                                            <asp:TextBox ID="txtParArea" runat="server" Width="100%" TextMode="MultiLine"></asp:TextBox>
                                                        </div>
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <br />
                                                            <asp:Button runat="server" class="button-margin SearchKey w3-button w3-purple w3-border w3-border-white w3-round-large"
                                                                Style="margin-top: -5px" Width="100%" ID="btnParAdd" Text="Save Permanent Address" OnClick="btnParAdd_Click" />
                                                        </div>
                                                    </div>

                                                </div>
                                                <br />
                                                <asp:Label ID="btnPresentAddress" runat="server" Text="Student Present Address" Width="100%" Style="text-align: center" Height="30pt"
                                                    class="button-margin SearchKey w3-button w3-cyan w3-border w3-border-white w3-round-large"> </asp:Label>
                                                <br />
                                                <br />
                                                <div id="item2">

                                                    <div class="row" style="margin-left: 3px">
                                                        <asp:CheckBox ID="CheckBox1" runat="server" Checked="false" Text="Same As Permanent Address" AutoPostBack="true" ForeColor="Red" Font-Bold="true" OnCheckedChanged="CheckBox1_CheckedChanged" />
                                                    </div>
                                                    <div class="row" style="margin-top: 10px">
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>Appartment No</b>
                                                            <asp:TextBox ID="txtPreApp" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>House No</b>
                                                            <asp:TextBox ID="txtPreHouse" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>RoadNo No</b>
                                                            <asp:TextBox ID="txtPreRoad" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>Post Code</b>
                                                            <asp:TextBox ID="txtPrePostcode" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>


                                                    <div class="row" style="margin-top: 10px">
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>Post Office</b>
                                                            <asp:TextBox ID="txtPrePost" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>Police Station</b>
                                                            <asp:TextBox ID="txtPrePoliceStation" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>District</b>
                                                            <asp:TextBox ID="txtPreDistrict" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>Country</b>
                                                            <asp:TextBox ID="txtPreCountry" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>


                                                    <div class="row" style="margin-top: 10px">
                                                        <div class="col-lg-9 col-md-9 col-sm-9">
                                                            <b>Area Info</b>
                                                            <asp:TextBox ID="txtPreArea" runat="server" Width="100%" TextMode="MultiLine"></asp:TextBox>
                                                        </div>
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <br />
                                                            <asp:Button runat="server" class="button-margin SearchKey w3-button w3-cyan w3-border w3-border-white w3-round-large"
                                                                Style="margin-top: -5px" Width="100%" ID="btnPreAdd" Text="Save Present Address" OnClick="btnPreAdd_Click" />
                                                        </div>
                                                    </div>

                                                    <br />
                                                </div>
                                                <br />
                                                <asp:Label ID="btnSSC" runat="server" Text="Student SSC Information" Width="100%" Style="text-align: center" Height="30pt"
                                                    class="button-margin SearchKey w3-button w3-teal w3-border w3-border-white w3-round-large"> </asp:Label>
                                                <br />
                                                <br />
                                                <div id="item3">

                                                    <div class="row" style="margin-top: 10px">
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>Major</b>
                                                            <asp:TextBox ID="txtSSCMajor" runat="server" Width="100%" CssClass="form-control" placeholder="science"></asp:TextBox>
                                                        </div>
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>Institution</b>
                                                            <asp:TextBox ID="txtSSCInstitution" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>Board</b>
                                                            <asp:TextBox ID="txtSSCBoard" runat="server" Width="100%" CssClass="form-control" placeholder="Dhaka"></asp:TextBox>
                                                        </div>
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>Result</b>
                                                            <asp:TextBox ID="txtSSCResult" runat="server" Width="100%" CssClass="form-control" placeholder="5.0"></asp:TextBox>
                                                        </div>
                                                    </div>





                                                    <div class="row" style="margin-top: 10px">
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>Duration</b>
                                                            <asp:TextBox ID="txtSSCDuration" runat="server" Width="100%" CssClass="form-control" placeholder="2 years"></asp:TextBox>
                                                        </div>
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>Year Of Passing</b>
                                                            <asp:TextBox ID="txtSSCYearOfPassing" runat="server" Width="100%" CssClass="form-control" TextMode="Number" placeholder="2020"></asp:TextBox>
                                                        </div>
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>Session</b>
                                                            <asp:TextBox ID="txtSSCSession" runat="server" Width="100%" CssClass="form-control" placeholder="2019-2020"></asp:TextBox>
                                                        </div>
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <br />
                                                            <asp:Button runat="server" class="button-margin SearchKey w3-button w3-teal w3-border w3-border-white w3-round-large" ID="btnSSCAdd" Text="Save SSC Result"
                                                                Style="width: 100%; margin-top: -5px" OnClick="btnSSCAdd_Click" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <br />
                                                <asp:Label ID="btnHSC" runat="server" Text="Student HSC Information" Width="100%" Style="text-align: center" Height="30pt"
                                                    class="button-margin SearchKey w3-button w3-light-blue w3-border w3-border-white w3-round-large"> </asp:Label>
                                                <br />
                                                <br />
                                                <div id="item4">
                                                    <div class="row" style="margin-top: 10px">
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>Major</b>
                                                            <asp:TextBox ID="txtHSCMajor" runat="server" Width="100%" CssClass="form-control" placeholder="science"></asp:TextBox>
                                                        </div>
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>Institution</b>
                                                            <asp:TextBox ID="txtHSCInstitution" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                                        </div>
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>Board</b>
                                                            <asp:TextBox ID="txtHSCBoard" runat="server" Width="100%" CssClass="form-control" placeholder="Dhaka"></asp:TextBox>
                                                        </div>
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>Result</b>
                                                            <asp:TextBox ID="txtHSCResult" runat="server" Width="100%" CssClass="form-control" placeholder="5.0"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="row" style="margin-top: 10px">
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>Duration</b>
                                                            <asp:TextBox ID="txtHSCDuration" runat="server" Width="100%" CssClass="form-control" placeholder="2 years"></asp:TextBox>
                                                        </div>
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>Year Of Passing</b>
                                                            <asp:TextBox ID="txtHSCYearOfPassing" runat="server" Width="100%" CssClass="form-control" TextMode="Number" placeholder="2020"></asp:TextBox>
                                                        </div>
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <b>Session</b>
                                                            <asp:TextBox ID="txtHSCSession" runat="server" Width="100%" CssClass="form-control" placeholder="2019-2020"></asp:TextBox>
                                                        </div>
                                                        <div class="col-lg-3 col-md-3 col-sm-3">
                                                            <br />
                                                            <asp:Button runat="server" class="button-margin SearchKey w3-button w3-light-blue w3-border w3-border-white w3-round-large" ID="btnHSCAdd" Text="Save HSC Result"
                                                                Style="width: 100%; margin-top: -5px" OnClick="btnHSCAdd_Click" />
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>

                                            <div class="row w-100">
                                                <div class="col-12">
                                                    <div class="row mt-3">
                                                        <div class="col-6 pr-0">
                                                            <asp:Button runat="server" class="btn btn-sm btn-success float-right mt-2 m-0" Style="width: 226px" ID="btnPopUpSave" Text="Update All" ValidationGroup="check" OnClick="btnSave_Click" />
                                                        </div>
                                                        <div class="col-6">
                                                            <asp:Button runat="server" class="btn btn-sm btn-danger mt-2 m-0" Style="width: 226px" ID="btnCancel" Text="Close" />
                                                        </div>
                                                    </div>

                                                </div>
                                            </div>


                                        </fieldset>
                                    </div>
                                </asp:Panel>


                            </div>
                            <div style="clear: both;"></div>


                            <asp:GridView ID="gvStudentList" runat="server" AllowSorting="True" CssClass="table-bordered table"
                                AutoGenerateColumns="False" ShowFooter="True" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                                <HeaderStyle BackColor="#4285f4" ForeColor="White" Height="10px" Font-Bold="True" />
                                <FooterStyle BackColor="#4285f4" ForeColor="White" Height="10px" Font-Bold="True" />
                                <AlternatingRowStyle BackColor="White" />
                                <RowStyle Height="10px" />

                                <Columns>
                                    <asp:TemplateField HeaderText="SL">
                                        <ItemTemplate><b><%# Container.DataItemIndex + 1 %></b></ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                        <HeaderStyle Width="40px" />
                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <div style="text-align: center">

                                                <asp:Image ID="Image1" Height="60px" Width="60px" runat="server" ImageUrl='<%#Eval("History")%>' Style="border-radius: 50%" />
                                            </div>
                                        </ItemTemplate>
                                        <ItemStyle Width="5%" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Roll">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkRoll" runat="server" ForeColor="White">Student</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:HiddenField runat="server" ID="hdnStudentID" Value='<%#Eval("StudentID") %>'></asp:HiddenField>
                                            <asp:Label runat="server" ID="lblStudnetRoll" Font-Bold="true" Text='<%#Eval("Roll") %>'></asp:Label>
                                            <br />
                                            <asp:LinkButton runat="server" ToolTip="Edit" BackColor="#e6fdf7" Font-Size="15px" Font-Bold="true" ID="lnkStudentId" Text='<%#Eval("BasicInfo.FullName") %>' CommandArgument='<%# Eval("StudentID") %>' OnClick="lnkEdit_Click">
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="Father & Mother">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblName1" Text='<%# "Father Name : "+ Eval("BasicInfo.FatherName")+"<br/>"+"Mother Name : "+ Eval("BasicInfo.MotherName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="left" />
                                    </asp:TemplateField>

                                    <%--<asp:TemplateField HeaderText="Registration No" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblName2" Text='<%#Eval("StudentAdditionalInformation.RegistrationNo") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle Width="100px" />
                                        </asp:TemplateField>--%>

                                    <asp:TemplateField HeaderText="Phone & Email">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblPhone" Text='<%#Eval("BasicInfo.Phone")+"<br>"+Eval("BasicInfo.Email")==null ? "" : Eval("BasicInfo.Email") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Year & Semester">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblName3" Text='<%# "Year : "+Eval("Attribute1")+"<br/>"+"Semester : "+Eval("Attribute2") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="Session">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblName4" Text='<%#Eval("AdmissionSession") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <%--<asp:TemplateField HeaderText="Current Session" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblCurrent" Text='<%#Eval("CurrentSession") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle Width="100px" />
                                        </asp:TemplateField>--%>

                                    <asp:TemplateField HeaderText="Hall">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblName5" Text='<%#Eval("Attribute3") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="left" />
                                    </asp:TemplateField>


                                    <%-- <asp:TemplateField HeaderText="Max Advised Number">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblAdvNumber" Text='<%#Eval("MaxNoTobeAdvised") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="100px" />
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <div style="text-align: center">
                                                <asp:LinkButton runat="server" ToolTip="Edit" ID="lnkEdit" CssClass="btn-primary btn-sm w-50" CommandArgument='<%# Eval("StudentID") %>' OnClick="lnkEdit_Click">
                                                         <strong><i class="fas fa-pencil-alt"></i>&nbsp;Edit</strong>
                                                </asp:LinkButton>
                                            </div>
                                        </ItemTemplate>
                                        <HeaderStyle Width="100px" />
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
                    </ContentTemplate>
                    <Triggers>

                        <asp:PostBackTrigger ControlID="btnUpload" />

                    </Triggers>
                </asp:UpdatePanel>
                <div id="divProgress" style="display: none; z-index: 1000; position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%);">
                    <asp:Image ID="LoadingImage" runat="server" ImageUrl="~/Images/Img/Waiting.gif" Height="150px" Width="150px" />
                </div>

            </div>

        </div>
    </div>

</asp:Content>
