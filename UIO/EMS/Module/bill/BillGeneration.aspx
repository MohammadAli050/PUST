<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true" CodeBehind="BillGeneration.aspx.cs" Inherits="EMS.Module.bill.BillGeneration" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="Server">
    Bill Generation
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="Server">

    <style>
        .msgPanel {
            margin-top: 20px;
            margin-bottom: 25px;
            border: 1px solid #aaa;
            background-color: #f9f9f9;
            padding: 5px;
        }

        .auto-style1 {
            width: 196px;
        }

        .auto-style2 {
            width: 231px;
        }

        .auto-style3 {
            width: 57px;
        }

        .auto-style4 {
            width: 47px;
        }

        .auto-style5 {
            width: 61px;
        }

        .auto-style6 {
            width: 182px;
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

    </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContainer" runat="Server">
    <div style="padding: 5px; width: 100%;">
        <div class="PageTitle">
            <label>Bill Generation</label>
        </div>
        <%--<asp:Button ID="testButton" runat="server" Text="NC Calculate" OnClick="testButton_Click" />--%>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pnlMessage" runat="server" Visible="true" CssClass="msgPanel">
                    <asp:Label ID="Label1" runat="server" Text="Message : " Font-Bold="true"></asp:Label>
                    <asp:Label ID="lblMessage" runat="server"></asp:Label>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>

        <div class="Message-Area" style="width: 100%;">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div>
                        <table style="padding: 5px; width: 100%;">
                            <tr>
                                <td class="auto-style5">Program</td>
                                <td class="auto-style2">
                                    <asp:DropDownList ID="programDropDownList" runat="server" Width="250px" AutoPostBack="True" OnSelectedIndexChanged="programDropDownList_SelectedIndexChanged"></asp:DropDownList>
                                </td>

                                <td class="auto-style3">Session</td>
                                <td class="auto-style1">
                                    <asp:DropDownList ID="sessionDropDownList" runat="server" Width="200px" AutoPostBack="True" OnSelectedIndexChanged="sessionDropDownList_SelectedIndexChanged"></asp:DropDownList>
                                </td>

                                <%--<td class="auto-style4">Batch</td>
                                <td class="auto-style6">
                                    <asp:DropDownList ID="admissionSessionDropDownList" runat="server" Width="200px" AutoPostBack="True" OnSelectedIndexChanged="batchDropDownList_SelectedIndexChanged"></asp:DropDownList>
                                </td>--%>


                                <td>
                                    <asp:Button ID="loadButton" runat="server" Width="100px" Text="Load" OnClick="loadButton_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="divProgress" style="display: none; width: 195px; float: right; margin: -95px -140px 0 0;">
                        <div style="float: left">
                            <asp:Image ID="LoadingImage" runat="server" ImageUrl="~/Images/Img/Waiting.gif" Height="50px" Width="50px" />
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>


        <div style="clear: both;"></div>

        <ajaxToolkit:UpdatePanelAnimationExtender
            ID="UpdatePanelAnimationExtender1"
            TargetControlID="UpdatePanel2"
            runat="server">
            <Animations>
            <OnUpdating>
               <Parallel duration="0">
                    <ScriptAction Script="InProgress();" />
                    <EnableAction AnimationTarget="btnLoad" 
                                  Enabled="false" />                   
                </Parallel>
            </OnUpdating>
            <OnUpdated>
                <Parallel duration="0">
                    <ScriptAction Script="onComplete();" />
                    <EnableAction   AnimationTarget="btnLoad" 
                                    Enabled="true" />
                </Parallel>
            </OnUpdated>
            </Animations>
        </ajaxToolkit:UpdatePanelAnimationExtender>

        <div>


            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>
                    <asp:Label ID="totalStudentLabel" Visible="False" runat="server" Font-Bold="true" Text="Total Student : "></asp:Label>
                    <asp:Label ID="lblCount" runat="server" Font-Bold="true"></asp:Label>

                    <asp:GridView runat="server" ID="StudentListGridView" AutoGenerateColumns="False" AllowPaging="false" PageSize="20"
                        Width="80%" ShowHeader="true" CssClass="gridCss">
                        <HeaderStyle BackColor="#737CA1" ForeColor="White" />
                        <AlternatingRowStyle BackColor="#F0F8FF" />
                        <Columns>
                            <asp:TemplateField HeaderText="SL" HeaderStyle-Width="4%">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Roll" >
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="rollLabel" Font-Bold="true" Text='<%#Eval("Roll") %>'></asp:Label>
                                    <asp:HiddenField ID="studentIdHiddenField" runat="server" Value='<%#Eval("StudentID") %>' />
                                    <asp:HiddenField ID="programIdHiddenField" runat="server" Value='<%#Eval("ProgramID") %>' />
                                    <%--<asp:HiddenField ID="sessionIdHiddenField" runat="server" Value='<%#Eval("RegSession") %>' />--%>
                                    <asp:HiddenField ID="studentAdmissionAcaCalIdHiddenField" runat="server" Value='<%#Eval("StudentAdmissionAcaCalId") %>' />
                                </ItemTemplate>
                                <HeaderStyle Width="200px" />
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Name">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="nameLabel" Text='<%#Eval("Name") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left"/>
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Button ID="createBillButton" runat="server" Text="Create Bill" OnClick="createBillButton_OnClick" />
                                    <hr />
                                    <asp:CheckBox ID="selectAllCheckBox" runat="server" AutoPostBack="true" OnCheckedChanged="selectAllCheckBox_OnCheckedChanged" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div style="text-align: center">
                                        <asp:CheckBox ID="itemCheckBox" runat="server" />
                                    </div>
                                </ItemTemplate>
                                <HeaderStyle Width="130px" />
                            </asp:TemplateField>
                        </Columns>
                        <RowStyle Height="25px" VerticalAlign="Middle" HorizontalAlign="Left" />
                        <EmptyDataTemplate>
                            No data found!
                        </EmptyDataTemplate>
                        <%-- <HeaderStyle CssClass="tableHead" />--%>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

    </div>
</asp:Content>

