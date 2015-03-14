<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<% Dial4Jobz.Models.Organization loggedInOrganization = (Dial4Jobz.Models.Organization)ViewData["LoggedInOrganization"]; %>
<% bool isLoggedIn = loggedInOrganization != null; %>

<table class="pricingtable"  cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <th class="bdrL_blue valignT highlight" colspan="11">
                <%--<strong style="font-family:Arial; font-size:12px; font-weight:bold">Get resume alert by SMS & E-Mail of Active Candidates who Register after you subscribe</strong>--%>
            </th>
        </tr>

                 
        <tr>
             <th width="20%" colspan="2" class="bdrL_blue valignT highlight">
                <strong class="font12"><span class="red"> Rs. 99 + TAX Each </span><br />
                 Resume Alert Services (OR) Hot Resumes</strong><br />
            </th>

             <th width="40%" style="text-align:center;" class="bdrL_blue valignT highlight">
                 <span class="font18">Get Alerts For A Vacancy From Suitable & Fresh Registered Candidates  </span><br />
                 <span class="font18">(OR)</span><br />
                 <span class="font18">Be On Top Of Searches by Suitable Candidates</span><br />
                 <span class="font18">Access Database</span><br />
                 <span class="maintext">Send 100 Emails To Suitable Candidates</span>

            </th>

             <th width="27%" style="text-align:center;" class="bdrL_blue valignT highlight">
                 <span class="font18"><b>7 Days / 25 Alerts</b>- Whichever Is Earlier </span><br />
                 <span class="font18">(OR)</span><br />
                 <span class="font18"><b>7 Days / 25 Contacts Viewed</b>- Whichever Is Earlier</span>
            </th>

           <%--  <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">-</span><br />
                <span class="font18">100 Emails To Suitable Candidates</span>
            </th>--%>
            <%if (isLoggedIn == true)
              { %>
            <th width="20%" class="bdrL_blue valignT highlight">
                <a href="<%: Url.Action("Index", "EmployerVas") %>"><img src="../../Content/Images/Subscribe now on click.png" class="btn" width="100px" height="25px"/></a>
            </th>
            <%} else { %>
               <th width="20%" class="bdrL_blue valignT highlight">
                 <span class="font18">Posting vacancy & Viewing Resumes ARE FREE</span>
                </th>
            <%} %>
        </tr>
               

        <tr>
            <th width="20%" colspan="2" class="bdrL_blue valignT highlight">
                <strong class="font12"><span class="red">Rs. 195/- + TAX COMBO</span> <br />
                  Resume Alert Services (&) Hot Resumes</strong><br />
            </th>

             <th width="40%" style="text-align:center;" class="bdrL_blue valignT highlight">
                 <span class="font18">Get Alerts For A Vacancy From Suitable & Fresh Registered Candidates</span><br />
                 <span class="font18">(AND)</span><br />
                 <span class="font18">Be On Top Of Searches by Suitable Candidates</span><br />
                 <span class="font18">Access Database<br /></span>
                 <span class="maintext"> Send 100 Emails To Suitable Candidates</span>
             </th>

            <th width="27%" style="text-align:center;" class="bdrL_blue valignT highlight">
                 <span class="font18">7 Days / 25 Alerts -  Whichever Is Earlier</span><br />
                 <span class="font18">(AND)</span><br />
                 <span class="font18">7 Days / 25 Contacts Viewed - Whichever Is Earlier</span>
            </th>

          <%--  <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
                <span class="font18">-</span><br />
                <span class="font18">100 Emails To Suitable Candidates</span>
            </th>--%>
                    
             <%if (isLoggedIn == true)
               { %>
                <th width="24%" class="bdrL_blue valignT highlight">
                    <a href="<%: Url.Action("Payment", "EmployerVas") %>"><img src="../../Content/Images/Subscribe now on click.png" width="100px" height="25px" class="btn" onclick="return vas_confirm('RAT,HORS','219')" /></a>
                </th>
            <%} else { %>   
                <th width="24%" class="bdrL_blue valignT highlight">
                   Already Registered <a class="signup" href="<%=Url.Content("~/login")%>" title="Login to Dial4Jobz">Login</a> <br />
                   (OR)<br />
                  <span class="font18"><a class="signup" href="<%=Url.Content("~/signup")%>?value=employer">Register Now</a></span><br />
                  <span class="font18">TO</span><br />
                  <span class="font18">View Resumes</span><br />
                  <span class="font18">Post Vacancies</span><br />
                  <span class="font18">Subscribe Value added services</span>
                </th>
            <%} %>
        </tr>

        <tr>
             <th width="20%" colspan="2"  class="bdrL_blue valignT highlight">
                <strong class="font12"><span class="red">Rs. 299/- + TAX SUPER COMBO </span><br />
                Resume Alert Services & Hot Resumes</strong><br />
            </th>

             <th width="40%" style="text-align:center;" class="bdrL_blue valignT highlight">
                 <span class="font18">Get Alerts For A Vacancy From Suitable & Fresh Registered Candidates </span><br />
                  <span class="font18">(AND)</span><br />
                  <span class="font18">Be On Top Of Searches by Suitable Candidates</span><br />
                  <span class="font18">Access Database</span><br />
                  <span class="maintext">Send 200 Emails To Suitable Candidates</span><br />
            </th>

            <th width="27%" style="text-align:center;" class="bdrL_blue valignT highlight">
                 <span class="font18">7 Days / 50 Alerts - Whichever Is Earlier</span><br />
                 <span class="font18">(AND)</span><br />
                 <span class="font18"> 7 Days / 50 Contacts Viewed - Whichever Is Earlier</span>
            </th>

           <%-- <th width="20%" style="text-align:center;" class="bdrL_blue valignT highlight">
             <span class="font18">-</span><br />
                 <span class="font18">200 Emails To Suitable Candidates</span>
            </th>--%>
            
                    
             <%if (isLoggedIn == true)
               { %>
                <th width="24%" class="bdrL_blue valignT highlight">
                    <a href="<%: Url.Action("Payment", "EmployerVas") %>"><img src="../../Content/Images/Subscribe now on click.png" width="100px" height="25px" class="btn" onclick="return vas_confirm('RAT,HORS','336')" /></a>
                </th>
              <%} else { %>   
                <th width="24%" class="bdrL_blue valignT highlight">
                     <%--<span class="font18">TO</span><br />
                     <span class="font18">View Resumes</span>
                     <span class="font18">Post Vacancies</span>
                     <span class="font18">Subscribe Value added services</span>--%>
                </th>
            <%} %>
        </tr>

       <%-- <tr>
            <th class="bdrL_blue valignT highlight" colspan="11">
                <ul>
                    <li>Vacancies posted will be displayed on top of all suitable Job search & Alert will be live for 30 days from the date of activation or till 25 Resumes per vacancy is sent whichever is earlier.</li>                    
                </ul>
            </th>
        </tr>--%>
    </table>

      <script type="text/javascript">

          function vas_confirm(plan, amount) {

              if (confirm("Confirm the order to buy for Rs. " + amount + "")) {

                  if (plan != undefined) {

                      $.ajax({
                          url: '/employer/employervas/SubscribedComboPlans',
                          type: 'POST',
                          data: { 'Plan': plan, 'Amount': amount },
                          datatype: 'json',
                          success: function (response) {
                              if (response.Success) {

                              }
                          },
                          error: function (xhr, status, error) {
                          }
                      });
                  }

              }
              else {
                  return false;
              }
          }   

      
</script>
           
