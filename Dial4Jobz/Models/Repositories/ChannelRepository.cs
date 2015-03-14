using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dial4Jobz.Models.Enums;
using System.Data.Objects;
using System.Collections;
using Dial4Jobz.Helpers;

namespace Dial4Jobz.Models.Repositories
{
    public class ChannelRepository : IChannelRepository
    {
        private Dial4JobzEntities _db = new Dial4JobzEntities();

        public void Save()
        {
            _db.SaveChanges();
        }

        #region channel entry

        public void AddChannelEntry(ChannelEntry channelentry)
        {
            _db.ChannelEntries.AddObject(channelentry);
        }

        #endregion channel entry

        #region manage channel partner

        public IQueryable<ChannelPartner> GetChannelPartners()
        {
            return _db.ChannelPartners;
        }

        public IQueryable<ChannelPartner> GetChannelPartnersWithEntries()
        {
            return _db.ChannelPartners.Include("ChannelEntries"); 
        }

        public IEnumerable<ChannelPartner> GetChannelPartnersbyUserName(string UserName)
        {
            return _db.ChannelPartners.Where(u => u.UserName.ToLower() == UserName.ToLower());
        }

        public IEnumerable<ChannelPartner> GetChannelPartnersbyEmail(string Email)
        {
            return _db.ChannelPartners.Where(u => u.Email.ToLower() == Email.ToLower());
        }

        public IQueryable<ChannelPartner> GetChannelPartnersByMobileNumber(string contactnumber)
        {
            return _db.ChannelPartners.Where(u => u.ContactNo == contactnumber);
        }

        public void AddChannelPartner(ChannelPartner channelpartner)
        {
            _db.ChannelPartners.AddObject(channelpartner);
        }

        public ChannelPartner GetChannelPartner(int id)
        {
            return _db.ChannelPartners.Where(ch => ch.Id == id).FirstOrDefault();
        }

        public void DeleteChannelPartner(int id)
        {
            var channelpartner = _db.ChannelPartners.Where(u => u.Id == id).SingleOrDefault();

            _db.ChannelPartners.DeleteObject(channelpartner);

            _db.SaveChanges();
        }

        #endregion manage channel partner

        #region manage channel user

        public IQueryable<ChannelUser> GetChannelUsers()
        {
            return _db.ChannelUsers;
        }

        public IQueryable<ChannelUser> GetChannelUsersbyPartner(int ChannelPartnerId)
        {
            return _db.ChannelUsers.Where(cu => cu.ChannelPartnerId == ChannelPartnerId);
        }

        public IQueryable<ChannelUser> GetChannelUsersWithEntries()
        {
            return _db.ChannelUsers.Include("ChannelEntries");
        }

        public IQueryable<ChannelUser> GetChannelUsersbyPartnerWithEntries(int ChannelPartnerId)
        {
            return _db.ChannelUsers.Include("ChannelEntries").Where(cu => cu.ChannelPartnerId == ChannelPartnerId);
        }

        public IEnumerable<ChannelUser> GetChannelUsersbyUserName(string UserName)
        {
            return _db.ChannelUsers.Where(u => u.UserName.ToLower() == UserName.ToLower());
        }

        public IEnumerable<ChannelUser> GetChannelUsersbyEmail(string Email)
        {
            return _db.ChannelUsers.Where(u => u.Email.ToLower() == Email.ToLower());
        }

        public IQueryable<ChannelUser> GetChannelUsersByMobileNumber(string contactnumber)
        {
            return _db.ChannelUsers.Where(u => u.ContactNo == contactnumber);
        }

        public void AddChannelUser(ChannelUser channeluser)
        {
            _db.ChannelUsers.AddObject(channeluser);
        }

        public ChannelUser GetChannelUser(int id)
        {
            return _db.ChannelUsers.Where(ch => ch.Id == id).FirstOrDefault();
        }

        public void DeleteChannelUser(int id)
        {
            var channeluser = _db.ChannelUsers.Where(u => u.Id == id).SingleOrDefault();

            _db.ChannelUsers.DeleteObject(channeluser);

            _db.SaveChanges();
        }

        #endregion manage channel user

        #region channel login

        //public bool CheckValidChannelPartner(string email, string password)
        //{
        //    return _db.ChannelPartners.Where(c => c.Email == email && SecurityHelper.GetMD5String(c.Password) == SecurityHelper.GetMD5String(SecurityHelper.GetMD5Bytes(password))).Count() > 0;
        //}

        //public bool CheckValidChannelUser(string email, string password)
        //{
        //    return _db.ChannelUsers.Where(c => c.Email == email && SecurityHelper.GetMD5String(c.Password) == SecurityHelper.GetMD5String(SecurityHelper.GetMD5Bytes(password))).Count() > 0;
        //}

        public ChannelPartner GetValidChannelPartner(string email, string password)
        {
            ChannelPartner channelpartner = _db.ChannelPartners.Where(c => c.Email.ToLower() == email.ToLower()).FirstOrDefault();

            if (channelpartner == null)
                return null;

            if (channelpartner != null)
            {
                if (SecurityHelper.GetMD5String(SecurityHelper.GetMD5Bytes(password)) == SecurityHelper.GetMD5String(channelpartner.Password))
                    return channelpartner;
            }

            return null;
        }

        public ChannelUser GetValidChannelUser(string email, string password)
        {
            ChannelUser channeluser = _db.ChannelUsers.Where(c => c.Email.ToLower() == email.ToLower()).FirstOrDefault();

            if (channeluser == null)
                return null;

            if (channeluser != null)
            {
                if (SecurityHelper.GetMD5String(SecurityHelper.GetMD5Bytes(password)) == SecurityHelper.GetMD5String(channeluser.Password))
                    return channeluser;
            }

            return null;
        }

        #endregion       
        
    }


}