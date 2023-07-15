<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true" CodeBehind="TypeDefinition.aspx.cs" Inherits="EMS.Module.bill.TypeDefinition" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Type Definition
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <style type="text/css">
        .style2 {
            height: 26px;
        }

        .style3 {
            font-family: Arial, Helvetica, sans-serif;
            font-size: 12px;
            line-height: 24px;
            color: #333333;
            vertical-align: Middle;
            height: 27px;
            width: 120px;
        }

        .style5 {
            height: 26px;
            width: 317px;
        }

        .style6 {
            width: 317px;
        }

        .modalBackground {
            background-color: #2a2d2a;
            filter: alpha(opacity=80);
            opacity: 0.8;
            z-index: 10000;
        }

        .font {
            font-size: 12px;
        }

        .cursor {
            cursor: pointer;
        }

        .gridCss {
        }

        .auto-style1 {
            width: 273px;
            margin-top:8px;
        }

        label {
            display:block
        }
        .ckbox input{
            float: left;
            margin-right: 13px;
            margin-top: 3px;
            
        }



    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContainer" runat="server">
    <div style="padding: 10px; width: 1250px;">
        <div class="PageTitle">
            <label>Type Definition Setup</label>
        </div>
        <div class="Message-Area">
            <asp:Label ID="Label1" runat="server" Text="Message : "></asp:Label>
            <asp:Label ID="lblMsg" runat="server" ForeColor="#CC0000"></asp:Label>
        </div>
        <br />
        <div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="pnlTypeDefinition" runat="server">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="typeLabel" runat="server" CssClass="control-newlabel" Text="Type"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlType" Width="263px" runat="server">
                                        <asp:ListItem>Section</asp:ListItem>
                                        <asp:ListItem>Course</asp:ListItem>
                                        <asp:ListItem>Discount</asp:ListItem>
                                        <asp:ListItem>Program</asp:ListItem>
                                        <asp:ListItem>Fee</asp:ListItem>
                                        <asp:ListItem>Fee_PCA</asp:ListItem>
                                        <asp:ListItem>PersonBlock</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="rvType" runat="server"
                                        ControlToValidate="ddlType" ErrorMessage="Type can not be empty"
                                        ValidationGroup="vdSave">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label2" runat="server" CssClass="control-newlabel" Text="Definition"></asp:Label>
                                </td>
                                <td class="auto-style1">
                                    <asp:TextBox ID="txtDefinition" runat="server" Width="250px"></asp:TextBox>
                                </td>
                                <%--<td align="right"  class="style2">
							        <asp:RequiredFieldValidator ID="rvCode" runat="server" 
                                        ControlToValidate="txtCode" ErrorMessage="Code can not be empty" 
                                        ValidationGroup="vdSave">*</asp:RequiredFieldValidator>
							        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                        ControlToValidate="txtDefinition" 
                                        ErrorMessage="Definition can not be empty" ValidationGroup="vdSave">*</asp:RequiredFieldValidator>
							        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                                        ControlToValidate="txtDefinition" 
                                        ErrorMessage="No space in defination." 
                                        ValidationExpression="^[\S]*$" ValidationGroup="vdSave">*</asp:RegularExpressionValidator>
						        </td>--%>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label4" runat="server" Text="Fund Type"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="fundTypeDropDownList" Width="263px" Style="margin-bottom: 10px;" runat="server"></asp:DropDownList>
                                </td>
                            </tr>
                            <%--<tr style="border-style: solid; border-color: Gray;">
                                <td align="left" class="style3">Accounts Head</td>
                                <td align="left" class="style5">
                                    <asp:DropDownList ID="ddlAccHead" runat="server" Height="20px" Width="258px">
                                    </asp:DropDownList>
                                </td>

                            </tr>--%>

                            <%--<tr>
                                <td align="left" class="style3">Priority</td>
                                <td align="left" class="style6">
                                    <asp:TextBox ID="txtPriority" runat="server" Width="41%" TabIndex="8"
                                        MaxLength="48"></asp:TextBox>
                                </td>
                                <td align="right" class="style2">
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                        ControlToValidate="txtPriority" ErrorMessage="Only numbers in Priority"
                                        ValidationExpression="^[0-9]+$" ValidationGroup="vdSave">*</asp:RegularExpressionValidator>
                                </td>
                            </tr>--%>

                            <tr>
                                <td>
                                    <asp:Label ID="Label3" runat="server" CssClass="control-newlabel" Text="Choose a type"></asp:Label>
                                </td>
                                <td class="auto-style1">
                                    <asp:CheckBox ID="chkIsCourseSpecificBilling" Text="Is Course Specific Billing" CssClass="ckbox" runat="server" />
                                    <asp:CheckBox ID="chkIsLifetimeOnceBilling" Text="Is Lifetime Once Billing" CssClass="ckbox" runat="server" />
                                    <asp:CheckBox ID="chkIsPerAcaCalBilling" Text="Is Per AcaCal Billing" CssClass="ckbox" runat="server" />
                                    <asp:CheckBox ID="chkIsAnnualBilling" Text="Is Annual Billing" CssClass="ckbox" runat="server" />
                                </td>
                                <td>
                                    <%--<asp:RequiredFieldValidator ID="rvCode" runat="server" 
                                        ControlToValidate="txtCode" ErrorMessage="Code can not be empty" 
                                        ValidationGroup="vdSave">*</asp:RequiredFieldValidator>--%>
                                </td>
                            </tr>


                            <tr>
                                <td class="style3">
                                    <asp:Button ID="saveButton" runat="server" Text="Save" Height="25px" Width="124px" OnClick="saveButton_OnClick" />
                                    <asp:Button ID="btnUpdate" runat="server" Visible="false" Text="Update" Height="25px" Width="124px" OnClick="btnUpdate_Click" />
                                </td>
                                <td style="height: 27px;" class="td" colspan="2">
                                    <asp:Button ID="btnClear" runat="server" Height="25px" Width="124px" Text="Clear" OnClick="btnClear_Click" />
                                    <asp:Button ID="btnCancel" Visible="false" runat="server" Height="25px" Width="124px" Text="Cancel Edit" OnClick="btnCancel_Click" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <%--<asp:panel ID="" runat="server">
                <table style="width: 476px;">
					<tr style="border-style:solid; border-color:Gray; ">
						<td  class="auto-style1" align="left" >
                         </td>
                    </tr>
				</table>
            </asp:panel>--%>

        <div style="padding: 1px; margin-top: 20px;">
            <asp:GridView ID="gvwCollection" runat="server" AutoGenerateColumns="False"
                EmptyDataText="No data found." CellPadding="4" Width="719px" ForeColor="#333333" CssClass="table-bordered">
                <%--<HeaderStyle BackColor="#ff9933" ForeColor="White" />--%>
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField DataField="TypeDefinitionID" Visible="false" HeaderText="Id">
                        <ItemStyle HorizontalAlign="Center" />
                        <HeaderStyle Width="150px" HorizontalAlign="Center" />
                    </asp:BoundField>

                    <%--<asp:TemplateField HeaderStyle-Width="50px">
                            <ItemTemplate>
                                <asp:CheckBox ID="CheckBox1" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="SL No.">
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                        <HeaderStyle />
                    </asp:TemplateField>
                    <asp:BoundField DataField="Type" HeaderText="Type">
                        <%--<ItemStyle HorizontalAlign="Center" />--%>
                        <HeaderStyle Width="200px" />
                    </asp:BoundField>

                    <asp:BoundField DataField="Definition" HeaderText="Discount Definition">
                        <%--<ItemStyle HorizontalAlign="Center" />--%>
                        <HeaderStyle Width="350px" />
                    </asp:BoundField>

                    <%--     <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="EditButton" CommandName="TypeDefinitionEdit" Text="<i class='icon-edit'></i>" ToolTip="Edit Type Definition" CommandArgument='<%# Bind("TypeDefinitionID") %>' runat="server"></asp:LinkButton>
                                <asp:LinkButton ID="DeleteButton" CommandName="TypeDefinitionDelete" Text="<i class='icon-remove'></i>" ToolTip="Delete Type Definition" CommandArgument='<%# Bind("TypeDefinitionID") %>' runat="server"></asp:LinkButton>
                            </ItemTemplate>
                            <HeaderStyle Width="80px"></HeaderStyle>
                            <ItemStyle CssClass="center" />
                        </asp:TemplateField>--%>


                    <asp:TemplateField HeaderText="Action" HeaderStyle-Width="100px">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEdit" runat="server" OnClick="btnEdit_Click" Text="Edit"
                                ToolTip="Item Edit" CommandArgument='<%#Eval("TypeDefinitionID") %>'>                                                
                            </asp:LinkButton>
                            <%--<asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click" Text="Delete"
                                OnClientClick="return confirm('Are you sure to Delete this ?')"
                                ToolTip="Item Delete" CommandArgument='<%#Eval("TypeDefinitionID") %>'>                                                
                            </asp:LinkButton>--%>
                        </ItemTemplate>
                        <HeaderStyle Width="100px"></HeaderStyle>
                    </asp:TemplateField>
                </Columns>

                <EditRowStyle BackColor="#7C6F57" />
                <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#4285f4" Font-Bold="True" ForeColor="White" HorizontalAlign="Center"/>
                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#E3EAEB" />
                <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#F8FAFA" />
                <SortedAscendingHeaderStyle BackColor="#246B61" />
                <SortedDescendingCellStyle BackColor="#D4DFE1" />
                <SortedDescendingHeaderStyle BackColor="#15524A" />
            </asp:GridView>
            <asp:HiddenField ID="hdnTypeDefinitionId" runat="server" />
        </div>
    </div>
</asp:Content>
