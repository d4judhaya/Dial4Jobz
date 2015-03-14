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
    public class SkillsController : Controller
    {
        Repository _repository = new Repository();

        public string Get(string q)
        {
            var skills = new List<JsonTokenizerResult>();
            foreach (Skill skill in _repository.GetSkills(q))
            {
                skills.Add(new JsonTokenizerResult()
                {
                    id = skill.Id.ToString(),
                    name = skill.Name
                });
            }

            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(skills);
        }

    }
}
