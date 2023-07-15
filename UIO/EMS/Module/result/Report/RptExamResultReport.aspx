<%@ Page Title="Continuous Marks Sheet" Language="C#" MasterPageFile="~/MasterPage/Common/CommonMasterPage.master" 
    AutoEventWireup="true" CodeBehind="RptExamResultReport.aspx.cs" Inherits="EMS.Module.result.Report.RptExamResultReport" %>

<%@ Register Src="~/UserControls/ProgramUserControl.ascx" TagPrefix="uc1" TagName="ProgramUserControl" %>
<%@ Register Src="~/UserControls/DepartmentUserControl.ascx" TagPrefix="uc1" TagName="DepartmentUserControl" %>
<%@ Register Src="~/UserControls/AdmissionSessionUserControl.ascx" TagPrefix="uc1" TagName="AdmissionSessionUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Title" runat="server">
    Continuous Marks Sheet
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
     <style>
    </style>
        <script src="../../../JavaScript/jquery-1.6.1.min.js"></script>
    <script src="../../../JavaScript/jquery-1.7.1.js"></script>

    <style>
        .btn:hover{
           background-position:0px 50px !important;
           transition:none
              
       }
    </style>

    <script>

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
            
            var session = $("#ctl00_MainContainer_ucSession_ddlSession").val(); 
            //var course = $("#ctl00_MainContainer_ddlCourse").val();
            //var sessionName = $("#ctl00_MainContainer_ucSession_ddlSession option:selected").text();
            var sessionName = '';
            var yearNo = $("#ctl00_MainContainer_ddlYearNo option:selected").text(); 
            var semesterNo = $("#ctl00_MainContainer_ddlSemesterNo option:selected").text();
            var exam = $("#ctl00_MainContainer_ddlExam option:selected").text();
            var programName = $("#ctl00_MainContainer_ucProgram_ddlProgram option:selected").text();
            var departmentName = $("#ctl00_MainContainer_ucDepartment_ddlDepartment option:selected").text();


            var ddlcourse = document.getElementById("<%=ddlCourse.ClientID%>");
            var course = ddlcourse.options[ddlcourse.selectedIndex].value;
            var courseName = ddlcourse.options[ddlcourse.selectedIndex].text;
            //console.log(sessionName);
           
            //var section = ddlsection.options[ddlsection.selectedIndex].value;
         <%--   var checkList1 = document.getElementById('<%= chkExamList.ClientID %>');
            var checkBoxList1 = checkList1.getElementsByTagName("input");
            var checkBoxSelectedItems1 = new Array();--%>
            var examList = [];
            var ary = "";
            //for (var i = 0; i < checkBoxList1.length; i++) {
            //    if (checkBoxList1[i].checked) {
            //        //console.log(checkBoxList1[i]);
            //       // checkBoxSelectedItems1.push(checkBoxList1[i].value);
            //        //alert('checked:' + checkBoxSelectedItems1.push(checkBoxList1[i].getAttribute("JSvalue")).value);
            //        //alert('checked - : ' + checkBoxList1[i].value)
            //        //  ary += checkBoxList1[i].labels[0].innerText + ","
            //        var obj = { ExamNameString: checkBoxList1[i].labels[0].innerText };
            //        examList.push(obj);
            //    }
            //}
        
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "RptExamResultReport.aspx/GetExamMarks",
                data: "{'courseSection':'" + course + "'}",

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
                        mywindow.document.write(makeResultTable(parsed, parsed1, departmentName, programName, sessionName, courseName, yearNo, semesterNo, exam));

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
        function makeResultTable(parsed, parsed1, departmentName, programName, sessionName, courseName, yearNo, semesterNo, exam)
        {
            var disExamList = findDistinctExam(parsed);
            var disStudentList = findDistinctStudent(parsed);
            var middleContent = "";
            var page = 0;
            var stdTotal = 0;
            var tableHeader = FormTableHeader(disExamList);
            var upperContent = FormUpperContent(parsed1,departmentName, programName, sessionName, courseName, yearNo, semesterNo, exam);
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

        function FormUpperContent(parsed1,departmentName, programName, sessionName, courseName, yearNo, semesterNo, exam)
        {
            var splitCourse = courseName.split(":");
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
            var logo = '<p class="Logo text-right"><img src="../../../Images/BrurNewLogo.PNG"  alt=""  width=auto height=70></p>';
            //mywindow.document.write('</div><div class="col-sm-2" style:"padding-left:0px;></div></div>');
            upperContent += '<div class="row"><div class = "col-xs-3" style="padding-top:37px;">';
            upperContent += ''+ logo+'</div><div class = "col-xs-7" style=";padding-top:32px;">';
            upperContent += '<div class="row"  style="font-size: 22px;font-weight: bold;text-align:center;">'+"BEGUM ROKEYA UNIVERSITY, RANGPUR"+'</div>';
            upperContent += '<div class="row" style="font-size: 17px;text-align:center;">'+"Office of the Controller of Examinations"+'</div>';
            //upperContent += '<div class="row" style="font-size: 16px;font-weight: bold;text-align:center;">' + year + " Year " + semester + " Semester Final Examination " + splitSession[0] + '</div>';
            upperContent += '<div class="row" style="font-size: 16px;font-weight: bold;text-align:center;">' + year + " Year " + semester + ' Semester Final Examination - ' + totalExam[5] +'</div>';
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
            upperContent += '<div class = "col-xs-4 " style="font-weight: bold;"> Course Code : ' + splitCourse[1] + '</div>';
            //upperContent += '<div class = "col-sm-3" style="margin-left:-110px; font-weight: bold;"> Course Code : </div>';
            upperContent += '<div class = "col-xs-4" style="font-weight: bold;"> Course Title : ' + splitCourse[0] +' </div></div>';
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
        <label>Continuous Marks Sheet</label>
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
                            <uc1:DepartmentUserControl runat="server" ID="ucDepartment"  OnDepartmentSelectedIndexChanged="OnDepartmentSelectedIndexChanged" />
                        </td>
                        <td class="auto-style4">
                            <b>Program :</b>
                        </td>
                        <td class="auto-style6">
                            <uc1:ProgramUserControl runat="server" ID="ucProgram" OnProgramSelectedIndexChanged="OnProgramSelectedIndexChanged" />
                        </td>
                        <td class="auto-style4"><b>Session :</b></td>
                        <td class="auto-style2">
                            <uc1:AdmissionSessionUserControl runat="server" ID="ucFilterCurrentSession" class="margin-zero dropDownList"/>
                        </td>
                    </tr>
                    <tr>
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
                        <td class="auto-style6">
                            <asp:DropDownList ID="ddlSemesterNo" Width="150px"  AutoPostBack="true"  runat="server" OnSelectedIndexChanged="ddlSemesterNo_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                        
                    </tr>
                    <tr>
                        <td class="auto-style4">
                            <asp:Label ID="Label10" runat="server" Text="Exam : "></asp:Label>
                        </td>
                        <td class="auto-style6">
                            <asp:DropDownList ID="ddlExam" Width="250px" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlExam_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                        <td class="auto-style4">
                            <b>Course :</b>
                        </td>
                        <td class="auto-style6">
                            <asp:DropDownList ID="ddlCourse" AutoPostBack="true" Width="350px" runat="server"></asp:DropDownList>
                        </td>
                       <td>
                            <button class="ml-2 btn btn-sm btn-primary w-100" onclick="Print()">Print</button>
                        </td>
                        <%--<td>
                            <asp:Button ID="btnLoad" runat="server" Text="Load" OnClick="btnLoad_Click" />
                        </td>--%>
                    </tr>
                    <tr>
                        
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                </table>
            </div>
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
                </Parallel>
            </OnUpdating>
            <OnUpdated>
                <Parallel duration="0">
                    <ScriptAction Script="onComplete();" />
                    <EnableAction   AnimationTarget="btnLoad" 
                                    Enabled="true" />
                </Parallel>
            </OnUpdated>
        </Animations>
    </ajaxToolkit:UpdatePanelAnimationExtender>
</div>   
</asp:Content>


