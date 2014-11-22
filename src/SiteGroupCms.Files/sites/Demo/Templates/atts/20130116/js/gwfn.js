$(function() {
	//导航交互
	$(".nav li").hover(function() {$(this).toggleClass("active");});
	//tabs切换
	var $tabs = $("[show]");
	$(".active[show]").each(function() {
		$($(this).attr("show")).siblings().hide();
	});
	$tabs.hover(function() {
		$(this).addClass("active").siblings().removeClass("active");
		$($(this).attr("show")).show().siblings().hide();
	}, function() {});
	
	//精品课程表格偶数行背景及鼠标hover
	$(".classListTable tbody tr:odd").addClass("even");
	$(".classListTable tbody tr").hover(function(){$(this).toggleClass("hover");});
	
	
});