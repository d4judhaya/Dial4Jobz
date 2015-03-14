<%@ Page Title="" Language="C#"  Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head  runat="server"> 

<title>Deposit / Send us a Chque or Draft</title>
<script src="<%=Url.Content("~/Scripts/Dial4Jobz.Employer.js")%>" type="text/javascript"></script>
</head>
<body>
    <div class="modal">
        <div class="modal-login-column">
            <div class="header">Call us to Pay through Phone from your Credit Card</div>
            <div class="black">Dear Customer</div><br />
            <p>You can now make payment through a call to us from your mobile or landline if you have a credit card.</p><br />
            <p>Before calling us kindly get “OTP” from your credit card issuing bank.</p><br />
            <p>On getting the same, along with OTP, keep your 16 digit credit card number & CVV number ready and Call us at 044 - 44455566 .</p><br />
        </div>
    </div>
</body>
</html>