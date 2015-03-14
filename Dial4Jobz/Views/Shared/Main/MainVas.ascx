<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<div class="main">
<% Dial4Jobz.Models.Organization loggedInOrganization = (Dial4Jobz.Models.Organization)ViewData["LoggedInOrganization"]; %>
<% bool isLoggedIn = loggedInOrganization != null; %>
<%Dial4Jobz.Models.Repositories.VasRepository _vasRepository = new Dial4Jobz.Models.Repositories.VasRepository(); %>
<%Dial4Jobz.Models.Repositories.Repository _repository = new Dial4Jobz.Models.Repositories.Repository(); %>

 <% var ActivatedList=0; %>

    <%if (isLoggedIn == true) { %>
        <%ActivatedList = _vasRepository.GetPlanActivatedResultRAT(loggedInOrganization.Id); %>
 <% } %>

       <table style="background-color:white;">
        <tr>
              <th colspan="2" style="background-color:White;">
                 <%if (isLoggedIn == true)
                   { %>
                         <a href="<%: Url.Action("Add", "Jobs") %>"><img src="../../../../Content/Images/PostVacancy.jpg" width="128" height="42" alt="Dial4Jobz Post Vacancy" /></a><img src="../../../../Content/Images/free2.jpg" width="55px" height="55px" alt="Post Vacancy" /><br />
                         
                         <%var activatedPlans = _vasRepository.GetActivePlans(loggedInOrganization.Id); %>
                         
                         <strong class="maintext">Your Active Plans: </strong><br />
                         <%foreach (var plans in activatedPlans) {%>
                         <%if (plans.PlanName == "BasicRATHORS")
                           { %>
                                <%:"E-Basic"%> For Hors(<%:plans.BasicCount%>)and RAT(<%:plans.RemainingCount %>) <%:","%>
                         <%} else { %>
                              <%:plans.PlanName%> (<%:plans.RemainingCount%>) <%:","%>
                              <%} %>
                         <% } %>
                         <br />
                         <%var jobsPostedList = _repository.GetJobsByOrganizationId(loggedInOrganization.Id);%>
                         <%if (jobsPostedList.Count() == 0) { %>
                         <%} else { %>
                             <%:Html.ActionLink("View", "PostedJobs", "Employer", new { target = "_blank" })%> candidate for your Vacancy<br />
                         <% } %>
                  <%} else { %>
                        <a class="signup" href="<%=Url.Content("~/signup")%>" title="Create an account on Dial4Jobz">
                        <img src="../../../../Content/Images/PostVacancy.jpg" width="128" height="42" alt="post vacancy" /></a><img src="../../../../Content/Images/free2.jpg" width="55px" height="55px" alt="Post Vacancy" /><br />
                        <h4>Looking For Candidates??? Searching For A Recruitment Solution Which actually solves???</h4>
                        <h4>Dial4Jobz Offers Easy Access...Optimal Results...Hot & Fresh Data...</h4>
                        <span style="color:rgb(58, 58, 202); font-family:Calibri; font-size:16px;">Keep watching this Space for Exciting Offers!!!</span>
                        <h4> Resume Alert Services, Hot Resume Services & More <a class="signup" href="<%=Url.Content("~/signup")%>" title="Create an account on Dial4Jobz">Click Here For More Details</a></h4>
                 <% } %>
                 
                 <%if (isLoggedIn == true)
                   { %>
                       <%var pendingpayment = _vasRepository.GetPendingOrdersEmployers(loggedInOrganization.Id); %>
                     <%if (pendingpayment.Count() > 0 )
                      { %>
                        <span class="maintext">
                            <%:Html.ActionLink("Pay Now & Activate", "MySubscription_Billing", "Employer", new { target = "_blank" })%> your pending orders.</span><br />
                    <% } %>
                <% } %>
               <br />
            </th>

            <%if (isLoggedIn == true)
              { %>
                <th style="background-color:White;">
                        <h4>Looking For Candidates??? Searching For A Recruitment Solution Which actually solves???</h4>
                        <h4>Dial4Jobz Offers Easy Access...Optimal Results...Hot & Fresh Data...</h4>
                      <span style="color:rgb(58, 58, 202); font-family:Calibri; font-size:16px;">Keep watching this Space for Exciting Offers!!!</span>
                      <h4> Resume Alert Services, Hot Resume Services & More <a href="<%: Url.Action("Index","EmployerVas",new {@target = "_blank" }) %>">Click Here For More Details</a></h4>
                </th>
            <% } %>
        </tr>
    </table>

</div>

