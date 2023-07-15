<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true" 
    CodeBehind="StudentYearPromotion.aspx.cs" Inherits="EMS.Module.student.StudentYearPromotion" %>

<%@ Register Src="~/UserControls/ProgramUserControl.ascx" TagPrefix="uc1" TagName="ProgramUserControl" %>
<%@ Register Src="~/UserControls/AdmissionSessionUserControl.ascx" TagPrefix="uc1" TagName="AdmissionSessionUserControl" %>
<%@ Register Src="~/UserControls/DepartmentUserControl.ascx" TagPrefix="uc1" TagName="DepartmentUserControl" %>

<asp:Content ID="Content4" ContentPlaceHolderID="Title" runat="server">
    Student Year Promotion
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Head" runat="server">
    <style type="text/css">
        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=80);
            opacity: 0.8;
            z-index: 10000;
        }
        .marginTop {
            margin-top: -5px;
        }
        
    </style>

    <style type="text/css">
        .auto-style1 {
            width: 71px;
        }

        .auto-style2 {
            width: 150px;
        }

        .auto-style4 {
            width: 100px;
        }

        .auto-style5 {
            width: 300px;
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
<asp:Content ID="Content6" ContentPlaceHolderID="MainContainer" runat="server">
<div>
    <div class="PageTitle">
        <label>Student Year Promotion</label>
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
            <%--<div class="row Message-Area">
                <table>
                    <tr>
                        <td class="auto-style1"><label class="label-table" style="margin:5px 0;">Department</label></td>
                        <td class="auto-style14"><uc1:DepartmentUserControl runat="server" ID="ucDepartment"  OnDepartmentSelectedIndexChanged="OnDepartmentSelectedIndexChanged" /></td>
                        <td class="auto-style3"><label class="label-table" style="margin:5px 0;">Program</label></td>
                        <td class="auto-style15"><uc1:ProgramUserControl runat="server" ID="ucProgram" OnProgramSelectedIndexChanged="OnProgramSelectedIndexChanged" class="margin-zero dropDownList" /></td>                        
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td><label class="label-table" style="margin:5px 0;">Year</label></td>
                        <td class="auto-style14"><asp:DropDownList ID="ddlFilterYear" Width="100px" AutoPostBack="true" OnSelectedIndexChanged="ddlFilterYear_SelectedIndexChanged" runat="server"></asp:DropDownList></td>
                        <td><label class="label-table" style="margin:5px 0;">Semester</label></td>
                        <td class="auto-style15"><asp:DropDownList ID="ddlFilterSemester" Width="150px" runat="server"></asp:DropDownList></td>
                        <td><label class="label-table" style="margin:5px 0;">Current Session</label></td>
                        <td><uc1:AdmissionSessionUserControl runat="server" ID="ucFilterCurrentSession" class="margin-zero dropDownList" /></td>
                        <td><asp:Button ID="btnLoad" runat="server" Text="Load" class="margin-zero btn-size" OnClick="btnLoad_Click" Width="80px" Height="30px" /></td>
                    </tr>
                </table>                               
            </div>--%>
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
                                <uc1:AdmissionSessionUserControl runat="server" ID="ucFilterCurrentSession" class="margin-zero dropDownList"/>
                        </td>
                        <td class="auto-style2">
                            <asp:Button ID="btnLoad" runat="server" Text="Load" class="margin-zero btn-size" OnClick="btnLoad_Click" />
                        </td>
                    </tr>
                </table>
            </div>

            <asp:Panel ID="pnlAssign" runat="server">
                <div class="Message-Area">
                    <asp:Button ID="btnShowPromoteDemote" runat="server" Text=" Promoted/Not Promoted  " OnClick="btnShowPromoteDemote_Click" />
                    <asp:Button ID="btnShowCurrentSession" runat="server" Text="  Update Current Session  " OnClick="btnShowCurrentSession_Click" />                    
                </div>
            </asp:Panel>            
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
                <asp:GridView ID="gvStudentList" runat="server" AutoGenerateColumns="False" AllowPaging="false" CellPadding="4" Width="100%"
                        ShowHeader="true" ShowFooter="True" CssClass="table-bordered" ForeColor="#333333" GridLines="None">
                        <HeaderStyle BackColor="#1C5E55" ForeColor="White" Height="30" Font-Bold="True" />
                        <FooterStyle BackColor="#1C5E55" ForeColor="White" Height="30" Font-Bold="True" />
                        <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="text-center" >
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkSelectAll" runat="server" Text="Select All"
                                    AutoPostBack="true" OnCheckedChanged="chkSelectAll_CheckedChanged" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div style="text-align: center">
                                    <asp:CheckBox runat="server" ID="ChkActive" Checked='<%#Eval("IsActive") %>'></asp:CheckBox>
                                </div>
                            </ItemTemplate>
                            <HeaderStyle Width="5%" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderStyle-CssClass="text-center" HeaderText="SL" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate><b><%# Container.DataItemIndex + 1 %></b></ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <HeaderStyle Width="2%" />
                        </asp:TemplateField>

                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblStudentID" Text='<%#Eval("StudentID") %>'></asp:Label>
                            </ItemTemplate>                       
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Student Id" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblStudnetRoll" Text='<%#Eval("Roll") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="5%" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Name" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblName"  Text='<%#Eval("BasicInfo.FullName") %>'></asp:Label>
                            </ItemTemplate>                                                       
                            <HeaderStyle Width="20%" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Registration No" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblName2"  Text='<%#Eval("StudentAdditionalInformation.RegistrationNo") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                            <HeaderStyle Width="10%" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Phone" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblPhone"  Text='<%#Eval("BasicInfo.Phone") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="10%" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Year" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblYearName" Text='<%#Eval("StudentAdditionalInformation.YearNo") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="5%" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Semester" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblSemesterName" Text='<%#Eval("StudentAdditionalInformation.SemesterNo") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="5%" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Current Session" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblCurrentSemester" Text='<%#Eval("CurrentSession") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Width="5%" />
                        </asp:TemplateField>                    
                                              
                    </Columns>

                   <EmptyDataTemplate>
                        No data found!
                    </EmptyDataTemplate>
                    <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />

                    <RowStyle Height="25px" VerticalAlign="Middle" HorizontalAlign="Left" BackColor="#E3EAEB" />
                    <EditRowStyle BackColor="#7C6F57" />
                    <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#F8FAFA" />
                    <SortedAscendingHeaderStyle BackColor="#246B61" />
                    <SortedDescendingCellStyle BackColor="#D4DFE1" />
                    <SortedDescendingHeaderStyle BackColor="#15524A" />                              
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
            <ajaxToolkit:ModalPopupExtender
                ID="ModalPopupExtender1"
                runat="server"
                TargetControlID="btnShowPopup"
                PopupControlID="pnPopUp"
                CancelControlID="btnCancel"
                BackgroundCssClass="modalBackground">
            </ajaxToolkit:ModalPopupExtender>

            <asp:Panel runat="server" ID="pnPopUp" Style="display: none;">
               <div style="height: 250px; width: 500px; margin: 5px; background-color: Window;">
                   <fieldset style="padding:30px 10px; margin: 5px; border-color: lightgreen;">
                       <legend>Promote/Demote</legend>                    
                       <div style="padding: 0 5px; width: 100%">
                           <div class="row">
                               <div class="col-6">
                                   <label class="label-table">Year No</label>
                               </div>
                               <div class="col-6">
                                   <asp:DropDownList ID="ddlYearNoPopUp" Width="150px" runat="server"></asp:DropDownList></td>
                               </div>
                           </div>  
                           <div class="row">
                               <div class="col-6">
                                   <label class="label-table">Semester No</label>
                               </div>
                                <div class="col-6">
                                    <asp:DropDownList ID="ddlSemesterNoPopUp" Width="150px" runat="server"></asp:DropDownList></td>
                               </div>
                           </div> 
                           <div class="row" style="padding:30px 0;">
                               <div class="col-12" style="text-align:center;">
                                   <asp:Button ID="btnAssignYearSection" runat="server" Text="Promote/Demote" OnClick="btnAssignYearSection_Click" Width="140px" Height="30px" /></td>
                                   <asp:Button runat="server" style="width:140px; height:30px;" ID="btnCancel" Text="Cancel" />
                               </div>                               
                           </div>                                                
                       </div>
                   </fieldset>
               </div>           
            </asp:Panel>


            <asp:Button ID="Button1" runat="server" Style="display: none" />
            <ajaxToolkit:ModalPopupExtender
                ID="ModalPopupExtender2"
                runat="server"
                TargetControlID="Button1"
                PopupControlID="Panel1"
                CancelControlID="btnCurrentSessionCancel"
                BackgroundCssClass="modalBackground">
            </ajaxToolkit:ModalPopupExtender>


            <asp:Panel runat="server" ID="Panel1" Style="display: none;">
               <div style="height: 200px; width: 500px; margin: 5px; background-color: Window;">
                   <fieldset style="padding:30px 10px; margin: 5px; border-color: lightgreen;">
                       <legend>Current Session</legend>                    
                       <div style="padding: 0 5px; width: 100%">
                           <div class="row">
                               <div class="col-6">
                                   <label class="label-table">Current Session</label>
                               </div>
                               <div class="col-6">
                                   <uc1:AdmissionSessionUserControl runat="server" ID="ucCurrentSession" class="margin-zero dropDownList" OnSessionSelectedIndexChanged="ucCurrentSession_SessionSelectedIndexChanged" />
                               </div>
                           </div>                             
                           <div class="row" style="padding:30px 0;">
                               <div class="col-12" style="text-align:center;">
                                   <asp:Button ID="btnCurrentSessionAssign" runat="server" Text="Assign" Width="80px" Height="30px" OnClick="btnCurrentSessionAssign_Click" />
                                   <asp:Button runat="server" style="width:80px; height:30px;" ID="btnCurrentSessionCancel" Text="Cancel" />
                               </div>                               
                           </div>                                                
                       </div>
                   </fieldset>
               </div>           
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div id="divProgress" style="display: none; z-index: 999999; position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%);">
        <asp:Image ID="LoadingImage" runat="server" ImageUrl="~/Images/Img/Waiting.gif" Height="150px" Width="150px" />
    </div>


</div>
</asp:Content>
