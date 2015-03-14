<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Dial4Jobz.Models.OrderDetail>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head >
    <title>Invoice</title>
    <script type = "text/javascript">        
        function PrintPanel() {
            var panel = document.getElementById("pnlContents");
            //var printWindow = window.open('', '', 'height=400,width=800');
            var printWindow = window.open('', '', 'height=400,width=800,location=0');
            printWindow.document.write('<html><head><title>DIV Contents</title>');
            printWindow.document.write('</head><body >');
            printWindow.document.write(panel.innerHTML);
            printWindow.document.write('</body></html>');
            printWindow.document.close();
            setTimeout(function () {
                printWindow.print();
            }, 500);
            return false;
        }
    </script>

    <style type="text/css">
        #fancybox-wrap
        {
           width:740px !important;
        }
        #fancybox-inner
        {
           width:720px !important;
        }
        
        
        
<!--
 /* Font Definitions */
 @font-face
	{font-family:"Cambria Math";
	panose-1:2 4 5 3 5 4 6 3 2 4;}
@font-face
	{font-family:Calibri;
	panose-1:2 15 5 2 2 2 4 3 2 4;}
@font-face
	{font-family:Tahoma;
	panose-1:2 11 6 4 3 5 4 4 2 4;}
 /* Style Definitions */
 p.MsoNormal, li.MsoNormal, div.MsoNormal
	{margin-top:0in;
	margin-right:0in;
	margin-bottom:10.0pt;
	margin-left:0in;
	line-height:115%;
	font-size:11.0pt;
	font-family:"Calibri","sans-serif";}
p.MsoAcetate, li.MsoAcetate, div.MsoAcetate
	{mso-style-link:"Balloon Text Char";
	margin:0in;
	margin-bottom:.0001pt;
	font-size:8.0pt;
	font-family:"Tahoma","sans-serif";}
span.apple-converted-space
	{mso-style-name:apple-converted-space;}
span.normaltext2
	{mso-style-name:normaltext2;}
span.bold1
	{mso-style-name:bold1;}
span.BalloonTextChar
	{mso-style-name:"Balloon Text Char";
	mso-style-link:"Balloon Text";
	font-family:"Tahoma","sans-serif";}
.MsoPapDefault
	{margin-bottom:10.0pt;
	line-height:115%;}
@page Section1
	{size:8.5in 11.0in;
	margin:1.0in 1.0in 1.0in 1.0in;}
div.Section1
	{page:Section1;}
-->
</style>

</head>
<body>
<% Dial4Jobz.Models.Repositories.VasRepository _vasrepository= new Dial4Jobz.Models.Repositories.VasRepository(); %>
<% Dial4Jobz.Models.VasPlan getplan = _vasrepository.GetVasPlanByPlanId(Convert.ToInt32(Model.PlanId)); %>
    <div id="pnlContents">
    <div class=Section1>

<div align=center>

