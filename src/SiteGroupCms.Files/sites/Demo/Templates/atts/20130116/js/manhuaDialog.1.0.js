/***
 * ����Jquery��������
 * ��дʱ�䣺2012��7��23��
 * version:manhuaDialog.1.0.js
***/
var t;//��ǰ�����Ķ���	
var _move=false;//�ƶ����
var _x,_y;//�����ؼ����Ͻǵ����λ��
var newz=1;//�¶����z-index
var oldz=1;//�ɶ����z-index
$(function() {
	$.fn.manhuaDialog = function(options) {
		var defaults = {
			Event : "click",								//������Ӧ�¼�
			title : "title",								//������ı���
			type : "text",									//����������(text������ID��URL��Iframe)
			content : "content",							//�����������(text�ı�������ID���ơ�URL��ַ��Iframe�ĵ�ַ)
			width : 500,									//������Ŀ��
			height : 400,									//������ĸ߶�
			closeID : "closeId",							//�رնԻ����ID
			isAuto : false,									//�Ƿ��Զ�����
			time : 2000,									//�����Զ�������ʱ�䣬ǰ����isAuto=true
			isClose : false,  								//�Ƿ��Զ��ر�		
			timeOut : 2000									//�����Զ��ر�ʱ�䣬ǰ����isClose=true
			
		};
		var options = $.extend(defaults,options);		
		$("body").prepend("<div class='floatBoxBg' id='fb"+options.title+"'></div><div class='floatBox' id='"+options.title+"'><div class='title' id='t"+options.title+"'><h4></h4><span class='closeDialog' id='c"+options.title+"'>X</span></div><div class='content'></div></div>");	
		var $this = $(this);								//��Ȼ��Ӧ�¼�����
		var $blank = $("#fb"+options.title);						//���ֲ����
		var $title = $("#"+options.title+" .title h4");				//������������
		var $content = $("#"+options.title+" .content");				//���������ݶ���
		var $dialog = $("#"+options.title+"");						//���������
		var $close = $("#c"+options.title);						//�رղ㰴ť����
		var $ttt =  $("#t"+options.title);	
		var $closeId = $("#"+options.closeID);
		var stc,st;
		if ($.browser.msie && ($.browser.version == "6.0") && !$.support.style) {//�ж�IE6
			$blank.css({height:$(document).height(),width:$(document).width()});
		}
		$close.live("click",function(){
			if ($("#hangyedialog")){
				$("#hangyedialog").hide();
			}
			$blank.hide();
			$dialog.hide();			
			if(st){
				clearTimeout(st);//�����ʱ��
			}
			if(stc){
				clearTimeout(stc);//�����ʱ��
			}
		});	
		$closeId.live("click",function(){
			if ($("#hangyedialog")){
				$("#hangyedialog").hide();
			}
			$blank.hide();
			$dialog.hide();			
			if(st){
				clearTimeout(st);//�����ʱ��
			}
			if(stc){
				clearTimeout(stc);//�����ʱ��
			}
		});	
		$ttt.live("mousedown",function(e){									  
			 _move=true;
			newz = parseInt($dialog.css("z-index"))
			$dialog.css({"z-index":newz+oldz});
			//t =  $dialog;//��ʼ����ǰ�����Ķ���
			_x=e.pageX-parseInt($dialog.css("left"));//������λ��
			_y=e.pageY-parseInt($dialog.css("top"));//����ϱ�λ��
			$dialog.fadeTo(50, 0.5);//�����ʼ�϶���͸����ʾ								 
		});
		$(document).live("mousemove",function(e){
			 if(_move){
				var x=e.pageX-_x;//�ƶ�ʱ�������λ�ü���ؼ����Ͻǵľ���λ��
				var y=e.pageY-_y;
				 $dialog.css({top:y,left:x});//�ؼ���λ��			
			}							 
		});
		$ttt.live("mouseup",function(e){
			_move=false;
			 $dialog.fadeTo("fast", 1);//�ɿ�����ֹͣ�ƶ����ָ��ɲ�͸��
			oldz = parseInt($dialog.css("z-index"));//�����󼤻���z-index							 
		});
		
		$content.css("height",parseInt(options.height)-30);
		//�ı�����¼�
		$this.live(options.Event,function(e){				
			$title.html(options.title);
			switch(options.type){
				case "url":									//�������ǵ�ַ��ʱ��					
					$content.ajaxStart(function(){
						$(this).html("loading...");
					});
					$.get(options.content,function(html){
						$content.html(html);						
					});
					break;
				case "text":								//���������ı���ʱ��
					$content.html(options.content);
					break;
				case "id":									//������������ID��ʱ��
					$content.html($("#"+options.content+"").html());
					break;
				case "iframe":								//��������Iframe��ʱ��
					$content.html("<iframe src=\""+options.content+"\" width=\"100%\" height=\""+(parseInt(options.height)-40)+"px"+"\" scrolling=\"auto\" frameborder=\"0\" marginheight=\"0\" marginwidth=\"0\"></iframe>");
					break;
				default:									//Ĭ������µ�ʱ��
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

