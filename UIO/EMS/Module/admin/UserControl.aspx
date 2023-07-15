<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true" CodeBehind="UserControl.aspx.cs" Inherits="EMS.Module.admin.UserControl" %>

<%@ Register Src="~/UserControls/ProgramUserControl.ascx" TagPrefix="uc1" TagName="ProgramUserControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="Server">User Control</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="Server">

    <%--<link href="../../ContentCSS/CSS/ChildSiteMaster.CSS" rel="stylesheet" />--%>

    <script type="text/javascript">
     
    </script>

    <style type="text/css">
        #ctl00_MainContainer_chkIsActive, #ctl00_MainContainer_chkAllCourse {
            width: 30px;
            height: 30px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContainer" runat="Server">
    <div>
        <div class="PageTitle">
            <label>User Control</label>
        </div>

        <asp:UpdatePanel ID="UpdatePanel01" runat="server">
            <ContentTemplate>

                <div class="Message-Area">
                    <label class="msgTitle">Message: </label>
                    <asp:Label runat="server" ID="lblMsg" Text="" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:UpdatePanel ID="UpdatePanel02" runat="server">
            <ContentTemplate>


                <div class="row">

                    <div class="col-lg-2 col-md-2 col-sm-2">
                        <b>User Type</b>
                        <asp:DropDownList runat="server" ID="ddlUserType" CssClass="form-control" OnSelectedIndexChanged="onUserTypeSelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    </div>

                    <div class="col-lg- col-md-2 col-sm-2">
                        <b>User Name</b>
                        <asp:DropDownList runat="server" ID="ddlTeacher" CssClass="form-control" OnSelectedIndexChanged="onTeacherSelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>

                    </div>

                    <div class="col-lg-2 col-md-2 col-sm-2">
                        <br />
                        <asp:Button runat="server" ID="btnLoadTeacher" OnClick="btnLoadTeacherClick" Width="100%" Text="Load" CssClass="btn-info" />
                    </div>

                    <div class="col-lg-2 col-md-2 col-sm-2">
                        <br />
                        <asp:TextBox runat="server" ID="txtNameSearch" CssClass="form-control"></asp:TextBox>
                    </div>

                    <div class="col-lg-2 col-md-2 col-sm-2">
                        <br />
                        <asp:Button runat="server" ID="btnSearch" Text="Search" Width="100%" OnClick="btnSearchClick" CssClass="btn-primary" />
                    </div>

                </div>


                <div class="row" style="margin-top: 10px">

                    <div class="col-lg- col-md-2 col-sm-2">
                        <b>Role</b>
                        <asp:DropDownList runat="server" ID="ddlRole" CssClass="form-control"></asp:DropDownList>
                    </div>

                    <div class="col-lg- col-md-2 col-sm-2">
                        <b>Login ID</b>
                        <asp:TextBox runat="server" ID="txtUserId" CssClass="form-control"></asp:TextBox>
                    </div>

                    <div class="col-lg- col-md-2 col-sm-2">
                        <br />
                        <asp:Button runat="server" ID="btnValidate" Width="100%" Text="Validate" OnClick="btnValidateClick" CssClass="btn-danger" />
                        <asp:Label runat="server" ID="lblValidate"></asp:Label>
                    </div>

                    <div class="col-lg- col-md-2 col-sm-2">
                        <b>Password</b>
                        <asp:TextBox runat="server" ID="txtPassword" CssClass="form-control" />
                    </div>

                    <div class="col-lg- col-md-2 col-sm-2">
                        <br />
                        <asp:Button runat="server" ID="btnSendPassword" Text="Password Send by Email" Width="100%" OnClick="btnSendPassword_Click" CssClass="btn-default" />
                    </div>
                </div>


                <div class="row" style="margin-top: 10px">

                    <div class="col-lg- col-md-2 col-sm-2">
                        <b>Valid From</b>
                        <asp:TextBox runat="server" ID="txtStartDate" CssClass="form-control"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender ID="validfrom" runat="server" TargetControlID="txtStartDate" Format="dd/MM/yyyy" />
                    </div>

                    <div class="col-lg- col-md-2 col-sm-2">
                        <b>Valid To</b>
                        <asp:TextBox runat="server" ID="txtEndDate" CssClass="form-control"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender ID="validto" runat="server" TargetControlID="txtEndDate" Format="dd/MM/yyyy" />
                    </div>

                    <div class="col-lg- col-md-2 col-sm-2">
                        <div style="text-align: center">
                            <b>Active User</b>
                        </div>
                        <div style="text-align: center">
                            <asp:CheckBox runat="server" ID="chkIsActive"></asp:CheckBox>
                        </div>
                    </div>

                    <div class="col-lg- col-md-2 col-sm-2">
                        <div style="text-align: center">
                            <b>Access All Course</b>
                        </div>
                        <div style="text-align: center">
                            <asp:CheckBox runat="server" ID="chkAllCourse" />
                        </div>
                    </div>
                </div>

                <div class="row" style="margin-top: 10px">

                    <div class="col-lg- col-md-2 col-sm-2">
                        <b>Program Access</b>
                        <br />
                        <uc1:ProgramUserControl runat="server" ID="ucAccessableProgram" />
                    </div>
                    <div class="col-lg- col-md-2 col-sm-2">
                        <br />
                        <asp:Button runat="server" ID="btnAdd" Text="Add" OnClick="btnAddClicked" CssClass="btn-info form-con" Width="100%" />
                    </div>
                    <div class="col-lg- col-md-2 col-sm-2">
                        <br />
                        <asp:Button runat="server" ID="btnAllProg" Text="All Program" OnClick="btnAllProgClicked" CssClass="btn-info" Width="100%" />
                    </div>
                    <div class="col-lg- col-md-2 col-sm-2">
                        <br />
                        <asp:Button runat="server" ID="btnClearProg" Text="Clear All" OnClick="btnClearProgClicked" CssClass="btn-default" Width="100%" />
                    </div>
                    <div class="col-lg- col-md-2 col-sm-2">
                    </div>
                </div>

                <div class="PasswordChangeByUser-container" style="margin-top: 10px">
                    <div class="div-margin">
                        <asp:CheckBoxList runat="server" ID="chkAccessList" RepeatDirection="Horizontal" RepeatColumns="10">
                        </asp:CheckBoxList>
                    </div>
                    <div class="div-margin">
                        <div class="row">
                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <asp:Button ID="btnSave" runat="server" Text="Save" Width="100%" CssClass="btn-primary" OnClick="btnSaveClicked" OnClientClick="return confirm('Are you sure want save this user information?');" />
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>


    </div>
</asp:Content>
