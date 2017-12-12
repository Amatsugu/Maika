var email, pass1, pass2, error;

$(document).ready(() =>{
	pass1 = $("#pass").on("propertychange change keyup input paste", CheckPassword);
	pass2 = $("#pass2").on("propertychange change keyup input paste", CheckPassword);
	email = $("#email").on("propertychange change keyup input paste", CheckEmail);
	$("form").on("submit", e =>{
		$.ajax();		
	});


});

function CheckPassword(e)
{
	if(pass2.val() != "")
	{
		if(pass1.val() != pass2.val())
		{
			pass1.addClass("error");
			pass2.addClass("error");
			return;
		}
	}
	pass1.removeClass("error");
	pass2.removeClass("error");
}

function CheckEmail()
{
	$.ajax({
		url:"/auth/checkemail/" + email.val(),
		method:"GET",
		success:function(r){
			email.removeClass("error");
		},
		error:function(e)
		{
			email.addClass("error");
		}
	});
}