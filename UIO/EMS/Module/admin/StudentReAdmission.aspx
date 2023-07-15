<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true" CodeBehind="StudentReAdmission.aspx.cs" Inherits="EMS.Module.admin.StudentReAdmission" %>


<%@ Register Src="~/UserControls/AdmissionSessionUserControl.ascx" TagPrefix="uc1" TagName="AdmissionSessionUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Student Re-Admission
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

        #ctl00_MainContainer_txtStudentId, #ctl00_MainContainer_btnLoad, #ctl00_MainContainer_admissionSession_ddlSession,#ctl00_MainContainer_btnReAdmission {
            height: 40px !important;
            font-size: 20px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContainer" runat="server">

    <div class="row">
        <div class="col-sm-12" style="font-size: 12pt; margin-top: 10pt;">
            <label><b style="color: black; font-size: 26px">Student Re-Admission</b></label>
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

                    <div class="row">
                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <b>Student ID</b>
                            <asp:TextBox ID="txtStudentId" runat="server" CssClass="form-control w-100"></asp:TextBox>
                        </div>

                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <br />
                            <asp:Button ID="btnLoad" runat="server" CssClass="btn-info w-100" OnClick="btnLoad_Click" Text="Load" OnClientClick="this.value = 'Please wait....'; this.disabled = true;" UseSubmitBehavior="false" />
                        </div>

                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <b>Re-Admission Session</b>
                            <br />
                            <uc1:AdmissionSessionUserControl runat="server" ID="admissionSession" />
                        </div>

                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <br />
                            <asp:Button ID="btnReAdmission" runat="server" CssClass="btn-danger w-100" OnClick="btnReAdmission_Click" Text="Re-Admission" OnClientClick="this.value = 'Please wait....'; this.disabled = true;" UseSubmitBehavior="false" />
                        </div>
                    </div>

                </div>
            </div>


            <div class="card" style="margin-top: 10px">
                <div class="card-body">
                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <b>Program Name</b>
                            <br />
                            <asp:Label ID="lblProgram" runat="server" Text=""></asp:Label>
                        </div>
                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <b>Student Name</b>
                            <br />
                            <asp:Label ID="lblName" runat="server" Text=""></asp:Label>
                        </div>

                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <b>Admission Session</b>
                            <br />
                            <asp:Label ID="lblAdmissionSession" runat="server" Text=""></asp:Label>
                        </div>

                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <b>Re-Admission Session</b>
                            <br />
                            <asp:Label ID="lblReAdmissionSession" runat="server" Text=""></asp:Label>
                        </div>

                    </div>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>



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
