    <%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Dial4Jobz.Models.Job>" %>

    <% Dial4Jobz.Models.Organization loggedInOrganization = (Dial4Jobz.Models.Organization)ViewData["LoggedInOrganization"]; %>
    <% Dial4Jobz.Models.Consultante LoggedInConsultants = (Dial4Jobz.Models.Consultante)ViewData["LoggedInConsultant"]; %>
    <% Dial4Jobz.Models.Repositories.Repository _repository = new Dial4Jobz.Models.Repositories.Repository(); %>

     <% Session["OrganizationId"] = Request.QueryString["organizationId"]; %>

      <% var organizationId = Convert.ToInt32(Session["OrganizationId"]); %>

      <% var organization = _repository.GetOrganization(organizationId); %>
   

    <h2>Requirement Summary</h2>
     <div class="mandatoryalign" align="right">
        <span class="red">*</span> Mandatory Fields
     </div>
     
    <div class="editor-label">
        <%: Html.Label("Position / Title of Vacancy")%> <span class="red">*</span>  
    </div>
    <div class="editor-field">
        <%: Html.TextBox("Position", Model.Position, new { @title = "Enter the name of this position" })%>
    </div>
        

      <script type="text/javascript">
          $(document).ready(function () {

              //for country state cities othercountry check box count.
              checkboxCount();

              //Cache these as variables so we only have to select once
              var $min = $("#ddlAnnualSalaryLakhsMin");
              var $minThousands = $("#ddlAnnualSalaryThousandsMin");
              var $max = $("#ddlAnnualSalaryLakhs");
              var $maxThousands = $("#ddlAnnualSalaryThousands");
              var $msg = $("#message");

              //Apply a single change event to fire on either dropdown
              $min.add($max).change(function () {

                  //Have some default text to display, an empty string
                  var text = "";

                  //Cache the vales as variables so we don't have to keep getting them
                  //We will parse the numbers out of the string values
                  var minVal = parseInt($min.val(), 10);
                  var maxVal = parseInt($max.val(), 10);
                  var minThousands = parseInt($minThousands.val(), 10);
                  var maxThousands = parseInt($maxThousands.val(), 10);
                  var totalLacks = (minVal * 100000) + (minThousands * 1000);
                  var totalMaxLacks = (maxVal * 100000) + (maxThousands * 1000);

                  //Determine if both are numbers, if so then they both have values
                  var bothHaveValues = !isNaN(totalLacks) && !isNaN(totalMaxLacks);

                  if (bothHaveValues) {
                      if (totalLacks >= totalMaxLacks) {
                          text += 'Minimum Salary should be less than maximum';
                      } else if (totalMaxLacks <= totalLacks) {
                          text += 'Maximum Salary should be greater than minimum';
                      }
                  }

                  //Display the text
                  $msg.html(text);
              });



              $.get("/Jobs/getBasicQualification/", function (data) {

                  //Developer Note: For Exact Match check process.....
                  var basiQualificationIds = $("#basiQualificationIds").val();
                  var basiQualificationArray = new Array();
                  basiQualificationArray = basiQualificationIds.split(",");

                  $.each(data.BasicQualification, function (key, value) {

                      if (key == 0) {
                          $("#BasicQualificationDiv").append("<input type='checkbox' name='checkAllBasicQualification' checked='checked' value='' /> Any <br />");
                      }

                      if ($.inArray("" + value.Id + "", basiQualificationArray) != -1) { //$("#basiQualificationIds").val().indexOf(value.Id) != -1) {
                          $("#BasicQualificationDiv").append("<input type='checkbox' checked='checked' id='BasicQualificationArea_" + value.Id + "' name='basicQualification' onclick='checkBasic(" + value.Id + ")' value='" + value.Id + "' /> " + value.Name + " <br />");
                          checkBasic(value.Id);
                      }
                      else {
                          $("#BasicQualificationDiv").append("<input type='checkbox' id='BasicQualificationArea_" + value.Id + "' name='basicQualification' onclick='checkBasic(" + value.Id + ")' value='" + value.Id + "' /> " + value.Name + " <br />");
                      }

                  });
              });

              $.get("/Jobs/getPostGraduation/", function (data) {

                  //Developer Note: For Exact Match check process.....
                  var postGraduationIds = $("#postGraduationIds").val();
                  var postGraduationArray = new Array();
                  postGraduationArray = postGraduationIds.split(",");

                  $.each(data.PostGraduation, function (key, value) {
                      if (key == 0) {
                          $("#PostGraduationDiv").append("<input type='checkbox' name='checkAllPostgraduation' checked='checked' value='' /> Any <br />");
                          $("#PostGraduationDiv").append("<input type='checkbox' name='checkAllPostgraduation' checked='checked' value='0' /> None <br />");
                      }

                      if ($.inArray("" + value.Id + "", postGraduationArray) != -1) { //"#postGraduationIds").val().indexOf(value.Id) != -1) {
                          $("#PostGraduationDiv").append("<input type='checkbox' checked='checked' id='PostGraduationArea_" + value.Id + "' name='PostGraduation' onclick='checkPost(" + value.Id + ")' value='" + value.Id + "' /> " + value.Name + " <br />");
                          checkPost(value.Id);
                      }
                      else {
                          $("#PostGraduationDiv").append("<input type='checkbox' id='PostGraduationArea_" + value.Id + "' name='PostGraduation' onclick='checkPost(" + value.Id + ")' value='" + value.Id + "' /> " + value.Name + " <br />");
                      }

                  });
              });

              $.get("/Jobs/getDoctorate/", function (data) {

                  //Developer Note: For Exact Match check process.....
                  var doctorateIds = $("#doctorateIds").val();
                  var doctorateArray = new Array();
                  doctorateArray = doctorateIds.split(",");


                  $.each(data.Doctorate, function (key, value) {
                      if (key == 0) {
                          $("#DoctrateDiv").append("<input type='checkbox' name='checkAllDoctrate' checked='checked' value='' /> Any <br />");
                          $("#DoctrateDiv").append("<input type='checkbox' name='checkAllDoctrate' checked='checked' value='0' /> None <br />");
                      }

                      if ($.inArray("" + value.Id + "", doctorateArray) != -1) { //$("#doctorateIds").val().indexOf(value.Id) != -1) {
                          $("#DoctrateDiv").append("<input type='checkbox' checked='checked' id='DoctrateArea_" + value.Id + "' name='Doctrate' onclick='checkDoc(" + value.Id + ")' value='" + value.Id + "' /> " + value.Name + " <br />");
                          checkDoc(value.Id);
                      }
                      else {
                          $("#DoctrateDiv").append("<input type='checkbox' id='DoctrateArea_" + value.Id + "' name='Doctrate' onclick='checkDoc(" + value.Id + ")' value='" + value.Id + "' /> " + value.Name + " <br />");
                      }

                  });
              });

              $.get("/Jobs/getotherCountry/152", function (data) {

                  //Developer Note: For Exact Match check process.....
                  var CountryIds = $("#CountryIds").val();
                  var CountryArray = new Array();
                  CountryArray = CountryIds.split(",");
                  //$.inArray("" + value.Id + "", postGraduationSpecializationArray) != -1
                  $.each(data.OtherCountry, function (key, value) {

                      if ($.inArray("" + value.Id + "", CountryArray) != -1) { //.indexof(value.Id)
                          $("#OtherCountryDiv").append("<input type='checkbox' checked='checked' id='OtherCountry_" + value.Id + "' name='PostingOtherCountry' onclick='checkOtherCountry()' value='" + value.Id + "' /> " + value.Name + " <br />");
                          checkOtherCountry();
                      }
                      else {
                          $("#OtherCountryDiv").append("<input type='checkbox' id='OtherCountry_" + value.Id + "' name='PostingOtherCountry' onclick='checkOtherCountry()' value='" + value.Id + "' /> " + value.Name + " <br />");
                      }
                  });
              });

              if ($("#CountryIds").val().indexOf("152") != -1) {
                  checkCountry(152);
              }

              $('#txtBasicQualifications').click(function (e) {
                  if (document.getElementById("BasicQualificationAreaDiv").style.display == "none") {
                      document.getElementById("BasicQualificationAreaDiv").style.display = "block";
                      e.stopPropagation();
                  }
                  else {
                      document.getElementById("BasicQualificationAreaDiv").style.display = "none";
                  }
              });

              $('#txtPostGraduation').click(function (e) {
                  if (document.getElementById("PostGraduationAreaDiv").style.display == "none") {
                      document.getElementById("PostGraduationAreaDiv").style.display = "block";
                      e.stopPropagation();
                  }
                  else {
                      document.getElementById("PostGraduationAreaDiv").style.display = "none";
                  }
              });

              $('#txtDoctrate').click(function (e) {
                  if (document.getElementById("DoctrateAreaDiv").style.display == "none") {
                      document.getElementById("DoctrateAreaDiv").style.display = "block";
                      e.stopPropagation();
                  }
                  else {
                      document.getElementById("DoctrateAreaDiv").style.display = "none";
                  }
              });

              $('#txtCountry').click(function (e) {
                  if (document.getElementById("CountryDiv").style.display == "none") {
                      document.getElementById("CountryDiv").style.display = "block";
                      e.stopPropagation();
                  }
                  else {
                      document.getElementById("CountryDiv").style.display = "none";
                  }
              });

              $('#txtState').click(function (e) {
                  if (document.getElementById("StateDiv").style.display == "none") {
                      document.getElementById("StateDiv").style.display = "block";
                      e.stopPropagation();
                  }
                  else {
                      document.getElementById("StateDiv").style.display = "none";
                  }
              });

              $('#txtCity').click(function (e) {
                  if (document.getElementById("CityDiv").style.display == "none") {
                      document.getElementById("CityDiv").style.display = "block";
                      e.stopPropagation();
                  }
                  else {
                      document.getElementById("CityDiv").style.display = "none";
                  }

              });

              $('#txtRegion').click(function (e) {
                  if (document.getElementById("RegionDiv").style.display == "none") {
                      document.getElementById("RegionDiv").style.display = "block";
                      e.stopPropagation();
                  }
                  else {
                      document.getElementById("RegionDiv").style.display = "none";
                  }
              });

              $('#txtOtherCountry').click(function (e) {
                  if (document.getElementById("OtherCountryDiv").style.display == "none") {
                      document.getElementById("OtherCountryDiv").style.display = "block";
                      e.stopPropagation();
                  }
                  else {
                      document.getElementById("OtherCountryDiv").style.display = "none";
                  }
              });


              $('#BasicQualificationAreaDiv,#PostGraduationAreaDiv,#DoctrateAreaDiv,#CountryDiv,#StateDiv,#CityDiv,#RegionDiv,#OtherCountryDiv').click(function (e) {
                  e.stopPropagation();
              });

              $(document).click(function () {

                  checkboxCount();
                  $("#BasicQualificationAreaDiv,#PostGraduationAreaDiv,#DoctrateAreaDiv,#CountryDiv,#StateDiv,#CityDiv,#RegionDiv,#OtherCountryDiv").css("display", "none");
              });

              //for country state cities othercountry check box count.
              $("input[type=checkbox]").click(function () {
                  checkboxCount();
              });

          });

          //for country state cities othercountry check box count.
          function checkboxCount() {
              var StatesCount = 0;
              var CitiesCount = 0;
              var RegionCount = 0;
              var OtherCountryCount = 0;
              $("input[type=checkbox]").click(function () {
                  var val = $(this).val();
                  OtherCountryCount = 0;
                  $("input:checkbox[name=PostingOtherCountry]:checked").each(function () {
                      OtherCountryCount = OtherCountryCount + 1;
                  });
                  debugger;
                  if (OtherCountryCount > 5 && !(OtherCountryCount == 142)) {
                      debugger;
                      if (OtherCountryCount > 6) {
                          alert("You can Select Only five Country or Please choose Any");
                          var g = 0;
                          $("input:checkbox[name=PostingOtherCountry]:checked").each(function () {
                              g = g + 1;
                          });
                          $("#txtOtherCountry").val("- Selected Other Countries ( " + g + " ) -");

                      } else {
                          debugger;
                          alert("You can Select Only five Country");
                          $("input:checkbox[id=OtherCountry_" + val + "]").each(function () {
                              this.checked = false;
                              var g = 0;
                              $("input:checkbox[name=PostingOtherCountry]:checked").each(function () {
                                  g = g + 1;
                              });
                              $("#txtOtherCountry").val("- Selected Other Countries ( " + g + " ) -");
                          });
                      }
                      OtherCountryCount = 0;
                  }
                  OtherCountryCount = 0;

                  $("input:checkbox[class=ddlcheckbox]:checked").each(function () {
                      StatesCount = StatesCount + 1;
                  });
                  if (StatesCount > 5) {
                      alert("You can select only five States");
                      $("input:checkbox[id=PostingState_" + val + "]").each(function () {
                          this.checked = false;
                          checkState(val);
                          SelectedStateCount();

                      });
                  }
                  StatesCount = 0;
                  $("input:checkbox[class=ddlcitycheckbox]:checked").each(function () {
                      CitiesCount = CitiesCount + 1;
                  });
                  if (CitiesCount > 5) {
                      alert("You can select only five Cities");
                      $("input:checkbox[id=PostingCity_" + val + "]").each(function () {
                          this.checked = false;
                          checkCity(val);
                          SelectedCityCount();

                      });
                  }
                  CitiesCount = 0;
                  $("input:checkbox[class=ddlregioncheckbox]:checked").each(function () {
                      RegionCount = RegionCount + 1;
                  });
                  if (RegionCount > 5) {
                      alert("You can select only five Region");
                      $("input:checkbox[id=PostingRegion_" + val + "]").each(function () {
                          this.checked = false;
                          SelectedRegionCount();

                      });
                  }
                  RegionCount = 0;
              });
          }

          function checkBasic(val) {
              //console.log(val);
              var g = 0;
              $("input:checkbox[name=basicQualification]:checked").each(function () {
                  g = g + 1;
              });

              var specLength = $("input:checkbox[class=BasicQualificationSpecialization ddlcheckbox]:checked").length;

              $("#txtBasicQualifications").val("- Selected Basic Qualifications ( " + g + " )( " + specLength + " ) -");
              if (g == 0) {
                  $("input:checkbox[name=checkAllBasicQualification]").each(function () {
                      this.checked = true;
                  });
              }
              else {
                  $("input:checkbox[name=checkAllBasicQualification]").each(function () {
                      this.checked = false;
                  });
              }
              getSpecialization(val, "BasicQualification", 0); //degree value, Qualification Prefix for naming objects,degree type
          }



          function checkPost(val) {
              var g = 0;
              $("input:checkbox[name=PostGraduation]:checked").each(function () {
                  g = g + 1;
              });

              var specLength = $("input:checkbox[class=PostGraduationSpecialization ddlcheckbox]:checked").length;

              $("#txtPostGraduation").val("- Selected Post Graduations ( " + g + " )( " + specLength + " ) -");
              if (g == 0) {
                  $("input:checkbox[name=checkAllPostgraduation]").each(function () {
                      this.checked = true;
                  });
              }
              else {
                  $("input:checkbox[name=checkAllPostgraduation]").each(function () {
                      this.checked = false;
                  });
              }
              getSpecialization(val, "PostGraduation", 1);
          }

          function checkDoc(val) {
              var g = 0;
              $("input:checkbox[name=Doctrate]:checked").each(function () {
                  g = g + 1;
              });

              var specLength = $("input:checkbox[class=DoctrateSpecialization ddlcheckbox]:checked").length;
              $("#txtDoctrate").val("- Selected Doctrate ( " + g + " )( " + specLength + " ) -");
              if (g == 0) {
                  $("input:checkbox[name=checkAllDoctrate]").each(function () {
                      this.checked = true;
                  });
              }
              else {
                  $("input:checkbox[name=checkAllDoctrate]").each(function () {
                      this.checked = false;
                  });
              }
              getSpecialization(val, "Doctrate", 2);
          }

          function getSpecialization(degreeId, qualificationPrefix, degreeType) {

              var currentSpecObjId = "#" + qualificationPrefix + "Specialization_" + degreeId + "";

              if (document.getElementById("" + qualificationPrefix + "Area_" + +degreeId + "").checked) {

                  $.get("/Search/getSpecialization/" + degreeId, function (data) {

                      //Developer Note: For Exact Match check process.....
                      var basiQualificationSpecializationIds = $("#basiQualificationSpecializationIds").val();
                      var basiQualificationArray = new Array();
                      basiQualificationArray = basiQualificationSpecializationIds.split(",");
                      //$.inArray("" + value.Id + "", basiQualificationArray) != -1

                      //Developer Note: For Exact Match check process.....
                      var postGraduationSpecializationIds = $("#postGraduationSpecializationIds").val();
                      var postGraduationSpecializationArray = new Array();
                      postGraduationSpecializationArray = postGraduationSpecializationIds.split(",");
                      //$.inArray("" + value.Id + "", postGraduationSpecializationArray) != -1

                      //Developer Note: For Exact Match check process.....
                      var doctorateSpecializationIds = $("#doctorateSpecializationIds").val();
                      var doctorateSpecializationArray = new Array();
                      doctorateSpecializationArray = doctorateSpecializationIds.split(",");
                      //$.inArray("" + value.Id + "", doctorateSpecializationArray) != -1


                      //console.log(data);
                      $.each(data.Specialization, function (key, value) {

                          if (value.Id == -1) {
                              $("#" + qualificationPrefix + "SpecializationDiv").find("div.specialization-container").append("<div id='" + qualificationPrefix + "Specialization_" + degreeId + "'></div>");
                              $(currentSpecObjId).append("<strong style='line-height:20px !important;'>-- " + value.Name + " --</strong> <br />");
                          }
                          else {

                              if ($.inArray("" + value.Id + "", basiQualificationArray) != -1 && degreeType == 0) {
                                  $(currentSpecObjId).append("<input type='checkbox' checked='checked' onclick='checkSpecialization(" + degreeType + ")' class='" + qualificationPrefix + "Specialization ddlcheckbox' name='" + qualificationPrefix + "Specialization_" + degreeId + "' value='" + value.Id + "' /> <div class='right'>" + value.Name + "</div> <br />");
                                  checkSpecialization(degreeType);
                              }
                              else if ($.inArray("" + value.Id + "", postGraduationSpecializationArray) != -1 && degreeType == 1) {
                                  $(currentSpecObjId).append("<input type='checkbox' checked='checked' onclick='checkSpecialization(" + degreeType + ")' class='" + qualificationPrefix + "Specialization ddlcheckbox' name='" + qualificationPrefix + "Specialization_" + degreeId + "' value='" + value.Id + "' /> <div class='right'>" + value.Name + "</div> <br />");
                                  checkSpecialization(degreeType);
                              }
                              else if ($.inArray("" + value.Id + "", doctorateSpecializationArray) != -1 && degreeType == 2) {
                                  $(currentSpecObjId).append("<input type='checkbox' checked='checked' onclick='checkSpecialization(" + degreeType + ")' class='" + qualificationPrefix + "Specialization ddlcheckbox' name='" + qualificationPrefix + "Specialization_" + degreeId + "' value='" + value.Id + "' /> <div class='right'>" + value.Name + "</div> <br />");
                                  checkSpecialization(degreeType);
                              }
                              else {
                                  $(currentSpecObjId).append("<input type='checkbox' onclick='checkSpecialization(" + degreeType + ")' class='" + qualificationPrefix + "Specialization ddlcheckbox' name='" + qualificationPrefix + "Specialization_" + degreeId + "' value='" + value.Id + "' /> <div class='right'>" + value.Name + "</div> <br />");
                              }
                          }

                      });
                  });

              }
              else {

                  $(currentSpecObjId).remove();
                  checkSpecialization(degreeType);
              }

          }

          function checkSpecialization(degreeType) {

              var qualificationName = '', specializationName = '', lengthContainer = '', prefix = '';
              switch (degreeType) {
                  case (0):
                      qualificationName = "basicQualification";
                      specializationName = "BasicQualificationSpecialization";
                      lengthContainer = "txtBasicQualifications";
                      prefix = 'Basic Qualifications';
                      break;
                  case (1):
                      qualificationName = "PostGraduation";
                      specializationName = "PostGraduationSpecialization";
                      lengthContainer = "txtPostGraduation";
                      prefix = "Post Graduations";
                      break;
                  case (2):
                      qualificationName = "Doctrate";
                      specializationName = "DoctrateSpecialization";
                      lengthContainer = "txtDoctrate";
                      prefix = "Doctrate";
                      break;
              }
              
              var qualificationCheckedLength = $("input:checkbox[name=" + qualificationName + "]:checked").length;
              var specializationCheckedLength = $("input:checkbox[class=" + specializationName + " ddlcheckbox]:checked").length;

              $("#" + lengthContainer + "").val("- Selected " + prefix + " ( " + qualificationCheckedLength + " )( " + specializationCheckedLength + " ) -");
          }

          function checkOtherCountry() {
              var g = 0;
              $("input:checkbox[name=PostingOtherCountry]:checked").each(function () {
                  g = g + 1;
              });
              $("#txtOtherCountry").val("- Selected Other Countries ( " + g + " ) -");
          }

          function checkCountry(val) {
              var selected = "-1,";
              $("input:checkbox[class=ddlcountry]:checked").each(function () {
                  selected += $(this).val() + ",";
              });
              var selectedArray = new Array();
              selectedArray = selected.split(",");
              
              if ($.inArray("" + val + "", selectedArray) != -1) {

                  //$("input:checkbox[name=PostingCountry]").attr("checked", "false");
                  $("#stateDiv_" + val + "").remove();
                  $("#txtState").val("- Selected States ( " + 0 + " ) -");
                  $("#txtCity").val("- Selected Cities ( " + 0 + " ) -");
                  $("#txtRegion").val("- Selected Regions ( " + 0 + " ) -");

                  if (document.getElementById("PostingCountry_" + val + "").checked) {
                      
                      $.get("/Jobs/getstatebyCountryId/" + val, function (data) {

                          //Developer Note: For Exact Match check process.....
                          var StateIds = $("#StateIds").val();
                          var StateArray = new Array();
                          StateArray = StateIds.split(",");
                          
                          $.each(data.State, function (key, value) {
                              
                              if (value.Id == "-1") {
                                  $("#StateDiv").append("<div id='stateDiv_" + val + "' ></div>");
                                  $("#stateDiv_" + val + "").append("<strong style='line-height:20px !important;'>-- " + value.Name + " --</strong> <br />");
                              }
                              else {
                                  if ($.inArray("" + value.Id + "", StateArray) != -1) {
                                      $("#stateDiv_" + val + "").append("<input type='checkbox' checked='checked' id='PostingState_" + value.Id + "' onclick='checkState(" + value.Id + ")' class='ddlcheckbox' name='PostingState' value='" + value.Id + "' />" + value.Name + "<br />");
                                      checkState(value.Id);
                                  }
                                  else {
                                      $("#stateDiv_" + val + "").append("<input type='checkbox' id='PostingState_" + value.Id + "' onclick='checkState(" + value.Id + ")' class='ddlcheckbox' name='PostingState' value='" + value.Id + "' />" + value.Name + "<br />");
                                  }
                              }
                          });
                      });
                      SelectedStateCount();

                  }
              }
              else {
                  var remove = "-1,";
                  $("input:checkbox[class=ddlcheckbox]:checked").each(function () {
                      remove += $(this).val() + ",";
                  });
                  var removeArray = new Array();
                  removeArray = remove.split(",");
                  
                  var countryId = val;
                  $.get("/Jobs/getstatebyCountryId/" + countryId, function (data) {
                      
                      $.each(data.State, function (key, value) {
                          if (value.Id == "-1") {
                              //$("#StateDiv").append("<div id='stateDiv_" + val + "' ></div>");
                              $("#stateDiv_" + countryId + "").remove();
                          }
                          else {

                              if ($.inArray("" + value.Id + "", removeArray) != -1) {
                                  $("#stateDiv_" + countryId + "").append("<input type='checkbox' checked='checked' id='PostingState_" + value.Id + "' onclick='checkState(" + value.Id + ")' class='ddlcheckbox' name='PostingState' value='" + value.Id + "' />" + value.Name + "<br />");
                                  $("#stateDiv_" + countryId + "").remove();
                                  
                                  checkState(value.Id);
                              }
                              else {
                                  $("#stateDiv_" + countryId + "").append("<input type='checkbox' id='PostingState_" + value.Id + "' onclick='checkState(" + value.Id + ")' class='ddlcheckbox' name='PostingState' value='" + value.Id + "' />" + value.Name + "<br />");
                                  $("#stateDiv_" + countryId + "").remove();
                              }
                          }
                      });

                      $("#stateDiv_" + countryId + "").remove();
                      SelectedStateCount();

                  });
                  SelectedCountryCount();
                  SelectedStateCount();

              }

              SelectedCountryCount();

          }

          function checkState(val) {
              var selected = "-1,";
              $("input:checkbox[class=ddlcheckbox]:checked").each(function () {
                  selected += $(this).val() + ",";
              });
              
              var selectedArray = new Array();
              selectedArray = selected.split(",");
              
              if ($.inArray("" + val + "", selectedArray) != -1) {
                  
                  $("#cityDiv_" + val + "").remove();


                  if (document.getElementById("PostingState_" + val + "").checked) {

                      $.get("/Jobs/getCitybyStateId/" + val, function (data) {
                          
                          //Developer Note: For Exact Match check process.....
                          var CityIds = $("#CityIds").val();
                          var CityArray = new Array();
                          CityArray = CityIds.split(",");
                          
                          $.each(data.City, function (key, value) {
                              if (value.Id == "-1") {
                                  $("#CityDiv").append("<div id='cityDiv_" + val + "' ></div>");
                                  $("#cityDiv_" + val + "").append("<strong style='line-height:20px !important;'>-- " + value.Name + " --</strong> <br />");
                              }
                              else {

                                  if ($.inArray("" + value.Id + "", CityArray) != -1) {
                                      $("#cityDiv_" + val + "").append("<input type='checkbox' checked='checked' name='PostingCity" + val + "' id='PostingCity_" + value.Id + "' onclick='checkCity(" + value.Id + ")' class='ddlcitycheckbox' value='" + value.Id + "' />" + value.Name + "<br />");
                                      
                                      checkCity(value.Id);
                                  }
                                  else {
                                      $("#cityDiv_" + val + "").append("<input type='checkbox' name='PostingCity" + val + "' id='PostingCity_" + value.Id + "' onclick='checkCity(" + value.Id + ")' class='ddlcitycheckbox' value='" + value.Id + "' />" + value.Name + "<br />");
                                  }
                              }
                          });
                      });
                  }
              }
              else {
                  var remove = "-1,";
                  $("input:checkbox[class=ddlcitycheckbox]:checked").each(function () {
                      remove += $(this).val() + ",";
                  });
                  var removeArray = new Array();
                  removeArray = remove.split(",");

                  
                  var state = val;
                  $.get("/Jobs/getCitybyStateId/" + state, function (data) {
                      $.each(data.City, function (key, value) {
                          if (value.Id == "-1") {
                              //$("#CityDiv").append("<div id='cityDiv_" + state + "' ></div>");
                              $("#cityDiv_" + state + "").remove();
                          }
                          else {

                              if ($.inArray("" + value.Id + "", removeArray) != -1) {
                                  $("#cityDiv_" + state + "").append("<input type='checkbox' name='PostingCity" + state + "' id='PostingCity_" + value.Id + "' onclick='checkCity(" + value.Id + ")' class='ddlcitycheckbox' value='" + value.Id + "' />" + value.Name + "<br />");
                                  $("#cityDiv_" + state + "").remove();
                                  
                                  checkCity(value.Id);
                              }
                              else {
                                  $("#cityDiv_" + state + "").append("<input type='checkbox' name='PostingCity" + state + "' id='PostingCity_" + value.Id + "' onclick='checkCity(" + value.Id + ")' class='ddlcitycheckbox' value='" + value.Id + "' />" + value.Name + "<br />");
                                  $("#cityDiv_" + state + "").remove();
                              }
                          }
                      });
                  });

                  //SelectedStateCount();
                  SelectedCityCount();
                  SelectedRegionCount();

              }
              SelectedStateCount();

          }

          function checkCity(val) {

              var selected = "-1,";
              $("input:checkbox[class=ddlcitycheckbox]:checked").each(function () {
                  selected += $(this).val() + ",";
              });
              var selectedArray = new Array();
              selectedArray = selected.split(",");
              
              if ($.inArray("" + val + "", selectedArray) != -1) {
                  
                  $("#regionDiv_" + val + "").remove();

                  if (document.getElementById("PostingCity_" + val + "").checked) {

                      $.get("/Jobs/getRegionbyCityId/" + val, function (data) {

                          //Developer Note: For Exact Match check process.....
                          var RegionIds = $("#RegionIds").val();
                          var RegionArray = new Array();
                          RegionArray = RegionIds.split(",");
                          
                          $.each(data.Region, function (key, value) {
                              if (value.Id == "-1") {
                                  $("#RegionDiv").append("<div id='regionDiv_" + val + "' ></div>");
                                  $("#regionDiv_" + val + "").append("<strong style='line-height:20px !important;'>-- " + value.Name + " --</strong> <br />");
                              }
                              else {
                                  if ($.inArray("" + value.Id + "", RegionArray) != -1) {
                                      $("#regionDiv_" + val + "").append("<input type='checkbox' checked='checked' name='PostingRegion" + val + "'  id='PostingRegion_" + value.Id + "' onclick='checkRegion(" + value.Id + ")' class='ddlregioncheckbox' value='" + value.Id + "' />" + value.Name + "<br />");
                                      
                                      checkRegion(value.Id);
                                  }
                                  else {
                                      $("#regionDiv_" + val + "").append("<input type='checkbox' name='PostingRegion" + val + "'  id='PostingRegion_" + value.Id + "' onclick='checkRegion(" + value.Id + ")' class='ddlregioncheckbox' value='" + value.Id + "' />" + value.Name + "<br />");

                                  }

                              }
                          });

                      });
                      //                        SelectedRegionCount();
                  }
              }
              else {
                  var remove = "-1,";
                  $("input:checkbox[class=ddlregioncheckbox]:checked").each(function () {
                      remove += $(this).val() + ",";
                  });
                  var removeArray = new Array();
                  removeArray = remove.split(",");

                  var city = val;
                  $.get("/Jobs/getRegionbyCityId/" + city, function (data) {
                      $.each(data.Region, function (key, value) {
                          if (value.Id == "-1") {
                              // $("#RegionDiv").append("<div id='regionDiv_" + val + "' ></div>");
                              $("#regionDiv_" + city + "").remove();
                          }
                          else {
                              if ($.inArray("" + value.Id + "", removeArray) != -1) {
                                  $("#regionDiv_" + city + "").append("<input type='checkbox' name='PostingRegion" + city + "'  id='PostingRegion_" + value.Id + "' onclick='checkRegion(" + value.Id + ")' class='ddlregioncheckbox' value='" + value.Id + "' />" + value.Name + "<br />");
                                  
                                  $("#regionDiv_" + city + "").remove();
                                  checkRegion(value.Id);
                              }
                              else {
                                  $("#regionDiv_" + city + "").append("<input type='checkbox' name='PostingRegion" + city + "'  id='PostingRegion_" + value.Id + "' onclick='checkRegion(" + value.Id + ")' class='ddlregioncheckbox' value='" + value.Id + "' />" + value.Name + "<br />");
                                  $("#regionDiv_" + city + "").remove();
                              }

                          }
                      });
                      $("#regionDiv_" + city + "").remove();
                      SelectedRegionCount();
                  });

                  SelectedCityCount();
                  SelectedRegionCount();
              }

              SelectedCityCount();
          }

          function checkRegion(val) {
              

              SelectedRegionCount();
          }

          function SelectedCountryCount() {
              var g = 0;
              $("input:checkbox[name=PostingCountry]:checked").each(function () {
                  g = g + 1;
              });
              $("#txtCountry").val("- Selected Countries ( " + g + " ) -");
          }

          function SelectedStateCount() {
              var g = 0;
              $("input:checkbox[name=PostingState]:checked").each(function () {
                  g = g + 1;
              });
              $("#txtState").val("- Selected States ( " + g + " ) -");
          }

          function SelectedCityCount() {
              var g = 0;
              $("input:checkbox[class=ddlcitycheckbox]:checked").each(function () {
                  g = g + 1;
              });
              $("#txtCity").val("- Selected Cities ( " + g + " ) -");
          }

          function SelectedRegionCount() {
              var g = 0;
              $("input:checkbox[class=ddlregioncheckbox]:checked").each(function () {
                  g = g + 1;
              });
              $("#txtRegion").val("- Selected Regions ( " + g + " ) -");
          }

          // experience validation to avoid select maximum value in minimum dropdown.

