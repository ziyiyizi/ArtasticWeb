using Model;
using Model.IRepos;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace DAL.Repos
{
    public class ArtworkRepository : RepositoryBase<artworks>, IArtwork
    {
        public ArtworkRepository(ArtasticContext context) : base(context)
        {

        }

        public async Task<IEnumerable<artworks>> GetAllByArtistId(long artistId)
        {
            if (artistId < 0)
            {
                return null;
            }

            return await GetAllList(e => e.Artist_ID == artistId);
        }

        public async Task<IEnumerable<artworks>> GetAllPage(PageRequest page)
        {
            if (page == null)
            {
                return null;
            }

            return await GetPageList(page);
        }

        public async Task<IEnumerable<artworks>> GetAllPageByName(string artworkName, PageRequest page)
        {
            if (page == null)
            {
                return null;
            }

            return await GetPageList(page, e => e.Artwork_name == artworkName);
        }

        public async Task<IEnumerable<artworks>> GetAllPageByTag(string tagName, PageRequest page)
        {
            if (page == null)
            {
                return null;
            }

            var result = await (from u in _dbcontext.artworks join ru in _dbcontext.tags
                on u.Artwork_ID equals ru.Artwork_ID
                where ru.Tag_name.Contains(tagName)
                select u).Skip((page.PageNumber) * page.PageSize)
                .Take(page.PageSize)
                .ToListAsync();
            return result;
        }

        public async Task<IEnumerable<artworks>> GetAllPageByTagEX(string tagName, PageRequest page)
        {
            if (page == null)
            {
                return null;
            }

            var result = await (from u in _dbcontext.artworks join ru in _dbcontext.tags
                on u.Artwork_ID equals ru.Artwork_ID
                where ru.Tag_name == tagName
                select u).Skip((page.PageNumber) * page.PageSize)
                .Take(page.PageSize)
                .ToListAsync();
            return result;
        }

        public async Task<IEnumerable<artworks>> GetAllPageByUserName(string userName, PageRequest page)
        {
            if (page == null)
            {
                return null;
            }

            var result = await (from u in _dbcontext.artworks join ru in _dbcontext.users
                on u.Artist_ID equals ru.User_ID
                where ru.User_name.Contains(userName)
                select u).Skip((page.PageNumber) * page.PageSize)
                .Take(page.PageSize)
                .ToListAsync();
            return result;
        }

        public async Task<IEnumerable<artworks>> GetAllPageByUserNameEX(string userName, PageRequest page)
        {
            if (page == null)
            {
                return null;
            }

            var result = await (from u in _dbcontext.artworks join ru in _dbcontext.users
                on u.Artist_ID equals ru.User_ID
                where ru.User_name == userName
                select u).Skip((page.PageNumber) * page.PageSize)
                .Take(page.PageSize)
                .ToListAsync();
            return result;
            
        }

        public async Task<IEnumerable<long>> GetIdAllPageCommentedSort(PageRequest page)
        {
            var ids = await (from s in _dbcontext.comments
                             group s by s.Artwork_ID into g
                             orderby g.Count() descending
                             select g.Key).Skip(page.PageNumber * page.PageSize).Take(page.PageSize).ToListAsync();
            return ids;
        }

        public async Task<IEnumerable<long>> GetIdAllPageLikedSort(PageRequest page)
        {
            var ids = await (from s in _dbcontext.likes
                       group s by s.Artwork_ID into g
                       orderby g.Count() descending
                       select g.Key).Skip(page.PageNumber * page.PageSize).Take(page.PageSize).ToListAsync();
            return ids;
        }

        public async Task<IEnumerable<long>> GetIdAllPageLikedSortBetweenTime(PageRequest page, DateTime start, DateTime end)
        {
            var ids = await(from s in _dbcontext.likes
                            where s.liketime >= start && s.liketime <= end
                            group s by s.Artwork_ID into g
                            orderby g.Count() descending
                            select g.Key).Skip(page.PageNumber * page.PageSize).Take(page.PageSize).ToListAsync();
            return ids;
        }

        public async Task<IEnumerable<artworks>> GetAllPageRandSort(PageRequest page)
        {
            return await GetPageList(page, null, e => Guid.NewGuid());
        }

        public async Task<IEnumerable<artworks>> GetAllPageTimeSort(PageRequest page)
        {
            if (page == null)
            {
                return null;
            }

            return await GetPageList(page, null, e => e.Uploadtime);
        }

        public async Task<artworks> GetById(long artworkId)
        {
            return await Get(e => e.Artwork_ID == artworkId);
        }

        public async Task<IEnumerable<artworks>> GetFollowing(long userId, PageRequest page)
        {
            if (page == null)
            {
                return null;
            }
            return await (from u in _dbcontext.follow join ru in _dbcontext.artworks
             on u.Artist_ID equals ru.Artist_ID
             where u.Follower_ID == userId
             orderby ru.Uploadtime
             select ru).Skip(page.PageNumber * page.PageSize)
             .Take(page.PageSize)
             .ToListAsync();
        }

        public async Task<string> GetNameById(long artworkId)
        {
            var res = await Get(e => e.Artwork_ID == artworkId);
            return res.Artwork_name;
        }

        public async Task<IEnumerable<long>> GetIdAllPageCommentedSortBetweenTime(PageRequest page, DateTime start, DateTime end)
        {
            var ids = await(from s in _dbcontext.comments
                            where s.Comment_time >= start && s.Comment_time <= end
                            group s by s.Artwork_ID into g
                            orderby g.Count() descending
                            select g.Key).Skip(page.PageNumber * page.PageSize).Take(page.PageSize).ToListAsync();
            return ids;
        }

        public async Task<IEnumerable<artworks>> GetIdRecommend(PageRequest page, long userId, DateTime start, DateTime end)
        {
            var tag = _dbcontext.clicks.Where(e => e.User_ID == userId && e.Clicktime >= start && e.Clicktime <= end)
                .Join(_dbcontext.tags, l => l.Artwork_ID, r => r.Artwork_ID, (l, r) => new { l.Click_ID, r.Artwork_ID, r.Tag_name })
                .GroupBy(e => e.Tag_name)
                .OrderByDescending(e => e.LongCount())
                .Select(e => e.Key)
                .Take(5);

            var res = await _dbcontext.tags.Where(e => !e.Tag_name.Equals("unknown") && tag.Contains(e.Tag_name))
                .OrderBy(e => Guid.NewGuid())
                .Take(page.PageSize)
                .Join(_dbSet, l => l.Artwork_ID, r => r.Artwork_ID, (l, r) => r)
                .ToListAsync();
           
            return res;
        }
    }
}
