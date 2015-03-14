using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Web.Script.Serialization;
using System.Text;

namespace Dial4Jobz.Models
{
    public partial class Location
    {
        public string ToString()
        {
            if (Country.Name.ToLower().Contains("india"))
            {
                if (CityId.HasValue)
                    return City.Name;

                if (StateId.HasValue)
                    return State.Name;

                return Country.Name;
            }
            else
            {
                return Country.Name;
            }
        }
    }
}