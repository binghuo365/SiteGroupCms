<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="progress.aspx.cs" Inherits="SiteGroupCms.progress" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">

		<script type="text/javascript" src="lib/js/ga.js">	</script>
		<script type="text/javascript" src="lib/js/jquery.js"></script>
		<script type="text/javascript" src="lib/js/jquery.chili-2.2.js"></script>
		<script type="text/javascript" src="lib/js/jquery.progressbar.min.js"></script>
		<script type="text/javascript">
		    $(document).ready(function () {
		        $("#pb1").progressBar({ max: 100, textFormat: 'fraction', callback: function (data) { if (data.running_value == data.value) { } } });
		    });

		    function beginUpload() {
		        $("#pb1").fadeIn();
		        var i = setInterval(function () {
		            $.getJSON("cs2.aspx", function (data) {
		                if (data == null) {
		                    clearInterval(i);
		                    location.reload(true);
		                    return;
		                }
		                var percentage = Math.floor(100 * parseInt(data.bytes_uploaded) / parseInt(data.bytes_total));
		                $("#pb1").progressBar(percentage);
		            });
		        }, 1500);

		        return true;
		    }
		</script>
	<div id="container">
	  <div class="contentblock">
		<h2>&nbsp;</h2>
				<table>
					<tr><span class="progressBar" id="pb1">75%</span></tr>
					
				</table>
		<a href="#" onclick="$('#pb1').progressBar(20);">20</a> 

		</div>
	</div>
	<iframe style="display: none;" name="progressFrame"></iframe>
    </div>
    </form>
</body>
</html>
