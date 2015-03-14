function getHeader(app, module, action, boolSuper, boolSubHeader, consultant)
{
  var subCatIdForPremiumVacancy  = (consultant == 'y') ? '81' : '80';
  var strHTML = "<div id='mainNav'><div class='wrapper'><b class='fl lfNavCurv'> &nbsp;</b> <ul class='fl'><li><a href='"+mnr_link+"/homePage/index' id='tab1' style='border-left: 0px;'>Main Menu</a></li><li><a href='#' class='dropD' rel='jobPostN' id='tab2'>Jobs &amp; Responses ▼</a></li><li><a href='#' class='dropD' rel='resdexN' id='tab3'>Resdex ▼</a></li><li><a href='#' class='dropD' rel='reportsN' id='tab4'>Reports ▼</a></li><li><a href='#' class='dropD' rel='adminN' id='tab5'>Administration ▼</a></li></ul> <b class='fr rtNavCurv'> &nbsp;</b></div></div>";

  strHTML += "<div id='subNav'><div class='wrapper'><div class='bdrSubNav'> <div class='plr7'><span id='subtab2' style='display:none;'><span id='subtab2_1'><a href='"+jp_link+"/Job/new?subCategoryId="+subCatIdForPremiumVacancy+"'>Post a Premium Vacancy</a></span> | <span id='subtab2_2'><a href='"+jp_link+"/Job/new?subCategoryId=1'>Post a Hot Vacancy</a></span> | <span id='subtab2_3'><a href='"+jp_link+"/Job/new?subCategoryId=2'>Post a Classified</a></span> | <span id='subtab2_4'><a href='"+jp_link+"/JobListing/default'>Manage Jobs &amp; Responses</a></span> | <span id='subtab2_5'><a href='"+eapps_link+"/folder/list'>My Folders</a></span> | <span id='subtab2_6'><a href='"+eapps_link+"/question/manage'>Questionnaires</a></span> | <span id='subtab2_7'><a href='"+eapps_link+"/draft/list'>Email Templates</a></span></span>";

  strHTML += "<span id='subtab3' style='display:none;'><span id='subtab3_1'><a href='"+rdx_link+"/search/advSearch'>Search Resumes</a></span> | <span id='subtab3_2'><a href='"+rdx_link+"/search/savedSearches'>Saved Searches</a></span> | <span id='subtab3_3'><a href='"+rdx_link+"/folder/list'>Personal Folders</a></span> | <span id='subtab3_4'><a href='"+rdx_link+"/ManageTemplates/listTemplates'>Email Templates</a></span> | <span id='subtab3_5'><a href='"+rdx_link+"/instaAdmin/manage'>SMS Templates</a></span> | <span id='subtab3_6'><a href='"+rdx_link+"/settings/search'>Search Settings</a></span></span><span id='subtab4' style='display:none;'><span id='superuser_1'><span id='subtab4_1'><a href='"+jp_link+"/MIS/mis'>Job Postings</a></span> | <span id='subtab4_2'><a href='"+eapps_link+"/mis/edit'>eApps</a></span> | </span><span id='subtab4_3'><a href='"+rdx_link+"/mis/usageMis'>Resdex</a></span> | <span id='subtab4_4'><a href='"+rdx_link+"/instaMIS/selectDate'>Mobile Solutions</a></span></span><span id='subtab5' style='display:none;'><span id='subtab5_1'><a href='"+mnr_link+"/homePage/productSettings'>Product Settings</a></span> | <span id='superuser_2'><span id='subtab5_2'><a href='"+login_link+"/company/subuser'>Manage Sub-Users</a></span> | <span id='subtab5_3'><a href='"+eapps_link+"/admin/list'>Assign Jobs</a></span> | <span id='subtab5_4'><a href='"+rdx_link+"/quota/manage'>Manage Quota</a></span> | <span id='subtab5_5'><a href='"+mnr_link+"/subscription/status'>Subscription Status</a></span> | <span id='subtab5_6'><a href='"+mnr_link+"/company/edit'>Company Profile</a></span> | </span><span id='subtab5_7'><a href='"+login_link+"/company/password'>Change Password</a></span> | <span id='subtab5_8'><a href='"+mnr_link+"/company/security'>Usage Guidelines</a></span></span></span></div></div></div></div>";

  strHTML += "<div class='rc_Box' id='jobPostN' style='width:170px;'><ul style='width:170px;'><li><a href='"+jp_link+"/Job/new?subCategoryId="+subCatIdForPremiumVacancy+"'>Post a Premium Vacancy</a></li><li><a href='"+jp_link+"/Job/new?subCategoryId=1'>Post a Hot Vacancy</a></li><li><a href='"+jp_link+"/Job/new?subCategoryId=2'>Post a Classified</a></li><li><a href='"+jp_link+"/JobListing/default'>Manage Jobs &amp; Responses</a></li><li class='bdrrcone'><strong style='text-indent:10px;display:block;'>eApps</strong><div><a href='"+eapps_link+"/folder/list'>My Folders</a>  <a href='"+eapps_link+"/question/manage'>Questionnaires</a> <a href='"+eapps_link+"/draft/list'>Email Templates</a></li> </ul></div><div class='rc_Box' id='resdexN'><ul>  <li><a href='"+rdx_link+"/search/advSearch'>Search Resumes</a></li>  <li><a href='"+rdx_link+"/search/savedSearches'>Saved Searches</a></li>  <li><a href='"+rdx_link+"/folder/list'>Personal Folders</a></li>  <li><a href='"+rdx_link+"/ManageTemplates/listTemplates'>Email Templates</a></li>  <li><a href='"+rdx_link+"/instaAdmin/manage'>SMS Templates</a></li>  <li class='bdrrcone'><a href='"+rdx_link+"/settings/search'>Search Settings</a></li> </ul></div><div class='rc_Box' id='reportsN'>  <ul>  <li><a rel='superuser' href='"+jp_link+"/MIS/mis'>Job Postings</a></li>  <li><a rel='superuser' href='"+eapps_link+"/mis/edit'>eApps</a></li>  <li><a href='"+rdx_link+"/mis/usageMis'>Resdex</a></li>  <li><a href='"+rdx_link+"/instaMIS/selectDate'>Mobile Solutions</a></li> </ul></div><div class='rc_Box' id='adminN'>  <ul> <li><a href='"+mnr_link+"/homePage/productSettings'>Product Settings</a></li> <li><a rel='superuser' href='"+login_link+"/company/subuser'>Manage Sub-Users</a></li> <li><a rel='superuser' href='"+eapps_link+"/admin/list'>Assign Jobs</a></li> <li><a rel='superuser' href='"+rdx_link+"/quota/manage'>Manage Quota</a></li> <li><a rel='superuser' href='"+mnr_link+"/subscription/status'>Subscription Status</a></li> <li><a rel='superuser' href='"+mnr_link+"/company/edit'>Company Profile</a></li> <li><a href='"+login_link+"/company/password'>Change Password</a></li> <li><a href='"+mnr_link+"/company/security'>Usage Guidelines</a></li> </ul></div><div class='cl'>&nbsp;</div>";

  var e, el, ele, i = 1, j = 1;
  boolSuper = parseInt(boolSuper);
  boolSubHeader = parseInt(boolSubHeader);
  var tabID = getHeaderTabID(app, module, action);
  gbi('header').innerHTML = strHTML;
  if(tabID > 0) {
    var tab_id = gbi("tab"+tabID);
    tab_id.innerHTML = tab_id.innerHTML.replace(' ▼', '');
    tab_id.className = "dropD1";
    tab_id.rel = "";
    tab_id.parentNode.className = "sel";
  }
  if(tabID == 3) 
    gbi("tab"+tabID).style.width = "";

  if(!boolSuper) {
    while(ele = gbi("superuser_"+i)) {
      ele.style.display = "none";
      i++;
    }

    var aTags = gbi("header").getElementsByTagName("a");
    for(var a in aTags) {
      if(aTags[a].rel == 'superuser') {
        aTags[a].parentNode.style.display = 'none';
      }
    }
  }

  if(boolSubHeader && (e = gbi("subtab" + tabID)) ) {
    while(el = gbi("subtab" + tabID + "_" + j))
    {
      var arrAnchor = el.getElementsByTagName("a");
      var arrLink = arrAnchor[0].href.split("/");
      var arrAction = arrLink.pop().split("?");
      var module_curr = arrLink.pop();
      var linkName = arrAnchor[0].innerHTML;
      var boldHtml = "<strong class='blk'>" + linkName + "</strong>";

      if(module_curr == "JobListing" && app == "BRV")
      { el.innerHTML = boldHtml; break; }
      else if(j == 1 && app == "JP" && action == "xMLReports" && module == "Mis")
      { el.innerHTML = boldHtml; break; }
      else if(app == "JP" && module_curr == module && (module == "JobListing" || module == "Upload"))
      { el.innerHTML = boldHtml; break; }
      else if(app == "RDX" && module_curr == module && ( (module == "search" && (action != "savedSearches" && action != "sharedSavedSearches") || (action == "sharedSavedSearches" && boldHtml.match("Saved Searches"))) || module == "folder" || module == "ManageTemplates" || module == "settings" || module == "instaMIS" || module == "instaAdmin" || (module == "mis" && boldHtml.match('Resdex') )))
      { el.innerHTML = boldHtml; break; }
      else if(app == "EAPPS" && module_curr == module && (module == "folder" || module == "draft" || module == "question" || module == "upload" || (module == "mis" && boldHtml.match("eApps")) || module == "othermedia"))
      { el.innerHTML = boldHtml; break; }
      else if(app == "RSC" && ((module_curr == module && module != 'mis') || (module == "search" && action == "duplicate")) || ( (action == "mis" || action == "generateReport") && boldHtml.match("Showcase")))
      { el.innerHTML = boldHtml; break; }
      else if( (arrAction[0] == action && module_curr == module) || (app == "JP" && (arrAction[0] == "new" || arrAction[0] == "newPreview" || arrAction[0] == "edit" || arrAction[0] == "refreshConfirmation")) )
      {
        if(app == "JP" && (arrAction[0] == "new" || arrAction[0] == "newPreview" || arrAction[0] == "edit" || arrAction[0] == "refreshConfirmation")) {
          if(arrAction[1]) {
            var catId = parseInt(arrAction[1].split('=').pop());
            if(catId == parseInt(subCatId))
            { if(module != "Upload") el.innerHTML = boldHtml; }
          }
        }
        else if(arrAction[0] == action && module_curr == module)
        { el.innerHTML = boldHtml; break; }
      }
      j++;
    }
    e.style.display = "";
  }
  else gbi("subNav").style.display = "none";
}

