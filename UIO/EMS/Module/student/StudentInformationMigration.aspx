<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true" CodeBehind="StudentInformationMigration.aspx.cs" Inherits="EMS.Module.student.StudentInformationMigration" %>

<%@ Register Src="~/UserControls/ProgramUserControl.ascx" TagPrefix="uc1" TagName="ProgramUserControl" %>
<%@ Register Src="~/UserControls/AdmissionSessionUserControl.ascx" TagPrefix="uc1" TagName="AdmissionSessionUserControl" %>
<%@ Register Src="~/UserControls/DepartmentUserControl.ascx" TagPrefix="uc1" TagName="DepartmentUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Student Information Upload
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

        #ctl00_MainContainer_ucDepartment_ddlDepartment, #ctl00_MainContainer_ucProgram_ddlProgram, #ctl00_MainContainer_ucAdmissionSession_ddlSession,
        #ctl00_MainContainer_btnLoad, #ctl00_MainContainer_btnMigration, #btnExcelUpload, #btnSampleExcel, #ctl00_MainContainer_btnStudentMigrateButton {
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
            <label><b style="color: black; font-size: 26px">Student Information</b></label>
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
                            <b>Choose Program</b>
                            <br />
                            <uc1:ProgramUserControl runat="server" ID="ucProgram" OnProgramSelectedIndexChanged="ucProgram_ProgramSelectedIndexChanged" />
                        </div>

                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <b>Choose Session</b>
                            <br />
                            <uc1:AdmissionSessionUserControl runat="server" ID="ucAdmissionSession" OnSessionSelectedIndexChanged="ucAdmissionSession_SessionSelectedIndexChanged" />
                        </div>

                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <br />
                            <asp:Button ID="btnLoad" runat="server" CssClass="btn-info w-100" Text="Load Student" OnClick="btnLoad_Click" />
                        </div>

                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <br />
                            <asp:Button ID="btnMigration" runat="server" CssClass="btn-primary w-100" Text="Upload Student" OnClick="btnMigration_Click" />
                        </div>

                    </div>
                </div>
            </div>


            <div class="card" style="margin-top: 10px" runat="server" id="DivViewStudent">
                <div class="card-body">

                    <asp:GridView runat="server" ID="gvStudentList" AllowSorting="True"
                        AutoGenerateColumns="False" ShowFooter="True" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                        <HeaderStyle BackColor="#4285f4" ForeColor="White" Height="10px" Font-Bold="True" />
                        <FooterStyle BackColor="#4285f4" ForeColor="White" Height="10px" Font-Bold="True" />
                        <AlternatingRowStyle BackColor="White" />
                        <RowStyle Height="10px" />

                        <Columns>

                            <asp:TemplateField HeaderText="SL#">
                                <ItemTemplate>
                                    <b><%# Container.DataItemIndex + 1 %></b>
                                </ItemTemplate>
                                <ItemStyle Width="5%" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Student ID">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblRoll" Text='<%#Eval("Roll") %>' ForeColor="Black" Font-Bold="true"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Name">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblName" Text='<%#Eval("FullName") %>' ForeColor="Black" Font-Bold="true"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Session">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblSession" Text='<%#Eval("Code") %>' ForeColor="Black" Font-Bold="true"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle />
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

            <div class="card w-100" style="margin-top: 10px;" runat="server" id="DivExcelUpload" >
                <div class="card-body" style="font-size: 20px;">
                    <div class="row">
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <asp:Label ID="Label2" runat="server" Text="Choose Excel File" Font-Bold="true"></asp:Label>
                            <br />
                            <asp:FileUpload ID="ExcelUpload" runat="server" accept=".xlsx,.xls" CssClass="w-100" BackColor="#cccccc" ClientIDMode="Static" Height="35px" />
                        </div>

                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <br />
                            <asp:Button ID="btnExcelUpload" runat="server" CssClass="btn-info w-100" Font-Size="18px" Text="Load Excel File To View Before Upload" OnClick="lnkExcelUpload_Click"
                                ClientIDMode="Static" CausesValidation="false"></asp:Button>
                        </div>

                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <br />
                            <asp:Button ID="btnSampleExcel" runat="server" CssClass="btn-info w-100" Font-Size="18px" Text="Download Sample Excel File To View " OnClick="lnkSampleExcel_Click"
                                ClientIDMode="Static" CausesValidation="false"></asp:Button>
                        </div>

                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <br />
                            <asp:Button ID="btnStudentMigrateButton" runat="server" CssClass="btn-danger w-100" Font-Size="18px" Text="Update Student Information" OnClick="lnkStudentMigrateButton_Click" OnClientClick="jsShowHideProgress();"></asp:Button>
                        </div>
                    </div>
                </div>
            </div>


            <br />

            <div class="card">
                <div class="card-body">
                    <div class="row">
                        <div class="col-lg-6 col-md-6 col-sm-6" runat="server" id="DivTotalStudent" style="text-align: center;">

                            <div class="card">
                                <div class="card-body">

                                    <div class="row">
                                        <asp:Label ID="lblTotalStudent" runat="server" Text="" Font-Bold="true" Font-Size="X-Large"></asp:Label>
                                    </div>
                                    <br />
                                    <asp:GridView ID="GVTotalStudentList" runat="server" Width="100%">
                                        <HeaderStyle BackColor="#4285f4" ForeColor="White" Height="30" Font-Bold="True" />
                                        <FooterStyle BackColor="#4285f4" ForeColor="White" Height="15px" Font-Bold="True" />
                                        <AlternatingRowStyle BackColor="White" />
                                    </asp:GridView>

                                </div>
                            </div>

                        </div>

                        <div class="col-lg-6 col-md-6 col-sm-6" runat="server" id="DivNotUploadedStudent" style="text-align: center;">

                            <div class="card">
                                <div class="card-body">

                                    <div class="row">
                                        <div class="col-lg-8 col-md-8 col-sm-8">
                                            <asp:Label ID="lblNotMigratedStudent" runat="server" Text="" Font-Bold="true" Font-Size="X-Large" ForeColor="Red"></asp:Label></b>
                                        </div>
                                        <div class="col-lg-4 col-md-4 col-sm-4">
                                            <asp:LinkButton ID="lnkDownloadExcel" runat="server" CssClass="btn-info btn-sm" Style="display: inline-block; width: 100%; text-align: center; font-size: 20px;" Font-Bold="true" Text="Download Excel File"
                                                OnClick="lnkDownloadExcel_Click" ClientIDMode="Static" CausesValidation="false" OnClientClick="jsShowHideProgress();"></asp:LinkButton>
                                        </div>
                                    </div>

                                    <br />
                                    <asp:GridView ID="GVNotUploadedStudentList" runat="server" Width="100%">
                                        <HeaderStyle BackColor="#4285f4" ForeColor="White" Height="30" Font-Bold="True" />
                                        <FooterStyle BackColor="#4285f4" ForeColor="White" Height="15px" Font-Bold="True" />
                                        <RowStyle BackColor="#ecf0f0" />
                                        <AlternatingRowStyle BackColor="#ffffff" />
                                    </asp:GridView>

                                </div>
                            </div>

                        </div>

                    </div>
                </div>
            </div>
        </ContentTemplate>


        <Triggers>
            <asp:PostBackTrigger ControlID="btnExcelUpload" />
            <asp:PostBackTrigger ControlID="btnSampleExcel" />
        </Triggers>

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
