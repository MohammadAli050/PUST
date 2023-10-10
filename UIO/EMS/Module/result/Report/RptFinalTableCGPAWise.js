function loadFunction() {
    event.preventDefault()
    var heldInId = $("#ctl00_MainContainer_ddlHeldIn").val();
    $("#table").children().remove();
    $("#divProgress").show()
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "RptFinalTableCGPAWise.aspx/GeneratefinalTabulationCGPAWise",
        data: "{'heldInId':'" + heldInId + "'}",
        dataType: "json",
        success: function (r) {
            var dataList = JSON.parse(r.d);
            var uniqueStudetn = GetUniqueColumnValues(dataList, "StudentID");
            //var dataListCopy = dataList.slice();

            // Sort the copied array by TranscriptCGPA
            //var sortbyCGPA = dataListCopy.sort(function (a, b) {
            //    return b.TranscriptCGPA - a.TranscriptCGPA;
            //});

            //console.log(sortbyCGPA)
            $("#print").show();
            var typeElement = "";
            typeElement += '<table class="academic">'
                            + '<tr>'
                                + '<td>Roll No.</td>'
                                + '<td>Reg. No.</td>'
                                + '<td>Student Name</td>'
                                + '<td>Father Name</td>'
                                + '<td>ECP11</td>'
                                + '<td>TC11</td>'
                                + '<td>SGPA11</td>'
                                + '<td>ECP12</td>'
                                + '<td>TC12</td>'
                                + '<td>SGPA12</td>'
                                + '<td>ECP21</td>'
                                + '<td>TC21</td>'
                                + '<td>SGPA21</td>'
                                + '<td>ECP22</td>'
                                + '<td>TC22</td>'
                                + '<td>SGPA22</td>'
                                + '<td>ECP31</td>'
                                + '<td>TC31</td>'
                                + '<td>SGPA31</td>'
                                + '<td>ECP32</td>'
                                + '<td>TC32</td>'
                                + '<td>SGPA32</td>'
                                + '<td>ECP41</td>'
                                + '<td>TC41</td>'
                                + '<td>SGPA41</td>'
                                + '<td>ECP42</td>'
                                + '<td>TC42</td>'
                                + '<td>SGPA42</td>'
                                + '<td>T.Credit</td>'
                            + '</tr>'
            $.each(uniqueStudetn, function (i, id) {
                let name = dataList.filter(a => a.StudentID == id)[0].StudentName;
                let father = dataList.filter(a => a.StudentID == id)[0].FatherName;
                let roll = dataList.filter(a => a.StudentID == id)[0].Roll;
                let reg = dataList.filter(a => a.StudentID == id)[0].RegNo;

                let ecp11 = dataList.filter(a => a.StudentID == id)[0].EarnedCredit;
                let ecp21 = dataList.filter(a => a.StudentID == id)[2].EarnedCredit;
                let ecp31 = dataList.filter(a => a.StudentID == id)[4].EarnedCredit;
                let ecp41 = dataList.filter(a => a.StudentID == id)[6].EarnedCredit;

                let ecp12 = dataList.filter(a => a.StudentID == id)[1].EarnedCredit;
                let ecp22 = dataList.filter(a => a.StudentID == id)[3].EarnedCredit;
                let ecp32 = dataList.filter(a => a.StudentID == id)[5].EarnedCredit;
                let ecp42 = dataList.filter(a => a.StudentID == id)[7].EarnedCredit;

                let tc11 = dataList.filter(a => a.StudentID == id)[0].TakenCredit;
                let tc21 = dataList.filter(a => a.StudentID == id)[2].TakenCredit;
                let tc31 = dataList.filter(a => a.StudentID == id)[4].TakenCredit;
                let tc41 = dataList.filter(a => a.StudentID == id)[6].TakenCredit;

                let tc12 = dataList.filter(a => a.StudentID == id)[1].TakenCredit;
                let tc22 = dataList.filter(a => a.StudentID == id)[3].TakenCredit;
                let tc32 = dataList.filter(a => a.StudentID == id)[5].TakenCredit;
                let tc42 = dataList.filter(a => a.StudentID == id)[7].TakenCredit;

                let sgpa11 = dataList.filter(a => a.StudentID == id)[0].TranscriptGPA;
                let sgpa21 = dataList.filter(a => a.StudentID == id)[2].TranscriptGPA;
                let sgpa31 = dataList.filter(a => a.StudentID == id)[4].TranscriptGPA;
                let sgpa41 = dataList.filter(a => a.StudentID == id)[6].TranscriptGPA;

                let sgpa12 = dataList.filter(a => a.StudentID == id)[1].TranscriptGPA;
                let sgpa22 = dataList.filter(a => a.StudentID == id)[3].TranscriptGPA;
                let sgpa32 = dataList.filter(a => a.StudentID == id)[5].TranscriptGPA;
                let sgpa42 = dataList.filter(a => a.StudentID == id)[7].TranscriptGPA;

                totalCredit = tc11 + tc12 + tc21 + tc22 + tc31 + tc32 + tc41 + tc42;

                typeElement += '<tr>'
                                + '<td>' + roll + '</td>'
                                + '<td>' + reg + '</td>'
                                + '<td style="white-space:nowrap">' + name + '</td>'
                                + '<td style="white-space:nowrap">' + father + '</td>'
                                + '<td>' + ecp11 + '</td>'
                                + '<td>' + tc11 + '</td>'
                                + '<td>' + sgpa11 + '</td>'
                                + '<td>' + ecp12 + '</td>'
                                + '<td>' + tc12 + '</td>'
                                + '<td>' + sgpa12 + '</td>'
                                + '<td>' + ecp21 + '</td>'
                                + '<td>' + tc21 + '</td>'
                                + '<td>' + sgpa21 + '</td>'
                                + '<td>' + ecp22 + '</td>'
                                + '<td>' + tc22 + '</td>'
                                + '<td>' + sgpa22 + '</td>'
                                + '<td>' + ecp31 + '</td>'
                                + '<td>' + tc31 + '</td>'
                                + '<td>' + sgpa31 + '</td>'
                                + '<td>' + ecp32 + '</td>'
                                + '<td>' + tc32 + '</td>'
                                + '<td>' + sgpa32 + '</td>'
                                + '<td>' + ecp41 + '</td>'
                                + '<td>' + tc41 + '</td>'
                                + '<td>' + sgpa41 + '</td>'
                                + '<td>' + ecp42 + '</td>'
                                + '<td>' + tc42 + '</td>'
                                + '<td>' + sgpa42 + '</td>'
                                + '<td>' + totalCredit + '</td>'
                             + '</tr>'

            })
                         + '</table>'

            $("#divProgress").hide()


            $("#table").append(typeElement);
        },
        error: function (r) {

        }
    })
}