function getHeaderTabID(app, module, action) {
  if(app == "MNR" && (module == "homePage" && action != "productSettings")) return 1;
  else if( (app == "JP" && module != "MIS" && module != 'Mis') || (app == "BRV") || (app == "EAPPS" && module != "mis" && module != "admin") ) return 2;
  else if(app == "RDX" && module != "mis" && module != "instaMIS" && module != "quota") return 3;
  else if(module == "mis" || module == "MIS" || module == "instaMIS" || module == "Mis") return 4;
  else if( (app == "MNR" && (module != "homePage" || module != "productSettings") && module != "user") || (app == "EAPPS" && module == "admin") || (app == "RDX" && module == "quota") ) return 5;
  else return 0;
}

function mnHide(obj) {
  try {
    var getmnLI=obj.getElementsByTagName('a');
    for(var i=0; i<getmnLI.length; i++) {
      if(getmnLI[i].rel) {
        gbi(getmnLI[i].rel).style.display='none';
        getmnLI[i].className='dropD';
        gbi('iframeId').style.display='none';
        objId='';
      }
    }
  }catch(e){ }
}

var objId='';
function mNShow(obj) {
  try {
    var mOb=gbi('mainNav');
    if(!gbi('iframeId'))
      mOb.innerHTML+='<iframe class="iframeBox" id="iframeId"></iframe>';

    if(obj.nodeName=='BODY') {
      mnHide(mOb);
    }

    var getmnLI=mOb.getElementsByTagName('a');
    var ob=obj.parentNode;

    if(ob.nodeName != 'BODY' && ob.parentNode.parentNode.parentNode)
      var varObj=obj.id || ob.id || ob.parentNode.id || ob.parentNode.parentNode.id || ob.parentNode.parentNode.parentNode.id;
    else
      var varObj=obj.id || ob.id || ob.parentNode.id || ob.parentNode.parentNode.id;
    if(!(varObj==objId))
      mnHide(mOb);

    leftPos=findPosX(obj);
    leftTop=findPosY(obj) -1;

    var iOb=gbi('iframeId');
    for(var i=0; i<getmnLI.length; i++) {
      if((getmnLI[i].className=='dropD') && obj.rel && (obj.innerHTML == getmnLI[i].innerHTML)) {
        iOb.style.border=0;
        gbi(obj.rel).style.left=iOb.style.left=leftPos+'px';
        gbi(obj.rel).style.top=iOb.style.top=leftTop+29+'px';
        getmnLI[i].className='selul';
        gbi(obj.rel).style.display='block';
        iOb.style.display='block';
        iOb.style.width=gbi(obj.rel).offsetWidth+'px';
        iOb.style.height=gbi(obj.rel).offsetHeight+'px';
        objId=obj.rel;
        break;
      }
    }
  }catch(e) { }
}

