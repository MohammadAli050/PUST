function loadFunction() {
    if (!$.trim($("#ctl00_MainContainer_Text1").val()).length) {
        Swal.fire({
            icon: 'error',
            title: 'No Roll',
            text: 'Please Input A Roll'
        })
    }
    else {
        var roll = $("#ctl00_MainContainer_Text1").val()

        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "RptAcademicGradeSheet.aspx/Loadacademicgradesheet",
            data: "{'roll':'" + roll + "'}",
            dataType: "json",
            success: function (data) {
                var loadData = JSON.parse(data.d)

                $("#print").show();

                var student = loadData.studentInfo[0];

                var academicdata = loadData.academicInfo;

                var list1stYear1stSem = [];
                var list1stYear2ndSem = [];

                var list2ndYear1stSem = [];
                var list2ndYear2ndSem = [];

                var list3rdYear1stSem = [];
                var list3rdYear2ndSem = [];

                var list4thYear1stSem = [];
                var list4thYear2ndSem = [];

                var totalCredit = 0;
                var y1s1Credit = 0;
                var y1s2Credit = 0;

                var y2s1Credit = 0;
                var y2s2Credit = 0;

                var y3s1Credit = 0;
                var y3s2Credit = 0;

                var y4s1Credit = 0;
                var y4s2Credit = 0;

                var y1s1gpa = 0;
                var y1s2gpa = 0;

                var y2s1gpa = 0;
                var y2s2gpa = 0;

                var y3s1gpa = 0;
                var y3s2gpa = 0;

                var y4s1gpa = 0;
                var y4s2gpa = 0;

                academicdata.forEach(function (item) {
                    if (item.YearId == 1) {
                        if (item.SemesterId == 1) {
                            list1stYear1stSem.push(item);
                        }
                        else if (item.SemesterId == 2) {
                            list1stYear2ndSem.push(item);
                        }
                    }
                    else if (item.YearId == 2) {
                        if (item.SemesterId == 1) {
                            list2ndYear1stSem.push(item);
                        }
                        else if (item.SemesterId == 2) {
                            list2ndYear2ndSem.push(item);
                        }
                    }
                    else if (item.YearId == 3) {
                        if (item.SemesterId == 1) {
                            list3rdYear1stSem.push(item);
                        }
                        else if (item.SemesterId == 2) {
                            list3rdYear2ndSem.push(item);
                        }
                    }
                    else if (item.YearId == 4) {
                        if (item.SemesterId == 1) {
                            list4thYear1stSem.push(item);
                        }
                        else if (item.SemesterId == 2) {
                            list4thYear2ndSem.push(item);
                        }
                    }
                })


                //-----------------Separate the list here-------------------------

                list1stYear1stSem.forEach(function (i) {
                    if (i.ObtainedTotalMarks == null) {
                        i.ObtainedGPA = '-';
                        i.ObtainedTotalMarks = "-";
                        i.ObtainedGrade = "-";
                    }
                    else {
                        if (i.ObtainedTotalMarks == 0.00 || i.ObtainedGPA == 0.00) {
                            if (!i.ObtainedGrade) {
                                i.ObtainedGrade = "F";
                            }
                        }
                        if (i.ObtainedGPA > 0.00) {
                            y1s1Credit = y1s1Credit + i.Credits;
                        }
                    }
                    y1s1gpa = parseFloat(y1s1gpa) + parseFloat(i.ObtainedGPA)
                })
                list1stYear2ndSem.forEach(function (i) {
                    if (!i.ObtainedTotalMarks) {
                        i.ObtainedGPA = '-';
                        i.ObtainedTotalMarks = "-";
                        i.ObtainedGrade = "-";
                    }
                    else {
                        if (i.ObtainedTotalMarks == 0.00 || i.ObtainedGPA == 0) {
                            if (!i.ObtainedGrade) {
                                i.ObtainedGrade = "F";
                            }
                        }
                        if (i.ObtainedGPA > 0.00)
                            y1s2Credit = y1s2Credit + i.Credits;
                    }
                    y1s2gpa = y1s2gpa + i.ObtainedGPA
                })

                list2ndYear1stSem.forEach(function (i) {
                    if (!i.ObtainedTotalMarks) {
                        i.ObtainedGPA = '-';
                        i.ObtainedTotalMarks = "-";
                        i.ObtainedGrade = "-";
                    }
                    else {
                        if (i.ObtainedTotalMarks == 0.00 || i.ObtainedGPA == 0) {
                            if (!i.ObtainedGrade) {
                                i.ObtainedGrade = "F";
                            }
                        }
                        if (i.ObtainedGPA > 0.00)
                            y2s1Credit = y2s1Credit + i.Credits;
                    }
                    y2s1gpa = y2s1gpa + i.ObtainedGPA
                })
                list2ndYear2ndSem.forEach(function (i) {
                    if (!i.ObtainedTotalMarks) {
                        i.ObtainedGPA = '-';
                        i.ObtainedTotalMarks = "-";
                        i.ObtainedGrade = "-";
                    }
                    else {
                        if (i.ObtainedTotalMarks == 0.00 || i.ObtainedGPA == 0) {
                            if (!i.ObtainedGrade) {
                                i.ObtainedGrade = "F";
                            }
                        }
                        if (i.ObtainedGPA > 0.00)
                            y2s2Credit = y2s2Credit + i.Credits
                    }
                    y2s2gpa = y2s2gpa + i.ObtainedGPA
                })

                list3rdYear1stSem.forEach(function (i) {
                    if (!i.ObtainedTotalMarks) {
                        i.ObtainedGPA = '-';
                        i.ObtainedTotalMarks = "-";
                        i.ObtainedGrade = "-";
                    }
                    else {
                        if (i.ObtainedTotalMarks == 0.00 || i.ObtainedGPA == 0) {
                            if (!i.ObtainedGrade) {
                                i.ObtainedGrade = "F";
                            }
                        }
                        if (i.ObtainedGPA > 0.00)
                            y3s1Credit = y3s1Credit + i.Credits;
                    }
                    y3s1gpa = y3s1gpa + i.ObtainedGPA
                })
                list3rdYear2ndSem.forEach(function (i) {
                    if (!i.ObtainedTotalMarks) {
                        i.ObtainedGPA = '-';
                        i.ObtainedTotalMarks = "-";
                        i.ObtainedGrade = "-";
                    }
                    else {
                        if (i.ObtainedTotalMarks == 0.00 || i.ObtainedGPA == 0.00) {
                            if (!i.ObtainedGrade) {
                                i.ObtainedGrade = "F";
                            }
                        }
                        if (i.ObtainedGPA > 0.00)
                            y3s2Credit = y3s2Credit + i.Credits;
                    }
                    y3s2gpa = y3s2gpa + i.ObtainedGPA
                })

                list4thYear1stSem.forEach(function (i) {
                    if (!i.ObtainedTotalMarks) {
                        i.ObtainedGPA = '-';
                        i.ObtainedTotalMarks = "-";
                        i.ObtainedGrade = "-";
                    }
                    else {
                        if (i.ObtainedTotalMarks == 0.00 || i.ObtainedGPA == 0) {
                            if (!i.ObtainedGrade) {
                                i.ObtainedGrade = "F";
                            }
                        }
                        if (i.ObtainedGPA > 0.00)
                            y4s1Credit = y4s1Credit + i.Credits;
                    }
                    y4s1gpa = y4s1gpa + i.ObtainedGPA
                })
                list4thYear2ndSem.forEach(function (i) {
                    if (!i.ObtainedTotalMarks) {
                        i.ObtainedGPA = '-';
                        i.ObtainedTotalMarks = "-";
                        i.ObtainedGrade = "-";
                    }
                    else {
                        if (i.ObtainedTotalMarks == 0.00 || i.ObtainedGPA == 0) {
                            if (!i.ObtainedGrade) {
                                i.ObtainedGrade = "F";
                            }
                        }
                        if (i.ObtainedGPA > 0.00)
                            y4s2Credit = y4s2Credit + i.Credits;
                    }
                    y4s2gpa = y4s2gpa + i.ObtainedGPA
                })

                totalCredit = y1s1Credit + y1s2Credit + y2s1Credit + y2s2Credit + y3s1Credit + y3s2Credit + y4s1Credit + y4s2Credit;

                var firstCGPA;
                var secondCGPA;
                var thirdCGPA;
                var fourthCGPA;
                var fifthCGPA;
                var sixthCGPA;
                var seventhCGPA;
                var eigthCGPA;

                //---------Create Table--------------------

                //------------------First Table-----------------------------
                var FirstYearFirstSemTable = ""
                                           + "<table class='text-center academic'>"
                                            + "<thead>"
                                                + "<tr>"
                                                    + "<th colspan='5'>1st Year 1st Semester</th>"
                                                + "<tr>"
                                                + "<tr>"
                                                    + "<th>Course No.</th>"
                                                    + "<th>Course Title</th>"
                                                    + "<th>Credit Hour(s)</th>"
                                                    + "<th>Grade Point</th>"
                                                    + "<th>Letter Grade</th>"
                                                + "<tr>"
                                            + "</thead>"
                                            + "<tbody>"
                list1stYear1stSem.forEach(function (i) {
                    FirstYearFirstSemTable += "<tr>"
                                                + "<td>" + i.FormalCode + "</td>"
                                                + "<td class='text-left'>" + i.Title + "</td>"
                                                + "<td>" + i.Credits + "</td>"
                                                + "<td>" + i.ObtainedGrade + "</td>"
                                                + "<td>" + i.ObtainedGPA + "</td>"                    
                                            + "</tr>"
                })
                if (list1stYear1stSem.length < list1stYear2ndSem.length) {
                    var length = list1stYear2ndSem.length - list1stYear1stSem.length
                    for (i = 0; i < length; i++) {
                        FirstYearFirstSemTable += "<tr>"
                                                    + "<td>-</td>"
                                                    + "<td>-</td>"
                                                    + "<td>-</td>"
                                                    + "<td>-</td>"
                                                    + "<td>-</td>"
                                                + "</tr>"
                    }
                }
                firstCGPA = (parseFloat(y1s1gpa) / list1stYear1stSem.length).toFixed(2) == "NaN" ? 0 : (parseFloat(y1s1gpa) / list1stYear1stSem.length).toFixed(2)
                FirstYearFirstSemTable += "</tbody>"
                                            + "<tfoot>"
                                                + "<tr>"
                                                    + "<td colspan='2'>Credits Earned: <b>" + parseFloat(y1s1Credit) + "</b></td>"
                                                    + "<td colspan='3'>Semester GPA: <b>" + parseFloat(y1s1gpa) + "</b></td>"
                                                + "</tr>"
                                                + "<tr>"
                                                    + "<td colspan='2'>Cumulative Credits: <b>" + (totalCredit - y1s2Credit - y2s1Credit - y2s2Credit - y3s1Credit - y3s2Credit - y4s1Credit - y4s2Credit) + "</b></td>"
                                                    + "<td colspan='3'>Cumulative Grade Points Avarage (CGPA) : <b>" + firstCGPA + "</b></td>"
                                                + "</tr>"
                                            + "</tfoot>"
                                           + "</table>"

                //---------------------Second Table-------------------------
                var FirstYearSecondSemTable = ""
                                           + "<table class='text-center academic'>"
                                            + "<thead>"
                                                + "<tr>"
                                                    + "<th colspan='5'>1st Year 2nd Semester</th>"
                                                + "<tr>"
                                                + "<tr>"
                                                    + "<th>Course No.</th>"
                                                    + "<th>Course Title</th>"
                                                    + "<th>Credit Hour(s)</th>"
                                                    + "<th>Grade Point</th>"
                                                    + "<th>Letter Grade</th>"
                                                + "<tr>"
                                            + "</thead>"
                                            + "<tbody>"
                list1stYear2ndSem.forEach(function (i) {
                    FirstYearSecondSemTable += "<tr>"
                                                + "<td>" + i.FormalCode + "</td>"
                                                + "<td class='text-left'>" + i.Title + "</td>"
                                                + "<td>" + i.Credits + "</td>"
                                                + "<td>" + i.ObtainedGrade + "</td>"
                                                + "<td>" + i.ObtainedGPA + "</td>"
                                            + "</tr>"
                })
                if (list1stYear2ndSem.length < list1stYear1stSem.length) {
                    var length = list1stYear1stSem.length - list1stYear2ndSem.length
                    for (i = 0; i < length; i++) {
                        FirstYearSecondSemTable += "<tr>"
                                                    + "<td>-</td>"
                                                    + "<td>-</td>"
                                                    + "<td>-</td>"
                                                    + "<td>-</td>"
                                                    + "<td>-</td>"
                                                + "</tr>"
                    }
                }
                secondCGPA = ((y1s2gpa + y1s1gpa) / (list1stYear1stSem.length + list1stYear2ndSem.length)).toFixed(2) == "NaN" ? 0 : ((y1s2gpa + y1s1gpa) / (list1stYear1stSem.length + list1stYear2ndSem.length)).toFixed(2);
                FirstYearSecondSemTable += "</tbody>"
                                            + "<tfoot>"
                                                + "<tr>"
                                                    + "<td colspan='2'>Credits Earned: <b>" + parseFloat(y1s2Credit) + "</b></td>"
                                                    + "<td colspan='3'>Semester GPA: <b>" + parseFloat(y1s2gpa) + "</b></td>"
                                                + "</tr>"
                                                + "<tr>"
                                                    + "<td colspan='2'>Cumulative Credits: <b>" + (totalCredit - y2s1Credit - y2s2Credit - y3s1Credit - y3s2Credit - y4s1Credit - y4s2Credit) + "</b></td>"
                                                    + "<td colspan='3'>Cumulative Grade Points Avarage (CGPA): <b>" + secondCGPA + "</b></td>"
                                                + "</tr>"
                                            + "</tfoot>"
                                           + "</table>"

                //---------------Third Table-----------------------
                var SecondYearFirstTable = ""
                                           + "<table class='text-center academic'>"
                                            + "<thead>"
                                                + "<tr>"
                                                    + "<th colspan='5'>2nd Year 1st Semester</th>"
                                                + "<tr>"
                                                + "<tr>"
                                                    + "<th>Course No.</th>"
                                                    + "<th>Course Title</th>"
                                                    + "<th>Credit Hour(s)</th>"
                                                    + "<th>Grade Point</th>"
                                                    + "<th>Letter Grade</th>"
                                                + "<tr>"
                                            + "</thead>"
                                            + "<tbody>"
                list2ndYear1stSem.forEach(function (i) {
                    SecondYearFirstTable += "<tr>"
                                                + "<td>" + i.FormalCode + "</td>"
                                                + "<td class='text-left'>" + i.Title + "</td>"
                                                + "<td>" + i.Credits + "</td>"
                                                + "<td>" + i.ObtainedGrade + "</td>"
                                                + "<td>" + i.ObtainedGPA + "</td>"
                                            + "</tr>"
                })
                if (list2ndYear1stSem.length < list2ndYear2ndSem.length) {
                    var length = list2ndYear2ndSem.length - list2ndYear1stSem.length
                    for (i = 0; i < length; i++) {
                        SecondYearFirstTable += "<tr>"
                                                    + "<td>-</td>"
                                                    + "<td>-</td>"
                                                    + "<td>-</td>"
                                                    + "<td>-</td>"
                                                    + "<td>-</td>"
                                                + "</tr>"
                    }
                }
                thirdCGPA = ((y1s2gpa + y1s1gpa + y2s2gpa) / (list1stYear1stSem.length + list1stYear2ndSem.length + list2ndYear1stSem.length)).toFixed(2) == "NaN" ? 0 : ((y1s2gpa + y1s1gpa + y2s2gpa) / (list1stYear1stSem.length + list1stYear2ndSem.length + list2ndYear1stSem.length)).toFixed(2);
                SecondYearFirstTable += "</tbody>"
                                            + "<tfoot>"
                                                + "<tr>"
                                                    + "<td colspan='2'>Credits Earned: <b>" + parseFloat(y2s1gpa) + "</b></td>"
                                                    + "<td colspan='3'>Semester GPA: <b>" + parseFloat(y1s2gpa) + "</b></td>"
                                                + "</tr>"
                                                + "<tr>"
                                                    + "<td colspan='2'>Cumulative Credits: <b>" + (totalCredit - y2s2Credit - y3s1Credit - y3s2Credit - y4s1Credit - y4s2Credit) + "</b></td>"
                                                    + "<td colspan='3'>Cumulative Grade Points Avarage (CGPA): <b>" + thirdCGPA + "</b></td>"
                                                + "</tr>"
                                            + "</tfoot>"
                                           + "</table>"

                //---------------Fourth Table--------------------
                var SecondYearSecondTable = ""
                                           + "<table class='text-center academic'>"
                                            + "<thead>"
                                                + "<tr>"
                                                    + "<th colspan='5'>2nd Year 2nd Semester</th>"
                                                + "<tr>"
                                                + "<tr>"
                                                    + "<th>Course No.</th>"
                                                    + "<th>Course Title</th>"
                                                    + "<th>Credit Hour(s)</th>"
                                                    + "<th>Grade Point</th>"
                                                    + "<th>Letter Grade</th>"
                                                + "<tr>"
                                            + "</thead>"
                                            + "<tbody>"
                list2ndYear2ndSem.forEach(function (i) {
                    SecondYearSecondTable += "<tr>"
                                                + "<td>" + i.FormalCode + "</td>"
                                                + "<td class='text-left'>" + i.Title + "</td>"
                                                + "<td>" + i.Credits + "</td>"
                                                + "<td>" + i.ObtainedGrade + "</td>"
                                                + "<td>" + i.ObtainedGPA + "</td>"
                                            + "</tr>"
                })
                if (list2ndYear2ndSem.length < list2ndYear1stSem.length) {
                    var length = list2ndYear1stSem.length - list2ndYear2ndSem.length
                    for (i = 0; i < length; i++) {
                        SecondYearSecondTable += "<tr>"
                                                    + "<td>-</td>"
                                                    + "<td>-</td>"
                                                    + "<td>-</td>"
                                                    + "<td>-</td>"
                                                    + "<td>-</td>"
                                                + "</tr>"
                    }
                }
                fourthCGPA = ((y1s2gpa + y1s1gpa + y2s2gpa + y2s2gpa) / (list1stYear1stSem.length + list1stYear2ndSem.length + list2ndYear1stSem.length + list2ndYear2ndSem.length)).toFixed(2) == "NaN" ? 0 : ((y1s2gpa + y1s1gpa + y2s2gpa + y2s2gpa) / (list1stYear1stSem.length + list1stYear2ndSem.length + list2ndYear1stSem.length + list2ndYear2ndSem.length)).toFixed(2);
                SecondYearSecondTable += "</tbody>"
                                            + "<tfoot>"
                                                + "<tr>"
                                                    + "<td colspan='2'>Credits Earned: <b>" + parseFloat(y2s2gpa) + "</b></td>"
                                                    + "<td colspan='3'>Semester GPA: <b>" + parseFloat(y1s2gpa) + "</b></td>"
                                                + "</tr>"
                                                + "<tr>"
                                                    + "<td colspan='2'>Cumulative Credits: <b>" + (totalCredit - y3s1Credit - y3s2Credit - y4s1Credit - y4s2Credit) + "</b></td>"
                                                    + "<td colspan='3'>Cumulative Grade Points Avarage (CGPA): <b>" + fourthCGPA + "</b></td>"
                                                + "</tr>"
                                            + "</tfoot>"
                                           + "</table>"

                //-------------Fifth Table----------------

                var ThirdYearFirstTable = ""
                                           + "<table class='text-center academic'>"
                                            + "<thead>"
                                                + "<tr>"
                                                    + "<th colspan='5'>3rd Year 1st Semester</th>"
                                                + "<tr>"
                                                + "<tr>"
                                                    + "<th>Course No.</th>"
                                                    + "<th>Course Title</th>"
                                                    + "<th>Credit Hour(s)</th>"
                                                    + "<th>Grade Point</th>"
                                                    + "<th>Letter Grade</th>"
                                                + "<tr>"
                                            + "</thead>"
                                            + "<tbody>"
                list3rdYear1stSem.forEach(function (i) {
                    ThirdYearFirstTable += "<tr>"
                                                + "<td>" + i.FormalCode + "</td>"
                                                + "<td class='text-left'>" + i.Title + "</td>"
                                                + "<td>" + i.Credits + "</td>"
                                                + "<td>" + i.ObtainedGrade + "</td>"
                                                + "<td>" + i.ObtainedGPA + "</td>"
                                            + "</tr>"
                })
                if (list3rdYear1stSem.length < list3rdYear2ndSem.length) {
                    var length = list3rdYear2ndSem.length - list3rdYear1stSem.length
                    for (i = 0; i < length; i++) {
                        ThirdYearFirstTable += "<tr>"
                                                    + "<td>-</td>"
                                                    + "<td>-</td>"
                                                    + "<td>-</td>"
                                                    + "<td>-</td>"
                                                    + "<td>-</td>"
                                                + "</tr>"
                    }
                }
                fifthCGPA = ((y1s2gpa + y1s1gpa + y2s2gpa + y2s2gpa + y3s1gpa) / (list1stYear1stSem.length + list1stYear2ndSem.length + list2ndYear1stSem.length + list2ndYear2ndSem.length + list3rdYear1stSem.length)).toFixed(2) == "NaN" ? 0 : ((y1s2gpa + y1s1gpa + y2s2gpa + y2s2gpa + y3s1gpa) / (list1stYear1stSem.length + list1stYear2ndSem.length + list2ndYear1stSem.length + list2ndYear2ndSem.length + list3rdYear1stSem.length)).toFixed(2)
                ThirdYearFirstTable += "</tbody>"
                                            + "<tfoot>"
                                                + "<tr>"
                                                    + "<td colspan='2'>Credits Earned: <b>" + parseFloat(y3s1Credit) + "</b></td>"
                                                    + "<td colspan='3'>Semester GPA: <b>" + parseFloat(y3s1gpa) + "<b></td>"
                                                + "</tr>"
                                                + "<tr>"
                                                    + "<td colspan='2'>Cumulative Credits: <b>" + (totalCredit - y3s2Credit - y4s1Credit - y4s2Credit) + "</b></td>"
                                                    + "<td colspan='3'>Cumulative Grade Points Avarage (CGPA): <b>" + fifthCGPA + "</b></td>"
                                                + "</tr>"
                                            + "</tfoot>"
                                           + "</table>"

                //-------------Sixth Table----------------

                var ThirdYearSecondTable = ""
                                           + "<table class='text-center academic'>"
                                            + "<thead>"
                                                + "<tr>"
                                                    + "<th colspan='5'>3rd Year 2nd Semester</th>"
                                                + "<tr>"
                                                + "<tr>"
                                                    + "<th>Course No.</th>"
                                                    + "<th>Course Title</th>"
                                                    + "<th>Credit Hour(s)</th>"
                                                    + "<th>Grade Point</th>"
                                                    + "<th>Letter Grade</th>"
                                                + "<tr>"
                                            + "</thead>"
                                            + "<tbody>"
                list3rdYear2ndSem.forEach(function (i) {
                    ThirdYearSecondTable += "<tr>"
                                                + "<td>" + i.FormalCode + "</td>"
                                                + "<td class='text-left'>" + i.Title + "</td>"
                                                + "<td>" + i.Credits + "</td>"
                                                + "<td>" + i.ObtainedGrade + "</td>"
                                                + "<td>" + i.ObtainedGPA + "</td>"
                                            + "</tr>"
                })
                if (list3rdYear2ndSem.length < list3rdYear1stSem.length) {
                    var length = list3rdYear1stSem.length - list3rdYear2ndSem.length
                    for (i = 0; i < length; i++) {
                        ThirdYearSecondTable += "<tr>"
                                                    + "<td>-</td>"
                                                    + "<td>-</td>"
                                                    + "<td>-</td>"
                                                    + "<td>-</td>"
                                                    + "<td>-</td>"
                                                + "</tr>"
                    }
                }
                sixthCGPA = ((y1s2gpa + y1s1gpa + y2s2gpa + y2s2gpa + y3s1gpa + y3s2gpa) / (list1stYear1stSem.length + list1stYear2ndSem.length + list2ndYear1stSem.length + list2ndYear2ndSem.length + list3rdYear1stSem.length + list3rdYear2ndSem.length)).toFixed(2) == "NaN" ? 0 : ((y1s2gpa + y1s1gpa + y2s2gpa + y2s2gpa + y3s1gpa + y3s2gpa) / (list1stYear1stSem.length + list1stYear2ndSem.length + list2ndYear1stSem.length + list2ndYear2ndSem.length + list3rdYear1stSem.length + list3rdYear2ndSem.length)).toFixed(2)
                ThirdYearSecondTable += "</tbody>"
                                            + "<tfoot>"
                                                + "<tr>"
                                                    + "<td colspan='2'>Credits Earned: <b>" + parseFloat(y3s2Credit) + "</b></td>"
                                                    + "<td colspan='3'>Semester GPA: <b>" + parseFloat(y3s2gpa) + "</b></td>"
                                                + "</tr>"
                                                + "<tr>"
                                                    + "<td colspan='2'>Cumulative Credits: <b>" + (totalCredit - y4s1Credit - y4s2Credit) + "</b></td>"
                                                    + "<td colspan='3'>Cumulative Grade Points Avarage (CGPA): <b>" + sixthCGPA + "</b></td>"
                                                + "</tr>"
                                            + "</tfoot>"
                                           + "</table>"

                //-------------Seventh Table----------------

                var FourthYearFirstTable = ""
                                           + "<table class='text-center academic'>"
                                            + "<thead>"
                                                + "<tr>"
                                                    + "<th colspan='5'>4th Year 1st Semester</th>"
                                                + "<tr>"
                                                + "<tr>"
                                                    + "<th>Course No.</th>"
                                                    + "<th>Course Title</th>"
                                                    + "<th>Credit Hour(s)</th>"
                                                    + "<th>Grade Point</th>"
                                                    + "<th>Letter Grade</th>"
                                                + "<tr>"
                                            + "</thead>"
                                            + "<tbody>"
                list4thYear1stSem.forEach(function (i) {
                    FourthYearFirstTable += "<tr>"
                                                + "<td>" + i.FormalCode + "</td>"
                                                + "<td class='text-left'>" + i.Title + "</td>"
                                                + "<td>" + i.Credits + "</td>"
                                                + "<td>" + i.ObtainedGrade + "</td>"
                                                + "<td>" + i.ObtainedGPA + "</td>"
                                            + "</tr>"
                })
                if (list4thYear1stSem.length < list4thYear2ndSem.length) {
                    var length = list4thYear2ndSem.length - list4thYear1stSem.length
                    for (i = 0; i < length; i++) {
                        FourthYearFirstTable += "<tr>"
                                                    + "<td>-</td>"
                                                    + "<td>-</td>"
                                                    + "<td>-</td>"
                                                    + "<td>-</td>"
                                                    + "<td>-</td>"
                                                + "</tr>"
                    }
                }
                seventhCGPA = ((y1s2gpa + y1s1gpa + y2s2gpa + y2s2gpa + y3s1gpa + y3s2gpa + y4s1gpa) / (list1stYear1stSem.length + list1stYear2ndSem.length + list2ndYear1stSem.length + list2ndYear2ndSem.length + list3rdYear1stSem.length + list3rdYear2ndSem.length + list4thYear1stSem.length)).toFixed(2) == "NaN" ? 0 : ((y1s2gpa + y1s1gpa + y2s2gpa + y2s2gpa + y3s1gpa + y3s2gpa + y4s1gpa) / (list1stYear1stSem.length + list1stYear2ndSem.length + list2ndYear1stSem.length + list2ndYear2ndSem.length + list3rdYear1stSem.length + list3rdYear2ndSem.length + list4thYear1stSem.length)).toFixed(2)
                FourthYearFirstTable += "</tbody>"
                                            + "<tfoot>"
                                                + "<tr>"
                                                    + "<td colspan='2'>Credits Earned: <b>" + parseFloat(y4s1Credit) + "</b></td>"
                                                    + "<td colspan='3'>Semester GPA: <b>" + parseFloat(y4s1gpa) + "</b></td>"
                                                + "</tr>"
                                                + "<tr>"
                                                    + "<td colspan='2'>Cumulative Credits: <b>" + (totalCredit - y4s2Credit) + "</b></td>"
                                                    + "<td colspan='3'>Cumulative Grade Points Avarage (CGPA): <b>" + seventhCGPA + "</b></td>"
                                                + "</tr>"
                                            + "</tfoot>"
                                           + "</table>"

                //-------------Eighth Table----------------

                var FourthYearSecondTable = ""
                                           + "<table class='text-center academic'>"
                                            + "<thead>"
                                                + "<tr>"
                                                    + "<th colspan='5'>4th Year 2nd Semester</th>"
                                                + "<tr>"
                                                + "<tr>"
                                                    + "<th>Course No.</th>"
                                                    + "<th>Course Title</th>"
                                                    + "<th>Credit Hour(s)</th>"
                                                    + "<th>Grade Point</th>"
                                                    + "<th>Letter Grade</th>"
                                                + "<tr>"
                                            + "</thead>"
                                            + "<tbody>"
                list4thYear2ndSem.forEach(function (i) {
                    FourthYearSecondTable += "<tr>"
                                                + "<td>" + i.FormalCode + "</td>"
                                                + "<td class='text-left'>" + i.Title + "</td>"
                                                + "<td>" + i.Credits + "</td>"
                                                + "<td>" + i.ObtainedGrade + "</td>"
                                                + "<td>" + i.ObtainedGPA + "</td>"
                                            + "</tr>"
                })
                if (list4thYear2ndSem.length < list4thYear1stSem.length) {
                    var length = list4thYear1stSem.length - list4thYear2ndSem.length
                    for (i = 0; i < length; i++) {
                        FourthYearSecondTable += "<tr>"
                                                    + "<td>-</td>"
                                                    + "<td>-</td>"
                                                    + "<td>-</td>"
                                                    + "<td>-</td>"
                                                    + "<td>-</td>"
                                                + "</tr>"
                    }
                }
                eigthCGPA = ((y1s2gpa + y1s1gpa + y2s2gpa + y2s2gpa + y3s1gpa + y3s2gpa + y4s1gpa + y4s2gpa) / (list1stYear1stSem.length + list1stYear2ndSem.length + list2ndYear1stSem.length + list2ndYear2ndSem.length + list3rdYear1stSem.length + list3rdYear2ndSem.length + list4thYear1stSem.length + list4thYear2ndSem.length)).toFixed(2) == "NaN" ? 0 : ((y1s2gpa + y1s1gpa + y2s2gpa + y2s2gpa + y3s1gpa + y3s2gpa + y4s1gpa + y4s2gpa) / (list1stYear1stSem.length + list1stYear2ndSem.length + list2ndYear1stSem.length + list2ndYear2ndSem.length + list3rdYear1stSem.length + list3rdYear2ndSem.length + list4thYear1stSem.length + list4thYear2ndSem.length)).toFixed(2)

                FourthYearSecondTable += "</tbody>"
                                            + "<tfoot>"
                                                + "<tr>"
                                                    + "<td colspan='2'>Credits Earned: <b>" + parseFloat(y4s2Credit) + "</b></td>"
                                                    + "<td colspan='3'>Semester GPA: <b>" + parseFloat(y4s2gpa) + "</b></td>"
                                                + "</tr>"
                                                + "<tr>"
                                                    + "<td colspan='2'>Cumulative Credits: <b>" + (totalCredit) + "</b></td>"
                                                    + "<td colspan='3'>Cumulative Grade Points Avarage (CGPA): <b>" + eigthCGPA + "</b></td>"
                                                + "</tr>"
                                            + "</tfoot>"
                                           + "</table>"


                var header = '<thead>'
                            + '<tr>'
                               + '<th colspan="2" class="text-center">'
                                    + '<img src="../../../Images/PABNA_logo.png" width="10%" /><br />'
                                    + '<h2>Pabna University of Scicene and Technology</h2>'
                                    + '<div>'
                                        + '<h3 style="font-size:25px"><u>Detailed Academic Record</u></h3>                        '
                                    + '</div>'
                                + '</th>'
                            + '</tr>'
                            + '<tr>'
                               + '<th>'
                                    + '<table id="headerTable">'
                                        + '<tr>'
                                            + '<td>Name of the Student</td>'
                                            + '<td>:</td>'
                                            + '<td>' + student.FullName + '</td>'
                                        + '</tr>'
                                        + '<tr>'
                                            + '<td>Session</td>'
                                            + '<td>:</td>'
                                            + '<td>' + student.Session + '</td>'
                                        + '</tr>'
                                        + '<tr>'
                                            + '<td>Roll Number</td>'
                                            + '<td>:</td>'
                                            + '<td>' + student.Roll + '</td>'
                                        + '</tr>'
                                    + '</table>'
                                + '</th>'
                            + '</tr>'
                           + '</thead>'

                var footer = '<tfoot>'
                            + '<tr>'
                                + '<td>'
                                    + '<div class="row" style="width:250%;margin-top:100px">'
                                        + ' <div class="col-lg-3">'
                                            + 'Prepared By : .........................'
                                        + '</div>'
                                        + '<div class="col-lg-3">'
                                            + 'Verified By : .........................'
                                        + '</div>'
                                        + '<div class="col-lg-6 text-center" style="margin-top:25px">'
                                            + '.................................................................................... <br />'
                                            + '<span>Controller of Examinaiton</span><br />'
                                            + '<span>Pabna University of science and Technology</span><br />'
                                            + '<span>Pabna, Bangladesh</span>'
                                        + '</div>'
                                    + '  </div>'
                                + '</td>'
                            + '</tr>'
                           + '</tfoot>'

                var frontTable = "<table class='mainTable'>"
                                + "<tbody>"
                                    + "<tr>"
                                        + "<td>" + FirstYearFirstSemTable + "</td>"
                                        + "<td>" + FirstYearSecondSemTable + "</td>"
                                    + "</tr>"
                                    + "<tr>"
                                        + "<td>" + SecondYearFirstTable + "</td>"
                                        + "<td>" + SecondYearSecondTable + "</td>"
                                    + "</tr>"
                                + "</tbody>"
                               + "</table>"

                var backTable = "<table class='mainTable'>"
                                + "<tbody>"
                                    + "<tr>"
                                        + "<td>" + ThirdYearFirstTable + "</td>"
                                        + "<td>" + ThirdYearSecondTable + "</td>"
                                    + "</tr>"
                                    + "<tr>"
                                        + "<td>" + FourthYearFirstTable + "</td>"
                                        + "<td>" + FourthYearSecondTable + "</td>"
                                    + "</tr>"
                                + "</tbody>"
                               + "</table>"

                $("#header").append(header)
                $("#first").append(frontTable)
                $("#second").append(backTable)



            },
            error: function (data) {
                Swal.fire({
                    icon: 'error',
                    title: 'ERROR',
                    text: 'Error on Collecting Data'
                })
            }
        })
    }

}

