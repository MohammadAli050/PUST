<%@ Page Language="C#" AutoEventWireup="true" Inherits="Security_Default" CodeBehind="LogIn.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>PABNA EMS LogIn</title>
    <asp:PlaceHolder runat="server" ID="plh">
        <link rel="shortcut icon" href="<%= string.Format("{0}/Images/PABNA_logo.png", CommonUtility.AppPath.ApplicationPath) %>" />
    </asp:PlaceHolder>

    <link href="../Bootstrap/css/bootstrap.css" rel="stylesheet" />
    <link href="../CSS/CSSFiles/SiteMaster.CSS" rel="stylesheet" />
    <script type="text/javascript" src="../JavaScript/jquery-1.7.1.js"></script>

    <!-- fontawesome icon -->
    <link rel="stylesheet" href="../Content/assets/fonts/fontawesome/css/fontawesome-all.min.css" />
    <!-- animation css -->
    <link rel="stylesheet" href="../Content/assets/plugins/animation/css/animate.min.css" />
    <!-- vendor css -->
    <link rel="stylesheet" href="../Content/assets/css/style.css" />
    <script type="text/javascript" src="../Content/assets/js/vendor-all.min.js"></script>
    <script type="text/javascript" src="../Content/assets/plugins/bootstrap/js/bootstrap.min.js"></script>

    <script type="text/javascript">
        function InProgress() {
            var panelProg = $get('divProgress');
            var panelProg1 = $get('logMain_Button1');
            panelProg.style.display = '';
            panelProg1.disabled = true;
        }

        function onComplete() {
            var panelProg = $get('divProgress');
            var panelProg1 = $get('logMain_Button1');
            panelProg.style.display = 'none';
            panelProg1.disabled = false;
        }
    </script>
    <%--<script>
            (function (i, s, o, g, r, a, m) {
                i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () {
                    (i[r].q = i[r].q || []).push(arguments)
                }, i[r].l = 1 * new Date(); a = s.createElement(o),
                m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)
            })(window, document, 'script', '//www.google-analytics.com/analytics.js', 'ga');

            ga('create', 'UA-62797653-1', 'auto');
            ga('send', 'pageview');

        </script>--%>
    <!--[if IE]>
            <style type="text/css">    
            td
            { 
                padding-left: 3px; 
            }
            </style>
        <![endif]-->
    <style type="text/css">
        body {
            font-family: "Helvetica Neue", Helvetica, Arial, sans-serif;
            font-size: 14px;
            line-height: 1.42857143;
            color: #333;
            background: #eee;
        }

        .container-login100 {
            width: 100%;
            min-height: 100vh;
            justify-content: center;
            align-items: center;
            padding: 15px;
            background-position: center;
            background-size: cover;
            background-repeat: no-repeat;
        }

        .middle {
            background: linear-gradient(-135deg, #fbc1ff 0%, #4e65ff 100%);
            transition: all .55s ease-in-out;
        }

            .middle:hover {
                box-shadow: 0 8px 17px 0 rgba(0,0,0,.2), 0 6px 20px 0 rgba(0,0,0,.19);
                transition: all .55s ease-in-out;
            }

        .imgs {
            display: block;
            width: 90px;
            margin: 0 auto;
            <%--border: 1px solid #8e87ff;--%>
            border-radius: 9px;
        }

        .below {
            background-color: #0b74e3 !important;
        }

            .below:hover {
                box-shadow: 0 8px 17px 0 rgba(0,0,0,.2), 0 6px 20px 0 rgba(0,0,0,.19);
                transition: all .55s ease-in-out;
            }
    </style>

</head>

<body>
    <div class="container-login100" style="background-image: url('../Content/assets/images/bg-01.jpg')">
        <form id="frmLogIn" runat="server">
            <%-- <div class="container">
                    <div class="row" >
                        <div class="col-md-12 col-lg-12">
                            <div class="login100-form">
                                <asp:ScriptManager ID="scMgtMas" runat="server" />
                       
                                    <div class="row width" style="">
                                        <div class="col-md-6 col-lg-6 login" style="background: #F0F0F0;border-radius: 6px;" >
                                            <img src="../Images/brur_logo.png" alt="" title="" style="margin: 14px auto; width: 100%" />

                                            <p class="welcome_text" style="font-weight: bold;font-size: 18px;">Welcome!</p>
                                            <p class="welcome_text" style="padding-bottom:20px">Enter your ID and Password to continue</p>
                                        </div>
                                        <div class="col-md-6 col-lg-6">
                                            <asp:UpdatePanel ID="upMain" runat="server">
                                                <ContentTemplate>
                                                    <div>
                                                        <div style="width: 100%;">
                                                            <div>
                                                                <asp:Login ID="logMain" runat="server" 
                                                                    Font-Names="Verdana" Font-Size="0.8em" ForeColor="#333333"
                                                                    OnAuthenticate="logMain_Authenticate" Width="100%"
                                                                    UserNameLabelText="Log In ID:" TitleText="LOG IN" RememberMeText="Remember Me." 
                                                                    BorderColor="#F2F2F2" BorderPadding="4" BorderStyle="Solid"  
                                                                    TextLayout="TextOnTop">

                                                                    <TextBoxStyle Font-Size="0.8em" BorderColor="#99CCFF" BorderStyle="Solid" BorderWidth="1px" Height="20px" Width="130px" Font-Names="Arial" />
                                                                    <LoginButtonStyle CssClass="button" BackColor="White" BorderColor="#CC9966" BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="0.8em" ForeColor="#990000" />
                                                                    <ValidatorTextStyle BorderStyle="None" BorderWidth="0px" />
                                                                    <InstructionTextStyle Font-Italic="True" ForeColor="Black" />
                                                                    <LabelStyle BorderStyle="None" BorderWidth="1px" Font-Names="Arial" Font-Size="Small" ForeColor="#0000CC" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                    <LayoutTemplate>
                                                                            <div>
                                                                                <span class="login100-form-title p-b-34" style="font-size: 18px;letter-spacing: normal;font-weight: bold;padding-right: 99px; font-family:'Roboto Condensed', sans-serif">Sign in</span>
                                                                            </div> 
                                                                            <div class=" rs1-wrap-input100 validate-input m-b-20">
                                                                        
                                                                                    <%--<asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName"></asp:Label>-->
                                                                        
                                                                                    <asp:TextBox ID="UserName" runat="server" CssClass="login-box-form" Placeholder="Enter User Name Here"></asp:TextBox>
                                                                        
                                                                            </div>
                                                                                
                                                                            <div class="rs1-wrap-input100 validate-input m-b-20">
                                                                       
                                                                                    <%--<asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password"></asp:Label>-->
                                                                        
                                                                                    <asp:TextBox ID="Password" runat="server" CssClass="login-box-form" Placeholder="Enter Password" TextMode="Password"></asp:TextBox>
                                                                        
                                                                            </div>
                                                                                
                                                                            <div style="width:100%">
                                                                                    <asp:Button ID="Button1" runat="server" CommandName="Login" CssClass="block1" Text="Log In" ValidationGroup="logMain" />
                                                                        
                                                                            </div>
                                                                            <%--<div class="w-full text-center p-t-27 p-b-239">
                                                                                <span class="txt1">Forgot </span><a class="txt2" href="#">Your Password ? </a>
                                                                            </div>-->
                                                                            <div class="w-full text-center">
                                                                                <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                                                            </div>
                                                                            
                                                                       
                                                                    </LayoutTemplate>
                                                                    <FailureTextStyle BackColor="#99CCFF" BorderColor="#3333FF" BorderStyle="Solid"
                                                                        BorderWidth="1px" HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                    <TitleTextStyle Font-Bold="True" Font-Size="0.9em"
                                                                        ForeColor="White" BorderStyle="None" Font-Names="Calibri" BackColor="#990000" />
                                                                </asp:Login>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                
                                    </div>
                        
                            </div>
                        </div>
                
                    </div>
                </div>--%>
            <asp:ScriptManager ID="scMgtMas" runat="server" />
            <asp:UpdatePanel ID="upMain" runat="server">
                <ContentTemplate>
                    <div>
                        <div style="">
                            <div>
                                <asp:Login ID="logMain" runat="server"
                                    Font-Names="Verdana" Font-Size="0.8em" ForeColor="#333333"
                                    OnAuthenticate="logMain_Authenticate" Width="100%"
                                    UserNameLabelText="Log In ID:" TitleText="LOG IN" RememberMeText="Remember Me."
                                    BorderColor="#F2F2F2" BorderPadding="4" BorderStyle="Solid"
                                    TextLayout="TextOnTop">

                                    <TextBoxStyle Font-Size="0.8em" BorderColor="#99CCFF" BorderStyle="Solid" BorderWidth="1px" Height="20px" Width="130px" Font-Names="Arial" />
                                    <LoginButtonStyle CssClass="button" BackColor="White" BorderColor="#CC9966" BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" Font-Size="0.8em" ForeColor="#990000" />
                                    <ValidatorTextStyle BorderStyle="None" BorderWidth="0px" />
                                    <InstructionTextStyle Font-Italic="True" ForeColor="Black" />
                                    <LabelStyle BorderStyle="None" BorderWidth="1px" Font-Names="Arial" Font-Size="Small" ForeColor="#0000CC" HorizontalAlign="Left" VerticalAlign="Middle" />
                                    <LayoutTemplate>
                                        <div class="auth-wrapper">
                                            <div class="auth-content">
                                                <div class="auth-bg">
                                                    <span class="r"></span>
                                                    <%--<span class="r1"></span>--%>
                                                    <span class="r s"></span>
                                                    <span class="r s"></span>
                                                    <%--<span class="r"></span>--%>
                                                    <%--<span class="r1"></span>--%>
                                                </div>
                                                <div class="card">
                                                    <div class="card-body text-center middle" style="">
                                                        <div class="mb-1">
                                                            <div class="row">
                                                                <div class="col-md-12">
                                                                    <img class="imgs" style="" src="../Images/PABNA_Logo.png" />
                                                                </div>
                                                                <div class="col-md-8">
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <%--<h3 class="mb-4" style="font-family:Cambria; font-size:40px; "><b>Login</b></h3>--%>
                                                        <div class="input-group mb-4">
                                                            <%--<asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName"></asp:Label>--%>
                                                            <asp:TextBox ID="UserName" runat="server" CssClass="form-control" Style="border-radius: 7px" placeholder="Enter your Login Id/username"></asp:TextBox>
                                                        </div>
                                                        <div class="input-group mb-4">
                                                            <%--<asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password"></asp:Label>--%>
                                                            <asp:TextBox ID="Password" runat="server" Style="border-radius: 7px" CssClass="form-control" placeholder="Enter your password" TextMode="Password"></asp:TextBox>
                                                        </div>
                                                        <asp:Button ID="Button1" runat="server" CommandName="Login" Style="border-radius: 7px; width: 100%;" CssClass="btn btn-info shadow-2 mb-4" Text="Login" Font-Bold="true" ValidationGroup="logMain" />
                                                        <div id="divProgress" style="display: none; z-index: 1000;">
                                                            <asp:Image ID="LoadingImage" runat="server" ImageUrl="~/Images/working.gif" Height="30px" Width="30px" />
                                                        </div>
                                                        <span style="font-weight: bold; font-size: 15px; color: yellow">
                                                            <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal></span>
                                                        <br />
                                                        <a style="font-weight: bold; font-size: 20px; color: white;" href="../PasswordRecovery.aspx" class="txt2">Set a new password</a>
                                                    </div>
                                                </div>


                                                <div class="card-body text-center below" style="">
                                                    <div class="card-body right" style="font-size: 15px; text-align: center; background-color: white; border: 1px solid black; color: black; border-radius: 4px;">
                                                        <div style="background-color: #033e0c; border-radius: 4px; border: 1.5px solid #2f2f5d;  text-align: center; height: 45px;">
                                                            <p style="font-size: 18px; color: white; margin-top: 10px;"><b>নির্দেশিকা</b></p>
                                                        </div>
                                                        <br />
                                                        আপনার Login ID জানা না থাকলে 
                                                    <br />
                                                        কিংবা Password ভুলে গেলে
                                                        <br />
                                                        <b>"Set a new password" </b>
                                                        <br />
                                                        link এ ক্লিক করুন।
                                                        <br />
                                                        শিক্ষার্থীদের Login ID সাধারণত
                                                    <br />
                                                        Student ID টি বিবেচিত হবে।
                                                    </div>
                                                </div>

                                            </div>


                                        </div>


                                    </LayoutTemplate>
                                    <FailureTextStyle BackColor="#99CCFF" BorderColor="#3333FF" BorderStyle="Solid"
                                        BorderWidth="1px" HorizontalAlign="Left" VerticalAlign="Middle" />
                                    <TitleTextStyle Font-Bold="True" Font-Size="0.9em"
                                        ForeColor="White" BorderStyle="None" Font-Names="Calibri" BackColor="#990000" />
                                </asp:Login>

                            </div>
                        </div>
                    </div>

                </ContentTemplate>
            </asp:UpdatePanel>
            <hr style="text-align: center; width: 75%;" />
            <div class="footer" style="text-align: center">
                <div>
                    Powered by <b>Edusoft Consultants Ltd</b><br />
                    Copyright &copy; 2013 - 2023 Edusoft Consultants Ltd. All rights reserved.
                   
                </div>
            </div>
            <ajaxToolkit:UpdatePanelAnimationExtender
                ID="UpdatePanelAnimationExtender1"
                TargetControlID="UpdatePanel2"
                runat="server">
                <Animations>
                    <OnUpdating>
                       <Parallel duration="0">
                            <ScriptAction Script="InProgress();" />
                            <EnableAction AnimationTarget="Button1" 
                                          Enabled="false" />                   
                        </Parallel>
                    </OnUpdating>
                    <OnUpdated>
                        <Parallel duration="0">
                            <ScriptAction Script="onComplete();" />
                            <EnableAction   AnimationTarget="Button1" 
                                            Enabled="true" />
                        </Parallel>
                    </OnUpdated>
                </Animations>
            </ajaxToolkit:UpdatePanelAnimationExtender>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                </ContentTemplate>
            </asp:UpdatePanel>
        </form>
    </div>
</body>
</html>
