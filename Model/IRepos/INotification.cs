using Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Model.IRepos
{
    public interface INotification : IRepository<notification>
    {
        Task<IEnumerable<notification>> GetByReceiverNameOrderByNotiTimeDesc(string receiverName);
        Task<int> UpdateByReceiverName(string receiverName);
        Task<int> CountNotify(string receiverName);

    }
}
