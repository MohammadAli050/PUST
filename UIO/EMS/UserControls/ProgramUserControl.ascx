﻿<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControl_ProgramUserControl" Codebehind="ProgramUserControl.ascx.cs" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div >            
            <div> 
                <asp:DropDownList ID="ddlProgram" Width="100%" CssClass="form-control" runat="server" OnSelectedIndexChanged="ddlProgram_SelectedIndexChanged" AutoPostBack="true">                    
                </asp:DropDownList>
            </div>
        </div>       
    </ContentTemplate>
</asp:UpdatePanel>
