var totalPrice=0;
var totalServiceTax=0;
var totalPriceForProducts = 0;
var indiaCode=66;
var arrSTD = Array();
function toggleFocusText(ele, action, defVal)
{
    if(action == "blur") 
    {
        ele.value = ele.value.replace(/^\s+/,'');
        ele.value = ele.value.replace(/\s+$/,'');
        if(ele.value=="") 
        {
            ele.value=defVal;
            ele.style.color="#8d8d8d";
        }
    }
    else if(action == "focus" && ele.value == defVal) 
    {
        ele.value="";
        ele.style.color = "#444";
    }
}

function validateCallBack()
{
    document.getElementById("rcb_contactPerson").style.display="none";
    document.getElementById("rcb_company").style.display="none";
    document.getElementById("rcb_email").style.display="none";
    document.getElementById("rcb_phone").style.display="none";
    document.getElementById("rcb_location").style.display="none";
    document.getElementById("rcb_usertype").style.display="none";
    document.getElementById("rcb_response").style.display="none";
    document.reqCallBack.contactPerson.className = document.reqCallBack.contactPerson.className.replace(" redBdr","");
    document.reqCallBack.company.className = document.reqCallBack.company.className.replace(" redBdr","");
    document.reqCallBack.email.className = document.reqCallBack.email.className.replace(" redBdr","");
    document.reqCallBack.phone.className = document.reqCallBack.phone.className.replace(" redBdr","");
    document.reqCallBack.country.className = document.reqCallBack.country.className.replace(" redBdr","");
    document.reqCallBack.state.className = document.reqCallBack.state.className.replace(" redBdr","");
    document.reqCallBack.city.className = document.reqCallBack.city.className.replace(" redBdr","");
    document.reqCallBack.response.className = document.reqCallBack.response.className.replace(" redBdr","");
    var contactPerson = trim(document.reqCallBack.contactPerson.value);
    var company = trim(document.reqCallBack.company.value);
    var email = trim(document.reqCallBack.email.value);
    var phone = trim(document.reqCallBack.phone.value);
    var country = trim(document.reqCallBack.country.value);
    var state = trim(document.reqCallBack.state.value);
    var city = trim(document.reqCallBack.city.value);
    var response = trim(document.reqCallBack.response.value);
    var errLocation="";
    var usertype = "";
    var i=0;
    for(i=0;i < document.reqCallBack.userType.length; i++)
    {
      if(document.reqCallBack.userType[i].checked)
      {
        usertype = document.reqCallBack.userType[i].value;
        break;
      }
    }
    var returnFlag=true;
    if(usertype == "")
    {
      document.getElementById("rcb_usertype").style.display="block";
      document.getElementById("rcb_usertype").innerHTML = "Please specify if you are a jobseeker or a recruiter";
      returnFlag= false;
    }
    if(contactPerson == "" || contactPerson == "Contact Person")
    {
        document.getElementById("rcb_contactPerson").style.display="block";
        document.getElementById("rcb_contactPerson").innerHTML = "Please enter your name";
        document.reqCallBack.contactPerson.className = document.reqCallBack.contactPerson.className + " redBdr";
        returnFlag= false;
    }
    if(company == "" || company == "Company")
    {
        document.getElementById("rcb_company").style.display="block";
        document.getElementById("rcb_company").innerHTML = "Please enter your company name";
        document.reqCallBack.company.className = document.reqCallBack.company.className + " redBdr";
        returnFlag= false;
    }
    if(email == "" || email == "Email")
    {
        document.getElementById("rcb_email").style.display="block";
        document.getElementById("rcb_email").innerHTML = "Please enter your email";
        document.reqCallBack.email.className = document.reqCallBack.email.className + " redBdr";
        returnFlag= false;
    }
    else if(!validateEmailAddr(email))
    {
        document.getElementById("rcb_email").style.display="block";
        document.getElementById("rcb_email").innerHTML = "Please enter a valid email";
        document.reqCallBack.email.className = document.reqCallBack.email.className + " redBdr";
        returnFlag= false;
    }
    if(phone == "" || phone == "Phone")
    {
        document.getElementById("rcb_phone").style.display="block";
        document.getElementById("rcb_phone").innerHTML = "Please enter your contact number";
        document.reqCallBack.phone.className = document.reqCallBack.phone.className + " redBdr";
        returnFlag= false;
    }
    else if(!validateTelephone(phone))
    {
        document.getElementById("rcb_phone").style.display="block";
        document.getElementById("rcb_phone").innerHTML = "Please enter a valid number";
        document.reqCallBack.phone.className = document.reqCallBack.phone.className + " redBdr";
        returnFlag= false;
    }
    if(city == "" || city == "City")
    {
        errLocation = errLocation + "City, ";
        document.reqCallBack.city.className = document.reqCallBack.city.className + " redBdr";
    }
    if(state == "" || state == "State")
    {
        errLocation = errLocation + "State, ";
        document.reqCallBack.state.className = document.reqCallBack.state.className + " redBdr";
    }
    if(country == "" || country == "Country")
    {
        errLocation = errLocation + "Country, ";
        document.reqCallBack.country.className = document.reqCallBack.country.className + " redBdr";
    }
   if(response == "" )
    {
        document.reqCallBack.response.className = document.reqCallBack.response.className + " redBdr";
        document.getElementById("rcb_response").style.display="block";
        //document.getElementById("rcb_response").innerHTML = "Please enter value in image";
        returnFlag= false;
    }

    if(errLocation != '')
    {
        document.getElementById("rcb_location").style.display="block";
        document.getElementById("rcb_location").innerHTML = "Please enter your " + errLocation.substring(0,errLocation.length-2);
        returnFlag= false;
    }


    if(returnFlag == false)
        return returnFlag;

    var response = document.getElementById("response").value;
    sendCallBackRequest(contactPerson,company,email,phone,country,state,city,usertype,response);
    return false;
}

