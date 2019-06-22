using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Model.IRepos;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using Model;
using System.Collections;

namespace DAL.Repos
{
    public class UserRepository : RepositoryBase<users>, IUser
    {
        public UserRepository(ArtasticContext context) : base(context)
        {
        }

        public async Task<long> CountWorks(long userId)
        {
            return await _dbcontext.artworks.Where(e => e.Artist_ID == userId).CountAsync();
        }

        public async Task<IList> GetAllFollowed(long userId, PageRequest page)
        {
            return await (from s in _dbcontext.users join ru in _dbcontext.follow
            on s.User_ID equals ru.Follower_ID
            where ru.Artist_ID == userId
            orderby ru.followtime descending
            select new
            {
                userId = s.User_ID,
                userName = s.User_name,
                userIcon = s.User_icon
            }).Skip(page.PageNumber * page.PageSize).Take(page.PageSize).ToListAsync();
        }

        public async Task<IList> GetAllFollowing(long userId, PageRequest page)
        {
            return await (from s in _dbcontext.users join ru in _dbcontext.follow
            on s.User_ID equals ru.Artist_ID
            where ru.Follower_ID == userId
            orderby ru.followtime descending
            select new
            {
                userId = s.User_ID,
                userName = s.User_name,
                userIcon = s.User_icon
            }).Skip(page.PageNumber * page.PageSize).Take(page.PageSize).ToListAsync();
        }

        public async Task<IEnumerable<users>> GetAllPageByName(string name, PageRequest page)
        {
            if (page == null || name == null)
            {
                return null;
            }

            return await GetPageList(page, e => e.User_name.Contains(name));
        }

        public async Task<users> GetById(long id)
        {
            return await _dbSet.SingleOrDefaultAsync(e => e.User_ID == id);
        }

        public async Task<users> GetByMail(string mail)
        {
            if (mail == null)
            {
                return null;
            }

            return await Get(e => e.User_mail == mail);
        }

        public Task<users> GetByMostWorkBtweenTime(DateTime start, DateTime end)
        {
            throw new NotImplementedException();
        }

        public async Task<users> GetByName(string name)
        {
            if (name == null)
            {
                return null;
            }

            return await Get(e => e.User_name == name);
        }

        public async Task<users> GetByNameOrMail(string name = "", string mail = "")
        {
            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(mail))
            {
                return null;
            }
            return string.IsNullOrEmpty(name) ? await Get(e => e.User_mail == mail) : await Get(e => e.User_name == name);
        }

        public async Task<IList> GetDetailsByName(string userName, PageRequest page)
        {
            return await (from s in _dbSet
                          where s.User_name.Contains(userName)
                          select new
                          {
                              userId = s.User_ID,
                              userName = s.User_name,
                              userIcon = s.User_icon
                          }).ToListAsync();
        }

        public async Task<IList> GetIconAndNameById(long userId)
        {
            return await (from s in _dbSet
             where s.User_ID == userId
             select new
             {
                 icon = s.User_icon,
                 name = s.User_name
             }).ToListAsync();
        }

        public async Task<string> GetIconById(long userId)
        {
            var res = await Get(e => e.User_ID == userId);
            return res == null ? "" : res.User_icon;
        }

        public async Task<long> GetIdByName(string name)
        {
            var res = await Get(e => e.User_name == name);
            return res == null ? -1 : res.User_ID;
        }

        public async Task<string> GetMailById(long userId)
        {
            var res = await Get(e => e.User_ID == userId);
            return res == null ? "" : res.User_mail;
        }

        public async Task<string> GetNameById(long userId)
        {
            var res = await Get(e => e.User_ID == userId);
            return res == null ? "" : res.User_name;
        }

        public async Task<string> GetNameByWorkId(long artworkId)
        {
            var res = await (from u in _dbcontext.artworks join ru in _dbcontext.users
                          on u.Artist_ID equals ru.User_ID
                          where u.Artwork_ID == artworkId
                          select ru.User_name).ToListAsync();
            return res.Count > 0 ? res[0] : null;
        }

        public async Task<string> GetPwdByName(string name)
        {
            if (name == null)
            {
                return null;
            }
            var u = await _dbSet.FirstOrDefaultAsync(e => e.User_name == name);
            return u?.User_password;
        }

        public async Task<string> GetStateById(long userId)
        {
            var res = await Get(e => e.User_ID == userId);
            return res == null ? "" : res.User_state;
        }

        public Task<string> GetStateByName(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetTokenById(long userId)
        {
            var res = await Get(e => e.User_ID == userId);
            return res == null ? "" : res.User_token;
        }

        public async Task<DateTime?> GetTokenTimeById(long userId)
        {
            var res = await Get(e => e.User_ID == userId);
            return res?.token_time;
        }

        public new async Task<int> Update(users user)
        {
            _dbSet.Attach(user);
            _dbcontext.Entry(user).State = EntityState.Unchanged;
            _dbcontext.Entry(user).Property(e => e.User_sex).IsModified = true;
            _dbcontext.Entry(user).Property(e => e.User_description).IsModified = true;
            _dbcontext.Entry(user).Property(e => e.User_password).IsModified = true;
            _dbcontext.Entry(user).Property(e => e.User_mail).IsModified = true;
            return await _dbcontext.SaveChangesAsync();
        }

        public async Task<int> UpdateIcon(users user)
        {
            _dbSet.Attach(user);
            _dbcontext.Entry(user).State = EntityState.Unchanged;
            _dbcontext.Entry(user).Property(e => e.User_icon).IsModified = true;
            return await _dbcontext.SaveChangesAsync();
        }
    }
}
