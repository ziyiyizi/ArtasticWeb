using Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Model.IRepos
{
    public interface ITag : IRepository<tags>
    {
        Task<IEnumerable<tags>> GetAllById(long artworkId);
        Task<IEnumerable<string>> GetAllTagNameById(long artworkId);
        Task<IEnumerable<tags>> GetPopularTagBetweenTime(DateTime start, DateTime end);

        Task<IEnumerable<tags>> GetSimilarTag(string key);
    }
}
