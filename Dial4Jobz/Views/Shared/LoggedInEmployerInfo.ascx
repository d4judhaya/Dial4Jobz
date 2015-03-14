<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<% Dial4Jobz.Models.Organization loggedInOrganization = (Dial4Jobz.Models.Organization)ViewData["LoggedInOrganization"]; %>

<% if (loggedInOrganization != null) { %>
        Organization Name: <%: loggedInOrganization.Name%><br />
        Industry: <%: loggedInOrganization.Industry.Name%><br />
        Contact Person: <%: loggedInOrganization.ContactPerson%><br />
        Email: <%: loggedInOrganization.Email%> 
        <%if(loggedInOrganization.IsMailVerified==true){ %>
            <span class="red">Verified</span>
        <%} else { %> 
            <span class="red">Not verified</span>
        <%} %><br />
        Website: <%: loggedInOrganization.Website%><br />
        Contact Number: <%: loggedInOrganization.ContactNumber%><br/>
        Mobile Number: <%: loggedInOrganization.MobileNumber%>
         <% if (loggedInOrganization.IsPhoneVerified == true) { %>
          <span class="red">Verified</span> <% } else {%>
          <span class="red">Not Verified</span><% } %>
<% } %>


