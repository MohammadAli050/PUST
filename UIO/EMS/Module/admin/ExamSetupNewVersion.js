var RoleId;

$(document).ready(function () {


    $("#department").change(function () {
        $("#tablebody").children().remove();
        $("#newtable").hide();
        $("#addnewcommittee").hide();
    });

    $("#program").change(function () {
        $("#tablebody").children().remove();
        $("#newtable").hide();
        $("#addnewcommittee").hide();
    })

    $("#year").change(function () {
        $("#tablebody").children().remove();
        $("#newtable").hide();
        $("#addnewcommittee").hide();
    })

    $("#semester").change(function () {
        $("#tablebody").children().remove();
        $("#newtable").hide();
        $("#addnewcommittee").hide();
    })

    $("#exam").change(function () {
        $("#tablebody").children().remove();
        $("#newtable").hide();
        $("#addnewcommittee").hide();
    })

    $("#session").change(function () {
        $("#tablebody").children().remove();
        $("#newtable").hide();
        $("#addnewcommittee").hide();
    })

    var RoleId = 1;
    var dsp = "block"
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "ExamSetupNewVersion.aspx/GetPageUserID",
        data: "{'abc':'" + Number(0) + "' }",
        dataType: "json",
        success: function (data) {
            var parsed = JSON.parse(data.d);
            RoleId = parsed;

            showhideUser(RoleId);
        },
        error: function (data) {
            alert("error")
        }
    })

    $('.select2').select2();

    $("#hdnRelationID").val(0);
    $("#hdnSetupID").val(0);
    $("#hdnID").val(0);
    $("#hdnbtn").val(0);


    //Deparment list api call
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "ExamSetupNewVersion.aspx/GetDepartmentList",
        data: "{'abc':'" + Number(0) + "' }",
        dataType: "json",
        success: function (data) {
            var parsed = JSON.parse(data.d)
            for (i = 0; i < parsed.length; i++) {
                $("#department").append($("<option></option>").val(parsed[i].DeptID).html(parsed[i].DetailedName));
                $("#department5").append($("<option></option>").val(parsed[i].DeptID).html(parsed[i].DetailedName));
                $("#dept1").append($("<option></option>").val(parsed[i].DeptID).html(parsed[i].DetailedName));
                $("#dept2").append($("<option></option>").val(parsed[i].DeptID).html(parsed[i].DetailedName));
                $("#dept3").append($("<option></option>").val(parsed[i].DeptID).html(parsed[i].DetailedName));
            }
        }

    });


    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "ExamSetupNewVersion.aspx/GetExternalDept",
        data: "{'abc':'" + Number(0) + "' }",
        dataType: "json",
        success: function (data) {
            var parsed = JSON.parse(data.d)
            //console.log(parsed)
            for (i = 0; i < parsed.length; i++) {
                $("#dept4").append($("<option></option>").val(parsed[i]).html(parsed[i]));
            }
        },
        error: function (r) {
            console.log(r.d)
        }
    })

    //External Call ID
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "ExamSetupNewVersion.aspx/GetExternalFacultyByDeptId",
        data: "{'Dept':'" + Number(0) + "' }",
        dataType: "json",
        success: function (data) {

            var parsed = JSON.parse(data.d);
            //console.log(parsed)            
            for (i = 0; i < parsed.length; i++) {
                $("#extmbm").append('<option value="0" selected="selected">Select</option>')
                $("#extmbm").append($("<option></option>").val(parsed[i].ExternalId).html(parsed[i].Name));

            }
        }

    });


    //Year list api call
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "ExamSetupNewVersion.aspx/GetYearList",
        data: "{'abc':'" + Number(0) + "' }",
        dataType: "json",
        success: function (data) {
            var parsed = JSON.parse(data.d)
            for (i = 0; i < parsed.length; i++) {
                $("#year").append($("<option></option>").val(parsed[i].YearNo).html(parsed[i].YearNoName));
                $("#Year").append($("<option></option>").val(parsed[i].YearNo).html(parsed[i].YearNoName));
            }
        }

    });


    //Semester list api call
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "ExamSetupNewVersion.aspx/GetSemesterList",
        data: "{'abc':'" + Number(0) + "' }",
        dataType: "json",
        success: function (data) {
            var parsed = JSON.parse(data.d)
            for (i = 0; i < parsed.length; i++) {
                $("#semester").append($("<option></option>").val(parsed[i].SemesterNo).html(parsed[i].SemesterNoName));
                $("#sem").append($("<option></option>").val(parsed[i].SemesterNo).html(parsed[i].SemesterNoName));
            }
        }

    });


    //Session list api call
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "ExamSetupNewVersion.aspx/GetSessionList",
        data: "{'abc':'" + Number(0) + "' }",
        dataType: "json",
        success: function (data) {
            var parsed = JSON.parse(data.d)
            for (i = 0; i < parsed.length; i++) {
                $("#session").append($("<option></option>").val(parsed[i].AcademicCalenderID).html(parsed[i].Code));
                $("#session1").append($("<option></option>").val(parsed[i].AcademicCalenderID).html(parsed[i].Code));
            }
        }

    });

    //Exam Year list api call
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "ExamSetupNewVersion.aspx/GetYearShal",
        data: "{'abc':'" + Number(0) + "' }",
        dataType: "json",
        success: function (data) {
            var parsed = JSON.parse(data.d)
            //console.log(parsed)
            for (i = 0; i < parsed.length; i++) {
                $("#exam").append($("<option></option>").val(parsed[i].Year).html(parsed[i].Year));
                $("#examyear").append($("<option></option>").val(parsed[i].Year).html(parsed[i].Year));
            }
        }

    });


    // Button Add New Click

    $("#addnew").click(function () {

        $("#prog").children().remove();
        $("#examname").children().remove();

        $('#department5').prop('selectedIndex', $("#department").val());

        deptPopChangeFunction($("#department").val(), $("#program").val())

        if ($('#year').val() == "0") {
            $('#Year').val('selectedIndex', 0);
        }
        else {
            $('#Year').val($('#year').val());
        }

        if ($('#semester').val() == "0") {
            $('#sem').val('selectedIndex', 0);
        }
        else {
            $('#sem').val($('#year').val());
        }

        if ($('#exam').val() == "0") {
            $('#examyear').val('selectedIndex', 0);
        }
        else {
            $('#examyear').val($('#exam').val());
        }

        if ($('#exam').val() == "0") {
            $('#examyear').val('selectedIndex', 0);
        }
        else {
            $('#examyear').val($('#exam').val());
        }

        if ($('#session').val() == "0") {
            $('#session1').val('selectedIndex', 0);
        }
        else {
            $('#session1').val($('#session').val());
        }



        $('#prog').prop('selectedIndex', 0);
        $('#examname').prop('selectedIndex', 0);


        $('#prog').append('<option value="0" selected="selected">Select</option>');
        $('#examname').append('<option value="0" selected="selected">Select</option>');

        $("#isactive").prop('checked', false)
        $("#hdnID").val(0);

    })


    // Add Committee Button Click

    $("#addnewcommittee").click(function () {

        $("#chairman").children().remove();
        $("#mbmone").children().remove();
        $("#mbmtwo").children().remove();
        $("#extmbm").children().remove();

        if ($("#department").val() == "0") {
            $('#dept1').prop('selectedIndex', 0);
            $('#dept2').prop('selectedIndex', 0);
            $('#dept3').prop('selectedIndex', 0);
            $('#dept4').prop('selectedIndex', 0);
            $('#chairman').prop('selectedIndex', 0);
            $('#mbmone').prop('selectedIndex', 0);
            $('#mbmtwo').prop('selectedIndex', 0);
            $('#extmbm').prop('selectedIndex', 0);
        }

        else {
            $('#dept1').val($("#department").val());
            dept1function($("#department").val(), 0)
            $('#dept2').val($("#department").val());
            dept2function($("#department").val(), 0)

            $('#dept3').val($("#department").val());
            dept3function($("#department").val(), 0)

        }


        $("#hdnbtn").val(1);

        $('#chairman').append('<option value="0" selected="selected">Select</option>');
        $('#mbmone').append('<option value="0" selected="selected">Select</option>');
        $('#mbmtwo').append('<option value="0" selected="selected">Select</option>');
        $('#extmbm').append('<option value="0" selected="selected">Select</option>');

    })


    //load button click function
    $("#loadbtn").click(function (e) {
        e.preventDefault();
        $("#tablebody").children().remove();

        var showDsp = showhideUser(RoleId);
        if (RoleId == 11) {
            $("#addnewcommittee").show();
            $("#addnew").hide();
        }
        else if (RoleId == 8) {
            $("#addnewcommittee").hide();
            $("#addnew").show();
        }
        else {
            $("#addnewcommittee").show();
            $("#addnew").show();
        }

        var dept = $("#department option:selected").val()
        var year = $("#year option:selected").val()
        var prog = $("#program option:selected").val()

        var sem = $("#semester option:selected").val()
        var shal = $("#exam option:selected").val()
        var session = $("#session option:selected").val();




        //backend load code
        examTable(prog, year, sem, shal, session, showDsp)
        $("#all").prop("checked", false);
    })

    //add new save modal 
    $("#savemodal1").click(function () {

        var progid = $("#prog option:selected").val();
        var yearid = $("#Year option:selected").val();
        var semno = $("#sem option:selected").val();
        //var examstart = $("#startdate").val()
        //var examend = $("#enddate").val()
        var heldin = $("#examname").val();
        var Id = $("#hdnID").val();

        if ($('#isactive').is(":checked")) {
            var check = true;
        } else {
            var check = false
        }

        let SendList = [];

        if (progid == 0 || yearid == 0 || semno == 0) {
            swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Not All fields are selected',
                footer: 'Select All Fields'
            })
            return;
        }


        if (heldin != '0') {

            var newList = {
                "Id": Id,
                "ExamHeldInId": heldin,
                "YearNo": yearid,
                "SemesterNo": semno,
                "ProgramId": progid,
                "Remarks": "Remark",
                "IsActive": check,
                "IsDeleted": false,
                "Attribute1": null,
                "Attribute2": null,
                "Attribute3": "Attribute",
                "Attribute4": "Attribute",
                "Attribute5": "Attribute",
                "CreatedBy": null,
                "CreatedDate": null,
                "ModifiedBy": null,
                "ModifiedDate": null,
            }

            SendList.push(newList);

            var listString = JSON.stringify(SendList);

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "ExamSetupNewVersion.aspx/SaveUpdateHeldInAndProgramRelation",
                data: "{'receiveListString':'" + listString + "'}",
                dataType: "json",
                success: function (data) {
                    $("#prog").children().remove();
                    $("#examname").children().remove();
                    $('#department5').prop('selectedIndex', 0);
                    $('#Year').prop('selectedIndex', 0);
                    $('#sem').prop('selectedIndex', 0);
                    $('#examyear').prop('selectedIndex', 0);
                    $('#session1').prop('selectedIndex', 0);
                    $('#myModal1').modal('toggle');

                    $('#prog').append('<option value="0" selected="selected">Select</option>');
                    $('#examname').append('<option value="0" selected="selected">Select</option>');

                    if (data != '') {
                        Swal.fire({
                            icon: 'success',
                            title: 'Success',
                            text: 'New Exam Added'
                        }).then((result) => {
                            $("#tablebody").children().remove();


                            var dept = $("#department option:selected").val()
                            var year = $("#year option:selected").val()
                            var prog = $("#program option:selected").val()

                            var sem = $("#semester option:selected").val()
                            var shal = $("#exam option:selected").val()
                            var session = $("#session option:selected").val();


                            //backend load code
                            var showDsp = showhideUser(RoleId);
                            examTable(prog, year, sem, shal, session, showDsp);
                            $("#myModal1").modal('toggle');
                            $('.modal-backdrop').remove();
                        })
                    }

                },
                error: function (r) {
                    console.log("error");
                }
            })
        }
        else {
            swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Held In not Selected',
                footer: 'Choose Held In'
            })
            return;
        }
        showhideUser(RoleId)
    })

    //add committee bulk save
    $("#savemodal").click(function () {
        var deptid1 = $("#dept1 option:selected").val();
        var deptid2 = $("#dept2 option:selected").val();
        var deptid3 = $("#dept3 option:selected").val();
        var deptid4 = $("#dept4 option:selected").val();
        var chairman = $("#chairman option:selected").val();
        var mbmone = $("#mbmone option:selected").val();
        var mbmtwo = $("#mbmtwo option:selected").val();
        var extmbm = $("#extmbm option:selected").val();

        var relId = $("#hdnRelationID").val();
        var SetId = $("#hdnSetupID").val();
        var btnId = $("#hdnbtn").val();
        var tablerow = $("#newtable tr").length
        var relationid;
        var listString = '';

        let SendList = [];
        if (btnId == 1) {
            for (i = 0; i < tablerow; i++) {
                if ($('#checkBox' + i).is(":checked")) {

                    relationid = parseInt($('#checkBox' + i).val())
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "ExamSetupNewVersion.aspx/GetExamCommitteeByRelationId",
                        data: "{'RelationId':'" + relationid + "' }",
                        dataType: "json",
                        success: function (data) {
                            if (data.d != null) {
                                var parsed = JSON.parse(data.d);
                                $("#hdnRelationID").val(parsed.HeldInProgramRelationId);
                                $("#hdnSetupID").val(parsed.ID);

                                var newList = {
                                    "Id": parsed.ID,
                                    "HeldInProgramRelationId": parsed.HeldInProgramRelationId,
                                    "ExamCommitteeChairmanDeptId": deptid1,
                                    "ExamCommitteeChairmanId": chairman,
                                    "ExamCommitteeMemberOneDeptId": deptid2,
                                    "ExamCommitteeMemberOneId": mbmone,
                                    "ExamCommitteeMemberTwoDeptId": deptid3,
                                    "ExamCommitteeMemberTwoId": mbmtwo,
                                    "ExamCommitteeExternalMemberDeptId": 0,
                                    "ExamCommitteeExternalMemberId": extmbm,
                                    "IsActive": null,
                                    "Attribute1": "Attribute",
                                    "Attribute2": "Attribute",
                                    "Attribute3": "Attribute",
                                    "CreatedBy": null,
                                    "CreatedDate": null,
                                    "ModifiedBy": null,
                                    "ModifiedDate": null,

                                }

                                SendList.push(newList);

                                listString = JSON.stringify(SendList);

                                $.ajax({
                                    type: "POST",
                                    contentType: "application/json; charset=utf-8",
                                    url: "ExamSetupNewVersion.aspx/SaveUpdateExamCommittees",
                                    data: "{'receiveListString':'" + listString + "'}",
                                    dataType: "json",
                                    success: function (data) {

                                        $('#dept1').prop('selectedIndex', 0);
                                        $('#dept2').prop('selectedIndex', 0);
                                        $('#dept3').prop('selectedIndex', 0);
                                        $('#dept4').prop('selectedIndex', 0);
                                        $('#chairman').prop('selectedIndex', 0);
                                        $('#mbmone').prop('selectedIndex', 0);
                                        $('#mbmtwo').prop('selectedIndex', 0);
                                        $('#extmbm').prop('selectedIndex', 0);

                                        $("#tablebody").children().remove();

                                        var dept = $("#department option:selected").val()
                                        var year = $("#year option:selected").val()
                                        var prog = $("#program option:selected").val()

                                        var sem = $("#semester option:selected").val()
                                        var shal = $("#exam option:selected").val()
                                        var session = $("#session option:selected").val();


                                        //backend load code
                                        var showDsp = showhideUser(RoleId);
                                        examTable(prog, year, sem, shal, session, showDsp)

                                    },
                                    error: function (r) {
                                        console.log("error");
                                    }
                                })

                                SendList = [];

                            }
                            else {
                                $("#hdnRelationID").val(relationid);

                                var newList = {
                                    "Id": 0,
                                    "HeldInProgramRelationId": relationid,
                                    "ExamCommitteeChairmanDeptId": deptid1,
                                    "ExamCommitteeChairmanId": chairman,
                                    "ExamCommitteeMemberOneDeptId": deptid2,
                                    "ExamCommitteeMemberOneId": mbmone,
                                    "ExamCommitteeMemberTwoDeptId": deptid3,
                                    "ExamCommitteeMemberTwoId": mbmtwo,
                                    "ExamCommitteeExternalMemberDeptId": 0,
                                    "ExamCommitteeExternalMemberId": extmbm,
                                    "IsActive": null,
                                    "Attribute1": "Attribute",
                                    "Attribute2": "Attribute",
                                    "Attribute3": "Attribute",
                                    "CreatedBy": null,
                                    "CreatedDate": null,
                                    "ModifiedBy": null,
                                    "ModifiedDate": null,

                                }

                                SendList.push(newList);

                                listString = JSON.stringify(SendList);

                                $.ajax({
                                    type: "POST",
                                    contentType: "application/json; charset=utf-8",
                                    url: "ExamSetupNewVersion.aspx/SaveUpdateExamCommittees",
                                    data: "{'receiveListString':'" + listString + "'}",
                                    dataType: "json",
                                    success: function (data) {

                                        $('#dept1').prop('selectedIndex', 0);
                                        $('#dept2').prop('selectedIndex', 0);
                                        $('#dept3').prop('selectedIndex', 0);
                                        $('#dept4').prop('selectedIndex', 0);
                                        $('#chairman').prop('selectedIndex', 0);
                                        $('#mbmone').prop('selectedIndex', 0);
                                        $('#mbmtwo').prop('selectedIndex', 0);
                                        $('#extmbm').prop('selectedIndex', 0);

                                        $("#tablebody").children().remove();

                                        var dept = $("#department option:selected").val()
                                        var year = $("#year option:selected").val()
                                        var prog = $("#program option:selected").val()

                                        var sem = $("#semester option:selected").val()
                                        var shal = $("#exam option:selected").val()
                                        var session = $("#session option:selected").val();


                                        //backend load code
                                        var showDsp = showhideUser(RoleId);
                                        examTable(prog, year, sem, shal, session, showDsp)
                                    },
                                    error: function (r) {
                                        console.log("error");
                                    }
                                })

                                SendList = [];


                            }

                        }
                    })
                    Swal.fire({
                        icon: 'success',
                        title: 'Success',
                        text: 'Committee Edited'
                    }).then((result) => {
                        if (result.isConfirmed) {
                            $('#myModal').modal('toggle');
                            $('.modal-backdrop').remove();
                        }
                    })
                }
            }

        }
        else {
            var newList = {
                "Id": SetId,
                "HeldInProgramRelationId": relId,
                "ExamCommitteeChairmanDeptId": deptid1,
                "ExamCommitteeChairmanId": chairman,
                "ExamCommitteeMemberOneDeptId": deptid2,
                "ExamCommitteeMemberOneId": mbmone,
                "ExamCommitteeMemberTwoDeptId": deptid3,
                "ExamCommitteeMemberTwoId": mbmtwo,
                "ExamCommitteeExternalMemberDeptId": 0,
                "ExamCommitteeExternalMemberId": extmbm,
                "IsActive": null,
                "Attribute1": "Attribute",
                "Attribute2": "Attribute",
                "Attribute3": "Attribute",
                "CreatedBy": null,
                "CreatedDate": null,
                "ModifiedBy": null,
                "ModifiedDate": null,

            }
            SendList.push(newList);

            var listString = JSON.stringify(SendList);

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "ExamSetupNewVersion.aspx/SaveUpdateExamCommittees",
                data: "{'receiveListString':'" + listString + "'}",
                dataType: "json",
                success: function (data) {

                    $('#dept1').prop('selectedIndex', 0);
                    $('#dept2').prop('selectedIndex', 0);
                    $('#dept3').prop('selectedIndex', 0);
                    $('#dept4').prop('selectedIndex', 0);
                    $('#chairman').prop('selectedIndex', 0);
                    $('#mbmone').prop('selectedIndex', 0);
                    $('#mbmtwo').prop('selectedIndex', 0);
                    $('#extmbm').prop('selectedIndex', 0);

                    if (data != '') {
                        Swal.fire({
                            icon: 'success',
                            title: 'Success',
                            text: 'Committee Edited'
                        }).then((result) => {
                            if (result.isConfirmed) {
                                $('#myModal').modal('toggle');
                                $('.modal-backdrop').remove();
                            }
                        })
                    }

                    $("#tablebody").children().remove();


                    var dept = $("#department option:selected").val()
                    var year = $("#year option:selected").val()
                    var prog = $("#program option:selected").val()

                    var sem = $("#semester option:selected").val()
                    var shal = $("#exam option:selected").val()
                    var session = $("#session option:selected").val();


                    //backend load code
                    var showDsp = showhideUser(RoleId);
                    examTable(prog, year, sem, shal, session, showDsp)
                },
                error: function (r) {
                    console.log("error");
                }
            })
        }
        $("#all").prop("checked", false);

        showhideUser(RoleId)
    })



    $("#all").click(function () {
        $('input:checkbox').not(this).prop('checked', this.checked);
    });

})

