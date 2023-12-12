function loadFunction() {
    var stdRoll = $("#Text1").val()
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "RptStudentTabulation.aspx/LoadStudentTabulationData",
        data: "{'stdRoll':'" + stdRoll + "'}",
        dataType: "json",
        success: function (r) {

            var data = JSON.parse(r.d)

            console.log(data)
            

            var studentInfo = data.studentInfo[0];
            var academicInfo = data.academicInfo;
            var deptInfo = data.department;

            $("#reportTable").children().remove();

            var first1stC = 0;
            var first2ndC = 0;

            var second1stC = 0;
            var second2ndC = 0;

            var third1stC = 0;
            var third2ndC = 0;

            var fourth1stC = 0;
            var fourth2ndC = 0;

            var list1stYear1stSem = [];
            var list1stYear2ndSem = [];

            var list2ndYear1stSem = [];
            var list2ndYear2ndSem = [];

            var list3rdYear1stSem = [];
            var list3rdYear2ndSem = [];

            var list4thYear1stSem = [];
            var list4thYear2ndSem = [];

            academicInfo.forEach(function (item) {
                if (item.YearId == 1) {
                    if (item.SemesterId == 1) {
                        list1stYear1stSem.push(item);
                        first1stC = first1stC + item.Credits;
                    }
                    else if (item.SemesterId == 2) {
                        list1stYear2ndSem.push(item);
                        first2ndC = first2ndC + item.Credits;
                    }
                }
                else if (item.YearId == 2) {
                    if (item.SemesterId == 1) {
                        list2ndYear1stSem.push(item);
                        second1stC = second1stC + item.Credits;
                    }
                    else if (item.SemesterId == 2) {
                        list2ndYear2ndSem.push(item);
                        second2ndC = second2ndC + item.Credits;
                    }
                }
                else if (item.YearId == 3) {
                    if (item.SemesterId == 1) {
                        list3rdYear1stSem.push(item);
                        third1stC = third1stC + item.Credits;
                    }
                    else if (item.SemesterId == 2) {
                        list3rdYear2ndSem.push(item);
                        third2ndC = third2ndC + +item.Credits;
                    }
                }
                else if (item.YearId == 4) {
                    if (item.SemesterId == 1) {
                        list4thYear1stSem.push(item);
                        fourth1stC = fourth1stC + item.Credits;
                    }
                    else if (item.SemesterId == 2) {
                        list4thYear2ndSem.push(item);
                        fourth2ndC = fourth2ndC + item.Credits;
                    }
                }
            })


            var infoTable = "<table style='border:1px solid black; white-space:nowrap;'>"
                + "<tr>"
                    + "<td>Name of Dormitory<td>"
                    + "<td>:<td>"
                    + "<td>" + studentInfo.NameofDormitory + "<td>"
                + "</tr>"
                + "<tr>"
                    + "<td>Students Name<td>"
                    + "<td>:<td>"
                    + "<td>" + studentInfo.FullName + "<td>"
                + "</tr>"
                + "<tr>"
                    + "<td>Father's Name : <td>"
                    + "<td>:<td>"
                    + "<td>" + studentInfo.FatherName + "<td>"
                + "</tr>"
                + "<tr>"
                    + "<td>Mother's Name : <td>"
                    + "<td>:<td>"
                    + "<td>" + studentInfo.MotherName + "<td>"
                + "</tr>"
                + "<tr>"
                    + "<td>Reg. No : <td>"
                    + "<td>:<td>"
                    + "<td>" + studentInfo.RegNo + "<td>"
                + "</tr>"
                + "<tr>"
                    + "<td>Session : <td>"
                    + "<td>:<td>"
                    + "<td>" + studentInfo.Session + "<td>"
                + "</tr>"
                + "<tr>"
                    + "<td>Roll No. : <td>"
                    + "<td>:<td>"
                    + "<td>" + studentInfo.Roll + "<td>"
                + "</tr>"
            + "</table>"


            var universityInfo = '<table class="text-center" style="white-space:nowrap">'
                                        + '<tr>'
                                        + '    <td>'
                                        + '      <img src="../../../Images/PABNA_logo.png"  style="width:12%"/>'
                                        + '    </td>'
                                        + '</tr>'
                                        + '<tr>'
                                        + '  <td>'
                                        + '    <h2>PABNA UNIVERSITY OF</h2>'
                                        + ' </td>'
                                        + '</tr>'
                                        + '<tr>'
                                        + '  <td>'
                                        + '    <h2>SCIENCE AND TECHNOLOGY</h2>'
                                        + ' </td>'
                                        + '</tr>'
                                        + '<tr>'
                                        + '  <td>' + deptInfo
                                        + '</td>'
                                        + '</tr>'
                                        + '<tr>'
                                        + '  <td>Tabulation Sheet for ' + deptInfo + ' Programme'
                                        + '</td>'
                                        + '</tr>'
                                        + '<tr>'
                                        + '  <td>Final Year Examination 2025'
                                        + '</td>'
                                        + '</tr>'
                                        + '<tr>'
                                        + '  <td>Held in 20.....</td>'
                                        + '</tr>'
            + '</table>'



            var gradingsys = "<table class='academic' style='white-space:nowrap'>"
                              + "<tr>"
                                + "<td colspan='3' class='text-center'>GRADING SYSTEM</td>"
                              + "</tr>"
                              + "<tr>"
                                + "<td>Marks Obtained</td>"
                                + "<td>Letter Grade</td>"
                                + "<td>Grade Point</td>"
                              + "</tr>"
                              + "<tr>"
                                + "<td>80% and above</td>"
                                + "<td>A+</td>"
                                + "<td>4.00</td>"
                              + "</tr>"
                              + "<tr>"
                                + "<td>75% to less than 80%</td>"
                                + "<td>A</td>"
                                + "<td>3.75</td>"
                              + "</tr>"
                              + "<tr>"
                                + "<td>70% to less than 75%</td>"
                                + "<td>A-</td>"
                                + "<td>3.50</td>"
                              + "</tr>"
                              + "<tr>"
                                + "<td>65% to less than 70%</td>"
                                + "<td>B+</td>"
                                + "<td>3.25</td>"
                              + "</tr>"
                              + "<tr>"
                                + "<td>60% to less than 65%</td>"
                                + "<td>B</td>"
                                + "<td>3.00</td>"
                              + "</tr>"
                              + "<tr>"
                                + "<td>55% to less than 60%</td>"
                                + "<td>B-</td>"
                                + "<td>2.75</td>"
                              + "<tr>"
                              + "<tr>"
                                + "<td>50% to less than 55%</td>"
                                + "<td>C+</td>"
                                + "<td>2.50</td>"
                              + "</tr>"
                              + "<tr>"
                                + "<td>45% to less than 50%</td>"
                                + "<td>C</td>"
                                + "<td>2.25</td>"
                              + "</tr>"
                              + "<tr>"
                                + "<td>40% to less than 45%</td>"
                                + "<td>D</td>"
                                + "<td>2.00</td>"
                              + "</tr>"
                              + "<tr>"
                                + "<td>Less than 40%</td>"
                                + "<td>F</td>"
                                + "<td>0.00</td>"
                              + "</tr></table>"



            //---------Table created for Year -1 and Semester 1st-------------------
            var firstYear1stSemInfo = '<table class="academic" style="white-space:nowrap">'
                                        + '<thead>'
                                            + '<tr>'
                                                + '<th>Year-1 (1st Semester):</th>'
                                                + '<th>Credit</th>'
                                            + '</tr>'
                                        + '</thead>'
                                        + '<tbody>'
            for (i = 0; i < list1stYear1stSem.length; i++) {
                firstYear1stSemInfo += '<tr>'
                                        + '<td>' + list1stYear1stSem[i].FormalCode + ' ' + list1stYear1stSem[i].Title + ' </td>'
                                        + '<td>' + list1stYear1stSem[i].Credits + ' Credit</td>'
                                    + '</tr>'
            }
            firstYear1stSemInfo += '</tbody>'
                                + '<tfoot class="text-right">'
                                    + '<tr>'
                                        + '<td colspan="2">Total ' + first1stC + ' Credits</td>'
                                    + '</tr>'
                                + '</tfoot>'
                            + '</table>'


            //---------- Table Created for Year-1 And Semester 2nd ----------------------------
            var firstYear2ndSemInfo = '<table class="academic" style="white-space:nowrap">'
                                        + '<thead>'
                                            + '<tr>'
                                                + '<th>Year-1 (2nd Semester):</th>'
                                                + '<th>Credit</th>'
                                            + '</tr>'
                                        + '</thead>'
                                        + '<tbody>'
            for (i = 0; i < list1stYear2ndSem.length; i++) {
                firstYear2ndSemInfo += '<tr>'
                                        + '<td>' + list1stYear2ndSem[i].FormalCode + ' ' + list1stYear2ndSem[i].Title + ' </td>'
                                        + '<td>' + list1stYear2ndSem[i].Credits + ' Credit</td>'
                                    + '</tr>'
            }
            firstYear2ndSemInfo += '</tbody>'
                                + '<tfoot class="text-right">'
                                    + '<tr>'
                                        + '<td colspan="2">Total ' + first2ndC + ' Credits</td>'
                                    + '</tr>'
                                + '</tfoot>'
                            + '</table>'


            //----------Table Created for Year-2 And Semester 1st--------------------
            var SecondYear1stSemInfo = '<table class="academic" style="white-space:nowrap">'
                                        + '<thead>'
                                            + '<tr>'
                                                + '<th>Year-2 (1st Semester):</th>'
                                                + '<th>Credit</th>'
                                            + '</tr>'
                                        + '</thead>'
                                        + '<tbody>'
            for (i = 0; i < list2ndYear1stSem.length; i++) {
                SecondYear1stSemInfo += '<tr>'
                                        + '<td>' + list2ndYear1stSem[i].FormalCode + ' ' + list2ndYear1stSem[i].Title + ' </td>'
                                        + '<td>' + list2ndYear1stSem[i].Credits + ' Credit</td>'
                                    + '</tr>'
            }
            SecondYear1stSemInfo += '</tbody>'
                                + '<tfoot class="text-right">'
                                    + '<tr>'
                                        + '<td colspan="2">Total ' + second1stC + ' Credits</td>'
                                    + '</tr>'
                                + '</tfoot>'
                            + '</table>'



            //----------Table Created for Year-2 And Semester 2nd--------------------
            var SecondYear2ndSemInfo = '<table class="academic" style="white-space:nowrap">'
                                        + '<thead>'
                                            + '<tr>'
                                                + '<th>Year-2 (2nd Semester):</th>'
                                                + '<th>Credit</th>'
                                            + '</tr>'
                                        + '</thead>'
                                        + '<tbody>'
            for (i = 0; i < list2ndYear2ndSem.length; i++) {
                SecondYear2ndSemInfo += '<tr>'
                                        + '<td>' + list2ndYear2ndSem[i].FormalCode + ' ' + list2ndYear2ndSem[i].Title + ' </td>'
                                        + '<td>' + list2ndYear2ndSem[i].Credits + ' Credit</td>'
                                    + '</tr>'
            }
            SecondYear2ndSemInfo += '</tbody>'
                                + '<tfoot class="text-right">'
                                    + '<tr>'
                                        + '<td colspan="2">Total ' + second2ndC + ' Credits</td>'
                                    + '</tr>'
                                + '</tfoot>'
                            + '</table>'


            //-------------------Table Created for Year-3 And Semester 1st-------------------------------------
            var thirdYear1stSemInfo = '<table class="academic" style="white-space:nowrap">'
                                        + '<thead>'
                                            + '<tr>'
                                                + '<th>Year-3 (1st Semester):</th>'
                                                + '<th>Credit</th>'
                                            + '</tr>'
                                        + '</thead>'
                                        + '<tbody>'
            for (i = 0; i < list3rdYear1stSem.length; i++) {
                thirdYear1stSemInfo += '<tr>'
                                        + '<td>' + list3rdYear1stSem[i].FormalCode + ' ' + list3rdYear1stSem[i].Title + ' </td>'
                                        + '<td>' + list3rdYear1stSem[i].Credits + ' Credit</td>'
                                    + '</tr>'
            }
            thirdYear1stSemInfo += '</tbody>'
                                + '<tfoot class="text-right">'
                                    + '<tr>'
                                        + '<td colspan="2">Total ' + third1stC + ' Credits</td>'
                                    + '</tr>'
                                + '</tfoot>'
                            + '</table>'



            //-------------------Table Created for Year-3 And Semester 1st-------------------------------------
            var thirdYear2ndSemInfo = '<table class="academic" style="white-space:nowrap">'
                                        + '<thead>'
                                            + '<tr>'
                                                + '<th>Year-3 (2nd Semester):</th>'
                                                + '<th>Credit</th>'
                                            + '</tr>'
                                        + '</thead>'
                                        + '<tbody>'
            for (i = 0; i < list3rdYear2ndSem.length; i++) {
                thirdYear2ndSemInfo += '<tr>'
                                        + '<td>' + list3rdYear2ndSem[i].FormalCode + ' ' + list3rdYear2ndSem[i].Title + ' </td>'
                                        + '<td>' + list3rdYear2ndSem[i].Credits + ' Credit</td>'
                                    + '</tr>'
            }
            thirdYear2ndSemInfo += '</tbody>'
                                + '<tfoot class="text-right">'
                                    + '<tr>'
                                        + '<td colspan="2">Total ' + third2ndC + ' Credits</td>'
                                    + '</tr>'
                                + '</tfoot>'
                            + '</table>'




            //-------------------Table Created for Year-3 And Semester 1st-------------------------------------
            var fourthYear1stSemInfo = '<table class="academic" style="white-space:nowrap">'
                                        + '<thead>'
                                            + '<tr>'
                                                + '<th>Year-4 (1st Semester):</th>'
                                                + '<th>Credit</th>'
                                            + '</tr>'
                                        + '</thead>'
                                        + '<tbody>'
            for (i = 0; i < list4thYear1stSem.length; i++) {
                fourthYear1stSemInfo += '<tr>'
                                        + '<td>' + list4thYear1stSem[i].FormalCode + ' ' + list4thYear1stSem[i].Title + ' </td>'
                                        + '<td>' + list4thYear1stSem[i].Credits + ' Credit</td>'
                                    + '</tr>'
            }
            fourthYear1stSemInfo += '</tbody>'
                                + '<tfoot class="text-right">'
                                    + '<tr>'
                                        + '<td colspan="2">Total ' + fourth1stC + ' Credits</td>'
                                    + '</tr>'
                                + '</tfoot>'
                            + '</table>'




            //-------------------Table Created for Year-3 And Semester 1st-------------------------------------
            var fourthYear2ndSemInfo = '<table class="academic" style="white-space:nowrap">'
                                        + '<thead>'
                                            + '<tr>'
                                                + '<th>Year-4 (2nd Semester):</th>'
                                                + '<th>Credit</th>'
                                            + '</tr>'
                                        + '</thead>'
                                        + '<tbody>'
            for (i = 0; i < list4thYear2ndSem.length; i++) {
                fourthYear2ndSemInfo += '<tr>'
                                        + '<td>' + list4thYear2ndSem[i].FormalCode + ' ' + list4thYear2ndSem[i].Title + ' </td>'
                                        + '<td>' + list4thYear2ndSem[i].Credits + ' Credit</td>'
                                    + '</tr>'
            }
            fourthYear2ndSemInfo += '</tbody>'
                                + '<tfoot class="text-right">'
                                    + '<tr>'
                                        + '<td colspan="2">Total ' + fourth2ndC + ' Credits</td>'
                                    + '</tr>'
                                + '</tfoot>'
                            + '</table>'



            var tablecoloum = ["Course no. & Credits", "Full Marks", "Continuous Assessment Marks", "Semester Final Marks", "Total Obtained Marks", "Obtained L.G", "Obtained G.P", "Point Secured"]

            var footerColoumn = ["Exam:.....Year:.....Sem:.....", "Exam:.....Year:.....Sem:.....", "Total Countable Marks", "Obtained L.G", "Obtained G.P", "Points Secured"]

            var footerForTable11 = "<table class='academic'>"

            j = 0;
            for (i = 0; i < 8; i++) {
                var lenthdata = 0;
                footerForTable11 += "<tr>"
                if (i == 0) {
                    footerForTable11 += "<td rowspan='8' style='white-space: nowrap; transform: rotate(-90deg)'>Improvemnet Marks</td>"
                                      + "<td></td>"
                                      + "<td class='text-center' colspan='2'>Total</td>"
                                      + "<td rowspan='2' style='white-space: nowrap; transform: rotate(-90deg)'>SGPA</td>"
                                      + "<td rowspan='2' style='white-space: nowrap; transform: rotate(-90deg)'>CGPA 1-2</td>"
                                      + "<td rowspan='2' style='white-space: nowrap; transform: rotate(-90deg)'>Remakrs</td>"
                }
                else if (i == 1) {
                    footerForTable11 += "<td></td>"
                    footerForTable11 += "<td class='text-center'>Point Secured</td>"
                    footerForTable11 += "<td class='text-center'>Credits</td>"
                }
                else {
                    footerForTable11 += "<td>" + footerColoumn[j] + "</td>"
                    if (i == 2) {
                        footerForTable11 += "<td rowspan='6'></td>"
                        footerForTable11 += "<td rowspan='6'></td>"
                        footerForTable11 += "<td rowspan='6'></td>"
                        footerForTable11 += "<td rowspan='6'></td>"
                        footerForTable11 += "<td rowspan='6'></td>"
                    }
                    j = j + 1;
                }
                footerForTable11 += "</tr>"
            }

            footerForTable11 += "</table>"





            //------------ First Year table with First sem and Second Sem-------------------------------------

            var FirstYearTable = "<table class='academic'>"
                                + "<thead>"
                                    + "<tr>"
                                        + "<th class='text-center' colspan='" + (list1stYear1stSem.length + 6) + "'>First Year First Semester</th>"
                                        + "<th class='text-center' colspan='" + (list1stYear2ndSem.length + 5) + "'>First Year Second Semester</th>"
                                    + "</tr>"
                                + "</thead><tbody>"
            for (i = 0; i < tablecoloum.length; i++) {
                var lenthdata = 0;
                FirstYearTable += "<tr>"
                                            + "<td colspan ='2'>" + tablecoloum[i] + "</td>"
                while (lenthdata < list1stYear1stSem.length) {
                    if (i == 0) {
                        FirstYearTable += "<td class='text-center'>" + list1stYear1stSem[lenthdata].FormalCode + " (" + list1stYear1stSem[lenthdata].Credits + ")</td>"
                    }
                    else if (i == 1) {
                        FirstYearTable += "<td class='text-center'>100</td>"
                    }
                    else {

                        if (i == 2 && lenthdata == list1stYear1stSem.length - 1) {
                            FirstYearTable += "<td rowspan='3'></td>"
                        }
                        else if (i == 3 && lenthdata == list1stYear1stSem.length - 1) {

                        }
                        else if (i == 4 && lenthdata == list1stYear1stSem.length - 1) {

                        }
                        else {
                            FirstYearTable += "<td></td>"
                        }
                    }

                    lenthdata++;
                }

                if (i == 0) {
                    FirstYearTable += "<td class='text-center' colspan='2'>Total</td>"
                    FirstYearTable += "<td rowspan='2' class='rotated-cell'>SGPA</td>"
                    FirstYearTable += "<td rowspan='2' class='rotated-cell'>Remarks</td>"
                }
                else if (i == 1) {
                    FirstYearTable += "<td class='text-center'>Points Secured</td>"
                    FirstYearTable += "<td class='text-center'>Credits</td>"
                }
                else if (i == 2) {
                    FirstYearTable += "<td rowspan ='6'></td>"
                    FirstYearTable += "<td class='text-center' rowspan ='6'><b>" + first1stC + "</b></td>"
                    FirstYearTable += "<td rowspan ='6'></td>"
                    FirstYearTable += "<td rowspan ='6'></td>"
                }

                lenthdata = 0;

                while (lenthdata < list1stYear2ndSem.length) {
                    if (i == 0) {
                        FirstYearTable += "<td class='text-center'>" + list1stYear2ndSem[lenthdata].FormalCode + " (" + list1stYear2ndSem[lenthdata].Credits + ")</td>"
                    }
                    else if (i == 1) {
                        FirstYearTable += "<td>100</td>"
                    }
                    else {

                        if (i == 2 && lenthdata == list1stYear2ndSem.length - 1) {
                            FirstYearTable += "<td rowspan='3'></td>"
                        }
                        else if (i == 3 && lenthdata == list1stYear2ndSem.length - 1) {

                        }
                        else if (i == 4 && lenthdata == list1stYear2ndSem.length - 1) {

                        }
                        else {
                            FirstYearTable += "<td></td>"
                        }
                    }

                    lenthdata++;
                }
                if (i == 0) {
                    FirstYearTable += "<td class='text-center' colspan='2'>Total</td>"
                    FirstYearTable += "<td rowspan='2' class='rotated-cell'>SGPA</td>"
                    FirstYearTable += "<td rowspan='2' class='rotated-cell'>CGPA 1-2</td>"
                    FirstYearTable += "<td rowspan='2' class='rotated-cell'>Remarks</td>"
                }
                if (i == 1) {
                    FirstYearTable += "<td class='text-center'>Points Secured</td>"
                    FirstYearTable += "<td class='text-center'>Credits</td>"
                }
                if (i == 2) {
                    FirstYearTable += "<td rowspan ='6'></td>"
                    FirstYearTable += "<td class='text-center' rowspan ='6'><b>" + first2ndC + "</b></td>"
                    FirstYearTable += "<td rowspan ='6'></td>"
                    FirstYearTable += "<td rowspan ='6'></td>"
                    FirstYearTable += "<td rowspan ='6'></td>"
                }


                FirstYearTable += "</tr>"
            }
            FirstYearTable += "</tbody><tfoot>"
            for (i = 0; i < footerColoumn.length; i++) {
                var lenthData = 0;
                FirstYearTable += "<tr>"
                if (i == 0) {
                    FirstYearTable += "<td rowspan='" + footerColoumn.length + "' style='transform: rotate(-90deg); white-space: nowrap'>Imporved Marks</td>"
                }
                FirstYearTable += "<td>" + footerColoumn[i] + "</td>"
                while (lenthData < list1stYear1stSem.length) {
                    FirstYearTable += "<td></td>"
                    lenthData++;
                }
                if (i == 0) {
                    FirstYearTable += "<td rowspan='" + footerColoumn.length + "'></td>"
                    FirstYearTable += "<td rowspan='" + footerColoumn.length + "'></td>"
                    FirstYearTable += "<td rowspan='" + footerColoumn.length + "'></td>"
                    FirstYearTable += "<td rowspan='" + footerColoumn.length + "'></td>"
                }
                lenthData = 0;
                while (lenthData < list1stYear2ndSem.length) {
                    FirstYearTable += "<td></td>"
                    lenthData++;
                }

                if (i == 0) {
                    FirstYearTable += "<td rowspan='6' colspan='5'>" + footerForTable11 + "</td>"
                }

                FirstYearTable += "</tr>"
            }
            FirstYearTable += "</tfoot></table>"





            //------------ Second Year table with First sem and Second Sem-------------------------------------

            var SecondYearTable = "<table class='academic'>"
                                + "<thead>"
                                    + "<tr>"
                                        + "<th class='text-center' colspan='" + (list2ndYear1stSem.length + 7) + "'>Second Year First Semester</th>"
                                        + "<th class='text-center' colspan='" + (list2ndYear2ndSem.length + 5) + "'>Second Year Second Semester</th>"
                                    + "</tr>"
                                + "</thead><tbody>"
            for (i = 0; i < tablecoloum.length; i++) {
                var lenthdata = 0;
                SecondYearTable += "<tr>"
                                            + "<td colspan='2'>" + tablecoloum[i] + "</td>"
                while (lenthdata < list2ndYear1stSem.length) {
                    if (i == 0) {
                        SecondYearTable += "<td class='text-center'>" + list2ndYear1stSem[lenthdata].FormalCode + " (" + list2ndYear1stSem[lenthdata].Credits + ")</td>"
                    }
                    else if (i == 1) {
                        SecondYearTable += "<td class='text-center'>100</td>"
                    }
                    else {

                        if (i == 2 && lenthdata == list2ndYear1stSem.length - 1) {
                            SecondYearTable += "<td rowspan='3'></td>"
                        }
                        else if (i == 3 && lenthdata == list2ndYear1stSem.length - 1) {

                        }
                        else if (i == 4 && lenthdata == list2ndYear1stSem.length - 1) {

                        }
                        else {
                            SecondYearTable += "<td></td>"
                        }
                    }

                    lenthdata++;
                }

                if (i == 0) {
                    SecondYearTable += "<td class='text-center' colspan='2'>Total</td>"
                    SecondYearTable += "<td rowspan='2' class='rotated-cell'>SGPA</td>"
                    SecondYearTable += "<td rowspan='2' class='rotated-cell'>CGPA 2-1</td>"
                    SecondYearTable += "<td rowspan='2' class='rotated-cell'>Remarks</td>"
                }
                else if (i == 1) {
                    SecondYearTable += "<td class='text-center'>Points Secured</td>"
                    SecondYearTable += "<td class='text-center'>Credits</td>"
                }
                else if (i == 2) {
                    SecondYearTable += "<td rowspan ='6'></td>"
                    SecondYearTable += "<td class='text-center' rowspan ='6'><b>" + second1stC + "</b></td>"
                    SecondYearTable += "<td rowspan ='6'></td>"
                    SecondYearTable += "<td rowspan ='6'></td>"
                    SecondYearTable += "<td rowspan ='6'></td>"
                }

                lenthdata = 0;

                while (lenthdata < list2ndYear2ndSem.length) {
                    if (i == 0) {
                        SecondYearTable += "<td class='text-center'>" + list2ndYear2ndSem[lenthdata].FormalCode + " (" + list2ndYear2ndSem[lenthdata].Credits + ")</td>"
                    }
                    else if (i == 1) {
                        SecondYearTable += "<td>100</td>"
                    }
                    else {

                        if (i == 2 && lenthdata == list2ndYear2ndSem.length - 1) {
                            SecondYearTable += "<td rowspan='3'></td>"
                        }
                        else if (i == 3 && lenthdata == list2ndYear2ndSem.length - 1) {

                        }
                        else if (i == 4 && lenthdata == list2ndYear2ndSem.length - 1) {

                        }
                        else {
                            SecondYearTable += "<td></td>"
                        }
                    }

                    lenthdata++;
                }
                if (i == 0) {
                    SecondYearTable += "<td class='text-center' colspan='2'>Total</td>"
                    SecondYearTable += "<td rowspan='2' class='rotated-cell'>SGPA</td>"
                    SecondYearTable += "<td rowspan='2' class='rotated-cell'>CGPA 2-2</td>"
                    SecondYearTable += "<td rowspan='2' class='rotated-cell'>Remarks</td>"
                }
                if (i == 1) {
                    SecondYearTable += "<td class='text-center'>Points Secured</td>"
                    SecondYearTable += "<td class='text-center'>Credits</td>"
                }
                if (i == 2) {
                    SecondYearTable += "<td rowspan ='6'></td>"
                    SecondYearTable += "<td class='text-center' rowspan ='6'><b>" + second2ndC + "</b></td>"
                    SecondYearTable += "<td rowspan ='6'></td>"
                    SecondYearTable += "<td rowspan ='6'></td>"
                    SecondYearTable += "<td rowspan ='6'></td>"
                }


                SecondYearTable += "</tr>"
            }
            SecondYearTable += "</tbody><tfoot>"
            for (i = 0; i < footerColoumn.length; i++) {
                var lenthData = 0;
                SecondYearTable += "<tr>"
                if (i == 0) {
                    SecondYearTable += "<td rowspan='" + footerColoumn.length + "' style='transform: rotate(-90deg); white-space: nowrap'>Imporved Marks</td>"
                }
                SecondYearTable += "<td>" + footerColoumn[i] + "</td>"
                while (lenthData < list2ndYear1stSem.length) {
                    SecondYearTable += "<td></td>"
                    lenthData++;
                }
                if (i == 0) {
                    SecondYearTable += "<td rowspan='" + footerColoumn.length + "'></td>"
                    SecondYearTable += "<td rowspan='" + footerColoumn.length + "'></td>"
                    SecondYearTable += "<td rowspan='" + footerColoumn.length + "'></td>"
                    SecondYearTable += "<td rowspan='" + footerColoumn.length + "'></td>"
                    SecondYearTable += "<td rowspan='" + footerColoumn.length + "'></td>"
                }
                lenthData = 0;
                while (lenthData < list2ndYear2ndSem.length) {
                    SecondYearTable += "<td></td>"
                    lenthData++;
                }

                if (i == 0) {
                    SecondYearTable += "<td rowspan='6' colspan='5'>" + footerForTable11 + "</td>"
                }

                SecondYearTable += "</tr>"
            }
            SecondYearTable += "</tfoot></table>"




            //------------ Third Year table with First sem and Second Sem-------------------------------------

            var ThirdYearTable = "<table class='academic'>"
                                + "<thead>"
                                    + "<tr>"
                                        + "<th class='text-center' colspan='" + (list3rdYear1stSem.length + 7) + "'>Third Year First Semester</th>"
                                        + "<th class='text-center' colspan='" + (list3rdYear1stSem.length + 6) + "'>Third Year Second Semester</th>"
                                    + "<tr>"
                                + "</thead><tbody>"
            for (i = 0; i < tablecoloum.length; i++) {
                var lenthdata = 0;
                ThirdYearTable += "<tr>"
                                            + "<td colspan='2'>" + tablecoloum[i] + "</td>"
                while (lenthdata < list3rdYear1stSem.length) {
                    if (i == 0) {
                        ThirdYearTable += "<td class='text-center'>" + list3rdYear1stSem[lenthdata].FormalCode + " (" + list3rdYear1stSem[lenthdata].Credits + ")</td>"
                    }
                    else if (i == 1) {
                        ThirdYearTable += "<td class='text-center'>100</td>"
                    }
                    else {

                        if (i == 2 && lenthdata == list3rdYear1stSem.length - 1) {
                            ThirdYearTable += "<td rowspan='3'></td>"
                        }
                        else if (i == 3 && lenthdata == list3rdYear1stSem.length - 1) {

                        }
                        else if (i == 4 && lenthdata == list3rdYear1stSem.length - 1) {

                        }
                        else {
                            ThirdYearTable += "<td></td>"
                        }
                    }

                    lenthdata++;
                }

                if (i == 0) {
                    ThirdYearTable += "<td class='text-center' colspan='2'>Total</td>"
                    ThirdYearTable += "<td rowspan='2' class='rotated-cell'>SGPA</td>"
                    ThirdYearTable += "<td rowspan='2' class='rotated-cell'>CGPA 3-1</td>"
                    ThirdYearTable += "<td rowspan='2' class='rotated-cell'>Remarks</td>"
                }
                else if (i == 1) {
                    ThirdYearTable += "<td class='text-center'>Points Secured</td>"
                    ThirdYearTable += "<td class='text-center'>Credits</td>"
                }
                else if (i == 2) {
                    ThirdYearTable += "<td rowspan ='6'></td>"
                    ThirdYearTable += "<td class='text-center' rowspan ='6'><b>" + third1stC + "</b></td>"
                    ThirdYearTable += "<td rowspan ='6'></td>"
                    ThirdYearTable += "<td rowspan ='6'></td>"
                    ThirdYearTable += "<td rowspan ='6'></td>"
                }

                lenthdata = 0;

                while (lenthdata < list3rdYear2ndSem.length) {
                    if (i == 0) {
                        ThirdYearTable += "<td class='text-center'>" + list3rdYear2ndSem[lenthdata].FormalCode + " (" + list3rdYear2ndSem[lenthdata].Credits + ")</td>"
                    }
                    else if (i == 1) {
                        ThirdYearTable += "<td>100</td>"
                    }
                    else {

                        if (i == 2 && lenthdata == list3rdYear2ndSem.length - 1) {
                            ThirdYearTable += "<td rowspan='3'></td>"
                        }
                        else if (i == 3 && lenthdata == list3rdYear2ndSem.length - 1) {

                        }
                        else if (i == 4 && lenthdata == list3rdYear2ndSem.length - 1) {

                        }
                        else {
                            ThirdYearTable += "<td></td>"
                        }
                    }

                    lenthdata++;
                }
                if (i == 0) {
                    ThirdYearTable += "<td class='text-center' colspan='2'>Total</td>"
                    ThirdYearTable += "<td rowspan='2' class='rotated-cell'>SGPA</td>"
                    ThirdYearTable += "<td rowspan='2' class='rotated-cell'>CGPA 3-2</td>"
                    ThirdYearTable += "<td rowspan='2' class='rotated-cell'>Remarks</td>"
                }
                if (i == 1) {
                    ThirdYearTable += "<td class='text-center'>Points Secured</td>"
                    ThirdYearTable += "<td class='text-center'>Credits</td>"
                }
                if (i == 2) {
                    ThirdYearTable += "<td rowspan ='6'></td>"
                    ThirdYearTable += "<td class='text-center' rowspan ='6'><b>" + third2ndC + "</b></td>"
                    ThirdYearTable += "<td rowspan ='6'></td>"
                    ThirdYearTable += "<td rowspan ='6'></td>"
                    ThirdYearTable += "<td rowspan ='6'></td>"
                }


                ThirdYearTable += "</tr>"
            }
            ThirdYearTable += "</tbody><tfoot>"
            for (i = 0; i < footerColoumn.length; i++) {
                var lenthData = 0;
                ThirdYearTable += "<tr>"
                if (i == 0) {
                    ThirdYearTable += "<td rowspan='" + footerColoumn.length + "' style='transform: rotate(-90deg); white-space: nowrap'>Imporved Marks</td>"
                }
                ThirdYearTable += "<td>" + footerColoumn[i] + "</td>"
                while (lenthData < list3rdYear1stSem.length) {
                    ThirdYearTable += "<td></td>"
                    lenthData++;
                }
                if (i == 0) {
                    ThirdYearTable += "<td rowspan='" + footerColoumn.length + "'></td>"
                    ThirdYearTable += "<td rowspan='" + footerColoumn.length + "'></td>"
                    ThirdYearTable += "<td rowspan='" + footerColoumn.length + "'></td>"
                    ThirdYearTable += "<td rowspan='" + footerColoumn.length + "'></td>"
                    ThirdYearTable += "<td rowspan='" + footerColoumn.length + "'></td>"
                }
                lenthData = 0;
                while (lenthData < list3rdYear2ndSem.length) {
                    ThirdYearTable += "<td></td>"
                    lenthData++;
                }

                if (i == 0) {
                    ThirdYearTable += "<td rowspan='6' colspan='5'>" + footerForTable11 + "</td>"
                }

                ThirdYearTable += "</tr>"
            }
            ThirdYearTable += "</tfoot></table>"



            //------------ Fourth Year table with First sem and Second Sem-------------------------------------

            var FourthYearTable = "<table class='academic'>"
                                + "<thead>"
                                    + "<tr>"
                                        + "<th class='text-center' colspan='" + (list4thYear1stSem.length + 7) + "'>Fourth Year First Semester</th>"
                                        + "<th class='text-center' colspan='" + (list4thYear2ndSem.length + 6) + "'>Fourth Year Second Semester</th>"
                                    + "<tr>"
                                + "</thead><tbody>"
            for (i = 0; i < tablecoloum.length; i++) {
                var lenthdata = 0;
                FourthYearTable += "<tr>"
                                            + "<td colspan='2'>" + tablecoloum[i] + "</td>"
                while (lenthdata < list4thYear1stSem.length) {
                    if (i == 0) {
                        FourthYearTable += "<td class='text-center'>" + list4thYear1stSem[lenthdata].FormalCode + " (" + list4thYear1stSem[lenthdata].Credits + ")</td>"
                    }
                    else if (i == 1) {
                        FourthYearTable += "<td class='text-center'>100</td>"
                    }
                    else {

                        if (i == 2 && lenthdata == list4thYear1stSem.length - 1) {
                            FourthYearTable += "<td rowspan='3'></td>"
                        }
                        else if (i == 3 && lenthdata == list4thYear1stSem.length - 1) {

                        }
                        else if (i == 4 && lenthdata == list4thYear1stSem.length - 1) {

                        }
                        else {
                            FourthYearTable += "<td></td>"
                        }
                    }

                    lenthdata++;
                }

                if (i == 0) {
                    FourthYearTable += "<td class='text-center' colspan='2'>Total</td>"
                    FourthYearTable += "<td rowspan='2' class='rotated-cell'>SGPA</td>"
                    FourthYearTable += "<td rowspan='2' class='rotated-cell'>CGPA 3-1</td>"
                    FourthYearTable += "<td rowspan='2' class='rotated-cell'>Remarks</td>"
                }
                else if (i == 1) {
                    FourthYearTable += "<td class='text-center'>Points Secured</td>"
                    FourthYearTable += "<td class='text-center'>Credits</td>"
                }
                else if (i == 2) {
                    FourthYearTable += "<td rowspan ='6'></td>"
                    FourthYearTable += "<td class='text-center' rowspan ='6'><b>" + fourth1stC + "</b></td>"
                    FourthYearTable += "<td rowspan ='6'></td>"
                    FourthYearTable += "<td rowspan ='6'></td>"
                    FourthYearTable += "<td rowspan ='6'></td>"
                }

                lenthdata = 0;

                while (lenthdata < list4thYear2ndSem.length) {
                    if (i == 0) {
                        FourthYearTable += "<td class='text-center'>" + list4thYear2ndSem[lenthdata].FormalCode + " (" + list4thYear2ndSem[lenthdata].Credits + ")</td>"
                    }
                    else if (i == 1) {
                        FourthYearTable += "<td>100</td>"
                    }
                    else {

                        if (i == 2 && lenthdata == list4thYear2ndSem.length - 1) {
                            FourthYearTable += "<td rowspan='3'></td>"
                        }
                        else if (i == 3 && lenthdata == list4thYear2ndSem.length - 1) {

                        }
                        else if (i == 4 && lenthdata == list4thYear2ndSem.length - 1) {

                        }
                        else {
                            FourthYearTable += "<td></td>"
                        }
                    }

                    lenthdata++;
                }
                if (i == 0) {
                    FourthYearTable += "<td class='text-center' colspan='2'>Total</td>"
                    FourthYearTable += "<td rowspan='2' class='rotated-cell'>SGPA</td>"
                    FourthYearTable += "<td rowspan='2' class='rotated-cell'>CGPA 4-2</td>"
                    FourthYearTable += "<td rowspan='2' class='rotated-cell'>Remarks</td>"
                }
                if (i == 1) {
                    FourthYearTable += "<td class='text-center'>Points Secured</td>"
                    FourthYearTable += "<td class='text-center'>Credits</td>"
                }
                if (i == 2) {
                    FourthYearTable += "<td rowspan ='6'></td>"
                    FourthYearTable += "<td class='text-center' rowspan ='6'><b>" + fourth2ndC + "</b></td>"
                    FourthYearTable += "<td rowspan ='6'></td>"
                    FourthYearTable += "<td rowspan ='6'></td>"
                    FourthYearTable += "<td rowspan ='6'></td>"
                }


                FourthYearTable += "</tr>"
            }
            FourthYearTable += "</tbody><tfoot>"
            for (i = 0; i < footerColoumn.length; i++) {
                var lenthData = 0;
                FourthYearTable += "<tr>"
                if (i == 0) {
                    FourthYearTable += "<td rowspan='" + footerColoumn.length + "' style='transform: rotate(-90deg); white-space: nowrap'>Imporved Marks</td>"
                }
                FourthYearTable += "<td>" + footerColoumn[i] + "</td>"
                while (lenthData < list4thYear1stSem.length) {
                    FourthYearTable += "<td></td>"
                    lenthData++;
                }
                if (i == 0) {
                    FourthYearTable += "<td rowspan='" + footerColoumn.length + "'></td>"
                    FourthYearTable += "<td rowspan='" + footerColoumn.length + "'></td>"
                    FourthYearTable += "<td rowspan='" + footerColoumn.length + "'></td>"
                    FourthYearTable += "<td rowspan='" + footerColoumn.length + "'></td>"
                    FourthYearTable += "<td rowspan='" + footerColoumn.length + "'></td>"
                }
                lenthData = 0;
                while (lenthData < list4thYear2ndSem.length) {
                    FourthYearTable += "<td></td>"
                    lenthData++;
                }

                if (i == 0) {
                    FourthYearTable += "<td rowspan='6' colspan='5'>" + footerForTable11 + "</td>"
                }

                FourthYearTable += "</tr>"
            }
            FourthYearTable += "</tfoot></table>"


            var SigTab1 = signaturetabulation("First Year", "Second Year");
            var sigMemberExam1 = signatureMemberExam("First Year", "Second Year");
            var examController1 = examControllerSig("First Year", "Second Year");

            var SigTab2 = signaturetabulation("Third Year", "Fourth Year");
            var sigMemberExam2 = signatureMemberExam("Third Year", "Fourth Year");
            var examController2 = examControllerSig("Third Year", "Fourth Year");


            //--------------- Front Page-------------------------------

            var FirstPageTable = "<table>"
                                    + "<thead>"
                                        + "<tr>"
                                          + "<td>"
                                            + "<div class='d-flex flex-row'>"
                                                + "<div class='p-2'>" + infoTable + "</div>"
                                                + "<div class='p-3'>" + universityInfo + "</div>"
                                                + "<div class='p-2'>" + firstYear1stSemInfo + "</div>"
                                                + "<div class='p-2'>" + firstYear2ndSemInfo + "</div>"
                                                + "<div class='p-2'>" + SecondYear1stSemInfo + "</div>"
                                                + "<div class='p-2'>" + SecondYear2ndSemInfo + "</div>"
                                            + "</div>"
                                          + "</td>"
                                        + "</tr>"
                                    + "</thead>"
                                    + "<tbody>"
                                        + "<tr>"
                                            + "<td>" + FirstYearTable + "</td>"
                                        + "</tr>"
                                        + "<tr>"
                                            + "<td>" + SecondYearTable + "</td>"
                                        + "</tr>"
                                    + "</tbody>"
                                    + "<tfoot>"
                                        + "<tr>"
                                            + "<td>"
                                                + "<div class='d-flex flex-row'>"
                                                    + "<div class='' style='width:35%;padding-top: 8px;'>" + SigTab1 + "</div>"
                                                    + "<div class='p-2' style='width:35%'>" + sigMemberExam1 + "</div>"
                                                    + "<div class='' style='width:35%;padding-top: 8px;'>" + examController1 + "</div>"
                                                + "</div>"
                                            + "</td>"
                                        + "</tr>"
                                    + "</tfoot>"
                               + "</table>"

            //----------Back Page-------------------------

            var SeondPageTable = "<table style='page-break-before:always'>"
                                    + "<thead>"
                                        + "<tr>"
                                          + "<td>"
                                            + "<div class='d-flex flex-row'>"
                                                + "<div class='p-2' style='width:35%'>" + gradingsys + "</div>"
                                                + "<div class='p-2' style='width:35%'>" + thirdYear1stSemInfo + "</div>"
                                                + "<div class='p-2' style='width:35%'>" + thirdYear2ndSemInfo + "</div>"
                                                + "<div class='p-2' style='width:35%'>" + fourthYear1stSemInfo + "</div>"
                                                + "<div class='p-2' style='width:35%'>" + fourthYear2ndSemInfo + "</div>"
                                            + "</div>"
                                          + "</td>"
                                        + "</tr>"
                                    + "</thead>"
                                    + "<tbody>"
                                        + "<tr>"
                                            + "<td>" + ThirdYearTable + "</td>"
                                        + "</tr>"
                                        + "<tr>"
                                            + "<td>" + FourthYearTable + "</td>"
                                        + "</tr>"
                                    + "</tbody>"
                                    + "<tfoot>"
                                        + "<tr>"
                                            + "<td colspan = ''>"
                                                + "<div class='d-flex flex-row'>"
                                                    + "<div class='' style='width:35%;padding-top: 8px;'>" + SigTab2 + "</div>"
                                                    + "<div class='p-2' style='width:35%'>" + sigMemberExam2 + "</div>"
                                                    + "<div class='' style='width:35%;padding-top: 8px;'>" + examController2 + "</div>"
                                                + "</div>"
                                            + "</td>"
                                        + "</tr>"
                                    + "</tfoot>"
                               + "</table>"

            $("#reportTable").append(FirstPageTable)
            $("#reportTable").append("</br>")
            $("#reportTable").append(SeondPageTable)

            $("#btnPrint").show();
            $("#showHide").show();

        },
        error: function (r) {
            console.log("error" + r);
        }
    })
}

