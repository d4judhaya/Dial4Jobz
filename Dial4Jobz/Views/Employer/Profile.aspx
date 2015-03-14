<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Dial4Jobz.Models.Organization>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Dial4Jobz - Employer Profile
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    <script src="<%=Url.Content("~/Scripts/Dial4Jobz.Job.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts/Dial4Jobz.Employer.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
	<% Html.RenderPartial("NavEmployer"); %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <% if (Request.IsAuthenticated == true)
       { %>
    <div class="identityname">
       Welcome!!! <b><%: this.Page.User.Identity.Name%></b>, You are in Employer Zone.We wish you to get the right candidates for your Vacancy
    </div>
    <% } else { %>
         <div class="identityname">
           Welcome!!! You are in Employer Zone.We wish you to get the right candidate for your Vacancy
        </div>
    <% } %>
  <center><h2>Employer Profile</h2></center>
    <div class="mandatoryalign" align="right">
        <span class="red">*</span> Mandatory Fields
    </div>

    <% Html.BeginForm("Save", "Employer", FormMethod.Post, new { @id = "Save" }); %>

        <%: Html.Hidden("Id",Model.Id )   %>
        <%: Html.Hidden("orgnName", ViewData["orgnName"])%>

         <div class="editor-label">
            <%: Html.Label("Employer Type") %>
        </div>

        <div class="editor-field">
            <%:Html.RadioButtonFor(model=> model.EmployerType,1)%>  Individual
            <%:Html.RadioButtonFor(model=> model.EmployerType,2)%>  Company
        </div>

        <div id="NumberOfEmployees">
            <div class="editor-label">
                <%: Html.Label("No of Employees") %>
            </div>

            <div class="editor-field">
               <%: Html.DropDownList("NumberOfEmployees", "--Choose One--")%>
            </div>
        </div>

        <div id="OwnershipType">
            <div class="editor-label">
                <%:Html.Label("Ownership Type") %>
            </div>

            <div  class="editor-field">
                <%:Html.DropDownList("OwnershipType","--Choose Ownership--") %>
            </div>
        </div>
        
        <div class="editor-label">
           <%: Html.Label("Employer's Name")%>
           <span class="red">*</span>
        </div>
        <div class="editor-field">
            <%: Html.TextBox("Name", Model.Name, new { @title = "Enter the organization's Name" })%> 
        </div>

        <div class="editor-label">
           <%: Html.Label("Industry")%>
           <span class="red">*</span>
        </div>
        <div class="editor-field">
            <%if (Model.IndustryId != 2378)
          { %>
             <%: Html.DropDownList("Industries", "Select")%> 
        <%} else { %>
            <%: Html.DropDownList("ConsultantIndustries", "Select")%>
        <%} %>
        </div> 

        <div class="editor-label">
          <%: Html.Label("Contact Person")%>
          <span class="red">*</span>
        </div>
        <div class="editor-field">
            <%: Html.TextBox("ContactPerson", Model.ContactPerson, new { @title = "Enter the organization's contact person" })%>   
        </div>

        <div class="editor-label">
          <%: Html.Label("Email")%>
        </div>
        <div class="editor-field">
            <%: Html.TextBox("Email", Model.Email, new { @title = "Enter the organization's email" })%>

            <%if (Model.IsMailVerified == true) { %>
                <%--<a><img src="../../Content/Images/Tick.png" class="btn" /></a>
                   Verified--%>
                   <img src="../../Content/Images/green_round_tick_sign_4246.jpg" width="14px" height="12px" alt="Verified" />Verified
                <% } else { %>
                    <span class="red">Not verified</span>
                    <%:Html.ActionLink("Verify", "EmailVerification", "Employer")%>
            <% } %>
            
           </div>   
       

        <div class="editor-label">
          <%: Html.Label("Website")%>
        </div>
        <div class="editor-field">
            <%: Html.TextBox("Website", Model.Website, new { @title = "Enter the organizations's website" })%>   
        </div>

        <div class="editor-label">
            <%:Html.Label("Employer Address") %> <span class="textlength">Max 300 characters</span>
        </div>

        <div class="editor-field">
            <%:Html.TextBox("Address", Model.Address, new { @title = "Enter Employer's Address", @maxlength = "300" })%>
        </div>

        <div class="editor-label">
            <%:Html.Label("Pincode") %>
        </div>

        <div class="editor-field">
            <%:Html.TextBox("Pincode", Model.Pincode, new { @title = "Enter Employer's Pincode", @maxlength = "6" })%>
        </div>


        <div class="editor-label">
          <%: Html.Label("Location")%>
        </div>
        <div class="editor-field">
            <% if (ViewData["Country"] != null) { %>
            <%: Html.DropDownList("Country", "Country")%>  
            <%} else{ %>
            <select id="Country" name="Country"></select>
            <%} %>

            <% if (ViewData["State"] != null){ %> 
            <%: Html.DropDownList("State", "State")%>   
            <%} else { %>            
            <select id="State" name="State"></select>
            <%} %>

            <% if (ViewData["City"] != null) { %>
                <%: Html.DropDownList("City", "City")%>   
            <% } else { %>
                <select id="City" name="City"></select>
            <% } %>

            <% if (ViewData["Region"] != null) { %>
                <%: Html.DropDownList("Region", "Region")%>   
            <% } else { %>
                <select id="Region" name="Region"></select>
            <% } %>
           
        </div>

        <div class="editor-label">
          <%: Html.Label("LandLine Number")%>
        </div>
        <div class="editor-field">
          <%: Html.TextBox("ContactNumber", Model.ContactNumber, new { @title = "Enter the organization's contact number" })%>    
        </div>

        <div class="editor-label">
            <%: Html.Label("Mobile Number")%>
            <span class="red">*</span>
        </div>
        <div class="editor-field">
            <%: Html.TextBox("MobileNumber", Model.MobileNumber, new { @title = "Enter the organization's mobile number" })%>  

             <%if (Model.IsPhoneVerified == true) { %>
                <%--<a><img src="../../Content/Images/Tick.png" class="btn" /></a>
                   Verified--%>
                   <img src="../../Content/Images/green_round_tick_sign_4246.jpg" width="14px" height="12px" alt="Verified" />Verified
                <% } else { %>
                    <span class="red">Not verified</span>
                      <%:Html.ActionLink("Verify", "VerifyOrganizationMobile", "Employer")%>
            <% } %>
        </div> 
            
            <input id="Save" type="submit" value="Update Profile" class="btn" title="Click to save changes" onclick="javascript:Dial4Jobz.Employer.Save(this);return false;"  />

        <% Html.EndForm(); %>

         <div id="loading">
            <img id="loading-image" src="<%=Url.Content("~/Areas/Admin/Content/Images/ajax_loader1.gif")%>" height="50" alt="Loading..." />
        </div>   

        <br /><br />

        <h2>Posted Jobs</h2>
            <% foreach(Dial4Jobz.Models.Job job in Model.Jobs) { %>
                <%if (job.Position != "")
                          { %>
                <%: Html.ActionLink(job.Position, "Edit","Jobs", new { id = job.Id }, new { title = "Edit this job posting" })%> <br/>   
                <%}
                 else
                  { %>
                       <%--<%: Html.ActionLink("", "Edit", "Jobs", new { id = job.Id }, new { target = "_blank" })%>--%>
                  <%} %>    
            <% } %>
 

</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">
    <div class="section larger">   
        <% Html.RenderPartial("Side/Welcome"); %> 
        <% Html.RenderPartial("Side/GettingStartedEmployer"); %>        
   </div> 
</asp:Content>