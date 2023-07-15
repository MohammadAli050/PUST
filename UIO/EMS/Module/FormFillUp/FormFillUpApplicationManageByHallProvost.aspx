<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true" CodeBehind="FormFillUpApplicationManageByHallProvost.aspx.cs" Inherits="EMS.Module.FormFillUp.FormFillUpApplicationManageByHallProvost" %>



<%@ Register Src="~/UserControls/ProgramUserControl.ascx" TagPrefix="uc1" TagName="ProgramUserControl" %>
<%@ Register Src="~/UserControls/DepartmentUserControl.ascx" TagPrefix="uc1" TagName="DepartmentUserControl" %>



<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Forward To Academic Section</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">


    <style type="text/css">
        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=80);
            opacity: 0.8;
            z-index: 10000;
        }

        .dropdown {
            width: 266px;
        }



        .select2-results__option {
            line-height: 20px !important;
            height: 34px !important;
        }

        .select2-container {
            width: 306px !important;
        }

        .blink {
            animation: blinker 0.6s linear infinite;
            color: #1c87c9;
            font-size: 30px;
            font-weight: bold;
            font-family: sans-serif;
        }

        @keyframes blinker {
            50% {
                opacity: 0;
            }
        }

        #ctl00_MainContainer_ucDepartment_ddlDepartment,#ctl00_MainContainer_ddlExamYear,#ctl00_MainContainer_ddlProgram, #ctl00_MainContainer_ucProgram_ddlProgram, #ctl00_MainContainer_ddlStatus, #ctl00_MainContainer_ddlCourse,
        #ctl00_MainContainer_btnUpdateInfo {
            height: 40px !important;
            font-size: 20px;
        }

        span.select2-selection.select2-selection--single, #ctl00_MainContainer_ddlInternalQSetterName, #ctl00_MainContainer_ddlThirdExaminerName, #ctl00_MainContainer_ddlExternalQSetterName, #ctl00_MainContainer_ddlInternalScriptExName, #ctl00_MainContainer_ddlExternalScriptExName {
            height: 40px;
        }

        span.select2.select2-container.select2-container--default {
            width: 100% !important;
        }

        .sweet-alert {
            z-index: 10000000 !important;
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


        function Search(x) {


            input = document.getElementById("myInput");
            filter = input.value.toUpperCase();
            table = document.getElementById("ctl00_MainContainer_gvApplicationList");
            tr = table.getElementsByTagName("tr");


            for (i = 0; i < tr.length; i++) {
                if (tr) {
                    txtValue = tr[i].innerText;
                    if (txtValue.toUpperCase().indexOf(filter) > -1) {
                        tr[i].style.display = "";
                    } else {
                        tr[i].style.display = "none";
                    }
                    tr[0].style.display = "";

                }

            }
        };



    </script>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContainer" runat="server">

    <asp:UpdatePanel runat="server" ID="UpdatePanel1">
        <ContentTemplate>
            <div class="row">
                <div class="col-lg-8 col-md-8 col-sm-8" style="font-size: 12pt; margin-top: 10pt;">
                    <label><b style="color: black; font-size: 26px">Forward To Academic Section</b></label>
                </div>
                <div class="col-lg-4 col-md-4 col-sm-4" runat="server" id="divSearch">
                    <br />
                    <input type="text" class="form-control" id="myInput" onkeyup="javascript: Search( this.value );"
                        placeholder="Search ..........." title="Type something" style="color: red; height: 34px; width: 100%; border-top-left-radius: 0px; border-bottom-left-radius: 0px;">
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="divProgress" style="display: none; z-index: 100000000; position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%);">
        <asp:Image ID="LoadingImage" runat="server" ImageUrl="~/Images/Img/Waiting.gif" Height="300px" Width="300px" />
        <div>
            <asp:Label ID="Label1" runat="server" Text="Processing your request.........." ForeColor="Red" Font-Bold="true" Font-Italic="true" Font-Size="30px"></asp:Label>
        </div>
    </div>

    <hr />

    <asp:UpdatePanel runat="server" ID="UpdatePanel02">
        <ContentTemplate>
            <div class="card">
                <div class="card-body" runat="server" id="divDD">

                    <div class="row">
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <b>Choose Exam Year</b>
                            <asp:DropDownList ID="ddlExamYear" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlExamYear_SelectedIndexChanged"></asp:DropDownList>
                        </div>

                         <div class="col-lg-4 col-md-4 col-sm-4">
                            <script type="text/javascript">
                                Sys.Application.add_load(initdropdown);
                            </script>
                            <b>Choose Held In<span style="color: red;">*</span></b>
                            <br />
                            <asp:DropDownList ID="ddlHeldIn" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlHeldIn_SelectedIndexChanged"></asp:DropDownList>
                        </div>

                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <b>Choose Program</b>
                            <asp:DropDownList ID="ddlProgram" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlProgram_SelectedIndexChanged"></asp:DropDownList>
                        </div>

                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <b>Application Status </b>
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                                <asp:ListItem Value="0" Text="Pending" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="2" Text="Forwarded"></asp:ListItem>
                            </asp:DropDownList>
                        </div>

                    </div>


               <%--     <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <b>Choose Department</b>
                            <br />
                            <uc1:DepartmentUserControl runat="server" ID="ucDepartment" OnDepartmentSelectedIndexChanged="ucDepartment_DepartmentSelectedIndexChanged" />
                        </div>
                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <b>Choose Program <span style="color: red">*</span></b>
                            <br />
                            <uc1:ProgramUserControl runat="server" ID="ucProgram" OnProgramSelectedIndexChanged="ucProgram_ProgramSelectedIndexChanged" />
                        </div>
                       
                    </div>--%>

                </div>
            </div>


            <div class="card" style="margin-top: 10px">
                        <div class="card-body" style="text-align: center">
                            <asp:Label runat="server" ID="lblDeadLine" Text="" Font-Bold="true" ForeColor="Blue" Font-Size="15px"></asp:Label>
                        </div>
                    </div>

            <div class="card" style="margin-top: 10px">
                <div class="card-body">

                    <div class="row" style="margin-top: 5px" runat="server" id="divbtnPanel">
                        <div class="col-lg-6 col-md-6 col-sm-6">
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:LinkButton ID="lnkApprove" runat="server" Width="100%" type="button" CssClass="btn-info btn-lg" Style="text-align: center" Font-Size="Small" OnClick="lnkApprove_Click">
                                                                    <b>Forward To Academic Section </b>
                            </asp:LinkButton>
                        </div>
                    </div>
                    <br />

                    <asp:GridView runat="server" ID="gvApplicationList" AutoGenerateColumns="False" AllowPaging="false" Font-Size="16px"
                        PageSize="20" CssClass="table-bordered" ShowFooter="true"
                        Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None"
                        PagerSettings-Mode="NumericFirstLast"
                        PagerStyle-Font-Bold="true" PagerStyle-Font-Size="Larger"
                        ShowHeader="true">
                        <HeaderStyle BackColor="#4285f4" ForeColor="White" Height="10px" Font-Bold="True" />
                        <FooterStyle BackColor="#4285f4" ForeColor="White" Height="10px" Font-Bold="True" />
                        <AlternatingRowStyle BackColor="White" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField HeaderText="SL">
                                <ItemTemplate><b><%# Container.DataItemIndex + 1 %></b></ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Program" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblProgram" Text='<%#Eval("ShortName") %>'>></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Student ID" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblRoll" Text='<%#Eval("Roll") %>'>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>


                            <asp:TemplateField HeaderText="Student Name" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblName" Text='<%#Eval("FullName") %>'>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderStyle-CssClass="header-center" HeaderText="Select Student">
                                <HeaderTemplate>
                                    <div style="text-align: center">
                                        <asp:CheckBox ID="chkSelectAll" runat="server" Text="Select All" OnCheckedChanged="chkSelectAll_CheckedChanged"
                                            AutoPostBack="true" />
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div style="text-align: center">
                                        <asp:Label ID="lblStudentId" runat="server" Visible="false" Text='<%#Eval("StudentID") %>'></asp:Label>
                                        <asp:Label ID="lblHeldInRelationId" runat="server" Visible="false" Text='<%#Eval("HeldInRelationId") %>'></asp:Label>

                                        <asp:CheckBox runat="server" ID="ChkSelect"></asp:CheckBox>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Status">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="lnkStatus" Text='<%# Eval("RoleName") +" "+ (Convert.ToInt32( Eval("ApplicationStatus"))==1 ? "Applied" : Convert.ToInt32( Eval("ApplicationStatus"))==2 ? "Forwarded" : Convert.ToInt32( Eval("ApplicationStatus"))==3 ? "Rejected" : Convert.ToInt32( Eval("ApplicationStatus"))==0 ? "Pending" : "" ) %>' ForeColor='<%#  Convert.ToInt32( Eval("ApplicationStatus"))==3 ? System.Drawing.Color.Red : System.Drawing.Color.Blue %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <%-- 

                                    <asp:TemplateField HeaderText="Course Code" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblFormalCode" Text='<%#Eval("FormalCode") %>'>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Title" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblTitle" Text='<%#Eval("Title") %>'>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Credit" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblCredit" Text='<%#Eval("Credits") %>'>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                     

                                    <asp:TemplateField HeaderStyle-CssClass="header-center">
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkAttendanceSave" runat="server" OnClick="lnkAttendanceSave_Click" OnClientClick="jsShowHideProgress()" Width="100%" Style="text-align: center" CssClass="btn-warning form-control">
                                                                    <b>Save</b>
                                            </asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div class=" checkbox-btn" id="status" style="text-align: center;">
                                                <asp:CheckBox ID="chkStatus" runat="server" Checked='<%# Convert.ToDecimal(Eval("AttendancePercentage"))>=60 ? true : false %>' />
                                                <div><span class="slide"></span></div>
                                            </div>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:LinkButton ID="lnkAttendanceSave2" runat="server" OnClick="lnkAttendanceSave_Click" OnClientClick="jsShowHideProgress()" Width="100%" Style="text-align: center" CssClass="btn-warning form-control">
                                                                    <b>Save</b>  </asp:LinkButton>
                                        </FooterTemplate>
                                        <HeaderStyle />
                                        <ItemStyle Width="5%" />
                                    </asp:TemplateField>


                            --%>
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

                    <div class="row" style="margin-top: 5px" runat="server" id="divbtnPanel2">
                        <div class="col-lg-6 col-md-6 col-sm-6">
                        </div>
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <asp:LinkButton ID="LinkButton1" runat="server" Width="100%" type="button" CssClass="btn-info btn-lg" Style="text-align: center" Font-Size="Small" OnClick="lnkApprove_Click">
                                                                    <b>Forward To Academic Section </b>
                            </asp:LinkButton>
                        </div>
                    </div>

                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>


    
    <div class="col-md-15 col-lg-12">
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>

                <asp:Button ID="Button4" runat="server" Style="display: none" />
                <ajaxToolkit:ModalPopupExtender ID="ModalPopupConfirmation" runat="server" TargetControlID="Button4" PopupControlID="Panel3"
                    BackgroundCssClass="modalBackground" CancelControlID="Button6">
                </ajaxToolkit:ModalPopupExtender>

                <asp:Panel runat="server" ID="Panel3" Style="display: none; padding: 5px;" BackColor="White" Width="30%">


                    <div class="panel panel-default">
                        <div class="panel-body">

                            <div class="row" style="text-align: center; color: blue; font-weight: bold">
                                <div class="col-lg-12 col-md-12 col-sm-12" style="text-align: center">
                                    <b>আপনি কি নিশ্চিত আপনি Academic Section এর নিকট পাঠাতে চান ?</b>
                                </div>
                            </div>

                            <div class="row" style="margin-top: 30px">
                                <div class="col-lg-4 col-md-4 col-sm-4">
                                    <asp:Button runat="server" ID="btnRequestConfirm" Text="YES" OnClick="btnRequestConfirm_Click" OnClientClick="jsShowHideProgress()" CssClass="btn-info btn-sm" Style="display: inline-block; width: 100%; height: 35px; text-align: center; font-size: 17px;" />
                                </div>
                                <div class="col-lg-4 col-md-4 col-sm-4">
                                </div>
                                <div class="col-lg-4 col-md-4 col-sm-4">
                                    <asp:Button runat="server" ID="Button6" Text="NO" CssClass="btn-danger btn-sm" Style="display: inline-block; width: 100%; height: 35px; text-align: center; font-size: 17px;" />
                                </div>
                            </div>

                        </div>
                    </div>



                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    

    <ajaxToolkit:UpdatePanelAnimationExtender
        ID="UpdatePanelAnimationExtender1" TargetControlID="UpdatePanel02" runat="server">
        <Animations>
            <OnUpdating>
                <Parallel duration="0">
                    <ScriptAction Script="InProgress();" />                  
                </Parallel>
            </OnUpdating>
            <OnUpdated>
                <Parallel duration="0">
                    <ScriptAction Script="onComplete();" />
                </Parallel>
            </OnUpdated>
        </Animations>
    </ajaxToolkit:UpdatePanelAnimationExtender>

</asp:Content>
