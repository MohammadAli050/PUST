<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true" CodeBehind="StudentFormFillUpApply.aspx.cs" Inherits="EMS.Module.FormFillUp.StudentFormFillUpApply" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Student Form Fill-Up Application
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


        function Search(x) {


            input = document.getElementById("myInput");
            filter = input.value.toUpperCase();
            table = document.getElementById("ctl00_MainContainer_gvCourseList");
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

    <style type="text/css">
        .remove-all-styles {
            all: revert;
        }

        .scrolling {
            position: absolute;
        }

        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=80);
            opacity: 0.8;
            z-index: 10000;
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

        .header-center {
            text-align: center;
        }

        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=80);
            opacity: 0.8;
            z-index: 10000;
        }

        .sweet-alert {
            z-index: 1000000;
        }

        #ctl00_MainContainer_txtStudentId, #ctl00_MainContainer_btnLoad, #ctl00_MainContainer_btnSubmit,#ctl00_MainContainer_btnSubmit2 {
            height: 40px !important;
            font-size: 20px;
        }
    </style>


</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContainer" runat="server">

    <div class="row">
        <div class="col-sm-12" style="font-size: 12pt; margin-top: 10pt;">
            <label><b style="color: black; font-size: 26px">Student Form Fill-Up Application</b></label>
        </div>
    </div>
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
                <div class="card-body">

                    <div class="row" style="text-align: center">
                        <div class="col-lg-3 col-md-3 col-sm-3">
                        </div>

                        <div class="col-lg-2 col-md-2 col-sm-2" style="text-align: left">
                            <b>Student ID</b>
                            <asp:TextBox ID="txtStudentId" runat="server" CssClass="form-control w-100"></asp:TextBox>

                            <div class="col-lg-12 col-md-12 col-sm-12" runat="server" id="divBtnLoad">
                                <br />
                                <asp:Button ID="btnLoad" runat="server" CssClass="btn-info w-100" Text="Load Course" OnClick="btnLoad_Click" OnClientClick="this.value = 'Please wait....'; this.disabled = true;" UseSubmitBehavior="false" />
                            </div>
                        </div>

                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <div class="card">
                                <div class="card-body">

                                    <div style="color: blue; background-color: #f3f3f3; font-size: 20px" class="card">Form Fill-Up নির্দেশনা </div>
                                    <ul style="color: red; text-align: left">
                                        <li>১ম বর্ষ ১ম সেমিস্টার নিম্নোক্ত কোর্সগুলো আপনার জন্য প্রদর্শিত হলো। 
                                     "Submit Form Fill Up Application" button click করে আপনার Form Fill-Up সম্পন্ন করুন।</li>
                                        <li>কোর্স সম্পর্কে আপনার কোন কিছু জানার থাকলে বিভাগে যোগাযোগ করুন।</li>
                                    </ul>

                                </div>
                            </div>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                        </div>
                    </div>

                </div>
            </div>


            <div class="card" style="margin-top: 10px">
                <div class="card-body">

                    <div class="row">
                        <div class="col-lg-8 col-md-8 col-sm-8">
                            <asp:Label runat="server" ID="lblStatus" Text="" Font-Bold="true" ForeColor="Crimson" Font-Size="20px"></asp:Label>
                        </div>

                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <br />
                            <asp:Button ID="btnSubmit" runat="server" CssClass="btn-danger w-100" Text="Submit Form Fill Up Application" OnClick="btnSubmit_Click" OnClientClick="this.value = 'Please wait....'; this.disabled = true;" UseSubmitBehavior="false" />
                        </div>

                    </div>

                    <br />

                    <asp:GridView runat="server" ID="gvCourseList" AutoGenerateColumns="False" AllowPaging="false"
                        PageSize="20" CssClass="table table-bordered table-responsive"
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


                            <asp:TemplateField HeaderStyle-CssClass="header-center">
                                <HeaderTemplate>
                                    <div style="text-align: center">
                                        <asp:Label runat="server" ID="ckhSelect" Text="Select All" Font-Bold="true"></asp:Label>
                                    </div>
                                    <div style="text-align: center">
                                        <asp:CheckBox ID="chkSelectAll" runat="server" Checked="true" OnCheckedChanged="chkSelectAll_CheckedChanged" Enabled="false"
                                            AutoPostBack="true" />

                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div style="text-align: center">

                                        <asp:HiddenField ID="hdnCourseHistoryId" runat="server" Value='<%#Eval("CourseHistoryId") %>' />
                                        <asp:HiddenField ID="hdnSetupId" runat="server" Value='<%#Eval("SetupId") %>' />
                                        <asp:HiddenField ID="hdnHeldInRelationId" runat="server" Value='<%#Eval("HeldInRelationId") %>' />

                                        <%--All Course Checked for 1_1 students--%>
                                        <asp:CheckBox runat="server" ID="ChkSelect" Enabled="false" Checked="true"></asp:CheckBox>
                                    </div>
                                </ItemTemplate>
                                <ItemStyle Width="5%" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Course Code" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblFormalCode" Text='<%#Eval("FormalCode") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Title" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblTitle" Text='<%#Eval("Title") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Credit" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblCredit" Text='<%#Eval("CourseCredit") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Type" HeaderStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblRegType" Text='<%#Eval("CourseType") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Status">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="lnkStatus" OnClick="lnkStatus_Click" CommandArgument='<%#Eval("SetupId") %>' Text='<%# Eval("RoleName") +" "+ (Convert.ToInt32( Eval("ApplicationStatus"))==1 ? "Submitted" : Convert.ToInt32( Eval("ApplicationStatus"))==2 ? "Approved" : Convert.ToInt32( Eval("ApplicationStatus"))==3 ? "Rejected" : Convert.ToInt32( Eval("ApplicationStatus"))==0 ? "Pending" : "" ) +"<br />"+Eval("Date") %>' ForeColor='<%#  Convert.ToInt32( Eval("ApplicationStatus"))==3 ? System.Drawing.Color.Red : System.Drawing.Color.Blue %>'></asp:LinkButton>
                                </ItemTemplate>
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

                    <div class="row">
                        <div class="col-lg-8 col-md-8 col-sm-8">
                        </div>

                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <br />
                            <asp:Button ID="btnSubmit2" runat="server" CssClass="btn-danger w-100" Text="Submit Form Fill Up Application" OnClick="btnSubmit_Click" OnClientClick="this.value = 'Please wait....'; this.disabled = true;" UseSubmitBehavior="false" />
                        </div>

                    </div>
                </div>
            </div>







            <asp:Button ID="Button1" runat="server" Style="display: none" />
            <ajaxToolkit:ModalPopupExtender ID="ModalPopUpHistory" runat="server" TargetControlID="Button1" PopupControlID="Panel1"
                CancelControlID="Button5" BackgroundCssClass="modalBackground">
            </ajaxToolkit:ModalPopupExtender>

            <asp:Panel runat="server" ID="Panel1" Style="display: none; padding: 5px; border-radius: 5px; overflow: scroll" BackColor="White" Width="700px" Height="350px">
                <fieldset style="padding: 5px; margin: 5px;">
                    <legend style="font-weight: bold; font-size: 20px; text-align: center">Student Form Fill-Up Application History</legend>

                    <div class="row" style="text-align: center">
                        <asp:Label ID="lblCourseInfo" runat="server" Text="" Font-Bold="true" ForeColor="Crimson"></asp:Label>
                    </div>
                    <div class="row" style="margin-top: 5px">

                        <div class="col-lg-12 col-md-12 col-sm-12">
                            <asp:GridView runat="server" ID="gvStatusHistory" AutoGenerateColumns="False" AllowPaging="false" PageSize="20"
                                PagerSettings-Mode="NumericFirstLast" Width="100%" ShowFooter="true"
                                PagerStyle-Font-Bold="true" PagerStyle-Font-Size="Larger"
                                ShowHeader="true">
                                <HeaderStyle BackColor="#4285f4" ForeColor="White" Height="10px" Font-Bold="True" />
                                <FooterStyle BackColor="#4285f4" ForeColor="White" Height="10px" Font-Bold="True" />
                                <AlternatingRowStyle BackColor="White" />
                                <RowStyle Height="10px" />
                                <Columns>
                                    <asp:TemplateField HeaderText="SL" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate><b><%# Container.DataItemIndex + 1 %></b></ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                        <HeaderStyle Width="3%" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Status">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblStatus" Text='<%# Eval("RoleName") +" "+ (Convert.ToInt32( Eval("ApplicationStatus"))==1 ? "Applied" : Convert.ToInt32( Eval("ApplicationStatus"))==2 ? "Approved" : Convert.ToInt32( Eval("ApplicationStatus"))==3 ? "Rejected" :Convert.ToInt32( Eval("ApplicationStatus"))==0 ? "Pending" : "" ) %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Status Date">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblStatusDate" Text='<%# Eval("Date")  %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle />
                                    </asp:TemplateField>

                                    <%-- <asp:TemplateField HeaderText="Reason">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblrsn" Text='<%# Eval("Reason")  %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle />
                                    </asp:TemplateField>--%>
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

                    </div>

                    <div class="row" style="margin-top: 5px">
                        <div class="col-lg-5 col-md-5 col-sm-5"></div>
                        <div class="col-lg-2 col-md-2 col-sm-2" style="text-align: center">
                            <asp:Button runat="server" ID="Button5" Text="CLOSE" CssClass="btn-danger btn-sm" Width="100%" Font-Bold="true" Style="text-align: center" />
                        </div>
                        <div class="col-lg-5 col-md-5 col-sm-5"></div>

                    </div>

                </fieldset>
            </asp:Panel>



        </ContentTemplate>
    </asp:UpdatePanel>


    <div class="col-md-15 col-lg-12">
        <asp:UpdatePanel ID="UpdatePanel9" runat="server">
            <ContentTemplate>

                <asp:Button ID="Button2" runat="server" Style="display: none" />
                <ajaxToolkit:ModalPopupExtender ID="ModalPopupConfirmation" runat="server" TargetControlID="Button2" PopupControlID="Panel2"
                    BackgroundCssClass="modalBackground" CancelControlID="Button3">
                </ajaxToolkit:ModalPopupExtender>

                <asp:Panel runat="server" ID="Panel2" Style="display: none; padding: 5px;" BackColor="White" Width="30%">


                    <div class="panel panel-default">
                        <div class="panel-body">

                            <div class="row" style="text-align: center; color: blue; font-weight: bold">
                                <div class="col-lg-12 col-md-12 col-sm-12" style="text-align: center">
                                    <b>আপনি কি নিশ্চিত আপনি Submit করতে চান ?</b>
                                </div>
                            </div>

                            <div class="row" style="margin-top: 30px">
                                <div class="col-lg-4 col-md-4 col-sm-4">
                                    <asp:Button runat="server" ID="btnRequestConfirm" Text="YES" OnClick="btnRequestConfirm_Click" OnClientClick="this.value = 'Please wait....'; this.disabled = true;" UseSubmitBehavior="false" CssClass="btn-info btn-sm" Style="display: inline-block; width: 100%; height: 35px; text-align: center; font-size: 17px;" />
                                </div>
                                <div class="col-lg-4 col-md-4 col-sm-4">
                                </div>
                                <div class="col-lg-4 col-md-4 col-sm-4">
                                    <asp:Button runat="server" ID="Button3" Text="NO" CssClass="btn-danger btn-sm" Style="display: inline-block; width: 100%; height: 35px; text-align: center; font-size: 17px;" />
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
                    <EnableAction AnimationTarget="btnLoad" Enabled="false" />                   
                </Parallel>
            </OnUpdating>
            <OnUpdated>
                <Parallel duration="0">
                    <ScriptAction Script="onComplete();" />
                    <EnableAction   AnimationTarget="btnLoad" Enabled="true" />
                </Parallel>
            </OnUpdated>
        </Animations>
    </ajaxToolkit:UpdatePanelAnimationExtender>

</asp:Content>
