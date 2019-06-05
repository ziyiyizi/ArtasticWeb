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
    public class NotificationRepository : RepositoryBase<notification>, INotification
    {
        public NotificationRepository(ArtasticContext context) : base(context)
        {

        }

        public async Task<int> CountNotify(string receiverName)
        {
            if (receiverName == null)
            {
                return 0;
            }

            var res = await (from s in _dbSet
            where s.Noti_State == "0" && s.Receiver_name == receiverName
            select s).ToListAsync();
            return res.Count;
        }

        public async Task<IEnumerable<notification>> GetByReceiverNameOrderByNotiTimeDesc(string receiverName)
        {
            if (receiverName == null)
            {
                return null;
            }
            return await (from s in _dbSet
                          where s.Noti_State == "0" && s.Receiver_name == receiverName
                          orderby s.Noti_Time descending
                          select s).ToListAsync();
        }

        public Task<int> UpdateByReceiverName(string receiverName)
        {
            throw new NotImplementedException();
        }
    }
}
