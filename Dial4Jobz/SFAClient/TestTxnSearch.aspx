<%@ Page language="c#" Codebehind="TestTxnSearch.aspx.cs" AutoEventWireup="false" Inherits="WebApplication1.TestPostTxnSearch" %>
<%@import namespace="SFA" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>TestTxnSearch</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<script language="C#" runat="server">
		public 	void  onClk_Submit(Object sender, EventArgs e)
		{
			PGSearchResponse  objPGSearchResponse;
			
			
			SFA.Merchant oMerchant=new SFA.Merchant();
			oMerchant.setMerchantTxnSearch( "00004575" ,"20080118" ,"20080118");
			
			SFAClient objSFAClient=new SFAClient("c:\\inetpub\\wwwroot\\SFAClient\\Config\\");
			objPGSearchResponse=objSFAClient.postTxnSearch(oMerchant);
			
			ArrayList pgSearch=objPGSearchResponse.PGResponseObjects;
			 System.Collections.IEnumerator oEnumerator= pgSearch.GetEnumerator();
			 while(oEnumerator.MoveNext())
			 {
				PGResponse oPGResponse=(PGResponse)oEnumerator.Current;
				Response.Write(oPGResponse.RespCode +" | " + oPGResponse.RespMessage+" | " +  oPGResponse.TxnId + " | " + oPGResponse.EPGTxnId + " | " +  oPGResponse.AuthIdCode +  " | " +  oPGResponse.RRN+  " | " +  oPGResponse.CVRespCode+  "<br>");
				
			 }
		
			Response.Write( "Response Code:" + objPGSearchResponse.RespCode +"<br>");
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
						<asp:button id="TxnSearch" text="TXnSearch " onClick="onClk_Submit" runat="server"></asp:button>
					</td>
				</tr>
			</table>
			<asp:label id="responselbl" runat="server" />
		</form>
	</body>
</HTML>
