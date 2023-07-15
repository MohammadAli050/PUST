<%@ Page Title="" Language="C#"
    MasterPageFile="~/MasterPage/Common/CommonMasterPage.master"
    AutoEventWireup="true"
    CodeBehind="ViewPDF.aspx.cs" Inherits="EMS.Module.ViewPDF" %>



<%@ Register Src="~/UserControls/ProgramUserControl.ascx" TagPrefix="uc1" TagName="ProgramUserControl" %>


<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Reports
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContainer" runat="server">
    <asp:Label Visible="false" ID="lblPath" runat="server" Text=""></asp:Label>
    <div>
        <asp:Label ID="Label1" runat="server" Text="No Data Found ! "></asp:Label>
        <asp:Button ID="btnDownload" runat="server" Text="Download" OnClick="btnDownload_Click" Visible="false" />
    </div>
</asp:Content>