function signaturetabulation(year1, year2) {

    var signatureTabulators = "<table class='academic footers' style='white-space:nowrap'>"
                                            + "<thead>"
                                                + "<tr>"
                                                    + "<th colspan='16' class='text-center'>Signatures of Tabulators</th>"
                                                + "</tr>"
                                            + "</thead>"
                                            + "<tbody>"
                                                + "<tr>"
                                                    + "<td colspan='8' class='text-center'>" + year1 + "</td>"
                                                    + "<td colspan='8' class='text-center'>" + year2 + "</td>"
                                                + "</tr>"
    for (i = 0; i < 4; i++) {
        signatureTabulators += "<tr>"
        for (j = 1; j <= 4; j++) {
            if (i == 0 && j % 2 == 0) {
                signatureTabulators += "<td colspan='4'>Second Semester</td>"
            }
            else if (i == 0 && j % 2 != 0) {
                signatureTabulators += "<td colspan='4'>First Semester</td>"
            }
            else {
                signatureTabulators += "<td colspan='4'>" + i + ".</td>"
            }
        }
        signatureTabulators += "</tr>"
    }
    for (i = 0; i < 3; i++) {
        signatureTabulators += "<tr>"
        for (j = 0; j < 16; j++) {
            signatureTabulators += "<td></td>"
        }
        signatureTabulators += "</tr>"
    }
    signatureTabulators += "</tbody>"
    signatureTabulators += "</table>"

    return (signatureTabulators)

}

