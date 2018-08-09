var maika;
var inviteButton;
var title;
var curUser;
$(document).ready(function(){
	var host = "wss://maikaws.kaisei.app/";
	if(window.location.hostname.includes("localhost"))
		host = "ws:localhost:4322";
	maika = new WebSocket(host);
	userList = $("#userList");
	inviteButton = $(".invite");
	title = $("#title").on("propertychange change keyup input paste", e =>{
		SendMessage({
			Type: "RoomInfo",
			Message:title.val()
		});
		document.title = title.val();
		//console.log(curUser.roomId);
		$.ajax({
			url:"/api/room/"+ curUser.roomId +"/name",
			method:"POST",
			data:{ Name: title.val() }
		});
	});
	maika.onopen =function(e){
		$.ajax({
			url:"/api/user",
			method:"GET",
			success:function(r){
				curUser = r;
				SendMessage({
					Type:"Join",
					Message:JSON.stringify(r)
				});
			}
		});
		console.log("Ready");
	};

	maika.onclose = function(e){
		console.log("close");
	};

	maika.onmessage = function(e){
		var data = JSON.parse(e.data);
		//console.log(data);
		switch(data.Type)
		{
			case "Draw":
				var drawData = JSON.parse(data.Message);
				for(var i = 0; i < drawData.length; i++)
				{
					var e = drawData[i];
					switch(e.type)
					{
						case "circle":
							Circle(mContext, e.p.x, e.p.y, e.size, e.color, e.filled, false, false);
							break;
						case "line":
							Brush(mContext, e.p1.x, e.p1.y, e.p2.x, e.p2.y, e.size, e.color,e.cap, false, false);
							break;
						case "rect":
							Rect(mContext, e.p.x, e.p.y, e.size.h, e.size.w, e.color, e.filled, false, false);
							break;
					}
				}
				break;
			case "Join":
				var user = JSON.parse(data.Message);
				inviteButton.before('<div class="icon user" data-id="'+ user.id +'">'+ user.username +'</div>')
				break;
			case "JoinInfo":
				var users = JSON.parse(data.Message);
				for(var i = 0; i < users.length; i++)
				{
					inviteButton.before('<div class="icon user" data-id="'+ users[i].Id +'">'+ users[i].Username +'</div>')
				}
				break;
			case "Leave":
				var user = JSON.parse(data.Message);
				var users = $(".user");
				for(var i = 0; i < users.length; i++)
				{
					if($(users[i]).data("id") == user.Id)
						users[i].remove();
				}
				break;
			case "RoomInfo":
				title.val(data.Message);
				document.title = data.Message;
				break;
		}
	};

	maika.onerror =function(e){
		console.log(e);
	};
});

function SendMessage(data)
{
	maika.send(JSON.stringify(data));
}