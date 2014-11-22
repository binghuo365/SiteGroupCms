$(document).ready(function(){
	var offset = $('#frm_videoplay').offset();
	$(".lightSwitcher").css({
		top : offset.top - 40,
		left: offset.left
	}).hide();
	$("#shadow").css("height", $(document).height()).hide();
	$(".lightSwitcher").hover(function() {
                $(this).addClass("mouse-over");
            }, function() {
                $(this).removeClass("mouse-over");
            }).click(function(){
		LightOpenOff();
	});
});
function LightOpenOff(){
	$("#shadow").toggle();
	if ($("#shadow").is(":hidden")){
		$(".lightSwitcher").removeClass("turnedOff");
		$(".lightSwitcher").hide();
	}
	else{
		$(".lightSwitcher").addClass("turnedOff");
		$(".lightSwitcher").show();
	}
}
function ShowHelpInfo(){
	window.open(site.Url+'/help/video.shtml');
}