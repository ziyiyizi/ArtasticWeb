using Microsoft.EntityFrameworkCore;
using Model;
using Model.IRepos;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Collections;

namespace DAL.Repos
{
    public class LikeRepository :RepositoryBase<likes>, ILike
    {
        public LikeRepository(ArtasticContext context) : base(context)
        {

        }

        public async Task<long> CountLikes(long artworkId)
        {
            return await _dbSet.CountAsync(e => e.Artwork_ID == artworkId);
           
        }

        public async Task<IEnumerable<long>> GetAllLikesPageTimeSort(long userId, PageRequest page)
        {
            if (userId < 0 || page == null)
            {
                return null;
            }

            return await (from s in _dbSet
             where s.User_ID == userId
             orderby s.liketime descending
             select s.Artwork_ID).Skip(page.PageNumber * page.PageSize)
             .Take(page.PageSize)
             .ToListAsync();

        }

        public async Task<IEnumerable<artworks>> GetAllPageLikeArtworksTimeSort(long userId, PageRequest page)
        {
            if (userId < 0 || page == null)
            {
                return null;
            }

            return await (from u in _dbcontext.likes join ru in _dbcontext.artworks
                          on u.Artwork_ID equals ru.Artwork_ID
                          where u.User_ID == userId
                          orderby u.liketime descending
                          select ru).Skip(page.PageNumber * page.PageSize)
             .Take(page.PageSize)
             .ToListAsync();

        }

        public Task<IEnumerable<long>> GetAllLikesTimeSort(long userId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<likes>> GetByAllKey(long userId, long artworkId)
        {
            var res = await (from s in _dbSet
                             where s.User_ID == userId && s.Artwork_ID == artworkId
                             select s).ToListAsync();
            return res;
        }

        public async Task<IList> GetLikeUserList(long artworkId, PageRequest page)
        {
            if (artworkId < 0 || page == null)
            {
                return null;
            }

            return await(from s in _dbcontext.likes join ru in _dbcontext.users
                         on s.User_ID equals ru.User_ID
                         where s.Artwork_ID == artworkId
                         orderby s.liketime descending
                         select new {
                             userName = ru.User_name,
                             userIcon = ru.User_icon,
                             liketime = s.liketime

                         }).Skip(page.PageNumber * page.PageSize)
             .Take(page.PageSize)
             .ToListAsync();
        }
    }
}