//          var $min = $("#ddlAnnualSalaryLakhsMin");
//          var $max = $("#ddlAnnualSalaryLakhs");
//          var $msg = $("#message");

//          //Apply a single change event to fire on either dropdown
//          $min.add($max).change(function () {
//              
//              //Have some default text to display, an empty string
//              var text = "";

//              //Cache the vales as variables so we don't have to keep getting them
//              //We will parse the numbers out of the string values
//              var minVal = parseInt($min.val(), 10);
//              var maxVal = parseInt($max.val(), 10)

//              //Determine if both are numbers, if so then they both have values
//              var bothHaveValues = !isNaN(minVal) && !isNaN(maxVal);

//              if (bothHaveValues) {
//                  if (minVal > maxVal) {
//                      text += 'Minimum Salary should be less than maximum';
//                  } else if (maxVal < minVal) {
//                      text += 'Maximum Salary should be greater than minimum';
//                  }
//              }

//              //Display the text
//              $msg.html(text);
//          });

      </script>
      

      <style type="text/css">
        .lf
        {
            float: left;            
        }
        .scrollbox
        {
            overflow: auto;
            width: 260px;
            height: 300px;
            border: 1px solid rgb(109, 144, 176);
            padding: 4px 2px 4px 3px;
            /*line-height:10px !important;*/
            line-height:21px !important;
            z-index: 60000;
            display:none;
            position:absolute;
            background-color:White;            
        }     
        
        .scrollboxlongdiv
        {
            overflow: auto;
            width: 540px;
            height: 300px;
            border: 1px solid rgb(109, 144, 176);
            padding: 4px 2px 4px 3px;
           /* line-height: 10px !important;*/
            z-index: 60000;
            display: none;
            position: absolute;
            background-color: White;
        }
        
        .dropleft
        {
            border-right: 1px solid #030CA6;
            float: left;
            width: 250px;
            padding-left: 8px;
        }   
        
        .ddlcheckbox
        {
            float: left;
            margin-right: 5px;
            margin-top: 3px;
        }
        
        
        #message{
            color:red;
        }
        
        /*label{
            display:block;
        }*/
        
        .right
        {
            float: left;
            width: 220px;
            line-height: 18px;
            padding-bottom: 5px;
        }
        
        .text
        {
            background: url(../../Content/Images/dropdown.gif) no-repeat scroll 98% 50% transparent;
        }  
        .ddlcountry
        {
            background: url(../../Content/Images/dropdown.gif) no-repeat scroll 98% 50% transparent;
        }  
             
    </style>

    

    <div class="editor-label">
        <%: Html.Label("Basic Qualification")%>

        <input type="hidden" id="basiQualificationIds" value="<%= ViewData["basiQualificationIds"] != null ? ViewData["basiQualificationIds"].ToString() : "" %>" />
        <input type="hidden" id="basiQualificationSpecializationIds" value="<%= ViewData["basiQualificationSpecializationIds"] != null ? ViewData["basiQualificationSpecializationIds"].ToString() : "" %>" />

    </div>
    <div class="editor-field">
            <input id="txtBasicQualifications" class="text" readonly="readonly" value="- Select Basic Qualification -" type="text" style="width:256px;cursor:default;" />            
                    
                    <div class="lf scrollboxlongdiv" id="BasicQualificationAreaDiv">
                    <%-- Basic Qualification checkbox list --%>
                    <div class="dropleft" id="BasicQualificationDiv">
                        <strong class="choose">Choose Basic Qualification</strong><br />
                    </div>
                    <div id="BasicQualificationSpecializationDiv" style="float: left; padding-left: 10px; width: 245px;">
                        <strong class="choose">Choose Specialization</strong><br />
                        <div class="specialization-container">
                        </div>
                    </div>
                </div>                    
        
    </div>   

    <div class="editor-label clear">
        <%: Html.Label("Post Graduation")%>

        <input type="hidden" id="postGraduationIds" value="<%= ViewData["postGraduationIds"] != null ? ViewData["postGraduationIds"].ToString() : "" %>" />
        <input type="hidden" id="postGraduationSpecializationIds" value="<%= ViewData["postGraduationSpecializationIds"] != null ? ViewData["postGraduationSpecializationIds"].ToString() : "" %>" />
        
    </div>
    <div class="editor-field">
    <input id="txtPostGraduation" class="text" readonly="readonly" value="- Select Post Graduation -" type="text" style="width:256px;cursor:default;" />            
                    <div class="lf scrollboxlongdiv" id="PostGraduationAreaDiv">
                    <%-- Post Graduation checkbox list --%>
                    <div class="dropleft" id="PostGraduationDiv">
                        <strong class="choose">Choose Post Graduation Qualification</strong><br />
                    </div>
                    <div id="PostGraduationSpecializationDiv" style="float: left; padding-left: 10px; width: 245px;">
                        <strong class="choose">Choose Specialization</strong><br />
                        <div class="specialization-container">
                        </div>
                    </div>
                </div>
                    
    </div>
    
    <div class="editor-label clear">
        <%: Html.Label("Doctorate")%>

        <input type="hidden" id="doctorateIds" value="<%= ViewData["doctorateIds"] != null ? ViewData["doctorateIds"].ToString() : "" %>" />
        <input type="hidden" id="doctorateSpecializationIds" value="<%= ViewData["doctorateSpecializationIds"] != null ? ViewData["doctorateSpecializationIds"].ToString() : "" %>" />

    </div>
    <div class="editor-field">
    <input id="txtDoctrate" class="text" readonly="readonly" value="- Select Doctrate -" type="text" style="width:256px;cursor:default;" />                                
             <div class="lf scrollboxlongdiv" id="DoctrateAreaDiv">
                    <%-- Doctorate checkbox list --%>
                    <div class="dropleft" id="DoctrateDiv">
                        <strong class="choose">Choose Doctorate Qualification</strong><br />
                    </div>
                    <div id="DoctrateSpecializationDiv" style="float: left; padding-left: 10px; width: 245px;">
                        <strong class="choose">Choose Specialization</strong><br />
                        <div class="specialization-container">
                        </div>
                    </div>
                </div>
    </div>
    

    <div class="editor-label">
        <%: Html.Label("Functional Area / Department")%>
        <span class="red">*</span> 
    </div>
    <div class="editor-field">
       <%: Html.DropDownList("Functions", "--Select--")%>
    </div>

    <%--<div class="editor-field">
        <%: Html.DropDownList("Functions", (SelectList)ViewData["Functions"])%>    
    </div>--%>

    <div class="editor-label">
        <%: Html.Label("Roles")%>
        <span class="red">*</span> 
    </div>
    <div class="editor-field">
    <%if (ViewData["Roles"] != null)
      { %>
        <%-- <%: Html.DropDownList("Roles", (IEnumerable<SelectListItem>)ViewData["Roles"], "Any")%>--%>
         <%: Html.DropDownList("Roles", "Any")%>
    <% } else { %>
        <select id="Roles" name="Roles"></select>
    <% } %>
       
    </div>

    <div class="editor-label">
        <%: Html.Label("Preferred Industries")%>
        <span class="red">*</span> 
    </div>

    <div class="editor-field">
    <%var industry = ViewData["JobIndustries"]; %>
    <%if (ViewData["JobIndustries"] != null) {%>
        
        <%: Html.ListBox("PreferredIndustries", new MultiSelectList((IEnumerable<Dial4Jobz.Models.Industry>)ViewData["JobIndustries"], "Id", "Name", (IEnumerable<int>)ViewData["JobPreferredIndustryId"]))%>
    <%} else { %>
        <%= Html.ListBox("PreferredIndustries", (IEnumerable<SelectListItem>)ViewData["Industries"], "Any")%>
    <%} %>
    </div>

     <div class="editor-label">
         <%:Html.Label("Candidates Should have") %> (Only choose If Candidate Should Compulsorily have 2 or 4 wheeler. Choosing This May Reduce Search Results)
     </div>
    
     <div class="editor-field">
        <%: Html.CheckBox("Twowheeler", Model.TwoWheeler)%> Two wheeler
        <%: Html.CheckBox("Fourwheeler", Model.FourWheeler)%> Four wheeler
     </div>

    <div class="editor-label">
        <%: Html.Label("License Types") %>
    </div> 
   <%-- <div class="editor-field">
        <%: Html.ListBox("lbLicenseTypes", new MultiSelectList((IEnumerable<Dial4Jobz.Models.LicenseType>)ViewData["LicenseTypes"], "Id", "Name", (IEnumerable<int>)ViewData["LicenseTypeIds"]))%>
    </div>
