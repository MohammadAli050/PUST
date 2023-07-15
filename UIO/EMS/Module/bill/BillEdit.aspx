<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true" CodeBehind="BillEdit.aspx.cs" Inherits="EMS.Module.bill.BillEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">

    <style>
        hr {
            margin-top: .5rem !important;
            margin-bottom: .5rem !important;
            border: 0 !important;
            border-top: 1px solid rgb(21, 124, 251) !important;
        }
    </style>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContainer" runat="server">
    <div>
        <div class="PageTitle">
            <label>Bill Edit</label>
        </div>

        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <div class="Message-Area">
                    <asp:Label ID="Label1" runat="server" Text="Message : "></asp:Label>
                    <asp:Label ID="lblMsg" ForeColor="Red" runat="server"></asp:Label>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <div class="Message-Area">
            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                <ContentTemplate>
                    <table>
                        <tr>
                            <td class="auto-style4">
                                <asp:Label ID="lblStudentName" runat="server" Text="Student Name : "></asp:Label>
                            </td>
                            <td class="auto-style4">
                                <asp:Label ID="lblTxtStudentName" Font-Bold="true" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr hidden="true">
                            <td class="auto-style4">
                                <asp:Label ID="Label3" runat="server" Text="Batch: "></asp:Label>
                            </td>
                            <td class="auto-style4">
                                <asp:Label ID="batchLabel" Font-Bold="true" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style4">
                                <asp:Label ID="lblStudentRoll" runat="server" Text="Student Roll : "></asp:Label>
                            </td>
                            <td class="auto-style4">
                                <asp:TextBox ID="txtStudentRoll" Placeholder="Student Roll" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style4">
                                <asp:Label ID="lblReferenceNo" runat="server" Text="Reference No : "></asp:Label>
                            </td>
                            <td class="auto-style4">
                                <asp:TextBox ID="txtReferenceNo" Placeholder="Reference No" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td class="auto-style4">
                                <asp:Button ID="btnLoadStudentBill" runat="server" Text="Load Bill" OnClick="btnLoadStudentBill_Click" />
                            </td>
                        </tr>

                        <tr hidden="true">
                            <%--style="visibility: hidden"--%>
                            <td class="auto-style4">
                                <asp:Label ID="lblFee" runat="server" Font-Bold="true" Text="Fee Item : "></asp:Label>
                            </td>
                            <td class="auto-style6" colspan="2">
                                <asp:DropDownList ID="ddlFeeItem" Width="160px" runat="server"></asp:DropDownList>
                            </td>
                            <td class="auto-style4">
                                <asp:Button ID="btnLoadFeeItem"  runat="server" Text="Add Fee Item" OnClick="btnLoadFeeItem_Click" />
                            </td>
                            <tr>
                                <td>
                                    <asp:Label ID="Label2" runat="server" Font-Bold="True" Text="Total Bill: "></asp:Label>
                                    <asp:Label ID="totalBillLabel" runat="server" Font-Bold="True" ></asp:Label>
                                </td>
                            </tr>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        
        <div class="Message-Area">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="GvFeeAmount" runat="server" AutoGenerateColumns="False" CssClass="table-bordered"
                        EmptyDataText="No data found." AllowPaging="false" CellPadding="4" PageSize="20" Width="800px">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <Columns>
                            <asp:TemplateField HeaderText="BillHistoryId" Visible="false">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblBillHistoryId" Text='<%#Eval("BillHistoryId") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Width="120px" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="BillHistoryMasterId" Visible="false">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblBillHistoryMasterId" Text='<%#Eval("BillHistoryMasterId") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Width="120px" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="StudentId" Visible="false">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStudentId" Text='<%#Eval("StudentId") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Width="120px" />
                            </asp:TemplateField>

                            <%--<asp:TemplateField Visible ="false"  HeaderText="FundTypeId">
                        <ItemTemplate >
                            <asp:Label ID="lblFundTypeId"  runat="server" Text='<%# Bind("FundTypeId") %>'></asp:Label> 
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                            <HeaderStyle Width="200px" />
                    </asp:TemplateField>--%>

                            <asp:TemplateField HeaderText="FeeTypeId" Visible="false">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblTypeDefinitionId" Text='<%#Eval("TypeDefinitionId") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Width="120px" />
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="SL">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Fee Name">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblTypeDefinition" Text='<%#Eval("Attribute1") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Width="450px" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Amount">
                                <HeaderTemplate>
                                    <%--<asp:Button ID="btnCalculateFee" runat="server" Text="Calculate Fee" OnClick="btnCalculateFee_Click" />--%>
                                    <%--<hr />--%>
                                    <asp:Label ID="lblAmount" runat="server" Text="Amount"></asp:Label>
                                </HeaderTemplate>

                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblAmount" Text='<%#Eval("Fees") %>'></asp:Label>
                                </ItemTemplate>
                                <%--<HeaderTemplate>
                            <asp:Button ID="btnCalculateBillAmount" runat="server" Text="Calculate Amount" OnClick=" />
                        </HeaderTemplate>--%>
                                <HeaderStyle Width="150px" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="New Amount">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtNewFeesAmount" Width="150px" placeholder="New Amount" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Comment">
                                <HeaderTemplate>
                                    <asp:Button ID="btnBillPosting" runat="server" Text="Save Bill" OnClientClick="return confirm('Do you really want to save this bill?'); this.disabled = true; " OnClick="btnBillPosting_Click" />
                                    <hr />
                                    <asp:Label ID="txtComment" runat="server" Text="Remark"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox ID="txtComment" Width="150px" placeholder="Remark" runat="server" Text="" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EditRowStyle BackColor="#999999" />
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#4285f4" Font-Bold="True" HorizontalAlign="Center" ForeColor="White" />
                        <%--<PagerStyle BackColor="#F7F6F3" ForeColor="#5D7B9D" HorizontalAlign="left" cssclass="gridview">--%>
                        <PagerStyle BackColor="#F7F6F3" ForeColor="#5D7B9D" HorizontalAlign="left" />
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#E9E7E2" />
                        <SortedAscendingHeaderStyle BackColor="#506C8C" />
                        <SortedDescendingCellStyle BackColor="#FFFDF8" />
                        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
