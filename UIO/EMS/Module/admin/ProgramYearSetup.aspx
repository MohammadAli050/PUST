<%@ Page Title="Program Year Setup" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true" 
    CodeBehind="ProgramYearSetup.aspx.cs" Inherits="EMS.Module.admin.ProgramYearSetup" %>

<%@ Register Src="~/UserControls/ProgramUserControl.ascx" TagPrefix="uc1" TagName="ProgramUserControl" %>
<%@ Register Src="~/UserControls/DepartmentUserControl.ascx" TagPrefix="uc1" TagName="DepartmentUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Program Year Setup
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">

    <style type="text/css">
        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=80);
            opacity: 0.8;
            z-index: 10000;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContainer" runat="server">
<div>
    <div class="PageTitle">
        <label>Program Year Setup</label>
    </div>

    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <div class="Message-Area">
                <label class="msgTitle">Message: </label>
                <asp:Label runat="server" ID="lblMsg" Text="" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
        <ContentTemplate>
            <div class="Message-Area">
                <label class="display-inline field-Title" style="float:left">Department</label>
                <%--<asp:DropDownList ID="programDropDownList" width="250px" AutoPostBack="True" OnSelectedIndexChanged="programDropDownList_OnSelectedIndexChanged" runat="server"></asp:DropDownList>--%>
                <div style="float:left; margin-right:10px">
                    <uc1:DepartmentUserControl runat="server" ID="ucDepartment"  OnDepartmentSelectedIndexChanged="ucDepartment_DepartmentSelectedIndexChanged" />
                </div>
                <label class="display-inline field-Title" style="float:left">Program</label>
                <div style="float:left; margin-right:10px">
                    <uc1:ProgramUserControl runat="server" ID="ucProgram"  class="margin-zero dropDownList" />
                </div>
                            
                <asp:Button ID="btnLoad" runat="server" Text="Load" class="margin-zero btn-size" OnClick="btnLoad_Click" />
                <asp:Button ID="btnAddYear" runat="server" Text="Add New Year" OnClick="btnAddYear_Click" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div style="clear: both;"></div>

    <ajaxToolkit:UpdatePanelAnimationExtender
                ID="UpdatePanelAnimationExtender1"
                TargetControlID="UpdatePanel3"
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

    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
            <div>
                <asp:GridView ID="gvYearList" runat="server" AllowSorting="True" CssClass="table-bordered"                                       
                AutoGenerateColumns="False" ShowFooter="True" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                    <HeaderStyle BackColor="#1C5E55" ForeColor="White" Height="30" Font-Bold="True" />
                    <FooterStyle BackColor="#1C5E55" ForeColor="White" Height="30" Font-Bold="True" />
                    <AlternatingRowStyle BackColor="White" />
                    <RowStyle Height="25" />

                    <Columns>

                        <asp:TemplateField HeaderText="SL" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate><b><%# Container.DataItemIndex + 1 %></b></ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <HeaderStyle Width="40px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="YearId" Visible="false">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblYearId" Text='<%#Eval("YearId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <HeaderStyle Width="100px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="YearName">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblYearName" Text='<%#Eval("YearName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <HeaderStyle Width="100px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="ProgramName">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblName" Font-Bold="true" Text='<%#Eval("ProgramObj.ShortName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <HeaderStyle Width="300px" />
                        </asp:TemplateField>

                        <%--<asp:TemplateField HeaderText="Year Name">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblYearName" Text='<%#Eval("StudentAdditionalInformation.StudentYear.YearName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="100px" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Semester Name">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblSemesterName" Text='<%#Eval("StudentAdditionalInformation.StudentYearSemester.SemesterName") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="100px" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>--%>

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

    <div id="divProgress" style="display: none; z-index: 1000; position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%);">
        <asp:Image ID="LoadingImage" runat="server" ImageUrl="~/Images/Img/Waiting.gif" Height="150px" Width="150px" />
    </div>

     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Button ID="btnShowPopUp" runat="server" Style="display: none" />
                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="btnShowPopUp"  PopupControlID="pnlShowPopUp"
                        CancelControlID="btnCancel" BackgroundCssClass="modalBackground">
                </ajaxToolkit:ModalPopupExtender>
                <asp:Panel ID="pnlShowPopUp" runat="server" BackColor="#ffffff" Width="765px" Style="display: none; border-radius: 3px;">
                    <div style="padding: 5px;">
                        <fieldset style="padding: 5px; border: 2px solid #5D7B9D;">
                            <legend style="font-weight: 100; font-size: medium;  color:#5D7B9D; text-align: center">Program Year Insert / Edit</legend>
                            <div style="padding: 5px;">
                                <b>Program Year Insert / Edit</b><br />
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
                                            <asp:Label ID="lblYearId" runat="server" Visible="false" ></asp:Label>
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
                                            <asp:Label ID="lblYear" runat="server" CssClass="control-newlabel2" Text="Year"></asp:Label>   
                                        </td>
                                        <td class="auto-style9">      
                                            <asp:TextBox ID="txtYear" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                                <asp:Button ID="btnAddNewYear" runat="server" Text="Add" class="margin-zero btn-size" OnClick="btnAddNewYear_Click" />
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

