<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Dial4Jobz.Models.Organization>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Dial4Jobz - Add New Organization
</asp:Content>

<asp:Content ContentPlaceHolderID="ScriptContent" runat="server">
    <script src="<%=Url.Content("~/Scripts/Dial4Jobz.Organization.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="add">
    <% Html.BeginForm("Add", "Organizations", FormMethod.Post, new { @id = "Add" }); %>
   
<h2>Organization Contact Details</h2>
        <div class="editor-label">
           <%: Html.Label("Name") %>
        </div>
        <div class="editor-field">
            <%: Html.TextBox("Name", "", new { @title = "Enter the organization's name" })%> 
        </div>

        <div class="editor-label">
           <%: Html.Label("Industry")%>
        </div>
        <div class="editor-field">
          <%: Html.DropDownList("Industries", (SelectList) ViewData["Industries"], "Select")%> 
        </div> 

        <div class="editor-label">
          <%: Html.Label("Contact Person")%>
        </div>
        <div class="editor-field">
            <%: Html.TextBox("ContactPerson", "", new { @title = "Enter the organization's contact person" })%>   
        </div>

        <div class="editor-label">
          <%: Html.Label("Email")%>
        </div>
        <div class="editor-field">
            <%: Html.TextBox("Email", "", new { @title = "Enter the organization's email" })%>   
        </div>

        <div class="editor-label">
          <%: Html.Label("Website")%>
        </div>
        <div class="editor-field">
            <%: Html.TextBox("Website", "", new { @title = "Enter the organizations's website" })%>   
        </div>

        <div class="editor-label">
          <%: Html.Label("Location")%>
        </div>
        <div class="editor-field">
            <%: Html.DropDownList("Country", (SelectList) ViewData["Countries"], "Country")%>   
            <select id="State" name="State"></select>
            <select id="City" name="City"></select>
            <select id="Region" name="Region"></select>
        </div>

        <div class="editor-label">
          <%: Html.Label("Contact Number")%>
        </div>
        <div class="editor-field">
          <%: Html.TextBox("ContactNumber", "", new { @title = "Enter the organization's contact number" })%>    
        </div>

        <div class="editor-label">
            <%: Html.Label("Mobile Number")%>
        </div>
        <div class="editor-field">
            <%: Html.TextBox("MobileNumber", "", new { @title = "Enter the organization's mobile number" })%>  
        </div> 
         
<h2>Requirement Summary</h2>

<div class="editor-label">
    <%: Html.Label("Basic Qualification")%>
</div>
<div class="editor-field">
    <div id="basic-qualification-container1" style="margin-bottom:4px;" class="basic-qualification-container">
        <%: Html.DropDownList("BasicQualificationDegree1", (SelectList)ViewData["BasicQualificationDegrees"], "Any")%>   
        <%: Html.TextBox("BasicQualificationSpecialization1", "", new { @title = "Enter required basic qualification degree" })%>    
    </div>

    <div>
        <input type="button" id="btnAddBasicQualification" value="add another basic qualification" />
        <input type="button" id="btnDelBasicQualification" value="remove basic qualification" />
    </div>
</div>

<div class="editor-label">
    <%: Html.Label("Post Graduation")%>
</div>
<div class="editor-field">
    <div id="post-graduation-container1" style="margin-bottom:4px;" class="post-graduation-container">
        <%: Html.DropDownList("PostGraduationDegree1", (SelectList)ViewData["PostGraduationDegrees"], "Any")%>   
        <%: Html.TextBox("PostGraduationSpecialization1", "", new { @title = "Enter required post graduation degree" })%>    
    </div>

    <div>
        <input type="button" id="btnAddPostGraduation" value="add another post graduation" />
        <input type="button" id="btnDelPostGraduation" value="remove post graduation" />
    </div>
</div>

<div class="editor-label">
    <%: Html.Label("Doctorate")%>
</div>
<div class="editor-field">
    <div id="doctorate-container1" style="margin-bottom:4px;" class="doctorate-container">
        <%: Html.DropDownList("DoctorateDegree1", (SelectList)ViewData["DoctorateDegrees"], "Any")%>   
        <%: Html.TextBox("DoctorateSpecialization1", "", new { @title = "Enter required doctorate degree" })%>    
    </div>

    <div>
        <input type="button" id="btnAddDoctorate" value="add another doctorate" />
        <input type="button" id="btnDelDoctorate" value="remove doctorate" />
    </div>
</div>

<div class="editor-label">
    <%: Html.Label("Functional Area")%>
</div>
<div class="editor-field">
    <%: Html.DropDownList("Functions", (SelectList) ViewData["Functions"], "Any")%> 
</div> 

<div class="editor-label">
    <%: Html.Label("Preferred Industries")%>