//main page dept change
function deptChangeFunction(val, RoleId) {
    $('#program').children().remove();
    var RoleID = RoleId.value
    var deptVal = val;
    if (RoleID == "11") {
        if (val == '0') {
            document.getElementById('program').disabled = true;
            $("#loadbtn").prop("disabled", true)
        } else {
            document.getElementById('program').disabled = false;
            $("#loadbtn").prop("disabled", false)
        }
    }
    else {
        if (this.value == '0') {
            document.getElementById('program').disabled = true;
        } else {
            document.getElementById('program').disabled = false;
        }
    }

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "ExamSetupNewVersion.aspx/GetProgramList",
        data: "{'DepartmentId':'" + parseInt(val) + "' }",
        dataType: "json",
        success: function (data) {
            var parsed = JSON.parse(data.d)
            //$("#program").append('<option value="0" selected="selected">Select</option>')
            for (i = 0; i < parsed.length; i++) {
                $("#program").append($("<option></option>").val(parsed[i].ProgramID).html(parsed[i].ShortName));
            }
        }

    });

}


function ExamYearChange(val) {
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "ExamSetupNewVersion.aspx/GetSessionList",
        data: "{'abc':'" + Number(0) + "' }",
        dataType: "json",
        success: function (data) {
            var parsed = JSON.parse(data.d)
            if (val > 0) {
                val = val - 1;
                parsed = parsed.filter(x => x.Year == val);
            }

            $("#session").html("");

            for (i = 0; i < parsed.length; i++) {
                $("#session").append($("<option></option>").val(parsed[i].AcademicCalenderID).html(parsed[i].Code));
                $("#session1").append($("<option></option>").val(parsed[i].AcademicCalenderID).html(parsed[i].Code));
            }
        }

    });
}


