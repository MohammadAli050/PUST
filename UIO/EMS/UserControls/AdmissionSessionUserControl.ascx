<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdmissionSessionUserControl.ascx.cs" Inherits="EMS.UserControls.AdmissionSessionUserControl" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div>           
            <div> 
                <asp:DropDownList ID="ddlSession" Width="100%" class="form-control" runat="server" OnSelectedIndexChanged="ddlSession_SelectedIndexChanged" AutoPostBack="true">                    
                </asp:DropDownList>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>