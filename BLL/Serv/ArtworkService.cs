using BLL.Models;
using DAL;
using Microsoft.Extensions.Primitives;
using Model;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using BLL.Utils;
using Microsoft.EntityFrameworkCore.Storage;

namespace BLL.Serv
{
    public class ArtworkService
    {
        private UnitOfWork _uw;
        private ArtasticContext _db;

        public ArtworkService(UnitOfWork uw, ArtasticContext db)
        {
            _uw = uw;
            _db = db;
        }

        public async Task<long> CountClicks(long artworkId)
        {
            return await _uw.ClickRepository.CountClicks(artworkId);
        }

        public async Task<artworks> GetArtworkWeekly()
        {
            var start = DateTime.Now.AddMinutes(-3600 * 24 * 7);
            var end = DateTime.Now;
            var artworkId = await _uw.ClickRepository.GetMostClicksArtworkIdWeekly(start, end);
            return await _uw.ArtworkRepository.GetById(artworkId);
        }

        public async Task<List<string>> GetPopularTags()
        {
            var start = DateTime.Now.AddMinutes(-3600 * 24 * 7);
            var end = DateTime.Now;
            var tags = await _uw.TagRepository.GetPopularTagBetweenTime(start, end);
            return tags.ToList();
        }

        public async Task<List<string>> GetSimilarTags(StringValues key)
        {
            if (StringValueUtils.IsNullOrEmpty(key))
            {
                return new List<string>();
            }
            var values = await _uw.TagRepository.GetSimilarTag(key);
            return values.ToList();
        }

        public async Task<ArtworkLikes> GetArtworkLikesAsync(StringValues artworkId)
        {
            if (StringValueUtils.IsNullOrEmpty(artworkId))
            {
                return null;
            }
            ArtworkLikes artworkLikes = new ArtworkLikes();
            PageRequest page = new PageRequest(10, 0);
            artworkLikes.likerslist = (await _uw.LikeRepository.GetLikeUserList(long.Parse(artworkId), page));
            artworkLikes.comments = (await _uw.CommentRepository.GetAllPageById(long.Parse(artworkId)));
            return artworkLikes;
        }

        public async Task<int> Comment(StringValues sender, StringValues receiver, StringValues content, StringValues artworkId)
        {
            if (StringValueUtils.IsNullOrEmpty(sender) || StringValueUtils.IsNullOrEmpty(artworkId))
            {
                return -1;
            }
            var _artworkId = long.Parse(artworkId);

            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                await _uw.CommentRepository.Insert(new comments(_artworkId, sender, receiver, DateTime.Now, content));

                if (!sender.Equals(receiver))
                {
                    string type = "comment";
                    if (StringValueUtils.IsNullOrEmpty(receiver))
                    {
                        receiver = await _uw.UserRepository.GetNameByWorkId(_artworkId);
                    }
                    else
                    {
                        type = "comment2";
                    }
                    notification n = new notification
                    {
                        Noti_Time = DateTime.Now,
                        Receiver_name = receiver,
                        Sender_Name = sender,
                        Work_Name = await _uw.ArtworkRepository.GetNameById(_artworkId),
                        Noti_State = "0",
                        type = type,
                        Work_ID = _artworkId,
                        Noti_Content = content
                    };

                    await _uw.NotificationRepository.Insert(n);
                }
                
                transaction.Commit();
            }
                    
            return await _uw.SaveChanges();
        }

        public async Task<bool> IsLike(long artworkId, long userId)
        {
            //IEnumerable<likes> f = await _uw.LikeRepository.GetByAllKey(userId, artworkId);
            //return f.Count() == 0 ? false : true;
            return await _uw.LikeRepository.Exists(e => e.Artwork_ID == artworkId && e.User_ID == userId);
        }

