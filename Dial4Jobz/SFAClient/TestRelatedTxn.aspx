<%@import namespace="SFA" %>
<%@ Page language="c#" Codebehind="TestRelatedTxn.aspx.cs" AutoEventWireup="false" Inherits="WebApplication1.TestPostRelatedTxn" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>TestPostRelatedTxn</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<script language="C#" runat="server">
		public 	void  onClk_Submit(Object sender, EventArgs e)
		{
			PGResponse  objPGResponse=new PGResponse();
			
			
			SFA.Merchant oMerchant=new SFA.Merchant();

			// For getting unique MerchantTxnID 
			// Only for testing purpose. 
			// In actual scenario the merchant has to pass his transactionID
			DateTime oldTime=new DateTime(1970,01,01,00,00,00);			
			DateTime currentTime=DateTime.Now;
			TimeSpan structTimespan=currentTime-oldTime;		
			string lMrtTxnID = ((long)structTimespan.TotalMilliseconds).ToString();
		    //setting MerchantRelatedTxnDetails
		    // // Order of Parameters =>            MerchantID, Vendor,    Partner,  MerchantTxnID,RootTxnSysRefNum,  RootPNRef,   RootAuthCode, CurrCode, MessageType, Amount, GMTTimeOffset, Ext1, Ext2, Ext3, Ext4,Ext5
			oMerchant.setMerchantRelatedTxnDetails ("00004575","00004575","00004575", lMrtTxnID, "200801180981895", "000000058925", "058925", "INR", "req.Refund","50","GMT+05:30","Ext1","Ext2","Ext3","Ext4","Ext5");
			
			
					
			SFAClient objSFAClient=new SFAClient("c:\\inetpub\\wwwroot\\SFAClient\\Config\\");
			objPGResponse=objSFAClient.postRelatedTxn(oMerchant);
			
					

			Response.Write("Response code is        :"+ objPGResponse.RespCode+"<br>");
			Response.Write("Response Message is     :"+ objPGResponse.RespMessage+"<br>");
			Response.Write("Response TxnId is       :"+ objPGResponse.TxnId+"<br>");
			Response.Write("Response EPGTxnId is    :"+ objPGResponse.EPGTxnId+"<br>");
			Response.Write("Response CVRespCode is  :"+ objPGResponse.CVRespCode+"<br>");
			
						
		}
		</script>
	</HEAD>
	<body MS_POSITIONING="GridLayout">
		<form id="Form1" method="post" runat="server">
		<br><br>
		<table align="center" width="70%" border="0">
				
				<tr>
					<td colspan="2" align="center">
						<asp:button id="rlttxn" text="RelatedTXN " onClick="onClk_Submit" runat="server"></asp:button>
					</td>
				</tr>
			</table>
			
			<br>
			<br>
			<br>
			<br>
			<asp:label id="responselbl" runat="server" />
		<!--	Response Code is &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:label id="responselbl1" runat="server" /><br>
			Message &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:label id="responselbl2" runat="server" /><br>
			Txn id &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:label id="responselbl3" runat="server" /><br>
			ePGTxnId 
			&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:label id="responselbl4" runat="server" /><br>
			CVRespCode 
			&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:label id="responselbl5" runat="server" /><br>
		-->
		</form>
	</body>
</HTML>
