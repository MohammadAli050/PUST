<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true" CodeBehind="FeeSetup.aspx.cs" Inherits="EMS.Module.bill.FeeSetup" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<%@ Register Src="~/UserControls/ProgramUserControl.ascx" TagPrefix="uc1" TagName="ProgramUserControl" %>
<%@ Register Src="~/UserControls/DepartmentUserControl.ascx" TagPrefix="uc1" TagName="DepartmentUserControl" %>
<%@ Register Src="~/UserControls/BatchUserControl.ascx" TagPrefix="uc1" TagName="BatchUserControl" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Fee Setup
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <style type="text/css">
        .style1 {
            height: 25px;
        }

        .auto-style2 {
            width: 177px;
        }

        .auto-style3 {
            height: 25px;
            width: 136px;
        }

        hr {
            margin-top: .5rem !important;
            margin-bottom: .5rem !important;
            border: 0 !important;
            border-top: 1px solid rgb(21, 124, 251) !important;
        }

    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContainer" runat="server">
    <div style="padding: 10px; width: 1250px;">
        <div class="PageTitle">
            <label>Fee Setup :: Program And Batch Wise</label>
        </div>
        <div class="Message-Area">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="pnlMessage" runat="server" Visible="true" CssClass="msgPanel">
                        <asp:Label ID="Label1" runat="server" Text="Message : "></asp:Label>
                        <asp:Label ID="lblMsg" runat="server"></asp:Label>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="Message-Area" style="height: auto">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <table style="width: 1238px;" id="tblHeader">
                        <tr>
                            <td colspan="2" class="style1">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label4" runat="server" Text="Department"></asp:Label>
                                        </td>
                                        <td>
                                            <%--<asp:DropDownList ID="programDropDownList" width="250px" AutoPostBack="True" OnSelectedIndexChanged="programDropDownList_OnSelectedIndexChanged" runat="server"></asp:DropDownList>--%>
                                            <uc1:DepartmentUserControl runat="server" ID="ucDepartment" OnDepartmentSelectedIndexChanged="ucDepartment_ProgramSelectedIndexChanged" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label3" runat="server" Text="Program"></asp:Label>
                                        </td>
                                        <td>
                                            <%--<asp:DropDownList ID="programDropDownList" width="250px" AutoPostBack="True" OnSelectedIndexChanged="programDropDownList_OnSelectedIndexChanged" runat="server"></asp:DropDownList>--%>
                                            <uc1:ProgramUserControl runat="server" ID="ucProgram" OnProgramSelectedIndexChanged="programDropDownList_OnSelectedIndexChanged" />
                                        </td>
                                        <td>
                                            <asp:HyperLink ID="HyperLink1" runat="server" ToolTip=" Guide Line for Fee Setup

