//$(document).ready(function () {
//    $('#getstudentdata').load("/TeacherModels/GetTeacherStudents");
//})

$(document).ready(function () {
    var url = "/TeacherModels/GetTeacherStudents";
    $.get(url, null, function (data) {
        $("#getstudentdata").html(data);
    }
})
 

$('#nav-button').on('mouseenter', () => {
    $('#nav-button').css('color', '	#0000FF')
})