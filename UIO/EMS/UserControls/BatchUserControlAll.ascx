﻿<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControl_BatchUserControlAll" Codebehind="BatchUserControlAll.ascx.cs" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div>           
            <div> 
                <asp:DropDownList ID="ddlBatch" Width="100px" CssClass="" runat="server" OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged" AutoPostBack="true">                    
                </asp:DropDownList>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>