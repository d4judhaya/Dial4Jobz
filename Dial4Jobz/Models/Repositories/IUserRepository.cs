using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dial4Jobz.Models.Repositories
{
    public interface IUserRepository
    {
        void Add(User user);
        User GetByUserName(string userName);
        void Save();
    }
}