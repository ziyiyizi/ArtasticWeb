using Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Model.IRepos
{
    public interface IClick : IRepository<clicks>
    {
        Task<IEnumerable<artworks>> GetLatestClickArtworks(long userId, int limit);
        Task<IEnumerable<IDictionary<string, int>>> GetClicksPerMonth(long artworkId);
        Task<IEnumerable<IDictionary<string, int>>> GetClicksSliceBySex(long artworkId);

    }
}
