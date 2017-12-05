var maika;
$(document).ready(function(){
	maika = new WebSocket("ws://localhost:4322");
	maika.onopen =function(e){
		console.log("Ready");
	};

	maika.onclose = function(e){
		console.log("close");
	};

	maika.onmessage = function(e){
		var data = JSON.parse(e.data);
		if(data.Type == "Draw" || data.Type == 1)
		{
			var drawData = JSON.parse(data.Message);
			drawData.forEach(e => {
				switch(e.type)
				{
					case "circle":
						Circle(mContext, e.p.x, e.p.y, e.r, e.color, e.filled, false);
						break;
					case "line":
						Brush(mContext, e.p1.x, e.p1.y, e.p2.x, e.p2.y, e.size, e.color,e.cap, false);
				}
			});
			//console.log(drawData);
		}
		console.log(data);
	};

	maika.onerror =function(e){
		console.log(e);
	};
});

function SendMessage(data)
{
	maika.send(JSON.stringify(data));
}