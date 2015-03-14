using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dial4Jobz.Models.Repositories;
using Dial4Jobz.Models;
using System.Web.Script.Serialization;

namespace Dial4Jobz.Controllers
{
    public class IndustriesController : Controller
    {
        Repository _repository = new Repository();

        public string Get(string q)
        {
            var industries = new List<JsonTokenizerResult>();
            foreach (Industry industry in _repository.GetIndustries(q))
            {
                industries.Add(new JsonTokenizerResult()
                {
                    id = industry.Id.ToString(),
                    name = industry.Name
                });
            }

            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(industries);
        }

    }
}
