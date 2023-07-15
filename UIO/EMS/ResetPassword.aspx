<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="EMS.ResetPassword" %>



<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <!-- The above 3 meta tags *must* come first in the head; any other head content must come *after* these tags -->
    <title>Password Reset</title>
    <link rel="shortcut icon" href="<%= Page.ResolveUrl("~")%>Images/buft_favicon.png" />
    <link href="Bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <script src="JavaScript/jquery-1.7.1.js"></script>
    <script src="JavaScript/jquery-1.6.1.min.js"></script>
    <script type="text/javascript">

        $(document).ready(function () {
            document.getElementById('divSuccess').style.display = 'none';
        });


        var check = function () {

            if (document.getElementById('password').value ==
              document.getElementById('confirm_password').value) {
                document.getElementById('message').style.color = 'green';
                document.getElementById('message').innerHTML = '';
                document.getElementById("confirm_password").style.borderColor = "green";
            } else {
                document.getElementById('message').style.color = 'red';
                document.getElementById('message').innerHTML = 'Passwords Do Not Match!';
                document.getElementById("confirm_password").style.borderColor = "red";
            }


        }

        var strPassValid = function () {
            var ucase = new RegExp("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*])(?=.{8,})");
            var password = document.getElementById("password");
            if (ucase.test($("#password").val())) {

                document.getElementById('passMessage').style.color = 'green';
                document.getElementById("password").style.borderColor = "green";
                document.getElementById('passMessage').innerHTML = '';
            } else {
                document.getElementById('passMessage').style.color = 'red';
                document.getElementById("password").style.borderColor = "red";
                document.getElementById('passMessage').innerHTML = 'At Least 8 character';
            }

            if (document.getElementById('password').value ==
              document.getElementById('confirm_password').value) {
                document.getElementById('message').style.color = 'green';
                document.getElementById('message').innerHTML = '';
                document.getElementById("confirm_password").style.borderColor = "green";
            } else {
                document.getElementById('message').style.color = 'red';
                document.getElementById('message').innerHTML = 'Passwords Do Not Match!';
                document.getElementById("confirm_password").style.borderColor = "red";
            }
        }

        function validate() {

            var ucase = new RegExp("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*])(?=.{8,})");
            var password = document.getElementById("password");
            var confirm_password = document.getElementById("confirm_password");
            if (password.value == confirm_password.value && ucase.test($("#password").val())) {

                $.ajax({
                    type: "POST",
                    url: "ResetPassword.aspx/UpdatePassword",
                    data: "{ChangedPassword: '" + $("#password").val() + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        var result = msg.d;
                        if (result.length > 0) {

                            $("#divNewPass").animate({
                                height: 'toggle'
                            });
                            $("#divConPass").animate({
                                height: 'toggle'
                            });
                            $("#divButton").animate({
                                height: 'toggle'
                            }, function () {
                                $("#divSuccess").animate({
                                    height: 'toggle'
                                });
                            }
                            );

                            //document.getElementById('divSuccess').style.display = 'block';
                            //document.getElementById('divNewPass').style.display = 'none';
                            //document.getElementById('divConPass').style.display = 'none';
                            //document.getElementById('divButton').style.display = 'none';
                        }
                    },
                    error: function () {
                        alert("Error: Contact to Admin");
                    }
                });


            }
            else {
                strPassValid();

                if (password.value != confirm_password.value) {
                    document.getElementById('message').innerHTML = 'Passwords Do Not Match!';
                }
                return;
            }
        }

        function reDirLogin() {
            window.location = 'http://ucam.mbstu.ac.bd/Security/LogIn.aspx';

        }

    </script>
</head>
<body>
    <hr>
    <div class="container">
        <div id="passwordreset" style="margin-top: 50px" class="mainbox col-md-6 col-md-offset-3 col-sm-8 col-sm-offset-2">
            <div class="panel panel-info">
                <div class="panel-heading">
                    <div class="panel-title">Create New Password</div>
                </div>
                <div class="panel-body">
                    <div id="identicalForm" class="form-horizontal">
                        <div id="divSuccess" class="form-group center-block">
                            <label class="text-center col-md-12" style="color: green">Your password has been reset successfully. Thank you!</label>
                            <div class="text-center col-md-12">
                                <input class="btn btn-lg btn-success btn-block" id="btnLoginPage" value="Go To Login Page" type="button" name="button" onclick="reDirLogin();">
                            </div>
                        </div>
                        <div id="divNewPass" class="form-group">
                            <div class="row">

                                <div class="col-sm-12" style="text-align:left">
                                    <%--<b style="color:red;font-weight:bold;font-size:18px">Password should be developed using the following rule</b>--%>
                                    <ul style="font-weight:bold">
                                        <li> Minimum 8 characters</li>
                                        <li> One uppercase alphabet  (A-Z)</li>
                                        <li> One lowercase alphabet  (a-z)</li>
                                        <li> One numeric  (0-9)</li>
                                        <li> One special character  (!@#$%^&*)</li>

                                    </ul>

                                </div>
                            </div>
                            <br />
                            <label for="password" class=" control-label col-sm-3 col-md-4">New password</label>
                            <div class="col-sm-8">
                                <input id="password" name="password" type="password" onkeyup='strPassValid();' class="input-lg form-control" placeholder="new password" autocomplete="off" required>
                                <span id='passMessage'></span>
                            </div>
                        </div>
                        <div id="divConPass" class="form-group">
                            <label for="confirm_password" class=" control-label col-sm-3 col-md-4">Confirm password</label>
                            <div class="col-sm-8">
                                <input id="confirm_password" name="confirm_password" type="password" onkeyup='check();' class="input-lg form-control" placeholder="confirm password" autocomplete="off" required>
                                <span id='message'></span>
                            </div>
                        </div>
                        <div id="divButton" class="form-group">
                            <div class="col-sm-offset-4 col-sm-9">
                                <input id="#btnSubmit" value="Submit" type="button" name="button" onclick="validate();" class="btn btn-success">
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>
</body>
</html>

