<%@ Page Title="Course Explorer" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true"
    Inherits="EMS.SyllabusMan.CourseExplorer" CodeBehind="CourseExplorer.aspx.cs" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<%@ Register Src="~/UserControls/ProgramUserControl.ascx" TagPrefix="uc1" TagName="ProgramUserControl" %>
<%@ Register Src="~/UserControls/DepartmentUserControl.ascx" TagPrefix="uc1" TagName="DepartmentUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="Server">
    Course List
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="Server">
    <link href="../ContentCSS/CSS/ChildSiteMaster.CSS" rel="stylesheet" />

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

        .table {
            border: 1px solid #008080;
        }


        .style10 {
            font-family: Arial, Helvetica, sans-serif;
            font-size: 12px;
            line-height: 24px;
            color: #333333;
            vertical-align: Middle;
            width: 100%;
            height: 100px;
        }

        .style11 {
            height: 28px;
        }

        .dxeButtonEdit {
            background-color: white;
            border: solid 1px #9F9F9F;
            width: 170px;
        }

        .dxeButtonEdit {
            background-color: white;
            border: solid 1px #9F9F9F;
            width: 170px;
        }

        .style12 {
            border: 1px solid Blue;
            font: 11px Arial, Helvetica, sans-serif;
            color: #666666;
            vertical-align: Middle;
            width: 27%;
        }

        .button_load_SaveOrUpdate {
            height: 38px;
            width: 90px;
            border-radius: 5px;
            padding-left: 23px;
            background-color: blue;
            color: white;
        }

        .button_Add {
            height: 38px;
            width: 175px;
            border-radius: 5px;
            padding-left: 23px;
            background-color: #368445;
            color: white;
        }

        .button_close {
            height: 38px;
            width: 90px;
            border-radius: 5px;
            padding-left: 23px;
            background-color: #d7393b;
            color: white;
        }

        #ctl00_MainContainer_ucDepartment_ddlDepartment,#ctl00_MainContainer_ucProgram_ddlProgram
        ,#ctl00_MainContainer_searchFormalCode,#ctl00_MainContainer_searchTitle,#ctl00_MainContainer_btnLoad,#ctl00_MainContainer_btnAddNew {
    height: 40px !important;
    width: 100% !important;
    font-size:20px !important;
}

    </style>
    <script type="text/javascript">
        function isNumber(e) {
            var charCode = (navigator.appName == 'Netscape') ? e.which : e.keyCode
            status = charCode
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                alert("Please make sure entries are numbers only")

                return false
            }

            return true;
        }

        function check() {

            var txtFoCode = document.getElementById("<%=txtFormalCode.ClientID%>");
            var txtVeCode = document.getElementById("<%=txtVersionCode.ClientID%>");
            var txtTtle = document.getElementById("<%=txtTitle.ClientID%>");
            var txtCdits = document.getElementById("<%=txtCredits.ClientID%>");

            if (txtFoCode.value == "" || txtVeCode.value == "" || txtTtle.value == "" || txtCdits.value == "") {

                document.getElementById('<%= btnSaveOrUpdate.ClientID %>').disabled = true;
            }
            else {
                document.getElementById('<%= btnSaveOrUpdate.ClientID %>').disabled = false;
            }
        }

        function onlyDotsAndNumbers(event) {
            var charCode = (event.which) ? event.which : event.keyCode
            if (charCode == 46) {
                return true;
            }
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                alert("Please make sure entries are numbers only")
                return false;
            }
            return true;
        }
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

