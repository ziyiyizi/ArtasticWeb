using Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Model.IRepos
{
    public interface IFollow : IRepository<follow>
    {
        Task<int> CountFollowed(long artistId);
        Task<int> CountFollowing(long userId);

        Task<IEnumerable<follow>> GetByAllKey(long userId, long artistId);

        Task<IEnumerable<long>> GetMostFollowedBetweenTime(DateTime start, DateTime end);
    }
}
