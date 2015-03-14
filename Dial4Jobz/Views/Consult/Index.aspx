<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Consultant
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>Welcome to Consultant Page</h2>

    <% Html.BeginForm("Index", "Consult", FormMethod.Get, new { }); %>

    <% Dial4Jobz.Models.Consultante LoggedInConsultant = (Dial4Jobz.Models.Consultante)ViewData["LoggedInConsultant"]; %>
    <% Dial4Jobz.Models.Repositories.VasRepository _vasRepository= new Dial4Jobz.Models.Repositories.VasRepository(); %>
    <% bool isLoggedIn = LoggedInConsultant != null; %>
    
    <% IEnumerable<Dial4Jobz.Models.OrderDetail> getActivePlans = null;%>

    <% if (isLoggedIn == true)
       { %>
            <% getActivePlans = _vasRepository.GetActivatedPlansByConsultant(LoggedInConsultant.Id); %>
            <%if (getActivePlans.Count() > 0)
               {%>
                  <h3>Welcome!!! <b><%: LoggedInConsultant.Name %></b>, You are in Consultant Page. Now You Can Update Your Profile and Upload Candidates Database.</h3>  
             <%} else { %>
                  <h3>Welcome!!! <b><%: LoggedInConsultant.Name %></b>, Your  plans are not active. Please Subscribe plans for upload Candidates Database.</h3>  
             <%} %>
    <%} %>
  
   <ul id="tabmenu">
    <li>
        Register & Login
        <ul>
             <%if (isLoggedIn == true)
               { %>
                <li><a href="<%:Url.Action("LogOff","Consult") %>">LogOut</a></li>
             <%} else { %>
                 <li><a href="#">Login and Register</a>
                <ul>
                 <%if (isLoggedIn == true) { %>
                <%} else { %> 
                    <li><a href="<%:Url.Action("Register","Consult") %>" target="_blank">Register</a></li>
                <%} %>
                    <li><a href="<%:Url.Action("LogOn","Consult") %>" target="_blank">Login</a></li>
                </ul>
            </li>
            <%} %>
                <li><a href="#">Profile Summary</a>
                <ul>
                 <%if (isLoggedIn == true)
                   { %>
                    <li><a href="<%:Url.Action("Profile","Consult") %>" target="_blank">Update Your Profile</a></li>
                 <%} else { %>
                     <li><a href="<%:Url.Action("LogOn","Consult") %>" target="_blank">Update Profile</a></li>
                 <%} %>

                   <%if (isLoggedIn == true)
                      { %>
                       <li><a href="<%:Url.Action("AddCandidate","Consult") %>" target="_blank">Add New Candidate</a></li>
                    <%} else { %>
                        <li><a href="<%:Url.Action("LogOn","Consult") %>" target="_blank">Add New Candidate</a></li>
                    <%} %>

                    <%if (isLoggedIn == true)
                      { %>
                       <li><a href="<%:Url.Action("Add","Consult") %>?consultantId=0" target="_blank">Post Vacancy</a></li>
                    <%} else { %>
                        <li><a href="<%:Url.Action("LogOn","Consult") %>" target="_blank">Post Vacancy</a></li>
                    <%} %>
                </ul>
            </li>
            <%--<li><a href="?13">Update Your Profile</a></li>--%>
        </ul>
    </li>
    <li>
        <a href="#">Reports</a>
        <ul>
        <% if (isLoggedIn == true)
           { %>
             <li><a href="<%: Url.Action("CandidateReports","Consult") %>" target="_blank">Candidates Reports</a>
            <%} else { %>
        
            <% } %>
            </li>
        <% if (isLoggedIn == true)
            { %>
                <li><a href="<%: Url.Action("VacancyReports","Consult") %>" target="_blank">Vacancy Reports</a>
        <%} else { %>
        
        <% } %>

             <%--   <ul>
                  
                    <li><a href="?222">Tabbed menu 2-2-2</a></li>
                    <li><a href="?223">Tabbed menu 2-2-3</a></li>
                    <li><a href="?224">Tabbed menu 2-2-4</a></li>
                </ul>--%>
            </li>
        <%--    <li><a href="?23"></a></li>--%>
        </ul>
    </li>

     <li>
        <a href="#">Billing & Subscriptions</a>
        <ul>
            <li><a href="<%: Url.Action("Index","EmployerVas") %>" target="_blank">Subscribe Employer Plans</a>
            <li><a href="<%: Url.Action("Plans","Consult") %>">Subscribe Your Plans</a></li>
                <ul>
                 <%if (isLoggedIn == true)
                   { %>
                        <li><a href="<%: Url.Action("PostedJobs","Consult") %>">Assign Vacancy for RAT</a></li>
                  <%} %>
                    
                </ul>
            </li>
            <li><a href="<%: Url.Action("VacancyReports","Consult") %>" target="_blank">Alert Sent Details</a>
                <ul>
                    <li><a href="<%:Url.Action("AlertSentDetails","Employer") %>">Resume Alert Details</a></li>
                    <li><a href="<%:Url.Action("ViewedCandidatesList","Employer") %>">Hot Resumes Viewed List</a></li>
                    <%--<li><a href="?224">Tabbed menu 2-2-4</a></li>--%>
                </ul>
            </li>
            <li><a href="<%: Url.Action("MySubscription_Billing","Consult") %>" target="_blank">Your Subscriptions and Invoice</a></li>
        </ul>
    </li>
  <%--  <li>
        <a href="#">Upload & Download</a>
        <ul>
             <%if (isLoggedIn == true)
              { %>
                <li><a href="<%: Url.Action("Download","Consult") %>">Download Excel</a></li>
            <%} else { %>
                <li><a href="<%:Url.Action("LogOn","Consult") %>">Download Excel</a></li>
            <%} %>
           <%if (isLoggedIn == true)
              { %>
                 <li><a href="<%: Url.Action("ImportData","Consult") %>" target="_blank">Import Candidates Database</a></li>
            <%} else { %>
                <li><a href="<%:Url.Action("LogOn","Consult") %>" target="_blank">Import Candidates Database</a></li>
           <%} %>
        </ul>
    </li>--%>

    <li>
        <a href="<%: Url.Action("MatchCandidates","Employer") %>" target="_blank">Search Candidates</a>
    </li>

    <li>
        <a href="<%: Url.Action("Index","Home") %>" target="_blank">Search Vacancies</a>
    </li>

    <li>
        <a href="#">Contact Us</a>
        <ul>
            <li><a href="#">Call 044 - 44455566</a></li>
            <li><a href="#">Call +91 9381516777</a></li>
        </ul>
    </li>
    <%--<li><a href="?4">Contact Us</a></li>--%><img src="../../Content/Images/consultant_large3.jpg" width="750px" height="350px" />
</ul>

  <% Html.EndForm(); %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    <link href="../../Content/tabmenu.css" rel="Stylesheet" type="text/css" />
    <script src="<%=Url.Content("~/Scripts/tabmenu.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">
</asp:Content>
