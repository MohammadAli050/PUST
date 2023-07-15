<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true" CodeBehind="RPTExamMarkEntry.aspx.cs" Inherits="EMS.Module.result.Report.RPTExamMarkEntry" %>


<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Examiner Mark Submit
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContainer" runat="server">


    <div class="PageTitle">
        <label>Examiner Mark Submit</label>
    </div>
    
         <div class="Message-Area">
           
        </div>
        <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pnlMessage" runat="server" Visible="true" CssClass="msgPanel">
                    <asp:Label ID="Label1" runat="server" Text="Message : "></asp:Label>
                    <asp:Label ID="lblMsg" runat="server"></asp:Label>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>--%>
   

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <rsweb:ReportViewer ID="ExamMarkEntryRV" Visible="True" runat="server" Font-Names="Verdana" Font-Size="8pt" Height="30%" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" asynrendering="false" SizeToReportContent="true" BackColor="Wheat" CssClass="center" BorderColor="WhiteSmoke" BorderStyle="Solid" BorderWidth="1">
                </rsweb:ReportViewer>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>
