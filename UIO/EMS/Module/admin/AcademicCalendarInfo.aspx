<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true" Inherits="BasicSetup_AcademicCalendarInfo" CodeBehind="AcademicCalendarInfo.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style6 {
            width: 100%;
        }

        .style7 {
            /*border: 1px solid #009900;*/
            width: 93px;
            height: 27px;
        }

        .style8 {
            width: 251px;
            height: 27px;
        }

        .style9 {
            height: 27px;
        }

        .style10 {
            /*border: 1px solid #009900;*/
            font-family: Arial, Helvetica, sans-serif;
            font-size: 12px;
            line-height: 24px;
            color: #333333;
            vertical-align: Middle;
            height: 24px;
            width: 200px;
        }

        .style11 {
            /*border: 1px solid Green;*/
            font: 11px Arial, Helvetica, sans-serif;
            color: #336600;
            vertical-align: Middle;
            width: 93px;
            height: 32px;
        }

        .style12 {
            /*border: 1px solid Green;*/
            font: 11px Arial, Helvetica, sans-serif;
            color: #336600;
            vertical-align: Middle;
            height: 32px;
        }

        .style13 {
            /*border: 1px solid Green;*/
            font: 11px Arial, Helvetica, sans-serif;
            color: #336600;
            vertical-align: Middle;
            height: 27px;
            width: 251px;
        }

        .style14 {
            width: 251px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContainer" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="border: 1px solid; width: 100%; height: 580px">
                <tr>
                    <td colspan="2" align="left" style="border-style: solid; border-color: Gray; border-width: 1px">

                        <table style="width: 100%; height: 30px;">
                            <tr>
                                <td class="style10">
                                    <asp:Label ID="lblHeader" runat="server" Font-Bold="True" ForeColor="#339933"
                                        Width="190px">Academic Calendar Info</asp:Label>
                                </td>
                                <td class="td" style="height: 24px">
                                    <asp:Label ID="lblMsg" runat="server" ForeColor="#CC0000" Width="100%"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr style="border-style: solid; border-color: Gray; border-width: 1px;">
                    <td class="td" valign="top" align="left" style="vertical-align: top; width:55%">
                        <asp:Panel ID="pnlStudents" runat="server"
                            BorderColor="ActiveBorder" BorderStyle="Ridge" BorderWidth="2px" Height="549px"
                            Width="100%">
                            <table style="border: 1px solid Gray; width: 100%; height: 550px;">
                                <tr style="border: 1px solid Gray;">
                                    <td class="td" colspan="2" style="border: 1px solid #009900; height: 27px;">
                                        <asp:TextBox ID="txtSrch" runat="server" TabIndex="1" Width="90%"></asp:TextBox>
                                    </td>
                                    <td align="center" class="td" style="border: 1px solid #009900; height: 27px;">
                                        <asp:Button ID="btnFind" runat="server" CssClass="button"
                                            OnClick="btnFind_Click" Text="Find" />
                                    </td>
                                </tr>
                                <tr style="border: 1px solid Gray;">
                                    <td align="center"
                                        style="border: 1px solid #009900; height: 27px;">
                                        <asp:Button ID="btnAdd" runat="server" CssClass="button" OnClick="btnAdd_Click"
                                            Text="Add" />
                                    </td>
                                    <td align="center"
                                        style="border: 1px solid #009900; height: 27px;">
                                        <asp:Button ID="btnEdit" runat="server" CssClass="button"
                                            OnClick="btnEdit_Click" Text="Edit" />
                                    </td>
                                    <td align="center"
                                        style="border: 1px solid #009900; height: 27px;">
                                        <asp:Button ID="btnDelete" runat="server" CssClass="button"
                                            OnClick="btnDelete_Click" Text="Delete" />
                                    </td>
                                </tr>
                                <tr style="border-style: solid; border-color: Gray; border-width: 1px;" class="tr">
                                    <td class="td" colspan="3" style="border: 1px solid #009900; vertical-align: top; height: 362px;">
                                        <asp:Panel ID="pnlGrid" runat="server" Height="433px" ScrollBars="Vertical">
                                            <asp:GridView ID="gvwStudents" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#006666" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" Height="120px" TabIndex="6" Width="100%">
                                                <RowStyle ForeColor="#000066" Height="24px" />
                                                <Columns>
                                                    <asp:CommandField ShowSelectButton="True">
                                                        <ControlStyle Width="40px" />
                                                        <FooterStyle Width="40px" />
                                                        <HeaderStyle Width="40px" />
                                                        <ItemStyle Width="40px" />

                                                        <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" BackColor="SkyBlue" />
                                                    </asp:CommandField>
                                                    <asp:BoundField DataField="Code" HeaderText="BatchCode">
                                                        <ControlStyle Width="60px" />
                                                        <FooterStyle Width="60px" />
                                                        <HeaderStyle Width="60px" />
                                                        <ItemStyle Width="60px" />
                                                         <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                                    </asp:BoundField>

                                                    <asp:BoundField DataField="Name" HeaderText="Name">
                                                        <ControlStyle Width="60px" />
                                                        <FooterStyle Width="60px" />
                                                        <HeaderStyle Width="60px" />
                                                        <ItemStyle Width="120px" />
                                                    </asp:BoundField>

                                                    <asp:BoundField DataField="StartDate" HeaderText="Start Date" DataFormatString="{0:dd MMM, yyyy}" />
                                                    <asp:BoundField DataField="EndDate" HeaderText="End Date" DataFormatString="{0:dd MMM, yyyy}">
                                                        <ControlStyle Width="24px" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Current" HeaderStyle-Width="5%">
                                                        <ItemTemplate>
                                                            <div style="text-align: center; color: red;">
                                                                <%# (Boolean.Parse(Eval("IsCurrent").ToString())) ? "Active" : "" %>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Registration" HeaderStyle-Width="5%">
                                                        <ItemTemplate>
                                                            <div style="text-align: center; color: red;">
                                                                <%# (Boolean.Parse(Eval("IsActiveRegi").ToString())) ? "Active" : "" %>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                   <%-- <asp:BoundField DataField="IsCurrent" HeaderText="Current" />
                                                    <asp:BoundField DataField="IsActiveRegi" HeaderText="Registration" />--%>
                                                    <asp:BoundField DataField="Id" HeaderText="ID" Visible="False" />
                                                </Columns>
                                                <FooterStyle BackColor="White" ForeColor="#000066" />
                                                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                                <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" Height="24px" />
                                                <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" Height="24px" />
                                            </asp:GridView>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                    <td style="border: 1px solid Gray; width:40%" valign="top" class="td">
                        <asp:Panel ID="pnlStudent" runat="server"
                            BorderColor="ActiveBorder" BorderStyle="Ridge" BorderWidth="2px"
                            Height="550px" Width="95%">
                            <table style="width: 100%; height: 550px;">
                                <tr style="border-style: solid; border-width: 1px;">
                                    <td align="left" class="td" style="height: 27px; width: 93px">Year</td>
                                    <td align="left"
                                        <%--style="border: 1px solid #009900;"--%>
                                        class="style13">
                                        <asp:TextBox ID="txtYear" runat="server" TabIndex="9" Width="45%" MaxLength="4"></asp:TextBox>
                                    </td>
                                    <td align="center" style=" height: 27px;" class="td">
                                        <asp:RequiredFieldValidator ID="rvYear" runat="server"
                                            ControlToValidate="txtYear" ErrorMessage="Year can not be empty"
                                            ToolTip="Year can not be empty" ValidationGroup="vdSave">*</asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="revYear" runat="server"
                                            ControlToValidate="txtYear" ErrorMessage="Year must be 4 digit"
                                            ValidationExpression="^\d\d\d\d" ValidationGroup="vdSave"
                                            ToolTip="Year must be 4 digit">*</asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr >
                                    <td align="left" class="style7">Calendar Type</td>
                                    <td align="left"
                                        class="style8">
                                        <asp:DropDownList ID="ddlCalType" runat="server"
                                            Width="116px">
                                        </asp:DropDownList>
                                    </td>
                                    <td align="right"  class="style9">
                                        <asp:RequiredFieldValidator ID="rvCal" runat="server"
                                            ControlToValidate="ddlCalType" ErrorMessage="Calender can not be empty"
                                            ToolTip="Calender can not be empty" ValidationGroup="vdSave">*</asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="td" style="width: 93px; height: 27px">Batch Code</td>
                                    <td align="left"  class="style14">
                                        <asp:TextBox ID="txtBatch" runat="server" MaxLength="3" TabIndex="8"
                                            Width="25%"></asp:TextBox>
                                    </td>
                                    <td align="center"  height: 27px;">
                                        <asp:RequiredFieldValidator ID="rvBatchCode" runat="server"
                                            ControlToValidate="txtBatch" ErrorMessage="Batch Code can not be empty"
                                            ToolTip="Batch Code can not be empty" ValidationGroup="vdSave">*</asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="revBatchCode" runat="server"
                                            ControlToValidate="txtBatch" ErrorMessage="Batch code must be 3 digit"
                                            ToolTip="Batch code must be 3 digit" ValidationExpression="^\d\d\d"
                                            ValidationGroup="vdSave">*</asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="td" style="width: 93px; height: 7px">&nbsp;</td>
                                    <td align="left"  class="style14">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:RadioButton ID="chkIsCUrr" runat="server" Text="Is Current" Font-Bold="true" ForeColor="Blue"
                                                        GroupName="radioGroup" /></td>
                                                <td>
                                                    <asp:RadioButton ID="ChkIsNext" runat="server" Text="Is Next" Font-Bold="true" ForeColor="Blue"
                                                        GroupName="radioGroup" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td align="center" style=" height: 27px;">&nbsp;</td>
                                </tr>
                                 <tr>
                                    <td colspan="3" style="height:2px">
                                        <hr />
                                    </td>
                                </tr>
                                 <tr>
                                    <td colspan="3" style="height: 25px">
                                         <div style="text-align:left">
                                        <asp:Label ID="Label3" runat="server" Font-Bold="true" Text="Trimester Date Range :"></asp:Label>
                                             </div></td>
                                </tr>
                                <tr>
                                    <td align="right" class="td" colspan="2" style="height: 7px">
                                        <table class="style6">
                                            <tr>
                                                <td align="center" class="td">Start Date</td>
                                                <td align="center" class="td">End Date</td>
                                            </tr>
                                            <tr>
                                                <td class="td">
                                                    <dxe:ASPxDateEdit ID="clrStartDate" runat="server" Width="100%">
                                                    </dxe:ASPxDateEdit>
                                                </td>
                                                <td class="td">
                                                    <dxe:ASPxDateEdit ID="clrEndDate" runat="server" Width="100%">
                                                    </dxe:ASPxDateEdit>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td align="center" style=" height: 27px;">
                                        <table cellspacing="1" style="height: 47px">
                                            <tr>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:RequiredFieldValidator ID="rvStartDt" runat="server"
                                                        ControlToValidate="clrStartDate" ErrorMessage="Start Date can not be empty"
                                                        ToolTip="Start Date can not be empty" ValidationGroup="vdSave">*</asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="rvEndDt" runat="server"
                                                        ControlToValidate="clrEndDate" ErrorMessage="End Date can not be empty"
                                                        ToolTip="End Date can not be empty" ValidationGroup="vdSave">*</asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                 <tr>
                                    <td colspan="3" style="height:2px">
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" style="height: 25px">
                                         <div style="text-align:left">
                                        <asp:Label ID="Label1" runat="server" Font-Bold="true"  Text="Admission Date Range :"></asp:Label>
                                             </div>
                                             </td>
                                </tr>                               
                                <tr>
                                    <td align="right" class="td" colspan="2" style="height: 7px">
                                        <table class="style6">
                                            <tr>
                                                <td align="center" class="td">Start Date</td>
                                                <td align="center" class="td">End Date</td>
                                            </tr>
                                            <tr>
                                                <td class="td">
                                                    <dxe:ASPxDateEdit ID="clrAdmissionStartDate" runat="server" Width="100%">
                                                    </dxe:ASPxDateEdit>
                                                </td>
                                                <td class="td">
                                                    <dxe:ASPxDateEdit ID="clrAdmissionEndDate" runat="server" Width="100%">
                                                    </dxe:ASPxDateEdit>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td align="center" style=" height: 27px; width: 100px">
                                        <table class="style6" style="height: 47px">
                                            <tr>
                                                <td align="center">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td align="center" class="td">
                                                    <asp:CheckBox ID="chkAdmiIsActive" runat="server" Text="IsActive" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                 <tr>
                                    <td colspan="3" style="height:2px">
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" style="height: 25px">
                                        <div style="text-align:left">
                                        <asp:Label ID="Label2" runat="server"  Font-Bold="true"  Text="Registration Date Range :"></asp:Label>
                                            </div>
                                    </td>
                                </tr>
                                <tr style="width: 100%">
                                    <td align="right" class="td" colspan="2" style="height: 7px">
                                        <table class="style6">
                                            <tr>
                                                <td align="center" class="td">Start Date</td>
                                                <td align="center" class="td">End Date</td>
                                            </tr>
                                            <tr>
                                                <td class="td">
                                                    <dxe:ASPxDateEdit ID="clrRegistrationStartDate" runat="server" Width="100%">
                                                    </dxe:ASPxDateEdit>
                                                </td>
                                                <td class="td">
                                                    <dxe:ASPxDateEdit ID="clrRegistrationEndDate" runat="server" Width="100%">
                                                    </dxe:ASPxDateEdit>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="width: 100px">
                                        <table class="style6" style="height: 47px">
                                            <tr>
                                                <td align="center">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td align="center" class="td">
                                                    <asp:CheckBox ID="chkRegiIsActive" runat="server" Text="IsActive" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>

                                </tr>
                                 <tr>
                                    <td colspan="3" style="height:2px">
                                        <hr />
                                        <hr />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style11">
                                        <asp:Button ID="butSave" runat="server" CssClass="button"
                                            OnClick="butSave_Click" Text="Save" ValidationGroup="vdSave" />
                                    </td>
                                    <td colspan="2" class="style12">
                                        <asp:Button ID="btnClose" runat="server" CssClass="button"
                                            OnClick="btnClose_Click" Text="Close" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" style="color:red; font-weight:bold;">Auto admission cancel process is run when you press the save button and IsCurrent Check Box is checked.
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" style="border-style: solid; border-color: Gray; border-width: 1px;"></td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>

                <tr>
                    <td align="left" colspan="2" style="border-style: solid; border-color: Gray; border-width: 1px">
                        <asp:ValidationSummary ID="vsCourse" runat="server" BorderStyle="None" ShowMessageBox="True"
                            ShowSummary="False" ValidationGroup="vdSave" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

