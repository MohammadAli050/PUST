<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true" CodeBehind="BillDelete.aspx.cs" Inherits="EMS.Module.bill.BillDelete" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Bill Delete
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">

    <%--<link href="../../ContentCSS/CSS/ChildSiteMaster.CSS" rel="stylesheet" />--%>

    <style type="text/css">
        
    </style>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContainer" runat="server">
    <div class="PageTitle">
        <label>Bill Delete</label>
    </div>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="Message-Area">
                <asp:Label ID="Label1" runat="server" Text="Message : "></asp:Label>
                <asp:Label ID="lblMsg" ForeColor="Red" runat="server"></asp:Label>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <div class="Message-Area">
                <table>
                    <tr>
                        <td class="auto-style4">
                            <b>Program:</b>
                        </td>
                        <td class="auto-style6">
                            <asp:DropDownList ID="programDropDownList" runat="server" Width="150px" AutoPostBack="True" OnSelectedIndexChanged="programDropDownList_OnSelectedIndexChanged"></asp:DropDownList>
                        </td>

                        <td class="auto-style4">
                            <b>Admission Session:</b>
                        </td>
                        <td class="auto-style6">
                            <asp:DropDownList ID="admissionSessionDropDownList" runat="server" Width="150px" AutoPostBack="True" ></asp:DropDownList>
                            <%--<asp:DropDownList ID="batchDropDownList" runat="server" Width="150px" AutoPostBack="True"></asp:DropDownList>--%>
                        </td>

                        <td class="auto-style4">
                            <b>Session:</b>
                        </td>
                        <td class="auto-style6">
                            <asp:DropDownList ID="sessionDropDownList" runat="server" Width="150px" AutoPostBack="True" ></asp:DropDownList>
                        </td>

                        <td class="auto-style4">
                            <b>Student Roll:</b>
                        </td>
                        <td class="auto-style6">
                            <asp:TextBox ID="txtStudentRoll" PlaceHolder="Student Roll" runat="server" Width="100px"></asp:TextBox>
                        </td>

                        <td class="auto-style4">
                            <asp:Button ID="btnLoad" runat="server" Text="Load Bill" OnClick="btnLoad_Click" />
                        </td>
                        
                        <td class="auto-style4">
                            <asp:Button ID="btnDelete" Visible="False" runat="server" Text="Load Payment" OnClick="btnDelete_Click"  />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
            <div class="Message-Area">
                <asp:Label ID="Label2" Font-Bold="True" Font-Underline="True" Text="Message: " runat="server" ></asp:Label>
                <asp:Label ID="infoLabel" Font-Bold="True" Font-Underline="True" ForeColor="red" runat="server" ></asp:Label>
                <br/>
                <asp:GridView ID="deleteBillGridView" runat="server" AutoGenerateColumns="False" CssClass="table-bordered"
                    EmptyDataText="No data found." AllowPaging="false" CellPadding="4">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:TemplateField HeaderText="SI." ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate><b><%# Container.DataItemIndex + 1 %></b></ItemTemplate>
                            <HeaderStyle Width="30px" />
                        </asp:TemplateField>

                        <asp:TemplateField Visible="false" HeaderText="Id">
                            <ItemTemplate>
                                <asp:Label ID="lblBillHistoryMasterId" runat="server" Text='<%# Bind("BillHistoryMasterId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <HeaderStyle Width="150px" />
                        </asp:TemplateField>

                        <asp:TemplateField Visible="false" HeaderText="StudentId">
                            <ItemTemplate>
                                <asp:Label ID="lblStudentId" runat="server" Text='<%# Bind("StudentId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle Width="150px" />
                        </asp:TemplateField>

                        <asp:BoundField DataField="Roll" HeaderText="Student Roll">
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle Width="100px" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Name" HeaderText="Student Name">
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle Width="350px" />
                        </asp:BoundField>

                        <asp:TemplateField Visible="True">
                            <HeaderTemplate>
                                <asp:Button ID="deleteButton" runat="server" Text="Delete" Font-Bold="True" OnClick="deleteButton_OnClick" OnClientClick="return confirm('Are you sure to DELETE this bill !!')"/>
                                <hr/>
                                <asp:CheckBox runat="server" ID="chkAllStudentHeader" OnCheckedChanged="chkAllStudent_CheckedChanged" Text="" AutoPostBack="true"></asp:CheckBox>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="CheckBox" runat="server" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:BoundField DataField="ReferenceNo" HeaderText="Reference No">
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle Width="100px" />
                        </asp:BoundField>

                        <asp:BoundField DataField="Amount" HeaderText="Amount">
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle Width="100px" />
                        </asp:BoundField>

                        <asp:BoundField DataField="BillingDate" HeaderText="Bill Date" DataFormatString="{0:dd-MMM-yy}">
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle Width="100px" />
                        </asp:BoundField>

                    </Columns>
                    <EditRowStyle BackColor="#999999" />
                    <FooterStyle BackColor="#b264fb" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#4285f4" HorizontalAlign="Center" Font-Bold="True" ForeColor="White" />
                    <%--<PagerStyle BackColor="#F7F6F3" ForeColor="#5D7B9D" HorizontalAlign="left" cssclass="gridview">--%>
                    <PagerStyle BackColor="#F7F6F3" ForeColor="#5D7B9D" HorizontalAlign="left" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    
<ajaxToolkit:UpdatePanelAnimationExtender ID="UpdatePanelAnimationExtender1" TargetControlID="UpdatePanel2" runat="server">
    <Animations>
        <OnUpdating>
            <Parallel duration="0">
                <ScriptAction Script="InProgress();" />
                <EnableAction AnimationTarget="btnPostPayment" Enabled="false" />                   
            </Parallel>
        </OnUpdating>
        <OnUpdated>
            <Parallel duration="0">
                <ScriptAction Script="onComplete();" />
                <EnableAction   AnimationTarget="btnPostPayment" Enabled="true" />
            </Parallel>
        </OnUpdated>
    </Animations>
</ajaxToolkit:UpdatePanelAnimationExtender>

</asp:Content>

