$(document).ready(function(){

	var $canvas = $("canvas");
	var context = $canvas[0].getContext("2d");
	
	var sBrush = document.createElement("button");
	sBrush.innerHTML = "Small Brush Here";
	sBrush.className = "brush";
	var mBrush = document.createElement("button");
	mBrush.innerHTML = "Medium Brush Here";
	mBrush.className = "brush";
	var lBrush = document.createElement("button");
	lBrush.innerHTML = "Large Brush Here";
	lBrush.className = "brush";
	var clearCanvas = document.createElement("button");
	clearCanvas.innerHTML = "Clear";
	clearCanvas.className = "clear";
	
	
	$("#brushControl").append(sBrush, mBrush, lBrush, clearCanvas);
	
	sBrush.addEventListener("click", function() {
		thickness = 5;
	});
	
	mBrush.addEventListener("click", function() {
		thickness = 10;
	});
	
	lBrush.addEventListener("click", function() {
		thickness = 20;
	});
	
	clearCanvas.addEventListener("click", function() {
		context = $canvas[0].getContext("2d");
		context.clearRect(0, 0, $canvas.width, $canvas.height);
		//alert("Hi");
	});
});