function signatureMemberExam(year1, year2) {

    var sigatureMembers = "<table class='academic' style='white-space:nowrap'>"
                                            + "<thead>"
                                                + "<tr>"
                                                    + "<th colspan='4' class='text-center'>Signatures of the Members of the Examination Committee</th>"
                                                + "</tr>"
                                            + "</thead>"
                                            + "<tbody>"
                                                + "<tr>"
                                                    + "<td colspan='2' class='text-center'>" + year1 + "</td>"
                                                    + "<td colspan='2' class='text-center'>" + year2 + "</td>"
                                                + "</tr>"
    for (i = 0; i < 6; i++) {
        sigatureMembers += "<tr>"
        for (j = 1; j <= 4; j++) {
            if (i == 0 && j % 2 == 0) {
                sigatureMembers += "<td>Second Semester</td>"
            }
            else if (i == 0 && j % 2 != 0) {
                sigatureMembers += "<td>First Semester</td>"
            }
            else {
                sigatureMembers += "<td>" + i + ".</td>"
            }
        }
        sigatureMembers += "</tr>"
    }
    sigatureMembers += "</tbody>"
    sigatureMembers += "</table>"

    return (sigatureMembers)

}

function examControllerSig(year1, year2) {
    var sigatureMembers = "<table class='academic' style='white-space:nowrap'>"
                                            + "<thead>"
                                                + "<tr>"
                                                    + "<th colspan='4' class='text-center'>Signatures of the Chairman of the Examination Committee</th>"
                                                    + "<th colspan='4' class='text-center'>Signatures of the Controller of the Examinations</th>"
                                                + "</tr>"
                                            + "</thead>"
                                            + "<tbody>"
                                                + "<tr>"
    for (i = 0; i < 2; i++) {
        sigatureMembers += "<td colspan='2' class='text-center'>" + year1 + "</td>"
        sigatureMembers += "<td colspan='2' class='text-center'>" + year2 + "</td>"
    }
    sigatureMembers += "</tr>"
    for (i = 0; i < 6; i++) {
        sigatureMembers += "<tr>"
        for (j = 0; j < 4; j++) {
            if (i == 0) {
                sigatureMembers += "<td class='text-center'>First Semester</td>"
                sigatureMembers += "<td class='text-center'>Second Semester</td>"
            }
            else {
                sigatureMembers += "<td class='text-center'></td>"
                sigatureMembers += "<td class='text-center'></td>"
            }
        }
        sigatureMembers += "</tr>"
    }
    sigatureMembers += "</tbody>"
    sigatureMembers += "</table>"

    return(sigatureMembers)
}

function printFunct() {

    event.preventDefault();

    var element = $("#reportTable").html();

    var typeElement = "";
    var mywindow = window.open("", "PRINT", 1600, 2400)
    typeElement += '<!DOCTYPE html><html>';
    typeElement += '<head><link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/css/bootstrap.min.css"><script src="https://cdn.jsdelivr.net/npm/jquery@3.6.1/dist/jquery.slim.min.js"></script><script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.1/dist/umd/popper.min.js"></script><script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/js/bootstrap.bundle.min.js"></script><style>body{margin-top:30px}table{font-size:15px}.academic{border-collapse: collapse;} .academic th, .academic td {border: 1px solid black;height: 1.5rem;width: 1%;}.footers td:empty{width:50px;} .rotated-cell {transform: rotate(-90deg); white-space: nowrap; padding: 5px; text-align:center;}@media print{@page{size:landscape}}</style></head>';
    typeElement += '<body >'
    typeElement += '<div class="container-fluid">'
    typeElement += element;
    typeElement += '<br/>'
    typeElement += '</div></body></html>';

    mywindow.document.write(typeElement)
    mywindow.document.close();
    mywindow.onload = function () {
        mywindow.print();
    };
}