<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Admin Home
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="NavContent" runat="server">
	<% Html.RenderPartial("NavAdmin"); %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%--
<% if (Request.IsAuthenticated == true)
   {
       Dial4Jobz.Models.User user = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(this.Page.User.Identity.Name).FirstOrDefault();
       if (user != null)
       {
           
       
%>--%>
<% Dial4Jobz.Models.Repositories.Repository _repository = new Dial4Jobz.Models.Repositories.Repository(); %>
<% Dial4Jobz.Models.Repositories.UserRepository _userRepository = new Dial4Jobz.Models.Repositories.UserRepository(); %>
<% Dial4Jobz.Models.Permission adminPermission = new Dial4Jobz.Models.Permission(); %>
<% if (Request.IsAuthenticated == true)
   {
       Dial4Jobz.Models.User user = new Dial4Jobz.Models.Repositories.UserRepository().GetUsersbyUserName(this.Page.User.Identity.Name).FirstOrDefault();
       if (user != null)
       {
           IEnumerable<Dial4Jobz.Models.AdminPermission> pageaccess = _userRepository.GetPermissionsbyUserId(user.Id);
           string pageAccess = "";
           string[] Page_Code = null;
           foreach (var page in pageaccess)
           {
               adminPermission = _userRepository.GetPermissionsNamebyPermissionId(Convert.ToInt32(page.PermissionId));
               if (string.IsNullOrEmpty(pageAccess))
               {
                   pageAccess = adminPermission.Name + ",";
               }
               else
               {
                   pageAccess = pageAccess + adminPermission.Name + ",";
               }
           }

           if (!string.IsNullOrEmpty(pageAccess))
           {
               Page_Code = pageAccess.Split(',');
           }
%>

<h3>Welcome <%:user.UserName%>!!!. You have logged in as Admin User. </h3><br />


<ul id="coolMenu" style="width:700px;">
			<li><a href="#">Home</a>
            <ul class="noJS">
                    <a href="#">Candidate Summary</a>
                    <% if (Page_Code != null && Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.CandidateSummaryFunction)) || user.IsSuperAdmin != null && user.IsSuperAdmin == true)
                       {
                    %>
					    <li> <%: Html.ActionLink("Function", "CandidateSummary", new { reportType = "Function" })%></li>
                    <% }
                       if (Page_Code != null && Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.CandidateSummaryIndustry)) || user.IsSuperAdmin != null && user.IsSuperAdmin == true)
                       { %>
                        <li> <%: Html.ActionLink("Industry", "CandidateSummary", new { reportType = "Industry" })%></li>
                    <% } %>
                   
                    <% if (Page_Code != null && Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.EmployerSummary)) || user.IsSuperAdmin != null && user.IsSuperAdmin == true)
                       { %>
                       <a href="#">Employer Summary</a>
                          <li> <%: Html.ActionLink("Employer", "EmployerSummary")%></li>
                    <% } %>
			</ul>
            </li>
			<li><a href="#">Users</a>
             <ul class="noJS"> 
                              
                <% if (Page_Code != null && Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.AddCandidate)) || user.IsSuperAdmin != null && user.IsSuperAdmin == true)
                   { %>
                    <li> <%: Html.ActionLink("AddCandidate", "AddCandidate", "AdminHome", null, new { target = "_blank" })%></li>
                 <% }
                   if (Page_Code != null && Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.AddEmployer)) || user.IsSuperAdmin != null && user.IsSuperAdmin == true)
                   { %>
                    <li> <%: Html.ActionLink("Add Employer", "AddEmployer", "AdminHome", null, new { target = "_blank" })%></li>
                    <% }
                 if (Page_Code != null && Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.AddConsultant)) || user.IsSuperAdmin != null && user.IsSuperAdmin == true)
                   { %>
                    <li> <%: Html.ActionLink("AddConsultant", "AddConsultant", "ConsultantReport", null, new { target = "_blank" })%></li>
                    <% }
                   if (Page_Code != null && Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.AddJob)) || user.IsSuperAdmin != null && user.IsSuperAdmin == true)
                   { %>
                    <li> <%:Html.ActionLink("Add Job", "Add", "JobPosting", null, new { target = "_blank" })%></li>
                 <% }
                   if (Page_Code != null && Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.AddAdmin)) || user.IsSuperAdmin != null && user.IsSuperAdmin == true)
                   {  %>
                    <li> <%: Html.ActionLink("Add Admin", "Index", "AdminPermission", null, new { target = "_blank" })%></li>
                 <% }
                   if (Page_Code != null && Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.AddChannelPartner)) || user.IsSuperAdmin != null && user.IsSuperAdmin == true)
                   {  %>
                    <li> <%: Html.ActionLink("Add Channel", "ManagePartners", "Channel", null, new { target = "_blank" })%></li>
                 <% } %> 
					
				</ul>
            </li>
			<li>
				<a href="#">Reports</a>
				<ul class="noJS">

                 <% if (Page_Code != null && Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.UserReport)) || user.IsSuperAdmin != null && user.IsSuperAdmin == true)
                    { %>
					    <li><%: Html.ActionLink("User Report", "UserReport", "AdminPermission", null, new { target = "_blank" })%></li>
                <% }
                    if (Page_Code != null && Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.EmployerReport)) || user.IsSuperAdmin != null && user.IsSuperAdmin == true)
                    { %>
                        <li><%: Html.ActionLink("Employer", "Index", "EmployerReport", null, new { target = "_blank" })%></li>
				 <% }

                    if (Page_Code != null && Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.CandidateRegisteredReport)) || user.IsSuperAdmin != null && user.IsSuperAdmin == true)
                    { %>
                        <li><%: Html.ActionLink("NotUpdatedCandidates", "NonUpdateCandidateReports", "CandidateReport", null, new { target = "_blank" })%></li>
				   <% }

                    if (Page_Code != null && Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.VacancyReport)) || user.IsSuperAdmin != null && user.IsSuperAdmin == true)
                    { %>
                        <li><%: Html.ActionLink("VacancyLists", "Index", "VacancyReport", null, new { target = "_blank" })%></li>
                <%}
            
                 if (Page_Code != null && Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.CandidateReport)) || user.IsSuperAdmin != null && user.IsSuperAdmin == true)
                    { %>
                        <li><%: Html.ActionLink("Candidate", "Index", "CandidateReport", null, new { target = "_blank" })%></li>
				<% } 
       
                if (Page_Code != null && Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.CandidateFullReport)) || user.IsSuperAdmin != null && user.IsSuperAdmin == true)
                  { %>
                        <li><%: Html.ActionLink("CandidateFullReports", "CandidateFullReports", "CandidateReport", null, new { target = "_blank" })%></li>
				<% } 
                  if (Page_Code != null && Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.ConsultantRegisteredReport)) || user.IsSuperAdmin != null && user.IsSuperAdmin == true)
                  { %>
                        <li><%: Html.ActionLink("ConsultantReports", "Index", "ConsultantReport", null, new { target = "_blank" })%></li>
				<% } 
       
                 if (Page_Code != null && Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.ChannelPartnerReport)) || user.IsSuperAdmin != null && user.IsSuperAdmin == true)
                 { %>
                        <li><%: Html.ActionLink("ChannelPartner", "ChannelPartnerReport", "Channel", null, new { target = "_blank" })%></li>
				<% } %>       
                         
          
                <li><%: Html.ActionLink("Admin Report", "AdminReport", "AdminPermission", null, new { target = "_blank" })%></li>	
				</ul>
			</li>
			<li><a href="#">Mails</a></li>
			<li><a href="#">SMS</a></li>
            <li><a href="#">Data</a>
            <ul class="noJS">
                <% if (Page_Code != null && Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.AddCandidate)) || user.IsSuperAdmin != null && user.IsSuperAdmin == true)
                   { %>
                    <li> <%: Html.ActionLink("Import Data", "ImportData")%></li>
                <% } %>
					
				</ul>
            </li>
            <li><a href="#">Candidate</a>
            <ul class="noJS"> 
                <%if (Page_Code != null && Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.CandidateActivationReport)) || user.IsSuperAdmin != null && user.IsSuperAdmin == true)
                  { %>
                       <li><%: Html.ActionLink("CandidateActivate", "CandidateVasActivation", "ActivationReport", null, new { target = "_blank" })%></li>
                <% }
                   
                 if (Page_Code != null && Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.ConsultantActivationReport)) || user.IsSuperAdmin != null && user.IsSuperAdmin == true)
                  { %>
                       <li><%: Html.ActionLink("ConsultantActivate", "ConsultantActivation", "ActivationReport", null, new { target = "_blank" })%></li>
                <% }
                   
                  if (Page_Code != null && Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.ConsultantActivatedReport)) || user.IsSuperAdmin != null && user.IsSuperAdmin == true)
                  { %>
                       <li><%: Html.ActionLink("ConsultantActivated", "ConsultantActivatedReport", "ActivationReport", null, new { target = "_blank" })%></li>
                 <% }

                  if (Page_Code != null && Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.CandiateActivatedReport)) || user.IsSuperAdmin != null && user.IsSuperAdmin == true)
                  { %>   
                      <li><%: Html.ActionLink("CandidateActivated", "CandidateActivatedReport", "ActivationReport", null, new { target = "_blank" })%></li>
                  <% } %>
					
				</ul>
                </li>
            <li><a href="#">Employer</a>
                <ul class="noJS"> 
                              
                <% if (Page_Code != null && Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.ActivationReport)) || user.IsSuperAdmin != null && user.IsSuperAdmin == true)
                   { %>
                     <li><%: Html.ActionLink("EmployerActivate", "Index", "ActivationReport", null, new { target = "_blank" })%></li>
                    <% }
                   if (Page_Code != null && Page_Code.Any(s => s.Contains(Dial4Jobz.Models.Constants.PageCode.ActivatedReport)) || user.IsSuperAdmin != null && user.IsSuperAdmin == true)
                   { %>
                     <li><%: Html.ActionLink("Activated List", "ActivatedReport", "ActivationReport", null, new { target = "_blank" })%></li>
                    <% } %>   
					
				</ul>
            
            </li>
		</ul>
        <img src="../../../../Content/Images/admin_menu1.jpg"  width="700px" height="400px" />
          <%}
   }
   else
   { %>
   <center>
      <h1>You are not allowed to access this page!!!</h1>
    </center>
   <%} %>
   

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
 <script src="<%=Url.Content("~/Scripts/Dial4Jobz.Admin.js")%>" type="text/javascript"></script>
 <link href="../../Content/Admin.css" rel="stylesheet" type="text/css" />
</asp:Content>
