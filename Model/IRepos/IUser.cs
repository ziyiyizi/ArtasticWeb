using Model.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Model.IRepos
{
    public interface IUser : IRepository<users>
    {
        Task<users> GetByName(string name);
        Task<users> GetById(long userId);
        Task<users> GetByMail(string mail);
        Task<users> GetByNameOrMail(string name, string mail);

        Task<long> GetIdByName(string name);
        Task<string> GetNameById(long userId);
        Task<string> GetNameByWorkId(long artworkId);
        Task<string> GetPwdByName(string name);
        Task<string> GetMailById(long userId);
        Task<string> GetIconById(long userId);
        Task<string> GetStateById(long userId);
        Task<string> GetStateByName(string name);
        Task<string> GetTokenById(long userId);
        Task<DateTime?> GetTokenTimeById(long userId);
        Task<IList> GetIconAndNameById(long userId);

        Task<IEnumerable<users>> GetAllPageByName(string name, PageRequest page);
        Task<IList> GetDetailsByName(string userName, PageRequest page);
        Task<IList> GetAllFollowing(long userId, PageRequest page);
        Task<IList> GetAllFollowed(long userId, PageRequest page);
        Task<long> CountWorks(long userId);
        Task<users> GetByMostWorkBtweenTime(DateTime start, DateTime end);
        Task<int> UpdateIcon(users user);
    }
}
