using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dial4Jobz.Models.Repositories
{
    public interface IChannelRepository 
    {
        void Save();

        IQueryable<ChannelPartner> GetChannelPartners();

        IQueryable<ChannelPartner> GetChannelPartnersWithEntries();

        IEnumerable<ChannelPartner> GetChannelPartnersbyUserName(string UserName);

        IEnumerable<ChannelPartner> GetChannelPartnersbyEmail(string Email);

        void AddChannelPartner(ChannelPartner channelpartner);

        ChannelPartner GetChannelPartner(int id);

        void DeleteChannelPartner(int id);

        IQueryable<ChannelUser> GetChannelUsers();

        IQueryable<ChannelUser> GetChannelUsersbyPartner(int ChannelPartnerId);

        IQueryable<ChannelUser> GetChannelUsersWithEntries();

        IQueryable<ChannelUser> GetChannelUsersbyPartnerWithEntries(int ChannelPartnerId);

        IEnumerable<ChannelUser> GetChannelUsersbyUserName(string UserName);

        IEnumerable<ChannelUser> GetChannelUsersbyEmail(string Email);

        void AddChannelUser(ChannelUser channeluser);

        ChannelUser GetChannelUser(int id);

        void DeleteChannelUser(int id);

        ChannelPartner GetValidChannelPartner(string email, string password);

        ChannelUser GetValidChannelUser(string email, string password);
    }
}
