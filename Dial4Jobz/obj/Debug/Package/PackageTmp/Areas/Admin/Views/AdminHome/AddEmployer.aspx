<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<Dial4Jobz.Models.Organization>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Dial4Jobz - Add Employer Profile
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    <script src="<%=Url.Content("~/Scripts/Dial4Jobz.Job.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts/Dial4Jobz.Employer.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts/PreventDoubleSubmit.js") %>" type="text/javascript"></script>
     <%--<script src="<%=Url.Content("~/Scripts/JquerySearchIt.js") %>" type="text/javascript"></script>--%>
     <script src="<%=Url.Content("~/Scripts/autocomplete.js") %>" type="text/javascript"></script>

    <script type="text/javascript">

        $(document).ready(function () {
            //$("#OwnershipType").hide();
            //$("#NumberOfEmployees").hide();
            $("#ConsultantIndustries").hide();

            $("input[name='EmployerType']").change(function () {
                //$("#Industries").show();
                var employerType = $('input[name="EmployerType"]:checked').val();
                // alert(employerType);
                if (employerType == 1) {
                    $("#ConsultantIndustries").show();
                    $("#Industries").hide();
                    $("#OwnershipType").hide();
                    $("#NumberOfEmployees").hide();
                }
                else if (employerType == 2) {
                    $("#Industries").show();
                    $("#OwnershipType").show();
                     $("#NumberOfEmployees").show();
                    $("#ConsultantIndustries").hide();
                }
            });

            $('#Name').autocomplete({
                source: function (request, response) {
                    $.getJSON("/Admin/AdminHome/getCompanyNameLists?term=" + request.term, function (data) {
                        response(data);
                    });
                },
                delay: 40
            });


            $("#Name").keyup(function (e) {
                string_a = $("#Name").val();
                string_b = string_a.replace(/ +(?= )/g, '');
                $(this).val(string_b);
                if (string_a.localeCompare(string_b) == 1) {
                    alert("Double Space Not allowed......");
                }
            });

            $("#MobileNumber").keydown(function (e) {
                if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 190]) !== -1) {
                    return;
                }
                if (((e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                    e.preventDefault();
                }
            });

            $("#ContactNumber").keydown(function (e) {
                if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 190]) !== -1) {
                    return;
                }
                if (((e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                    e.preventDefault();
                }
            });
        });
    
    </script>
    
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
	<% Html.RenderPartial("NavAdmin"); %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="mandatoryalign" align="right">
        <span class="red">*</span> Mandatory Fields
    </div>
    <div class="profileupdate">
        <%:TempData["Msg"] %>
    </div>
       
    <% Html.BeginForm("SaveEmployer", "AdminHome", FormMethod.Post, new { @id = "Save" }); %>

         <center><h3>Direct Verification</h3></center> 
           <a id="directverification"></a>
            <script type ="text/javascript">
                        

                $('#directverification').text('Direct Verification') // Sets text for link.
                .attr('href', '#');

                $("#directverification").click(function () {
                    alert("Verified Successfully");
                    $('#verifymobile').text('Direct Verification')
                .attr('href', 'GetMobileNumber?DirectEmployerVerification=' + $('#MobileNumber').val());
                });

                $(document).ready(function () {
                    $('#directverification').click(function () {
                        var name = $('#MobileNumber').val();
                        var data = 'mobileNumber=' + name;
                        $.ajax({
                            type: "GET",
                            url: "DirectEmployerVerification",
                            data: data,
                            success: function (data) {
                                //alert(data);

                            }
                        });
                        return false;
                    });
                });
         </script>

         <%: Html.Hidden("Id",Model.Id )   %>
         <%: Html.Hidden("orgnName", ViewData["orgnName"])%>

         <br />
        <h3>Employer Personal Details</h3>     
        
        <div class="editor-label">
            <%: Html.Label("Employer Type") %>
        </div>

        <div class="editor-field">
            <%:Html.RadioButtonFor(model=> model.EmployerType,1)%>  Individual
            <%:Html.RadioButtonFor(model=> model.EmployerType,2)%>  Company
        </div>

        <%if (Model.EmployerType == 1)
          { %>
        <%}
          else
          { %>
        <div id="NumberOfEmployees">
        <div class="editor-label">
            <%: Html.Label("No of Employees")%>
        </div>

        <div class="editor-field">
           <%: Html.DropDownList("NumberOfEmployees", "--Choose One--")%>
        </div>
        </div>
        <%} %>


         <%if (Model.EmployerType == 1)
          { %>
        <%}
          else
          { %>
        <div id="OwnershipType">
        <div class="editor-label">
            <%:Html.Label("Ownership Type") %>
        </div>

        <div  class="editor-field">
            <%:Html.DropDownList("OwnershipType","--Choose Ownership--") %>
        </div>
        </div>
          <%} %>
               
        <div class="editor-label">
           <%: Html.Label("Employer's Name")%>
           <span class="red">*</span>
        </div>
        <div class="editor-field">
            <%: Html.TextBox("Name", Model.Name, new { @title = "Enter the Organization's Name" })%>  
            <a id="checkCompany" href="#" >Validate Company</a> | <a id="getCompany"></a>

            <script type ="text/javascript">
                    $('#getCompany').text('Get Company') // Sets text for company.
                .attr('href', '#');
                        $('#deleteCompany').text('Delete Company') // Sets text for company.
                .attr('href', '#');

                    $("#getCompany").click(function () {
                        $('#getCompany').text('Get Company')
                .attr('href', 'GetCompanyDetail?validateCompany=' + $('#Name').val());
                    });

                    $("#Name").blur(function () {
                        $("#checkCompany").trigger('click');
                    });

                    $("#deleteCompany").click(function () {
                        var name = $('#Name').val();
                        var Id = $('#Id').val();
                        $('#deleteCompany').text('Delete Company')
                .attr('href', 'DeleteCompanyDetail?userName=' + name + '=' + Id);
                    });

                    $(document).ready(function () {
                        $('#checkCompany').click(function () {

                            var name = $.trim($('#Name').val());
                            var data = 'validateCompany=' + name;
                            $.ajax({
                                type: "GET",
                                url: "ValidateCompany",
                                data: data,
                                success: function (data) {
                                    alert(data);
                                }
                            });
                            return false;
                        });
                    });

     </script>
        </div>
                          
        <div class="editor-label">
           <%: Html.Label("Industry")%>
           <span class="red">*</span>
        </div>
        <div class="editor-field">
       <%-- <%if (Model.IndustryId != 2378)
          { %>--%>
             <%: Html.DropDownList("Industries", "Select")%> 
        <%--<%} else { %>--%>
            <%: Html.DropDownList("ConsultantIndustries", "Select")%>
    
            <%--<%: Html.DropDownList("Industries", "Select")%> 
        <%} %>--%>
        
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
         <%-- <span class="red">*</span>--%>
        </div>
        <div class="editor-field">
            <%: Html.TextBox("Email", Model.Email, new { @title = "Enter the organization's email" })%> 

        <%--Get the company detail using existing email--%>

            <a id="checkEmail" href="#" ></a> | <a id="getEmail"></a> 

      <script type="text/javascript">
          $('#getEmail').text('Get Profile') // Sets text for email.
                            .attr('href', '#');

          $("#Email").blur(function () {
              $('#checkEmail').trigger('click');

              $('#getEmail').text('Get Profile')
                                .attr('href', 'GetCompanyDetail?validateEmail=' + $('#Email').val());
          });

          $(document).ready(function () {

              $('#checkEmail').blur(function () {
                  var name = $('#Email').val();
                  var status = '';
                  var split = name.split(' ');
                  var mail = split[1];
                  if (mail != null) {
                      alert('Enter correct emailid');
                      return false;
                  }

                  else if (name != null) {  //remove
                      var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
                      if (filter.test(name)) { //upto


                          var data = 'validateEmail=' + name;
                          $.ajax({
                              type: "GET",
                              url: "ValidateCompany",
                              data: data,
                              success: function (data) {
                                  if (data == "Email not exist") {
                                      alert("Email not exist");
                                      return false;
                                  } else if (data == "Email exist") {
                                      document.location.href = "/Admin/AdminHome/GetCompanyDetail?validateEmail=" + name;
                                      alert("Email exist");
                                  } else {
                                      alert("Email not exist");
                                      document.location.href = "/Admin/AdminHome/AddEmployer";
                                      return false;
                                  }

                              }

                          });
                      } //remove
                      else {
                          alert("Enter Valid Emil ID");
                          return false;
                      } //remove

                  }


                  return false;
              });

          });
                    
            </script>

            <%--End get company detail--%>

            
            <%--verify the email using the following--%>
            
            <a id="verifyemail"></a>
            <script type ="text/javascript">
                $('#verifyemail').text('Verify Email') // Sets text for company.
                .attr('href', '#');

                $("#verifyemail").click(function () {
                    alert("Verification sent successfully");
                    $('#verifyemail').text('Verify Email')
                .attr('href', 'GetEmail?emailVerification=' + $('#Email').val());
                });

                $(document).ready(function () {
                    $('#verifyemail').click(function () {
                        var name = $('#Email').val();
                        var data = 'email=' + name;
                        $.ajax({
                            type: "GET",
                            url: "EmailVerification",
                            data: data,
                            success: function (data) {
                                //alert("");

                            }
                        });
                        return false;
                    });
                });
         </script>

         <%--end verification--%>
                          
            <%if (Model.IsMailVerified ==true) { %>
                       <%--<img src="../../Content/Images/Tick.png" class="btn" />
                        Verified--%>
                        <img src="../../Content/Images/green_round_tick_sign_4246.jpg" width="14px" height="12px" alt="Verified" />Verified
                <% } else { %>
                    <span class="red">Not verified</span>
            <% } %>
        </div>

        <div class="editor-label">
            <%: Html.Label("Mobile Number")%>
            <span class="red">*</span>
        </div>
        <div class="editor-field">
            <%: Html.TextBox("MobileNumber", Model.MobileNumber, new { @title = "Enter the organization's mobile number", @maxlength = "10" })%>  

            <a id="checkMobile" href="#">Validate Mobile</a>| <a id="getMobile"></a>
            
                <script type ="text/javascript">
                    $('#getMobile').text('Get Profile') // Sets text for mobile.
                    .attr('href', '#');

                    $("#MobileNumber").blur(function () {
                        $('#checkMobile').trigger('click');
                        $('#getMobile').text('Get Profile')
                         .attr('href', 'GetCompanyDetail?validateMobileNumber=' + $('#MobileNumber').val());
                    });

                    $(document).ready(function () {
                        $('#checkMobile').click(function () {
                            var name = $('#MobileNumber').val();
                            var data = 'validateMobileNumber=' + name;
                            $.ajax({
                                type: "GET",
                                url: "ValidateCompany",
                                data: data,
                                success: function (data) {
                                    if (data == "Mobile not exist") {
                                        alert("Mobile not exist");
                                        return false;
                                    } else if (data == "Mobile exist") {
                                        document.location.href = "/Admin/AdminHome/GetCompanyDetail?validateMobileNumber=" + name;
                                        alert("Mobile Number exist");
                                    } else {
                                        alert("Mobile Number not exist");
                                        //document.location.href = "/Admin/AdminHome/AddEmployer";
                                        return false;
                                    }

                                }
                            });
                            return false;
                        });
                    });

            </script>

            <%--Mobile verification start--%>

            <a id="verifymobile"  ></a>
            <script type ="text/javascript">
                $('#verifymobile').text('|Verify Mobilenumber|') // Sets text for company.
                .attr('href', '#');

                $("#verifymobile").click(function () {
                    alert("Verification code sent successfully");
                    $('#verifymobile').text('Verify MobileNumber')
                .attr('href', 'GetMobileNumber?verifyMobileNumber=' + $('#MobileNumber').val());
                });

                $(document).ready(function () {
                    $('#verifymobile').click(function () {
                        var name = $('#MobileNumber').val();
                        var data = 'mobileNumber=' + name;
                        $.ajax({
                            type: "GET",
                            url: "VerifyMobileNumber",
                            data: data,
                            success: function (data) {
                                //alert(data);

                            }
                        });
                        return false;
                    });
                });
         </script>

            <%if (Model.IsPhoneVerified ==true) { %>
                <%--<a><img src="../../Content/Images/Tick.png" class="btn" /></a>
                   Verified--%>
                   <img src="../../Content/Images/green_round_tick_sign_4246.jpg" width="14px" height="12px" alt="Verified" />Verified
                <% } else { %>
                    <span class="red">Not verified</span>
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
          <%: Html.TextBox("ContactNumber", Model.ContactNumber, new { @title = "Enter the organization's contact number", @maxlength = "10" })%>    
        </div>

        <div class="editor-label">
            <%:Html.Label("Enter Verification Code") %>
        </div>
        <div class="editor-field" style="width:50px;">
            <%: Html.TextBox("PhVerificationNo", null, new { @title = "Enter the Verification Code" })%>

             <a id="confirmverify"  ></a>

        <script type ="text/javascript">
            $('#confirmverify').text('Confirm') // Sets text for company.
                .attr('href', '#');

            $("#confirmverify").click(function () {
                alert("Verified Successfully");
                $('#confirmverify').text('Confirm')
                .attr('href', 'GetMobileNumber?confirmMobileVerification=' + $('#PhVerificationNo').val());
            });

            $(document).ready(function () {
                $('#confirmverify').click(function () {
                    var name = $('#PhVerificationNo').val();
                    var data = 'confirmCode=' + name;
                    $.ajax({
                        type: "GET",
                        url: "ConfirmMobileVerification",
                        data: data,
                        success: function (data) {
                            alert("Verified");
                        }
                    });
                    return false;
                });
            });
         </script>

         <%--Mobile verification End--%>

       </div> 
          <input id="Save" type="submit" value="Update Profile" class="btn" title="Click to save changes" onclick="javascript:Dial4Jobz.Employer.Save(this);return false;"  />
        <% Html.EndForm(); %>

</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">
    <div class="section larger">   
       <% Html.RenderPartial("AdminEmployer"); %>     
   </div> 
  
</asp:Content>

