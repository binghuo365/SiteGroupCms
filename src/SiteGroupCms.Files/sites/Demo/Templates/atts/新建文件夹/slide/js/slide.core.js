
var Drag={
	obj: null,
	leftTime: null,
	rightTime: null,
	init: function (o,minX,maxX,btnRight,btnLeft) {
		o.onmousedown=Drag.start;
		o.hmode=true;
		if(o.hmode&&isNaN(parseInt(o.style.left))) { o.style.left="0px"; }
		if(!o.hmode&&isNaN(parseInt(o.style.right))) { o.style.right="0px"; }
		o.minX=typeof minX!='undefined'?minX:null;
		o.maxX=typeof maxX!='undefined'?maxX:null;
		o.onDragStart=new Function();
		o.onDragEnd=new Function();
		o.onDrag=new Function();
		btnLeft.onmousedown=Drag.startLeft;
		btnRight.onmousedown=Drag.startRight;
		btnLeft.onmouseup=Drag.stopLeft;
		btnRight.onmouseup=Drag.stopRight;
	},
	start: function (e) {
		var o=Drag.obj=this;
		e=Drag.fixE(e);
		var x=parseInt(o.hmode?o.style.left:o.style.right);
		o.onDragStart(x);
		o.lastMouseX=e.clientX;
		if(o.hmode) {
			if(o.minX!=null) { o.minMouseX=e.clientX-x+o.minX; }
			if(o.maxX!=null) { o.maxMouseX=o.minMouseX+o.maxX-o.minX; }
		} else {
			if(o.minX!=null) { o.maxMouseX= -o.minX+e.clientX+x; }
			if(o.maxX!=null) { o.minMouseX= -o.maxX+e.clientX+x; }
		}
		document.onmousemove=Drag.drag;
		document.onmouseup=Drag.end;
		return false;
	},
	drag: function (e) {
		e=Drag.fixE(e);
		var o=Drag.obj;
		var ex=e.clientX;
		var x=parseInt(o.hmode?o.style.left:o.style.right);
		var nx;
		if(o.minX!=null) { ex=o.hmode?Math.max(ex,o.minMouseX):Math.min(ex,o.maxMouseX); }
		if(o.maxX!=null) { ex=o.hmode?Math.min(ex,o.maxMouseX):Math.max(ex,o.minMouseX); }
		nx=x+((ex-o.lastMouseX)*(o.hmode?1:-1));
		$i("scrollcontent").style[o.hmode?"left":"right"]=(-nx*barUnitWidth)+"px";
		Drag.obj.style[o.hmode?"left":"right"]=nx+"px";
		Drag.obj.lastMouseX=ex;
		Drag.obj.onDrag(nx);
		return false;
	},
	startLeft: function () {
		Drag.leftTime=setInterval("Drag.scrollLeft()",1);
	},
	scrollLeft: function () {
		var c=$i("scrollcontent");
		var o=$i("scrollbar");
		if((parseInt(o.style.left.replace("px",""))<(590-162-10))&&(parseInt(o.style.left.replace("px",""))>=0)) {
			o.style.left=(parseInt(o.style.left.replace("px",""))+1)+"px";
			c.style.left=(-(parseInt(o.style.left.replace("px",""))+1)*barUnitWidth)+"px";
		} else {
			Drag.stopLeft();
		}
	},
	stopLeft: function () {
		clearInterval(Drag.leftTime);
	},
	startRight: function () {
		Drag.rightTime=setInterval("Drag.scrollRight()",1);
	},
	scrollRight: function () {
		var c=$i("scrollcontent");
		var o=$i("scrollbar");
		if((parseInt(o.style.left.replace("px",""))<=(590-162-10))&&(parseInt(o.style.left.replace("px",""))>0)) {
			o.style.left=(parseInt(o.style.left.replace("px",""))-1)+"px";
			c.style.left=(-(parseInt(o.style.left.replace("px",""))-1)*barUnitWidth)+"px";
		} else {
			Drag.stopRight();
		}
	},
	stopRight: function () {
		clearInterval(Drag.rightTime);
	},
	end: function () {
		document.onmousemove=null;
		document.onmouseup=null;
		Drag.obj.onDragEnd(parseInt(Drag.obj.style[Drag.obj.hmode?"left":"right"]));
		Drag.obj=null;
	},
	fixE: function (e) {
		if(typeof e=='undefined') { e=window.event; }
		if(typeof e.layerX=='undefined') { e.layerX=e.offsetX; }
		return e;
	}
};
var scrollbar = $i('scrollbar');
var scrollleft = $i('scrollleft');
var scrollright = $i('scrollright');
$("#scrollcontent").css({ width: '600px', left: '-0px', position:'absolute'});
var _tmp1="",_tmp2="",data=slideJSON;
$.each(data,function(i){
	_tmp1+='<li><a href="'+data[i].link+'"><img title="'+data[i].title+'" src="'+data[i].img+'"></a></li>';
	if(i==$CurrentPage-1){
		_tmp2+='<li><a href="'+data[i].link+'"><img title="'+data[i].title+'" src="'+data[i].img+'" class="current"></a></li>';
	}else{
		_tmp2+='<li><a href="'+data[i].link+'"><img title="'+data[i].title+'" src="'+data[i].img+'"></a></li>';
	}
});
$("#imagelist").html(_tmp1);
$("#scrollcontent").html(_tmp2);
if($TotalPage>5){
	if(scrollbar&&scrollright){
		Drag.init(scrollbar,0,418,scrollleft,scrollright);
	}
	var scrollcontentWidth = (118*$TotalPage+120)+'px';
	var scrollcontentLeft = '-0px';
	var scroolbarLeft = '0px';
	if($CurrentPage>3){
		var $$CurrentPage = ($CurrentPage+2>$TotalPage)? ($TotalPage-5): ($CurrentPage-3);
		scrollcontentLeft = '-'+(118*$$CurrentPage)+'px';
		scroolbarLeft = ((118*$$CurrentPage)/barUnitWidth)+'px';

	}
	$("#scrollcontent").css({ width: scrollcontentWidth, left: scrollcontentLeft});
	$("#scrollbar").css({ left: scroolbarLeft});
}
else{
    $("#scrollleft").hide();
    $("#scrollright").hide();
    $("#scrollbar img").hide();
}