<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true"
    CodeBehind="StudentResultHistory.aspx.cs" Inherits="EMS.Module.student.StudentResultHistory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Student Result History
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript">

        function InProgress() {
            var panelProg = $get('PnProcess');
            panelProg.style.display = 'inline-block';
        }

        function onComplete() {
            var panelProg = $get('PnProcess');
            panelProg.style.display = 'none';
        }

    </script>
    <style>
        .tableHead {
            text-align: center;
        }

        .auto-style1 {
            width: 250px;
            height: 50px;
        }

        .auto-style2 {
            width: 350px;
            height: 50px;
        }

        .auto-style3 {
            width: 300px;
        }

        .auto-style4 {
            width: 200px;
        }

        .auto-style5 {
            height: 50px;
        }

        .auto-style6 {
            width: 300px;
            height: 50px;
        }

        .tableNew {
            display: table;
            border-color: grey;
            width: 100% !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContainer" runat="server">


    <div class="row">
        <div class="col-sm-12" style="font-size: 12pt; margin-top: 10pt;">
            <label><b style="color: black; font-size: 26px">Student Result History</b></label>
        </div>
    </div>
    <div id="divProgress" style="display: none; z-index: 100000000; position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%);">
        <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Img/Waiting.gif" Height="300px" Width="300px" />
        <div>
            <asp:Label ID="Label3" runat="server" Text="Processing your request.........." ForeColor="Red" Font-Bold="true" Font-Italic="true" Font-Size="30px"></asp:Label>
        </div>
    </div>

    <hr />


    <div>


        <div class="Message-Area">
            <asp:UpdatePanel runat="server" ID="UpdatePanel01">
                <ContentTemplate>
                    <script type="text/javascript">
                        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InIEvent);
                    </script>

                    <div class="card">
                        <div class="card-body">
                            <div class="row">
                                <div class="col-lg-2 c0l-md-2 col-sm-2">
                                    <b>Student ID</b>
                                    <asp:TextBox runat="server" ID="txtStudentId" class="form-control" placeholder="Student ID" Width="100%" Height="35px" />
                                </div>
                                <div class="col-lg-2 c0l-md-2 col-sm-2">
                                    <br />
                                    <asp:Button runat="server" ID="btnLoad" Text="Load" OnClick="btnLoad_Click" Height="35px" Width="100%" CssClass="btn-info" />
                                </div>
                            </div>

                            <div class="row" style="margin-top: 10px">
                                <div class="col-lg-1 col-md-1 col-sm-1">
                                    <b>Student Name</b>
                                </div>
                                <div class="col-lg-2 col-md-2 col-sm-2" style="text-align: left">
                                    <asp:Label runat="server" ID="lblStudentName" Text=".............." />
                                </div>
                                <div class="col-lg-2 col-md-2 col-sm-2" style="text-align: right">
                                    <b>Program</b>
                                </div>
                                <div class="col-lg-2 col-md-2 col-sm-2" style="text-align: left">
                                    <asp:Label runat="server" ID="lblStudentProgram" Text="............." />
                                </div>
                                <div class="col-lg-2 col-md-2 col-sm-2" style="text-align: right">
                                    <b>Active Status</b>
                                </div>
                                <div class="col-lg-2 col-md-2 col-sm-2" style="text-align: left">
                                    <asp:Label runat="server" ID="lblActiveStatus" />
                                </div>
                            </div>
                        </div>
                    </div>

                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div style="position: static">
            <asp:UpdatePanel runat="server" ID="UpdatePanel03">
                <ContentTemplate>
                    <div class="card" style="margin-top: 5px">
                        <div class="card-body">

                            <div class="row">
                                <asp:Label runat="server" Font-Size="Large" ID="lblResult">Trimester wise SGPA and CGPA</asp:Label>
                            </div>

                            <div class="row">
                                <div class="tableNew" style="height: auto">
                                    <asp:Panel ID="pnlResult" runat="server" Wrap="False">
                                        <asp:GridView ID="gvResult" runat="server" AutoGenerateColumns="False" TabIndex="6" Width="50%">

                                            <HeaderStyle BackColor="#4285f4" ForeColor="White" Height="10px" Font-Bold="True" />
                                            <FooterStyle BackColor="#4285f4" ForeColor="White" Height="10px" Font-Bold="True" />
                                            <AlternatingRowStyle BackColor="White" />
                                            <RowStyle Height="10px" />

                                            <Columns>
                                                <asp:TemplateField HeaderText="Sl. No">
                                                    <ItemTemplate><b><%# Container.DataItemIndex + 1 %></b></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Held In" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblExam" Font-Bold="True" Text='<%#Eval("ExamDetail") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Year" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblYearName" Font-Bold="True" Text='<%#Eval("YearNo") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Semester" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblSemester" Font-Bold="True" Text='<%#Eval("SemesterNo") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Credit" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblCredit" Font-Bold="True" Text='<%#Eval("Credit") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="SGPA" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblGPA" Font-Bold="True" Text='<%#Eval("GPA") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="CGPA" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblCGPA" Font-Bold="True" Text='<%#Eval("CGPA") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>


                                            <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                                            <RowStyle Height="10px" VerticalAlign="Middle" HorizontalAlign="Left" BackColor="#E3EAEB" />
                                            <EditRowStyle BackColor="#7C6F57" />
                                            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                            <SortedAscendingCellStyle BackColor="#F8FAFA" />
                                            <SortedAscendingHeaderStyle BackColor="#246B61" />
                                            <SortedDescendingCellStyle BackColor="#D4DFE1" />
                                            <SortedDescendingHeaderStyle BackColor="#15524A" />
                                        </asp:GridView>
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div style="position: static">
            <asp:UpdatePanel runat="server" ID="UpdatePanel04">
                <ContentTemplate>
                    <div class="card" style="margin-top: 5px">
                        <div class="card-body">
                            <div class="row">
                                <asp:Label runat="server" ID="lblRegistered" Font-Size="Large" class="">Result of completed/registered courses</asp:Label>
                            </div>
                            <div class="row">
                                <div class="tableNew" style="height: auto">
                                    <asp:Panel ID="PnlRegisteredCourse" runat="server" Width="100%" Wrap="False">
                                        <asp:GridView ID="gvRegisteredCourse" runat="server" AutoGenerateColumns="False" TabIndex="6" Width="100%" ShowFooter="true">
                                            <HeaderStyle BackColor="#4285f4" ForeColor="White" Height="10px" Font-Bold="True" />
                                            <FooterStyle BackColor="#4285f4" ForeColor="White" Height="10px" Font-Bold="True" />
                                            <AlternatingRowStyle BackColor="White" />
                                            <RowStyle Height="10px" />

                                            <Columns>

                                                <asp:TemplateField HeaderText="Sl. No">
                                                    <ItemTemplate><b><%# Container.DataItemIndex + 1 %></b></ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Held In">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblExamName" Font-Bold="True" Text='<%#Eval("ExamName") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:TemplateField HeaderText="Year">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblYearName" Font-Bold="True" Text='<%#Eval("YearName") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Semester">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblSemester" Font-Bold="True" Text='<%#Eval("Semester") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Course Code">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblCourseCode" Text='<%#Eval("CourseCode") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Course Title">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblCourseName" Text='<%#Eval("CourseName") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Credit">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblCourseCredit" Font-Bold="True" Text='<%#Eval("CourseCredit") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Grade">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblObtainedGrade" Font-Bold="True" Text='<%#Eval("ObtainedGrade") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Grade Point">
                                                    <ItemTemplate>
                                                        <div style="text-align: center">
                                                            <asp:Label runat="server" ID="lblObtainedGrade" Font-Bold="True" Text='<%#Eval("ObtainedGPA") %>' />
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Status">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblStatus" Font-Bold="True" Text='<%#Eval("CourseStatus") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <%-- <asp:TemplateField HeaderText="Course Status" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblCourseStatus" Font-Bold="True" Text='<%#Eval("CourseStatus") %>'> 
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                            </Columns>

                                            <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                                            <RowStyle Height="10px" VerticalAlign="Middle" HorizontalAlign="Left" BackColor="#E3EAEB" />
                                            <EditRowStyle BackColor="#7C6F57" />
                                            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                            <SortedAscendingCellStyle BackColor="#F8FAFA" />
                                            <SortedAscendingHeaderStyle BackColor="#246B61" />
                                            <SortedDescendingCellStyle BackColor="#D4DFE1" />
                                            <SortedDescendingHeaderStyle BackColor="#15524A" />
                                        </asp:GridView>
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div style="position: static">
            <asp:UpdatePanel runat="server" ID="UpdatePanel05">
                <ContentTemplate>
                    <div class="card" style="margin-top: 5px">
                        <div class="card-body">
                            <div class="row">
                                <asp:Label runat="server" ID="lblWaiver" Font-Size="Large" class="">Transferred/Waived courses</asp:Label>
                            </div>
                            <div class="row">
                                <div class="tableNew" style="height: auto">
                                    <asp:Panel ID="PnlWaivedCourse" runat="server" Width="100%" Wrap="False">
                                        <asp:GridView ID="gvWaiVeredCourse" runat="server" AutoGenerateColumns="False" TabIndex="6" Width="100%">
                                            <HeaderStyle BackColor="#4285f4" ForeColor="White" Height="10px" Font-Bold="True" />
                                            <FooterStyle BackColor="#4285f4" ForeColor="White" Height="10px" Font-Bold="True" />
                                            <AlternatingRowStyle BackColor="White" />
                                            <RowStyle Height="10px" />
                                            <Columns>

                                                <asp:TemplateField HeaderText="Sl. No">
                                                    <ItemTemplate><b><%# Container.DataItemIndex + 1 %></b></ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Course ID" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblCourseCode" Font-Bold="True" Text='<%#Eval("CourseCode") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Course Name" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblCourseName" Text='<%#Eval("CourseName") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Credit" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label runat="server" ID="lblCourseCredit" Font-Bold="True" Text='<%#Eval("CourseCredit") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                            <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                                            <RowStyle Height="10px" VerticalAlign="Middle" HorizontalAlign="Left" BackColor="#E3EAEB" />
                                            <EditRowStyle BackColor="#7C6F57" />
                                            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                            <SortedAscendingCellStyle BackColor="#F8FAFA" />
                                            <SortedAscendingHeaderStyle BackColor="#246B61" />
                                            <SortedDescendingCellStyle BackColor="#D4DFE1" />
                                            <SortedDescendingHeaderStyle BackColor="#15524A" />
                                        </asp:GridView>
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <ajaxToolkit:UpdatePanelAnimationExtender ID="UpdatePanelAnimationExtender01" TargetControlID="UpdatePanel04" runat="server">
            <Animations>
                <OnUpdating><Parallel duration="0"><ScriptAction Script="InProgress();" /><EnableAction AnimationTarget="btnLoad" Enabled="false" /></Parallel></OnUpdating>
                <OnUpdated><Parallel duration="0"><ScriptAction Script="onComplete();" /><EnableAction   AnimationTarget="btnLoad" Enabled="true" /></Parallel></OnUpdated>
            </Animations>
        </ajaxToolkit:UpdatePanelAnimationExtender>

    </div>
</asp:Content>
