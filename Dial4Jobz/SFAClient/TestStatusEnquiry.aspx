<%@ Page language="c#" Codebehind="TestStatusEnquiry.aspx.cs" AutoEventWireup="false" Inherits="WebApplication1.TesPostStatusEnquiry" %>
<%@import namespace="SFA" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>TesPostStatusEnquiry</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<script language="C#" runat="server">
	  public 	void  onClk_Submit(Object sender, EventArgs e)
		{
			PGSearchResponse  objPGSearchResponse=new PGSearchResponse();
			SFA.Merchant oMerchant=new SFA.Merchant();
			oMerchant.setMerchantOnlineInquiry( "00004575", "1200692569500");
			
			
			SFAClient objSFAClient=new SFAClient("c:\\inetpub\\wwwroot\\SFAClient\\Config\\");
			objPGSearchResponse=objSFAClient.postStatusEnquiry(oMerchant);
			
			ArrayList pgSearch=objPGSearchResponse.PGResponseObjects;
			System.Collections.IEnumerator oEnumerator= pgSearch.GetEnumerator();
			while(oEnumerator.MoveNext())
			{
				PGResponse oPGResponse=(PGResponse)oEnumerator.Current;
				Response.Write (oPGResponse.RespCode +" | " + oPGResponse.RespMessage+" | " +  oPGResponse.TxnId + " | " + oPGResponse.EPGTxnId + " | " + oPGResponse.AuthIdCode +  " | " +  oPGResponse.RRN+  " | " +  oPGResponse.CVRespCode+  "\n");
				
			}
			string response=objPGSearchResponse.RespMessage;
      Response.Write( "<br>" );
			Response.Write( " Response Code:" + objPGSearchResponse.RespCode +"<br>");
			Response.Write( " Response Message:" +objPGSearchResponse.RespMessage + "<br>");
	
		}
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
			<br><br>
			<table align="center" width="70%" border="0">
				
				<tr>
					<td colspan="2" align="center">
						<asp:button id="stsenq" text=" StatusEnquiry " onClick="onClk_Submit" runat="server"></asp:button>
					</td>
				</tr>
			</table>
			
		</form>
	</body>
</HTML>
