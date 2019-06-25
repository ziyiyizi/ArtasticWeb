using BLL.Models;
using DAL;
using Model;
using Model.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore.Storage;
using BLL.Utils;

namespace BLL.Serv
{
    public class UserService
    {
        private UnitOfWork _uw;
        private ArtasticContext _db;

        public UserService(UnitOfWork uw, ArtasticContext db)
        {
            _uw = uw;
            _db = db;
        }

        public async Task<long> CountFollows(long userId)
        {
            return await _uw.FollowRepository.CountFollowed(userId);
        }

        public async Task<users> Login(string username, string pwd)
        {
            if (StringValueUtils.IsNullOrEmpty(username) || StringValueUtils.IsNullOrEmpty(pwd))
            {
                return null;
            }

            users user = await _uw.UserRepository.GetByName(username);
            if (user != null)
            {
                return user.User_password == pwd && user.User_state.Equals("1") ? user : null;
            }
            return user;
        }

        public async Task<int> AddUser(users user)
        {
            return await _uw.UserRepository.Insert(user);
        }

        public async Task<int> Register(Params p)
        {
            var time = DateTime.Now;
            var token = Guid.NewGuid().ToString("N");
            var tokenTime = DateTime.Now.AddDays(1);

            var user = new users
            {
                User_name = p.userName,
                User_password = p.password,
                User_mail = p.email,
                User_sex = p.sex,
                Registertime = time,
                token_time = tokenTime,
                User_token = token,
                User_state = "1",
                User_icon = "/a60613b465f2dd5c6f5848d3feb40ffd.jpg"
            };
            return await _uw.UserRepository.Insert(user);
        }

        public async Task<bool> IsNameOrMailExists(string name, string email)
        {
            return await _uw.UserRepository.Exists(e => e.User_name == name || e.User_mail == email);
        }

        public async Task<bool> IsFollow(long followingId, long followedId)
        {
            if (followedId == followingId)
            {
                return false;
            }
            //IEnumerable<follow> f = await _uw.FollowRepository.GetByAllKey(followingId, followedId);
            //return f.Count() == 0 ? false : true;
            return await _uw.FollowRepository.Exists(e => e.Follower_ID == followingId && e.Artist_ID == followedId);
        }

        public async Task<int> Follow(long followingId, long followedId)
        {
            if (followedId == followingId || followingId < 0 || followedId < 0 || await IsFollow(followingId, followedId))
            {
                return -1;
            }
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                follow f = new follow
                {
                    Artist_ID = followedId,
                    Follower_ID = followingId,
                    followtime = DateTime.Now
                };
                await _uw.FollowRepository.Insert(f);

                notification n = new notification
                {
                    Noti_Time = DateTime.Now,
                    Receiver_name = await _uw.UserRepository.GetNameById(followedId),
                    Sender_Name = await _uw.UserRepository.GetNameById(followingId),
                    Work_Name = "",
                    Noti_State = "0",
                    type = "follow",
                    Work_ID = 0
                };

                await _uw.NotificationRepository.Insert(n);

                transaction.Commit();
            }
            
            return await _uw.SaveChanges();

        }

        public async Task<int> Follow(string artistName, long followingId)
        {
            long followedId = await _uw.UserRepository.GetIdByName(artistName);
            return await Follow(followingId, followedId);

        }

        public async Task<UserDetails> GetUserDetailsOneAsync(string userName, PageRequest page)
        {
            UserDetails userDetails = null;
            if (StringValueUtils.IsNullOrEmpty(userName) || page == null)
            {
                return userDetails;
            }
             
            users user = await _uw.UserRepository.GetByName(userName);
            if (user != null)
            {
                var frenzy = await _uw.FollowRepository.CountFollowed(user.User_ID);

                var workNum = await _uw.UserRepository.CountWorks(user.User_ID);

                var followers = await _uw.UserRepository.GetAllFollowed(user.User_ID, page);

                var following = await _uw.UserRepository.GetAllFollowing(user.User_ID, page);

                userDetails = new UserDetails
                {
                    artistId = user.User_ID,
                    artistName = user.User_name,
                    frenzy = frenzy,
                    iconURL = user.User_icon,
                    description = user.User_description,
                    joinyear = user.Registertime.Year.ToString(),
                    worknum = workNum,
                    followers = followers,
                    following = following
                };
            }
            return userDetails;
        }

        public async Task<IEnumerable<UserDetails>> GetUserDetailsAsync(string userName, PageRequest page)
        {
            List<UserDetails> userDetailsList = new List<UserDetails>();

            var details = await _uw.UserRepository.GetDetailsByName(userName, page);
            foreach (var _details in details)
            {
                long userId = long.Parse(_details.GetType().GetProperty("userId").GetValue(_details).ToString());
                UserDetails userDetails = new UserDetails
                {
                    artistId = userId,
                    iconURL = _details.GetType().GetProperty("userIcon").GetValue(_details).ToString(),
                    artistName = _details.GetType().GetProperty("userName").GetValue(_details).ToString(),
                    frenzy = await _uw.FollowRepository.CountFollowed(userId)
                };
                userDetailsList.Add(userDetails);
            }
            return userDetailsList;
        }

        public async Task<users> GetUserOne(long userId)
        {
            if (userId < 0)
            {
                return null;
            }
            return await _uw.UserRepository.GetById(userId);
        }

        public async Task<users> GetUserOne(string userName)
        {
            if (StringValueUtils.IsNullOrEmpty(userName))
            {
                return null;
            }
            return await _uw.UserRepository.GetByName(userName);
        }

        public async Task<int> UpdateUserOne(users user)
        {
            return await _uw.UserRepository.Update(user);
        }
    }
}