--%>

  <div class="editor-field">
    <%if (ViewData["LicenseTypes"] != null )
      {%>
	    <%: Html.ListBox("lbLicenseTypes", new MultiSelectList((IEnumerable<Dial4Jobz.Models.LicenseType>)ViewData["LicenseTypes"], "Id", "Name", (IEnumerable<int>)ViewData["LicenseTypeIds"]))%>
    <%} else { %>
	    <%: Html.ListBox("lbLicenseTypes", new SelectList((System.Collections.Generic.IEnumerable<SelectListItem>)ViewData["License"], "Value", "Text", (IEnumerable<int>)ViewData["LicenseTypeIds"]))%>
    <%} %>
  </div>
          

    <div class="editor-label">
        <%: Html.Label("Location(s) of Posting")%> 
        <span class="red">*</span> 
        
        <input type="hidden" id="CountryIds" value="<%= ViewData["CountryIds"] != null ? ViewData["CountryIds"].ToString() : "" %>" />
        <input type="hidden" id="StateIds" value="<%= ViewData["StateIds"] != null ? ViewData["StateIds"].ToString() : "" %>" />
        <input type="hidden" id="CityIds" value="<%= ViewData["CityIds"] != null ? ViewData["CityIds"].ToString() : "" %>" />
        <input type="hidden" id="RegionIds" value="<%= ViewData["RegionIds"] != null ? ViewData["RegionIds"].ToString() : "" %>" />

    </div>
    <div class="editor-field">
        India :
        <br />
        <input id="txtCountry" class="text" readonly="readonly" value="- Select Country -" type="text" style="width: 256px; cursor: default;" />
        <div class="lf scrollbox" id="CountryDiv" style="height:150px;">
            <%-- country checkbox list --%>            
            <input id="PostingCountry_152" class="ddlcountry" type="checkbox"  value="152" onclick="checkCountry(152)" name="PostingCountry" <%= ViewData["CountryIds"] != null && ViewData["CountryIds"].ToString().Contains("152") ? "checked='checked'" : ""%> /> India <br/>
        </div>
        <br />
        <input id="txtState" class="text" readonly="readonly" value="- Select State -" type="text" style="width: 256px; cursor: default;" />
        <div class="lf scrollbox" id="StateDiv">
            <%-- state checkbox list --%>
        </div>
        <br />
        <input id="txtCity" class="text" readonly="readonly" value="- Select Cities -" type="text" style="width: 256px; cursor: default;" />
        <div class="lf scrollbox" id="CityDiv">
            <%-- city checkbox list --%>
        </div>
        <br />
        <input id="txtRegion" class="text" readonly="readonly" value="- Select Regions -" type="text" style="width: 256px; cursor: default;" />
        <div class="lf scrollbox" id="RegionDiv">
            <%-- region checkbox list --%>
        </div>
        <br />
        
        Other Country :
        <br />
        <input id="txtOtherCountry" class="text" readonly="readonly" value="- Select Other Country -" type="text" style="width: 256px; cursor: default;" />
        <div class="lf scrollbox" id="OtherCountryDiv">
            <%-- Industry checkbox list --%>
        </div>
                
    </div>
       
    

    <div class="editor-label">
        <%: Html.Label("Gender")%>
    </div>
    <div class="editor-field">
        <%-- <%: Html.CheckBox("Male", Model.Male.HasValue ? Model.Male.Value : false) %> Male
        <%:Html.CheckBox("Female",Model.Female.HasValue ? Model.Female.Value :false) %> Female--%>
        <%: Html.CheckBox("Male", Model.Male.HasValue ? Model.Male.Value : true) %> Male
        <%: Html.CheckBox("Female",Model.Female.HasValue ? Model.Female.Value : true) %> Female
    </div>

    <div class="editor-label">
        <%: Html.Label("Experience")%>
    </div>

    <div class="editor-field">
        <%: Html.DropDownList("ddlTotalExperienceYearsFrom", (SelectList)ViewData["TotalExperienceYearsFrom"])%> Min
        <%: Html.DropDownList("ddlTotalExperienceYearsTo", (SelectList)ViewData["TotalExperienceYearsTo"])%> Max
    </div>


    <div class="editor-label">
        <%: Html.Label("Annual Salary") %>
    </div>

    <div class="editor-field">
        <%: Html.DropDownList("ddlAnnualSalaryLakhsMin", (SelectList)ViewData["MinAnnualSalaryLakhs"])%> Lakhs
        <%: Html.DropDownList("ddlAnnualSalaryThousandsMin", (SelectList)ViewData["MinAnnualSalaryThousands"])%> Thousands
        <%--<label for="MaxSalaryDropdown">Min Experience</label>--%>
      
        <%: Html.DropDownList("ddlAnnualSalaryLakhs", (SelectList)ViewData["AnnualSalaryLakhs"])%> Lakhs
        <%: Html.DropDownList("ddlAnnualSalaryThousands", (SelectList)ViewData["AnnualSalaryThousands"])%> Thousands
         <%--<label for="MaxSalaryDropdown">Min Experience</label>--%>
    </div>

    <div id="message"></div>
    

    <div class="editor-label">
        <%: Html.Label("Skills") %>
    </div>
    <div class="editor-field">
        <input type="text" id="Skills" name="Skills" />
        <% var skills = string.Empty; %>
        <% foreach (var cs in Model.JobSkills) { %>
            <% var skill = String.Format("{{\"id\": {0}, \"name\": \"{1}\"}}", cs.Skill.Id.ToString(), cs.Skill.Name); %>
            <% skills = skills == string.Empty ? skill : skills + ", " + skill; %>
        <% } %>
        <%: Html.Hidden("SkillsHidden", "[" + skills + "]") %>
    </div>

    <div class="editor-label">
        <%: Html.Label("Languages") %>
          <span class="red">*</span> 
    </div>
    <div class="editor-field">
        <input type="text" id="Languages" name="Languages" />
        <% var languages = string.Empty; %>
        <% foreach (var cl in Model.JobLanguages) { %>
            <% var language = String.Format("{{\"id\": {0}, \"name\": \"{1}\"}}", cl.Language.Id.ToString(), cl.Language.Name); %>
            <% languages = languages == string.Empty ? language : languages + ", " + language; %>
        <% } %>
        <%: Html.Hidden("LanguagesHidden", "[" + languages + "]")%>
    </div>

    <div class="editor-label">
        <%: Html.Label("Type of Vacancy")%>
    </div>
    <div id="PreferredType" class="editor-field">
        <%: Html.CheckBox("Any")%> Any
        <%: Html.CheckBox("Contract", Model.PreferredContract.HasValue ?  Model.PreferredContract.Value : false) %> Contract
        <%: Html.CheckBox("PartTime", Model.PreferredParttime.HasValue ? Model.PreferredParttime.Value : false) %> Part Time
        <%: Html.CheckBox("FullTime", Model.PreferredFulltime.HasValue ? Model.PreferredFulltime.Value : false) %> Full Time
        <%: Html.CheckBox("WorkFromHome", Model.PreferredWorkFromHome.HasValue ? Model.PreferredWorkFromHome.Value : false) %> Work from Home
    </div>

  
    <div id="PreferredTime" class="hidden">
           <div class="editor-label">
               <%: Html.Label("Preferred Time") %>
           </div>

           <div class="editor-field" style="width:85px;">
                <%:Html.TextBox("ddlPreferredTimeFrom", Model.PreferredTimeFrom, new { @class = "preferredtime" })%> to

                <%:Html.TextBox("ddlPreferredTimeTo", Model.PreferredTimeTo, new { @class = "preferredtime" })%>

           </div>
    </div>

    <div class="editor-label">
       <%: Html.Label("Preferred Work Shift") %>
    </div>

    <div class="editor-field">
        <%: Html.CheckBox("GeneralShift", Model.GeneralShift.HasValue ? Model.GeneralShift.Value : false) %> General Shift
        <%: Html.CheckBox("NightShift", Model.NightShift.HasValue ? Model.NightShift.Value :false) %> Night Shift
    </div>

    <h2>Contact Details for Requirements</h2>
    
    <div class="editor-label">
        <%: Html.Label("Contact Person")%>
        <span class="red">*</span>
    </div>
    <div class="editor-field">
       <%if (loggedInOrganization != null) { %>
            <%: Html.TextBox("RequirementsContactPerson", loggedInOrganization.ContactPerson, new { @title = "Enter the contact person for this requirement" })%>
        <%} else if(organization!=null)  { %>
            <%: Html.TextBox("RequirementsContactPerson", organization.ContactPerson, new { @title = "Enter the contact person for this requirement" })%>
         <%} else if(LoggedInConsultants != null)  { %>
            <%: Html.TextBox("RequirementsContactPerson", LoggedInConsultants.ContactPerson, new { @title = "Enter the contact person for this requirement" })%>
        <% } else { %>
         <%: Html.TextBox("RequirementsContactPerson", Model.ContactPerson, new { @title = "Enter the contact person for this requirement" })%>
        <% }%>
    </div>
   
    <div class="editor-label">
        <%: Html.Label("Contact Number")%>
    </div>
    <div class="editor-field">
     <%if (loggedInOrganization != null) { %>
        <%: Html.TextBox("RequirementsContactNumber", loggedInOrganization.ContactNumber, new { @title = "Enter the contact number for this requirement", @maxlength = "10" })%>
    <%} else if(organization!=null)  { %>
        <%: Html.TextBox("RequirementsContactNumber", organization.ContactNumber, new { @title = "Enter the contact person for this requirement" })%>
    <%} else if(LoggedInConsultants != null)  { %>
            <%: Html.TextBox("RequirementsContactNumber", LoggedInConsultants.ContactNumber, new { @title = "Enter the contact person for this requirement" })%>
    <%} else { %>
        <%: Html.TextBox("RequirementsContactNumber", Model.ContactNumber, new { @title = "Enter the contact person for this requirement" })%>
     <% } %>

    </div>

    <div class="editor-label">
        <%: Html.Label("Mobile Number")%>
        <span class="red">*</span>
    </div>
    <div class="editor-field">
         <%if (loggedInOrganization != null) { %>
            <%: Html.TextBox("RequirementsMobileNumber", loggedInOrganization.MobileNumber, new { @title = "Enter the mobile number for this requirement", @maxlength = "10" })%>
            <%if (loggedInOrganization.IsPhoneVerified == true)
              { %>
                 <a><img src="../../Content/Images/Tick.png" class="btn" /></a>Verified
            <%}
              else
              {%>
              Not Verified
            <%} %>
         <%} else if(organization!=null)  { %>
              <%: Html.TextBox("RequirementsMobileNumber", organization.MobileNumber, new { @title = "Enter the contact person for this requirement" })%>
         <%} else if(LoggedInConsultants != null)  { %>
              <%: Html.TextBox("RequirementsMobileNumber", LoggedInConsultants.MobileNumber, new { @title = "Enter the contact person for this requirement" })%>
         <%} else { %>
            <%: Html.TextBox("RequirementsMobileNumber", Model.MobileNumber, new { @title = "Enter the contact person for this requirement" })%>
         <% } %>
    </div>

            <div class="editor-label">
       		     <%: Html.Label("International Number")%>
            </div>
      		 <div class="editor-field">
        	        <%: Html.TextBox("RequirementsInternationalNumber", Model.InternationalNumber, new { @title = "Enter the contact person for this requirement" })%>
    		</div>
    <div class="editor-label">
        <%: Html.Label("Email Address")%>
    </div>

    <div class="editor-field">
         <%if (loggedInOrganization != null) { %>
            <%: Html.TextBox("RequirementsEmailAddress", loggedInOrganization.Email, new { @title = "Enter the email address for this requirement" })%>
            <%if (loggedInOrganization.IsPhoneVerified == true)
              { %>
                 <a><img src="../../Content/Images/Tick.png" alt="verified" class="btn" /></a>Verified
            <%}
              else
              {%>
              Not Verified
            <%} %>
          <%} else if(organization!=null)  { %>
            <%: Html.TextBox("RequirementsEmailAddress", organization.Email, new { @title = "Enter the contact person for this requirement" })%>
         <%} else if(LoggedInConsultants != null)  { %>
              <%: Html.TextBox("RequirementsEmailAddress", LoggedInConsultants.Email, new { @title = "Enter the contact person for this requirement" })%>
         <%} else { %>
           <%: Html.TextBox("RequirementsEmailAddress", Model.EmailAddress, new { @title = "Enter the contact person for this requirement" })%>
         <% } %>
    </div>

    <div class="editor-label">
        <%: Html.Label("Communication Mode")%>
         <span class="red">* select any one or both</span> 
    </div>
    <div class="editor-field">
        <%: Html.CheckBox("CommunicationSMS",Model.CommunicateViaSMS.HasValue ? Model.CommunicateViaSMS.Value :false) %> SMS
        <%: Html.CheckBox("CommunicationEmail",Model.CommunicateViaEmail.HasValue ? Model.CommunicateViaEmail.Value :false) %> Email
    </div>

    <div class="editor-label">
        <%:Html.Label("Description") %> 
    </div>
    <div class="editor-field textarea">
        <%:Html.TextArea("Description", Model.Description, new { @title = "" })%>
    </div>

  
     <%--<input type="checkbox" id="HideDetails"/>Hide Email and Mobile Number--%>
    <div class="editor-label">
        <%:Html.Label("Click to Hide Contact Details") %>
    </div>      

    <div class="editor-field">
        <%:Html.CheckBox("HideDetails",Model.HideDetails) %> Hide Details
    </div>
    
    <div class="editor-field">
        <%:Html.CheckBox("BasicTerms") %> <%: Html.ActionLink("I agree Terms and Conditions", "Terms", "Home")%>
    </div>

   
