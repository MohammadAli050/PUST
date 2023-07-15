<%@ Page Title="Course Removal" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true" 
    CodeBehind="UnRegistration.aspx.cs" Inherits="EMS.Module.registration.UnRegistration" %>
<%@ Register Src="~/UserControls/ProgramUserControl.ascx" TagPrefix="uc1" TagName="ProgramUserControl" %>
<%@ Register Src="~/UserControls/DepartmentUserControl.ascx" TagPrefix="uc1" TagName="DepartmentUserControl" %>
<%@ Register Src="~/UserControls/AdmissionSessionUserControl.ascx" TagPrefix="uc1" TagName="AdmissionSessionUserControl" %>
<%@ Register Src="~/UserControls/BatchUserControl.ascx" TagPrefix="uc1" TagName="BatchUserControl" %>
<%--<%@ Register Src="~/UserControls/SessionUserControl.ascx" TagPrefix="uc1" TagName="SessionUserControl" %>--%>


<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
Course Removal

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
     <script src="../../../JavaScript/jquery-1.6.1.min.js"></script>
    <script src="../../../JavaScript/jquery-1.7.1.js"></script>   
    <style>
           .checkbox2 label {
            display: inline;
            margin-left: 5px;
            height: 20px;
            vertical-align: middle;
            font-size: 1.0em;
        }

        .checkbox2 input {
            display: inline;
            margin-left: 15px;
            height: 25px;
            width: 25px;
            vertical-align: middle;
        }
    </style> 
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContainer" runat="server">
    <div>
         <div class="PageTitle">
        <label>Course Removal from Registration</label>
    </div>
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
                                        <td class="auto-style2" style="padding-right:20px;">
                                            <%--<asp:DropDownList ID="programDropDownList" width="250px" AutoPostBack="True" OnSelectedIndexChanged="programDropDownList_OnSelectedIndexChanged" runat="server"></asp:DropDownList>--%>
                                            <uc1:DepartmentUserControl runat="server" ID="ucDepartment" OnDepartmentSelectedIndexChanged="OnDepartmentSelectedIndexChanged" />
                                        </td>
                                        <td class="auto-style4">
                                            <asp:Label  ID="Label3" runat="server" Text="Program : "></asp:Label>
                                        </td>
                                        <td class="auto-style2" style="width:400px;">
                                            <%--<asp:DropDownList ID="programDropDownList" width="250px" AutoPostBack="True" OnSelectedIndexChanged="programDropDownList_OnSelectedIndexChanged" runat="server"></asp:DropDownList>--%>
                                            <uc1:ProgramUserControl runat="server"  ID="ucProgram" OnProgramSelectedIndexChanged="OnProgramSelectedIndexChanged"   />
                                        </td>
                                        <%--<td><asp:Label ID="Label2" runat="server" Text="Batch"></asp:Label></td>
                                        <td>
                                            <uc1:BatchUserControl runat="server" ID="ucBatch"/>
                                        </td>--%>
                                       
                                
                                    </tr>
                                    <tr>
                                        <%--<td class="auto-style4"><asp:Label ID="Label8" runat="server" Text="Final Exam : "></asp:Label></td>
                                        <td class="auto-style2">
                                            <asp:DropDownList ID="ddlFinalExam" Width="150px" runat="server"></asp:DropDownList>
                                        </td>
                                        <td class="auto-style4"><asp:Label ID="Label6" runat="server" Text="Year : "></asp:Label></td>
                                        <td class="auto-style2">
                                            <asp:DropDownList ID="ddlYear" Width="100px" AutoPostBack="true" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged" runat="server"></asp:DropDownList>
                                        </td>
                                        <td class="auto-style4"><asp:Label ID="Label7" runat="server" Text="Semester : "></asp:Label></td>
                                        <td class="auto-style2">
                                            <asp:DropDownList ID="ddlSemester" Width="150px" runat="server"></asp:DropDownList>
                                        </td>--%>
                                          <tr style="height:50px;">
                        <%--<td class="auto-style4">
                            <asp:Label ID="Label3" Width="120px" runat="server" Text="Versity Session"></asp:Label>
                        </td>
                        <td class="auto-style5">
                            <uc1:AdmissionSessionUserControl Width="150px" runat="server" ID="ucVersitySession" class="margin-zero dropDownList"/>
                        </td>--%>                        
                        <td class="auto-style4"><asp:Label ID="Label8" runat="server" Text="Year : "></asp:Label></td>
                        <td class="auto-style6">
                            <asp:DropDownList ID="ddlYearNo" Width="150px" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlYearNo_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                        <td class="auto-style4"><asp:Label ID="Label9" runat="server" Text="Semester : "></asp:Label></td>
                        <td class="auto-style6" style="width:30px;">
                            <asp:DropDownList ID="ddlSemesterNo" Width="150px"  AutoPostBack="true"  runat="server" OnSelectedIndexChanged="ddlSemesterNo_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                        <td class="auto-style4">
                            <asp:Label ID="Label10" runat="server" Text="Exam : "></asp:Label>
                        </td>
                        <td class="auto-style6">
                            <asp:DropDownList ID="ddlExam" Width="350px" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlExam_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                 
                    </tr>    
                                    </tr>
                                    <tr>
                                            <td class="auto-style4">
                            <asp:Label ID="Label1" runat="server" Text="Course : "></asp:Label>
                        </td>
                                           <td class="auto-style6">
                            <asp:DropDownList ID="ddlCourse" Width="350px" runat="server" AutoPostBack="true" ></asp:DropDownList>
                        </td>
                                         <td class="auto-style4">
                                <asp:Button runat="server" ID="btnLoad" Text="Load" OnClick="btnLoad_Click" Height="35px" Width="70px" />
                            </td>
                                    </tr>
                               
                                </table>
                            </td>
                        </tr>
                    </table>

                      <div class="Message-Area" style="margin-bottom: 20px;">
                        <label class="msgTitle">Message: </label>
                        <asp:Label runat="server" ID="lblDeleteMsg" Text="" Visible="false" />
                    </div>


                       <table>
                 
                           <asp:CheckBoxList ID="chkStudentList" runat="server" Width="800">
                           </asp:CheckBoxList>
                               <tr>
                                <asp:Button runat="server" ID="stdButton"  Text="Remove" OnClick="btnLoad_StudentSubmit" OnClientClick="return confirm('Are you sure?');" Height="35px" Width="100px" Visible="false" />
                    </tr>
                </table>
                </ContentTemplate>
            </asp:UpdatePanel>

        <div class="divInner1">
            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                <ContentTemplate>
                    <asp:GridView runat="server" ID="gvStudentList" AutoGenerateColumns="False"
                        ShowHeader="true" CssClass="gridCss" Width="80%">
                        <HeaderStyle BackColor="#4285f4" ForeColor="White" />
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>


                            <asp:TemplateField HeaderText="SL" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate><b><%# Container.DataItemIndex + 1 %></b></ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Student ID">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblRoll" Font-Bold="true" Text='<%#Eval("Roll") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle Width="120px" HorizontalAlign="Center" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>

                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkSelectAll" runat="server" Text="All"
                                        AutoPostBack="true" CssClass="checkbox2"  OnCheckedChanged="chkSelectAll_CheckedChanged" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div style="text-align: center">
                                        <asp:CheckBox runat="server" CssClass="checkbox2"  ID="ChkSelect" ></asp:CheckBox>
                                    </div>
                                </ItemTemplate>
                                <HeaderStyle Width="100px" />
                            </asp:TemplateField>

                        </Columns>

                        <RowStyle Height="25px" VerticalAlign="Middle" HorizontalAlign="Left" />
                        <EmptyDataTemplate>
                            No data found!
                        </EmptyDataTemplate>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
     </div>
     <div>
        
         <div id="divProgress" style="display: none; z-index: 1000; position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%);">
                    <asp:Image ID="LoadingImage" runat="server" ImageUrl="~/Images/Img/Waiting.gif" Height="150px" Width="150px" />
                </div>
    </div>
</asp:Content>