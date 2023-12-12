$(document).ready(function () {
    //$("#btnLoad").click(function () {
    //    event.preventDefault();
    //    var heldInID = $("#ctl00_MainContainer_ddlHeldIn").val();
    //    console.log(heldInID)        
    //})
})
var stdroll = 0
function gradeCal(grade) {
    switch (grade) {
        case 4:
            return "A+"
            break;
        case 3.75:
            return "A"
            break;
        case 3.50:
            return "A-"
            break;
        case 3.25:
            return "B+"
            break;
        case 3.00:
            return "B"
            break;
        case 2.75:
            return "B-"
            break;
        case 2.50:
            return "C+"
            break;
        case 2.25:
            return "C"
            break;
        case 2.00:
            return "D"
            break;
        case 0:
            return "F"
            break;
    }
}

function loadFunction() {
    var heldInID = $("#ctl00_MainContainer_ddlHeldIn").val();
    var roll = $("#rollInput").val();
    if (heldInID == null || heldInID == '0') {
        Swal.fire({
            icon: 'error',
            title: 'No Semester & Held in',
            text: 'Please Select Semester & Held in'
        })
    }
    else {
        $("#divProgress").show();
        $("#tablediv").children().remove();
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "RptGradeSheetProvisional.aspx/Loadgradesheetprovisonal",
            data: "{'heldInID':'" + heldInID + "','roll':'" + roll + "'}",
            dataType: "json",
            success: function (data) {
                var data = JSON.parse(data.d);
                var academic = data.academicInfo;
                var student = data.studentInfo[0];
                var totalData = data.totaldata;

                var gpa = 0;
                var totalcredit = 0;


                var tableInfo = "<table style='font-family:monospace; letter-spacing:4px;font-size:18px'>"
                                    + "<tbody>"
                                       + "<tr>"
                                        + "<td>Student's Name</td>"
                                        + "<td>:</td>"
                                        + "<td>" + student.FullName + "</td>"
                                       + "</tr>"
                                       + "<tr>"
                                        + "<td>Father's Name</td>"
                                        + "<td>:</td>"
                                        + "<td>" + student.FatherName + "</td>"
                                       + "</tr>"
                                       + "<tr>"
                                        + "<td>Mother's Name</td>"
                                        + "<td>:</td>"
                                        + "<td>" + student.MotherName + "</td>"
                                       + "</tr>"
                                       + "<tr>"
                                        + "<td>Roll No.</td>"
                                        + "<td>:</td>"
                                        + "<td>" + student.Roll + "</td>"
                                       + "</tr>"
                                       + "<tr>"
                                        + "<td>Registration No.</td>"
                                        + "<td>:</td>"
                                        + "<td>" + student.RegNo + "</td>"
                                        + "<td></td>"
                                        + "<td>Session</td>"
                                        + "<td>:</td>"
                                        + "<td>" + student.Session + "</td>"
                                       + "</tr>"
                                    + "</tbody>"



                var fullTable = "<table class='text-center' id='tableInfo'>"
                                + "<thead class='text-center'>"
                                    + "<tr>"
                                        + "<th>Course No.</th>"
                                        + "<th>Course Title</th>"
                                        + "<th>Credit</th>"
                                        + "<th>Letter Grade</th>"
                                        + "<th>Grade Point</th>"
                                    + "</tr>"
                                + "</thead>"
                                + "</tbody>"
                for (i = 0; i < academic.length; i++) {
                    academic[i].ObtainedGrade = gradeCal(academic[i].ObtainedGPA);
                    fullTable += "<tr>"
                                    + "<td class='text-left'>" + academic[i].FormalCode + "</td>"
                                    + "<td class='text-left' style='width:5%'>" + academic[i].Title + "</td>"
                                    + "<td>" + academic[i].CourseCredit + "</td>"
                                    + "<td>" + academic[i].ObtainedGrade + "</td>"
                                    + "<td>" + academic[i].ObtainedGPA + "</td>"
                               + "</tr>"
                    totalcredit = totalcredit + academic[i].CourseCredit;
                    gpa = gpa + academic[i].ObtainedGPA;
                }
                fullTable += "</tbody>"
                           + "<tfoot>"
                            + "<tr>"
                                + "<td colspan='2' class='text-right'><strong>Total</strong>"
                                + "<td> <b>" + totalcredit + "</b></td>"
                                + "<td></td>"
                                + "<td><b>" + (gpa / academic.length).toFixed(2) + "</b></td>"
                            + "</tr>"
                           + "</tfoot>"
                    + "</table>"

                var footerTable = "<table style='font-size:12px'>"
                                    + "<tr>"
                                        + "<td>Registered Total Credit in this Term</td>"
                                        + "<td style='padding-left:15px;padding-right:15px;'>:</td>"
                                        + "<td>" + totalcredit + "</td>"
                                    + "</tr>"
                                    + "<tr>"
                                        + "<td>Earned Total Credit in this Term</td>"
                                        + "<td style='padding-left:15px;padding-right:15px;'>:</td>"
                                        + "<td>" + totalcredit + "</td>"
                                    + "</tr>"
                                    + "<tr>"
                                        + "<td>GPA</td>"
                                        + "<td style='padding-left:15px;padding-right:15px;'>:</td>"
                                        + "<td>" + (gpa / academic.length).toFixed(2) + "</td>"
                                    + "</tr>"
                                + "</table>"

                $("#tablediv").append(tableInfo);
                $("#tablediv").append("<br/>");
                $("#tablediv").append(fullTable);
                $("#tablediv").append("<br/>");
                $("#tablediv").append(footerTable);
                $("#divProgress").hide();

            },
            error: function (data) {

            }
        })
    }
}

