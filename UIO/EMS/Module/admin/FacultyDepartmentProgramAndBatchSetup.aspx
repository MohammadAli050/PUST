<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true" CodeBehind="FacultyDepartmentProgramAndBatchSetup.aspx.cs" Inherits="EMS.Module.admin.FacultyDepartmentProgramAndBatchSetup" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Faculty, Department, Program Setup

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">

    <style>
        .blink {
            animation: blinker 0.6s linear infinite;
            color: #1c87c9;
            font-size: 30px;
            font-weight: bold;
            font-family: sans-serif;
        }

        @keyframes blinker {
            50% {
                opacity: 0;
            }
        }

        #ctl00_MainContainer_ddlFaculty, #ctl00_MainContainer_ddlDepartment, #ctl00_MainContainer_ddlProgramFaculty, #ctl00_MainContainer_ddlProgramDepartment, #ctl00_MainContainer_ddlProgram {
            height: 38px !important;
        }
    </style>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContainer" runat="server">


    <div>
        <div class="col-sm-12" style="font-size: 12pt; margin-top: 10pt;">
            <label><b style="color: black; font-size: 26px">Faculty, Department, Program Setup</b></label>
        </div>
    </div>
    <div id="divProgress" style="display: none; z-index: 100000000; position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%);">
        <asp:Image ID="LoadingImage" runat="server" ImageUrl="~/Images/Img/Waiting.gif" Height="300px" Width="300px" />
        <div>
            <asp:Label ID="Label1" runat="server" Text="Processing your request.........." ForeColor="Red" Font-Bold="true" Font-Italic="true" Font-Size="30px"></asp:Label>
        </div>
    </div>

    <hr />



    <asp:UpdatePanel runat="server" ID="UpdatePanel02">
        <ContentTemplate>

            <div class="card">
                <div class="card-body">

                    <div class="row">
                        <div class="col-lg-2 col-md-2 col-sm-2">

                            <div class="card">
                                <div class="card-body">
                                    <asp:LinkButton ID="lnkFaculty" runat="server" OnClick="lnkFaculty_Click">
                                                <div class="panel-heading" style="background-color: #4CA1D7;">
                                                    <h4 class="panel-title" style="text-align: center">
                                                        <span style="font-size: 20px; color:white">Faculty </span>
                                                    </h4>
                                                </div>
                                    </asp:LinkButton>
                                    <br />
                                    <asp:LinkButton ID="lnkDepartment" runat="server" OnClick="lnkDepartment_Click">
                                                <div class="panel-heading" style="background-color: #4CA1D7">
                                                    <h4 class="panel-title" style="text-align: center">
                                                        <span style="font-size: 20px; color:white">Department </span>
                                                    </h4>
                                                </div>
                                    </asp:LinkButton>
                                    <br />
                                    <asp:LinkButton ID="lnkProgram" runat="server" OnClick="lnkProgram_Click">
                                                <div class="panel-heading" style="background-color: #4CA1D7">
                                                    <h4 class="panel-title" style="text-align: center">
                                                        <span style="font-size: 20px; color:white">Program </span>
                                                    </h4>
                                                </div>
                                    </asp:LinkButton>
                                    <br />
                                    <asp:LinkButton ID="lnkBatch" runat="server" OnClick="lnkBatch_Click" Visible="false">
                                                <div class="panel-heading" style="background-color: #4CA1D7">
                                                    <h4 class="panel-title" style="text-align: center">
                                                        <span style="font-size: 20px; color:white">Batch </span>
                                                    </h4>
                                                </div>
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>

                        <div class="col-lg-10 col-md-10 col-sm-10">

                            <div class="card">
                                <div class="card-body">
                                    <%--------------------------- Faculty Setup Information -----------------------------%>

                                    <div runat="server" id="DivFaculty">


                                        <div class="card">
                                            <div class="card-body">
                                                <div class="row">
                                                    <div class="col-lg-12 col-md-12 col-sm-12" style="position: static">
                                                        <div class="row">
                                                            <div class="col-lg-4 col-md-4 col-sm-4">
                                                                <b>Select Faculty</b>
                                                                <asp:DropDownList ID="ddlFaculty" runat="server" CssClass="form-control" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddlFaculty_SelectedIndexChanged"></asp:DropDownList>
                                                            </div>
                                                            <div class="col-lg-2 col-md-2 col-sm-2">
                                                                <br />
                                                                <asp:LinkButton ID="lnkAddNewFaculty" runat="server" Width="100%" type="button" OnClick="lnkAddNewFaculty_Click" CssClass="btn btn-info btn-lg" Style="text-align: center" Font-Size="Small">
                                                                    <b>Add New</b>
                                                                </asp:LinkButton>
                                                            </div>

                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="card" runat="server" id="DivFacultyAdd" style="margin-top: 10px">
                                            <div class="card-body">

                                                <asp:HiddenField ID="hdnFacultySetupId" runat="server" />

                                                <div class="row">
                                                    <div class="col-lg-4 col-md-4 col-sm-4">
                                                        <b>Faculty Name <span style="color: red">*</span> </b>
                                                        <asp:RequiredFieldValidator ID="CompareValidator4" runat="server"
                                                            ControlToValidate="txtFacultyName" ErrorMessage="Required" Font-Size="10pt" Font-Bold="true"
                                                            ForeColor="Red" Display="Dynamic" CssClass="blink"
                                                            ValidationGroup="VGF"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="txtFacultyName" CssClass="form-control" Width="100%" runat="server" placeholder="Faculty of Computer Science & Engineering"></asp:TextBox>
                                                    </div>
                                                    <div class="col-lg-2 col-md-2 col-sm-2">
                                                        <b>Faculty Code<span style="color: red">*</span></b>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                            ControlToValidate="txtFacultyCode" ErrorMessage="Required" Font-Size="10pt" Font-Bold="true"
                                                            ForeColor="Red" Display="Dynamic" CssClass="blink"
                                                            ValidationGroup="VGF"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="txtFacultyCode" CssClass="form-control" Width="100%" runat="server" placeholder="FSE"></asp:TextBox>
                                                    </div>

                                                    <div class="col-lg-2 col-md-2 col-sm-2">
                                                        <br />
                                                        <asp:LinkButton ID="lnkFacultySave" runat="server" ValidationGroup="VGF" Width="100%" type="button" OnClick="lnkFacultySave_Click" CssClass="btn-info btn-lg" Style="text-align: center" Font-Size="Small">
                                                                    <b>Save</b>
                                                        </asp:LinkButton>
                                                    </div>

                                                    <div class="col-lg-2 col-md-2 col-sm-2">
                                                        <br />
                                                        <asp:LinkButton ID="lnkFacultyCancel" runat="server" Width="100%" type="button" OnClick="lnkFacultyCancel_Click" CssClass="btn-danger btn-lg" Style="text-align: center" Font-Size="Small">
                                                                    <b>Cancel</b>
                                                        </asp:LinkButton>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>

                                        <div class="card" runat="server" id="DivFacultyGrid" style="margin-top: 10px">
                                            <div class="card-body">
                                                <asp:GridView runat="server" ID="gvFacultyList" AllowSorting="True" CssClass="table table-bordered"
                                                    AutoGenerateColumns="False" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                                                    <HeaderStyle BackColor="#4CA1D7" ForeColor="White" Height="10px" Font-Bold="True" />
                                                    <FooterStyle BackColor="#4CA1D7" ForeColor="White" Height="10px" Font-Bold="True" />
                                                    <AlternatingRowStyle BackColor="White" />
                                                    <RowStyle Height="10px" />

                                                    <Columns>

                                                        <asp:TemplateField HeaderText="SL#">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblSL" Text='<%# Container.DataItemIndex + 1 %>' ForeColor="Black" Font-Bold="true"></asp:Label>

                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Name">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblName" Text='<%#Eval("FacultyName") %>' ForeColor="Black"></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Code">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblCode" Text='<%#Eval("FacultyCode") %>' ForeColor="Black"></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <div style="padding: 5px; text-align: center">
                                                                    <asp:LinkButton ID="editFaculty" ToolTip="Edit Faculty" CssClass="btn-primary btn-sm" CommandArgument='<%#Eval("Id")%>' runat="server" OnClick="editFaculty_Click">
                                                                             <strong><i class="fas fa-pencil-alt"></i></strong>
                                                                    </asp:LinkButton>
                                                                    &nbsp; &nbsp; &nbsp;
                                                    <asp:LinkButton ID="RemoveFaculty" ToolTip="Remove Faculty" CommandArgument='<%#Eval("Id")%>' runat="server" OnClientClick="return confirm('Are you sure you want to Remove this Faculty ?');" CssClass="btn-danger btn-sm" OnClick="RemoveFaculty_Click">                         
                                                                                <strong><i class="fas fa-trash"></i></strong>
                                                    </asp:LinkButton>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>

                                    </div>

                                    <%--------------------------- Faculty Setup Information -----------------------------%>


                                    <%--------------------------- Department Setup Information -----------------------------%>

                                    <div runat="server" id="DivDepartment">

                                        <div class="card" style="margin-top: 10px">
                                            <div class="card-body">
                                                <div class="row">
                                                    <div class="col-lg-12 col-md-12 col-sm-12" style="position: static">

                                                        <div class="row">
                                                            <div class="col-lg-4 col-md-4 col-sm-4">
                                                                <b>Select Department</b>
                                                                <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="form-control" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged"></asp:DropDownList>
                                                            </div>
                                                            <div class="col-lg-2 col-md-2 col-sm-2">
                                                                <br />
                                                                <asp:LinkButton ID="lnkAddNewDepartment" runat="server" Width="100%" type="button" OnClick="lnkAddNewDepartment_Click" CssClass="btn-info btn-lg" Style="text-align: center" Font-Size="Small">
                                                                    <b>Add New</b>
                                                                </asp:LinkButton>
                                                            </div>

                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="card" runat="server" id="DivDeptAdd" style="margin-top: 10px">
                                            <div class="card-body">

                                                <asp:HiddenField ID="hdnDepartmentSetupId" runat="server" />

                                                <div class="row">
                                                    <div class="col-lg-3 col-md-3 col-sm-3">
                                                        <b>Department Name <span style="color: red">*</span> </b>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                                            ControlToValidate="txtDeptName" ErrorMessage="Required" Font-Size="10pt" Font-Bold="true"
                                                            ForeColor="Red" Display="Dynamic" CssClass="blink"
                                                            ValidationGroup="VGD"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="txtDeptName" CssClass="form-control" Width="100%" runat="server" placeholder="Computer Science & Engineering"></asp:TextBox>
                                                    </div>
                                                    <div class="col-lg-2 col-md-2 col-sm-2">
                                                        <b>Department Code<span style="color: red">*</span></b>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                                            ControlToValidate="txtDeptCode" ErrorMessage="Required" Font-Size="10pt" Font-Bold="true"
                                                            ForeColor="Red" Display="Dynamic" CssClass="blink"
                                                            ValidationGroup="VGD"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="txtDeptCode" CssClass="form-control" Width="100%" runat="server" placeholder="CSE"></asp:TextBox>
                                                    </div>

                                                    <div class="col-lg-3 col-md-3 col-sm-3">
                                                        <b>Detail Name <span style="color: red">*</span> </b>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                                                            ControlToValidate="txtDeptDetailName" ErrorMessage="Required" Font-Size="10pt" Font-Bold="true"
                                                            ForeColor="Red" Display="Dynamic" CssClass="blink"
                                                            ValidationGroup="VGD"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="txtDeptDetailName" CssClass="form-control" Width="100%" runat="server" placeholder="Department of Computer Science & Engineering"></asp:TextBox>
                                                    </div>

                                                    <div class="col-lg-2 col-md-2 col-sm-2">
                                                        <b>Opening Date</b>
                                                        <asp:TextBox runat="server" ID="txtDeptOpeningDate" Width="100%" Height="40px" CssClass="form-control" placeholder="dd/MM/yyyy" DataFormatString="{0:dd/MM/yyyy}" />
                                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDeptOpeningDate" Format="dd/MM/yyyy" />

                                                    </div>
                                                    <div class="col-lg-2 col-md-2 col-sm-2">
                                                        <b>Closing Date</b>
                                                        <asp:TextBox runat="server" ID="txtDeptClosingDate" Width="100%" Height="40px" CssClass="form-control" placeholder="dd/MM/yyyy" DataFormatString="{0:dd/MM/yyyy}" />
                                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtDeptClosingDate" Format="dd/MM/yyyy" />

                                                    </div>

                                                </div>

                                                <div class="row">
                                                    <div class="col-lg-2 col-md-2 col-sm-2">
                                                        <br />
                                                        <asp:LinkButton ID="lnkDeptSave" runat="server" ValidationGroup="VGD" Width="100%" type="button" OnClick="lnkDeptSave_Click" CssClass="btn-info btn-lg" Style="text-align: center" Font-Size="Small">
                                                                    <b>Save</b>
                                                        </asp:LinkButton>
                                                    </div>

                                                    <div class="col-lg-2 col-md-2 col-sm-2">
                                                        <br />
                                                        <asp:LinkButton ID="lnkDeptCancel" runat="server" Width="100%" type="button" OnClick="lnkDeptCancel_Click" CssClass="btn-danger btn-lg" Style="text-align: center" Font-Size="Small">
                                                                    <b>Cancel</b>
                                                        </asp:LinkButton>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>

                                        <div class="card" runat="server" id="DivDeptGrid" style="margin-top: 10px">
                                            <div class="card-body">
                                                <asp:GridView runat="server" ID="gvDeptList" AllowSorting="True" CssClass="table table-bordered"
                                                    AutoGenerateColumns="False" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                                                    <HeaderStyle BackColor="#4CA1D7" ForeColor="White" Height="10px" Font-Bold="True" />
                                                    <FooterStyle BackColor="#4CA1D7" ForeColor="White" Height="10px" Font-Bold="True" />
                                                    <AlternatingRowStyle BackColor="White" />
                                                    <RowStyle Height="10px" />

                                                    <Columns>

                                                        <asp:TemplateField HeaderText="SL#">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblSL" Text='<%# Container.DataItemIndex + 1 %>' ForeColor="Black" Font-Bold="true"></asp:Label>

                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Dept. Name">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblDeptName" Text='<%#Eval("Name") %>' ForeColor="Black"></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Code">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblDeptCode" Text='<%#Eval("Code") %>' ForeColor="Black"></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Detail Name">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblDeptDetailName" Text='<%#Eval("DetailedName") %>' ForeColor="Black"></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Opening Date">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblDeptOpenDate" Font-Bold="False" Text='<%#Eval("OpeningDate", "{0:dd/MM/yyyy}") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Closing Date">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblDeptCloseDate" Font-Bold="False" Text='<%#Eval("ClosingDate", "{0:dd/MM/yyyy}") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <div style="padding: 5px; text-align: center">
                                                                    <asp:LinkButton ID="EditDept" ToolTip="Edit Department" CssClass="btn-primary btn-sm" CommandArgument='<%#Eval("DeptID")%>' runat="server" OnClick="EditDepartment_Click">
                                                                             <strong><i class="fas fa-pencil-alt"></i></strong>
                                                                    </asp:LinkButton>
                                                                    &nbsp;&nbsp;&nbsp;
                                                     <asp:LinkButton ID="RemoveDept" ToolTip="Remove Department" CommandArgument='<%#Eval("DeptID")%>' runat="server" OnClientClick="return confirm('Are you sure you want to Remove this Department ?');" CssClass="btn-danger btn-sm" OnClick="RemoveDepartment_Click">                         
                                                                                <strong><i class="fas fa-trash"></i></strong>
                                                     </asp:LinkButton>
                                                                </div>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="12%" />
                                                        </asp:TemplateField>

                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>

                                    </div>

                                    <%--------------------------- Department Setup Information -----------------------------%>


                                    <%--------------------------- Program Setup Information -----------------------------%>

                                    <div runat="server" id="DivProgram">

                                        <div class="card" style="margin-top: 10px">
                                            <div class="card-body">
                                                <div class="row">
                                                    <div class="col-lg-12 col-md-12 col-sm-12" style="position: static">

                                                        <div class="row">
                                                            <div class="col-lg-3 col-md-3 col-sm-3">
                                                                <b>Select Faculty</b>
                                                                <asp:DropDownList ID="ddlProgramFaculty" runat="server" CssClass="form-control" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddlProgramFaculty_SelectedIndexChanged"></asp:DropDownList>
                                                            </div>

                                                            <div class="col-lg-3 col-md-3 col-sm-3">
                                                                <b>Select Department</b>
                                                                <asp:DropDownList ID="ddlProgramDepartment" runat="server" CssClass="form-control" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddlProgramDepartment_SelectedIndexChanged"></asp:DropDownList>
                                                            </div>

                                                            <div class="col-lg-3 col-md-3 col-sm-3">
                                                                <b>Select Program</b>
                                                                <asp:DropDownList ID="ddlProgram" runat="server" CssClass="form-control" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddlProgram_SelectedIndexChanged"></asp:DropDownList>
                                                            </div>
                                                            <div class="col-lg-2 col-md-2 col-sm-2">
                                                                <br />
                                                                <asp:LinkButton ID="lnkAddNewProgram" runat="server" Width="100%" type="button" OnClick="lnkAddNewProgram_Click" CssClass="btn-info btn-lg" Style="text-align: center" Font-Size="Small">
                                                                    <b>Add New</b>
                                                                </asp:LinkButton>
                                                            </div>

                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="card" runat="server" id="DivProgramAdd" style="margin-top: 10px">
                                            <div class="card-body">

                                                <asp:HiddenField ID="hdnProgramId" runat="server" />

                                                <div class="row">

                                                    <div class="col-lg-3 col-md-3 col-sm-3">
                                                        <b>Select Faculty<span style="color: red">*</span></b>
                                                        <asp:CompareValidator ID="CompareValidator1" runat="server"
                                                            ControlToValidate="ddlAddProgramFaculty" ErrorMessage="Required" Font-Size="10pt" Font-Bold="true"
                                                            ForeColor="Red" Display="Dynamic" ValueToCompare="0" Operator="NotEqual" CssClass="blink"
                                                            ValidationGroup="VGP"></asp:CompareValidator>
                                                        <asp:DropDownList ID="ddlAddProgramFaculty" runat="server" CssClass="form-control" Width="100%"></asp:DropDownList>
                                                    </div>

                                                    <div class="col-lg-3 col-md-3 col-sm-3">
                                                        <b>Select Department<span style="color: red">*</span></b>
                                                        <asp:CompareValidator ID="CompareValidator2" runat="server"
                                                            ControlToValidate="ddlAddProgramDepartment" ErrorMessage="Required" Font-Size="10pt" Font-Bold="true"
                                                            ForeColor="Red" Display="Dynamic" ValueToCompare="0" Operator="NotEqual" CssClass="blink"
                                                            ValidationGroup="VGP"></asp:CompareValidator>
                                                        <asp:DropDownList ID="ddlAddProgramDepartment" runat="server" CssClass="form-control" Width="100%"></asp:DropDownList>
                                                    </div>
                                                    <div class="col-lg-3 col-md-3 col-sm-3">
                                                        <b>Select Program Type<span style="color: red">*</span></b>
                                                        <asp:CompareValidator ID="CompareValidator3" runat="server"
                                                            ControlToValidate="ddlProgramType" ErrorMessage="Required" Font-Size="10pt" Font-Bold="true"
                                                            ForeColor="Red" Display="Dynamic" ValueToCompare="0" Operator="NotEqual" CssClass="blink"
                                                            ValidationGroup="VGP"></asp:CompareValidator>
                                                        <asp:DropDownList ID="ddlProgramType" runat="server" CssClass="form-control" Width="100%"></asp:DropDownList>
                                                    </div>

                                                    <div class="col-lg-3 col-md-3 col-sm-3">
                                                        <b>Select Calender Type<span style="color: red">*</span></b>
                                                        <asp:CompareValidator ID="CompareValidator5" runat="server"
                                                            ControlToValidate="ddlCalenderType" ErrorMessage="Required" Font-Size="10pt" Font-Bold="true"
                                                            ForeColor="Red" Display="Dynamic" ValueToCompare="0" Operator="NotEqual" CssClass="blink"
                                                            ValidationGroup="VGP"></asp:CompareValidator>
                                                        <asp:DropDownList ID="ddlCalenderType" runat="server" CssClass="form-control" Width="100%"></asp:DropDownList>
                                                    </div>

                                                </div>
                                                <div class="row" style="margin-top: 5px">
                                                    <div class="col-lg-3 col-md-3 col-sm-3">
                                                        <b>Degree Name <span style="color: red">*</span> </b>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"
                                                            ControlToValidate="txtProgramName" ErrorMessage="Required" Font-Size="10pt" Font-Bold="true"
                                                            ForeColor="Red" Display="Dynamic" CssClass="blink"
                                                            ValidationGroup="VGP"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="txtProgramName" CssClass="form-control" Width="100%" runat="server" placeholder="Bachelor of Computer Science and Engineering"></asp:TextBox>
                                                    </div>
                                                    <div class="col-lg-3 col-md-3 col-sm-3">
                                                        <b>Detail Name<span style="color: red">*</span></b>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server"
                                                            ControlToValidate="txtProgramDetailName" ErrorMessage="Required" Font-Size="10pt" Font-Bold="true"
                                                            ForeColor="Red" Display="Dynamic" CssClass="blink"
                                                            ValidationGroup="VGP"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="txtProgramDetailName" CssClass="form-control" Width="100%" runat="server" placeholder="Bachelor of Computer Science and Engineering"></asp:TextBox>
                                                    </div>

                                                    <div class="col-lg-3 col-md-3 col-sm-3">
                                                        <b>Program Code <span style="color: red">*</span> </b>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server"
                                                            ControlToValidate="txtProgramCode" ErrorMessage="Required" Font-Size="10pt" Font-Bold="true"
                                                            ForeColor="Red" Display="Dynamic" CssClass="blink"
                                                            ValidationGroup="VGP"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="txtProgramCode" CssClass="form-control" Width="100%" runat="server" placeholder="000"></asp:TextBox>
                                                    </div>

                                                    <div class="col-lg-3 col-md-3 col-sm-3">
                                                        <b>Short Name <span style="color: red">*</span> </b>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server"
                                                            ControlToValidate="txtProgramShortName" ErrorMessage="Required" Font-Size="10pt" Font-Bold="true"
                                                            ForeColor="Red" Display="Dynamic" CssClass="blink"
                                                            ValidationGroup="VGP"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="txtProgramShortName" CssClass="form-control" Width="100%" runat="server" placeholder="CSE"></asp:TextBox>
                                                    </div>


                                                </div>

                                                <div class="row" style="margin-top: 5px">
                                                    <div class="col-lg-3 col-md-3 col-sm-3">
                                                        <b>Total Credit<span style="color: red">*</span> </b>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server"
                                                            ControlToValidate="txtTotalCredit" ErrorMessage="Required" Font-Size="10pt" Font-Bold="true"
                                                            ForeColor="Red" Display="Dynamic" CssClass="blink"
                                                            ValidationGroup="VGP"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="txtTotalCredit" CssClass="form-control" Width="100%" TextMode="Number" step=".1" runat="server" placeholder="150"></asp:TextBox>
                                                    </div>
                                                    <div class="col-lg-3 col-md-3 col-sm-3">
                                                        <b>Duration(In Semester)<span style="color: red">*</span> </b>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server"
                                                            ControlToValidate="txtDuration" ErrorMessage="Required" Font-Size="10pt" Font-Bold="true"
                                                            ForeColor="Red" Display="Dynamic" CssClass="blink"
                                                            ValidationGroup="VGP"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="txtDuration" CssClass="form-control" Width="100%" runat="server" TextMode="Number" step="1" placeholder="12"></asp:TextBox>
                                                    </div>

                                                    <div class="col-lg-3 col-md-3 col-sm-3">
                                                        <br />
                                                        <asp:LinkButton ID="lnkProgramSave" runat="server" ValidationGroup="VGP" Width="100%" type="button" OnClick="lnkProgramSave_Click" CssClass="btn-info btn-lg" Style="text-align: center" Font-Size="Small">
                                                                    <b>Save</b>
                                                        </asp:LinkButton>
                                                    </div>
                                                    <div class="col-lg-3 col-md-3 col-sm-3">
                                                        <br />
                                                        <asp:LinkButton ID="lnkProgramCancel" runat="server" Width="100%" type="button" OnClick="lnkProgramCancel_Click" CssClass="btn-danger btn-lg" Style="text-align: center" Font-Size="Small">
                                                                    <b>Cancel</b>
                                                        </asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="card" runat="server" id="DivProgramGrid" style="margin-top: 10px">
                                            <div class="card-body">
                                                <asp:GridView runat="server" ID="gvProgramList" AllowSorting="True" CssClass="table table-bordered"
                                                    AutoGenerateColumns="False" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                                                    <HeaderStyle BackColor="#4CA1D7" ForeColor="White" Height="10px" Font-Bold="True" />
                                                    <FooterStyle BackColor="#4CA1D7" ForeColor="White" Height="10px" Font-Bold="True" />
                                                    <AlternatingRowStyle BackColor="White" />
                                                    <RowStyle Height="10px" />

                                                    <Columns>

                                                        <asp:TemplateField HeaderText="SL#">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblSL" Text='<%# Container.DataItemIndex + 1 %>' ForeColor="Black" Font-Bold="true"></asp:Label>

                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Faculty">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblProgramFaculty" Text='<%#Eval("Attribute1") %>' ForeColor="Black"></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Department">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblProgramDepartment" Text='<%#Eval("Attribute2") %>' ForeColor="Black"></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Code">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblProgramCode" Text='<%#Eval("Code") %>' ForeColor="Black"></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle />
                                                        </asp:TemplateField>


                                                        <asp:TemplateField HeaderText="Short Name">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblShortName" Text='<%#Eval("ShortName") %>' ForeColor="Black"></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Degree Name">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblDegreeName" Text='<%#Eval("DegreeName") %>' ForeColor="Black"></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Detail Name">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblDetailName" Text='<%#Eval("DetailName") %>' ForeColor="Black"></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Total Credit">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblCredit" Text='<%#Eval("TotalCredit") %>' ForeColor="Black"></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Duration">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblDuration" Text='<%#Eval("Duration") +" Semester" %>' ForeColor="Black"></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle />
                                                        </asp:TemplateField>


                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <div style="padding: 5px; text-align: center">
                                                                    <asp:LinkButton ID="EditProgram" ToolTip="Edit Program" CssClass="btn-primary btn-sm" CommandArgument='<%#Eval("ProgramID")%>' runat="server" OnClick="EditProgram_Click">
                                                                             <strong><i class="fas fa-pencil-alt"></i></strong>
                                                                    </asp:LinkButton>
                                                                    &nbsp;&nbsp;&nbsp;
                                                    <asp:LinkButton ID="RemoveProgram" ToolTip="Remove Program" CommandArgument='<%#Eval("ProgramID")%>' runat="server" OnClientClick="return confirm('Are you sure you want to Remove this Program ?');" CssClass="btn-danger btn-sm" OnClick="RemoveProgram_Click">                         
                                                                                <strong><i class="fas fa-trash"></i></strong>
                                                    </asp:LinkButton>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>


                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>

                                    </div>

                                    <%--------------------------- Program Setup Information -----------------------------%>


                                    <%--------------------------- Batch Setup Information -----------------------------%>

                                    <div runat="server" id="DivBatch">

                                        <div class="card" style="margin-top: 10px">
                                            <div class="card-body">
                                                <div class="row">
                                                    <div class="col-lg-12 col-md-12 col-sm-12" style="position: static">

                                                        <div class="row">
                                                            <div class="col-lg-3 col-md-3 col-sm-3">
                                                                <b>Select Session</b>
                                                                <asp:DropDownList ID="ddlSession" runat="server" CssClass="form-control" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddlSession_SelectedIndexChanged"></asp:DropDownList>
                                                            </div>

                                                            <div class="col-lg-3 col-md-3 col-sm-3">
                                                                <b>Select Program</b>
                                                                <asp:DropDownList ID="ddlBatchProgram" runat="server" CssClass="form-control" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddlBatchProgram_SelectedIndexChanged"></asp:DropDownList>
                                                            </div>
                                                            <div class="col-lg-2 col-md-2 col-sm-2">
                                                                <br />
                                                                <asp:LinkButton ID="lnkAddNewBatch" runat="server" Width="100%" type="button" OnClick="lnkAddNewBatch_Click" CssClass="btn-info btn-lg" Style="text-align: center" Font-Size="Small">
                                                                    <b>Add New</b>
                                                                </asp:LinkButton>
                                                            </div>

                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="card" runat="server" id="DivBatchAdd" style="margin-top: 10px">
                                            <div class="card-body">

                                                <asp:HiddenField ID="hdnBatchId" runat="server" />

                                                <div class="row">
                                                    <div class="col-lg-3 col-md-3 col-sm-3">
                                                        <b>Select Session</b>
                                                        <asp:CompareValidator ID="CompareValidator6" runat="server"
                                                            ControlToValidate="ddlAddSession" ErrorMessage="Required" Font-Size="10pt" Font-Bold="true"
                                                            ForeColor="Red" Display="Dynamic" ValueToCompare="0" Operator="NotEqual" CssClass="blink"
                                                            ValidationGroup="VGB"></asp:CompareValidator>
                                                        <asp:DropDownList ID="ddlAddSession" runat="server" CssClass="form-control" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddlSession_SelectedIndexChanged"></asp:DropDownList>
                                                    </div>

                                                    <div class="col-lg-3 col-md-3 col-sm-3">
                                                        <b>Select Program</b>
                                                        <asp:CompareValidator ID="CompareValidator7" runat="server"
                                                            ControlToValidate="ddlAddProgram" ErrorMessage="Required" Font-Size="10pt" Font-Bold="true"
                                                            ForeColor="Red" Display="Dynamic" ValueToCompare="0" Operator="NotEqual" CssClass="blink"
                                                            ValidationGroup="VGB"></asp:CompareValidator>
                                                        <asp:DropDownList ID="ddlAddProgram" runat="server" CssClass="form-control" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddlBatchProgram_SelectedIndexChanged"></asp:DropDownList>
                                                    </div>

                                                    <div class="col-lg-2 col-md-2 col-sm-2">
                                                        <b>Batch No</b>
                                                        <asp:RequiredFieldValidator ID="CompareValidator8" runat="server"
                                                            ControlToValidate="txtBatchNo" ErrorMessage="Required" Font-Size="10pt" Font-Bold="true"
                                                            ForeColor="Red" Display="Dynamic" CssClass="blink"
                                                            ValidationGroup="VGB"></asp:RequiredFieldValidator>
                                                        <asp:TextBox ID="txtBatchNo" runat="server" CssClass="form-control" Width="100%" TextMode="Number" step="1"></asp:TextBox>
                                                    </div>

                                                    <div class="col-lg-2 col-md-2 col-sm-2">
                                                        <br />
                                                        <asp:LinkButton ID="lnkBatchSave" runat="server" ValidationGroup="VGB" Width="100%" type="button" OnClick="lnkBatchSave_Click" CssClass="btn-info btn-lg" Style="text-align: center" Font-Size="Small">
                                                                    <b>Save</b>
                                                        </asp:LinkButton>
                                                    </div>

                                                    <div class="col-lg-2 col-md-2 col-sm-2">
                                                        <br />
                                                        <asp:LinkButton ID="lnkBatchCancel" runat="server" Width="100%" type="button" OnClick="lnkBatchCancel_Click" CssClass="btn-danger btn-lg" Style="text-align: center" Font-Size="Small">
                                                                    <b>Cancel</b>
                                                        </asp:LinkButton>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>


                                        <div class="card" runat="server" id="DivBatchGrid" style="margin-top: 10px">
                                            <div class="card-body">
                                                <asp:GridView runat="server" ID="gvBatchList" AllowSorting="True" CssClass="table table-bordered"
                                                    AutoGenerateColumns="False" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                                                    <HeaderStyle BackColor="#4CA1D7" ForeColor="White" Height="10px" Font-Bold="True" />
                                                    <FooterStyle BackColor="#4CA1D7" ForeColor="White" Height="10px" Font-Bold="True" />
                                                    <AlternatingRowStyle BackColor="White" />
                                                    <RowStyle Height="10px" />

                                                    <Columns>

                                                        <asp:TemplateField HeaderText="SL#">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblSL" Text='<%# Container.DataItemIndex + 1 %>' ForeColor="Black" Font-Bold="true"></asp:Label>

                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Session">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblSession" Text='<%#Eval("Attribute2") %>' ForeColor="Black"></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Program">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblBatchProgram" Text='<%#Eval("Attribute1") %>' ForeColor="Black"></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Batch No">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblBatchNo" Text='<%#Eval("BatchNO") %>' ForeColor="Black"></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle />
                                                        </asp:TemplateField>


                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <div style="padding: 5px; text-align: center">
                                                                    <asp:LinkButton ID="editBatch" ToolTip="Edit Batch" CssClass="btn-primary btn-sm" CommandArgument='<%#Eval("BatchId")%>' runat="server" OnClick="editBatch_Click">
                                                                             <strong><i class="fas fa-pencil-alt"></i></strong>
                                                                    </asp:LinkButton>
                                                                    &nbsp; &nbsp; &nbsp;
                                                    <asp:LinkButton ID="RemoveBatch" ToolTip="Remove Batch" CommandArgument='<%#Eval("BatchId")%>' runat="server" OnClientClick="return confirm('Are you sure you want to Remove this Batch ?');" CssClass="btn-danger btn-sm" OnClick="RemoveBatch_Click">                         
                                                                                <strong><i class="fas fa-trash"></i></strong>
                                                    </asp:LinkButton>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>

                                    </div>

                                    <%--------------------------- Batch Setup Information -----------------------------%>
                                </div>
                            </div>
                        </div>

                    </div>

                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>





    <ajaxToolkit:UpdatePanelAnimationExtender
        ID="UpdatePanelAnimationExtender1" TargetControlID="UpdatePanel02" runat="server">
        <Animations>
            <OnUpdating>
                <Parallel duration="0">
                    <ScriptAction Script="InProgress();" />
                    <EnableAction AnimationTarget="btnAddNew" Enabled="false" />                   
                </Parallel>
            </OnUpdating>
            <OnUpdated>
                <Parallel duration="0">
                    <ScriptAction Script="onComplete();" />
                    <EnableAction   AnimationTarget="btnAddNew" Enabled="true" />
                </Parallel>
            </OnUpdated>
        </Animations>
    </ajaxToolkit:UpdatePanelAnimationExtender>

</asp:Content>
