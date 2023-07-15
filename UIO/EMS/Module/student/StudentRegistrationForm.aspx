<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true" CodeBehind="StudentRegistrationForm.aspx.cs" Inherits="EMS.Module.student.StudentRegistrationForm" %>


<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<%@ Register Src="~/UserControls/ProgramUserControl.ascx" TagPrefix="uc1" TagName="ProgramUserControl" %>
<%@ Register Src="~/UserControls/DepartmentUserControl.ascx" TagPrefix="uc1" TagName="DepartmentUserControl" %>


<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Student Registration Card
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">

    <script src="https://cdnjs.cloudflare.com/ajax/libs/jspdf/1.3.2/jspdf.min.js"></script>
    <script src="../../../JavaScript/jspdf.debug.js"></script>
    <script type="text/javascript">

        function InProgress() {
            var panelProg = $get('PnProcess');
            panelProg.style.display = 'inline-block';
        }

        function onComplete() {
            var panelProg = $get('PnProcess');
            panelProg.style.display = 'none';
        }

    </script>
    <style>
        #ctl00_MainContainer_txtStudentId {
            height: 40px !important;
            font-size: 20px;
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

        #ctl00_MainContainer_ucDepartment_ddlDepartment, #ctl00_MainContainer_ucProgram_ddlProgram, #ctl00_MainContainer_txtStudentId, #ctl00_MainContainer_ddlHallInfo, #ctl00_MainContainer_btnLoad {
            height: 40px !important;
            font-size: 20px;
        }

        #ctl00_MainContainer_ddlHeldIn {
            height: 40px !important;
            font-size: 20px;
        }

        span.select2-selection.select2-selection--single {
            height: 40px;
        }

        span.select2.select2-container.select2-container--default {
            width: 100% !important;
        }

        .sweet-alert {
            z-index: 10000000 !important;
        }

        td input {
            height: 25px;
            width: 25px;
        }

        #ctl00_MainContainer_gvStudentList_ctl01_chkSelectAll {
            height: 25px;
            width: 25px;
        }
    </style>

    <script>
        function btn_Click() {


            var htmlString = ""
            var mywindow = window.open('', 'PRINT', 'height=' + 800 + ',width=' + 1200);
            htmlString += '<html>';
            htmlString += '<head> <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/css/bootstrap.min.css">'
                + '<style> body{margin: 10px;}'
                + '@media print{html,body{ page-break-after:always; font-family: cursiv; } #mainDiv{margin-top:20px; page-break-after:always} container{page-break-after:always} #secondaryDiv {page-break-after:always} tr{page-break-inside:always;} tr td{page-break-inside:always;}}'
                + '</style></head> ';
            htmlString += '<body >';
            // Body Head
            htmlString += '<div class="container-fluid" id="mainDiv" style="font-family: cursiv;">'

            // logo and UV Name //
            htmlString += '<div class="row">'
            htmlString += '    <div class="col-sm-12 mg-t-12 mg-sm-t-0 gap" style="text-align: center;">'
            htmlString += '        <img src="/mbstu/Images/MBSTU_lgo.png" style="margin-right: 605px; margin-top: 7px; width: 105px; height: 100px;" />'
            htmlString += '        <p style="padding: 0; margin: -90px 0px 10px 55px; font-size: 22px; font-weight: bold; color: black">Mawlana Bhashani Science and Technolgy University</p>'
            htmlString += '       <p style="font-size: 16px; margin-top: -10px; margin-left: 6%; color: black">Santosh, Tangail - 1902,Bangladesh</p>'
            htmlString += '        <p style="font-size: 16px; margin-top: -10px; margin-left: 33.5%; font-family: cursiv; font-weight: bold; background-color: black; color: white; width: 39%;">STUDENT REGISTRATION FORM</p>'
            htmlString += '    </div>'
            htmlString += '</div><br>'

            // Student ID //
            htmlString += '    <div class="row">'
            htmlString += '    <div class="col-lg-12 col-md-12 col-md-12">'
            htmlString += '      <table style="float: right; border: 1px solid black; border-collapse: collapse;">'
            htmlString += '        <tr style="border: 1px solid black; border-collapse: collapse;">'
            htmlString += '          <th colspan="9" style="border: 1px solid black; border-collapse: collapse; text-align: center;">Student ID</th>'
            htmlString += '        </tr>'
            htmlString += '        <tr style="border: 1px solid black; border-collapse: collapse;">'
            htmlString += '          <td style="width: 20%;  border-collapse: collapse;">Registration No.</td>'
            htmlString += '            <td style="border: 1px solid black; border-collapse: collapse;"></td>'
            htmlString += '            <td style="border: 1px solid black; border-collapse: collapse;"></td>'
            htmlString += '            <td style="border: 1px solid black; border-collapse: collapse;"></td>'
            htmlString += '            <td style="border: 1px solid black; border-collapse: collapse;"></td>'
            htmlString += '            <td style="border: 1px solid black; border-collapse: collapse;"></td>'
            htmlString += '            <td style="border: 1px solid black; border-collapse: collapse;"></td>'
            htmlString += '            <td style="border: 1px solid black; border-collapse: collapse;"></td>'
            htmlString += '            <td style="border: 1px solid black; border-collapse: collapse;"></td>'
            htmlString += '        </tr>'
            htmlString += '       </table><br>'
            htmlString += '    </div>'
            htmlString += '</div><br>'

            htmlString += '    <div class="row">'
            htmlString += '    <div class="col-lg-12 col-md-12 col-md-12">'
            htmlString += '    <p style="text-align: center;">(To be filled up by the student)</p>'
            htmlString += '    </div>'
            htmlString += '</div><br>'

            // Form body //
            htmlString += '<div class="form" style="color: black; height: 1300px;">'
            htmlString += '    <div class="row">'
            htmlString += '        <div class="col-lg-3 col-md-3 col-md-3">'
            htmlString += '           <label>1. Name of the Student: i) In Bengali: <br>(According to S.S.C. Certificate)</label>'
            htmlString += '        </div>'
            htmlString += '       <div class="col-lg-9 col-md-9 col-md-9">'
            htmlString += '            <input type="text" id="name" style="width: 100%; float: right;" />'
            htmlString += '        </div>'
            htmlString += '    </div>'
            htmlString += '    <div class="row">'
            htmlString += '        <div class="col-lg-3 col-md-3 col-md-3"><label style="margin-right: 30px;">ii) In English: <br>(Block Letters)</label>'
            htmlString += '       </div>'
            htmlString += '        <div class="col-lg-9 col-md-9 col-md-9">'
            htmlString += '           <input type="text" id="name2" style="width: 100%; float: right;" />'
            htmlString += '        </div>'
            htmlString += '    </div>'
            htmlString += '   <div class="row">'
            htmlString += '      <div class="col-lg-3 col-md-3 col-md-3">'
            htmlString += "            <label>2. Father's Name: </label>"
            htmlString += '       </div>'
            htmlString += '        <div class="col-lg-9 col-md-9 col-md-9">'
            htmlString += '            <input type="text" id="fname" style="width: 100%; float: right;" />'
            htmlString += '        </div>'
            htmlString += '    </div>'
            htmlString += '   <div class="row">'
            htmlString += '       <div class="col-lg-3 col-md-3 col-md-3">'
            htmlString += "    <label>3. Mother's Name:</label>"
            htmlString += '       </div>'
            htmlString += '        <div class="col-lg-9 col-md-9 col-md-9">'
            htmlString += '            <input type="text" id="mname" style="width: 100%; float: right;" />'
            htmlString += '        </div>'
            htmlString += '    </div>'
            htmlString += '   <div class="row">'
            htmlString += '        <div class="col-lg-3 col-md-3 col-md-3">'
            htmlString += '           <label>4. Date of Birth:</label>'
            htmlString += '       </div>'
            htmlString += '        <div class="col-lg-9 col-md-9 col-md-9">'
            htmlString += '            <input type="text" id="bdate" style="width: 100%; float: right;" />'
            htmlString += '        </div>'
            htmlString += '   </div>'
            htmlString += '    <div class="row">'
            htmlString += '        <div class="col-lg-3 col-md-3 col-md-3"><label>5. Legal Guardians Name: </label><br><label>(In absence of parents)</label>'
            htmlString += '        </div>'
            htmlString += '       <div class="col-lg-9 col-md-9 col-md-9">'
            htmlString += '            <input type="text" id="gname" style="width: 100%; float: right;" />'
            htmlString += '        </div>'
            htmlString += '    </div>'
            htmlString += '    <div class="row">'
            htmlString += '        <div class="col-lg-3 col-md-3 col-md-3"><label>6. Permanent Address: </label>'
            htmlString += '       </div>'
            htmlString += '       <div class="col-lg-9 col-md-9 col-md-9">'
            htmlString += '           <input type="text" id="paddress" style="width: 100%; float: right;" />'
            htmlString += '        </div>'
            htmlString += '    </div>'
            htmlString += '    <div class="row">'
            htmlString += '        <div class="col-lg-3 col-md-3 col-md-3"><label>7. Address for Correspondence: </label>'
            htmlString += '        </div>'
            htmlString += '       <div class="col-lg-9 col-md-9 col-md-9">'
            htmlString += '           <input type="text" id="coadd" style="width: 100%; float: right;" />'
            htmlString += '        </div>'
            htmlString += '    </div>'

            htmlString += '    <div class="row">'
            htmlString += '        <div class="col-lg-3 col-md-3 col-md-3"><label>8. Name of the Faculty: </label>'
            htmlString += '       </div>'
            htmlString += '       <div class="col-lg-4 col-md-4 col-md-4">'
            htmlString += '            <input type="text" id="facname" style="width: 100%;" />'
            htmlString += '        </div>'
            htmlString += '       <div class="col-lg-1 col-md-1 col-md-1"><label style="float: right;">Department: </label>'
            htmlString += '        </div>'
            htmlString += '       <div class="col-lg-4 col-md-4 col-md-4">'
            htmlString += '            <input type="text" id="dept" style="width: 100%;float: right;" />'
            htmlString += '        </div>'
            htmlString += '    </div>'

            htmlString += '    <div class="row">'
            htmlString += '       <div class="col-lg-3 col-md-3 col-md-3"><label>9. Name of the Hall: </label><br><label>(Resident/Attached)</label>'
            htmlString += '       </div>'
            htmlString += '       <div class="col-lg-4 col-md-4 col-md-4">'
            htmlString += '            <input type="text" id="resident" style="width: 100%;" />'
            htmlString += '        </div>'
            htmlString += '        <div class="col-lg-1 col-md-1 col-md-1"><label style="float: right;">Session:</label>'
            htmlString += '        </div>'
            htmlString += '        <div class="col-lg-4 col-md-4 col-md-4">'
            htmlString += '           <input type="text" id="session" style="width: 100%; float: right;" />'
            htmlString += '        </div>'
            htmlString += '   </div>'

            // Educational Qualification //
            htmlString += '    <div class="row">'
            htmlString += '        <div class="col-lg-12 col-md-12 col-md-12"><label>10. Precious Academic Records :</label> <br />'
            htmlString += '            <table style="border: 1px solid black; border-collapse: collapse; width: 100%;">'
            htmlString += '                <tr style="border: 1px solid black; border-collapse: collapse; text-align: center;">'
            htmlString += '                    <th style="border: 1px solid black; border-collapse: collapse;" colspan="2">S.S.C./Equivalent</th>'
            htmlString += '                    <th style="border: 1px solid black; border-collapse: collapse;" colspan="2">H.S.C./Equivalent</th>'
            htmlString += '                </tr>'
            htmlString += '                <tr style="border: 1px solid black; border-collapse: collapse; width: 25%;">'
            htmlString += '                    <th style="border: 1px solid black; border-collapse: collapse; width: 25%; text-align: right">Year of Passing</th>'
            htmlString += '                    <td style="border: 1px solid black; border-collapse: collapse; width: 25%;"></td>'
            htmlString += '                   <th style="border: 1px solid black; border-collapse: collapse; width: 25%; text-align: right">Year of Passing</th>'
            htmlString += '                    <td style="border: 1px solid black; border-collapse: collapse; width: 25%;"></td>'
            htmlString += '                </tr>'
            htmlString += '                <tr style="border: 1px solid black; border-collapse: collapse; width: 25%;">'
            htmlString += '                    <th style="border: 1px solid black; border-collapse: collapse; width: 25%; text-align: right">Group</th>'
            htmlString += '                   <td style="border: 1px solid black; border-collapse: collapse; width: 25%;"></td>'
            htmlString += '                    <th style="border: 1px solid black; border-collapse: collapse; width: 25%; text-align: right">Group</th>'
            htmlString += '                    <td style="border: 1px solid black; border-collapse: collapse; width: 25%;"></td>'
            htmlString += '                </tr>'
            htmlString += '               <tr style="border: 1px solid black; border-collapse: collapse; width: 25%;">'
            htmlString += '                    <th style="border: 1px solid black; border-collapse: collapse; width: 25%; text-align: right">Centre</th>'
            htmlString += '                   <td style="border: 1px solid black; border-collapse: collapse; width: 25%;"></td>'
            htmlString += '                    <th style="border: 1px solid black; border-collapse: collapse; width: 25%; text-align: right">Centre</th>'
            htmlString += '                    <td style="border: 1px solid black; border-collapse: collapse; width: 25%;"></td>'
            htmlString += '               </tr>'
            htmlString += '               <tr style="border: 1px solid black; border-collapse: collapse; width: 25%;">'
            htmlString += '                    <th style="border: 1px solid black; border-collapse: collapse width: 25%; text-align: right">Roll no.</th>'
            htmlString += '                   <td style="border: 1px solid black; border-collapse: collapse; width: 25%;"></td>'
            htmlString += '                    <th style="border: 1px solid black; border-collapse: collapse; width: 25%; text-align: right">Roll no.</th>'
            htmlString += '                    <td></td>'
            htmlString += '                </tr>'
            htmlString += '                <tr style="border: 1px solid black; border-collapse: collapse; width: 25%;">'
            htmlString += '                    <th style="border: 1px solid black; border-collapse: collapse; width: 25%; text-align: right">Grade Point/G.P.A.</th>'
            htmlString += '                    <td style="border: 1px solid black; border-collapse: collapse; width: 25%;"></td>'
            htmlString += '                    <th style="border: 1px solid black; border-collapse: collapse; width: 25%; text-align: right">Grade Point/G.P.A.</th>'
            htmlString += '                    <td style="border: 1px solid black; border-collapse: collapse; width: 25%;"></td>'
            htmlString += '                </tr>'
            htmlString += '               <tr style="border: 1px solid black; border-collapse: collapse; width: 25%;">'
            htmlString += '                    <th style="border: 1px solid black; border-collapse: collapse; width: 25%; text-align: right">Name of the School/Institution</th>'
            htmlString += '                    <td style="border: 1px solid black; border-collapse: collapse; width: 25%;"></td>'
            htmlString += '                    <th style="border: 1px solid black; border-collapse: collapse; width: 25%; text-align: right">Name of the School/Institution</th>'
            htmlString += '                    <td style="border: 1px solid black; border-collapse: collapse; width: 25%;"></td>'
            htmlString += '                </tr>'
            htmlString += '                <tr style="border: 1px solid black; border-collapse: collapse; width: 25%;">'
            htmlString += '                    <th style="border: 1px solid black; border-collapse: collapse; width: 25%; text-align: right">Board</th>'
            htmlString += '                   <td style="border: 1px solid black; border-collapse: collapse; width: 25%;"></td>'
            htmlString += '                    <th style="border: 1px solid black; border-collapse: collapse; width: 25%; text-align: right">Board</th>'
            htmlString += '                    <td style="border: 1px solid black; border-collapse: collapse; width: 25%;"></td>'
            htmlString += '                </tr>'
            htmlString += '            </table>'
            htmlString += '       </div>'
            htmlString += '    </div><br><br>'

            htmlString += '    <div class="row">'
            htmlString += '        <div class="col-lg-12 col-md-12 col-md-12">'
            htmlString += '           <p>I do hereby undertake to abide by the University Fundamental Rules, "all political activities of any kind by the students of Mawlana Bhashani Science and Technolgy University (MBSTU) are totally banned on and off the Univerity campus." I will be held responsible for any violation of this rule, or for any damage that I may do to the University property and for any sort of missconduct. I will also follow the proctorial rules as framed for the students and maintain academic discipline.'
            htmlString += '           </p>'
            htmlString += '            <br />'
            htmlString += '        </div>'
            htmlString += '   </div>'

            // Signature //
            htmlString += '   <div class="row" style="margin-top: 60px;">'
            htmlString += '        <div class="col-lg-12 col-md-12 col-sm-12"><label>The above statement of the student are correct.</label>'
            htmlString += '        <label style="text-align: center; text-decoration-line: overline; float: right;">Signature Of the Student </label>'
            htmlString += '   </div>'
            htmlString += '   </div>'

            htmlString += '   <div class="row" style="margin-top: 140px;">'
            htmlString += '        <div class="col-lg-4 col-md-4 col-sm-4"><label style="text-decoration-line: overline;">Signature of the Dealing Officer </label></div>'
            htmlString += '        <div class="col-lg-4 col-md-4 col-sm-4" style=" text-align: center;"><label style="text-decoration-line: overline;">Signature of the Provost </label><br><label style="margin-top: -10px;">(Office seal and date) </label></div>'
            htmlString += '        <div class="col-lg-4 col-md-4 col-sm-4" style="text-align: center;"><label style="text-decoration-line: overline; float: right;">Signature of the Chairman </label></div>'
            htmlString += '   </div><br>'
            htmlString += '   </div>'

            // Office Use Portion //
            htmlString += '<div class="container" id="secondaryDiv" style="font-family: cursiv; padding-right: 15px; padding-left: 15px;">'
            htmlString += '<div class="row">'
            htmlString += '    <div class="col-sm-12 mg-t-12 mg-sm-t-0 gap" style="text-align: center;">'
            htmlString += '<p style="text-align: center; margin-bottom: -25px; font-size: 15px;">(For office use only)</p>'
            htmlString += '        <img src="/mbstu/Images/MBSTU_lgo.png" style="margin-right: 605px; margin-top: 7px; width: 105px; height: 100px;" />'
            htmlString += '        <p style="padding: 0; margin: -90px 0px 10px 55px; font-size: 22px; font-weight: bold; color: black">Mawlana Bhashani Science and Technolgy University</p>'
            htmlString += '       <p style="font-size: 16px; margin-top: -10px; margin-left: 6%; color: black">Santosh, Tangail - 1902,Bangladesh</p>'
            htmlString += '        <p style="font-size: 16px; margin-top: -10px; margin-left: 33.5%; font-family: cursiv; font-weight: bold; background-color: black; color: white; width: 39%;">STUDENT REGISTRATION CARD</p>'
            htmlString += '    </div>'
            htmlString += '</div><br>'

            htmlString += '    <div class="row">'
            htmlString += '        <div class="col-lg-3 col-md-3 col-md-3">'
            htmlString += '           <label>1. Name of the Student: i) In Bengali: <br>(According to S.S.C. Certificate)</label>'
            htmlString += '        </div>'
            htmlString += '       <div class="col-lg-9 col-md-9 col-md-9">'
            htmlString += '            <input type="text" id="name" style="width: 100%; float: right;" />'
            htmlString += '        </div>'
            htmlString += '    </div>'
            htmlString += '    <div class="row">'
            htmlString += '        <div class="col-lg-3 col-md-3 col-md-3"><label>ii) In English: <br>(Block Letters)</label>'
            htmlString += '       </div>'
            htmlString += '        <div class="col-lg-9 col-md-9 col-md-9">'
            htmlString += '           <input type="text" id="name2" style="width: 100%; float: right;" />'
            htmlString += '        </div>'
            htmlString += '    </div>'
            htmlString += '    <div class="row">'
            htmlString += '      <div class="col-lg-3 col-md-3 col-md-3">'
            htmlString += "            <label>2. Father's Name: </label>"
            htmlString += '       </div>'
            htmlString += '        <div class="col-lg-9 col-md-9 col-md-9">'
            htmlString += '            <input type="text" id="fname" style="width: 100%; float: right;" />'
            htmlString += '        </div>'
            htmlString += '    </div>'
            htmlString += '    <div class="row">'
            htmlString += '       <div class="col-lg-3 col-md-3 col-md-3">'
            htmlString += "           <label>3. Mother's Name:</label>"
            htmlString += '       </div>'
            htmlString += '        <div class="col-lg-9 col-md-9 col-md-9">'
            htmlString += '            <input type="text" id="mname" style="width: 100%; float: right;" />'
            htmlString += '        </div>'
            htmlString += '    </div>'

            htmlString += '    <div class="row">'
            htmlString += '        <div class="col-lg-3 col-md-3 col-md-3"><label>8. Name of the Faculty: </label>'
            htmlString += '       </div>'
            htmlString += '       <div class="col-lg-9 col-md-9 col-md-9">'
            htmlString += '            <input type="text" id="facname" style="width: 100%; float: right;" />'
            htmlString += '        </div>'
            htmlString += '    </div>'

            htmlString += '    <div class="row">'
            htmlString += '       <div class="col-lg-3 col-md-3 col-md-3"><label>9. Name of the Hall</label>'
            htmlString += '       </div>'
            htmlString += '       <div class="col-lg-9 col-md-9 col-md-9">'
            htmlString += '            <input type="text" id="resident" style="width: 100%; float: right;" />'
            htmlString += '        </div>'
            htmlString += '    </div><br>'

            htmlString += '    <div class="row" style="margin-top: 20px;">'
            htmlString += '       <div class="col-lg-12 col-md-12 col-md-12">'
            htmlString += '          <label style="float: left;">The above named student has been registered at this University and his/her Registration no. is</label>'
            htmlString += '           <table style="float: right; border: 1px solid black; border-collapse: collapse; width: 35%; height: 34px;">'
            htmlString += '              <tr style="border: 1px solid black; border-collapse: collapse; width: 100%; height: 34px;">'
            htmlString += '                <td colspan="9" style="border: 1px solid black; border-collapse: collapse;"></td>'
            htmlString += '                <td style="border: 1px solid black; border-collapse: collapse; height: 34px;"></td>'
            htmlString += '                <td style="border: 1px solid black; border-collapse: collapse; height: 34px;"></td>'
            htmlString += '                <td style="border: 1px solid black; border-collapse: collapse; height: 34px;"></td>'
            htmlString += '                <td style="border: 1px solid black; border-collapse: collapse; height: 34px;"></td>'
            htmlString += '                <td style="border: 1px solid black; border-collapse: collapse; height: 34px;"></td>'
            htmlString += '                <td style="border: 1px solid black; border-collapse: collapse; height: 34px;"></td>'
            htmlString += '                <td style="border: 1px solid black; border-collapse: collapse; height: 34px;"></td>'
            htmlString += '                <td style="border: 1px solid black; border-collapse: collapse; height: 34px;"></td>'
            htmlString += '              </tr>'
            htmlString += '            </table><br>'
            htmlString += '        </div>'
            htmlString += '    </div>'

            htmlString += '    <div class="row" style="margin-top: 20px;">'
            htmlString += '       <div class="col-lg-8 col-md-8 col-md-8">'
            htmlString += '          <label style="margin-left: 60px;">Administration Building</label><br>'
            htmlString += '          <label style="text-align: center;">Mawlana Bhashani and Technolgy University</label><br>'
            htmlString += '          <label style="margin-left: 30px;">Santosh, Tangail- 1902, Bangladesh</label>'
            htmlString += '        </div>'
            htmlString += '       <div class="col-lg-4 col-md-4 col-md-4" style="margin-top: -95px;">'
            htmlString += '          <label style="margin-left: 905px;">Registrar</label><br>'
            htmlString += '          <label style="margin-left: 906px;">MBSTU</label><br>'
            htmlString += '        </div>'
            htmlString += '    </div>'

            htmlString += '</div>'
            htmlString += '</div>'
            htmlString += '</body></html>'

            mywindow.document.write(htmlString);
        }


        ///Images/MBSTU_logo.png
        const img = new Image();
        img.src = window.location.origin + "/Images/MBSTU_logo.png";

        function btn_Download(htmlString) {
            //console.log("hi");

            var favorite = [];
            var studentRollList = [];
            $.each($("input[type='checkbox']:checked"), function (i, v) {
                let roll = $(v).parent().attr('stdroll')
                //console.log(roll)

                studentRollList.push(roll);

                ////var childObj = $(this).parent().children()[1];
                //console.log(roll);
                //if (roll != undefined) {
                //    if (roll.tagName != "LABEL") {
                //        // console.log(roll);            
                //        //console.log(d);
                //        //favorite.push(roll);
                //        //var obj = { Roll: roll };
                //        studentRollList.push(roll);
                //    }
                //}
            });

            //var RollList = JSON.stringify(studentRollList);

            //console.log(studentRollList);
            <%--var studentRoll = document.getElementById('<%= txtStudentId.ClientID %>').value;--%>

            <%--var name = document.getElementById('<%= studentname.ClientID %>').value;--%>


            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "StudentRegistrationForm.aspx/GetStudentInformationForRegistrationCard",
                data: "{RollList:'" + studentRollList + "'}",
                dataType: "json",
                success: function (data) {

                    var parsed = JSON.parse(data.d)                    var StudentData = JSON.parse(data.d);

                    console.log("Result Data");
                    console.log(StudentData.ResponseData);

                    var doc = new jsPDF();
                    for (var i = 0; i < StudentData.ResponseData.length; i++) {
                        var parsed = StudentData[i];

                        var Roll = StudentData.ResponseData[i].Roll;
                        var StudentName = StudentData.ResponseData[i].NameInUpperCase;
                        var AdmissionSession = StudentData.ResponseData[i].AdmissionSession;
                        var DepartmentName = StudentData.ResponseData[i].DepartmentName;
                        var FacultyName = StudentData.ResponseData[i].FacultyName;
                        var FatherName = StudentData.ResponseData[i].FatherName;
                        var HallName = StudentData.ResponseData[i].HallName;
                        var MotherName = StudentData.ResponseData[i].MotherName;
                        var PhotoPath = StudentData.ResponseData[i].PhotoPath;


                        if (i > 0) {
                            doc.addPage();
                        }

                        doc.setFontSize(9);
                        doc.setTextColor(0, 0, 0);
                        doc.text("Registrar", 170, 125, null, null);
                        doc.setFont("Times New Roman", "normal");

                        doc.setFontSize(9);
                        doc.setTextColor(0, 0, 0);
                        doc.text("MBSTU", 170, 130, null, null);
                        doc.setFont("Times New Roman", "normal");


                        doc.setFontSize(9);
                        doc.setTextColor(0, 0, 0);
                        doc.text("Santosh, Tangail - 1902, Bangladesh", 50, 135, null, null, "center");
                        doc.setFont("Times New Roman", "normal");

                        doc.setFontSize(9);
                        doc.setTextColor(0, 0, 0);
                        doc.text("Mawlana Bhashani Science and Technology University", 50, 130, null, null, "center");
                        doc.setFont("Times New Roman", "normal");

                        doc.setFontSize(9);
                        doc.setTextColor(0, 0, 0);
                        doc.text("Administrator Building", 50, 125, null, null, "center");
                        doc.setFont("Times New Roman", "normal");

                        //Student ID
                        doc.setFontSize(9);
                        doc.setTextColor(0, 0, 0);
                        doc.text(Roll, 158, 94, 0, 0);
                        doc.rect(155, 90, 40, 6);

                        doc.setFontSize(11);
                        doc.setTextColor(0, 0, 0);
                        doc.text("The above named student has been registered at this University and his/her Student ID is ", 14.5, 94.5, null, null);
                        doc.setFont("Times New Roman", "normal");

                        //Admission session
                        doc.setFontSize(9);
                        doc.setTextColor(0, 0, 0);
                        doc.text(AdmissionSession, 72, 86.5, 0, 0);
                        doc.rect(70, 82.5, 125, 6);

                        doc.setFontSize(12);
                        doc.setTextColor(0, 0, 0);
                        doc.text("Admission Session :", 14.5, 86, null, null);
                        doc.setFont("Times New Roman", "normal");


                        //Hall name
                        doc.setFontSize(9);
                        doc.setTextColor(0, 0, 0);
                        doc.text(HallName, 72, 79, 0, 0);
                        doc.rect(70, 75, 125, 6);

                        doc.setFontSize(12);
                        doc.setTextColor(0, 0, 0);
                        doc.text("Name of the Hall :", 30.5, 78.5, null, null, "center");
                        doc.setFont("Times New Roman", "normal");

                        //Department name
                        doc.setFontSize(9);
                        doc.setTextColor(0, 0, 0);
                        doc.text(DepartmentName, 72, 71.5, 0, 0);
                        doc.rect(70, 67.5, 125, 6);

                        doc.setFontSize(12);
                        doc.setTextColor(0, 0, 0);
                        doc.text("Department :", 14.5, 71, null, null);
                        doc.setFont("Times New Roman", "normal");

                        //Faculty name
                        doc.setFontSize(9);
                        doc.setTextColor(0, 0, 0);
                        doc.text(FacultyName, 72, 64, 0, 0);
                        doc.rect(70, 60, 125, 6);

                        doc.setFontSize(12);
                        doc.setTextColor(0, 0, 0);
                        doc.text("Name of the Faculty :", 33, 64, null, null, "center");
                        doc.setFont("Times New Roman", "normal");

                        //Mother name
                        doc.setFontSize(9);
                        doc.setTextColor(0, 0, 0);
                        doc.text(MotherName, 72, 56.5, 0, 0);
                        doc.rect(70, 52.5, 125, 6);

                        doc.setFontSize(12);
                        doc.setTextColor(0, 0, 0);
                        doc.text("Mother's Name :", 29, 57, null, null, "center");
                        doc.setFont("Times New Roman", "normal");

                        //Father name
                        doc.setFontSize(9);
                        doc.setTextColor(0, 0, 0);
                        doc.text(FatherName, 72, 49.5, 0, 0);
                        doc.rect(70, 45, 125, 6);

                        doc.setFontSize(12);
                        doc.setTextColor(0, 0, 0);
                        doc.text("Father's Name :", 28, 50, null, null, "center");
                        doc.setFont("Times New Roman", "normal");

                        //Name English
                        doc.setFontSize(9);
                        doc.setTextColor(0, 0, 0);
                        doc.text(StudentName, 72, 42, 0, 0);
                        doc.rect(70, 37.5, 125, 6);

                        doc.setFontSize(9);
                        doc.setTextColor(0, 0, 0);
                        doc.text("i) In English :", 58, 42, null, null, "center");
                        doc.setFont("Times New Roman", "normal");

                        doc.setFontSize(9);
                        doc.setTextColor(0, 0, 0);
                        doc.text("(According to S.S.C. certificate)", 35, 38, null, null, "center");
                        doc.setFont("Times New Roman", "normal");

                        doc.setFontSize(9);
                        doc.setTextColor(0, 0, 0);
                        doc.text("i) In Bengali :", 58, 35, null, null, "center");
                        doc.setFont("Times New Roman", "normal");

                        doc.setFontSize(12);
                        doc.setTextColor(0, 0, 0);
                        doc.text("Name of Student :", 30, 35, null, null, "center");
                        doc.setFont("Times New Roman", "normal");

                        //Name Bangla
                        doc.setFontSize(9);
                        //doc.text("Bangla", 72, 34.5, 0, 0);
                        doc.setTextColor(0, 0, 0);
                        doc.rect(70, 30, 125, 6);

                        doc.setFontSize(12);
                        doc.setTextColor(0, 0, 0);
                        doc.text("Santosh, Tangail- 1902, Bangladesh", 105, 20, null, null, "center");
                        doc.setFont("Times New Roman", "bold");


                        var imgs = new Image()
                        imgs.src = window.location.origin + '/Upload/Avatar/Student/' + PhotoPath;
                        let extension = imgs.src.split('.').pop();


                        doc.setFontSize(19);
                        doc.setTextColor(0, 0, 0);
                        doc.text("Mawlana Bhashani Science and Technology University", 105, 15, null, null, "center");
                        doc.setFont("Times New Roman", "normal");



                        doc.setFontSize(12);
                        doc.setTextColor(0, 0, 0);
                        doc.text("(For office use only)", 105, 7, null, null, "center");
                        doc.setFont("Times New Roman", "normal");


                        doc.setFontSize(13);
                        doc.setFillColor(0, 0, 0);
                        doc.rect(67.5, 21, 75, 6, 'F');

                        doc.setTextColor(255, 255, 255);
                        doc.text(70.5, 25.5, 'STUDENT REGISTRATION CARD');


                        doc.addImage(img, 'png', 5, 4, 20, 19);
                        imgs.onload = function () {

                            console.log(imgs.src);

                            // this waits till imgs src loads
                            doc.addImage(imgs, extension, 185, 4, 20, 19);
                            //doc.save('RegistrationCard - ' + Roll + '.pdf');
                        };

                    }

                    doc.save('RegistrationCard - ' + Roll + '.pdf');

                }
            });


            //doc.text(20, 20 ,'This is the default font.');

            //doc.setFont("courier", "normal");
            //doc.text("This is courier normal.", 20, 30);

            //doc.setFont("times", "italic");
            //doc.text("This is times italic.", 20, 40);

            //doc.setFont("helvetica", "bold");
            //doc.text("This is helvetica bold.", 20, 50);

            //doc.setFont("courier", "bolditalic");
            //doc.text("This is courier bolditalic.", 20, 60);

            //doc.setFont("times", "normal");
            //doc.text("This is centred text.", 105, 80, null, null, "center");
            //doc.text("This is right aligned text", 200, 100, null, null, "right");
            //doc.text("And some more", 200, 110, null, null, "right");
            //doc.text("Back to left", 20, 120);

            //doc.text("10 degrees rotated", 20, 140, null, 10);
            //doc.text("-10 degrees rotated", 20, 160, null, -10);

            // Save the PDF

        }


    </script>

    <%--   <style>
        .tableForm th, tr, td {
            border: 1px solid black;
            width: 100%;
        }--%>
    <%--<script src="stdregform.js"></script>--%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContainer" runat="server">

    <div class="row">
        <div class="col-sm-12" style="font-size: 12pt; margin-top: 10pt;">
            <label><b style="color: black; font-size: 26px">Student Registration Card</b></label>
        </div>
    </div>

    <hr />

    <%--starts from here--%>
    <asp:UpdatePanel runat="server" ID="UpdatePanel02">
        <ContentTemplate>
            <div class="card">
                <div class="card-body">

                    <div class="row">
                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <b>Choose Department</b>
                            <br />
                            <uc1:DepartmentUserControl runat="server" ID="ucDepartment" OnDepartmentSelectedIndexChanged="ucDepartment_DepartmentSelectedIndexChanged" />
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <b>Choose Program <span style="color: red">*</span></b>
                            <br />
                            <uc1:ProgramUserControl runat="server" ID="ucProgram" OnProgramSelectedIndexChanged="ucProgram_ProgramSelectedIndexChanged" />
                        </div>
                        <div class="col-lg-5 col-md-5 col-sm-5">
                            <%--<script type="text/javascript">
                                Sys.Application.add_load(initdropdown);
                            </script>--%>
                            <b>Choose Semester & Held In<span style="color: red;">*</span></b>
                            <br />
                            <asp:DropDownList ID="ddlHeldIn" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlHeldIn_SelectedIndexChanged"></asp:DropDownList>
                        </div>


                    </div>

                    <div class="row" style="margin-top: 10px">

                        <div class="col-lg-4 col-md-4 col-sm-4">
                            <b>Hall Information</b>
                            <asp:DropDownList ID="ddlHallInfo" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlHallInfo_SelectedIndexChanged"></asp:DropDownList>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <b>Enter Student ID</b>
                            <asp:TextBox ID="txtStudentId" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>

                        <div class="col-lg-2 col-md-2 col-sm-2">
                            <br />
                            <asp:LinkButton ID="lnkLoad" runat="server" Width="100%" Height="40px" type="button" CssClass="btn-info btn-lg" Style="text-align:center;" Font-Size="Small" OnClick="btnLoad_Click">
                                                                    <b>Load Student</b>
                            </asp:LinkButton>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3">
                            <br />
                            <input type="button" class="btn btn-success" style="height: 40px; width: 100%" value="Download Registration Card" onclick="btn_Download()" />
                        </div>
                    </div>

                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>


    <asp:UpdatePanel runat="server" ID="UpdatePanel1">
        <ContentTemplate>

            <div class="card" style="margin-top: 10px">
                <div class="card-body">
                    <asp:GridView runat="server" ID="gvStudentList" AllowSorting="True" CssClass="table table-bordered"
                        AutoGenerateColumns="False" ShowFooter="True" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                        <HeaderStyle BackColor="#4285f4" ForeColor="White" Height="10px" Font-Bold="True" />
                        <FooterStyle BackColor="#4285f4" ForeColor="White" Height="10px" Font-Bold="True" />
                        <AlternatingRowStyle BackColor="White" />
                        <RowStyle Height="10px" />

                        <Columns>

                            <asp:TemplateField HeaderText="SL#">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblSL" Text='<%# Container.DataItemIndex + 1 %>' ForeColor="Black" Font-Bold="true"></asp:Label>

                                </ItemTemplate>
                                <ItemStyle Width="1%" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Student Id">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStudentRoll" Font-Bold="true" Text='<%#  Eval("Roll") %>'></asp:Label>

                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Student Name">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStudentName" Font-Bold="true" Text='<%# Eval("FullName") %>'></asp:Label>

                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Hall Code">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblhallCode" Font-Bold="true" Text='<%# Eval("HallCode") %>'></asp:Label>

                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderStyle-CssClass="header-center">
                                <HeaderTemplate>
                                    <div style="text-align: center">
                                        <asp:Label runat="server" ID="lblStudentRoll" Font-Bold="true" Text=""></asp:Label>
                                    </div>
                                    <div style="text-align: center">

                                        <asp:CheckBox ID="chkSelectAll" runat="server" OnCheckedChanged="chkSelectAll_CheckedChanged"
                                            AutoPostBack="true" />
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div style="text-align: center">
                                        <%--<asp:HiddenField ID="hdnStudentID" runat="server" Value='<%#Eval("StudentID") %>' />
                                        <asp:HiddenField ID="hdnYearId" runat="server" Value='<%#Eval("YearId") %>' />
                                        <asp:HiddenField ID="hdnSemesterId" runat="server" Value='<%#Eval("SemesterId") %>' />--%>
                                        <asp:CheckBox runat="server" ID="ChkActive" stdroll='<%#Eval("Roll")%>'></asp:CheckBox>
                                    </div>
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
                </div>

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <%-- <div class="row" style="margin-top: 10pt;">
        <div class="col-lg-2 col-md-2 col-sm-2">
            <asp:TextBox ID="txtStudentId" runat="server"></asp:TextBox>
        </div>
        <div class="col-lg-2 col-md-2 col-sm-2" runat="server">
            <input type="button" class="btn btn-success" style="height: 40px; margin-left: 30px" value="Download" onclick="btn_Download()" />
        </div>
    </div>--%>

    <%-- <div class="row">
        <div class="col-sm-4 mg-t-4 mg-sm-t-0 gap"></div>
        <div class="col-sm-4 mg-t-4 mg-sm-t-0 gap">
            <label style="margin-top: 25px; text-align: center; font-size: 26px; font-family: 'trebuchet ms', 'lucida sans unicode', 'lucida grande', 'lucida sans', arial, sans-serif; color: black;">to print the registration form, click the <b style="color: blue;">print</b> button</label>
        </div>
        <div class="col-sm-4 mg-t-4 mg-sm-t-0 gap"></div>
    </div>
    <br />

    <div class="row">
        <div class="col-sm-12 mg-t-12 mg-sm-t-0 gap" style="text-align: center;">
            <button id="btnprint" class="btn btn-primary" onclick="btn_click()" style="width: 120px; margin-left: 92px;">print</button>
            <button id="btndownload" class="btn btn-success" style="width: 120px;" onclick="btn_download()">download</button>
            <input type="button" class="btn btn-success" onclick="btn_download()" value="download" />
        </div>
    </div>--%>
</asp:Content>