function trim(stringToTrim) 
{ //for removing first and last white space of the string
    if(stringToTrim)
      return stringToTrim.replace(/^\s+|\s+$/g,"");
    else
      return stringToTrim;
}

function validateEmailAddr(email)
{
    var emailPattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
    return emailPattern.test(email);
}

function validateTelephone(num)
{
    var expression = /^[0-9\-]*$/;
    if(expression.test(num))
    {
        num = num.replace(/-/g,"");
        if(num.length < 10)
            return false;
        else
            return true;
    }
        return false;
}

function processBuyNow(prodId,catId)
{
    var cartProducts = document.getElementById('cartProducts').value;
    if(cartProducts)
    {
        document.getElementById('buyNowProdId').value=prodId;
        document.getElementById('buyNowCatId').value=catId;
        document.getElementById('buyNowProdName').innerHTML = getProdName(catId,prodId);
        modalwin('400','showBuyNowConf',document.getElementById('cartProducts') );
    }
    else
    {
        emptyCart(); 
        addToCart(prodId,catId); 
        document.shoppingCart.submit();
    }
}
function addToCart(prodId, catId)
{
    var qty="";
    if(document.getElementById('qty'+prodId))
    {
        qty = document.getElementById('qty'+prodId).options[document.getElementById('qty'+prodId).selectedIndex].value;
    }
    var cartProducts = document.getElementById('cartProducts').value;
    if (catId == '100') 
    {
        if (cartProducts)
        {
            document.getElementById('lbCatName').innerHTML = document.getElementById('buyCat'+catId).innerHTML;
            document.getElementById('tempCart').value=catId+":"+prodId+":"+qty;
            modalwin('400', 'addCartInfo1',document.getElementById('cartProducts') );
            return false;
        }
    }
    if(cartProducts)
    {
        if(catAlreadyInCart(catId,cartProducts))
        {
            document.getElementById('lbCatName').innerHTML = document.getElementById('buyCat'+catId).innerHTML;
            document.getElementById('tempCart').value=catId+":"+prodId+":"+qty;
            modalwin('400', 'addCartInfo',document.getElementById('cartProducts') );
            return false;
        }
        else
        {
            cartProducts = cartProducts + ","+catId+":"+prodId+":"+qty;
        }
    }
    else
    {
        cartProducts = catId+":"+prodId+":"+qty;
    }
    createCartElements(cartProducts);
    document.getElementById('cartProducts').value=cartProducts;
    return true;
}
function replaceComboCart()
{
  var tempCart = document.getElementById('tempCart').value;
  var arr = tempCart.split(":");
  var prodId = arr[1];
  var catId = arr[0];
  emptyCart();
  addToCart(prodId, catId);
}
function replaceCart()
{
  var tempCart = document.getElementById('tempCart').value;
  if(tempCart)
  {
    var arrTempCart = tempCart.split(":");
    var prodToRemove = getExistingProductFromCategory(arrTempCart[0]);
    if(prodToRemove)
    {
      removeFromCart(prodToRemove);
      addToCart(arrTempCart[1],arrTempCart[0]);
    }
  }
}

