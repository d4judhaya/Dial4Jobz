<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Dial4Jobz.Models.User>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>ChangePassword</title>
    <script type="text/javascript">
        function Validation() {
            if ($("#Pwd").val() == "") {
                alert("Password is required");
                $("#Pwd").focus();
                return false;
            }

            if ($("#Pwd").val() != "") {
                if ($("#Pwd").val().length < 5) {
                    alert("Password should contain atleast 5 characters");
                    $("#Pwd").focus();
                    return false;
                }
            }            

            UpdateAdmin();
        }

        function UpdateAdmin() {
            $("#loading").show();
            var $popup = $("#fancybox-outer");
            var form = $popup.find("form");
            var data = form.serialize();
            var url = form.attr('action');

            $.ajax({
                type: "POST",
                url: url,
                data: data,
                dataType: "json",
                success: function (data) {
                    $("#loading").hide();
                    if (data.error != "1") {
                        $("#fancybox-overlay").css("display", "none");
                        $("#fancybox-wrap").css("display", "none");
                        alert(data.message);
                        Databind("1"); // this function is in index page
                    }
                    else {
                        alert(data.message);
                    }
                }
            });
            return false;

        }    
    </script>

</head>
<body>
    <div style="width:650px;padding-left:20px;">
    <h2> Change Admin User Password</h2>
    <% using (Html.BeginForm("ChangePassword", "AdminPermission", FormMethod.Post))
       { %>
              <%: Html.HiddenFor(model => model.Id)%>
              <strong>User Name:</strong> <%= Model.UserName %>
              <div class="editor-label">
                  Password <span class="red">*</span>  
              </div>
              <div class="editor-field">
                   <%: Html.TextBox("Pwd")%> 
              </div>
              <input type="submit" id="contactbtn" value="Save" onclick="javascript:Validation();return false;" />  <br />      
    <% } %>
    </div>    
</body>
</html>
