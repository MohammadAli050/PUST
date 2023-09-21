$(document).ready(function () {
    //$("#btnLoad").click(function () {
    //    event.preventDefault();
    //    var heldInID = $("#ctl00_MainContainer_ddlHeldIn").val();
    //    console.log(heldInID)        
    //})
})


function loadFunction() {
    var heldInID = $("#ctl00_MainContainer_ddlHeldIn").val();
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
            url: "RptSemesterResult.aspx/LoadSemesterResultReport",
            data: "{'heldInID':'" + heldInID + "'}",
            dataType: "json",
            success: function (data) {
                var data = JSON.parse(data.d);
                var studentID = GetUniqueColumnValues(data, "StudentID");
                var CourseName = GetUniqueColumnValues(data, "FormalCode");
                var list = [];
                var count = 0;
                var studentMap = {};

                data.forEach(studentData => {
                    var studentID = studentData.StudentID;
                    if (!studentMap[studentID]) {
                        studentMap[studentID] = {
                            roll: studentData.Roll,
                            reg: studentData.Reg,
                            fullName: studentData.FullName,
                            fatherName: studentData.FatherName,
                            totalPointSecured: studentData.TotalPointSecured,
                            gpa: studentData.GPA,
                            marks: {}
                        };
                    }
                    studentMap[studentID].marks[studentData.FormalCode] = studentData.ObtainedGrade || ""; // Use ObtainedGrade property
                });

                var tableInfo = "<table class='text-center' id='tableInfo'>"
                                    + "<thead>"
                                       + "<tr>"
                                            + "<th rowspan='2' >Roll No.</th>"
                                            + "<th rowspan='2' >Reg No.</th>"
                                            + "<th rowspan='2' >Student's Name</th>"
                                            + "<th rowspan='2' >Father's Name</th>";

                for (var i = 0; i < CourseName.length; i++) {
                    tableInfo += "<th >" + CourseName[i] + "</th>";
                }
                tableInfo += "<th rowspan='2' style='transform: rotate(90deg);'>Points Secured</th>"
                          + "<th rowspan='2' style='transform: rotate(90deg);'>SGPA</th>"
                          + "<th rowspan='2' style='transform: rotate(90deg);'>YGPA</th>"
                          + "<th rowspan='2' style='transform: rotate(90deg);'>Comments</th>"
                          + "</tr>"
                          + "<tr>"
                            + "<th colspan='" + CourseName.length + "' >LG</th>"
                          + "</tr>"
                     + "</thead>"
                     + "<tbody>"
                studentID.forEach(studentID => {
                    var studentInfo = studentMap[studentID];
                    tableInfo += "<tr>"
                                + "<td>" + studentInfo.roll + "</td>"
                                + "<td>" + studentInfo.roll + "</td>"
                                + "<td>" + studentInfo.fullName + "</td>"
                                + "<td>" + studentInfo.fatherName + "</td>"
                    CourseName.forEach(subject => {
                        var mark = studentInfo.marks[subject]
                        tableInfo += "<td>" + mark + "</td>"
                    })
                    tableInfo += "<td>" + studentInfo.totalPointSecured + "</td>"
                                + "<td>" + studentInfo.gpa + "</td>"
                                + "<td>" + studentInfo.gpa + "</td>"
                    if (studentInfo.gpa >= 2) {
                        tableInfo += "<td>P</td>"
                    }
                    else {
                        tableInfo += "<td>F</td>"
                    }
                    tableInfo += "</tr>"
                })
                tableInfo += "</tbody>"
                     + "</table>";

                $("#tablediv").append(tableInfo);
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
    var sem = split[1] + " " + split[2] + " " + split[3] + " " + split[4]
    var element = $("#tablediv").html();
    var typeElement = "";
    var mywindow = window.open("", "PRINT", 1600, 2400)
    typeElement += '<!DOCTYPE html><html>';
    typeElement += '<head><link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/css/bootstrap.min.css"><script src="https://cdn.jsdelivr.net/npm/jquery@3.6.1/dist/jquery.slim.min.js"></script><script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.1/dist/umd/popper.min.js"></script><script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/js/bootstrap.bundle.min.js"></script><style>body{margin-top:30px}#tableInfo{border-collapse: collapse;} #tableInfo th,#tableInfo td{border:1px solid black;white-space:nowrap;padding:20px}footer span {font-family: serif;font-size: 20px;}footer label {font-family: serif;font-size: 20px;}@media print{@page{size:landscape}}</style></head>';
    typeElement += '<body >'
    typeElement += '<div class="container-fluid">'
    typeElement += '<table class="text-center">'
    typeElement += '<thead>'
    typeElement += '<tr>'
    typeElement += '<th><img style="width:5%" src="../../../Images/PABNA_logo.png" /><h1>Pabna University of Science and Technology</h1><th>'
    typeElement += '</tr>'
    typeElement += '<tr>'
    typeElement += '<th><h3>' + Dept + '</h3><th>'
    typeElement += '</tr>'
    typeElement += '<tr>'
    typeElement += '<th>Result sheet of ' + sem + ' Examination-<th>'
    typeElement += '</tr>'
    typeElement += '<tr>'
    typeElement += '<th>Session:' + split[0] + '<th>'
    typeElement += '</tr>'
    typeElement += '</thead>'
    typeElement += '<tbody>'
    typeElement += '<tr>'
    typeElement += '<td>' + element + '</td>'
    typeElement += '</tr>'
    typeElement += '</tbody>'
    typeElement += '</table>'
    typeElement += '<br/>'
    typeElement += '<div>'
    typeElement += '</div>'
    typeElement += '<footer class="container-fluid">'
    typeElement += '<div class="row">'
    typeElement += '<div class="col-4">'
    typeElement += '<label>Name & Signature of the Tabulators</label>'
    typeElement += '<br />'
    typeElement += '<div style="margin-left:23px">'
    typeElement += '<span>1.</span><br />'
    typeElement += '<span>2.</span><br />'
    typeElement += '<span>3.</span><br />'
    typeElement += '</div>'
    typeElement += '</div>'
    typeElement += '<div class="col-4">'
    typeElement += '<label>Date of the Published Result</label>'
    typeElement += '</div>'
    typeElement += '<div class="col-4" style="margin-top:100px;">'
    typeElement += '<div class="text-center">'
    typeElement += '<hr />'
    typeElement += '<label>Controller of Examinations</label>'
    typeElement += '</div>'
    typeElement += '</div>'
    typeElement += '</div>'
    typeElement += '</footer>'
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