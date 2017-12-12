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
var interval = 1000 * .2;
var cH, cW;
var brushShape = "round";
var isEraser = false;

$(document).ready(function() //TODO: Capture states for network transmition
{
	nextCapture = new Date().getTime() + interval;
	capturedEvents = [];
	var send = function()
	{
		//TODO Send data
		if(capturedEvents.length > 0)
		{
			//console.log(capturedEvents);
			SendMessage({
				Type:"Draw",
				Message: JSON.stringify(capturedEvents)
			});
			capturedEvents = [];
		}
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
	$(window).on("resize", e =>{
		console.log("Resize");
		xScale = cW/mCanvas.outerWidth();
		yScale = cH/mCanvas.outerHeight();
	}).trigger("resize");


	//On mouse events on the canvas
	mCanvas.mousedown(MouseDown).mousemove(MouseMove).mouseup(MouseUp).mouseleave(MouseOut);
	previewCanvas.on("mousemove mousedown mouseup mouseleave", function(e)
	{
		mCanvas.trigger(e);
		e.preventDefault();
	});
	$(window).on("scroll", function(){ previewContext.clearRect(0, 0, cW, cH) });
	previewCanvas.mousemove(DrawToolPreview).mouseleave(function()
	{ 
		previewContext.clearRect(0, 0, cW, cH);
    });

    

    /* line tool Dropdown Style */
    /*
    $("#smallLine").click(function () {
        size = $("#smallLine").val();
    });

    $("#mediumLine").click(function () {
        size = $("#mediumLine").val();
    });

    $("#largeLine").click(function () {
        size = $("#largeLine").val();
    });
    */
});

function DrawToolPreview(e)
{
	previewContext.clearRect(0, 0, cW, cH);
	if(isEraser)
	{
		Circle(previewContext, e.clientX - mCanvas.offset().left, e.clientY - mCanvas.offset().top, size/2, "#000", false, false);
		
	}else
	{
		if(brushShape == "round")
		Circle(previewContext, e.clientX - mCanvas.offset().left, e.clientY - mCanvas.offset().top, size/2, color, true, false);
		else
		Rect(previewContext, ((e.clientX - mCanvas.offset().left) * xScale) - (size/2), ((e.clientY - mCanvas.offset().top) * yScale) - (size/2), size, size, color, true, false);
	}
}

function MouseDown(e)
{
	x1 = x2;
	y1 = y2;
	x2 = e.clientX - mCanvas.offset().left;
	y2 = e.clientY - mCanvas.offset().top;
	flag = true;
	if(isEraser)
	{
		Circle(mContext, x2, y2, size/2, "#fff", true);
	}else
	{
		if(brushShape == "round")
			Circle(mContext, x2, y2, size/2, color, true);
		else
			Rect(mContext, (x2 * xScale) - (size/2), (y2 * yScale) - (size/2), size, size, color, true);
	}
}

function MouseMove(e)
{
	if (flag) 
	{
		x1 = x2;
		y1 = y2;
		x2 = e.clientX - mCanvas.offset().left;
		y2 = e.clientY - mCanvas.offset().top;
		if(isEraser)
			Brush(mContext, x1, y1, x2, y2, size, "#fff", "round");
		else
			Brush(mContext, x1, y1, x2, y2, size, color, brushShape);
	}
}

function MouseUp(e)
{
	flag = false;
}

function MouseOut(e)
{
	//flag = false;
}

function Brush(context, x1, y1, x2, y2, size, color, cap = "round", send = true)
{
	context.beginPath();
	context.moveTo((x1 + window.scrollX) * xScale, (y1 + window.scrollY) * yScale);
	context.lineTo((x2 + window.scrollX) * xScale, (y2 + window.scrollY) * yScale);
	context.lineJoin = context.lineCap = cap;
	context.lineWidth = size;	
	context.strokeStyle = color;
	context.stroke();
	context.closePath();
	if(!send)
		return;
	capturedEvents.push( 
	{
		type:"line",
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
	else
		context.stroke();
	context.closePath();
	if(!send)
		return;
	capturedEvents.push( 
	{
		type:"circle",
		p:
		{
			x:x1,
			y:y1
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
		context.strokeRect(x, y, w ,h);
	context.closePath();
	if(!send)
		return;
	capturedEvents.push( 
		{
			type:"rect",
			p:
			{
				x:x1,
				y:y1
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