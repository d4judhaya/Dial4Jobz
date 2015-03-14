<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Plans
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="editor-field" style="font-family:Bookman Old Style; color:Black; font-size:14px;">
       <h3>Consultant Best Pricing Plans</h3>
    </div>

    

    <table id="ResumeWrite" class="Vasprice" cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr class="FE">
            <th class="bdrL_blue valignT highlight" colspan="9">
            <div class="editor-field" style="font-family:Bookman Old Style; color:Black; font-size:14px;"><h3>Add Candidates</h3></div>
                <ul>
                      
                    <li> Candidate Contact details can be Recruitment Consultants Details or Candidates Personal details</li>
                    <li> All Candidate Profiles Submitted will be displayed in all suitable Searches by Employers / Recruiters </li>
                    <li> Consultants will be provided an Unique ID for every Resume Submitted</li>.
                    <li> Resumes once submitted can be edited / updated only if the subscription is active</li>
                    <li> Consultants discretion to Charge a fee from the Employer</li>
                </ul>
            </th>
        </tr>
        <tr>
             <th colspan="3" style="text-align:center;" class="bdrL_blue valignT highlight">
                <strong class="font14" style="text-align:center">Plan</strong>
            </th>

            <th colspan="1" class="bdrL_blue valignT highlight">
                <strong class="font14">Submission of Resumes with Contact Details</strong>
            </th>

            <th colspan="1" class="bdrL_blue valignT highlight">
                <strong class="font14">Duration</strong>
            </th>

            <th colspan="1" class="bdrL_blue valignT highlight">
                <strong class="font14">Amount</strong>
            </th>
            <th  colspan="1" class="bdrL_blue valignT highlight">  
               
            </th>
        </tr>

         <tr align="center">
            <th colspan="3" style="text-align:center;" class="bdrL_blue valignT highlight">
                <strong class="font14" style="text-align:center">RC30</strong>
            </th>

            <th colspan="1" style="text-align:center;" class="bdrL_blue valignT highlight">
                <strong class="font14">150</strong>
            </th>

            <th colspan="1" class="bdrL_blue valignT highlight">
                <strong class="font14">30</strong>
            </th>

            <th colspan="1" class="bdrL_blue valignT highlight">
                <strong class="font14">1000</strong>
            </th>

            <th colspan="1" class="bdrL_blue valignT highlight">
                <a><img src="../../Content/Images/Subscribe now on click.png"  alt="subscribe now" width="100px" height="25px" class="btn" onclick="return vas_confirm('RC30','Consultant Plans','1000')" /></a>
            </th>
           
        </tr>

         <tr align="center">
            <th colspan="3" style="text-align:center;" class="bdrL_blue valignT highlight">
                <strong class="font14" style="text-align:center">RC60</strong>
            </th>

            <th colspan="1" style="text-align:center;" class="bdrL_blue valignT highlight">
                <strong class="font14">300</strong>
            </th>

            <th colspan="1" class="bdrL_blue valignT highlight">
                <strong class="font14">60</strong>
            </th>

            <th colspan="1" class="bdrL_blue valignT highlight">
                <strong class="font14">1800</strong>
            </th>

            <th colspan="1" class="bdrL_blue valignT highlight">
                <a><img src="../../Content/Images/Subscribe now on click.png"  alt="subscribe now" width="100px" height="25px" class="btn" onclick="return vas_confirm('RC60','Consultant Plans','1800')" /></a>
            </th>
           
        </tr>

        <tr align="center">
            <th colspan="3" style="text-align:center;" class="bdrL_blue valignT highlight">
                <strong class="font14" style="text-align:center">RC90</strong>
            </th>

            <th colspan="1"  style="text-align:center;" class="bdrL_blue valignT highlight">
                <strong class="font14">500</strong>
            </th>

            <th colspan="1" class="bdrL_blue valignT highlight">
                <strong class="font14">90</strong>
            </th>

            <th colspan="1" class="bdrL_blue valignT highlight">
                <strong class="font14">2700</strong>
            </th>

            <th colspan="1" class="bdrL_blue valignT highlight">
                <a><img src="../../Content/Images/Subscribe now on click.png"  alt="subscribe now" width="100px" height="25px" class="btn" onclick="return vas_confirm('RC90','Consultant Plans','2700')" /></a>
            </th>
           
        </tr>

         <tr align="center">
            <th colspan="3" style="text-align:center;" class="bdrL_blue valignT highlight">
                <strong class="font14" style="text-align:center">RC180</strong>
            </th>

            <th colspan="1" style="text-align:center;" class="bdrL_blue valignT highlight">
                <strong class="font14">No Limit</strong>
            </th>

            <th colspan="1" class="bdrL_blue valignT highlight">
                <strong class="font14">180</strong>
            </th>

            <th colspan="1" class="bdrL_blue valignT highlight">
                <strong class="font14">6000</strong>
            </th>

            <th colspan="1" class="bdrL_blue valignT highlight">
                <a><img src="../../Content/Images/Subscribe now on click.png"  alt="subscribe now" width="100px" height="25px" class="btn" onclick="return vas_confirm('RC180','Consultant Plans','6000')" /></a>
            </th>
           
        </tr>

        <tr align="center">
            <th colspan="3" style="text-align:center;" class="bdrL_blue valignT highlight">
                <strong class="font14" style="text-align:center">RC365</strong>
            </th>

            <th colspan="1" style="text-align:center;" class="bdrL_blue valignT highlight">
                <strong class="font14">No Limit</strong>
            </th>

            <th colspan="1" class="bdrL_blue valignT highlight">
                <strong class="font14">365</strong>
            </th>

            <th colspan="1" class="bdrL_blue valignT highlight">
                <strong class="font14">10000</strong>
            </th>

            <th colspan="1" class="bdrL_blue valignT highlight">
                <a><img src="../../Content/Images/Subscribe now on click.png"  alt="subscribe now" width="100px" height="25px" class="btn" onclick="return vas_confirm('RC365','Consultant Plans','10000')" /></a>
            </th>
           
        </tr>
       
    </table>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    <script type="text/javascript">
        function vas_confirm(plan, vasType, amount) {
            if (confirm("Confirm the order to buy for Rs. " + amount + "")) {
                if (plan != undefined) {
                    $("#wait").show();
                    $.ajax({
                        url: '/employer/employervas/Subscribed',
                        type: 'POST',
                        data: { 'Plan': plan, 'VasType': vasType },
                        datatype: 'json',
                        success: function (response) {
                            if (response.Success) {
                                Dial4Jobz.Common.ShowMessageBar(response.Message);
                            }

                            if (response.ReturnUrl != null) {
                                window.location = response.ReturnUrl;
                            }
                        },
                        error: function (xhr, status, error) {
                        }
                    });
                }

            }
            else {
                return false;
            }
        }   
    </script>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="NavContent" runat="server">
    <%Html.RenderPartial("NavConsultant");%>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="SideContent" runat="server">
</asp:Content>
