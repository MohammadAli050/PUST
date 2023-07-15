<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DepartmentUserControl.ascx.cs" Inherits="EMS.UserControls.DepartmentUserControl" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div >            
            <div> 
                <asp:DropDownList ID="ddlDepartment" Width="100%" CssClass="form-control" runat="server" OnSelectedIndexChanged="ddlDepartment_SelectedIndexChanged" AutoPostBack="true">                    
                </asp:DropDownList>
            </div>
        </div>       
    </ContentTemplate>
</asp:UpdatePanel>