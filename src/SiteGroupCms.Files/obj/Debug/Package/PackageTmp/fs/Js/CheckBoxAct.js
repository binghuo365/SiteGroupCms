function SelectAll()
{
	var length=document.forms[0].chkNum.value;
	if(length==0)
	{
		return;
	}
	if(length==1)
	{
		document.forms[0].chkBox.checked=document.forms[0].chkAll.checked;
		
		//�����еı�����ɫ
		document.getElementById("tr_0").style.backgroundColor="red";
	}
	if(length>1)
	{
		for(var i=0;i<length;i++)
		{
			document.forms[0].chkBox[i].checked=document.forms[0].chkAll.checked;
			
			//�����еı�����ɫ
            if (document.forms[0].chkBox[i].checked)
            {
			    document.getElementById("tr_" + i).style.backgroundColor="#eeeeee";
			}
			else
			{
			    document.getElementById("tr_" + i).style.backgroundColor="";
			}
		}
	}
}
function UnSelectAll(itemIndex)
{	 
	if(document.forms[0].chkAll.checked)
	{	   	    		
		document.forms[0].chkAll.checked = document.forms[0].chkBox.checked;				
	}
	
	//�����еı�����ɫ
    if (document.forms[0].chkBox[itemIndex].checked)
    {
	    document.getElementById("tr_" + itemIndex).style.backgroundColor="#eeeeee";
	}
	else
	{
	    document.getElementById("tr_" + itemIndex).style.backgroundColor="";
	}	
}