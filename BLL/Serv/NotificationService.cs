using DAL;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Serv
{
    public class NotificationService
    {
        private UnitOfWork _uw;

        public NotificationService(UnitOfWork uw)
        {
            _uw = uw;
        }

        public async Task<int> UpdateNotification(string receiver)
        {
            return await _uw.NotificationRepository.UpdateByReceiverName(receiver);
        }

        public async Task<IEnumerable<notification>> PullNotification(string receiver) 
        {
            IEnumerable<notification> lst = await _uw.NotificationRepository.GetByReceiverNameOrderByNotiTimeDesc(receiver);
            await UpdateNotification(receiver);
            return lst;
        }

        public async Task<int> CountNotification(string receiver)
        {
            return await _uw.NotificationRepository.CountNotify(receiver);
        }
    }
}
