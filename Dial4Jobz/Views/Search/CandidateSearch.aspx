<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Dial4Jobz.Models.Candidate>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Candidate Search
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    <script src="<%=Url.Content("~/Scripts/Dial4Jobz.Candidate.js")%>" type="text/javascript"></script>
    <script src="<%=Url.Content("~/Scripts/Dial4Jobz.Job.js")%>" type="text/javascript"></script>


    <link href="../../Content/jquery-ui.1.8.1.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../Scripts/jquery.min.js"></script>
    <script type="text/javascript" src="../../Scripts/jquery-ui.min.1.8.1.js"></script>
    <script type="text/javascript">
        $(function () {
            $('#what').autocomplete({
                source: function (request, response) {
                    $.ajax(
            {
                url: "/Search/GetSkillsAlone", type: "GET", dataType: "json",
                data: { term: extractLast(request.term), term: extractLast(request.term) },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    response(data);
                }

            });
                },
                focus: function () {

                    return false;
                },
                select: function (event, ui) {
                    var terms = split(this.value);

                    terms.pop();

                    terms.push(ui.item.value);

                    terms.push("");
                    this.value = terms.join(",");
                    return false;
                },
                minLength: 1

            });
            //languages search

            $(function () { 
            $('#language').autocomplete({
                source: function (request, response) {
                    $.ajax(
            {
                url: "/Search/GetLanguagesAlone", type: "GET", dataType: "json",
                data: { term: extractLast(request.term), term: extractLast(request.term) },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    response(data);
                }

            });
                },
                focus: function () {

                    return false;
                },
                select: function (event, ui) {
                    var terms = split(this.value);

                    terms.pop();

                    terms.push(ui.item.value);

                    terms.push("");
                    this.value = terms.join(",");
                    return false;
                },
                minLength: 1

            });


            $("#language").bind("keydown", function (event) {
                if (event.keyCode === $.ui.keyCode.TAB &&
                    $(this).data("autocomplete").menu.active) {
                                        event.preventDefault();
                                    }
                                })
                                function split(val) {
                                    return val.split(/,\s*/);
                                    alert(val);
                                }
                                function extractLast(term) {
                                    return split(term).pop();
                                }

                            });
                    //end of langauges search


                                $("#what").bind("keydown", function (event) {
                                    if (event.keyCode === $.ui.keyCode.TAB &&
                    $(this).data("autocomplete").menu.active) {
                                        event.preventDefault();
                                    }
                                })
                                function split(val) {
                                    return val.split(/,\s*/);
                                    alert(val);
                            }
                        function extractLast(term) {
                            return split(term).pop();
                        }

        });

        $(document).ready(function () {
            $.get("/Search/getGroupCountryStateCity/", function (data) {

                $.each(data.location, function (key, value) {
                    if (key == 0) {
                        $("#LocalityDiv").append("<input type='checkbox' name='checkAllLocality' checked='checked' value='' /> Any <br />");
                        $("#PrefLocalityDiv").append("<input type='checkbox' name='checkAllPrefLocality' checked='checked' value='' /> Any <br />");
                    }
                    if (value.StateId == "-1") {
                        $("#LocalityDiv").append("<span style='font-weight:bold; padding-left:20px; color:#969696; font-style:italic'>--- " + value.Name + " ---</span> <br />");
                        $("#PrefLocalityDiv").append("<span style='font-weight:bold; padding-left:20px; color:#969696; font-style:italic'>--- " + value.Name + " ---</span> <br />");
                    }
                    else if (value.StateId == "-4") {
                        $("#LocalityDiv").append("<input type='checkbox' id='MetroLocality' name='MetroLocality' onclick='checkAllMetroLoc()' value='" + value.Id + "' /><span style='font-weight:bold; '>--- " + value.Name + " ---</span> <br />");
                        $("#PrefLocalityDiv").append("<input type='checkbox' id='MetroPrefLocality' name='MetroPrefLocality' onclick='checkAllMetroPrefLoc()' value='" + value.Id + "' /><span style='font-weight:bold; '>--- " + value.Name + " ---</span> <br />");
                    }
                    else if (value.StateId == "-5") {
                        $("#LocalityDiv").append("<input type='checkbox' name='MetroCity' onclick='checkLoc()' value='" + value.Id + "' /> " + value.Name + " <br />");
                        $("#PrefLocalityDiv").append("<input type='checkbox' name='PrefMetroCity' onclick='checkPrefLoc()' value='" + value.Id + "' /> " + value.Name + " <br />");
                    }
                    else if (value.StateId == "-2") {
                        $("#LocalityDiv").append("<input type='checkbox' id='LocalityState_" + value.Id + "' name='LocalityState' onclick='checkLocState(" + value.Id + ")' value='" + value.Id + "' /><span style='font-weight:bold; '>--- " + value.Name + " ---</span> <br />");
                        $("#PrefLocalityDiv").append("<input type='checkbox' id='LocalityPrefState_" + value.Id + "' name='LocalityPrefState' onclick='checkPrefLocState(" + value.Id + ")' value='" + value.Id + "' /><span style='font-weight:bold; '>--- " + value.Name + " ---</span> <br />");
                    }
                    else {
                        $("#LocalityDiv").append("<input type='checkbox' name='Locality' onclick='checkLoc()' value='" + value.Id + "' /> " + value.Name + " <br />");
                        $("#PrefLocalityDiv").append("<input type='checkbox' name='PrefLocality' onclick='checkPrefLoc()' value='" + value.Id + "' /> " + value.Name + " <br />");
                    }

                });
            });

            $.get("/Search/getIndustries/", function (data) {
                $.each(data.industry, function (key, value) {
                    if (key == 0) {
                        $("#IndustryDiv").append("<input type='checkbox' name='checkAllIndustry' checked='checked' value='' /> Any <br />");
                    }

                    $("#IndustryDiv").append("<input type='checkbox' name='PrefIndustry' onclick='checkInd()' value='" + value.Id + "' /> " + value.Name + " <br />");


                });
            });

            //preferred Type

            //            $.get("/Search/getPreferredType/", function (data) {
            //                $.each(data.preferredType, function (key, value) {
            //                    if (key == 0) {
            //                        
            //                    }

            //                });
            //            });


            $.get("/Search/getFunctions/", function (data) {
                $.each(data.functionalArea, function (key, value) {
                    if (key == 0) {
                        $("#FunctionDiv").append("<input type='checkbox' name='checkAllFunction' class='ddlcheckbox' checked='checked' value='' /> <div class='right'> Any </div><br />");
                    }

                    $("#FunctionDiv").append("<input type='checkbox' id='FunctionalArea_" + value.Id + "' name='FunctionalArea' class='ddlcheckbox' onclick='checkfunc(" + value.Id + ")' value='" + value.Id + "' /> <div class='right'>" + value.Name + "</div> <br />");


                });
            });



            $.get("/Search/getBasicQualification/", function (data) {
                $.each(data.BasicQualification, function (key, value) {
                    if (key == 0) {
                        $("#BasicQualificationDiv").append("<input type='checkbox' name='checkAllBasicQualification' checked='checked' value='' /> Any <br />");
                    }

                    $("#BasicQualificationDiv").append("<input id='BasicQualificationArea_" + value.Id + "' type='checkbox' name='basicQualification' onclick='checkBasic(" + value.Id + ")' value='" + value.Id + "' /> " + value.Name + " <br />");


                });
            });

            $.get("/Search/getPostGraduation/", function (data) {
                $.each(data.PostGraduation, function (key, value) {
                    if (key == 0) {
                        $("#PostGraduationDiv").append("<input type='checkbox' name='checkAllPostgraduation' checked='checked' value='' /> Any <br />");
                        $("#PostGraduationDiv").append("<input type='checkbox' name='checkAllPostgraduation' checked='checked' value='0' /> None <br />");
                    }

                    $("#PostGraduationDiv").append("<input id='PostGraduationArea_" + value.Id + "' type='checkbox' name='PostGraduation' onclick='checkPost(" + value.Id + ")' value='" + value.Id + "' /> " + value.Name + " <br />");


                });
            });

            $.get("/Search/getDoctorate/", function (data) {
                $.each(data.Doctorate, function (key, value) {
                    if (key == 0) {
                        $("#DoctrateDiv").append("<input type='checkbox' name='checkAllDoctrate' checked='checked' value='' /> Any <br />");
                        $("#DoctrateDiv").append("<input type='checkbox' name='checkAllDoctrate' checked='checked' value='0' /> None <br />");
                    }

                    $("#DoctrateDiv").append("<input id='DoctrateArea_" + value.Id + "' type='checkbox' name='Doctrate' onclick='checkDoc(" + value.Id + ")' value='" + value.Id + "' /> " + value.Name + " <br />");


                });
            });



            $('#txtLocation').click(function (e) {
                if (document.getElementById("LocalityDiv").style.display == "none") {
                    document.getElementById("LocalityDiv").style.display = "block";
                    e.stopPropagation();
                }
                else {
                    document.getElementById("LocalityDiv").style.display = "none";
                }
            });

            $('#txtPrefLocation').click(function (e) {
                if (document.getElementById("PrefLocalityDiv").style.display == "none") {
                    document.getElementById("PrefLocalityDiv").style.display = "block";
                    e.stopPropagation();
                }
                else {
                    document.getElementById("PrefLocalityDiv").style.display = "none";
                }
            });

            $('#txtFunction').click(function (e) {
                if (document.getElementById("FunctionAreaDiv").style.display == "none") {
                    document.getElementById("FunctionAreaDiv").style.display = "block";
                    e.stopPropagation();
                }
                else {
                    document.getElementById("FunctionAreaDiv").style.display = "none";
                }
            });

            $('#txtIndustries').click(function (e) {
                if (document.getElementById("IndustryDiv").style.display == "none") {
                    document.getElementById("IndustryDiv").style.display = "block";
                    e.stopPropagation();
                }
                else {
                    document.getElementById("IndustryDiv").style.display = "none";
                }
            });

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

            $('#LocalityDiv,#PrefLocalityDiv,#FunctionAreaDiv,#IndustryDiv,#BasicQualificationAreaDiv,#PostGraduationAreaDiv,#DoctrateAreaDiv').click(function (e) {
                e.stopPropagation();
            });

            $(document).click(function () {
                $("#LocalityDiv,#PrefLocalityDiv,#FunctionAreaDiv,#IndustryDiv,#BasicQualificationAreaDiv,#PostGraduationAreaDiv,#DoctrateAreaDiv").css("display", "none");
            });

        });


               

        function checkLocState(val) {
            if (document.getElementById("LocalityState_" + val).checked) {
                $.get("/Search/getCitybyStateId/" + val, function (data) {
                    $.each(data.location, function (key, value) {
                        $("input:checkbox[name=Locality]").each(function () {
                            var location = $(this).val();
                            if (location == value.Id) {
                                this.checked = true;
                            }
                        });

                    });
                    checkLoc();
                });

            }
            else {
                $.get("/Search/getCitybyStateId/" + val, function (data) {
                    $.each(data.location, function (key, value) {
                        $("input:checkbox[name=Locality]").each(function () {
                            var location = $(this).val();
                            if (location == value.Id) {
                                this.checked = false;
                            }
                        });
                    });
                    checkLoc();
                });
            }
        }

        function checkPrefLocState(val) {
            if (document.getElementById("LocalityPrefState_" + val).checked) {
                $.get("/Search/getCitybyStateId/" + val, function (data) {
                    $.each(data.location, function (key, value) {
                        $("input:checkbox[name=PrefLocality]").each(function () {
                            var location = $(this).val();
                            if (location == value.Id) {
                                this.checked = true;
                            }
                        });

                    });
                    checkPrefLoc();
                });

            }
            else {
                $.get("/Search/getCitybyStateId/" + val, function (data) {
                    $.each(data.location, function (key, value) {
                        $("input:checkbox[name=PrefLocality]").each(function () {
                            var location = $(this).val();
                            if (location == value.Id) {
                                this.checked = false;
                            }
                        });
                    });
                    checkPrefLoc();
                });
            }
        }

        function checkAllMetroLoc() {
            if (document.getElementById("MetroLocality").checked) {
                $("input:checkbox[name=MetroCity]").each(function () {
                    this.checked = true;
                });
            }
            else {
                $("input:checkbox[name=MetroCity]").each(function () {
                    this.checked = false;
                });
            }
            checkLoc();
        }

        function checkAllMetroPrefLoc() {
            if (document.getElementById("MetroPrefLocality").checked) {
                $("input:checkbox[name=PrefMetroCity]").each(function () {
                    this.checked = true;
                });
            }
            else {
                $("input:checkbox[name=PrefMetroCity]").each(function () {
                    this.checked = false;
                });
            }
            checkPrefLoc();
        }


        function checkLoc() {
            var g = 0;
            $("input:checkbox[name=Locality]:checked").each(function () {
                g = g + 1;
            });
            $("input:checkbox[name=MetroCity]:checked").each(function () {
                g = g + 1;
            });

            $("#txtLocation").val("- Selected Current Locations ( " + g + " ) -");
            if (g == 0) {
                $("input:checkbox[name=checkAllLocality]").each(function () {
                    this.checked = true;
                });
            }
            else {
                $("input:checkbox[name=checkAllLocality]").each(function () {
                    this.checked = false;
                });
            }
        }

        function checkPrefLoc() {
            var g = 0;
            $("input:checkbox[name=PrefLocality]:checked").each(function () {
                g = g + 1;
            });

            $("input:checkbox[name=PrefMetroCity]:checked").each(function () {
                g = g + 1;
            });

            $("#txtPrefLocation").val("- Selected Preferred Locations ( " + g + " ) -");
            if (g == 0) {
                $("input:checkbox[name=checkAllPrefLocality]").each(function () {
                    this.checked = true;
                });
            }
            else {
                $("input:checkbox[name=checkAllPrefLocality]").each(function () {
                    this.checked = false;
                });
            }
        }

        function checkInd() {
            var g = 0;
            $("input:checkbox[name=PrefIndustry]:checked").each(function () {
                g = g + 1;
            });
            $("#txtIndustries").val("- Selected Industry ( " + g + " ) -");
            if (g == 0) {
                $("input:checkbox[name=checkAllIndustry]").each(function () {
                    this.checked = true;
                });
            }
            else {
                $("input:checkbox[name=checkAllIndustry]").each(function () {
                    this.checked = false;
                });
            }
        }

        function checkBasic(val) {
            //console.log(val);
            var g = 0;
            $("input:checkbox[name=basicQualification]:checked").each(function () {
                g = g + 1;
            });

            var specLength = $("input:checkbox[name=BasicQualificationSpecialization]:checked").length;
            $("#txtBasicQualifications").val("- Selected Basic Qualifications ( " + g + " )( "+ specLength +" ) -");
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

            var specLength = $("input:checkbox[name=PostGraduationSpecialization]:checked").length;
            $("#txtPostGraduation").val("- Selected Post Graduations ( " + g + " )( "+ specLength +" ) -");
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

            var specLength = $("input:checkbox[name=DoctrateSpecialization]:checked").length;
            $("#txtDoctrate").val("- Selected Doctrate ( " + g + " )( "+ specLength +" ) -");
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
            getSpecialization(val, "Doctrate",2);
        }

        function getSpecialization(degreeId, qualificationPrefix, degreeType) {

            var currentSpecObjId = "#" + qualificationPrefix + "Specialization_" + degreeId + "";

            if (document.getElementById("" + qualificationPrefix + "Area_" + + degreeId + "").checked) {

                $.get("/Search/getSpecialization/" + degreeId, function (data) {
                    //console.log(data);
                    $.each(data.Specialization, function (key, value) {
                        
                        if (value.Id == -1) {
                            $("#" + qualificationPrefix + "SpecializationDiv").find("div.specialization-container").append("<div id='" + qualificationPrefix + "Specialization_" + degreeId + "'></div>");
                            $(currentSpecObjId).append("<strong style='line-height:20px !important;'>-- " + value.Name + " --</strong> <br />");
                        }
                        else {
                            $(currentSpecObjId).append("<input type='checkbox' onclick='checkSpecialization(" + degreeType + ")' class='ddlcheckbox' name='" + qualificationPrefix + "Specialization' value='" + value.Id + "' /> <div class='right'>" + value.Name + "</div> <br />");
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
            var specializationCheckedLength = $("input:checkbox[name=" + specializationName + "]:checked").length;

            $("#" + lengthContainer + "").val("- Selected " + prefix + " ( " + qualificationCheckedLength + " )( " + specializationCheckedLength + " ) -");
        }

        function checkfunc(val) {
            if (document.getElementById("FunctionalArea_" + val + "").checked) {

                $.get("/Search/getRole/" + val, function (data) {
                    $.each(data.Role, function (key, value) {
                        if (value.Id == "-1") {
                            $("#AreaDiv").append("<div id='role_" + val + "' ></div>");
                            $("#role_" + val + "").append("<strong style='line-height:20px !important;'>-- " + value.Name + " --</strong> <br />");
                        }
                        else {
                            $("#role_" + val + "").append("<input type='checkbox' onclick='checkrole()' class='ddlcheckbox' name='Role' value='" + value.Id + "' /> <div class='right'>" + value.Name + "</div> <br />");
                        }


                    });
                });

            }
            else {

                $("#role_" + val + "").remove();
            }


            checkrole();
        }

        function checkrole() {
            var g = 0;
            var r = 0;
            $("input:checkbox[name=FunctionalArea]:checked").each(function () {
                g = g + 1;
            });
            $("input:checkbox[name=Role]:checked").each(function () {
                r = r + 1;
            });
            $("#txtFunction").val("Selected Functional Area ( " + g + " ) Role ( " + r + " ) ");
            if (g == 0) {
                $("input:checkbox[name=checkAllFunction]").each(function () {
                    this.checked = true;
                });
            }
            else {
                $("input:checkbox[name=checkAllFunction]").each(function () {
                    this.checked = false;
                });
            }
        }

        
    </script>
    <style type="text/css">
        .section li
        {
            color: #324B81;
        }
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
            line-height: 10px !important;
            z-index: 60000;
            display: none;
            position: absolute;
            background-color: White;
        }
        
        .scrollboxlongdiv
        {
            overflow: auto;
            width: 540px;
            height: 300px;
            border: 1px solid rgb(109, 144, 176);
            padding: 4px 2px 4px 3px;
            line-height: 10px !important;
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
        
        .choose
        {
            color: #969696;
            font: bold 13px Arial,Helvetica,sans-serif;
            padding-left: 5px;
        }
        
        .ddlcheckbox
        {
            float: left;
            margin-right: 5px;
            margin-top: 3px;
        }
        
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
    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
<% Dial4Jobz.Models.Consultante loggedInConsultant = (Dial4Jobz.Models.Consultante)ViewData["LoggedInConsultant"]; %>
<%if (loggedInConsultant != null)
  { %>
    <% Html.RenderPartial("NavConsultant"); %>
<%}
  else
  { %>
    <% Html.RenderPartial("NavEmployer"); %>
    <%} %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
 <% Dial4Jobz.Models.Organization loggedInOrganization = (Dial4Jobz.Models.Organization)ViewData["LoggedInOrganization"]; %>
 <% bool isLoggedIn = loggedInOrganization != null; %>

 <table>
    <tr>
    <td rowspan="3" colspan="3">
        <%if (isLoggedIn == false)
          { %>
        <a class="signup" href="<%=Url.Content("~/signup")%>" title="Create an account on Dial4Jobz"><img src="../../Content/Images/PostVacancy.jpg" width="128" height="42"/></a><img src="../../Content/Images/free2.jpg" width="85px" height="55px" /><br />
        <% } else { %>
             <a href="<%: Url.Action("Add", "Jobs") %>"><img src="../../Content/Images/PostVacancy.jpg" width="128" height="42"/></a><img src="../../Content/Images/free2.jpg" width="85px" height="55px" /><br />
        <% } %>

        <%if (Request.IsAuthenticated == true)
          { %>
            <h3>Recruit Candidates for less than <b>Rs.500/-</b>  <%:Html.ActionLink("Hot Resumes", "Index", "EmployerVas")%> </h3 >
        <%} else { %>
            <h3>Recruit Candidates for less than <b>Rs.500/-</b><a class="login" href="<%=Url.Content("~/login")%>" title="Login to Dial4Jobz"> Hot Resumes</a></h3>
        <%} %>
    </td>
    </tr>
    </table>


    <% if (Request.IsAuthenticated == true)
       { %>
    <div class="identityname">
       Welcome!!! <b><%: this.Page.User.Identity.Name%></b>, You are in Employer Zone.We wish you to get the right candidates for your Vacancy.....
    </div>
    <% }
       else
       { %>
         <div class="identityname">
           Welcome!!! You are in Employer Zone.We wish you to get the right candidate for your Vacancy.....
        </div>
    <% } %>


    <% Html.BeginForm("CandidateResults", "Search", FormMethod.Post, new { target = "_blank" }); %>
    <h1>
        Search Candidates</h1>
    <br />
    <table width="150%">
        <tr>
            <td>
            <h3>Search Candidate Skills: </h3>
                <input id="what" name="what" type="text" placeholder="Candidate Skills..." style="width: 230px;" />
            </td>
            <td>
             <h3>Search Candidate Languages: </h3>
                <input id="language" name="language" type="text" placeholder="Languages..." style="width: 230px;" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <h3>Experience :</h3>
                <br />
                <%: Html.DropDownList("MinExperienceYears", (SelectList)ViewData["MinExperienceYears"])%>
                -
                <%: Html.DropDownList("MaxExperienceYears", (SelectList)ViewData["MaxExperienceYears"])%>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <h3>Salary Expectation : </h3>
                <br />
                <%: Html.DropDownList("MinAnnualSalaryLakhs", (SelectList)ViewData["MinAnnualSalaryLakhs"])%>
                Lakhs
                <%: Html.DropDownList("MinAnnualSalaryThousands", (SelectList)ViewData["MinAnnualSalaryThousands"])%>
                Thousands -
                <%: Html.DropDownList("MaxAnnualSalaryLakhs", (SelectList)ViewData["MaxAnnualSalaryLakhs"])%>
                Lakhs
                <%: Html.DropDownList("MaxAnnualSalaryThousands", (SelectList)ViewData["MaxAnnualSalaryThousands"])%>
                Thousands
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <table>
                    <tr>
                        <td>
                            <h3>Candidate Current Location : </h3>
                            <br />
                            <input id="txtLocation" name="where" readonly="readonly" class="text" value="- Select Current Locations -"
                                type="text" style="width: 256px; cursor: default;" />
                            <div class="lf scrollbox" id="LocalityDiv">
                                <%-- Locality checkbox list --%>
                            </div>
                        </td>
                        <td>
                            <input type="radio" checked="checked" name="AndOrLocations" value="" />And
                            <input type="radio" name="AndOrLocations" value="1" />
                            Or
                        </td>
                        <td>
                            <h3>Candidate Preferred Location : </h3>
                            <br />
                            <input id="txtPrefLocation" name="where" readonly="readonly" class="text" value="- Select Preferred Locations -"
                                type="text" style="width: 256px; cursor: default;" />
                            <div class="lf scrollbox" id="PrefLocalityDiv">
                                <%-- Locality checkbox list --%>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <h3>Function : </h3>
                <input id="txtFunction" name="where" class="text" readonly="readonly" value="- Select Functional Area And Role -"
                    type="text" style="width: 256px; cursor: default;" />
                <div class="lf scrollboxlongdiv" id="FunctionAreaDiv">
                    <div class="dropleft" id="FunctionDiv">
                        <strong class="choose">Choose a Functional Area</strong><br />
                    </div>
                    <div id="AreaDiv" style="float: left; padding-left: 10px; width: 245px;">
                        <strong class="choose">Choose a Role</strong><br />
                    </div>
                </div>
                <%--<input id="txtFunction" name="where" readonly="readonly" value="- Select Functional Area -" onclick="displayfundiv()" type="text" style="width:256px;" />            
                    <div class="lf scrollbox" id="FunctionDiv" >

                    </div>--%>
            </td>
            <td valign="top">
                <h3>Industry wise Search : </h3>
                <br />
                <input id="txtIndustries" name="where" class="text" readonly="readonly" value="- Select Industry -"
                    type="text" style="width: 256px; cursor: default;" />
                <div class="lf scrollbox" id="IndustryDiv">
                    <%-- Industry checkbox list --%>
                </div>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <h3>Basic Qualification : </h3>
                <br />
                <input id="txtBasicQualifications" name="where" class="text" readonly="readonly"
                    value="- Select Basic Qualification -" type="text" style="width: 256px; cursor: default;" />
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
            </td>
            <td valign="top">
                <h3>Post Graduation :</h3>
                <br />
                <input id="txtPostGraduation" name="where" class="text" readonly="readonly" value="- Select Post Graduation -"
                    type="text" style="width: 256px; cursor: default;" />
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
            </td>
        </tr>
        <tr>
            <td valign="top">
                <h3>Doctorate : </h3>
                <br />
                <input id="txtDoctrate" name="where" class="text" readonly="readonly" value="- Select Doctrate -"
                    type="text" style="width: 256px; cursor: default;" />
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
            </td>
            <td valign="top">
                <h3>Age : </h3>
                <br />
                <%: Html.DropDownList("MinAge", (SelectList)ViewData["MinAgeYears"])%>
                -
                <%: Html.DropDownList("MaxAge", (SelectList)ViewData["MaxAgeYears"])%>
            </td>


        </tr>
        <tr>
            <td colspan="2">
            <h3>Preferred Type: </h3>
                <input type="checkbox" name="TypeOfVacancy" value="1" />
                Full time
                <input type="checkbox" name="TypeOfVacancy" value="2" />
                Part time
                <input type="checkbox" name="TypeOfVacancy" value="3" />
                Contract
                <input type="checkbox" name="TypeOfVacancy" value="4" />
                Work From Home
               <%-- <input type="checkbox" name="TypeOfVacancy" value="5" />--%>
            </td>
        </tr>

          <tr>
            <td colspan="2">
            <h3>Preferred Work Shift: </h3>
                <input type="checkbox" name="TypeOfWorkShift" value="1" />
                General Shift
                <input type="checkbox" name="TypeOfWorkShift" value="2" />
                Night Shift
            </td>
        </tr>

        <tr>
            <td>
                &nbsp;
            </td>
            <td style="padding-left: 15px;">
                <input id="Search" type="submit" value="Search" class="btn-search" title="Search jobs" />
            </td>
        </tr>
    </table>
    <% Html.EndForm(); %>
    
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">
</asp:Content>
