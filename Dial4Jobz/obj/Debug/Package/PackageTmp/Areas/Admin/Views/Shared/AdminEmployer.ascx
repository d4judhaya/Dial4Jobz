<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<div class="section">
        <% Dial4Jobz.Models.Repositories.Repository _repository = new Dial4Jobz.Models.Repositories.Repository(); %>
        <% Dial4Jobz.Models.Repositories.VasRepository _vasRepository = new Dial4Jobz.Models.Repositories.VasRepository(); %>
        <% var activatedOrNot = _vasRepository.PlanSubscribedForEmployer(Model.Id);%>
        <% Dial4Jobz.Models.OrderDetail getActivated = _vasRepository.PlanActivatedDetails(Model.Id); %>
        <% List<int?> countofPlans = _vasRepository.GetOrganizationPendingPlans(Model.Id); %>
    <ul>
         <li>
              <% Dial4Jobz.Models.Job job=new Dial4Jobz.Models.Job(); %>
              
                 <%: Html.ActionLink("DashBoard", "AdminDashBoard", "AdminHome", new { organizationId = Model.Id }, new { target = "_blank" })%>  | <br />

                 <%: Html.ActionLink("Send Account Details", "AccountDetailsEmployer", "AdminHome", new { EmployerId = Model.Id, consultantId=0 }, new { title = "Account Details" })%> | <br />
             
                 <%if(Model.Email!=null) { %>
                    <%: Html.ActionLink("Send Vas Details", "SendVasDetailsToEmployer", "AdminHome", new { organizationId = Model.Id, consultantId=0 }, new { title = "Send Vas Details" })%> <br />
                 <%} %><br />
         
                    <%if (Model.VerifiedByAdmin == null || Model.VerifiedByAdmin == "") { %>
                            <%:Html.ActionLink("Verify Profile", "verifyEmployerByAdmin", "AdminHome", new { organizationId = Model.Id }, new { @class = "adminbutton" })%> 
                    <%} else { %>
                        <h3> Verified By: <%:Model.VerifiedByAdmin%></h3>
                    <%} %>

                    <%if (activatedOrNot==true)
                      { %>
                       <div style="color:rgba(224, 43, 132, 1); font-family:Calibri; font-size:14px;font-weight:bold;">
                        Click the Plan Name to send the Remainder<br />
                        <%foreach (var order in countofPlans)
                          { %>
                            <% Dial4Jobz.Models.OrderDetail getOrder = _vasRepository.GetOrderDetail(Convert.ToInt32(order)); %>
                            <%: Html.ActionLink(getOrder.PlanName, "EmployerReminder", "AdminHome", new { organizationId = Model.Id, orderId=getOrder.OrderId }, new { title="Send Remainder" })%>
                            <%:"," %><br />
                        <%} %>
                       </div>
                            <%} %>
                    <%-- <%if (getActivated != null)
                      { %>--%>
                        <div style="color:rgba(224, 43, 132, 1); font-family:Calibri; font-size:14px; font-weight:bold;">
                                Employer have activated the following plan 
                         </div>
                      <% foreach (Dial4Jobz.Models.OrderDetail orderdetail in new Dial4Jobz.Models.Repositories.VasRepository().GetActivatedPlansList(Model.Id))
                         { %>
                               <h3><%: orderdetail.PlanName %></h3>
                               <%if(orderdetail.RemainingCount!=null) { %>
                                     (<%:orderdetail.RemainingCount%>)
                               <%} %>
                            <%} %>
                           <%-- <div style="color:rgba(224, 43, 132, 1); font-family:Calibri; font-size:14px; font-weight:bold;">
                                Employer have activated the following plan 
                            </div>
                            <h3><%:getActivated.PlanName%></h3>
                            <%if (getActivated.RemainingCount != null)
                            { %>   
                                (<%:getActivated.RemainingCount%>)
                            <% } %>--%>
                            
                        <%--<%}
                      else if (activatedOrNot == false)
                      { %>
                            <%:"" %>
                        <%} %> <br />--%>

        </li>
        
       </ul>
</div>