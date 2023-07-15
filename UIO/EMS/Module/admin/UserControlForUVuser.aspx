<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true" CodeBehind="UserControlForUVuser.aspx.cs" Inherits="EMS.Module.admin.UserControlForUVuser" %>

<%@ Register Src="~/UserControls/ProgramUserControl.ascx" TagPrefix="uc1" TagName="ProgramUserControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Title" Runat="Server">User Control</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" Runat="Server">

    <link href="../../ContentCSS/CSS/ChildSiteMaster.CSS" rel="stylesheet" />

    <script type="text/javascript">
     
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContainer" Runat="Server">
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
                <div class="PasswordChangeByUser-container">
                    <%--<div class="div-margin">
                        <label class="display-inline field-Title" style="float:left"><b>Program</b></label>
                        <div>
                            <uc1:ProgramUserControl runat="server" ID="ucProgram" OnProgramSelectedIndexChanged="OnProgramSelectedIndexChanged"/>
                        </div>                        
                        
                    </div>--%>
                    <div class="div-margin">
                    <label class="display-inline field-Title"><b>User Type</b></label>
                        <asp:DropDownList runat ="server" ID="ddlUserType" Width="150px" OnSelectedIndexChanged="onUserTypeSelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                         </div>
                    <div class="div-margin">
                        <label class="display-inline field-Title"><b>User Name</b></label>
                        <asp:DropDownList runat="server" ID="ddlTeacher" Width="300px" OnSelectedIndexChanged="onTeacherSelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        <asp:Button runat="server" ID="btnLoadTeacher" OnClick="btnLoadTeacherClick" Text="Load" />
                        <asp:TextBox runat="server" ID="txtNameSearch" ></asp:TextBox>
                        <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearchClick"/>
                    </div>
                    <%--<div class="div-margin">
                    <label class="display-inline field-Title"><b>Office</b></label>
                        <asp:DropDownList runat ="server" ID="ddlOffice" Width="150px" OnSelectedIndexChanged="onOfficeSelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    </div>--%>
                    <div class="div-margin">
                        <label class="display-inline field-Title"><b>Role</b></label>
                        <asp:DropDownList runat="server" ID="ddlRole" Width="150px"></asp:DropDownList>
                    </div>
                    <div class="div-margin">
                        <label class="display-inline field-Title"><b>Access All Course</b></label>
                        <asp:CheckBox runat="server" ID="chkAllCourse" />
                    </div>
                    <div class="div-margin">
                        <label class="display-inline field-Title"><b>Login ID</b></label>
                        <asp:TextBox runat="server" ID="txtUserId"></asp:TextBox>
                        <asp:Button runat="server" ID="btnValidate" Text="Validate" OnClick="btnValidateClick" />
                        <asp:Label runat="server" ID="lblValidate"></asp:Label>
                    </div>
                    <div class="div-margin">
                        <label class="display-inline field-Title"><b>Password</b></label>
                        <asp:TextBox runat="server" ID="txtPassword" TextMode="Password" Enabled="false" />
                        <asp:Button runat="server" ID="btnSendPassword" Text="Send by Email" OnClick="btnSendPassword_Click" />
                    </div>
                    <div class="div-margin">
                        <label class="display-inline field-Title"><b>Valid From</b></label>
                        <asp:TextBox runat="server" ID="txtStartDate"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender ID="validfrom" runat="server" TargetControlID="txtStartDate" Format="dd/MM/yyyy" />
                    </div>
                    <div class="div-margin">
                        <label class="display-inline field-Title"><b>Valid To</b></label>
                        <asp:TextBox runat="server" ID="txtEndDate"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender ID="validto" runat="server" TargetControlID="txtEndDate" Format="dd/MM/yyyy" />
                    </div>
                    <div class="div-margin">
                        <label class="display-inline field-Title"><b>Active User</b></label>
                        <asp:CheckBox runat="server" ID="chkIsActive"></asp:CheckBox>
                    </div>
                    <div class="div-margin">
                        <label class="display-inline field-Title" style="float:left"><b>Program Access</b></label>
                        <div style="float:left">
                            <uc1:ProgramUserControl runat="server" ID="ucAccessableProgram"/>
                        </div>
                        <asp:Button runat="server" ID="btnAdd" Text="Add" OnClick="btnAddClicked"/>
                        <asp:Button runat="server" ID="btnAllProg" Text="All Program" OnClick="btnAllProgClicked" />  
                        <asp:Button runat="server" ID="btnClearProg" Text="Clear All" OnClick="btnClearProgClicked" /> 
                    </div>
                    <div class="div-margin">
                        <asp:CheckBoxList runat="server" ID="chkAccessList">
                            
                        </asp:CheckBoxList>  
                    </div>
                    <div class="div-margin">
                        <label class="display-inline field-Title"></label>
                        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSaveClicked" OnClientClick="return confirm('Are you sure want save this user information?');" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        
    </div>
</asp:Content>
