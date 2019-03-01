//$(document).ready(function () {
//    $('#getstudentdata').load("/TeacherModels/GetTeacherStudents");
////})


var url = "/TeacherModels/GetStudents";
$.get(url, null, function (data) {
    $("#getstudentdata").html(data);
});



    


$('#nav-button').on('mouseenter', () => {
    $('#nav-button').css('color', '	#0000FF')
})