
var pageNum=20;
var outlines=document.getElementById("news_list_ul").getElementsByTagName("li");



var pageCount=0;
if(outlines.length%pageNum>0){
 pageCount = ((outlines.length - (outlines.length%pageNum))/pageNum +1);
}
else
 pageCount = outlines.length/pageNum;
var CP=document.getElementById("CP");
var FileBody=document.getElementById("content");

function getCurrPage(_currentPage){
 var cPage =1;
 if( _currentPage<=0 || _currentPage=="")
  cPage =1;
 else if(_currentPage>pageCount)
  cPage = pageCount;
 else
  cPage = _currentPage;
 return cPage;
}


function goto(){
  toPage(CP.value);
}


function toPage(_pageNo){

 var pageHtml="";
 var cP = getCurrPage(_pageNo);
 var startPos = cP*pageNum-pageNum;
 var endPos = 0;
 if(cP*pageNum>outlines.length)
  endPos=outlines.length;
 else
  endPos = cP*pageNum;
 for(var i=startPos;i<endPos;i++){
   if(i==0){
    //pageHtml+="<TR><TD WIDTH='\"20\' HEIGHT='\"21\' ALIGN='\"CENTER\' VALIGN=''\"MIDDLE\'>";
   }
   if(null==outlines[0] || outlines[0]==""){
		return false;
	}
	
   
   pageHtml+="<li>";
   pageHtml += outlines[i].innerHTML;
   pageHtml+="</li>";
 }
 CP.value = cP;
 FileBody.innerHTML= pageHtml;
 showPageLineNum();
 return false;
}

function showPageLineNum(){
 var pL = "";
 if(CP.value!=1){
  pL+="<a href='javascript:void(0)' onclick='toPage(1)'>\u9996\u9875</a>&nbsp;";
  pL+="<a href='javascript:void(0)' onclick='toPage("+(CP.value-1)+")'>\u4E0A\u4E00\u9875</a>&nbsp;";
 }
 else{
  pL+="\u9996\u9875&nbsp;"
  pL+="\u4E0A\u4E00\u9875&nbsp;"
 }
 for(var pageN=1;pageN<=pageCount;pageN++){
  if(pageN==CP.value){
  pL+="<b> "+pageN+" </b>&nbsp;";
  }
  else
  pL += "<a href='javascript:void(0)' onclick='toPage("+pageN+")'>"+pageN+"</a>&nbsp;";
 }
 if(CP.value<pageCount){
  pL+="<a href='javascript:void(0)' onclick='toPage("+((CP.value)*1+1)+")'>\u4E0B\u4E00\u9875</a>&nbsp;";
  pL+="<a href='javascript:void(0)' onclick='toPage("+pageCount+")'>\u5C3E\u9875</a>&nbsp;";
 }
 else{
  pL+="\u4E0B\u4E00\u9875&nbsp;"
  pL+="\u5C3E\u9875&nbsp;"
 }
 pL += "\u5171<b> "+pageCount+" </b>\u9875&nbsp;\u5171<b> "+outlines.length+" </b>\u6761\u8BB0\u5F55";
 var showPageLine = document.getElementById("pl");
 showPageLine.innerHTML = pL;
}