function getExistingProductFromCategory(catId)
{
  var cartProducts = document.getElementById('cartProducts').value;
  return catAlreadyInCart(catId,cartProducts);
}

function catAlreadyInCart(catId,cartProducts)
{
    var arrCartProducts = cartProducts.split(",");
    var arrProductDetails;
    var i=0;
    for(i=0;i<arrCartProducts.length;i++)
    {
        arrProductDetails = arrCartProducts[i].split(":");
        if(arrProductDetails[0] == catId)
        {
            return arrProductDetails[1];
        }
        if (arrProductDetails[0] == '100') 
        {
            return arrProductDetails[1];
        }
    }
    return false;
}

function createCartElements(cartProducts)
{
    emptyCart();
    if(cartProducts != "")
    {
        var arrCartProducts = cartProducts.split(",");
        var arrProductDetails;
        var finalHTML = "<table border='0' cellspacing='0' cellpadding='0'>";
        var prodHTML = "";
        var totalPrice = 0;
        var tempPrice=0;
        var i=0;
        for(i=0;i<arrCartProducts.length;i++)
        {
            arrProductDetails = arrCartProducts[i].split(":");
            if(arrProductDetails[2] == "")
                arrProductDetails[2] = 1;
            prodHTML = prodHTML + createHTMLForProduct(arrProductDetails[0],arrProductDetails[1],arrProductDetails[2]);
            highlightTupple(arrProductDetails[1]);
        }
        prodHTML = prodHTML + getTotalPriceHTML();
        finalHTML = finalHTML + prodHTML + getCartFooter();
        document.getElementById("cartCont").innerHTML=finalHTML;
    }
    else
    {
        document.getElementById("cartCont").innerHTML="Your cart is empty";
    }
}

function highlightTupple(prodId)
{
  var arrElement = document.getElementById('prc'+prodId).parentNode.id;
  var newElement='';
  var newElementIndex=10;
  var i=0;
  if(arrElement.indexOf('r') >=0)
    newElement="j";
  else
    newElement="r";
  newElementIndex = arrElement.substring(2,3);
  while(i<newElementIndex)
  {
    if(document.getElementById(newElement+"1"+newElementIndex))
      break;
    else
      newElementIndex--;
  }
  changeTuppleColor(newElement,newElementIndex,'#fafafa');
}

function changeTuppleColor(newElement,newElementIndex,color)
{
  var i=1;
  for(i=1;i<6;i++)
  {
    if(document.getElementById(newElement+i+newElementIndex))
    { 
        document.getElementById(newElement+i+newElementIndex).style.background = color;
    }
    else
      break;
  }
}


function emptyCart()
{
    document.getElementById("cartCont").innerHTML="";
    document.getElementById('cartProducts').value = "";
    totalPrice=0;
    totalServiceTax=0;
    totalPriceForProducts=0;//SUMIT
    var i=1;
    for(i=1;i<6;i++)
    {
      changeTuppleColor("r",i,"#fff");
      changeTuppleColor("j",i,"#fff");
    }
}