document.onmouseover=function(e) {
  var element = (navigator.appName == 'Microsoft Internet Explorer') ? window.event.srcElement : e.target;
  mNShow(element);
}

function findPosX(obj) {
  var curleft = 0;
  if(obj.offsetParent) {
    while(1) { 
      curleft += obj.offsetLeft;
      if(!obj.offsetParent)
        break;
      obj = obj.offsetParent;
    }
  }
  else if(obj.x) {
    curleft += obj.x;
  }
  return curleft;
}

function findPosY(obj)
{
  var curtop = 0;
  if(obj.offsetParent) {
    while(1) {
      curtop += obj.offsetTop;
      if(!obj.offsetParent)
        break;
      obj = obj.offsetParent;
    }
  }
  else if(obj.y) {
    curtop += obj.y;
  }
  return curtop;
}

function gbi(layerid) {
  return document.getElementById(layerid);
}

function showFooter(id) {
  var d = new Date();
  var str = "<div class='cls'><img src='" + Images_Path + "/spacer.gif' height='1' width='1' /><div id='footer'> <a href='http://corp.naukri.com/mynaukri/mn_newsmartsearch.php?searchtype=bpwjobs_page&phrase=INFONAUK&tem=naukinner&search=Search' target='_blank'>Careers</a> - <a href='http://corp.naukri.com/mynaukri/mn_aboutus.php' target='_blank'>About Us</a> - <a href='http://corp.naukri.com/mynaukri/staticpages.php?displaypage=ourclients' target='_blank'>Clients</a> - <a href='http://corp.naukri.com/mynaukri/mn_termsconditions.php' target='_blank'>Terms &amp; Conditions</a> - <a href='http://corp.naukri.com/mynaukri/mn_faqs.php'target='_blank'>FAQ's</a> - <a href='http://www.naukri.com/mynaukri/mn_contactus.php' target='_blank'>Contact Us</a> - <a href='http://w5.naukri.com/fdbck/main/feedback.php?app_id=10' target='_blank'>Report a Problem</a> - <a href='http://www.naukri.com/sitemap/sitemap.htm' target='_blank'>Site Map</a> - <a href='http://corp.naukri.com/mynaukri/mn_resources.php' target='_blank'>Resources</a><br />Our Partners: <a href='http://www.jeevansathi.com' target='_blank'>Jeevansathi Matrimonials</a> - <a href='http://www.icicicommunities.org' target='_blank'>ICICIcommunities.org</a> - <a href='http://www.99acres.com' target='_blank'>99acres</a> - <a href='http://www.99acres.com' target='_blank'>Real Estate In India</a><br />All rights reserved <strong>&copy;</strong> " + d.getFullYear() + " Info Edge India Ltd.</div><div id='lastdiv'><img src='" + Images_Path + "/spacer.gif' alt='' /></div>";
  if(id) gbi(id).innerHTML = str; else document.write(str);
}



