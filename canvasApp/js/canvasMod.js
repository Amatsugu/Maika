//Change code to be modular, currently all in one file
//Basic Canvas prototype
//CSCI 305 Internet Programming
//Khamraj Rojit, Christina Wei, Yu Lin, Eddy Lin

var flag = false, dFlag = false;
var x1, x2, y1, y2;
var canvas;
var thickness = 5;

var caputeredEvents = [];
var nextCapture;
var interval = 1000*(20/60)

$(document).ready(function() //TODO: Capture states for network transmition
{
	nextCapture = new Date().getTime() + interval;
	setTimeout(function()
	{
		//TODO Send data
		
		nextCapture = new Date().getTime() + interval;
		captureEvents = [];
	}, interval);
	canvas = $("canvas");
	var context = canvas[0].getContext("2d");
	var xScale = canvas.attr("width")/canvas.outerWidth(), yScale = canvas.attr("height")/canvas.outerHeight(); //TODO: Caclucate scale factor	
	//On mouse events on the canvas
	canvas.mousedown(function(e){
		x1 = x2;
		y1 = y2;
		x2 = e.clientX - canvas.offset().left;
		y2 = e.clientY - canvas.offset().top;

		flag = true;
		dFlag = true;
		if (dFlag) 
		{
			context.beginPath();
			context.fillStyle = color;
			context.arc((x2 + window.scrollX) * xScale, (y2 + window.scrollY) * yScale, thickness/2, 0, 2 * Math.PI);
			context.fill();
			//context.fillRect((x2 + window.scrollX) * xScale, (y2 + window.scrollY) * yScale, thickness, thickness);
			context.closePath();
			dFlag = false;
			captureEvents[captureEvents.length] = 
			{
				tpye:"dot",
				p:
				{
					x:x2,
					y:y2
				},
				color:color,
				size:thickness
			};
		}
	}).mousemove(function(e){
		if (flag) 
		{
			x1 = x2;
			y1 = y2;
			x2 = e.clientX - canvas.offset().left;
			y2 = e.clientY - canvas.offset().top;
			context.beginPath();
			context.moveTo((x1 + window.scrollX) * xScale, (y1 + window.scrollY) * yScale);
			context.lineTo((x2 + window.scrollX) * xScale, (y2 + window.scrollY) * yScale);
			context.lineJoin = context.lineCap = 'round';
			context.lineWidth = thickness;	
			context.strokeStyle = color;
			context.stroke();
			context.closePath();
			captureEvents[captureEvents.length] = 
			{
				tpye:"line",
				p1:
				{
					x:x1,
					y:y1
				},
				p2:
				{
					x:x2,
					y:y2
				},
				color:color,
				size:thickness
			};
		}
	}).mouseup(function(){
		flag = false;
	}).mouseleave(function(){
		canvas.mouseup();
	});
});