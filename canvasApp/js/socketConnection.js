var maika;
$(document).ready(function(){
	maika = new WebSocket("ws://localhost:4322");
	maika.onopen =function(e){
		console.log("Ready");
		maika.send({
			Type:"Join",
			Message:"Hello"
		});
	};

	maika.onclose = function(e){
		console.log("close");
	};

	maika.onmessage = function(e){
		return false;
	};

	maika.onerror =function(e){
		console.log(e);
	};
});