function createHTMLForProduct(catId,prodId,qty)
{
    var priceWithTax = document.getElementById('prc'+prodId).innerHTML;
    var arrPrice = priceWithTax.split("#");
    var strElem = "<tr><td width='60%' class='lH16'>"+getProdName(catId,prodId);
    if(qty > 1)
        strElem = strElem + "&nbsp;&nbsp;<strong>(x "+qty+")</strong>";
    if(curr == "Rs.")  
      strElem = strElem + "<br /> [ <a href=\"javascript:void(0);\" onclick=\"removeFromCart('"+prodId+"');\" class='font10'>Remove</a> ] </td> <td width='2%'>:</td> <td width='38%'>"+curr+" "+ roundNumber((parseFloat(arrPrice[0]) * parseFloat(qty)),2) +"</td> </tr>";
    else
      strElem = strElem + "<br /> [ <a href=\"javascript:void(0);\" onclick=\"removeFromCart('"+prodId+"');\" class='font10'>Remove</a> ] </td> <td width='2%'>:</td> <td width='38%'>"+curr+" "+ roundNumber(((parseFloat(arrPrice[0]) + parseFloat(arrPrice[1])) * parseFloat(qty)),2) +"</td> </tr>";
      //SUMIT CODE START
    /*totalServiceTax = parseFloat(totalServiceTax) + (parseFloat(arrPrice[1]) * parseFloat(qty));
    totalServiceTax = roundNumber(totalServiceTax,2);
    totalPrice = parseFloat(totalPrice) + ((parseFloat(arrPrice[0]) + parseFloat(arrPrice[1]))*parseFloat(qty));
    totalPrice = roundNumber(totalPrice,2);
    */
    if(curr == "Rs.") {
    totalPriceForProducts = totalPriceForProducts + roundNumber((parseFloat(arrPrice[0]) * parseFloat(qty)),2);
    totalServiceTax = Math.ceil(totalPriceForProducts * (svtax/100));
    totalPrice = totalPriceForProducts + totalServiceTax;
    } else {
    totalPrice = parseFloat(totalPrice) + ((parseFloat(arrPrice[0]) + parseFloat(arrPrice[1]))*parseFloat(qty));
    totalPrice = roundNumber(totalPrice,2);
    }
    //END
    return strElem;
}

function getProdName(catId,prodId)
{
    return document.getElementById('buyProd'+prodId).innerHTML;
}

function getTotalPriceHTML()
{
  var str="";
  if(curr == "Rs.")
     str = "<tr> <td class='noBdr'>Service Tax(12.36%)</td> <td class='noBdr'>:</td> <td class='noBdr'>"+curr+" "+ totalServiceTax +"</td> </tr>";
     str = str + "<tr> <td class='bdrTop txtrt'>Total Price</td> <td class='bdrTop'>:</td> <td class='bdrTop'>"+curr+" "+totalPrice+" </td> </tr>";
    return str;
}

function getCartFooter()
{
    var str = "<tr> <td class='bdrTop txtrt' colspan='3'> <span class='subBor mt4'> <input type='submit' class='submit w138' value='Proceed to Pay &gt;&gt;' name='SubmitPost' /> </span> </td> </tr> </table>";
    return str;
}


function removeFromCart(prodId)
{
    if (prodId == '1699' || prodId == '1697' || prodId == '1695' || prodId == '1693' || prodId == '1691' || prodId == '1689' || prodId == '1687' || prodId == '1685') {
       emptyCart();
       document.getElementById("cartCont").innerHTML="Your cart is empty";
       return;
    }
    var newCart="";
    var cartProducts = document.getElementById('cartProducts').value;
    var arrCartProducts = cartProducts.split(",");
    var arrProductDetails;
    var i=0;
    for(i=0;i<arrCartProducts.length;i++)
    {
        arrProductDetails = arrCartProducts[i].split(":");
        if(arrProductDetails[1] != prodId)
        {
            if(newCart == "")
                newCart=arrCartProducts[i];
            else
                newCart = newCart + ","+arrCartProducts[i];
        }
    }
    createCartElements(newCart);
    document.getElementById('cartProducts').value = newCart;
}

function winOpen(jpgName, winName)
{
  winName="pop";
  myWindow=window.open(jpgName,winName,'width=850,height=600, scrollbars=yes')
}

function openFaq(obj, layerid)
{
  var i=1;
  while(gbi('fq'+i))
  {
    gbi('fq'+i).style.display='none';
    var lia=gbi('fq'+i).parentNode.getElementsByTagName('a');
    lia[0].className='';
    i=i+1;
  }
  gbi(layerid).style.display='block';
  obj.className='sel';
}

function nUser()
{
    document.formPay.action=newUserAction;
    document.formPay.submit();
}

function eUser()
{
    document.getElementById('existingUserLogin').style.display='block';
    document.getElementById('payInfo').style.display='block';
    document.getElementById('submitBtn').value='Login';
    document.formPay.action=existingUserAction;
}


