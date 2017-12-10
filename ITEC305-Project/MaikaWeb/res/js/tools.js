var brush, pencil, colorPicker;
var colorWindow;
var hue;
var colorCanvas;
var colorCtx;
var h, w;
var sH = 300;
var bLColor = [1, 0, 0, 1];
var sX, sY;
var hR, hG, hB, hex;
var open = false;
var outlineColor;
$(document).ready(()=>{
	brush = $(".brush").click(e=>{
		brushShape = "round";
	});
	pencil = $(".pencil").click(e=>{
		brushShape = "square";
	});
	h = w = 256;
	sX = sY = 128;
	colorPicker = $(".color").click(e=>{
		if(!open)
			colorWindow.fadeIn();
		else
			colorWindow.fadeOut();
		open = !open;
	});
	hue = $("#hue");
	hR = $("#rgb #r");
	hG = $("#rgb #g");
	hB = $("#rgb #b");
	hex = $("#hexInput");
	$("#rgb input").on("propertychange change keyup input paste", e=>{
		/*bLColor = [hR.val()/255, hG.val()/255, hB.val()/255, 1];
		quadGradient();
		DrawSelector(sX, sY);*/
	});
	colorWindow = $("#colorDropdown").css("display", "grid").hide();
	colorCanvas = $("#colorDropdown #color");
	colorCtx = colorCanvas[0].getContext("2d");
	colorWindow.css({
		left:colorPicker.offset().left,
		top:colorPicker.offset().top + 50
	});
	hue.on("mousedown mousemove", e =>{
		if(e.buttons != 1)
			return;
		sH = (e.clientX - hue.offset().left)/hue.width();
		sH *= 360;
		GetRGB();
	});
	colorCanvas.on("mousedown mousemove", e =>{
		if(e.buttons != 1)
			return;
		quadGradient(colorCtx);
		e.preventDefault();
		DrawSelector(e.clientX - colorCanvas.offset().left, e.clientY - colorCanvas.offset().top);
	});

	GetRGB();
});

function rgbToHex(r, g, b) {
    return "#" + ((1 << 24) + (r << 16) + (g << 8) + b).toString(16).slice(1);
}

function GetRGB()
{
	var c = 1 * 1;
	var x = c * (1 - Math.abs(((sH/60) % 2)-1));
	var m = 1 - c;
	var rgb;
	if(sH > 300)
		rgb = {r:c, g:0, b:x};
	else if(sH > 240)
		rgb = {r:x, g:0, b:c};
	else if(sH > 180)
		rgb = {r:0, g:x, b:c};
	else if(sH > 120)
		rgb = {r:0, g:c, b:x};
	else if(sH > 60)
		rgb = {r:x, g:c, b:0};
	else 
		rgb = {r:c, g:x, b:0};
	rgb.r = (rgb.r + m);
	rgb.g = (rgb.g + m);
	rgb.b = (rgb.b + m);
	bLColor = [rgb.r, rgb.g, rgb.b, 1];
	quadGradient();
	DrawSelector(sX, sY);
}

function GetSelectedColor(x, y)
{
	var p = colorCtx.getImageData(x, y, 1, 1).data;
	color = "rgb("+p[0]+","+p[1]+","+p[2]+")";
	outlineColor = "rgb("+(255-p[0])+","+(255-p[1])+","+(255-p[2])+")";
	hR.val(Math.round(p[0]));
	hG.val(Math.round(p[1]));
	hB.val(Math.round(p[2]));
	hex.val(rgbToHex(p[0], p[1], p[2]));
	colorPicker.css("background", color);
}

function DrawSelector(x, y)
{
	sX = x;
	sY = y;
	GetSelectedColor(x,y);
	colorCtx.beginPath();
	colorCtx.fillStyle = outlineColor;
	colorCtx.arc(x , y, 8, 0, 2 * Math.PI);
	colorCtx.fill();
	colorCtx.closePath();
	colorCtx.beginPath();
	colorCtx.fillStyle = color;
	colorCtx.arc(x , y, 6, 0, 2 * Math.PI);
	colorCtx.fill();
	colorCtx.closePath();
}

function quadGradient() {
	var corners = {
		topLeft: [1, 1, 1, 1],
		topRight: [0, 0, 0, 1],
		bottomLeft: bLColor,
		bottomRight: [0, 0, 0, 1]
	}
    var gradient, startColor, endColor, fac;

    for(var i = 0; i < h; i++) {
        gradient = colorCtx.createLinearGradient(0, i, w, i);
        fac = i / (h - 1);

        startColor = arrayToRGBA(
          lerp(corners.topLeft, corners.bottomLeft, fac)
        );
        endColor = arrayToRGBA(
          lerp(corners.topRight, corners.bottomRight, fac)
        );

        gradient.addColorStop(0, startColor);
        gradient.addColorStop(1, endColor);

        colorCtx.fillStyle = gradient;
        colorCtx.fillRect(0, i, w, i);
    }
}

function arrayToRGBA(arr) {
    var ret = arr.map(function(v) {
        // map to [0, 255] and clamp
        return Math.max(Math.min(Math.round(v * 255), 255), 0);
    });

    // alpha should retain its value
    ret[3] = arr[3];

    return 'rgba(' + ret.join(',') + ')';
}

function lerp(a, b, fac) {
    return a.map(function(v, i) {
        return v * (1 - fac) + b[i] * fac;
    });
}