<%@ Page language="c#" Codebehind="TestMoto.aspx.cs" AutoEventWireup="false" Inherits="WebApplication1.WebForm2" %>
<%@import namespace="SFA" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>TestMoto</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<script language="C#" runat="server">
		public string getRemoteAddr() 
		{		
			string UserIPAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
			return UserIPAddress;
		}
		
		public string  getSecureCookie(HttpRequest Request) 
		{		
			HttpCookie secureCookie = Request.Cookies["vsc"];
			if(secureCookie!=null)
			{
				return secureCookie.Value;
			}
			else{
				return "";
			}
		}

		public void setCookies()
		{
			Response.Cookies ["vsc"].Value = "CookieTest";			
			Response.Cookies ["vsc"].Expires = DateTime.Now.AddMonths(5);					
		}
		
		public 	void  SubmitRequest()
		{			
			PGResponse oPgResp			 = new PGResponse();
			CustomerDetails oCustomer	 = new CustomerDetails();
			SessionDetail oSessionDetail = new SessionDetail();
			AirLineTransaction oAirLine  = new AirLineTransaction();
			MerchanDise oMerchanDise     = new MerchanDise();
			
			SFA.CardInfo objCardInfo	 = new SFA.CardInfo();						
			SFA.Merchant objMerchant     = new SFA.Merchant();
		    		
			ShipToAddress objShipToAddress	= new ShipToAddress();
			BillToAddress oBillToAddress	= new BillToAddress();
			ShipToAddress oShipToAddress	= new ShipToAddress();
			MPIData objMPI					= new MPIData();			
			PGReserveData oPGreservData		= new PGReserveData();
			Address oHomeAddress			= new Address();
			Address oOfficeAddress			= new Address();
			
			CardPresentTxn oCardPresentTxn  = new CardPresentTxn();
			
			// For getting unique MerchantTxnID 
			// Only for testing purpose. 
			// In actual scenario the merchant has to pass his transactionID
			DateTime oldTime=new DateTime(1970,01,01,00,00,00);			
			DateTime currentTime=DateTime.Now;
			TimeSpan structTimespan=currentTime-oldTime;		
			string lMrtTxnID = ((long)structTimespan.TotalMilliseconds).ToString();
						
			//Setting Merchant Details
			objMerchant.setMerchantDetails("00004575", "00004575", "00004575", "10.10.10.167", lMrtTxnID,"",null,null,"INR","","req.Sale","3000","GMT+05:30","","","","","");
			
			// Setting Card Details
			objCardInfo.setCardDetails ("VISA","4335900000120054","111","2012","12","Tester","CREDI");
			
			// Setting BillingAddress
			oBillToAddress.setAddressDetails ("CID","MahaLakshmi","Aline1","Aline2","Aline3","Pune","MH","48927489","IND","tester@opussoft.com");
			
			// Setting ShippingAddress
			oShipToAddress.setAddressDetails ("Add1","Add2","Add3","City","State","443543","IND","tester@opussoft.com");
			
			// Setting MPI ResponseDetails (in case merchant connects to mpi, the response from MPI needs to be filled appropriately here)
			objMPI.setMPIResponseDetails ("05","NTBlZjRjMThjMjc1NTUxYzk1MTY=","Y","AAAAAAAAAAAAAAAAAAAAAAAAAAA=",null,"1000","356");
						
			// Setting Name home/office Address Details 
			// Order of Parameters =>        AddLine1, AddLine2,      AddLine3,   City,   State ,  Zip,          Country, Email id
			oHomeAddress.setAddressDetails ("2Sandeep","UttamCorner","Chinchwad","Pune", "state", "4385435873", "IND",   "test@test.com");
			oOfficeAddress.setAddressDetails ("2Opus","MayFairTowers","Wakdewadi","Pune","state","4385435873","IND","test@test.com");

			// Setting  Customer Details 
			// Order of Parameters =>  First Name,LastName ,Office Address Object,Home Address Object,Mobile No,RegistrationDate, flag for matching bill to address and ship to address
			oCustomer.setCustomerDetails ("Sandeep","patil", oOfficeAddress, oHomeAddress,"9423203297","13-06-2007","Y");

			// Setting Session Details         
			// Order of Parameters => Remote Address, Cookies Value            Browser Country,Browser Local Language,Browser Local Lang Variant,Browser User Agent
			oSessionDetail.setSessionDetails (getRemoteAddr(),"","",Request.ServerVariables["HTTP_ACCEPT_LANGUAGE"],"",Request.ServerVariables ["HTTP_USER_AGENT"]);

			// Setting MerchanDise Details 
			// (In case Airline Transaction Details is filled, set oMerchanDise to null)
			// Order of Parameters =>  Item Purchased,Quantity,Brand,ModelNumber,Buyers Name,flag value for matching CardName and BuyerName
			oMerchanDise.setMerchanDiseDetails ("Computer","2","Intel","P4","Sandeep Patil","Y");

			// Setting AirLine Transaction Details  
			// (In case Merchandise Details is filled, set oAirLine to null)
			// Order of Parameters => Booking Date,FlightDate,Flight Time,Flight Number,Passenger Name,Number Of Tickets,flag for matching card name and customer name,PNR,sector from,sector to'
			oAirLine.setAirLineTransactionDetails ("10-06-2007", "22-06-2007","13:20","119", "Sandeep", "1",  "Y", "25c","Pune","Mumbai");
			
			// Setting Card Present Transaction Details  			
			// Order of Parameters => Track2, TerminalId'
			oCardPresentTxn.setCardPresentTxnDetails ("4335900000120054=0901126664","12345679");
			

			// Creating SFAClient instance (Pass the log configuration file with full path)
			SFAClient objSFAClient = new SFAClient("c:\\inetpub\\wwwroot\\SFAClient\\Config\\");
			
			// Call postMOTO to send transaction request
		    	oPgResp = objSFAClient.postMOTO(objCardInfo, objMPI, objMerchant, oBillToAddress, oShipToAddress, oPGreservData, oCustomer, oSessionDetail, oAirLine, oMerchanDise,oCardPresentTxn);
								
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
	</HEAD>
	<BODY bgColor="#ffccff">
		<%
          SubmitRequest();
        %>
	</BODY>
</HTML>