function validateClientRegistrationPage()
{
    document.getElementById('emailErr').style.display="none";   
    document.getElementById('pwdErr').style.display="none";   
    document.getElementById('companynameErr').style.display="none";   
    document.getElementById('industryErr').style.display="none";   
    document.getElementById('addressErr').style.display="none";   
    document.getElementById('countryErr').style.display="none";   
    document.getElementById('cityErr').style.display="none";   
    document.getElementById('stateErr').style.display="none";   
    document.getElementById('pincodeErr').style.display="none";   
    document.getElementById('contactNumberErr').style.display="none";   
    document.getElementById('contactpersonErr').style.display="none";   
    document.getElementById('termsErr').style.display="none";   
    if(document.getElementById("instaPosting").value)
      document.getElementById('pmodeErr').style.display="none";   
    var email = document.getElementById('email').value;
    var pwd = document.getElementById('password').value;
    var pwdCnfrm = document.getElementById('passwordConfirm').value;
    var compName = document.getElementById('companyname').value;
    var industry = document.getElementById('indtype').options[document.getElementById('indtype').selectedIndex].value;
//    var companyType = document.getElementById('clitype').value;
    var contadd1 = document.getElementById('contadd1').value;
    var contadd2 = document.getElementById('contadd2').value;
    var contadd3 = document.getElementById('contadd3').value;
    var country = document.getElementById('country').options[document.getElementById('country').selectedIndex].value;
    if(country == indiaCode)
    {
        var state = document.getElementById('state').options[document.getElementById('state').selectedIndex].value;
        var city = document.getElementById('city').options[document.getElementById('city').selectedIndex].value;
        var pincode = document.getElementById('pincode').options[document.getElementById('pincode').selectedIndex].value;
    }
    else
    {
        var state = 0;
        var city = document.getElementById('ocity').value;
        var pincode = document.getElementById('opincode').value;
    }
    var contactNumber = document.getElementById('contactNumber').value;
    var std = document.getElementById('std').value;
    var isd = document.getElementById('isd').value;
    var contactperson = document.getElementById('contactperson').value;
    var terms = document.getElementById('terms');
    var numErrors=0;

    if(trim(email) == "" || !validateEmailAddr(email))
    {
        if(trim(email) == "")
            showError('emailErr','Email Cannot be blank');
        else
            showError('emailErr','Please enter a valid email');
        numErrors++;
    }

    if(pwd == "" || pwd != pwdCnfrm)
    {
        if(pwd == "")
            showError('pwdErr','Password cannot be blank');
        else
            showError('pwdErr','Password Does not match Confirm Password');
        numErrors++;
    }
    if(compName == "")
    {
        showError('companynameErr','Company Name cannot be blank');
        numErrors++;
    }

    if(industry <=0)
    {
        showError('industryErr','Please select your industry');
        numErrors++;
    }

    if(contadd1 == "")
    {
        showError('addressErr','Address cannot be blank');
        numErrors++;
    }

    if(country <=0)
    {
        showError('countryErr','Please select your country');
        numErrors++;
    }
    if(city == "" || city == "-1")
    {
        showError('cityErr','Please select your City');
        numErrors++;
    }
    if(country == indiaCode && state <=0)
    {
        showError('stateErr','Please select your State');
        numErrors++;
    }

    if(pincode == "" || pincode == "-1")
    {
        showError('pincodeErr','Please select your Pincode/Zipcode');
        numErrors++;
    }

    if(contactNumber == "" || std == "" || isd == "")
    {
        showError('contactNumberErr','Contact Number cannot be blank');
        numErrors++;
    }

    if(trim(contactperson) == "")
    {
        showError('contactpersonErr','Contact Person cannot be blank');
        numErrors++;
    }
    if(terms.checked == false)
    {
        showError('termsErr','Please accept the terms and conditions');
        numErrors++;
    }
    if(document.getElementById("instaPosting").value)
    {
        if(!checkPaymentMode())    
        {
            numErrors++;
        }
    }
    if(numErrors > 0)
    {
       return false;
    }
    document.getElementById("isd").disabled=false;
    document.getElementById("std").disabled=false;

//    return true;
  if(showConfForReg == 1)
  {
    modalwin('400', 'showRegConf',document.getElementById('cartProducts'));
    showConfForReg = 0;
    return false;
  }
  return true;
}

