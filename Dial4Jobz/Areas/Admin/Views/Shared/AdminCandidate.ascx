<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Dial4Jobz.Models.Candidate>" %>

<div class="section">
        <% Dial4Jobz.Models.Repositories.Repository _repository = new Dial4Jobz.Models.Repositories.Repository(); %>
        <% Dial4Jobz.Models.Repositories.VasRepository _vasRepository = new Dial4Jobz.Models.Repositories.VasRepository(); %>
        <% var activatedOrNot = _vasRepository.PlanSubscribedForCandidate(Model.Id);%>
        <% Dial4Jobz.Models.User user = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(this.Page.User.Identity.Name).FirstOrDefault(); %>
        <% Dial4Jobz.Models.Repositories.UserRepository _userRepository = new Dial4Jobz.Models.Repositories.UserRepository(); %>
      
        <% List<int?> orderdetail = _vasRepository.GetCandidatePendingPlans(Model.Id); %>
        <% List<int?> activatedDetail = _vasRepository.GetCandidateActivePlans(Model.Id); %>
        <% Dial4Jobz.Models.OrderDetail getActivated = _vasRepository.PlanActivatedDetailsForCandidate(Model.Id); %>
        <% Dial4Jobz.Models.AdminUserEntry createdBy = _userRepository.GetCreatedBy(Model.Id, 1); %>
    <ul>
         <li>
            <%: Html.ActionLink("Add VAS", "Index", "CandidatesVas", new { Area = "" }, new { title = "Vas for per Position", id = Model.Id })%> | <br />
            <%: Html.ActionLink("Candidate DashBoard", "CandidateDashBoard", "AdminHome", new { candidateId = Model.Id }, new { target = "_blank" })%> |<br />
            <%: Html.ActionLink("Send Account Details", "AccountDetails", "AdminHome", new { candidateId = Model.Id }, new { title = "Account Details" })%> |<br />
            <% if (Model.Email != null)
              { %>
                    <%: Html.ActionLink("Send Candidate VAS Details", "SendVasDetailsToCandidate", "AdminHome", new { candidateId = Model.Id }, new { title = "Send VAS Details" })%>
            <% } %><br />

            <h3>Created By:</h3>
            <%if(createdBy!=null) { %>
                <%: createdBy.User.UserName %>(<%: Model.CreatedDate %>)
            <%} %>
            
            <%if(Model.VerifiedByAdmin == null || Model.VerifiedByAdmin == "") { %>
                 <%:Html.ActionLink("Verify Profile", "verifyCandidateByAdmin", "AdminHome", new { candidateId = Model.Id }, new { @class = "adminbutton" })%> <br />
            <%} else { %>
               <h3> Verified By: <%: Model.VerifiedByAdmin %></h3>
            <%} %>

            <%if (activatedOrNot==true) { %>
              <div style="color:rgba(224, 43, 132, 1); font-family:Calibri; font-size:14px; font-weight:bold;">
              Click the Plan Name to send the Remainder<br />
              <%foreach (var order in orderdetail)
                { %>
                    <% Dial4Jobz.Models.OrderDetail getOrder = _vasRepository.GetOrderDetail(Convert.ToInt32(order)); %>
                    <%: Html.ActionLink(getOrder.PlanName, "CandidateReminder", "AdminHome", new { orderIds = getOrder.OrderId, flag = "candidate" }, new { title = "Send Remainder" })%>
                    (<%: getOrder.OrderMaster.SubscribedBy %>)(<%: getOrder.OrderMaster.OrderDate.Value.ToString("dd/MM/yyyy") %>)
                    <%:"," %><br />
              <%} %>
                    
              </div>
            
            <%} %>
            <%if(getActivated!=null) { %>
             <h3>Candidate have activated the following plan </h3><br />
              <div style="color:rgba(224, 43, 132, 1); font-family:Calibri; font-size:14px; font-weight:bold;">
                <%foreach (var order in activatedDetail)
                  { %>
                  <% Dial4Jobz.Models.OrderDetail getOrder = _vasRepository.GetOrderDetail(Convert.ToInt32(order)); %>
                    <%:getOrder.PlanName%> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;(<%: getOrder.RemainingCount%>)&nbsp;&nbsp;&nbsp;&nbsp;<%: getOrder.OrderMaster.ActivatedBy%><br />
                <%} %>
               </div>
            <%} else if(activatedOrNot==false) { %>
                <%:"There is no activation" %>
            <%} %> <br />

             <%-- <% if (getActivated!=null) { %>
                <h3>Candidate have activated the following plan </h3><br />
              <div style="color:rgba(224, 43, 132, 1); font-family:Calibri; font-size:14px; font-weight:bold;">
                
                    <%:getActivated.PlanName %>
                 <%if (getActivated.RemainingCount != null)
                   { %>   
                        (<%:getActivated.RemainingCount%>)
                 <% } %>
               </div>
            <%} else if(activatedOrNot==false) { %>
                <%:"" %>
            <%} %> <br />--%>
        </li>
        
       </ul>
</div>