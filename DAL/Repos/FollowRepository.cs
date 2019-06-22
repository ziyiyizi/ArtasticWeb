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

        public async Task<long> CountFollowed(long artistId)
        {
            return await _dbSet.Where(e => e.Artist_ID == artistId).LongCountAsync();
        }

        public async Task<long> CountFollowing(long userId)
        {
            return await _dbSet.Where(e => e.Follower_ID == userId).LongCountAsync();
        }

        public async Task<IEnumerable<follow>> GetByAllKey(long userId, long artistId)
        {
            var res = await (from s in _dbSet
                            where s.Follower_ID == userId && s.Artist_ID == artistId
                            select s).ToListAsync();
            return res;
        }

        public async Task<IEnumerable<long>> GetMostFollowedBetweenTime(DateTime start, DateTime end)
        {
            var ids = await(from s in _dbSet
                            where s.followtime >= start && s.followtime <= end
                            group s by s.Artist_ID into g
                            orderby g.Count() descending
                            select g.Key).Take(10).ToListAsync();
            return ids;
        }
    }
}
