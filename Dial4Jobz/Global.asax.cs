using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Dial4Jobz.Models.Constraints;
using Dial4Jobz.Models.Extensions;

namespace Dial4Jobz
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Route to home actions
            routes.MapRouteLowercase(
                "AboutDial4Jobz",
                "About",
                new { controller = "Home", action = "About" }
            );

            routes.MapRouteLowercase(
                "TermsandconditionsofDial4Jobz",
                "Terms",
                new { controller = "Home", action = "Terms" }
            );

            routes.MapRouteLowercase(
                "Privacy",
                "Privacy",
                new { controller = "Home", action = "Privacy" }
            );

            routes.MapRouteLowercase(
                "ContactDial4Jobz",
                "Contact",
                new { controller = "Home", action = "Contact" }
            );

            routes.MapRouteLowercase(
                "Security",
                "Security",
                new { controller = "Home", action = "Security" }
            );

            routes.MapRouteLowercase(
                "SmsTerms",
                "SmsTerms",
                new { controller = "Home", action = "SmsTerms" }
            );

            routes.MapRouteLowercase(
               "Home",
               "Home",
               new { controller = "Home", action = "Home" }
           );

            // route to account actions
            routes.MapRouteLowercase(
                "Login",
                "Login",
                new { controller = "Account", action = "LogOn" }
            );

            routes.MapRouteLowercase(
               "ChangePassword",
               "ChangePassword",
               new { controller = "Account", action = "ChangePassword" }
           );

            routes.MapRouteLowercase(
                "Signup",
                "Signup",
                new { controller = "Account", action = "Signup" }
            );

            routes.MapRouteLowercase(
                  "ForgotPassword",
                  "ForgotPassword",
                  new { controller = "Account", action = "ForgotPassword" }
              );

            routes.MapRouteLowercase(
                "ChangeCandidatePassword",
                "ChangeCandidatePassword",
                new { controller = "Account", action = "ChangeCandidatePassword" }
                );

            routes.MapRouteLowercase(
               "ChangeOrganizationPassword",
               "ChangeOrganizationPassword",
               new { controller = "Account", action = "ChangeOrganizationPassword" }
               );


            //// Route to user profile
            //routes.MapRouteLowercase(
            //    "Users",
            //    "{userName}/{action}",
            //     new { controller = "Profile", action = "Index", userName = "" },
            //     new { userName = new NotNullOrEmptyConstraint() }
            //);


            routes.MapRouteLowercase(
               "MobileVerifyOrganization",
               "employer/phonenoverification",
                new { controller = "Employer", action = "PhoneNoVerification" }
           );

            // Route to user profile
            routes.MapRouteLowercase(
                "AddJob",
                "employer/jobs/add",
                 new { controller = "Jobs", action = "Add" }
            );

            routes.MapRouteLowercase(
              "UpdateEmployerProfile",
              "employer/Profile",
               new { controller = "Employer", action = "Profile" }
          );

            routes.MapRouteLowercase(
                "EmployerPayment",
                "employer/employervas/payment",
                 new { controller = "Employervas", action = "Payment" }
            );

            routes.MapRouteLowercase(
                "downloadresume",
                "candidates/download",
                new { controller = "Candidates", action = "Download" }
                );

        
            routes.MapRouteLowercase(
                  "NetbankingTransfer",
                  "employer/employervas/electronictransfer",
                  new { controller = "EmployerVas", action = "ElectronicTransfer" }
             );

            routes.MapRouteLowercase(
                  "PickupCash",
                  "employer/employervas/callusforpickupcash",
                  new { controller = "EmployerVas", action = "CallUsForPickupCash" }
             );

            routes.MapRouteLowercase(
               "DepositCheque",
               "employer/employervas/depositchequedraft",
               new { controller = "EmployerVas", action = "DepositChequeDraft" }
            );

            routes.MapRouteLowercase(
               "PaythroughPhoneCreditCard",
               "employer/employervas/paythroughphonecreditcard",
               new { controller = "EmployerVas", action = "paythroughphonecreditcard" }
            );

            routes.MapRouteLowercase(
               "",
               "employer/employervas/ccavrequest",
               new { controller = "EmployerVas", action = "CCAVRequest" }
            );

            routes.MapRouteLowercase(
            "",
            "employer/employervas/ccavresponse",
            new { controller = "EmployerVas", action = "CCAVResponse" }
            );

            routes.MapRouteLowercase(
                "candidatedetails",
                "employer/candidates/details/{id}",
                new { controller = "Candidates", action = "Details", id = UrlParameter.Optional }
           );

            routes.MapRouteLowercase(
                "candidatesearch",
                "employer/search/candidatesearch",
                new { controller = "Search", action = "CandidateSearch" }
           );

            routes.MapRouteLowercase(
                "candidateresult",
                "employer/search/candidateresults",
                new { controller = "Search", action = "CandidateResults" }
            );

            routes.MapRouteLowercase(
                "",
                "employer/candidates/Index",
                new { controller = "Candidates", action = "Index" }
                );


            routes.MapRouteLowercase(
               "",
               "employer/employervas/paynow",
                new { controller = "Employervas", action = "PayNow" }
           );

            routes.MapRouteLowercase(
                "AfterJobPost",
                "employer/jobs/AfterJobPost/{id}",
                new { controller = "Jobs", action = "AfterJobPost", id = UrlParameter.Optional }
           );



            routes.MapRouteLowercase(
                "MatchingCandidates",
                "employer/jobs/matchingjobsforcandidate",
                new { controller = "Jobs", action = "MatchingJobsForCandidate", id = UrlParameter.Optional }
                );

            routes.MapRouteLowercase(
              "Find Jobs",
              "jobs/findjobs",
               new { controller = "Jobs", action = "FindJobs" }
          );

            routes.MapRouteLowercase(
                "candidatepaymentstatus",
                "candidates/candidatesvas/candidatespayment",
                new { controller = "CandidatesVas", action = "CandidatesPayment" }
                );


            routes.MapRouteLowercase(
                "Details",
                "jobs/details/{id}",
                new { controller = "Jobs", action = "Details", id = UrlParameter.Optional }
             );

            // Route to user profile
            routes.MapRouteLowercase(
                "EditJob",
                "employer/jobs/edit/{id}",
                 new { controller = "Jobs", action = "Add", id = UrlParameter.Optional }
            );

            routes.MapRouteLowercase(
                "DeletePosition",
                "employer/jobs/delete/{id}",
                new { controller = "Jobs", action = "Delete", id = UrlParameter.Optional }
                );

            routes.MapRouteLowercase(
                "ActivateJob",
                "employer/jobs/activateratvacancy/{id}",
                new { controller = "Jobs", action = "ActivateRATVacancy", id = UrlParameter.Optional }
                );

            routes.MapRouteLowercase(
                "SmsPurchase",
                "employer/smspurchase",
                new { controller = "Employer", action = "SmsPurchase" }
                );


            //Route to EmployerVas

            routes.MapRouteLowercase(
               "AddVas",
               "employer/employervas/index",
                new { controller = "Employervas", action = "Index" }
           );

            routes.MapRouteLowercase(
                "EmployerVasSubscribtion",
                "employer/employervas/Subscribed",
                new { controller = "Employervas", action = "Subscribed" }
                );

            routes.MapRouteLowercase(
                "EmployerComboSubscribe",
                "employer/employervas/SubscribedComboPlans",
                new { controller = "Employervas", action = "SubscribedComboPlans" }
                );

            routes.MapRouteLowercase(
                "EmployerPaymentSuccessfull",
                "employer/employervas/employerpayment",
                new { controller = "EmployerVas", action = "EmployerPayment" }
                );

          

            //Route to Candidatevas
            routes.MapRouteLowercase(
               "AddCandidatesVas",
               "candidates/candidatesvas/index",
                new { controller = "CandidatesVas", action = "Index" }
           );

            routes.MapRouteLowercase(
                "CandidatesDashboard",
                "candidates/dashboard",
                new { controller = "Candidates", action = "DashBoard" }
                );

            routes.MapRouteLowercase(
                "CandidatesSubscriptionBilling",
                "candidates/candidatessubscriptionbilling",
                new { controller = "Candidates", action = "CandidatesSubscriptionBilling" }
                );

            routes.MapRouteLowercase(
                "CandidatesInvoice",
                "candidates/candidatesinvoice",
                new { controller = "Candidates", action = "CandidatesInvoice" }
                );

            routes.MapRouteLowercase(
                 "",
                 "candidates/candidatesvas/payment",
                 new { controller = "CandidatesVas", action = "Payment" }
            );

            routes.MapRouteLowercase(
                "",
                "candidates/candidatesvas/electronictransfer",
                new { controller = "CandidatesVas", action = "ElectronicTransfer" }
           );

            routes.MapRouteLowercase(
                "",
                "admin/adminhome/updatesidetails",
                new { controller = "AdminHome", action = "UpdateSIDetails" }
           );

            routes.MapRouteLowercase(
                "",
                "admin/activationreport/savepaymentdetails",
                new { controller = "ActivationReport", action = "SavePaymentDetails" }
                );  


            routes.MapRouteLowercase(
               "",
               "candidates/candidatesvas/depositchequedraft",
               new { controller = "CandidatesVas", action = "DepositChequeDraft" }
            );

            routes.MapRouteLowercase(
              "",
              "candidates/candidatesvas/callusforpickupcash",
              new { controller = "CandidatesVas", action = "CallUsForPickupCash" }
           );

            routes.MapRouteLowercase(
              "",
              "candidates/candidatesvas/ccavrequest",
              new { controller = "CandidatesVas", action = "CCAVRequest" }
           );

            routes.MapRouteLowercase(
            "",
            "candidates/candidatesvas/ccavresponse",
            new { controller = "CandidatesVas", action = "CCAVResponse" }
            );

            routes.MapRouteLowercase(
                "VasDetails",
                "candidatesvas/details",
                new { controller = "CandidatesVas", action = "Details" }
            );

            routes.MapRouteLowercase(
                "",
                "candidates/candidatesvas/whoisemployer",
                new { controller = "CandidatesVas", action = "WhoIsEmployer" }
            );

            routes.MapRouteLowercase(
                "",
                "candidates/candidatesvas/vasdetails",
                new { controller = "CandidatesVas", action = "VasDetails" }
                );

            routes.MapRouteLowercase(
               "CandidateVasSubscribtion",
               "candidates/candidatesvas/Subscribed",
               new { controller = "CandidatesVas", action = "Subscribed" }
               );


            //Candidates profiles
            routes.MapRouteLowercase(
               "verify again",
               "candidates/secondverify",
               new { controller = "Candidates", action = "SecondVerify" }
               );


            //*****To set default home page***************
            routes.MapRouteLowercase(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );


            routes.MapRouteLowercase(
                "EditCandiate",
                "candidates/edit/{id}",
                new { controller = "Candidates", action = "Save", id = UrlParameter.Optional }
            );


            routes.MapRouteLowercase(
                "SendSMS",
                "jobs/send/{id}",
                new { controller = "Jobs", action = "Send", id = UrlParameter.Optional }
            );

            routes.MapRouteLowercase(
                "ImportData",
                "admin/importdata",
                new { controller = "Admin", action = "ImportData" }
            );

            //add candidate page verification
            routes.MapRouteLowercase(
                "DirectVerification",
                "admin/adminhome/directverification/{id}",
                new { controller = "AdminHome", action = "DirectVerification", id = UrlParameter.Optional }
                );

            //jobmatches page mobile verification
            routes.MapRouteLowercase(
                "Verify",
                "admin/jobmatches/directverification/{id}",
                new { controller = "JobMatches", action = "DirectVerification", id = UrlParameter.Optional }
                );

            routes.MapRouteLowercase(
                "matching candidates",
                "employer/candidatematchesjob/candidatematch/{id}",
                new { controller = "CandidateMatchesJob", action = "CandidateMatch", id = UrlParameter.Optional }
                );

            routes.MapRouteLowercase(
                "deleteJob",
                "jobs/deletejob",
                new { controller = "Jobs", action = "DeleteJob" }
                );

            routes.MapRouteLowercase(
                "JobPostVas",
                "",
                new { controller = "Jobs", action = "JobPostVas" }
                );

            routes.MapRouteLowercase(
              "ccavPayment",
              "employer/ccavpayment/index",
              new { controller = "CCAVPayment", action = "Index" }
           );

            routes.MapRouteLowercase(
                "",
                "jobs/jobsviewedlist",
                new { controller = "Jobs", action = "JobsViewedList" }
                );

            routes.MapRouteLowercase(
                "",
                "candidates/dprviewedlist",
                new { controller = "Candidates", action = "DPRViewedList" }
                );
                 

            routes.MapRouteLowercase(
              "",
              "employer/ccavpayment/ccavrequest",
              new { controller = "CCAVPayment", action = "CCAVRequest" }
         );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterRoutes(RouteTable.Routes);
        }
    }
}