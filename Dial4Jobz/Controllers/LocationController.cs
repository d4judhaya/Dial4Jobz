using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dial4Jobz.Models.Repositories;

namespace Dial4Jobz.Controllers
{
    public class LocationController : Controller
    {
        Repository _repository = new Repository();   

        public ActionResult States(string country)
        {
            return Json(new SelectList(_repository.GetStates(Convert.ToInt32(country)), "Id", "Name"));
        }

        public ActionResult Cities(string state)
        {
            return Json(new SelectList(_repository.GetCities(Convert.ToInt32(state)), "Id", "Name"));
        }

        public ActionResult Regions(string city)
        {
            return Json(new SelectList(_repository.GetRegions(Convert.ToInt32(city)), "Id", "Name"));
        }

    }
}