function printFunction() {
    event.preventDefault();
    var header = $("#header").html();
    var frontTable1 = $("#first").html();
    var backTable1 = $("#second").html();
    var footer = '<tfoot>'
                + '<tr>'
                    + '<td colspan="2">'
                        + '<div class="d-flex" style="margin-top: 100px">'
                            + '<div class="col-lg-3">Prepared By : .........................</div>'
                            + '<div class="col-lg-3">Verified By : .........................</div>'
                            + '<div class="col-lg-6 text-center">....................................................................................'
                                + '<br />'
                                + '<span>Controller of Examinaiton</span><br />'
                                + '<span>Pabna University of science and Technology</span><br />'
                                + '<span>Pabna, Bangladesh</span></div>'
                        + '</div>'
                    + '</td>'
                + '</tr>'
            + '</tfoot>'

    var fronttable = "<table>"
                    + header
                    + "<tbody>"
                        + "<tr>"
                            + "<td>" + frontTable1 + "</td>"
                        + "</tr>"                        
                    + "</tbody>"
                    +footer
                   + "</table>"

    var backTable = "<table>"
                    + header
                    + "<tbody>"
                        + "<tr>"
                            + "<td>" + backTable1 + "</td>"
                        + "</tr>"                        
                    + "</tbody>"
                    +footer
                   + "</table>"
    var typeElement = "";
    var mywindow = window.open("", "PRINT", 1600, 1600);
    var typeElement = '<!DOCTYPE html><html>';
    typeElement += '<head>';
    typeElement += '<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/css/bootstrap.min.css">';
    typeElement += '<script src="https://cdn.jsdelivr.net/npm/jquery@3.6.1/dist/jquery.slim.min.js"></script>';
    typeElement += '<script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.1/dist/umd/popper.min.js"></script>';
    typeElement += '<script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/js/bootstrap.bundle.min.js"></script>';
    typeElement += '<style>';
    typeElement += 'body{margin-top:30px}table{font-size:10px}.academic{border-collapse: collapse;}';
    typeElement += '.academic th, .academic td{border:1px solid black;}.mainTable td table{vertical-align:top;width:100%;height:30%;white-space:nowrap}';
    typeElement += '.mainTable td{padding:0 15px 5px 0;}.mainTable tr{margin:10px}';
    typeElement += 'footer span {font-family: serif;font-size: 20px;}';
    typeElement += 'footer label {font-family: serif;font-size: 20px;}';
    typeElement += '@media print {tfoot {page-break-after: always;}}';
    typeElement += '</style>';
    typeElement += '<title>Rpt Student Academic Grade Sheet</title>';
    typeElement += '</head>';
    typeElement += '<body>';
    typeElement += '<div class="container-fluid">';
    typeElement += fronttable;
    typeElement += '</div>';
    typeElement += '<div style="page-break-before: always;"></div>';
    typeElement += '<div class="container-fluid">';
    typeElement += backTable;
    typeElement += '</div>';
    typeElement += '</body></html>';

    mywindow.document.write(typeElement);
    mywindow.document.close();

    mywindow.onload = function () {
        mywindow.print();
    };

}

