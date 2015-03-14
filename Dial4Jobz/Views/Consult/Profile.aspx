<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Dial4Jobz.Models.Consultante>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Profile
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

   <% Html.BeginForm("Profile", "Consult", FormMethod.Post, new { @id = "Save" }); %>

   <h2>Update Your Profile</h2>

            <div class="editor-label">
                <%: Html.Label("Consultant's Name") %>
            </div>

            <div class="editor-field">
                <%: Html.TextBox("Name", Model.Name) %>
                <span class="red">*</span>
            </div>

            <div class="editor-label">
                <%: Html.Label("Select Your Display Name") %>
            </div>

            <div class="editor-field">
                <%: Html.TextBox("DisplayName",Model.DisplayName) %>
            </div>

            <div class="editor-label">
                <%: Html.Label("Industry") %>
            </div>

            <div class="editor-field">
                <%: Html.DropDownList("Industries","Select") %>
                <span class="red">*</span>
            </div>

            <div class="editor-label">
                <%: Html.Label("Contact Person") %>
            </div>

            <div class="editor-field">
                <%: Html.TextBox("ContactPerson", Model.ContactPerson, new { @Title = "Enter the Consultant Contact Person Name" })%>
            </div>

            <div class="editor-label">
                <%: Html.Label("Email") %>
            </div>

            <div class="editor-field">
                <%: Html.TextBox("Email", Model.Email, new { @Title="Enter the Consultant Email Address"}) %>
            </div>

            <div class="editor-label">
                <%: Html.Label("Website") %>
            </div>

            <div class="editor-field">
                <%: Html.TextBox("Website", Model.Website, new{ @Title ="Enter the Consultant Website Address"}) %>
            </div>

            <div class="editor-label">
                <%:Html.Label("Consultant Address")%>
                <span class="textlength">Max 300 characters</span>
            </div>

            <div class="editor-field">
                <%:Html.TextBox("Address", Model.Address, new { @Title = "Enter Consultant Address", @maxlength = "300" })%>
            </div>

            <div class="editor-label">
                <%:Html.Label("Pincode") %>
            </div>

            <div class="editor-field">
                <%:Html.TextBox("Pincode", Model.Pincode, new { @Title = "Enter Consultant Pincode", @maxlength = "6" })%>
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
                <%: Html.Label("Contact Number")%>
            </div>

            <div class="editor-field">
                <%: Html.TextBox("ContactNumber", Model.ContactNumber, new { @Title = "Enter the Consultant Contact Number" })%>
            </div>

            <div class="editor-label">
                <%: Html.Label("Mobile Number")%>
                <span class="red">*</span>
            </div>

            <div class="editor-field">
                <%: Html.TextBox("MobileNumber", Model.MobileNumber, new { @Title = "Enter the Consultant's Mobile Number" })%>
            </div>

            <input id="Save" type="submit" value="Update Profile" class="btn" title="Click to save changes" onclick="javascript:Dial4Jobz.Consultant.Profile(this);return false;"  />
        <% Html.EndForm(); %>

         <%--<div id="loading">
            <img id="loading-image" src="<%=Url.Content("~/Areas/Admin/Content/Images/ajax_loader1.gif")%>" height="50" alt="Loading..." />
        </div>   --%>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    <link href="../../Content/bootstrap.css" rel="Stylesheet" type="text/css" />
    <script src="<%=Url.Content("~/Scripts/Dial4Jobz.Consultant.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts/Dial4Jobz.Job.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
      <% Html.RenderPartial("NavConsultant"); %>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">
    <%: Html.ActionLink("Add Candidate","AddCandidate","Consult") %>
</asp:Content>
