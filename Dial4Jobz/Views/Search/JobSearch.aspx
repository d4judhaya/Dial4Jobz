<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Dial4Jobz.Models.Job>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	JobSearch
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">

<script src="<%=Url.Content("~/Scripts/Dial4Jobz.Candidate.js")%>" type="text/javascript"></script>

    <link href="../../Content/jquery-ui.1.8.1.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../Scripts/jquery.min.js"></script>
    <script type="text/javascript" src="../../Scripts/jquery-ui.min.1.8.1.js"></script>
    <script type="text/javascript">
        $(function () {

            $('#what').autocomplete({
                source: function (request, response) {
                    $.ajax(
                    {
                    url: "/Search/getSkillData", type: "GET", dataType: "json",
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

            $(function () {
                $('#CandidateFunctions').change(function () {
                    $.ajax({
                        url: "/Candidates/Roles",
                        type: 'GET',
                        data: { 'functionId': $(this).val() },
                        dataType: 'json',
                        success: function (response) {
                            if (response.Success) {
                                $('#Roles').children().remove();

                                var results = [];
                                // Converting the JSON object into an array
                                $.each(response.Roles, function (index, role) {
                                    results.push(role);
                                });

                                // Sorting the array items by Id
                                results.sort(function (a, b) {
                                    return a.Id - b.Id;
                                });

                                $.each(results, function (index, role) {
                                    $('#Roles').append(
            '<option value="' + role.Id + '">' +
                role.Name +
            '</option>');
                                });
                            }
                        },
                        error: function (xhr, status, error) {

                        }
                    });
                });
            });

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

        $(function () {
            $('#where').autocomplete({
                source: function (request, response) {
                    $.ajax(
            {
                url: "/Search/getData", type: "GET", dataType: "json",
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

            $("#where").bind("keydown", function (event) {
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
            $('select').each(function () {
                $(this).val("");
            });
        });

        $("#Search").click(function () {
            $("input[type=hidden]").each(function () {
                $(this).val("");
            });
            $("input:radio[name=RefineSalary]:checked").each(function () {
                this.checked = false;
            });
            $("input:radio[name=RefineExp]:checked").each(function () {
                this.checked = false;
            });
            $("input:checkbox[name=RefineOrganization]:checked").each(function () {
                this.checked = false;
            });
            $("input:checkbox[name=RefineFunction]:checked").each(function () {
                this.checked = false;
            });
        });

        function checkSalaryMax() {
            var SalaryMin = document.getElementById("MinAnnualSalaryLakhs").value;
            var SalaryMax = document.getElementById("MaxAnnualSalaryLakhs").value;

            if (SalaryMin == "0") {
                document.getElementById("MaxAnnualSalaryLakhs").selectedIndex = 0;
            }

            if (SalaryMax == "0") {
                document.getElementById("MinAnnualSalaryLakhs").selectedIndex = 0;
            }

            if (SalaryMin == "51") {
                document.getElementById("MaxAnnualSalaryLakhs").selectedIndex = 0;
            }

            if (SalaryMax == "51") {
                document.getElementById("MinAnnualSalaryLakhs").selectedIndex = 0;
            }

            if (SalaryMax != "") {
                if (parseInt(SalaryMax, 10) <= parseInt(SalaryMin, 10)) {
                    alert("Maximum Salary should be greater than Minimum Salary");
                    document.getElementById("MaxAnnualSalaryLakhs").selectedIndex = 0;
                    document.getElementById("MaxAnnualSalaryLakhs").focus();
                    return false;
                }
            }
        }

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        #CandidateFunctions, #PreferredIndustries
        {
            width: 260px;
        }
    </style>
    <% Html.BeginForm("JobResults", "Search", FormMethod.Post, new { target="_blank" }); %>
    <table width="100%">
        <tr>
            <h1>
                Search Jobs</h1>
            <br />

            <h3>Search Your Technical Skills: </h3>
            <td>
                <input id="what" name="what" type="text" placeholder="job title, skill, or company..."style="width: 230px;" />
            </td>

           
            <td>
            <h3>Search Location: </h3>
                <input id="where" name="where" placeholder="Type City Name..." type="text" style="width: 230px; margin-left: 0px; margin-right: 0px;" />
            </td>

        </tr>
          
        <tr>
            <td>
                <h3>Search Your Languages: </h3><br />
                    <input id="language" name="language" type="text" placeholder="Languages..." style="width: 230px;" />
            </td>
        </tr>

        <tr>
            <td>
                <h3>Job Function : </h3>
                <br />
                <%: Html.DropDownList("CandidateFunctions", "--Any--")%>
            </td>
            <td>
                 <h3>Job Roles: </h3><br />
                <%: Html.DropDownList("Roles","--Any--") %>
            </td>
        </tr>

        <tr>
            <td>
                <h3>Experience</h3>
                <br />
                <%: Html.DropDownList("MinExperienceYears", (SelectList)ViewData["MinExperienceYears"])%>
                <%: Html.DropDownList("TotalExpMonths", (SelectList)ViewData["Months"])%>
            </td>
        </tr>

        <tr>
            <td valign="top">
                <h3>Preferred Industries : </h3>
                <br />
                <%= Html.ListBox("PreferredIndustries", new SelectList((System.Collections.Generic.IEnumerable<SelectListItem>)ViewData["Industries"], "Value", "Text"))%>
            </td>
            <td>
                <h3>Salary Expectation : </h3>
                <br />
                <%: Html.DropDownList("MinAnnualSalaryLakhs", (SelectList)ViewData["MinAnnualSalaryLakhs"], new { @onchange = "checkSalaryMax()" })%>
                -
                <%: Html.DropDownList("MaxAnnualSalaryLakhs", (SelectList)ViewData["MaxAnnualSalaryLakhs"], new { @onchange = "checkSalaryMax()" })%>
                <br />
                <span style="font-size: 11px; color: #888888;">In Lakhs per Annum </span>
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
            </td>
        </tr>

         <tr>
            <td colspan="2">
            <h3>Preferred Work Shift: </h3><br />
                <input type="checkbox" name="TypeOfShift" value="1" />
                General Shift
                <input type="checkbox" name="TypeOfShift" value="2" />
                Night Shift
            </td>
        </tr>

        <tr>
            <td>
                <h3>Freshness:</h3><br />
                <%=Html.DropDownList("Freshness", new[]{
                                new SelectListItem{ Text="Select", Value=""},
                                new SelectListItem{ Text="< 1 Day", Value="1"},
                                new SelectListItem{ Text="< 1 Week", Value="2"},
                                new SelectListItem{ Text="< 1 Month", Value="3"}                                
                                }, new { @style = "width:100px" })%>
            </td>

            <td style="padding-left: 15px;">
                <input id="Search" type="submit" value="Search" class="btn-search" title="Search jobs" />
            </td>
        </tr>
    </table>
        
        <% Html.EndForm(); %>
        
</asp:Content>



<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
	<% Html.RenderPartial("Nav"); %>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">
    <% Html.RenderPartial("Side/Welcome"); %> 
    <% Html.RenderPartial("Side/GettingStarted"); %> 
    <% Html.RenderPartial("Side/Video"); %> 
</asp:Content>
