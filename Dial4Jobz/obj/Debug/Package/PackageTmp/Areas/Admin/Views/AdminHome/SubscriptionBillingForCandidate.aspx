<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Candidate Subscription Details
    <% Session["candidateId"] = Request.QueryString["candidateId"];%>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<% var candidateId = Convert.ToInt32(Session["candidateId"]);%>

<% ViewData["LoggedInCandidate"] = candidateId; %>
<% Dial4Jobz.Models.Repositories.VasRepository _vasRepository = new Dial4Jobz.Models.Repositories.VasRepository(); %>


    <h2>Candidate Subscription Billing Details</h2>

    <table>
        <tr>
            <td>
                S.No
            </td>
            <td>
                Plan Subscribed
            </td>
            <td>
                Date of subscription
            </td>
            <td>
                Date of payment
            </td>
            <td>
                Amount
            </td>
            <td>
                Discount Amount
            </td>
            <td>
                Date activation
            </td>
            <td>
                Validity in count
            </td>
            <td>
                Valid till
            </td>
            <td>
                Remaining Counts
            </td>
            <td>
                Print Invoice
            </td>

            <td>
                Send Invoice By Mail
            </td>
        </tr>
        <tbody>
            <%  int i = 0;
                foreach (Dial4Jobz.Models.OrderDetail orderdetail in new Dial4Jobz.Models.Repositories.VasRepository().GetOrderDetailsbyCandidateId(ViewData["LoggedInCandidate"] != null ? Convert.ToInt32(ViewData["LoggedInCandidate"].ToString()) : 0))
               {
                   i++; %>
               <tr>
                <% Dial4Jobz.Models.VasPlan vasplan = _vasRepository.GetVasPlanByPlanId(Convert.ToInt32(orderdetail.PlanId)); %>
                <td>
                    <%: i %>
                </td>
                <td>
                     <%if (vasplan != null)
                  {%>
                        <%: vasplan.PlanName%>
                <%} else  { %>
                    <%:orderdetail.PlanName %>
                <%} %>
                </td>
                <td>
                 <%if (orderdetail.OrderMaster.OrderDate != null)
                  { %>
                    <%: orderdetail.OrderMaster.OrderDate.Value.ToString("dd-MM-yyyy")%>
                <%}
                  else
                  { %>
                <% } %>
                </td>
                <td>
                    <% var orderpayment = orderdetail.OrderMaster.OrderPayments.Where(op => op.OrderId == orderdetail.OrderId && orderdetail.OrderMaster.PaymentStatus==true).FirstOrDefault();

                       if (orderpayment != null)
                       { %> 
                            <%: orderdetail.ActivationDate != null ? orderdetail.ActivationDate.Value.ToString("dd-MM-yyyy") : ""%>
                            
                    <% }
                       else
                       { %>
                           
                           <%-- Pending - <a href="<%: Url.Action("Payment", "EmployerVas", new { orderId = orderdetail.OrderId }) %>"><img src="../../Content/Images/MYCart.png" /></a>--%>
                            Pending - 
                            <a href="<%: System.Configuration.ConfigurationManager.AppSettings["SiteFullURL"].ToString() %>/Candidates/CandidatesVas/Payment?orderId=<%: Dial4Jobz.Models.Constants.EncryptString(orderdetail.OrderId.ToString()) %>" ><img src="../../Content/Images/MYCart.png" /></a>
                           
                    <% } %>
                </td>
                 <td>
                        <%: orderdetail.Amount%>
                </td>
                <td>
                    <%if (orderdetail.DiscountAmount != null)
                      { %>
                      <%:orderdetail.DiscountAmount%>
                    <%} else {%>
                        <%:"0" %>
                    <%} %>
                </td>
                <td>
                    <%: orderdetail.ActivationDate != null ? orderdetail.ActivationDate.Value.ToString("dd-MM-yyyy") : "" %>
                </td>
                <td>
                   <%if (orderdetail.ValidityCount == 999999)
                     { %>
                        <%:"Unlimited" %>
                   <%} else { %>
                        <%: orderdetail.ValidityCount %>
                   <%} %>
                </td>
                <td>
                    <%: orderdetail.ValidityTill != null ? orderdetail.ValidityTill.Value.ToString("dd-MM-yyyy") : ""%>
                </td>
                <td>
                <%if (orderdetail.RemainingCount == 999999)
                  { %>
                        <%:"Unlimited" %>
                    <%} else { %>
                        <%: orderdetail.RemainingCount%>
                    <%} %>
                </td>                
                <td>
                    <%--<a class="callpickupcash" href="<%: Url.Action("InvoiceForCandidate", "AdminHome", new { orderId = orderdetail.OrderId }) %>">Print Invoice</a>--%>
                     <a class="callpickupcash" href="<%: System.Configuration.ConfigurationManager.AppSettings["SiteURL"].ToString() %>/Candidates/CandidatesInvoice?orderId=<%: orderdetail.OrderId %>">Print Invoice</a>
                </td>

                <td>
                    <a href="<%: Url.Action("SendInvoiceBymailToCandidate", "AdminHome", new { orderId = orderdetail.OrderId, Id=orderdetail.OrderMaster.CandidateId }) %>"><input type="submit" id="contactbtn" value="Send Mail" /></a>
                </td>
                </tr>
            <% } %>
        </tbody>
    </table>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
<script src="<%=Url.Content("~/Scripts/Dial4Jobz.Candidate.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
    <% Html.RenderPartial("NavAdmin"); %>
</asp:Content>


