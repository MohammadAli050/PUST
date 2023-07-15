<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" AutoEventWireup="true" CodeBehind="ResultSheet.aspx.cs" Inherits="EMS.Module.student.Result.ResultSheet" %>
<%@ Register Src="~/UserControls/ProgramUserControl.ascx" TagPrefix="uc1" TagName="ProgramUserControl" %>
<%@ Register Src="~/UserControls/DepartmentUserControl.ascx" TagPrefix="uc1" TagName="DepartmentUserControl" %>
<%@ Register Src="~/UserControls/AdmissionSessionUserControl.ascx" TagPrefix="uc1" TagName="AdmissionSessionUserControl" %>
<%@ Register Src="~/UserControls/BatchUserControl.ascx" TagPrefix="uc1" TagName="BatchUserControl" %>
<%@ Register Src="~/UserControls/SessionUserControl.ascx" TagPrefix="uc1" TagName="SessionUserControl" %>
<%--1704056--%>
<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Student Result Sheet
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <script src="../../../JavaScript/jquery-1.6.1.min.js"></script>
    <script src="../../../JavaScript/jquery-1.7.1.js"></script>
    <style>
    </style>
    <script>

        function setColors(selector) {
            var elements = $(selector);
            for (var i = 0; i < elements.length; i++) {
                var eltBackground = $(elements[i]).css('background-color');
                var eltColor = $(elements[i]).css('color');

                var elementStyle = elements[i].style;
                if (eltBackground) {
                    elementStyle.oldBackgroundColor = {
                        value: elementStyle.backgroundColor,
                        importance: elementStyle.getPropertyPriority('background-color'),
                    };
                    elementStyle.setProperty('background-color', eltBackground, 'important');
                }
                if (eltColor) {
                    elementStyle.oldColor = {
                        value: elementStyle.color,
                        importance: elementStyle.getPropertyPriority('color'),
                    };
                    elementStyle.setProperty('color', eltColor, 'important');
                }
            }
        }

        function InProgress() {
            var panelProg = $get('divProgress');
            panelProg.style.display = '';
        }

        function onComplete() {
            var panelProg = $get('divProgress');
            panelProg.style.display = 'none';
        }

        function Print()
        {
         
            var examSession = $("#<%= ddlExam.ClientID %>").val();
            var year = $("#<%= ddlYearNo.ClientID %>").val();
            var examType = $("#<%= ddlExamType.ClientID %>").val();
            var semester = $("#<%= ddlSemesterNo.ClientID %>").val();

            if(examSession == '0' || year == '0' || semester == '0' || examType == '0' )
            {
                alert('Empty field not allowed');
            }
            else
            {
                InProgress();
                var program = $("#ctl00_MainContainer_ucDepartment_ddlDepartment").val();
                var dept = $("#ctl00_MainContainer_ucDepartment_ddlDepartment option:selected").text();

                var programName = $("#ctl00_MainContainer_ucProgram_ddlProgram option:selected").text();
                var ddlexam = document.getElementById("<%=ddlExam.ClientID%>");
                var examName = ddlexam.options[ddlexam.selectedIndex].text;
                var ddlyear= document.getElementById("<%=ddlYearNo.ClientID%>");
                var yearName = ddlyear.options[ddlyear.selectedIndex].text;
                var ddlsemester = document.getElementById("<%=ddlSemesterNo.ClientID%>");
                var semesterName = ddlsemester.options[ddlsemester.selectedIndex].text;
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "ResultSheet.aspx/GetResultSheetData",
                    data: "{'AcaCalId':'" + examSession + "','YearId':'" + year + "','SemesterId':'" + semester + "','ExamType':'" + examType + "'}",
                    dataType: "json",

                    success: function (data) {
                        onComplete();
                        var parsed = JSON.parse(data.d);
                        //console.log(parsed);
                        if(parsed.length == 0)
                        {
                            alert("No data found");
                        }
                        else
                        {
                            var mywindow = window.open('', 'PRINT', 'height=' + 800 + ',width=' + 1600);
                            var startHead = '<html><head> <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.0/css/bootstrap.min.css"><style>';

                            mywindow.document.write(startHead);
                            mywindow.document.write(FormCssString());
                            mywindow.document.write(makeResultTable2(parsed,programName,examName,yearName,semesterName,dept, examType, examSession));

                            mywindow.document.write('</body></html>');


                            ////var mywindow = window.open("", "_blank");
                            //var html = getHtml1(parsed);
                            //mywindow.document.write(html);
                            mywindow.document.close();
                            mywindow.focus();

                            mywindow.print = function() {
                                setColors('body *');
                                // setTimeout(function () {
                                //   resetColors('body *');
                                //   resetIconColors();
                                // }, 100);
                            }
                        }
                    },
                    error: function (e) {
                        console.log(e);
                    },
                });
            }

        }

        function PrintMeritList()
        {
         
             var examSession = $("#<%= ddlExam.ClientID %>").val();
             var year = $("#<%= ddlYearNo.ClientID %>").val();
            var examType = $("#<%= ddlExamType.ClientID %>").val();

             var semester = $("#<%= ddlSemesterNo.ClientID %>").val();

             if(examSession == '0' || year == '0' || semester == '0' || examType == '0' )
             {
                 alert('Empty field not allowed');
             }
             else
             {
                 InProgress();
                 var program = $("#ctl00_MainContainer_ucDepartment_ddlDepartment").val();
                 var dept = $("#ctl00_MainContainer_ucDepartment_ddlDepartment option:selected").text();

                 var programName = $("#ctl00_MainContainer_ucProgram_ddlProgram option:selected").text();
                 var ddlexam = document.getElementById("<%=ddlExam.ClientID%>");
                var examName = ddlexam.options[ddlexam.selectedIndex].text;
                var ddlyear= document.getElementById("<%=ddlYearNo.ClientID%>");
                var yearName = ddlyear.options[ddlyear.selectedIndex].text;
                var ddlsemester = document.getElementById("<%=ddlSemesterNo.ClientID%>");
                 var semesterName = ddlsemester.options[ddlsemester.selectedIndex].text;
                 //var dept = program;
                 
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "ResultSheet.aspx/GetResultSheetData",
                    data: "{'AcaCalId':'" + examSession + "','YearId':'" + year + "','SemesterId':'" + semester + "','ExamType':'" + examType + "'}",
                    dataType: "json",

                    success: function (data) {
                        onComplete();
                        var parsed = JSON.parse(data.d);
                        //console.log(parsed);
                        var sortedWithCGPA = JSON.parse(data.d);
                        sortedWithCGPA.sort((a, b) => (a.CGPA < b.CGPA) ? 1 : -1);


                        if(parsed.length == 0)
                        {
                            alert("No data found");
                        }
                        else
                        {
                            var mywindow = window.open('', 'PRINT', 'height=' + 800 + ',width=' + 1600);
                            var startHead = '<html><head> <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.0/css/bootstrap.min.css"><style>';

                            mywindow.document.write(startHead);
                            mywindow.document.write(FormCssString());
                            mywindow.document.write(makeResultTable(parsed,programName,examName,yearName,semesterName,sortedWithCGPA,dept, examType, examSession));

                            mywindow.document.write('</body></html>');


                            ////var mywindow = window.open("", "_blank");
                            //var html = getHtml1(parsed);
                            //mywindow.document.write(html);
                            mywindow.document.close();
                            mywindow.focus();

                            mywindow.print = function() {
                                setColors('body *');
                                // setTimeout(function () {
                                //   resetColors('body *');
                                //   resetIconColors();
                                // }, 100);
                            }
                        }
                    },
                    error: function (e) {
                        console.log(e);
                    },
                });
            }

        }
        function makeResultTable(parsed, programName,examName, yearName, semesterName, meritList, dept, examType, examSession)
        {
            var disCourseList = findDistinctCourse(parsed);
            var disStudentList = findDistinctStudent(parsed);
            var disMeritList = findDistinctStudent(meritList);
            var positionArray = ["1st","2nd","3rd","4th","5th","6th","7th","8th","9th","10th"];
            var middleContent = "";
            var positionCount = 0;
            var page = 0;
            var positionDone = false;
            var tableHeader = FormTableHeader(disCourseList,programName,examName, yearName, semesterName,1);
            var upperContent = FormUpperContent(programName,examName, yearName, semesterName,dept,examType);
            var footerContent = FormFooterContent();
            middleContent += upperContent;
            middleContent += tableHeader;
          
          
            var remarks = "";
            for (var i = 0; i < disStudentList.length; i++)
            {
                remarks = "";
                positionDone = false;
                var stdGPA = disStudentList[i].GPA == null ? "-" : disStudentList[i].GPA.toFixed(2);
                var stdCGPA = disStudentList[i].CGPA == null ? "-" : disStudentList[i].CGPA.toFixed(2);
                if(i % 20 == 0 && i != 0 )
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

                var regNo = disStudentList[i].RegistrationNo == null ? "-" : disStudentList[i].RegistrationNo ;
               
                middleContent += "<tr>";
                middleContent += "<td>" + (i + 1) + "</td>";
                middleContent += "<td>" + disStudentList[i].Roll + "</td>"
                middleContent += "<td>" + regNo + "</td>"
                middleContent += "<td>" + disStudentList[i].Code + "</td>"


                middleContent += "<td style='text-align:left;'>" + disStudentList[i].FullName + "</td>";
                for(var j=0;j<disCourseList.length; j++)
                {
                    var crcObj = parsed.find(x=>x.Roll === disStudentList[i].Roll  && x.CourseID === disCourseList[j].CourseID);

                    if(crcObj != null || crcObj != undefined)
                    {
                        var obGPA = crcObj.ObtainedGPA == null ? "-" : crcObj.ObtainedGPA.toFixed(2);

                       
                        if(crcObj.ObtainedGrade == "F")
                        {
                            middleContent += "<td style='background: url(../../../Images/GreyBC.png) !important;'>" + crcObj.ObtainedGrade + "</td>";
                            middleContent += "<td style='background: url(../../../Images/GreyBC.png) !important;'> " + obGPA + "</td>";
                            remarks += crcObj.FormalCode + " ";
                        }
                        else if(examType == "2" && crcObj.ExamId == examSession)
                        {
                            middleContent += "<td style='background: url(../../../Images/GreyBC.png) !important;'>" + crcObj.ObtainedGrade + "</td>";
                            middleContent += "<td style='background: url(../../../Images/GreyBC.png) !important;'> " + obGPA + "</td>";
                        }
                        else
                        {
                            middleContent += "<td>" + crcObj.ObtainedGrade + "</td>";
                            middleContent += "<td> " + obGPA + "</td>";
                           
                        }
                    }
                    else
                    {
                        middleContent += "<td>" + "-" + "</td>";
                        middleContent += "<td>" + "-" + "</td>";
                    }
                }
                if(disStudentList[i].ExamStatus == "Not Promoted")
                {
                    remarks = "";
                }
                middleContent += "<td >" + stdGPA + "</td>";
                middleContent += "<td>" + stdCGPA + "</td>";
                middleContent += "<td>" + disStudentList[i].ExamStatus + "</td>";
                middleContent += "<td>" + remarks + "</td>";

                if(positionCount < 10)
                {
                    for(var k = 0; k<10; k++)
                    {
                        if(disStudentList[i].Roll == disMeritList[k].Roll)
                        {
                            positionCount++;
                            middleContent += "<td>" + positionArray[k] + "</td>";
                            positionDone = true;
                            break;
                        }
                    }
                }
                if(!positionDone)
                {
                    middleContent += "<td></td>";
                }
                middleContent += "</tr>";

                if(i == disStudentList.length - 1)
                {
                    middleContent += "</tbody></table>";

                    middleContent += "</div><div class='col-sm-1'></div></div>";
                    middleContent += footerContent;

                    middleContent += "</div>";

                }




            }

            //middleContent += "</div><div class='col-sm-1'></div></div>";



            return middleContent;




          
        }

        function makeResultTable2(parsed, programName,examName, yearName, semesterName,dept, examType, examSession)
        {
            var disCourseList = findDistinctCourse(parsed);
            var disStudentList = findDistinctStudent(parsed);
            var middleContent = "";
            var positionCount = 0;
            var page = 0;
            var positionDone = false;
            var tableHeader = FormTableHeader(disCourseList,programName,examName, yearName, semesterName,0);
            var upperContent = FormUpperContent(programName,examName, yearName, semesterName,dept,examType);
            var footerContent = FormFooterContent();
            middleContent += upperContent;
            middleContent += tableHeader;

            //console.log(disStudentList);
          
            var remarks = "";
            for (var i = 0; i < disStudentList.length; i++)
            {
                remarks = "";
                positionDone = false;
                var stdGPA = disStudentList[i].GPA == null ? "-" : disStudentList[i].GPA.toFixed(2);
                var stdCGPA = disStudentList[i].CGPA == null ? "-" : disStudentList[i].CGPA.toFixed(2);

                if(i % 20 == 0 && i != 0 )
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

                var regNo = disStudentList[i].RegistrationNo == null ? "-" : disStudentList[i].RegistrationNo ;
               
                middleContent += "<tr>";
                middleContent += "<td>" + (i + 1) + "</td>";
                middleContent += "<td>" + disStudentList[i].Roll + "</td>"
                middleContent += "<td>" + regNo + "</td>"
                middleContent += "<td>" + disStudentList[i].Code + "</td>"


                middleContent += "<td style='text-align:left;'>" + disStudentList[i].FullName + "</td>";
                for(var j=0;j<disCourseList.length; j++)
                {
                    var crcObj = parsed.find(x=>x.Roll === disStudentList[i].Roll  && x.CourseID === disCourseList[j].CourseID);
                    if(crcObj != null || crcObj != undefined)
                    {
                        var obGPA = crcObj.ObtainedGPA == null ? "-" : crcObj.ObtainedGPA.toFixed(2);

                       
                        if(crcObj.ObtainedGrade == "F")
                        {
                            middleContent += "<td style='background: url(../../../Images/GreyBC.png) !important;'>" + crcObj.ObtainedGrade + "</td>";
                            middleContent += "<td style='background: url(../../../Images/GreyBC.png) !important;'> " + obGPA + "</td>";
                            remarks += crcObj.FormalCode + " ";
                        }
                        else if(examType == "2" && crcObj.ExamId == examSession)
                        {
                            middleContent += "<td style='background: url(../../../Images/GreyBC.png) !important;'>" + crcObj.ObtainedGrade + "</td>";
                            middleContent += "<td style='background: url(../../../Images/GreyBC.png) !important;'> " + obGPA + "</td>";
                        }
                        else
                        {
                            middleContent += "<td>" + crcObj.ObtainedGrade + "</td>";
                            middleContent += "<td> " + obGPA + "</td>";
                           
                        }
                    }
                    else
                    {
                        middleContent += "<td>" + "-" + "</td>";
                        middleContent += "<td>" + "-" + "</td>";
                    }
                }
                if(disStudentList[i].ExamStatus == "Not Promoted")
                {
                    remarks = "";
                }
                middleContent += "<td >" + stdGPA + "</td>";
                middleContent += "<td>" + stdCGPA + "</td>";
                middleContent += "<td>" + disStudentList[i].ExamStatus + "</td>";
                middleContent += "<td>" + remarks + "</td>";

                
                middleContent += "</tr>";

                if(i == disStudentList.length - 1)
                {
                    middleContent += "</tbody></table>";

                    middleContent += "</div><div class='col-sm-1'></div></div>";
                    middleContent += footerContent;

                    middleContent += "</div>";

                }




            }

            //middleContent += "</div><div class='col-sm-1'></div></div>";



            return middleContent;




          
        }


        function findDistinctCourse(parsed)
        {
            var flags = [], distinctCourse = [], l = parsed.length, x;
            var StGPA = []; FlagStGpa = []; Flag2 = []; uniqueRegName = [];
            var summary = [];
            for (x = 0; x < l; x++) {
                if (flags[parsed[x].CourseID]) continue;
                flags[parsed[x].CourseID] = true;
                distinctCourse.push(parsed[x]);
            }
            distinctCourse.sort((a, b) => (a.CourseID > b.CourseID) ? 1 : -1)
            return distinctCourse;
        }
        function findDistinctStudent(parsed) {
            var flags = [], distinctStudent = [], l = parsed.length, x;
            var StGPA = []; FlagStGpa = []; Flag2 = []; uniqueRegName = [];
            var summary = [];
            for (x = 0; x < l; x++) {
                if (flags[parsed[x].Roll]) continue;
                flags[parsed[x].Roll] = true;
                distinctStudent.push(parsed[x]);
            }
            //distinctStudent.sort((a, b) => (a.Roll < b.Roll) ? 1 : -1)

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
            CssString += '.colBk {background-color: #1a4567; -webkit-print-color-adjust:exact !important;}';
                        //@media print{.box-text { font-size: 27px !important; color: blue !important;-webkit-print-color-adjust: exact !important;}

            CssString += '</style></head><body>';


            //mywindow.document.write('@media print #customers {backgrosund-color: grey; -webkit-print-color-adjust: exact !important;}');

            // mywindow.document.write('#customers th {');


            return CssString;

        }
        function FormTableHeader(disCourseList,programName,examName, yearName, semesterName, print)
        {
            var middleContent = "";
            var splitProg = programName.split(" ");
            var examSplit = examName.split(" ");
            
            var yname = "";
            if(yearName == "1")
            {
                yname = "1st Year";
            }
            else if(yearName == "2")
            {
                yname = "2nd Year";
            }
            else if(yearName == "3")
            {
                yname = "3rd Year";
            }
            else
            {
                yname = "4th Year";
            }

            var sname = semesterName == "1" ? "1st" : "2nd";
            var headerExamName = yname + " " + sname + " Semester" +" " + "Final Exam " + examSplit[5];
 
            //var disCourseList = findDistinctCourse(parsed);
            var FullExamName = yname + " " + sname + " Semester" +" " + "Final Exam - " + examSplit[5];
            var numOfCourses = 0; // Modify
            middleContent += "<div class='row' style='padding-top:10px;'>";
            middleContent += "<div class='col-sm-1'></div>";
            middleContent += "<div class='col-sm-10'>";
            middleContent += "<table id='customers'><thead>";
            middleContent += "<tr><th rowspan='3'>SL No</th>";
            middleContent += "<th rowspan='3'> ID No </th>";
            middleContent += "<th rowspan='3'> Reg No</th>";
            middleContent += "<th rowspan='3'> Session</th>";
            middleContent += "<th rowspan='3'> Name</th>";
            middleContent += "<th colspan='" + (disCourseList.length * 2) + "'>" + FullExamName + "</th>";
            middleContent += "<th rowspan='3'> SGPA</th>";
            middleContent += "<th rowspan='3'> CGPA</th>";
            middleContent += "<th rowspan='3'> Result</th>";
            middleContent += "<th rowspan='3'> Remarks</th>";
            if(print == 1)
            {
                middleContent += "<th rowspan='3'> Merit Position</th>";
            }



            middleContent += "</tr><tr>";
            for (var i = 0; i < disCourseList.length; i++)
            {
                middleContent +=  "<th colspan='2'>"+disCourseList[i].FormalCode +"</th>";

            }
            middleContent += "</tr><tr>";
            for (var i = 0; i < disCourseList.length; i++) {
                middleContent += "<th>" + "LG" + "</th>";
                middleContent += "<th>" + "GP" + "</th>";

            }

            middleContent += "</tr></thead>" + "<tbody>";
            return middleContent;
        }

        function FormUpperContent(programName,examName, yearName, semesterName,dept,examType)
        {
            var splitProg = programName.split(" ");
            var examSplit = examName.split(" ");
            //console.log(splitProg);
            var yname = "";
            if(yearName == "1")
            {
                yname = "1st Year";
            }
            else if(yearName == "2")
            {
                yname = "2nd Year";
            }
            else if(yearName == "3")
            {
                yname = "3rd Year";
            }
            else
            {
                yname = "4th Year";
            }

            var sname = semesterName == "1" ? "1st" : "2nd";
            var upperContent = "";
            var imprv = examType == "1" ? "" : " (Improvement)";
            var logo = '<p class="Logo"><img src="../../../Images/BrurNewLogo.PNG"  alt=""  width=auto height=70></p>';
            //mywindow.document.write('</div><div class="col-sm-2" style:"padding-left:0px;></div></div>');
            upperContent += '<div class="row"><div class = "col-sm-2" style=""></div><div class = "col-sm-2" style="padding-top:30px;">';
            upperContent += ''+ logo+'</div><div class = "col-sm-7" style="margin-left:-80px;padding-top:15px;text-align:center;">';
            upperContent += '<div class="row"><h3>'+"BEGUM ROKEYA UNIVERSITY, RANGPUR"+'</h3></div>';

            upperContent += '<div class="row" style="font-size: 17px;">'+"Result Sheet"+imprv+'</div>';
            //upperContent += '<div class="row">'+ splitProg[0] + " " + yearName + " " + semesterName + " "+ "Final Examination - "+ examSplit[1] +'</div>';
            upperContent += '<div class="row">'+ splitProg[0] + " " + yname + " " + sname + " Semester" +" " + "Final Exam - " + examSplit[5]  +'</div>';

            // upperContent += '<div class="row">'+"Economics" +'</div></div><div class ="col-sm-2" style=""></div></div>';
            upperContent += '</div><div class ="col-sm-1" style=""></div></div>';

            upperContent += '<div class= "row"> <div class=col-sm-1></div>';
            upperContent += '<div class=col-sm-10>________________________________________________________';
            upperContent += '_______________________________________________________________________</div>';
            upperContent += '<div class=col-sm-1></div></div>';

            upperContent += '<div class= "row"> <div class=col-sm-5></div>';
            upperContent += '<div class=col-sm-6 style="padding-left: 85px;font-size: 16px;font-weight: bold;"> Department : ' + dept  ;
            upperContent += '</div>';
            upperContent += '<div class=col-sm-1></div></div>';
            //upperContent += '</div>'

            //upperContent +='<p class="b" style="font-weight:bold;">'+"BSS in Economy"+'</p></div></div>';
            //upperContent += '<div class="row"><div class = "col-sm-5" style=""><p class="b" style="line-height:8px;font-weight:bold;margin-top:3px;">'+ "1st Yesr 1st Semester" +'</p></div><div class = "col-sm-7" style="font-size:14px;font-weight:bold;padding-left:0px;padding-top:0px;" >Result Sheet</div></div>';
                    
            //upperContent += '<div class="row"><div class = "col-sm-6" style=""><p class="b" style="line-height:5px;font-weight:bold;">Final Exam 2010</p></div><div class = "col-sm-6"></div></div>';
            return upperContent;
        }

        function FormFooterContent()
        {
            var today = new Date();
            var dd = String(today.getDate()).padStart(2, '0');
            var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
            var yyyy = today.getFullYear();

            today = dd + '.' + mm + '.' + yyyy;
            var footerContent = ""
            footerContent += "<div class='row' style='padding-top:25px;'>";
            footerContent += "<div class='col-sm-3' style='padding-left:55px;'>" + "Date: "+today  + "</div>";
            footerContent += "<div class='col-sm-3'>" +  "</div>";
            footerContent += "<div class='col-sm-3'>" + "Deputy Controller" + "</div>";
            footerContent += "<div class='col-sm-3'>" + "Controller of Examinations(In Charge)" + "</div></div>";

            footerContent += "<div class='row' style='padding-top:5px;'>";
            footerContent += "<div class='col-sm-3' style='padding-left:55px;'>" + "Prepared By................" + "</div>";
            footerContent += "<div class='col-sm-3'>"+ "Checked By..............." + "</div>";
            footerContent += "<div class='col-sm-3'>" + "Begum Rokeya University, Rangpur" + "</div>";
            footerContent += "<div class='col-sm-3'>" + "Begum Rokeya University, Rangpur" + "</div></div>";

            return footerContent;


        }

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContainer" runat="server">
     <div>
             <div class="PageTitle">
        <label>Result Sheet</label>
    </div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <table style="width: 100%;" id="tblHeader">
                        <tr>
                            <td colspan="2" class="style1">
                                <table style="width: 100%;">
                                    <tr>
                                        <td class="auto-style4">
                                            <asp:Label  ID="Label4" runat="server" Text="Department : "></asp:Label>
                                        </td>
                                        <td class="auto-style2" style="padding-right:20px;">
                                            <%--<asp:DropDownList ID="programDropDownList" width="250px" AutoPostBack="True" OnSelectedIndexChanged="programDropDownList_OnSelectedIndexChanged" runat="server"></asp:DropDownList>--%>
                                            <uc1:DepartmentUserControl runat="server" ID="ucDepartment" OnDepartmentSelectedIndexChanged="OnDepartmentSelectedIndexChanged" />
                                        </td>
                                            <td class="auto-style2" >

                                            <asp:Label  ID="Label3" runat="server" Text="Program : "></asp:Label>
                                        </td>
                                        <td class="auto-style2" style="width:420px;padding-right:20px;">
                                            <%--<asp:DropDownList ID="programDropDownList" width="250px" AutoPostBack="True" OnSelectedIndexChanged="programDropDownList_OnSelectedIndexChanged" runat="server"></asp:DropDownList>--%>
                                            <uc1:ProgramUserControl runat="server" ID="ucProgram" OnProgramSelectedIndexChanged ="OnProgramSelectedIndexChanged"  />
                                        </td>
                                                      <td class="auto-style4"><asp:Label ID="Label1" runat="server" Text="Session : "></asp:Label></td>
                         <td class="auto-style2">
                                            <%--<asp:DropDownList ID="programDropDownList" width="250px" AutoPostBack="True" OnSelectedIndexChanged="programDropDownList_OnSelectedIndexChanged" runat="server"></asp:DropDownList>--%>
                                            <uc1:AdmissionSessionUserControl runat="server" ID="ucAdmissionSession"   />
                                        </td>
                                        <%--<td><asp:Label ID="Label2" runat="server" Text="Batch"></asp:Label></td>
                                        <td>
                                            <uc1:BatchUserControl runat="server" ID="ucBatch"/>
                                        </td>--%>
                                       
                                        <td class="auto-style4"></td>
                                    </tr>
                                    
                                                                       <tr style="height:50px;">
                        <%--<td class="auto-style4">
                            <asp:Label ID="Label3" Width="120px" runat="server" Text="Versity Session"></asp:Label>
                        </td>
                        <td class="auto-style5">
                            <uc1:AdmissionSessionUserControl Width="150px" runat="server" ID="ucVersitySession" class="margin-zero dropDownList"/>
                        </td>--%>                        
                        <td class="auto-style4"><asp:Label ID="Label8" runat="server" Text="Year : "></asp:Label></td>
                        <td class="auto-style6">
                            <asp:DropDownList ID="ddlYearNo" Width="150px" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlYearNo_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                        <td class="auto-style4"><asp:Label ID="Label9" runat="server" Text="Semester : "></asp:Label></td>
                        <td class="auto-style6" style="width:30px;">
                            <asp:DropDownList ID="ddlSemesterNo" Width="150px"  AutoPostBack="true"  runat="server" OnSelectedIndexChanged="ddlSemesterNo_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                        <td class="auto-style4">
                            <asp:Label ID="Label10" runat="server" Text="Exam : "></asp:Label>
                        </td>
                        <td class="auto-style6">
                            <asp:DropDownList ID="ddlExam" Width="350px" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlExam_SelectedIndexChanged"></asp:DropDownList>
               
                 
                    </tr>  
                                       
                                    <tr>
                                                 </td>
                                                                                <td class="auto-style4">
                            <asp:Label ID="Label2" runat="server" Text="Exam Type: "></asp:Label>
                        </td>
                        <td class="auto-style6">
                            <asp:DropDownList ID="ddlExamType" Width="350px" runat="server" AutoPostBack="true" >
                                        <asp:ListItem Text="Select Exam Type" Selected="True" value="0"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="Regular"> </asp:ListItem>
                                        <asp:ListItem Value="2" Text="Improvement"> </asp:ListItem>

                            </asp:DropDownList>
                        </td>
                                    </tr>
                                    <tr>
                                       <td class="auto-style4">
                                           <button onclick="Print()"> Print</button>
<%--                                            <asp:Button ID="btnSaveToServer" runat="server" Text="Upload Student" OnClick="btnSaveToServer_Click" />--%>
                                       </td>
                                            <td class="auto-style4">
                                           <button onclick="PrintMeritList()"> Merit List</button>
<%--                                            <asp:Button ID="btnSaveToServer" runat="server" Text="Upload Student" OnClick="btnSaveToServer_Click" />--%>
                                       </td>
                                   </tr>
                                </table>
                            </td>
                        </tr>
                    </table>

                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

    <div>
        
         <div id="divProgress" style="display: none; z-index: 1000; position: fixed; top: 50%; left: 50%; transform: translate(-50%, -50%);">
                    <asp:Image ID="LoadingImage" runat="server" ImageUrl="~/Images/Img/Waiting.gif" Height="150px" Width="150px" />
                </div>
    </div>
</asp:Content>
