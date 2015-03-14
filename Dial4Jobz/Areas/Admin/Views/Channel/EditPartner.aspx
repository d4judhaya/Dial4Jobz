<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<Dial4Jobz.Models.ChannelPartner>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Update</title>
    <script type="text/javascript">
        function Validation() {
            if ($("#UserName").val() == "") {
                alert("User Name is required");
                $("#UserName").focus();
                return false;
            }

            if ($("#Email").val() == "") {
                alert("Email is required");
                $("#Email").focus();
                return false;
            }

            if ($("#Email").val() != "") {
                var email = $("#Email").val();
                var valid = /^([a-zA-Z0-9_.-])+@([a-zA-Z0-9_.-])+\.([a-zA-Z])+([a-zA-Z])+/;
                if (!email.match(valid)) {
                    alert("Please Enter Valid Email Id");
                    $("#Email").focus();
                    return false;
                }
            }

            if ($("#ContactNo").val() == "") {
                alert("Contact No is required");
                $("#ContactNo").focus();
                return false;
            }

            if ($("#ContactNo").val() != "") {
                var Mobilenumber = $("#ContactNo").val()
                if (isNaN(Mobilenumber)) {
                    alert("Please Enter Valid Contact No");
                    $("#ContactNo").focus();
                    return false;
                }
            }

            if ($("#Address").val() == "") {
                alert("Address is required");
                $("#Address").focus();
                return false;
            }

            if ($("#Pincode").val() == "") {
                alert("Pincode is required");
                $("#Pincode").focus();
                return false;
            }

            UpdatePartner();
        }

        function UpdatePartner() {
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
                },
                error: function (xhr, status, error) {
                    $("#loading").hide();
                    alert(error);
                } 
            });
            return false;
        }
        

    </script>
</head>
<body>
    <div style="width:650px;padding-left:20px;">
    <h2> Update Channel Partner</h2>
    <% using (Html.BeginForm("EditPartner", "Channel", FormMethod.Post))
       { %>
              <%: Html.HiddenFor(model => model.Id)%>
              <div class="editor-label">
                  User Name <span class="red">*</span>  
              </div>
              <div class="editor-field">
                   <%: Html.TextBoxFor(model => model.UserName, new { maxlength = "50" })%> 
              </div>
              <div class="editor-label">
                  Email Id <span class="red">*</span>  
              </div>
              <div class="editor-field">
                   <%: Html.TextBoxFor(model => model.Email, new { maxlength = "50" })%> 
              </div>
              <div class="editor-label">
                  Contact No <span class="red">*</span>  
              </div>
              <div class="editor-field">
                   <%: Html.TextBoxFor(model => model.ContactNo, new { maxlength = "30" })%> 
              </div>
              <div class="editor-label">
                  Address <span class="red">*</span>  
              </div>
              <div class="editor-field">
                   <%: Html.TextBoxFor(model => model.Address, new { maxlength = "100" })%> 
              </div>
              <div class="editor-label">
                  Pincode <span class="red">*</span>  
              </div>
              <div class="editor-field">
                   <%: Html.TextBoxFor(model => model.Pincode, new { maxlength = "6" })%> 
              </div>           

              <input type="submit" id="contactbtn" value="Update" onclick="javascript:Validation();return false;" />  <br />      
    <% } %>
    </div>
</body>
</html>