Notes: Fee header will be created by Edusoft admin as per BRUR.
Guide line:
1.	Select Program from Program dropdown.
2.	Select Admission Session from Admission session dropdown.
3.	If want to edit fee setup click on Load button.
4.	If want create a new fee setup for newly admitted student, then click on Load All Fee button.
5.	After clicking on Load/Load all Fee button, fee type/fee header will be shown.
6.	Amount and Fund types are editable.
7.	After changing any option click one Save button.
8.	Fee setup will be ready for specific department and new admitted session.
9.	Now if multiple program fee setup is same, then no need to create new set up. Just copy the setup to another program. For example:
a.	For Bangla program, creating a new fee setup and English and Bangla program fee setup are same.
b.	Select the English program and admission session from Carry Selected Fee Setup To option.
"
                                                NavigateUrl="" Target="_blank"><b>User Guidelines</b></asp:HyperLink>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Admission Session</td>
                                        <td>
                                            <asp:DropDownList ID="admissionSessionDropDownList" AutoPostBack="True" Width="180px" OnSelectedIndexChanged="admissionSessionDropDownList_OnSelectedIndexChanged" runat="server"></asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:Button ID="downLoadButton" runat="server" Text="Load Report" AutoPostBack="True" ToolTip="-Select- refers all Department" OnClick="downLoadButton_Click" />
                                        </td>
                                        <%--<td style="width: 150px; height: 25px">
                                            <asp:DropDownList ID="ddlFeeType" Visible ="false" runat="server"></asp:DropDownList>
                                        </td>--%>
                                    </tr>
                                    <%--<tr>
                                        <td>
                                            <asp:Label ID="Label5" runat="server" Text="Scholarship Status"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="scholarShipDropDownList" width="250px"  runat="server">
                                                <asp:ListItem Value="-1" Text="Select"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="Regular"></asp:ListItem>
                                                <asp:ListItem Value="2" Text="Board Scholarship"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>--%>
                                    <%--<tr>
                                        <td>
                                            <asp:Label ID="Label6" runat="server" Text="Parent Job Status"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="govNonGovDropDownList" width="250px"  runat="server">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <asp:Button ID="loadButton" runat="server" AutoPostBack="True" Text="Load" OnClick="loadButton_OnClick" />
                                            <asp:Button ID="btnLoadAllFee" runat="server" AutoPostBack="True" Text="Load All Fee" OnClick="btnLoadAllFee_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <%--<tr>
                            <td style="height: 25px">&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>--%>
                        <%--<tr>
                            <td colspan="2" style="height: 25px">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnSave" runat="server" CssClass="button" Height="25px"
                                                Text="Save" Width="124px" OnClick="btnSave_Click" />
                                        </td>
                                        <td>
                                            <asp:Button ID="btnCancel" runat="server" CssClass="button" Height="25px"
                                                Text="Cancel" Width="124px" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>--%>
                        <%-- <tr>
                    <td colspan="2">
                        <table>
                            <tr>
                                <td>
                                    <asp:Panel ID="Panel1" runat="server">
                                        <asp:GridView ID="gvDiscount" runat="server" AutoGenerateColumns="False">
                                        </asp:GridView>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>--%>
                    </table>

                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div class="Message-Area">
            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                <ContentTemplate>
                    <table>
                        <tr>
                            <td class="auto-style2">
                                <asp:Label ID="Label2" runat="server" Text="Carry Selected Fee Setup To Program"></asp:Label>
                            </td>
                            <td class="auto-style3">
                                <asp:DropDownList ID="ddlProgramForward" Width="250px" AutoPostBack="True" OnSelectedIndexChanged="ddlProgramForward_OnProgramSelectedIndexChanged" runat="server"></asp:DropDownList>
                            </td>
                            <td>
                                <asp:Label ID="Label5" runat="server" Text="Admission Session"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="forwardAdmissionSessionDropDownList" Width="180px" runat="server"></asp:DropDownList>
                                <%--<uc1:BatchUserControl runat="server" ID="ucBatchForward" />--%>
                            </td>
                            <td>
                                <asp:Button ID="btnForward" runat="server" Text="Forward" OnClick="btnForward_Click" Width="81px" />
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div>
            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="Panel1" runat="server">
                        <table>
                            <tr>
                                <td style="float: left;">


                                    <asp:GridView runat="server" ID="feeSetupGridView" AutoGenerateColumns="False" OnRowDataBound="feeSetupGridView_OnRowDataBound"
                                        ShowHeader="true" ForeColor="#333333" CssClass="table-bordered">
                                        <AlternatingRowStyle BackColor="White" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="SL">
                                                <ItemTemplate>
                                                    <%# Container.DataItemIndex + 1 %>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fee Type">
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdnFeeSetupID" runat="server" Value='<%#Eval("FeeSetupId") %>' />
                                                    <asp:HiddenField ID="hdmAcaCalID" runat="server" Value='<%#Eval("AcademicCalendarId") %>' />
                                                    <asp:HiddenField ID="hdmBatchID" runat="server" Value='<%#Eval("BatchId") %>' />
                                                    <asp:HiddenField ID="hdnProgramID" runat="server" Value='<%#Eval("ProgramId") %>' />
                                                    <asp:HiddenField ID="hdnTypeDefID" runat="server" Value='<%#Eval("TypeDefinitionId") %>' />


                                                    <asp:HiddenField ID="fundTypeIdHiddenField" runat="server" Value='<%#Eval("Type.FundTypeId") %>' />
                                                    <asp:HiddenField ID="fundTypeIdInFeeSetUpTableHiddenField" runat="server" Value='<%#Eval("FundTypeId") %>' />

                                                    <asp:Label runat="server" ID="lblType" Text='<%#Eval("Type.Definition") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle Width="200px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Amount">
                                                <HeaderTemplate>
                                                    <asp:Button ID="saveButton" runat="server" Style="margin-top: 5px;" Text="Save"
                                                        OnClick="saveButton_OnClick" Font-Bold="True" />
                                                    <hr />
                                                    <asp:Label ID="lblAmount" runat="server" Text="Amount"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="txtAmount" Text='<%#Eval("Amount") %>'></asp:TextBox>
                                                </ItemTemplate>
                                                <HeaderStyle Width="120px" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Fund Type">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="fundTypeDropDownList" runat="server"></asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>
                                        <EditRowStyle BackColor="#7C6F57" />
                                        <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                        <HeaderStyle BackColor="#4285f4" Font-Bold="True" HorizontalAlign="Center" ForeColor="White" />
                                        <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                                        <RowStyle BackColor="#E3EAEB" />
                                        <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                        <SortedAscendingCellStyle BackColor="#F8FAFA" />
                                        <SortedAscendingHeaderStyle BackColor="#246B61" />
                                        <SortedDescendingCellStyle BackColor="#D4DFE1" />
                                        <SortedDescendingHeaderStyle BackColor="#15524A" />

                                    </asp:GridView>


                                </td>
                                <td>
                                    <rsweb:ReportViewer ID="ReportViewer1" Visible="True" runat="server" Font-Names="Verdana" Font-Size="8pt" Height="30%" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" asynrendering="false" SizeToReportContent="true" BackColor="Wheat" CssClass="center" BorderColor="WhiteSmoke" BorderStyle="Solid" BorderWidth="1">
                                    </rsweb:ReportViewer>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <div id="reportViewerDiv" style="visibility: visible">
    </div>
</asp:Content>