//add new dept change
function deptPopChangeFunction(val, val1) {
    $('#prog').children().remove();
    var deptVal = val;
    if (this.value == '0') {
        document.getElementById('program').disabled = true;
    } else {
        document.getElementById('program').disabled = false;
    }

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "ExamSetupNewVersion.aspx/GetProgramList",
        data: "{'DepartmentId':'" + parseInt(val) + "' }",
        dataType: "json",
        success: function (data) {
            var parsed = JSON.parse(data.d)
            //if (val1 == 0) {
            //    $("#prog").append('<option value="0" selected="selected">Select</option>')
            //}
            for (i = 0; i < parsed.length; i++) {
                if (parsed[i].ProgramID == val1) {
                    $("#prog").append($("<option selected></option>").val(parsed[i].ProgramID).html(parsed[i].ShortName));
                }
                else {
                    $("#prog").append($("<option></option>").val(parsed[i].ProgramID).html(parsed[i].ShortName));
                }
            }

        }

    });

}


//pop up modal committee bulk dropdown
function dept1function(val, val1) {
    $('#chairman').children().remove();
    var deptVal = val;
    if (val != 0) {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "ExamSetupNewVersion.aspx/GetFacultyByDeptId",
            data: "{'DeptId':'" + parseInt(val) + "' }",
            dataType: "json",
            success: function (data) {
                var parsed = JSON.parse(data.d);
                $("#chairman").empty()
                if (val1 == 0) {
                    $("#chairman").append('<option value="0" selected="selected">Select</option>')
                }
                for (i = 0; i < parsed.length; i++) {
                    if (parsed[i].EmployeeId == val1) {
                        $("#chairman").append($("<option selected></option>").val(parsed[i].EmployeeId).html(parsed[i].CodeAndName));
                    }
                    else {
                        $("#chairman").append($("<option></option>").val(parsed[i].EmployeeId).html(parsed[i].CodeAndName));
                    }
                }
                $('.select2').select2();
            }

        });
    }

}