<table class=MsoNormalTable border=1 cellspacing=0 cellpadding=0 width=680
 style='width:510.0pt;background:white;border-collapse:collapse;border:none'>
 <tr>
  <td style='border:none;padding:3.75pt 3.75pt 3.75pt 3.75pt'>
  <table class=MsoNormalTable border=1 cellspacing=0 cellpadding=0 width="100%"
   style='width:100.0%;border-collapse:collapse;border:none'>
   <tr>
    <td style='border:none;padding:1.5pt 1.5pt 1.5pt 1.5pt'>
    <p class=MsoNormal style='margin-bottom:0in;margin-bottom:.0001pt;
    line-height:normal'><span style='font-size:10.0pt'><img width=251
    height=98 id="Picture 3" src="../../Content/Images/logo_with_number.jpg"
    alt="D4J Permenant logo 291111"></span></p>
    </td>
    <td valign=top style='border:none;padding:1.5pt 1.5pt 1.5pt 1.5pt'>
    <p class=MsoNormal align=right style='margin-bottom:0in;margin-bottom:.0001pt;
    text-align:right;line-height:normal'><b><span style='font-size:16.0pt;
    font-family:"Arial","sans-serif";color:#0D11C5;background:white'>D</span></b><b><span
    style='font-size:14.0pt;font-family:"Arial","sans-serif";color:#0D11C5;
    background:white'>ial4Jobz India Pvt Ltd</span></b><b><span style='font-size:19.5pt;
    font-family:"Times New Roman","serif";color:black'>.</span></b><span
    style='font-size:19.5pt;font-family:"Times New Roman","serif";color:black'>&nbsp;</span><span
    style='font-size:8.5pt;font-family:"Times New Roman","serif";color:black'><br/>
    </span><span style='font-size:7.0pt;font-family:"Arial","sans-serif";
    color:#0D11C5;background:white'>#32,3rd Cross Street Ext.AGS colony(SEA
    SIDE)</span><span style='font-family:"Arial","sans-serif";color:#3333FF;
    background:white'>,</span></p>

    <p class=MsoNormal align=right style='margin-bottom:0in;margin-bottom:.0001pt;
    text-align:right;line-height:normal'><span style='font-family:"Arial","sans-serif";
    color:#0D11C5;background:white'> </span><span style='font-size:7.0pt;
    font-family:"Arial","sans-serif";color:#0D11C5;background:white'>Kottivakkam</span><span
    class=apple-converted-space><span style='font-size:10.5pt;font-family:"Arial","sans-serif";
    color:red;background:white'>&nbsp;</span></span><span style='font-size:
    10.5pt;font-family:"Arial","sans-serif";color:red;background:white'>|<span
    class=apple-converted-space>&nbsp;</span></span><span style='font-size:
    7.0pt;font-family:"Arial","sans-serif";color:#0D11C5;background:white'>Chennai
    - 600041</span><span style='font-size:8.5pt;font-family:"Times New Roman","serif";
    color:black'><br/>
    </span><span style='font-size:7.0pt;font-family:"Times New Roman","serif";
    color:#0D11C5'>www.dial4jobz.com</span></p>
    </td>
   </tr>
   <tr>
    <td colspan=2 style='border:none;padding:1.5pt 1.5pt 1.5pt 1.5pt'>
    <table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0
     width="100%" style='width:100.0%;border-collapse:collapse'>
     <tr style='height:30.0pt'>
      <td style='padding:0in 0in 0in 0in;height:30.0pt'>
      <p class=MsoNormal align=center style='margin-bottom:0in;margin-bottom:
      .0001pt;text-align:center;line-height:normal'><span style='font-size:
      13.5pt;font-family:"Times New Roman","serif";color:black'>INVOICE</span></p>
      </td>
     </tr>
    </table>
    </td>
   </tr>
  </table>
  <p class=MsoNormal style='margin-bottom:0in;margin-bottom:.0001pt;line-height:
  16.5pt'><span style='font-size:9.0pt;font-family:"Arial","sans-serif";
  color:black;display:none'>&nbsp;</span></p>
  <table class=MsoNormalTable border=1 cellspacing=0 cellpadding=0 width="100%"
   style='width:100.0%;border-collapse:collapse;border:none'>
   <tr>
    <td rowspan=2 valign=top style='width:50%; border:none;padding:1.5pt 1.5pt 1.5pt 1.5pt'>
    <p class=MsoNormal style='margin-bottom:0in;margin-bottom:.0001pt;
    line-height:normal'><b><span style='font-size:9.0pt;font-family:"Times New Roman","serif"'>Billed
    To: &nbsp;<%: Model.OrderMaster.Consultante.Name%></span></b><b><span style='font-size:9.0pt;
    font-family:"Times New Roman","serif"'><br/>
    </span></b><b><span style='font-size:9.0pt;font-family:"Times New Roman","serif"'><%: Model.OrderMaster.Consultante.Address %>
    &nbsp;</span></b><b><span style='font-size:9.0pt;font-family:"Times New Roman","serif"'><br>
    </span></b><b><span style='font-size:9.0pt;font-family:"Times New Roman","serif"'><%: Model.OrderMaster.Consultante.Pincode %>    </span></b>
      <br />
     <span style='font-size:9.0pt;font-family:"Times New Roman","serif"'><%: Model.OrderMaster.Consultante.MobileNumber %>
    </p>
    </td>
    <td style='border:none;padding:1.5pt 1.5pt 1.5pt 1.5pt'>
    <p class=MsoNormal style='margin-bottom:0in;margin-bottom:.0001pt;
    line-height:normal'><b><span style='font-size:9.0pt;font-family:"Times New Roman","serif"'>Invoice
    No. :</span></b><span style='font-size:9.0pt;font-family:"Times New Roman","serif"'>&nbsp;</span><span
    style='font-size:9.0pt;font-family:"Times New Roman","serif"'>   
    <%if (Model.OrderMaster.InvoiceId != null)
      { %>
        <%: Model.OrderMaster.Invoice.InvoiceNo %>
    <%} else { %>
        <%: Model.OrderId %>
    <%} %>
    </td>
   </tr>
   <tr>
    <td style='border:none;padding:1.5pt 1.5pt 1.5pt 1.5pt'>
    <p class=MsoNormal style='margin-bottom:0in;margin-bottom:.0001pt;
    line-height:normal'><span style='font-size:8.5pt;font-family:"Times New Roman","serif";
    color:black'>Date :</span><span style='font-size:9.0pt;font-family:"Times New Roman","serif"'>&nbsp;<%if (Model.OrderMaster.OrderDate != null)
                                                                                                          { %> <%: Model.OrderMaster.OrderDate.Value.ToString("dd-MM-yyyy")%> <%}
                                                                                                          else
                                                                                                          { %><%: Model.ActivationDate.Value.ToString("dd-MM-yyyy")%> <%} %>&nbsp;</span><span
    style='font-size:9.0pt;font-family:"Times New Roman","serif"'><br>
    </span><span style='font-size:8.5pt;font-family:"Times New Roman","serif";
    color:black'>Order No.:</span><span style='font-size:9.0pt;font-family:
    "Times New Roman","serif"'>&nbsp;<%: Model.OrderId %>&nbsp;</span><span
    style='font-size:9.0pt;font-family:"Times New Roman","serif"'><br>
    </span>
    <% var orderpayment = Model.OrderMaster.OrderPayments.Where(op => op.OrderId == Model.OrderId).FirstOrDefault();

       if (orderpayment != null)
       { %> 
    <span style='font-size:8.5pt;font-family:"Times New Roman","serif";
    color:black'>Mode of Payment:</span><span style='font-size:9.0pt;
    font-family:"Times New Roman","serif"'>&nbsp;<b><%: ((Dial4Jobz.Models.Enums.PaymentMode)orderpayment.PaymentMode) %></b>&nbsp;</span>
    <% } %>
    <span
    style='font-size:9.0pt;font-family:"Times New Roman","serif"'><br>
    </span><span style='font-size:8.5pt;font-family:"Times New Roman","serif";
    color:black'>Customer ID: <%: Model.OrderMaster.Consultante.Id %></span></p>
    </td>
   </tr>
  </table>
  <p class=MsoNormal style='margin-bottom:0in;margin-bottom:.0001pt;line-height:
  16.5pt'><span style='font-size:9.0pt;font-family:"Arial","sans-serif";
  color:black;display:none'>&nbsp;</span></p>
  <table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width="100%"
   style='width:100.0%;border-collapse:collapse'>
   <tr>
    <td style='padding:0in 0in 0in 0in'>
    <table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width=670
     style='width:502.5pt;border-collapse:collapse'>
     <tr>
      <td style='padding:.75pt .75pt .75pt .75pt'>
      <div align=center>
      <table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0
       width="100%" style='width:100.0%;border-collapse:collapse'>
       <tr style='height:22.5pt'>
        <td width="9%" style='width:9.94%;border:solid #E1E1E3 1.0pt;
        background:#EBEBEB;padding:7.5pt 3.75pt 7.5pt 3.75pt;height:22.5pt'>
        <p class=MsoNormal align=center style='margin-bottom:0in;margin-bottom:
        .0001pt;text-align:center;line-height:normal'><b><span
        style='font-size:8.5pt;font-family:"Times New Roman","serif";
        color:black'>S No.</span></b></p>
        </td>
        <td width="33%" style='width:33.94%;border:solid #E1E1E3 1.0pt;
        border-left:none;background:#EBEBEB;padding:7.5pt 3.75pt 7.5pt 3.75pt;
        height:22.5pt'>
        <p class=MsoNormal style='margin-bottom:0in;margin-bottom:.0001pt;
        line-height:normal'><b><span style='font-size:8.5pt;font-family:"Times New Roman","serif";
        color:black'>Product</span></b></p>
        </td>
        <td width="7%" style='width:7.94%;border:solid #E1E1E3 1.0pt;
        border-left:none;background:#EBEBEB;padding:7.5pt 3.75pt 7.5pt 3.75pt;
        height:22.5pt'>
        <p class=MsoNormal align=center style='margin-bottom:0in;margin-bottom:
        .0001pt;text-align:center;line-height:normal'><b><span
        style='font-size:8.5pt;font-family:"Times New Roman","serif";
        color:black'>Term</span></b></p>
        </td>
        <td width="7%" style='width:7.94%;border:solid #E1E1E3 1.0pt;
        border-left:none;background:#EBEBEB;padding:7.5pt 3.75pt 7.5pt 3.75pt;
        height:22.5pt'>
        <p class=MsoNormal align=center style='margin-bottom:0in;margin-bottom:
        .0001pt;text-align:center;line-height:normal'><b><span
        style='font-size:8.5pt;font-family:"Times New Roman","serif";
        color:black'>Rate (INR.)</span></b></p>
        </td>
        <td width="9%" style='width:9.94%;border:solid #E1E1E3 1.0pt;
        border-left:none;background:#EBEBEB;padding:7.5pt 3.75pt 7.5pt 3.75pt;
        height:22.5pt'>
        <p class=MsoNormal align=center style='margin-bottom:0in;margin-bottom:
        .0001pt;text-align:center;line-height:normal'><b><span
        style='font-size:8.5pt;font-family:"Times New Roman","serif";
        color:black'>Disc(INR.)</span></b></p>
        </td>
        <td width="9%" style='width:9.94%;border:solid #E1E1E3 1.0pt;
        border-left:none;background:#EBEBEB;padding:7.5pt 3.75pt 7.5pt 3.75pt;
        height:22.5pt'>
        <p class=MsoNormal align=center style='margin-bottom:0in;margin-bottom:
        .0001pt;text-align:center;line-height:normal'><b><span
        style='font-size:8.5pt;font-family:"Times New Roman","serif";
        color:black'>Amount(INR.)</span></b></p>
        </td>
        <td width="10%" style='width:10.38%;border:solid #E1E1E3 1.0pt;
        border-left:none;background:#EBEBEB;padding:7.5pt 3.75pt 7.5pt 3.75pt;
        height:22.5pt'>
        <p class=MsoNormal align=center style='margin-bottom:0in;margin-bottom:
        .0001pt;text-align:center;line-height:normal'><b><span
        style='font-size:8.5pt;font-family:"Times New Roman","serif";
        color:black'>S.Tax(INR.)</span></b></p>
        </td>
        <td width="9%" style='width:9.98%;border:solid #E1E1E3 1.0pt;
        border-left:none;background:#EBEBEB;padding:7.5pt 3.75pt 7.5pt 3.75pt;
        height:22.5pt'>
        <p class=MsoNormal align=center style='margin-bottom:0in;margin-bottom:
        .0001pt;text-align:center;line-height:normal'><b><span
        style='font-size:8.5pt;font-family:"Times New Roman","serif";
        color:black'>Total (INR.)</span></b></p>
        </td>
        <td style='border:none;padding:0in 0in 0in 0in' width="9%"><p class='MsoNormal'>&nbsp;</td>
       </tr>
       <tr>
        <td width="9%" style='width:9.94%;padding:1.5pt 1.5pt 1.5pt 1.5pt'></td>
        <td width="33%" style='width:33.94%;padding:1.5pt 1.5pt 1.5pt 1.5pt'></td>
        <td width="7%" style='width:7.94%;padding:1.5pt 1.5pt 1.5pt 1.5pt'></td>
        <td width="7%" style='width:7.94%;padding:1.5pt 1.5pt 1.5pt 1.5pt'></td>
        <td width="9%" style='width:9.94%;padding:1.5pt 1.5pt 1.5pt 1.5pt'></td>
        <td width="10%" style='width:10.38%;padding:1.5pt 1.5pt 1.5pt 1.5pt'></td>
        <td width="9%" style='width:9.98%;padding:1.5pt 1.5pt 1.5pt 1.5pt'></td>
        <td width="9%" style='width:9.94%;padding:1.5pt 1.5pt 1.5pt 1.5pt'></td>
       </tr>
       <tr>
        <td width="9%" style='width:9.94%;border:solid #E1E1E3 1.0pt;
        padding:1.5pt 1.5pt 1.5pt 1.5pt'>
        <p class=MsoNormal align=center style='margin-bottom:0in;margin-bottom:
        .0001pt;text-align:center;line-height:normal'><span style='font-size:
        8.5pt;font-family:"Times New Roman","serif";color:black'>1</span></p>
        </td>
        <td width="33%" style='width:33.94%;border:solid #E1E1E3 1.0pt;
        border-left:none;padding:1.5pt 1.5pt 1.5pt 1.5pt'>
        <p class=MsoNormal style='margin-bottom:0in;margin-bottom:.0001pt;
        line-height:normal'><span style='font-size:8.5pt;font-family:"Times New Roman","serif";
        color:black'>
        <% if (getplan != null)
           { %>
           <%: getplan.PlanName %>
        <%} else { %>
            <%: Model.PlanName%>
        <%} %>
        </span></p>

        </td>
        <td width="7%" style='width:7.94%;border:solid #E1E1E3 1.0pt;
        border-left:none;padding:1.5pt 1.5pt 1.5pt 1.5pt'>
        <p class=MsoNormal align=center style='margin-bottom:0in;margin-bottom:
        .0001pt;text-align:center;line-height:normal'><span style='font-size:
        8.5pt;font-family:"Times New Roman","serif";color:black'>
         <%if (Model.VasPlan != null)
          { %>
              <%: Model.VasPlan.ValidityDays != null ? Model.VasPlan.ValidityDays.Value.ToString() + " days" : ""%> </span></p>
         <%}
           else if (Model.Amount == 336 || Model.Amount == 219)
           { %>
           <%:"7 days"%>
         <%} else { %>
            
         <%} %>
        </td>
        <td width="7%" style='width:7.94%;border:solid #E1E1E3 1.0pt;
        border-left:none;padding:1.5pt 1.5pt 1.5pt 1.5pt'>
        <p class=MsoNormal align=center style='margin-bottom:0in;margin-bottom:
        .0001pt;text-align:center;line-height:normal'><span style='font-size:
        8.5pt;font-family:"Times New Roman","serif";color:black'>
          <% double exactValue = Math.Ceiling((Model.Amount.Value - ((Model.Amount.Value / 112.36) * 12.36))); %>
          <% double amountValue = ((Model.Amount.Value / 112.36) * 12.36); %>
          <% double minusValue = Convert.ToInt32(Model.Amount.Value - amountValue); %>
          <% double ? amount= Model.Amount.Value; %>

        <%if (Model.OrderMaster.InvoiceId != null)
        { %>
            <%:Convert.ToInt32((Model.Amount.Value - ((Model.Amount.Value / 112.36) * 12.36)))%>
        <%} else { %>
              <%:amount %>
        <%} %>
        </span></p>
        </td>
        <td width="9%" style='width:9.94%;border:solid #E1E1E3 1.0pt;
        border-left:none;padding:1.5pt 1.5pt 1.5pt 1.5pt'>
        <p class=MsoNormal align=center style='margin-bottom:0in;margin-bottom:
        .0001pt;text-align:center;line-height:normal'><span style='font-size:
        8.5pt;font-family:"Times New Roman","serif";color:black'>   <% double discountValue = ((exactValue * 25 / 100)); %>
         <%if (Model.OrderMaster.InvoiceId == null)
            { %>
            <%if (Model.DiscountAmount != null)
              { %>
                 <%:Convert.ToInt32(exactValue * 25 / 100)%>
            <%}
              else
              { %>   
                <%: Convert.ToInt32(0)%>
            <%} %>
        <%} else { %>
             <%:"0" %>
        <%} %>
        </span></p>
        </td>
        <td width="10%" style='width:10.38%;border:solid #E1E1E3 1.0pt;
        border-left:none;padding:1.5pt 1.5pt 1.5pt 1.5pt'>
        <p class=MsoNormal align=center style='margin-bottom:0in;margin-bottom:
        .0001pt;text-align:center;line-height:normal'><span style='font-size:
        8.5pt;font-family:"Times New Roman","serif";color:black'>
        
        <% double amounttax = (minusValue - discountValue); %>
        <%if (Model.OrderMaster.InvoiceId == null)
        { %>
        <%if (Model.DiscountAmount != null)
          { %>
            <%:Convert.ToInt32((minusValue - discountValue))%>
        <%}
          else
          { %>  
            <%:Convert.ToInt32(minusValue) %>
        <%} %>
        <%} else { %>
            <%:amount %>
        <%} %>

        </span></p>
        </td>
        <td width="10%" style='width:10.38%;border:solid #E1E1E3 1.0pt;
        border-left:none;padding:1.5pt 1.5pt 1.5pt 1.5pt'>
        <p class=MsoNormal align=center style='margin-bottom:0in;margin-bottom:
        .0001pt;text-align:center;line-height:normal'><span style='font-size:
        8.5pt;font-family:"Times New Roman","serif";color:black'>
       
             <%if (Model.OrderMaster.InvoiceId == null)
                { %>
                  <%--  <%: Convert.ToInt32((Model.Amount.Value/112.36) * 12.36) %>--%>
                    <%if(Model.DiscountAmount!=null) { %>
                        <%:Convert.ToInt32((amounttax) * 12.36 /100) %>
                    <%} else { %>
                        <%: Convert.ToInt32((Model.Amount.Value/112.36) * 12.36) %>
                    <%} %>
          <%} else { %>
             <%:"NIL" %>
          <%} %>
        
        </span></p>
        </td>
        <td width="9%" style='width:9.94%;border:solid #E1E1E3 1.0pt;
        border-left:none;padding:1.5pt 1.5pt 1.5pt 1.5pt'>
        <p class=MsoNormal align=center style='margin-bottom:0in;margin-bottom:
        .0001pt;text-align:center;line-height:normal'><span style='font-size:
        8.5pt;font-family:"Times New Roman","serif";color:black'>
         <%if (Model.OrderMaster.InvoiceId == null)
         { %>
                <%--<%: Model.Amount%>--%>
                <%if (Model.DiscountAmount != null)
                   { %>
                       <%:Model.DiscountAmount%>
                  <%} else { %>
                      <%:Model.Amount%>
                  <%} %>
          <%} else { %>
             <%:amount%>
        <%} %>
        </span></p>
        </td>
        <td style='border:none;padding:0in 0in 0in 0in' width="9%"><p class='MsoNormal'>&nbsp;</td>
       </tr>
       <tr>
        <td width="9%" style='width:9.94%;padding:1.5pt 1.5pt 1.5pt 1.5pt'></td>
        <td width="33%" style='width:33.94%;padding:1.5pt 1.5pt 1.5pt 1.5pt'></td>
        <td width="7%" style='width:7.94%;padding:1.5pt 1.5pt 1.5pt 1.5pt'></td>
        <td width="7%" style='width:7.94%;padding:1.5pt 1.5pt 1.5pt 1.5pt'></td>
        <td width="9%" style='width:9.94%;padding:1.5pt 1.5pt 1.5pt 1.5pt'></td>
        <td width="10%" style='width:10.38%;padding:1.5pt 1.5pt 1.5pt 1.5pt'></td>
        <td width="9%" style='width:9.98%;padding:1.5pt 1.5pt 1.5pt 1.5pt'></td>
        <td width="9%" style='width:9.94%;padding:1.5pt 1.5pt 1.5pt 1.5pt'></td>
       </tr>
       <tr>
        <td width="90%" colspan=7 style='width:90.06%;border:solid #E1E1E3 1.0pt;
        padding:1.5pt 1.5pt 1.5pt 1.5pt'>
        <p class=MsoNormal align=center style='margin-bottom:0in;margin-bottom:
        .0001pt;text-align:center;line-height:normal'><b><span
        style='font-size:9.0pt;font-family:"Times New Roman","serif"'>                                                                                                                                           
        Grand Total        INR.  <%if (Model.DiscountAmount != null)
          { %>
            <%:Model.DiscountAmount%>
        <%} else { %>
            <%: Model.Amount%>
        <%} %>
    
            
        </span></b></p>
        </td>
        <td width="9%" style='width:9.94%;border:solid #E1E1E3 1.0pt;
        border-left:none;padding:1.5pt 1.5pt 1.5pt 1.5pt'></td>
       </tr>
       <tr style='height:30.0pt'>
        <td width="100%" colspan=8 style='width:100.0%;border:solid #E1E1E3 1.0pt;
        border-top:none;padding:1.5pt 1.5pt 1.5pt 1.5pt;height:30.0pt'>
        <p class=MsoNormal style='margin-bottom:0in;margin-bottom:.0001pt;
        line-height:normal'><b><span style='font-size:9.0pt;font-family:"Times New Roman","serif"'>Amount
        in words (INR.):&nbsp;
          <%if (Model.DiscountAmount != null)
          { %>
             <% string words = Dial4Jobz.Helpers.StringHelper.NumbersToWords(Convert.ToInt32(Model.DiscountAmount.Value)); %><%: words%> only
        <% }
          else
          {%>
        <% string words = Dial4Jobz.Helpers.StringHelper.NumbersToWords(Convert.ToInt32(Model.Amount.Value)); %><%: words%> only
        <%} %></span></b></p>
        </td>
       </tr>
       <% if (orderpayment != null && orderpayment.OrderMaster.PaymentStatus==true)
          { %> 
       <tr style='height:30.0pt'>
        <td width="100%" colspan=8 style='width:100.0%;border:solid #E1E1E3 1.0pt;
        border-top:none;padding:1.5pt 1.5pt 1.5pt 1.5pt;height:30.0pt'>
        <p class=MsoNormal style='margin-bottom:0in;margin-bottom:.0001pt;
        line-height:normal'><b><span style='font-size:9.0pt;font-family:"Times New Roman","serif"'>Received payment through <%: ((Dial4Jobz.Models.Enums.PaymentMode)orderpayment.PaymentMode) %> on <%: Model.ActivationDate!=null ? Model.ActivationDate.Value.ToString("dd-MM-yyyy") : "" %></span></b></p>
        </td>
       </tr>
       <% } %>
      </table>
      </div>
      </td>
     </tr>
    </table>
    </td>
   </tr>
  </table>
  </td>
 </tr>
 <tr>
  <td style='border:none;padding:3.75pt 3.75pt 3.75pt 3.75pt'>
  <table class=MsoNormalTable border=1 cellspacing=0 cellpadding=0 width="100%"
   style='width:100.0%;border-collapse:collapse;border:none'>
   <tr>
    <td valign=top style='border:none;padding:1.5pt 1.5pt 1.5pt 1.5pt'>
    <p class=MsoNormal style='margin-bottom:0in;margin-bottom:.0001pt;
    line-height:normal'><span style='font-size:9.0pt;font-family:"Times New Roman","serif"'>PAN
    No. :</span><span style='font-size:9.0pt;font-family:"Times New Roman","serif"'>&nbsp;</span><span
    style='font-size:9.0pt;font-family:"Times New Roman","serif"'>AAFCD1899L<br/>
  <%--  Service Tax Regn.:--%></span><span style='font-size:9.0pt;font-family:"Times New Roman","serif"'>&nbsp;</span><span
    style='font-size:9.0pt;font-family:"Times New Roman","serif"'><%--ABIPG8745PST001--%></span></p>
    </td>
   </tr>
   <tr>
    <td style='border:none;padding:1.5pt 1.5pt 1.5pt 1.5pt'>
    <p class=MsoNormal style='margin-bottom:0in;margin-bottom:.0001pt;
    line-height:normal'><b><span style='font-size:9.0pt;font-family:"Times New Roman","serif"'>Taxes:</span></b><span
    style='font-size:9.0pt;font-family:"Times New Roman","serif"'>&nbsp;</span><span
    style='font-size:9.0pt;font-family:"Times New Roman","serif"'><br>
    Items under Service Tax:</span></p>
    </td>
   </tr>
  </table>
  </td>
 </tr>
 <tr>
  <td style='border:none;padding:3.75pt 3.75pt 3.75pt 3.75pt'>
  <div align=center>
  <table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width="100%"
   style='width:100.0%;border-collapse:collapse'>
   <tr>
    <td style='padding:0in 0in 0in 0in'></td>
   </tr>
   <tr>
    <td style='padding:0in 0in 0in 0in'>
    <p class=MsoNormal style='margin-bottom:0in;margin-bottom:.0001pt;
    line-height:normal'><b><span style='font-size:9.0pt;font-family:"Times New Roman","serif"'>Terms
    &amp; Conditions:</span></b><span style='font-size:9.0pt;font-family:"Times New Roman","serif"'>&nbsp;</span><span
    style='font-size:9.0pt;font-family:"Times New Roman","serif"'><br>
    * All cheques/drafts to be made in favour of</span><span style='font-size:
    9.0pt;font-family:"Times New Roman","serif"'>&nbsp;<b>Dial4Jobz India Private Ltd&nbsp;</b></span><span
    style='font-size:9.0pt;font-family:"Times New Roman","serif"'><br>
    * Mention the Order No. and Customer ID &amp; Name at the back of your
    cheque/draft.</span><span style='font-size:9.0pt;font-family:"Times New Roman","serif"'>&nbsp;</span><span
    style='font-size:9.0pt;font-family:"Times New Roman","serif"'><br>
    * The customer agrees to the Terms &amp; Conditions of use available on our
    website http://www.dial4jobz.com</span><span style='font-size:9.0pt;
    font-family:"Times New Roman","serif"'>&nbsp;</span><span style='font-size:
    9.0pt;font-family:"Times New Roman","serif"'><br>
    * For all future communications please specify your customer  ID &amp; Name</span></p>
    <p class=MsoNormal style='margin-bottom:0in;margin-bottom:.0001pt;
    line-height:normal'><span style='font-size:9.0pt;font-family:"Times New Roman","serif"'>&nbsp;</span></p>
    <p class=MsoNormal style='margin-bottom:0in;margin-bottom:.0001pt;
    line-height:normal'><span style='font-size:9.0pt;font-family:"Times New Roman","serif"'>This
    is a computer generated invoice, hence signature is not required. </span></p>
    </td>
   </tr>
  </table>
  </div>
  </td>
 </tr>
</table>

</div>

<p class=MsoNormal>&nbsp;</p>

</div>
    </div>
   
<input type="button" id="btnPrint" value="print" onclick="return PrintPanel();" style="color:#FFFFFF; background: none repeat scroll 0 0 #7C8399;border-color: #999999 -moz-use-text-color #CCCCCC #999999;" />


</body>
</html>
