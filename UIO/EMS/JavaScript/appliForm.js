
//$(document).ready(function () {
//    console.log("ready!");
//    ToggleDiv(false);
//});


function GetInfo() {
    var studentOfficialId = $('#ctl00_MainContainer_StudentApplicationOfficialInfoId').val();
    $.ajax({
        type: "POST",
        url: "StudentApplicationForm.aspx/InfoUpdateStatus",
        data: "{officialId: '" + studentOfficialId + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (rslt) {
            var result = rslt.d;
            ToggleDiv(true);
        },
        error: function (xhr, status, error) {
            Console.log(xhr.responseText);
        }
    });
}

function ToggleDiv(status) {

    if (status) {
        $('#personalDiv').show();
        $('#EducationDiv').show();
        $('#PreviousSemesterDiv').show();
        $('#CourseDiv').show();
        $('#picAndSigDiv').show();
    }
    else {
        $('#personalDiv').hide();
        $('#EducationDiv').hide();
        $('#PreviousSemesterDiv').hide();
        $('#CourseDiv').hide();
        $('#picAndSigDiv').hide();
    }    
}


function toBottom() {
    window.scrollTo(0, document.body.scrollHeight);
}