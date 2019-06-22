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
    public class ClickRepository : RepositoryBase<clicks>, IClick
    {
        public ClickRepository(ArtasticContext context) : base(context)
        {

        }

        public Task<IEnumerable<IDictionary<string, int>>> GetClicksPerMonth(long artworkId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IDictionary<string, int>>> GetClicksSliceBySex(long artworkId)
        {
            throw new NotImplementedException();
        }

        public async Task<long> GetMostClicksArtworkIdWeekly(DateTime start, DateTime end)
        {
            var res = await _dbSet.Where(e => e.Clicktime >= start && e.Clicktime <= end)
                .GroupBy(e => e.Artwork_ID)
                .OrderByDescending(e => e.LongCount())
                .Select(e => e.Key).Take(1).ToListAsync();
            return res.First();
        }

        public async Task<IEnumerable<artworks>> GetLatestClickArtworks(long userId, int limit)
        {
            var res = await (from s in _dbSet
                             where s.User_ID == userId
                             join ru in _dbcontext.artworks
                             on s.Artwork_ID equals ru.Artwork_ID
                             orderby s.Clicktime descending
                             select ru).Take(limit).ToListAsync();
            return res;
        }

        public async Task<long> CountClicks(long artworkId)
        {
            return await _dbSet.LongCountAsync(e => e.Artwork_ID == artworkId);
        }
    }
}
