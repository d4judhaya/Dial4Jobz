using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dial4Jobz.Models.Results
{
    public class JsonActionResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Html { get; set; }
        public string ReturnUrl { get; set; }
    }
}