using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dial4Jobz.Models.Results
{
    public class JsonRolesResult : JsonActionResult
    {
        public List<JsonRole> Roles { get; set; }
        public List<JsonFunction> Functions { get; set; }

    }
}