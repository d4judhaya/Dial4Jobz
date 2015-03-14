<%@ Page language="c#" Debug="true"%>
<%@import namespace="SFA" %>
<%@ Import Namespace="Dial4Jobz.Controllers" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" > 

<html>
  <head>
    <title>Payment Response</title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
    <meta name="CODE_LANGUAGE" content="C#"/>
    <meta name="vs_defaultClientScript" content="JavaScript"/>
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5"/>
    <script language="C#" runat="server">
        public void display()
        {

            PGResponse oPgResp = new PGResponse();
            EncryptionUtil lEncUtil = new EncryptionUtil();
            string respcd = null;
            string respmsg = null;
            string astrResponseData = null;
            string strMerchantId, astrFileName = null;
            string strKey = null;
            string strDigest = null;
            string astrsfaDigest = null;

            strMerchantId = "00004575";
            astrFileName = "d://inetpub//vhosts//dial4jobz.in//httpdocs//Key//00004575.key";

            if (Request.ServerVariables["REQUEST_METHOD"] == "POST")
            {

                astrResponseData = Request.Form["DATA"];
                strDigest = Request.Form["EncryptedData"];
                astrsfaDigest = lEncUtil.getHMAC(astrResponseData, astrFileName, strMerchantId);

                if (strDigest.Equals(astrsfaDigest))
                {
                    oPgResp.getResponse(astrResponseData);
                    respcd = oPgResp.RespCode;
                    respmsg = oPgResp.RespMessage;
                }
            }

            if (oPgResp.RespCode == "0")
            {
                //Response.Redirect("http://www.dial4jobz.in/candidates/candidatesvas/index?value=" + EncryptString(oPgResp.TxnId.ToString()), true);
                Response.Redirect("http://www.dial4jobz.in/candidates/candidatesvas/CandidatesPayment?value=" + EncryptString(oPgResp.TxnId.ToString()), true);

            }
            else
            {
                Response.Redirect("http://www.dial4jobz.in/candidates/candidatesvas/CandidatesPayment?value=" + 0);
                //Response.Redirect("http://www.dial4jobz.in/employer/employervas/payment");
            }

            Response.Write("Response code      :" + oPgResp.RespCode);
            Response.Write("<br>");
            Response.Write("\nResponse Message :" + oPgResp.RespMessage);
            Response.Write("<br>");
            Response.Write("\nMerchant Txn Id  :" + oPgResp.TxnId);
            Response.Write("<br>");
            Response.Write("\nEpg Txn Id		:" + oPgResp.EPGTxnId);
            Response.Write("<br>");
            Response.Write("\nAuthId Code		:" + oPgResp.AuthIdCode);
            Response.Write("<br>");
            Response.Write("RRN			    :" + oPgResp.RRN);
            Response.Write("<br>");
            Response.Write("CVRESP Code	    :" + oPgResp.CVRespCode);
            Response.Write("<br>");
            Response.Write("Cookie String	    :" + oPgResp.Cookie);
            Response.Write("<br>");
            Response.Write("FDMS Score		    :" + oPgResp.FDMSScore);
            Response.Write("<br>");
            Response.Write("FDMS Result        :" + oPgResp.FDMSResult);
        }

    // This method is used to Encrypt
    public static string EncryptString(string string_To_Encrypt)
    {
        try
        {
            Byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(string_To_Encrypt);

            string encryptedString = Convert.ToBase64String(b);

            return encryptedString;
        }
        catch (Exception Ex)
        {
            throw Ex;
        }
    }
    </script>
    
    
  </head>
  <body MS_POSITIONING="GridLayout">
      
      <%
          display();
      %>
         
  </body>
</html>
