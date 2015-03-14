using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dial4Jobz.Models.Repositories
{
    public interface IVasRepository
    {
        IQueryable<VasPlan> VasPlans();
        IQueryable<VasPlan> VasPlans(string q);

        IQueryable<VasPlan> GetVasPlan();
        IQueryable<VasPlan> GetVasPlan(string q);


      //  IQueryable<VasFE> VasFe();
        //IQueryable<VasFE> VasFe(string q);
        //IQueryable<VasHotResume> VasHot();
        //IQueryable<VasHotResume> VasHot(string q);

        
       // IQueryable<VasFE> GetFes(int planId);
        //IQueryable<VasHotResume> GetHotEmp(int PlanId);


       // VasFE GetVasById(int VasId);
        void Save();

       
    }
}