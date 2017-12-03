//Change code to be modular, currently all in one file
//Basic Canvas prototype
//CSCI 305 Internet Programming
//Khamraj Rojit, Christina Wei, Yu Lin, Eddy Lin

var flag = false, dFlag = false;
var x1, x2, y1, y2;
var canvas;

$(document).ready(function() //TODO: Capture states for network transmition
{
	canvas = $("canvas");
	var context = $canvas[0].getContext("2d");
	var xScale = 1920/canvas.offsetWidth, yScale = 1080/canvas.offsetHeigh; //TODO: Caclucate scale factor	
	//On mouse events on the canvas
	$canvas.mousedown(function(e){
		x1 = x2;
		y1 = y2;
		x2 = e.clientX - canvas.offsetLeft;
		y2 = e.clientY - canvas.offsetTop;

		flag = true;
		dFlag = true;
		if (dFlag) 
		{
			context.beginPath();
			context.fillStyle = color;
			context.fillRect((x2 + window.scrollX) * xScale, (y2 + window.scrollY) * yScale, 2, 2);
			context.closePath();
			dFlag = false;
		}
	}).mousemove(function(e){
		
		if (flag) 
		{
			x1 = x2;
			y1 = y2;
			x2 = e.clientX - canvas.offsetLeft;
			y2 = e.clientY - canvas.offsetTop;
			context.beginPath();
			context.moveTo((x1 + window.scrollX) * xScale, (y1 + window.scrollY) * yScale);
			context.lineTo((x2 + window.scrollX) * xScale, (y2 + window.scrollY) * yScale);
			context.lineJoin = context.lineCap = 'round';
			context.strokeStyle = color;
			context.stroke();
			context.closePath();
		}
	}).mouseup(function(){
		flag = false;
	}).mouseleave(function(){
		$canvas.mouseup();
	});
});