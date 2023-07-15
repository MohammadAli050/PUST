<%@ Page Title="Image Migration" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.Master" AutoEventWireup="true" CodeBehind="StudentImageMigration.aspx.cs" Inherits="EMS.StudentImageMigration" %>



<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">

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

    <div class="row">
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <div class="col-lg-12 col-md-12 col-sm-12">
                    <div class="col-sm-12" style="font-size: 12pt; margin-top: 4pt; margin-bottom: 4pt;">
                        <label><b style="color: #FF5722; font-size: 26px">Image Migration</b></label>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>



    <div class="col-sm-12">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="Message-Area">
                    <asp:Label ID="lblMsg" ForeColor="Red" runat="server"></asp:Label>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div class="col-sm-12">
        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
                <div id="divProgress" style="display: none; z-index: 1000000; position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%);">
                    <asp:Image ID="LoadingImage" runat="server" ImageUrl="~/Images/Img/Waiting.gif" Height="300px" Width="300px" />
                    <br />
                    <b style="color: red">Processing your request .......</b>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <asp:UpdatePanel ID="UpdatePanel02" runat="server">
        <ContentTemplate>

            <div class="card">
                <div class="card-body">
                    <div class="row">
                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <asp:Label ID="Label2" runat="server" Text="Number Of Image " Font-Bold="true" Font-Size="Large"></asp:Label>
                            <asp:DropDownList ID="ddlNumber" runat="server" CssClass="form-control" Height="35px" Style="border-radius: 8px"></asp:DropDownList>

                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:LinkButton ID="btnMigrate" runat="server" CssClass="btn btn-info" Width="100%" OnClick="btnMigrate_Click" Style="margin-top: 25px">
                                        <strong>&nbsp;Convert Image Name With PersonId</strong>
                                
                            </asp:LinkButton>
                        </div>

                    </div>


                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>




    <ajaxToolkit:UpdatePanelAnimationExtender
        ID="UpdatePanelAnimationExtender1"
        TargetControlID="UpdatePanel02"
        runat="server">
        <Animations>
                <OnUpdating>
                    <Parallel duration="0">
                        <ScriptAction Script="InProgress();" />
                        <EnableAction AnimationTarget="lnkLoadAssign" Enabled="false" />                  
                    </Parallel>
                </OnUpdating>
                <OnUpdated>
                    <Parallel duration="0">
                        <ScriptAction Script="onComplete();" />
                        <EnableAction   AnimationTarget="lnkLoadAssign" Enabled="true" />
                    </Parallel>
                </OnUpdated>
        </Animations>
    </ajaxToolkit:UpdatePanelAnimationExtender>

</asp:Content>
