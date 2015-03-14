<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Home.Master" Inherits="System.Web.Mvc.ViewPage<Dial4Jobz.Models.LogOnModel>" %>

<asp:Content ID="loginTitle" ContentPlaceHolderID="TitleContent" runat="server">
   Dial4Jobz
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
   <script src="<%=Url.Content("~/Scripts/Dial4Jobz.Home.js")%>" type="text/javascript" ></script>
   <script src="<%=Url.Content("~/Scripts/Dial4Jobz.GoogleAnalyticsSite.js")%>" type="text/javascript"></script>

     <script type="text/jscript">
         $(document).ready(function () {
             var tempDDVal = '<%= Session["Function"] %>';
             $("#ddlRoles option:contains(" + tempDDVal + ")").attr('selected', 'selected');
             // $("#ddlRoles option:contains(" + tempDDVal + ")").find('option: selected').attr('Functions');
         });
                

         function ddlRoleChanged() {
             var selectedValue = $('#ddlRoles option:selected').val();
             window.location = '/employer/MatchCandidates?func=' + selectedValue.valueOf();
         };        
</script>   

   <script type="text/jscript">
       //For Find jobs popup
       $(document).ready(function () {

           //For welcome text
           var loginAs = '<%= Session["LoginAs"] %>';
           var userName = '<%= Session["UserName"] %>';
           if (loginAs == "Candidate") {
               // alert("cand");
               var lblText = "Welcome " + userName + "!!! , You are logged in as Job seeker.. We wish you to get your Dream job....";
           }
           else if (loginAs == "Employer") {
               lblText = "Welcome " + userName + "!!! , You are logged in as Employer.. We wish you to get the right candidate for your Vacancy...";
           }
           else {
               //lblText = "Welcome to Dial4Jobz, India's 1st Interactive Job portal. All Services are free for this month for all!";
           }
           $('#<%=lblMarquee.ClientID %>').html(lblText);
           //End od marque
           $('#imgFindJobs').click(function () {
               $('#popup_box').fadeIn("slow");

               $("#divMain").css({
                   "opacity": "0.3"
               });
           });

           $('#imgFindJobsClose').click(function () {
               $('#popup_box').fadeOut("slow");

               $("#divMain").css({
                   "opacity": "1"
               });
           });

           $('#imgHomeNumber1').click(function () {
               $('#popup_box').fadeOut("slow");

               $("#divMain").css({
                   "opacity": "1"
               });
           });

           $('#imgCloseFindJobSeekers').click(function () {
               $('#divFindJobseekers').fadeOut("slow");

               $("#divMain").css({
                   "opacity": "1"
               });
           });
       });