function printFunction() {
    var Dept = $("#ctl00_MainContainer_ucDepartment_ddlDepartment option:selected").text();
    var heldIn = $("#ctl00_MainContainer_ddlHeldIn option:selected").text();
    var split = heldIn.split(" ");
    var roll = $("#rollInput").val();
    var sem = split[1] + " " + split[2] + " " + split[3] + " " + split[4]
    var element = $("#tablediv").html();
    var header = "";
    stdroll = roll;

    header += '<div>'
            + '<h1 class="text-center">Pabna Universtiy of Science and Tecnology</h1>'
            + '<h1 class="text-center">Pabna-6600</h1>'
            + '<div class="d-flex flex-row justify-content-between">'
                + '<div class="p-2"></div>'
                + '<div class="p-2" style="width: 12%; margin-left: 35vw;">'
                    + '<img style="width: 100%" src="../../../Images/PABNA_logo.png" />'
                + '</div>'
                + '<div class="p-2" style="margin-right: 20vw">'
                    + '<table class="gradeTable" style="width: 120%">'
                        + '<thead>'
                            + '<tr class="bg-secondary">'
                                + '<th>Numerical Grade</th>'
                                + '<th>Letter Grade</th>'
                                + '<th>Grade Point</th>'
                            + '</tr>'
                        + '</thead>'
                        + '<tbody>'
                            + '<tr class="text-center">'
                                + '<td class="text-left">80% and above</td>'
                                + '<td>A+</td>'
                                + '<td>4.00</td>'
                            + '</tr>'
                            + '<tr class="text-center">'
                                + '<td class="text-left">75% to less than 80%</td>'
                                + '<td>A</td>'
                                + '<td>3.75</td>'
                            + '</tr>'
                            + '<tr class="text-center">'
                                + '<td class="text-left">70% to less than 75%</td>'
                                + '<td>A-</td>'
                                + '<td>3.50</td>'
                            + '</tr>'
                            + '<tr class="text-center">'
                                + '<td class="text-left">65% to less than 70%</td>'
                                + '<td>B+</td>'
                                + '<td>3.25</td>'
                            + '</tr>'
                            + '<tr class="text-center">'
                                + '<td class="text-left">60% to less than 65%</td>'
                                + '<td>B</td>'
                                + '<td>3.00</td>'
                            + '</tr>'
                            + '<tr class="text-center">'
                                + '<td class="text-left">55% to less than 60%</td>'
                                + '<td>B-</td>'
                                + '<td>2.75</td>'
                            + '</tr>'
                            + '<tr class="text-center">'
                                + '<td class="text-left">50% to less than 55%</td>'
                                + '<td>C+</td>'
                                + '<td>2.50</td>'
                            + '</tr>'
                            + '<tr class="text-center">'
                                + '<td class="text-left">45% to less than 50%</td>'
                                + '<td>C</td>'
                                + '<td>2.25</td>'
                            + '</tr>'
                            + '<tr class="text-center">'
                                + '<td class="text-left">40% to less than 45%</td>'
                                + '<td>D</td>'
                                + '<td>2.00</td>'
                            + '</tr>'
                            + '<tr class="text-center">'
                                + '<td class="text-left">less than 40%</td>'
                                + '<td>F</td>'
                                + '<td>0.00</td>'
                            + '</tr>'
                        + '</tbody>'
                    + '</table>'
                + '</div>'
            + '</div>'
        + '</div>';

    var footer = '<footer>'
        + '<div class="d-flex justify-content-between">'
            + '<div class="p-2">'
                + '<hr />'
                + '<span>Prepared by</span> <br />'
                + '<span>Date:</span>'
            + '</div>'
            + '<div class="p-2">'
                + '<hr />'
                + '<span>Checked by</span> <br />'
            + '</div>'
            + '<div class="p-2">'
                + '<hr />'
                + '<span>Controller of Examinations</span> <br />'
            + '</div>'
        + '</div>'
    + '</footer>'

    var newelement = '<div class="text-center">'
                        + '<h2>Grade Sheet (Provisional)</h2>'
                        + '<span> Year Semester Exam</span><br />'
                        + '<span>Department of the University</span>'
                    + '</div>'

    var typeElement = "";
    var mywindow = window.open("", "PRINT", 1600, 2400)
    typeElement += '<!DOCTYPE html><html>';
    typeElement += '<head><title>Grade Sheet(Provisional)</title><link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/css/bootstrap.min.css"><script src="https://cdn.jsdelivr.net/npm/jquery@3.6.1/dist/jquery.slim.min.js"></script><script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.1/dist/umd/popper.min.js"></script><script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/js/bootstrap.bundle.min.js"></script><style>body{margin-top:30px}#tableInfo{border-collapse: collapse;} #tableInfo th,#tableInfo td{border:1px solid black;white-space:nowrap;padding:10px;font-size:20px}.gradeTable {border-collapse: collapse;}.gradeTable th, .gradeTable td {border: 1px solid black;font-size: 12px;}footer span {font-family: serif;font-size: 15px;}</style></head>';
    typeElement += '<body >'
    typeElement += '<div class="container-fluid">'
    typeElement += header
    typeElement += '<br />'
    typeElement += newelement
    typeElement += '<br />'
    typeElement += element
    typeElement += '</div>'
    typeElement += '<br />'
    typeElement += footer
    typeElement += '</body></html>';
    mywindow.document.write(typeElement)
    mywindow.document.close();
    mywindow.onload = function () {
        mywindow.print();
    };
}

function GetUniqueColumnValues(data, column) {
    var list = [];
    $.each(data, function (i, v) {
        if (list.filter(a=>a == v[column]).length == 0) {
            list.push(v[column])
        }
    })
    return list;
}