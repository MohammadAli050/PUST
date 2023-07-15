<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true" 
    CodeBehind="CurriculumDistribution.aspx.cs" Inherits="EMS.Module.Setup.CurriculumDistribution" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Curriculum Distribution
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <style type="text/css">
        .modalBackground
        {
            background-color: #2a2d2a;
            filter: alpha(opacity=80);
            opacity: 0.8;
            z-index: 10000;
        }

        .font
        {
            font-size: 12px;
        }

        .cursor
        {
            cursor: pointer;
        }

        .auto-style3
        {
            width: 100px;
        }
        .button_Page_Re_load {
           height: 38px;
           width: 167px;
           border-radius: 5px;
           padding-left: 23px;
           background-color: #368445;
           color: white;
                    }
        .button_Clear_save {
            height: 38px;
            width: 100px;
            border-radius: 5px;
            padding-left: 23px;
            background-color: #368445;
            color: white;

        }
        .button_delete {
            height: 38px;
            width: 100px;
            border-radius: 5px;
            padding-left: 17px;
            background-color: #d7393b;
            color: white;

        }

    </style>
    
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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContainer" runat="server">
    <div class="well" style="height: auto; width: auto; margin-top:20px;">
        <div class="PageTitle">
            <label>Curriculum Distribution</label>
        </div>

        <div class="Message-Area">
            <asp:UpdatePanel ID="UpdatePanelMessage" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="pnlMessage" runat="server" Visible="true">
                        <asp:Label ID="Label2" runat="server" Text=""></asp:Label>
                        <asp:Label ID="lblMessage" runat="server" ForeColor="#CC0000" Width="50%"></asp:Label>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <asp:Panel ID="Panel1" runat="server">
            <div class="Message-Area">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div id="divProgress" style="display: none; float: right; z-index: 1000;">
                            <asp:Image ID="LoadingImage" runat="server" ImageUrl="~/Images/Img/Waiting.gif" Height="35px" Width="35px" />
                        </div>
                        <div class="loadArea"> 
                            <table style="padding: 1px;" border="0">
                                <tr>
                                    <td><b>Program :</b></td> 
                                    <td>
                                        <asp:DropDownList ID="ddlProgram" runat="server" AutoPostBack="true" Width="200px" OnSelectedIndexChanged="ddlProgram_SelectedIndexChanged"></asp:DropDownList>
                                        <asp:Label ID="lblProgramId" runat="server" Visible="false"></asp:Label>
                                    </td>                                                                                                                                                                                                  
                                </tr>
                                <tr>
                                    <td><b>Tree :</b></td> 
                                    <td>
                                        <asp:DropDownList ID="ddlTree" runat="server" AutoPostBack="true" Width="200px" OnSelectedIndexChanged="ddlTree_SelectedIndexChanged"></asp:DropDownList>
                                        <asp:Label ID="lblTreeId" runat="server" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td><b>Distribution:</b></td> 
                                    <td>
                                        <asp:DropDownList ID="ddlCalenderDistribution" runat="server" AutoPostBack="true" Width="200px" OnSelectedIndexChanged="ddlCalenderDistribution_SelectedIndexChanged" ></asp:DropDownList>
                                        <asp:Label ID="lblCalenderDistributionId" runat="server" Visible="false"></asp:Label>
                                    </td>
                                    <td style="align-content:center; width:200px" >
                                        <asp:Button ID="btnAddNewDistName" runat="server" Text="Add New Distribution" visible="false" OnClick="btnAddNewDistName_Click" />
                                    </td>  
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnReloadTree" runat="server" Text="Page Re-load "  Visible="false" Class="button_Page_Re_load"  OnClick="btnReloadTree_Click"/></td>
                                    <td>
                                        <asp:Button ID="btnClearFields" runat="server" Text=" Clear " Visible="false" Class="button_Clear_save"  OnClick="btnClearFields_Click"/>
                                    </td> 
                                                                                                                                                                                                                                   
                                </tr>
                            </table>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </asp:Panel>

        <div class="Message-Area">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <table>
                        <tr>
                            <td valign="top">
                                <div style="Height:540px; width:427px; overflow-y:auto; float:left; border-style: solid; border-width: 2px; border-color:#FFA500; padding: 10px;">
                                    <asp:Label ID="lblTreeView" runat="server" Font-Bold="true" Text="Tree View"></asp:Label>                                    
                                    <asp:Panel ID="pnlTreeView" runat="server">
                                        <asp:TreeView ID="tvwCalendar" runat="server" Width="100%" ShowLines="True"
                                            Height="98%" onselectednodechanged="tvwCalendar_SelectedNodeChanged"
                                            LineImagesFolder="~/TreeLineImages" TabIndex="4">
                                            <ParentNodeStyle ForeColor="#660066" />
                                            <HoverNodeStyle ForeColor="#FF6600" />
                                            <SelectedNodeStyle ForeColor="Lime" Font-Bold="True" Font-Underline="True"/>
                                            <RootNodeStyle ForeColor="#000066" />
                                            <LeafNodeStyle ForeColor="#006699" />
                                        </asp:TreeView>
                                    </asp:Panel>
                                </div>
                            </td>
                            <td class="auto-style5">&nbsp;</td>
                            <td valign="top">
                                <div style="Height:540px; width:400px; overflow-y:auto; float:left; border-style: solid; border-width: 2px; border-color:#FFA500; padding: 10px;">
                                    <asp:Panel ID="pnlAddTreeItem" runat="server">
                                        <asp:Label ID="lblAddTreeItem" runat="server" Font-Bold="true" Text="Add Tree Item"></asp:Label>
                                        <asp:RadioButtonList ID="rbAddTreeItem" RepeatDirection="Horizontal" style="display:inline" AutoPostBack="true" runat="server" OnSelectedIndexChanged="rbAddTreeItem_SelectedIndexChanged">
                                            <asp:ListItem Text="Course" Value="1" Selected="True" />
                                            <asp:ListItem Text="Node" Value="2" />
                                        </asp:RadioButtonList>
                                        <asp:Panel ID="pnlAddCourse" Visible="true" runat="server">
                                            <table class="table" style="width: 100%; height:auto;">
                                                <tr>
                                                    <td style="width:150px">
                                                        <asp:Label ID="lblAddCourseTrimester" runat="server" Text="Trimester"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlAddCourseTrimester" Width="180px" runat="server"></asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblAddCourse" runat="server" Text="Course"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlAddCourse" Width="180px" runat="server"></asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblAddCourseCrComplete" runat="server" Text="Cr. To Complete"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtAddCourseCrComplete" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblAddCoursePriority" runat="server" Text="Priority"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtAddCoursePriority" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblAddCourseParentNode" runat="server" Text="Parent Node"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlAddCourseParentNode" Width="180px" runat="server"></asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td>
                                                        <asp:Button ID="btnAddCourseSave" runat="server" Text=" Save " Class="button_Clear_save" OnClick="btnAddCourseSave_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <asp:Panel ID="pnlAddNode" Visible="false" runat="server">
                                            <table class="table" style="width: 100%; height:auto;">
                                                <tr>
                                                    <td style="width:150px">
                                                        <asp:Label ID="lblAddNodeTrimester" runat="server" Text="Trimester"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlAddNodeTrimester" Width="180px" runat="server"></asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblAddNodeName" runat="server" Text="Node Name"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtAddNodeName" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblAddNodeNodes" runat="server" Text="Nodes"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlAddNodeNodes" Width="180px" runat="server"></asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblAddNodeCrComplete" runat="server" Text="Cr. To Complete"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtAddNodeCrComplete" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblAddNodePriority" runat="server" Text="Priority"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtAddNodePriority" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblAddNodeIsMajor" runat="server" Text="Is Major Related"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkAddNodeIsMajor" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblAddNodeParentNode" runat="server" Text="Parent Node"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlAddNodeParentNode" Width="180px" runat="server"></asp:DropDownList>
                                                    </td>
                                                </tr>
                                                 <tr>
                                                    <td></td>
                                                    <td>
                                                        <asp:Button ID="btnAddNodeSave" runat="server" Text=" Save " Class="button_Clear_save" OnClick="btnAddNodeSave_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </asp:Panel>
                                    <br />
                                    <asp:Panel ID="pnlEditTreeItem" runat="server">
                                        <asp:Label ID="lblEditTreeItem" runat="server" Font-Bold="true"  Text="Edit Tree Item"></asp:Label>
                                        <asp:RadioButtonList ID="rbEditTreeItem" RepeatDirection="Horizontal" style="display:inline" AutoPostBack="true" runat="server" OnSelectedIndexChanged="rbEditTreeItem_SelectedIndexChanged">
                                            <asp:ListItem Text="Course" Value="1" Selected="True" />
                                            <asp:ListItem Text="Node" Value="2" />
                                        </asp:RadioButtonList>
                                        <asp:Panel ID="pnlEditCourse" Visible="true" runat="server">
                                            <table class="table" style="width: 100%; height:auto;">
                                                <tr>
                                                    <td style="width:150px">
                                                        <asp:Label ID="lblEditCourseTrimester" runat="server" Text="Trimester"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlEditCourseTrimester" Width="180px" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlEditCourseTrimester_SelectedIndexChanged"></asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblEditCourseOld" runat="server" Text="Current Courses"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlEditCourseOld" Width="180px" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlEditCourseOld_SelectedIndexChanged"></asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblEditCourseNew" runat="server" Text="New Courses"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlEditCourseNew" Width="180px" runat="server"></asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblEditCourseCrComplete" runat="server" Text="Cr. To Complete"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtEditCourseCrComplete" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblEditCoursePriority" runat="server" Text="Priority"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtEditCoursePriority" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblEditCourseParentNode" runat="server" Text="Parent Node"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlEditCourseParentNode" Width="180px" runat="server"></asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td>
                                                        <asp:Button ID="btnEditCourseSave" runat="server" Text=" Save " Class="button_Clear_save" OnClick="btnEditCourseSave_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <asp:Panel ID="pnlEditNode" Visible="false" runat="server">
                                            <table class="table" style="width: 100%; height:auto;">
                                                <tr>
                                                    <td style="width:150px">
                                                        <asp:Label ID="lblEditNodeTrimester" runat="server" Text="Trimester"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlEditNodeTrimester" Width="180px" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlEditNodeTrimester_SelectedIndexChanged"></asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblEditNodeOld" runat="server" Text="Current Nodes"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlEditNodeOld" Width="180px" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlEditNodeOld_SelectedIndexChanged"></asp:DropDownList>
                                                    </td>
                                                </tr>

                                               <tr>
                                                    <td>
                                                        <asp:Label ID="lblEditNodeNew" runat="server" Text="New Nodes"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlEditNodeNew" Width="180px"  runat="server"></asp:DropDownList>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblEditNodeName" runat="server" Text="Node Name"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtEditNodeName" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblEditNodeCrComplete" runat="server" Text="Cr. To Complete"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtEditNodeCrComplete" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblEditNodePriority" runat="server" Text="Priority"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtEditNodePriority" runat="server"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblEditNodeIsMajor" runat="server" Text="Is Major Related"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkEditNodeIsMajor" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblEditNodeParentNode" runat="server" Text="Parent Node"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlEditNodeParentNode" Width="180px" runat="server"></asp:DropDownList>
                                                    </td>
                                                </tr>
                                                 <tr>
                                                    <td></td>
                                                    <td>
                                                        <asp:Button ID="btnEditNodeSave" runat="server" Text=" Save " Class="button_Clear_save" OnClick="btnEditNodeSave_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </asp:Panel>
                                    <br />
                                    <asp:Panel ID="pnlDeleteTreeItem" runat="server">
                                        <asp:Label ID="lblDeleteTreeItem" runat="server" Font-Bold="true" Text="Remove Tree Item"></asp:Label>
                                        <asp:RadioButtonList ID="rbDeleteTreeItem" RepeatDirection="Horizontal" style="display:inline" AutoPostBack="true" runat="server" OnSelectedIndexChanged="rbDeleteTreeItem_SelectedIndexChanged">
                                            <asp:ListItem Text="Course" Value="1" Selected="True" />
                                            <asp:ListItem Text="Node" Value="2" />
                                        </asp:RadioButtonList>
                                        <asp:Panel ID="pnlDeleteCourse" Visible="true" runat="server">
                                            <table class="table" style="width: 100%; height:auto;">
                                                <tr>
                                                    <td style="width:150px">
                                                        <asp:Label ID="lblDeleteCourseTrimester" runat="server" Text="Trimester"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlDeleteCourseTrimester" Width="180px" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDeleteCourseTrimester_SelectedIndexChanged"></asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblDeleteCourse" runat="server" Text="Courses"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlDeleteCourse" Width="180px" runat="server"></asp:DropDownList>
                                                    </td>
                                                </tr>
                                                
                                                <tr>
                                                    <td></td>
                                                    <td>
                                                        <asp:Button ID="btnDeleteCourse" runat="server" Text=" Remove " Class="button_delete" OnClick="btnDeleteCourse_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <asp:Panel ID="pnlDeleteNode" Visible="false" runat="server">
                                            <table class="table" style="width: 100%; height:auto;">
                                                <tr>
                                                    <td style="width:150px">
                                                        <asp:Label ID="lblDeleteNodeTrimester" runat="server" Text="Trimester"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlDeleteNodeTrimester" Width="180px" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDeleteNodeTrimester_SelectedIndexChanged"></asp:DropDownList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblDeleteNode" runat="server" Text="Nodes"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlDeleteNode" Width="180px" runat="server" ></asp:DropDownList>
                                                    </td>
                                                </tr>

                                                 <tr>
                                                    <td></td>
                                                    <td>
                                                        <asp:Button ID="btnDeleteNode" runat="server" Text=" Remove " Class="button_delete" OnClick="btnDeleteNode_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </asp:Panel>
                                </div>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
                <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="btnShowPopup" PopupControlID="pnlpopup"
                    CancelControlID="btnCancel" BackgroundCssClass="modalBackground">
                </ajaxToolkit:ModalPopupExtender>
                <asp:Panel ID="pnlpopup" runat="server" BackColor="#ffffff" Width="765px" Style="display: none; border-radius: 3px;">
                    <div style="padding: 5px;">
                        <fieldset style="padding: 5px; border: 2px solid #ff9933;">
                            <legend style="font-weight: 100; font-size: small; font-variant: small-caps; color: blue; text-align: center">Message</legend>
                            <div style="padding: 5px;">
                                <div class="Message-Area">
                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                        <ContentTemplate>
                                            <asp:Panel ID="Panel2" runat="server" Visible="true">
                                                <asp:Label ID="lblNew" runat="server" Text="Message : "></asp:Label>
                                                <asp:Label ID="lblMsg" runat="server" ForeColor="#CC0000"></asp:Label>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <asp:Button ID="CancelButton" runat="server" Text="Close" CssClass="cursor"/></td>
                        </fieldset>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
        
        <asp:UpdatePanel ID="UpdatePanelAddTreeDistribution" runat="server">
            <ContentTemplate>
                <asp:Button ID="btnAddTreeDistributionPopUp" runat="server" Style="display: none" />
                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtenderAddTreeDistribution" runat="server" TargetControlID="btnAddTreeDistributionPopUp" PopupControlID="pnlAddTreeDistribution"
                    CancelControlID="btnCancelAddTreeDistributionPopUp" BackgroundCssClass="modalBackground">
                </ajaxToolkit:ModalPopupExtender>
                <asp:Panel ID="pnlAddTreeDistribution" runat="server" BackColor="#ffffff" Width="765px" Style="display: none; border-radius: 3px;">
                    <div style="padding: 5px;">
                        <fieldset style="padding: 5px; border: 2px solid #ff9933;">
                            <legend style="font-weight: 100; font-size: small; font-variant: small-caps; color: blue; text-align: center">Message</legend>
                            <div style="padding: 5px;">
                                <div class="Message-Area">
                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                        <ContentTemplate>
                                            <asp:Panel ID="Panel4" runat="server" Visible="true">
                                                <asp:Label ID="Label1" runat="server" Text="Message : "></asp:Label>
                                                <asp:Label ID="lblMsgAddTreeDistributionPopUp" runat="server" ForeColor="#CC0000"></asp:Label>
                                                <br />
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label3" runat="server" Text="Syllabus Name"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtDistribution" Placeholder="New Syllabus Name" Width="300px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label4" runat="server" Text="Semester Type"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlCalendarType" runat="server"></asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnSaveDistName" runat="server" Text="Save" OnClick="btnSaveDistName_Clicked" />
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="btnCancelAddTreeDistributionPopUp" runat="server" Text="Close" OnClick="btnCancelAddTreeDistributionPopUp_Click" CssClass="cursor"/>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </fieldset>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
