using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Dial4Jobz.Models.Repositories;

namespace Dial4Jobz.Models
{
    [MetadataType(typeof(Candidate_Validation))]
    public partial class Candidate
    {
        Repository _repository = new Repository();

        public String DiplayCandidate
        {
            get
            {
                if (string.IsNullOrEmpty(Name))
                    return "No Name";
                else
                    return Name;
            }
        }

        public Industry GetIndustry(int industryId)
        {
            return _repository.GetIndustry(industryId);
        }

        public Specialization GetSpecialization(int specializationId)
        {
            return _repository.GetSpecialization(specializationId);
        }

        public Location GetLocation(int locationId)
        {
            return _repository.GetLocationById(locationId);
        }

     
        public MaritalStatus GetMaritalStatus(int maritalId)
        {
            return _repository.GetMaritalStatus(maritalId);
        }
              
      
    }

    public class Candidate_Validation
    {
        [Required(ErrorMessage = "Candidate Name is required")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        public DateTime DOB { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public int Gender { get; set; }

         //[Required(ErrorMessage = "Moblie Number Required")]
        //public int MobileNumber { get; set; }

        [Required(ErrorMessage = "Address is Required")]
        public string Address { get; set; }

        //[Required(ErrorMessage = "Mobile Number is Required")]
        //public int ContactNumber { get; set; }

        //[Required(ErrorMessage = "Please provide 10 digit Mobile number")]
        [Range(1000000000, 9999999999, ErrorMessage = "Allowed 10 digit Mobile Number only")]
        public int ContactNumber { get; set; }

        // [Required(ErrorMessage = "Please provide 10 digit Mobile number")]
        [Range(1000000000, 9999999999, ErrorMessage = "Invalid Contact Number, Allowed 10 digit Mobile Number only")]
        public int MobileNumber { get; set; }

        [Required(ErrorMessage = "Position Required")]
        public string Position { get; set; }

       // [Required(ErrorMessage = "Current Function is Required")]
        // public int FunctionId { get; set; }

        [Required(ErrorMessage = "Marital Status is Required")]
        public int MaritalId { get; set; }

       // [Required(ErrorMessage = "Current Industry is Required")]
       // public int IndustryId { get; set; }

        [Required(ErrorMessage = "Pincode is Required")]
        public string Pincode { get; set; }
                
        [StringLength(70, ErrorMessage = "Present Company cannot exceed 70 characters")]
        public string PresentCompany { get; set; }
                
        [StringLength(70, ErrorMessage = "Previous Company cannot exceed 70 characters")]
        public string PreviousCompany { get; set; }
                       
        [StringLength(500, ErrorMessage = "Description should not exceed more than 500 characters")]
        public string Description { get; set; }

        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\-+)|([A-Za-z0-9]+\.+)|([A-Za-z0-9]+\++))*[A-Za-z0-9]+@((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6}$", ErrorMessage = "Invalid Email Address")]
        //[RegularExpression("/^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/", ErrorMessage="Invalid Email Address")]
        public string Email { get; set; }

        //by vignesh

        //[Required(ErrorMessage = "Email-Id is Required")]
        //[DataType(DataType.EmailAddress)]
        //[RegularExpression(@"^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\-+)|([A-Za-z0-9]+\.+)|([A-Za-z0-9]+\++))*[A-Za-z0-9]+@((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6}$", ErrorMessage = "Invalid Email Address")]
        ////[RegularExpression("/^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/", ErrorMessage="Invalid Email Address")]
        //public string Email { get; set; }
        
       
        //[Required(ErrorMessage = "Industry is required")]
        //[Range(1, int.MaxValue, ErrorMessage = "Industry is required")]
        //public int IndustryId { get; set; }

        //[Required(ErrorMessage = "Location is Required")]
        //[Range(1, int.MaxValue, ErrorMessage = "Location is Required")]
        //public int LocationId { get; set; }

        
    }
    
}