</div>
<div class="editor-field">
    <input id="PreferredIndustries" type="text" name="PreferredIndustries" />   
</div>

<div class="editor-label">
    <%: Html.Label("Location(s) of Posting")%>
</div>
<div class="editor-field">
    <div id="location-container1" style="margin-bottom:4px;" class="location-container">
        <%: Html.DropDownList("PostingCountry1", (SelectList) ViewData["Countries"], "Select Country")%>   
        <select id="PostingState1" name="PostingState1"></select>
        <select id="PostingCity1" name="PostingCity1"></select>
        <select id="PostingRegion1" name="PostingRegion1"></select>
    </div>

    <div>
        <input type="button" id="btnAddLocation" value="add another location" />
        <input type="button" id="btnDelLocation" value="remove location" />
    </div>
</div>

<div class="editor-label">
    <%: Html.Label("Position")%>
</div>
<div class="editor-field">
    <%: Html.TextBox("Position", "", new { @title = "Enter the name of this position" })%>    
</div> 

<div class="editor-label">
    <%: Html.Label("Gender")%>
</div>
<div class="editor-field">
    <%: Html.CheckBox("Male") %> Male  
    <%: Html.CheckBox("Female")%> Female
</div> 

<div class="editor-label">
           <%: Html.Label("Experience")%>
        </div>
        <div class="editor-field">
           <select id="MinExperienceYears" name="MinExperienceYears">
              <option value="1">0</option>
              <option value="1">1</option>
              <option value="2">2</option>
              <option value="3">3</option>
              <option value="4">4</option>
              <option value="5">5</option>
              <option value="6">6</option>
              <option value="7">7</option>
              <option value="8">8</option>
              <option value="9">9</option>
              <option value="10">10</option>
              <option value="11">11</option>
              <option value="12">12</option>
           </select> MinExpyears

           <select id="MaximumExperienceYears" name="MaximumExperienceYears">
              <option value="1">0</option>
              <option value="1">1</option>
              <option value="2">2</option>
              <option value="3">3</option>
              <option value="4">4</option>
              <option value="5">5</option>
              <option value="6">6</option>
              <option value="7">7</option>
              <option value="8">8</option>
              <option value="9">9</option>
              <option value="10">10</option>
              <option value="11">11</option>
              <option value="12">12</option>
           </select> MaxExpYears
        </div> 
        
        <div class="editor-label">
           <%: Html.Label("Budget")%>
        </div>
        <div class="editor-field">
           <select id="BudgetLakhs" name="BudgetLakhs">
              <option value="1">0</option>
              <option value="1">1</option>
              <option value="2">2</option>
              <option value="3">3</option>
              <option value="4">4</option>
              <option value="5">5</option>
              <option value="6">6</option>
              <option value="7">7</option>
              <option value="8">8</option>
              <option value="9">9</option>
              <option value="10">10</option>
              <option value="11">11</option>
              <option value="12">12</option>
              <option value="13">13</option>
              <option value="14">14</option>
              <option value="15">15</option>
              <option value="16">16</option>
              <option value="17">17</option>
              <option value="18">18</option>
              <option value="19">19</option>
              <option value="20">20</option>
              <option value="21">21</option>
              <option value="22">22</option>
              <option value="23">23</option>
              <option value="24">24</option>
              <option value="25">25</option>
              <option value="26">26</option>
              <option value="27">27</option>
              <option value="28">28</option>
              <option value="29">29</option>
              <option value="30">30</option>
              <option value="31">31</option>
              <option value="32">32</option>
              <option value="33">33</option>
              <option value="34">34</option>
              <option value="35">35</option>
              <option value="36">36</option>
              <option value="37">37</option>
              <option value="38">38</option>
              <option value="39">39</option>
              <option value="40">40</option>
              <option value="41">41</option>
              <option value="42">42</option>
              <option value="43">43</option>
              <option value="44">44</option>
              <option value="45">45</option>
              <option value="46">46</option>
              <option value="47">47</option>
              <option value="48">48</option>
              <option value="49">49</option>
              <option value="50">50</option>
              <option value="51">50+</option>
           </select> lakhs

           <select id="BudgetThousands" name="BudgetThousands">
               <option value="0">0</option>
              <option value="1">10</option>
              <option value="2">15</option>
              <option value="3">20</option>
              <option value="4">25</option>
              <option value="5">30</option>
              <option value="6">35</option>
              <option value="7">40</option>
              <option value="8">45</option>
              <option value="9">50</option>
              <option value="10">55</option>
              <option value="11">60</option>
              <option value="12">65</option>
              <option value="13">70</option>
              <option value="14">75</option>
              <option value="15">80</option>
              <option value="16">85</option>
              <option value="17">90</option>
              <option value="18">95</option>
           </select> Thousands   
        </div>   
           
            <div class="editor-label">
           <%: Html.Label("MaxBudget")%>
        </div>
        <div class="editor-field">
           <select id="MaximumSalaryLakhs" name="MaximumSalaryLakhs">
              <option value="1">0</option>
              <option value="1">1</option>
              <option value="2">2</option>
              <option value="3">3</option>
              <option value="4">4</option>
              <option value="5">5</option>
              <option value="6">6</option>
              <option value="7">7</option>
              <option value="8">8</option>
              <option value="9">9</option>
              <option value="10">10</option>
              <option value="11">11</option>
              <option value="12">12</option>
              <option value="13">13</option>
              <option value="14">14</option>
              <option value="15">15</option>
              <option value="16">16</option>
              <option value="17">17</option>
              <option value="18">18</option>
              <option value="19">19</option>
              <option value="20">20</option>
              <option value="21">21</option>
              <option value="22">22</option>
              <option value="23">23</option>
              <option value="24">24</option>
              <option value="25">25</option>
              <option value="26">26</option>
              <option value="27">27</option>
              <option value="28">28</option>
              <option value="29">29</option>
              <option value="30">30</option>
              <option value="31">31</option>
              <option value="32">32</option>
              <option value="33">33</option>
              <option value="34">34</option>
              <option value="35">35</option>
              <option value="36">36</option>
              <option value="37">37</option>
              <option value="38">38</option>
              <option value="39">39</option>
              <option value="40">40</option>
              <option value="41">41</option>
              <option value="42">42</option>
              <option value="43">43</option>
              <option value="44">44</option>
              <option value="45">45</option>
              <option value="46">46</option>
              <option value="47">47</option>
              <option value="48">48</option>
              <option value="49">49</option>
              <option value="50">50</option>
              <option value="51">50+</option>
           </select> lakhs

           <select id="MaximumSalaryThousands" name="MaximumSalaryThousands">
               <option value="0">0</option>
              <option value="1">10</option>
              <option value="2">15</option>
              <option value="3">20</option>
              <option value="4">25</option>
              <option value="5">30</option>
              <option value="6">35</option>
              <option value="7">40</option>
              <option value="8">45</option>
              <option value="9">50</option>
              <option value="10">55</option>
              <option value="11">60</option>
              <option value="12">65</option>
              <option value="13">70</option>
              <option value="14">75</option>
              <option value="15">80</option>
              <option value="16">85</option>
              <option value="17">90</option>
              <option value="18">95</option>
           </select> Thousands   
        </div> 
