using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Dial4Jobz.Models.Repositories;


namespace Dial4Jobz.Models
{
    [MetadataType(typeof(VasPlan_Validation))]
    public partial class VasPlan
    {
        Repository _repository = new Repository();
        //public string DisplayPlan
        //{
        //    //get
        //    //{
        //    //    //if (string.IsNullOrEmpty(Plan))
        //    //    //    return "No Plan";
        //    //    //else
        //    //    //    return Plan;
        //    //}
        //}

        public class VasPlan_Validation
        {
            [Required(ErrorMessage = "Plan is required")]
            public string plan { get; set; }
        }
    }
}