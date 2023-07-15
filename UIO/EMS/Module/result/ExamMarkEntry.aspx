<%@ Page Title="" Language="C#"
    MasterPageFile="~/MasterPage/Common/CommonMasterPage.master"
    AutoEventWireup="true"
    CodeBehind="ExamMarkEntry.aspx.cs"
    Inherits="EMS.Module.result.ExamMarkEntry" %>



<%@ Register Src="~/UserControls/ProgramUserControl.ascx" TagPrefix="uc1" TagName="ProgramUserControl" %>
<%@ Register Src="~/UserControls/DepartmentUserControl.ascx" TagPrefix="uc1" TagName="DepartmentUserControl" %>
<%@ Register Src="~/UserControls/AdmissionSessionUserControl.ascx" TagPrefix="uc1" TagName="AdmissionSessionUserControl" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Examiner Mark Submit
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
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

    <style type="text/css">
        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=80);
            opacity: 0.8;
            z-index: 10000;
        }

        .header-center {
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContainer" runat="server">
    <div>
        <div class="PageTitle">
            <label>Examiner Mark Submit</label>
        </div>
        <div class="Message-Area">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="pnlMessage" runat="server" Visible="true" CssClass="msgPanel">
                        <asp:Label ID="Label1" runat="server" Text="Message : "></asp:Label>
                        <asp:Label ID="lblMsg" runat="server"></asp:Label>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="divProgress" style="display: none; z-index: 1000; position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%);">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Img/Waiting.gif" Height="150px" Width="150px" />
        </div>

        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
            <ContentTemplate>
                <div class="Message-Area">
                    <table>
                        <tr>
                            <td class="auto-style4">
                                <b>Department :</b>
                            </td>
                            <td>
                                <uc1:DepartmentUserControl runat="server" ID="ucDepartment" OnDepartmentSelectedIndexChanged="OnDepartmentSelectedIndexChanged" />
                            </td>
                            <td class="auto-style4">
                                <b>Program :</b>
                            </td>
                            <td class="auto-style6">
                                <uc1:ProgramUserControl runat="server" ID="ucProgram" OnProgramSelectedIndexChanged="OnProgramSelectedIndexChanged" />
                            </td>
                            <td class="auto-style4"><b>Session :</b></td>
                            <td class="auto-style2">
                                <uc1:AdmissionSessionUserControl runat="server" ID="ucFilterCurrentSession" class="margin-zero dropDownList" />
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <%--<td class="auto-style4">
                            <asp:Label ID="Label3" Width="120px" runat="server" Text="Versity Session"></asp:Label>
                            </td>
                            <td class="auto-style5">
                                <uc1:AdmissionSessionUserControl Width="150px" runat="server" ID="ucVersitySession" class="margin-zero dropDownList"/>
                            </td>--%>
                            <td class="auto-style4">
                                <asp:Label ID="Label5" runat="server" Text="Year : "></asp:Label></td>
                            <td class="auto-style6">
                                <asp:DropDownList ID="ddlYearNo" Width="150px" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlYearNo_SelectedIndexChanged"></asp:DropDownList>
                            </td>
                            <td class="auto-style4">
                                <asp:Label ID="Label10" runat="server" Text="Semester : "></asp:Label></td>
                            <td class="auto-style6">
                                <asp:DropDownList ID="ddlSemesterNo" Width="150px" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlSemesterNo_SelectedIndexChanged"></asp:DropDownList>
                            </td>
                            <td class="auto-style4">
                                <asp:Label ID="Label15" runat="server" Text="Exam : "></asp:Label>
                            </td>
                            <td class="auto-style6">
                                <asp:DropDownList ID="ddlExam" Width="250" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlExam_SelectedIndexChanged"></asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btnLoad" runat="server" class="btn btn-sm btn-secondary w-100" Text="Load" OnClick="btnLoad_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <br />

        <%--\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\FirstExaminer\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\--%>

        <div>
            <div>
                <div class="PageTitle">
                    <label>First Examiner Courses</label>
                </div>
                <div class="Message-Area">
                    <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                        <ContentTemplate>
                            <asp:GridView runat="server" ID="gvExamMarkEntryFirstExaminer" AllowSorting="True" CssClass="table-bordered"
                                AutoGenerateColumns="False" ShowFooter="True" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                                <HeaderStyle BackColor="#1C5E55" ForeColor="White" Height="30" Font-Bold="True" />
                                <FooterStyle BackColor="#1C5E55" ForeColor="White" Height="30" Font-Bold="True" />
                                <AlternatingRowStyle BackColor="White" />
                                <RowStyle Height="25" />

                                <Columns>
                                    <asp:TemplateField HeaderText="ExamTemplateItemId" HeaderStyle-Width="350px" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblExmaSetupId" Text='<%#Eval("ExamTemplateItemId") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="350" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ExamSetupId" HeaderStyle-Width="350px" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblExamSetupId" Text='<%#Eval("ExamSetupID") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="350" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Course Section" HeaderStyle-Width="350px">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblCourseSection" Text='<%#Eval("CourseSection") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="350" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Exam">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblYearNo" Text='<%#Eval("Exam") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Exam Mark" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="header-center">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblExamMark" Text='<%#Eval("ExamMark") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="View" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="header-center">
                                        <ItemTemplate>
                                            <asp:Button ID="btnViewFirstExaminer" runat="server" OnClick="btnViewFirstExaminer_Click" Text="View"
                                                CommandArgument='<%#Eval("AcaCalSectionID")+","+ Eval("ExamSetupID")+","+ Eval("ExamTemplateItemId") %>' OnClientClick="target ='_blank';"></asp:Button>
                                        </ItemTemplate>
                                        <HeaderStyle />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Download" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="header-center">
                                        <ItemTemplate>
                                            <asp:UpdatePanel ID="upbtnDownloadFirstExaminer" runat="server">
                                                <ContentTemplate>
                                                    <asp:Button ID="btnDownloadFirstExaminer" runat="server" OnClick="btnDownloadFirstExaminer_Click" Text="Download" Visible='<%# Eval("IsButtonVisible").ToString() == "True" ? true : false %>'
                                                        CommandArgument='<%#Eval("AcaCalSectionID")+","+ Eval("ExamSetupID") +","+ Eval("ExamTemplateItemId") %>'></asp:Button>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="btnDownloadFirstExaminer" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </ItemTemplate>
                                        <HeaderStyle />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Upload" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="header-center">
                                        <ItemTemplate>
                                            <asp:Button ID="btnUploadFirstExaminer" runat="server" OnClick="btnUploadFirstExaminer_Click" Text="Upload" Visible='<%# Eval("IsButtonVisible").ToString() == "True" ? true : false %>'
                                                CommandArgument='<%#Eval("AcaCalSectionID")+","+ Eval("ExamSetupID")+","+ Eval("ExamTemplateItemId") %>'></asp:Button>
                                        </ItemTemplate>
                                        <HeaderStyle />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Submit to <br/>Exam Committee" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="header-center">
                                        <ItemTemplate>
                                            <asp:Button ID="btnSubmitToExamCommitteeFirstExaminer" runat="server"
                                                Style="width: 90px;"
                                                OnClientClick="return confirm('Are you sure for Submitting Mark?')"
                                                OnClick="btnSubmitToExamCommitteeFirstExaminer_Click" Text="Submit"
                                                Visible='<%# Eval("IsButtonVisible").ToString() == "True" ? true : false %>'
                                                CommandArgument='<%#Eval("AcaCalSectionID")+","+ Eval("ExamSetupID")+","+ Eval("ExamTemplateItemId") %>'></asp:Button>
                                        </ItemTemplate>
                                        <HeaderStyle />
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
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>

        <%--=================================== POP UP MODAL FirstExaminer Excel Upload=================================--%>
        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
            <ContentTemplate>
                <asp:Button ID="btnShowPopUpFirstExaminerExcelUpload" runat="server" Style="display: none" />
                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtenderFirstExaminerExcelUpload" runat="server" TargetControlID="btnShowPopUpFirstExaminerExcelUpload" PopupControlID="pnlShowPopUpFirstExaminerExcelUpload"
                    CancelControlID="btnCancelFirstExaminerExcelUpload" BackgroundCssClass="modalBackground">
                </ajaxToolkit:ModalPopupExtender>
                <asp:Panel ID="pnlShowPopUpFirstExaminerExcelUpload" runat="server" BackColor="#ffffff" Width="765px" Style="display: none; border-radius: 3px;">
                    <div style="padding: 5px;">
                        <fieldset style="padding: 5px; border: 2px solid #5D7B9D;">
                            <legend style="font-weight: 100; font-size: medium; color: #5D7B9D; text-align: center">Exam Mark Upload (First Examiner)</legend>
                            <div style="padding: 5px;">
                                <b>Exam Mark Upload (First Examiner)</b><br />
                                <br />

                                <div class="form-horizontal">
                                    <div class="Message-Area">
                                        <table>
                                            <tr>
                                                <td class="auto-style8">
                                                    <asp:Label ID="lblFileUpload" runat="server" CssClass="control-newlabel2" Text="File Upload:"></asp:Label>
                                                </td>
                                                <td class="auto-style9">
                                                    <asp:FileUpload ID="FileUploadExamMarkFirstExaminer" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="auto-style9" colspan="2">
                                                    <asp:Button ID="FirstExaminerExamMarkUpload" runat="server"
                                                        OnClientClick="return confirm('Are you sure to upload selected file?')"
                                                        OnClick="FirstExaminerExamMarkUpload_Click" Text="Upload" Style="width: 85px" />
                                                    <asp:HiddenField ID="hfFirstExaminerExamMarkUploadAcaCalSectionId" runat="server" />
                                                    <asp:HiddenField ID="hfFirstExaminerExamMarkUploadExamTemplateItemId" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>

                                <br />
                                <div style="text-align: right;">
                                    <asp:Button ID="btnCancelFirstExaminerExcelUpload" runat="server" Text="Cancel" />
                                </div>
                            </div>
                        </fieldset>
                    </div>
                </asp:Panel>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="FirstExaminerExamMarkUpload" />
            </Triggers>
        </asp:UpdatePanel>
        <%--=================================== END POP UP MODAL FirstExaminer View=================================--%>


        <%--=================================== POP UP MODAL FirstExaminer View=================================--%>
        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
                <asp:Button ID="btnShowPopUpFirstExaminer" runat="server" Style="display: none" />
                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtenderFirstExaminer" runat="server" TargetControlID="btnShowPopUpFirstExaminer" PopupControlID="pnlShowPopUpFirstExaminer"
                    CancelControlID="btnCancelFirstExaminer" BackgroundCssClass="modalBackground">
                </ajaxToolkit:ModalPopupExtender>
                <asp:Panel ID="pnlShowPopUpFirstExaminer" runat="server" BackColor="#ffffff" Width="765px" Style="display: none; border-radius: 3px;">
                    <div style="padding: 5px;">
                        <fieldset style="padding: 5px; border: 2px solid #5D7B9D;">
                            <legend style="font-weight: 100; font-size: medium; color: #5D7B9D; text-align: center">Exam Mark (First Examiner)</legend>
                            <div style="padding: 5px;">
                                <b>Exam Mark (First Examiner)</b><br />
                                <br />

                                <div class="form-horizontal">
                                    <div class="Message-Area">
                                        <table>
                                            <tr>
                                                <td class="auto-style8">
                                                    <asp:Label ID="Label4" runat="server" CssClass="control-newlabel2" Text="Course"></asp:Label>
                                                </td>
                                                <td class="auto-style9">:
                                                    <asp:Label ID="lblFirstExaminerModalViewCourse" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="auto-style8">
                                                    <asp:Label ID="Label6" runat="server" CssClass="control-newlabel2" Text="Exam"></asp:Label>
                                                </td>
                                                <td class="auto-style9">:
                                                    <asp:Label ID="lblFirstExaminerModalViewExam" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="auto-style8">
                                                    <asp:Label ID="Label7" runat="server" CssClass="control-newlabel2" Text="Total Student"></asp:Label>
                                                </td>
                                                <td class="auto-style9">:
                                                    <asp:Label ID="lblFirstExaminerModalViewTotalStudent" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="auto-style8">
                                                    <asp:Label ID="Label8" runat="server" CssClass="control-newlabel2" Text="Absent Count"></asp:Label>
                                                </td>
                                                <td class="auto-style9">:
                                                    <asp:Label ID="lblFirstExaminerModalViewAbsentCount" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>

                                <br />

                                <div class="form-horizontal">
                                    <div class="Message-Area" style="height: 300px; overflow: scroll;">
                                        <asp:GridView runat="server" ID="gvExamMarkFirstExaminerView" AllowSorting="True" CssClass="table-bordered"
                                            AutoGenerateColumns="False" ShowFooter="True" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                                            <HeaderStyle BackColor="#1C5E55" ForeColor="White" Height="30" Font-Bold="True" />
                                            <FooterStyle BackColor="#1C5E55" ForeColor="White" Height="30" Font-Bold="True" />
                                            <AlternatingRowStyle BackColor="White" />
                                            <RowStyle Height="25" />

                                            <Columns>
                                                <asp:TemplateField HeaderText="SI." ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate><b><%# Container.DataItemIndex + 1 %></b></ItemTemplate>
                                                    <HeaderStyle Width="30px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="ExamMarkID" Visible="false" HeaderStyle-Width="350px">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblExamMarkID" Text='<%#Eval("ExamMarkId") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="350" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Student ID" HeaderStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblRoll" Text='<%#Eval("Roll") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="100" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Name">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblYearNo" Text='<%#Eval("Name") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Mark">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblMark" Text='<%#Eval("Mark") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Present/Absent">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblPresentAbsent" Text='<%#Eval("PresentAbsent") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle />
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
                                <br />
                                <div style="text-align: right;">
                                    <asp:Button ID="btnCancelFirstExaminer" runat="server" Text="Cancel" />
                                </div>
                            </div>
                        </fieldset>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
        <%--=================================== END POP UP MODAL FirstExaminer View=================================--%>

        <%--\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\END FirstExaminer\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\--%>


        <%--\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\SecontExaminer\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\--%>
        <div>
            <div>
                <div class="PageTitle">
                    <label>Second Examiner Courses</label>
                </div>
                <div class="Message-Area">
                    <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                        <ContentTemplate>
                            <asp:GridView runat="server" ID="gvExamMarkEntrySecondExaminer" AllowSorting="True" CssClass="table-bordered"
                                AutoGenerateColumns="False" ShowFooter="True" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                                <HeaderStyle BackColor="#1C5E55" ForeColor="White" Height="30" Font-Bold="True" />
                                <FooterStyle BackColor="#1C5E55" ForeColor="White" Height="30" Font-Bold="True" />
                                <AlternatingRowStyle BackColor="White" />
                                <RowStyle Height="25" />

                                <Columns>
                                    <asp:TemplateField HeaderText="ExamTemplateItemId" HeaderStyle-Width="350px" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblExmaSetupId" Text='<%#Eval("ExamTemplateItemId") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="350" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ExamSetupId" HeaderStyle-Width="350px" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblExamSetupId" Text='<%#Eval("ExamSetupID") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="350" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Course Section" HeaderStyle-Width="350px">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblCourseSection" Text='<%#Eval("CourseSection") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="350" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Exam">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblYearNo" Text='<%#Eval("Exam") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Exam Mark" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="header-center">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblExamMark" Text='<%#Eval("ExamMark") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="View" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="header-center">
                                        <ItemTemplate>
                                            <asp:Button ID="btnViewSecondExaminer" runat="server" OnClick="btnViewSecondExaminer_Click" Text="View"
                                                CommandArgument='<%#Eval("AcaCalSectionID")+","+ Eval("ExamSetupID")+","+ Eval("ExamTemplateItemId") %>' OnClientClick="target ='_blank';"></asp:Button>
                                        </ItemTemplate>
                                        <HeaderStyle />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Download" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="header-center">
                                        <ItemTemplate>
                                            <asp:UpdatePanel ID="upbtnDownloadSecondExaminer" runat="server">
                                                <ContentTemplate>
                                                    <asp:Button ID="btnDownloadSecondExaminer" runat="server" OnClick="btnDownloadSecondExaminer_Click" Text="Download"
                                                        Visible='<%# Eval("IsButtonVisible").ToString() == "True" ? true : false %>'
                                                        CommandArgument='<%#Eval("AcaCalSectionID")+","+ Eval("ExamSetupID")+","+ Eval("ExamTemplateItemId") %>'></asp:Button>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="btnDownloadSecondExaminer" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </ItemTemplate>
                                        <HeaderStyle />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Upload" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="header-center">
                                        <ItemTemplate>
                                            <asp:Button ID="btnUploadSecondExaminer" runat="server" OnClick="btnUploadSecondExaminer_Click" Text="Upload"
                                                Visible='<%# Eval("IsButtonVisible").ToString() == "True" ? true : false %>'
                                                CommandArgument='<%#Eval("AcaCalSectionID")+","+ Eval("ExamSetupID")+","+ Eval("ExamTemplateItemId") %>'></asp:Button>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Submit to <br/>Exam Committee" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="header-center">
                                        <ItemTemplate>
                                            <asp:Button ID="btnSubmitToExamCommitteeSecondExaminer" runat="server" OnClientClick="return confirm('Are you sure for Submitting Mark?')" OnClick="btnSubmitToExamCommitteeSecondExaminer_Click" Text="Submit"
                                                Style="width: 90px;"
                                                Visible='<%# Eval("IsButtonVisible").ToString() == "True" ? true : false %>'
                                                CommandArgument='<%#Eval("AcaCalSectionID")+","+ Eval("ExamSetupID")+","+ Eval("ExamTemplateItemId") %>'></asp:Button>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
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
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>

        <%--=================================== POP UP MODAL SecontExaminer Excel Upload=================================--%>
        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
            <ContentTemplate>
                <asp:Button ID="btnShowPopUpSecontExaminerExcelUpload" runat="server" Style="display: none" />
                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtenderSecontExaminerExcelUpload" runat="server" TargetControlID="btnShowPopUpSecontExaminerExcelUpload" PopupControlID="pnlShowPopUpSecontExaminerExcelUpload"
                    CancelControlID="btnCancelSecontExaminerExcelUpload" BackgroundCssClass="modalBackground">
                </ajaxToolkit:ModalPopupExtender>
                <asp:Panel ID="pnlShowPopUpSecontExaminerExcelUpload" runat="server" BackColor="#ffffff" Width="765px" Style="display: none; border-radius: 3px;">
                    <div style="padding: 5px;">
                        <fieldset style="padding: 5px; border: 2px solid #5D7B9D;">
                            <legend style="font-weight: 100; font-size: medium; color: #5D7B9D; text-align: center">Exam Mark Upload (Secont Examiner)</legend>
                            <div style="padding: 5px;">
                                <b>Exam Mark Upload (Secont Examiner)</b><br />
                                <br />

                                <div class="form-horizontal">
                                    <div class="Message-Area">
                                        <table>
                                            <tr>
                                                <td class="auto-style8">
                                                    <asp:Label ID="Label2" runat="server" CssClass="control-newlabel2" Text="File Upload:"></asp:Label>
                                                </td>
                                                <td class="auto-style9">
                                                    <asp:FileUpload ID="FileUploadExamMarkSecontExaminer" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="auto-style9" colspan="2">
                                                    <asp:Button ID="SecontExaminerExamMarkUpload" runat="server"
                                                        OnClientClick="return confirm('Are you sure to upload selected file?')"
                                                        OnClick="SecontExaminerExamMarkUpload_Click" Text="Upload" Style="width: 85px" />
                                                    <asp:HiddenField ID="hfSecontExaminerExamMarkUploadAcaCalSectionId" runat="server" />
                                                    <asp:HiddenField ID="hfSecontExaminerExamMarkUploadExamTemplateItemId" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>

                                <br />
                                <div style="text-align: right;">
                                    <asp:Button ID="btnCancelSecontExaminerExcelUpload" runat="server" Text="Cancel" />
                                </div>
                            </div>
                        </fieldset>
                    </div>
                </asp:Panel>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="SecontExaminerExamMarkUpload" />
            </Triggers>
        </asp:UpdatePanel>
        <%--=================================== END POP UP MODAL SecontExaminer View=================================--%>


        <%--=================================== POP UP MODAL SecondExaminer View=================================--%>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Button ID="btnShowPopUpSecondExaminer" runat="server" Style="display: none" />
                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtenderSecondExaminer" runat="server" TargetControlID="btnShowPopUpSecondExaminer" PopupControlID="pnlShowPopUpSecondExaminer"
                    CancelControlID="btnCancelSecondExaminer" BackgroundCssClass="modalBackground">
                </ajaxToolkit:ModalPopupExtender>
                <asp:Panel ID="pnlShowPopUpSecondExaminer" runat="server" BackColor="#ffffff" Width="765px" Style="display: none; border-radius: 3px;">
                    <div style="padding: 5px;">
                        <fieldset style="padding: 5px; border: 2px solid #5D7B9D;">
                            <legend style="font-weight: 100; font-size: medium; color: #5D7B9D; text-align: center">Exam Mark (Second Examiner)</legend>
                            <div style="padding: 5px;">
                                <b>Exam Mark (Second Examiner)</b><br />
                                <br />

                                <div class="form-horizontal">
                                    <div class="Message-Area">
                                        <table>
                                            <tr>
                                                <td class="auto-style8">
                                                    <asp:Label ID="Label9" runat="server" CssClass="control-newlabel2" Text="Course"></asp:Label>
                                                </td>
                                                <td class="auto-style9">:
                                                    <asp:Label ID="lblSecondExaminerModalViewCourse" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="auto-style8">
                                                    <asp:Label ID="Label11" runat="server" CssClass="control-newlabel2" Text="Exam"></asp:Label>
                                                </td>
                                                <td class="auto-style9">:
                                                    <asp:Label ID="lblSecondExaminerModalViewExam" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="auto-style8">
                                                    <asp:Label ID="Label12" runat="server" CssClass="control-newlabel2" Text="Total Student"></asp:Label>
                                                </td>
                                                <td class="auto-style9">:
                                                    <asp:Label ID="lblSecondExaminerModalViewTotalStudent" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="auto-style8">
                                                    <asp:Label ID="Label13" runat="server" CssClass="control-newlabel2" Text="Absent Count"></asp:Label>
                                                </td>
                                                <td class="auto-style9">:
                                                    <asp:Label ID="lblSecondExaminerModalViewAbsentCount" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>

                                <br />

                                <div class="form-horizontal">
                                    <div class="Message-Area" style="height: 300px; overflow: scroll;">
                                        <asp:GridView runat="server" ID="gvExamMarkSecondExaminerView" AllowSorting="True" CssClass="table-bordered"
                                            AutoGenerateColumns="False" ShowFooter="True" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                                            <HeaderStyle BackColor="#1C5E55" ForeColor="White" Height="30" Font-Bold="True" />
                                            <FooterStyle BackColor="#1C5E55" ForeColor="White" Height="30" Font-Bold="True" />
                                            <AlternatingRowStyle BackColor="White" />
                                            <RowStyle Height="25" />

                                            <Columns>
                                                <asp:TemplateField HeaderText="SI." ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate><b><%# Container.DataItemIndex + 1 %></b></ItemTemplate>
                                                    <HeaderStyle Width="30px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="ExamMarkID" Visible="false" HeaderStyle-Width="350px">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblExamMarkID" Text='<%#Eval("ExamMarkId") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="350" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Student ID" HeaderStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblRoll" Text='<%#Eval("Roll") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="100" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Name">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblYearNo" Text='<%#Eval("Name") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Mark">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblMark" Text='<%#Eval("Mark") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Present/Absent">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblPresentAbsent" Text='<%#Eval("PresentAbsent") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle />
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

                                <br />
                                <div style="text-align: right;">
                                    <asp:Button ID="btnCancelSecondExaminer" runat="server" Text="Cancel" />
                                </div>
                            </div>
                        </fieldset>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
        <%--=================================== END POP UP MODAL SecondExaminer View=================================--%>

        <%--\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\END SecontExaminer\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\--%>


        <%--\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\ThirdExaminer\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\--%>
        <div>
            <div>
                <div class="PageTitle">
                    <label>Third Examiner Courses</label>
                </div>
                <div class="Message-Area">
                    <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                        <ContentTemplate>
                            <asp:GridView runat="server" ID="gvExamMarkEntryThirdExaminer" AllowSorting="True" CssClass="table-bordered"
                                AutoGenerateColumns="False" ShowFooter="True" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                                <HeaderStyle BackColor="#1C5E55" ForeColor="White" Height="30" Font-Bold="True" />
                                <FooterStyle BackColor="#1C5E55" ForeColor="White" Height="30" Font-Bold="True" />
                                <AlternatingRowStyle BackColor="White" />
                                <RowStyle Height="25" />

                                <Columns>
                                    <asp:TemplateField HeaderText="ExamTemplateItemId" HeaderStyle-Width="350px" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblExmaSetupId" Text='<%#Eval("ExamTemplateItemId") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="350" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ExamSetupId" HeaderStyle-Width="350px" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblExamSetupId" Text='<%#Eval("ExamSetupID") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="350" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Course Section" HeaderStyle-Width="350px">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblCourseSection" Text='<%#Eval("CourseSection") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Width="350" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Exam">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblYearNo" Text='<%#Eval("Exam") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Exam Mark" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="header-center">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblExamMark" Text='<%#Eval("ExamMark") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle />
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="View" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="header-center">
                                        <ItemTemplate>
                                            <asp:Button ID="btnViewThirdExaminer" runat="server" OnClick="btnViewThirdExaminer_Click" Text="View"
                                                CommandArgument='<%#Eval("AcaCalSectionID")+","+ Eval("ExamSetupID")+","+ Eval("ExamTemplateItemId") %>' OnClientClick="target ='_blank';"></asp:Button>
                                        </ItemTemplate>
                                        <HeaderStyle />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Download" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="header-center">
                                        <ItemTemplate>
                                            <asp:UpdatePanel ID="upbtnDownloadThirdExaminer" runat="server">
                                                <ContentTemplate>
                                                    <asp:Button ID="btnDownloadThirdExaminer" runat="server" OnClick="btnDownloadThirdExaminer_Click" Text="Download"
                                                        Visible='<%# Eval("IsButtonVisible").ToString() == "True" ? true : false %>'
                                                        CommandArgument='<%#Eval("AcaCalSectionID")+","+ Eval("ExamSetupID")+","+ Eval("ExamTemplateItemId") %>'></asp:Button>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="btnDownloadThirdExaminer" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </ItemTemplate>
                                        <HeaderStyle />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Upload" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="header-center">
                                        <ItemTemplate>
                                            <asp:Button ID="btnUploadThirdExaminer" runat="server" OnClick="btnUploadThirdExaminer_Click" Text="Upload"
                                                Visible='<%# Eval("IsButtonVisible").ToString() == "True" ? true : false %>'
                                                CommandArgument='<%#Eval("AcaCalSectionID")+","+ Eval("ExamSetupID")+","+ Eval("ExamTemplateItemId") %>'></asp:Button>
                                        </ItemTemplate>
                                        <HeaderStyle />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Submit to <br/>Exam Committee" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="header-center">
                                        <ItemTemplate>
                                            <asp:Button ID="btnSubmitToExamCommitteeThirdExaminer" runat="server" OnClientClick="return confirm('Are you sure for Submitting Mark?')" OnClick="btnSubmitToExamCommitteeThirdExaminer_Click" Text="Submit"
                                                Style="width: 90px;"
                                                Visible='<%# Eval("IsButtonVisible").ToString() == "True" ? true : false %>'
                                                CommandArgument='<%#Eval("AcaCalSectionID")+","+ Eval("ExamSetupID")+","+ Eval("ExamTemplateItemId") %>'></asp:Button>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
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
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>

        <%--=================================== POP UP MODAL ThirdExaminer Excel Upload=================================--%>
        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
            <ContentTemplate>
                <asp:Button ID="btnShowPopUpThirdExaminerExcelUpload" runat="server" Style="display: none" />
                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtenderThirdExaminerExcelUpload" runat="server" TargetControlID="btnShowPopUpThirdExaminerExcelUpload" PopupControlID="pnlShowPopUpThirdExaminerExcelUpload"
                    CancelControlID="btnCancelThirdExaminerExcelUpload" BackgroundCssClass="modalBackground">
                </ajaxToolkit:ModalPopupExtender>
                <asp:Panel ID="pnlShowPopUpThirdExaminerExcelUpload" runat="server" BackColor="#ffffff" Width="765px" Style="display: none; border-radius: 3px;">
                    <div style="padding: 5px;">
                        <fieldset style="padding: 5px; border: 2px solid #5D7B9D;">
                            <legend style="font-weight: 100; font-size: medium; color: #5D7B9D; text-align: center">Exam Mark Upload (Third Examiner)</legend>
                            <div style="padding: 5px;">
                                <b>Exam Mark Upload (Third Examiner)</b><br />
                                <br />

                                <div class="form-horizontal">
                                    <div class="Message-Area">
                                        <table>
                                            <tr>
                                                <td class="auto-style8">
                                                    <asp:Label ID="Label3" runat="server" CssClass="control-newlabel2" Text="File Upload:"></asp:Label>
                                                </td>
                                                <td class="auto-style9">
                                                    <asp:FileUpload ID="FileUploadExamMarkThirdExaminer" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="auto-style9" colspan="2">
                                                    <asp:Button ID="ThirdExaminerExamMarkUpload" runat="server"
                                                        OnClientClick="return confirm('Are you sure to upload selected file?')"
                                                        OnClick="ThirdExaminerExamMarkUpload_Click" Text="Upload" Style="width: 85px" />
                                                    <asp:HiddenField ID="hfThirdExaminerExamMarkUploadAcaCalSectionId" runat="server" />
                                                    <asp:HiddenField ID="hfThirdExaminerExamMarkUploadExamTemplateItemId" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>

                                <br />
                                <div style="text-align: right;">
                                    <asp:Button ID="btnCancelThirdExaminerExcelUpload" runat="server" Text="Cancel" />
                                </div>
                            </div>
                        </fieldset>
                    </div>
                </asp:Panel>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="ThirdExaminerExamMarkUpload" />
            </Triggers>
        </asp:UpdatePanel>
        <%--=================================== END POP UP MODAL ThirdExaminer View=================================--%>


        <%--=================================== POP UP MODAL ThirdExaminer View=================================--%>
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <asp:Button ID="btnShowPopUpThirdExaminer" runat="server" Style="display: none" />
                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtenderThirdExaminer" runat="server" TargetControlID="btnShowPopUpThirdExaminer" PopupControlID="pnlShowPopUpThirdExaminer"
                    CancelControlID="btnCancelThirdExaminer" BackgroundCssClass="modalBackground">
                </ajaxToolkit:ModalPopupExtender>
                <asp:Panel ID="pnlShowPopUpThirdExaminer" runat="server" BackColor="#ffffff" Width="765px" Style="display: none; border-radius: 3px;">
                    <div style="padding: 5px;">
                        <fieldset style="padding: 5px; border: 2px solid #5D7B9D;">
                            <legend style="font-weight: 100; font-size: medium; color: #5D7B9D; text-align: center">Exam Mark (Third Examiner)</legend>
                            <div style="padding: 5px;">
                                <b>Exam Mark (Third Examiner)</b><br />
                                <br />

                                <div class="form-horizontal">
                                    <div class="Message-Area">
                                        <table>
                                            <tr>
                                                <td class="auto-style8">
                                                    <asp:Label ID="Label14" runat="server" CssClass="control-newlabel2" Text="Course"></asp:Label>
                                                </td>
                                                <td class="auto-style9">:
                                                    <asp:Label ID="lblThirdExaminerModalViewCourse" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="auto-style8">
                                                    <asp:Label ID="Label16" runat="server" CssClass="control-newlabel2" Text="Exam"></asp:Label>
                                                </td>
                                                <td class="auto-style9">:
                                                    <asp:Label ID="lblThirdExaminerModalViewExam" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="auto-style8">
                                                    <asp:Label ID="Label17" runat="server" CssClass="control-newlabel2" Text="Total Student"></asp:Label>
                                                </td>
                                                <td class="auto-style9">:
                                                    <asp:Label ID="lblThirdExaminerModalViewTotalStudent" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="auto-style8">
                                                    <asp:Label ID="Label18" runat="server" CssClass="control-newlabel2" Text="Absent Count"></asp:Label>
                                                </td>
                                                <td class="auto-style9">:
                                                    <asp:Label ID="lblThirdExaminerModalViewAbsentCount" runat="server" Text=""></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>

                                <br />

                                <div class="form-horizontal">
                                    <div class="Message-Area" style="height: 300px; overflow: scroll;">
                                        <asp:GridView runat="server" ID="gvExamMarkThirdExaminerView" AllowSorting="True" CssClass="table-bordered"
                                            AutoGenerateColumns="False" ShowFooter="True" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                                            <HeaderStyle BackColor="#1C5E55" ForeColor="White" Height="30" Font-Bold="True" />
                                            <FooterStyle BackColor="#1C5E55" ForeColor="White" Height="30" Font-Bold="True" />
                                            <AlternatingRowStyle BackColor="White" />
                                            <RowStyle Height="25" />

                                            <Columns>
                                                <asp:TemplateField HeaderText="SI." ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate><b><%# Container.DataItemIndex + 1 %></b></ItemTemplate>
                                                    <HeaderStyle Width="30px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="ExamMarkID" Visible="false" HeaderStyle-Width="350px">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblExamMarkID" Text='<%#Eval("ExamMarkId") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="350" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Student ID" HeaderStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblRoll" Text='<%#Eval("Roll") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="100" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Name">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblYearNo" Text='<%#Eval("Name") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Mark">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblMark" Text='<%#Eval("Mark") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Present/Absent">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblPresentAbsent" Text='<%#Eval("PresentAbsent") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle />
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

                                <br />
                                <div style="text-align: right;">
                                    <asp:Button ID="btnCancelThirdExaminer" runat="server" Text="Cancel" />
                                </div>
                            </div>
                        </fieldset>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
        <%--=================================== END POP UP MODAL ThirdExamine Viewr=================================--%>

        <%--\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\END ThirdExaminer\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\--%>
    </div>
</asp:Content>