</script>
 </asp:Content>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContent" runat="server">
 <div id="divMain"> 
    
    <table id="tblLogin" runat="server" class="HomeResolution" cellpadding="0" 
         cellspacing="0" 
         style="margin-left:-18px; margin-top:-20px; margin-bottom:-40PX; height:747px; border: solid 2px #888;">
        <tr id="trLogin" class="LoginRow">
            <td style="border-color:White; padding-left:6px; height:10px;">
                 <%var login = Session["LoginAs"]; %>
            
                    <%if (login == "Candidate" && Request.IsAuthenticated==true)
                      { %>
                        <a  href="<%=Url.Action("Edit", "Candidates")%>?value=candidate" style="color:Black;font-size:13px;font-weight:normal;font-family: Bookman Old Style;" title="Candidate Profile"><img src="../../Content/Images/updateProile.jpg" width="130"; height="40"; /></a>                                        
                    <%} else { %>
                        <a class="login" href="<%=Url.Content("~/login")%>?value=candidate" style="color:Black;font-size:13px;font-weight:normal;font-family:Bookman Old Style;" title="Login & Register as Candidate"><img src="../../Content/Images/candilogin.jpg" width="130"; height="40"; /></a>
                    
                    <%} %>
            </td>
            <td style="border-color:White; color:White; width:700px; height:10px;">                
            </td>
            <td style="border-color:White; padding-right:6px; height:10px; padding-left:13px;">
            
                   <%var login = Session["LoginAs"]; %>
                   <%if (login == "Employer" && Request.IsAuthenticated==true)
                     { %>
                      <a  href="<%=Url.Action("Profile", "Employer")%>" style="color:Black;font-size:13px;font-weight:normal;font-family:Bookman Old Style;" title="Update Employer Profile"><img src="../../Content/Images/updateProfile1.jpg" width="130"; height="40"; /></a>         
                      <%} else{ %>
                            <a class="login" href="<%=Url.Content("~/login")%>?value=employer" style="color:Black;font-size:13px;font-weight:normal;font-family:Bookman Old Style;" title="Login & Register as Employer"><img src="../../Content/Images/empLogin.jpg" width="130"; height="40"; /> </a>
                      <% } %>
            </td>
        </tr>
        <tr id="trNumbers">
            <td style="border-color:White; padding-left:2px;  height:10px;">               
               <a class="homePopup" href="<%=Url.Content("Home/HomePhoneNumber")%>"><img id="img2" src="../../Content/Images/HomeNumber1.png" height="25px" width="112px"/></a>              
            </td>          
            <td style="border-color:White; width:800px; padding-left:20px; color:White; height:10px;">
                <asp:Label ID="lblMarquee" CssClass="Marquee_Text" Text="" 
                        runat="server"></asp:Label>                
            </td>
                <td style="border-color:White;padding-right:2px;height:10px;">                     
                     <a class="homeMobile" href="<%=Url.Content("Home/CreatePopupMobile")%>"><img id="imgHomeNumber2" src="../../Content/Images/HomeNumber2.png" alt="click this number to save in your mobile" height="25px" width="112px"/></a>              
                </td>            
        </tr>
        
        <%--<tr style=" height:100px">--%>
        <tr style=" height:43px">
         <td style="border-color:White; padding-left:2px;  height:10px;">               
            
         </td>          
         <td style="border-color:White; width:800px; padding-left:20px; color:White; height:10px;">
                <asp:Label ID="Label5" CssClass="Marquee_Text" Text="" 
                        runat="server"></asp:Label>
                       
          </td>
          <td style="border-color:White;padding-right:2px;height:10px;">                     
               <a href="https://plus.google.com/u/1/b/112676010347811639727/112676010347811639727/posts" target="_blank"><img src="../../Content/Images/gooplus.png" width="20px";height="20px"; alt="Follow me on Google Plus" /></a>&nbsp;
               <a href="https://www.facebook.com/pages/dial4jobzcom/741395695889731" target="_blank"><img src="../../Content/Images/facebook.png" width="20px";height="20px";  alt="Follow me on Facebook"/></a>&nbsp;                
               <a href="http://www.linkedin.com/company/dial4jobz-millennium%27s?trk=tabs_biz_home" target="_blank"><img src="../../Content/Images/linkedn.png" width="20px";height="20px"; alt="Follow me on LinkedIn"/></a>&nbsp;
           </td>    

         
        </tr>
        <tr id="trLogo" runat="server" style="vertical-align:bottom; padding-top:50px;color:#EC672A;font-size:17px;font-weight:bold">
            <td id="tdLogo" align="center" runat="server" colspan="3" style=" border-color:White; vertical-align:bottom;">                
              <img src= "../../Content/Images/d4jlogo 12102013.jpg" height="120px" width="330px" />  <br />
              Any Role / Any Level / Any Industry
            </td>
        </tr>        
        <tr id="tr1" runat="server">           
            <td id="td2" runat="server" align="center" colspan="3" style="border-color:White; padding:23px;">
                <table>
                    <tr>
                      <td style="padding-right:2px; padding-left:159px;">
                     <%--<b>For Employers to Search Candidates Here</b> <img src="../../Content/Images/searchjobseekers.jpg" width="53px" height="50px" /> --%>
                         <label id="Label1" style="font-weight:bold; color:Orange; font-size:17px; font-family:Calibri;"  runat="server">Employers!!! Search Candidates Here</label><img src="../../Content/Images/searchjobseekers.jpg" width="42" height="25" alt="Search Job Seekers">  <%= Html.DropDownList("RolesForJobSeekers", ViewData["RolesForJobSeekers"] as SelectList,"Find Candidates" , new {onchange = "javascript:ddlRoleChanged();", id="ddlRoles",  @class = "dropdownStyle2"})%>
                      </td>
                    </tr>
                    
                    <tr>
                        <td align="center" style=" border-color:White; padding-right:80px; padding-left:10px">
                          <table cellpadding="2" cellspacing="4" style="border-color:White; width: 581px;height: 288px;">
                        <tr>
                            <td align="center" colspan="2" style="border-color:White; width:100%;">
                                <%--<label id="Label6" style="font-weight:bold; color:Orange; font-size:17px"  runat="server">Find Jobs</label>--%>
                            </td>
                        </tr>
                        <tr style="width:652px; height:268px;">
                        <td style="border-color:White; width:25%">         
                        <%--<label id="Label7" style="font-weight:bold; color:Orange; text-align:center; font-size:17px"  runat="server">Find Jobs</label><br /><br />--%><%--&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>           
                            <%
                                Dial4Jobz.Models.LogOnModel  m = new Dial4Jobz.Models.LogOnModel();
                                var tempTitles = m.lstTitles;
                                var tempFunctions = m.lstFunctios;
                                int count = tempFunctions.Count / 3;
                                
                                for(int i = 0; i<count; i++)
                                {%>
                                 <%--<a class="functions" href="/jobs/Index?function=<%=tempFunctions[i]%>&Flag=HomePage"><%=tempTitles[i]%></a><br />   --%>                    
                                   <a class="functions" href="/jobs/Index?function=<%=tempFunctions[i]%>"><%=tempTitles[i]%></a><br /> 
                                <%} 
                            %>
                        </td> 
                        <td style="border-color:White; width:25%">                    
                            <% for (int i = count; i < 16; i++) { %>
                                <%--<a class="functions" href="/jobs/Index?function=<%=tempFunctions[i]%>&Flag=HomePage"><%=tempTitles[i]%></a><br /> --%>          
                                 <a class="functions" href="/jobs/Index?function=<%=tempFunctions[i]%>"><%=tempTitles[i]%></a><br />
                            <% } %>
                        </td>   
                         <td style="border-color:White; width:19%">                    
                            <% for (int i = 16; i < tempFunctions.Count; i++)
                               { %>
                                <%--<a class="functions" href="/jobs/Index?function=<%=tempFunctions[i]%>&Flag=HomePage"><%=tempTitles[i]%></a><br />--%>         
                                <a class="functions" href="/jobs/Index?function=<%=tempFunctions[i]%>"><%=tempTitles[i]%></a><br /> 
                            <% } %>
                        </td>                    
                </tr> 
        </table> 
        </td>

        </tr>      
        </table>  
        </td>
        </tr>
     <%--   <tr style="height:400px">
        <td colspan="3">
  
            <div class="outer-circle">
            </div>
        </td>
        </tr>--%>
     </table>       
    </div>

    <%--For  Find jobs Pop up        --%>
    <%--<div id="popup_box" class="popup_box" style="display:none">
        <table style="border-color:White; width:100%;">
            <tr>
                <td align="center" colspan="2" style="border-color:White; width:100%;">
                    <label id="Label8" style="font-weight:bold; font-size:17px"  runat="server">Choose Your Dream Job!!!</label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <img id="imgFindJobsClose" src="<%=Url.Content("~/Content/Images/Homecancel.png")%>" style="cursor:pointer" alt="Cancel" height="20px" width="20px" />
                </td>
            </tr>
            <tr>
                <td style="border-color:White; width:50%">                    
                    <%
                        Dial4Jobz.Models.LogOnModel  m = new Dial4Jobz.Models.LogOnModel();
                        var tempTitles = m.lstTitles;
                        var tempFunctions = m.lstFunctios;
                        int count = tempFunctions.Count / 2;
                        for(int i = 0; i<count; i++)
                        {%>
                              <a class="functions" href="/jobs/Index?what=<%=tempFunctions[i]%>&Flag=HomePage"><%=tempTitles[i]%></a><br />                         
                        <%} 
                    %>
                </td>
                <td style="border-color:White; width:50%">                    
                    <%                        
                        for (int i = count; i < tempFunctions.Count; i++)
                        {%>
                           <a class="functions" href="/jobs/Index?what=<%=tempFunctions[i]%>&Flag=HomePage"><%=tempTitles[i]%></a><br />                        
                        <%}
                    %>
                </td>                
            </tr> 
        </table>
    </div>--%>
   
 </asp:Content>