function dept2function(val, val1) {
    $('#mbmone').children().remove();
    var deptVal = val;

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "ExamSetupNewVersion.aspx/GetFacultyByDeptId",
        data: "{'DeptId':'" + parseInt(val) + "' }",
        dataType: "json",
        success: function (data) {
            var parsed = JSON.parse(data.d);
            $("#mbmone").empty();
            if (val1 == 0) {
                $("#mbmone").append('<option value="0" selected="selected">Select</option>')
            }
            for (i = 0; i < parsed.length; i++) {
                if (parsed[i].EmployeeId == val1) {
                    $("#mbmone").append($("<option selected></option>").val(parsed[i].EmployeeId).html(parsed[i].CodeAndName));
                }
                else {
                    $("#mbmone").append($("<option></option>").val(parsed[i].EmployeeId).html(parsed[i].CodeAndName));
                }
            }
            $('.select2').select2();

        }

    });
}

function dept3function(val, val1) {
    $('#mbmtwo').children().remove();
    var deptVal = val;

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "ExamSetupNewVersion.aspx/GetFacultyByDeptId",
        data: "{'DeptId':'" + parseInt(val) + "' }",
        dataType: "json",
        success: function (data) {
            var parsed = JSON.parse(data.d)
            $("#mbmtwo").empty();
            if (val1 == 0) {
                $("#mbmtwo").append('<option value="0" selected="selected">Select</option>')
            }
            for (i = 0; i < parsed.length; i++) {

                if (parsed[i].EmployeeId == val1) {
                    $("#mbmtwo").append($("<option selected></option>").val(parsed[i].EmployeeId).html(parsed[i].CodeAndName));
                }
                else {
                    $("#mbmtwo").append($("<option></option>").val(parsed[i].EmployeeId).html(parsed[i].CodeAndName));
                }

            }
            $('.select2').select2();

        }

    });
}

