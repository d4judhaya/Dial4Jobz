<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	ImportData
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Import Data</h2>
     
    
     <% using (Html.BeginForm("ImportData", "Consult", 
                    FormMethod.Post, new { enctype = "multipart/form-data" }))
        {%>
        <div>
        <input name="uploadFile" type="file" />
        <input type="submit" value="Upload File" />
            
            </div>
<%} %>

 <%if (ViewData["ImportStatus"] != null && (bool)ViewData["ImportStatus"] == true)
   {%>
      <br/><h3>Imported candidates data success</h3>
 <%  }
   if (ViewData["ImportStatus"] != null && (bool)ViewData["ImportStatus"] == false)
   { %>
      <br/><h3>Imported candidates data not success</h3>
 <% } %>

  <br/>
    <p>
        <%: Html.ActionLink("Back Admin Home", "Index") %>
    </p>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
</asp:Content>
