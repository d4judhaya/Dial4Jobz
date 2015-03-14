<%@ Page  Language="C#" Inherits="System.Web.Mvc.ViewPage<Dial4Jobz.Models.Job>" %>

<%--<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Dial4Jobz.Models.Candidate>" %>--%>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Delete</title>
    <script src="<%=Url.Content("~/Scripts/Dial4Jobz.Job.js")%>" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#no').click(function () {
                alert('This Job is not deleted');
                window.location = "/Employer/PostedJobs";
            });
        });
    </script>
    </head>
    <body>
        <div class="modal">
        <div class="modal-login-column">
            <div class="header">Delete Confirmation</div>
             <%Html.BeginForm("Delete", "Jobs", FormMethod.Post, new { @id = "delete" });
                { %>
                    <h3>Are you sure you want to delete?</h3>
                    <input id="delete" type="submit" value="Delete" style="width:179px; height:26px; color:white; border-color:#00CC00 #007300 #007300 #00CC00;" />
                <%} %>
                 <input id="no" value="No" type="button" style="width:179px; height:26px; color:Black; background-color:Blue; border-color:#00CC00 #007300 #007300 #00CC00;" />
        </div>
        </div>
    </body>
    </html>

 



    



