<%@ Page Title="Tree Master" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true" 
    CodeBehind="TreeMasterSetup.aspx.cs" Inherits="EMS.Module.setup.TreeMasterSetup" %>

<%@ Register Assembly="DevExpress.Web.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTabControl" TagPrefix="dxtc" %>

<%@ Register assembly="DevExpress.Web.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxClasses" tagprefix="dxw" %>
<%@ Register assembly="DevExpress.Web.ASPxEditors.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %>
<%@ Register assembly="DevExpress.Web.ASPxGridView.v9.2, Version=9.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dxwgv" %>
<%@ Register Src="~/UserControls/CoursePicker/CoursePickerCtl.ascx" TagName="CoursePicker" TagPrefix="uctl" %>
<%@ Register Src="~/UserControls/CourseSelector.ascx" TagName="CourseSelector" TagPrefix="uctl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Tree Master
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <style type="text/css">
       .button_delete {
            height: 38px;
            width: 100px;
            border-radius: 5px;
            padding-left: 26px;
            background-color: #d7393b;
            color: white;

        }
       .button_all {
            height: 38px;
            width: 100px;
            border-radius: 5px;
            padding-left: 15px;
            background-color: #368445;
            color: white;

        }
       .button_edit {
            height: 38px;
            width: 100px;
            border-radius: 5px;
            padding-left: 35px;
            background-color: #368445;
            color: white;
       }
       .button_course {
            height: 38px;
            width: 100px;
            border-radius: 5px;
            padding-left: 10px;
            background-color: #368445;
            color: white;
       }
        
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContainer" runat="server">
     <div class="well" style="margin-top:20px;">
        <div>
            <div class="PageTitle">
                <label>Program Tree Setup</label>
            </div>

            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <div class="Message-Area" style="height: 36px;">
                        <asp:Label ID="Label3" runat="server" Text="Message : " Font-Bold="true"></asp:Label>
                        <asp:label ID="lblMsg" runat="server" ForeColor="Red" ></asp:label>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>

            <br />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="Message-Area" >
                    <table style=" width:944px; ">
                        <tr>
                            <td style="width:10%;" 
                                align="left">
                                &nbsp; Program :
                            </td>
                            <td style=" class="style7">
                                <asp:DropDownList ID="ddlPrograms" AutoPostBack="true" runat="server" Width="98%" 
                                    onselectedindexchanged="ddlPrograms_SelectedIndexChanged" TabIndex="1">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="width:10%;" 
                                align="left">
                                &nbsp; Tree :
                            </td>
                            <td style="width:30%;">
                                <asp:DropDownList ID="ddlTree" AutoPostBack="true" runat="server" Width="98%" 
                                    onselectedindexchanged="ddlTree_SelectedIndexChanged" Enabled="False" 
                                    TabIndex="2">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="border: 1px solid Gray; vertical-align:middle; height: 39px;" 
                                colspan="2">
                                <asp:Panel ID="pnlUpperCont" runat="server" Width="100%" Height="98%" 
                                    HorizontalAlign="Left">
                                    <table style="width: 100%;">
                                        <tr>
                                            <td>
                                                <asp:Button ID="butAddRoot" runat="server" Text="Add Root" onclick="butAddRoot_Click"
                                                    ToolTip="Click this button to add the root node of a tree." AlternateText="Add Root" Class="button_all"
                                                    TabIndex="3"/>
                                                <%--<asp:ImageButton ID="butAddRoot" runat="server" onclick="butAddRoot_Click"
                                                    ToolTip="Click this button to add the root node of a tree." AlternateText="Add Root" 
                                                     TabIndex="3" />--%>
                                               <%-- ImageUrl="~/ButtonImages/AddRootS.jpg"--%>
                                            </td>
                                            <td>
                                                <asp:Button ID="butAddNode" runat="server" Text="Add Node" onclick="butAddNode_Click" Class="button_all"
                                                    ToolTip="Click this button to add a node under a selected root or node." AlternateText="Add Node"
                                                    TabIndex="4"/>
                                                <%--<asp:ImageButton ID="butAddNode" runat="server" onclick="butAddNode_Click" 
                                                    ToolTip="Add Node" AlternateText="Click this button to add a node under a selected root or node." 
                                                    ImageUrl="~/ButtonImages/AddNodeS.jpg" TabIndex="4" />--%>
                                            </td>
                                            <td>
                                                <asp:Button ID="butAddSet" runat="server" Text="Add Set" onclick="butAddSet_Click" Class="button_all"
                                                    ToolTip="Click this button to add a virtual node set detail under a selected node." AlternateText="Add Set"
                                                    TabIndex="5"/>
                                                <%--<asp:ImageButton ID="butAddSet" runat="server" onclick="butAddSet_Click" 
                                                    ToolTip="Add Set" AlternateText="Click this button to add a virtual node set detail under a selected node." 
                                                    ImageUrl="~/ButtonImages/AddSetS.jpg" TabIndex="5" />--%>
                                            </td>
                                            <td>
                                                <asp:Button ID="butAddCourse" runat="server" Text="Add Course" onclick="butAddCourse_Click" Class="button_course"
                                                    ToolTip="Click this button to add a course under a selected node." AlternateText="Add Course"
                                                    TabIndex="6"/>
                                                <%--<asp:ImageButton ID="butAddCourse" runat="server" onclick="butAddCourse_Click" 
                                                    ToolTip="Add Course" AlternateText="Click this button to add a course under a selected node." 
                                                    ImageUrl="~/ButtonImages/AddCourseS.jpg" TabIndex="6" />--%>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnEdit" runat="server" Text="Edit" onclick="btnEdit_Click" Class="button_edit"
                                                    ToolTip="Click this button to edit a selected node or course. Sets are not editable." AlternateText="Add Course"
                                                    TabIndex="6"/>
                                                <%--<asp:ImageButton ID="btnEdit" runat="server" onclick="btnEdit_Click" 
                                                    ToolTip="Edit" 
                                                    AlternateText="Click this button to edit a selected node or course. Sets are not editable." 
                                                    ImageUrl="~/ButtonImages/EditS.jpg" TabIndex="7" />--%>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnDel" runat="server" Text="Delete" onclick="btnDel_Click" Class="button_delete" 
                                                    ToolTip="Click this button to delete a selected node, course, root or set." AlternateText="Add Course"
                                                    TabIndex="7"/>
                                                <%--<asp:ImageButton ID="btnDel" runat="server" onclick="btnDel_Click" 
                                                    ToolTip="Delete" AlternateText="Click this button to delete a selected node, course, root or set." 
                                                    ImageUrl="~/ButtonImages/DeleteS.jpg" TabIndex="8"/>--%>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                            <td style="width:50%;border-style:solid; border-color:Gray; border-width:1px; height: 39px;vertical-align:middle;" colspan="2">
                                <asp:Label ID="lblCaption" runat="server" Width="100%" Height="98%" 
                                    Font-Names="Verdana" Font-Size="Medium"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="border: 1px solid Gray;" colspan="2" class="style3" align = "left">
                                <asp:Panel ID="pnlTree" runat="server" Width="550" Height="99%" ScrollBars="Vertical">
                                    <asp:TreeView ID="tvwMaster" runat="server" ShowLines="True" 
                                        Height="98%" onselectednodechanged="tvwMaster_SelectedNodeChanged" 
                                        LineImagesFolder="~/TreeLineImages" TabIndex="9">
                                        <ParentNodeStyle ForeColor="#660066" />
                                        <HoverNodeStyle ForeColor="#FF6600" />
                                        <SelectedNodeStyle ForeColor="Lime" Font-Bold="True" Font-Underline="True"/>
                                        <RootNodeStyle ForeColor="#000066" />
                                        <LeafNodeStyle ForeColor="#006699" />
                                    </asp:TreeView>
                                </asp:Panel>
                            </td>
                            <td style="border: 1px solid Gray;" colspan="2" class="style3">
                                <dxtc:ASPxPageControl ID="pcTabControl" runat="server" ActiveTabIndex="0" 
                                                    Height="356px" Width="470px">
                                                    <TabPages>
                                                        <dxtc:TabPage Text="General Information" Name="GI">
                                                            <ActiveTabStyle HorizontalAlign="Center">
                                                            </ActiveTabStyle>
                                                            <TabStyle HorizontalAlign="Center">
                                                            </TabStyle>
                                                            <ContentCollection>
                                                                <dxw:ContentControl runat="server">
                                                                    <asp:Panel ID="pnlControl" runat="server" Height="99%" Visible="False" 
                                                                        HorizontalAlign="Left" ScrollBars="Vertical">
                                                                        <table style="width:99%;"><%--height: 421px;--%>
                                                                            <tr>
                                                                                <td>                                        
                                                                                     <asp:Panel ID="pnlReg" runat="server" Height="116%" Visible="False" 
                                                                                         style="margin-bottom: 6px" BorderStyle="None">
                                                                                        <table style="width:100%; height: 187px;">
                                                                                            <tr>
                                                                                                <td align="left" class="style12">
                                                                                                    <asp:RequiredFieldValidator ID="rfvNodeName" runat="server" 
                                                                                                        ControlToValidate="txtName" ErrorMessage="Node Name can not be empty" 
                                                                                                        ValidationGroup="ValidateTree">*</asp:RequiredFieldValidator>
                                                                                                    <asp:Label ID="lblName" runat="server" Text="Name" Visible="False"></asp:Label>
                                                                                                </td>
                                                                                                <td align="left" colspan="3">
                                                                                                    <asp:TextBox ID="txtName" runat="server" Width="200px" TabIndex="10" 
                                                                                                        MaxLength="150" Visible="False"></asp:TextBox>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td align="left" class="style12">
                                                                                                    <asp:Label ID="lblRequiredUnits" runat="server" Text="Required Credits" 
                                                                                                        Visible="False"></asp:Label>
                                                                                                </td>
                                                                                                <td align="left">
                                                                                                    <dxe:ASPxSpinEdit ID="spnRequUnits" runat="server" DecimalPlaces="2" 
                                                                                                        Height="21px" Increment="0.1" LargeIncrement="1" Number="0" Visible="False" 
                                                                                                        Width="100px" TabIndex="11">
                                                                                                        <SpinButtons ShowLargeIncrementButtons="True">
                                                                                                        </SpinButtons>
                                                                                                    </dxe:ASPxSpinEdit>
                                                                                                </td>
                                                                                                <td align="left">
                                                                                                    <asp:Label ID="lblPass" runat="server" Text="Passing GPA" Visible="False"></asp:Label>
                                                                                                </td>
                                                                                                <td align="left">
                                                                                                    <dxe:ASPxSpinEdit ID="spnPass" runat="server" Height="21px" Increment="0.1" 
                                                                                                        LargeIncrement="1" Number="0" Visible="False" Width="90px" TabIndex="12">
                                                                                                        <SpinButtons ShowLargeIncrementButtons="True">
                                                                                                        </SpinButtons>
                                                                                                    </dxe:ASPxSpinEdit>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td class="style12" align="left">
                                                                                                    <asp:Label ID="lblOperator" runat="server" Text="Select Operator" 
                                                                                                        Visible="False"></asp:Label>
                                                                                                </td>
                                                                                                <td colspan="3">
                                                                                                    <asp:DropDownList ID="ddlOperators" runat="server" AutoPostBack="true" 
                                                                                                        Width="100px" TabIndex="13" Visible="False">
                                                                                                    </asp:DropDownList>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td class="style12" align="left" colspan="4">
                                                                                                    <uctl:CourseSelector ID="ctlCourseSelect" runat="server" 
                                                                                                        DropDownListTabIndex="16" FindButtonTabIndex="15" TextCodeTabIndex="14" 
                                                                                                        Visible="False" />
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td align="left" class="style12">
                                                                                                    <asp:Label ID="lblPriority" runat="server" Text="Priority" Visible="False"></asp:Label>
                                                                                                </td>
                                                                                                <td colspan="3">
                                                                                                    <dxe:ASPxSpinEdit ID="spnPriority" runat="server" Height="21px" Number="0" 
                                                                                                        Visible="False" Width="100px" TabIndex="17">
                                                                                                    </dxe:ASPxSpinEdit>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td class="style12" align="left">
                                                                                                    <asp:Label ID="lblNode" runat="server" Text="Select Node" Visible="False"></asp:Label>
                                                                                                </td>
                                                                                                <td colspan="3">
                                                                                                    <asp:DropDownList ID="ddlNodes" runat="server" AutoPostBack="true" Width="270px" 
                                                                                                        TabIndex="18" Visible="False">
                                                                                                    </asp:DropDownList>
                                                                                                </td>
                                                                                            </tr>

                                                                                            <tr>
                                                                                                <td align="left" class="style12">
                                                                                                    <asp:Label ID="lblMinCOurse" runat="server" Text="Minimum Courses" 
                                                                                                        Visible="False"></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <dxe:ASPxSpinEdit ID="spnMinCourse" runat="server" Height="21px" Number="0" 
                                                                                                        Visible="False" Width="90px" TabIndex="19">
                                                                                                    </dxe:ASPxSpinEdit>
                                                                                                </td>
                                                                                                <td align="left">
                                                                                                    <asp:Label ID="lblMaxCoure" runat="server" Text="Maximum Courses" 
                                                                                                        Visible="False"></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <dxe:ASPxSpinEdit ID="spnMaxCourse" runat="server" Height="21px" Number="0" 
                                                                                                        Visible="False" Width="90px" TabIndex="20">
                                                                                                    </dxe:ASPxSpinEdit>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td align="left" class="style10">
                                                                                                    <asp:Label ID="lblMinCredits" runat="server" Text="Minimum Credits" 
                                                                                                        Visible="False"></asp:Label>
                                                                                                </td>
                                                                                                <td class="style11">
                                                                                                    <dxe:ASPxSpinEdit ID="spnMinCredits" runat="server" Height="21px" 
                                                                                                        Increment="0.1" LargeIncrement="1" Number="0" Visible="False" Width="90px" TabIndex="21">
                                                                                                        <SpinButtons ShowLargeIncrementButtons="True">
                                                                                                        </SpinButtons>
                                                                                                    </dxe:ASPxSpinEdit>
                                                                                                </td>
                                                                                                <td align="left" class="style11">
                                                                                                    <asp:Label ID="lblMaxCredits" runat="server" Text="Maximum Credits" 
                                                                                                        Visible="False"></asp:Label>
                                                                                                </td>
                                                                                                <td class="style11">
                                                                                                    <dxe:ASPxSpinEdit ID="spnMaxCreds" runat="server" Height="21px" Increment="0.1" 
                                                                                                        LargeIncrement="1" Number="0" Visible="False" Width="90px" TabIndex="22">
                                                                                                        <SpinButtons ShowLargeIncrementButtons="True">
                                                                                                        </SpinButtons>
                                                                                                    </dxe:ASPxSpinEdit>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td align="left" class="style10">
                                                                                                    <asp:CheckBox ID="chkIsBundle" runat="server" Text="Is Bundle" 
                                                                                                        Visible="False" TabIndex="23"/>
                                                                                                </td>
                                                                                                <td class="style11">
                                                                                                    <asp:CheckBox ID="chkIsActive" runat="server" Checked="True" Text="Is Active" 
                                                                                                        Visible="False" TabIndex="24"/>
                                                                                                </td>
                                                                                                <td class="style11" >
                                                                                                    <asp:CheckBox ID="chkIsAssoc" runat="server" Checked="false" Text="Is Accosiated" 
                                                                                                        Visible="False" TabIndex="25"/>
                                                                                                </td>
                                                                                               <td class="style11" >
                                                                                                    <asp:CheckBox ID="chkIsMajor" runat="server" Checked="false" Text="Is Major" 
                                                                                                        Visible="False" TabIndex="25"/>
                                                                                                </td>
                                                                                            </tr>

                                                                                        </table>
                                                                                     </asp:Panel>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Panel ID="pnlSet" runat="server" Height="99%" Visible="False">
                                                                                        <table style="width:100%; height: 231px;">
                                                                                            <tr>
                                                                                                <td class="style9" align="left">
                                                                                                    <asp:CompareValidator ID="cvNetNo" runat="server" ErrorMessage="Set No must be numeric" 
                                                                                                        Operator="DataTypeCheck" Type="Integer" ValidationGroup="ValidateTree" 
                                                                                                        ControlToValidate="txtVNodeSet">*</asp:CompareValidator>
                                                                                                    <asp:RequiredFieldValidator ID="rfvSetNo" runat="server" 
                                                                                                        ControlToValidate="txtVNodeSet" ErrorMessage="Set number can not be empty." 
                                                                                                        ValidationGroup="ValidateTree">*</asp:RequiredFieldValidator>
                                                                                                    <asp:Label ID="lblSet" runat="server" Text="Node Set No"></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:TextBox ID="txtVNodeSet" runat="server" Width="100px" 
                                                                                                        onkeypress="return isNumber(event)" TabIndex="25" MaxLength="10"></asp:TextBox>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:Label ID="lblRequiredUnitsSet" runat="server" Text="Required Credits"></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <dxe:ASPxSpinEdit ID="spnRequUnitsSet" runat="server" DecimalPlaces="2" 
                                                                                                        Height="21px" Increment="0.1" LargeIncrement="1" Number="0" Visible="False" 
                                                                                                        Width="100px" TabIndex="26">
                                                                                                        <SpinButtons ShowLargeIncrementButtons="True">
                                                                                                        </SpinButtons>
                                                                                                    </dxe:ASPxSpinEdit>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td class="style9" align="left">
                                                                                                    <asp:Label ID="lblSetOperator" runat="server" Text="Select Set Operator"></asp:Label>
                                                                                                </td>
                                                                                                <td colspan="3">
                                                                                                    <asp:DropDownList ID="ddlSetOptrs" runat="server" AutoPostBack="true" 
                                                                                                        Width="100px" TabIndex="27">
                                                                                                    </asp:DropDownList>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td align="left" colspan="4">
                                                                                                    <asp:CheckBox ID="chkStudSpec" runat="server" AutoPostBack="True" 
                                                                                                        OnCheckedChanged="chkStudSpec_CheckedChanged" TabIndex="28" 
                                                                                                        Text="Student Specific Major" />
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td align="center" class="style9">
                                                                                                    <asp:RadioButton ID="rbtSetCourse" runat="server" ForeColor="#000066" 
                                                                                                        Text="Operand Course" AutoPostBack="True" 
                                                                                                        oncheckedchanged="rbtSetCourse_CheckedChanged" TabIndex="29" />
                                                                                                </td>
                                                                                                <td align="center" colspan="3">
                                                                                                    <asp:RadioButton ID="rbtSetNode" runat="server" ForeColor="#000066" 
                                                                                                        Text="Operand Node" AutoPostBack="True" 
                                                                                                        oncheckedchanged="rbtSetNode_CheckedChanged" TabIndex="30" />
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td align="left" class="style9">
                                                                                                    <asp:Label ID="lblOpCour" runat="server" Text="Select Operand Course"></asp:Label>
                                                                                                </td>
                                                                                                <td colspan="3">
                                                                                                    <dxe:ASPxComboBox ID="ddlSetCrs" runat="server" AutoPostBack="True" 
                                                                                                        DropDownStyle="DropDown" EnableIncrementalFiltering="True" 
                                                                                                        OnSelectedIndexChanged="ddlSetCrs_SelectedIndexChanged" 
                                                                                                        ValueType="System.String" Width="270px" TabIndex="31">
                                                                                                    </dxe:ASPxComboBox>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td class="style9" align="left">
                                                                                                    <asp:Label ID="lblOpNode" runat="server" Text="Select Operand Node"></asp:Label>
                                                                                                </td>
                                                                                                <td colspan="3">
                                                                                                    <dxe:ASPxComboBox ID="ddlSetNode" runat="server" AutoPostBack="True" 
                                                                                                        DropDownStyle="DropDown" EnableIncrementalFiltering="True" Height="20px" 
                                                                                                        ValueType="System.String" Width="270px" TabIndex="32">
                                                                                                    </dxe:ASPxComboBox>
                                                                                                </td>
                                                                                            </tr>

                                                                                            <tr>
                                                                                                <td align="left" class="style9">
                                                                                                    <asp:Label ID="lblWild" runat="server" Text="Wild Card"></asp:Label>
                                                                                                </td>
                                                                                                <td colspan="3">
                                                                                                    <asp:TextBox ID="txtWildCard" runat="server" Width="100px" TabIndex="33"></asp:TextBox>
                                                                                                </td>
                                                                                            </tr>

                                                                                        </table>
                                                                                     </asp:Panel>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:Panel>
                                                                </dxw:ContentControl>
                                                            </ContentCollection>
                                                        </dxtc:TabPage>
                                                        <dxtc:TabPage Text="Set Prerequisites" Name="PREREQ">
                                                            <ContentCollection>
                                                                <dxw:ContentControl runat="server">
                                                                    <asp:Panel ID="pnlPREREQ" runat="server" Height="98%" ScrollBars="Vertical" Enabled="true">
                                                                        <table style="width:100%;"> 
                                                                            <tr>
                                                                                <td colspan = "2">
                                                                                    <table style="width:100%;">
                                                                                        <tr>
                                                                                            <td style="width:25%;">
                                                                                                <asp:Label ID="Label1" runat="server" Text="Prerequisit Name " Width="100%"></asp:Label>                                                                                        
                                                                                            </td>
                                                                                            <td style="width:25%;">                                                                                            
                                                                                                <dxe:ASPxComboBox ID="cboPreReqName" runat="server" AutoPostBack="True" 
                                                                                                    DropDownStyle="DropDown" EnableIncrementalFiltering="True" 
                                                                                                    ValueType="System.String" Width="100%" 
                                                                                                    OnSelectedIndexChanged="cboPreReqName_SelectedIndexChanged" TabIndex="34">
                                                                                                </dxe:ASPxComboBox>                                                                                            
                                                                                            </td>
                                                                                            <td style="width:10%;" align = "right">
                                                                                                <asp:Label ID="Label2" runat="server" Text="Name " Width="100%"></asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                    
                                                                                                <asp:TextBox ID="txtPreReqName" runat="server" Width="89%" TabIndex="35"></asp:TextBox>
                                                                                    
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>                                                                                                                                       
                                                                            <tr>
                                                                                <td style="width:30%;">                                                                         
                                                                                    <asp:Button ID="btnPreReqCourse" runat="server" Text="Set Course" 
                                                                                        OnClick="btnPreReqCourse_Click" TabIndex="36"/>
                                                                                </td>
                                                                                <td style="width:70%;"> 
                                                                                    <asp:Button ID="btnPreReqNode" runat="server" Text="Set Node" 
                                                                                        OnClick="btnPreReqNode_Click" TabIndex="37"/>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan="2"></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td colspan = "2">
                                                                                    <asp:Panel ID="pnlPreReqArea" runat="server">
                                                                                        <table style="width:100%;">                                                                                    
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <dxwgv:ASPxGridView ID="gdvPreReq" runat="server" 
                                                                                                        AutoGenerateColumns="False" Width="408px" KeyFieldName="Id" 
                                                                                                        OnCellEditorInitialize="gdvPreReq_CellEditorInitialize" TabIndex="38"
                                                                                                        OnRowInserted="gdvPreReq_RowInserted" OnRowInserting="gdvPreReq_RowInserting" 
                                                                                                        OnRowUpdated="gdvPreReq_RowUpdated" OnRowUpdating="gdvPreReq_RowUpdating" 
                                                                                                        OnCancelRowEditing="gdvPreReq_CancelRowEditing" 
                                                                                                        OnInitNewRow="gdvPreReq_InitNewRow" 
                                                                                                        OnStartRowEditing="gdvPreReq_StartRowEditing" 
                                                                                                        OnRowDeleted="gdvPreReq_RowDeleted" OnRowDeleting="gdvPreReq_RowDeleting" OnRowValidating="gdvPreReq_RowValidating"                                                                                                 
                                                                                                        >
                                                                                                        <Columns>
                                                                                                            <dxwgv:GridViewCommandColumn VisibleIndex="0"  ButtonType="Button" CellStyle-HorizontalAlign="Left" CellStyle-Wrap="True">
                                                                                                                <EditButton Visible="True">
                                                                                                                </EditButton>
                                                                                                                <NewButton Text="Add" Visible="True">
                                                                                                                </NewButton> 
                                                                                                                <DeleteButton Visible="True">
                                                                                                                </DeleteButton>
                                                                                                                <ClearFilterButton Visible="True">
                                                                                                                </ClearFilterButton>                                                                                                        
                                                                                                                <CellStyle HorizontalAlign="Left" Wrap="True">
                                                                                                                </CellStyle>
                                                                                                            </dxwgv:GridViewCommandColumn>
                                                                                                            <dxwgv:GridViewDataComboBoxColumn Caption="Course" FieldName="NodeCourse.ChildCourse.FullCodeAndCourse" 
                                                                                                            VisibleIndex="1">                                                                        
                                                                                                                <PropertiesComboBox ValueType="System.Int32"  EnableSynchronization="False"
                                                                                                                    EnableIncrementalFiltering="True"></PropertiesComboBox>
                                                                                                            </dxwgv:GridViewDataComboBoxColumn>
                                                                                                            <dxwgv:GridViewDataComboBoxColumn Caption="Node" FieldName="Node.Name" 
                                                                                                            VisibleIndex="2">                                                                        
                                                                                                                <PropertiesComboBox ValueType="System.Int32"  EnableSynchronization="False"
                                                                                                                    EnableIncrementalFiltering="True" ></PropertiesComboBox>
                                                                                                            </dxwgv:GridViewDataComboBoxColumn>
                                                                                                            <dxwgv:GridViewDataSpinEditColumn Caption="Required Units" 
                                                                                                                VisibleIndex="3" FieldName="ReqCredits" PropertiesSpinEdit-NullText="0.00" PropertiesSpinEdit-Increment=".25" PropertiesSpinEdit-LargeIncrement="1">
                                                                                                                <PropertiesSpinEdit DisplayFormatString="g" SpinButtons-ShowLargeIncrementButtons="true">
        <SpinButtons ShowLargeIncrementButtons="True"></SpinButtons>
        </PropertiesSpinEdit>
                                                                                                            </dxwgv:GridViewDataSpinEditColumn>
                                                                                                            <dxwgv:GridViewDataComboBoxColumn Caption="Operator" FieldName="Operator.Name" 
                                                                                                            VisibleIndex="4">                                    
                                                                                                                <PropertiesComboBox ValueType="System.String" EnableSynchronization="False" EnableIncrementalFiltering="True" ></PropertiesComboBox>
                                                                                                            </dxwgv:GridViewDataComboBoxColumn>
                                                                                                        </Columns>
                                                                                                        <SettingsBehavior ConfirmDelete="True" />
                                                                                                        <SettingsPager Mode="ShowAllRecords">
                                                                                                        </SettingsPager>
                                                                                                        <SettingsEditing Mode="Inline" />
                                                                                                        <Settings ShowVerticalScrollBar="true" />
                                                                                                    </dxwgv:ASPxGridView>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <table style="width:100%;">
                                                                                                        <tr>
                                                                                                            <td style="width:60%;">
                                                                                                                <asp:Label ID="lblOccurance" runat="server"></asp:Label>
                                                                                                            </td>
                                                                                                            <td style="width:20%;">
                                                                                                        
                                                                                                                <dxe:ASPxSpinEdit ID="speOperatorcourseOccurance" runat="server" AutoPostBack="True" 
                                                                                                                    Height="21px" Number="0" Width = "100%" TabIndex="39">
                                                                                                                </dxe:ASPxSpinEdit>
                                                                                                        
                                                                                                            </td>
                                                                                                            <td style="width:20%;">                                                                                    
                                                                                                                <dxe:ASPxSpinEdit ID="speOperatorNodeOccurance" runat="server" AutoPostBack="True" 
                                                                                                                    Height="21px" Number="0" Width="100%" TabIndex="40">
                                                                                                                </dxe:ASPxSpinEdit>
                                                                                                            </td>
                                                                                                        </tr>                                                                            
                                                                                                    </table>                                                                        
                                                                                                   </td>                                                                    
                                                                                               </tr>
                                                                                        </table>
                                                                                    </asp:Panel> 
                                                                                </td>
                                                                            </tr>                                                                   
                                                                        </table>
                                                                    </asp:Panel>
                                                                </dxw:ContentControl>
                                                            </ContentCollection>
                                                        </dxtc:TabPage>                                                                                                
                                                    </TabPages>
                                                </dxtc:ASPxPageControl>
                            </td>
                        </tr> 
                        <tr>
                        <td colspan = "4">
                        <table style="width:100%;border: 1px solid Gray;">
                            <tr>
                                <td>
                                    <asp:Panel ID="pnlSavCan" runat="server" Height="99%">
                                        <table style="width:100%;">
                                            <tr>
                                                <td style="width:50%;"></td>
                                                <td align =  "center">

                                                    <asp:ImageButton ID="butSave" runat="server" 
                                                        AlternateText="Click this button to save the data on the interface to database" 
                                                        ImageUrl="~/ButtonImages/Save.jpg" onclick="butSave_Click" TabIndex="41" 
                                                        ValidationGroup="ValidateTree" style="height: 20px" />
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="butCancel" runat="server" AlternateText="Cancel" 
                                                        ImageUrl="~/ButtonImages/Cancel.jpg" onclick="butCancel_Click" 
                                                        TabIndex="42" style="height: 20px" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                        </td>
                        </tr>
                        <tr>
                            <td style="border: 1px solid Gray; vertical-align:middle;" colspan="4" class="style4">
                                <asp:ValidationSummary ID="vsTreeMaster" runat="server" ShowMessageBox="True" 
                                    ShowSummary="False" ValidationGroup="ValidateTree" />
                            </td>
                        </tr>
                    </table>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
