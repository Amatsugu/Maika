//Change code to be modular, currently all in one file
//Basic Canvas prototype
//CSCI 305 Internet Programming
//Khamraj Rojit, Christina Wei, Yu Lin, Eddy Lin

var flag = false, dFlag = false;
var x1, x2, y1, y2;
var mCcanvas, previewCanvas, mContext, previewContext;
var size = 5;
var color = "#000";
var xScale, yScale;
var capturedEvents;
var interval = (20/60)
var cH, cW;

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
	mCanvas = $("#mainCanvas");
	previewCanvas = $("#previewCanvas");
	mContext = mCanvas[0].getContext("2d");
	previewContext = previewCanvas[0].getContext("2d");
	cW = mCanvas.attr("width");
	cH = mCanvas.attr("height");
	Clear(mContext, "#fff");
	xScale = cW/mCanvas.outerWidth();
	yScale = cH/mCanvas.outerHeight();
	//On mouse events on the canvas
	mCanvas.mousedown(MouseDown).mousemove(MouseMove).mouseup(MouseUp).mouseleave(MouseOut);
	previewCanvas.on("mousemove mousedown mouseup mouseleave", function(e)
	{
		mCanvas.trigger(e);
	});
	$(window).on("scroll", function(){ previewContext.clearRect(0, 0, cW, cH) });
	previewCanvas.mousemove(DrawToolPreview).mouseleave(function()
	{ 
		previewContext.clearRect(0, 0, cW, cH);
	});
});

function DrawToolPreview(e)
{
	previewContext.clearRect(0, 0, cW, cH);
	Circle(previewContext, e.clientX - mCanvas.offset().left, e.clientY - mCanvas.offset().top, size/2, color, true, false);
}

function MouseDown(e)
{
	x1 = x2;
	y1 = y2;
	x2 = e.clientX - mCanvas.offset().left;
	y2 = e.clientY - mCanvas.offset().top;
	flag = true;
	Circle(mContext, x2, y2, size/2, color, true);
}

function MouseMove(e)
{
	if (flag) 
	{
		x1 = x2;
		y1 = y2;
		x2 = e.clientX - mCanvas.offset().left;
		y2 = e.clientY - mCanvas.offset().top;
		Brush(mContext, x1, y1, x2, y2, size, color);
	}
}

function MouseUp(e)
{
	flag = false;
}

function MouseOut(e)
{
	flag = false;
}

function Brush(context, x1, y1, x2, y2, size, color, cap = "round")
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


function Circle(context, x, y, r, color, filled = false, send = true)
{
	context.beginPath();
	context.fillStyle = color;
	context.arc((x + window.scrollX) * xScale, (y + window.scrollY) * yScale, size/2, 0, 2 * Math.PI);
	if(filled)
		context.fill();
	context.closePath();
	if(!send)
		return;
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

function Rect(context, x, y, h, w, color, filled  = false, send = true)
{
	context.beginPath();
	context.fillStyle = color;
	if(filled)
		context.fillRect(x, y, w, h);
	else
		context.strokeRect(x,y, w ,h);
	context.closePath();
	if(!send)
		return;
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

function Clear(context, clearColor)
{
	Rect(context, 0, 0, cH, cW, clearColor, true, false);
}