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

        public Task<IEnumerable<tags>> GetPopularTagBetweenTime(DateTime start, DateTime end)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<tags>> GetSimilarTag(string key)
        {
            throw new NotImplementedException();
        }
    }
}
