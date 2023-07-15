<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true" CodeBehind="ExamCommitteeDashboard2.aspx.cs" Inherits="EMS.Module.admin.ExamCommitteeDashboard2" %>

<%@ Register Src="~/UserControls/ProgramUserControl.ascx" TagPrefix="uc1" TagName="ProgramUserControl" %>
<%@ Register Src="~/UserControls/DepartmentUserControl.ascx" TagPrefix="uc1" TagName="DepartmentUserControl" %>



<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Exam Committee Dashboard
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

        #ctl00_MainContainer_ucDepartment_ddlDepartment, #ctl00_MainContainer_ucProgram_ddlProgram,
        #ctl00_MainContainer_ddlInternalQSetterDept_ddlDepartment, #ctl00_MainContainer_ddlExternalQSetterDept_ddlDepartment, #ctl00_MainContainer_ddlInternalScriptExDept_ddlDepartment, #ctl00_MainContainer_ddlExternalScriptExDept_ddlDepartment, #ctl00_MainContainer_btnUpdateInfo {
            height: 40px !important;
            font-size: 20px;
        }

        span.select2-selection.select2-selection--single, #ctl00_MainContainer_ddlInternalQSetterName, #ctl00_MainContainer_ddlExternalQSetterName, #ctl00_MainContainer_ddlInternalScriptExName, #ctl00_MainContainer_ddlExternalScriptExName {
            height: 40px;
        }

        span.select2.select2-container.select2-container--default {
            width: 100% !important;
        }

        .sweet-alert {
            z-index: 10000000 !important;
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
            <label><b style="color: black; font-size: 26px">Exam Committee Dashboard</b></label>
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
                    <%--<div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:Button ID="btnUpdateInfo" runat="server" CssClass="btn-info w-100" Text="Assign Question Setter And Script Examiner" OnClick="btnUpdateInfo_Click" OnClientClick="this.value = 'Please wait....'; this.disabled = true;" UseSubmitBehavior="false" />
                        </div>
                    </div>--%>
                    <div class="card">
                        <%--<div class="col-12">
                            <div class="col-md-12">
                                Chairman :
                                <asp:Label runat="server" ID="lblChairman"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                Member 1 :
                                <asp:Label runat="server" ID="lblMember1"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                Member 2 :
                                <asp:Label runat="server" ID="lblMember2"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                External Member :
                                <asp:Label runat="server" ID="lblMemberExt"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                Tabulator 1 :
                                <asp:Label runat="server" ID="lblTabulator1"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                Tabulator 2 :
                                <asp:Label runat="server" ID="lblTabulator2"></asp:Label>
                            </div>
                            <div class="col-md-4">
                                Tabulator 3 :
                                <asp:Label runat="server" ID="lblTabulator3"></asp:Label>
                            </div>
                        </div>--%>
                        <table>
                            <tr>
                                <td>Chairman : 
                                </td>
                                <td>
                                    <asp:Label runat="server" Font-Bold="true" ID="lblChairman"></asp:Label>
                                </td>
                                <td>Member 1 : 
                                </td>
                                <td>
                                    <asp:Label runat="server" Font-Bold="true" ID="lblMember1"></asp:Label>
                                </td>
                                <td>Tabulator 1 : 
                                </td>
                                <td>
                                    <asp:Label runat="server" Font-Bold="true" ID="lblTabulator1"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td>Member 2 : 
                                </td>
                                <td>
                                    <asp:Label runat="server" Font-Bold="true" ID="lblMember2"></asp:Label>
                                </td>
                                <td>Tabulator 2 : 
                                </td>
                                <td>
                                    <asp:Label runat="server" Font-Bold="true" ID="lblTabulator2"></asp:Label>
                                </td>

                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:Button ID="btnLoadTabulation" runat="server" Text="View Tabulation" CssClass="btn-info w-100" Style="margin-bottom: 5px" OnClick="btnLoadTabulation_Click" /></td>
                                <td>Ex. Member : 
                                </td>
                                <td>
                                    <asp:Label runat="server" Font-Bold="true" ID="lblMemberExt"></asp:Label>
                                </td>
                                <td>Tabulator 3 : 
                                </td>
                                <td>
                                    <asp:Label runat="server" Font-Bold="true" ID="lblTabulator3"></asp:Label>
                                </td>
                            </tr>
                        </table>
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
                                        <asp:HiddenField runat="server" ID="hdnStatusId" Value='<%# Eval("StatusId") %>'></asp:HiddenField>
                                    </ItemTemplate>
                                    <ItemStyle Width="1%" />
                                </asp:TemplateField>

                                <%--<asp:TemplateField HeaderStyle-CssClass="header-center">
                                    <HeaderTemplate>
                                        <div style="text-align: center">
                                            <asp:Label runat="server" ID="ckhSelect" Text="Select All" Font-Bold="true"></asp:Label>
                                        </div>
                                        <div style="text-align: center">
                                            <asp:CheckBox ID="chkSelectAll" runat="server" OnCheckedChanged="chkSelectAll_CheckedChanged"
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
                                </asp:TemplateField>--%>

                                <asp:TemplateField HeaderText="Course Info">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblFormalCode" Font-Bold="true" Text='<%# Eval("FormalCode") %>'></asp:Label>
                                        <br />
                                        <asp:Label runat="server" ID="lblTitle" Text='<%# Eval("Title") %>'></asp:Label>
                                        <br />
                                        <asp:Label runat="server" ID="lblCredit" Text='<%# "Credit : "+ Eval("Credits") + ", Total Student : " + Eval("StudentCount")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="15%" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Course Teacher">
                                    <ItemTemplate>
                                        <div style="text-align: center">
                                            <asp:Label runat="server" ID="lblCourseTeacherName" Font-Bold="true" Text='<%# Eval("CourseTeacherName") %>'></asp:Label>
                                            <br />
                                            <asp:Label runat="server" ID="lblCourseTeacherStatus" Font-Italic="true" ForeColor='<%# Convert.ToInt32(Eval("StatusId")) == 0 ? System.Drawing.Color.Red : System.Drawing.Color.Green %>' Text='<%# Convert.ToInt32(Eval("StatusId")) == 0 ? "Not Submitted" : "Submitted" %>'></asp:Label>
                                            <br />
                                            <asp:Button runat="server" ID="btnCourseTeacherMark" Visible='<%# Convert.ToInt32(Eval("StatusId")) == 0 ? false : true %>' Text="View" OnClick="btnCourseTeacherMark_Click" CommandArgument='<%#Eval("AcaCal_SectionID")%>'></asp:Button>
                                        </div>
                                    </ItemTemplate>
                                    <ItemStyle Width="18%" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Internal Script Examiner">
                                    <ItemTemplate>
                                        <div style="text-align: center">
                                            <asp:Label runat="server" ID="lblInSEName" Font-Bold="true" Text='<%# Eval("InternalSetterName") %>'></asp:Label>
                                            <br />
                                            <asp:Label runat="server" ID="lblInSEStatus" Font-Italic="true" Visible='<%# Convert.ToInt32(Eval("StatusId")) < 2 ? false : true %>' ForeColor='<%# Convert.ToInt32(Eval("StatusId")) < 2 ? System.Drawing.Color.Red : System.Drawing.Color.Green %>' Text='<%# Convert.ToInt32(Eval("StatusId")) < 2 ? "Not Submitted" : "Submitted" %>'></asp:Label>
                                            <br />
                                            <asp:Button runat="server" ID="btnInSEMark" Visible='<%# Convert.ToInt32(Eval("StatusId")) < 2 ? false : true %>' Text="View" OnClick="btnInSEMark_Click" CommandArgument='<%#Eval("AcaCal_SectionID")%>'></asp:Button>
                                        </div>
                                    </ItemTemplate>
                                    <ItemStyle Width="18%" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="External Script Examiner">
                                    <ItemTemplate>
                                        <div style="text-align: center">
                                            <asp:Label runat="server" ID="lblExSEName" Font-Bold="true" Text='<%# Eval("ExternalSetterName") %>'></asp:Label>
                                            <br />
                                            <asp:Label runat="server" ID="lblExStatus" Font-Italic="true" Visible='<%# Convert.ToInt32(Eval("StatusId")) < 3 ? false : true %>' ForeColor='<%# Convert.ToInt32(Eval("StatusId")) < 3 ? System.Drawing.Color.Red : System.Drawing.Color.Green %>' Text='<%# Convert.ToInt32(Eval("StatusId")) < 3 ? "Not Submitted" : "Submitted" %>'></asp:Label>
                                            <br />
                                            <asp:Button runat="server" ID="btnExSEMark" Visible='<%# Convert.ToInt32(Eval("StatusId")) < 3 ? false : true %>' Text="View" OnClick="btnInSEMark_Click" CommandArgument='<%#Eval("AcaCal_SectionID")%>'></asp:Button>
                                        </div>
                                    </ItemTemplate>
                                    <ItemStyle Width="18%" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Third Examiner">
                                    <ItemTemplate>
                                        <div style="text-align: center">
                                            <asp:Label runat="server" ID="lblTEName" Font-Bold="true" Text='<%# Eval("ThirdExaminerName") %>'></asp:Label>
                                            <br />
                                            <asp:Label runat="server" ID="lblTEStatus" Font-Italic="true" Visible='<%# Convert.ToInt32(Eval("StatusId")) < 4 ? false : true %>' ForeColor='<%# Convert.ToInt32(Eval("StatusId")) < 4 ? System.Drawing.Color.Red : System.Drawing.Color.Green %>' Text='<%# Convert.ToInt32(Eval("StatusId")) < 4 ? "Not Submitted" : "Submitted" %>'></asp:Label>
                                            <br />
                                            <asp:Button runat="server" ID="btnTEMark" Visible='<%# Convert.ToInt32(Eval("StatusId")) < 4 ? false : true %>' Text="View" OnClick="btnInSEMark_Click" CommandArgument='<%#Eval("AcaCal_SectionID")%>'></asp:Button>
                                        </div>
                                    </ItemTemplate>
                                    <ItemStyle Width="18%" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="100% Marksheet">
                                    <ItemTemplate>
                                        <div style="text-align: center">
                                            <asp:Button runat="server" ID="btn100Mark" Visible='<%# Convert.ToInt32(Eval("StatusId")) < 5 ? false : true %>' Text="View" Font-Bold="true" CssClass="btn-info btn-sm" OnClick="btn100Mark_Click" CommandArgument='<%#Eval("AcaCal_SectionID")%>'></asp:Button>
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