function dept4function(val, val1) {
    $('#extmbm').children().remove();
    if (val == 0) {
        $('#extmbm').prop('disabled', true);
    }
    else {
        $('#extmbm').prop('disabled', false);
    }
    var deptVal = val;
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "ExamSetupNewVersion.aspx/GetExternalFacultyByDeptId",
        data: "{'Dept':'" + val + "' }",
        dataType: "json",
        success: function (data) {
            var parsed = JSON.parse(data.d);
            $("#extmbm").empty();
            if (val1 == 0) {
                $("#extmbm").append('<option value="0" selected="selected">Select</option>')
            }
            for (i = 0; i < parsed.length; i++) {
                if (parsed[i].ExternalId == val1) {
                    $("#extmbm").append($("<option selected></option>").val(parsed[i].ExternalId).html(parsed[i].Name));
                }
                else {
                    $("#extmbm").append($("<option></option>").val(parsed[i].ExternalId).html(parsed[i].Name));
                }
            }
            $('.select2').select2();

        }

    });
}


function changeFunction(val, val2) {
    var AcacalId = $("#session1 option:selected").val();
    var Year = $("#examyear option:selected").val();
    if (Year != '0' && AcacalId != '0') {
        $("#examname").children().remove();
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "ExamSetupNewVersion.aspx/GetHeldInList",
            data: "{'AcacalId':'" + parseInt(AcacalId) + "','Year':'" + Year + "'}",
            dataType: "json",
            success: function (data) {
                $('#examname').children().remove();
                $('#examname').append('<option value="0" selected="selected">Select</option>');
                var parsed = JSON.parse(data.d);
                for (i = 0; i < parsed.length; i++) {
                    if (val2 == parsed[i].Id) {
                        $("#examname").append($("<option selected></option>").val(parsed[i].Id).html(parsed[i].ExamName));
                    }
                    else {
                        $("#examname").append($("<option></option>").val(parsed[i].Id).html(parsed[i].ExamName));
                    }

                }
            },
            error: function (r) {
                $('#examname').children().remove();
                $('#examname').append('<option value="0" selected="selected">Select</option>');
                console.log("error");
            }
        })
    }
}


