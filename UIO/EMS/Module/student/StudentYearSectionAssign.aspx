<%@ Page Title="Student Year Section Assign" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true" 
    CodeBehind="StudentYearSectionAssign.aspx.cs" Inherits="EMS.Module.student.StudentYearSectionAssign" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/ProgramUserControl.ascx" TagPrefix="uc1" TagName="ProgramUserControl" %>
<%@ Register Src="~/UserControls/AdmissionSessionUserControl.ascx" TagPrefix="uc1" TagName="AdmissionSessionUserControl" %>
<%@ Register Src="~/UserControls/DepartmentUserControl.ascx" TagPrefix="uc1" TagName="DepartmentUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Student Year Section Assign
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <style type="text/css">
        .modalBackground
        {
            background-color: #2a2d2a;
            filter: alpha(opacity=80);
            opacity: 0.8;
            z-index: 10000;
        }
        .modalPopup
        {
            background-color: #FFFFFF;
            border-width: 3px;
            border-style: solid;
            border-color: black;
            padding-top: 10px;
            padding-left: 10px;
            width: 300px;
            height: 140px;
        }
        .font
        {
            font-size: 12px;
        }

        .cursor
        {
            cursor: pointer;
        }

        /*.auto-style4 {
            width: 212px;
        }*/
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
<asp:Content ID="Content3" ContentPlaceHolderID="MainContainer" runat="server">
<div>
    <div class="PageTitle">
        <label>Student Year Section Assign</label>
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
                <table id="Table1" style="padding: 5px; width: 100%; height: 70px;" border="0" runat="server">
                        <tr>
                            <td class="auto-style4">Department : </b></td>
                            <td class="auto-style2">
                                <uc1:DepartmentUserControl runat="server" ID="ucDepartment" OnDepartmentSelectedIndexChanged="OnDepartmentSelectedIndexChanged" />
                           </td>
                            <td class="auto-style4"><b>Program : </b></td>
                            <td class="auto-style2">
                                <uc1:ProgramUserControl runat="server" ID="ucProgram" OnProgramSelectedIndexChanged="OnProgramSelectedIndexChanged" class="margin-zero dropDownList" />
                            </td>           
                        </tr>
                        <tr>
                            <td class="auto-style4"><asp:Label ID="Label8" runat="server" Text="Year No :"></asp:Label></td>
                            <td class="auto-style2">
                                <asp:DropDownList ID="ddlYearNo" Width="180" AutoPostBack="true" runat="server" ></asp:DropDownList>
                            </td>
                            <td class="auto-style4"><asp:Label ID="Label9" runat="server" Text="Semester No :"></asp:Label></td>
                            <td class="auto-style2">
                                <asp:DropDownList ID="ddlSemesterNo" Width="180"  AutoPostBack="true"  runat="server" ></asp:DropDownList>
                            </td>
                            <td class="auto-style4"><asp:Label ID="Label1" Width="120" runat="server" Text="Current Session :"></asp:Label></td>
                            <td class="auto-style2">
                                 <uc1:AdmissionSessionUserControl runat="server" ID="ucSession" class="margin-zero dropDownList"/>
                            </td>
                            <td class="auto-style2">
                                <asp:Button ID="btnLoad" runat="server" Text="Load" class="margin-zero btn-size" OnClick="btnLoad_Click" />
                            </td>
                         </tr>
                    </table>             
            </div>

            <asp:Panel ID="pnlAssign" runat="server">
               <div class="Message-Area">
                    <asp:Button ID="btnShowYearSection" runat="server" Text="  Assign Year Section  " OnClick="btnShowYearSection_Click" />                  
                </div>
            </asp:Panel>

            <cc1:ModalPopupExtender ID="mp1" runat="server" PopupControlID="Panel1" TargetControlID="btnShowYearSection"  BackgroundCssClass="modalBackground">
            </cc1:ModalPopupExtender>

            <asp:Panel ID="Panel1" runat="server" CssClass="modalPopup" Width="400px" Height="350px" align="center" style = "display:none">
                <b>Year Section Assign</b><br />
                <table>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblPopUpMsg" runat="server" Font-Bold="true" ForeColor="Red" Width="350px" Text=""></asp:Label>  
                        </td>
                    </tr>
                        
                    <tr>
                        <td>
                            <asp:Label ID="lblYearSection" runat="server" Width="150px" Text="Year Section : "></asp:Label>  
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlYearSection" Width="100px" runat="server"></asp:DropDownList>
                        </td>
                    </tr>
                        
                    <tr>
                        <td>
                            <asp:Button ID="btnAssignYearSection" runat="server" Text="Assign" class="margin-zero btn-size" OnClick="btnAssignYearSection_Click" />
                            <%--<asp:Button ID="btnUpdate" runat="server" Text="Update" Visible="false" OnClick="btnUpdate_Click" />--%>
                            <asp:Button ID="btnClose" runat="server" Text="Close" OnClick="btnClose_Click"/>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnShowYearSection" />
            <asp:PostBackTrigger ControlID="btnAssignYearSection" />
        </Triggers>
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
                <asp:GridView ID="gvStudentList" runat="server" AllowSorting="True" CssClass="table-bordered"                                       
                AutoGenerateColumns="False" ShowFooter="True" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                    <HeaderStyle BackColor="#1C5E55" ForeColor="White" Height="30" Font-Bold="True" />
                    <FooterStyle BackColor="#1C5E55" ForeColor="White" Height="30" Font-Bold="True" />
                    <AlternatingRowStyle BackColor="White" />
                    <RowStyle Height="25" />

                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkSelectAll" runat="server" Text="Select All"
                                    AutoPostBack="true" OnCheckedChanged="chkSelectAll_CheckedChanged" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div style="text-align: center">
                                    <asp:CheckBox runat="server" ID="ChkActive" Checked='<%#Eval("IsActive") %>'></asp:CheckBox>
                                </div>
                            </ItemTemplate>
                            <HeaderStyle Width="50px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="SL" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate><b><%# Container.DataItemIndex + 1 %></b></ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <HeaderStyle Width="40px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Student Id" ItemStyle-HorizontalAlign="Center" Visible="false">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblStudentID" Text='<%#Eval("StudentID") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <HeaderStyle Width="100px" />
                        </asp:TemplateField>

                       
                        <asp:TemplateField HeaderText="Student Id" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblStudnetRoll" Text='<%#Eval("Roll") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <HeaderStyle Width="100px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Name" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblName"  Text='<%#Eval("BasicInfo.FullName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                            <HeaderStyle Width="300px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Registration No" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblRegistrationNo"  Text='<%#Eval("StudentAdditionalInformation.RegistrationNo") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <HeaderStyle Width="100px" />
                        </asp:TemplateField>


                        <asp:TemplateField HeaderText="Year" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblYear"  Text='<%#Eval("StudentAdditionalInformation.YearNo") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <HeaderStyle Width="50px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Semester" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblSemester"  Text='<%#Eval("StudentAdditionalInformation.SemesterNo") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <HeaderStyle Width="100px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Admission Session" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblAdmissionSession"  Text='<%#Eval("AdmissionSession") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <HeaderStyle Width="100px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Current Session" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblCurrentSession"  Text='<%#Eval("CurrentSession") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <HeaderStyle Width="100px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Year Section" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblYearSection" Text='<%#Eval("StudentAdditionalInformation.StudentYearSection.Name") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="100px" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>



                        <%-- <asp:TemplateField HeaderText="Max Advised Number">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblAdvNumber" Text='<%#Eval("MaxNoTobeAdvised") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Width="100px" />
                </asp:TemplateField>--%>
                        <%--<asp:TemplateField HeaderText="Edit">
                            <ItemTemplate>
                                <div style="text-align: center">
                                    <asp:LinkButton runat="server" ToolTip="Edit" Text="Edit" Visible ="false" ID="lnkEdit"
                                        CommandArgument='<%#Eval("StudentID") %>'
                                        OnClick="lnkEdit_Click">
                                    </asp:LinkButton>
                                </div>
                            </ItemTemplate>
                            <HeaderStyle Width="100px" />
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
</div>
</asp:Content>
