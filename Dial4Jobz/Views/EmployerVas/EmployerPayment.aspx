<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Employer Payment Status
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 
 <% if (ViewData["Success"] != null && ViewData["Success"].ToString() != "")
           { %>
             <h2><%=ViewData["Success"]%></h2><br />

             <%:Html.ActionLink("Go to your Page","MatchCandidates","Employer") %>
                  
        <% }
            if (ViewData["Failure"] != null && ViewData["Failure"].ToString() != "")
           { %>
             <h2><%=ViewData["Failure"]%></h2>        
              <%:Html.ActionLink("Go to your Page","MatchCandidates","Employer") %>
        <% } %>


</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
<%Html.RenderPartial("NavEmployer"); %>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">
 <div><h5>Sales Enquiries</h5>
                <ul>
                <li>Mobile Number:+91-9381516777 </li>
                <li>Contact Number:044 - 44455566</li>
                <li>E-Mail:smc@dial4Jobz.com </li>
                </ul>    
                </div>

                <div><h5>Customer Support</h5>
                <ul>
                <li><a title ="Contact Me">Manikandan</a></li>
                <li>Contact Number:044 - 44455566 </li>
                <li>E-Mail:smo@dial4jobz.com </li>
                </ul>    
                </div>
</asp:Content>
