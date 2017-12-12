$(document).ready(function(){

	var $canvas = $("canvas");
	var context = $canvas[0].getContext("2d");
	
	var pencilTool = document.createElement("button");
	pencilTool.innerHTML = "pencil tool here";
	pencilTool.className = "tool";
	var sprayTool = document.createElement("button");
	sprayTool.innerHTML = "spray tool here";
	sprayTool.className = "tool";
	
	//$("#toolControl").append(pencilTool, squareTool, sprayTool);
	
	pencilTool.addEventListener("click", function() {
		context.lineWidth = 2;	
	});
});
	
	