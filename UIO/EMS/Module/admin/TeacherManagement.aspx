<%@ Page Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true"
    CodeBehind="TeacherManagement.aspx.cs" Inherits="EMS.Module.admin.Teacher_Management" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Employee Profile Management
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <%-- <link href="../../ContentCSS/CSS/ChildSiteMaster.CSS" rel="stylesheet" />--%>

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

          #ctl00_MainContainer_ddlUserRole,#ctl00_MainContainer_ddlDesgFilter,#ctl00_MainContainer_txtSearchCode, #ctl00_MainContainer_txtSearchTeacherName, #ctl00_MainContainer_ddlDepartment1, #ctl00_MainContainer_ddlStatus1, #ctl00_MainContainer_btnDowload, #ctl00_MainContainer_btnLoad, #ctl00_MainContainer_btnAddNew {
            height: 40px !important;
            width: 100% !important;
            font-size: 20px !important;
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
        <div class="col-sm-12" style="font-size: 12pt; margin-top: 10pt;">
            <label><b style="color: black; font-size: 26px">Employee Profile Management</b></label>
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

                <div class="row">
                 <div class="col-lg-2 col-md-2 col-sm-2">
                        <b>Teacher ID</b>
                        <asp:TextBox ID="txtSearchCode" runat="server" CssClass="form-control" />
                    </div>
                    <div class="col-lg-2 col-md-2 col-sm-2">
                        <b>Teacher Name</b>
                        <asp:TextBox ID="txtSearchTeacherName" runat="server" CssClass="form-control" />

                    </div>
                    <div class="col-lg-2 col-md-2 col-sm-2">
                        <b>Choose Department Name</b>
                        <asp:DropDownList runat="server" ID="ddlDepartment1" AutoPostBack="false" EnableViewState="true" CssClass="form-control"></asp:DropDownList>
                    </div>
                    <div class="col-lg-2 col-md-2 col-sm-2">
                        <b>Choose Status</b>
                        <asp:DropDownList runat="server" ID="ddlStatus1" AutoPostBack="false" EnableViewState="true" CssClass="form-control"></asp:DropDownList>
                    </div>
                    <div class="col-lg-2 col-md-2 col-sm-2">
                        <b>Choose Designation</b>
                        <asp:DropDownList runat="server" ID="ddlDesgFilter" AutoPostBack="false" EnableViewState="true" CssClass="form-control">
                            <asp:ListItem Text="All" Value="All"></asp:ListItem>
                            <asp:ListItem Text="Vice-Chancellor" Value="Vice-Chancellor"></asp:ListItem>
                            <asp:ListItem Text="Director" Value="Director"></asp:ListItem>
                            <asp:ListItem Text="Chairman" Value="Chairman"></asp:ListItem>
                            <asp:ListItem Text="Professor" Value="Professor"></asp:ListItem>
                            <asp:ListItem Text="Associate Professor" Value="Associate Professor"></asp:ListItem>
                            <asp:ListItem Text="Assistant Professor" Value="Assistant Professor"></asp:ListItem>
                            <asp:ListItem Text="Lecturer" Value="Lecturer"></asp:ListItem>
                            <asp:ListItem Text="Hall Provost" Value="Hall Provost"></asp:ListItem>
                            <asp:ListItem Text="Registrar" Value="Registrar"></asp:ListItem>
                            <asp:ListItem Text="Asst. Registrar" Value="Asst. Registrar"></asp:ListItem>
                            <asp:ListItem Text="Deputy-Registrar" Value="Deputy-Registrar"></asp:ListItem>
                            <asp:ListItem Text="Controller" Value="Controller"></asp:ListItem>
                            <asp:ListItem Text="Asst. Controller" Value="Asst. Controller"></asp:ListItem>
                            <asp:ListItem Text="Deputy-Controller" Value="Deputy-Controller"></asp:ListItem>
                            <asp:ListItem Text="Section Officer" Value="Section Officer"></asp:ListItem>
                            <asp:ListItem Text="IT Officer" Value="IT Officer"></asp:ListItem>
                            <%--please add in modal designation dropdown also if you add any item in here--%>
                        </asp:DropDownList>
                    </div>
                    <div class="col-lg-2 col-md-2 col-sm-2">
                        <b>Choose User Role</b>
                        <asp:DropDownList runat="server" ID="ddlUserRole" AutoPostBack="false" EnableViewState="true" CssClass="form-control">
                            </asp:DropDownList>
                    </div>

                </div>
                <div class="row">

                    <div class="col-lg-3 col-md-3 col-sm-3">
                        <br />
                        <asp:Button ID="btnLoad" runat="server" Text="Click Here To Load" OnClick="btnLoad_Click" Width="100%" />
                    </div>
                    <div class="col-lg-3 col-md-3 col-sm-3">
                        <br />
                        <asp:Button ID="btnAddNew" runat="server" OnClick="btnAddNew_Click" Text="Click Here To Add New" Width="100%"></asp:Button>
                    </div>
                    <div class="col-lg-3 col-md-3 col-sm-3">
                        <br />
                        <asp:Button ID="btnDowload" runat="server" Text="Click Here To Download Info" OnClick="btnDowload_Click" Width="100%" />

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
                                    <div style="height: 550px; width: 1050px; margin: 5px; background-color: Window; overflow: scroll">
                                        <fieldset style="padding: 0px 10px; margin: 5px; border-color: #0D2D62;">

                                            <div class="row">
                                                <div class="col-lg-10 col-md-10 col-sm-10">
                                                    <b>Employee Info</b>
                                                </div>
                                                <div class="col-lg-2 col-md-2 col-sm-2" style="text-align: right">
                                                    <asp:Button runat="server" ID="btnClose" Text="Close" CssClass="btn-danger" />
                                                </div>
                                            </div>

                                            <br />

                                            <div class="row">
                                                <div class="col-lg-2 col-md-2 col-sm-2">
                                                </div>
                                                <div class="col-lg-8 col-md-8 col-sm-8" style="text-align: center">

                                                    <asp:Image runat="server" ID="imgPhoto" Height="200" Width="200" />
                                                    <asp:HiddenField runat="server" ID="hfPersonID" Value="" />
                                                    <asp:HiddenField runat="server" ID="hfPhotoPath" Value="" />
                                                    <div class="row" style="margin-top: 10px">
                                                        <div class="col-lg-8 col-md-8 col-sm-8">
                                                            <asp:FileUpload ID="FileUploadPhoto" runat="server" CssClass="w-100" BackColor="#cccccc" Height="35px" />
                                                            <label style="color: red">***Less than 200KB</label>
                                                        </div>
                                                        <div class="col-lg-4 col-md-4 col-sm-4">
                                                            <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" CssClass="btn-info w-100" Height="35px" />
                                                        </div>
                                                    </div>

                                                </div>
                                                <div class="col-lg-2 col-md-2 col-sm-2">
                                                </div>
                                            </div>


                                            <div class="row" style="margin-top: 20px">
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <b>Name In English<span style="color: red; font-size: 15px">*</span></b>
                                                    <asp:RequiredFieldValidator ID="CompareValidator4" runat="server"
                                                        ControlToValidate="txtNameInEnglish" ErrorMessage="Required" Font-Size="15pt" Font-Bold="true"
                                                        ForeColor="Red" Display="Dynamic" CssClass="blink"
                                                        ValidationGroup="VG1"></asp:RequiredFieldValidator>
                                                    <asp:TextBox runat="server" ID="txtNameInEnglish" CssClass="form-control" />
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <b>Name In Bangla<span style="color: red; font-size: 15px">*</span></b>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                        ControlToValidate="txtName" ErrorMessage="Required" Font-Size="15pt" Font-Bold="true"
                                                        ForeColor="Red" Display="Dynamic" CssClass="blink"
                                                        ValidationGroup="VG1"></asp:RequiredFieldValidator>
                                                    <asp:TextBox runat="server" ID="txtName" CssClass="form-control" />
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <b>Father's Name</b>
                                                    <asp:TextBox runat="server" ID="txtFatherName" CssClass="form-control" />

                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <b>Mother's Name</b>
                                                    <asp:TextBox runat="server" ID="txtMotherName" CssClass="form-control" />
                                                </div>
                                            </div>


                                            <div class="row" style="margin-top: 10px">
                                                <div class="col-lg-6 col-md-6 col-sm-6">
                                                    <b>Department</b>
                                                    <%--<asp:CompareValidator ID="CompareValidator1" runat="server"
                                                        ControlToValidate="ddlDepartment" ErrorMessage="Required" Font-Size="15pt" Font-Bold="true"
                                                        ForeColor="Red" Display="Dynamic" ValueToCompare="0" Operator="NotEqual" CssClass="blink"
                                                        ValidationGroup="VG1"></asp:CompareValidator>--%>
                                                    <asp:DropDownList runat="server" ID="ddlDepartment" AutoPostBack="false" EnableViewState="true" CssClass="form-control"></asp:DropDownList>
                                                    <asp:TextBox runat="server" Visible="false" ID="txtProgram" CssClass="form-control" ReadOnly="true" Enabled="false" />
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <b>Designation<span style="color: red; font-size: 15px">*</span></b>
                                                    <asp:CompareValidator ID="CompareValidator2" runat="server"
                                                        ControlToValidate="ddlDesignation" ErrorMessage="Required" Font-Size="15pt" Font-Bold="true"
                                                        ForeColor="Red" Display="Dynamic" ValueToCompare="Select" Operator="NotEqual" CssClass="blink"
                                                        ValidationGroup="VG1"></asp:CompareValidator>
                                                    <asp:DropDownList runat="server" ID="ddlDesignation" CssClass="form-control">

                                                        <asp:ListItem Text="Select" Value="Select"></asp:ListItem>
                                                        <asp:ListItem Text="Vice-Chancellor" Value="Vice-Chancellor"></asp:ListItem>
                                                        <asp:ListItem Text="Chairman" Value="Chairman"></asp:ListItem>
                                                        <asp:ListItem Text="Professor" Value="Professor"></asp:ListItem>
                                                        <asp:ListItem Text="Associate Professor" Value="Associate Professor"></asp:ListItem>
                                                        <asp:ListItem Text="Assistant Professor" Value="Assistant Professor"></asp:ListItem>
                                                        <asp:ListItem Text="Lecturer" Value="Lecturer"></asp:ListItem>
                                                        <asp:ListItem Text="Hall Provost" Value="Hall Provost"></asp:ListItem>
                                                        <asp:ListItem Text="Registrar" Value="Registrar"></asp:ListItem>
                                                        <asp:ListItem Text="Asst. Registrar" Value="Asst. Registrar"></asp:ListItem>
                                                        <asp:ListItem Text="Deputy-Registrar" Value="Deputy-Registrar"></asp:ListItem>
                                                        <asp:ListItem Text="Controller" Value="Controller"></asp:ListItem>
                                                        <asp:ListItem Text="Asst. Controller" Value="Asst. Controller"></asp:ListItem>
                                                        <asp:ListItem Text="Deputy-Controller" Value="Deputy-Controller"></asp:ListItem>
                                                        <asp:ListItem Text="Section Officer" Value="Section Officer"></asp:ListItem>

                                                    </asp:DropDownList>
                                                </div>

                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <b>Hall Info</b>
                                                    <asp:DropDownList ID="ddlHallInfo" runat="server" Width="100%" CssClass="form-control">
                                                    </asp:DropDownList>
                                                </div>

                                            </div>

                                            <div class="row" style="margin-top: 10px">
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <b>Employee Type</b>
                                                    <asp:DropDownList runat="server" ID="ddlEmployeeType" CssClass="form-control"></asp:DropDownList>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <b>Teacher Type</b>
                                                    <asp:DropDownList runat="server" ID="ddlTeacherType" CssClass="form-control"></asp:DropDownList>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <b>Teacher Code</b>
                                                    <asp:TextBox runat="server" ID="txtTeacherCode" CssClass="form-control" />
                                                    <asp:Label runat="server" ID="lblValidationStat" ForeColor="Crimson" />
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <br />
                                                    <asp:Button runat="server" ID="btnValidate" Text="Validate" OnClick="btnValidate_Click" CssClass="btn-primary w-100" />
                                                    <asp:HiddenField ID="hfTeacherCodeChanged" Value="0_0" runat="server" />
                                                </div>
                                            </div>

                                            <div class="row" style="margin-top: 10px">
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <b>Active Status</b>
                                                    <asp:DropDownList runat="server" ID="ddlStatus" AutoPostBack="false" EnableViewState="true" CssClass="form-control"></asp:DropDownList>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <b>Gender</b>
                                                    <asp:DropDownList ID="ddlGender" runat="server" AutoPostBack="false" EnableViewState="true" CssClass="form-control"></asp:DropDownList>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <b>Marital Status</b>
                                                    <asp:DropDownList ID="ddlMaritalStat" runat="server" AutoPostBack="false" EnableViewState="true" CssClass="form-control"></asp:DropDownList>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <b>Religion</b>
                                                    <asp:DropDownList ID="ddlReligion" runat="server" AutoPostBack="false" EnableViewState="true" CssClass="form-control"></asp:DropDownList>
                                                </div>
                                            </div>

                                            <div class="row" style="margin-top: 10px">
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <b>Date Of Birth</b>
                                                    <asp:TextBox runat="server" ID="txtDob" placeholder="Date" DataFormatString="{0:dd/MM/yyyy}" CssClass="form-control" />
                                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDob" Format="dd/MM/yyyy" />

                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <b>Date Of Joining</b>
                                                    <asp:TextBox runat="server" ID="txtDoj" placeholder="Date" DataFormatString="{0:dd/MM/yyyy}" CssClass="form-control" />
                                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtDoj" Format="dd/MM/yyyy" />
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <b>Nationality</b>
                                                    <asp:TextBox runat="server" ID="txtNationality" CssClass="form-control" />
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <b>Remarks</b>
                                                    <asp:TextBox runat="server" ID="txtRemarks" CssClass="form-control" />
                                                    <asp:Label runat="server" ID="lblPhotoPath" CssClass="form-control" Visible="false" />
                                                </div>
                                            </div>

                                            <div class="row" style="margin-top: 10px">
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <b>Library Card Number</b>
                                                    <asp:TextBox runat="server" ID="txtLibCard" CssClass="form-control" />
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <b>SMS Contact Number<span style="color: red; font-size: 15px">*</span></b>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                                        ControlToValidate="txtSMSContact" ErrorMessage="Required" Font-Size="15pt" Font-Bold="true"
                                                        ForeColor="Red" Display="Dynamic" CssClass="blink"
                                                        ValidationGroup="VG1"></asp:RequiredFieldValidator>
                                                    <asp:TextBox ID="txtSMSContact" runat="server" CssClass="form-control" placeholder="+880XXXXXXXXXX" />
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                                        ControlToValidate="txtSMSContact" ErrorMessage="Wrong Format"
                                                        Style="z-index: 101; left: 424px; position: absolute; top: 285px"
                                                        ValidationExpression="^(?:\+?88)?01[15-9]\d{8}$" ValidationGroup="check">
                                                    </asp:RegularExpressionValidator>
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <asp:HiddenField ID="hdnContact" Value="0_0" runat="server" />

                                                    <b>Mobile Number 1</b>
                                                    <asp:TextBox ID="txtMobile1" runat="server" CssClass="form-control" />
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <b>Mobile Number 2</b>
                                                    <asp:TextBox ID="txtMobile2" runat="server" CssClass="form-control" />
                                                </div>
                                            </div>

                                            <div class="row" style="margin-top: 10px">
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <b>Phone Residential</b>
                                                    <asp:TextBox ID="txtPhnRes" runat="server" CssClass="form-control" />
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <b>Phone Office</b>
                                                    <asp:TextBox ID="txtPhnOff" runat="server" CssClass="form-control" />
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <b>Phone Emergency </b>
                                                    <asp:TextBox ID="txtPhnEmergency" runat="server" CssClass="form-control" />
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <b>Email Personal</b>
                                                    <asp:TextBox ID="txtEmailPersonal" runat="server" CssClass="form-control" />
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
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <b>Email Official<span style="color: red; font-size: 15px">*</span></b>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                                        ControlToValidate="txtEmailOfficial" ErrorMessage="Required" Font-Size="15pt" Font-Bold="true"
                                                        ForeColor="Red" Display="Dynamic" CssClass="blink"
                                                        ValidationGroup="VG1"></asp:RequiredFieldValidator>
                                                    <asp:TextBox ID="txtEmailOfficial" runat="server" CssClass="form-control" />
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <b>Email Other</b>
                                                    <asp:TextBox ID="txtEmailOther" runat="server" CssClass="form-control" />
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <br />
                                                    <asp:Button runat="server" ValidationGroup="VG1" CssClass="btn-success w-100" ID="btnPopUpSave" Text="Save" OnClick="btnSave_Click" />
                                                </div>
                                                <div class="col-lg-3 col-md-3 col-sm-3">
                                                    <br />
                                                    <asp:Button runat="server" CssClass="btn-danger w-100" ID="btnCancel" Text="Cancel" />

                                                </div>
                                            </div>




                                        </fieldset>
                                    </div>
                                </asp:Panel>


                            </div>
                            <div style="clear: both;"></div>

                            <div>
                                <asp:GridView ID="gvTeacherList" runat="server" AllowSorting="True" CssClass="table-bordered"
                                    AutoGenerateColumns="False" ShowFooter="True" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">

                                    <HeaderStyle BackColor="#4285f4" ForeColor="White" Height="10px" Font-Bold="True" />
                                    <FooterStyle BackColor="#4285f4" ForeColor="White" Height="10px" Font-Bold="True" />
                                    <AlternatingRowStyle BackColor="White" />
                                    <RowStyle Height="10px" />

                                    <Columns>
                                        <asp:TemplateField HeaderText="SL">
                                            <ItemTemplate><b><%# Container.DataItemIndex + 1 %></b></ItemTemplate>
                                            <%--<ItemStyle HorizontalAlign="Center" />--%>
                                            <HeaderStyle Width="40px" />
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <div style="text-align: center">
                                                    <asp:Image ID="Image1" Height="60px" Width="60px" runat="server" ImageUrl='<%#Eval("Remarks")%>' Style="border-radius: 50%" />
                                                </div>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Roll">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lnkRoll" runat="server" ForeColor="White">ID</asp:LinkButton>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:HiddenField runat="server" ID="hdnStudentID" Value='<%#Eval("EmployeeID") %>'></asp:HiddenField>
                                                <asp:Label runat="server" ID="lblTeacherId" Text='<%#Eval("Code") %>'></asp:Label>
                                            </ItemTemplate>
                                            <%--<ItemStyle HorizontalAlign="Center" />--%>
                                            <HeaderStyle Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Name">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblName" Font-Bold="true" Text='<%#Eval("FullName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="300px" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Name In Bangla">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblBanglaName" Font-Bold="true" Text='<%#Eval("BanglaName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="300px" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Designation">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblDesignation" Font-Bold="true" Text='<%#Eval("Designation") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="300px" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Department Name">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="Label1" Font-Bold="true" Text='<%#Eval("DeptName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <%--<ItemStyle HorizontalAlign="Center" />--%>
                                            <HeaderStyle Width="300px" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="Label2" Font-Bold="true" Text='<%#Eval("StatusDetails") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="100px" />
                                            <%--<ItemStyle HorizontalAlign="Center" />--%>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Contact Number">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblPhone" Text='<%#Eval("SMSContactSelf") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="100px" />
                                            <%--<ItemStyle HorizontalAlign="Center" />--%>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Email">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Font-Bold="true" ID="lblEmail" Text='<%#Eval("Email") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="100px" />
                                            <%--<ItemStyle HorizontalAlign="Center" />--%>

                                        </asp:TemplateField>


                                        <%--<asp:TemplateField HeaderText="Email">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblEmail" Text='<%#Eval("ContactDetails.EmailOffice") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle Width="100px" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>--%>

                                        <asp:TemplateField HeaderText="User ID">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Font-Bold="true" ID="lblUserLogInId" Text='<%#Eval("LogInID") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="100px" />
                                            <%--<ItemStyle HorizontalAlign="Center" />--%>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <div style="text-align: center">
                                                    <asp:LinkButton runat="server" ToolTip="Edit" Text="Edit" ID="lnkEdit"
                                                        CommandArgument='<%#Eval("EmployeeID") %>'
                                                        OnClick="lnkEdit_Click">
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
                        </div>
                    </ContentTemplate>
                    <Triggers>

                        <asp:PostBackTrigger ControlID="btnUpload" />
                        <asp:PostBackTrigger ControlID="btnDowload" />
                    </Triggers>
                </asp:UpdatePanel>
                <div id="divProgress" style="display: none; z-index: 1000; position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%);">
                    <asp:Image ID="LoadingImage" runat="server" ImageUrl="~/Images/Img/Waiting.gif" Height="150px" Width="150px" />
                </div>

            </div>

        </div>
    </div>

</asp:Content>