        public async Task<int> Like(StringValues artworkId, StringValues userId)
        {
            if (StringValueUtils.IsNullOrEmpty(userId) || StringValueUtils.IsNullOrEmpty(artworkId))
            {
                return -1;
            }
            if (long.Parse(artworkId) < 0 || long.Parse(userId) < 0 || await IsLike(long.Parse(artworkId), long.Parse(userId)))
            {
                return -1;
            }
            var _userId = long.Parse(userId);
            var _artworkId = long.Parse(artworkId);

            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                await _uw.LikeRepository.Insert(new likes(_userId, _artworkId, DateTime.Now));

                string receiver = await _uw.UserRepository.GetNameByWorkId(_artworkId);
                string sender = await _uw.UserRepository.GetNameById(_userId);
                if (!sender.Equals(receiver))
                {
                    notification n = new notification
                    {
                        Noti_Time = DateTime.Now,
                        Receiver_name = await _uw.UserRepository.GetNameByWorkId(_artworkId),
                        Sender_Name = await _uw.UserRepository.GetNameById(_userId),
                        Work_Name = await _uw.ArtworkRepository.GetNameById(_artworkId),
                        Noti_State = "0",
                        type = "like",
                        Work_ID = _artworkId
                    };

                    await _uw.NotificationRepository.Insert(n);
                }
               
                transaction.Commit();
            }

               
            return await _uw.SaveChanges();
        }

        public async Task<int> Click(StringValues artworkId, StringValues userId)
        {
            if (StringValueUtils.IsNullOrEmpty(artworkId) || long.Parse(artworkId) < 0)
            {
                return -1;
            }
            long clientId = 114514;
            if (!StringValueUtils.IsNullOrEmpty(userId))
            {
                clientId = long.Parse(userId);
            }
            return await _uw.ClickRepository.Insert(new clicks(System.Guid.NewGuid().ToString("N"), clientId, long.Parse(artworkId), DateTime.Now));
        }

        public async Task<ArtworkDetails> GetArtworkDetailsOneAsync(long artworkId, long clientId = -1)
        {
            if (artworkId < 0)
            {
                return null;
            }
            // TODO Auto-generated method stub
            ArtworkDetails artWorkDetails = new ArtworkDetails();
            artworks _artworks = await _uw.ArtworkRepository.GetById(artworkId);
            long userId = _artworks.Artist_ID;
            artWorkDetails.artworkId = artworkId;
            artWorkDetails.artworkName = _artworks.Artwork_name;
            artWorkDetails.artistId = _artworks.Artist_ID;

            var userNameAndIcon = await _uw.UserRepository.GetIconAndNameById(userId);
            var obj = userNameAndIcon[0];
            artWorkDetails.artistName = obj.GetType().GetProperty("name").GetValue(obj).ToString();
            artWorkDetails.iconURL = obj.GetType().GetProperty("icon").GetValue(obj).ToString();
            ////获取头像
            artWorkDetails.date = _artworks.Uploadtime.ToString();
            artWorkDetails.frenzy = await _uw.LikeRepository.CountLikes(artworkId);
            artWorkDetails.tags = await _uw.TagRepository.GetAllTagNameById(artworkId);
            artWorkDetails.description = _artworks.Artwork_description;
            artWorkDetails.fileURL = _artworks.Artdata1;
            if (clientId == -1)
            {
                artWorkDetails.isLike = false;
            }
            else
            {
                artWorkDetails.isLike = await IsLike(artworkId, clientId);
            }
            return artWorkDetails;
        }

        public async Task<ArtworkDetails> GetArtworkDetailsOneAsync(artworks _artworks, long clientId = -1)
        {
            ArtworkDetails artWorkDetails = new ArtworkDetails();
            if (_artworks == null)
            {
                return artWorkDetails;
            }
            long artworkId = _artworks.Artwork_ID;
            long userId = _artworks.Artist_ID;
            artWorkDetails.artworkId = artworkId;
            artWorkDetails.artworkName = _artworks.Artwork_name;
            artWorkDetails.artistId = _artworks.Artist_ID;

            var userNameAndIcon = await _uw.UserRepository.GetIconAndNameById(userId);
            var obj = userNameAndIcon[0];
            artWorkDetails.artistName = obj.GetType().GetProperty("name").GetValue(obj).ToString();
            artWorkDetails.iconURL = obj.GetType().GetProperty("icon").GetValue(obj).ToString();
            //获取头像
            artWorkDetails.date = _artworks.Uploadtime.ToString();
            artWorkDetails.frenzy = await _uw.LikeRepository.CountLikes(artworkId);
            artWorkDetails.tags = await _uw.TagRepository.GetAllTagNameById(artworkId);
            artWorkDetails.description = _artworks.Artwork_description;
            artWorkDetails.fileURL = _artworks.Artdata1;
            if (clientId == -1)
            {
                artWorkDetails.isLike = false;
            }
            else
            {
                artWorkDetails.isLike = await IsLike(artworkId, clientId);
            }
            return artWorkDetails;
        }

        public async Task<List<ArtworkDetails>> GetArtworkDetailsAsync(StringValues type, StringValues userId, PageRequest page)
        {
            IEnumerable<artworks> _artworks = new List<artworks>();
            List<ArtworkDetails> artworkDetails = new List<ArtworkDetails>();
            long clientId = -1;
            if (!StringValueUtils.IsNullOrEmpty(userId))
            {
                clientId = long.Parse(userId);
            }
            if (type.Equals("popular"))
            {
                var end = DateTime.Now;
                var start = DateTime.Now.AddSeconds(-3600 * 24 * 7);
                
                var _artworkIds = await _uw.ArtworkRepository.GetIdAllPageLikedSortBetweenTime(page, start, end);
                foreach (var value in _artworkIds)
                {
                    artworkDetails.Add(await GetArtworkDetailsOneAsync(value, clientId));
                }
            }
            else if (type.Equals("latest"))
            {
                _artworks = await _uw.ArtworkRepository.GetAllPageTimeSort(page);
            }
            else if (type.Equals("feed"))
            {
                _artworks = await _uw.ArtworkRepository.GetFollowing(clientId, page);
                
            }
            else if (type.Equals("mylikes"))
            {
                _artworks = await _uw.LikeRepository.GetAllPageLikeArtworksTimeSort(clientId, page);
            }
            else if (type.Equals("tweet"))
            {
                var end = DateTime.Now;
                var start = DateTime.Now.AddSeconds(-3600 * 24 * 30);
                var _artworkIds = await _uw.ArtworkRepository.GetIdAllPageCommentedSortBetweenTime(page, start, end);
                foreach (var value in _artworkIds)
                {
                    artworkDetails.Add(await GetArtworkDetailsOneAsync(value, clientId));
                }
            }
            else
            {
                _artworks = await _uw.ArtworkRepository.GetAllPageRandSort(page);
            }

            if (artworkDetails.Count == 0)
            {
                foreach (var value in _artworks)
                {
                    artworkDetails.Add(await GetArtworkDetailsOneAsync(value, clientId));
                }
            }
            return artworkDetails;
        }
    }
}
