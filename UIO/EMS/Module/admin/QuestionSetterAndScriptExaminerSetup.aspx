<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true" CodeBehind="QuestionSetterAndScriptExaminerSetup.aspx.cs" Inherits="EMS.Module.admin.QuestionSetterAndScriptExaminerSetup" %>

<%@ Register Src="~/UserControls/ProgramUserControl.ascx" TagPrefix="uc1" TagName="ProgramUserControl" %>
<%@ Register Src="~/UserControls/DepartmentUserControl.ascx" TagPrefix="uc1" TagName="DepartmentUserControl" %>



<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Question Setter And Script Examiner Setup
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">


    <style type="text/css">
        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=80);
            opacity: 0.8;
            z-index: 10000;
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

        #ctl00_MainContainer_ddlExaminerType, #ctl00_MainContainer_ddlExternalScriptExDept, #ctl00_MainContainer_ddlExternalQSetterDept, #ctl00_MainContainer_ucDepartment_ddlDepartment, #ctl00_MainContainer_ucProgram_ddlProgram, #ctl00_MainContainer_ddlThirdExaminerDept_ddlDepartment,
        #ctl00_MainContainer_ddlExternalThirdExaminer, #ctl00_MainContainer_ddlExternalThirdExaminerDept, #ctl00_MainContainer_ddlInternalQSetterDept_ddlDepartment, #ctl00_MainContainer_ddlExternalQSetterDept_ddlDepartment, #ctl00_MainContainer_ddlInternalScriptExDept_ddlDepartment, #ctl00_MainContainer_ddlExternalScriptExDept_ddlDepartment, #ctl00_MainContainer_btnUpdateInfo {
            height: 40px !important;
            font-size: 20px;
        }

        span.select2-selection.select2-selection--single, #ctl00_MainContainer_ddlInternalQSetterName, #ctl00_MainContainer_ddlThirdExaminerName, #ctl00_MainContainer_ddlExternalQSetterName, #ctl00_MainContainer_ddlInternalScriptExName, #ctl00_MainContainer_ddlExternalScriptExName {
            height: 40px;
        }

        span.select2.select2-container.select2-container--default {
            width: 100% !important;
        }

        .sweet-alert {
            z-index: 10000000 !important;
            background-color: gray !important;
        }

            .sweet-alert h2 {
                color: white !important;
                font-weight: bold;
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

        function initdropdown() {
            $('#ctl00_MainContainer_ddlHeldIn').select2({
                //allowClear: true
                //,
                //placeholder: { id: '0', text: 'Select' }
            });

            $('#ctl00_MainContainer_ddlInternalQSetterName').select2({
                //allowClear: true
                //,
                //placeholder: { id: '0', text: 'Select' }
            });

            $('#ctl00_MainContainer_ddlExternalQSetterName').select2({
                //allowClear: true,
                //placeholder: { id: '0', text: 'Select' }
            });

            $('#ctl00_MainContainer_ddlInternalScriptExName').select2({
                //allowClear: true,
                //placeholder: { id: '0', text: 'Select' }
            });
            $('#ctl00_MainContainer_ddlExternalScriptExName').select2({
                //allowClear: true,
                //placeholder: { id: '0', text: 'Select' }
            });
            $('#ctl00_MainContainer_ddlThirdExaminerName').select2({
                //allowClear: true,
                //placeholder: { id: '0', text: 'Select' }
            });
            $('#ctl00_MainContainer_ddlExternalThirdExaminer').select2({
                //allowClear: true,
                //placeholder: { id: '0', text: 'Select' }
            });
        }

        function jsShowHideProgress() {
            setTimeout(function () {
                document.getElementById('divProgress').style.display = 'block';
            }, 200);
            deleteCookie();

            var timeInterval = 500; // milliseconds (checks the cookie for every half second )

            var loop = setInterval(function () {
                if (IsCookieValid()) {
                    document.getElementById('divProgress').style.display =
                    'none'; clearInterval(loop)
                }

            }, timeInterval);
        }
        // cookies
        function deleteCookie() {
            var cook = getCookie('ExcelDownloadFlag');
            if (cook != "") {
                document.cookie = "ExcelDownloadFlag=;Path=/; expires=Thu, 01 Jan 1970 00:00:00 UTC";
            }
        }

        function IsCookieValid() {
            var cook = getCookie('ExcelDownloadFlag');
            return cook != '';
        }

        function getCookie(cname) {
            var name = cname + "=";
            var ca = document.cookie.split(';');
            for (var i = 0; i < ca.length; i++) {
                var c = ca[i];
                while (c.charAt(0) == ' ') {
                    c = c.substring(1);
                }
                if (c.indexOf(name) == 0) {
                    return c.substring(name.length, c.length);
                }
            }
            return "";
        }


    </script>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContainer" runat="server">


    <div class="row">
        <div class="col-sm-12" style="font-size: 12pt; margin-top: 10pt;">
            <label><b style="color: black; font-size: 26px">Question Setter And Script Examiner Setup</b></label>
        </div>
    </div>
    <div id="divProgress" style="display: none; z-index: 100000000; position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%);">
        <asp:Image ID="LoadingImage" runat="server" ImageUrl="~/Images/Img/Waiting.gif" Height="300px" Width="300px" />
        <div>
            <asp:Label ID="Label1" runat="server" Text="Processing your request.........." ForeColor="Red" Font-Bold="true" Font-Italic="true" Font-Size="30px"></asp:Label>
        </div>
    </div>

    <hr />

    <asp:UpdatePanel runat="server" ID="UpdatePanel02">
        <ContentTemplate>
            <div class="card">
                <div class="card-body">

                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <b>Choose Department</b>
                            <br />
                            <uc1:DepartmentUserControl runat="server" ID="ucDepartment" OnDepartmentSelectedIndexChanged="ucDepartment_DepartmentSelectedIndexChanged" />
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <b>Choose Program <span style="color: red">*</span></b>
                            <br />
                            <uc1:ProgramUserControl runat="server" ID="ucProgram" OnProgramSelectedIndexChanged="ucProgram_ProgramSelectedIndexChanged" />
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <script type="text/javascript">
                                Sys.Application.add_load(initdropdown);
                            </script>
                            <b>Choose Semester & Held in to Load Courses<span style="color: red;">*</span></b>
                            <br />
                            <asp:DropDownList ID="ddlHeldIn" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlHeldIn_SelectedIndexChanged"></asp:DropDownList>
                        </div>
                    </div>

                </div>
            </div>




            <div class="card" style="margin-top: 10px">
                <div class="card-body">
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:Button ID="btnUpdateInfo" runat="server" CssClass="btn-info w-100" Text="Assign Question Setter And Script Examiner" OnClick="btnUpdateInfo_Click" OnClientClick="this.value = 'Please wait....'; this.disabled = true;" UseSubmitBehavior="false" />
                        </div>
                    </div>
                    <div style="margin-top: 10px">
                        <asp:GridView runat="server" ID="gvScheduleList" AllowSorting="True" CssClass="table table-bordered table-responsive"
                            AutoGenerateColumns="False" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None"
                            AllowPaging="true" PageSize="15" OnPageIndexChanging="gvScheduleList_PageIndexChanging" PagerStyle-HorizontalAlign="Right">
                            <HeaderStyle BackColor="#4285f4" ForeColor="White" Height="10px" Font-Bold="True" />
                            <FooterStyle BackColor="#4285f4" ForeColor="White" Height="10px" Font-Bold="True" />
                            <AlternatingRowStyle BackColor="White" />
                            <RowStyle Height="10px" />

                            <Columns>

                                <asp:TemplateField HeaderText="SL#">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblSL" Text='<%# Container.DataItemIndex + 1 %>' ForeColor="Black" Font-Bold="true"></asp:Label>

                                    </ItemTemplate>
                                    <ItemStyle Width="1%" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderStyle-CssClass="header-center">
                                    <HeaderTemplate>
                                        <div style="text-align: center">
                                            <asp:Label runat="server" ID="ckhSelect" Font-Bold="true"></asp:Label>
                                        </div>
                                        <div style="text-align: center">
                                            <asp:CheckBox ID="chkSelectAll" runat="server" Text="Select All" OnCheckedChanged="chkSelectAll_CheckedChanged"
                                                AutoPostBack="true" />
                                        </div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div style="text-align: center">

                                            <asp:HiddenField ID="hdnAcaCalSectionID" runat="server" Value='<%#Eval("AcaCal_SectionID") %>' />
                                            <asp:HiddenField ID="hdnSetupId" runat="server" Value='<%#Eval("SetupId") %>' />

                                            <asp:CheckBox runat="server" ID="ChkActive"></asp:CheckBox>
                                        </div>
                                    </ItemTemplate>
                                    <ItemStyle Width="5%" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Course Info">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblFormalCode" Font-Bold="true" Text='<%# "Course Code : "+ Eval("FormalCode") +"     , Credit : "+Eval("Credits")+"    , Section : "+Eval("SectionName")  %>'></asp:Label>
                                        <br />
                                        <asp:Label runat="server" ID="lblTitle" Font-Bold="true" Text='<%# "Title : "+Eval("Title") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="15%" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Internal Question Setter">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblInQS" Font-Bold="true" Text='<%# "Dept : "+ Eval("InternalQSDeptCode") %>'></asp:Label>
                                        <br />
                                        <asp:Label runat="server" ID="LabelQSCode" Font-Bold="true" Text='<%# "Code : "+ Eval("InternalQSCode") %>'></asp:Label>
                                        <br />
                                        <asp:Label runat="server" ID="lblInQSName" Font-Bold="true" Text='<%# "Name : "+Eval("InternalQSName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="12%" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="External Question Setter">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblExQS" Font-Bold="true" Text='<%# "Dept : "+ Eval("ExternalQSDeptCode") %>'></asp:Label>
                                        <br />
                                        <asp:Label runat="server" ID="lblExQSName" Font-Bold="true" Text='<%# "Name : "+Eval("ExternalQSName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="12%" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Internal Script Examiner">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblInSE" Font-Bold="true" Text='<%# "Dept : "+ Eval("InternalScriptExaminerDeptCode") %>'></asp:Label>
                                        <br />
                                        <asp:Label runat="server" ID="lblInSEName" Font-Bold="true" Text='<%# "Name : "+Eval("InternalSetterName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="12%" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="External Script Examiner">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblExSE" Font-Bold="true" Text='<%# "Dept : "+ Eval("ExternalScriptExaminerDeptCode") %>'></asp:Label>
                                        <br />
                                        <asp:Label runat="server" ID="lblExSEName" Font-Bold="true" Text='<%# "Name : "+Eval("ExternalSetterName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="12%" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Third Examiner">
                                    <ItemTemplate>
                                        <asp:UpdatePanel runat="server" ID="UpdatePanel02">
                                            <ContentTemplate>
                                                <asp:LinkButton ID="lnkEligibleStudent" ToolTip="Eligible Student" Text='<%# "Eligible Student : "+ Eval("StudentCount") %>' Enabled='<%# Convert.ToInt32(Eval("StudentCount"))>0 ? true : false %>' CommandArgument='<%#Eval("AcaCal_SectionID")%>' runat="server" OnClick="lnkEligibleStudent_Click">                                                                       
                                                </asp:LinkButton>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="lnkEligibleStudent" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                        <asp:Label runat="server" ID="lblTE" Font-Bold="true" Text='<%# "Dept : "+ Eval("ThirdExaminerDeptCode") %>'></asp:Label>
                                        <br />
                                        <asp:Label runat="server" ID="lblTEName" Font-Bold="true" Text='<%# "Name : "+Eval("ThirdExaminerName") %>'></asp:Label>
                                        <div style="text-align: center">
                                            <asp:LinkButton ID="AssignThirdExaminer" ToolTip="Third Examiner" Visible='<%# Convert.ToInt32(Eval("StudentCount"))>0 ? true : false %>' CssClass="btn-success btn-sm w-50" CommandArgument='<%#Eval("SetupId")%>' runat="server" OnClick="AssignThirdExaminer_Click">
                                                                             <strong><i class="fas fa-solid fa-check"></i>&nbsp;Assign Third Examiner</strong>
                                            </asp:LinkButton>
                                        </div>
                                    </ItemTemplate>
                                    <ItemStyle Width="15%" />
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <div style="padding: 5px">
                                            <asp:LinkButton ID="EditMember" ToolTip="Edit Member" Visible='<%# Convert.ToInt32(Eval("SetupId"))>0 ? true : false %>' CssClass="btn-primary btn-sm w-50" CommandArgument='<%#Eval("SetupId")%>' runat="server" OnClick="EditMember_Click">
                                                                             <strong><i class="fas fa-pencil-alt"></i>&nbsp;Edit</strong>
                                            </asp:LinkButton>
                                            <br />
                                            <br />
                                            <asp:LinkButton ID="DeleteMember" ToolTip="Remove" Visible='<%# Convert.ToInt32(Eval("SetupId"))>0 ? true : false %>' CommandArgument='<%#Eval("SetupId")%>' runat="server" OnClientClick="return confirm('Are you sure you want to Remove this Information ?');" CssClass="btn-danger btn-sm w-50" OnClick="DeleteMember_Click">                         
                                                                                <strong><i class="fas fa-trash"></i>&nbsp;Remove</strong>
                                            </asp:LinkButton>
                                        </div>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" />
                                </asp:TemplateField>




                            </Columns>

                            <PagerStyle BackColor="#4285f4" ForeColor="White" HorizontalAlign="Center" />
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
            </div>



        </ContentTemplate>

    </asp:UpdatePanel>




    <%------------------------------------------   Modal Pop Up  ---------------------------------------%>


    <div class="col-md-15 col-lg-12">
        <asp:UpdatePanel ID="UpdatePanel9" runat="server">
            <ContentTemplate>

                <asp:Button ID="Button1" runat="server" Style="display: none" />
                <ajaxToolkit:ModalPopupExtender ID="ModalPopUpInformation" runat="server" TargetControlID="Button1" PopupControlID="Panel2"
                    BackgroundCssClass="modalBackground" CancelControlID="Button2">
                </ajaxToolkit:ModalPopupExtender>

                <asp:Panel runat="server" ID="Panel2" Style="display: none; padding: 5px; overflow-y: scroll"
                    BackColor="White" Width="60%" Height="60%">


                    <div class="panel panel-default">
                        <div class="panel-body">
                            <asp:HiddenField ID="hdnIsUpdate" runat="server" />

                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-sm-12" style="text-align: center; color: blue; font-weight: bold">
                                    <b>Question Setter And Script Examiner Setup</b>

                                </div>
                            </div>
                            <hr />

                            <div class="row">
                                <div class="col-lg-6 col-md-6 col-sm-6">
                                    <b>Internal Question Setter Dept</b>
                                    <br />
                                    <uc1:DepartmentUserControl runat="server" ID="ddlInternalQSetterDept" OnDepartmentSelectedIndexChanged="ddlInternalQSetterDept_DepartmentSelectedIndexChanged" />
                                </div>
                                <div class="col-lg-6 col-md-6 col-sm-6">
                                    <b>Internal Question Setter Name</b>
                                    <asp:DropDownList ID="ddlInternalQSetterName" runat="server" CssClass="form-control w-100" AutoPostBack="true" OnSelectedIndexChanged="ddlInternalQSetterName_SelectedIndexChanged"></asp:DropDownList>
                                </div>
                            </div>

                            <div class="row" style="margin-top: 10px">
                                <div class="col-lg-6 col-md-6 col-sm-6">
                                    <b>External Question Setter Dept</b>
                                    <br />
                                    <asp:DropDownList ID="ddlExternalQSetterDept" runat="server" CssClass="form-control w-100" AutoPostBack="true" OnSelectedIndexChanged="ddlExternalQSetterDept_DepartmentSelectedIndexChanged"></asp:DropDownList>
                                </div>
                                <div class="col-lg-6 col-md-6 col-sm-6">
                                    <b>External Question Setter Name</b>
                                    <asp:DropDownList ID="ddlExternalQSetterName" runat="server" CssClass="form-control w-100" AutoPostBack="true" OnSelectedIndexChanged="ddlExternalQSetterName_SelectedIndexChanged"></asp:DropDownList>
                                </div>
                            </div>

                            <div class="row" style="margin-top: 10px">
                                <div class="col-lg-6 col-md-6 col-sm-6">
                                    <b>Internal Script Examiner Dept</b>
                                    <br />
                                    <uc1:DepartmentUserControl runat="server" ID="ddlInternalScriptExDept" OnDepartmentSelectedIndexChanged="ddlInternalScriptExDept_DepartmentSelectedIndexChanged" />
                                </div>
                                <div class="col-lg-6 col-md-6 col-sm-6">
                                    <b>Internal Script Examiner Name</b>
                                    <asp:DropDownList ID="ddlInternalScriptExName" runat="server" CssClass="form-control w-100"></asp:DropDownList>
                                </div>
                            </div>

                            <div class="row" style="margin-top: 10px">
                                <div class="col-lg-6 col-md-6 col-sm-6">
                                    <b>External Script Examiner Dept</b>
                                    <asp:DropDownList ID="ddlExternalScriptExDept" runat="server" CssClass="form-control w-100" AutoPostBack="true" OnSelectedIndexChanged="ddlExternalScriptExDept_DepartmentSelectedIndexChanged"></asp:DropDownList>
                                </div>
                                <div class="col-lg-6 col-md-6 col-sm-6">
                                    <b>External Script Examiner Name</b>
                                    <asp:DropDownList ID="ddlExternalScriptExName" runat="server" CssClass="form-control w-100"></asp:DropDownList>
                                </div>
                            </div>

                        </div>
                    </div>
                    <div class="row" style="margin-top: 10px">
                        <div class="col-lg-8 col-md-8 col-sm-8">
                        </div>
                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <asp:Button ID="btnSaveUpdate" runat="server" Font-Bold="true" CssClass="btn-success btn-sm w-100" Text="SAVE" OnClick="btnSaveUpdate_Click" OnClientClick="this.value = 'Updating Data....'; this.disabled = true;" UseSubmitBehavior="false" />
                        </div>
                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <asp:Button runat="server" ID="Button2" Font-Bold="true" Text="CLOSE" CssClass="btn-danger btn-sm w-100" />
                        </div>
                    </div>
                    <div class="row" style="margin-top: 10px; margin-left: 5px">

                        <b style="color: red">Note : &nbsp </b>
                        <b>Internal script examiner (By default Internal Question Setter) & External script examiner (By default External Question Setter).</b>
                        <br />
                        <b style="color: blue">But You can change it. </b>
                    </div>

                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>



    <%--            ----------------------------------------   Modal Pop Up  ---------------------------------------%>




    <%------------------------------------------  Third Examiner Modal Pop Up  ---------------------------------------%>


    <div class="col-md-15 col-lg-12">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>

                <asp:Button ID="Button3" runat="server" Style="display: none" />
                <ajaxToolkit:ModalPopupExtender ID="ModalPopupThirdExaminer" runat="server" TargetControlID="Button3" PopupControlID="Panel1"
                    BackgroundCssClass="modalBackground" CancelControlID="Button5">
                </ajaxToolkit:ModalPopupExtender>

                <asp:Panel runat="server" ID="Panel1" Style="display: none; padding: 5px; overflow-y: scroll"
                    BackColor="White" Width="60%" Height="45%">


                    <div class="panel panel-default">
                        <div class="panel-body">
                            <asp:HiddenField ID="hdnThirdExSetupId" runat="server" />

                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-sm-12" style="text-align: center; color: blue; font-weight: bold">
                                    <b>Third Examiner Setup</b>

                                </div>
                            </div>
                            <hr />

                            <div class="row">
                                <div class="col-lg-6 col-md-6 col-sm-6">
                                    <b>Examiner Type</b>
                                    <asp:DropDownList ID="ddlExaminerType" runat="server" Width="100%" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlExaminerType_SelectedIndexChanged">
                                        <asp:ListItem Value="0" Selected="True">Internal (Inside University)</asp:ListItem>
                                        <asp:ListItem Value="1">External (Outside University)</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <div class="row" runat="server" id="DivInternal" style="margin-top: 10px">
                                <div class="col-lg-6 col-md-6 col-sm-6">
                                    <b>Third Examiner Dept</b>
                                    <br />
                                    <uc1:DepartmentUserControl runat="server" ID="ddlThirdExaminerDept" OnDepartmentSelectedIndexChanged="ddlThirdExaminerDept_DepartmentSelectedIndexChanged" />
                                </div>
                                <div class="col-lg-6 col-md-6 col-sm-6">
                                    <b>Third Examiner Name</b>
                                    <asp:DropDownList ID="ddlThirdExaminerName" runat="server" CssClass="form-control w-100"></asp:DropDownList>
                                </div>
                            </div>

                            <div class="row" runat="server" id="DivExternal" style="margin-top: 10px">
                                <div class="col-lg-6 col-md-6 col-sm-6">
                                    <b>Third Examiner Dept</b>
                                    <br />
                                    <asp:DropDownList ID="ddlExternalThirdExaminerDept" runat="server" CssClass="form-control w-100" AutoPostBack="true" OnSelectedIndexChanged="ddlExternalThirdExaminerDept_SelectedIndexChanged"></asp:DropDownList>

                                </div>
                                <div class="col-lg-6 col-md-6 col-sm-6">
                                    <b>Third Examiner Name</b>
                                    <asp:DropDownList ID="ddlExternalThirdExaminer" runat="server" CssClass="form-control w-100"></asp:DropDownList>
                                </div>
                            </div>

                        </div>
                    </div>
                    <div class="row" style="margin-top: 10px" runat="server" id="divSave">
                        <div class="col-lg-8 col-md-8 col-sm-8">
                        </div>
                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <asp:Button ID="btnExaminerUpdate" runat="server" Font-Bold="true" CssClass="btn-success btn-sm w-100" Text="SAVE" OnClick="btnExaminerUpdate_Click" OnClientClick="this.value = 'Updating Data....'; this.disabled = true;" UseSubmitBehavior="false" />
                        </div>
                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <asp:Button runat="server" ID="Button5" Font-Bold="true" Text="CLOSE" CssClass="btn-danger btn-sm w-100" />
                        </div>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>



    <%--            ----------------------------------------   Modal Pop Up  ---------------------------------------%>




    <%------------------------------------------  Confirmation Modal Pop Up  ---------------------------------------%>


    <%-- <div class="col-md-15 col-lg-12">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>

                <asp:Button ID="Button3" runat="server" Style="display: none" />
                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="Button3" PopupControlID="Panel1"
                    BackgroundCssClass="modalBackground" CancelControlID="Button2">
                </ajaxToolkit:ModalPopupExtender>

                <asp:Panel runat="server" ID="Panel1" Style="display: none; padding: 5px; overflow-y: scroll"
                    BackColor="White" Width="40%" Height="40%">


                    <div class="panel panel-default">
                        <div class="panel-body">
                            <asp:HiddenField ID="HiddenField1" runat="server" />
                            <div class="row">
                                <div class="col-lg-12 col-md-12 col-sm-12" style="text-align: center; color: red; font-weight: bold">
                                    <b>Do you want to set the question Setter as Script Examiner?</b>
                                </div>
                            </div>
                            <hr />
                        </div>
                    </div>
                    <div class="row" style="margin-top: 10px">
                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <asp:Button ID="Button4" runat="server" Font-Bold="true" CssClass="btn-success btn-sm w-100" Text="YES" OnClick="Button4_Click" />
                        </div>
                        <div class="col-lg-8 col-md-8 col-sm-8">
                        </div>
                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <asp:Button runat="server" ID="Button5" Font-Bold="true" Text="NO" OnClick="Button5_Click" CssClass="btn-danger btn-sm w-100" />
                        </div>
                    </div>

                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>--%>



    <%--            ----------------------------------------   Modal Pop Up  ---------------------------------------%>






    <ajaxToolkit:UpdatePanelAnimationExtender
        ID="UpdatePanelAnimationExtender1" TargetControlID="UpdatePanel02" runat="server">
        <Animations>
            <OnUpdating>
                <Parallel duration="0">
                    <ScriptAction Script="InProgress();" />
                    <EnableAction AnimationTarget="btnLoad" Enabled="false" />                   
                </Parallel>
            </OnUpdating>
            <OnUpdated>
                <Parallel duration="0">
                    <ScriptAction Script="onComplete();" />
                    <EnableAction   AnimationTarget="btnLoad" Enabled="true" />
                </Parallel>
            </OnUpdated>
        </Animations>
    </ajaxToolkit:UpdatePanelAnimationExtender>


</asp:Content>
