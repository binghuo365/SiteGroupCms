$(document).ready(function(){
	$('section details a.join').hover(
	function(){
		$(this).html("GO IN!");
	},
	function(){
		$(this).html("JOIN US");
	}
	)
	var slider={
		config:{
			width:{max:'640px',min:'0px'},
			height:{max:'450px',min:'80px'}
		},
		all:$('section'),
		left:$('section.left'),
		right:$('section.right'),
		top:$('section.top'),
		bottom:$('section.bottom')
	}
	slider.all.content = $('section>.wrapper')
	slider.all.hover(function(){
		slider.all.content.stop().fadeOut(400);
		$(this).find('.wrapper').stop().fadeTo(400,1);
	})
	$('section.intro>.wrapper').mouseenter();
	slider.all.content.stop(true,true);
	slider.left.mouseenter(function(){
		slider.all.stop();
		slider.left.animate({
			'width':slider.config.width.max
		},
		{ queue: false, duration: 500 }
		);
		slider.right.animate({
			'width':slider.config.width.min
		},
		{ queue: false, duration: 
		500 }
		);
	})
	slider.right.mouseenter(function(){
		slider.all.stop();
		slider.right.animate({
			'width':slider.config.width.max
		},
		{ queue: false, duration: 500 }
		);
		slider.left.animate({
			'width':slider.config.width.min
		},
		{ queue: false, duration: 500 }
		)
	})
	slider.top.mouseenter(function(){
		slider.top.animate({
			'height':slider.config.height.max
		},
		{ queue: false, duration: 500 }
		);
		slider.bottom.animate({
			'height':slider.config.height.min
		},
		{ queue: false, duration: 500 }
		)
	})
	slider.bottom.mouseenter(function(){
		slider.bottom.animate({
			'height':slider.config.height.max
		},
		{ queue: false, duration: 500 }
		);
		slider.top.animate({
			'height':slider.config.height.min
		},
		{ queue: false, duration: 500 }
		)
	})	
})
//若用户4000ms内没有触发以上操作，显示提示信息
$(window).load(function(){
	document.stID = window.setTimeout(function showTip(){
		document.tip = $('<div/>', {
			'class': 'tip',
			text: '试试把鼠标移至其他色块，看看会出现什么？'
		}).appendTo('section.intro').animate({'bottom':'0'});
	},4000);
	$('section.programmer, section.designer, section.editor').one('mouseenter',function(){
		if (document.tip) {
			document.tip.fadeOut(200);
		}
		else{
			window.clearTimeout(document.stID);
		}
	});
})