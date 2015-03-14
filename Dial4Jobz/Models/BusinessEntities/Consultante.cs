using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Dial4Jobz.Models.Repositories;


namespace Dial4Jobz.Models
{
    [MetadataType(typeof(Consultante_Validation))]
    public partial class Consultante
    {
        Repository _repository = new Repository();

        public String DisplayConsultant
        {
            get
            {
                if (string.IsNullOrEmpty(Name))
                    return "No Name";
                else
                    return Name;
            }
        }

    }

    public class Consultante_Validation
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Industry is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Industry is required")]
        public int IndustryId { get; set; }

        [Required(ErrorMessage = "Contact Person is required")]
        public string ContactPerson { get; set; }

        [Required(ErrorMessage = "Address  is required")]
        [StringLength(300, ErrorMessage = "Address cannot exceed 300 characters")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Pincode  is required")]
        public string Pincode { get; set; }
      
        //[Required(ErrorMessage = "Mobile Number is Required")]
        //[Range(1000000000, 9999999999, ErrorMessage = "Invalid Mobile Number, Allowed 10 digit Number only")]
        //public string MobileNumber { get; set; }

        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\-+)|([A-Za-z0-9]+\.+)|([A-Za-z0-9]+\++))*[A-Za-z0-9]+@((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6}$", ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        //[Required(ErrorMessage = "Email Address is required")]
        //public string Email { get; set; }

     }

}