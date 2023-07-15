<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true" 
    CodeBehind="BatchSetup.aspx.cs" Inherits="EMS.Module.admin.BatchSetup" %>

<%@ Register Src="~/UserControls/ProgramUserControl.ascx" TagPrefix="uc1" TagName="ProgramUserControl" %>
<%@ Register Src="~/UserControls/DepartmentUserControl.ascx" TagPrefix="uc1" TagName="DepartmentUserControl" %>
<%@ Register Src="~/UserControls/AdmissionSessionUserControl.ascx" TagPrefix="uc1" TagName="AdmissionSessionUserControl" %>
<%@ Register Src="~/UserControls/BatchUserControl.ascx" TagPrefix="uc1" TagName="BatchUserControl" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Title" Runat="Server">
    Batch
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Head" Runat="Server">
     <style type="text/css">
        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=80);
            opacity: 0.8;
            z-index: 10000;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContainer" Runat="Server">
    <div style="padding: 10px; width: 1250px;">
        <div class="PageTitle">
            <label>Batch Setup :: Batch Setup</label>
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
                                            <uc1:DepartmentUserControl runat="server" ID="ucDepartment"  OnDepartmentSelectedIndexChanged="ucDepartment_ProgramSelectedIndexChanged" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label3" runat="server" Text="Program"></asp:Label>
                                        </td>
                                        <td>
                                            <%--<asp:DropDownList ID="programDropDownList" width="250px" AutoPostBack="True" OnSelectedIndexChanged="programDropDownList_OnSelectedIndexChanged" runat="server"></asp:DropDownList>--%>
                                            <uc1:ProgramUserControl runat="server" ID="ucProgram" OnProgramSelectedIndexChanged="ucProgram_ProgramSelectedIndexChanged" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Admission Session</td>
                                        <td>
                                            <uc1:AdmissionSessionUserControl runat="server" ID="ucAdmissionSession"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" Text="Add New"></asp:Button>
                                        </td>
                                        <td>
                                            <asp:Button ID="loadButton" runat="server" AutoPostBack="True" Text="Load" OnClick="loadButton_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>

                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div style="clear: both;"></div>
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <div style="margin-top: 15px;">
                    <asp:GridView runat="server" ID="gvBatch" AllowSorting="True" CssClass="table-bordered"                                       
                AutoGenerateColumns="False" ShowFooter="True" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                    <HeaderStyle BackColor="#1C5E55" ForeColor="White" Height="30" Font-Bold="True" />
                    <FooterStyle BackColor="#1C5E55" ForeColor="White" Height="30" Font-Bold="True" />
                    <AlternatingRowStyle BackColor="White" />
                    <RowStyle Height="25" />

                        <Columns>
                            <asp:TemplateField HeaderText="Program" HeaderStyle-Width="350px">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblGroupName" Text='<%#Eval("Program.ShortName") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Width="350" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Session">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblType" Text='<%#Eval("Session.Code") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Batch NO">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblRemarks" Text='<%#Eval("BatchNO") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Action" HeaderStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEdit" runat="server" OnClick="btnEdit_Click" Text="Edit"
                                        ToolTip="Item Edit" CommandArgument='<%#Eval("BatchId") %>'>                                                
                                    </asp:LinkButton>
                                    <%--<asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click" Visible="false" Text="Delete"
                                        OnClientClick="return confirm('Are you sure to Delete this ?')"
                                        ToolTip="Item Delete" CommandArgument='<%#Eval("BatchId") %>'>                                                
                                    </asp:LinkButton>--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                         <EmptyDataTemplate>
                            <label>Data Not Found</label>
                        </EmptyDataTemplate>
                        <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />

                        <RowStyle Height="25px" VerticalAlign="Middle" HorizontalAlign="Left" BackColor="#E3EAEB" />
                        <EditRowStyle BackColor="#7C6F57" />
                        <EmptyDataTemplate>
                            No data found!
                        </EmptyDataTemplate>
                        <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#F8FAFA" />
                        <SortedAscendingHeaderStyle BackColor="#246B61" />
                        <SortedDescendingCellStyle BackColor="#D4DFE1" />
                        <SortedDescendingHeaderStyle BackColor="#15524A" />
                    </asp:GridView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
                <asp:Button ID="btnShowPopUp" runat="server" Style="display: none" />
                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="btnShowPopUp"  PopupControlID="pnlShowPopUp"
                        CancelControlID="btnCancel" BackgroundCssClass="modalBackground">
                </ajaxToolkit:ModalPopupExtender>
                <asp:Panel ID="pnlShowPopUp" runat="server" BackColor="#ffffff" Width="765px" Style="display: none; border-radius: 3px;">
                    <div style="padding: 5px;">
                        <fieldset style="padding: 5px; border: 2px solid #5D7B9D;">
                            <legend style="font-weight: 100; font-size: medium;  color:#5D7B9D; text-align: center">Batch Insert / Edit</legend>
                            <div style="padding: 5px;">
                                <b>Batch Insert / Edit</b><br />
                                <div class="Message-Area">
                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                        <ContentTemplate>
                                            <asp:Panel ID="Panel1" runat="server" Visible="true">
                                                <asp:Label ID="Label2" runat="server" Text="Message : "></asp:Label>
                                                <asp:Label ID="lblMessage" runat="server"  ForeColor="#CC0000"></asp:Label>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>

                                <table>
                                    <tr>
                                        <td class="auto-style8">
                                            <asp:Label ID="lbldepartment" runat="server" CssClass="control-newlabel2" Text="Department"></asp:Label>   
                                        </td>
                                        <td class="auto-style9"> 
                                            <asp:DropDownList ID="ddlDepartment" AutoPostBack="true" runat="server" Width="180" OnSelectedIndexChanged="ucDepartment2_ProgramSelectedIndexChanged"></asp:DropDownList> 
                                            <asp:Label ID="lblBatchId" runat="server" Visible="false" ></asp:Label>
                                            <%--<uc1:DepartmentUserControl runat="server" ID="ucDepartment2"  OnProgramSelectedIndexChanged="ucDepartment2_ProgramSelectedIndexChanged" />--%> 
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style8">
                                            <asp:Label ID="lblProgram" runat="server" CssClass="control-newlabel2" Text="Program"></asp:Label>   
                                        </td>
                                        <td class="auto-style9">      
                                            <uc1:ProgramUserControl runat="server" ID="ucProgram2"  OnProgramSelectedIndexChanged="ucProgram2_ProgramSelectedIndexChanged" /> 
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style8">
                                            <asp:Label ID="lblAdmSession" runat="server" CssClass="control-newlabel2" Text="Admission Session"></asp:Label>   
                                        </td>
                                        <td class="auto-style9">      
                                            <uc1:AdmissionSessionUserControl runat="server" ID="ucAdmissionSession2" OnSessionSelectedIndexChanged="ucAdmissionSession2_SessionSelectedIndexChanged"  />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style8">
                                            <asp:Label ID="lblBatchNo" runat="server" CssClass="control-newlabel2" Text="Batch No"></asp:Label>   
                                        </td>
                                        <td class="auto-style9">      
                                            <asp:TextBox ID="txtBatchNo" runat="server" Width="120"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save"></asp:Button>
                                        </td>
                                        <td>
                                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </fieldset>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>