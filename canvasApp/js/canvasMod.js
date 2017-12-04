//Change code to be modular, currently all in one file
//Basic Canvas prototype
//CSCI 305 Internet Programming
//Khamraj Rojit, Christina Wei, Yu Lin, Eddy Lin

var flag = false, dFlag = false;
var x1, x2, y1, y2;
var canvas, context;
var size = 5;
var color = "#000";
var xScale, yScale;
var capturedEvents;
var interval = (20/60)

$(document).ready(function() //TODO: Capture states for network transmition
{
	nextCapture = new Date().getTime() + interval;
	capturedEvents = [];
	var send = function()
	{
		//TODO Send data
		if(capturedEvents.length > 0)
		{
			console.log(capturedEvents);
		}
		capturedEvents = [];
		setTimeout(send, interval);
	};
	send();
	canvas = $("canvas");
	context = canvas[0].getContext("2d");
	xScale = canvas.attr("width")/canvas.outerWidth();
	 yScale = canvas.attr("height")/canvas.outerHeight();
	//On mouse events on the canvas
	canvas.mousedown(function(e){
		x1 = x2;
		y1 = y2;
		x2 = e.clientX - canvas.offset().left;
		y2 = e.clientY - canvas.offset().top;
		
		flag = true;
		Circle(x2, y2, size/2, color, true);
	}).mousemove(function(e){
		if (flag) 
		{
			x1 = x2;
			y1 = y2;
			x2 = e.clientX - canvas.offset().left;
			y2 = e.clientY - canvas.offset().top;
			Brush(x1, y1, x2, y2, size, color);
		}
	}).mouseup(function(){
		flag = false;
	}).mouseleave(function(){
		canvas.mouseup();
	});
});

function Brush(x1, y1, x2, y2, size, color, cap = "round")
{
	context.beginPath();
	context.moveTo((x1 + window.scrollX) * xScale, (y1 + window.scrollY) * yScale);
	context.lineTo((x2 + window.scrollX) * xScale, (y2 + window.scrollY) * yScale);
	context.lineJoin = context.lineCap = cap;
	context.lineWidth = size;	
	context.strokeStyle = color;
	context.stroke();
	context.closePath();
	capturedEvents.push( 
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
		size:size,
		cap:cap
	});
}


function Circle(x, y, r, color, filled = false)
{
	context.beginPath();
	context.fillStyle = color;
	context.arc((x + window.scrollX) * xScale, (y + window.scrollY) * yScale, size/2, 0, 2 * Math.PI);
	if(filled)
		context.fill();
	context.closePath();
	capturedEvents.push( 
	{
		tpye:"circle",
		p:
		{
			x:(x + window.scrollX) * xScale,
			y:(y + window.scrollY) * yScale
		},
		color:color,
		size:size,
		filled:filled
	});
}

function Rect(x, y, h, w, color, filled  = false)
{
	context.beginPath();
	context.fillStyle = color;
	context.Rect(x, y, h, w, color, filled);
	context.closePath();
	capturedEvents.push( 
		{
			tpye:"rect",
			p:
			{
				x:(x + window.scrollX) * xScale,
				y:(y + window.scrollY) * yScale
			},
			color:color,
			size:{
				h:h,
				w:w
			},
			filled:filled
		});
}