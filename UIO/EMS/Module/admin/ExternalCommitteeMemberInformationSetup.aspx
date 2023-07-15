<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true" CodeBehind="ExternalCommitteeMemberInformationSetup.aspx.cs" Inherits="EMS.Module.admin.ExternalCommitteeMemberInformationSetup" %>

<%@ Register Src="~/UserControls/ProgramUserControl.ascx" TagPrefix="uc1" TagName="ProgramUserControl" %>
<%@ Register Src="~/UserControls/AdmissionSessionUserControl.ascx" TagPrefix="uc1" TagName="AdmissionSessionUserControl" %>
<%@ Register Src="~/UserControls/DepartmentUserControl.ascx" TagPrefix="uc1" TagName="DepartmentUserControl" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    External Committee Member Information Setup
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
            <label><b style="color: black; font-size: 26px">Add External Committee Member Information</b></label>
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
                    <div class="row" style="margin-top: 10px; font-size: 25px; ">

                        <div class="col-lg-6 col-md-6 col-sm-6">
                            <asp:Button ID="btnAddNew" runat="server" CssClass="btn-info w-100" Text="Click here to Add New External Member" OnClick="btnAddNew_Click" />
                        </div>
                    </div>
                </div>
            </div>


            <asp:Panel ID="pnlAddAndUpdate" runat="server" Style="margin-top: 10px">
                <div class="card">
                    <div class="card-body">

                        <asp:HiddenField ID="hdnSetupId" runat="server" />

                        <div class="row">

                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <b>Name <span style="color: red">*</span></b>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                    ControlToValidate="txtName" ErrorMessage="Required" Font-Size="15pt" Font-Bold="true"
                                    ForeColor="Red" Display="Dynamic" CssClass="blink"
                                    ValidationGroup="VG1"></asp:RequiredFieldValidator>
                                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                            </div>

                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <b>Department<span style="color: red">*</span></b>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                    ControlToValidate="txtDepartment" ErrorMessage="Required" Font-Size="15pt" Font-Bold="true"
                                    ForeColor="Red" Display="Dynamic" CssClass="blink"
                                    ValidationGroup="VG1"></asp:RequiredFieldValidator>
                                <asp:TextBox ID="txtDepartment" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                            </div>

                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <b>Designation<span style="color: red">*</span></b>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                    ControlToValidate="txtDesignation" ErrorMessage="Required" Font-Size="15pt" Font-Bold="true"
                                    ForeColor="Red" Display="Dynamic" CssClass="blink"
                                    ValidationGroup="VG1"></asp:RequiredFieldValidator>
                                <asp:TextBox ID="txtDesignation" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                            </div>

                        </div>

                        <div class="row" style="margin-top: 10px">

                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <b>Phone<span style="color: red">*</span></b>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                                    ControlToValidate="txtPhone" ErrorMessage="Required" Font-Size="15pt" Font-Bold="true"
                                    ForeColor="Red" Display="Dynamic" CssClass="blink"
                                    ValidationGroup="VG1"></asp:RequiredFieldValidator>
                                <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                            </div>

                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <b>Email<span style="color: red">*</span></b>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"
                                    ControlToValidate="txtEmail" ErrorMessage="Required" Font-Size="15pt" Font-Bold="true"
                                    ForeColor="Red" Display="Dynamic" CssClass="blink"
                                    ValidationGroup="VG1"></asp:RequiredFieldValidator>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                            </div>

                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <b>University Name<span style="color: red">*</span></b>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server"
                                    ControlToValidate="txtUniversity" ErrorMessage="Required" Font-Size="15pt" Font-Bold="true"
                                    ForeColor="Red" Display="Dynamic" CssClass="blink"
                                    ValidationGroup="VG1"></asp:RequiredFieldValidator>
                                <asp:TextBox ID="txtUniversity" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                            </div>

                        </div>

                        <div class="row" style="margin-top: 10px">
                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <asp:Button ID="btnAddUpdate" runat="server" CssClass="btn-info w-100" ValidationGroup="VG1" OnClick="btnAddUpdate_Click" Text="Click here to Save Info" />

                            </div>
                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <asp:Button ID="btnCancel" runat="server" CssClass="btn-danger w-100" Text="Cancel" OnClick="btnCancel_Click" />

                            </div>
                        </div>

                    </div>
                </div>
            </asp:Panel>


            <div class="card" style="margin-top:10px">
                <div class="card-body">

                    <asp:GridView runat="server" ID="gvExternalList" AllowSorting="True"
                        AutoGenerateColumns="False" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None"
                        AllowPaging="true" PageSize="20" OnPageIndexChanging="gvExternalList_PageIndexChanging" PagerStyle-HorizontalAlign="Right">
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

                            <asp:TemplateField HeaderText="Name">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblName" Text='<%#Eval("Name") %>' ForeColor="Black" Font-Bold="true"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Department">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblDepartment" Text='<%#Eval("Department") %>' ForeColor="Black" Font-Bold="true"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Designation">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblDesignation" Text='<%#Eval("Designation") %>' ForeColor="Black" Font-Bold="true"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Phone">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblPhone" Text='<%#Eval("Phone") %>' ForeColor="Black" Font-Bold="true"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Email">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblEmail" Text='<%#Eval("Email") %>' ForeColor="Black" Font-Bold="true"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="University Name">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblUniversityName" Text='<%#Eval("UniversityName") %>' ForeColor="Black" Font-Bold="true"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <ItemTemplate>
                                    <div style="padding: 5px">
                                        <asp:LinkButton ID="EditMember" ToolTip="Edit Member" CssClass="btn-primary btn-sm" CommandArgument='<%#Eval("ExternalId")%>' runat="server" OnClick="EditMember_Click">
                                                                             <strong><i class="fas fa-pencil-alt"></i>&nbsp;Edit</strong>
                                        </asp:LinkButton>
                                    </div>
                                </ItemTemplate>
                                <ItemStyle Width="8%" />
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <ItemTemplate>
                                    <div style="padding: 5px">
                                        <asp:LinkButton ID="DeleteMember" ToolTip="Remove Member" CommandArgument='<%#Eval("ExternalId")%>' runat="server" OnClientClick="return confirm('Are you sure you want to Remove this External Member ?');" CssClass="btn-danger btn-sm" OnClick="DeleteMember_Click" >                         
                                                                                <strong><i class="fas fa-trash"></i>&nbsp;Remove</strong>
                                        </asp:LinkButton>
                                    </div>
                                </ItemTemplate>
                                <ItemStyle Width="8%" />

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