function enableStateDD()
{
    var country = document.getElementById('country').options[document.getElementById('country').selectedIndex].value;
    if(country == indiaCode)
    {
        document.getElementById('state').disabled = false;
        document.getElementById('city').disabled = false;
        document.getElementById('pincode').disabled = false;
        document.getElementById('ocitydiv').style.display="none";
        document.getElementById('opindiv').style.display="none";
    }
    else
    {
        document.getElementById('state').disabled = true;
        document.getElementById('city').disabled = true;
        document.getElementById('pincode').disabled = true;
        if(country != -1)
        {
          document.getElementById('ocitydiv').style.display="block";
          document.getElementById('opindiv').style.display="block";
        }
    }
    if(country > 0)
        document.getElementById('isd').value = arrISD[country];
    else
        document.getElementById('isd').value = "";
    document.getElementById('isd').disabled=true;
}

function emptyCountryDependentFields()
{
    selectSTD('');
    removeOptions(document.getElementById('city'));
    removeOptions(document.getElementById('pincode'));
    document.getElementById('state').selectedIndex=0;
    arrSTD.length=0;
}

function fetchCityDropDown()
{
    try 
    {
        selectSTD('');
        var submitRequest = getXmlHttpObject();
        var objState = document.getElementById('state');
        var stateVal = objState.options[objState.selectedIndex].value;
        removeOptions(document.getElementById('city'));
        removeOptions(document.getElementById('pincode'));
        submitRequest.onreadystatechange = function () 
        {
            if(submitRequest.readyState == 4)
            {
                if(submitRequest.status == 200)
                {
                    if(submitRequest.responseXML)
                    {
                        var responseData = submitRequest.responseXML;
                        var cityNode = responseData.getElementsByTagName('CITIES');
                        var cityCount = cityNode.length;
                        arrSTD.length = 0;
                        for(var ci=0;ci<cityCount;ci++)
                        {
                            addOption(document.getElementById('city'),cityNode[ci].getElementsByTagName('NAME')[0].firstChild.nodeValue,cityNode[ci].getElementsByTagName('ID')[0].firstChild.nodeValue);
                            arrSTD[cityNode[ci].getElementsByTagName('ID')[0].firstChild.nodeValue] = cityNode[ci].getElementsByTagName('STDCODE')[0].firstChild.nodeValue
                        }
                    }
                }
            }
        }
        var queryString = 'stateid='+stateVal+'&id=mm';
        submitRequest.open("GET", fetch_city_url+'?'+queryString, true);
        submitRequest.setRequestHeader("X-Requested-With", "XMLHttpRequest");
        submitRequest.send(null);

    }
    catch(e) {}
}

function fetchPinCodeDropDown()
{
    try
    {
        var submitRequest = getXmlHttpObject();
        var objState = document.getElementById('state');
        var stateVal = objState.options[objState.selectedIndex].value;
        var objCity = document.getElementById('city');
        var cityVal = objCity.options[objCity.selectedIndex].value;
        selectSTD(cityVal);
        removeOptions(document.getElementById('pincode'));        
        submitRequest.onreadystatechange = function ()
        {
            if(submitRequest.readyState == 4)
            {
                if(submitRequest.status == 200)
                {
                    if(submitRequest.responseXML)
                    {
                        var responseData = submitRequest.responseXML;
                        var pinCodeNode = responseData.getElementsByTagName('PINCODES');
                        var pinCodeCount = pinCodeNode.length;
                        for(var ci=0;ci<pinCodeCount;ci++)
                        {
                            addOption(document.getElementById('pincode'),pinCodeNode[ci].getElementsByTagName('LABEL')[0].firstChild.nodeValue,pinCodeNode[ci].getElementsByTagName('ID')[0].firstChild.nodeValue);
                        }

                    }
                }
            }
        }
        var queryString = 'stateid='+stateVal+'&cityid='+cityVal+'&id=mm';
        submitRequest.open("GET", fetch_pincode_url+'?'+queryString, true);
        submitRequest.setRequestHeader("X-Requested-With", "XMLHttpRequest");
        submitRequest.send(null);
    }
    catch(e) {}
}

function selectSTD(city)
{
    if(city && city > 0)
    {
        document.getElementById('std').value = arrSTD[city];
        document.getElementById('std').disabled = true;
    }
    else
    {
        document.getElementById('std').value = "";
        document.getElementById('std').disabled = false;
    }

}