var browser_1=navigator.userAgent;
var browser_Name={'MSIE':'MSIE', 'Chrome':'Chrome', 'Firefox':'Firefox', 'Opera':'Opera', 'Safari':'Version'};
var browser_Version;
var browser_Name1;
function fetchBrowser_Name(){
  for(x in browser_Name){
    if((browser_1.indexOf(x))!=-1){
      if(x=='MSIE'){
        browser_1=browser_1.split('MSIE');
        browser1=browser_1[1].split(';')
          browser_Name1=x;
        browser_Version=browser1[0]
          break;
      }
      else{
        browser_1=browser_1.split(browser_Name[x]+'/');
        browser1=browser_1[1].split(' ');
        browserName1=x;
        browser_Version=browser1[0];
        break;
      }
    }
  }
  if(!browser_Name1) {
    browser_Name1='Other';
    browser_Version='Other';
  }
}
fetchBrowser_Name();


feedback_h_1 = (typeof(window.innerHeight) != 'undefined' ? window.innerHeight : document.documentElement.clientHeight);
var scrOfX = 0, scrOfY = 0;
function getSurveyScrollXY() {
  scrOfX = 0, scrOfY = 0;
  feedback_h_1 = (typeof(window.innerHeight) != 'undefined' ? window.innerHeight : document.documentElement.clientHeight);
  if( typeof( window.pageYOffset ) == 'number' ) {
    //Netscape compliant
    scrOfY = window.pageYOffset;
    scrOfX = window.pageXOffset;
  } else if( document.body && ( document.body.scrollLeft || document.body.scrollTop ) ) {
    //DOM compliant
    scrOfY = document.body.scrollTop;
    scrOfX = document.body.scrollLeft;
  } else if( document.documentElement && ( document.documentElement.scrollLeft || document.documentElement.scrollTop ) ) {
    //IE6 standards compliant mode
    scrOfY = document.documentElement.scrollTop;
    scrOfX = document.documentElement.scrollLeft;
  }
  //return [ scrOfX, scrOfY ];
}
function setSurveyFeedback() {
  getSurveyScrollXY();
  if(gbi('recruiterSurvey'))
    gbi('recruiterSurvey').style.top=scrOfY+(feedback_h_1/2)-60+"px";
}
if(browser_Version<=6 && browser_Name1=='MSIE') {
    window.onscroll=setSurveyFeedback;
    window.onresize=setSurveyFeedback;
}
function gbi(layid){
  return document.getElementById(layid);
}

function showFeedbackLink(link) {
    var str = "<div class='recruiterSurvey' id='recruiterSurvey'><a href='" + link + "' target=_blank>&nbsp;</a></div>";
    document.write(str);    
    if(browser_Version<=6 && browser_Name1=='MSIE') {
        gbi('recruiterSurvey').style.position='absolute';
    }
    setSurveyFeedback();
}

