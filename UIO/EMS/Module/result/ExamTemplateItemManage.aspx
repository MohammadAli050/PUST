<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true"
    CodeBehind="ExamTemplateItemManage.aspx.cs" Inherits="EMS.Module.result.ExamTemplateItemManage" %>

<asp:Content ID="Content3" ContentPlaceHolderID="Title" runat="Server">
    Exam Template Item
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
    <link rel="stylesheet" type="text/css" href="../../CSS/MarkTemplate.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContainer" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <div class="PageTitle">
                    <label>Exam Template Item</label>
                </div>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <div class="Message-Area">
                            <label class="msgTitle">Message: </label>
                            <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label><br />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />


                <div class="card">
                    <div class="card-body">
                        <div class="row">
                            <div class="col-lg-5 col-md-5 col-sm-5">
                                <b>Template Name</b>
                                <asp:DropDownList ID="ddlExamTemplateName" CssClass="form-control" Width="100%" AutoPostBack="True" runat="server" OnSelectedIndexChanged="ddlTemplateName_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <b>Template Mark</b>
                                <asp:TextBox ID="txtTemplateMark" Enabled="false" CssClass="form-control" Width="100%" Placeholder="Template Mark" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-lg-5 col-md-5 col-sm-5">
                                <div class="row">
                                    <b>Choose Template Type</b>
                                </div>
                                <div class="row">
                                    <div class="col-lg-4 col-md-4 col-sm-4">
                                        <asp:RadioButton ID="rdbBasicColumn" AutoPostBack="true" Text="Basic Column" GroupName="StatusRadioGroup" runat="server" OnCheckedChanged="rdbBasicColumn_CheckedChanged" Checked="True" />
                                    </div>
                                    <div class="col-lg-4 col-md-4 col-sm-4">
                                        <asp:RadioButton ID="rdbCalculativeColumn" AutoPostBack="true" Text="Calculative Column" GroupName="StatusRadioGroup" runat="server" OnCheckedChanged="rdbCalculativeColumn_CheckedChanged" />
                                    </div>
                                    <div class="col-lg-4 col-md-4 col-sm-4">
                                        <asp:RadioButton ID="rdbGradeColumn" AutoPostBack="true" Text="Grade Column" GroupName="StatusRadioGroup" runat="server" OnCheckedChanged="rdbGradeColumn_CheckedChanged" />
                                    </div>
                                </div>

                            </div>
                        </div>
                        <div class="row" style="margin-top: 15px">
                            <div class="col-lg-3 col-md-3 col-sm-3">
                                <b>Exam Name</b>
                                <asp:TextBox ID="txtExamName" Placeholder="Exam/Column Name" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                            </div>
                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <b>Exam Mark</b>
                                <asp:TextBox ID="txtExamMark" Enabled="false" Placeholder="Exam/Column Mark" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                            </div>
                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <b>Pass Mark</b>
                                <asp:TextBox ID="txtExamPassMark" Enabled="false" Placeholder="Exam Pass Mark" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                            </div>
                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <b>Sequence</b>
                                <asp:TextBox ID="txtSequence" Placeholder="Column Sequence" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                            </div>
                            <div class="col-lg-3 col-md-3 col-sm-3">
                                <b>Print Sequence</b>
                                <asp:TextBox ID="txtPrintSequence" Placeholder="Print Sequence" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row" style="margin-top: 15px">
                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <b>Continous Assesment</b>
                                <br />
                                <asp:RadioButton ID="rdbContinousAssesmentTrue" Text="Yes" AutoPostBack="true" GroupName="ContinousAssesmentRadioGroup" runat="server" />
                                <br />
                                <asp:RadioButton ID="rdbContinousAssesmentFalse" Text="No" AutoPostBack="true" GroupName="ContinousAssesmentRadioGroup" runat="server" />
                            </div>
                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <b>Viva</b>
                                <br />
                                <asp:RadioButton ID="rdbVivaTrue" Text="Yes" AutoPostBack="true" GroupName="VivaRadioGroup" runat="server" />
                                <br />
                                <asp:RadioButton ID="rdbVivaFalse" Text="No" AutoPostBack="true" GroupName="VivaRadioGroup" runat="server" />
                            </div>
                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <b>Show In Tabulation</b>
                                <br />
                                <asp:RadioButton ID="rdbTrue" Text="Yes" AutoPostBack="true" GroupName="ShowInTabulationRadioGroup" runat="server" OnCheckedChanged="rdbTrue_CheckedChanged" />
                                <br />
                                <asp:RadioButton ID="rdbFalse" Text="No" AutoPostBack="true" GroupName="ShowInTabulationRadioGroup" runat="server" OnCheckedChanged="rdbFalse_CheckedChanged" Checked="True" />
                            </div>
                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <b>Is Final Exam</b>
                                <br />
                                <asp:RadioButton ID="rdbIsFinalExamTrue" Text="Yes" AutoPostBack="true" GroupName="IsFinalExamRadioGroup" runat="server" />
                                <br />
                                <asp:RadioButton ID="rdbIsFinalExamFalse" Text="No" AutoPostBack="true" GroupName="IsFinalExamRadioGroup" runat="server" />
                            </div>
                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <b>Is Single Question</b>
                                <br />
                                <asp:RadioButton ID="rdbIsSingleQuestionTrue" Text="Yes" AutoPostBack="true" GroupName="IsSingleQuestionRadioGroup" runat="server" />
                                <br />
                                <asp:RadioButton ID="rdbIsSingleQuestionFalse" Text="No" AutoPostBack="true" GroupName="IsSingleQuestionRadioGroup" runat="server" />

                            </div>
                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <b style="font-size: 13px">Show All Continuous In Sub-Total</b>
                                <br />
                                <asp:RadioButton ID="rdbShowAllContinuousInSubTotalTrue" Text="Yes" AutoPostBack="true" GroupName="ShowAllContinuousInSubTotalRadioGroup" runat="server" />
                                <br />
                                <asp:RadioButton ID="rdbShowAllContinuousInSubTotalFalse" Text="No" AutoPostBack="true" GroupName="ShowAllContinuousInSubTotalRadioGroup" runat="server" />

                            </div>

                        </div>
                        <div class="row" style="margin-top: 15px">
                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <b>Show All In Grand Total</b>
                                <br />
                                <asp:RadioButton ID="rdbShowAllInGrandTotalTrue" Text="Yes" AutoPostBack="true" GroupName="ShowAllInGrandTotalRadioGroup" runat="server" />
                                <br />
                                <asp:RadioButton ID="rdbShowAllInGrandTotalFalse" Text="No" AutoPostBack="true" GroupName="ShowAllInGrandTotalRadioGroup" runat="server" />

                            </div>

                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <b>Multiple Examiner</b>
                                <asp:DropDownList ID="multipleExamDropDownList" AutoPostBack="True" runat="server" Width="100%" CssClass="form-control">
                                    <asp:ListItem Selected="True" Text="Select" Value="-1"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <b>No Of Examiner</b>
                                <asp:DropDownList ID="ddlNoOfExaminer" runat="server" Width="100%" CssClass="form-control">
                                    <asp:ListItem Selected="True" Text="-Select-" Value="-1"></asp:ListItem>
                                    <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                    <asp:ListItem Text="5" Value="5"></asp:ListItem>

                                </asp:DropDownList>
                            </div>
                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <asp:Panel ID="pnlTabulationShow" Visible="false" runat="server">
                                    <b>Tabulation Title</b>
                                    <asp:TextBox ID="txtTabulationTitle" runat="server" Width="100%" CssClass="form-control"></asp:TextBox>
                                </asp:Panel>
                            </div>
                        </div>
                        <div class="row" style="margin-top: 15px">
                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <asp:Panel ID="pnlRelationalColumn" Visible="false" runat="server">
                                    <div class="control-group">
                                        <b>Relational Exam</b>
                                        <div class="controls">
                                            <asp:TextBox ID="txtColumnNo1" runat="server" Width="30px"></asp:TextBox>
                                            <asp:TextBox ID="txtColumnNo2" runat="server" Width="30px"></asp:TextBox>
                                            <asp:TextBox ID="txtColumnNo3" runat="server" Width="30px"></asp:TextBox>
                                            <asp:TextBox ID="txtColumnNo4" runat="server" Width="30px"></asp:TextBox>
                                            <asp:TextBox ID="txtColumnNo5" runat="server" Width="30px"></asp:TextBox>
                                            <asp:TextBox ID="txtColumnNo6" runat="server" Width="30px"></asp:TextBox>
                                            <asp:TextBox ID="txtColumnNo7" runat="server" Width="30px"></asp:TextBox>
                                            <asp:TextBox ID="txtColumnNo8" runat="server" Width="30px"></asp:TextBox>
                                            <asp:TextBox ID="txtColumnNo9" runat="server" Width="30px"></asp:TextBox>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>

                            <div class="col-lg-2 col-md-2 col-sm-2">

                                <asp:Panel ID="pnlColumnType" Visible="false" runat="server">
                                    <div class="control-group">
                                        <b>Calculation Type</b>

                                        <div class="checkbox inline">
                                            <asp:RadioButton ID="rdbPercentage" AutoPostBack="true" GroupName="CalculationTypeRadioGroup" runat="server" Checked="True" OnCheckedChanged="rdbpercentage_CheckedChanged" />
                                            <asp:Label ID="lblpercentage" runat="server" Text="Percentage"></asp:Label>
                                        </div>

                                        <div class="checkbox inline">
                                            <asp:RadioButton ID="rdbSum" AutoPostBack="true" GroupName="CalculationTypeRadioGroup" runat="server" OnCheckedChanged="rdbSum_CheckedChanged" />
                                            <asp:Label ID="lblSum" runat="server" Text="Sum"></asp:Label>
                                        </div>

                                        <div class="checkbox inline">
                                            <asp:RadioButton ID="rdbBestOne" AutoPostBack="true" GroupName="CalculationTypeRadioGroup" runat="server" OnCheckedChanged="rdbBestOne_CheckedChanged" />
                                            <asp:Label ID="lblBestOne" runat="server" Text="Best One"></asp:Label>
                                        </div>

                                        <div class="checkbox inline">
                                            <asp:RadioButton ID="rdbBestTwo" AutoPostBack="true" GroupName="CalculationTypeRadioGroup" runat="server" OnCheckedChanged="rdbBestTwo_CheckedChanged" />
                                            <asp:Label ID="lblBestTwo" runat="server" Text="Best Two"></asp:Label>
                                        </div>

                                        <div class="checkbox inline">
                                            <asp:RadioButton ID="rdbBestThree" AutoPostBack="true" GroupName="CalculationTypeRadioGroup" runat="server" OnCheckedChanged="rdbBestThree_CheckedChanged" />
                                            <asp:Label ID="lblBestThree" runat="server" Text="Best Three"></asp:Label>
                                        </div>

                                        <div class="checkbox inline">
                                            <asp:RadioButton ID="rdbAverage" AutoPostBack="true" GroupName="CalculationTypeRadioGroup" runat="server" OnCheckedChanged="rdbAverage_CheckedChanged" />
                                            <asp:Label ID="lblAverage" runat="server" Text="Average"></asp:Label>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>

                            <div class="col-lg-4 col-md-4 col-sm-4">
                                <asp:Panel ID="pnlPercentage" Visible="false" runat="server">
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12">
                                            <b>Multiply</b>
                                            <asp:TextBox ID="txtMultiplyBy" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-12 col-md-12 col-sm-12">
                                            <b>Divide</b>
                                            <asp:TextBox ID="txtDivideBy" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                        </div>
                                    </div>
                                </asp:Panel>

                            </div>
                        </div>
                        <div class="row" style="margin-top: 20px">
                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <asp:Button ID="AddButton" runat="server" Text="Add" OnClick="AddButton_Click" CssClass="btn-info" Width="100%" />
                            </div>
                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <asp:Button ID="UpdateButton" Visible="false" runat="server" Text="Update" OnClick="UpdateButton_Click" CssClass="btn-info" Width="100%" />

                            </div>
                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <asp:Button ID="CancelButton" Visible="false" runat="server" Text="Cancel Edit" OnClick="CancelButton_Click" CssClass="btn-danger" Width="100%" />
                            </div>
                        </div>
                    </div>

                    <div style="padding: 5px">
                        <asp:GridView ID="GvExamTemplateItem" runat="server" AllowSorting="True" CssClass="table-bordered"
                            AutoGenerateColumns="False" ShowFooter="True" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None"
                            OnRowCommand="GvExamTemplateItem_RowCommand">
                            <HeaderStyle BackColor="#4285f4" ForeColor="White" Height="10px" Font-Bold="True" />
                            <FooterStyle BackColor="#4285f4" ForeColor="White" Height="10px" Font-Bold="True" />
                            <AlternatingRowStyle BackColor="White" />
                            <RowStyle Height="25" />

                            <Columns>
                                <asp:BoundField DataField="ExamTemplateItemId" Visible="false" HeaderText="Id">
                                    <ItemStyle HorizontalAlign="Center" />
                                    <HeaderStyle Width="150px" />
                                </asp:BoundField>

                                <asp:TemplateField HeaderText="Template Name">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblExamTemplateName" Text='<%#Eval("ExamTemplate.ExamTemplateName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Width="200px" />
                                </asp:TemplateField>

                                <asp:BoundField DataField="ExamName" HeaderText="Exam Name">
                                    <ItemStyle HorizontalAlign="Center" />
                                    <HeaderStyle Width="150px" />
                                </asp:BoundField>

                                <asp:BoundField DataField="ExamMark" HeaderText="Exam Mark">
                                    <ItemStyle HorizontalAlign="Center" />
                                    <HeaderStyle Width="150px" />
                                </asp:BoundField>

                                <%--      <asp:BoundField DataField="PassMark" HeaderText="Pass Mark">
                                <ItemStyle HorizontalAlign="Center" />
                                <HeaderStyle Width="150px" />
                            </asp:BoundField>--%>

                                <asp:BoundField DataField="ColumnTypeName" HeaderText="Column Type Name">
                                    <ItemStyle HorizontalAlign="Center" />
                                    <HeaderStyle Width="150px" />
                                </asp:BoundField>

                                <asp:BoundField DataField="ColumnSequence" HeaderText="Sequence">
                                    <ItemStyle HorizontalAlign="Center" />
                                    <HeaderStyle Width="100px" />
                                </asp:BoundField>

                                <%-- <asp:BoundField DataField="PrintColumnSequence" HeaderText="Print Sequence">
                                <ItemStyle HorizontalAlign="Center" />
                                <HeaderStyle Width="100px" />
                            </asp:BoundField>--%>


                                <asp:TemplateField HeaderText="Continuous Assessment">
                                    <ItemTemplate>
                                        <div style="text-align: center">
                                            <asp:Label ID="lblContinuous" runat="server" Font-Bold="true"
                                                Text='<%# Eval("Attribute1")%>'></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle Width="80px"></HeaderStyle>
                                    <ItemStyle CssClass="center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Viva">
                                    <ItemTemplate>
                                        <div style="text-align: center">
                                            <asp:Label ID="lblViva" runat="server" Font-Bold="true"
                                                Text='<%# Eval("Attribute2")%>'></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle Width="40px"></HeaderStyle>
                                    <ItemStyle CssClass="center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Is Final Exam">
                                    <ItemTemplate>
                                        <div style="text-align: center">
                                            <asp:Label ID="lblIsFinal" runat="server" Font-Bold="true"
                                                Text='<%# Convert.ToBoolean(Eval("IsFinalExam"))==false ? "No" : "Yes" %>'
                                                ForeColor='<%# Convert.ToBoolean(Eval("IsFinalExam"))==false ? System.Drawing.Color.Black : System.Drawing.Color.Blue %>'></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle Width="80px"></HeaderStyle>
                                    <ItemStyle CssClass="center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Is Single Question">
                                    <ItemTemplate>
                                        <div style="text-align: center">
                                            <asp:Label ID="lblIsSingle" runat="server" Font-Bold="true"
                                                Text='<%# Convert.ToBoolean(Eval("SingleQuestionAnswer"))==false ? "No" : "Yes" %>'
                                                ForeColor='<%# Convert.ToBoolean(Eval("SingleQuestionAnswer"))==false ? System.Drawing.Color.Black : System.Drawing.Color.Blue %>'></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle Width="80px"></HeaderStyle>
                                    <ItemStyle CssClass="center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="In Sub-Total">
                                    <ItemTemplate>
                                        <div style="text-align: center">
                                            <asp:Label ID="lblInSubTotal" runat="server" Font-Bold="true"
                                                Text='<%# Convert.ToBoolean(Eval("ShowAllContinuousInSubTotal"))==false ? "No" : "Yes" %>'
                                                ForeColor='<%# Convert.ToBoolean(Eval("ShowAllContinuousInSubTotal"))==false ? System.Drawing.Color.Black : System.Drawing.Color.Blue %>'></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle Width="80px"></HeaderStyle>
                                    <ItemStyle CssClass="center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="In Grand Total">
                                    <ItemTemplate>
                                        <div style="text-align: center">
                                            <asp:Label ID="lblInGrandTotal" runat="server" Font-Bold="true"
                                                Text='<%# Convert.ToBoolean(Eval("ShowAllInGrandTotal"))==false ? "No" : "Yes" %>'
                                                ForeColor='<%# Convert.ToBoolean(Eval("ShowAllInGrandTotal"))==false ? System.Drawing.Color.Black : System.Drawing.Color.Blue %>'></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle Width="80px"></HeaderStyle>
                                    <ItemStyle CssClass="center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Multiple Examiner">
                                    <ItemTemplate>
                                        <div style="text-align: center">
                                            <asp:Label ID="lblMultipleEx" runat="server" Font-Bold="true"
                                                Text='<%# Convert.ToString(Eval("MultipleExaminer"))=="0" || Convert.ToString(Eval("MultipleExaminer"))=="No_Examiner" ? "No" : "Yes" %>'
                                                ForeColor='<%# Convert.ToString(Eval("MultipleExaminer"))=="0" || Convert.ToString(Eval("MultipleExaminer"))=="No_Examiner" ? System.Drawing.Color.Black : System.Drawing.Color.Blue %>'></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle Width="80px"></HeaderStyle>
                                    <ItemStyle CssClass="center" />
                                </asp:TemplateField>

                                <asp:BoundField DataField="NumberOfExaminer" HeaderText="No of Examiner">
                                    <ItemStyle HorizontalAlign="Center" />
                                    <HeaderStyle Width="100px" />
                                </asp:BoundField>

                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <div style="text-align: center">
                                            <asp:LinkButton ID="EditButton" CommandName="ExamTemplateItemEdit" Text="Edit" ToolTip="Edit Exam Template Item" CommandArgument='<%# Bind("ExamTemplateItemId") %>' runat="server"></asp:LinkButton>
                                            <asp:LinkButton ID="DeleteButton" CommandName="ExamTemplateItemDelete" Text="Delete" ForeColor="Red" OnClientClick="return confirm('Do you really want to delete this exam template item?');" ToolTip="Delete Exam Template Item" CommandArgument='<%# Bind("ExamTemplateItemId") %>' runat="server"></asp:LinkButton>
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle Width="80px"></HeaderStyle>
                                    <ItemStyle CssClass="center" />
                                </asp:TemplateField>
                            </Columns>

                            <PagerStyle BackColor="#4285f4" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle Height="10px" VerticalAlign="Middle" HorizontalAlign="Left" BackColor="#E3EAEB" />
                            <EditRowStyle BackColor="#7C6F57" />
                            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                            <SortedAscendingCellStyle BackColor="#F8FAFA" />
                            <SortedAscendingHeaderStyle BackColor="#246B61" />
                            <SortedDescendingCellStyle BackColor="#D4DFE1" />
                            <SortedDescendingHeaderStyle BackColor="#15524A" />

                        </asp:GridView>
                    </div>
                </div>
                <asp:HiddenField ID="hdnExamTemplateItemId" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
