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
    public class FunctionsController : Controller
    {
        Repository _repository = new Repository();

        public string Get(string q)
        {
            var functions = new List<JsonTokenizerResult>();
            foreach (Function function in _repository.GetFunctions(q))
            {
                functions.Add(new JsonTokenizerResult()
                {
                    id = function.Id.ToString(),
                    name = function.Name
                    
                });
            }

            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(functions);
        }

    }
}
