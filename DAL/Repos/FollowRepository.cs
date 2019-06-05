using Model.IRepos;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repos
{
    public class FollowRepository : RepositoryBase<follow>, IFollow
    {
        public FollowRepository(ArtasticContext context) : base(context)
        {

        }

        public async Task<int> CountFollowed(long artistId)
        {
            //var res = await (from s in _dbSet
            //                where s.Artist_ID == artistId
            //                select s).ToListAsync();
            //return res.Count;
            return await _dbSet.Where(e => e.Artist_ID == artistId).CountAsync();
        }

        public async Task<int> CountFollowing(long userId)
        {
            //var res = await (from s in _dbSet
            //                where s.Follower_ID == userId
            //                select s).ToListAsync();
            //return res.Count;
            return await _dbSet.Where(e => e.Follower_ID == userId).CountAsync();
        }

        public async Task<IEnumerable<follow>> GetByAllKey(long userId, long artistId)
        {
            var res = await (from s in _dbSet
                            where s.Follower_ID == userId && s.Artist_ID == artistId
                            select s).ToListAsync();
            return res;
        }

        public Task<IEnumerable<long>> GetMostFollowedBetweenTime(DateTime start, DateTime end)
        {
            throw new NotImplementedException();
        }
    }
}
