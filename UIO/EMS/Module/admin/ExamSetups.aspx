<%@ Page Title="Exam and Committee Setup" Language="C#"
    MasterPageFile="~/MasterPage/Common/CommonMasterPage.master"
    AutoEventWireup="true"
    CodeBehind="ExamSetups.aspx.cs"
    Inherits="EMS.Module.admin.ExamSetups" %>

<%@ Register Src="~/UserControls/ProgramUserControl.ascx" TagPrefix="uc1" TagName="ProgramUserControl" %>
<%@ Register Src="~/UserControls/AdmissionSessionUserControl.ascx" TagPrefix="uc1" TagName="AdmissionSessionUserControl" %>
<%@ Register Src="~/UserControls/DepartmentUserControl.ascx" TagPrefix="uc1" TagName="DepartmentUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Exam and Committee Setup
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <style type="text/css">
        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=80);
            opacity: 0.8;
            z-index: 10000;
        }

        .dropdown {
            width: 266px;
        }



        .select2-results__option {
            line-height: 20px !important;
            height: 34px !important;
        }

        .select2-container {
            width: 306px !important;
        }
    </style>


    <link href="../../CSS/select2.min.css" rel="stylesheet" />
    <script src="../../JavaScript/select2.min.js"></script>





</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContainer" runat="server">

    <div class="container-fluid">
        <div class="col-md-12">
            <div class="h2 text-primary pt-2 border-bottom">Exam and Committee Setup</div>
        </div>
        <div class="col-md-12">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="pnlMessage" runat="server" Visible="true" CssClass="">
                        <asp:Label ID="Label1" runat="server" Text="Message : "></asp:Label>
                        <asp:Label ID="lblMsg" runat="server"></asp:Label>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="col-md-12">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="card mt-2">
                                <div class="card-body">
                                    <div class="form-row">
                                        <div class="col-md-12 row">
                                            <div class="col-4 mt-2">
                                                <asp:Label ID="Label8" runat="server" Text="Department: "></asp:Label>
                                            </div>
                                            <div class="col-8 mb-2">
                                                <uc1:DepartmentUserControl runat="server" ID="ucDepartment" OnDepartmentSelectedIndexChanged="OnDepartmentSelectedIndexChanged" />

                                            </div>
                                            <div class="col-4 mt-2">
                                                <asp:Label ID="Label3" runat="server" Text="Program: "></asp:Label>

                                            </div>
                                            <div class="col-8 mb-2">
                                                <uc1:ProgramUserControl runat="server" ID="ucProgram" />

                                            </div>
                                            <div class="col-4 mt-2">
                                                <asp:Label ID="Label4" runat="server" Text="Year: "></asp:Label>

                                            </div>
                                            <div class="col-8 mb-2">
                                                <asp:DropDownList ID="ddlYear" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="card mt-2">
                                <div class="card-body">
                                    <div class="form-row">
                                        <div class="col-md-12 row">
                                            <div class="col-4 mt-2">
                                                <asp:Label ID="Labelsn" runat="server" Text="Semester No: "></asp:Label>

                                            </div>
                                            <div class="col-8 mb-2">
                                                <asp:DropDownList ID="ddlSemesterNo" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>

                                            </div>
                                        </div>
                                        <div class="col-md-12 row">
                                            <div class="col-4 mt-2">
                                                <asp:Label ID="Label5" runat="server" Text="Exam Year: "></asp:Label>

                                            </div>
                                            <div class="col-8 mb-2">
                                                <asp:DropDownList ID="ddlShal" runat="server" Width="100%" CssClass="form-control"></asp:DropDownList>

                                            </div>
                                        </div>
                                        <div class="col-md-12 row">
                                            <div class="col-4 mt-2">
                                                <asp:Label ID="Label6" runat="server" Text="Session: "></asp:Label>

                                            </div>
                                            <div class="col-8 mb-2">
                                                <uc1:AdmissionSessionUserControl runat="server" ID="ucAdmissionSession" />

                                            </div>
                                        </div>
                                        <div class="col-md-12 row">
                                            <div class="col-4 mt-2">
                                                <asp:Button ID="loadButton" runat="server" AutoPostBack="True" CssClass="btn btn-sm btn-primary w-100" Text="Load" OnClick="loadButton_Click" />

                                            </div>
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div class="col-md-12">
            <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                <ContentTemplate>
                    <div class="card mt-2">
                        <div class="card-body row">
                            <div class="col-2">
                                <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-sm w-100 btn-secondary" OnClick="btnAdd_Click" Text="Add New"></asp:Button>
                            </div>
                            <div class="col-3">
                                <asp:Button ID="btnAddCommittee" runat="server" CssClass="btn btn-sm btn-secondary w-100" Text="Bulk Committee (Add/Edit)" OnClick="btnAddCommittee_Click" Visible="false" />
                            </div>
                        </div>
                    </div>

                    <%--<div class="row">
                        <div class="col-lg-3">
                            
                        </div>
                        <div class="col-sm-9">
                            <asp:Panel ID="panelShowHideBulkCommittee" runat="server">
                                    
                                </asp:Panel>
                        </div>
                    </div>--%>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <%--=================================== Grid View =================================--%>
        <div class="col-md-12">
            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>
                    <div class="card mt-2">
                        <div class="card-body row">
                            <div class="col-12">
                                <asp:GridView runat="server" ID="gvExamSetup" AllowSorting="True" CssClass="table table-bordered"
                                    AutoGenerateColumns="False" ShowFooter="True" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                                    <HeaderStyle BackColor="#4285f4" ForeColor="White" Height="30" Font-Bold="True" />
                                    <FooterStyle BackColor="#1C5E55" ForeColor="White" Height="30" Font-Bold="True" />
                                    <AlternatingRowStyle BackColor="White" />
                                    <RowStyle Height="25" />

                                    <Columns>
                                        <asp:TemplateField HeaderText="ExmaSetupId" HeaderStyle-Width="350px" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblExmaSetupId" Text='<%#Eval("ExamSetupID") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="350" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ExamSetupDetailId" HeaderStyle-Width="350px" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblExamSetupDetailId" Text='<%#Eval("ExamSetupDetailId") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="350" />
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <div style="text-align: center">
                                                    SL.
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div style="text-align: center">
                                                    <%#Container.DataItemIndex+1 %>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <div style="text-align: center">
                                                    <asp:CheckBox ID="cbSelectAll" runat="server"
                                                        AutoPostBack="true" OnCheckedChanged="cbSelectAll_CheckedChanged" />
                                                    All                                       
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div style="text-align: center">
                                                    <asp:CheckBox runat="server" ID="cbSelect"></asp:CheckBox>
                                                </div>
                                            </ItemTemplate>
                                            <HeaderStyle Width="100px" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Program" HeaderStyle-Width="350px">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblProgramName" Text='<%#Eval("ProgramName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="350" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Exam Information">
                                            <ItemTemplate>
                                                <div>
                                                    <span style="font-weight: bold;">Year:</span><asp:Label runat="server" ID="lblYearNo" Text='<%#Eval("YearNoName") %>'></asp:Label>
                                                </div>
                                                <div>
                                                    <span style="font-weight: bold;">Semester:</span>
                                                    <asp:Label runat="server" ID="lblSemesterNo" Text='<%#Eval("SemesterNoName") %>'></asp:Label>
                                                </div>
                                                <div>
                                                    <span style="font-weight: bold;">Exam Year:</span>
                                                    <asp:Label runat="server" ID="lblShal" Text='<%#Eval("Shal") %>'></asp:Label>
                                                </div>
                                                <div>
                                                    <span style="font-weight: bold;">Exam:</span>
                                                    <asp:Label runat="server" ID="lblExamName" Text='<%#Eval("ExamName") %>'></asp:Label>
                                                </div>
                                                <div>
                                                    <span style="font-weight: bold;">Session:</span>
                                                    <asp:Label runat="server" ID="lblSession" Text='<%#Eval("FullCode") %>'></asp:Label>
                                                </div>
                                            </ItemTemplate>
                                            <HeaderStyle />
                                        </asp:TemplateField>

                                        <%--<asp:TemplateField HeaderText="Year No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblYearNo" Text='<%#Eval("YearNoName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Semester No" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblSemesterNo" Text='<%#Eval("SemesterNoName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Shal" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblShal" Text='<%#Eval("Shal") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Exam Name" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblExamName" Text='<%#Eval("ExamName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Session" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblSession" Text='<%#Eval("FullCode") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle />
                                </asp:TemplateField>--%>

                                        <asp:TemplateField HeaderText="Committees">
                                            <ItemTemplate>
                                                <div>
                                                    <span style="font-weight: bold;">Chairman:</span>
                                                    <asp:Label runat="server" ID="lblChairmanName" Text='<%#Eval("ChairmanName") %>'></asp:Label>
                                                </div>
                                                <div>
                                                    <span style="font-weight: bold;">Member One:</span>
                                                    <asp:Label runat="server" ID="lblMemberOneName" Text='<%#Eval("MemberOneName") %>'></asp:Label>
                                                </div>
                                                <div>
                                                    <span style="font-weight: bold;">Member Two:</span>
                                                    <asp:Label runat="server" ID="lblMemberTwoName" Text='<%#Eval("MemberTwoName") %>'></asp:Label>
                                                </div>
                                                <div>
                                                    <span style="font-weight: bold;">External:</span>
                                                    <asp:Label runat="server" ID="lblExternalMemberName" Text='<%#Eval("ExternalMemberName") %>'></asp:Label>
                                                </div>
                                            </ItemTemplate>
                                            <HeaderStyle Width="390px" />
                                        </asp:TemplateField>

                                        <%--<asp:TemplateField HeaderText="Chairman">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblChairmanName" Text='<%#Eval("ChairmanName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Member One">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblMemberOneName" Text='<%#Eval("MemberOneName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Member Two">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblMemberTwoName" Text='<%#Eval("MemberTwoName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle />
                                </asp:TemplateField>--%>

                                        <asp:TemplateField HeaderText="Action" HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnEdit" runat="server" OnClick="btnEdit_Click" Text="Edit Exam"
                                                    ToolTip="Item Edit" CommandArgument='<%#Eval("ExamSetupID")+","+ Eval("ExamSetupDetailId")+","+ Eval("SemesterNo") +","+ Eval("ExamSetupWithExamCommitteesId")%>'>                                                
                                                </asp:LinkButton>
                                                |<br />
                                                <asp:LinkButton ID="btnDelete" runat="server" OnClick="btnDelete_Click" Text="Delete Exam"
                                                    OnClientClick="return confirm('Are you sure to Delete this ?')"
                                                    ToolTip="Item Delete" CommandArgument='<%#Eval("ExamSetupID")+","+ Eval("ExamSetupDetailId")+","+ Eval("SemesterNo") +","+ Eval("ExamSetupWithExamCommitteesId")%>'>                                                
                                                </asp:LinkButton>
                                                |<br />
                                                <asp:LinkButton ID="btnEditCommittee" runat="server" OnClick="btnEditCommittee_Click" Text="Edit Committee"
                                                    ToolTip="Item Edit" CommandArgument='<%#Eval("ExamSetupID")+","+ Eval("ExamSetupDetailId")+","+ Eval("SemesterNo")+","+ Eval("ExamSetupWithExamCommitteesId")%>'>                                                
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle Width="125px" />
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
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <%--=================================== END Grid View =================================--%>


        <%--=================================== POP UP MODAL Exam Setup =================================--%>
        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
                <asp:Button ID="btnShowPopUp" runat="server" Style="display: none" />
                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="btnShowPopUp" PopupControlID="pnlShowPopUp"
                    CancelControlID="btnCancel" BackgroundCssClass="modalBackground">
                </ajaxToolkit:ModalPopupExtender>
                <asp:Panel ID="pnlShowPopUp" runat="server" BackColor="#ffffff" Width="765px" Style="display: none; border-radius: 3px;">
                    <div style="padding: 5px;">
                        <fieldset style="padding: 5px; border: 2px solid #5D7B9D;">
                            <legend style="font-weight: 100; font-size: medium; color: #5D7B9D; text-align: center">Exam Setup Insert / Edit</legend>
                            <div style="padding: 5px;">
                                <b>Exam Setup Insert / Edit</b><br />
                                <div class="Message-Area">
                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                        <ContentTemplate>
                                            <asp:Panel ID="Panel1" runat="server" Visible="true">
                                                <asp:Label ID="Label2" runat="server" Text="Message : "></asp:Label>
                                                <asp:Label ID="lblMessage" runat="server" ForeColor="#CC0000"></asp:Label>
                                            </asp:Panel>


                                            <asp:HiddenField ID="hfExamSetupId" runat="server" />
                                            <asp:HiddenField ID="hfExamSetupDetailId" runat="server" />
                                            <asp:HiddenField ID="hfSemesterNo" runat="server" />

                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>

                                <table>
                                    <tr>
                                        <td class="auto-style8">
                                            <asp:Label ID="Label13" runat="server" Text="Department <span style='color:red;font-weight:bold'>*</span>"></asp:Label>
                                        </td>
                                        <td class="auto-style9">
                                            <uc1:DepartmentUserControl runat="server" ID="ucDepartmentModal" OnDepartmentSelectedIndexChanged="OnDepartmentSelectedIndexChangedModal" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style8">
                                            <asp:Label ID="lblProgramModal" runat="server" CssClass="control-newlabel2" Text="Program <span style='color:red;font-weight:bold'>*</span>"></asp:Label>
                                        </td>
                                        <td class="auto-style9">
                                            <uc1:ProgramUserControl runat="server" ID="ucProgramModal" OnProgramSelectedIndexChanged="ucProgramModal_ProgramSelectedIndexChanged" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style8">
                                            <asp:Label ID="lblYearModal" runat="server" CssClass="control-newlabel2" Text="Year <span style='color:red;font-weight:bold'>*</span>"></asp:Label>
                                        </td>
                                        <td class="auto-style9">
                                            <asp:DropDownList ID="ddlYearModal" runat="server" Style="width: 200px;"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style8">
                                            <asp:Label ID="lblSemesterModal" runat="server" CssClass="control-newlabel2" Text="Semester <span style='color:red;font-weight:bold'>*</span>"></asp:Label>
                                        </td>
                                        <td class="auto-style9">
                                            <asp:DropDownList ID="ddlSemesterModal" runat="server" Style="width: 200px;"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style8">
                                            <asp:Label ID="lblShalModal" runat="server" CssClass="control-newlabel2" Text="Exam Year <span style='color:red;font-weight:bold'>*</span>"></asp:Label>
                                        </td>
                                        <td class="auto-style9">
                                            <asp:DropDownList ID="ddlShalModal" runat="server" Style="width: 200px;"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style8">
                                            <asp:Label ID="Label10" runat="server" CssClass="control-newlabel2" Text="Session <span style='color:red;font-weight:bold'>*</span>"></asp:Label>
                                        </td>
                                        <td class="auto-style9">
                                            <uc1:AdmissionSessionUserControl runat="server" ID="ucAdmissionSessionModal" OnSessionSelectedIndexChanged="ucAdmissionSessionModal_SessionSelectedIndexChanged" />
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="auto-style8">
                                            <asp:Label ID="lblExamNameModal" runat="server" CssClass="control-newlabel2" Text="Exam Name <span style='color:red;font-weight:bold'>*</span>"></asp:Label>
                                        </td>
                                        <td class="auto-style9">
                                            <%--<asp:TextBox ID="txtExamNameModal" runat="server" style="width: 350px;"></asp:TextBox>--%>
                                            <asp:DropDownList ID="ddlExamName" runat="server" Style="width: 350px;">
                                                <%--<asp:ListItem Value="-1">--Select--</asp:ListItem>--%>
                                                <asp:ListItem Value="1">Final</asp:ListItem>
                                                <asp:ListItem Value="2">Mid</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style8">
                                            <asp:Label ID="lblExamStartDateModal" runat="server" CssClass="control-newlabel2" Text="Exam Start Date"></asp:Label>
                                        </td>
                                        <td class="auto-style9">
                                            <asp:TextBox ID="txtExamStartDateModal" runat="server" placeholder="Date" DataFormatString="{0:dd/MM/yyyy}"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtExamStartDateModal" Format="dd/MM/yyyy" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style8">
                                            <asp:Label ID="lblExamEndDateModal" runat="server" CssClass="control-newlabel2" Text="Exam End Date"></asp:Label>
                                        </td>
                                        <td class="auto-style9">
                                            <asp:TextBox ID="txtExamEndDateModal" runat="server" placeholder="Date" DataFormatString="{0:dd/MM/yyyy}"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtExamEndDateModal" Format="dd/MM/yyyy" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style8">
                                            <asp:Label ID="lblIsActive" runat="server" CssClass="control-newlabel2" Text="IsActive"></asp:Label>
                                        </td>
                                        <td class="auto-style9">
                                            <asp:CheckBox ID="cbIsActive" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style8">
                                            <br />
                                        </td>
                                        <td class="auto-style9"></td>
                                    </tr>

                                    <tr>
                                        <td>
                                            <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text=""></asp:Button>
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
        <%--=================================== END POP UP MODAL Exam Setup =================================--%>

        <%--=================================== POP UP MODAL Exam Committee =================================--%>
        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
            <ContentTemplate>
                <asp:Button ID="btnShowPopUpExamCommittee" runat="server" Style="display: none" />
                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtenderExamCommittee" runat="server" TargetControlID="btnShowPopUpExamCommittee" PopupControlID="pnlShowPopUpExamCommittee"
                    CancelControlID="btnCancelExamCommittee" BackgroundCssClass="modalBackground">
                </ajaxToolkit:ModalPopupExtender>
                <asp:Panel ID="pnlShowPopUpExamCommittee" runat="server" BackColor="#ffffff" Width="765px" Style="display: none; border-radius: 3px;">
                    <div style="padding: 5px;">
                        <fieldset style="padding: 5px; border: 2px solid #5D7B9D;">
                            <legend style="font-weight: 100; font-size: medium; color: #5D7B9D; text-align: center">Exam Committee Insert / Edit</legend>
                            <div style="padding: 5px;">
                                <b>Exam Committee Insert / Edit</b><br />
                                <div class="Message-Area">
                                    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                        <ContentTemplate>
                                            <asp:Panel ID="Panel3" runat="server" Visible="true">
                                                <asp:Label ID="Label7" runat="server" Text="Message : "></asp:Label>
                                                <asp:Label ID="lblMessageExamCommittee" runat="server" ForeColor="#CC0000"></asp:Label>
                                            </asp:Panel>


                                            <asp:HiddenField ID="hfExamSetupWithExamCommitteesId" runat="server" />
                                            <%--<asp:HiddenField ID="hfBulkCommitteeAddEditModal" runat="server" />--%>
                                            <%--<asp:HiddenField ID="HiddenField3" runat="server" />--%>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>

                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="Label14" runat="server" Text="Department: "></asp:Label></td>
                                        <td>
                                            <uc1:DepartmentUserControl runat="server" ID="DepartmentUserControl1" OnDepartmentSelectedIndexChanged="DepartmentUserControl1_DepartmentSelectedIndexChanged" />
                                        </td>
                                        <td class="auto-style8">
                                            <asp:Label ID="Label9" runat="server" CssClass="control-newlabel2" Text="Chairman"></asp:Label>
                                        </td>
                                        <td class="auto-style9">

                                            <script type="text/javascript">

                                                $(document).ready(function () {
                                                    initdropdownExamCommitteeChairman();

                                                });

                                                function initdropdownExamCommitteeChairman() {
                                                    $("#ctl00_MainContainer_ddlExamCommitteeChairman").select2({ placeholder: { id: '0', text: '-Select-' }, allowClear: true });
                                                }
                                            </script>
                                            <script type="text/javascript">
                                                Sys.Application.add_load(initdropdownExamCommitteeChairman);
                                            </script>

                                            <asp:DropDownList ID="ddlExamCommitteeChairman" runat="server" class="dropdown"></asp:DropDownList>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>
                                            <asp:Label ID="Label15" runat="server" Text="Department: "></asp:Label></td>
                                        <td>
                                            <uc1:DepartmentUserControl runat="server" ID="DepartmentUserControl2" OnDepartmentSelectedIndexChanged="DepartmentUserControl2_DepartmentSelectedIndexChanged" />
                                        </td>
                                        <td class="auto-style8">
                                            <asp:Label ID="Label11" runat="server" CssClass="control-newlabel2" Text="Member One"></asp:Label>
                                        </td>
                                        <td class="auto-style9">

                                            <script type="text/javascript">

                                                $(document).ready(function () {
                                                    initdropdownExamCommitteeMemberOne();

                                                });

                                                function initdropdownExamCommitteeMemberOne() {
                                                    $("#ctl00_MainContainer_ddlExamCommitteeMemberOne").select2({ placeholder: { id: '0', text: '-Select-' }, allowClear: true });
                                                }
                                            </script>
                                            <script type="text/javascript">
                                                Sys.Application.add_load(initdropdownExamCommitteeMemberOne);
                                            </script>

                                            <asp:DropDownList ID="ddlExamCommitteeMemberOne" runat="server" class="dropdown"></asp:DropDownList>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td>
                                            <asp:Label ID="Label16" runat="server" Text="Department: "></asp:Label></td>
                                        <td>
                                            <uc1:DepartmentUserControl runat="server" ID="DepartmentUserControl3" OnDepartmentSelectedIndexChanged="DepartmentUserControl3_DepartmentSelectedIndexChanged" />
                                        </td>
                                        <td class="auto-style8">
                                            <asp:Label ID="Label12" runat="server" CssClass="control-newlabel2" Text="Member Two"></asp:Label>
                                        </td>
                                        <td class="auto-style9">

                                            <script type="text/javascript">

                                                $(document).ready(function () {
                                                    initdropdownExamCommitteeMemberTwo();

                                                });

                                                function initdropdownExamCommitteeMemberTwo() {
                                                    $("#ctl00_MainContainer_ddlExamCommitteeMemberTwo").select2({ placeholder: { id: '0', text: '-Select-' }, allowClear: true });
                                                }
                                            </script>
                                            <script type="text/javascript">
                                                Sys.Application.add_load(initdropdownExamCommitteeMemberTwo);
                                            </script>

                                            <asp:DropDownList ID="ddlExamCommitteeMemberTwo" runat="server" class="dropdown"></asp:DropDownList>
                                        </td>
                                    </tr>


                                    <tr>
                                        <td>
                                            <asp:Label ID="Label17" runat="server" Text="Department: "></asp:Label></td>
                                        <td>
                                            <uc1:DepartmentUserControl runat="server" ID="DepartmentUserControl4" OnDepartmentSelectedIndexChanged="DepartmentUserControl4_DepartmentSelectedIndexChanged" />
                                        </td>
                                        <td class="auto-style8">
                                            <asp:Label ID="Label18" runat="server" CssClass="control-newlabel2" Text="External Member"></asp:Label>
                                        </td>
                                        <td class="auto-style9">

                                            <script type="text/javascript">

                                                $(document).ready(function () {
                                                    initdropdownExamCommitteeExternalMember();

                                                });

                                                function initdropdownExamCommitteeExternalMember() {
                                                    $("#ctl00_MainContainer_ddlExamCommitteeExternalMember").select2({ placeholder: { id: '0', text: '-Select-' }, allowClear: true });
                                                }
                                            </script>
                                            <script type="text/javascript">
                                                Sys.Application.add_load(initdropdownExamCommitteeExternalMember);
                                            </script>

                                            <asp:DropDownList ID="ddlExamCommitteeExternalMember" runat="server" class="dropdown"></asp:DropDownList>
                                        </td>
                                    </tr>


                                    <tr>
                                        <td class="auto-style8">
                                            <br />
                                        </td>
                                        <td class="auto-style9"></td>
                                    </tr>

                                    <tr>
                                        <td>
                                            <asp:Button ID="btnSaveExamCommittee" runat="server" OnClick="btnSaveExamCommittee_Click" Text=""></asp:Button>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnCancelExamCommittee" runat="server" Text="Cancel" />
                                        </td>
                                    </tr>
                                </table>



                            </div>
                        </fieldset>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
        <%--=================================== END POP UP MODAL Exam Committee =================================--%>
    </div>
</asp:Content>
