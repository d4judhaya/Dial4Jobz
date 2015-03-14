using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Dial4Jobz.Models.Repositories;

namespace Dial4Jobz.Models
{
    [MetadataType(typeof(Job_Validation))]

    public partial class Job
    {
        Repository _repository = new Repository();
        
        public string DisplayPosition
        {
            get
            {
                if (string.IsNullOrEmpty(Position))
               
                    return "No Position";
                else
                    return Position;
            }
        }

        public Function GetFunction(int functionId)
        {
            return _repository.GetFunction(functionId);
        }

        public EmployeesCount GetEmployeesCount(int id)
        {
            return _repository.GetEmployeesCount(id);
        }

        public Specialization GetSpecialization(int specializationId)
        {
            return _repository.GetSpecialization(specializationId);
        }
    }

    public class Job_Validation
    {
     
        //[Required(ErrorMessage = "Organization is required")]
        //[Range(1, int.MaxValue, ErrorMessage = "Organization is required")]
        //public int OrganizationId { get; set; }
       
        [Required(ErrorMessage = "Function Required")]
        public int FunctionId { get; set; }

        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\-+)|([A-Za-z0-9]+\.+)|([A-Za-z0-9]+\++))*[A-Za-z0-9]+@((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6}$", ErrorMessage = "Invalid Email Address")]
        public string EmailAddress { get; set; }

       
        //[Required(ErrorMessage = "Contact Person Required")]
        //public string ContactPerson { get; set; }

        //[Required(ErrorMessage = "Mobile Number is Required")]
        //public string MobileNumber { get; set; }
                
        //[Required(ErrorMessage = "Email Address is Required")]
        //public string EmailAddress { get; set; }
        
        //[Required(ErrorMessage = "Position is Required")]
        //public string Position { get; set; }
        
    }
}