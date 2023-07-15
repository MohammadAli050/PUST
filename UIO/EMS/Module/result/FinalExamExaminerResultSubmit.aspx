<%@ Page Title="Final Exam Marks Entry" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master"  AutoEventWireup="true" 
    CodeBehind="FinalExamExaminerResultSubmit.aspx.cs" Inherits="EMS.Module.result.FinalExamExaminerResultSubmit" %>

<%@ Register Src="~/UserControls/ProgramUserControl.ascx" TagPrefix="uc1" TagName="ProgramUserControl" %>
<%@ Register Src="~/UserControls/DepartmentUserControl.ascx" TagPrefix="uc1" TagName="DepartmentUserControl" %>
<%@ Register Src="~/UserControls/AdmissionSessionUserControl.ascx" TagPrefix="uc1" TagName="AdmissionSessionUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="Server">
    Final Exam Marks Entry
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="Server">
    <script type="text/javascript">

        function InProgress() {
            var panelProg = $get('divProgress');
            panelProg.style.display = '';
        }

        function onComplete() {
            var panelProg = $get('divProgress');
            panelProg.style.display = 'none';
        }


        $(document).ready(function () {
            $("form").keypress(function (e) {
                //Enter key
                if (e.which == 13) {
                    return false;
                }
            });
        });



        function myFunction(e) {
            var code = e.keyCode || e.which;
            console.log(code);
            //if (code === 13) {
            //    e.preventDefault();
            //    var pageElems = document.querySelectorAll('input.inp'),

            //        elem = e.srcElement || e.target,
            //        focusNext = false,
            //        focusPrev = false,
            //        len = pageElems.length;
            //    for (var i = 0; i < len; i++) {
            //        var pe = pageElems[i];
            //        if (focusNext) {
            //            if (pe.style.display !== 'none') {
            //                $(pe).focus();
            //                break;
            //            }
            //        } else if (pe === elem) {
            //            focusNext = true;
            //        }
            //    }
            //}
            //Down
            if (code === 40) {
                e.preventDefault();
                var pageElems = document.querySelectorAll('input.inp'),

                    elem = e.srcElement || e.target,
                    focusNext = false,
                    len = pageElems.length;
                for (var i = 0; i < len; i++) {
                    var pe = pageElems[i];
                    if (focusNext) {
                        if (pe.style.display !== 'none') {
                            pe = pageElems[i + 9];
                            $(pe).focus();
                            break;
                        }
                    } else if (pe === elem) {
                        focusNext = true;
                    }
                }
            }
            //Up
            else if (code === 38) {
                e.preventDefault();
                var pageElems = document.querySelectorAll('input.inp'),

                    elem = e.srcElement || e.target,
                    focusPrev = false,
                    len = pageElems.length;
                for (var i = 0; i <= len; i++) {
                    if (i == len) {
                        var pe = pageElems[i - 1];
                    }
                    else {
                        var pe = pageElems[i];
                    }
                    if (focusPrev) {
                        if (pe.style.display !== 'none') {
                            pe = pageElems[i - 11];
                            $(pe).focus();
                            break;
                        }
                    } else if (pe === elem) {
                        focusPrev = true;
                    }
                }

            }
            //Right
            else if (code === 39) {
                e.preventDefault();
                var pageElems = document.querySelectorAll('input.inp'),

                    elem = e.srcElement || e.target,
                    focusPrev = false,
                    len = pageElems.length;
                for (var i = 0; i <= len; i++) {

                    var pe = pageElems[i];

                    if (focusPrev) {
                        if (pe.style.display !== 'none') {
                            //pe = pageElems[i - 2];
                            //angular.element(pe).focus();
                            $(pe).focus();
                            break;
                        }
                    } else if (pe === elem) {
                        focusPrev = true;
                    }
                }
            }
            //Left
            else if (code === 37) {
                e.preventDefault();
                var pageElems = document.querySelectorAll('input.inp'),

                    elem = e.srcElement || e.target,
                    focusPrev = false,
                    len = pageElems.length;
                for (var i = 0; i <= len; i++) {
                    if (i == len) {
                        var pe = pageElems[i - 1];
                    }
                    else {
                        var pe = pageElems[i];
                    }
                    if (focusPrev) {
                        if (pe.style.display !== 'none') {
                            pe = pageElems[i - 2];
                            $(pe).focus();
                            break;
                        }
                    } else if (pe === elem) {
                        focusPrev = true;
                    }
                }
            }
            else { }
        }

        
        $(document).on('keydown', 'input[pattern]', function (e) {
                    var input = $(this);
                    var oldVal = input.val();
                    var regex = new RegExp(input.attr('pattern'), 'g');

                    setTimeout(function () {
                        var newVal = input.val();
                        if (!regex.test(newVal)) {
                            input.val(oldVal);
                        }
                    }, 0);
                });
    </script>        
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContainer" Runat="Server">
    <div>
    <div class="PageTitle">
        <label>Final Exam Marks Entry</label>
    </div>

    <asp:UpdatePanel ID="UpdatePanel01" runat="server">
        <ContentTemplate>
            <div class="Message-Area">
                <label class="msgTitle">Message: </label>
                <asp:Label runat="server" class="msgTitle" ID="lblMsg" ForeColor="Red" Text="" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div id="divProgress" style="display: none; z-index: 1000; position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%);">
        <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Img/Waiting.gif" Height="150px" Width="150px" />
    </div>

    <asp:UpdatePanel ID="UpdatePanel02" runat="server">
        <ContentTemplate>
            <div class="Message-Area">
                <table>
                    <tr>
                        <td class="auto-style4">
                            <b>Department :</b>
                        </td>
                        <td>
                            <uc1:DepartmentUserControl runat="server" ID="ucDepartment"  OnDepartmentSelectedIndexChanged="OnDepartmentSelectedIndexChanged" />
                        </td>
                        <td class="auto-style4">
                            <b>Program :</b>
                        </td>
                        <td class="auto-style6">
                            <uc1:ProgramUserControl runat="server" ID="ucProgram" OnProgramSelectedIndexChanged="OnProgramSelectedIndexChanged" />
                        </td>
                        <td class="auto-style4"><b>Session :</b></td>
                        <td class="auto-style2">
                            <uc1:AdmissionSessionUserControl runat="server" ID="ucFilterCurrentSession" class="margin-zero dropDownList"/>
                        </td>
                    </tr>
                    <tr>
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
                        <td class="auto-style6">
                            <asp:DropDownList ID="ddlSemesterNo" Width="150px"  AutoPostBack="true"  runat="server" OnSelectedIndexChanged="ddlSemesterNo_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                        <td class="auto-style4">
                            <asp:Label ID="Label10" runat="server" Text="Exam : "></asp:Label>
                        </td>
                        <td class="auto-style6">
                            <asp:DropDownList ID="ddlExam" Width="250" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlExam_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style4">
                            <b>Examiner Type :</b>
                        </td>
                        <td class="auto-style6">
                            <asp:DropDownList ID="ddlExaminerType" Width="150px" runat="server" OnSelectedIndexChanged="ddlExaminerType_SelectedIndexChanged" AutoPostBack="True">
                                <asp:ListItem Text="First Examiner" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Second Examiner" Value="2"></asp:ListItem>
                                <asp:ListItem Text="Third Examiner" Value="3"></asp:ListItem>
                            </asp:DropDownList>

                        </td>
                        <td class="auto-style4">
                            <b>Course :</b>
                        </td>
                        <td class="auto-style6">
                            <asp:DropDownList ID="ddlCourse" AutoPostBack="true" Width="350px" OnSelectedIndexChanged="ddlCourse_SelectedIndexChanged" runat="server"></asp:DropDownList>
                        </td>
                        <%--<td class="auto-style4">
                            <b>Course Exam :</b>
                        </td>
                        <td class="auto-style6">
                            <asp:DropDownList ID="ddlContinousExam" Width="150px" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlContinousExam_SelectedIndexChanged"></asp:DropDownList>
                            <asp:Label ID="lblExamTemplateItemId" Visible="false" runat="server"></asp:Label>
                        </td>--%>
                        <td class="auto-style4">
                            <b>Answer Limit :</b>
                        </td>
                        <td class="auto-style6">
                            <asp:DropDownList ID="ddlQuestionNo" Width="150px" runat="server" AutoPostBack="True" >
                                <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                <asp:ListItem Text="10" Value="10"></asp:ListItem>
                            </asp:DropDownList>
                            
                        </td>

                        
                        <td>
                            <asp:Button ID="btnLoad" runat="server" Text="Load" OnClick="btnLoad_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="auto-style4">
                            <asp:Label ID="Label1" runat="server" Text="Mark : " Font-Bold="true" ForeColor="Blue"></asp:Label>
                            <asp:Label ID="lblExamMark" runat="server" Text="" Font-Bold="true" ForeColor="Blue"></asp:Label>
                            <asp:Label ID="lblExamTemplateItemId" runat="server" Visible="false"></asp:Label>
                            <asp:Label ID="lblExaminerType" runat="server" Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                </table>
            </div>
            <div class="Message-Area">
                <asp:Panel runat="server" ID="pnlTotalMark" Visible="false">
                   <table> 
                     <tr>
                        <td class="auto-style4">
                            <asp:Button runat="server" ID="btnFinalExamMarkSubmitToCommittee" Text="Submit Mark To Exam Committee" OnClick="btnFinalExamMarkSubmitToCommittee_Click" OnClientClick="return confirm('Are you sure? You will not be able to change any marks of selected course after submission!');"/>
                        </td>
                    </tr>
                    </table>
                </asp:Panel>
            </div>
            <br />
            
            <asp:GridView ID="ResultEntryGrid" runat="server" AllowSorting="True" CssClass="table-bordered "
            AutoGenerateColumns="False" ShowFooter="True" Width="90%" CellPadding="4" ForeColor="#333333" GridLines="None"
              >
                <%--OnRowDataBound="ResultEntryGrid_OnRowDataBound"  OnRowCommand="ResultEntryGrid_RowCommand"--%>
                    <HeaderStyle BackColor="#1C5E55" ForeColor="White" Height="30" Font-Bold="True" HorizontalAlign="Center" />
                    <FooterStyle BackColor="#1C5E55" ForeColor="White" Height="30" Font-Bold="True" />
                    <AlternatingRowStyle BackColor="White" />
                    <RowStyle Height="25" />
                <Columns>
                    <asp:TemplateField Visible ="false"  HeaderText="Student Id">
                        <ItemTemplate >
                            <asp:Label ID="lblStudentId"  runat="server" Text='<%# Bind("StudentID") %>'></asp:Label> 
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                            <HeaderStyle Width="150px" />
                    </asp:TemplateField>

                    <asp:TemplateField Visible ="false"  HeaderText="Course History Id">
                        <ItemTemplate >
                            <asp:Label ID="lblCourseHistoryId"  runat="server" Text='<%# Bind("CourseHistoryId") %>'></asp:Label> 
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                            <HeaderStyle Width="150px" />
                    </asp:TemplateField>

                    <asp:TemplateField Visible ="false"  HeaderText="Exam Mark First Second Third Id">
                        <ItemTemplate >
                            <asp:Label ID="lblExamMarkFirstSecondThirdExaminerId"  runat="server" Text='<%# Bind("ExamMarkFirstSecondThirdExaminerId") %>'></asp:Label> 
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                            <HeaderStyle Width="150px" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="SI." ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate><b><%# Container.DataItemIndex + 1 %></b></ItemTemplate>
                        <HeaderStyle Width="30px" />
                    </asp:TemplateField>

                    <%--<asp:TemplateField  HeaderText="ExamMarkDetailsID" Visible="false">
                        <ItemTemplate >
                            <asp:Label ID="lblExamMarkDetailId" runat="server" Text='<%# Bind("ExamMarkDetailId") %>'></asp:Label> 
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>--%>

                    <asp:TemplateField  HeaderText="Student Id">
                        <ItemTemplate >
                            <asp:Label ID="lblStudentRoll" width="100px"  runat="server" Text='<%# Bind("Roll") %>'></asp:Label> 
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>

                    <asp:TemplateField  HeaderText="Student Name">
                        <ItemTemplate >
                            <asp:Label ID="lblStudentName" width="180px"  runat="server" Text='<%# Bind("FullName") %>'></asp:Label> 
                        </ItemTemplate>
                        <ItemStyle  />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="1">
                        <ItemTemplate >
                            <asp:TextBox ID="txtQustion1Mark" width="50px" class="inp" pattern="^\d*(\.\d{0,2})?$"  onKeyUp="myFunction(event)" runat="server" Text='<%# Bind("Question1Marks") %>'/>
                        </ItemTemplate>
                        <ItemStyle CssClass="center" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="2">
                        <ItemTemplate >
                            <asp:TextBox ID="txtQustion2Mark" width="50px" CssClass="inp" pattern="^\d*(\.\d{0,2})?$" onKeyUp="myFunction(event)" runat="server" Text='<%# Bind("Question2Marks") %>'/>
                        </ItemTemplate>
                        <ItemStyle CssClass="center" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="3">
                        <ItemTemplate >
                            <asp:TextBox ID="txtQustion3Mark" width="50px" CssClass="inp" pattern="^\d*(\.\d{0,2})?$"  onKeyUp="myFunction(event)" runat="server" Text='<%# Bind("Question3Marks") %>'/>
                        </ItemTemplate>
                        <ItemStyle CssClass="center" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="4">
                        <ItemTemplate >
                            <asp:TextBox ID="txtQustion4Mark" width="50px" CssClass="inp" pattern="^\d*(\.\d{0,2})?$" onKeyUp="myFunction(event)" runat="server" Text='<%# Bind("Question4Marks") %>'/>
                        </ItemTemplate>
                        <ItemStyle CssClass="center" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="5">
                        <ItemTemplate >
                            <asp:TextBox ID="txtQustion5Mark" width="50px" CssClass="inp" pattern="^\d*(\.\d{0,2})?$"  onKeyUp="myFunction(event)" runat="server" Text='<%# Bind("Question5Marks") %>'/>
                        </ItemTemplate>
                        <ItemStyle CssClass="center" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="6">
                        <ItemTemplate >
                            <asp:TextBox ID="txtQustion6Mark" width="50px" CssClass="inp" pattern="^\d*(\.\d{0,2})?$"  onKeyUp="myFunction(event)" runat="server" Text='<%# Bind("Question6Marks") %>'/>
                        </ItemTemplate>
                        <ItemStyle CssClass="center" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="7">
                        <ItemTemplate >
                            <asp:TextBox ID="txtQustion7Mark" width="50px" CssClass="inp" pattern="^\d*(\.\d{0,2})?$"  onKeyUp="myFunction(event)" runat="server" Text='<%# Bind("Question7Marks") %>'/>
                        </ItemTemplate>
                        <ItemStyle CssClass="center" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="8">
                        <ItemTemplate >
                            <asp:TextBox ID="txtQustion8Mark" width="50px" CssClass="inp" pattern="^\d*(\.\d{0,2})?$"  onKeyUp="myFunction(event)" runat="server" Text='<%# Bind("Question8Marks") %>'/>
                        </ItemTemplate>
                        <ItemStyle CssClass="center" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="9">
                        <ItemTemplate >
                            <asp:TextBox ID="txtQustion9Mark" width="50px" CssClass="inp" pattern="^\d*(\.\d{0,2})?$" onKeyUp="myFunction(event)" runat="server" Text='<%# Bind("Question9Marks") %>'/>
                        </ItemTemplate>
                        <ItemStyle CssClass="center" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="10">
                        <ItemTemplate >
                            <asp:TextBox ID="txtQustion10Mark" width="50px" CssClass="inp" pattern="^\d*(\.\d{0,2})?$" onKeyUp="myFunction(event)" runat="server" Text='<%# Bind("Question10Marks") %>'/>
                        </ItemTemplate>
                        <ItemStyle CssClass="center" />
                    </asp:TemplateField>

                    <asp:TemplateField Visible="false" HeaderText="Answered Count">
                        <ItemTemplate >
                            <asp:Label ID="lblQuestionAnsweredCount" runat="server"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="center" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Total Mark">
                        <HeaderTemplate>
                            <asp:Button ID="btnSubmitFinalExamMark" runat="server"  Text="Save All" OnClick="btnSubmitFinalExamMark_Click" OnClientClick="return confirm('Do you really want save exam mark?');" />
                        </HeaderTemplate>
                        <ItemTemplate >
                            <asp:Label ID="lblTotalMark" width="70px" CssClass="inp" runat="server" Text='<%# Bind("ExaminerTotalMark") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="center" />
                    </asp:TemplateField>



                    <%-- <asp:TemplateField HeaderText="" Visible="false">
                        <ItemTemplate >
                                <asp:Label ID="lblExamStatus" width="70px"  runat="server" Text='<%# Bind("ExamStatus") %>'/>
                        </ItemTemplate>
                        <ItemStyle CssClass="center" />
                    </asp:TemplateField>--%>

                    <%-- <asp:TemplateField HeaderText="Status">
                        <ItemTemplate >
                            <asp:CheckBox ID="chkStatus" runat="server" Font-Bold="true" Checked='<%#(Eval("ExamStatus")).ToString() == "2" ? true : false %>' Text="Absent" AutoPostBack="true" OnCheckedChanged="chkStatus_CheckedChanged"/>
                        </ItemTemplate>
                        <ItemStyle CssClass="center" />
                    </asp:TemplateField>--%>

                    <asp:TemplateField  HeaderText="Remark">
                        <ItemTemplate>
                            <asp:Label ID="lblRemark" ForeColor="Red"  width="200px" runat="server"></asp:Label>
                            <%--<asp:LinkButton ID="SubmitButton" CommandName="ResultSubmit" Text="Save" CommandArgument='<%# Bind("CourseHistoryId") %>' runat="server"></asp:LinkButton>--%>
                        </ItemTemplate>
                        <HeaderStyle Width="200px"></HeaderStyle>
                        <ItemStyle CssClass="center" />
                    </asp:TemplateField>
                </Columns>
            
                <EmptyDataTemplate>
                    <label>Data Not Found</label>No data found!
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

    <ajaxToolkit:UpdatePanelAnimationExtender
        ID="UpdatePanelAnimationExtender1"
        TargetControlID="UpdatePanel02"
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
</div>   
</asp:Content>

