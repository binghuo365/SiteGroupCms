$(function() {
	$(".needFirstItem > *:first-child").addClass("firstItem");
	$(".needLastItem > *:last-child").addClass("lastItem");
	
		
	//带默认值的文本域交互
	$(".hasDefaultValue").each(function() {
		var $thisInput = $(this);
		
		$thisInput.val($thisInput.attr("defaultValue"));
		$thisInput.addClass("defaultValue");
		$thisInput.blur();
		
		$thisInput.focus(function() {
			if($thisInput.hasClass("defaultValue")) {
				$thisInput.val("");
				$thisInput.removeClass("defaultValue")
			}
		});
		$thisInput.blur(function() {
			if($thisInput.val() == "") {
				$thisInput.val($thisInput.attr("defaultValue"));
				$thisInput.addClass("defaultValue");
			}
		});
	});	
	
	$("#searchSubmit").click(function() {
		$("#searchForm").submit();
		return false;
	});
	//响应回车按键
	$("keyword").keydown(function(e) {
		//alert(e.keyCode);//13
		if(e.keyCode == 13) {
			$pageGo.click();
		}
	});
	
});