function editcom(val) {

    $("#hdnbtn").val(0);

    var editcomSuccess = false;

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "ExamSetupNewVersion.aspx/GetExamCommitteeByRelationId",
        data: "{'RelationId':'" + val.RelationId + "' }",
        dataType: "json",
        success: function (data) {
            if (data.d != null) {
                var parsed = JSON.parse(data.d);
                if (val.ChairmanName == "") {
                    deptval = $("#department").val();
                    $("#dept1").val($("#department").val());
                    dept1function(deptval, 0)
                }
                else {
                    $("#dept1").val(parsed.ExamCommitteeChairmanDeptId);
                    dept1function(parsed.ExamCommitteeChairmanDeptId, parsed.ExamCommitteeChairmanId);
                }
                if (val.MemberOneName == "") {
                    $("#dept2").val($("#department").val());
                }
                else {
                    $("#dept2").val(parsed.ExamCommitteeMemberOneDeptId);
                    dept2function(parsed.ExamCommitteeMemberOneDeptId, parsed.ExamCommitteeMemberOneId);

                }
                if (val.MemberTwoName == "") {
                    $("#dept3").val($("#department").val());
                }
                else {
                    $("#dept3").val(parsed.ExamCommitteeMemberTwoDeptId);
                    dept3function(parsed.ExamCommitteeMemberTwoDeptId, parsed.ExamCommitteeMemberTwoId);

                }
                if (val.ExternalName == "") {
                    dept4function(0, 0);
                }
                else {
                    $("#extmbm").val(parsed.ExamCommitteeExternalMemberId);
                    externalmember(parsed.ExamCommitteeExternalMemberId);
                }
                //dept4function(parsed.ExamCommitteeChairmanDeptId, parsed.ExamCommitteeExternalMemberId);
                editcomSuccess = true;

                $("#hdnRelationID").val(parsed.HeldInProgramRelationId);
                $("#hdnSetupID").val(parsed.ID);


            }
            else {
                $("#hdnRelationID").val(val.RelationId);
                $("#hdnSetupID").val(0);
                editcomSuccess = false;
            }

        }
    }).then(function () {
        if (editcomSuccess == false) {

            $("#dept1").val($("#department").val());
            dept1function($("#department").val(), 0)
            $("#dept2").val($("#department").val());
            dept2function($("#department").val(), 0)
            $("#dept3").val($("#department").val());
            dept3function($("#department").val(), 0);
            dept4function(0, 0);
        }
    })
}


