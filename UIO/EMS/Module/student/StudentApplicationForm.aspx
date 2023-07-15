<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true" UnobtrusiveValidationMode="none" CodeBehind="StudentApplicationForm.aspx.cs" Inherits="EMS.Module.student.StudentApplicationForm" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Student Application Form
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.2/css/all.min.css" rel="stylesheet" />
    <script src="../../JavaScript/appliForm.js"></script>
    <style> 
        .blink_me {
            animation: blinker 1s linear infinite;
            text-decoration-line:underline;
        }

        @keyframes blinker {
            50% {
                opacity: 0;
            }
        }
    </style>
    <script>
        setTimeout(function() {
            $('#ctl00_MainContainer_FinalMsg').fadeOut('fast');
        }, 1000);
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContainer" runat="server">
    <div class="container">                      

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:HiddenField ID="StudentApplicationOfficialInfoId" runat="server" />
                <div class="page-header">
                    <h4 class="page-title text-center">আবেদন পত্র</h4>
                </div>

                <div class="row">
                    <div class="col-lg-12">
                        <asp:Panel ID="FinalMsg" runat="server" Visible="false" CssClass="alert alert-success">
                            <asp:Label ID="lblFinalMsg" runat="server" Text="আপনার আবেদন পত্র চূড়ান্ত ভাবে জমা সম্পন্ন হয়েছে।"></asp:Label>                            
                            <asp:LinkButton ID="lnkAdminCardDownlad" runat="server" Font-Underline="true" CssClass="text-info" Text="প্রবেশপত্র ডাউনলোড করার জন্য এইখানে ক্লিক করুণ" OnClick="lnkAdminCardDownlad_Click"></asp:LinkButton>
                            <asp:HiddenField ID="hdnOfficialIdForAdmitCard" runat="server" />
                        </asp:Panel>
                    </div>
                </div>


                <div class="card">
                    <div class="card-header text-center">
                        <h5>১. অফিসিয়াল তথ্য</h5>
                    </div>

                    <div class="card-body">
                        <div class="row">
                            <div class="col-lg-6">
                                <div class="row">
                                    <label class="col-sm-5 form-control-label">অনুষদ: </label>
                                    <div class="col-sm-7 mb-2">
                                        <asp:TextBox ID="txtDepartment" runat="server" CssClass="form-control"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required" ValidationGroup="Official" ControlToValidate="txtDepartment" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                    </div>
                                </div>

                                <div class="row">
                                    <label class="col-sm-5 form-control-label">বিভাগ: <span class="text-danger">*</span></label>
                                    <div class="col-sm-7 mb-2">
                                        <asp:TextBox ID="txtFaculty" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Required" ValidationGroup="Official" ControlToValidate="txtFaculty" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="row">
                                    <label class="col-sm-5 form-control-label">হল: <span class="text-danger">*</span> </label>
                                    <div class="col-sm-7 mb-2">
                                        <asp:TextBox ID="txtHall" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Required" ValidationGroup="Official" ControlToValidate="txtHall" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="row">
                                    <label class="col-sm-5 form-control-label">পরীক্ষার্থী: <span class="text-danger">*</span></label>
                                    <div class="col-sm-7 mb-2">
                                        <asp:DropDownList ID="ddlExaminarType" runat="server" CssClass="form-control"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Required" InitialValue="0" ValidationGroup="Official" ControlToValidate="ddlExaminarType" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="row">
                                    <label class="col-sm-5 form-control-label">অনার্স: <span class="text-danger">*</span></label>
                                    <div class="col-sm-7 mb-2">
                                        <asp:DropDownList ID="ddlProgram" runat="server" CssClass="form-control"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="Required" InitialValue="0" ValidationGroup="Official" ControlToValidate="ddlProgram" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="row">
                                    <label class="col-sm-5 form-control-label">শিক্ষাবর্ষ: <span class="text-danger">*</span></label>
                                    <div class="col-sm-7 mb-2">
                                        <asp:TextBox ID="txtStudentAcademicYear" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="Required" ValidationGroup="Official" ControlToValidate="txtStudentAcademicYear" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                            </div>

                            <div class="col-lg-6">
                                <div class="row">
                                    <label class="col-sm-5 form-control-label">আইডি নম্বর: <span class="text-danger">*</span></label>
                                    <div class="col-sm-7 mb-2">
                                        <asp:TextBox ID="txtIDNo" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Required" ValidationGroup="Official" ControlToValidate="txtIDNo" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="row">
                                    <label class="col-sm-5 form-control-label">রেজিঃ নং: <span class="text-danger">*</span></label>
                                    <div class="col-sm-7 mb-2">
                                        <asp:TextBox ID="txtRegistrationNo" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Required" ValidationGroup="Official" ControlToValidate="txtRegistrationNo" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="row">
                                    <label class="col-sm-5 form-control-label">বর্ষ: <span class="text-danger">*</span></label>
                                    <div class="col-sm-7 mb-2">
                                        <asp:DropDownList ID="ddlStudentExamSession" runat="server" CssClass="form-control"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Required" InitialValue="0" ValidationGroup="Official" ControlToValidate="ddlStudentExamSession" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="row">
                                    <label class="col-sm-5 form-control-label">সেমিস্টার: <span class="text-danger">*</span></label>
                                    <div class="col-sm-7 mb-2">
                                        <asp:DropDownList ID="ddlStudentExamSemester" runat="server" CssClass="form-control"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="Required" InitialValue="0" ValidationGroup="Official" ControlToValidate="ddlStudentExamSemester" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="row">
                                    <label class="col-sm-5 form-control-label">সাল: <span class="text-danger">*</span></label>
                                    <div class="col-sm-7 mb-2">
                                        <asp:DropDownList ID="ddlStudentExamYear" runat="server" CssClass="form-control"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Required" InitialValue="0" ValidationGroup="Official" ControlToValidate="ddlStudentExamYear" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-sm-12">
                                        <asp:Button ID="btnOfficialInfo" runat="server" Text="Submit" OnClick="btnOfficialInfo_Click" ValidationGroup="Official" CssClass="btn btn-md btn-success float-right" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <br />

                <asp:Panel ID="personalDiv" runat="server" class="card">
                    <div class="card-header  text-center">
                        <h5>২. ব্যক্তিগত বিবরণ (সকল তথ্য মাধ্যমিক/ সমমান সনদ অনুযায়ী পূরণ করতে হবে)</h5>
                    </div>

                    <div class="card-body">
                        <div class="row">
                            <div class="col-lg-6">
                                <div class="row">
                                    <label class="col-sm-5 form-control-label">নাম ( বাংলায় ): <span class="text-danger">*</span></label>
                                    <div class="col-sm-7 mb-2">
                                        <asp:TextBox ID="txtNameBng" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="Required" ValidationGroup="Personal" ControlToValidate="txtNameBng" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="row">
                                    <label class="col-sm-5 form-control-label">মাতার নাম: <span class="text-danger">*</span></label>
                                    <div class="col-sm-7 mb-2">
                                        <asp:TextBox ID="txtMotherName" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="Required" ValidationGroup="Personal" ControlToValidate="txtMotherName" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="row">
                                    <label class="col-sm-5 form-control-label">পিতার অবর্তমানে অভিভাবকের নাম: <span class="text-danger">*</span></label>
                                    <div class="col-sm-7 mb-2">
                                        <asp:TextBox ID="txtGuardianName" runat="server" CssClass="form-control"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="Required" ValidationGroup="Personal" ControlToValidate="txtGuardianName" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                    </div>
                                </div>

                                <div class="row">
                                    <label class="col-sm-5 form-control-label">জন্ম তারিখ: <span class="text-danger">*</span></label>
                                    <div class="col-sm-7 mb-2">
                                        <asp:TextBox ID="txtDOB" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" Format="dd/MM/yyyy" BehaviorID="jCalender" TargetControlID="txtDOB" runat="server" />
                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="Required" ValidationGroup="Personal" ControlToValidate="txtDOB" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                        <asp:CompareValidator ID="CompareValidator1" runat="server" Type="Date" ValidationGroup="Personal" ControlToValidate="txtDOB" Operator="DataTypeCheck" ErrorMessage="date is not valid."></asp:CompareValidator>
                                    </div>
                                </div>


                                <div class="row">
                                    <label class="col-sm-5 form-control-label">বর্তমান ঠিকানা: <span class="text-danger">*</span></label>
                                    <div class="col-sm-7 mb-2">
                                        <asp:TextBox ID="txtPresentAddress" runat="server" TextMode="MultiLine" Rows="4" CssClass="form-control" Style="height: auto !important;"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="Required" ValidationGroup="Personal" ControlToValidate="txtPresentAddress" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-6">
                                <div class="row">
                                    <label class="col-sm-5 form-control-label">নাম ( ইংরেজিতে - বড় অক্ষরে): <span class="text-danger">*</span></label>
                                    <div class="col-sm-7 mb-2">
                                        <asp:TextBox ID="txtNameEng" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="Required" ValidationGroup="Personal" ControlToValidate="txtNameEng" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="row">
                                    <label class="col-sm-5 form-control-label">পিতার নাম: <span class="text-danger">*</span></label>
                                    <div class="col-sm-7 mb-2">
                                        <asp:TextBox ID="txtFatherName" runat="server" CssClass="form-control"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="Required" ValidationGroup="Personal" ControlToValidate="txtFatherName" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                    </div>
                                </div>

                                <div class="row">
                                    <label class="col-sm-5 form-control-label">জাতীয়তা: <span class="text-danger">*</span></label>
                                    <div class="col-sm-7 mb-2">
                                        <asp:DropDownList ID="ddlNationality" runat="server" CssClass="form-control"></asp:DropDownList>
                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ErrorMessage="Required" InitialValue="0" ValidationGroup="Personal" ControlToValidate="ddlNationality" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                    </div>
                                </div>

                                <div class="row">
                                    <label class="col-sm-5 form-control-label">মুঠোফোনে নম্বর: <span class="text-danger">*</span></label>
                                    <div class="col-sm-2  mb-2">
                                        <asp:Label ID="lblMobilePrefix" runat="server" Text="+88"></asp:Label>
                                    </div>
                                    <div class="col-sm-5 mb-2">
                                        <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ErrorMessage="Required" ValidationGroup="Personal" ControlToValidate="txtMobile" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtMobile" ValidationExpression="^[0][1]\d{9}$" ValidationGroup="Personal" ErrorMessage="mobile no not valid."></asp:RegularExpressionValidator>
                                        <ajaxToolkit:FilteredTextBoxExtender ID="filterResultSecondary" runat="server" TargetControlID="txtMobile" FilterType="Custom" ValidChars="+1234567890" Enabled="true" />
                                    </div>
                                </div>

                                <div class="row">
                                    <label class="col-sm-5 form-control-label">স্থায়ী ঠিকানা: <span class="text-danger">*</span></label>
                                    <div class="col-sm-7 mb-2">
                                        <asp:TextBox ID="txtPermanentAddress" runat="server" TextMode="MultiLine" Rows="4" CssClass="form-control" Style="height: auto !important;"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ErrorMessage="Required" ValidationGroup="Personal" ControlToValidate="txtPermanentAddress" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                    </div>
                                </div>


                                <div class="row">
                                    <div class="col-sm-12">
                                        <asp:Button ID="btnPersonalInfoSubmit" runat="server" Text="Submit" OnClick="btnPersonalInfoSubmit_Click" ValidationGroup="Personal" CssClass="btn btn-md btn-success float-right" />
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </asp:Panel>

                <br />

                <asp:Panel ID="EducationDiv" runat="server" class="card">
                    <div class="card-header  text-center">
                        <h5>৩. শিক্ষাগত বিবরণ</h5>
                    </div>

                    <asp:HiddenField ID="hdnEducationInfoId" runat="server" />

                    <div class="card-body">
                        <div class="row">
                            <div class="col-lg-6">
                                <div class="row">
                                    <label class="col-sm-5 form-control-label">পরীক্ষার নাম: <span class="text-danger">*</span></label>
                                    <div class="col-sm-7 mb-2">
                                        <asp:DropDownList ID="ddlExam" runat="server" CssClass="form-control"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ErrorMessage="Required" InitialValue="0" ValidationGroup="Education" ControlToValidate="ddlExam" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="row">
                                    <label class="col-sm-5 form-control-label">স্কুল / কলেজ: <span class="text-danger">*</span></label>
                                    <div class="col-sm-7 mb-2">
                                        <asp:TextBox ID="txtSchoolOrCollege" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" ErrorMessage="Required" ValidationGroup="Education" ControlToValidate="txtSchoolOrCollege" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="row">
                                    <label class="col-sm-5 form-control-label">বছর: <span class="text-danger">*</span></label>
                                    <div class="col-sm-7 mb-2">
                                        <asp:DropDownList ID="ddlYear" runat="server" CssClass="form-control"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" ErrorMessage="Required" InitialValue="0" ValidationGroup="Education" ControlToValidate="ddlYear" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-6">
                                <div class="row">
                                    <label class="col-sm-5 form-control-label">বোর্ড: <span class="text-danger">*</span></label>
                                    <div class="col-sm-7 mb-2">
                                        <asp:DropDownList ID="ddlEducationBoard" runat="server" CssClass="form-control"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server" ErrorMessage="Required" InitialValue="0" ValidationGroup="Education" ControlToValidate="ddlEducationBoard" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="row">
                                    <label class="col-sm-5 form-control-label">রোল নম্বর: <span class="text-danger">*</span></label>
                                    <div class="col-sm-7 mb-2">
                                        <asp:TextBox ID="txtRoll" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server" ErrorMessage="Required" ValidationGroup="Education" ControlToValidate="txtRoll" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="row">
                                    <label class="col-sm-5 form-control-label">গ্রেড: <span class="text-danger">*</span></label>
                                    <div class="col-sm-7 mb-2">
                                        <asp:TextBox ID="txtGrade" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server" ErrorMessage="Required" ValidationGroup="Education" ControlToValidate="txtGrade" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="row">
                                    <label class="col-sm-5 form-control-label">যে সমস্ত বিষয়ে পরীক্ষা দিয়েছে (সংক্ষেপে): <span class="text-danger">*</span></label>
                                    <div class="col-sm-7 mb-2">
                                        <asp:TextBox ID="txtExamShortDetail" runat="server" CssClass="form-control"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator25" runat="server" ErrorMessage="Required" ValidationGroup="Education" ControlToValidate="txtExamShortDetail" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-sm-12">
                                        <asp:Button ID="btnEducationSubmit" runat="server" Text="Submit" OnClick="btnEducationSubmit_Click" ValidationGroup="Education" CssClass="btn btn-md btn-success float-right" />
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-12">
                                <asp:GridView ID="grdEducation" runat="server" AutoGenerateColumns="false" Width="100%" ShowHeaderWhenEmpty="true">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" BackColor="#4285f4" Font-Bold="true" ForeColor="White" />
                                    <RowStyle HorizontalAlign="Left" VerticalAlign="Top" ForeColor="Black" />
                                    <AlternatingRowStyle BackColor="" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="সিরিয়াল." ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate><b><%# Container.DataItemIndex + 1 %><span>.</span></b></ItemTemplate>
                                            <HeaderStyle Width="50px" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="পরীক্ষার নাম">
                                            <ItemTemplate>
                                                <asp:Label ID="lblExamName" runat="server" Text='<%# Eval("ExamNameEng") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="100px" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="বোর্ড">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBoard" runat="server" Text='<%# Eval("BoardEng") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="100px" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="স্কুল / কলেজ">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSchoolCollege" runat="server" Text='<%# Eval("AppliedStudentSchoolOrCollege") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="250px" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="রোল নম্বর">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRoll" runat="server" Text='<%# Eval("AppliedStudentRoll") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="100px" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="বছর">
                                            <ItemTemplate>
                                                <asp:Label ID="lblExamYear" runat="server" Text='<%# Eval("EducationYear") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="80px" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="গ্রেড">
                                            <ItemTemplate>
                                                <asp:Label ID="lblExamGrade" runat="server" Text='<%# Eval("AppliedStudentGrade") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="50px" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="পরীক্ষার নামযে সমস্ত বিষয়ে পরীক্ষা দিয়েছে (সংক্ষেপে)">
                                            <ItemTemplate>
                                                <asp:Label ID="lblExamRemarks" runat="server" Text='<%# Eval("AppliedStudentExamRemarks") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="250px" />
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkEducationEdit" runat="server" Text="Edit" CommandArgument='<%# Eval("StudentApplicationFormEducationId") %>' OnClick="lnkEducationEdit_Click"></asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle Width="50px" />
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkEducationDelete" runat="server" Text="Delete" OnClientClick="return confirm('Do you really want to delete?');" CommandArgument='<%# Eval("StudentApplicationFormEducationId") %>' OnClick="lnkEducationDelete_Click"></asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle Width="50px" />
                                        </asp:TemplateField>

                                    </Columns>
                                    <EmptyDataTemplate>
                                        <p>No data found</p>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </asp:Panel>

                <br />

                <asp:Panel ID="PreviousSemesterDiv" runat="server" class="card">
                    <div class="card-header  text-center">
                        <h5>৪. পূর্ববর্তী সেমিস্টার ফাইনাল পরীক্ষার পাশের বিবরণ (প্রযোজ্য ক্ষেত্রে)</h5>
                    </div>

                    <asp:HiddenField ID="hdnPreviousSemeseterId" runat="server" />

                    <div class="card-body">
                        <div class="row">
                            <div class="col-lg-6">
                                <div class="row">
                                    <label class="col-sm-5 form-control-label">পরীক্ষার নাম: <span class="text-danger">*</span></label>
                                    <div class="col-sm-7 mb-2">
                                        <asp:TextBox ID="txtPreviousSemesterExamName" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator26" runat="server" ErrorMessage="Required" ValidationGroup="PreSemester" ControlToValidate="txtPreviousSemesterExamName" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="row">
                                    <label class="col-sm-5 form-control-label">ফলাফল (GPA/CGPA): <span class="text-danger">*</span></label>
                                    <div class="col-sm-7 mb-2">
                                        <asp:TextBox ID="txtPreviousSemesterExamResult" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator27" runat="server" ErrorMessage="Required" ValidationGroup="PreSemester" ControlToValidate="txtPreviousSemesterExamResult" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="row">
                                    <label class="col-sm-5 form-control-label">কোর্স কোড: <span class="text-danger">*</span></label>
                                    <div class="col-sm-7 mb-2">
                                        <asp:DropDownList ID="ddlPreviousSemesterCourseCode" runat="server" CssClass="form-control"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator28" runat="server" ErrorMessage="Required" InitialValue="0" ValidationGroup="PreSemester" ControlToValidate="ddlPreviousSemesterCourseCode" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                            </div>

                            <div class="col-lg-6">

                                <div class="row">
                                    <label class="col-sm-5 form-control-label">পরীক্ষার বছর: <span class="text-danger">*</span></label>
                                    <div class="col-sm-7 mb-2">
                                        <asp:DropDownList ID="ddlExamYear" runat="server" CssClass="form-control"></asp:DropDownList>
                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator29" runat="server" ErrorMessage="Required" InitialValue="0" ValidationGroup="PreSemester" ControlToValidate="ddlExamYear" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                    </div>
                                </div>

                                <div class="row">
                                    <label class="col-sm-5 form-control-label">কোর্স GP: <span class="text-danger">*</span></label>
                                    <div class="col-sm-7 mb-2">
                                        <asp:TextBox ID="txtPreviousSemesterCourseGP" runat="server" CssClass="form-control"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator30" runat="server" ErrorMessage="Required" ValidationGroup="PreSemester" ControlToValidate="txtPreviousSemesterCourseGP" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-sm-12">
                                        <asp:Button ID="btnPreviousSemesterInfoSubmit" runat="server" Text="Submit" OnClick="btnPreviousSemesterInfoSubmit_Click" ValidationGroup="PreSemester" CssClass="btn btn-md btn-success float-right" />
                                    </div>
                                </div>

                            </div>

                            <div class="col-lg-12">
                                <asp:GridView ID="grdPreviousSemester" runat="server" AutoGenerateColumns="false" Width="100%" ShowHeaderWhenEmpty="true">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" BackColor="#4285f4" Font-Bold="true" ForeColor="White" />
                                    <RowStyle HorizontalAlign="Left" VerticalAlign="Top" ForeColor="Black" />
                                    <AlternatingRowStyle BackColor="" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="সিরিয়াল." ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate><b><%# Container.DataItemIndex + 1 %><span>.</span></b></ItemTemplate>
                                            <HeaderStyle Width="50px" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="পরীক্ষার নাম">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPreExamName" runat="server" Text='<%# Eval("PreviousSemesterExamName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="100px" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="পরীক্ষার বছর">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPreYear" runat="server" Text='<%# Eval("ExamYear") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="80px" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="ফলাফল (GPA/CGPA)">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPreResult" runat="server" Text='<%# Eval("PreviousSemesterResult") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="80px" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="কোর্স GP">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPreGp" runat="server" Text='<%# Eval("PreviousSemseterCourseGP") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="80px" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="কোর্স কোড">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPreCode" runat="server" Text='<%# Eval("CourseCode") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="100px" />
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkPreviousSemesterEdit" runat="server" Text="Edit" CommandArgument='<%# Eval("PreviousSemesterId") %>' OnClick="lnkPreviousSemesterEdit_Click"></asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle Width="50px" />
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkPreviousSemesterDelete" runat="server" Text="Delete" OnClientClick="return confirm('Do you really want to delete?');" CommandArgument='<%# Eval("PreviousSemesterId") %>' OnClick="lnkPreviousSemesterDelete_Click"></asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle Width="50px" />
                                        </asp:TemplateField>

                                    </Columns>
                                    <EmptyDataTemplate>
                                        <p>No data found</p>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </asp:Panel>

                <br />

                <asp:Panel ID="CourseDiv" runat="server" class="card">
                    <div class="card-header  text-center">
                        <h5>৫. যে কোর্সে পরীক্ষার্থী পরীক্ষা দিতে ইচ্ছুক তার বিস্তারিত নাম </h5>
                    </div>

                    <asp:HiddenField ID="hdnExamCourseId" runat="server" />

                    <div class="card-body">
                        <div class="row">
                            <div class="col-lg-6">
                                <div class="row">
                                    <label class="col-sm-5 form-control-label">কোর্স কোড: <span class="text-danger">*</span></label>
                                    <div class="col-sm-7 mb-2">
                                        <asp:DropDownList ID="ddlCourseCode" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlCourseCode_SelectedIndexChanged"></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator31" runat="server" ErrorMessage="Required" InitialValue="0" ValidationGroup="ExpectedCourse" ControlToValidate="ddlCourseCode" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-6">

                                <div class="row">
                                    <label class="col-sm-5 form-control-label">কোর্সের শিরোনাম: <span class="text-danger">*</span></label>
                                    <div class="col-sm-7 mb-2">
                                        <asp:TextBox ID="txtCourseSummary" runat="server" CssClass="form-control"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator32" runat="server" ErrorMessage="Required" ValidationGroup="ExpectedCourse" ControlToValidate="txtCourseSummary" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                    </div>
                                </div>


                                <div class="row">
                                    <div class="col-sm-12">
                                        <asp:Button ID="btnExamCourseSubmit" runat="server" Text="Submit" OnClick="btnExamCourseSubmit_Click" ValidationGroup="ExpectedCourse" CssClass="btn btn-md btn-success float-right" />
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-12">
                                <asp:GridView ID="grdExpectedCourse" runat="server" AutoGenerateColumns="false" Width="100%" ShowHeaderWhenEmpty="true">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" BackColor="#4285f4" Font-Bold="true" ForeColor="White" />
                                    <RowStyle HorizontalAlign="Left" VerticalAlign="Top" ForeColor="Black" />
                                    <AlternatingRowStyle BackColor="" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="সিরিয়াল." ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate><b><%# Container.DataItemIndex + 1 %><span>.</span></b></ItemTemplate>
                                            <HeaderStyle Width="50px" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="কোর্স কোড">
                                            <ItemTemplate>
                                                <asp:Label ID="lblExpectedCode" runat="server" Text='<%# Eval("CourseCode") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="100px" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="কোর্সের শিরোনাম">
                                            <ItemTemplate>
                                                <asp:Label ID="lblExpectedCourseSummary" runat="server" Text='<%# Eval("AppliedCourseSummary") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="250px" />
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkExpectedCourseEdit" runat="server" Text="Edit" CommandArgument='<%# Eval("AppliedCourseId") %>' OnClick="lnkExpectedCourseEdit_Click"></asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle Width="50px" />
                                        </asp:TemplateField>

                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkExpectedCourseDelete" runat="server" Text="Delete" OnClientClick="return confirm('Do you really want to delete?');" CommandArgument='<%# Eval("AppliedCourseId") %>' OnClick="lnkExpectedCourseDelete_Click"></asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle Width="50px" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <p>No data found</p>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </asp:Panel>

                <br />

                <asp:Panel ID="picAndSigDiv" runat="server" class="card">
                    <div class="card-header  text-center">
                        <h5>৬. ছবি এবং স্বাক্ষর</h5>
                    </div>


                    <div class="card-body">
                        <div class="row">
                            <div class="col-lg-12">
                                <div class="alert alert-warning" role="alert">
                                    File size maximum 200KB. Accpted file format is jpg, jpeg & png. File dimensions for photo is 300 X 300 pixel and for signature is 100 X 80 pixel;
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-12">
                                <div class="alert alert-primary" role="alert" style="height: 50px !important;">
                                    <p>For Picture resize <b><a target="_blank" href="https://picresize.com/" class="blink_me">Click Here</a></p>
                                    </b>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-lg-12">
                                <asp:UpdatePanel ID="UpdatePanelMassage" runat="server">
                                    <ContentTemplate>
                                        <asp:Panel ID="messagePanel" runat="server">
                                            <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                                        </asp:Panel>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>

                        <div class="row">

                            <div class="col-lg-6">
                                <div class="row">
                                    <div class="col-sm-5 form-control-label">
                                        <asp:Image ID="imgPhoto" runat="server" Style="width: 150px; height: 150px; max-width: 300px; max-height: 300px;" />
                                    </div>
                                    <div class="col-sm-7 mb-2">
                                        <asp:FileUpload ID="imgPhotoUpload" runat="server" />
                                        <br />
                                        <asp:Button ID="btnImageUpload" runat="server" Text="Upload" OnClick="btnImageUpload_Click" CausesValidation="false" CssClass="btn btn-md btn-primary" />
                                    </div>
                                </div>
                            </div>

                            <div class="col-lg-6">
                                <div class="row">
                                    <div class="col-sm-5 form-control-label">
                                        <asp:Image ID="imgSignature" runat="server" Width="300px" Height="80px" />
                                    </div>
                                    <div class="col-sm-7 mb-2">
                                        <asp:FileUpload ID="imgSignatureUpload" runat="server" />
                                        <br />
                                        <asp:Button ID="btnSignatureUpload" runat="server" Text="Upload" OnClick="btnSignatureUpload_Click" CausesValidation="false" CssClass="btn btn-md btn-primary" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-lg-12 text-center">
                                <asp:Button ID="btnFinalSubmit" runat="server" Text="ফর্ম দেখান" OnClick="btnFinalSubmit_Click" CssClass="btn btn-lg btn-success" />
                            </div>
                        </div>
                    </div>

                    <div>
                        <rsweb:ReportViewer ID="ReportViewer1" runat="server" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" asynrendering="true" Width="100%" Height="100%" SizeToReportContent="true">
                        </rsweb:ReportViewer>
                    </div>
                </asp:Panel>               
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnImageUpload" />
                <asp:PostBackTrigger ControlID="btnSignatureUpload" />
                <asp:PostBackTrigger ControlID="lnkAdminCardDownlad" />
            </Triggers>
        </asp:UpdatePanel>        
    </div>
</asp:Content>
