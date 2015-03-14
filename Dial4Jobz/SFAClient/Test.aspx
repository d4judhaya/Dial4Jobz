<%@ Page language="c#"%>
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
			Response.Write ("Response code      :" );
			Response.Write  ("<br>");
			Response.Write ("\nResponse Message :" );
			Response.Write  ("<br>");
			Response.Write ("\nMerchant Txn Id  :" );
			Response.Write  ("<br>");
			Response.Write ("\nEpg Txn Id		:" );
			Response.Write  ("<br>");
			Response.Write ("\nAuthId Code		:" );
			Response.Write  ("<br>");
			Response.Write ("RRN			    :" );
			Response.Write  ("<br>");
			Response.Write ("CVRESP Code	    :" );
			}
    </script>
    
    
  </head>
  <body>
      
      <%
          display();
      %>
         
  </body>
</html>
