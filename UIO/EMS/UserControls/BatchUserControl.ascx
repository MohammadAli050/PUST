﻿<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControl_BatchUserControl" Codebehind="BatchUserControl.ascx.cs" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div>           
            <div> 
                <asp:DropDownList ID="ddlBatch" Width="120" class="margin-zero dropDownList" runat="server" OnSelectedIndexChanged="ddlBatch_SelectedIndexChanged" AutoPostBack="true">                    
                </asp:DropDownList>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
