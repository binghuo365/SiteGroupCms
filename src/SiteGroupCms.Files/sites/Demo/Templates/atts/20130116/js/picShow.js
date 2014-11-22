$(function() {
	//图片轮播，671X348，设置详见http://workshop.rs/2009/12/image-gallery-with-fancy-transitions-effects/
	$('#flash').jqFancyTransitions({ effect: 'wave', width: 671, height: 348, links: true });
	
	//中心公告文字滚动
	var c_width = $("#noticeBox").width();
	var l_width = 0;
	var l_box = $("#noticeList");
	var l_itmes = $("li", l_box);
	l_itmes.each(function(){
		l_width += $(this).width();
	});
	if(l_width > c_width) {
		l_box.width(l_width * 2);
		l_itmes.clone().appendTo(l_box);
		var marleft=0;
		function rollText(){
			marleft = (marleft + 1) % l_width;
			l_box.css("margin-left",-marleft);
		}
		var int=setInterval(rollText,30);
		$("#noticeBox").hover(
			function(){
				clearInterval(int);
			},
			function(){
				int=setInterval(rollText,30);
			});
	}//if(l_width > c_width)
	
	
	var sWidth = $("#focus").width(); //获取焦点图的宽度（显示面积）
	var len = $("#focus ul li").length; //获取焦点图个数
	var index = 0;
	var picTimer;
	
	//以下代码添加数字按钮和按钮后的半透明条，还有上一页、下一页两个按钮
	var btn = "<div class='btnBg'></div><div class='btn'>";
	for(var i=0; i < len; i++) {
		btn += "<span></span>";
	}
	btn += "</div>";
	$("#focus").append(btn);
	$("#focus .btnBg").css("opacity",0.5);

	//为小按钮添加鼠标滑入事件，以显示相应的内容
	$("#focus .btn span").css("opacity",0.4).mouseover(function() {
		index = $("#focus .btn span").index(this);
		showPics(index);
	}).eq(0).trigger("mouseover");

	//本例为左右滚动，即所有li元素都是在同一排向左浮动，所以这里需要计算出外围ul元素的宽度
	$("#focus ul").css("width",sWidth * (len));
	
	//鼠标滑上焦点图时停止自动播放，滑出时开始自动播放
	$("#focus").hover(function() {
		clearInterval(picTimer);
	},function() {
		picTimer = setInterval(function() {
			showPics(index);
			index++;
			if(index == len) {index = 0;}
		},3000); //此3000代表自动播放的间隔，单位：毫秒
	}).trigger("mouseleave");
	
	//显示图片函数，根据接收的index值显示相应的内容
	function showPics(index) { //普通切换
		var nowLeft = -index*sWidth; //根据index值计算ul元素的left值
		$("#focus ul").stop(true,false).animate({"left":nowLeft},300); //通过animate()调整ul元素滚动到计算出的position
		//$("#focus .btn span").removeClass("on").eq(index).addClass("on"); //为当前的按钮切换到选中的效果
		$("#focus .btn span").stop(true,false).animate({"opacity":"0.4"},300).eq(index).stop(true,false).animate({"opacity":"1"},300); //为当前的按钮切换到选中的效果
	}
});