<div class="editor-label">
    <%: Html.Label("Skills")%>
</div>
<div class="editor-field">
    <input id="Skills" type="text" name="Skills" />      
</div>

<div class="editor-label">
    <%: Html.Label("Languages")%>
</div>
<div class="editor-field">
    <input id="Languages" type="text" name="Languages" />    
</div>       

<div class="editor-label">
    <%: Html.Label("Preferred Type")%>
</div>
<div class="editor-field">
    <select id="PreferredType" name="PreferredType">
        <option value="0">Any</option> 
        <option value="1">Contract</option> 
        <option value="2">Part Time</option> 
        <option value="3">Full Time</option> 
    </select>
</div>  
        
<h2>Contact Details for Requirements</h2>
     
<div class="editor-label">
    <%: Html.Label("Contact Person")%>
</div>
<div class="editor-field">
    <%: Html.TextBox("RequirementsContactPerson", "", new { @title = "Enter the contact person for this requirement" })%>    
</div>    
 
<div class="editor-label">
    <%: Html.Label("Contact Number")%>
</div>
<div class="editor-field">
    <%: Html.TextBox("RequirementsContactNumber", "", new { @title = "Enter the contact number for this requirement" })%>    
</div>   

 <div class="editor-label">
    <%: Html.Label("Mobile Number")%>
</div>
<div class="editor-field">
    <%: Html.TextBox("RequirementsMobileNumber", "", new { @title = "Enter the mobile number for this requirement" })%>    
</div>    
               
<div class="editor-label">
    <%: Html.Label("Email Address")%>
</div>
<div class="editor-field">
    <%: Html.TextBox("RequirementsEmailAddress", "", new { @title = "Enter the email address for this requirement" })%>    
</div> 
        
<div class="editor-label">
    <%: Html.Label("Communication Mode")%>
</div>
<div class="editor-field">
    <%: Html.CheckBox("CommunicationSMS")%> SMS    
    <%: Html.CheckBox("CommunicationEmail")%> Email    
</div> 
        
<input id="Add" type="submit" value="Add Requirement" class="btn" title="Click to add requirement and search for qualifying candidates" onclick="javascript:Dial4Jobz.Organization.Add(this);return false;"/>
<% Html.EndForm(); %>

</div>

<div id="results">

</div>

</asp:Content>