function sendCallBackRequest(contactPerson,company,email,phone,country,state,city,usertype,response)
{
    try
    {
        var submitRequest = getXmlHttpObject();
        submitRequest.onreadystatechange = function ()
        {
            if(submitRequest.readyState == 4)
            {
                if(submitRequest.status == 200)
                {
                    if(submitRequest.responseText)
                    {
                        if(submitRequest.responseText == "SUCCESS")
                        {
                          document.getElementById('captch_image').src = document.getElementById('captch_image').src+'/reload=1';
                          document.getElementById("confirmmess").style.display="block";
                          document.getElementById("errormess").style.display="none";
                          document.getElementById("confirmmess").innerHTML = "<em class=\"cnfrmIcon\"></em> <strong class='rdCol'></strong>Your Request has been sent to our Team.<br /> They will get in touch with you shortly.";
                          document.getElementById("contactPerson").style.color="#8d8d8d";
                          document.getElementById("company").style.color="#8d8d8d";
                          document.getElementById("email").style.color="#8d8d8d";
                          document.getElementById("phone").style.color="#8d8d8d";
                          document.getElementById("country").style.color="#8d8d8d";
                          document.getElementById("state").style.color="#8d8d8d";
                          document.getElementById("city").style.color="#8d8d8d";
                          document.getElementById("contactPerson").value="Contact Person";
                          document.getElementById("company").value="Company";
                          document.getElementById("email").value="Email";
                          document.getElementById("phone").value="Phone";
                          document.getElementById("country").value="Country";
                          document.getElementById("state").value="State";
                          document.getElementById("city").value="City";
                          document.getElementById("response").value="";

                          document.getElementsByName("userType")[0].checked=false;
                          document.getElementsByName("userType")[1].checked=false;
                          window.scrollTo(0,0);
                        }
                        else
                        {
                            document.getElementById('captch_image').src = document.getElementById('captch_image').src+'/reload=1';
                            document.getElementById('response').value="";
                            var arrErrors = submitRequest.responseText.split(",");
                            var i=0;
                            var errorLength = arrErrors.length;
                            for(i=0;i<errorLength;i++)
                            {
                                if(document.getElementById("rcb_"+arrErrors[i]))
                                {
                                    document.getElementById("rcb_"+arrErrors[i]).style.display="block";
                                    if(arrErrors[i] == 'response')
                                        document.reqCallBack.response.className = document.reqCallBack.response.className + " redBdr";
                                    else
                                        document.getElementById("rcb_"+arrErrors[i]).innerHTML = "Please enter a valid value";
                                }
                            }
                        }
                    }
                }
            }
        }
        var queryString = 'contactPerson='+escape(contactPerson)+'&company='+escape(company)+'&email='+escape(email)+'&phone='+escape(phone)+"&country="+escape(country)+"&city="+escape(city)+"&state="+escape(state)+"&userType="+escape(usertype)+"&xhr=1&response="+response;
        submitRequest.open("GET", document.reqCallBack.action+'?'+queryString, true);
        submitRequest.setRequestHeader("X-Requested-With", "XMLHttpRequest");
        submitRequest.send(null);

    }    
    catch(e) {}
}
function getXmlHttpObject()
{
    try{
            // Opera 8.0+, Firefox, Safari
      httpObject = new XMLHttpRequest();
      httpObject.overrideMimeType('text/xml');
        } catch (e){
            // Internet Explorer Browsers
            try{
                httpObject = new ActiveXObject("Msxml2.XMLHTTP");
            } catch (e) {
                try{
                    httpObject = new ActiveXObject("Microsoft.XMLHTTP");
                } catch (e){
                    // Something went wrong
                    alert("XMLHTTP Not Supported.");
                    return false;
                }
            }
        }
    return httpObject;
}

function addOption(selectbox,text,value )
{
    var optn = document.createElement("OPTION");
    optn.text = text;
    optn.value = value;
    selectbox.options.add(optn);
}


function removeOptions(selectbox)
{
    var i;
    for(i=selectbox.options.length-1;i>=0;i--)
    {
        if(selectbox.options[i].value > 0)
            selectbox.remove(i);
    }
}

function showError(divid,errMsg)
{
    document.getElementById(divid).style.display="block";
    document.getElementById(divid).innerHTML = errMsg;
}

