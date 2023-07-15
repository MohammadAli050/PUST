<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true" CodeBehind="StudentYearSemesterPromotion.aspx.cs" Inherits="EMS.Module.student.StudentYearSemesterPromotion" %>


<%@ Register Src="~/UserControls/ProgramUserControl.ascx" TagPrefix="uc1" TagName="ProgramUserControl" %>
<%@ Register Src="~/UserControls/AdmissionSessionUserControl.ascx" TagPrefix="uc1" TagName="AdmissionSessionUserControl" %>
<%@ Register Src="~/UserControls/DepartmentUserControl.ascx" TagPrefix="uc1" TagName="DepartmentUserControl" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
     Assign Year Semester For Students
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">

    <script src="https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.3.min.js"></script>
    <link href="../../CSS/select2.min.css" rel="stylesheet" />
    <script src="../../JavaScript/select2.full.min.js"></script>

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
            height: 35px !important;
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

        #ctl00_MainContainer_ucDepartment_ddlDepartment, #ctl00_MainContainer_ucProgram_ddlProgram, #ctl00_MainContainer_ucAdmissionSession_ddlSession,
        #ctl00_MainContainer_btnLoad, #ctl00_MainContainer_ddlHeldIn, #ctl00_MainContainer_modalHeldIn, #ctl00_MainContainer_btnConfirm, #ctl00_MainContainer_btnCancel, #ctl00_MainContainer_btnPromote {
            height: 40px !important;
            font-size: 20px;
        }

        
        span.select2-selection.select2-selection--single {
            height: 40px;
            font-size: 20px;
        }

        span.select2.select2-container.select2-container--default {
            width: 100% !important;
        }

    </style>


    <script type="text/javascript">

        function initdropdown() {
            $('#ctl00_MainContainer_ddlHeldIn').select2({
                allowClear: true
            });

            $('#ctl00_MainContainer_modalHeldIn').select2({

                allowClear: true,
                dropdownParent: $('#ctl00_MainContainer_Panel1')
            });
        }

        function InProgress() {
            var panelProg = $get('divProgress');
            panelProg.style.display = '';
        }

        function onComplete() {
            var panelProg = $get('divProgress');
            panelProg.style.display = 'none';
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
            <label><b style="color: black; font-size: 26px">Assign Year Semester For Students</b></label>
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

                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <b>Choose Department</b>
                            <br />
                            <uc1:DepartmentUserControl runat="server" ID="ucDepartment" OnDepartmentSelectedIndexChanged="ucDepartment_DepartmentSelectedIndexChanged" />
                        </div>

                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <b>Choose Program <span style="color:red;">*</span> </b>
                            <br />
                            <uc1:ProgramUserControl runat="server" ID="ucProgram" OnProgramSelectedIndexChanged="ucProgram_ProgramSelectedIndexChanged" />
                        </div>

                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <b>Choose Session</b>
                            <br />
                            <uc1:AdmissionSessionUserControl runat="server" ID="ucAdmissionSession" OnSessionSelectedIndexChanged="ucAdmissionSession_SessionSelectedIndexChanged" />
                        </div>
                        <div class="col-lg-5 col-md-5 col-sm-5" style="font-size:15px;">
                            <script type="text/javascript">
                                Sys.Application.add_load(initdropdown);
                            </script>
                            <b>Choose Semester & Held in to Load Student</b>
                            <br />
                            <asp:DropDownList ID="ddlHeldIn" runat="server" AutoPostBack="true" Width="100%" CssClass="form-control" OnSelectedIndexChanged="ddlHeldIn_SelectedIndexChanged"></asp:DropDownList>
                        </div>

                        
                    </div>
                    <div class="row">
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <br />
                            <asp:Button ID="btnLoad" runat="server" CssClass="btn-info w-100" Text="Click Here To Load Student" OnClick="btnLoad_Click" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="card" style="margin-top: 5px">
                <div class="card-body">

                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:Button ID="btnPromote" runat="server" CssClass="btn-danger w-100" Text="Click Here to Assign Year Semester For Students" OnClick="btnPromote_Click" />
                        </div>
                    </div>

                    <br />

                    <asp:GridView runat="server" ID="gvStudentList" AllowSorting="True" OnSorting="gvStudentList_Sorting"
                        AutoGenerateColumns="False" ShowFooter="True" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                        <HeaderStyle BackColor="#4285f4" ForeColor="White" Height="10px" Font-Bold="True" />
                        <FooterStyle BackColor="#4285f4" ForeColor="White" Height="10px" Font-Bold="True" />
                        <AlternatingRowStyle BackColor="White" />
                        <RowStyle Height="10px" />

                        <Columns>

                            <asp:TemplateField HeaderText="SL#">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblSL" Text='<%# Container.DataItemIndex + 1 %>' ForeColor="Black" Font-Bold="true"></asp:Label>

                                </ItemTemplate>
                                <ItemStyle Width="5%" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderStyle-CssClass="header-center">
                                <HeaderTemplate>
                                   <%-- <div style="text-align: center">
                                        <asp:Label runat="server" ID="ckhSelect" Text="Select All" Font-Bold="true"></asp:Label>
                                    </div>--%>
                                    <div style="text-align: center">

                                        <asp:CheckBox ID="chkSelectAll" runat="server" OnCheckedChanged="chkSelectAll_CheckedChanged"
                                            AutoPostBack="true" />
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div style="text-align: center">
                                        <asp:HiddenField ID="hdnStudentRoll" runat="server" Value='<%#Eval("StudentID") %>' />
                                        <asp:HiddenField ID="hdnPromotionHistoryId" runat="server" Value='<%#Eval("PromotionHistoryId") %>' />

                                        <asp:CheckBox runat="server" ID="ChkActive"></asp:CheckBox>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Student ID">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblRoll" Text='<%#Eval("Roll") %>' ForeColor="Black" Font-Bold="true"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Student Name">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblName" Text='<%#Eval("FullName") %>' ForeColor="Black" Font-Bold="true"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderStyle-CssClass="header-center">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkAcademicSession" runat="server" ForeColor="White" CommandName="Sort" CommandArgument="AcademicSession">
                                                    <strong>Session&nbsp;<i class="fas fa-sort"></i></strong>
                                    </asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblSession" Text='<%#Eval("AcademicSession") %>' ForeColor="Black" Font-Bold="true"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderStyle-CssClass="header-center">
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lnkHeldIn" runat="server" ForeColor="White" CommandName="Sort" CommandArgument="ExamName">
                                                    <strong>Held In&nbsp;<i class="fas fa-sort"></i></strong>
                                    </asp:LinkButton>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblExamName" Text='<%#Eval("ExamName") %>' ForeColor="Black" Font-Bold="true"></asp:Label>
                                </ItemTemplate>
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



            <asp:Button ID="Button1" runat="server" Style="display: none" />
            <ajaxToolkit:ModalPopupExtender
                ID="ModalPopUpExamNameInformation"
                runat="server"
                TargetControlID="Button1"
                PopupControlID="Panel1"
                CancelControlID="btnCancel"
                BackgroundCssClass="modalBackground">
            </ajaxToolkit:ModalPopupExtender>

            <asp:Panel runat="server" ID="Panel1" Style="display: none;" Width="40%">

                <div class="card">
                    <div class="card-body">
                        <%--<div class="row">
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <b>Academic Session</b>
                                <br />
                                <uc1:AdmissionSessionUserControl runat="server" ID="AdmissionSessionUserControl1" OnSessionSelectedIndexChanged="AdmissionSessionUserControl1_SessionSelectedIndexChanged" />
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <b>Year</b>
                                <br />
                                <asp:DropDownList ID="ddlModalYear" runat="server" AutoPostBack="true" Width="100%" CssClass="form-control" OnSelectedIndexChanged="ddlModalYear_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <b>Semester</b>
                                <br />
                                <asp:DropDownList ID="ddlModalSemester" runat="server" AutoPostBack="true" Width="100%" CssClass="form-control" OnSelectedIndexChanged="ddlModalSemester_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                        </div>--%>
                        <div class="row" style="margin-top: 5px">
                            <div class="col-lg-12 col-md-12 col-sm-12">
                                <b>Held In </b>
                                <br />
                                <asp:DropDownList ID="modalHeldIn" runat="server" Width="100%"></asp:DropDownList>
                            </div>

                        </div>

                        <div class="row" style="margin-top: 10px">
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <asp:Button ID="btnConfirm" runat="server" Text="Confirm" CssClass="btn-success w-100" OnClick="btnConfirm_Click" />

                            </div>

                            <div class="col-lg-4 col-md-4 col-sm-4">
                            </div>

                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <asp:Button runat="server" CssClass="btn-danger w-100" ID="btnCancel" Text="Close" />
                            </div>
                        </div>
                    </div>
                </div>

            </asp:Panel>


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
