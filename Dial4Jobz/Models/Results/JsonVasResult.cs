using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dial4Jobz.Models.Results
{
    public class JsonVasResult : JsonActionResult
    {
        public List<JsonVas> GetPlans { get; set; }
    }
}