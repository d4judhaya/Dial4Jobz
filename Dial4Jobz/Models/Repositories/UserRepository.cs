using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dial4Jobz.Models.Repositories
{
    public class UserRepository : IUserRepository
    {
        //*******************gemStore - Angualar JS************************//
        private Dial4JobzEntities _db = new Dial4JobzEntities();

        public void Add(User user)
        {
            if (!_db.Users.Any(u => u.UserName == user.UserName))
                _db.Users.AddObject(user);
        }

        public User GetByUserName(string userName)
        {
            return _db.Users.SingleOrDefault(u => u.UserName == userName);
        }

        //Admin username get
        public IEnumerable<User> GetUsersbyUserName(string userName)
        {
            return _db.Users.Where(u => u.UserName == userName);
        }

        public IEnumerable<AdminUserEntry> GetUsersById(int userId, int entryId,int entryType)
        {
            return _db.AdminUserEntries.Where(u => u.AdminId == userId && u.EntryId==entryId && u.EntryType== entryType);
        }

        public AdminUserEntry GetCreatedBy(int entryId, int entryType)
        {
            return _db.AdminUserEntries.FirstOrDefault(u => u.EntryId == entryId && u.EntryType == entryType);
        }

        public AdminUserEntry GetUserForUpdate(int entryId,int entryType)
        {
            return _db.AdminUserEntries.SingleOrDefault(c=> c.EntryId == entryId && c.EntryType == entryType);
        }

        public User GetByEmail(string email)
        {
            return _db.Users.SingleOrDefault(u => u.Email == email);
        }

        public User GetByMobileNumber(string mobileNumber)
        {
            return _db.Users.SingleOrDefault(u => u.Mobilenumber == mobileNumber);
        }

        public User GetById(int id)
        {
            return _db.Users.SingleOrDefault(u => u.Id == id);
        }

        public void Add(Candidate candidate)
        {
            if (!_db.Candidates.Any(c => c.UserName == candidate.UserName))
                _db.Candidates.AddObject(candidate);
        }

        public void AddConsultant(Consultante consultant)
        {
            if (!_db.Consultantes.Any(c => c.UserName == consultant.UserName))
                _db.Consultantes.AddObject(consultant);
        }

        public Candidate GetCandidateByUserName(string userName)
        {
            return _db.Candidates.SingleOrDefault(c => c.UserName == userName);
        }

        public Candidate GetCandidateByEmail(string email)
        {
            return _db.Candidates.SingleOrDefault(c => c.Email == email);
        }

        public Candidate GetCandidateByMobileNumber(string mobileNumber)
        {
            return _db.Candidates.SingleOrDefault(c => c.ContactNumber == mobileNumber);
        }

        public Candidate GetCandidateByResumeFileName(string resumefilename)
        {
            return _db.Candidates.SingleOrDefault(c => c.ResumeFileName == resumefilename);
        }


        public Candidate GetCandidateById(int id)
        {
            return _db.Candidates.SingleOrDefault(c => c.Id == id);
        }

        public Candidate GetCandidateByConsultantId(int consultantId)
        {
            return _db.Candidates.SingleOrDefault(c => c.ConsultantId == consultantId);
        }


        public void Add(Organization organization)
        {
            if (!_db.Organizations.Any(o => o.UserName == organization.UserName))
                _db.Organizations.AddObject(organization);
        }

        public Organization GetOrganizationByUserName(string userName)
        {
            return _db.Organizations.SingleOrDefault(o => o.UserName == userName);
        }

        public Consultante GetConsultantsByUserName(string userName)
        {
            return _db.Consultantes.SingleOrDefault(c => c.UserName == userName);
        }

        public User GetAdminUsersByUserName(string userName)
        {
            return _db.Users.SingleOrDefault(u => u.UserName == userName);
        }


        public Organization GetOrganizationByEmail(string email)
        {
            return _db.Organizations.SingleOrDefault(o => o.Email == email);
        }

        public Organization GetOrganizationByMobileNumber(string mobile)
        {
            return _db.Organizations.SingleOrDefault(o => o.MobileNumber == mobile);
        }

        public Organization GetOrganizationById(int id)
        {
            return _db.Organizations.SingleOrDefault(o => o.Id == id);
        }

        public Consultante GetConsultantsById(int id)
        {
            return _db.Consultantes.SingleOrDefault(c => c.Id == id);
        }
                
        public Organization GetOrganizationByName(string companyName)
        {
            return _db.Organizations.SingleOrDefault(o => o.Name == companyName);
        }

        public Consultante GetConsultantByName(string Name)
        {
            return _db.Consultantes.SingleOrDefault(o => o.Name == Name);
        }

        public Consultante GetConsultantUserName(string userName)
        {
            return _db.Consultantes.SingleOrDefault(co => co.UserName == userName);
        }

        public Consultante GetConsultantEmail(string email)
        {
            return _db.Consultantes.SingleOrDefault(c => c.Email == email);
        }

        public Consultante GetConsultantMobile(string mobile)
        {
            return _db.Consultantes.SingleOrDefault(c => c.MobileNumber == mobile);
        }

        //Developer note Admin Permission
        public void AddUserPermissions(AdminPermission userPermisssion)
        {
            _db.AdminPermissions.AddObject(userPermisssion);
        }

        public void DeleteUserPermissions(int userId)
        {
            var user = from up in _db.AdminPermissions
                       where up.UserId == userId
                       select up;
            if (user.Count() > 0)
            {
                foreach (var updelete in user)
                {
                    _db.AdminPermissions.DeleteObject(updelete);
                }
                _db.SaveChanges();
            }
        }

        public IEnumerable<Permission> GetAdminPermissions()
        {
            List<Permission> adminPermissions = new List<Permission>();

            var list = (from permissions in _db.Permissions
                        orderby permissions.Name ascending
                        select permissions);

            foreach (var d in list)
                adminPermissions.Add(new Permission() { Id = d.Id, Name = d.Name });

            return adminPermissions;

        }

        public IEnumerable<AdminPermission> GetPermissionsbyUserId(int UserId)
        {
            List<AdminPermission> UserPermissionList = new List<AdminPermission>();

            var Permissions = (from permission in _db.AdminPermissions
                               where permission.UserId == UserId
                               select permission);

            foreach (var Per in Permissions)
                UserPermissionList.Add(new AdminPermission() { PermissionId = Per.PermissionId });

            return UserPermissionList;
        }
        public Permission GetPermissionsNamebyPermissionId(int Id)
        {
            return _db.Permissions.SingleOrDefault(s => s.Id == Id);
        }

        public User GetUserTopId()
        {
            return _db.Users.OrderByDescending(u => u.Id).FirstOrDefault();
        }

        //End of admission Permission

        
        public void Save()
        {
            _db.SaveChanges();
        }

        public void Refresh(User user)
        {
            _db.Refresh(System.Data.Objects.RefreshMode.StoreWins, user);
        }
    }
}