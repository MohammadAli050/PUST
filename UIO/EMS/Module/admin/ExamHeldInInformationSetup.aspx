<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true" CodeBehind="ExamHeldInInformationSetup.aspx.cs" Inherits="EMS.Module.admin.ExamHeldInInformationSetup" %>

<%@ Register Src="~/UserControls/AdmissionSessionUserControl.ascx" TagPrefix="uc1" TagName="AdmissionSessionUserControl" %>
<%@ Register Assembly="TimePicker" Namespace="MKB.TimePicker" TagPrefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
 Exam Held In Information Setup
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

        input#ctl00_MainContainer_chkIsActive {
            width: 75px;
            height: 30px;
        }

        #ctl00_MainContainer_ucAdmissionSession_ddlSession,#ctl00_MainContainer_btnAddUpdate,#ctl00_MainContainer_btnCancel,#ctl00_MainContainer_txtHeldInName,#ctl00_MainContainer_btnAddNew, #ctl00_MainContainer_admissionSession_ddlSession ,#ctl00_MainContainer_txtExamStartDate, #ctl00_MainContainer_txtExamEndDate, #ctl00_MainContainer_ddlStartYear, #ctl00_MainContainer_ddlStartMonth, #ctl00_MainContainer_ddlEndYear, #ctl00_MainContainer_ddlEndMonth, #ctl00_MainContainer_txtResultSubmissionDate, #ctl00_MainContainer_txtResultPublishDate, #ctl00_MainContainer_txtRemarks {
            height: 40px !important;
            font-size: 20px;
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
            <label><b style="color: black; font-size: 26px">Exam Held In Information Setup</b></label>
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
                            <b>Choose Session</b>
                            <br />
                            <uc1:AdmissionSessionUserControl runat="server" ID="admissionSession" OnSessionSelectedIndexChanged="btnLoad_Click" />
                        </div>


                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <br />
                            <asp:Button ID="btnAddNew" runat="server" CssClass="btn-info w-100" Text="Click Here To Add New Held In" OnClick="btnAddNew_Click" />
                        </div>
                    </div>
                </div>
            </div>


            <asp:Panel ID="pnlAddAndUpdate" runat="server" Style="margin-top: 10px">
                <div class="card">
                    <div class="card-body">

                        <asp:HiddenField ID="hdnSetupId" runat="server" />

                        <div class="row">

                            <div class="col-lg-3 col-md-3 col-sm-3">
                                <b>Choose Session <span style="color: red">*</span></b>
                                <br />
                                <uc1:AdmissionSessionUserControl runat="server" ID="ucAdmissionSession" />
                            </div>

                            <div class="col-lg-3 col-md-3 col-sm-3">
                                <b>Held In Name</b>
                                <asp:TextBox ID="txtHeldInName" runat="server" Height="35px" CssClass="form-control" Width="100%"></asp:TextBox>
                            </div>

                            <div class="col-lg-3 col-md-3 col-sm-3">
                                <b>Choose Exam Start Date</b>
                                <asp:TextBox ID="txtExamStartDate" runat="server" Height="35px" CssClass="form-control" Width="100%"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtExamStartDate" Format="dd/MM/yyyy" />
                            </div>

                            <div class="col-lg-3 col-md-3 col-sm-3">
                                <b>Choose Exam End Date</b>
                                <asp:TextBox ID="txtExamEndDate" runat="server" Height="35px" CssClass="form-control" Width="100%"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtExamEndDate" Format="dd/MM/yyyy" />
                            </div>

                        </div>

                        <div class="row" style="margin-top: 10px">
                            <div class="col-lg-3 col-md-3 col-sm-3">
                                <b>Choose Exam Start Year</b>
                                <asp:DropDownList ID="ddlStartYear" runat="server" Height="35px" CssClass="form-control" Width="100%"></asp:DropDownList>
                            </div>
                            <div class="col-lg-3 col-md-3 col-sm-3">
                                <b>Exam Start Month</b>
                                <asp:DropDownList ID="ddlStartMonth" runat="server" Height="35px" CssClass="form-control" Width="100%"></asp:DropDownList>
                            </div>
                            <div class="col-lg-3 col-md-3 col-sm-3">
                                <b>Choose Exam End Year</b>
                                <asp:DropDownList ID="ddlEndYear" runat="server" Height="35px" CssClass="form-control" Width="100%"></asp:DropDownList>
                            </div>
                            <div class="col-lg-3 col-md-3 col-sm-3">
                                <b>Choose Exam End Month</b>
                                <asp:DropDownList ID="ddlEndMonth" runat="server" Height="35px" CssClass="form-control" Width="100%"></asp:DropDownList>
                            </div>
                        </div>

                        <div class="row" style="margin-top: 10px">
                            <div class="col-lg-3 col-md-3 col-sm-3">
                                <b>Choose Result Submission Date</b>
                                <asp:TextBox ID="txtResultSubmissionDate" runat="server" Height="35px" CssClass="form-control" Width="100%"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtResultSubmissionDate" Format="dd/MM/yyyy" />
                            </div>

                            <div class="col-lg-3 col-md-3 col-sm-3">
                                <b>Choose Result Publish Date</b>
                                <asp:TextBox ID="txtResultPublishDate" runat="server" Height="35px" CssClass="form-control" Width="100%"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtResultPublishDate" Format="dd/MM/yyyy" />
                            </div>
                            <div class="col-lg-3 col-md-3 col-sm-3">
                                <b>Remarks</b>
                                <asp:TextBox ID="txtRemarks" runat="server" Height="35px" CssClass="form-control" Width="100%"></asp:TextBox>
                            </div>
                            <div class="col-lg-3 col-md-3 col-sm-3">
                                <b>Is Active</b>
                                <br />
                                <asp:CheckBox ID="chkIsActive" runat="server" />
                            </div>
                        </div>

                        <div class="row" style="margin-top: 10px">
                            <div class="col-lg-3 col-md-3 col-sm-3">
                                <asp:Button ID="btnAddUpdate" runat="server" CssClass="btn-info w-100" OnClick="btnAddUpdate_Click" Text="Click Here To Save Info" />

                            </div>
                            <div class="col-lg-3 col-md-3 col-sm-3">
                                <asp:Button ID="btnCancel" runat="server" CssClass="btn-danger w-100" Text="Cancel" OnClick="btnCancel_Click" />

                            </div>
                        </div>

                    </div>
                </div>
            </asp:Panel>


            <div class="card" style="margin-top: 10px">
                <div class="card-body">

                    <asp:GridView runat="server" ID="gvHeldInList" AllowSorting="True" BorderStyle="Solid" CssClass="table table-bordered table-responsive"
                        AutoGenerateColumns="False" Width="100%" CellPadding="4" ForeColor="#333333"
                        AllowPaging="true" PageSize="20" OnPageIndexChanging="gvHeldInList_PageIndexChanging" PagerStyle-HorizontalAlign="Right">
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

                            <asp:TemplateField HeaderText="Active">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblActive" Text='<%# Convert.ToBoolean(Eval("IsActive")) ==true ? "Yes" : "No" %>' ForeColor="Black" Font-Bold="true"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Session">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblSession" Text='<%#Eval("Session") %>' ForeColor="Black" Font-Bold="true"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Held In Name">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblHeldInName" Text='<%#Eval("ExamName") %>' ForeColor="Black" Font-Bold="true"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Exam Start Date">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStartDate" Text='<%# Eval("ExamStartDate")==null ? "" : Eval("ExamStartDate", "{0: dd/MM/yyyy}") %>' ForeColor="Black" Font-Bold="true"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Exam End Date">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblEndDate" Text='<%# Eval("ExamEndDate")==null ? "" : Eval("ExamEndDate", "{0: dd/MM/yyyy}") %>' ForeColor="Black" Font-Bold="true"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Start Month Year">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStartMonthYear" Text='<%# Eval("HeldInStartMonth")+" "+ Eval("HeldInStartYear") %>' ForeColor="Black" Font-Bold="true"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="End Month Year">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblEndMonthYear" Text='<%# Eval("HeldInEndMonth")+" "+ Eval("HeldInEndYear") %>' ForeColor="Black" Font-Bold="true"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Result Submit Date">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblSubmitDate" Text='<%# Eval("LastDateOfResultSubmission")==null ? "" : Eval("LastDateOfResultSubmission", "{0: dd/MM/yyyy}") %>' ForeColor="Black" Font-Bold="true"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Result Publish Date">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblPublishDate" Text='<%# Eval("ResultPublishDate")==null ? "" : Eval("ResultPublishDate", "{0: dd/MM/yyyy}") %>' ForeColor="Black" Font-Bold="true"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Remarks">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblRemarks" Text='<%#Eval("Remarks") %>' ForeColor="Black" Font-Bold="true"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <ItemTemplate>
                                    <div style="padding: 5px">
                                        <asp:LinkButton ID="EditHeldIn" ToolTip="Edit Held In" CssClass="btn-primary btn-sm" CommandArgument='<%#Eval("Id")%>' runat="server" OnClick="EditHeldIn_Click">
                                                                             <strong><i class="fas fa-pencil-alt"></i>&nbsp;Edit</strong>
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
        </ContentTemplate>
    </asp:UpdatePanel>


    <ajaxToolkit:UpdatePanelAnimationExtender
        ID="UpdatePanelAnimationExtender1" TargetControlID="UpdatePanel02" runat="server">
        <Animations>
            <OnUpdating>
                <Parallel duration="0">
                    <ScriptAction Script="InProgress();" />
                    <EnableAction AnimationTarget="btnAddNew" Enabled="false" />                   
                </Parallel>
            </OnUpdating>
            <OnUpdated>
                <Parallel duration="0">
                    <ScriptAction Script="onComplete();" />
                    <EnableAction   AnimationTarget="btnAddNew" Enabled="true" />
                </Parallel>
            </OnUpdated>
        </Animations>
    </ajaxToolkit:UpdatePanelAnimationExtender>


</asp:Content>
