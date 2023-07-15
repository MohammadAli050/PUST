<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true" UnobtrusiveValidationMode="none" 
    CodeBehind="StudentManagement.aspx.cs" Inherits="EMS.Module.student.StudentManagement" %>

<%@ Register Src="~/UserControls/ProgramUserControl.ascx" TagPrefix="uc1" TagName="ProgramUserControl" %>
<%@ Register Src="~/UserControls/AdmissionSessionUserControl.ascx" TagPrefix="uc1" TagName="AdmissionSessionUserControl" %>
<%@ Register Src="~/UserControls/DepartmentUserControl.ascx" TagPrefix="uc1" TagName="DepartmentUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Student Profile
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">



    <style type="text/css">
        .btn{
            background-image:none !important
        }

        .oo{
            margin:0 auto!important;
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
                <div class="Message-Area">
                     <table id="Table1" style="padding: 5px; width: 100%; height: 70px;" border="0" runat="server">
                        <tr>
                            <td class="auto-style4">Department : </b></td>
                            <td class="auto-style2">
                                <uc1:DepartmentUserControl runat="server" ID="ucDepartment" OnDepartmentSelectedIndexChanged="OnDepartmentSelectedIndexChanged" />
                           </td>
                            <td class="auto-style4"><b>Program : </b></td>
                            <td class="auto-style2">
                                <uc1:ProgramUserControl runat="server" ID="ucProgram" OnProgramSelectedIndexChanged="OnProgramSelectedIndexChanged" class="margin-zero dropDownList" />
                            </td>           
                        </tr>
                        <tr>
                            <td class="auto-style4"><asp:Label ID="Label8" runat="server" Text="Year No :"></asp:Label></td>
                            <td class="auto-style2">
                                <asp:DropDownList ID="ddlYearNo" Width="180" AutoPostBack="true" runat="server" ></asp:DropDownList>
                            </td>
                            <td class="auto-style4"><asp:Label ID="Label9" runat="server" Text="Semester No :"></asp:Label></td>
                            <td class="auto-style2">
                                <asp:DropDownList ID="ddlSemesterNo" Width="180"  AutoPostBack="true"  runat="server" ></asp:DropDownList>
                            </td>
                            <td class="auto-style4"><asp:Label ID="Label1" Width="120" runat="server" Text="Current Session :"></asp:Label></td>
                            <td class="auto-style2">
                                 <uc1:AdmissionSessionUserControl runat="server" ID="ucSession" class="margin-zero dropDownList"/>
                            </td>
                            <td class="auto-style2">
                                <asp:Button ID="btnLoad" runat="server" Text="Load" class="margin-zero btn-size" OnClick="btnLoad_Click" />
                            </td>
                         </tr>
                    </table>
                </div>
                <div class="loadedArea">
                    <asp:Button ID="btnAddNew" runat="server" Visible="false" class="margin-zero btn-size1" OnClick="btnAddNew_Click" Text="Add New"></asp:Button>
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
                                    <div style="height: 500px; width: 1050px; margin: 5px; background-color: Window; overflow: scroll">
                                        <fieldset style="padding: 0px 10px; margin: 5px; border-color: lightgreen;">
                                            <legend>Student Info</legend>                                           
                                            <div style="padding: 0px 5px; width: 100%">
                                                <div class="TeacherManagement-container">
                                                    <div class="row">

                                                        <div class="col-12 text-center">
                                                            <div class="w-25" style="margin: 0 auto">
                                                                <asp:Image runat="server" CssClass="img-fluid" ID="imgPhoto" Height="100" Width="100" />
                                                                <asp:HiddenField runat="server" ID="hfPersonID" Value="" />
                                                                <asp:HiddenField runat="server" ID="hfPhotoPath" Value="" />
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-12 mt-2">

                                                                    <asp:FileUpload CssClass="form-control w-25 oo " ID="FileUploadPhoto" runat="server" />

                                                                </div>
                                                                <div class="col-12">
                                                                    <asp:Button ID="btnUpload" runat="server" Style="margin-left: -3px;" CssClass="btn btn-sm btn-primary mt-2" Text="Upload" OnClick="btnUpload_Click" />
                                                                    <label>***Less than 200KB</label>
                                                                </div>

                                                            </div>
                                                        </div>

                                                        <div class="col-3 mt-2">
                                                            <label class="">Full Name</label>
                                                        </div>
                                                        <div class="col-3 mt-2">
                                                            <asp:TextBox runat="server" ID="txtName" class="form-control" Width="" />                                                            
                                                        </div>
                                                        <div class="col-3 mt-2">
                                                            <label class="">Student Roll</label>

                                                        </div>
                                                        <div class="col-3 mt-2">
                                                            <asp:TextBox runat="server" ID="txtStudentRoll" class="form-control" />
                                                            <asp:Button runat="server" ID="btnValidate" Text="Check" OnClick="btnValidate_Click" />
                                                            <asp:Label runat="server" ID="lblValidationStat" class="margin-zero" />
                                                            <asp:HiddenField ID="hfStudentRollChanged" Value="0_0" runat="server" />
                                                        </div>
                                                        <div class="col-3 mt-2">
                                                            <label class="">Father's Name</label>

                                                        </div>
                                                        <div class="col-3 mt-2">
                                                            <asp:TextBox runat="server" ID="txtFatherName" class="form-control" Width="" />

                                                        </div>

                                                        <div class="col-3 mt-2">
                                                            <label class="">Mother's Name</label>

                                                        </div>
                                                        <div class="col-3 mt-2">
                                                            <asp:TextBox runat="server" ID="txtMotherName" class="form-control" Width="" />                                                           
                                                        </div>

                                                        <div class="col-3 mt-2">
                                                            <label class="">Program</label>

                                                        </div>
                                                        <div class="col-3 mt-2">
                                                            <asp:DropDownList runat="server" ID="ddlProgram" CssClass="form-control" OnSelectedIndexChanged="OnPopupProgramSelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>                                                            
                                                        </div>
                                                        <div class="col-3 mt-2">
                                                            <label class="">Session</label>

                                                        </div>
                                                        <div class="col-3 mt-2">

                                                            <asp:DropDownList runat="server" ID="ddlSession" class=" form-control"></asp:DropDownList>
                                                          
                                                        </div>
                                                        <div class="col-3 mt-2">
                                                            <label class="">DOB</label>

                                                        </div>
                                                        <div class="col-3 mt-2">
                                                            <asp:TextBox runat="server" ID="txtDob" class="form-control datepicker" placeholder="Date" DataFormatString="{0:dd/MM/yyyy}" />
                                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDob" Format="dd/MM/yyyy" />
                                                        </div>
                                                        <div class="col-3 mt-2">
                                                            <label class="">Nationality</label>
                                                        </div>
                                                        <div class="col-3 mt-2">
                                                            <asp:TextBox runat="server" ID="txtNationality" class="form-control" />

                                                        </div>
                                                        <div class="col-3 mt-2">
                                                            <label class="">Gender</label>

                                                        </div>
                                                        <div class="col-3 mt-2">
                                                            <asp:DropDownList ID="ddlGender" runat="server" class="form-control" AutoPostBack="false" EnableViewState="true"></asp:DropDownList>
                                                        </div>
                                                        <div class="loadedArea d-none">

                                                            <%-- <label class="display-inline field-Title3">Campus</label>
                                                                <asp:DropDownList runat="server" ID="ddlCampus" class="margin-zero dropDownList"></asp:DropDownList>--%>
                                                        </div>
                                                        <div class="col-3 mt-2">
                                                            <label class="">Marital Status</label>
                                                        </div>
                                                        <div class="col-3 mt-2">
                                                            <asp:DropDownList ID="ddlMaritalStat" class="form-control" runat="server" AutoPostBack="false" EnableViewState="true"></asp:DropDownList>

                                                        </div>
                                                        <div class="col-3 mt-2">
                                                            <label class="">Religion</label>

                                                        </div>
                                                        <div class="col-3 mt-2">
                                                            <asp:DropDownList ID="ddlReligion" runat="server" class="form-control" AutoPostBack="false" EnableViewState="true"></asp:DropDownList>

                                                        </div>
                                                        <div class="col-3 mt-2">
                                                            <asp:HiddenField ID="hdnMailing" Value="0_0" runat="server" />
                                                            <label class="">Mailing Address</label>
                                                        </div>
                                                        <div class="col-3 mt-2">
                                                            <asp:TextBox ID="txtMailingAddress" runat="server" class="form-control" />

                                                        </div>
                                                        <div class="col-3 mt-2">
                                                            <asp:HiddenField ID="hdnContact" Value="0_0" runat="server" />
                                                            <label class="">Phone</label>
                                                        </div>
                                                        <div class="col-3 mt-2">
                                                            <asp:TextBox ID="txtPhone" runat="server" class="form-control" />
                                                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtPhone" FilterType="Custom" ValidChars="+881234567890" Enabled="true" />
                                                        </div>
                                                        <div class="col-3 mt-2">
                                                            <label class="">SMS Contact No</label>

                                                        </div>
                                                        <div class="col-3 mt-2">
                                                            <asp:TextBox ID="txtSMSContact" runat="server" class="form-control" placeholder="+880XXXXXXXXXX" />
                                                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtSMSContact" FilterType="Custom" ValidChars="+881234567890" Enabled="true" />
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                                                ControlToValidate="txtSMSContact" ErrorMessage="Wrong Format"
                                                                Style="z-index: 101; left: 424px; position: absolute; top: 285px"
                                                                ValidationExpression="^(?:\+?88)?01[15-9]\d{8}$" ValidationGroup="check">
                                                            </asp:RegularExpressionValidator>                                                            
                                                        </div>
                                                        <div class="col-3 mt-2">
                                                            <label class="">Guardian Phone</label>

                                                        </div>
                                                        <div class="col-3 mt-2">
                                                            <asp:TextBox ID="txtPhnGuardian" runat="server" class="form-control" />
                                                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtPhnGuardian" FilterType="Custom" ValidChars="+881234567890" Enabled="true" />
                                                        </div>
                                                        <div class="col-3 mt-2">
                                                            <label class="">Email</label>

                                                        </div>
                                                        <div class="col-3 mt-2">
                                                            <asp:TextBox ID="txtEmailPersonal" runat="server" class="form-control" />

                                                        </div>

                                                        <div class="row w-100">
                                                            <div class="col-12">
                                                                <div class="row mt-3">
                                                                    <div class="col-6 pr-0">
                                                                        <asp:Button runat="server" class="btn btn-sm btn-success float-right mt-2 m-0" style="width: 226px" ID="btnPopUpSave" Text="Save" ValidationGroup="check" OnClick="btnSave_Click" />
                                                                    </div>
                                                                    <div class="col-6">
                                                                        <asp:Button runat="server" class="btn btn-sm btn-danger mt-2 m-0" style="width: 226px" ID="btnCancel" Text="Cancel" />
                                                                    </div>
                                                                </div>
                                                            
                                                            </div>
                                                        </div>
                                                        

                                                    </div>
                                                    <%--<div class="loadedArea">
                                                            <label class="display-inline field-Title2">Employee Type</label>
                                                            <asp:DropDownList runat="server" ID="ddlEmployeeType" Width="150px" class="margin-zero dropDownList"></asp:DropDownList>
                                                        </div>--%>
                                                    <%--This part not showing--%>
                                                    <div style="display:none;">
                                                            <div class="loadedArea">
                                                            </div>
                                                            <div class="loadedArea loadedArea-divided"></div>
                                                            <div class="loadedArea loadedArea-Font-Size">
                                                                <label class="display-inline field-Title41">Exam</label>

                                                                <label class="display-inline field-Title41">Board</label>

                                                                <label class="display-inline field-Title42">Result</label>

                                                                <label class="display-inline field-Title43">GPA(without 4 sub)</label>

                                                                <label class="display-inline field-Title44">GPA</label>

                                                                <label class="display-inline field-Title45">School / College / University Name</label>

                                                                <label class="display-inline field-Title42">Year</label>
                                                            </div>
                                                            <div class="loadedArea loadedArea-Font-Size">
                                                                <asp:HiddenField ID="hdnSecondary" Value="0_0" runat="server" />

                                                                <asp:DropDownList ID="ddlExamTypeSecondary" runat="server" class="margin-zero dropDownList41" AutoPostBack="false" onchange="ExamTypeSecondary();" />

                                                                <asp:DropDownList ID="ddlBoardSecondary" runat="server" class="margin-zero dropDownList41" AutoPostBack="false" />

                                                                <asp:DropDownList ID="ddlResultTypeSecondary" runat="server" class="margin-zero dropDownList42" onchange="ResultTypeSecondary();">
                                                                    <%--AutoPostBack="true" OnSelectedIndexChanged="ResultTypeSecondary_Change" >--%>
                                                                    <asp:ListItem Value="0">GPA</asp:ListItem>
                                                                    <asp:ListItem Value="1">1st Division</asp:ListItem>
                                                                    <asp:ListItem Value="2">2nd Division</asp:ListItem>
                                                                    <asp:ListItem Value="3">3rd Division</asp:ListItem>
                                                                </asp:DropDownList>

                                                                <asp:TextBox ID="txtGW4SSecondary" runat="server" class="margin-zero input-Size41" />
                                                                <ajaxToolkit:FilteredTextBoxExtender ID="filterRW4SSecondary" runat="server" TargetControlID="txtGW4SSecondary" FilterType="Custom" ValidChars="1234567890." Enabled="true" />

                                                                <asp:TextBox ID="txtGPASecondary" runat="server" class="margin-zero input-Size42" />
                                                                <ajaxToolkit:FilteredTextBoxExtender ID="filterResultSecondary" runat="server" TargetControlID="txtGPASecondary" FilterType="Custom" ValidChars="1234567890." Enabled="true" />

                                                                <asp:TextBox ID="txtInstituteSecondary" runat="server" class="margin-zero input-Size43" />

                                                                <asp:TextBox ID="txtPassingYearSecondary" runat="server" class="margin-zero input-Size44" placeholder="YYYY" />
                                                                <ajaxToolkit:MaskedEditExtender runat="server" ID="MaskedEditExtender3" TargetControlID="txtPassingYearSecondary" MessageValidatorTip="true" Mask="9999" MaskType="Number" InputDirection="LeftToRight" AcceptNegative="None" ErrorTooltipEnabled="true" />
                                                            </div>
                                                            <div class="loadedArea loadedArea-Font-Size">
                                                                <asp:HiddenField ID="hdnHigherSecondary" Value="0_0" runat="server" />

                                                                <asp:DropDownList ID="ddlExamTypeHigherSecondary" runat="server" class="margin-zero dropDownList41" AutoPostBack="false" onchange="ExamTypeHigherSecondary();" />

                                                                <asp:DropDownList ID="ddlBoardHigherSecondary" runat="server" class="margin-zero dropDownList41" AutoPostBack="false" />

                                                                <asp:DropDownList ID="ddlResultTypeHigherSecondary" runat="server" class="margin-zero dropDownList42" onchange="ResultTypeHigherSecondary();">
                                                                    <%--AutoPostBack="true" OnSelectedIndexChanged="ResultTypeHigherSecondary_Change" >--%>
                                                                    <asp:ListItem Value="0">GPA</asp:ListItem>
                                                                    <asp:ListItem Value="1">1st Division</asp:ListItem>
                                                                    <asp:ListItem Value="2">2nd Division</asp:ListItem>
                                                                    <asp:ListItem Value="3">3rd Division</asp:ListItem>
                                                                    <asp:ListItem Value="7">Appear</asp:ListItem>
                                                                </asp:DropDownList>

                                                                <asp:TextBox ID="txtGW4SHigherSecondary" runat="server" class="margin-zero input-Size41" />
                                                                <ajaxToolkit:FilteredTextBoxExtender ID="filterRW4SHigherSecondary" runat="server" TargetControlID="txtGW4SHigherSecondary" FilterType="Custom" ValidChars="1234567890." Enabled="true" />

                                                                <asp:TextBox ID="txtGPAHigherSecondary" runat="server" class="margin-zero input-Size42" />
                                                                <ajaxToolkit:FilteredTextBoxExtender ID="filterResultHigherSecondary" runat="server" TargetControlID="txtGPAHigherSecondary" FilterType="Custom" ValidChars="1234567890." Enabled="true" />

                                                                <asp:TextBox ID="txtInstituteHigherSecondary" runat="server" class="margin-zero input-Size43" />

                                                                <asp:TextBox ID="txtPassingYearHigherSecondary" runat="server" class="margin-zero input-Size44" placeholder="YYYY" />
                                                                <ajaxToolkit:MaskedEditExtender runat="server" ID="MaskedEditExtender2" TargetControlID="txtPassingYearHigherSecondary" MessageValidatorTip="true" Mask="9999" MaskType="Number" InputDirection="LeftToRight" AcceptNegative="None" ErrorTooltipEnabled="true" />
                                                            </div>
                                                            <div class="loadedArea loadedArea-Font-Size">
                                                                <asp:HiddenField ID="hdnUnderGrad" Value="0_0" runat="server" />

                                                                <asp:DropDownList ID="ddlExamTypeUndergraduate" runat="server" class="margin-zero dropDownList41" AutoPostBack="false" onchange="ExamTypeUndergraduate();" />

                                                                <span class="empty-span41"></span>

                                                                <asp:DropDownList ID="ddlResultTypeUndergraduate" runat="server" class="margin-zero dropDownList42" onchange="ResultTypeUndergraduate();">
                                                                    <%--AutoPostBack="true" OnSelectedIndexChanged="ResultTypeUndergraduate_Change" >--%>
                                                                    <asp:ListItem Value="0">GPA</asp:ListItem>
                                                                    <asp:ListItem Value="4">1st Class</asp:ListItem>
                                                                    <asp:ListItem Value="5">2nd Class</asp:ListItem>
                                                                    <asp:ListItem Value="6">3rd Class</asp:ListItem>
                                                                    <asp:ListItem Value="7">Appear</asp:ListItem>
                                                                    <asp:ListItem Value="8">Pass</asp:ListItem>
                                                                </asp:DropDownList>

                                                                <span class="empty-span42"></span>

                                                                <asp:TextBox ID="txtGPAUndergraduate" runat="server" class="margin-zero input-Size42" />
                                                                <ajaxToolkit:FilteredTextBoxExtender ID="filterResultUndergraduate" runat="server" TargetControlID="txtGPAUndergraduate" FilterType="Custom" ValidChars="1234567890." Enabled="true" />

                                                                <asp:TextBox ID="txtInstituteUndergraduate" runat="server" class="margin-zero input-Size43" />

                                                                <asp:TextBox ID="txtPassingYearUndergraduate" runat="server" class="margin-zero input-Size44" placeholder="YYYY" />
                                                                <ajaxToolkit:MaskedEditExtender runat="server" ID="MaskedEditExtender1" TargetControlID="txtPassingYearUndergraduate" MessageValidatorTip="true" Mask="9999" MaskType="Number" InputDirection="LeftToRight" AcceptNegative="None" ErrorTooltipEnabled="true" />
                                                            </div>
                                                            <div class="loadedArea loadedArea-Font-Size">
                                                                <asp:HiddenField ID="hdnGrad" Value="0_0" runat="server" />

                                                                <asp:DropDownList ID="ddlExamTypeGraduate" runat="server" class="margin-zero dropDownList41" AutoPostBack="false" onchange="ExamTypeGraduate();" />

                                                                <span class="empty-span41"></span>

                                                                <asp:DropDownList ID="ddlResultTypeGraduate" runat="server" class="margin-zero dropDownList42" onchange="ResultTypeGraduate();">
                                                                    <%--AutoPostBack="true" OnSelectedIndexChanged="ResultTypeUndergraduate_Change" >--%>
                                                                    <asp:ListItem Value="0">GPA</asp:ListItem>
                                                                    <asp:ListItem Value="4">1st Class</asp:ListItem>
                                                                    <asp:ListItem Value="5">2nd Class</asp:ListItem>
                                                                    <asp:ListItem Value="6">3rd Class</asp:ListItem>
                                                                    <asp:ListItem Value="7">Appear</asp:ListItem>
                                                                    <asp:ListItem Value="8">Pass</asp:ListItem>
                                                                </asp:DropDownList>

                                                                <span class="empty-span42"></span>

                                                                <asp:TextBox ID="txtGPAGraduate" runat="server" class="margin-zero input-Size42" />
                                                                <ajaxToolkit:FilteredTextBoxExtender ID="filterResultGraduate" runat="server" TargetControlID="txtGPAGraduate" FilterType="Custom" ValidChars="1234567890." Enabled="true" />

                                                                <asp:TextBox ID="txtInstituteGraduate" runat="server" class="margin-zero input-Size43" />

                                                                <asp:TextBox ID="txtPassingYearGraduate" runat="server" class="margin-zero input-Size44" placeholder="YYYY" />
                                                                <ajaxToolkit:MaskedEditExtender runat="server" ID="mskEditConfDate" TargetControlID="txtPassingYearGraduate" MessageValidatorTip="true" Mask="9999" MaskType="Number" InputDirection="LeftToRight" AcceptNegative="None" ErrorTooltipEnabled="true" />
                                                            </div>
                                                        </div>                                                   
                                                </div>
                                            </div>                                            
                                        </fieldset>
                                    </div>
                                    <div style="margin-top: 10px">
                                        <asp:Button runat="server" ID="btnClose" Text="Cancel" Style="width: 150px; height: 30px;" Visible="false" Enabled="false" />
                                    </div>
                                </asp:Panel>


                            </div>
                            <div style="clear: both;"></div>

                            <div>
                                <asp:GridView ID="gvStudentList" runat="server" AllowSorting="True" CssClass="table-bordered"
                                    AutoGenerateColumns="False" ShowFooter="True" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                                    <HeaderStyle BackColor="#1C5E55" ForeColor="White" Height="30" Font-Bold="True" HorizontalAlign="Center" />
                                    <FooterStyle BackColor="#1C5E55" ForeColor="White" Height="30" Font-Bold="True" />
                                    <AlternatingRowStyle BackColor="White" />
                                    <RowStyle Height="25" />

                                    <Columns>
                                        <asp:TemplateField HeaderText="SL">
                                            <ItemTemplate><b><%# Container.DataItemIndex + 1 %></b></ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle Width="40px"  />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Roll" ItemStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lnkRoll" runat="server"  ForeColor="White">Student Id</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:HiddenField runat="server" ID="hdnStudentID" Value='<%#Eval("StudentID") %>'></asp:HiddenField>
                                                <asp:Label runat="server" ID="lblStudnetRoll" Text='<%#Eval("Roll") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle Width="100px" HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Name" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblName1" Text='<%#Eval("BasicInfo.FullName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="left" />
                                            <HeaderStyle Width="200px" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Registration No" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblName2" Text='<%#Eval("StudentAdditionalInformation.RegistrationNo") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle Width="100px" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Phone" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblPhone" Text='<%#Eval("BasicInfo.Phone") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="100px" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>

                                         <asp:TemplateField HeaderText="Year" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblName3" Text='<%#Eval("StudentAdditionalInformation.YearNo") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle Width="50px" />
                                        </asp:TemplateField>

                                         <asp:TemplateField HeaderText="Semester" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblName4" Text='<%#Eval("StudentAdditionalInformation.SemesterNo") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle Width="100px" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Admission Session" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblName4" Text='<%#Eval("AdmissionSession") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle Width="100px" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Current Session" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblName4" Text='<%#Eval("CurrentSession") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle Width="100px" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Hall" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblName5" Text='<%#Eval("StudentAdditionalInformation.Attribute1") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="left" />
                                            <HeaderStyle Width="250px" />
                                        </asp:TemplateField>


                                        <%-- <asp:TemplateField HeaderText="Max Advised Number">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblAdvNumber" Text='<%#Eval("MaxNoTobeAdvised") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="100px" />
                                    </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="Edit">
                                            <ItemTemplate>
                                                <div style="text-align: center">
                                                    <asp:LinkButton runat="server" ToolTip="Edit" Text="Edit" ID="lnkEdit" CommandArgument='<%#Eval("StudentID") %>' OnClick="lnkEdit_Click"></asp:LinkButton>
                                                </div>
                                            </ItemTemplate>
                                            <HeaderStyle Width="100px" />
                                        </asp:TemplateField>
                                    </Columns>

                                    <EmptyDataTemplate>
                                        <label>Data Not Found</label>No data found!
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
                            </div>
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
