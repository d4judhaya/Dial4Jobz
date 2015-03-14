<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Dial4Jobz.Models.ChannelPartner>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
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
                success: function (response) {
                    $("#loading").hide();
                    if (response.Success == true) {
                        $("#fancybox-overlay").css("display", "none");
                        $("#fancybox-wrap").css("display", "none");
                        alert(response.Message);
                        Databind("1"); // this function is in ManagePartner page
                    }
                    else {
                        alert(response.Message);
                    }
                }
            });
            return false;

        }    
    </script>

</head>
<body>
    <div style="width:650px;padding-left:20px;">
    <h2> Change Channel Partner Password</h2>
    <% using (Html.BeginForm("ChangePartnerPassword", "Channel", FormMethod.Post))
       { %>
              <%: Html.HiddenFor(model => model.Id)%>
              <div class="editor-label">
                  User Name 
              </div>
              <div class="editor-field">
                  <%= Model.UserName %>
              </div>
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

