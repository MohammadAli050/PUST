<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true"
    CodeBehind="ExamTemplateManage.aspx.cs" Inherits="EMS.Module.result.ExamTemplateManage" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content3" ContentPlaceHolderID="Title" runat="Server">
    Exam Template
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">

    <script src="https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.3.min.js"></script>
    <link href="../../CSS/select2.min.css" rel="stylesheet" />
    <script src="../../JavaScript/select2.full.min.js"></script>

    <style type="text/css">
        .modalBackground {
            background-color: #2a2d2a;
            filter: alpha(opacity=80);
            opacity: 0.8;
            z-index: 10000;
        }

        .modalPopup {
            background-color: #FFFFFF;
            border-width: 3px;
            border-style: solid;
            border-color: black;
            padding-top: 10px;
            padding-left: 10px;
            width: 300px;
            height: 140px;
        }

        .font {
            font-size: 12px;
        }

        .cursor {
            cursor: pointer;
        }

        .auto-style4 {
            width: 212px;
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

        function initdropdown() {
            $('#ctl00_MainContainer_ddlHeldIn').select2({
                allowClear: true
                //,
                //placeholder: { id: '0', text: 'Select' }
            });
        }

        function jsShowHideProgress() {
            setTimeout(function () {
                document.getElementById('divProgress').style.display = 'block';
            }, 200);
            deleteCookie();

            var timeInterval = 500; // milliseconds (checks the cookie for every half second )

            var loop = setInterval(function () {
                if (IsCookieValid()) {
                    document.getElementById('divProgress').style.display =
                    'none'; clearInterval(loop)
                }

            }, timeInterval);
        }
        // cookies
        function deleteCookie() {
            var cook = getCookie('ExcelDownloadFlag');
            if (cook != "") {
                document.cookie = "ExcelDownloadFlag=;Path=/; expires=Thu, 01 Jan 1970 00:00:00 UTC";
            }
        }

        function IsCookieValid() {
            var cook = getCookie('ExcelDownloadFlag');
            return cook != '';
        }

        function getCookie(cname) {
            var name = cname + "=";
            var ca = document.cookie.split(';');
            for (var i = 0; i < ca.length; i++) {
                var c = ca[i];
                while (c.charAt(0) == ' ') {
                    c = c.substring(1);
                }
                if (c.indexOf(name) == 0) {
                    return c.substring(name.length, c.length);
                }
            }
            return "";
        }


    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContainer" runat="Server">

    <div class="row">
        <div class="col-sm-12" style="font-size: 12pt; margin-top: 10pt;">
            <label><b style="color: black; font-size: 26px">Exam Template Item</b></label>
        </div>
    </div>
    <div id="divProgress" style="display: none; z-index: 100000000; position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%);">
        <asp:Image ID="LoadingImage" runat="server" ImageUrl="~/Images/Img/Waiting.gif" Height="300px" Width="300px" />
        <div>
            <asp:Label ID="Label1" runat="server" Text="Processing your request.........." ForeColor="Red" Font-Bold="true" Font-Italic="true" Font-Size="30px"></asp:Label>
        </div>
    </div>

    <div>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <div class="Message-Area">
                    <label class="msgTitle">Message: </label>
                    <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label><br />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="card" style="margin-top: 5px">
            <div class="card-body">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>

                        <div class="row col-lg-2 col-md-2 col-sm-2" style="">
                            <asp:Button ID="btnAdd" runat="server" Text="Add New Template" CssClass="btn-info w-100" />
                        </div>

                        <div style="margin-top: 10px">
                            <asp:GridView ID="GvExamTemplate" runat="server" AllowSorting="True" CssClass="table-bordered"
                                AutoGenerateColumns="False" ShowFooter="True" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None"
                                OnRowCommand="GvExamTemplate_RowCommand" OnPageIndexChanging="GvExamTemplate_PageIndexChanging">
                                <HeaderStyle BackColor="#4285f4" ForeColor="White" Height="10px" Font-Bold="True" />
                                <FooterStyle BackColor="#4285f4" ForeColor="White" Height="10px" Font-Bold="True" />
                                <AlternatingRowStyle BackColor="White" />
                                <RowStyle Height="25" />

                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <Columns>
                                    <asp:BoundField DataField="ExamTemplateId" Visible="false" HeaderText="Id">
                                        <ItemStyle HorizontalAlign="Center" />
                                        <HeaderStyle Width="250px" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="ExamTemplateName" HeaderText="Template Name">
                                        <ItemStyle HorizontalAlign="Left" />
                                        <HeaderStyle Width="350px" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="ExamTemplateMarks" HeaderText="Template Mark">
                                        <HeaderStyle Width="120px" />
                                    </asp:BoundField>

                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <div style="text-align: center">
                                                <asp:LinkButton ID="ExamTemplateEditButton" CommandName="ExamTemplateEdit" Text="Edit" CssClass="btn-info btn-sm" ToolTip="Edit Exam Template" CommandArgument='<%# Bind("ExamTemplateId") %>' runat="server"></asp:LinkButton>

                                            </div>
                                        </ItemTemplate>
                                        <ItemStyle Width="5%" />

                                    </asp:TemplateField>

                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <div style="text-align: center">

                                                <asp:LinkButton ID="ExamTemplateDeleteButton" CommandName="ExamTemplateDelete" Text="Delete" CssClass="btn-danger btn-sm" ToolTip="Delete Exam Template" CommandArgument='<%# Bind("ExamTemplateId") %>' runat="server"></asp:LinkButton>
                                            </div>

                                        </ItemTemplate>
                                        <ItemStyle Width="5%" />
                                    </asp:TemplateField>
                                </Columns>
                                <PagerStyle BackColor="#4285f4" ForeColor="White" HorizontalAlign="Center" />
                                <RowStyle Height="10px" VerticalAlign="Middle" HorizontalAlign="Left" BackColor="#E3EAEB" />
                                <EditRowStyle BackColor="#7C6F57" />
                                <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                <SortedAscendingCellStyle BackColor="#F8FAFA" />
                                <SortedAscendingHeaderStyle BackColor="#246B61" />
                                <SortedDescendingCellStyle BackColor="#D4DFE1" />
                                <SortedDescendingHeaderStyle BackColor="#15524A" />
                            </asp:GridView>
                        </div>

                        <cc1:ModalPopupExtender ID="mp1" runat="server" PopupControlID="Panel1" TargetControlID="btnAdd" BackgroundCssClass="modalBackground">
                        </cc1:ModalPopupExtender>

                        <asp:Panel ID="Panel1" runat="server" CssClass="modalPopup" Width="500px" Height="200px" align="center" Style="display: none">

                            <div class="row">
                                <div class="col-lg-3 col-md-3 col-sm-3">
                                </div>
                                <div class="col-lg-6 col-md-6 col-sm-6">
                                    <b>Exam Template</b>
                                </div>
                                <div class="col-lg-3 col-md-3 col-sm-3">
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-lg-7 col-md-7 col-sm-7" style="text-align: left">
                                    <asp:Label ID="lblExamTemplate" runat="server" Text="Exam Template Name"></asp:Label>
                                    <asp:TextBox ID="txtExamTemplateName" Placeholder="Exam Template Name" runat="server" CssClass="form-control" Font-Bold="true" Text=""></asp:TextBox>
                                </div>
                                <div class="col-lg-5 col-md-5 col-sm-5" style="text-align: left">
                                    <asp:Label ID="lblExamTemplateMark" runat="server" CssClass="control-newlabel" Text="Exam Template Mark"></asp:Label>
                                    <asp:TextBox ID="txtExamTemplateMark" Placeholder="Exam Template Mark" runat="server" CssClass="form-control" Font-Bold="true"></asp:TextBox>
                                </div>
                            </div>

                            <div class="row" style="margin-top: 5px">
                                <div class="col-lg-1 col-md-1 col-sm-1">
                                </div>
                                <div class="col-lg-3 col-md-3 col-sm-3">
                                    <asp:Button ID="btnSave" runat="server" CssClass="btn-info" Width="100%" Text="Save" Visible="true" OnClick="btnSave_Click" />
                                </div>
                                <div class="col-lg-3 col-md-3 col-sm-3">
                                    <asp:Button ID="btnUpdate" runat="server" CssClass="btn-info" Width="100%" Text="Update" Visible="false" OnClick="btnUpdate_Click" />
                                </div>
                                <div class="col-lg-3 col-md-3 col-sm-3">
                                    <asp:Button ID="btnClose" runat="server" CssClass="btn-danger" Width="100%" Text="Close" OnClick="btnClose_Click" />
                                </div>
                                <div class="col-lg-2 col-md-2 col-sm-2">
                                </div>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
        </div>

    </div>

    <asp:HiddenField ID="HiddenExamTemplateId" runat="server" />
</asp:Content>