//edit exam 
function editexam(val, val1, val2) {
    var dept = $("#department option:selected").val()
    var year = $("#year option:selected").val()
    var prog = $("#program option:selected").val()

    var sem = $("#semester option:selected").val()
    var shal = $("#exam option:selected").val()
    var session = $("#session option:selected").val();
    var exmSession = 0;

    $("#prog").children().remove();
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "ExamSetupNewVersion.aspx/GetExamByRelationId",
        data: "{'RelationId':'" + val + "' }",
        dataType: "json",
        success: function (data) {
            var parsed = JSON.parse(data.d);
            $("#hdnID").val(parsed.Id);
            deptid = $("#department").val()
            deptPopChangeFunction(deptid, parsed.ProgramId);
            $("#department5").val(deptid);
            $("#Year").val(parsed.YearNo);
            $("#sem").val(parsed.SemesterNo);
            $("#examyear").val(val1)
            $("#session1 option").each(function () {
                if ($(this).text() === val2) {
                    exmSession = $(this).val();
                    return false;
                }
            })
            $("#session1").val(exmSession)
            changeFunction(val1)


            if (parsed.IsActive == true) {
                $("#isactive").prop('checked', true)
            }
            else {
                $("#isactive").prop('checked', false)
            }

            var showDsp = showhideUser(RoleId);
            examTable(prog, year, sem, shal, session, showDsp)

        }
    })
}

function deleteFunction(val) {
    event.preventDefault();
    var relID = val

    Swal.fire({
        title: 'Do you want to save the changes?',
        showCancelButton: true,
        confirmButtonText: 'Delete',
    }).then((result) => {
        /* Read more about isConfirmed, isDenied below */
        if (result.isConfirmed) {

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "ExamSetupNewVersion.aspx/GetExamByRelationId",
                data: "{'RelationId':'" + val + "' }",
                dataType: "json",
                success: function (data) {
                    var parsed = JSON.parse(data.d);
                    let SendList = [];
                    var newList = {
                        "Id": parsed.Id,
                        "ExamHeldInId": parsed.ExamHeldInId,
                        "YearNo": parsed.YearNo,
                        "SemesterNo": parsed.SemesterNo,
                        "ProgramId": parsed.ProgramId,
                        "Remarks": "Remark",
                        "IsActive": false,
                        "IsDeleted": true,
                        "Attribute1": null,
                        "Attribute2": null,
                        "Attribute3": "Attribute",
                        "Attribute4": "Attribute",
                        "Attribute5": "Attribute",
                        "CreatedBy": parsed.CreatedBy,
                        "CreatedDate": parsed.CreatedDate,
                        "ModifiedBy": null,
                        "ModifiedDate": null,
                    }

                    SendList.push(newList);

                    var listString = JSON.stringify(SendList);

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "ExamSetupNewVersion.aspx/SaveUpdateHeldInAndProgramRelation",
                        data: "{'receiveListString':'" + listString + "'}",
                        dataType: "json",
                        success: function (data) {


                            Swal.fire(
                                'Deleted!',
                                'Exam is deleted',
                                'success'
                           ).then((result) => {
                               if (result.isConfirmed) {
                                   var dept = $("#department option:selected").val()
                                   var year = $("#year option:selected").val()
                                   var prog = $("#program option:selected").val()

                                   var sem = $("#semester option:selected").val()
                                   var shal = $("#exam option:selected").val()
                                   var session = $("#session option:selected").val();
                                   var showDsp = showhideUser(RoleId);

                                   //backend load code
                                   var showDsp = showhideUser(RoleId);
                                   examTable(prog, year, sem, shal, session, showDsp)
                               }
                           })

                        },
                        error: function (r) {
                            console.log("error");
                        }
                    })

                }
            })
        }
    })

}

function externalmember(val) {
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "ExamSetupNewVersion.aspx/GetExternalFacultyById",
        data: "{'ExternalId':'" + parseInt(val) + "' }",
        dataType: "json",
        success: function (data) {
            var parsed = JSON.parse(data.d)
            $("#dept4").val(parsed.Department);
            dept4function(parsed.Department, parsed.ExternalId);
        },
        error: function (r) {
            console.log("error");
        }

    });
}

