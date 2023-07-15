<%@ Page Title="Exam Committee Dashboard" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true"
    CodeBehind="ExamCommitteeDashboard.aspx.cs" Inherits="EMS.Module.result.ExamCommitteeDashboard" %>

<%@ Import Namespace="AjaxControlToolkit.HTMLEditor.ToolbarButton" %>

<%@ Register Src="~/UserControls/ProgramUserControl.ascx" TagPrefix="uc1" TagName="ProgramUserControl" %>
<%@ Register Src="~/UserControls/AdmissionSessionUserControl.ascx" TagPrefix="uc1" TagName="AdmissionSessionUserControl" %>
<%@ Register Src="~/UserControls/DepartmentUserControl.ascx" TagPrefix="uc1" TagName="DepartmentUserControl" %>


<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Exam Committee Dashboard
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <style type="text/css">
        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=80);
            opacity: 0.8;
            z-index: 10000;
        }

        .marginTop {
            margin-top: -5px;
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
    </style>
    <link href="../../CSS/select2.min.css" rel="stylesheet" />
    <script src="../../JavaScript/select2.min.js"></script>

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
            setTimeout(function ()
            {
                document.getElementById('imgloadinggif').style.display = 'block';
            }, 200);
            deleteCookie();

            var timeInterval = 500; // milliseconds (checks the cookie for every half second )

            var loop = setInterval(function () {
                if (IsCookieValid()) {
                    document.getElementById('imgloadinggif').style.display =
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

        function print1(val) {
            console.log(val);

            //var d = $('#'+val).data('CommandArgument');
            //console.log(d);
        }

        
        function print(val)
        {
            
            //var ToId = val.split('_');
            var SecId = $('#' + val)[0].getAttribute("data");
            //var courseName = val[1];            
            //console.log(ToId);
            console.log(SecId);
            
            var courseCode= $('#'+val).parent().children()[7].innerHTML;
            var courseName  = $('#' + val).parent().children()[6].innerHTML;
            //var courseName  = 0;
            console.log('Sakim');
            console.log(courseCode);
            console.log(courseName);
            var session = $("#ctl00_MainContainer_ucSession_ddlSession").val(); 
            //var course = $("#ctl00_MainContainer_ddlCourse").val();
            var sessionName = $("#ctl00_MainContainer_ucFilterCurrentSession_ddlSession option:selected").text();
            
            var yearNo = $("#ctl00_MainContainer_ddlYearNo option:selected").text(); 
            var semesterNo = $("#ctl00_MainContainer_ddlSemesterNo option:selected").text();
            var exam = $("#ctl00_MainContainer_ddlExam option:selected").text();
            var programName = $("#ctl00_MainContainer_ucProgram_ddlProgram option:selected").text();
            var departmentName = $("#ctl00_MainContainer_ucDepartment_ddlDepartment option:selected").text();


            
            
            //var section = ddlsection.options[ddlsection.selectedIndex].value;
         
            var examList = [];
            var ary = "";
            
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "ExamCommitteeDashboard.aspx/GetExamMarks",
                data: "{'strAcaCalSectionId':'" + SecId + "'}",

                dataType: "json",

                success: function (data) {
                    // onComplete();
                    var Array = JSON.parse(data.d);
                    //console.log(Array);

                    var parsed = Array.list1;
                    var parsed1 = Array.list2;
                    console.log(parsed);
                    console.log(parsed1);
                    //console.log(findDistinctStudent(parsed));
                    if (parsed.length == 0) {
                        alert("No data found");
                    }
                    else {
                        var mywindow = window.open('', 'PRINT', 'height=' + 800 + ',width=' + 1600);
                        var startHead = '<html><head> <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.0/css/bootstrap.min.css"><style>';

                        mywindow.document.write(startHead);
                        mywindow.document.write(FormCssString());
                        mywindow.document.write(makeResultTable(parsed, parsed1, departmentName, programName, sessionName, courseName,courseCode, yearNo, semesterNo, exam));

                        mywindow.document.write('</html>');


                        ////var mywindow = window.open("", "_blank");
                        //var html = getHtml1(parsed);
                        //mywindow.document.write(html);
                        mywindow.document.close();
                        mywindow.focus();
                    }
                },
                error: function (e) {
                    console.log(e);
                },
            });
            //alert(ary);
        }
        function makeResultTable(parsed, parsed1, departmentName, programName, sessionName, courseName,courseCode, yearNo, semesterNo, exam)
        {
            var disExamList = findDistinctExam(parsed);
            var disStudentList = findDistinctStudent(parsed);
            var middleContent = "";
            var page = 0;
            var stdTotal = 0;
            var tableHeader = FormTableHeader(disExamList);
            var upperContent = FormUpperContent(parsed1,departmentName, programName, sessionName, courseName,courseCode, yearNo, semesterNo, exam);
            var footerContent = FormFooterContent(parsed1);
            middleContent += upperContent;
            middleContent += tableHeader;
          
            // console.log(disStudentList.length);
            for (var i = 0; i < disStudentList.length; i++)
            {
                stdTotal = 0;
                if(i % 25 == 0 && i != 0 )
                {
                    page++;
                    middleContent += "</tbody></table>";
                    

                    middleContent += "</div><div class='col-sm-1'></div></div>";
                    middleContent += footerContent;
                    if(page>1)
                    {
                        middleContent += "</div>";
                    }
                    middleContent += "<div id='newPage'>";
                    middleContent += upperContent;

                    middleContent += tableHeader;
                }

                //  var regNo = disStudentList[i].RegistrationNo == null ? "-" : disStudentList[i].RegistrationNo ;
               
                middleContent += "<tr>";
                middleContent += "<td>" + (i + 1) + "</td>";
                middleContent += "<td>" + disStudentList[i].Roll + "</td>"


                //  middleContent += "<td>" + disStudentList[i].FullName + "</td>";
                for(var j=0;j<disExamList.length; j++)
                {
                    var crcObj = parsed.find(x=>x.Roll === disStudentList[i].Roll  && x.ExamName === disExamList[j].ExamName);
                    if(crcObj != null || crcObj != undefined)
                    {
                        middleContent += "<td>" + crcObj.MarksOrGrade + "</td>";
                        stdTotal += parseInt(crcObj.MarksOrGrade) ;
                        // middleContent += "<td>" + crcObj.ObtainedGPA.toFixed(2) + "</td>";
                    }
                    else
                    {
                        middleContent += "<td>" + "-" + "</td>";
                        //middleContent += "<td>" + "-" + "</td>";
                    }
                  


                }
                //middleContent += "<td>" + stdTotal.toFixed(2) + "</td>";
       

                middleContent += "</tr>";

                if(i == disStudentList.length - 1)
                {
                    middleContent += "</tbody></table>";

                    middleContent += "</div><div class='col-sm-1'></div></div>";
                    middleContent += footerContent;

                    middleContent += "</div>";

                }




            }

            middleContent += "</div><div class='col-sm-1'></div></div>";



            return middleContent;




          
        }

        function findDistinctExam(parsed)
        {
            var flags = [], distinctExam = [], l = parsed.length, x;
            var StGPA = []; FlagStGpa = []; Flag2 = []; uniqueRegName = [];
            var summary = [];
            for (x = 0; x < l; x++) {
                if (flags[parsed[x].ExamName]) continue;
                flags[parsed[x].ExamName] = true;
                distinctExam.push(parsed[x]);
            }
            distinctExam.sort((a, b) => (a.ColumnSequence > b.ColumnSequence) ? 1 : -1)
            return distinctExam;
        }
        function findDistinctStudent(parsed) {
            var flags = [], distinctStudent = [], l = parsed.length, x;
            var StGPA = []; FlagStGpa = []; Flag2 = []; uniqueRegName = [];
            var summary = [];
            for (x = 0; x < l; x++) {
                if (flags[parsed[x].Roll]) continue;
                flags[parsed[x].Roll] = true;
                //console.log(parsed[x].Roll);
                distinctStudent.push(parsed[x]);
            }
            distinctStudent.sort((a, b) => (parseInt(a.Roll) > parseInt(b.Roll)) ? 1 : -1)

            return distinctStudent;
        }
        function FormCssString() {
            var CssString = "";


            CssString += 'div.header {display:s block;position:fixed;}  #newPage{page-break-before: always;}  #customers{page-break-inside:auto;font-family: "Trebuchet MS", Arial, Helvetica, sans-serif;font-size:14px;border-collapse: collapse;';
            CssString += 'width:100%;}#customers thead{}#customers td{border: 1px solid rgb(0,0,0);padding: 2px; } #customers th {border: 1px solid rgb(0,0,0);';
            CssString += 'padding:4px; text-align: center; background-color: grey;} #customers td{text-align:center;}';
            CssString += '#customers  tr.noBorderRow { border: 0;font-size:20px;} #customers  td.noBorderFoot { border: 0;font-size:20px;} ';
            CssString += '#customers  th.noBorder { border: 0;font-size:20px;} #customers td.t{text-align:center;} ';
            CssString += '#customers td.crcl{width: 20px; height: 20px;border: 2px solid rgb(0,0,0);margin-left:20px;border-radius: 50%; font-size: 12px;color: black; line-height: 20px;text-align:center;}';
            CssString += ' #customers tr:nth-child(even) {  background-color: white;}';
            CssString += 'p.Logo{padding-left: 40px; }'
            CssString += '#customers tr {page-break-after:auto}#customers tr:hover {backgrosund-color: white;}  .footer {position: fixed;  bottom: 0;} .header2 {position: fixed;  top: 0;}';
            CssString += '</style></head>';
            //mywindow.document.write('@media print #customers {backgrosund-color: grey; -webkit-print-color-adjust: exact !important;}');

            // mywindow.document.write('#customers th {');


            return CssString;

        }
        function FormTableHeader(disExamList)
        {
            
            var middleContent = "";
            //var disCourseList = findDistinctCourse(parsed);
            var FullExamName = " "; // Modify
            var numOfCourses = 0; // Modify
            middleContent += "<div class='row' style='padding-top:10px;'>";
            middleContent += "<div class='col-sm-1'></div>";
            middleContent += "<div class='col-sm-10'>";
            middleContent += "<table id='customers'><thead>";
            middleContent += "<tr><th rowspan='2' style='width:50px;' >SL No</th>";
            middleContent += "<th rowspan='2'> ID No </th>";
            //middleContent += "<th rowspan='2'>  ID No </th>";
            //for(var i= 0 ; i<disExamList.length; i++)
            //{
            //    middleContent += "<th> "+disExamList[i].ExamName+" </th>";
            //}
            middleContent += "<th rowspan='2' style='width:85px'> Class Attendance<br/>(5%) </th>";
            middleContent += "<th colspan='3'> Quiz/In course/Class <br/>Test (10%) </th>";
            middleContent += "<th colspan='3'> Assignment/Presentation/<br/>Class Performance(10%)</th>";
            middleContent += "<th rowspan='2'> Mid<br/> Semester<br/>(25%)</th>";
            middleContent += "<th rowspan='2'> Total</th>";
            middleContent += "</tr>";
            middleContent += "<th > 1st</th>";
            middleContent += "<th > 2nd</th>";
            middleContent += "<th > Total</th>";
            middleContent += "<th > Assignment </th>";
            middleContent += "<th > Presentation/<br/>Class Performance </th>";
            middleContent += "<th > Total</th></tr>";
            middleContent += "<tr>";
            middleContent += "<th> 1 </th>";
            middleContent += "<th> 2 </th>";
            middleContent += "<th> 3 </th>";
            middleContent += "<th> 4 </th>";
            middleContent += "<th> 5 </th>";
            middleContent += "<th> 6 </th>";
            middleContent += "<th> 7 </th>";
            middleContent += "<th> 8 </th>";
            middleContent += "<th> 9 </th>";
            middleContent += "<th> 10 </th>";
            middleContent += "<th> 11 </th>";
            middleContent += "</tr>";







            middleContent += "</thead>" + "<tbody>";
            return middleContent;
        }

        function FormUpperContent(parsed1,departmentName, programName, sessionName, courseName,courseCode, yearNo, semesterNo, exam)
        {
            
            var totalExam = exam.split(" ");
            //var splitSession = sessionName.split(",");
            console.log(totalExam);
            //console.log(splitSession);
            //var examSplit = examName.split(" ");
            //console.log(examSplit);
            if (yearNo == '1') {
                var year = '1st';
            }
            else if (yearNo == '2') {
                var year = '2nd';
            }
            else if (yearNo == '3') {
                var year = '3rd';
            }
            else if (yearNo == '4') {
                var year = '4th';
            }
            else if (yearNo == '5') {
                var year = '5th';
            }
            else {

            }


            if (semesterNo == '1') {
                var semester = '1st';
            }
            else if (semesterNo == '2') {
                var semester = '2nd';
            }
            else if (semesterNo == '3') {
                var semester = '3rd';
            }
            else if (semesterNo == '4') {
                var semester = '4th';
            }
            else if (semesterNo == '5') {
                var semester = '5th';
            }
            else {

            }
            var upperContent = "";
            var logo = '<p class="Logo text-right"><img src="../../Images/BrurNewLogo.PNG"  alt=""  width=auto height=70></p>';
            //mywindow.document.write('</div><div class="col-sm-2" style:"padding-left:0px;></div></div>');
            upperContent += '<div class="row"><div class = "col-xs-3" style="padding-top:37px;">';
            upperContent += ''+ logo+'</div><div class = "col-xs-7" style=";padding-top:32px;">';
            upperContent += '<div class="row"  style="font-size: 22px;font-weight: bold;text-align:center;">'+"BEGUM ROKEYA UNIVERSITY, RANGPUR"+'</div>';
            upperContent += '<div class="row" style="font-size: 17px;text-align:center;">'+"Office of the Controller of Examinations"+'</div>';
            //upperContent += '<div class="row" style="font-size: 16px;font-weight: bold;text-align:center;">' + year + " Year " + semester + " Semester Final Examination " + splitSession[0] + '</div>';
            upperContent += '<div class="row" style="font-size: 16px;font-weight: bold;text-align:center;">' + yearNo + " Year " + semesterNo + ' Semester Final Examination - ' + totalExam[5] +'</div>';
            upperContent += '<div class="row" style="font-size: 16px;font-weight: bold;text-align:center;">'+"Mark Sheet (Continuous Assesment)"+'</div>';



            //upperContent += '<div class="row" style="padding-left:150px; font-size: 17px;">'+"Result Sheet"+'</div>';
            //upperContent += '<div class="row">'+ splitProg[0] + " " + yearName + " " + semesterName + " "+ "Final Examination - "+ examSplit[1] +'</div>';
            //// upperContent += '<div class="row">'+"Economics" +'</div></div><div class ="col-sm-2" style=""></div></div>';
            upperContent += '</div><div class ="col-sm-1" style=""></div></div>';
            upperContent += '<div class="row" style="padding-left:0px;padding-top:30px;"><div class = "col-xs-5" style="font-weight: bold;"> Department :'+ departmentName+'</div>';

            //upperContent += '<div class = "col-sm-4" style="margin-left:-110px;font-weight: bold;"> Session : ' + splitSession[1] + '</div>';
            upperContent += '<div class = "col-xs-3"></div>'; 
            upperContent += '<div class = "col-xs-4" style="font-weight: bold;"> Session: ' + parsed1.SessionName +' </div></div>';
            upperContent += '<div class="row"  style="padding-left:0px;padding-top:10px;"><div class = "col-xs-4" style="font-weight: bold;">Total Marks : ' + parsed1.ExamMark+ '</div>';
            upperContent += '<div class = "col-xs-4" style="font-weight: bold;"> Course Code : ' + courseCode + '</div>';
            //upperContent += '<div class = "col-sm-3" style="margin-left:-110px; font-weight: bold;"> Course Code : </div>';
            upperContent += '<div class = "col-xs-4" style="font-weight: bold;"> Course Title : ' + courseName +' </div></div>';
            //upperContent += '<div class = "col-sm-5" style="margin-left:-40px; font-weight: bold;"> Course Title :  </div></div>';

            // upperContent += '<div class="row"><h2>'+"Mark Sheet(Continuous Assesment)"+'</h2></div>';

            //upperContent += '<div class= "row"> <div class=col-sm-1></div>';
            //upperContent += '<div class=col-sm-10>________________________________________________________';
            //upperContent += '_______________________________________________________________________</div>';
            //upperContent += '<div class=col-sm-1></div></div>';

            //upperContent += '<div class= "row"> <div class=col-sm-5></div>';
            //upperContent += '<div class=col-sm-4 style="padding-left: 85px;font-size: 16px;font-weight: bold;"> Department : ' + splitProg[2]  ;
            //upperContent += '</div>';
            //upperContent += '<div class=col-sm-3></div></div>';
            //upperContent += '</div>'

            //upperContent +='<p class="b" style="font-weight:bold;">'+"BSS in Economy"+'</p></div></div>';
            //upperContent += '<div class="row"><div class = "col-sm-5" style=""><p class="b" style="line-height:8px;font-weight:bold;margin-top:3px;">'+ "1st Yesr 1st Semester" +'</p></div><div class = "col-sm-7" style="font-size:14px;font-weight:bold;padding-left:0px;padding-top:0px;" >Result Sheet</div></div>';
                    
            //upperContent += '<div class="row"><div class = "col-sm-6" style=""><p class="b" style="line-height:5px;font-weight:bold;">Final Exam 2010</p></div><div class = "col-sm-6"></div></div>';
            return upperContent;
        }

        function FormFooterContent(parsed1)
        {
            var today = new Date();
            var dd = String(today.getDate()).padStart(2, '0');
            var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
            var yyyy = today.getFullYear();

            today = dd + '.' + mm + '.' + yyyy;
            var footerContent = ""
            footerContent += "<div class='row' style='padding-top:25px;'>";
            footerContent += "<div class='col-xs-5' style='font-weight:bold;' >" + "Date: "+today  + "</div>";
            footerContent += "<div class='col-xs-2'></div>";
            footerContent += "<div class='col-xs-5' style='font-weight:bold;'>" + "Examiner's Signature"+ "</div></div>";

            footerContent += "<div class='row' style='padding-top:5px;'>";
            footerContent += "<div class='col-xs-1'></div>"
            footerContent += "<div class='col-xs-4' ></div>";
            footerContent += "<div class='col-xs-2'></div>";
            footerContent += "<div class='col-xs-5' style='text-align:left;font-weight:bold;'>Examiner Name: " + parsed1.TeacherName +"</div></div>";

            footerContent += "<div class='row' style='padding-top:5px;'>";
            footerContent += "<div class='col-xs-1'></div>"
            footerContent += "<div class='col-xs-4' ></div>";
            footerContent += "<div class='col-xs-2'></div>";
            footerContent += "<div class='col-xs-5' style='text-align:left;font-weight:bold;'>" + "Designation: .............................."+ "</div></div>";

            footerContent += "<div class='row' style='padding-top:5px;'>";
            footerContent += "<div class='col-xs-1'></div>"
            footerContent += "<div class='col-xs-4' ></div>";
            footerContent += "<div class='col-xs-2'></div>";
            footerContent += "<div class='col-xs-5' style='text-align:left;font-weight:bold;'>" + "Department: " + parsed1.DepartmentName +""+ "</div></div>";

      

            return footerContent;


        }


    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContainer" runat="server">
    <div>
        <div class="PageTitle">
            <label>Exam Committee Dashboard</label>
        </div>

        <asp:UpdatePanel ID="UpdatePanel01" runat="server">
            <ContentTemplate>
                <div class="Message-Area">
                    <label class="msgTitle">Message: </label>
                    <asp:Label runat="server" class="msgTitle" ID="lblMsg" ForeColor="Red" Text="" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <div id="divProgress" style="display: none; z-index: 1000; position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%);">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Img/Waiting.gif" Height="150px" Width="150px" />
        </div>

        <asp:UpdatePanel ID="UpdatePanel02" runat="server">
            <ContentTemplate>
                <div class="Message-Area">
                    <table>
                        <tr>
                            <td class="auto-style4">
                                <b>Department :</b>
                            </td>
                            <td>
                                <uc1:DepartmentUserControl runat="server" ID="ucDepartment" OnDepartmentSelectedIndexChanged="OnDepartmentSelectedIndexChanged" />
                            </td>
                            <td class="auto-style4">
                                <b>Program :</b>
                            </td>
                            <td class="auto-style6">
                                <uc1:ProgramUserControl runat="server" ID="ucProgram" OnProgramSelectedIndexChanged="OnProgramSelectedIndexChanged" />
                            </td>
                            <td class="auto-style4"><b>Session :</b></td>
                            <td class="auto-style2">
                                <uc1:AdmissionSessionUserControl runat="server" ID="ucFilterCurrentSession" class="margin-zero dropDownList" />
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <%--<td class="auto-style4">
                            <asp:Label ID="Label3" Width="120px" runat="server" Text="Versity Session"></asp:Label>
                            </td>
                            <td class="auto-style5">
                                <uc1:AdmissionSessionUserControl Width="150px" runat="server" ID="ucVersitySession" class="margin-zero dropDownList"/>
                            </td>--%>
                            <td class="auto-style4">
                                <asp:Label ID="Label8" runat="server" Text="Year : "></asp:Label></td>
                            <td class="auto-style6">
                                <asp:DropDownList ID="ddlYearNo" Width="150px" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlYearNo_SelectedIndexChanged"></asp:DropDownList>
                            </td>
                            <td class="auto-style4">
                                <asp:Label ID="Label9" runat="server" Text="Semester : "></asp:Label></td>
                            <td class="auto-style6">
                                <asp:DropDownList ID="ddlSemesterNo" Width="150px" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlSemesterNo_SelectedIndexChanged"></asp:DropDownList>
                            </td>
                            <td class="auto-style4">
                                <asp:Label ID="Label10" runat="server" Text="Exam : "></asp:Label>
                            </td>
                            <td class="auto-style6">
                                <asp:DropDownList ID="ddlExam" Width="250" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlExam_SelectedIndexChanged"></asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btnLoad" runat="server" Text="Load" OnClick="btnLoad_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="Message-Area">
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="btnTabulationProcess" runat="server" Text="Process Tabulation" CssClass="btn-primary" OnClick="btnTabulationProcess_Click" />
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnView" Text="Preview Tabulation" CssClass="btn-success" OnClientClick="jsShowHideProgress();" OnClick="btnView_Click" Height="20pt" Width="120pt" Style="text-align: center;" />
                            </td>
                            <td>
                                <asp:Button runat="server" ID="btnLoadTabulation" Text="Download Tabulation" OnClientClick="jsShowHideProgress();" OnClick="btnLoadTabulation_Click" CssClass="btn-info" Height="20pt" Width="120pt" Style="text-align: center;" />
                            </td>
                            <td>
                                <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-sm w-100 btn-secondary" Text="Add / Edit Examiner Submission Date" OnClick="btnAdd_Click"></asp:Button>
                            </td>
                            <td>
                                <div style="display: none; z-index: 1000; position: fixed; top: 70%; left: 50%; transform: translate(-50%, -50%); width: 500px; height: 500px" id="imgloadinggif">
                                    <%--<image id="image1" 
                                        src="../../Images/Img/loader.gif" alt="loading.." />--%>
                                    <asp:Image ID="Image2" runat="server" src="../../Images/Img/loader.gif" alt="loading.." />
                                    <asp:Label ID="Label5" runat="server" Text="Processing your request..........." ForeColor="Blue" Font-Bold="true" Font-Size="Large"></asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                        </tr>
                    </table>
                </div>
                <%--   </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
            <ContentTemplate>--%>

                <br />
                <asp:GridView ID="gvExamCommitteDashboard" runat="server" AllowSorting="True" CssClass="table-bordered"
                    AutoGenerateColumns="False" ShowFooter="True" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None"
                    OnRowCommand="gvExamCommitteDashboard_RowCommand">
                    <HeaderStyle BackColor="#1C5E55" ForeColor="White" Height="30" Font-Bold="True" HorizontalAlign="Center" />
                    <FooterStyle BackColor="#1C5E55" ForeColor="White" Height="30" Font-Bold="True" />
                    <AlternatingRowStyle BackColor="White" />
                    <RowStyle Height="25" />
                    <Columns>
                        <asp:TemplateField Visible="false" HeaderText="Student Id">
                            <ItemTemplate>
                                <asp:Label ID="lblAcacalSectionId" runat="server" Text='<%# Bind("AcaCal_SectionID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField Visible="false" HeaderText="Student Id">
                            <ItemTemplate>
                                <asp:Label ID="lblCourseId" runat="server" Text='<%# Bind("CourseID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField Visible="false" HeaderText="Student Id">
                            <ItemTemplate>
                                <asp:Label ID="lblVersionId" runat="server" Text='<%# Bind("VersionID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField>
                            <HeaderTemplate>
                                <div style="text-align: center">
                                    <asp:CheckBox ID="cbSelectAll" runat="server"
                                        AutoPostBack="true" OnCheckedChanged="cbSelectAll_CheckedChanged" />
                                    All                                       
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div style="text-align: center">
                                    <asp:CheckBox runat="server" ID="cbSelect"></asp:CheckBox>
                                </div>
                            </ItemTemplate>
                            <HeaderStyle Width="30px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="SI." ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate><b><%# Container.DataItemIndex + 1 %></b></ItemTemplate>
                            <HeaderStyle Width="30px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Course Exam Info">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblCourseCode" Text='<%# "<b>Course Code : </b>"+Eval("CourseCode") %>' />
                                <br />
                                <asp:Label runat="server" ID="lblCourseName" Text='<%# "<b>Course Name : </b>"+Eval("CourseName") %>' />
                                <br />
                                <asp:Label runat="server" ID="lblYear" Text='<%# "<b>Year : </b>"+Eval("YearNo") %>' />
                                <br />
                                <asp:Label runat="server" ID="lblSemester" Text='<%#"<b>Semester : </b>"+Eval("SemesterNo") %>' />
                                <br />
                                <asp:Label runat="server" ID="lblExam" Text='<%#"<b>Exam : </b>"+Eval("ExamName") %>' />
                                <br />
                                <asp:Label runat="server" ID="lblTearcherOneName" Text='<%#"<b>Course Teacher : </b>"+Eval("TearcherOneName") %>' />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left"  Width="200px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Status" Visible="false">
                            <ItemTemplate>

                                <asp:Label ID="lblIsContinousMarkSubmit" runat="server" Text='<%# Bind("IsContinousMarkSubmit") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Continuous Exam">
                            <ItemTemplate>
                                <asp:Label ID="LabelContinuousSubDate" runat="server" Text='<%# Eval("IsContinousMarkSubmit").ToString() == "0" ? "" : "Submitted On: " %>'></asp:Label>
                                <asp:Label runat="server" ID="lblSubmitToCommittee" DataFormatString="{0:dd-MMM-yy}" Text='<%# Eval("IsContinousMarkSubmit").ToString() == "0" ? "" : Convert.ToDateTime(Eval("FinalSubmissionDate")).ToShortDateString() %>' />
                                <%--<asp:LinkButton ID="BackButton" CommandName="ContinuousExamBack" Text="Back" Enabled='<%#(Eval("IsContinousMarkSubmit")).ToString() == "1" ? false : true %>'
                                 CommandArgument='<%# Bind("AcaCal_SectionID") %>' runat="server"></asp:LinkButton>--%>
                                <br />
                                <br />
                                <asp:Button ID="btnContinuousExamBack" CommandName="ContinuousExamBack" Text="Back" runat="server" CommandArgument='<%# Bind("AcaCal_SectionID") %>' OnClientClick="return confirm('Do you really want back exam mark?');" />
                                <%--<asp:Button ID="c_view" CommandName="ContinuousExamView" Enabled='<%# Eval("ExaminerMarkSubmissionStatus").ToString() == "0" ? false : true  %>' Text="View" runat="server" Data='<%# Eval("AcaCal_SectionID") %>' OnClientClick="print(this.id);" />--%>
                                
                                <asp:UpdatePanel runat="server" ID="up1">
                                    <ContentTemplate>
                                          <asp:Button ID="btnContinuousView" CommandName="ContinuousView" Enabled='<%# Eval("ExaminerMarkSubmissionStatus").ToString() == "0" ? false : true  %>' Text="View" runat="server" CommandArgument='<%#Eval("AcaCal_SectionID")%>' OnClick="btnContinuousView_OnClick" />
                              
                                    </ContentTemplate>
                                    <Triggers>
                                          <asp:PostBackTrigger ControlID="btnContinuousView" />
                                    </Triggers>
                                </asp:UpdatePanel>
                                <%--<button id="c_<%# Eval("AcaCal_SectionID") %>" onclick="print(this.id)">View</button>--%>
                                <span style="display: none"><%# Eval("CourseName") %></span>
                                <span style="display: none"><%# Eval("CourseCode") %></span>
                            </ItemTemplate>
                            <ItemStyle CssClass="center"  Width="160px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="1st Examiner">
                            <ItemTemplate>

                                <asp:Label ID="lblFirstExaminerName" runat="server" Text=' <%#"<b>Examiner: </b>"+ Eval("FirstExaminerName") %>'></asp:Label>
                                <br />
                                <asp:Label ID="lblFirstExaminerMarkSubmissionStatus" runat="server" Text='<%# Eval("FirstExaminerMarkSubmissionStatus").ToString() == "0" ? "Not Submitted" : "Submitted" %>' Font-Bold="True" ForeColor="Red"></asp:Label>
                                <br />
                                
                                <asp:Label ID="LabelSubmissionDate1" runat="server" Text="Submission Deadline: "></asp:Label>
                                <asp:Label ID="LabelSubmissionDate11" runat="server" DataFormatString="{0:dd-MM-yyyy}" Text='<%#Eval("FirstExaminerMarkSubmissionDate") == null  ? "" : Convert.ToDateTime(Eval("FirstExaminerMarkSubmissionDate")).ToShortDateString() %>'></asp:Label>
                                <br /> 
                                <asp:Label ID="Label1Sub" runat="server" Text='<%# Eval("FirstExaminerMarkSubmissionStatus").ToString() == "0" ? "" : "Submitted On: " %>'></asp:Label>
                                <asp:Label ID="lblFirstExaminerSubmissionDate" runat="server" DataFormatString="{0:dd-MM-yyyy}" Text='<%# Eval("FirstExaminerMarkSubmissionStatus").ToString() == "0" ? "" : Convert.ToDateTime(Eval("FirstExaminerMarkSubmissionStatusDate")).ToShortDateString() %>'></asp:Label>
                                <br />
                                <asp:Button ID="btnFirstExaminerBack" CommandName="ExaminerExamBack1" Enabled='<%# Eval("FirstExaminerMarkSubmissionStatus").ToString() == "0" ? false : true  %>' Text="Back" runat="server" CommandArgument='<%# Bind("AcaCal_SectionID") %>' OnClientClick="return confirm('Sure to back exam mark?');" />
                                <%--<asp:Button ID="btnExaminerView1" CommandName="ExaminerExamView1" Enabled='<%# Eval("FirstExaminerMarkSubmissionStatus").ToString() == "0" ? false : true  %>' Text="View" runat="server" CommandArgument='<%#Eval("AcaCal_SectionID")+","+ Eval("ExamSetupId") %>' OnClick="btnViewFirstExaminer_Click" />--%>
                                <asp:Button ID="btnExaminerView1" CommandName="ExaminerExamView1" Enabled='<%# Eval("ExaminerMarkSubmissionStatus").ToString() == "0" ? false : true  %>' Text="View" runat="server" CommandArgument='<%#Eval("AcaCal_SectionID")+","+ Eval("ExamSetupId") %>' OnClick="btnViewFirstExaminer_Click" />
                            </ItemTemplate>
                            <ItemStyle CssClass="center" Width="250px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="2nd Examiner">
                            <ItemTemplate>
                                <asp:Label ID="lblExaminerName2" runat="server" Text=' <%#"<b>Examiner: </b>"+ Eval("SecondExaminerName") %>'></asp:Label>
                                <br />
                                <asp:Label ID="lblExaminerMarkSubmissionStatus2" runat="server" Text='<%# Eval("SecondExaminerMarkSubmissionStatus").ToString() == "0" ? "Not Submitted" : "Submitted" %>' Font-Bold="True" ForeColor="Red"></asp:Label>
                                
                                <br /> 
                                <asp:Label ID="LabelSubmissionDate2" runat="server" Text="Submission Deadline: "></asp:Label>
                                <asp:Label ID="LabelSubmissionDate22" runat="server" DataFormatString="{0:dd-MM-yyyy}" Text='<%#Eval("SecondExaminerMarkSubmissionDate") == null  ? "" : Convert.ToDateTime(Eval("SecondExaminerMarkSubmissionDate")).ToShortDateString() %>'></asp:Label>

                                
                                <br />
                                <asp:Label ID="LabelSub2" runat="server" Text='<%# Eval("SecondExaminerMarkSubmissionStatus").ToString() == "0" ? "" : "Submitted On: " %>'></asp:Label>
                                <asp:Label ID="lblExaminerSubmissionDate2" runat="server" DataFormatString="{0:dd-MM-yyyy}" Text='<%# Eval("SecondExaminerMarkSubmissionStatus").ToString() == "0" ? "" : Convert.ToDateTime(Eval("SecondExaminerMarkSubmissionStatusDate")).ToShortDateString() %>'></asp:Label>
                                <br />
                                <asp:Button ID="btnSecondExaminerBack" Enabled='<%# Eval("SecondExaminerMarkSubmissionStatus").ToString() == "0" ? false : true  %>' CommandName="ExaminerExamBack2" Text="Back" runat="server" CommandArgument='<%# Bind("AcaCal_SectionID") %>' OnClientClick="return confirm('Sure to back exam mark?');" />
                                <%--<asp:Button ID="btnExaminerView2" CommandName="ExaminerExamView2" Enabled='<%# Eval("SecondExaminerMarkSubmissionStatus").ToString() == "0" ? false : true  %>' Text="View" runat="server" CommandArgument='<%#Eval("AcaCal_SectionID")+","+ Eval("ExamSetupId") %>' OnClick="btnViewSecondExaminer_Click" />--%>
                                <asp:Button ID="btnExaminerView2" CommandName="ExaminerExamView2" Enabled='<%# Eval("ExaminerMarkSubmissionStatus").ToString() == "0" ? false : true  %>' Text="View" runat="server" CommandArgument='<%#Eval("AcaCal_SectionID")+","+ Eval("ExamSetupId") %>' OnClick="btnViewSecondExaminer_Click" />
                            </ItemTemplate>
                            <ItemStyle CssClass="center"  Width="250px"  />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="3rd Examination student count">
                            <ItemTemplate>
                                <asp:Label ID="lblExaminer3EligibleStudent" runat="server" Text=' <%#"<b>Eligible Count: </b>"+ Eval("ThirdExaminationEligibleStudentCount") %>'></asp:Label>
                                <br />
                                <asp:Label ID="lblExaminer3TotalStudent" runat="server" Text='<%#"<b>Total Count: </b>"+ Eval("TotalStudent") %>'></asp:Label>
                                <br />
                                <asp:Button ID="btnAssignThirdExaminer" Text="Add/Edit Third Examiner" runat="server" Enabled='<%# Eval("ThirdExaminationEligibleStudentCount").ToString() == "0" ? false : true  %>' CommandArgument='<%# Bind("AcaCal_SectionID") %>' OnClick="btnExaminer_Click" />
                                <asp:Button ID="btnThirdExaminerStudentView" Text="Student List" runat="server" Enabled='<%# Eval("ThirdExaminationEligibleStudentCount").ToString() == "0" ? false : true  %>' CommandArgument='<%# Bind("AcaCal_SectionID") %>' OnClick="btnThirdExaminerStudentView_OnClick" />
                            </ItemTemplate>

                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="3rd  Examiner">
                            <ItemTemplate>
                                <asp:Label ID="lblExaminerName3" runat="server" Text=' <%#"<b>Examiner: </b>"+ Eval("ThirdExaminerName") %>'></asp:Label>
                                <br />
                                <asp:Label ID="lblExaminerMarkSubmissionStatus3" runat="server" Text='<%# Eval("ThirdExaminationEligibleStudentCount").ToString() == "0" ? "Not Required" : "Required" %>' Font-Bold="True" ForeColor="Firebrick"></asp:Label>
                                <br />
                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("ThirdExaminerMarkSubmissionStatus").ToString() == "0" ? "Not Submitted" : "Submitted" %>' Font-Bold="True" ForeColor="Red"></asp:Label>
                                
                                <br /> 
                                <asp:Label ID="LabelSubmissionDate3" runat="server" Text="Submission Deadline: "></asp:Label>
                                <asp:Label ID="LabelSubmissionDate33" runat="server" DataFormatString="{0:dd-MM-yyyy}" Text='<%#Eval("ThirdExaminerMarkSubmissionDate") == null  ? "" : Convert.ToDateTime(Eval("ThirdExaminerMarkSubmissionDate")).ToShortDateString() %>'></asp:Label>

                                <br />
                                <asp:Label ID="LabelSub3" runat="server" Text='<%# Eval("ThirdExaminerMarkSubmissionStatus").ToString() == "0" ? "" : "Submitted On: " %>'></asp:Label>
                                <asp:Label ID="lblExaminerSubmissionDate3" runat="server" DataFormatString="{0:dd-MM-yyyy}" Text='<%# Eval("ThirdExaminerMarkSubmissionStatus").ToString() == "0" ? "" : Convert.ToDateTime(Eval("SecondExaminerMarkSubmissionStatusDate")).ToShortDateString() %>'></asp:Label>
                                <br />
                                <%--<asp:Button ID="btnThirdExaminerBack" CommandName="ExaminerExamBack3" Text="Back" Enabled='<%# Eval("ThirdExaminerMarkSubmissionStatus").ToString() == "0" ? false : true  %>' runat="server" CommandArgument='<%# Bind("AcaCal_SectionID") %>' OnClientClick="return confirm('Do you really want back exam mark?');" />--%>
                                <asp:Button ID="btnExaminerView3" CommandName="ExaminerExamView3" Enabled='<%# Eval("ThirdExaminerMarkSubmissionStatus").ToString() == "0" ? false : true  %>' Text="View" runat="server" CommandArgument='<%#Eval("AcaCal_SectionID")+","+ Eval("ExamSetupId") %>' OnClick="btnViewThirdExaminer_Click" />
                                <%--<asp:Button ID="btnExaminerView3" CommandName="ExaminerExamView3" Enabled='<%# Eval("ExaminerMarkSubmissionStatus").ToString() == "0" ? false : true  %>' Text="View" runat="server" CommandArgument='<%#Eval("AcaCal_SectionID")+","+ Eval("ExamSetupId") %>' OnClick="btnViewThirdExaminer_Click" />--%>
                            </ItemTemplate>
                            <ItemStyle CssClass="center"  Width="250px" />
                        </asp:TemplateField>

                        <%--<asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Button ID="btnSubmitAllMark" runat="server"  Text="Submit All" OnClick="btnSubmitAllMark_Click" OnClientClick="return confirm('Do you really want save exam mark?');" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:LinkButton ID="SubmitButton" CommandName="ResultSubmit" Text="Submit" CommandArgument='<%# Bind("CourseHistoryId") %>' runat="server"></asp:LinkButton>
                            <asp:LinkButton ID="DeleteButton" CommandName="TestSetDelete" Text="Delete" CommandArgument='<%# Bind("StudentId") %>' runat="server"></asp:LinkButton>
                        </ItemTemplate>
                    <HeaderStyle Width="80px"></HeaderStyle>
                    <ItemStyle CssClass="center" />
                    </asp:TemplateField>--%>
                    </Columns>

                    <%--<EmptyDataTemplate>
                        <label>Data Not Found</label>No data found!
                    </EmptyDataTemplate>--%>
                    <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />

                    <RowStyle Height="25px" VerticalAlign="Middle" HorizontalAlign="Left" BackColor="#E3EAEB" />
                    <EditRowStyle BackColor="#7C6F57" />
                    <EmptyDataTemplate>
                        No data found!
                                       
                    </EmptyDataTemplate>
                    <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#F8FAFA" />
                    <SortedAscendingHeaderStyle BackColor="#246B61" />
                    <SortedDescendingCellStyle BackColor="#D4DFE1" />
                    <SortedDescendingHeaderStyle BackColor="#15524A" />
                </asp:GridView>
                <%--=================================== POP UP MODAL Assign third examiner =================================--%>
                <div>
                    <asp:Button ID="btnShowPopup" runat="server" Style="display: none" />
                    <ajaxToolkit:ModalPopupExtender
                        ID="ModalPopupExtender1"
                        runat="server"
                        TargetControlID="btnShowPopup"
                        PopupControlID="pnPopUp"
                        CancelControlID="btnCancel"
                        BackgroundCssClass="modalBackground">
                    </ajaxToolkit:ModalPopupExtender>
                    <asp:Panel runat="server" ID="pnPopUp" Style="display: none; background-color: #CCCCCC;">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <div class="updatePanel-Wrapper" style="width: 487px; padding: 5px; margin: 5px;">
                                    <fieldset style="padding: 10px; margin: 5px; border-color: lightgreen;">
                                        <legend>Assign Examiner</legend>
                                        <div class="StudentCourseHistory-container">
                                            <div style="float: left; width: 100%;">
                                                <div style="float: left">
                                                    <fieldset style="padding: 10px; margin: 5px; border-color: lightgreen; width: 100%;">
                                                        <table>
                                                            <asp:Label ID="lblID" runat="server" Visible="false" class="margin-zero input-Size"></asp:Label>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblCourseSection" runat="server" Text="Course Section"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="TxtSection" Width="100%" Enabled="false" runat="server"></asp:TextBox>
                                                                </td>
                                                            </tr>

                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblEx3" runat="server" Text="Third Examiner"></asp:Label>
                                                                </td>
                                                                <td class="auto-style9">
                                                                    <script type="text/javascript">

                                                                        $(document).ready(function () {
                                                                            initdropdownExamCommitteeChairman();

                                                                        });

                                                                        function initdropdownExamCommitteeChairman() {
                                                                            $("#ctl00_MainContainer_ddlEx3").select2({ placeholder: { id: '0', text: '-Select-' }, allowClear: true });
                                                                        }
                                                                    </script>
                                                                    <script type="text/javascript">
                                                                        Sys.Application.add_load(initdropdownExamCommitteeChairman);
                                                                    </script>
                                                                    <asp:DropDownList ID="ddlEx3" runat="server" class="dropdown"></asp:DropDownList>
                                                                </td>
                                                            </tr>



                                                        </table>

                                                    </fieldset>
                                                    <div style="clear: both;"></div>
                                                    <div style="margin-top: 10px; margin-bottom: 5px;">
                                                        <asp:Button runat="server" ID="btnExaminerAssign" OnClick="btnExaminerAssign_OnClick" Style="width: 150px; height: 30px;" OnClientClick="return confirm('Want to save/update?');" />
                                                        <asp:Button runat="server" ID="btnCancel" Text="Cancel" Style="width: 150px; height: 30px;" />
                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                    </fieldset>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                </div>
                <%--=================================== End pop UP MODAL Assign third examiner=================================--%>


                <%--=================================== POP UP MODAL FirstExaminer View=================================--%>
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="btnShowPopUpFirstExaminer" runat="server" Style="display: none" />
                        <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtenderFirstExaminer" runat="server" TargetControlID="btnShowPopUpFirstExaminer" PopupControlID="pnlShowPopUpFirstExaminer"
                            CancelControlID="btnCancelFirstExaminer" BackgroundCssClass="modalBackground">
                        </ajaxToolkit:ModalPopupExtender>
                        <asp:Panel ID="pnlShowPopUpFirstExaminer" runat="server" BackColor="#ffffff" Width="765px" Style="display: none; border-radius: 3px;">
                            <div style="padding: 5px;">
                                <fieldset style="padding: 5px; border: 2px solid #5D7B9D;">
                                    <legend style="font-weight: 100; font-size: medium; color: #5D7B9D; text-align: center">Exam Mark (First Examiner)</legend>
                                    <div style="padding: 5px;">
                                        <b>Exam Mark (First Examiner)</b><br />
                                        <br />

                                        <div class="form-horizontal">
                                            <div class="Message-Area">
                                                <table>
                                                    <tr>
                                                        <td class="auto-style8">
                                                            <asp:Label ID="Label4" runat="server" CssClass="control-newlabel2" Text="Course"></asp:Label>
                                                        </td>
                                                        <td class="auto-style9">:
                                                            <asp:Label ID="lblFirstExaminerModalViewCourse" runat="server" Text=""></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="auto-style8">
                                                            <asp:Label ID="Label6" runat="server" CssClass="control-newlabel2" Text="Exam"></asp:Label>
                                                        </td>
                                                        <td class="auto-style9">:
                                                            <asp:Label ID="lblFirstExaminerModalViewExam" runat="server" Text=""></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="auto-style8">
                                                            <asp:Label ID="Label7" runat="server" CssClass="control-newlabel2" Text="Total Student"></asp:Label>
                                                        </td>
                                                        <td class="auto-style9">:
                                                            <asp:Label ID="lblFirstExaminerModalViewTotalStudent" runat="server" Text=""></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="auto-style8">
                                                            <asp:Label ID="Label2" runat="server" CssClass="control-newlabel2" Text="Absent Count"></asp:Label>
                                                        </td>
                                                        <td class="auto-style9">:
                                                            <asp:Label ID="lblFirstExaminerModalViewAbsentCount" runat="server" Text=""></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>

                                        <br />

                                        <div class="form-horizontal">
                                            <div class="Message-Area" style="height: 300px; overflow: scroll;">
                                                <asp:GridView runat="server" ID="gvExamMarkFirstExaminerView" AllowSorting="True" CssClass="table-bordered"
                                                    AutoGenerateColumns="False" ShowFooter="True" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                                                    <HeaderStyle BackColor="#1C5E55" ForeColor="White" Height="30" Font-Bold="True" />
                                                    <FooterStyle BackColor="#1C5E55" ForeColor="White" Height="30" Font-Bold="True" />
                                                    <AlternatingRowStyle BackColor="White" />
                                                    <RowStyle Height="25" />

                                                    <Columns>
                                                        <asp:TemplateField HeaderText="SI." ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate><b><%# Container.DataItemIndex + 1 %></b></ItemTemplate>
                                                            <HeaderStyle Width="30px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="ExamMarkID" Visible="false" HeaderStyle-Width="350px">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblExamMarkID" Text='<%#Eval("ExamMarkId") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="350" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Student ID" HeaderStyle-Width="100px">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblRoll" Text='<%#Eval("Roll") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="100" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Name">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblYearNo" Text='<%#Eval("Name") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Mark">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblMark" Text='<%#Eval("Mark") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Present/Absent">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblPresentAbsent" Text='<%#Eval("PresentAbsent") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle />
                                                        </asp:TemplateField>

                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <label>Data Not Found</label>
                                                    </EmptyDataTemplate>
                                                    <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />

                                                    <RowStyle Height="25px" VerticalAlign="Middle" HorizontalAlign="Left" BackColor="#E3EAEB" />
                                                    <EditRowStyle BackColor="#7C6F57" />
                                                    <EmptyDataTemplate>
                                                        No data found!
                                                    </EmptyDataTemplate>
                                                    <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                                    <SortedAscendingCellStyle BackColor="#F8FAFA" />
                                                    <SortedAscendingHeaderStyle BackColor="#246B61" />
                                                    <SortedDescendingCellStyle BackColor="#D4DFE1" />
                                                    <SortedDescendingHeaderStyle BackColor="#15524A" />
                                                </asp:GridView>
                                            </div>
                                        </div>

                                        <br />
                                        <div style="text-align: right;">
                                            <asp:Button ID="btnCancelFirstExaminer" runat="server" Text="Cancel" />
                                        </div>
                                    </div>
                                </fieldset>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <%--=================================== END POP UP MODAL FirstExaminer View=================================--%>

                <%--=================================== POP UP MODAL SecondExaminer View=================================--%>
                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="btnShowPopUpSecondExaminer" runat="server" Style="display: none" />
                        <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtenderSecondExaminer" runat="server" TargetControlID="btnShowPopUpSecondExaminer" PopupControlID="pnlShowPopUpSecondExaminer"
                            CancelControlID="btnCancelSecondExaminer" BackgroundCssClass="modalBackground">
                        </ajaxToolkit:ModalPopupExtender>
                        <asp:Panel ID="pnlShowPopUpSecondExaminer" runat="server" BackColor="#ffffff" Width="765px" Style="display: none; border-radius: 3px;">
                            <div style="padding: 5px;">
                                <fieldset style="padding: 5px; border: 2px solid #5D7B9D;">
                                    <legend style="font-weight: 100; font-size: medium; color: #5D7B9D; text-align: center">Exam Mark (Second Examiner)</legend>
                                    <div style="padding: 5px;">
                                        <b>Exam Mark (Second Examiner)</b><br />
                                        <br />

                                        <div class="form-horizontal">
                                            <div class="Message-Area">
                                                <table>
                                                    <tr>
                                                        <td class="auto-style8">
                                                            <asp:Label ID="Label3" runat="server" CssClass="control-newlabel2" Text="Course"></asp:Label>
                                                        </td>
                                                        <td class="auto-style9">:
                                                            <asp:Label ID="lblSecondExaminerModalViewCourse" runat="server" Text=""></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="auto-style8">
                                                            <asp:Label ID="Label11" runat="server" CssClass="control-newlabel2" Text="Exam"></asp:Label>
                                                        </td>
                                                        <td class="auto-style9">:
                                                            <asp:Label ID="lblSecondExaminerModalViewExam" runat="server" Text=""></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="auto-style8">
                                                            <asp:Label ID="Label12" runat="server" CssClass="control-newlabel2" Text="Total Student"></asp:Label>
                                                        </td>
                                                        <td class="auto-style9">:
                                                            <asp:Label ID="lblSecondExaminerModalViewTotalStudent" runat="server" Text=""></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="auto-style8">
                                                            <asp:Label ID="Label13" runat="server" CssClass="control-newlabel2" Text="Absent Count"></asp:Label>
                                                        </td>
                                                        <td class="auto-style9">:
                                                            <asp:Label ID="lblSecondExaminerModalViewAbsentCount" runat="server" Text=""></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>

                                        <br />

                                        <div class="form-horizontal">
                                            <div class="Message-Area" style="height: 300px; overflow: scroll;">
                                                <asp:GridView runat="server" ID="gvExamMarkSecondExaminerView" AllowSorting="True" CssClass="table-bordered"
                                                    AutoGenerateColumns="False" ShowFooter="True" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                                                    <HeaderStyle BackColor="#1C5E55" ForeColor="White" Height="30" Font-Bold="True" />
                                                    <FooterStyle BackColor="#1C5E55" ForeColor="White" Height="30" Font-Bold="True" />
                                                    <AlternatingRowStyle BackColor="White" />
                                                    <RowStyle Height="25" />

                                                    <Columns>
                                                        <asp:TemplateField HeaderText="SI." ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate><b><%# Container.DataItemIndex + 1 %></b></ItemTemplate>
                                                            <HeaderStyle Width="30px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="ExamMarkID" Visible="false" HeaderStyle-Width="350px">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblExamMarkID" Text='<%#Eval("ExamMarkId") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="350" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Student ID" HeaderStyle-Width="100px">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblRoll" Text='<%#Eval("Roll") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="100" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Name">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblYearNo" Text='<%#Eval("Name") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Mark">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblMark" Text='<%#Eval("Mark") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Present/Absent">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblPresentAbsent" Text='<%#Eval("PresentAbsent") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle />
                                                        </asp:TemplateField>

                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <label>Data Not Found</label>
                                                    </EmptyDataTemplate>
                                                    <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />

                                                    <RowStyle Height="25px" VerticalAlign="Middle" HorizontalAlign="Left" BackColor="#E3EAEB" />
                                                    <EditRowStyle BackColor="#7C6F57" />
                                                    <EmptyDataTemplate>
                                                        No data found!
                                                    </EmptyDataTemplate>
                                                    <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                                    <SortedAscendingCellStyle BackColor="#F8FAFA" />
                                                    <SortedAscendingHeaderStyle BackColor="#246B61" />
                                                    <SortedDescendingCellStyle BackColor="#D4DFE1" />
                                                    <SortedDescendingHeaderStyle BackColor="#15524A" />
                                                </asp:GridView>
                                            </div>
                                        </div>

                                        <br />
                                        <div style="text-align: right;">
                                            <asp:Button ID="btnCancelSecondExaminer" runat="server" Text="Cancel" />
                                        </div>
                                    </div>
                                </fieldset>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <%--=================================== END POP UP MODAL SecondExaminer View=================================--%>

                <%--=================================== POP UP MODAL ThirdExaminer Student List =================================--%>
                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="btnThirdExaminerStudentList" runat="server" Style="display: none" />
                        <ajaxToolkit:ModalPopupExtender ID="ModalPopupThirdExaminerStudentList" runat="server" TargetControlID="btnThirdExaminerStudentList" PopupControlID="pnlThirdExaminerStudentList"
                            CancelControlID="btnCancelSecondExaminer" BackgroundCssClass="modalBackground">
                        </ajaxToolkit:ModalPopupExtender>
                        <asp:Panel ID="pnlThirdExaminerStudentList" runat="server" BackColor="#ffffff" Width="765px" Style="display: none; border-radius: 3px;">
                            <div style="padding: 5px;">
                                <fieldset style="padding: 5px; border: 2px solid #5D7B9D;">
                                    <legend style="font-weight: 100; font-size: medium; color: #5D7B9D; text-align: center">Third Examiner Eligible Student List</legend>
                                    <div style="padding: 5px;">
                                        <b>Third Examiner Eligible Student List</b><br />
                                        <br />
                                        <div class="form-horizontal">
                                            <div class="Message-Area" style="height: 300px; overflow: scroll;">
                                                <asp:GridView runat="server" ID="ThirdExaminerStudentListGridView" AllowSorting="True" CssClass="table-bordered"
                                                    AutoGenerateColumns="False" ShowFooter="True" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                                                    <HeaderStyle BackColor="#1C5E55" ForeColor="White" Height="30" Font-Bold="True" />
                                                    <FooterStyle BackColor="#1C5E55" ForeColor="White" Height="30" Font-Bold="True" />
                                                    <AlternatingRowStyle BackColor="White" />
                                                    <RowStyle Height="25" />

                                                    <Columns>
                                                        <asp:TemplateField HeaderText="SI." ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate><b><%# Container.DataItemIndex + 1 %></b></ItemTemplate>
                                                            <HeaderStyle Width="30px" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Student ID" HeaderStyle-Width="100px">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblRoll" Text='<%#Eval("Roll") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="100" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Name">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lbStudentName" Text='<%#Eval("Name") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="1st Examiner Mark">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lbl1stMark" Text='<%#Eval("CGPA") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="2nd Examiner Mark">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lbl2ndMark" Text='<%#Eval("AutoOpenCr") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderStyle />
                                                        </asp:TemplateField>

                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <label>Data Not Found</label>
                                                    </EmptyDataTemplate>
                                                    <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />

                                                    <RowStyle Height="25px" VerticalAlign="Middle" HorizontalAlign="Left" BackColor="#E3EAEB" />
                                                    <EditRowStyle BackColor="#7C6F57" />
                                                    <EmptyDataTemplate>
                                                        No data found!
                                                    </EmptyDataTemplate>
                                                    <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                                    <SortedAscendingCellStyle BackColor="#F8FAFA" />
                                                    <SortedAscendingHeaderStyle BackColor="#246B61" />
                                                    <SortedDescendingCellStyle BackColor="#D4DFE1" />
                                                    <SortedDescendingHeaderStyle BackColor="#15524A" />
                                                </asp:GridView>
                                            </div>
                                        </div>

                                        <br />
                                        <div style="text-align: right;">
                                            <asp:Button ID="Button2" runat="server" Text="Cancel" />
                                        </div>
                                    </div>
                                </fieldset>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <%--=================================== END POP UP MODAL ThirdExaminer Student List =================================--%>

                <%--=================================== POP UP MODAL 1st, 2nd, 3rd examiner marks submission Date =================================--%>
                <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="btnExaminerSubmissionDate" runat="server" Style="display: none" />
                        <ajaxToolkit:ModalPopupExtender ID="ModalPopupExaminerSubmissionDate" runat="server" TargetControlID="btnExaminerSubmissionDate" PopupControlID="pnlExaminerSubmissionDate"
                            CancelControlID="btnCancelSecondExaminer" BackgroundCssClass="modalBackground">
                        </ajaxToolkit:ModalPopupExtender>
                        <asp:Panel ID="pnlExaminerSubmissionDate" runat="server" BackColor="#ffffff" Width="1200px" Style="display: none; border-radius: 3px;">
                            <div style="padding: 5px;">
                                <fieldset style="padding: 5px; border: 2px solid #5D7B9D;">
                                    <legend style="font-weight: 100; font-size: medium; color: #5D7B9D; text-align: center">First, Second, Third examiner Marks Submission Date Set up</legend>
                                    <div style="padding: 5px;">
                                        <table>
                                            <tr>
                                                <td class="auto-style7">
                                                    <asp:Label ID="Label14" runat="server" Text="1st Examiner Submission Date:"></asp:Label>
                                                </td>
                                                <td class="auto-style4">
                                                    <asp:TextBox runat="server" ID="txtSubmissionDate1"  OnTextChanged="txtSubmissionDate3_OnTextChanged" Width="100px" class="margin-zero input-Size datepicker" placeholder="Date" DataFormatString="{0:dd/MM/yyyy}" Height="16px" AutoPostBack="True" />
                                                    <ajaxToolkit:CalendarExtender ID="reqSubmissionStart" runat="server" TargetControlID="txtSubmissionDate1" Format="dd/MM/yyyy" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="auto-style7">
                                                    <asp:Label ID="Label15" runat="server" Text="2nd Examiner Submission Date:"></asp:Label>
                                                </td>
                                                <td class="auto-style4">
                                                    <asp:TextBox runat="server" ID="txtSubmissionDate2"  OnTextChanged="txtSubmissionDate3_OnTextChanged" Width="100px" class="margin-zero input-Size datepicker" placeholder="Date" DataFormatString="{0:dd/MM/yyyy}" Height="16px" AutoPostBack="True" />
                                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtSubmissionDate2" Format="dd/MM/yyyy" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="auto-style7">
                                                    <asp:Label ID="Label16" runat="server" Text="3rd Examiner Submission Date:"></asp:Label>
                                                </td>
                                                <td class="auto-style4">
                                                    <asp:TextBox runat="server" ID="txtSubmissionDate3" OnTextChanged="txtSubmissionDate3_OnTextChanged" Width="100px" class="margin-zero input-Size datepicker" placeholder="Date" DataFormatString="{0:dd/MM/yyyy}" Height="16px" AutoPostBack="True" />
                                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtSubmissionDate3" Format="dd/MM/yyyy" />
                                                </td>
                                            </tr>
                                            
                                        </table>
                                        <table>
                                            <tr>
                                                <td class="auto-style4">
                                                    <asp:Button ID="saveSubmissionDate" runat="server" Text="Save/Update Deadline for Selected Course" OnClick="saveSubmissionDate_OnClick" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="labelHints" runat="server" ForeColor="Red" Text="*Selected Empty deadline will be Saved & Saved deadline will be updated."></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                        <div class="form-horizontal">
                                            <div class="Message-Area" style="height: 400px; overflow: scroll;">
                                                <asp:GridView runat="server" ID="submissionDateGridView" AllowSorting="True" CssClass="table-bordered"
                                                    AutoGenerateColumns="False" ShowFooter="True" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                                                    <HeaderStyle BackColor="#1C5E55" ForeColor="White" Height="30" Font-Bold="True" />
                                                    <FooterStyle BackColor="#1C5E55" ForeColor="White" Height="30" Font-Bold="True" />
                                                    <AlternatingRowStyle BackColor="White" />
                                                    <RowStyle Height="25" />

                                                    <Columns>
                                                        
                                                        <asp:TemplateField Visible="false" >
                                                            <ItemTemplate>
                                                                <asp:Label ID="popLabelAcacalSectionId" runat="server" Text='<%# Bind("AcaCal_SectionID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="SI." ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate><b><%# Container.DataItemIndex + 1 %></b></ItemTemplate>
                                                            <HeaderStyle Width="30px" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <div style="text-align: center">
                                                                    <asp:CheckBox ID="sdSelectAll" runat="server"
                                                                        AutoPostBack="true" OnCheckedChanged="sdSelectAll_CheckedChanged" />
                                                                    All                                       
                                                                </div>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <div style="text-align: center">
                                                                    <asp:CheckBox runat="server" ID="sdSelect"></asp:CheckBox>
                                                                </div>
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="30px" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Course Exam Info">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblCourseCode" Text='<%# "<b>Course Code : </b>"+Eval("CourseCode") %>' />
                                                                <br />
                                                                <asp:Label runat="server" ID="lblCourseName" Text='<%# "<b>Course Name : </b>"+Eval("CourseName") %>' />
                                                                <br />
                                                                <asp:Label runat="server" ID="lblYear" Text='<%# "<b>Year: </b>"+Eval("YearNo") %>' />

                                                                <asp:Label runat="server" ID="lblSemester" Text='<%#"<b>Semester: </b>"+Eval("SemesterNo") %>' />
                                                                
                                                                <asp:Label runat="server" ID="lblExam" Text='<%#"<b>Exam: </b>"+Eval("ExamName") %>' />
                                                                <br />
                                                                <asp:Label runat="server" ID="lblTearcherOneName" Text='<%#"<b>Course Teacher: </b>"+Eval("TearcherOneName") %>' />
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="300" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="1st Examiner">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblExaminer1" Text='<%#Eval("FirstExaminerName") %>'></asp:Label>
                                                                <br /> 
                                                                <asp:Label ID="LabelSubmissionDate1" runat="server" Text="Submission Deadline: "></asp:Label>
                                                                <asp:Label ID="LabelSubmissionDate11" runat="server" DataFormatString="{0:dd-MM-yyyy}" Text='<%#Eval("FirstExaminerMarkSubmissionDate") == null  ? "" : Convert.ToDateTime(Eval("FirstExaminerMarkSubmissionDate")).ToShortDateString() %>'></asp:Label>
                                                                
                                                            </ItemTemplate>
                                                            <HeaderStyle />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="2nd Examiner">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblExaminer2" Text='<%#Eval("SecondExaminerName") %>'></asp:Label>
                                                                <br /> 
                                                                <asp:Label ID="LabelSubmissionDate2" runat="server" Text="Submission Deadline: "></asp:Label>
                                                                <asp:Label ID="LabelSubmissionDate22" runat="server" DataFormatString="{0:dd-MM-yyyy}" Text='<%#Eval("SecondExaminerMarkSubmissionDate") == null  ? "" : Convert.ToDateTime(Eval("SecondExaminerMarkSubmissionDate")).ToShortDateString() %>'></asp:Label>

                                                            </ItemTemplate>
                                                            <HeaderStyle />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="3rd Examiner">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblExaminer3" Text='<%#Eval("ThirdExaminerName") %>'></asp:Label>
                                                                <br /> 
                                                                <asp:Label ID="LabelSubmissionDate3" runat="server" Text="Submission Deadline: "></asp:Label>
                                                                <asp:Label ID="LabelSubmissionDate33" runat="server" DataFormatString="{0:dd-MM-yyyy}" Text='<%#Eval("ThirdExaminerMarkSubmissionDate") == null  ? "" : Convert.ToDateTime(Eval("ThirdExaminerMarkSubmissionDate")).ToShortDateString() %>'></asp:Label>

                                                            </ItemTemplate>
                                                            <HeaderStyle />
                                                        </asp:TemplateField>

                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <label>Data Not Found</label>
                                                    </EmptyDataTemplate>
                                                    <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />

                                                    <RowStyle Height="25px" VerticalAlign="Middle" HorizontalAlign="Left" BackColor="#E3EAEB" />
                                                    <EditRowStyle BackColor="#7C6F57" />
                                                    <EmptyDataTemplate>
                                                        No data found!
                                                    </EmptyDataTemplate>
                                                    <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                                    <SortedAscendingCellStyle BackColor="#F8FAFA" />
                                                    <SortedAscendingHeaderStyle BackColor="#246B61" />
                                                    <SortedDescendingCellStyle BackColor="#D4DFE1" />
                                                    <SortedDescendingHeaderStyle BackColor="#15524A" />
                                                </asp:GridView>
                                            </div>
                                        </div>

                                        <br />
                                        <div style="text-align: right;">
                                            <asp:Button ID="btnCancel3" runat="server" Text="Cancel" OnClick="btnCancel3_OnClick"/>
                                        </div>
                                    </div>
                                </fieldset>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <%--=================================== END POP UP MODAL 1st, 2nd, 3rd examiner marks submission Date =================================--%>
                
                <%--=================================== POP UP MODAL for Group wise Examiner Back =================================--%>
                <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="btnGroupExaminerBack" runat="server" Style="display: none" />
                        <ajaxToolkit:ModalPopupExtender ID="ModalPopupGroupExaminerBack" runat="server" TargetControlID="btnGroupExaminerBack" PopupControlID="pnlGroupExaminerBack"
                            CancelControlID="btnCancelSecondExaminer" BackgroundCssClass="modalBackground">
                        </ajaxToolkit:ModalPopupExtender>
                        <asp:Panel ID="pnlGroupExaminerBack" runat="server" BackColor="#ffffff" Width="765px" Style="display: none; border-radius: 3px;">
                            <div style="padding: 5px;">
                                <fieldset style="padding: 5px; border: 2px solid #5D7B9D;">
                                    <legend id="legendGroupExaminerBack" runat="server" style="font-weight: 100; font-size: medium; color: #5D7B9D; text-align: center"></legend>
                                    <div style="padding: 5px;">
                                        
                                        <div class="form-horizontal">
                                            <div class="Message-Area" style="height: 300px; overflow: scroll;">
                                                <asp:GridView runat="server" ID="gvGroupExaminerBack" AllowSorting="True" CssClass="table-bordered"
                                                    AutoGenerateColumns="False" ShowFooter="True" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                                                    <HeaderStyle BackColor="#1C5E55" ForeColor="White" Height="30" Font-Bold="True" />
                                                    <FooterStyle BackColor="#1C5E55" ForeColor="White" Height="30" Font-Bold="True" />
                                                    <AlternatingRowStyle BackColor="White" />
                                                    <RowStyle Height="25" />

                                                    <Columns>
                                                        
                                                        <asp:TemplateField Visible="false" >
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPopGroupExaminerBackAcaCalSectionId" runat="server" Text='<%# Bind("AcaCalSectionId") %>'></asp:Label>
                                                                <asp:Label ID="lblPopGroupExaminerIdBack" runat="server" Text='<%# Bind("ExaminerId") %>'></asp:Label>
                                                                <asp:Label ID="lblPopGroupExaminerNoBack" runat="server" Text='<%# Bind("CreatedBy") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="SI." ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate><b><%# Container.DataItemIndex + 1 %></b></ItemTemplate>
                                                            <HeaderStyle Width="30px" />
                                                        </asp:TemplateField>                                                  

                                                        <asp:TemplateField HeaderText="Examiner Name">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblPopGroupExaminerNameBack" Text='<%#Eval("ExaminerName") %>' />
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="300" />
                                                        </asp:TemplateField>   
                                                        
                                                        <asp:TemplateField HeaderText="Submission Date">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblPopGroupSubmissionDateBack" DataFormatString="{0:dd-MM-yyyy}" Text='<%#Eval("CreatedDate") == null  ? "" : Convert.ToDateTime(Eval("CreatedDate")).ToShortDateString() %>' />
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="300" />
                                                        </asp:TemplateField>  

                                                        <asp:TemplateField HeaderText="Back">
                                                            <ItemTemplate>
                                                                <asp:Button ID="btnPopGroupExaminerBack" runat="server" Text="Back" OnClick="btnPopGroupExaminerBack_OnClick"/>
                                                            </ItemTemplate>
                                                            <HeaderStyle />
                                                        </asp:TemplateField>

                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <label>Data Not Found</label>
                                                    </EmptyDataTemplate>
                                                    <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />

                                                    <RowStyle Height="25px" VerticalAlign="Middle" HorizontalAlign="Left" BackColor="#E3EAEB" />
                                                    <EditRowStyle BackColor="#7C6F57" />
                                                    <EmptyDataTemplate>
                                                        No data found!
                                                    </EmptyDataTemplate>
                                                    <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                                    <SortedAscendingCellStyle BackColor="#F8FAFA" />
                                                    <SortedAscendingHeaderStyle BackColor="#246B61" />
                                                    <SortedDescendingCellStyle BackColor="#D4DFE1" />
                                                    <SortedDescendingHeaderStyle BackColor="#15524A" />
                                                </asp:GridView>
                                            </div>
                                        </div>

                                        <br />
                                        <div style="text-align: right;">
                                            <asp:Button ID="Button4" runat="server" Text="Cancel" />
                                        </div>
                                    </div>
                                </fieldset>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <%--=================================== END POP UP MODAL for Group wise Examiner Back =================================--%>
            

                <%--=================================== POP UP MODAL for Group wise Examiner VIEW =================================--%>
                <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="btnGroupExaminerView" runat="server" Style="display: none" />
                        <ajaxToolkit:ModalPopupExtender ID="ModalPopupGroupExaminerView" runat="server" TargetControlID="btnGroupExaminerView" PopupControlID="pnlGroupExaminerView"
                            CancelControlID="btnCancelSecondExaminer" BackgroundCssClass="modalBackground">
                        </ajaxToolkit:ModalPopupExtender>
                        <asp:Panel ID="pnlGroupExaminerView" runat="server" BackColor="#ffffff" Width="765px" Style="display: none; border-radius: 3px;">
                            <div style="padding: 5px;">
                                <fieldset style="padding: 5px; border: 2px solid #5D7B9D;">
                                    <legend  id="legendGroupExaminerView" runat="server" style="font-weight: 100; font-size: medium; color: #5D7B9D; text-align: center"
                                            ></legend>
                                    <div style="padding: 5px;">
                                        <%--<b>1st examiner View</b><br />
                                        <br />--%>
                                        <div class="form-horizontal">
                                            <div class="Message-Area" style="height: 300px; overflow: scroll;">
                                                <asp:GridView runat="server" ID="gvGroupExaminerView" AllowSorting="True" CssClass="table-bordered"
                                                    AutoGenerateColumns="False" ShowFooter="True" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                                                    <HeaderStyle BackColor="#1C5E55" ForeColor="White" Height="30" Font-Bold="True" />
                                                    <FooterStyle BackColor="#1C5E55" ForeColor="White" Height="30" Font-Bold="True" />
                                                    <AlternatingRowStyle BackColor="White" />
                                                    <RowStyle Height="25" />

                                                    <Columns>
                                                        
                                                        <asp:TemplateField Visible="false" >
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPopGroupExaminerViewAcaCalSectionId" runat="server" Text='<%# Bind("AcaCalSectionId") %>'></asp:Label>
                                                                <asp:Label ID="lblPopGroupExaminerIdView" runat="server" Text='<%# Bind("ExaminerId") %>'></asp:Label>
                                                                <asp:Label ID="lblPopGroupExaminerNoView" runat="server" Text='<%# Bind("CreatedBy") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="SI." ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate><b><%# Container.DataItemIndex + 1 %></b></ItemTemplate>
                                                            <HeaderStyle Width="30px" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Examiner Name">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblPopGroupExaminerView" Text='<%#Eval("ExaminerName") %>' />
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="300" />
                                                        </asp:TemplateField>


                                                        <asp:TemplateField HeaderText="View">
                                                            <ItemTemplate>
                                                                <asp:Button ID="btnPopGroupExaminerView" runat="server" CommandArgument='<%#Eval("ExamSetupDetailId") %>'   Text="View" OnClick="btnPopGroupExaminerView_OnClick"/> 
                                                            </ItemTemplate>
                                                            <HeaderStyle />
                                                        </asp:TemplateField>

                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <label>Data Not Found</label>
                                                    </EmptyDataTemplate>
                                                    <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />

                                                    <RowStyle Height="25px" VerticalAlign="Middle" HorizontalAlign="Left" BackColor="#E3EAEB" />
                                                    <EditRowStyle BackColor="#7C6F57" />
                                                    <EmptyDataTemplate>
                                                        No data found!
                                                    </EmptyDataTemplate>
                                                    <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                                    <SortedAscendingCellStyle BackColor="#F8FAFA" />
                                                    <SortedAscendingHeaderStyle BackColor="#246B61" />
                                                    <SortedDescendingCellStyle BackColor="#D4DFE1" />
                                                    <SortedDescendingHeaderStyle BackColor="#15524A" />
                                                </asp:GridView>
                                            </div>
                                        </div>

                                        <br />
                                        <div style="text-align: right;">
                                            <asp:Button ID="Button3" runat="server" Text="Cancel" />
                                        </div>
                                    </div>
                                </fieldset>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <%--=================================== END POP UP MODAL for Group wise Examiner VIEW =================================--%>

                
            
            <%--=================================== POP UP MODAL for Group wise Examiner 2- VIEW =================================--%>
                <%--<asp:UpdatePanel ID="UpdatePanel11" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="btnGroupExaminer2View" runat="server" Style="display: none" />
                        <ajaxToolkit:ModalPopupExtender ID="ModalGroupExaminer2View" runat="server" TargetControlID="btnGroupExaminer2View" PopupControlID="pnlGroupExaminer2View"
                            CancelControlID="btnCancelSecondExaminer" BackgroundCssClass="modalBackground">
                        </ajaxToolkit:ModalPopupExtender>
                        <asp:Panel ID="pnlGroupExaminer2View" runat="server" BackColor="#ffffff" Width="765px" Style="display: none; border-radius: 3px;">
                            <div style="padding: 5px;">
                                <fieldset style="padding: 5px; border: 2px solid #5D7B9D;">
                                    <legend style="font-weight: 100; font-size: medium; color: #5D7B9D; text-align: center">2nd examiner View</legend>
                                    <div style="padding: 5px;">
                                        <b>2nd examiner View</b><br />
                                        <br />
                                        <div class="form-horizontal">
                                            <div class="Message-Area" style="height: 300px; overflow: scroll;">
                                                <asp:GridView runat="server" ID="gvGroupExaminer2View" AllowSorting="True" CssClass="table-bordered"
                                                    AutoGenerateColumns="False" ShowFooter="True" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                                                    <HeaderStyle BackColor="#1C5E55" ForeColor="White" Height="30" Font-Bold="True" />
                                                    <FooterStyle BackColor="#1C5E55" ForeColor="White" Height="30" Font-Bold="True" />
                                                    <AlternatingRowStyle BackColor="White" />
                                                    <RowStyle Height="25" />

                                                    <Columns>
                                                        
                                                        <asp:TemplateField Visible="false" >
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPopGroupExaminer2ViewAcaCalSectionId" runat="server" Text='<%# Bind("AcaCalSectionId") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="SI." ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate><b><%# Container.DataItemIndex + 1 %></b></ItemTemplate>
                                                            <HeaderStyle Width="30px" />
                                                        </asp:TemplateField>                                                 

                                                        <asp:TemplateField HeaderText="Examiner Name">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblPopGroupExaminer2View" Text='<%#Eval("ExaminerName2.CodeAndName") %>' />
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="300" />
                                                        </asp:TemplateField>                                                  

                                                        <asp:TemplateField HeaderText="View">
                                                            <ItemTemplate>
                                                                <asp:Button ID="btnPopGroupExaminer2View" runat="server" OnClick="btnPopGroupExaminer2View_OnClick" Text="View" />
                                                            </ItemTemplate>
                                                            <HeaderStyle />
                                                        </asp:TemplateField>

                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <label>Data Not Found</label>
                                                    </EmptyDataTemplate>
                                                    <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />

                                                    <RowStyle Height="25px" VerticalAlign="Middle" HorizontalAlign="Left" BackColor="#E3EAEB" />
                                                    <EditRowStyle BackColor="#7C6F57" />
                                                    <EmptyDataTemplate>
                                                        No data found!
                                                    </EmptyDataTemplate>
                                                    <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                                    <SortedAscendingCellStyle BackColor="#F8FAFA" />
                                                    <SortedAscendingHeaderStyle BackColor="#246B61" />
                                                    <SortedDescendingCellStyle BackColor="#D4DFE1" />
                                                    <SortedDescendingHeaderStyle BackColor="#15524A" />
                                                </asp:GridView>
                                            </div>
                                        </div>

                                        <br />
                                        <div style="text-align: right;">
                                            <asp:Button ID="Button5" runat="server" Text="Cancel" />
                                        </div>
                                    </div>
                                </fieldset>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>--%>
                <%--=================================== END POP UP MODAL for Group wise Examiner 2- VIEW =================================--%>

                <%--=================================== POP UP MODAL for Group wise Examiner 2- Back =================================--%>
                <%--<asp:UpdatePanel ID="UpdatePanel12" runat="server">
                    <ContentTemplate>
                        <asp:Button ID="btnGroupExaminer2Back" runat="server" Style="display: none" />
                        <ajaxToolkit:ModalPopupExtender ID="ModalGroupExaminer2Back" runat="server" TargetControlID="btnGroupExaminer2Back" PopupControlID="pnlGroupExaminer2Back"
                            CancelControlID="btnCancelSecondExaminer" BackgroundCssClass="modalBackground">
                        </ajaxToolkit:ModalPopupExtender>
                        <asp:Panel ID="pnlGroupExaminer2Back" runat="server" BackColor="#ffffff" Width="765px" Style="display: none; border-radius: 3px;">
                            <div style="padding: 5px;">
                                <fieldset style="padding: 5px; border: 2px solid #5D7B9D;">
                                    <legend style="font-weight: 100; font-size: medium; color: #5D7B9D; text-align: center">2nd examiner Back</legend>
                                    <div style="padding: 5px;">
                                        <b>2nd examiner Back</b><br />
                                        <br />
                                        <div class="form-horizontal">
                                            <div class="Message-Area" style="height: 300px; overflow: scroll;">
                                                <asp:GridView runat="server" ID="gvGroupExaminer2Back" AllowSorting="True" CssClass="table-bordered"
                                                    AutoGenerateColumns="False" ShowFooter="True" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                                                    <HeaderStyle BackColor="#1C5E55" ForeColor="White" Height="30" Font-Bold="True" />
                                                    <FooterStyle BackColor="#1C5E55" ForeColor="White" Height="30" Font-Bold="True" />
                                                    <AlternatingRowStyle BackColor="White" />
                                                    <RowStyle Height="25" />

                                                    <Columns>
                                                        
                                                        <asp:TemplateField Visible="false" >
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPopGroupExaminer2BackAcaCalSectionId" runat="server" Text='<%# Bind("AcaCalSectionId") %>'></asp:Label>
                                                                <asp:Label ID="lblPopGroupExaminerId2Back" runat="server" Text='<%# Bind("SecondExaminer") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="SI." ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate><b><%# Container.DataItemIndex + 1 %></b></ItemTemplate>
                                                            <HeaderStyle Width="30px" />
                                                        </asp:TemplateField>


                                                        <asp:TemplateField HeaderText="Examiner Name">
                                                            <ItemTemplate>
                                                                <asp:Label runat="server" ID="lblPopGroupExaminer2Back" Text='<%#Eval("ExaminerName2.CodeAndName") %>' />
                                                            </ItemTemplate>
                                                            <HeaderStyle Width="300" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Back">
                                                            <ItemTemplate>
                                                                <asp:Button ID="btnPopGroupExaminer2Back" runat="server" OnClick="btnPopGroupExaminer2Back_OnClick" Text="Back"/>
                                                            </ItemTemplate>
                                                            <HeaderStyle />
                                                        </asp:TemplateField>

                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <label>Data Not Found</label>
                                                    </EmptyDataTemplate>
                                                    <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />

                                                    <RowStyle Height="25px" VerticalAlign="Middle" HorizontalAlign="Left" BackColor="#E3EAEB" />
                                                    <EditRowStyle BackColor="#7C6F57" />
                                                    <EmptyDataTemplate>
                                                        No data found!
                                                    </EmptyDataTemplate>
                                                    <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                                    <SortedAscendingCellStyle BackColor="#F8FAFA" />
                                                    <SortedAscendingHeaderStyle BackColor="#246B61" />
                                                    <SortedDescendingCellStyle BackColor="#D4DFE1" />
                                                    <SortedDescendingHeaderStyle BackColor="#15524A" />
                                                </asp:GridView>
                                            </div>
                                        </div>

                                        <br />
                                        <div style="text-align: right;">
                                            <asp:Button ID="Button7" runat="server" Text="Cancel" />
                                        </div>
                                    </div>
                                </fieldset>
                            </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>--%>
                <%--=================================== END POP UP MODAL for Group wise Examiner 2- Back =================================--%>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnLoadTabulation" />
                <asp:PostBackTrigger ControlID="btnView" />

            </Triggers>
        </asp:UpdatePanel>

        <br />
        <br />
        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
            <ContentTemplate>
                <center>
                <rsweb:ReportViewer ID="ReportViewer1" runat="server" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" asynrendering="true" Width="100%" Height="100%" SizeToReportContent="true">
                </rsweb:ReportViewer>
                </center>
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
                    <EnableAction AnimationTarget="btnLoad" 
                                  Enabled="false" />  
                   <EnableAction AnimationTarget="btnLoadTabulation" 
                                  Enabled="false" />                   
                </Parallel>
            </OnUpdating>
            <OnUpdated>
                <Parallel duration="0">
                    <ScriptAction Script="onComplete();" />
                    <EnableAction   AnimationTarget="btnLoad" 
                                    Enabled="true" />
                    <EnableAction AnimationTarget="btnLoadTabulation" 
                                  Enabled="true" />  
                </Parallel>
            </OnUpdated>
            </Animations>
        </ajaxToolkit:UpdatePanelAnimationExtender>
    </div>
</asp:Content>

