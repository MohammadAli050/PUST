
var GetFacultyList = [];


function addbutton(data) {
    var total_element = $(".element").length; //$(".element").length;
    var lastid = $(".element:last").attr("id");
    let split_id = lastid.split("_");
    let nextindex = Number(split_id[1]);
    
    let max = 0;
    if (nextindex <= 10) {
        if (data.length > 0) {
            for (var s = 0; s < data.length; s++) {
                let id = `txt_${nextindex++}`;
                $(".element").after("<div class='element' id='div_" + nextindex + "'></div>");
                $("#div_" + nextindex).append("<div id='" + id + "' style='width: 50%;float:left;margin-left:15px'>" + data[s].FacultyName + "</div>&nbsp;<span style='float:left' id='remove_" + nextindex + "' onclick=\"removebutton(this.id," + data[s].Id + ")\" class='remove' style='height: 27pt;width: 45pt;color: white;background-color: #b50d0f;border-radius: 5px;'>Remove</span><div style='clear:both></div>'");
            }
        }
        else if (data.length == 0) {

        }
        else {
            let id = `txt_${nextindex++}`;
            $(".element").after("<div class='element' id='div_" + nextindex + "'></div>");
            $("#div_" + nextindex).append("<div id='" + id + "' style='width: 50%;float:left;margin-left:15px'>" + data.FacultyName + "</div>&nbsp;<span style='float:left' id='remove_" + nextindex + "' onclick=\"removebutton(this.id," + data.Id + ")\" class='remove' style='height: 27pt;width: 45pt;color: white;background-color: #b50d0f;border-radius: 5px;'>Remove</span><div style='clear:both></div>'");
        }
        
        
    }
}
function removebutton(Id, extraFacultyId) {

    $.ajax({
        type: "POST",
        url: "ClassRoutine.aspx/DeleteExtraFaculty",
        data: "{'ExtraFacultyId':'" + extraFacultyId + "'}",
        contentType: "application/json",
        dataType: "json",
        success: function (r) {
            var list = JSON.parse(r.d);
            console.log(list);
            var sms = list.ErrorList[0].Message;

            if (sms == 'Faculty deleted successfully.') {
                $('#SMS3').show();
                $('#SMS2').hide();
                $('#SMS').hide();
                var id = Id;
                var split_id = id.split("_");
                var deleteindex = split_id[1];
                $("#" + id).parent().remove();
            }
            
            else {
                $('#SMS3').hide();
                $('#SMS2').hide();
                $('#SMS').hide();
            }
        },
        error: function (r) {
            console.log(r.d);
        }
    });

}



function loadFacuelty(id) {
  
    $('#show').show();
    var val = '';
    var val = $('#' + id).parent().children().first().text();
    
    $.ajax({
        type: "POST",
        url: "ClassRoutine.aspx/GetAllFaculty",
        data: "{}",
        contentType: "application/json",
        dataType: "json",
        success: function (r) {
            var list = JSON.parse(r.d);
            //console.log(list);
            var facultyList = [];
            for (var i = 0; i < list.length; i++) {
                var RN = {
                    Code: list[i].Code,
                    EmployeeID: list[i].EmployeeID


                };
                facultyList.push(RN);
            }

            var tbody = '';

            $('#dropd').children().remove();

            tbody += '';

            tbody += '<div id="Section_id" style="display:none">' + val +'</div><select id="txt_1" class="dropdown" style="width:100%;margin-left:15px">';
            tbody += ' <option value="">--Select--</option>';
            for (var d = 0; d < facultyList.length; d++) {

                tbody += ' <option value="' + facultyList[d].EmployeeID + '">' + facultyList[d].Code + '</option>';
            }
            tbody += ' </select>';

            

            $('#dropd').append(tbody);


            $('#txt_1').select2({ placeholder: { id: '0', text: '-Select-' }, allowClear: true });

            var F = getFacultyList(val);

            var Data1 = JSON.parse(F);
            var Data = JSON.parse(Data1.d);

            addbutton(Data);

        },
        error: function (r) {
            console.log(r.d);
        }
    });
}


function save(secId, facultyId) {
   
    var facultyName =  $('#txt_1 :selected').text();
    
    var userId = $('#ctl00_MainContainer_HiddenField5').val();
    

    var sectionId = parseInt(secId);
    var ffacultyId = parseInt(facultyId);
    var UserId = parseInt(userId);


    $.ajax({
        type: "POST",
        url: "ClassRoutine.aspx/InsertExtraFaculty",
        data: "{'SectionId':'" + sectionId + "','FacultyId':'" + ffacultyId + "','userId':'" + UserId + "'}",
        contentType: "application/json",
        dataType: "json",
        success: function (r) {
            var list = JSON.parse(r.d); 
            var sms = list.ErrorList[0].Message;
            var Data1 = [];
            var F = getFacultyList(sectionId);

            Data1 = JSON.parse(F);
            var Data = JSON.parse(Data1.d);
            var ab = (Data.length - 1);
           
            if (sms == 'Extra faculty added successfully.') {
                $('#SMS').show();
                $('#SMS2').hide();
                $('#SMS3').hide();
                addbutton(Data[ab]);
            }
            else if (sms == 'Faculty already exists.') {
                $('#SMS2').show();
                $('#SMS').hide();
                $('#SMS3').hide();
            }
            else {
                $('#SMS').hide();
                $('#SMS2').hide();
                $('#SMS3').hide();
            }
            
            
            
            

        },
        error: function (r) {
            console.log(r.d);
        }
    });



   
}

$(function () {
    loadFaculty();
})

function loadFaculty() {
    $('#txt_1').select2({ placeholder: { id: '0', text: '-Select-' }, allowClear: true });
}



function getFacultyList(sectionId) {
    //GetFacultyList = [];
    var f =  $.ajax({
        type: "POST",
        url: "ClassRoutine.aspx/GetExtraFacultyBySectionId",
        data: "{'SectionId':'" + sectionId + "'}",
        contentType: "application/json",
        dataType: "json",
        success: function (r) {
           var list = JSON.parse(r.d);
            //console.log(list);

            //GetFacultyList = list;

        },
        async: false,
        error: function (r) {
            console.log(r.d);
        }
    });

    return f.responseText;
}