function showhideUser(val) {
    if (val == 11) {
        //$("#addnewcommittee").show();
        $("#addnew").hide();
        dsp = "block";
        if ($("#department").val() == "0") {
            $("#loadbtn").prop("disabled", true)
        }
        else {
            $("#loadbtn").prop("disabled", false)
        }
    }
    else if (val == 8) {
        $("#addnewcommittee").hide();
        $("#addnew").show();
        dsp = "none";
        $(".editcombtn").hide();
    }
    else {
        $("#addnewcommittee").show();
        $("#addnew").show();
        $(".editcombtn").show();
    }

}

function examTable(prog, year, sem, shal, session, showDsp) {
    $('#tablebody').children().remove();
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "ExamSetupNewVersion.aspx/GetExamList",
        data: "{'programId':'" + prog + "','yearNo':'" + year + "','semesterNo':'" + sem + "','shal':'" + shal + "','sessionId':'" + session + "'}",
        dataType: "json",
        success: function (data) {

            var parsed = JSON.parse(data.d);

            //console.log(parsed)

            if (parsed == null) {
                $("#newtable").hide();
                $("#addnewcommittee").hide();
                Swal.fire({
                    icon: 'error',
                    title: 'No Data',
                    text: 'No data found',
                    footer: '<p>Try Again</p>'
                })
            }
            else {

                $("#newtable").show();
                $("#tablebody").children().remove();
                var relationid
                var rows = '';
                var sl = 1;
                var editcomval = []

                for (i = 0; i < parsed.length; i++) {
                    var id = parsed[i].RelationId;
                    var programname = parsed[i].ProgramName;
                    var yearno = parsed[i].YearName; //$(this).find("YearNo").text();
                    var SemesterNo = parsed[i].SemesterName; //$(this).find("SemesterNo").text();
                    var ExamSession = parsed[i].ExamSession; //$(this).find("ExamSession").text();
                    var ExamYear = parsed[i].ExamYear; //$(this).find("ExamYear").text();
                    var ChairmanName = parsed[i].ChairmanName; //$(this).find("ChairmanName").text();
                    var MemberOneName = parsed[i].MemberOneName; //$(this).find("MemberOneName").text();
                    var MemberTwoName = parsed[i].MemberTwoName; //$(this).find("MemberTwoName").text();
                    var ExternalName = parsed[i].ExternalName; //$(this).find("ExternalName").text();

                    var ChairmanCode = parsed[i].ChairmanCode;
                    var MemberOneCode = parsed[i].MemberOneCode;
                    var MemberTwoCode = parsed[i].MemberTwoCode;

                    if (ChairmanCode != null && ChairmanCode != "")
                        ChairmanName = "(" + ChairmanCode + ")" + ChairmanName;
                    if (MemberOneCode != null && MemberOneCode != "")
                        MemberOneName = "(" + MemberOneCode + ")" + MemberOneName;
                    if (MemberTwoCode != null && MemberTwoCode != "")
                        MemberTwoName = "(" + MemberTwoCode + ")" + MemberTwoName;

                    editcomval = parsed[i]

                    rows += "<tr><td>" + sl + "</td><td><input type='checkbox' id='checkBox" + sl + "' value='" + parsed[i].RelationId + "' /></td><td><strong>" + programname + "</strong><br/>" + parsed[i].ExamName + "</td><td><strong>Year: </strong>" + yearno + "<br /><strong>Semester: </strong>" + SemesterNo + " <br /><strong>Exam Year: </strong>" + ExamYear + "<br /><strong>Session: </strong>" + ExamSession + "</td><td><strong>Chairman: </strong>" + ChairmanName + "<br /><strong>Member 1: </strong>" + MemberOneName + "<br /><strong>Member 2: </strong>" + MemberTwoName + "<br /><strong>External: </strong>" + ExternalName + "</td><td>" + parsed[i].ChairmanDept + "<br/>" + parsed[i].MemberOneDept + "<br/>" + parsed[i].MemberTwoDept + "<br/>" + parsed[i].ExternalDept + "<br/>" + parsed[i].ExternalDesg + "<br/>" + parsed[i].ExternalUniversity + "</td><td><a href='' class='btn btn-success' data-toggle='modal' data-target='#myModal1' id='editexam" + sl + "' onclick='editexam(" + parsed[i].RelationId + "," + ExamYear + "," + JSON.stringify(ExamSession) + ")' style='width:95%'>Edit Exam</a><br/><a href='' class='btn btn-info editcombtn' data-toggle='modal' data-target='#myModal' onclick='editcom(" + JSON.stringify(editcomval) + ")' style='margin-top:5px;display: none'>Edit Committee</a><br/><a href='' class='btn btn-danger' id='deletebtn" + sl + "' onClick='deleteFunction(" + parsed[i].RelationId + ")' style='width: 96%;'>Delete</a></td></tr>";

                    sl = sl + 1;
                }

                showhideUser(RoleId);
                $('#tablebody').append(rows);
                for (i = 1; i <= sl; i++) {
                    if (RoleId == 11) {
                        $("#editexam" + i).hide()
                        $("#deletebtn" + i).hide()
                    }
                }
                sl = 1;
            }
        },
        error: function (r) {
            console.log("error");
        }
    })
}

