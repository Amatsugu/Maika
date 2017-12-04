var maika;
$(document).ready(function(){
	maika = new WebSocket("ws://localhost:4322");
	maika.addEventListener("open", function(e){
		maika.send({
			Type:"Join",
			Message:"Hello"
		});
	})
});