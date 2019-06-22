using Model.IRepos;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace DAL.Repos
{
    public class TagRepository : RepositoryBase<tags>, ITag
    {
        public TagRepository(ArtasticContext context) : base(context)
        {

        }

        public async Task<IEnumerable<tags>> GetAllById(long artworkId)
        {
            return await GetAllList(e => e.Artwork_ID == artworkId);
        }

        public async Task<IEnumerable<string>> GetAllTagNameById(long artworkId)
        {
            if (artworkId < 0)
            {
                return null;
            }

            return await (from e in _dbSet
                          where e.Artwork_ID == artworkId
                          select e.Tag_name).ToListAsync();
        }

        public async Task<IEnumerable<string>> GetPopularTagBetweenTime(DateTime start, DateTime end)
        {
            //from s in _dbcontext.likes
            //where s.liketime >= start && s.liketime <= end
            //join ru in _dbcontext.tags
            //on s.Artwork_ID equals ru.Artwork_ID

            var res = _dbcontext.likes.Where(e => (e.liketime >= start && e.liketime <= end))
                .Join(_dbcontext.tags, e => e.Artwork_ID, r => r.Artwork_ID, (p, c) => new { p.Artwork_ID, c.Tag_name })
                .GroupBy(e => e.Tag_name)
                .OrderByDescending(e => e.LongCount())
                .Select(e => e.Key)
                .Take(5);
            return await res.ToListAsync();
        }

        public async Task<IEnumerable<string>> GetSimilarTag(string key)
        {
            var ids = _dbSet.Where(e => e.Tag_name == key).Select(e => e.Artwork_ID);
            var res = _dbSet.Where(e => ids.Contains(e.Artwork_ID) && e.Tag_name != key)
                .GroupBy(e => e.Tag_name)
                .OrderByDescending(e => e.LongCount())
                .Select(e => e.Key)
                .Take(5);

            return await res.ToListAsync();
            
        }
    }
}
