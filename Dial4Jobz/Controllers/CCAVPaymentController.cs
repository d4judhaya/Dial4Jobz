using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCA.Util;

namespace Dial4Jobz.Controllers
{
    public class CCAVPaymentController : BaseController
    {
        public string strAccessCode = "AVZH01BE45AF56HZFA";// put the access key in the quotes provided here.
        public string strEncRequest = "";
        //
        // GET: /CCAVPayment/

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CCAVRequest()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CCAVRequest(FormCollection collection)
        {
            CCACrypto ccaCrypto = new CCACrypto();
            string workingKey = "91BE0EE8FE42847C3CA5E923E8D9752B";//put in the 32bit alpha numeric key in the quotes provided here 	
            string ccaRequest = "";

            foreach (string name in Request.Form)
            {
                if (name != null)
                {
                    if (!name.StartsWith("_"))
                    {
                        ccaRequest = ccaRequest + name + "=" + Request.Form[name] + "&";
                    }
                }
            }

            strEncRequest = ccaCrypto.Encrypt(ccaRequest, workingKey);

            return View();
        }

    }
}
