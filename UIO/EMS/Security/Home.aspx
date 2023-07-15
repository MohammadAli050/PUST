<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/MainMasterPage.master" AutoEventWireup="true"
    Inherits="Security_Home" CodeBehind="Home.aspx.cs" %>

<asp:Content ID="Content3" ContentPlaceHolderID="Title" Runat="Server">
   Home
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <link href="../ContentCSS/CSS/bootstrap-4.1.3/css/bootstrap.css" rel="stylesheet" />
    <link href="../ContentCSS/CSS/fontawesome-free-5.9.0-web/css/all.css" rel="stylesheet" />
    <link href="../ContentCSS/CSS/mdb.min.css" rel="stylesheet" />
    <script src="../ContentCSS/CSS/jquery.min.js"></script>
    <script src="../ContentCSS/CSS/bootstrap-4.1.3/js/bootstrap.js"></script>
    
   <style type="text/css">
        .style3 {
            width: 202px;
            height: 28px;
        }
        hr{
            margin-top: 1rem;
            margin-bottom: 1rem;
            border: 0;
            border-top: 1px solid #4CA1D7;

        }
        .Main-Hotline-container {
            width: 950px;
            /*background-color: #d9eaf7;*/
            padding: 10px;
            margin-top: 20px;
            /*margin: 0 auto;*/
            border: 6px #4CA1D7 solid;
            margin-left: auto;
            margin-right: auto;
            text-align: center;
        }

        .pay-btn {
            margin-top: -7px;
        }
       .gallery {
           -webkit-column-count: 3;
           -moz-column-count: 3;
           column-count: 3;
           -webkit-column-width: 33%;
           -moz-column-width: 33%;
           column-width: 33%;
       }

           .gallery .pics {
               -webkit-transition: all 350ms ease;
               transition: all 350ms ease;
           }

           .gallery .animation {
               -webkit-transform: scale(1);
               -ms-transform: scale(1);
               transform: scale(1);
           }

       @media (max-width: 450px) {
           .gallery {
               -webkit-column-count: 1;
               -moz-column-count: 1;
               column-count: 1;
               -webkit-column-width: 100%;
               -moz-column-width: 100%;
               column-width: 100%;
           }
       }

       @media (max-width: 400px) {
           .btn.filter {
               padding-left: 1.1rem;
               padding-right: 1.1rem;
           }
       }
       .btn:hover{
           background-position:0px 50px !important;
           transition:none
              
       }
       .btn-sm{
           color:black;

           background:green;
       }
       .text-success{
           color:#4285f4!important;
       }
    </style>

    <script>
        $(function () {
            var selectedClass = "";
            $(".filter").click(function () {
                selectedClass = $(this).attr("data-rel");
                $("#gallery").fadeTo(100, 0.1);
                $("#gallery div").not("." + selectedClass).fadeOut().removeClass('animation');
                setTimeout(function () {
                    $("." + selectedClass).fadeIn().addClass('animation');
                    $("#gallery").fadeTo(300, 1);
                }, 300);
            });
        });
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContainer" runat="Server">
    
    <div class="container">
        <div class="Main-Hotline-container">
            <div style="clear: both;">
                <hr />
                    <h2 style="color: black; font-family: Calibri;"><b>Welcome to Pabna University of Science and Technology (PUST) UCAM</b></h2> 
                    <h2 style="color: black; font-family: Calibri;"></h2>
                    <h2 style="color: black; font-family: Calibri;"></h2>
                <hr />
                <br />
                <br />
            </div>
        </div>
        <div class="row d-none">
            <div class="col-12">
                <div class="row">
                    <div class="col-12 mt-4">
                        <div class="row">
                            
                            <%--<div class="col-md-4">
                                <div class="card hoverable" >
                                    <img class="card-img-top" style="height:250px"  src="../Images/MBSTU/vc-2.1.jpg" alt="Card image cap"/>
                                        
                                    
                                </div>
                            </div>--%>
                             <div class="col-md-4">
                                <%--<div class="card hoverable" >
                                    <img class="card-img-top" style="height:250px" src="../Images/brur/provc.jpg" alt="Card image cap"/>
                                        
                                    <div class="card-body">
                                        <!-- Title -->
                                        <h4 class="card-title text-success font-weight-bold"><a>Pro-Vice Chancellor</a></h4>
                                        <h5 class="card-title"><a> Dr. Sarifa Salowa Dina</a></h5>
                                        
                                        <!-- Text -->
                                        <p class="card-text">
                                            Professor <br />
                                            Dpt of Bangla Begum Rokeya University, Rangpur.<br /> 
                                            +8801715076779
                                        </p>
                                        <!-- Button -->
                                        
                                    </div>
                                </div>--%>
                            </div>
                             <div class="col-md-4">
                               <%-- <div class="card hoverable" >
                                    <img class="card-img-top" style="height:250px"  src="../Images/brur/mis_hasibrashid.jpg" alt="Card image cap"/>
                                        
                                    <div class="card-body" style="padding-bottom:83px">
                                        <!-- Title -->
                                        <h4 class="card-title text-success font-weight-bold"><a>Treasurer</a></h4>
                                        <h5 class="card-title"><a>Prof. Dr. Hasibur Rashid</a></h5>
                                        
                                        <!-- Text -->
                                        <p class="card-text">
                                            <%--Mobile: +8801720261141 <br />
                                            E-mail: treasurer@brur.ac.bd
                                        </p>
                                        <!-- Button -->
                                        
                                    </div>
                                </div>--%>
                            </div>
                        </div>
                           
                    </div>
                    <div class="col-12 mt-4">
                        <div class="row">

                            <!-- Grid column -->
                            <div class="col-md-12 d-flex justify-content-center mb-3 mt-3">

                                <button type="button" class="btn btn-sm btn-secondary waves-effect filter" data-rel="all">All</button>
                                <button type="button" class="btn btn-sm btn-secondary waves-effect filter" data-rel="1">Class</button>
                                <%--<button type="button" class="btn btn-outline-black waves-effect filter" data-rel="2">Sea</button>--%>

                            </div>
                            <!-- Grid column -->

                        </div>
                        <!-- Grid row -->

                        <!-- Grid row -->
                        <div class="gallery" id="gallery">

                            <!-- Grid column -->
                            <div class="mb-3 pics animation all 1 hoverable">
                                <img class="img-fluid" style="height:205px" src="../Images/MBSTU/class.jpg" alt="Card image cap">
                            </div>
                            <div class="mb-3 pics animation all  hoverable">
                                <img class="img-fluid" style="height:205px;width:100%" src="../Images/MBSTU/1425739767.jpg" alt="Card image cap">
                            </div>
                            <div class="mb-3 pics animation all 1 hoverable">
                                <img class="img-fluid" style="height:205px" src="../Images/MBSTU/class_room.jpg" alt="Card image cap">
                            </div>
                            <!-- Grid column -->

                            <!-- Grid column -->
                            <div class="mb-3 pics animation all  hoverable">
                                <img class="img-fluid" style="height:205px;" src="../Images/MBSTU/glance-1.1.jpg" alt="Card image cap">
                            </div>
                            <!-- Grid column -->

                            <!-- Grid column -->
                            <div class="mb-3 pics animation all  hoverable">
                                <img class="img-fluid" style="height:205px" src="../Images/MBSTU/web-rokeya-begum-university-1530085395281-1536557954755.jpg" alt="Card image cap">
                            </div>
                            <!-- Grid column -->

                            <!-- Grid column -->
                            <div class="mb-3 pics animation all hoverable">
                                <img class="img-fluid" style="height:205px" src="../Images/MBSTU/bgrokeya.jpg" alt="Card image cap">
                            </div>
                            <!-- Grid column -->

                        </div>
                    </div>
                </div>
            </div>        

        </div>
    </div>
</asp:Content>

