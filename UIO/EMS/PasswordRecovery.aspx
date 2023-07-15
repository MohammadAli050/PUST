<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PasswordRecovery.aspx.cs" Inherits="EMS.PasswordRecovery" %>




<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <!-- The above 3 meta tags *must* come first in the head; any other head content must come *after* these tags -->
    <title>Password Recovery</title>
    <link rel="shortcut icon" href="<%= Page.ResolveUrl("~")%>Images/buft_favicon.png" />
    <link href="Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="Bootstrap/css/font-awesome.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">
    <script src="JavaScript/jquery-1.7.1.js"></script>

    <script type="text/javascript">
        function CheckLoginIDAndSendURL() {

            document.getElementById('imgProgess').style.display = 'block';
            document.getElementById('btnSubmit').disabled = true;
            //var loginID = document.getElementById("loginIDInput").value;
            var loginID = $('#loginIDInput').val();

            if (loginID.length == 0) {

                $('#message').text('Please input valid Login ID');
                document.getElementById('imgProgess').style.display = 'none';
                document.getElementById('btnSubmit').disabled = false;
                return;
            }

            $.ajax({
                type: "POST",
                url: "PasswordRecovery.aspx/CheckLoginID",
                data: "{loginID: '" + loginID + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var result = msg.d;
                    if (result.length > 0) {

                        document.getElementById('message').innerHTML = result;
                    }
                    document.getElementById('imgProgess').style.display = 'none';
                    document.getElementById('btnSubmit').disabled = false;
                },
                error: function () {
                    alert("Error : Contact with admin!");
                    document.getElementById('imgProgess').style.visibility = 'none';
                    document.getElementById('btnSubmit').disabled = false;
                }
            });
        }
    </script>

    <style>
        .panel {
                box-shadow: 0 3px 3px 0 rgba(0,0,0,.2), 0 6px 10px 0 rgba(0,0,0,.19);
        }

    </style>
</head>
<body>

    <hr>
    <div class="container">
        <div class="row">
            <div class="row">
                <div class="col-lg-2 col-md-2 col-sm-2">
                </div>

                <div class="col-lg-4 col-md-4 col-sm-4">
                    <div class="panel panel-default">
                        <div class="panel-body">
                            <div class="text-center">
                                <h3><i class="fa fa-lock fa-4x"></i></h3>
                                <h2 class="text-center">Recover/Forget Password</h2>
                                <p>You can send your password reset link from here.</p>
                                <span style="color: blue" id='message'></span>

                                <div class="panel-body">

                                    <%--<form class="form" action="#" onsubmit="return CheckLoginIDAndSendURL();">--%>
                                    <fieldset>
                                        <legend style="display: none;"></legend>
                                        <div class="form-group" style="display: inline-block; vertical-align: middle; text-align: center">
                                            <img id="imgProgess" src="Images/Default.gif" style="display: none" />
                                        </div>

                                        <div class="form-group">
                                            <div class="input-group">
                                                <span class="input-group-addon"><i class="fa fa-user color-blue"></i></span>

                                                <input id="loginIDInput" placeholder="Login ID / Student ID / Email ID" class="form-control" type="text" required>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <input id="btnSubmit" class="btn btn-lg btn-success btn-block" value="Send Password Reset Link" type="button" onclick="CheckLoginIDAndSendURL();">
                                        </div>
                                    </fieldset>
                                    <%--</form>--%>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-lg-4 col-md-4 col-sm-4">
                    <div class="panel">
                        <div style="background-color: #f6933b; border-radius: 4px; border: 1.5px solid #ab802d; text-align: center; height: 51px;">
                            <p style="font-size: 20px; color: white; margin-top: 12px;"><b>নির্দেশিকা</b></p>
                        </div>
                        <div class="panel-body" style="text-align: center; font-size: 17px; color: #333; background-color: #efeded">
                            আপনার <b>Login ID</b> জানা থাকলে  <b>Login ID</b> দিতে পারেন। 
                            <br />
                            <p style="text-align: center; color: red; margin-top: 10px;">অথবা</p> 
                            শিক্ষার্থীদের জন্য আপনার <b>Student ID</b> (যেমন CE20001) দিতে পারেন। 
                            <br />
                            <br />
                            শিক্ষার্থীদের <b>Student ID</b> টি <b>Login ID</b> হিসেবে ব্যবহৃত হবে। 
                            <br />
                            <p style="text-align: center; color: red; margin-top: 10px;">অথবা</p> 
                            শিক্ষক /কর্মকর্তা /কর্মচারীদের ক্ষেত্রে বিশ্ববিদ্যালয়ের 
                            <br />
                            ডায়েরিতে উল্লেখিত প্রাতিষ্ঠানিক ইমেইল <b>ID</b> টি ব্যবহার করুন। 
                        </div>
                    </div>
                </div>

                <div class="col-lg-2 col-md-2 col-sm-2">
                </div>

            </div>
        </div>
    </div>
</body>
</html>