function validateRequestInfo()
{
    document.getElementById("firstNameErr").style.display="none";
    document.getElementById("designationErr").style.display="none";
    document.getElementById("emailErr").style.display="none";
    document.getElementById("companyErr").style.display="none";
    document.getElementById("locationErr").style.display="none";
    document.getElementById("phoneErr").style.display="none";
    document.getElementById("requirementErr").style.display="none";
    var firstName = document.getElementById('firstName').value;
    var designation = document.getElementById('designation').value;
    var email = document.getElementById('email').value;
    var company = document.getElementById('company').value;
    var loc = document.getElementById('location').options[document.getElementById('location').selectedIndex].value;
    var countryCode = document.getElementById('countryCode').value;
    var cityCode = document.getElementById('cityCode').value;
    var phone = document.getElementById('phone').value;
    var requirement = document.getElementById('requirement').value;
    var response = document.getElementById('recaptcha_response_field').value;
    var numErrors = 0;
    var phoneError="";
    if(trim(firstName) == "")
    {
        showError('firstNameErr','You have not given your name');
        numErrors++;
    }
    if(trim(designation) == "")
    {
        showError('designationErr','You have not given your designation');
        numErrors++;
    }
    if(trim(email) == "" || !validateEmailAddr(email))
    {
        showError('emailErr','Please specify a valid email id');
        numErrors++;
    }
    if(trim(company) == "")
    {
        showError('companyErr','You have not given your company name');
        numErrors++;
    }
    if(trim(loc) == "" || loc <=0 || loc.indexOf("-") > 0)
    {
        showError('locationErr','Please select a city from the drop down');
        numErrors++;
    }
    if(trim(countryCode) == "" || trim(cityCode) == "" || trim(phone) == "")
    {
        if(trim(countryCode) == "")
            phoneError = phoneError + "You have not given Country Code in The Phone No.<br/>";
        if(trim(cityCode) == "")
            phoneError = phoneError + "You have not given City Code in The Phone No.<br/>";
        if(trim(phone) == "")
            phoneError = phoneError + "You have not given your phone number";
        showError('phoneErr',phoneError);
        numErrors++;
    }
    if(trim(requirement) == "")
    {
        showError('requirementErr','You have not specified your Requirements');
        numErrors++;
    }
    if(trim(response) == "")
    {
        document.getElementById('recaptcha_response_field').className = document.getElementById('recaptcha_response_field').className + " redBdr";
        numErrors++;
    }
    if(numErrors > 0)
        return false;

    return true;
}


function checkPaymentMode()
{
    var payModeICICI = document.getElementById('masterVisaICICI').checked;
    var payModeCC = document.getElementById('CCAvenue').checked;
    var payModeCC1 = document.getElementById('CCAvenue1').checked;
    var payModeChg = document.getElementById('cheque').checked;
    var showUSD = document.getElementById('showUSD').value;
    if(showUSD == "y")
        var payModeAmex = document.getElementById('Amex').checked;
    else
        var payModeAmex = false;
    if(payModeICICI || payModeCC || payModeCC1 || payModeChg || payModeAmex)
        return true;

    showError('pmodeErr','Please select a payment mode');
    return false;
}

function focuschangeNew(layerid)
{
  var objNew=document.getElementById(layerid);

  if(((objNew.parentNode.style.display)=='none') || ((objNew.parentNode.parentNode.style.display)=='none') || ((objNew.parentNode.parentNode.parentNode.style.display)=='none'))
  {
    var inputnew=document.getElementById(layerid1).getElementsByTagName('input');
    inputnew[inputnew.length-1].focus();
  }
  else
  {objNew.focus();}
}

function focuschangeNew1(layerid)
{
  var byTag=document.getElementById(layerid).getElementsByTagName('a');
  byTag[1].focus();
}

function setFieldFocus(e)
{
  var country = document.getElementById('country').options[document.getElementById('country').selectedIndex].value;
  if(country > 0)
  {
    if(window.event.shiftKey && window.event.keyCode == 9)
    {
      return 0;
    }
    else
    {
      if(country == indiaCode)
        document.getElementById('state').focus();
      else
        document.getElementById('ocity').focus();
    }
  }
}

function roundNumber(num, decimals) 
{
  var newnumber = new Number(num+'').toFixed(parseInt(decimals));
  return parseFloat(newnumber);
}



