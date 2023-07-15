<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true" CodeBehind="ResultHistoryUpload.aspx.cs" Inherits="EMS.Module.result.ResultHistoryUpload" %>

<%@ Register Src="~/UserControls/ProgramUserControl.ascx" TagPrefix="uc1" TagName="ProgramUserControl" %>
<%@ Register Src="~/UserControls/DepartmentUserControl.ascx" TagPrefix="uc1" TagName="DepartmentUserControl" %>
<%@ Register Src="~/UserControls/SessionUserControl.ascx" TagPrefix="uc1" TagName="SessionUserControl" %>
<%@ Register Src="~/UserControls/BatchUserControl.ascx" TagPrefix="uc1" TagName="BatchUserControl" %>


<asp:Content ID="Content4" ContentPlaceHolderID="Title" runat="server">
    Student Result Upload
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Head" runat="server">
    <style type="text/css">
        .auto-style1 {
            width: 71px;
        }

        .auto-style2 {
            width: 150px;
        }

        .auto-style4 {
            width: 130px;
        }

        .auto-style5 {
            width: 300px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="MainContainer" runat="server">

<div>
    <div class="PageTitle">
        <label>Student Result Upload</label>
    </div>
    <div>   
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <div class="Message-Area">
                    <label class="msgTitle">Message: </label>
                    <asp:Label runat="server" class="msgTitle" ID="lblMsg" ForeColor="Red" Text="" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <div style="clear: both;"></div>
        
        <div class="Message-Area">
            <asp:Panel ID="pnlFileUpload" runat="server">
                <table>
                    <tr>
                        <td>
                            <asp:FileUpload ID="UploadPanel" Width="400px" runat="server" />
                        </td>
                        <td>
                            <asp:Button ID="btnLoadSheet" runat="server" Text="Load Sheet" OnClick="LoadSheet_Click" />
                        </td>
                        <td colspan="3">
                            <asp:DropDownList runat="server" placeholder="Sheet Name" Width="130px" AutoPostBack="true" ID="ddlSheetName" OnSelectedIndexChanged="ddlSheetName_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:Label ID="Label1" runat="server" Text="File Name : "></asp:Label>
                            <asp:Label ID="lblFileName" Font-Bold="true" runat="server" Width="500px" ></asp:Label>
                            <%--<asp:Label ID="txtFileName" runat="server" Width="350px"></asp:Label>--%>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>

        <div class="Message-Area" style="height: auto">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <table style="width: 100%;" id="tblHeader">
                        <tr>
                            <td colspan="2" class="style1">
                                <table style="width: 100%;">
                                    <tr>
                                        <td class="auto-style4">
                                            <asp:Label  ID="Label4" runat="server" Text="Department : "></asp:Label>
                                        </td>
                                        <td class="auto-style2">
                                            <%--<asp:DropDownList ID="programDropDownList" width="250px" AutoPostBack="True" OnSelectedIndexChanged="programDropDownList_OnSelectedIndexChanged" runat="server"></asp:DropDownList>--%>
                                            <uc1:DepartmentUserControl runat="server" ID="ucDepartment"  OnDepartmentSelectedIndexChanged="ucDepartment_ProgramSelectedIndexChanged" />
                                        </td>
                                        <td class="auto-style4">
                                            <asp:Label  ID="Label3" runat="server" Text="Program : "></asp:Label>
                                        </td>
                                        <td class="auto-style2">
                                            <%--<asp:DropDownList ID="programDropDownList" width="250px" AutoPostBack="True" OnSelectedIndexChanged="programDropDownList_OnSelectedIndexChanged" runat="server"></asp:DropDownList>--%>
                                            <uc1:ProgramUserControl runat="server" ID="ucProgram" OnProgramSelectedIndexChanged="ucProgram_ProgramSelectedIndexChanged" />
                                        </td>
                                        <%--<td><asp:Label ID="Label2" runat="server" Text="Batch"></asp:Label></td>
                                        <td>
                                            <uc1:BatchUserControl runat="server" ID="ucBatch"/>
                                        </td>--%>
                                         <td class="auto-style4"><asp:Label ID="Label2" runat="server" Text="Course : "></asp:Label></td>
                                        <td class="auto-style2">
                                            <asp:DropDownList ID="ddlCourse" Width="250px" runat="server"></asp:DropDownList>
                                        </td>
                                        
                                    </tr>
                                    <tr>
                                        <td class="auto-style4"><asp:Label ID="Label8" runat="server" Text="Year No : "></asp:Label></td>
                                        <td class="auto-style2">
                                            <asp:DropDownList ID="ddlYearNo" Width="100px" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlYearNo_SelectedIndexChanged"></asp:DropDownList>
                                        </td>
                                        <td class="auto-style4"><asp:Label ID="Label9" runat="server" Text="Semester No : "></asp:Label></td>
                                        <td class="auto-style2">
                                            <asp:DropDownList ID="ddlSemesterNo" Width="150px"  AutoPostBack="true"  runat="server" OnSelectedIndexChanged="ddlSemesterNo_SelectedIndexChanged"></asp:DropDownList>
                                        </td>
                                         <td class="auto-style4"><asp:Label ID="Label10" runat="server" Text="Exam : "></asp:Label></td>
                                        <td class="auto-style2">
                                            <asp:DropDownList ID="ddlExam" runat="server"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                       
                                        <td class="auto-style4"><asp:Label ID="Label6" runat="server" Text="Year : "></asp:Label></td>
                                        <td class="auto-style2">
                                            <asp:DropDownList ID="ddlYear" Width="100px" AutoPostBack="true" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged" runat="server"></asp:DropDownList>
                                        </td>
                                        <td class="auto-style4"><asp:Label ID="Label7" runat="server" Text="Semester : "></asp:Label></td>
                                        <td class="auto-style2">
                                            <asp:DropDownList ID="ddlSemester" Width="150px" runat="server"></asp:DropDownList>
                                        </td>

                                       <td class="auto-style4"><asp:Label  ID="Label5" runat="server" Text="Result Session : "></asp:Label></td>
                                        <td class="auto-style2">
                                            <uc1:SessionUserControl runat="server" ID="ucSession"/>
                                        </td>
                                    </tr>

                                    <tr>
                                        
                                        <td class="auto-style4">
                                            <asp:Button ID="btnCheckStudentAvilability" runat="server" Text="Check Student" OnClick="btnCheckStudentAvilability_Click" />
                                        </td>
                                        <td class="auto-style2">
                                            <asp:Button ID="btnCheckCourseAvailability" runat="server" Text="Check Course" OnClick="btnCheckCourseAvailability_Click" />
                                        </td>
                                        <td class="auto-style4">
                                            <asp:Button ID="btnSaveToServer" runat="server" Text="Upload Result" OnClick="btnSaveToServer_Click" OnClientClick="return confirm('Are you sure? You want to upload result!');" />
                                        </td>
                                        <td></td>
                                        <td></td>
                                        <td>
                                            <asp:Button ID="btnDeleteData" runat="server" BackColor="Red" Text="Delete Result" OnClick="btnDeleteData_Click" OnClientClick="return confirm('Are you sure? You will not be able to view result of selected year, semester after delete!');" />
                                        </td>
                                   </tr>
                                </table>
                            </td>
                        </tr>
                    </table>

                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>


    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <asp:GridView ID="gvStudentResultInfo" runat="server" AllowSorting="True" CssClass="table-bordered"                                       
                 ShowFooter="True" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                <HeaderStyle BackColor="#1C5E55" ForeColor="White" Height="30" Font-Bold="True" />
                <FooterStyle BackColor="#1C5E55" ForeColor="White" Height="30" Font-Bold="True" />
                <AlternatingRowStyle BackColor="White" />
                <RowStyle Height="25" />

                <EmptyDataTemplate>
                    <label>Data Not Found</label>
                </EmptyDataTemplate>

                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkSelectAll" runat="server" Text="Select All"
                                AutoPostBack="true" OnCheckedChanged="chkSelectAll_CheckedChanged" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div style="text-align: center">
                                <asp:CheckBox runat="server" ID="ChkActive" ></asp:CheckBox>
                            </div>
                        </ItemTemplate>
                        <HeaderStyle Width="50px" />
                    </asp:TemplateField>
                </Columns>

                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />

                <RowStyle Height="25px" VerticalAlign="Middle" HorizontalAlign="Left" BackColor="#E3EAEB" />
                <EditRowStyle BackColor="#7C6F57" />
                <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#F8FAFA" />
                <SortedAscendingHeaderStyle BackColor="#246B61" />
                <SortedDescendingCellStyle BackColor="#D4DFE1" />
                <SortedDescendingHeaderStyle BackColor="#15524A" />
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
        
<asp:HiddenField ID="FilePathHiddenField" runat="server" />
<asp:HiddenField ID="FileNameHiddenField" runat="server" />
<asp:HiddenField ID="SheetNameHiddenField" runat="server" />
</asp:Content>