<asp:Content ID="Content3" ContentPlaceHolderID="MainContainer" runat="server">
    <div>
        <div>
            <div class="well" style="margin-top: 20px;">
                <div class="PageTitle">
                    <label>Course List</label>
                </div>

                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <div class="Message-Area">
                            <asp:Label ID="Label2" runat="server" Text="Message : " Font-Bold="true"></asp:Label>
                            <asp:Label ID="lblMessage" ForeColor="Red" runat="server"></asp:Label>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>


                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>

                        <div class="row">
                            <div class="col-lg-3 col-md-3 col-sm-3">
                                <b>Choose Department</b>
                                <br />

                                <uc1:DepartmentUserControl runat="server" ID="ucDepartment" OnDepartmentSelectedIndexChanged="OnDepartmentSelectedIndexChanged" />
                            </div>
                            <div class="col-lg-3 col-md-3 col-sm-3">
                                <b>Choose Program</b>
                                <br />
                                <uc1:ProgramUserControl runat="server" ID="ucProgram" class="margin-zero dropDownList" />
                            </div>
                            <div class="col-lg-3 col-md-3 col-sm-3">
                                <b>Course Code</b>
                                <br />

                                <asp:TextBox ID="searchFormalCode" runat="server" Width="350px"></asp:TextBox>
                            </div>
                            <div class="col-lg-3 col-md-3 col-sm-3">
                                <b>Course Name</b>
                                <br />
                                <asp:TextBox ID="searchTitle" runat="server" Width="350px"></asp:TextBox>

                            </div>
                        </div>
                        <div class="row" style="margin-top: 10px">
                            <div class="col-lg-3 col-md-3 col-sm-3">
                                <asp:Button ID="btnLoad" runat="server" Text="Click Here To Load Courses" Width="100%" OnClick="btnLoad_Click" CssClass="btn-info" />

                            </div>
                            <div class="col-lg-3 col-md-3 col-sm-3">
                                <asp:Button ID="btnAddNew" runat="server" Width="100%" Text="Click Here To Add New Course" CssClass="btn-success" OnClick="btnAddNewCourse_Click" />

                            </div>
                            <div class="col-lg-2 col-md-2 col-sm-2">
                                <asp:Button ID="btnExportExcel" runat="server" class="button_load_SaveOrUpdate" Visible="false" Text="Excel Export" OnClick="btnExportExcel_Click" />

                            </div>
                        </div>



                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnExportExcel" />

                    </Triggers>
                </asp:UpdatePanel>

                <div id="div1" style="display: none; z-index: 1000; position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%);">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Img/Waiting.gif" Height="150px" Width="150px" />
                </div>
            </div>
            <ajaxToolkit:UpdatePanelAnimationExtender
                ID="UpdatePanelAnimationExtender1"
                TargetControlID="UpdatePanel1"
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


            <div>
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <div>
                            <div>
                                <div style="margin-bottom: 5px; margin-top: 5px; float: left; width: 100%;">
                                </div>

                                <div class="Teacher-container">
                                    <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
                                    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnShowPopup" PopupControlID="pnPopUp" CancelControlID="btnClose" BackgroundCssClass="modalBackground">
                                    </ajaxToolkit:ModalPopupExtender>
                                    <asp:Panel runat="server" ID="pnPopUp" Style="display: none;">

                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                            <ContentTemplate>
                                                <div style="height: auto; width: auto; padding: 5px; margin: 5px; background-color: Window;">

                                                    <fieldset style="padding: 10px; margin: 5px; border-color: lightgreen;">
                                                        <legend>Course Info</legend>
                                                        <div class="well" style="padding: 5px; width: 100%">
                                                            <div>
                                                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                                    <ContentTemplate>
                                                                        <div class="Message-Area">
                                                                            <asp:Label ID="Label1" runat="server" Text="Message : " Font-Bold="true"></asp:Label>
                                                                            <asp:Label ID="lblPopUpMassege" ForeColor="Red" runat="server"></asp:Label>
                                                                        </div>
                                                                    </ContentTemplate>
                                                                </asp:UpdatePanel>
                                                            </div>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <label class="display-inline field-Title">Formal Code: *</label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:HiddenField ID="hdnCourseId" runat="server" />
                                                                        <asp:HiddenField ID="hdnVersionId" runat="server" />
                                                                        <asp:TextBox runat="server" ID="txtFormalCode" class="margin-zero label-width" Width="236px" onblur="check();" onkeyup="check();" AutoPostBack="false" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <label class="display-inline field-Title">Version Code: *</label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox runat="server" ID="txtVersionCode" class="margin-zero label-width" Width="236px" onblur="check();" onkeyup="check();" />
                                                                    </td>
                                                                </tr>

                                                                <tr>
                                                                    <td>
                                                                        <label class="display-inline field-Title">Program :</label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlProgram" runat="server" Width="250px"></asp:DropDownList>
                                                                    </td>
                                                                </tr>

                                                                <tr>
                                                                    <td>
                                                                        <label class="display-inline field-Title">Transcript Code:</label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox runat="server" ID="txtTranscriptCode" class="margin-zero label-width" Width="236px" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <label class="display-inline field-Title">Title: *</label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox runat="server" ID="txtTitle" class="margin-zero label-width" Width="236px" onblur="check();" onkeyup="check();" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <label class="display-inline field-Title">Course Content:</label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox runat="server" ID="txtCourseContent" TextMode="MultiLine" Height="60" class="margin-zero label-width" Width="236px" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <label class="display-inline field-Title">Credits: *</label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox runat="server" ID="txtCredits" class="margin-zero label-width" Width="236px" onkeyup="check();" onkeypress="return onlyDotsAndNumbers(event)" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <label class="display-inline field-Title">Marks: </label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox runat="server" ID="txtMarks" class="margin-zero label-width" Width="236px" />

                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <label class="display-inline field-Title">Course Group:</label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox runat="server" ID="txtCourseGroup" class="margin-zero label-width" Width="236px" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <label class="display-inline field-Title">Thesis/Project:</label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlMultiple" runat="server" AutoPostBack="false" EnableViewState="true" Width="250px">
                                                                            <asp:ListItem Value="0">No</asp:ListItem>
                                                                            <asp:ListItem Value="1">Yes</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <label class="display-inline field-Title">Type:</label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlCourseType" runat="server" AutoPostBack="false" EnableViewState="true" Width="250px">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <label class="display-inline field-Title">Is Active:</label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlIsActive" runat="server" AutoPostBack="false" EnableViewState="true" Width="250px">
                                                                            <asp:ListItem Value="1">Yes</asp:ListItem>
                                                                            <asp:ListItem Value="0">No</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Button ID="btnSaveOrUpdate" runat="server" Text="SaveOrUpdate" Class="button_load_SaveOrUpdate" OnClick="btnSaveOrUpdate_Click" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Button ID="btnClose" runat="server" Text="Close" Class="button_close" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                    </fieldset>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </asp:Panel>
                                </div>
                                <div style="clear: both;"></div>

                                <div>
                                    <asp:GridView ID="gvCourselists" OnSorting="gvStudentBillView_Sorting" AllowSorting="True" runat="server" CssClass="table-bordered"
                                        AutoGenerateColumns="False" ShowFooter="True" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">

                                        <HeaderStyle BackColor="#4285f4" ForeColor="White" Height="10px" Font-Bold="True" />
                                        <FooterStyle BackColor="#4285f4" ForeColor="White" Height="10px" Font-Bold="True" />
                                        <AlternatingRowStyle BackColor="White" />
                                        <RowStyle Height="10px" />

                                        <Columns>
                                            <asp:TemplateField HeaderText="SL" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate><b><%# Container.DataItemIndex + 1 %></b></ItemTemplate>
                                                <HeaderStyle Width="35px" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Course Code" ItemStyle-HorizontalAlign="Left">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="lnkFormalCode" runat="server" ForeColor="White" CommandName="Sort" CommandArgument="FormalCode">Course Code</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblFormalCode" Font-Bold="True" Text='<%#Eval("FormalCode") %>' />
                                                </ItemTemplate>
                                                <HeaderStyle Width="100px" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Transcript Code" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblTranscriptCode" Font-Bold="True" Text='<%#Eval("TranscriptCode") %>' />
                                                </ItemTemplate>
                                                <HeaderStyle Width="100px" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <%--<asp:TemplateField HeaderText="Course Group" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblCourseGroup" Font-Bold="True" Text='<%#Eval("CourseGroup") %>' />
                                                </ItemTemplate>
                                                <HeaderStyle Width="80px" />
                                                 <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>--%>
                                            <asp:TemplateField HeaderText="Title" ItemStyle-HorizontalAlign="Left">
                                                <HeaderTemplate>
                                                    <asp:LinkButton ID="lnkTitle" runat="server" ForeColor="White" CommandName="Sort" CommandArgument="Title">Title</asp:LinkButton>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblTitle" Font-Bold="True" Text='<%#Eval("Title") %>' />
                                                </ItemTemplate>
                                                <%--<HeaderStyle Width="100%"/>--%>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Credits" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblCredits" Font-Bold="True" Text='<%#Eval("Credits") %>' />
                                                </ItemTemplate>
                                                <HeaderStyle Width="50px" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Marks">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblMarks" Font-Bold="True" Text='<%#Eval("CourseExtend.Marks") %>' />
                                                </ItemTemplate>
                                                <HeaderStyle Width="80px" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Thesis/Project" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblThesisOrProject" Font-Bold="True" Text='<%#Eval("HasMultipleACUSpan") == null ? "" : (Boolean.Parse(Eval("HasMultipleACUSpan").ToString())) ? "Yes" : "No" %>' />
                                                </ItemTemplate>
                                                <HeaderStyle Width="60px" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Is Active" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblIsActive" Font-Bold="True" Text='<%#  Eval("IsActive") == null ? "" : (Boolean.Parse(Eval("IsActive").ToString())) ? "Yes" : "No" %>' />
                                                </ItemTemplate>
                                                <HeaderStyle Width="60px" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Type" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblTypeDefinitionID" Font-Bold="True" Text='<%#Eval("CourseType") %>' />
                                                </ItemTemplate>
                                                <HeaderStyle Width="60px" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <div style="text-align: center">
                                                        <asp:LinkButton runat="server" ToolTip="Edit" ID="lnkEdit" CommandArgument='<%#Eval("CourseID")+","+ Eval("VersionID")%>' OnClick="lnkEdit_Click">
                                                            <span class="action-container">Edit</span>
                                                        </asp:LinkButton>
                                                    </div>
                                                </ItemTemplate>
                                                <HeaderStyle Width="40px" />
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
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div id="divProgress" style="display: none; z-index: 1000; position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%);">
                    <asp:Image ID="LoadingImage" runat="server" ImageUrl="~/Images/Img/Waiting.gif" Height="150px" Width="150px" />
                </div>

            </div>
        </div>
    </div>
</asp:Content>
