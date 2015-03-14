<%@ Page  Language="C#" Inherits="System.Web.Mvc.ViewPage<Dial4Jobz.Models.Candidate>" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>ContactCandidates</title>
    <script src="<%=Url.Content("~/Scripts/Dial4Jobz.Candidate.js")%>" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#no').click(function () {
                alert('This Job is not deleted');
                 window.location = "/Admin/AdminHome";
            });
        });
    </script>
    </head>
    <body>
        <div class="modal">
        <div class="modal-login-column">
            <div class="header">Delete</div>
             <%Html.BeginForm("DeleteCandidate", "CandidateReport", FormMethod.Post, new { @id = Model.Id });
                { %>
                    <h3>Are you sure you want to delete?</h3>
                    <input id="delete" type="submit" value="Delete" style="width:179px; height:26px; color:white; border-color:#00CC00 #007300 #007300 #00CC00;" />
                <%} %>
                 <input id="no" value="No" type="button" style="width:179px; height:26px; color:Black; background-color:Blue; border-color:#00CC00 #007300 #007300 #00CC00;" />
        </div>
        </div>
    </body>
    </html>

 



    



