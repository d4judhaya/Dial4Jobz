<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<div class="section">
        <% Dial4Jobz.Models.Repositories.Repository _repository = new Dial4Jobz.Models.Repositories.Repository(); %>
        <% Dial4Jobz.Models.Repositories.VasRepository _vasRepository = new Dial4Jobz.Models.Repositories.VasRepository(); %>
        <% Dial4Jobz.Models.Repositories.UserRepository _userRepository = new Dial4Jobz.Models.Repositories.UserRepository(); %>
        <% Dial4Jobz.Models.User user = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(this.Page.User.Identity.Name).FirstOrDefault(); %>

        <% var activatedOrNot = _vasRepository.PlanSubscribedForEmployer(Model.Id);%>
        <% Dial4Jobz.Models.OrderDetail getActivated = _vasRepository.PlanActivatedDetails(Model.Id); %>
        <% List<int?> countofPlans = _vasRepository.GetOrganizationPendingPlans(Model.Id); %>
         <% Dial4Jobz.Models.AdminUserEntry createdBy = null;
           if (user != null)
           {
               createdBy = _userRepository.GetCreatedBy(Model.Id, 2);
           } %>
    
    <ul>
         <li>
              <% Dial4Jobz.Models.Job job=new Dial4Jobz.Models.Job(); %>
              
                 <%: Html.ActionLink("DashBoard", "AdminDashBoard", "AdminHome", new { organizationId = Model.Id }, new { target = "_blank" })%>  | <br />

                 <%: Html.ActionLink("Send Account Details", "AccountDetailsEmployer", "AdminHome", new { EmployerId = Model.Id, consultantId=0 }, new { title = "Account Details" })%> | <br />
             
                 <%if(Model.Email!=null) { %>
                    <%: Html.ActionLink("Send Vas Details", "SendVasDetailsToEmployer", "AdminHome", new { organizationId = Model.Id, consultantId=0 }, new { title = "Send Vas Details" })%> <br />
                 <%} %><br />

                    <h3>Created By:</h3>
                    <%if(createdBy!=null) { %>
                        <%: createdBy.User.UserName %> (<%: Model.CreateDate %>)
                    <%} %>
         
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
                            <%: Html.ActionLink(getOrder.PlanName, "EmployerReminder", "AdminHome", new { orderIds = getOrder.OrderId, flag = "employer" }, new { title = "Send Remainder" })%>
                            <%:"," %><br />
                        <%} %>
                       </div>
                            <%} %>
                    
                        <div style="color:rgba(224, 43, 132, 1); font-family:Calibri; font-size:14px; font-weight:bold;">
                                Employer have activated the following plan 
                         </div>
                      <% foreach (Dial4Jobz.Models.OrderDetail orderdetail in new Dial4Jobz.Models.Repositories.VasRepository().GetActivatedPlansList(Model.Id))
                         { %>
                               <h3><%: orderdetail.PlanName %></h3>
                               <%if(orderdetail.RemainingCount!=null) { %>
                                     (<%:orderdetail.RemainingCount%>) (<%: orderdetail.ActivationDate.Value.ToString("dd-MM-yyyy") %>)&nbsp; &nbsp;&nbsp;<%: orderdetail.OrderMaster.ActivatedBy %>
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