<%@ Page language="c#" Debug="true"%>
<%@import namespace="SFA" %>
<%@ Import Namespace="Dial4Jobz.Controllers" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" > 

<html>
  <head>
    <title>SFAResponse</title>
    <meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" Content="C#">
    <meta name=vs_defaultClientScript content="JavaScript">
    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
    <script language="C#" runat="server">
    public void display()
	{
	
			PGResponse oPgResp=new PGResponse();
           // EmployerVasController empvas = new EmployerVasController();
			EncryptionUtil lEncUtil= new EncryptionUtil();  
			string respcd=null;
			string respmsg=null;
			string astrResponseData=null;
			string strMerchantId,astrFileName=null;
			string strKey=null;
			string strDigest=null;
			string astrsfaDigest=null;

            strMerchantId = "00004575";
            astrFileName = "d://inetpub//vhosts//dial4jobz.in//httpdocs//Key//00004575.key";
            
			if(Request.ServerVariables["REQUEST_METHOD"]=="POST")
			{
					
					astrResponseData = Request.Form["DATA"];					
					strDigest = Request.Form["EncryptedData"];
					astrsfaDigest = lEncUtil.getHMAC(astrResponseData, astrFileName, strMerchantId);

					if (strDigest.Equals(astrsfaDigest))
					{
	    				oPgResp.getResponse(astrResponseData);
	    				respcd	=	oPgResp.RespCode;
	    				respmsg	=	oPgResp.RespMessage;
					}
			}
        
            if (oPgResp.RespCode == "0")
            {
                //if (Session["OrderId"] != null || oPgResp.TxnId != null)
                //{
                                     
                //    Dial4Jobz.Models.VasEmpoyerSm vasemployersms = new Dial4Jobz.Models.Repositories.Repository().GetVasEmployerSmsbyId(Convert.ToInt32(oPgResp.TxnId.ToString()));
                //    vasemployersms.Bought = true;
                //    new Dial4Jobz.Models.Repositories.Repository().Save();

                //    Session.Remove("OrderId");
                //} 
                    
                //Mahesh-PlanAct-Jan2
                //else
                    //Response.Redirect("../httpdocs/Views/employer/dashboard?value=activate", false);
                    Response.Redirect("http://www.dial4jobz.in/employer/dashboard?value=activate", true);
             
            }
		    
			Response.Write ("Response code      :" +oPgResp.RespCode);
			Response.Write  ("<br>");
			Response.Write ("\nResponse Message :" + oPgResp.RespMessage);
			Response.Write  ("<br>");
			Response.Write ("\nMerchant Txn Id  :" + oPgResp.TxnId);
			Response.Write  ("<br>");
			Response.Write ("\nEpg Txn Id		:" + oPgResp.EPGTxnId);
			Response.Write  ("<br>");
			Response.Write ("\nAuthId Code		:" + oPgResp.AuthIdCode);
			Response.Write  ("<br>");
			Response.Write ("RRN			    :" + oPgResp.RRN);
			Response.Write  ("<br>");
			Response.Write ("CVRESP Code	    :" + oPgResp.CVRespCode);
			Response.Write  ("<br>");
			Response.Write ("Cookie String	    :" + oPgResp.Cookie);
			Response.Write  ("<br>");
			Response.Write ("FDMS Score		    :" + oPgResp.FDMSScore);
			Response.Write  ("<br>");
			Response.Write ("FDMS Result        :" +oPgResp.FDMSResult);
			}
    </script>
    
    
  </head>
  <body MS_POSITIONING="GridLayout">
      
      <%
          display();
      %>
         
  </body>
</html>
