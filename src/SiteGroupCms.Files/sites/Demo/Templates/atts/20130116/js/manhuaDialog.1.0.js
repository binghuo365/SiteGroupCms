/***
 * 漫画Jquery弹出层插件
 * 编写时间：2012年7月23号
 * version:manhuaDialog.1.0.js
***/
var t;//当前激活层的对象	
var _move=false;//移动标记
var _x,_y;//鼠标离控件左上角的相对位置
var newz=1;//新对象的z-index
var oldz=1;//旧对象的z-index
$(function() {
	$.fn.manhuaDialog = function(options) {
		var defaults = {
			Event : "click",								//触发响应事件
			title : "title",								//弹出层的标题
			type : "text",									//弹出层类型(text、容器ID、URL、Iframe)
			content : "content",							//弹出层的内容(text文本、容器ID名称、URL地址、Iframe的地址)
			width : 500,									//弹出层的宽度
			height : 400,									//弹出层的高度
			closeID : "closeId",							//关闭对话框的ID
			isAuto : false,									//是否自动弹出
			time : 2000,									//设置自动弹出层时间，前提是isAuto=true
			isClose : false,  								//是否自动关闭		
			timeOut : 2000									//设置自动关闭时间，前提是isClose=true
			
		};
		var options = $.extend(defaults,options);		
		$("body").prepend("<div class='floatBoxBg' id='fb"+options.title+"'></div><div class='floatBox' id='"+options.title+"'><div class='title' id='t"+options.title+"'><h4></h4><span class='closeDialog' id='c"+options.title+"'>X</span></div><div class='content'></div></div>");	
		var $this = $(this);								//当然响应事件对象
		var $blank = $("#fb"+options.title);						//遮罩层对象
		var $title = $("#"+options.title+" .title h4");				//弹出层标题对象
		var $content = $("#"+options.title+" .content");				//弹出层内容对象
		var $dialog = $("#"+options.title+"");						//弹出层对象
		var $close = $("#c"+options.title);						//关闭层按钮对象
		var $ttt =  $("#t"+options.title);	
		var $closeId = $("#"+options.closeID);
		var stc,st;
		if ($.browser.msie && ($.browser.version == "6.0") && !$.support.style) {//判断IE6
			$blank.css({height:$(document).height(),width:$(document).width()});
		}
		$close.live("click",function(){
			if ($("#hangyedialog")){
				$("#hangyedialog").hide();
			}
			$blank.hide();
			$dialog.hide();			
			if(st){
				clearTimeout(st);//清除定时器
			}
			if(stc){
				clearTimeout(stc);//清除定时器
			}
		});	
		$closeId.live("click",function(){
			if ($("#hangyedialog")){
				$("#hangyedialog").hide();
			}
			$blank.hide();
			$dialog.hide();			
			if(st){
				clearTimeout(st);//清除定时器
			}
			if(stc){
				clearTimeout(stc);//清除定时器
			}
		});	
		$ttt.live("mousedown",function(e){									  
			 _move=true;
			newz = parseInt($dialog.css("z-index"))
			$dialog.css({"z-index":newz+oldz});
			//t =  $dialog;//初始化当前激活层的对象
			_x=e.pageX-parseInt($dialog.css("left"));//获得左边位置
			_y=e.pageY-parseInt($dialog.css("top"));//获得上边位置
			$dialog.fadeTo(50, 0.5);//点击后开始拖动并透明显示								 
		});
		$(document).live("mousemove",function(e){
			 if(_move){
				var x=e.pageX-_x;//移动时根据鼠标位置计算控件左上角的绝对位置
				var y=e.pageY-_y;
				 $dialog.css({top:y,left:x});//控件新位置			
			}							 
		});
		$ttt.live("mouseup",function(e){
			_move=false;
			 $dialog.fadeTo("fast", 1);//松开鼠标后停止移动并恢复成不透明
			oldz = parseInt($dialog.css("z-index"));//获得最后激活层的z-index							 
		});
		
		$content.css("height",parseInt(options.height)-30);
		//文本框绑定事件
		$this.live(options.Event,function(e){				
			$title.html(options.title);
			switch(options.type){
				case "url":									//当类型是地址的时候					
					$content.ajaxStart(function(){
						$(this).html("loading...");
					});
					$.get(options.content,function(html){
						$content.html(html);						
					});
					break;
				case "text":								//当类型是文本的时候
					$content.html(options.content);
					break;
				case "id":									//当类型是容器ID的时候
					$content.html($("#"+options.content+"").html());
					break;
				case "iframe":								//当类型是Iframe的时候
					$content.html("<iframe src=\""+options.content+"\" width=\"100%\" height=\""+(parseInt(options.height)-40)+"px"+"\" scrolling=\"auto\" frameborder=\"0\" marginheight=\"0\" marginwidth=\"0\"></iframe>");
					break;
				default:									//默认情况下的时候
					$content.html(options.content);
					break;
			}
			
			$blank.show();
			$blank.animate({opacity:"0.5"},"normal");		
			$dialog.css({display:"block",left:(($(document).width())/2-(parseInt(options.width)/2)-5)+"px",top:((document.documentElement.clientHeight)/2-(parseInt(options.height)/2))+"px",width:options.width,height:options.height});
			//$dialog.animate({top:($(document).scrollTop()+options.scrollTop)+"px"},"normal");
			//$dialog.animate({top:options.scrollTop+"px"},"normal");
			if (options.isClose){
				stc = setTimeout(function (){			
					$close.trigger("click");
					clearTimeout(stc);
				},options.timeOut);	
			}
			
		});	
		if (options.isAuto){
			st = setTimeout(function (){			
				$this.trigger(options.Event);
				clearTimeout(st);
			},options.time);	
		}
	}
});

