using Model.IRepos;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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

        public Task<IEnumerable<artworks>> GetLatestClickArtworks(long userId, int limit)
        {
            throw new NotImplementedException();
        }
    }
}
