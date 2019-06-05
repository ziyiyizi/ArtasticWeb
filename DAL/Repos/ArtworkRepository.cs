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

        public async Task<IEnumerable<artworks>> GetAllPageCommentedSort(PageRequest page)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<long>> GetIdAllPageCommentedSort(PageRequest page)
        {
            var ids = await (from s in _dbcontext.comments
                             group s by s.Artwork_ID into g
                             orderby g.Count() descending
                             select g.Key).Skip(page.PageNumber * page.PageSize).Take(page.PageSize).ToListAsync();
            return ids;
        }

        public async Task<IEnumerable<artworks>> GetAllPageLikedSort(PageRequest page)
        {
            throw new NotImplementedException();       
        }

        public async Task<IEnumerable<long>> GetIdAllPageLikedSort(PageRequest page)
        {
            var ids = await (from s in _dbcontext.likes
                       group s by s.Artwork_ID into g
                       orderby g.Count() descending
                       select g.Key).Skip(page.PageNumber * page.PageSize).Take(page.PageSize).ToListAsync();
            return ids;
        }

        public Task<IEnumerable<artworks>> GetAllPageLikedSortBetweenTime(PageRequest page, DateTime start, DateTime end)
        {
            throw new NotImplementedException();
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
    }
}
