using Model.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Model.IRepos
{
    public interface ILike : IRepository<likes>
    {
        Task<IList> GetLikeUserList(long artworkId, PageRequest page);

        Task<IEnumerable<long>> GetAllLikesTimeSort(long userId);

        Task<IEnumerable<long>> GetAllLikesPageTimeSort(long userId, PageRequest page);

        Task<long> CountLikes(long artworkId);

        Task<IEnumerable<likes>> GetByAllKey(long userId, long artworkId);

    }
}
