<%@import namespace="SFA" %>
<%@ Page language="c#" Codebehind="TestSSl.aspx.cs" AutoEventWireup="false" Inherits="WebApplication1.TestSSl" %>
<%--<%@ Page language="c#" Codebehind="~/SFAClient/TestSSl.aspx" AutoEventWireup="false" Inherits="WebApplication1.TestSSl" %>--%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
  <HEAD>
		<title>TestSSL</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="C#" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
		<script language="C#" runat="server">
		public string getRemoteAddr()
		{
			string UserIPAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
			if(UserIPAddress==null){
			UserIPAddress= Request.ServerVariables["REMOTE_ADDR"];
			}
			return UserIPAddress;
		}
		public string  getSecureCookie(HttpRequest Request){
		
			HttpCookie secureCookie = Request.Cookies["vsc"];
				if(secureCookie!=null)
				{
				return secureCookie.ToString();
				}
				else{
				return "";
				}
	
		
		}
		
		public 	void  onClk_Submit(Object sender, EventArgs e)
		{
			PGResponse objPGResponse=new PGResponse();
			CustomerDetails oCustomer=new CustomerDetails();
			SessionDetail oSession=new SessionDetail();
			AirLineTransaction oAirLine=new AirLineTransaction();
			MerchanDise oMerchanDise=new MerchanDise();
			
			SFA.CardInfo objCardInfo=new SFA.CardInfo();			
			
			SFA.Merchant objMerchant=new SFA.Merchant();
			
			ShipToAddress objShipToAddress=new ShipToAddress();
			BillToAddress oBillToAddress=new BillToAddress();
			ShipToAddress oShipToAddress=new ShipToAddress();
			MPIData objMPI=new MPIData();			
			PGReserveData oPGreservData=new PGReserveData();
			Address oHomeAddress=new Address();
			Address oOfficeAddress=new Address();
			// For getting unique MerchantTxnID 
			// Only for testing purpose. 
			// In actual scenario the merchant has to pass his transactionID
			DateTime oldTime=new DateTime(1970,01,01,00,00,00);			
			DateTime currentTime=DateTime.Now;
			TimeSpan structTimespan=currentTime-oldTime;		
            //string lMrtTxnID = ((long)structTimespan.TotalMilliseconds).ToString();

            string lMrtTxnID = "";

            if (Request.QueryString["orderid"] != null && !string.IsNullOrEmpty(Request.QueryString["orderid"].ToString()))
                lMrtTxnID = Request.QueryString["orderid"].ToString();
            else
                lMrtTxnID = ((long)structTimespan.TotalMilliseconds).ToString();

            string ResponseUrl = "";

            if (Request.QueryString["ResponseUrl"] != null && !string.IsNullOrEmpty(Request.QueryString["ResponseUrl"].ToString()))
                ResponseUrl = Request.QueryString["ResponseUrl"].ToString();
            else
                ResponseUrl = "http://www.dial4jobz.in//SFAClient//SFAResponse.aspx";
			
            //string amount = Request.QueryString[0];
            string amount = Request.QueryString["amount"];
            
            Session["VASAmount"] = amount;
            
			//Setting Merchant Details
            objMerchant.setMerchantDetails("00004575", "00004575", "00004575", "", lMrtTxnID, "Ord123", ResponseUrl, "POST", "INR", "INV123", "req.Sale", amount, "GMT+05:30", "ASP.NET64", "true", "ASP.NET64", "ASP.NET64", "ASP.NET64");
			
			// Setting BillToAddress Details
			oBillToAddress.setAddressDetails( "CID","Maha Lakshmi","Aline 1","Aline2","Aline3","Pune","MH","48927489","IND","tester@opussoft.com");
			
			// Setting ShipToAddress Details
			oShipToAddress.setAddressDetails ("$23@#|","<script>","Add 3","City","State","443543","IND","tester@opussoft.com");
			
			//Setting MPI datails.
			//objMPI.setMPIRequestDetails ("1000","INR10.00","356","2","2 shirts","","","","0","","image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/vnd.ms-powerpoint, application/vnd.ms-excel, application/msword, application/x-shockwave-flash, */*","Mozilla/4.0 (compatible; MSIE 5.5; Windows NT 5.0)");
						
			// Setting Name home/office Address Details 
			// Order of Parameters =>        AddLine1, AddLine2,      AddLine3,   City,   State ,  Zip,          Country, Email id
			oHomeAddress.setAddressDetails( "2Sandeep","Uttam Corner","Chinchwad","Pune","state","4385435873","IND", "test@test.com");
			
			// Order of Parameters =>        AddLine1, AddLine2,      AddLine3,   City,   State ,  Zip,          Country, Email id
			oOfficeAddress.setAddressDetails("2Opus","MayFairTowers"	,"Wakdewadi","Pune","state","4385435873","IND", "test@test.com");

			// Stting  Customer Details 
			// Order of Parameters =>  First Name,LastName ,Office Address Object,Home Address Object,Mobile No,RegistrationDate, flag for matching bill to address and ship to address 
			oCustomer.setCustomerDetails("Sandeep","patil",oOfficeAddress,oHomeAddress,"9423203297","13-06-2007","Y");

			//Setting Merchant Dise Details 
			// Order of Parameters =>       Item Purchased,Quantity,Brand,ModelNumber,Buyers Name,flag value for matching CardName and BuyerName
			oMerchanDise.setMerchanDiseDetails("Computer", "2","Intel","P4","Sandeep Patil","Y");

			//Setting  Session Details        
			// Order of Parameters =>     Remote Address, Cookies Value            Browser Country,Browser Local Language,Browser Local Lang Variant,Browser User Agent'
			oSession.setSessionDetails (getRemoteAddr(), getSecureCookie(Request)  ,"",Request.ServerVariables["HTTP_ACCEPT_LANGUAGE"], "",Request.ServerVariables ["HTTP_USER_AGENT"]);

			//Settingr AirLine Transaction Details  
			//Order of Parameters =>               Booking Date,FlightDate,Flight   Time,Flight Number,Passenger Name,Number Of Tickets,flag for matching card name and customer name,PNR,sector from,sector to'
			oAirLine.setAirLineTransactionDetails ("10-06-2007", "22-06-2007","13:20","119", "Sandeep", "1",  "Y", "25c","Pune", "Mumbai");

			SFAClient objSFAClient=new SFAClient("d:\\inetpub\\vhosts\\dial4jobz.in\\httpdocs\\Config\\");
			objPGResponse=objSFAClient.postSSL( objMPI, objMerchant, oBillToAddress, oShipToAddress,oPGreservData,oCustomer,oSession,oAirLine,oMerchanDise);

			if (objPGResponse.RedirectionUrl!= "" &objPGResponse.RedirectionUrl!=null)
			{
			string strResponseURL=objPGResponse.RedirectionUrl;			
			Response.Redirect(strResponseURL);
			}
			else
			{
			Response.Write( "Response Code:" +objPGResponse.RespCode);
			Response.Write( "Response message:"+objPGResponse.RespMessage);
			}

            //if (objPGResponse.RespCode == "0")
            //{
            //    Dial4Jobz.Controllers.EmployerVasController empvas = new Dial4Jobz.Controllers.EmployerVasController();
            //    empvas.OnSuccessOfPayment(0);
            //}

			//responselbl.Text=response;
		
  		}
		</script>
</HEAD>
	<body bgColor="#99ccff" MS_POSITIONING="GridLayout">
		<form id="moto" method="post" runat="server">
			<input type="hidden" name="isFormSubmitted" runat="server" ID="Hidden1">
			<br><br><br><br><br><br><br><br>
			<table align="center" width="70%" border="0">
				
				<tr>
					<td colspan="2" align="center">
						<asp:button id="paybutton" text=" SSL " OnLoad="onClk_Submit" runat="server"></asp:button>
					</td>
				</tr>
			</table>
			<asp:label id="responselbl" runat="server" />
		</form>
	</body>
</HTML>
