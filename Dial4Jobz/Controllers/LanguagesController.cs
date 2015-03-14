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
    public class LanguagesController : Controller
    {
        Repository _repository = new Repository();

        public string Get(string q)
        {
            var languages = new List<JsonTokenizerResult>();
            foreach (Language language in _repository.GetLanguages(q))
            {
                languages.Add(new JsonTokenizerResult()
                {
                    id = language.Id.ToString(),
                    name = language.Name
                });
            }

            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(languages);
        }

    }
}
