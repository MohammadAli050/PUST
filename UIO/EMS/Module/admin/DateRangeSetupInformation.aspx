<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true" CodeBehind="DateRangeSetupInformation.aspx.cs" Inherits="EMS.Module.admin.DateRangeSetupInformation" %>


<%@ Register Assembly="TimePicker" Namespace="MKB.TimePicker" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Date Range Setup
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

        #ctl00_MainContainer_ddlHeldIn, #ctl00_MainContainer_ddlExamYear, #ctl00_MainContainer_ddlActivityType, #ctl00_MainContainer_ucProgram_ddlProgram, #ctl00_MainContainer_ddlStatus, #ctl00_MainContainer_ddlCourse,
        #ctl00_MainContainer_btnUpdateInfo {
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
        }


        input {
            height: 40px !important;
        }

        #ctl00_MainContainer_chkActive {
            height: 35px !important;
            width: 35px;
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


    <asp:UpdatePanel runat="server" ID="UpdatePanel1">
        <ContentTemplate>
            <div class="row">
                <div class="col-lg-8 col-md-8 col-sm-8" style="font-size: 12pt; margin-top: 10pt;">
                    <label><b style="color: black; font-size: 26px">Date Range Setup</b></label>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
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
                            <b>Choose Exam Year</b>
                            <asp:DropDownList ID="ddlExamYear" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlExamYear_SelectedIndexChanged"></asp:DropDownList>
                        </div>

                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <b>Choose Held In<span style="color: red;">*</span></b>
                            <br />
                            <asp:DropDownList ID="ddlHeldIn" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlHeldIn_SelectedIndexChanged"></asp:DropDownList>
                        </div>

                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <b>Activity Type<span style="color: red;">*</span></b>
                            <asp:DropDownList ID="ddlActivityType" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlActivityType_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>

                    </div>

                </div>
            </div>

            <div class="card" style="margin-top: 10px" runat="server" id="DivInfo">
                <div class="card-body">

                    <asp:HiddenField ID="hdnSaveUpdateId" runat="server" />

                    <div class="row">

                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <asp:Label ID="Label2" runat="server" Text="Start Date" Font-Bold="true" Font-Size="Large" Width="100%"></asp:Label>
                            <asp:TextBox runat="server" ID="DateFromTextBox" Width="100%" Height="40px" CssClass="form-control" placeholder="dd/MM/yyyy" DataFormatString="{0:dd/MM/yyyy}" />
                            <ajaxToolkit:CalendarExtender ID="reqSubmissionStart" runat="server" TargetControlID="DateFromTextBox" Format="dd/MM/yyyy" />
                            <br />
                            <asp:Label ID="Label7" runat="server" Text="Start Time" Font-Bold="true" Font-Size="Large"></asp:Label>
                            <cc1:TimeSelector ID="TimeSelector1" runat="server"></cc1:TimeSelector>

                        </div>
                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <asp:Label ID="Label3" runat="server" Text="End Date" Font-Bold="true" Font-Size="Large"></asp:Label>
                            <asp:TextBox runat="server" ID="DateToTextBox" Width="100%" Height="40px" CssClass="form-control" placeholder="dd/MM/yyyy" DataFormatString="{0:dd/MM/yyyy}" />
                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="DateToTextBox" Format="dd/MM/yyyy" />
                            <br />
                            <asp:Label ID="Label6" runat="server" Text="End Time:" Font-Bold="true" Font-Size="Large"></asp:Label>
                            <cc1:TimeSelector ID="TimeSelector2" runat="server"></cc1:TimeSelector>
                        </div>
                        <div class="col-lg-2 col-md-2 col-sm-2" style="text-align: center">
                            <b>Active Status</b>
                            <br />
                            <asp:CheckBox ID="chkActive" runat="server" Checked="true" />
                        </div>

                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <br />
                            <asp:Button ID="btnSaveUpdate" runat="server" OnClick="btnSaveUpdate_Click" CssClass="btn-info w-100" Text="Save" OnClientClick="this.value = 'Updating Data....'; this.disabled = true;" UseSubmitBehavior="false" />
                        </div>

                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <br />
                            <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" CssClass="btn-danger w-100" Text="Cancel" OnClientClick="this.value = 'Canceling....'; this.disabled = true;" UseSubmitBehavior="false" />
                        </div>

                    </div>
                </div>
            </div>


            <div class="card" style="margin-top: 10px">
                <div class="card-body">
                    <asp:GridView runat="server" ID="gvItemList" AllowSorting="True" CssClass="table table-bordered table-responsive"
                        AutoGenerateColumns="False" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None"
                        AllowPaging="true" PageSize="15" PagerStyle-HorizontalAlign="Right">
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

                            <asp:TemplateField HeaderText="Activity Type">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblType" Font-Bold="True" Text='<%#Eval("Attribute3") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Start Date">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStartDate" Font-Bold="False" Text='<%#Eval("StartDate", "{0:dd/MM/yyyy}") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Start Time">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStartTime" Font-Bold="True" Text='<%#Eval("StartTime", "{0:hh:mm tt}") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="End Date">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblEndDate" Font-Bold="False" Text='<%#Eval("EndDate", "{0:dd/MM/yyyy}") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="End Time">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblEndTime" Font-Bold="True" Text='<%#Eval("EndTime", "{0:hh:mm tt}") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Status">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblIsActive" Font-Bold="True"
                                        ForeColor='<%# (Convert.ToInt32(Eval("ActiveStatus")))==1 ? System.Drawing.Color.Blue : System.Drawing.Color.Red %>'
                                        Text='<%# (Convert.ToInt32(Eval("ActiveStatus")))==1 ? "Open" : "Close" %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <ItemTemplate>
                                    <div style="padding: 5px">
                                        <asp:LinkButton ID="Edit" ToolTip="Edit Member" Visible='<%# Convert.ToInt32(Eval("Id"))>0 ? true : false %>' CssClass="btn-primary btn-sm w-50" CommandArgument='<%#Eval("Id")%>' runat="server" OnClick="Edit_Click">
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
                </Parallel>
            </OnUpdating>
            <OnUpdated>
                <Parallel duration="0">
                    <ScriptAction Script="onComplete();" />
                </Parallel>
            </OnUpdated>
        </Animations>
    </ajaxToolkit:UpdatePanelAnimationExtender>

</asp:Content>
