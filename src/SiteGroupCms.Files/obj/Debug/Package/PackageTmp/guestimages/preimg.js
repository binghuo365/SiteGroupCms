/*
* 功能：大图预览
* 编写：2012.12.26 by 王洋
*/
$(function(){
	var x = 10;
	var y = 20;
	$("a.tooltip").mouseover(function(e){
		this.myTitle = this.title;
		this.title = "";//this.title 置空	
		var imgTitle = this.myTitle? "<br/>" + this.myTitle : "";//判断图片的标题
		//用this href 获取 图片
		var htmlimg='';
		var htmlimg="<div id='tooltip'>";
		//htmlimg+="<div style='height:10px;padding:0;'>"+imgTitle+"</div>";
		htmlimg+="<img src='"+ this.href +"' alt='项目预览图' width='600' height='550' /><\/div>";
		//var tooltip = "<div id='tooltip'>"+imgTitle+"<img src='"+ this.href +"' alt='项目预览图' width='600' height='550' /><\/div>"; //创建 div 元素  <\/div>  转义
		$("body").append(htmlimg);	//把它追加到文档中						 
		$("#tooltip")
			.css({
				"top": (e.pageY+y) + "px",
				"left":  (e.pageX+x)  + "px"
			}).show("fast");	  //设置x坐标和y坐标，并且显示
    })
	.mouseout(function(){
		this.title = this.myTitle;//将title赋值，因为下次还要读取
		$("#tooltip").remove();	 //移除 
    })
	//跟随鼠标移动
	.mousemove(function(e){
		$("#tooltip")
			.css({
				"top": (e.pageY+y) + "px",
				"left":  (e.pageX+x)  + "px"
			});
	})
	//取消默认的打开链接
	.click(function(){
		return false;
		});
})