function printFunction() {
    event.preventDefault();
    var departMent = $("#ctl00_MainContainer_ucDepartment_ddlDepartment option:selected").text();
    var element = $("#table").html();

    var typeElement = "";
    typeElement += '<!DOCTYPE html><html>';
    typeElement += '<head><title>Final Tabulation CGPA wise</title><link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/css/bootstrap.min.css"><script src="https://cdn.jsdelivr.net/npm/jquery@3.6.1/dist/jquery.slim.min.js"></script><script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.1/dist/umd/popper.min.js"></script><script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/js/bootstrap.bundle.min.js"></script><style>body{margin-top:30px}.academic{border-collapse: collapse;} .academic th,.academic td{border:1px solid black;white-space:nowrap;padding:10px;font-size:20px}@media print{@page{size:landscape}}</style></head>';
    typeElement += '<body >'
    typeElement += '<div class="container-fluid">'
    typeElement += '<div class="d-flex justify-content-center">'
    typeElement += '<div class="p-2" style="width: 10%;">'
    typeElement += '<img src="../../../Images/PABNA_logo.png" style="width: 50%;"/>'
    typeElement += '</div>'
    typeElement += '<div class="p-2 text-center">'
    typeElement += '<h3>Pabna University of Science and Technology</h3>'
    typeElement += '<h4>' + departMent + '</h4>'
    typeElement += '<h5>Final Result sheet</h5>'
    typeElement += '<h5>Session</h5>'
    typeElement += '</div>'
    typeElement += '</div>'
    typeElement += '<br/>'
    typeElement += element
    typeElement += '</div>'
    typeElement += '</body>'
    typeElement += '</html>'

    var mywindow = window.open("", "PRINT", 1600, 2400);
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