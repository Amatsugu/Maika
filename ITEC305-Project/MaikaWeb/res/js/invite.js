var inviteModal;
var wrapper;
var iLink;
var public;
var inviteId = null;
$(document).ready(() =>{
	inviteModal =  $("#inviteWrapper").css("display", "flex").hide();
	wrapper = $("#wrapper");
	iLink = $("#inviteLink");
	public = $("#public");
	inviteButton.click(e => {
		inviteModal.fadeIn(500);
		wrapper.css("filter", "blur(20px)");
		pubPri(e);
	});
	public.click(pubPri);
	inviteModal.click(e =>{
		if(inviteModal.is(e.target))
		{
			inviteModal.fadeOut();
			wrapper.css("filter", "none");
		}	
	});
});

function pubPri(e)
{
	if(public.prop("checked"))
		iLink.val(window.location);
	else
	{
		if(inviteId == null)
		{
			$.ajax({
				url:"/api/invite",
				method:"POST",
				data:{ Id:curUser.roomId},
				success:d=>
				{
					inviteId = d.id;
					iLink.val("https://maika.luminousvector.com/i/" + inviteId);
				}
			});	
		}else
			iLink.val("https://maika.luminousvector.com/i/" + inviteId);
	}
}