using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dial4Jobz.Models
{
    public sealed class ContactCandidates
    {
        public string EmployerEmail { get; set; }
        public string Subject { get; set; }
        public string MinExperience { get; set; }
        public string MaxExperience { get; set; }
        public string MinAnnualSalaryLakhs { get; set; }
        public string MaxAnnualSalaryLakhs { get; set; }
        public string OtherSalary { get; set; }
        public string JobLocation { get; set; }
        public string Message { get; set; }
        public string SaveMailTemplate { get; set; }
        public string SessionMailTemplate { get; set; }
        public string SendApplyButton { get; set; }

        public string Title { get; set; }
        public string SmsVacancy { get; set; }
        public string SmsLocation { get; set; }
        public string SmsCompanyName { get; set; }
        public string SmsMobileNo { get; set; }
        public string SmsEmailId { get; set; }
        public string SmsContactPerson { get; set; }
        public string SmsDirectPosition { get; set; }

        public string InterviewDate { get; set; }
        public string InterviewLocation { get; set; }
        public string InterviewCompany { get; set; }
        public string InterviewContactPerson { get; set; }
        public string InterviewContactNo { get; set; }
        public string InterviewPosition { get; set; }
        public string InterviewJobSalary { get; set; }
        public string InterviewJobLocation { get; set; }
        public string InterviewJobAddress { get; set; }
        
    }
}