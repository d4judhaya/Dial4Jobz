using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Dial4Jobz.Helpers;

namespace Dial4Jobz.Models.Filters
{
    public class ReservedWord : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (string.IsNullOrEmpty((string)value))
                return false;

            string userName = ((string)value).ToLower();
            return !StringHelper.IsReservedWord(userName);
        }
    }
}